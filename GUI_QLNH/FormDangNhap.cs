using BLL_QLNH;
using DAL_QLDT;
using DTO_QLNH;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI_QLNH
{
    public partial class FormDangNhap : Form
    {
        private string lastAutoFillUser = null;

        // expose logged-in user for Program.Main to read after ShowDialog
        public AppUser LoggedInUser { get; private set; }

        // Layout config
        private const int LeftMin = 240;
        private const int LeftMax = 360;
        private const double LeftRatio = 0.32; // ~32% width

        public FormDangNhap()
        {
            InitializeComponent();
        }

        // True khi đang chạy trong Designer
        private static bool IsDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            try { return Process.GetCurrentProcess().ProcessName.Equals("devenv", StringComparison.OrdinalIgnoreCase); }
            catch { return false; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsDesignMode()) return;

            // Canh layout ban đầu
            UpdateResponsiveLayout();

            // Gắn sự kiện (tháo cũ để tránh gắn trùng)
            chkShowPass.CheckedChanged -= chkShowPass_CheckedChanged;
            chkShowPass.CheckedChanged += chkShowPass_CheckedChanged;

            txtMatKhau.KeyDown -= txtMatKhau_KeyDown;
            txtMatKhau.KeyDown += txtMatKhau_KeyDown;

            txtTenDangNhap.TextChanged -= TxtTenDangNhap_TextChanged;
            txtTenDangNhap.TextChanged += TxtTenDangNhap_TextChanged;

            btnDangNhap.Click -= btnDangNhap_Click;
            btnDangNhap.Click += btnDangNhap_Click;

            // Mặc định ẩn mật khẩu
            txtMatKhau.UseSystemPasswordChar = !chkShowPass.Checked;

            // Mặc định chọn Quản lý nếu chưa chọn gì (tự tìm radio)
            var rbQL = GetRadioQuanLy();
            var rbNV = GetRadioNhanVien();
            if (rbQL != null && rbNV != null && !rbQL.Checked && !rbNV.Checked)
                rbQL.Checked = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (IsDesignMode()) return;
            UpdateResponsiveLayout();
        }

        /// <summary>
        /// Giữ tỉ lệ panel trái và căn giữa card đăng nhập, cho input/nút co giãn.
        /// </summary>
        private void UpdateResponsiveLayout()
        {
            // Null-guard
            if (splitMain == null || pnlRight == null || pnlContent == null) return;
            if (splitMain.Width <= 0) return;

            // 1) panel trái
            int leftWidth = (int)Math.Round(ClientSize.Width * LeftRatio);
            leftWidth = Math.Max(LeftMin, Math.Min(leftWidth, LeftMax));
            int maxDistance = splitMain.Width - 200; // chừa tối thiểu panel phải
            splitMain.SplitterDistance = Math.Max(LeftMin, Math.Min(leftWidth, maxDistance));

            // 2) card căn giữa
            if (pnlRight.ClientSize.Width <= 0 || pnlRight.ClientSize.Height <= 0) return;

            int maxCardWidth = 560;
            int margin = 40;
            int targetWidth = Math.Min(maxCardWidth, Math.Max(480, pnlRight.ClientSize.Width - margin * 2));
            pnlContent.Width = targetWidth;
            pnlContent.Left = (pnlRight.ClientSize.Width - pnlContent.Width) / 2;
            pnlContent.Top = 40;

            // 3) input co giãn
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

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            txtMatKhau.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        private void txtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsDesignMode()) return;
            if (e.KeyCode == Keys.Enter) btnDangNhap.PerformClick();
        }

        private void TxtTenDangNhap_TextChanged(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;

            try
            {
                var user = txtTenDangNhap.Text.Trim();

                if (string.IsNullOrEmpty(user))
                {
                    if (lastAutoFillUser != null)
                    {
                        if (!txtMatKhau.Focused) txtMatKhau.Text = string.Empty;
                        lastAutoFillUser = null;
                    }
                    return;
                }

                if (txtMatKhau.Focused) return; // không override khi user đang gõ
                if (string.Equals(user, lastAutoFillUser, StringComparison.Ordinal)) return;

                string pwd = null;
                try { pwd = TaiKhoanBLL.GetPasswordByUser(user); } catch { /* ignore */ }
                if (string.IsNullOrEmpty(pwd))
                {
                    try { pwd = NhanVienBLL.GetPasswordByMaNV(user); } catch { /* ignore */ }
                }

                if (!string.IsNullOrEmpty(pwd))
                {
                    txtMatKhau.Text = pwd;
                    lastAutoFillUser = user;
                }
                else
                {
                    if (lastAutoFillUser != null)
                    {
                        txtMatKhau.Text = string.Empty;
                        lastAutoFillUser = null;
                    }
                }
            }
            catch { /* ignore */ }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;

            string user = txtTenDangNhap.Text.Trim();
            string pass = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập và Mật khẩu.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ===== Chọn role theo radio (tự tìm control) =====
                // Prefer explicit named radio controls if present in Designer, fallback to helper search
                var rbQL = this.Controls.Find("rdoQuanLy", true).OfType<RadioButton>().FirstOrDefault() ?? GetRadioQuanLy();
                var rbNV = this.Controls.Find("rdoNhanVien", true).OfType<RadioButton>().FirstOrDefault() ?? GetRadioNhanVien();

                bool isQuanLy = rbQL != null && rbQL.Checked;
                bool isNhanVien = rbNV != null && rbNV.Checked;

                if (!isQuanLy && !isNhanVien)
                {
                    MessageBox.Show("Vui lòng chọn Quản lý hoặc Nhân viên trước khi đăng nhập.",
                        "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (isQuanLy)
                {
                    // Quản lý (bảng TaiKhoan)
                    var tk = new TaiKhoan { TenDangNhap = user, MatKhau = pass };
                    if (TaiKhoanBLL.DangNhapTaiKhoan(tk, out string vaiTro, out string hoTen))
                    {
                        if (string.IsNullOrWhiteSpace(vaiTro)) vaiTro = "Admin";
                        if (string.IsNullOrWhiteSpace(hoTen)) hoTen = user;

                        var appUser = new AppUser { UserName = user, FullName = hoTen, Role = vaiTro };
                        MessageBox.Show($"Đăng nhập thành công!\nXin chào {appUser.FullName} ({appUser.Role}).",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // set LoggedInUser and close dialog with OK so Program.Main will start FormMenu
                        this.LoggedInUser = appUser;
                        this.DialogResult = DialogResult.OK;
                        return;
                    }

                    MessageBox.Show("Sai tài khoản Quản lý (bảng TaiKhoan).",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMatKhau.SelectAll(); txtMatKhau.Focus();
                    return;
                }

                if (isNhanVien)
                {
                    // Nhân viên (MãNV + mật khẩu bảng NhanVien)
                    if (NhanVienBLL.DangNhapNhanVien(user, pass, out string tenNv))
                    {
                        if (string.IsNullOrWhiteSpace(tenNv)) tenNv = user;

                        // Set session current MaNV so child forms can read
                        AppSession.CurrentMaNV = user;

                        var appUser = new AppUser { UserName = user, FullName = tenNv, Role = "NhanVien" };
                        MessageBox.Show($"Đăng nhập thành công!\nXin chào {appUser.FullName} (Nhân viên).",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // set LoggedInUser and close dialog with OK so Program.Main will start FormMenuNhanVien
                        this.LoggedInUser = appUser;
                        this.DialogResult = DialogResult.OK;
                        return;
                    }

                    MessageBox.Show("Sai tài khoản Nhân viên (Mã NV + mật khẩu bảng NhanVien).",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMatKhau.SelectAll(); txtMatKhau.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ======= Điều hướng sau đăng nhập =======
        private void OpenMenuQuanLy(AppUser appUser)
        {
            var menu = new FormMenu(appUser);              // ctor(AppUser) đã có
            this.Hide();
            menu.FormClosed += (s, args) => this.Close();
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void OpenMenuNhanVien(AppUser appUser)
        {
            var menuNv = new FormMenuNhanVien(appUser);    // ctor(AppUser) đã có
            this.Hide();
            menuNv.FormClosed += (s, args) => this.Close();
            menuNv.StartPosition = FormStartPosition.CenterScreen;
            menuNv.Show();
        }

        // ======================= Helpers tìm Radio =======================
        private static System.Collections.Generic.IEnumerable<Control> AllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (var ch in AllControls(c)) yield return ch;
            }
        }

        // tìm theo danh sách name (đệ quy toàn form)
        private RadioButton FindRadioByNames(params string[] names)
        {
            foreach (var name in names)
            {
                var found = this.Controls.Find(name, true);
                if (found != null && found.Length > 0 && found[0] is RadioButton rb) return rb;
            }
            return null;
        }

        // tìm theo text hiển thị (contains, không phân biệt hoa thường)
        private RadioButton FindRadioByText(params string[] texts)
        {
            foreach (var rb in AllControls(this).OfType<RadioButton>())
            {
                foreach (var t in texts)
                {
                    if (!string.IsNullOrWhiteSpace(rb.Text) &&
                        rb.Text.IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return rb;
                    }
                }
            }
            return null;
        }

        private RadioButton GetRadioQuanLy()
        {
            return FindRadioByNames("rdoQuanLy", "rbQuanLy", "radioQuanLy", "radQuanLy")
                ?? FindRadioByText("Quản", "Quản Lý");
        }

        private RadioButton GetRadioNhanVien()
        {
            return FindRadioByNames("rdoNhanVien", "rbNhanVien", "radioNhanVien", "radNhanVien")
                ?? FindRadioByText("Nhân", "Nhân Viên");
        }
    }
}
