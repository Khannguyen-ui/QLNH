using System;
using System.Collections.Generic;
using System.ComponentModel; // LicenseManager
using System.Windows.Forms;
using DTO_QLNH; // class NhanVien { MaNV, TenNV, NoiSinh, NgayLamViec, MatKhau }
using BLL_QLNH; // static NhanVienBLL { GetAll(), Search(string), Insert(NhanVien), Update(NhanVien), Delete(string) }
using GUI_QLNH.Common;
using System.Data;
using System.Linq;
using System.Drawing;

namespace GUI_QLNH
{
    public partial class FormNhanVien : Form, IExportable
    {
        private const string SelectColName = "Chon";

        private enum FormMode { Idle, Adding, Editing }
        private FormMode _mode = FormMode.Idle;

        public FormNhanVien()
        {
            InitializeComponent(); // Designer cần dòng này
            // Không gọi dữ liệu tại constructor để tránh lỗi khi mở Design

            // Ensure Export button exists at runtime (Designer may not have one)
            EnsureExportButtonExists();

            // Hide designer "Bỏ qua" button if present
            try { var b = this.Controls.Find("btnBoQua", true).FirstOrDefault() as Control; if (b != null) b.Visible = false; } catch { }
        }

        private void EnsureExportButtonExists()
        {
            try
            {
                // If Designer provided button, do nothing
                if (this.Controls.Find("btnExport", true).Length > 0) return;

                // Try to find a bottom panel to host the button
                Control host = null;
                var candidateNames = new[] { "panelBottom", "panelButtons", "pnlBottom", "panel1", "panelControls" };
                foreach (var n in candidateNames)
                {
                    var f = this.Controls.Find(n, true);
                    if (f != null && f.Length > 0) { host = f[0]; break; }
                }

                // Fallback: use parent of existing action button like btnThoat or btnXoa
                if (host == null)
                {
                    var refBtn = this.Controls.Find("btnThoat", true).OfType<Control>().FirstOrDefault()
                                 ?? this.Controls.Find("btnXoa", true).OfType<Control>().FirstOrDefault();
                    host = refBtn?.Parent ?? this;
                }

                var btnExport = new Button
                {
                    Name = "btnExport",
                    Text = "Xuất",
                    Size = new Size(90, 32),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right
                };

                host.Controls.Add(btnExport);

                // Position near the right bottom
                btnExport.Top = Math.Max(8, host.Height - btnExport.Height - 10);
                btnExport.Left = Math.Max(8, host.Width - btnExport.Width - 12);

                // Keep aligned on resize
                host.Resize -= Host_Resize_ForExport;
                host.Resize += Host_Resize_ForExport;

                btnExport.Click += btnExport_Click;
            }
            catch { /* ignore in Designer */ }
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

        // Phát hiện mở trong Designer để chặn chạy DB, BLL…
        private static bool IsDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            try
            {
                return string.Equals(
                    System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    "devenv", StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            if (IsDesignMode()) return; // an toàn cho Designer
            ConfigureGrid();
            LoadData();
            SetMode(FormMode.Idle);
        }

        // ================= GRID =================
        private void ConfigureGrid()
        {
            dgvNhanVien.AutoGenerateColumns = true;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNhanVien.MultiSelect = false;
            dgvNhanVien.ReadOnly = false;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.AllowUserToDeleteRows = false;
            dgvNhanVien.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvNhanVien.RowHeadersVisible = false;

            // Cho phép người dùng chỉnh độ rộng cột mà không kéo theo form
            dgvNhanVien.AllowUserToResizeColumns = true;
            dgvNhanVien.AllowUserToResizeRows = false;

            // dgvNhanVien.CellClick += dgvNhanVien_CellClick;
        }

        private void LoadData()
        {
            var data = NhanVienBLL.GetAll();
            dgvNhanVien.DataSource = data;
            EnsureSelectColumn();

            // Gán header/format nếu có cột
            if (dgvNhanVien.Columns.Contains("MaNV"))
            {
                var c = dgvNhanVien.Columns["MaNV"];
                c.HeaderText = "Mã NV";
                c.ReadOnly = true;
            }
            if (dgvNhanVien.Columns.Contains("TenNV"))
                dgvNhanVien.Columns["TenNV"].HeaderText = "Tên nhân viên";
            if (dgvNhanVien.Columns.Contains("NoiSinh"))
                dgvNhanVien.Columns["NoiSinh"].HeaderText = "Nơi sinh";
            if (dgvNhanVien.Columns.Contains("NgayLamViec"))
            {
                var c = dgvNhanVien.Columns["NgayLamViec"];
                c.HeaderText = "Ngày làm việc";
                c.DefaultCellStyle.Format = "dd/MM/yyyy";
            }
            if (dgvNhanVien.Columns.Contains("MatKhau"))
            {
                var c = dgvNhanVien.Columns["MatKhau"];
                c.HeaderText = "Mật khẩu";
                c.ReadOnly = false;
                c.Visible = true;
                c.Width = Math.Max(120, c.Width);
            }
        }

        // Thêm cột tick chọn nếu chưa có
        private void EnsureSelectColumn()
        {
            if (!dgvNhanVien.Columns.Contains(SelectColName))
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = SelectColName,
                    HeaderText = "Chọn",
                    Width = 55,
                    ReadOnly = false
                };
                dgvNhanVien.Columns.Insert(0, chk);
            }
            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
                col.ReadOnly = col.Name != SelectColName;

            foreach (DataGridViewRow r in dgvNhanVien.Rows)
                r.Cells[SelectColName].Value = false;
        }

        // ================= FORM HELPERS =================
        private void ResetForm()
        {
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtNoiSinh.Clear();
            dtpNgayLamViec.Value = DateTime.Today;
            txtSearch.Clear();
            txtMatKhau.Text = string.Empty;
            txtMaNV.Focus();
        }

        private NhanVien ReadForm() => new NhanVien
        {
            MaNV = txtMaNV.Text.Trim(),
            TenNV = txtTenNV.Text.Trim(),
            NoiSinh = txtNoiSinh.Text.Trim(),
            NgayLamViec = dtpNgayLamViec.Value,
            MatKhau = txtMatKhau.Text
        };

        private void FillFormFromRow(DataGridViewRow row)
        {
            if (row == null) return;
            txtMaNV.Text = Convert.ToString(row.Cells["MaNV"].Value);
            txtTenNV.Text = Convert.ToString(row.Cells["TenNV"].Value);
            txtNoiSinh.Text = Convert.ToString(row.Cells["NoiSinh"].Value);

            if (DateTime.TryParse(Convert.ToString(row.Cells["NgayLamViec"].Value), out DateTime d))
                dtpNgayLamViec.Value = d;

            if (dgvNhanVien.Columns.Contains("MatKhau"))
            {
                var v = row.Cells["MatKhau"]?.Value;
                txtMatKhau.Text = v == null || v == DBNull.Value ? string.Empty : Convert.ToString(v);
            }
        }

        private void SetMode(FormMode m)
        {
            _mode = m;
            bool adding = m == FormMode.Adding;
            bool editing = m == FormMode.Editing;

            // enable/disable controls
            btnThem.Enabled = !adding;
            btnLuu.Enabled = adding || editing;
            // Make Sửa available when not adding/editing (idle)
            btnSua.Enabled = !adding && !editing;
            // Xóa should be available when not adding
            btnXoa.Enabled = !adding;
            btnMoi.Enabled = !adding;

            // when editing, do not allow changing primary key
            txtMaNV.ReadOnly = editing;

            // focus adjustments
            if (adding) txtMaNV.Focus();
            else if (editing) txtTenNV.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var nv = ReadForm();
                nv.MaNV = (nv.MaNV ?? "").Trim();
                nv.TenNV = (nv.TenNV ?? "").Trim();

                if (string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.TenNV))
                {
                    MessageBox.Show("Mã và Tên nhân viên không được để trống!");
                    return;
                }

                // basic validation: max lengths (adjust if your DB limits differ)
                if (nv.MaNV.Length > 20) { MessageBox.Show("Mã NV quá dài."); return; }
                if (nv.TenNV.Length > 100) { MessageBox.Show("Tên NV quá dài."); return; }

                // check existence
                var list = NhanVienBLL.GetAll();
                var existed = list.Find(x => string.Equals(x.MaNV, nv.MaNV, StringComparison.OrdinalIgnoreCase));

                string err = null;
                if (_mode == FormMode.Adding)
                {
                    if (existed != null)
                    {
                        MessageBox.Show($"Mã nhân viên '{nv.MaNV}' đã tồn tại. Vui lòng chọn mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var ok = NhanVienBLL.Insert(nv, out err);
                    if (ok)
                    {
                        MessageBox.Show("Thêm thành công!");
                        LoadData();
                        SelectRowByMa(nv.MaNV);
                        var stored = NhanVienBLL.GetByMaNV(nv.MaNV);
                        if (stored != null)
                        {
                            txtMaNV.Text = stored.MaNV;
                            txtTenNV.Text = stored.TenNV;
                            txtNoiSinh.Text = stored.NoiSinh;
                            dtpNgayLamViec.Value = stored.NgayLamViec == DateTime.MinValue ? DateTime.Today : stored.NgayLamViec;
                            if (!string.IsNullOrEmpty(stored.MatKhau)) txtMatKhau.Text = stored.MatKhau;
                        }
                        SetMode(FormMode.Idle);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại: " + (err ?? "Không rõ nguyên nhân"), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (_mode == FormMode.Editing)
                {
                    if (existed == null)
                    {
                        MessageBox.Show($"Không tìm thấy nhân viên '{nv.MaNV}' để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var ok = NhanVienBLL.Update(nv, out err);
                    if (ok)
                    {
                        MessageBox.Show("Cập nhật thành công!");
                        LoadData();
                        SelectRowByMa(nv.MaNV);
                        var stored = NhanVienBLL.GetByMaNV(nv.MaNV);
                        if (stored != null)
                        {
                            txtMaNV.Text = stored.MaNV;
                            txtTenNV.Text = stored.TenNV;
                            txtNoiSinh.Text = stored.NoiSinh;
                            dtpNgayLamViec.Value = stored.NgayLamViec == DateTime.MinValue ? DateTime.Today : stored.NgayLamViec;
                            if (!string.IsNullOrEmpty(stored.MatKhau)) txtMatKhau.Text = stored.MatKhau;
                        }
                        SetMode(FormMode.Idle);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại: " + (err ?? "Không rõ nguyên nhân"), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không ở chế độ Thêm hoặc Sửa.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                dgvNhanVien.EndEdit();

                var ids = new List<string>();
                foreach (DataGridViewRow r in dgvNhanVien.Rows)
                {
                    if (r.IsNewRow) continue;
                    if (r.Cells[SelectColName].Value is bool b && b)
                    {
                        var id = Convert.ToString(r.Cells["MaNV"].Value);
                        if (!string.IsNullOrWhiteSpace(id)) ids.Add(id);
                    }
                }

                if (ids.Count > 0)
                {
                    if (MessageBox.Show($"Xóa {ids.Count} nhân viên đã chọn?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                    int ok = 0, fail = 0;
                    foreach (var id in ids)
                    {
                        string err;
                        if (NhanVienBLL.Delete(id, out err)) ok++; else fail++;
                    }

                    LoadData(); ResetForm();
                    MessageBox.Show($"Đã xóa: {ok} | Lỗi: {fail}");
                }
                else
                {
                    var id = txtMaNV.Text.Trim();
                    if (string.IsNullOrEmpty(id))
                    { MessageBox.Show("Chọn dòng để xóa hoặc tick trong lưới!"); return; }

                    if (MessageBox.Show("Xóa nhân viên này?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    string err;
                    if (NhanVienBLL.Delete(id, out err))
                    { MessageBox.Show("Đã xóa!"); LoadData(); ResetForm(); }
                    else MessageBox.Show("Không thể xóa!\n" + (err ?? "Không rõ nguyên nhân"));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi"); }
        }

        private void btnMoi_Click(object sender, EventArgs e) { ResetForm(); LoadData(); }

        // Remove btnBoQua handler usage (we keep method if wired but it's hidden)
        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(FormMode.Idle);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            var k = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(k)) { LoadData(); return; }
            dgvNhanVien.DataSource = NhanVienBLL.Search(k);
            EnsureSelectColumn();
        }

        private void btnThoat_Click(object sender, EventArgs e) => Close();

        // ================= EVENTS =================
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvNhanVien.Columns[e.ColumnIndex].Name == SelectColName)
            { // only toggle checkbox – do not fill form
                dgvNhanVien.CommitEdit(DataGridViewDataErrorContexts.Commit);
                return;
            }

            var r = dgvNhanVien.Rows[e.RowIndex];
            if (r.Cells["MaNV"].Value == null) return;

            FillFormFromRow(r);
            // Keep mode idle; user must click Sửa to enter editing
            SetMode(FormMode.Idle);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(FormMode.Adding);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text))
            { MessageBox.Show("Chọn một dòng để sửa!"); return; }

            SetMode(FormMode.Editing);
            MessageBox.Show("Chỉnh thông tin rồi nhấn Lưu để cập nhật.");
            txtTenNV.Focus();
        }

        private void SelectRowByMa(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma)) return;
            foreach (DataGridViewRow r in dgvNhanVien.Rows)
            {
                var v = Convert.ToString(r.Cells["MaNV"].Value);
                if (!string.IsNullOrWhiteSpace(v) && string.Equals(v.Trim(), ma.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    r.Selected = true;
                    dgvNhanVien.CurrentCell = r.Cells["MaNV"];
                    dgvNhanVien.FirstDisplayedScrollingRowIndex = Math.Max(0, r.Index - 2);
                    break;
                }
            }
        }

        // IExportable implementation
        public string ExportKey => "NhanVien";
        public string DefaultExportFileName => "DanhSachNhanVien";
        public DataTable GetExportData() => ExportHelper.DataTableFromGrid(dgvNhanVien);

        // Add handler for export button (if you have button named btnExport)
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportHelper.ExportIfAllowed(this, this);
        }
    }
}

