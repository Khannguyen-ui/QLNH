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
            // Bật style & text rendering chuẩn WinForms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Bắt lỗi toàn cục để không bị crash “đột tử”
            Application.ThreadException += OnThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            while (true)
            {
                // Show login dialog
                using (var login = new FormDangNhap())
                {
                    login.StartPosition = FormStartPosition.CenterScreen;
                    var result = login.ShowDialog();

                    if (result != DialogResult.OK || login.LoggedInUser == null)
                    {
                        // user cancelled or failed -> exit app
                        return;
                    }

                    // Got a logged in user -> run appropriate main form
                    AppUser user = login.LoggedInUser;
                    try
                    {
                        var role = user?.Role ?? string.Empty;
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

                    // When main form closes, loop back to show login again
                }
            }
        }

        // ===== Handlers lỗi toàn cục =====
        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try { MessageBox.Show(Convert.ToString(e.ExceptionObject), "Lỗi không xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
        }
    }
}
