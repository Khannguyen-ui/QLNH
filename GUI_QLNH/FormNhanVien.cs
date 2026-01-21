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
            // Chế độ này giúp nhận diện thay đổi ngay khi nhấn
            dgvNhanVien.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvNhanVien.RowHeadersVisible = false;

            dgvNhanVien.AllowUserToResizeColumns = true;
            dgvNhanVien.AllowUserToResizeRows = false;

            // --- THÊM DÒNG NÀY ĐỂ CHECKBOX NHẠY HƠN ---
            dgvNhanVien.CurrentCellDirtyStateChanged += (s, e) => {
                if (dgvNhanVien.IsCurrentCellDirty && dgvNhanVien.CurrentCell is DataGridViewCheckBoxCell)
                {
                    dgvNhanVien.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };
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
            // 1. Tạo cột Checkbox nếu chưa tồn tại
            if (!dgvNhanVien.Columns.Contains(SelectColName))
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = SelectColName,
                    HeaderText = "Chọn",
                    Width = 55,
                    ReadOnly = false, // Phải để false để người dùng có thể tích chọn
                    Visible = true,
                    TrueValue = true,  // Định nghĩa giá trị khi tích
                    FalseValue = false // Định nghĩa giá trị khi bỏ tích
                };
                dgvNhanVien.Columns.Insert(0, chk);
            }

            // 2. Thiết lập ReadOnly: Chỉ cột "Chọn" là cho phép sửa, các cột khác khóa lại
            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
            {
                col.ReadOnly = (col.Name != SelectColName);
            }

            // 3. Khởi tạo giá trị mặc định là false cho tất cả các hàng
            foreach (DataGridViewRow r in dgvNhanVien.Rows)
            {
                if (r.Cells[SelectColName].Value == null)
                    r.Cells[SelectColName].Value = false;
            }
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
                // Trim để xóa khoảng trắng thừa đầu đuôi
                nv.MaNV = (nv.MaNV ?? "").Trim();
                nv.TenNV = (nv.TenNV ?? "").Trim();
                nv.NoiSinh = (nv.NoiSinh ?? "").Trim();
                nv.MatKhau = (nv.MatKhau ?? "").Trim();

                // ==========================================================
                // 1. KIỂM TRA MÃ NHÂN VIÊN
                // ==========================================================
                if (string.IsNullOrWhiteSpace(nv.MaNV))
                {
                    MessageBox.Show("Vui lòng nhập Mã nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    return;
                }

                // Kiểm tra ký tự đầu tiên phải là CHỮ
                if (!char.IsLetter(nv.MaNV[0]))
                {
                    MessageBox.Show("Mã nhân viên phải BẮT ĐẦU bằng chữ cái (Ví dụ: NV01).", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    txtMaNV.SelectAll();
                    return;
                }

                // [MỚI] Kiểm tra không chứa ký tự đặc biệt (Chỉ cho phép Chữ và Số)
                // Nếu có ký tự nào KHÔNG phải chữ và KHÔNG phải số -> Báo lỗi
                if (nv.MaNV.Any(c => !char.IsLetterOrDigit(c)))
                {
                    MessageBox.Show("Mã nhân viên không được chứa ký tự đặc biệt hoặc khoảng trắng!\n(Chỉ chấp nhận chữ và số)",
                                    "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    txtMaNV.SelectAll();
                    return;
                }

                // Kiểm tra độ dài Mã NV (> 20 ký tự)
                if (nv.MaNV.Length > 20)
                {
                    MessageBox.Show("Mã nhân viên không được vượt quá 20 ký tự!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    return;
                }

                // ==========================================================
                // 2. KIỂM TRA TÊN NHÂN VIÊN
                // ==========================================================
                if (string.IsNullOrWhiteSpace(nv.TenNV))
                {
                    MessageBox.Show("Vui lòng nhập Tên nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenNV.Focus();
                    return;
                }

                if (nv.TenNV.Length > 100)
                {
                    MessageBox.Show("Tên nhân viên không được vượt quá 100 ký tự!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenNV.Focus();
                    return;
                }

                // Kiểm tra Ký tự đặc biệt & Số trong Tên
                if (nv.TenNV.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
                {
                    MessageBox.Show("Tên nhân viên không được chứa số hoặc ký tự đặc biệt!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenNV.Focus();
                    txtTenNV.SelectAll();
                    return;
                }

                // ==========================================================
                // 3. KIỂM TRA CÁC TRƯỜNG KHÁC
                // ==========================================================
                if (string.IsNullOrWhiteSpace(nv.NoiSinh))
                {
                    MessageBox.Show("Vui lòng nhập Nơi sinh.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNoiSinh.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(nv.MatKhau))
                {
                    MessageBox.Show("Vui lòng nhập Mật khẩu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                // ==========================================================
                // 4. XỬ LÝ LƯU DATABASE
                // ==========================================================
                var list = NhanVienBLL.GetAll();
                var existed = list.Find(x => string.Equals(x.MaNV, nv.MaNV, StringComparison.OrdinalIgnoreCase));
                string err = null;

                // --- TRƯỜNG HỢP THÊM MỚI ---
                if (_mode == FormMode.Adding)
                {
                    if (existed != null)
                    {
                        MessageBox.Show($"Mã nhân viên '{nv.MaNV}' đã tồn tại. Vui lòng chọn mã khác.", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaNV.Focus();
                        txtMaNV.SelectAll();
                        return;
                    }

                    var ok = NhanVienBLL.Insert(nv, out err);
                    if (ok)
                    {
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        SelectRowByMa(nv.MaNV);

                        var stored = NhanVienBLL.GetByMaNV(nv.MaNV);
                        if (stored != null) FillFormFromObject(stored);

                        SetMode(FormMode.Idle);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại: " + (err ?? "Không rõ nguyên nhân"), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // --- TRƯỜNG HỢP SỬA ---
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
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        SelectRowByMa(nv.MaNV);

                        var stored = NhanVienBLL.GetByMaNV(nv.MaNV);
                        if (stored != null) FillFormFromObject(stored);

                        SetMode(FormMode.Idle);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại: " + (err ?? "Không rõ nguyên nhân"), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Helper nhỏ để fill form từ object (cho gọn code trên)
        private void FillFormFromObject(NhanVien stored)
        {
            txtMaNV.Text = stored.MaNV;
            txtTenNV.Text = stored.TenNV;
            txtNoiSinh.Text = stored.NoiSinh;
            dtpNgayLamViec.Value = stored.NgayLamViec == DateTime.MinValue ? DateTime.Today : stored.NgayLamViec;
            if (!string.IsNullOrEmpty(stored.MatKhau)) txtMatKhau.Text = stored.MatKhau;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                dgvNhanVien.EndEdit(); // Kết thúc chỉnh sửa trên lưới để lấy giá trị Checkbox mới nhất

                // 1. LẤY DANH SÁCH MÃ CẦN XÓA (TỪ CHECKBOX)
                var ids = new List<string>();
                foreach (DataGridViewRow r in dgvNhanVien.Rows)
                {
                    if (r.IsNewRow) continue;
                    // Kiểm tra cột Checkbox (SelectColName = "Chon")
                    if (r.Cells[SelectColName].Value is bool b && b)
                    {
                        var id = Convert.ToString(r.Cells["MaNV"].Value);
                        if (!string.IsNullOrWhiteSpace(id)) ids.Add(id);
                    }
                }

                // 2. XỬ LÝ XÓA NHIỀU DÒNG (BATCH DELETE)
                if (ids.Count > 0)
                {
                    if (MessageBox.Show($"Bạn có chắc chắn muốn xóa {ids.Count} nhân viên đã chọn?", "Xác nhận xóa",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                    int ok = 0;
                    List<string> errors = new List<string>(); // Danh sách chứa thông báo lỗi chi tiết

                    foreach (var id in ids)
                    {
                        // [UX] Chặn nhanh việc xóa chính mình trên giao diện (Optional)
                        // (Dù BLL sẽ chặn, nhưng chặn ở đây đỡ gọi xuống DB)
                        // Giả sử bạn có lưu User hiện tại trong AppSession
                        /*
                        if (id == AppSession.CurrentMaNV) 
                        {
                            errors.Add($"- {id}: Không thể xóa tài khoản đang đăng nhập.");
                            continue; 
                        }
                        */

                        string err;
                        if (NhanVienBLL.Delete(id, out err))
                        {
                            ok++;
                        }
                        else
                        {
                            // Ghi lại lý do lỗi cụ thể cho từng mã
                            errors.Add($"- {id}: {err ?? "Lỗi không xác định"}");
                        }
                    }

                    // Reload lại lưới sau khi xóa xong
                    LoadData();
                    ResetForm();

                    // Hiển thị kết quả
                    if (errors.Count == 0)
                    {
                        MessageBox.Show($"Đã xóa thành công {ok} nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Thông báo vừa có thành công vừa có thất bại
                        string msg = $"Đã xóa: {ok}\nThất bại: {errors.Count}\n\nChi tiết lỗi:\n" + string.Join("\n", errors);
                        MessageBox.Show(msg, "Kết quả xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                // 3. XỬ LÝ XÓA 1 DÒNG (SINGLE DELETE)
                else
                {
                    var id = txtMaNV.Text.Trim();
                    if (string.IsNullOrEmpty(id))
                    {
                        MessageBox.Show("Vui lòng chọn dòng cần xóa trên lưới hoặc tích vào ô chọn!", "Chưa chọn dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên [{id}]?", "Xác nhận xóa",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    string err;
                    if (NhanVienBLL.Delete(id, out err))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ResetForm();
                    }
                    else
                    {
                        // Hiển thị thông báo lỗi chi tiết từ BLL trả về (Admin, FK, Không tồn tại...)
                        MessageBox.Show("Không thể xóa nhân viên này!\n\nLý do: " + (err ?? "Lỗi hệ thống"), "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không mong muốn: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            // Chặn click vào header
            if (e.RowIndex < 0) return;

            // 1. Nếu nhấn vào đúng cột "Chọn" (Checkbox)
            if (dgvNhanVien.Columns[e.ColumnIndex].Name == SelectColName)
            {
                // Ép kết thúc chỉnh sửa để giá trị True/False cập nhật vào DataSource ngay
                dgvNhanVien.EndEdit();

                // Trả về luôn, KHÔNG chạy lệnh FillFormFromRow ở dưới 
                // để tránh việc đang tick chọn mà dữ liệu cứ nhảy lên Form
                return;
            }

            // 2. Nếu nhấn vào các cột khác (Mã, Tên, Nơi sinh...)
            var r = dgvNhanVien.Rows[e.RowIndex];
            if (r.Cells["MaNV"].Value == null || r.Cells["MaNV"].Value == DBNull.Value) return;

            // Đổ dữ liệu lên các TextBox để xem hoặc sửa
            FillFormFromRow(r);

            // Đưa Form về trạng thái Idle (chưa cho sửa ngay cho đến khi bấm nút Sửa)
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
            txtTenNV.Focus();
            txtTenNV.SelectAll();
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

