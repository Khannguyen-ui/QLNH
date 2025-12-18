using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_QLNH
{
    partial class FormMenu
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FormMenu));
            pnlSidebar = new Panel();
            btnLogout = new Button();
            picThongKe = new PictureBox();
            btnThongKe = new Button();
            picDatTiec = new PictureBox();
            btnDatTiec = new Button();
            picThucDon = new PictureBox();
            btnThucDon = new Button();
            picNhanVien = new PictureBox();
            btnNhanVien = new Button();
            picThucKhach = new PictureBox();
            btnThucKhach = new Button();
            picLogo = new PictureBox();
            pnlHeader = new Panel();
            lblDate = new Label();
            lblTime = new Label();
            lblGreet = new Label();
            pnlContentBorder = new Panel();
            pnlContent = new Panel();
            timerClock = new System.Windows.Forms.Timer(components);
            pnlSidebar.SuspendLayout();
            ((ISupportInitialize)picThongKe).BeginInit();
            ((ISupportInitialize)picDatTiec).BeginInit();
            ((ISupportInitialize)picThucDon).BeginInit();
            ((ISupportInitialize)picNhanVien).BeginInit();
            ((ISupportInitialize)picThucKhach).BeginInit();
            ((ISupportInitialize)picLogo).BeginInit();
            pnlHeader.SuspendLayout();
            pnlContentBorder.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(48, 199, 236);
            pnlSidebar.Controls.Add(btnLogout);
            pnlSidebar.Controls.Add(picThongKe);
            pnlSidebar.Controls.Add(btnThongKe);
            pnlSidebar.Controls.Add(picDatTiec);
            pnlSidebar.Controls.Add(btnDatTiec);
            pnlSidebar.Controls.Add(picThucDon);
            pnlSidebar.Controls.Add(btnThucDon);
            pnlSidebar.Controls.Add(picNhanVien);
            pnlSidebar.Controls.Add(btnNhanVien);
            pnlSidebar.Controls.Add(picThucKhach);
            pnlSidebar.Controls.Add(btnThucKhach);
            pnlSidebar.Controls.Add(picLogo);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(18, 18, 15, 18);
            pnlSidebar.Size = new Size(262, 875);
            pnlSidebar.TabIndex = 0;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLogout.BackColor = Color.FromArgb(70, 174, 247);
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.ForeColor = Color.Black;
            btnLogout.Location = new Point(22, 800);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(218, 50);
            btnLogout.TabIndex = 11;
            btnLogout.Text = "LogOut";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // picThongKe
            // 
            picThongKe.BackColor = Color.FromArgb(48, 199, 236);
            picThongKe.Image = (Image)resources.GetObject("picThongKe.Image");
            picThongKe.Location = new Point(8, 498);
            picThongKe.Name = "picThongKe";
            picThongKe.Size = new Size(55, 55);
            picThongKe.SizeMode = PictureBoxSizeMode.Zoom;
            picThongKe.TabIndex = 5;
            picThongKe.TabStop = false;
            // 
            // btnThongKe
            // 
            btnThongKe.BackColor = Color.FromArgb(22, 185, 212);
            btnThongKe.FlatAppearance.BorderSize = 0;
            btnThongKe.FlatStyle = FlatStyle.Flat;
            btnThongKe.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThongKe.ForeColor = Color.White;
            btnThongKe.Location = new Point(72, 498);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Size = new Size(172, 55);
            btnThongKe.TabIndex = 6;
            btnThongKe.Text = "Thống kê";
            btnThongKe.UseVisualStyleBackColor = false;
            btnThongKe.Click += btnThongKe_Click;
            // 
            // picDatTiec
            // 
            picDatTiec.BackColor = Color.FromArgb(48, 199, 236);
            picDatTiec.Image = (Image)resources.GetObject("picDatTiec.Image");
            picDatTiec.Location = new Point(8, 433);
            picDatTiec.Name = "picDatTiec";
            picDatTiec.Size = new Size(55, 55);
            picDatTiec.SizeMode = PictureBoxSizeMode.Zoom;
            picDatTiec.TabIndex = 2;
            picDatTiec.TabStop = false;
            picDatTiec.Click += picDatTiec_Click;
            // 
            // btnDatTiec
            // 
            btnDatTiec.BackColor = Color.FromArgb(22, 185, 212);
            btnDatTiec.FlatAppearance.BorderSize = 0;
            btnDatTiec.FlatStyle = FlatStyle.Flat;
            btnDatTiec.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnDatTiec.ForeColor = Color.White;
            btnDatTiec.Location = new Point(72, 433);
            btnDatTiec.Name = "btnDatTiec";
            btnDatTiec.Size = new Size(175, 55);
            btnDatTiec.TabIndex = 3;
            btnDatTiec.Text = "Đặt tiệc";
            btnDatTiec.UseVisualStyleBackColor = false;
            btnDatTiec.Click += btnDatTiec_Click;
            // 
            // picThucDon
            // 
            picThucDon.BackColor = Color.FromArgb(48, 199, 236);
            picThucDon.Image = (Image)resources.GetObject("picThucDon.Image");
            picThucDon.Location = new Point(8, 368);
            picThucDon.Name = "picThucDon";
            picThucDon.Size = new Size(55, 55);
            picThucDon.SizeMode = PictureBoxSizeMode.Zoom;
            picThucDon.TabIndex = 0;
            picThucDon.TabStop = false;
            // 
            // btnThucDon
            // 
            btnThucDon.BackColor = Color.FromArgb(22, 185, 212);
            btnThucDon.FlatAppearance.BorderSize = 0;
            btnThucDon.FlatStyle = FlatStyle.Flat;
            btnThucDon.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThucDon.ForeColor = Color.White;
            btnThucDon.Location = new Point(72, 368);
            btnThucDon.Name = "btnThucDon";
            btnThucDon.Size = new Size(172, 55);
            btnThucDon.TabIndex = 1;
            btnThucDon.Text = "Thực đơn";
            btnThucDon.UseVisualStyleBackColor = false;
            btnThucDon.Click += btnThucDon_Click;
            // 
            // picNhanVien
            // 
            picNhanVien.BackColor = Color.FromArgb(48, 199, 236);
            picNhanVien.Image = (Image)resources.GetObject("picNhanVien.Image");
            picNhanVien.Location = new Point(8, 303);
            picNhanVien.Name = "picNhanVien";
            picNhanVien.Size = new Size(55, 55);
            picNhanVien.SizeMode = PictureBoxSizeMode.Zoom;
            picNhanVien.TabIndex = 0;
            picNhanVien.TabStop = false;
            picNhanVien.Click += picNhanVien_Click;
            // 
            // btnNhanVien
            // 
            btnNhanVien.BackColor = Color.FromArgb(22, 185, 212);
            btnNhanVien.FlatAppearance.BorderSize = 0;
            btnNhanVien.FlatStyle = FlatStyle.Flat;
            btnNhanVien.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNhanVien.ForeColor = Color.White;
            btnNhanVien.Location = new Point(72, 303);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Size = new Size(172, 55);
            btnNhanVien.TabIndex = 0;
            btnNhanVien.Text = "Nhân viên";
            btnNhanVien.UseVisualStyleBackColor = false;
            btnNhanVien.Click += btnNhanVien_Click;
            // 
            // picThucKhach
            // 
            picThucKhach.BackColor = Color.FromArgb(48, 199, 236);
            picThucKhach.Image = (Image)resources.GetObject("picThucKhach.Image");
            picThucKhach.Location = new Point(8, 238);
            picThucKhach.Name = "picThucKhach";
            picThucKhach.Size = new Size(55, 55);
            picThucKhach.SizeMode = PictureBoxSizeMode.Zoom;
            picThucKhach.TabIndex = 0;
            picThucKhach.TabStop = false;
            // 
            // btnThucKhach
            // 
            btnThucKhach.BackColor = Color.FromArgb(22, 185, 212);
            btnThucKhach.FlatAppearance.BorderSize = 0;
            btnThucKhach.FlatStyle = FlatStyle.Flat;
            btnThucKhach.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnThucKhach.ForeColor = Color.White;
            btnThucKhach.Location = new Point(72, 238);
            btnThucKhach.Name = "btnThucKhach";
            btnThucKhach.Size = new Size(175, 55);
            btnThucKhach.TabIndex = 0;
            btnThucKhach.Text = "Thực khách";
            btnThucKhach.UseVisualStyleBackColor = false;
            btnThucKhach.Click += btnThucKhach_Click;
            // 
            // picLogo
            // 
            picLogo.BackColor = Color.FromArgb(48, 199, 236);
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(39, 22);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(166, 146);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(48, 199, 236);
            pnlHeader.Controls.Add(lblDate);
            pnlHeader.Controls.Add(lblTime);
            pnlHeader.Controls.Add(lblGreet);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(262, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1341, 105);
            pnlHeader.TabIndex = 1;
            pnlHeader.Paint += PnlHeader_Paint;
            // 
            // lblDate
            // 
            lblDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI", 10F);
            lblDate.ForeColor = Color.White;
            lblDate.Location = new Point(1103, 58);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(100, 23);
            lblDate.TabIndex = 2;
            lblDate.Text = "Friday, ………";
            // 
            // lblTime
            // 
            lblTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTime.ForeColor = Color.White;
            lblTime.Location = new Point(1103, 22);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(131, 28);
            lblTime.TabIndex = 1;
            lblTime.Text = "08:02:38 PM";
            // 
            // lblGreet
            // 
            lblGreet.Anchor = AnchorStyles.Top;
            lblGreet.AutoSize = true;
            lblGreet.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblGreet.ForeColor = Color.White;
            lblGreet.Location = new Point(414, 28);
            lblGreet.Name = "lblGreet";
            lblGreet.Size = new Size(321, 41);
            lblGreet.TabIndex = 0;
            lblGreet.Text = "Xin Chào Bạn Quản Lý";
            // 
            // pnlContentBorder
            // 
            pnlContentBorder.BackColor = Color.FromArgb(216, 238, 248);
            pnlContentBorder.Controls.Add(pnlContent);
            pnlContentBorder.Dock = DockStyle.Fill;
            pnlContentBorder.Location = new Point(262, 105);
            pnlContentBorder.Name = "pnlContentBorder";
            pnlContentBorder.Padding = new Padding(12, 12, 12, 12);
            pnlContentBorder.Size = new Size(1341, 770);
            pnlContentBorder.TabIndex = 2;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.White;
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(12, 12);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(1317, 746);
            pnlContent.TabIndex = 0;
            // 
            // timerClock
            // 
            timerClock.Enabled = true;
            timerClock.Interval = 500;
            timerClock.Tick += timerClock_Tick;
            // 
            // FormMenu
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1603, 875);
            Controls.Add(pnlContentBorder);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1369, 797);
            Name = "FormMenu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainQuanLy";
            WindowState = FormWindowState.Maximized;
            pnlSidebar.ResumeLayout(false);
            ((ISupportInitialize)picThongKe).EndInit();
            ((ISupportInitialize)picDatTiec).EndInit();
            ((ISupportInitialize)picThucDon).EndInit();
            ((ISupportInitialize)picNhanVien).EndInit();
            ((ISupportInitialize)picThucKhach).EndInit();
            ((ISupportInitialize)picLogo).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlContentBorder.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.PictureBox picThucKhach;
        private System.Windows.Forms.Button btnThucKhach;
        private System.Windows.Forms.PictureBox picNhanVien;
        private System.Windows.Forms.Button btnNhanVien;
        private System.Windows.Forms.PictureBox picThucDon;
        private System.Windows.Forms.Button btnThucDon;
        private System.Windows.Forms.PictureBox picDatTiec;
        private System.Windows.Forms.Button btnDatTiec;
        private System.Windows.Forms.PictureBox picThongKe;
        private System.Windows.Forms.Button btnThongKe;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblGreet;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Panel pnlContentBorder;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Timer timerClock;
    }
}
