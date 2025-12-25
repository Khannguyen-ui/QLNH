using System;
using System.Threading;
using System.Windows.Forms;
using GUI_QLNH;
using DTO_QLNH;

namespace GUI_QLNH
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Bắt lỗi toàn cục
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            while (true)
            {
                // Mở form đăng nhập
                using (var login = new FormDangNhap())
                {
                    login.StartPosition = FormStartPosition.CenterScreen;
                    var result = login.ShowDialog();

                    // Nếu đóng form hoặc đăng nhập thất bại thì thoát luôn
                    if (result != DialogResult.OK || login.LoggedInUser == null)
                    {
                        return;
                    }

                    // Lấy thông tin user đăng nhập thành công
                    AppUser user = login.LoggedInUser;
                    try
                    {
                        var role = user?.Role ?? string.Empty;

                        // Phân quyền mở form
                        if (string.Equals(role, "NhanVien", StringComparison.OrdinalIgnoreCase) ||
                            role.IndexOf("nhan", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Application.Run(new FormMenuNhanVien(user));
                        }
                        else
                        {
                            Application.Run(new FormMenu(user));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể khởi chạy form chính: " + ex.Message, "Lỗi",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Lỗi Thread", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(e.ExceptionObject.ToString(), "Lỗi Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }
    }
}