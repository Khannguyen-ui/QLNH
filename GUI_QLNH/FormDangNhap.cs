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
            // BƯỚC 1: Bắt lỗi ngay tại Constructor
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
            // BƯỚC 2: Bắt lỗi tại sự kiện Load
            try
            {
                base.OnLoad(e);
                if (IsDesignMode()) return;

                // Thử canh chỉnh layout, nếu lỗi thì bỏ qua chứ không cho crash
                try { UpdateResponsiveLayout(); } catch { }

                // Gắn sự kiện
                chkShowPass.CheckedChanged -= chkShowPass_CheckedChanged;
                chkShowPass.CheckedChanged += chkShowPass_CheckedChanged;

                txtMatKhau.KeyDown -= txtMatKhau_KeyDown;
                txtMatKhau.KeyDown += txtMatKhau_KeyDown;

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
                    MessageBox.Show("Lỗi khi tìm RadioButton: " + exRadio.Message);
                }
            }
            catch (Exception ex)
            {
                // Đây là nơi sẽ hiện ra lỗi khiến Form bạn bị tắt ngúm
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

                // Fix lỗi Crash nếu Width quá nhỏ
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
            catch { /* Bỏ qua lỗi giao diện để form không bị tắt */ }
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
            // BƯỚC 3: QUAN TRỌNG NHẤT
            // Nếu Form chưa hiện lên hẳn (Visible = false) thì KHÔNG CHẠY LOGIC NÀY
            // Vì nếu chạy lúc này, nó gọi SQL -> Lỗi kết nối -> Sập Form
            if (!this.Visible || IsDesignMode()) return;

            try
            {
                var user = txtTenDangNhap.Text.Trim();
                if (string.IsNullOrEmpty(user)) return; // Bỏ qua nếu rỗng

                if (txtMatKhau.Focused) return;
                if (string.Equals(user, lastAutoFillUser, StringComparison.Ordinal)) return;

                // Thêm Try-Catch từng lệnh gọi SQL để không sập chương trình
                string pwd = null;
                try
                {
                    pwd = TaiKhoanBLL.GetPasswordByUser(user);
                }
                catch (Exception exSQL)
                {
                    // Nếu lỗi SQL ở đây, ta chỉ ghi nhận chứ không crash
                    Debug.WriteLine("Lỗi auto-fill SQL: " + exSQL.Message);
                }

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
            catch { /* ignore */ }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;

            string user = txtTenDangNhap.Text.Trim();
            string pass = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập và Mật khẩu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var rbQL = this.Controls.Find("rdoQuanLy", true).OfType<RadioButton>().FirstOrDefault() ?? GetRadioQuanLy();
                var rbNV = this.Controls.Find("rdoNhanVien", true).OfType<RadioButton>().FirstOrDefault() ?? GetRadioNhanVien();

                bool isQuanLy = rbQL != null && rbQL.Checked;
                bool isNhanVien = rbNV != null && rbNV.Checked;

                if (!isQuanLy && !isNhanVien)
                {
                    MessageBox.Show("Vui lòng chọn Quản lý hoặc Nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (isQuanLy)
                {
                    var tk = new TaiKhoan { TenDangNhap = user, MatKhau = pass };
                    // Nếu lỗi SQL xảy ra ở đây, nó sẽ nhảy xuống catch thay vì tắt app
                    if (TaiKhoanBLL.DangNhapTaiKhoan(tk, out string vaiTro, out string hoTen))
                    {
                        if (string.IsNullOrWhiteSpace(vaiTro)) vaiTro = "Admin";
                        if (string.IsNullOrWhiteSpace(hoTen)) hoTen = user;

                        var appUser = new AppUser { UserName = user, FullName = hoTen, Role = vaiTro };
                        MessageBox.Show($"Đăng nhập thành công!\nXin chào {appUser.FullName}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.LoggedInUser = appUser;
                        this.DialogResult = DialogResult.OK;
                        this.Close(); // Đóng form để về Program.cs
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu Quản lý.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (isNhanVien)
                {
                    if (NhanVienBLL.DangNhapNhanVien(user, pass, out string tenNv))
                    {
                        if (string.IsNullOrWhiteSpace(tenNv)) tenNv = user;
                        AppSession.CurrentMaNV = user;

                        var appUser = new AppUser { UserName = user, FullName = tenNv, Role = "NhanVien" };
                        MessageBox.Show($"Đăng nhập thành công!\nXin chào {appUser.FullName}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.LoggedInUser = appUser;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Sai mã nhân viên hoặc mật khẩu.", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Đây là chỗ bắt lỗi SQL khi bấm nút Đăng nhập
                MessageBox.Show("Lỗi kết nối CSDL hoặc lỗi hệ thống:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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