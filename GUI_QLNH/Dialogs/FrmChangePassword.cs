using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_QLNH.Dialogs
{
    // Dialog đổi mật khẩu viết full bằng code, không cần Designer
    public class FrmChangePassword : Form
    {
        private TextBox txtOld, txtNew, txtNew2;
        private Button btnOK, btnCancel;
        private CheckBox chkShow;

        // Lấy dữ liệu sau khi bấm OK
        public string CurrentPassword => txtOld.Text;
        public string NewPassword => txtNew.Text;

        public FrmChangePassword()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Đổi mật khẩu";
            Font = new Font("Segoe UI", 10);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ClientSize = new Size(440, 230);
            Padding = new Padding(12);

            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(4)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 3; i++) tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

            // Controls
            txtOld = new TextBox { UseSystemPasswordChar = true, Dock = DockStyle.Fill };
            txtNew = new TextBox { UseSystemPasswordChar = true, Dock = DockStyle.Fill };
            txtNew2 = new TextBox { UseSystemPasswordChar = true, Dock = DockStyle.Fill };
            chkShow = new CheckBox { Text = "Hiện mật khẩu", AutoSize = true, Anchor = AnchorStyles.Left };

            // Add rows
            tlp.Controls.Add(new Label { Text = "Mật khẩu hiện tại:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            tlp.Controls.Add(txtOld, 1, 0);

            tlp.Controls.Add(new Label { Text = "Mật khẩu mới:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
            tlp.Controls.Add(txtNew, 1, 1);

            tlp.Controls.Add(new Label { Text = "Nhập lại mật khẩu:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
            tlp.Controls.Add(txtNew2, 1, 2);

            tlp.Controls.Add(chkShow, 1, 3);

            // Buttons
            var pnlBtn = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 52,
                Padding = new Padding(6)
            };
            btnOK = new Button { Text = "Đồng ý", Width = 110 };
            btnCancel = new Button { Text = "Hủy", Width = 100 };

            btnOK.Click += (s, e) =>
            {
                // validate cơ bản
                if (string.IsNullOrWhiteSpace(txtOld.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu hiện tại."); txtOld.Focus(); return;
                }
                if (string.IsNullOrWhiteSpace(txtNew.Text) || txtNew.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu mới tối thiểu 6 ký tự."); txtNew.Focus(); return;
                }
                if (txtNew.Text != txtNew2.Text)
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp."); txtNew2.Focus(); return;
                }
                DialogResult = DialogResult.OK;
            };
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            chkShow.CheckedChanged += (s, e) =>
            {
                bool show = chkShow.Checked;
                txtOld.UseSystemPasswordChar = !show;
                txtNew.UseSystemPasswordChar = !show;
                txtNew2.UseSystemPasswordChar = !show;
            };

            pnlBtn.Controls.AddRange(new Control[] { btnOK, btnCancel });

            Controls.Add(tlp);
            Controls.Add(pnlBtn);

            // phím tắt
            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }
    }
}
