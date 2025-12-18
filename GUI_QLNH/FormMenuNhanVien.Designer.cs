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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMenuNhanVien));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnOrder = new System.Windows.Forms.Button();
            this.btnLapThanhToan = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.sidebarTopSpacer = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.sidebarBottomSpacer = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblGreet = new System.Windows.Forms.Label();
            this.pnlClock = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.pnlContentBorder = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlClock.SuspendLayout();
            this.pnlContentBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.White;
            this.pnlSidebar.Controls.Add(this.btnOrder);
            this.pnlSidebar.Controls.Add(this.btnLapThanhToan);
            this.pnlSidebar.Controls.Add(this.btnInfo);
            this.pnlSidebar.Controls.Add(this.sidebarTopSpacer);
            this.pnlSidebar.Controls.Add(this.picLogo);
            this.pnlSidebar.Controls.Add(this.sidebarBottomSpacer);
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Padding = new System.Windows.Forms.Padding(14, 16, 12, 16);
            this.pnlSidebar.Size = new System.Drawing.Size(231, 700);
            this.pnlSidebar.TabIndex = 2;
            // 
            // btnOrder
            // 
            this.btnOrder.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOrder.FlatAppearance.BorderSize = 0;
            this.btnOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrder.Location = new System.Drawing.Point(14, 320);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Padding = new System.Windows.Forms.Padding(14, 0, 12, 0);
            this.btnOrder.Size = new System.Drawing.Size(205, 44);
            this.btnOrder.TabIndex = 0;
            this.btnOrder.Text = "Order cho khách";
            this.btnOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnLapThanhToan
            // 
            this.btnLapThanhToan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLapThanhToan.FlatAppearance.BorderSize = 0;
            this.btnLapThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLapThanhToan.Location = new System.Drawing.Point(14, 276);
            this.btnLapThanhToan.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.btnLapThanhToan.Name = "btnLapThanhToan";
            this.btnLapThanhToan.Padding = new System.Windows.Forms.Padding(14, 0, 12, 0);
            this.btnLapThanhToan.Size = new System.Drawing.Size(205, 44);
            this.btnLapThanhToan.TabIndex = 6;
            this.btnLapThanhToan.Text = "Chờ thanh toán";
            this.btnLapThanhToan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLapThanhToan.Click += new System.EventHandler(this.btnLapThanhToan_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInfo.FlatAppearance.BorderSize = 0;
            this.btnInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInfo.Location = new System.Drawing.Point(14, 232);
            this.btnInfo.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Padding = new System.Windows.Forms.Padding(14, 0, 12, 0);
            this.btnInfo.Size = new System.Drawing.Size(205, 44);
            this.btnInfo.TabIndex = 1;
            this.btnInfo.Text = "Thông tin nhân viên";
            this.btnInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // sidebarTopSpacer
            // 
            this.sidebarTopSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.sidebarTopSpacer.Location = new System.Drawing.Point(14, 199);
            this.sidebarTopSpacer.Name = "sidebarTopSpacer";
            this.sidebarTopSpacer.Size = new System.Drawing.Size(205, 33);
            this.sidebarTopSpacer.TabIndex = 2;
            // 
            // picLogo
            // 
            this.picLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picLogo.BackgroundImage")));
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(14, 16);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(205, 183);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 3;
            this.picLogo.TabStop = false;
            // 
            // sidebarBottomSpacer
            // 
            this.sidebarBottomSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sidebarBottomSpacer.Location = new System.Drawing.Point(14, 628);
            this.sidebarBottomSpacer.Name = "sidebarBottomSpacer";
            this.sidebarBottomSpacer.Size = new System.Drawing.Size(205, 12);
            this.sidebarBottomSpacer.TabIndex = 4;
            // 
            // btnLogout
            // 
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Location = new System.Drawing.Point(14, 640);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(14, 0, 12, 0);
            this.btnLogout.Size = new System.Drawing.Size(205, 44);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblGreet);
            this.pnlHeader.Controls.Add(this.pnlClock);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(231, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1220, 76);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblGreet
            // 
            this.lblGreet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGreet.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblGreet.Location = new System.Drawing.Point(0, 0);
            this.lblGreet.Name = "lblGreet";
            this.lblGreet.Size = new System.Drawing.Size(950, 76);
            this.lblGreet.TabIndex = 2;
            this.lblGreet.Text = "Xin chào";
            this.lblGreet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlClock
            // 
            this.pnlClock.Controls.Add(this.lblDate);
            this.pnlClock.Controls.Add(this.lblTime);
            this.pnlClock.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlClock.Location = new System.Drawing.Point(950, 0);
            this.pnlClock.Name = "pnlClock";
            this.pnlClock.Padding = new System.Windows.Forms.Padding(0, 10, 20, 0);
            this.pnlClock.Size = new System.Drawing.Size(270, 76);
            this.pnlClock.TabIndex = 3;
            // 
            // lblDate
            // 
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDate.Location = new System.Drawing.Point(0, 34);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(250, 24);
            this.lblDate.TabIndex = 0;
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTime
            // 
            this.lblTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTime.Location = new System.Drawing.Point(0, 10);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(250, 24);
            this.lblTime.TabIndex = 1;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pnlContentBorder
            // 
            this.pnlContentBorder.Controls.Add(this.pnlContent);
            this.pnlContentBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentBorder.Location = new System.Drawing.Point(231, 76);
            this.pnlContentBorder.Name = "pnlContentBorder";
            this.pnlContentBorder.Padding = new System.Windows.Forms.Padding(14);
            this.pnlContentBorder.Size = new System.Drawing.Size(1220, 624);
            this.pnlContentBorder.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(14, 14);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1192, 596);
            this.pnlContent.TabIndex = 0;
            // 
            // FormMenuNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 700);
            this.Controls.Add(this.pnlContentBorder);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.MinimumSize = new System.Drawing.Size(1120, 640);
            this.Name = "FormMenuNhanVien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainNhanVien";
            this.pnlSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlClock.ResumeLayout(false);
            this.pnlContentBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
    }
}
