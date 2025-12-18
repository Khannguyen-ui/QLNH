using System.Drawing;
using System.Windows.Forms;

namespace GUI_QLNH
{
    partial class FormThongTinNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Label labelLoginMa;
        private Label lblMaNvCurrent;

        private FlowLayoutPanel pnlHeader;

        private GroupBox grpBasic;
        private TableLayoutPanel tlpBasic;

        private Label lbMa;
        private Label lbHoTen;
        private Label lbNoiSinh;
        private Label lbNgayLam;

        private TextBox txtMaNV;
        private TextBox txtHoTen;
        private TextBox txtNoiSinh;
        private DateTimePicker dtpNgayLam;

        private FlowLayoutPanel pnlButtons;
        private Button btnReload;
        private Button btnChangePwd;
        private Button btnUpdate;
        private Button btnClose;

        private ErrorProvider errProvider;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lblTitle = new Label();
            labelLoginMa = new Label();
            lblMaNvCurrent = new Label();
            pnlHeader = new FlowLayoutPanel();
            grpBasic = new GroupBox();
            tlpBasic = new TableLayoutPanel();
            lbMa = new Label();
            txtMaNV = new TextBox();
            lbHoTen = new Label();
            txtHoTen = new TextBox();
            lbNoiSinh = new Label();
            txtNoiSinh = new TextBox();
            lbNgayLam = new Label();
            dtpNgayLam = new DateTimePicker();
            pnlButtons = new FlowLayoutPanel();
            btnClose = new Button();
            btnUpdate = new Button();
            btnChangePwd = new Button();
            btnReload = new Button();
            errProvider = new ErrorProvider(components);
            pnlHeader.SuspendLayout();
            grpBasic.SuspendLayout();
            tlpBasic.SuspendLayout();
            pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errProvider).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(0, 6);
            lblTitle.Margin = new Padding(0, 0, 16, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(256, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Thông Tin Nhân Viên";
            // 
            // labelLoginMa
            // 
            labelLoginMa.AutoSize = true;
            labelLoginMa.Location = new Point(288, 14);
            labelLoginMa.Margin = new Padding(16, 8, 0, 0);
            labelLoginMa.Name = "labelLoginMa";
            labelLoginMa.Size = new Size(219, 23);
            labelLoginMa.TabIndex = 1;
            labelLoginMa.Text = "   Mã NV đang đăng nhập: ";
            // 
            // lblMaNvCurrent
            // 
            lblMaNvCurrent.AutoSize = true;
            lblMaNvCurrent.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMaNvCurrent.Location = new Point(507, 14);
            lblMaNvCurrent.Margin = new Padding(0, 8, 0, 0);
            lblMaNvCurrent.Name = "lblMaNvCurrent";
            lblMaNvCurrent.Size = new Size(55, 23);
            lblMaNvCurrent.TabIndex = 2;
            lblMaNvCurrent.Text = "NV???";
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(labelLoginMa);
            pnlHeader.Controls.Add(lblMaNvCurrent);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(16, 16);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(0, 6, 0, 0);
            pnlHeader.Size = new Size(1211, 73);
            pnlHeader.TabIndex = 2;
            pnlHeader.WrapContents = false;
            // 
            // grpBasic
            // 
            grpBasic.Controls.Add(tlpBasic);
            grpBasic.Dock = DockStyle.Top;
            grpBasic.Location = new Point(16, 89);
            grpBasic.Name = "grpBasic";
            grpBasic.Padding = new Padding(12);
            grpBasic.Size = new Size(1211, 234);
            grpBasic.TabIndex = 1;
            grpBasic.TabStop = false;
            grpBasic.Text = "Thông tin cơ bản";
            // 
            // tlpBasic
            // 
            tlpBasic.ColumnCount = 2;
            tlpBasic.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tlpBasic.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpBasic.Controls.Add(lbMa, 0, 0);
            tlpBasic.Controls.Add(txtMaNV, 1, 0);
            tlpBasic.Controls.Add(lbHoTen, 0, 1);
            tlpBasic.Controls.Add(txtHoTen, 1, 1);
            tlpBasic.Controls.Add(lbNoiSinh, 0, 2);
            tlpBasic.Controls.Add(txtNoiSinh, 1, 2);
            tlpBasic.Controls.Add(lbNgayLam, 0, 3);
            tlpBasic.Controls.Add(dtpNgayLam, 1, 3);
            tlpBasic.Dock = DockStyle.Fill;
            tlpBasic.Location = new Point(12, 35);
            tlpBasic.Name = "tlpBasic";
            tlpBasic.RowCount = 4;
            tlpBasic.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpBasic.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpBasic.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpBasic.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpBasic.Size = new Size(1187, 187);
            tlpBasic.TabIndex = 0;
            // 
            // lbMa
            // 
            lbMa.Anchor = AnchorStyles.Left;
            lbMa.AutoSize = true;
            lbMa.Location = new Point(3, 9);
            lbMa.Name = "lbMa";
            lbMa.Size = new Size(118, 23);
            lbMa.TabIndex = 0;
            lbMa.Text = "Mã nhân viên:";
            // 
            // txtMaNV
            // 
            txtMaNV.Dock = DockStyle.Fill;
            txtMaNV.Location = new Point(143, 3);
            txtMaNV.Name = "txtMaNV";
            txtMaNV.ReadOnly = true;
            txtMaNV.Size = new Size(1041, 30);
            txtMaNV.TabIndex = 1;
            txtMaNV.TabStop = false;
            // 
            // lbHoTen
            // 
            lbHoTen.Anchor = AnchorStyles.Left;
            lbHoTen.AutoSize = true;
            lbHoTen.Location = new Point(3, 51);
            lbHoTen.Name = "lbHoTen";
            lbHoTen.Size = new Size(66, 23);
            lbHoTen.TabIndex = 2;
            lbHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            txtHoTen.Dock = DockStyle.Fill;
            txtHoTen.Location = new Point(143, 45);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(1041, 30);
            txtHoTen.TabIndex = 3;
            // 
            // lbNoiSinh
            // 
            lbNoiSinh.Anchor = AnchorStyles.Left;
            lbNoiSinh.AutoSize = true;
            lbNoiSinh.Location = new Point(3, 93);
            lbNoiSinh.Name = "lbNoiSinh";
            lbNoiSinh.Size = new Size(77, 23);
            lbNoiSinh.TabIndex = 4;
            lbNoiSinh.Text = "Nơi sinh:";
            // 
            // txtNoiSinh
            // 
            txtNoiSinh.Dock = DockStyle.Fill;
            txtNoiSinh.Location = new Point(143, 87);
            txtNoiSinh.Name = "txtNoiSinh";
            txtNoiSinh.Size = new Size(1041, 30);
            txtNoiSinh.TabIndex = 5;
            // 
            // lbNgayLam
            // 
            lbNgayLam.Anchor = AnchorStyles.Left;
            lbNgayLam.AutoSize = true;
            lbNgayLam.Location = new Point(3, 145);
            lbNgayLam.Name = "lbNgayLam";
            lbNgayLam.Size = new Size(121, 23);
            lbNgayLam.TabIndex = 6;
            lbNgayLam.Text = "Ngày làm việc:";
            // 
            // dtpNgayLam
            // 
            dtpNgayLam.Dock = DockStyle.Left;
            dtpNgayLam.Format = DateTimePickerFormat.Short;
            dtpNgayLam.Location = new Point(143, 129);
            dtpNgayLam.Name = "dtpNgayLam";
            dtpNgayLam.ShowCheckBox = true;
            dtpNgayLam.Size = new Size(205, 30);
            dtpNgayLam.TabIndex = 7;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Controls.Add(btnUpdate);
            pnlButtons.Controls.Add(btnChangePwd);
            pnlButtons.Controls.Add(btnReload);
            pnlButtons.Dock = DockStyle.Top;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(16, 323);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(8);
            pnlButtons.Size = new Size(1211, 94);
            pnlButtons.TabIndex = 0;
            pnlButtons.WrapContents = false;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(1082, 11);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 53);
            btnClose.TabIndex = 0;
            btnClose.Text = "Hủy";
            btnClose.Click += btnClose_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(873, 11);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(203, 53);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "Cập nhật thông tin";
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnChangePwd
            // 
            btnChangePwd.Location = new Point(665, 11);
            btnChangePwd.Name = "btnChangePwd";
            btnChangePwd.Size = new Size(202, 53);
            btnChangePwd.TabIndex = 2;
            btnChangePwd.Text = "Đổi mật khẩu";
            btnChangePwd.Click += btnChangePwd_Click;
            // 
            // btnReload
            // 
            btnReload.Location = new Point(549, 11);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(110, 53);
            btnReload.TabIndex = 3;
            btnReload.Text = "Tải lại";
            btnReload.Click += btnReload_Click;
            // 
            // errProvider
            // 
            errProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errProvider.ContainerControl = this;
            // 
            // FormThongTinNhanVien
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1243, 609);
            Controls.Add(pnlButtons);
            Controls.Add(grpBasic);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormThongTinNhanVien";
            Padding = new Padding(16);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Thông Tin Nhân Viên";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            grpBasic.ResumeLayout(false);
            tlpBasic.ResumeLayout(false);
            tlpBasic.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errProvider).EndInit();
            ResumeLayout(false);

        }
    }
}
