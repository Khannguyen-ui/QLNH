using System;
using System.Windows.Forms;
using System.Drawing;

namespace GUI_QLNH
{
    partial class FormMenuNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlSidebar;
        private Panel pnlHeader;
        private Panel pnlContentBorder;
        private Panel pnlContent;

        private Label lblGreet;
        private Label lblTime;
        private Label lblDate;
        private Timer timerClock;

        private PictureBox picLogo;
        private Button btnInfo;
        private Button btnOrder;
        private Button btnLapThanhToan;
        private Button btnLogout;
        private Panel sidebarTopSpacer;
        private Panel sidebarBottomSpacer;

        // NEW: khung bên phải của header để chứa clock/date
        private Panel pnlClock;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMenuNhanVien));
            pnlSidebar = new Panel();
            btnOrder = new Button();
            btnLapThanhToan = new Button();
            btnInfo = new Button();
            sidebarTopSpacer = new Panel();
            picLogo = new PictureBox();
            sidebarBottomSpacer = new Panel();
            btnLogout = new Button();
            pnlHeader = new Panel();
            lblGreet = new Label();
            pnlClock = new Panel();
            lblDate = new Label();
            lblTime = new Label();
            pnlContentBorder = new Panel();
            pnlContent = new Panel();
            pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlHeader.SuspendLayout();
            pnlClock.SuspendLayout();
            pnlContentBorder.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.White;
            pnlSidebar.Controls.Add(btnOrder);
            pnlSidebar.Controls.Add(btnLapThanhToan);
            pnlSidebar.Controls.Add(btnInfo);
            pnlSidebar.Controls.Add(sidebarTopSpacer);
            pnlSidebar.Controls.Add(picLogo);
            pnlSidebar.Controls.Add(sidebarBottomSpacer);
            pnlSidebar.Controls.Add(btnLogout);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(14, 16, 12, 16);
            pnlSidebar.Size = new Size(231, 700);
            pnlSidebar.TabIndex = 2;
            // 
            // btnOrder
            // 
            btnOrder.Dock = DockStyle.Top;
            btnOrder.FlatAppearance.BorderSize = 0;
            btnOrder.FlatStyle = FlatStyle.Flat;
            btnOrder.Location = new Point(14, 320);
            btnOrder.Margin = new Padding(0, 6, 0, 0);
            btnOrder.Name = "btnOrder";
            btnOrder.Padding = new Padding(14, 0, 12, 0);
            btnOrder.Size = new Size(205, 44);
            btnOrder.TabIndex = 0;
            btnOrder.Text = "Order cho khách";
            btnOrder.TextAlign = ContentAlignment.MiddleLeft;
            btnOrder.Click += btnOrder_Click;
            // 
            // btnLapThanhToan
            // 
            btnLapThanhToan.Dock = DockStyle.Top;
            btnLapThanhToan.FlatAppearance.BorderSize = 0;
            btnLapThanhToan.FlatStyle = FlatStyle.Flat;
            btnLapThanhToan.Location = new Point(14, 276);
            btnLapThanhToan.Margin = new Padding(0, 6, 0, 0);
            btnLapThanhToan.Name = "btnLapThanhToan";
            btnLapThanhToan.Padding = new Padding(14, 0, 12, 0);
            btnLapThanhToan.Size = new Size(205, 44);
            btnLapThanhToan.TabIndex = 6;
            btnLapThanhToan.Text = "Chờ thanh toán";
            btnLapThanhToan.TextAlign = ContentAlignment.MiddleLeft;
            btnLapThanhToan.Click += btnLapThanhToan_Click;
            // 
            // btnInfo
            // 
            btnInfo.Dock = DockStyle.Top;
            btnInfo.FlatAppearance.BorderSize = 0;
            btnInfo.FlatStyle = FlatStyle.Flat;
            btnInfo.Location = new Point(14, 232);
            btnInfo.Margin = new Padding(0, 6, 0, 0);
            btnInfo.Name = "btnInfo";
            btnInfo.Padding = new Padding(14, 0, 12, 0);
            btnInfo.Size = new Size(205, 44);
            btnInfo.TabIndex = 1;
            btnInfo.Text = "Thông tin nhân viên";
            btnInfo.TextAlign = ContentAlignment.MiddleLeft;
            btnInfo.Click += btnInfo_Click;
            // 
            // sidebarTopSpacer
            // 
            sidebarTopSpacer.Dock = DockStyle.Top;
            sidebarTopSpacer.Location = new Point(14, 199);
            sidebarTopSpacer.Name = "sidebarTopSpacer";
            sidebarTopSpacer.Size = new Size(205, 33);
            sidebarTopSpacer.TabIndex = 2;
            // 
            // picLogo
            // 
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            picLogo.Dock = DockStyle.Top;
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(14, 16);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(205, 183);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 3;
            picLogo.TabStop = false;
            // 
            // sidebarBottomSpacer
            // 
            sidebarBottomSpacer.Dock = DockStyle.Bottom;
            sidebarBottomSpacer.Location = new Point(14, 628);
            sidebarBottomSpacer.Name = "sidebarBottomSpacer";
            sidebarBottomSpacer.Size = new Size(205, 12);
            sidebarBottomSpacer.TabIndex = 4;
            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Location = new Point(14, 640);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(14, 0, 12, 0);
            btnLogout.Size = new Size(205, 44);
            btnLogout.TabIndex = 5;
            btnLogout.Text = "Đăng xuất";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.Click += btnLogout_Click;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblGreet);
            pnlHeader.Controls.Add(pnlClock);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(231, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1220, 76);
            pnlHeader.TabIndex = 1;
            // 
            // lblGreet
            // 
            lblGreet.Dock = DockStyle.Fill;
            lblGreet.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblGreet.Location = new Point(0, 0);
            lblGreet.Name = "lblGreet";
            lblGreet.Size = new Size(950, 76);
            lblGreet.TabIndex = 2;
            lblGreet.Text = "Xin chào";
            lblGreet.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlClock
            // 
            pnlClock.Controls.Add(lblDate);
            pnlClock.Controls.Add(lblTime);
            pnlClock.Dock = DockStyle.Right;
            pnlClock.Location = new Point(950, 0);
            pnlClock.Name = "pnlClock";
            pnlClock.Padding = new Padding(0, 10, 20, 0);
            pnlClock.Size = new Size(270, 76);
            pnlClock.TabIndex = 3;
            // 
            // lblDate
            // 
            lblDate.Dock = DockStyle.Top;
            lblDate.Location = new Point(0, 34);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(250, 24);
            lblDate.TabIndex = 0;
            lblDate.TextAlign = ContentAlignment.TopRight;
            // 
            // lblTime
            // 
            lblTime.Dock = DockStyle.Top;
            lblTime.Location = new Point(0, 10);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(250, 24);
            lblTime.TabIndex = 1;
            lblTime.TextAlign = ContentAlignment.TopRight;
            // 
            // pnlContentBorder
            // 
            pnlContentBorder.Controls.Add(pnlContent);
            pnlContentBorder.Dock = DockStyle.Fill;
            pnlContentBorder.Location = new Point(231, 76);
            pnlContentBorder.Name = "pnlContentBorder";
            pnlContentBorder.Padding = new Padding(14);
            pnlContentBorder.Size = new Size(1220, 624);
            pnlContentBorder.TabIndex = 0;
            // 
            // pnlContent
            // 
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(14, 14);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1192, 596);
            pnlContent.TabIndex = 0;
            // 
            // FormMenuNhanVien
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1451, 700);
            Controls.Add(pnlContentBorder);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
            Font = new Font("Segoe UI", 9.75F);
            MinimumSize = new Size(1120, 640);
            Name = "FormMenuNhanVien";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainNhanVien";
            pnlSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlClock.ResumeLayout(false);
            pnlContentBorder.ResumeLayout(false);
            ResumeLayout(false);

        }
        #endregion
    }
}
