using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL_QLNH;
using DTO_QLNH;

namespace GUI_QLNH
{
    public partial class FormDangNhap : Form
    {
        private string lastAutoFillUser = null;
        public AppUser LoggedInUser { get; private set; }

        // Layout config
        private const int LeftMin = 240;
        private const int LeftMax = 360;
        private const double LeftRatio = 0.32;

        public FormDangNhap()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chết người tại InitializeComponent (Constructor):\n" + ex.Message
                                + "\n\nNguyên nhân: Có thể do Icon, Hình ảnh resource bị thiếu, hoặc Control bị lỗi.");
            }
        }

        private static bool IsDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            try { return Process.GetCurrentProcess().ProcessName.Equals("devenv", StringComparison.OrdinalIgnoreCase); }
            catch { return false; }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                if (IsDesignMode()) return;

                // Thử canh chỉnh layout
                try { UpdateResponsiveLayout(); } catch { }

                // --- GẮN SỰ KIỆN ---
                chkShowPass.CheckedChanged -= chkShowPass_CheckedChanged;
                chkShowPass.CheckedChanged += chkShowPass_CheckedChanged;

                txtMatKhau.KeyDown -= txtMatKhau_KeyDown;
                txtMatKhau.KeyDown += txtMatKhau_KeyDown;

                // [MỚI] Gắn sự kiện Enter cho Tên đăng nhập
                txtTenDangNhap.KeyDown -= txtTenDangNhap_KeyDown;
                txtTenDangNhap.KeyDown += txtTenDangNhap_KeyDown;

                txtTenDangNhap.TextChanged -= TxtTenDangNhap_TextChanged;
                txtTenDangNhap.TextChanged += TxtTenDangNhap_TextChanged;

                btnDangNhap.Click -= btnDangNhap_Click;
                btnDangNhap.Click += btnDangNhap_Click;

                txtMatKhau.UseSystemPasswordChar = !chkShowPass.Checked;

                // Tìm Radio button an toàn
                try
                {
                    var rbQL = GetRadioQuanLy();
                    var rbNV = GetRadioNhanVien();
                    if (rbQL != null && rbNV != null && !rbQL.Checked && !rbNV.Checked)
                        rbQL.Checked = true;
                }
                catch (Exception exRadio)
                {
                    Debug.WriteLine("Lỗi khi tìm RadioButton: " + exRadio.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nghiêm trọng trong OnLoad: " + ex.Message);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (IsDesignMode()) return;
            try { UpdateResponsiveLayout(); } catch { }
        }

        private void UpdateResponsiveLayout()
        {
            if (splitMain == null || pnlRight == null || pnlContent == null) return;
            if (splitMain.Width <= 0) return;

            try
            {
                int leftWidth = (int)Math.Round(ClientSize.Width * LeftRatio);
                leftWidth = Math.Max(LeftMin, Math.Min(leftWidth, LeftMax));

                if (splitMain.Width > LeftMin)
                {
                    int maxDistance = splitMain.Width - 50;
                    splitMain.SplitterDistance = Math.Max(LeftMin, Math.Min(leftWidth, maxDistance));
                }

                if (pnlRight.ClientSize.Width <= 0 || pnlRight.ClientSize.Height <= 0) return;

                int maxCardWidth = 560;
                int margin = 40;
                int targetWidth = Math.Min(maxCardWidth, Math.Max(480, pnlRight.ClientSize.Width - margin * 2));
                pnlContent.Width = targetWidth;
                pnlContent.Left = (pnlRight.ClientSize.Width - pnlContent.Width) / 2;
                pnlContent.Top = 40;

                int innerPad = 20;
                int inputLeft = 170;
                int inputWidth = pnlContent.ClientSize.Width - inputLeft - innerPad;

                txtTenDangNhap.Left = inputLeft;
                txtTenDangNhap.Width = inputWidth;

                txtMatKhau.Left = inputLeft;
                txtMatKhau.Width = inputWidth;

                grpRole.Left = inputLeft - 2;
                grpRole.Width = inputWidth + 4;

                btnDangNhap.Left = inputLeft;
                btnDangNhap.Width = inputWidth;
            }
            catch { }
        }

        // --- CÁC SỰ KIỆN XỬ LÝ ---

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            txtMatKhau.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        // [MỚI] Xử lý Enter tại ô Mật khẩu
        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsDesignMode()) return;
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Chặn tiếng Bíp
                btnDangNhap.PerformClick();
            }
        }

        // [MỚI] Xử lý Enter tại ô Tên đăng nhập (Thông minh)
        private void txtTenDangNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsDesignMode()) return;
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Chặn tiếng Bíp

                // Nếu chưa có mật khẩu -> Nhảy xuống nhập mật khẩu
                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    txtMatKhau.Focus();
                }
                // Nếu đã có mật khẩu -> Đăng nhập luôn
                else
                {
                    btnDangNhap.PerformClick();
                }
            }
        }

        private void TxtTenDangNhap_TextChanged(object sender, EventArgs e)
        {
            if (!this.Visible || IsDesignMode()) return;

            try
            {
                var user = txtTenDangNhap.Text.Trim();
                if (string.IsNullOrEmpty(user)) return;

                if (txtMatKhau.Focused) return;
                if (string.Equals(user, lastAutoFillUser, StringComparison.Ordinal)) return;

                string pwd = null;
                try
                {
                    pwd = TaiKhoanBLL.GetPasswordByUser(user);
                }
                catch { }

                if (string.IsNullOrEmpty(pwd))
                {
                    try { pwd = NhanVienBLL.GetPasswordByMaNV(user); } catch { }
                }

                if (!string.IsNullOrEmpty(pwd))
                {
                    txtMatKhau.Text = pwd;
                    lastAutoFillUser = user;
                }
            }
            catch { }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;

            string user = txtTenDangNhap.Text.Trim();
            string pass = txtMatKhau.Text.Trim();

            // 1. Kiểm tra rỗng
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu.",
                                "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy Radio Button
                var rbQL = this.Controls.Find("rdoQuanLy", true).OfType<RadioButton>().FirstOrDefault() ?? GetRadioQuanLy();
                var rbNV = this.Controls.Find("rdoNhanVien", true).OfType<RadioButton>().FirstOrDefault() ?? GetRadioNhanVien();

                bool isQuanLy = rbQL != null && rbQL.Checked;
                bool isNhanVien = rbNV != null && rbNV.Checked;

                if (!isQuanLy && !isNhanVien)
                {
                    MessageBox.Show("Vui lòng chọn vai trò: Quản lý hoặc Nhân viên.",
                                    "Chưa chọn vai trò", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ============================================================
                // TRƯỜNG HỢP 1: NGƯỜI DÙNG CHỌN "QUẢN LÝ"
                // ============================================================
                if (isQuanLy)
                {
                    var tk = new TaiKhoan { TenDangNhap = user, MatKhau = pass };

                    // Thử đăng nhập đúng vai trò Quản lý
                    if (TaiKhoanBLL.DangNhapTaiKhoan(tk, out string vaiTro, out string hoTen))
                    {
                        // -> THÀNH CÔNG
                        if (string.IsNullOrWhiteSpace(vaiTro)) vaiTro = "Admin";
                        if (string.IsNullOrWhiteSpace(hoTen)) hoTen = user;

                        var appUser = new AppUser { UserName = user, FullName = hoTen, Role = vaiTro };
                        MessageBox.Show($"Đăng nhập thành công!\nXin chào quản lý: {appUser.FullName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.LoggedInUser = appUser;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        // -> THẤT BẠI. NHƯNG KHOAN, KIỂM TRA XEM CÓ PHẢI LÀ NHÂN VIÊN KHÔNG?
                        string tempTenNV;
                        if (NhanVienBLL.DangNhapNhanVien(user, pass, out tempTenNV))
                        {
                            // Nếu vào được đây, tức là User/Pass đúng, nhưng nó là Nhân Viên
                            MessageBox.Show($"Tài khoản \"{user}\" là tài khoản NHÂN VIÊN.\n\nVui lòng chọn vào ô \"Nhân Viên\" để đăng nhập.",
                                            "Sai Vai Trò", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // [CẬP NHẬT MỚI] Thay vì báo lỗi chung chung, gọi hàm kiểm tra chi tiết
                            CheckAccountExistenceAndShowError(user);
                        }
                        return;
                    }
                }

                // ============================================================
                // TRƯỜNG HỢP 2: NGƯỜI DÙNG CHỌN "NHÂN VIÊN"
                // ============================================================
                if (isNhanVien)
                {
                    // Thử đăng nhập đúng vai trò Nhân viên
                    if (NhanVienBLL.DangNhapNhanVien(user, pass, out string tenNv))
                    {
                        // -> THÀNH CÔNG
                        if (string.IsNullOrWhiteSpace(tenNv)) tenNv = user;
                        AppSession.CurrentMaNV = user;

                        var appUser = new AppUser { UserName = user, FullName = tenNv, Role = "NhanVien" };
                        MessageBox.Show($"Đăng nhập thành công!\nXin chào nhân viên: {appUser.FullName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.LoggedInUser = appUser;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        // -> THẤT BẠI. KHOAN, KIỂM TRA XEM CÓ PHẢI LÀ QUẢN LÝ KHÔNG?
                        // Gọi thử hàm đăng nhập Quản lý để check
                        var tkCheck = new TaiKhoan { TenDangNhap = user, MatKhau = pass };
                        string tempRole, tempName;

                        if (TaiKhoanBLL.DangNhapTaiKhoan(tkCheck, out tempRole, out tempName))
                        {
                            // Nếu vào được đây, tức là User/Pass đúng, nhưng nó là Quản Lý
                            MessageBox.Show($"Tài khoản \"{user}\" là tài khoản QUẢN LÝ.\n\nVui lòng chọn vào ô \"Quản Lý\" để đăng nhập.",
                                            "Sai Vai Trò", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // [CẬP NHẬT MỚI] Thay vì báo lỗi chung chung, gọi hàm kiểm tra chi tiết
                            CheckAccountExistenceAndShowError(user);
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL hoặc lỗi hệ thống:\n" + ex.Message,
                                "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // HÀM PHỤ TRỢ MỚI: Kiểm tra tài khoản tồn tại hay sai mật khẩu
        // =========================================================================
        private void CheckAccountExistenceAndShowError(string user)
        {
            // Kiểm tra tồn tại bên Admin (TaiKhoan)
            // Lưu ý: Hàm GetPasswordByUser trả về null nếu không tìm thấy user
            string passAdmin = null;
            try { passAdmin = TaiKhoanBLL.GetPasswordByUser(user); } catch { }

            // Kiểm tra tồn tại bên Nhân viên
            string passNV = null;
            try { passNV = NhanVienBLL.GetPasswordByMaNV(user); } catch { }

            // Nếu cả 2 đều null hoặc rỗng -> Tài khoản hoàn toàn không có trong hệ thống
            if (string.IsNullOrEmpty(passAdmin) && string.IsNullOrEmpty(passNV))
            {
                MessageBox.Show($"Tài khoản \"{user}\" không tồn tại trong hệ thống!",
                                "Tài khoản không tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Nếu tìm thấy 1 trong 2 -> Có tồn tại nhưng đăng nhập thất bại -> Sai mật khẩu
                MessageBox.Show("Mật khẩu không đúng. Vui lòng kiểm tra lại.",
                                "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helpers
        private static System.Collections.Generic.IEnumerable<Control> AllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (var ch in AllControls(c)) yield return ch;
            }
        }
        private RadioButton FindRadioByNames(params string[] names)
        {
            foreach (var name in names)
            {
                var found = this.Controls.Find(name, true);
                if (found != null && found.Length > 0 && found[0] is RadioButton rb) return rb;
            }
            return null;
        }
        private RadioButton FindRadioByText(params string[] texts)
        {
            foreach (var rb in AllControls(this).OfType<RadioButton>())
            {
                foreach (var t in texts)
                {
                    if (!string.IsNullOrWhiteSpace(rb.Text) && rb.Text.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0) return rb;
                }
            }
            return null;
        }
        private RadioButton GetRadioQuanLy() => FindRadioByNames("rdoQuanLy", "rbQuanLy", "radioQuanLy", "radQuanLy") ?? FindRadioByText("Quản", "Quản Lý");
        private RadioButton GetRadioNhanVien() => FindRadioByNames("rdoNhanVien", "rbNhanVien", "radioNhanVien", "radNhanVien") ?? FindRadioByText("Nhân", "Nhân Viên");
    }
}