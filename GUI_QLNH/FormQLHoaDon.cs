// KHÔNG import Microsoft.Office.Interop.Excel tại đây để tránh trùng kiểu
using BLL_QLNH;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using GUI_QLNH.Common;
using System.Linq;

namespace GUI_QLNH
{
    public partial class FormQLHoaDon : Form, IExportable
    {
        private readonly HoaDonBLL _bll = new HoaDonBLL();

        // cache dữ liệu top món để vẽ chart
        private System.Data.DataTable _topCache = null;

        // in ấn
        private PrintDocument _printDoc;
        private string _printSoPhieu;

        public FormQLHoaDon()
        {
            InitializeComponent();

            // Bảo vệ mở trong Designer
            if (dgvHD != null)
                dgvHD.AutoGenerateColumns = false;
            if (dgvTop != null)
                dgvTop.AutoGenerateColumns = true;

            // Combo tháng
            if (cboThang != null)
            {
                cboThang.Items.Clear();
                for (int m = 1; m <= 12; m++) cboThang.Items.Add(m);
                cboThang.SelectedIndex = DateTime.Today.Month - 1;
            }

            // đăng ký vẽ chart cho panel
            if (chartTop != null)
            {
                chartTop.Paint -= ChartTop_Paint;
                chartTop.Paint += ChartTop_Paint;
            }

            // tài liệu in
            _printDoc = new PrintDocument();
            _printDoc.PrintPage += PrintDoc_PrintPage;

            EnsureExportButtonExists();
        }

        private void EnsureExportButtonExists()
        {
            try
            {
                if (this.Controls.Find("btnExport", true).Length > 0) return;

                Control host = null;
                var candidateNames = new[] { "panelBottom", "panelButtons", "pnlBottom", "panel1", "panelControls", "panelBottom" };
                foreach (var n in candidateNames)
                {
                    var f = this.Controls.Find(n, true);
                    if (f != null && f.Length > 0) { host = f[0]; break; }
                }
                if (host == null)
                {
                    var refBtn = this.Controls.Find("btnClose", true).OfType<Control>().FirstOrDefault();
                    host = refBtn?.Parent ?? this;
                }

                var btnExport = new Button { Name = "btnExport", Text = "Xuất", Size = new Size(90, 32), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
                host.Controls.Add(btnExport);
                btnExport.Top = Math.Max(8, host.Height - btnExport.Height - 10);
                btnExport.Left = Math.Max(8, host.Width - btnExport.Width - 12);
                host.Resize -= Host_Resize_ForExport;
                host.Resize += Host_Resize_ForExport;
                btnExport.Click += BtnExport_Click;
            }
            catch { }
        }

        private void Host_Resize_ForExport(object sender, EventArgs e)
        {
            try
            {
                var host = sender as Control;
                var btn = host?.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnExport");
                if (btn != null)
                {
                    btn.Top = Math.Max(8, host.Height - btn.Height - 10);
                    btn.Left = Math.Max(8, host.Width - btn.Width - 12);
                }
            }
            catch { }
        }

        // IExportable for invoice list
        public string ExportKey => "HoaDon";
        public string DefaultExportFileName => "HoaDonList";
        public DataTable GetExportData() => dgvHD?.DataSource as DataTable;

        private void BtnExport_Click(object sender, EventArgs e)
        {
            ExportHelper.ExportIfAllowed(this, this);
        }

        // ===== Lifecycle =====
        private void FormQLHoaDon_Load(object sender, EventArgs e)
        {
            // mặc định khoảng 7 ngày gần nhất
            dtFrom.ShowCheckBox = true;
            dtTo.ShowCheckBox = true;
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today;

            dtNgay.Value = DateTime.Today;
            numNam.Value = DateTime.Today.Year;

            rdoNgay.Checked = true;
            ModeChanged(null, null);

            // mở tab Thống kê mặc định
            ShowThongKe();
        }

        // ===== Toggle 2 chế độ giao diện =====
        private void ShowThongKe()
        {
            tableTop.Visible = false;
            dgvHD.Visible = true;
            panelBottom.Visible = true;
            LoadHoaDon();
        }

        private void ShowTopMon()
        {
            tableTop.Visible = true;
            dgvHD.Visible = false;
            panelBottom.Visible = false;
            LoadMonTop();
        }

        // ===== Events từ Designer =====
        private void btnThongKe_Click(object sender, EventArgs e) => ShowThongKe();
        private void btnTopMon_Click(object sender, EventArgs e) => ShowTopMon();

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadHoaDon();
            LoadMonTop();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();

            rdoNgay.Checked = true;
            dtFrom.Checked = false;
            dtTo.Checked = false;
            dtFrom.Value = DateTime.Today.AddDays(-7);
            dtTo.Value = DateTime.Today;

            cboThang.SelectedIndex = DateTime.Today.Month - 1;
            numNam.Value = DateTime.Today.Year;

            LoadHoaDon();
            LoadMonTop();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvHD.CurrentRow == null) return;
            var soPhieu = Convert.ToString(dgvHD.CurrentRow.Cells["colSoPhieu"].Value ?? "");
            if (string.IsNullOrWhiteSpace(soPhieu)) return;

            // Nếu có FormHoaDon(string) thì mở, không thì fallback preview
            try
            {
                var t = Type.GetType("GUI_QLNH.FormHoaDon");
                if (t != null)
                {
                    var ctor = t.GetConstructor(new[] { typeof(string) });
                    if (ctor != null)
                    {
                        using (var f = (Form)ctor.Invoke(new object[] { soPhieu }))
                        {
                            f.StartPosition = FormStartPosition.CenterParent;
                            f.ShowDialog(this);
                            return;
                        }
                    }
                }
            }
            catch { /* ignore, fallback */ }

            _printSoPhieu = soPhieu;
            using (var dlg = new PrintPreviewDialog())
            {
                dlg.Document = _printDoc;
                dlg.Width = 800; dlg.Height = 600;
                dlg.ShowDialog(this);
            }
        }

        private void dgvHD_DoubleClick(object sender, EventArgs e) => btnView_Click(sender, e);

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvHD.CurrentRow == null)
            {
                MessageBox.Show("Chọn một hoá đơn trước khi in.", "In", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _printSoPhieu = Convert.ToString(dgvHD.CurrentRow.Cells["colSoPhieu"].Value ?? "");
            if (string.IsNullOrWhiteSpace(_printSoPhieu))
            {
                MessageBox.Show("Số phiếu không hợp lệ.");
                return;
            }

            using (var dlg = new PrintPreviewDialog())
            {
                dlg.Document = _printDoc;
                dlg.Width = 800; dlg.Height = 600;
                dlg.ShowDialog(this);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        // ===== Bộ lọc: Ngày / Tháng / Năm =====
        private void rdoNgay_CheckedChanged(object sender, EventArgs e) => ModeChanged(sender, e);
        private void rdoThang_CheckedChanged(object sender, EventArgs e) => ModeChanged(sender, e);
        private void rdoNam_CheckedChanged(object sender, EventArgs e) => ModeChanged(sender, e);

        private void ModeChanged(object sender, EventArgs e)
        {
            if (rdoNgay.Checked)
            {
                dtFrom.Visible = true; lblFrom.Text = "Từ";
                dtTo.Visible = true; lblTo.Text = "đến:";
                dtNgay.Visible = false;
                cboThang.Visible = false;
                numNam.Visible = false;
            }
            else if (rdoThang.Checked)
            {
                dtFrom.Visible = false; dtTo.Visible = false;
                dtNgay.Visible = false;
                cboThang.Visible = true;
                numNam.Visible = true;
                lblFrom.Text = "Tháng"; lblTo.Text = "Năm:";
            }
            else // Năm
            {
                dtFrom.Visible = false; dtTo.Visible = false;
                dtNgay.Visible = false; cboThang.Visible = false;
                numNam.Visible = true;
                lblFrom.Text = "Năm"; lblTo.Text = "";
            }
        }

        // ===== Nạp danh sách hoá đơn =====
        private void LoadHoaDon()
        {
            try
            {
                DateTime? from = null, to = null;

                if (rdoNgay.Checked)
                {
                    from = dtFrom.Checked ? dtFrom.Value.Date : (DateTime?)null;
                    to = dtTo.Checked ? dtTo.Value.Date : (DateTime?)null;
                }
                else if (rdoThang.Checked)
                {
                    int y = (int)numNam.Value;
                    int m = Math.Max(1, cboThang.SelectedIndex + 1);
                    from = new DateTime(y, m, 1);
                    to = new DateTime(y, m, DateTime.DaysInMonth(y, m));
                }
                else // Năm
                {
                    int y = (int)numNam.Value;
                    from = new DateTime(y, 1, 1);
                    to = new DateTime(y, 12, 31);
                }

                // _bll.Search(keyword, from, to) → cột: SoPhieu, NgayTT, TongTam, GiamPT, ThanhTien, NVTT
                var tb = _bll.Search(txtKeyword.Text.Trim(), from, to);

                // fallback tính ThanhTien nếu BLL chưa trả về
                if (tb != null && tb.Rows.Count > 0 && !tb.Columns.Contains("ThanhTien"))
                {
                    tb.Columns.Add("ThanhTien", typeof(decimal));
                    foreach (DataRow r in tb.Rows)
                    {
                        decimal tongTam = 0m, giam = 0m;
                        decimal.TryParse(Convert.ToString(r["TongTam"]), NumberStyles.Any, CultureInfo.CurrentCulture, out tongTam);
                        decimal.TryParse(Convert.ToString(r["GiamPT"]), NumberStyles.Any, CultureInfo.CurrentCulture, out giam);
                        r["ThanhTien"] = Math.Round(tongTam * (1 - giam / 100m), 0, MidpointRounding.AwayFromZero);
                    }
                }

                dgvHD.DataSource = tb;

                // summary
                lblCount.Text = $"Có {(tb == null ? 0 : tb.Rows.Count)} hoá đơn";

                decimal total = 0m;
                if (tb != null)
                {
                    foreach (DataRow r in tb.Rows)
                    {
                        decimal v;
                        if (decimal.TryParse(Convert.ToString(r["ThanhTien"]), NumberStyles.Any, CultureInfo.CurrentCulture, out v))
                            total += v;
                    }
                }
                lblTotal.Text = $"TỔNG THÀNH TIỀN: {total:n0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hoá đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== Nạp món Top + vẽ chart =====
        private void LoadMonTop()
        {
            try
            {
                DateTime? from = null, to = null;

                if (rdoNgay.Checked)
                {
                    from = dtFrom.Checked ? dtFrom.Value.Date : (DateTime?)null;
                    to = dtTo.Checked ? dtTo.Value.Date : (DateTime?)null;
                }
                else if (rdoThang.Checked)
                {
                    int y = (int)numNam.Value;
                    int m = Math.Max(1, cboThang.SelectedIndex + 1);
                    from = new DateTime(y, m, 1);
                    to = new DateTime(y, m, DateTime.DaysInMonth(y, m));
                }
                else // Năm
                {
                    int y = (int)numNam.Value;
                    from = new DateTime(y, 1, 1);
                    to = new DateTime(y, 12, 31);
                }

                // BLL trả về cột: TenMon, SoLuong
                var top = _bll.GetTopMon(from, to);
                dgvTop.DataSource = top;

                _topCache = top;
                if (chartTop != null) chartTop.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải món top: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChartTop_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = chartTop.ClientRectangle;
            g.Clear(Color.White);

            if (_topCache == null || _topCache.Rows.Count == 0)
            {
                using (var pen = new Pen(Color.Silver))
                    g.DrawRectangle(pen, rect.Left + 8, rect.Top + 8, rect.Width - 16, rect.Height - 16);
                TextRenderer.DrawText(g, "Không có dữ liệu", chartTop.Font, rect, Color.Gray,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            int n = Math.Min(10, _topCache.Rows.Count);
            double maxv = 0;
            for (int i = 0; i < n; i++)
            {
                double v = 0; double.TryParse(Convert.ToString(_topCache.Rows[i]["SoLuong"]), out v);
                if (v > maxv) maxv = v;
            }
            if (maxv <= 0) maxv = 1;

            int padding = 20;
            int areaW = rect.Width - padding * 2;
            int areaH = rect.Height - padding * 2 - 40;
            int barW = Math.Max(8, areaW / Math.Max(1, n) - 10);
            int gap = Math.Max(6, (areaW - n * barW) / Math.Max(1, n + 1));
            int x = rect.Left + padding + gap;
            int baseY = rect.Top + padding + areaH;

            using (var barBrush = new SolidBrush(Color.SteelBlue))
            using (var axisPen = new Pen(Color.Gainsboro, 1))
            using (var font = new Font("Segoe UI", 8f))
            using (var textBrush = new SolidBrush(Color.Black))
            {
                // grid lines
                for (int yi = 0; yi <= 5; yi++)
                {
                    int yy = rect.Top + padding + (int)(areaH * yi / 5.0);
                    g.DrawLine(axisPen, rect.Left + padding, yy, rect.Left + padding + areaW, yy);
                }

                for (int i = 0; i < n; i++)
                {
                    double v = 0; double.TryParse(Convert.ToString(_topCache.Rows[i]["SoLuong"]), out v);
                    int h = (int)(areaH * (v / maxv));
                    var rBar = new Rectangle(x, baseY - h, barW, h);
                    g.FillRectangle(barBrush, rBar);
                    g.DrawRectangle(Pens.DarkBlue, rBar);

                    // label dưới cột
                    string label = Convert.ToString(_topCache.Rows[i]["TenMon"]);
                    var lblRect = new Rectangle(x - 4, baseY + 2, barW + 8, 36);
                    var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near, Trimming = StringTrimming.EllipsisCharacter };
                    g.DrawString(label, font, textBrush, lblRect, sf);

                    x += barW + gap;
                }
            }
        }

        // ===== In hoá đơn đơn giản (fallback) =====
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_printSoPhieu)) return;
                var tb = _bll.GetChiTietHoaDon(_printSoPhieu);

                var font = new Font("Consolas", 10);
                int y = e.MarginBounds.Top;

                e.Graphics.DrawString("PHIẾU HOÁ ĐƠN - SỐ PHIẾU: " + _printSoPhieu,
                    new Font("Arial", 12, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, y);
                y += 30;

                e.Graphics.DrawString("Tên món".PadRight(40) + "SL".PadLeft(6) + " Giá".PadLeft(12) + " Thành tiền".PadLeft(14),
                    font, Brushes.Black, e.MarginBounds.Left, y);
                y += 22;

                decimal total = 0m;
                foreach (DataRow r in tb.Rows)
                {
                    string ten = Convert.ToString(r["TenMon"]);
                    int sl = 0; int.TryParse(Convert.ToString(r["SoLuong"]), out sl);
                    decimal gia = 0m; decimal.TryParse(Convert.ToString(r["GiaBan"]), out gia);
                    decimal thanh = sl * gia; total += thanh;

                    string left = (ten ?? "").PadRight(40);
                    if (left.Length > 40) left = left.Substring(0, 40);

                    string line = left + sl.ToString().PadLeft(6)
                                  + gia.ToString("n0").PadLeft(12)
                                  + thanh.ToString("n0").PadLeft(14);

                    e.Graphics.DrawString(line, font, Brushes.Black, e.MarginBounds.Left, y);
                    y += 18;

                    if (y > e.MarginBounds.Bottom - 40) { e.HasMorePages = true; return; }
                }
                y += 10;
                e.Graphics.DrawString("TỔNG: " + total.ToString("n0"),
                    new Font("Arial", 11, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, y);
            }
            catch (Exception ex)
            {
                e.Graphics.DrawString("Lỗi in: " + ex.Message, new Font("Arial", 9), Brushes.Red, 10, 10);
            }
        }

        private void lblFrom_Click(object sender, EventArgs e)
        {

        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
