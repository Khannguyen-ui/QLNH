using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BLL_QLNH;

namespace GUI_QLNH
{
    public partial class FormLapThanhToan : Form
    {
        private readonly HoaDonBLL _bll = new HoaDonBLL();

        /// <summary>Mã nhân viên hiện tại (FormMenu gán trước khi Show form).</summary>
        public string CurrentManv { get; set; }

        /// <summary>Cache các số phiếu đã thanh toán trong phiên để khoá nút nhanh.</summary>
        private readonly System.Collections.Generic.HashSet<string> _paidCache =
            new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public FormLapThanhToan()
        {
            InitializeComponent();

            this.Load += (s, e) => LoadData();

            // Grid
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định dạng
            colNgayTT.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            colTongTam.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTongTam.DefaultCellStyle.Format = "n0";
            colTongCuoi.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTongCuoi.DefaultCellStyle.Format = "n0";
            colGiamPT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colGiamPT.DefaultCellStyle.Format = "n2";

            // Sự kiện
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnSearch.PerformClick(); };
            dgv.CellDoubleClick += dgv_CellDoubleClick;
            dgv.SelectionChanged += Dgv_SelectionChanged;
            chkDate.CheckedChanged += chkDate_CheckedChanged;

            // Mặc định không lọc ngày
            chkDate.Checked = false;
            dtFrom.Enabled = false;
            dtTo.Enabled = false;

            EnsurePayButtonExists();
          
        }

        /// <summary>Đảm bảo có nút Thanh toán (phòng khi Designer mất control).</summary>
        private void EnsurePayButtonExists()
        {
            if (btnThanhToan != null && btnThanhToan.Parent != null) return;

            btnThanhToan = new Button
            {
                Name = "btnThanhToan",
                Text = "Thanh toán",
                Size = new Size(110, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            panelBottom.Controls.Add(btnThanhToan);
            btnThanhToan.Top = 18;
            btnThanhToan.Left = panelBottom.Width - btnThanhToan.Width - 12;
            panelBottom.Resize += (s, e) =>
            {
                btnThanhToan.Left = panelBottom.Width - btnThanhToan.Width - 12;
            };
            btnThanhToan.Click += btnThanhToan_Click;
        }

        // ------------ Helpers ------------
        private static string GetCell(DataGridViewRow r, string colName)
        {
            if (r == null) return "";
            object v = null;
            try { v = r.Cells[colName].Value; } catch { }
            return v == null ? "" : Convert.ToString(v).Trim();
        }
        private string Cur_MaHD() => dgv.CurrentRow == null ? null : GetCell(dgv.CurrentRow, "colMaHD");
        private string Cur_SoPhieu() => dgv.CurrentRow == null ? null : GetCell(dgv.CurrentRow, "colSoPhieu");

        /// <summary>Ưu tiên trả mã trạng thái logic: HoaDon.TrangThai → DatTiec.TrangThai → text hiển thị.</summary>
        private string Cur_TrangThai()
        {
            if (dgv.CurrentRow == null) return null;
            try
            {
                var drv = dgv.CurrentRow.DataBoundItem as DataRowView;
                if (drv != null)
                {
                    if (drv.DataView.Table.Columns.Contains("TrangThaiHoaDon"))
                    {
                        var v = drv["TrangThaiHoaDon"];
                        if (v != null && v != DBNull.Value) return v.ToString().Trim();
                    }
                    if (drv.DataView.Table.Columns.Contains("TrangThaiPhieu"))
                    {
                        var v = drv["TrangThaiPhieu"];
                        if (v != null && v != DBNull.Value) return v.ToString().Trim();
                    }
                }
            }
            catch { }
            return GetCell(dgv.CurrentRow, "colTrangThai");
        }

        private bool Cur_IsPaid()
        {
            var sp = Cur_SoPhieu();
            if (!string.IsNullOrWhiteSpace(sp) && _paidCache.Contains(sp)) return true;

            var code = Cur_TrangThai() ?? "";
            if (code.Equals(HoaDonBLL.TRANG_THAI_PAID, StringComparison.OrdinalIgnoreCase)) return true;
            if (code.Equals("DA_TT", StringComparison.OrdinalIgnoreCase)) return true;

            var disp = dgv.CurrentRow == null ? "" : Convert.ToString(dgv.CurrentRow.Cells["colTrangThai"].Value ?? "");
            return disp.IndexOf("đã thanh toán", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>Focus tới dòng có số phiếu.</summary>
        public void FocusBySoPhieu(string soPhieu)
        {
            txtSearch.Text = soPhieu ?? "";
            chkDate.Checked = false; dtFrom.Enabled = dtTo.Enabled = false;

            LoadData();

            if (dgv.Rows.Count == 0 || string.IsNullOrWhiteSpace(soPhieu)) return;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                var r = dgv.Rows[i];
                var sp = Convert.ToString(r.Cells["colSoPhieu"].Value)?.Trim();
                if (string.Equals(sp, soPhieu.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    r.Selected = true;
                    try { dgv.CurrentCell = r.Cells["colSoPhieu"]; } catch { }
                    try { dgv.FirstDisplayedScrollingRowIndex = i; } catch { }
                    break;
                }
            }
        }

        private bool OpenHoaDonForm(string soPhieu, string maNV, decimal giam)
        {
            try
            {
                var formType = Type.GetType("GUI_QLNH.FormThanhToanHoaDon");
                if (formType == null) { MessageBox.Show("Không tìm thấy FormThanhToanHoaDon."); return false; }

                Form f = null;
                var ctor3 = formType.GetConstructor(new[] { typeof(string), typeof(string), typeof(decimal) });
                if (ctor3 != null) f = (Form)ctor3.Invoke(new object[] { soPhieu, maNV ?? "", giam });
                else
                {
                    var ctor1 = formType.GetConstructor(new[] { typeof(string) });
                    if (ctor1 != null) f = (Form)ctor1.Invoke(new object[] { soPhieu });
                    else
                    {
                        var ctor0 = formType.GetConstructor(Type.EmptyTypes);
                        if (ctor0 != null) f = (Form)ctor0.Invoke(null);
                    }
                }
                if (f == null) { MessageBox.Show("Không thể khởi tạo FormThanhToanHoaDon."); return false; }

                // Nếu đã thanh toán → mở chế độ ReadOnly
                try
                {
                    var p = f.GetType().GetProperty("ReadOnlyMode");
                    if (p != null) p.SetValue(f, Cur_IsPaid());
                }
                catch { }

                var dr = f.ShowDialog(this);

                bool isPaid = false;
                try
                {
                    var prop = f.GetType().GetProperty("IsPaid");
                    if (prop != null) isPaid = (bool)(prop.GetValue(f) ?? false);
                }
                catch { }

                return isPaid || dr == DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mở FormThanhToanHoaDon: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ------------ Data ------------
        private void LoadData()
        {
            if (string.IsNullOrEmpty(CurrentManv)) return;
            string kw = (txtSearch.Text ?? "").Trim();
            DateTime? from = null, to = null;
            if (string.IsNullOrWhiteSpace(kw) && chkDate.Checked) { from = dtFrom.Value.Date; to = dtTo.Value.Date; }


            DataTable tb = _bll.ListChoThanhToan(kw, from, to, CurrentManv);

            if (tb != null)
            {
                if (!tb.Columns.Contains("StatusDisplay")) tb.Columns.Add("StatusDisplay", typeof(string));
                if (!tb.Columns.Contains("StatusRaw")) tb.Columns.Add("StatusRaw", typeof(string));

                int cntPaid = 0, cntUnpaid = 0;
                foreach (DataRow r in tb.Rows)
                {
                    var maHD = Convert.ToString(r["MaHD"] ?? "").Trim();
                    var hdStatus = r.Table.Columns.Contains("TrangThaiHoaDon") ? Convert.ToString(r["TrangThaiHoaDon"] ?? "").Trim() : "";
                    var phStatus = r.Table.Columns.Contains("TrangThaiPhieu") ? Convert.ToString(r["TrangThaiPhieu"] ?? "").Trim() : "";

                    string disp, raw;
                    if (!string.IsNullOrEmpty(maHD))
                    {
                        raw = string.IsNullOrEmpty(hdStatus) ? phStatus : hdStatus;
                        if (!string.IsNullOrEmpty(hdStatus) && hdStatus.Equals(HoaDonBLL.TRANG_THAI_PAID, StringComparison.OrdinalIgnoreCase))
                        { disp = "Đã thanh toán"; cntPaid++; }
                        else
                        { disp = string.Equals(phStatus, "DA_TT", StringComparison.OrdinalIgnoreCase) ? "Đã thanh toán" : "Chưa thanh toán (HĐ)"; cntUnpaid += disp.StartsWith("Chưa") ? 1 : 0; }
                    }
                    else
                    {
                        raw = phStatus;
                        disp = string.Equals(phStatus, "DA_TT", StringComparison.OrdinalIgnoreCase) ? "Đã thanh toán" : "Chờ thanh toán (Phiếu)";
                        if (disp.StartsWith("Chờ")) cntUnpaid++; else cntPaid++;
                    }

                    r["StatusDisplay"] = disp;
                    r["StatusRaw"] = raw;
                }

                dgv.DataSource = tb;
                lblCount.Text = $"Có {tb.Rows.Count} hoá đơn (Lọc theo nhân viên: {CurrentManv})";
                try { colTrangThai.DataPropertyName = "StatusDisplay"; } catch { }

                // Tô màu
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    var row = dgv.Rows[i];
                    string s = Convert.ToString(row.Cells["colTrangThai"].Value ?? "").Trim();
                    if (s.Contains("Đã thanh toán")) row.DefaultCellStyle.BackColor = Color.FromArgb(218, 247, 166);
                    else if (s.Contains("Chưa") || s.Contains("Chờ")) row.DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
                    else row.DefaultCellStyle.BackColor = Color.White;
                }

                lblCount.Text = $"Có {tb.Rows.Count} hoá đơn (Chưa TT: {cntUnpaid}, Đã TT: {cntPaid})";
            }
            else
            {
                dgv.DataSource = tb;
                lblCount.Text = "Có 0 hoá đơn";
            }
        }

        // ------------ Events ------------
        private void btnSearch_Click(object sender, EventArgs e) => LoadData();

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            chkDate.Checked = false;
            dtFrom.Enabled = dtTo.Enabled = false;
            LoadData();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = chkDate.Checked;
            if (chkDate.Checked && string.IsNullOrWhiteSpace(txtSearch.Text)) LoadData();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var soPhieu = Cur_SoPhieu();
            if (string.IsNullOrEmpty(soPhieu)) return;

            // Nếu chưa "PAID/DA_TT" thì mở form để thanh toán
            bool paid = OpenHoaDonForm(soPhieu, CurrentManv ?? "", 0m);
            LoadData();
            if (paid)
            {
                _paidCache.Add(soPhieu);
                FocusBySoPhieu(soPhieu);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string soPhieu = Cur_SoPhieu();
            if (string.IsNullOrWhiteSpace(soPhieu))
            {
                MessageBox.Show("Chưa chọn dòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Cur_IsPaid())
            {
                MessageBox.Show("Hoá đơn này đã thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentManv))
            {
                MessageBox.Show("Chưa có mã nhân viên (CurrentManv). Vui lòng đăng nhập hoặc gán CurrentManv.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool paid = OpenHoaDonForm(soPhieu, CurrentManv, 0m);
            LoadData();
            if (paid)
            {
                _paidCache.Add(soPhieu);
                FocusBySoPhieu(soPhieu);
            }
        }

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                btnThanhToan.Enabled = dgv.CurrentRow != null && !Cur_IsPaid();
            }
            catch { btnThanhToan.Enabled = true; }
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();
    }
}
