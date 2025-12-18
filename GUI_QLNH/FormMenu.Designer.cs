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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMenu));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();
            this.picThongKe = new System.Windows.Forms.PictureBox();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.picDatTiec = new System.Windows.Forms.PictureBox();
            this.btnDatTiec = new System.Windows.Forms.Button();
            this.picThucDon = new System.Windows.Forms.PictureBox();
            this.btnThucDon = new System.Windows.Forms.Button();
            this.picNhanVien = new System.Windows.Forms.PictureBox();
            this.btnNhanVien = new System.Windows.Forms.Button();
            this.picThucKhach = new System.Windows.Forms.PictureBox();
            this.btnThucKhach = new System.Windows.Forms.Button();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblGreet = new System.Windows.Forms.Label();
            this.pnlContentBorder = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThongKe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDatTiec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThucDon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNhanVien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThucKhach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlContentBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Controls.Add(this.picThongKe);
            this.pnlSidebar.Controls.Add(this.btnThongKe);
            this.pnlSidebar.Controls.Add(this.picDatTiec);
            this.pnlSidebar.Controls.Add(this.btnDatTiec);
            this.pnlSidebar.Controls.Add(this.picThucDon);
            this.pnlSidebar.Controls.Add(this.btnThucDon);
            this.pnlSidebar.Controls.Add(this.picNhanVien);
            this.pnlSidebar.Controls.Add(this.btnNhanVien);
            this.pnlSidebar.Controls.Add(this.picThucKhach);
            this.pnlSidebar.Controls.Add(this.btnThucKhach);
            this.pnlSidebar.Controls.Add(this.picLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Padding = new System.Windows.Forms.Padding(21, 21, 18, 21);
            this.pnlSidebar.Size = new System.Drawing.Size(315, 1050);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(174)))), ((int)(((byte)(247)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.Black;
            this.btnLogout.Location = new System.Drawing.Point(27, 960);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(261, 60);
            this.btnLogout.TabIndex = 11;
            this.btnLogout.Text = "LogOut";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // picThongKe
            // 
            this.picThongKe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.picThongKe.Image = ((System.Drawing.Image)(resources.GetObject("picThongKe.Image")));
            this.picThongKe.Location = new System.Drawing.Point(10, 598);
            this.picThongKe.Margin = new System.Windows.Forms.Padding(4);
            this.picThongKe.Name = "picThongKe";
            this.picThongKe.Size = new System.Drawing.Size(66, 66);
            this.picThongKe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThongKe.TabIndex = 5;
            this.picThongKe.TabStop = false;
            // 
            // btnThongKe
            // 
            this.btnThongKe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(185)))), ((int)(((byte)(212)))));
            this.btnThongKe.FlatAppearance.BorderSize = 0;
            this.btnThongKe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThongKe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThongKe.ForeColor = System.Drawing.Color.White;
            this.btnThongKe.Location = new System.Drawing.Point(86, 598);
            this.btnThongKe.Margin = new System.Windows.Forms.Padding(4);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(207, 66);
            this.btnThongKe.TabIndex = 6;
            this.btnThongKe.Text = "Thống kê";
            this.btnThongKe.UseVisualStyleBackColor = false;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // picDatTiec
            // 
            this.picDatTiec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.picDatTiec.Image = ((System.Drawing.Image)(resources.GetObject("picDatTiec.Image")));
            this.picDatTiec.Location = new System.Drawing.Point(10, 520);
            this.picDatTiec.Margin = new System.Windows.Forms.Padding(4);
            this.picDatTiec.Name = "picDatTiec";
            this.picDatTiec.Size = new System.Drawing.Size(66, 66);
            this.picDatTiec.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDatTiec.TabIndex = 2;
            this.picDatTiec.TabStop = false;
            // 
            // btnDatTiec
            // 
            this.btnDatTiec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(185)))), ((int)(((byte)(212)))));
            this.btnDatTiec.FlatAppearance.BorderSize = 0;
            this.btnDatTiec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDatTiec.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDatTiec.ForeColor = System.Drawing.Color.White;
            this.btnDatTiec.Location = new System.Drawing.Point(86, 520);
            this.btnDatTiec.Margin = new System.Windows.Forms.Padding(4);
            this.btnDatTiec.Name = "btnDatTiec";
            this.btnDatTiec.Size = new System.Drawing.Size(210, 66);
            this.btnDatTiec.TabIndex = 3;
            this.btnDatTiec.Text = "Đặt tiệc";
            this.btnDatTiec.UseVisualStyleBackColor = false;
            this.btnDatTiec.Click += new System.EventHandler(this.btnDatTiec_Click);
            // 
            // picThucDon
            // 
            this.picThucDon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.picThucDon.Image = ((System.Drawing.Image)(resources.GetObject("picThucDon.Image")));
            this.picThucDon.Location = new System.Drawing.Point(10, 442);
            this.picThucDon.Margin = new System.Windows.Forms.Padding(4);
            this.picThucDon.Name = "picThucDon";
            this.picThucDon.Size = new System.Drawing.Size(66, 66);
            this.picThucDon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThucDon.TabIndex = 0;
            this.picThucDon.TabStop = false;
            // 
            // btnThucDon
            // 
            this.btnThucDon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(185)))), ((int)(((byte)(212)))));
            this.btnThucDon.FlatAppearance.BorderSize = 0;
            this.btnThucDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThucDon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThucDon.ForeColor = System.Drawing.Color.White;
            this.btnThucDon.Location = new System.Drawing.Point(86, 442);
            this.btnThucDon.Margin = new System.Windows.Forms.Padding(4);
            this.btnThucDon.Name = "btnThucDon";
            this.btnThucDon.Size = new System.Drawing.Size(207, 66);
            this.btnThucDon.TabIndex = 1;
            this.btnThucDon.Text = "Thực đơn";
            this.btnThucDon.UseVisualStyleBackColor = false;
            this.btnThucDon.Click += new System.EventHandler(this.btnThucDon_Click);
            // 
            // picNhanVien
            // 
            this.picNhanVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.picNhanVien.Image = ((System.Drawing.Image)(resources.GetObject("picNhanVien.Image")));
            this.picNhanVien.Location = new System.Drawing.Point(10, 364);
            this.picNhanVien.Margin = new System.Windows.Forms.Padding(4);
            this.picNhanVien.Name = "picNhanVien";
            this.picNhanVien.Size = new System.Drawing.Size(66, 66);
            this.picNhanVien.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picNhanVien.TabIndex = 0;
            this.picNhanVien.TabStop = false;
            // 
            // btnNhanVien
            // 
            this.btnNhanVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(185)))), ((int)(((byte)(212)))));
            this.btnNhanVien.FlatAppearance.BorderSize = 0;
            this.btnNhanVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNhanVien.ForeColor = System.Drawing.Color.White;
            this.btnNhanVien.Location = new System.Drawing.Point(86, 364);
            this.btnNhanVien.Margin = new System.Windows.Forms.Padding(4);
            this.btnNhanVien.Name = "btnNhanVien";
            this.btnNhanVien.Size = new System.Drawing.Size(207, 66);
            this.btnNhanVien.TabIndex = 0;
            this.btnNhanVien.Text = "Nhân viên";
            this.btnNhanVien.UseVisualStyleBackColor = false;
            this.btnNhanVien.Click += new System.EventHandler(this.btnNhanVien_Click);
            // 
            // picThucKhach
            // 
            this.picThucKhach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.picThucKhach.Image = ((System.Drawing.Image)(resources.GetObject("picThucKhach.Image")));
            this.picThucKhach.Location = new System.Drawing.Point(10, 286);
            this.picThucKhach.Margin = new System.Windows.Forms.Padding(4);
            this.picThucKhach.Name = "picThucKhach";
            this.picThucKhach.Size = new System.Drawing.Size(66, 66);
            this.picThucKhach.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThucKhach.TabIndex = 0;
            this.picThucKhach.TabStop = false;
            // 
            // btnThucKhach
            // 
            this.btnThucKhach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(185)))), ((int)(((byte)(212)))));
            this.btnThucKhach.FlatAppearance.BorderSize = 0;
            this.btnThucKhach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThucKhach.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThucKhach.ForeColor = System.Drawing.Color.White;
            this.btnThucKhach.Location = new System.Drawing.Point(86, 286);
            this.btnThucKhach.Margin = new System.Windows.Forms.Padding(4);
            this.btnThucKhach.Name = "btnThucKhach";
            this.btnThucKhach.Size = new System.Drawing.Size(210, 66);
            this.btnThucKhach.TabIndex = 0;
            this.btnThucKhach.Text = "Thực khách";
            this.btnThucKhach.UseVisualStyleBackColor = false;
            this.btnThucKhach.Click += new System.EventHandler(this.btnThucKhach_Click);
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(47, 27);
            this.picLogo.Margin = new System.Windows.Forms.Padding(4);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(199, 175);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(199)))), ((int)(((byte)(236)))));
            this.pnlHeader.Controls.Add(this.lblDate);
            this.pnlHeader.Controls.Add(this.lblTime);
            this.pnlHeader.Controls.Add(this.lblGreet);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(315, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1609, 126);
            this.pnlHeader.TabIndex = 1;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlHeader_Paint);
            // 
            // lblDate
            // 
            this.lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(1324, 69);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(120, 28);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Friday, ………";
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(1324, 27);
            this.lblTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(157, 32);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "08:02:38 PM";
            // 
            // lblGreet
            // 
            this.lblGreet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblGreet.AutoSize = true;
            this.lblGreet.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblGreet.ForeColor = System.Drawing.Color.White;
            this.lblGreet.Location = new System.Drawing.Point(497, 33);
            this.lblGreet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGreet.Name = "lblGreet";
            this.lblGreet.Size = new System.Drawing.Size(381, 48);
            this.lblGreet.TabIndex = 0;
            this.lblGreet.Text = "Xin Chào Bạn Quản Lý";
            // 
            // pnlContentBorder
            // 
            this.pnlContentBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(238)))), ((int)(((byte)(248)))));
            this.pnlContentBorder.Controls.Add(this.pnlContent);
            this.pnlContentBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentBorder.Location = new System.Drawing.Point(315, 126);
            this.pnlContentBorder.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContentBorder.Name = "pnlContentBorder";
            this.pnlContentBorder.Padding = new System.Windows.Forms.Padding(15);
            this.pnlContentBorder.Size = new System.Drawing.Size(1609, 924);
            this.pnlContentBorder.TabIndex = 2;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(15, 15);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(4);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1579, 894);
            this.pnlContent.TabIndex = 0;
            // 
            // timerClock
            // 
            this.timerClock.Enabled = true;
            this.timerClock.Interval = 500;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1924, 1050);
            this.Controls.Add(this.pnlContentBorder);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1639, 947);
            this.Name = "FormMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainQuanLy";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlSidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picThongKe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDatTiec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThucDon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNhanVien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picThucKhach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContentBorder.ResumeLayout(false);
            this.ResumeLayout(false);

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
