using DTO_QLNH; // AppUser
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;

namespace GUI_QLNH
{
    public partial class FormMenu : Form
    {
        // Chống nháy khi vẽ/resize
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        private Button _currentBtn;
        private Form _activeChild;
        private Panel _navIndicator;

        private AppUser _currentUser;

        // ĐỔI CHO KHỚP NAMESPACE/CLASS FORM CON CỦA BẠN
        private const string FormThucKhachQnHs = "GUI_QLNHS.FormThucKhach";
        private const string FormNhanVienQnH = "GUI_QLNH.FormNhanVien";
        private const string FormThucDonQnH = "GUI_QLNH.FormThucDon";
        private const string FormDatTiecQnHs = "GUI_QLNHS.FormDatTiec";
        private const string FormHoaDonQnHs = "GUI_QLNHS.FormQLHoaDon";

        public FormMenu()
        {
            InitializeComponent();

            // mượt
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            ApplyTheme();
            timerClock_Tick(null, EventArgs.Empty); // đồng hồ chạy ngay

            // Do not auto-open any child on startup. Show empty content and let user choose.
        }

        public FormMenu(AppUser user) : this()
        {
            _currentUser = user;
            var name = string.IsNullOrWhiteSpace(user?.FullName) ? user?.UserName : user.FullName;

            if (lblGreet != null)
                lblGreet.Text = string.IsNullOrWhiteSpace(name) ? "Xin Chào Bạn Quản Lý" : $"Xin chào, {name}";
            if (!string.IsNullOrWhiteSpace(user?.Role))
                Text = $"MainQuanLy - {user.Role}";
        }

        // ===== Palette
        private Color C1 => ColorTranslator.FromHtml("#16D9E3");
        private Color C2 => ColorTranslator.FromHtml("#30C7EC");
        private Color C3 => ColorTranslator.FromHtml("#46AEF7");

        private Color ColSidebar => C2;
        private Color ColBtn => ColorTranslator.FromHtml("#16B9D4");
        private Color ColBtnHover => ColorTranslator.FromHtml("#1EC2E0");
        private Color ColBtnActive => C3;
        private Color ColText => Color.White;
        private Color ColTextActive => Color.Black;
        private Color ColBorder => ColorTranslator.FromHtml("#D8EEF8");

        // ===== Theme
        private void ApplyTheme()
        {
            // Header gradient
            if (pnlHeader != null)
            {
                EnableDoubleBuffer(pnlHeader);
                pnlHeader.BackColor = C2;
                pnlHeader.Paint -= PnlHeader_Paint;
                pnlHeader.Paint += PnlHeader_Paint;
            }

            if (lblGreet != null) { lblGreet.Text = "Xin Chào Bạn Quản Lý"; lblGreet.ForeColor = ColText; }
            if (lblTime != null) lblTime.ForeColor = ColText;
            if (lblDate != null) lblDate.ForeColor = ColText;

            // Sidebar
            if (pnlSidebar != null)
            {
                EnableDoubleBuffer(pnlSidebar);
                PaintFlat(pnlSidebar, ColSidebar);

                foreach (var btn in pnlSidebar.Controls.OfType<Button>())
                    StyleSideButton(btn);

                if (picLogo != null) picLogo.BackColor = ColSidebar;
                foreach (var pb in new[] { picThucKhach, picNhanVien, picThucDon, picDatTiec, picThongKe })
                    if (pb != null) pb.BackColor = ColSidebar;

                if (pnlSidebar.Parent is Panel parentLeft)
                    parentLeft.BackColor = ColSidebar;

                if (_navIndicator == null)
                {
                    _navIndicator = new Panel
                    {
                        Width = 4,
                        Height = 44,
                        BackColor = C2,
                        Left = pnlSidebar.Padding.Left,
                        Visible = false
                    };
                    pnlSidebar.Controls.Add(_navIndicator);
                    _navIndicator.BringToFront();
                }

                pnlSidebar.Resize -= Sidebar_Resize_Repaint;
                pnlSidebar.Resize += Sidebar_Resize_Repaint;
            }

            // Content
            if (pnlContentBorder != null) pnlContentBorder.BackColor = ColBorder;
            if (pnlContent != null)
            {
                EnableDoubleBuffer(pnlContent);
                pnlContent.BackColor = Color.White;
            }
        }

        private void Sidebar_Resize_Repaint(object sender, EventArgs e) => PaintFlat(pnlSidebar, ColSidebar);

        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = pnlHeader.ClientRectangle;
            using (var br = new LinearGradientBrush(r, C1, C3, 0f))
            {
                br.InterpolationColors = new ColorBlend
                {
                    Positions = new[] { 0f, 0.5f, 1f },
                    Colors = new[] { C1, C2, C3 }
                };
                e.Graphics.FillRectangle(br, r);
            }
            using (var pen = new Pen(Color.FromArgb(210, Color.White), 1f))
                e.Graphics.DrawLine(pen, 0, r.Height - 1, r.Width, r.Height - 1);
        }

        private static void EnableDoubleBuffer(Control c)
        {
            if (c == null) return;
            var pi = c.GetType().GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (pi != null) pi.SetValue(c, true, null);
        }

        private void PaintFlat(Control root, Color c)
        {
            if (root == null) return;

            if (root is Panel || root is PictureBox || root is Label ||
                root is FlowLayoutPanel || root is GroupBox)
                root.BackColor = c;

            var btn = root as Button;
            if (btn != null)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = ColBtn;
                btn.ForeColor = ColText;
            }

            foreach (Control child in root.Controls) PaintFlat(child, c);
        }

        private void StyleSideButton(Button b)
        {
            if (b == null) return;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = ColBtn;
            b.ForeColor = ColText;
            b.Cursor = Cursors.Hand;
            b.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            b.Height = 44;

            b.MouseEnter -= Btn_MouseEnter;
            b.MouseLeave -= Btn_MouseLeave;
            b.MouseEnter += Btn_MouseEnter;
            b.MouseLeave += Btn_MouseLeave;
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null && b != _currentBtn) b.BackColor = ColBtnHover;
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b != null && b != _currentBtn) b.BackColor = ColBtn;
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            try
            {
                var now = DateTime.Now;
                if (lblTime != null) lblTime.Text = now.ToString("hh:mm:ss tt");
                if (lblDate != null) lblDate.Text = now.ToString("dddd, MMMM dd, yyyy");
            }
            catch { }
        }

        // ===== Mở form con – thuần C# 7.3 (dùng Substring, không dùng ..)
        private void OpenChildSafe(string formQualifiedName, object btnSender, string title)
        {
            try
            {
                Type t = null;

                // 1) direct
                t = Type.GetType(formQualifiedName);

                // 2) search assemblies
                if (t == null)
                {
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        try { t = asm.GetType(formQualifiedName); }
                        catch { t = null; }
                        if (t != null) break;
                    }
                }

                // 3) fallback theo short name (SubString – C#7.3)
                if (t == null)
                {
                    string shortName;
                    int dot = formQualifiedName.LastIndexOf('.');
                    if (dot >= 0 && dot + 1 < formQualifiedName.Length)
                        shortName = formQualifiedName.Substring(dot + 1);
                    else
                        shortName = formQualifiedName;

                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        Type[] types = null;
                        try { types = asm.GetTypes(); } catch { types = new Type[0]; }

                        foreach (var ty in types)
                        {
                            if (string.Equals(ty.Name, shortName, StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(ty.FullName, formQualifiedName, StringComparison.OrdinalIgnoreCase))
                            { t = ty; break; }
                        }
                        if (t != null) break;
                    }
                }

                if (t == null) throw new Exception("Form không tồn tại hoặc sai namespace.");

                Form child = null;
                // Try ctor(AppUser) when available and we have a current user
                try
                {
                    if (_currentUser != null)
                    {
                        var ctor = t.GetConstructor(new[] { typeof(AppUser) });
                        if (ctor != null)
                        {
                            child = ctor.Invoke(new object[] { _currentUser }) as Form;
                        }
                    }
                }
                catch { child = null; }

                if (child == null)
                {
                    try { child = Activator.CreateInstance(t) as Form; } catch { child = null; }
                }
                if (child == null) throw new Exception("Không khởi tạo được form.");

                OpenChild(child, btnSender);
                Text = "MainQuanLy - " + title;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Chưa có form \"" + title + "\" trong project hoặc sai namespace.\nChi tiết: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenChild(Form child, object btnSender)
        {
            // reset màu các nút
            if (pnlSidebar != null)
            {
                foreach (var btn in pnlSidebar.Controls.OfType<Button>())
                {
                    btn.BackColor = ColBtn;
                    btn.ForeColor = ColText;
                }
            }

            // set active
            _currentBtn = btnSender as Button;
            if (_currentBtn != null)
            {
                _currentBtn.BackColor = ColBtnActive;
                _currentBtn.ForeColor = ColTextActive;

                if (_navIndicator != null)
                {
                    _navIndicator.Visible = true;
                    _navIndicator.BackColor = C2;
                    _navIndicator.Top = _currentBtn.Top;
                    _navIndicator.Height = _currentBtn.Height;
                    _navIndicator.BringToFront();
                }
            }

            // đóng form cũ
            try
            {
                if (_activeChild != null)
                {
                    _activeChild.Close();
                    _activeChild.Dispose();
                }
            }
            catch { }

            // host form con
            _activeChild = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            if (pnlContent != null)
            {
                pnlContent.Controls.Clear();
                pnlContent.Controls.Add(child);
            }
            child.BringToFront();
            child.Show();
        }

        // ===== Buttons
        private void btnThucKhach_Click(object sender, EventArgs e) => OpenChildSafe(FormThucKhachQnHs, sender, "Thực khách");
        private void btnNhanVien_Click(object sender, EventArgs e) => OpenChildSafe(FormNhanVienQnH, sender, "Nhân viên");
        private void btnThucDon_Click(object sender, EventArgs e) => OpenChildSafe(FormThucDonQnH, sender, "Thực đơn");
        private void btnDatTiec_Click(object sender, EventArgs e) => OpenChildSafe(FormDatTiecQnHs, sender, "Đặt tiệc");
        private void btnThongKe_Click(object sender, EventArgs e) => OpenChildSafe(FormHoaDonQnHs, sender, "Hóa đơn/Thống kê");

        private void btnThuChi_Click(object sender, EventArgs e)
            => MessageBox.Show("Chưa có form Thu/Chi trong project.", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                // Open login dialog
                var login = new FormDangNhap(); // đổi nếu form đăng nhập của bạn tên khác
                Hide();
                var dr = login.ShowDialog();

                if (dr == DialogResult.OK && login.LoggedInUser != null)
                {
                    // Reopen appropriate menu based on role
                    var user = login.LoggedInUser;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(user.Role) && user.Role.IndexOf("nhan", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            var menuNv = new FormMenuNhanVien(user);
                            menuNv.StartPosition = FormStartPosition.CenterScreen;
                            menuNv.Show();
                        }
                        else
                        {
                            var menu = new FormMenu(user);
                            menu.StartPosition = FormStartPosition.CenterScreen;
                            menu.Show();
                        }
                    }
                    catch
                    {
                        // if opening fails, just exit
                    }

                    Close();
                    return;
                }

                // Cancelled or login failed → exit application
                Close();
            }
            catch
            {
                Close();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ApplyTheme();                  // ép lại bảng màu khi form đã hiển thị
            PaintFlat(pnlSidebar, ColSidebar);
        }
    }
}
