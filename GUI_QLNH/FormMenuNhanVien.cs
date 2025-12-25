using DTO_QLNH;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GUI_QLNH
{
    public partial class FormMenuNhanVien : Form
    {
        private Button _currentBtn;
        private Form _activeChild;
        private readonly AppUser _currentUser;
        private Label _lblPlaceholder;

        // ===== Palette =====
        private static readonly Color ColHeaderFrom = Color.FromArgb(37, 99, 235);
        private static readonly Color ColHeaderTo = Color.FromArgb(59, 130, 246);
        private static readonly Color ColHeaderText = Color.White;

        private static readonly Color ColSidebarBg = Color.White;
        private static readonly Color ColBtnBg = Color.FromArgb(246, 247, 249);
        private static readonly Color ColBtnHover = Color.FromArgb(233, 236, 239);
        private static readonly Color ColBtnActive = Color.FromArgb(219, 234, 254);
        private static readonly Color ColBtnText = Color.FromArgb(23, 23, 23);
        private static readonly Color ColBorder = Color.FromArgb(229, 231, 235);

        // Giảm giật – chỉ bật khi runtime
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (!DesignMode) cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public FormMenuNhanVien(AppUser user)
        {
            _currentUser = user;
            InitializeComponent();

            // DoubleBuffer an toàn
            TryEnableDoubleBuffer(pnlContent);
            TryEnableDoubleBuffer(pnlContentBorder);
            TryEnableDoubleBuffer(pnlHeader);
            TryEnableDoubleBuffer(pnlSidebar);
            if (!DesignMode) this.DoubleBuffered = true;

            ApplyTheme();

            // Placeholder
            EnsurePlaceholder();
            ShowPlaceholder();

            // --- SỬA LỖI TIMER: Tự tạo nếu Designer chưa có ---
            if (timerClock == null)
            {
                timerClock = new System.Windows.Forms.Timer();
                this.components?.Add(timerClock); // Gắn vào components để tự dispose
            }

            timerClock.Interval = 1000;
            timerClock.Tick -= UpdateClock;
            timerClock.Tick += UpdateClock;
            UpdateClock(null, EventArgs.Empty);
            timerClock.Start(); // Dùng Start() thay vì Enabled = true
            // --------------------------------------------------

            // Xin chào
            var name = string.IsNullOrWhiteSpace(user?.FullName) ? user?.UserName : user.FullName;
            if (string.IsNullOrWhiteSpace(name)) name = "Nhân viên";
            lblGreet.Text = $"Xin chào, {name}";
            Text = "MainNhanVien";
        }

        private static void TryEnableDoubleBuffer(Control c)
        {
            try
            {
                c?.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                  ?.SetValue(c, true, null);
            }
            catch { }
        }

        private void ApplyTheme()
        {
            // Header
            pnlHeader.BackColor = ColHeaderFrom;
            lblGreet.ForeColor = ColHeaderText;
            lblTime.ForeColor = ColHeaderText;
            lblDate.ForeColor = ColHeaderText;

            // Sidebar
            pnlSidebar.BackColor = ColSidebarBg;
            foreach (var b in pnlSidebar.Controls.OfType<Button>()) StyleSideButton(b);

            // Content
            pnlContentBorder.BackColor = ColBorder;
            pnlContent.BackColor = Color.White;

            // Gradient header
            pnlHeader.Paint -= pnlHeader_Paint;
            pnlHeader.Paint += pnlHeader_Paint;
        }

        private void StyleSideButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = ColBtnBg;
            b.ForeColor = ColBtnText;
            b.Cursor = Cursors.Hand;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(14, 0, 12, 0);

            b.MouseEnter -= SideBtn_MouseEnter;
            b.MouseLeave -= SideBtn_MouseLeave;
            b.MouseEnter += SideBtn_MouseEnter;
            b.MouseLeave += SideBtn_MouseLeave;
        }

        private void SideBtn_MouseEnter(object s, EventArgs e)
        {
            var b = s as Button;
            if (b != null && b != _currentBtn) b.BackColor = ColBtnHover;
        }
        private void SideBtn_MouseLeave(object s, EventArgs e)
        {
            var b = s as Button;
            if (b != null && b != _currentBtn) b.BackColor = ColBtnBg;
        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            using (var br = new LinearGradientBrush(pnlHeader.ClientRectangle, ColHeaderFrom, ColHeaderTo, LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(br, pnlHeader.ClientRectangle);
            using (var p = new Pen(Color.FromArgb(0, 0, 0, 40), 1))
                e.Graphics.DrawLine(p, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            if (lblTime != null) lblTime.Text = now.ToString("h:mm:ss tt");
            if (lblDate != null) lblDate.Text = now.ToString("dddd, dd MMMM yyyy");
        }

        // ===== Open child vào khung content =====
        private void OpenChildSafe(string formQualifiedName, object btnSender, string title)
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                var t = asm.GetType(formQualifiedName, false, false) ?? Type.GetType(formQualifiedName, false, false);

                // Nếu không tìm thấy class, có thể do sai Namespace. Thử tìm lỏng hơn:
                if (t == null)
                {
                    t = asm.GetTypes().FirstOrDefault(x => x.Name == formQualifiedName.Split('.').Last());
                }

                if (t == null || !typeof(Form).IsAssignableFrom(t)) throw new InvalidOperationException();

                var child = Activator.CreateInstance(t) as Form ?? throw new InvalidOperationException();
                OpenChild(child, btnSender);
                Text = $"MainNhanVien - {title}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chưa mở được form \"{title}\".\nLỗi: {ex.Message}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenChild(Form child, object btnSender)
        {
            foreach (var b in pnlSidebar.Controls.OfType<Button>()) b.BackColor = ColBtnBg;
            _currentBtn = btnSender as Button;
            if (_currentBtn != null) _currentBtn.BackColor = ColBtnActive;

            if (_activeChild != null)
            {
                try { _activeChild.Close(); _activeChild.Dispose(); } catch { }
                _activeChild = null;
            }

            _activeChild = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            HidePlaceholder();
            pnlContent.SuspendLayout();
            pnlContent.Controls.Add(child);
            child.BringToFront();
            pnlContent.ResumeLayout();
            child.Show();
        }

        // ==== Handlers sidebar ====
        private void btnInfo_Click(object sender, EventArgs e)
            => OpenChildSafe("GUI_QLNH.FormThongTinNhanVien", sender, "Thông tin nhân viên");

        private void btnOrder_Click(object sender, EventArgs e)
            => OpenChildSafe("GUI_QLNH.FormOrderChoKhach", sender, "Order cho khách");

        // --- ĐÂY LÀ PHẦN ĐÃ SỬA CHỮA ---
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Tắt timer an toàn
                if (timerClock != null)
                {
                    timerClock.Stop();
                }

                // Đóng form này -> Program.cs sẽ tự mở lại Form Đăng nhập
                this.Close();
            }
        }
        // --------------------------------

        private void btnLapThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                // Đảm bảo bạn đã có class FormLapThanhToan
                var f = new FormLapThanhToan();

                // Truyền mã nhân viên vào nếu Form đó có property này
                try
                {
                    // Dùng Reflection để gán tránh lỗi biên dịch nếu chưa có Property
                    var prop = f.GetType().GetProperty("CurrentManv");
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(f, _currentUser?.UserName);
                    }
                }
                catch { }

                OpenChild(f, sender);
                Text = "MainNhanVien - Lập thanh toán";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở Form Lập Thanh Toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsurePlaceholder()
        {
            try
            {
                if (pnlContent == null) return;
                if (_lblPlaceholder != null) return;

                _lblPlaceholder = new Label();
                _lblPlaceholder.AutoSize = false;
                _lblPlaceholder.Dock = DockStyle.Fill;
                _lblPlaceholder.TextAlign = ContentAlignment.MiddleCenter;
                _lblPlaceholder.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
                _lblPlaceholder.ForeColor = Color.FromArgb(120, 120, 120);
                _lblPlaceholder.BackColor = Color.Transparent;
                _lblPlaceholder.Text = "Vui lòng chọn chức năng ở thanh bên trái";

                pnlContent.Controls.Add(_lblPlaceholder);
                _lblPlaceholder.BringToFront();
            }
            catch { }
        }

        private void ShowPlaceholder()
        {
            try
            {
                if (_lblPlaceholder == null) EnsurePlaceholder();
                if (_lblPlaceholder != null) _lblPlaceholder.Visible = true;
            }
            catch { }
        }

        private void HidePlaceholder()
        {
            try { if (_lblPlaceholder != null) _lblPlaceholder.Visible = false; } catch { }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (timerClock != null)
                {
                    timerClock.Stop();
                    timerClock.Tick -= UpdateClock;
                }
                if (_activeChild != null)
                {
                    _activeChild.Close();
                    _activeChild.Dispose();
                    _activeChild = null;
                }
            }
            catch { }
            base.OnFormClosed(e);
        }
    }
}