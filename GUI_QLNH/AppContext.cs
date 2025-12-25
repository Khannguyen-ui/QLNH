using System;
using System.Windows.Forms;
using DTO_QLNH;

namespace GUI_QLNH
{
    public class AppContext : ApplicationContext
    {
        public AppContext()
        {
            ShowLogin();
        }

        private void ShowLogin()
        {
            var login = new FormDangNhap
            {
                StartPosition = FormStartPosition.CenterScreen
            };

            if (login.ShowDialog() == DialogResult.OK &&
                login.LoggedInUser != null)
            {
                OpenMainForm(login.LoggedInUser);
            }
            else
            {
                ExitThread(); // ❗ THOÁT APP
            }
        }

        private void OpenMainForm(AppUser user)
        {
            Form mainForm;

            var role = user.Role ?? string.Empty;
            if (role.Contains("nhan", StringComparison.OrdinalIgnoreCase))
                mainForm = new FormMenuNhanVien(user);
            else
                mainForm = new FormMenu(user);

            // Khi đóng menu → quay lại login
            mainForm.FormClosed += (s, e) => ShowLogin();
            mainForm.Show();
        }
    }
}
