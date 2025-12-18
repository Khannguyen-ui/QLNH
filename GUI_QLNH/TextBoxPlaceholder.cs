using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GUI_QLNH
{
    public static class TextBoxPlaceholder
    {
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        /// <summary>
        /// Đặt “placeholder/cue banner” cho TextBox (hỗ trợ .NET Framework).
        /// showEvenWhenFocused = true để vẫn hiển thị khi TextBox có focus.
        /// </summary>
        public static void Set(TextBox tb, string text, bool showEvenWhenFocused = true)
        {
            if (tb == null) return;

            // Nếu handle chưa tạo, đợi tạo rồi set
            if (!tb.IsHandleCreated)
            {
                tb.HandleCreated += (s, e) =>
                {
                    try
                    {
                        SendMessage(tb.Handle, EM_SETCUEBANNER,
                                    (IntPtr)(showEvenWhenFocused ? 1 : 0),
                                    text ?? "");
                    }
                    catch { }
                };
                return;
            }

            try
            {
                SendMessage(tb.Handle, EM_SETCUEBANNER,
                            (IntPtr)(showEvenWhenFocused ? 1 : 0),
                            text ?? "");
            }
            catch { }
        }

        // Clear the TextBox content
        public static void Clear(TextBox tb)
        {
            if (tb == null) return;
            tb.Text = string.Empty;
        }

        // Get the user text (returns empty if none)
        public static string GetText(TextBox tb)
        {
            if (tb == null) return string.Empty;
            return tb.Text ?? string.Empty;
        }

        // Set user text into TextBox (removes cue banner display responsibility remains)
        public static void SetText(TextBox tb, string text)
        {
            if (tb == null) return;
            tb.Text = text ?? string.Empty;
        }
    }
}
