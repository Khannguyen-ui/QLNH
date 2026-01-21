using System;
using System.Windows.Forms;
using System.Drawing;

namespace GUI_QLNH
{
    partial class FormDatTiec
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            splitMain = new SplitContainer();
            tv = new TreeView();
            pnlRight = new Panel();
            dgv = new DataGridView();
            pnlButtons = new Panel();
            btnThoat = new Button();
            btnMoi = new Button();
            btnXoa = new Button();
            btnBoQua = new Button();
            btnLuu = new Button();
            btnSua = new Button();
            btnThem = new Button();
            btnChiTiet = new Button();
            pnlForm = new Panel();
            btnTim = new Button();
            txtSearch = new TextBox();
            cboCa = new ComboBox();
            cboPhong = new ComboBox();
            nudSoLuong = new NumericUpDown();
            cboMaNV = new ComboBox();
            cboMaTK = new ComboBox();
            dtpNgayDK = new DateTimePicker();
            txtSoPhieu = new TextBox();
            lblSearch = new Label();
            lblCa = new Label();
            lblPhong = new Label();
            lblSL = new Label();
            lblNoiSinh = new Label();
            lblTen = new Label();
            lblNgay = new Label();
            lblMa = new Label();
            lblTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            pnlButtons.SuspendLayout();
            pnlForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(tv);
            splitMain.Panel1MinSize = 160;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pnlRight);
            splitMain.Size = new Size(1286, 719);
            splitMain.SplitterDistance = 278;
            splitMain.TabIndex = 0;
            // 
            // tv
            // 
            tv.Dock = DockStyle.Fill;
            tv.FullRowSelect = true;
            tv.HideSelection = false;
            tv.Location = new Point(0, 0);
            tv.Name = "tv";
            tv.Size = new Size(278, 719);
            tv.TabIndex = 0;
            tv.BeforeExpand += tv_BeforeExpand;
            tv.AfterSelect += tv_AfterSelect;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(dgv);
            pnlRight.Controls.Add(pnlButtons);
            pnlRight.Controls.Add(pnlForm);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(0, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(1004, 719);
            pnlRight.TabIndex = 0;
            // 
            // dgv
            // 
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ColumnHeadersHeight = 34;
            dgv.Dock = DockStyle.Fill;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.Location = new Point(0, 220);
            dgv.Name = "dgv";
            dgv.RowHeadersVisible = false;
            dgv.RowHeadersWidth = 62;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Size = new Size(1004, 435);
            dgv.TabIndex = 1;
            dgv.CellClick += dgv_CellClick;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnThoat);
            pnlButtons.Controls.Add(btnMoi);
            pnlButtons.Controls.Add(btnXoa);
            pnlButtons.Controls.Add(btnBoQua);
            pnlButtons.Controls.Add(btnLuu);
            pnlButtons.Controls.Add(btnSua);
            pnlButtons.Controls.Add(btnThem);
            pnlButtons.Controls.Add(btnChiTiet);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(0, 655);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(1004, 64);
            pnlButtons.TabIndex = 2;
            // 
            // btnThoat
            // 
            btnThoat.Dock = DockStyle.Left;
            btnThoat.Location = new Point(650, 0);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(90, 64);
            btnThoat.TabIndex = 0;
            btnThoat.Text = "Thoát";
            btnThoat.Click += btnThoat_Click;
            // 
            // btnMoi
            // 
            btnMoi.Dock = DockStyle.Left;
            btnMoi.Location = new Point(560, 0);
            btnMoi.Name = "btnMoi";
            btnMoi.Size = new Size(90, 64);
            btnMoi.TabIndex = 1;
            btnMoi.Text = "Mới";
            btnMoi.Click += btnMoi_Click;
            // 
            // btnXoa
            // 
            btnXoa.Dock = DockStyle.Left;
            btnXoa.Location = new Point(470, 0);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(90, 64);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.Click += btnXoa_Click;
            // 
            // btnBoQua
            // 
            btnBoQua.Dock = DockStyle.Left;
            btnBoQua.Location = new Point(380, 0);
            btnBoQua.Name = "btnBoQua";
            btnBoQua.Size = new Size(90, 64);
            btnBoQua.TabIndex = 3;
            btnBoQua.Text = "Bỏ qua";
            btnBoQua.Click += btnBoQua_Click;
            // 
            // btnLuu
            // 
            btnLuu.Dock = DockStyle.Left;
            btnLuu.Location = new Point(290, 0);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(90, 64);
            btnLuu.TabIndex = 4;
            btnLuu.Text = "Lưu";
            btnLuu.Click += btnLuu_Click;
            // 
            // btnSua
            // 
            btnSua.Dock = DockStyle.Left;
            btnSua.Location = new Point(200, 0);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(90, 64);
            btnSua.TabIndex = 5;
            btnSua.Text = "Sửa";
            btnSua.Click += btnSua_Click;
            // 
            // btnThem
            // 
            btnThem.Dock = DockStyle.Left;
            btnThem.Location = new Point(110, 0);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(90, 64);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.Click += btnThem_Click;
            // 
            // btnChiTiet
            // 
            btnChiTiet.Dock = DockStyle.Left;
            btnChiTiet.Location = new Point(0, 0);
            btnChiTiet.Name = "btnChiTiet";
            btnChiTiet.Size = new Size(110, 64);
            btnChiTiet.TabIndex = 7;
            btnChiTiet.Text = "Chi tiết";
            btnChiTiet.Click += btnChiTiet_Click;
            // 
            // pnlForm
            // 
            pnlForm.Controls.Add(btnTim);
            pnlForm.Controls.Add(txtSearch);
            pnlForm.Controls.Add(cboCa);
            pnlForm.Controls.Add(cboPhong);
            pnlForm.Controls.Add(nudSoLuong);
            pnlForm.Controls.Add(cboMaNV);
            pnlForm.Controls.Add(cboMaTK);
            pnlForm.Controls.Add(dtpNgayDK);
            pnlForm.Controls.Add(txtSoPhieu);
            pnlForm.Controls.Add(lblSearch);
            pnlForm.Controls.Add(lblCa);
            pnlForm.Controls.Add(lblPhong);
            pnlForm.Controls.Add(lblSL);
            pnlForm.Controls.Add(lblNoiSinh);
            pnlForm.Controls.Add(lblTen);
            pnlForm.Controls.Add(lblNgay);
            pnlForm.Controls.Add(lblMa);
            pnlForm.Controls.Add(lblTitle);
            pnlForm.Dock = DockStyle.Top;
            pnlForm.Location = new Point(0, 0);
            pnlForm.Name = "pnlForm";
            pnlForm.Padding = new Padding(8);
            pnlForm.Size = new Size(1004, 220);
            pnlForm.TabIndex = 3;
            // 
            // btnTim
            // 
            btnTim.Location = new Point(546, 177);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(70, 39);
            btnTim.TabIndex = 0;
            btnTim.Text = "Tìm";
            btnTim.Click += btnTim_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(120, 182);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(420, 31);
            txtSearch.TabIndex = 1;
            // 
            // cboCa
            // 
            cboCa.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCa.Location = new Point(822, 94);
            cboCa.Name = "cboCa";
            cboCa.Size = new Size(120, 33);
            cboCa.TabIndex = 2;
            // 
            // txtPhong
            // 
            this.cboPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhong.Location = new System.Drawing.Point(581, 133); // Tọa độ chuẩn
            this.cboPhong.Name = "cboPhong";
            this.cboPhong.Size = new System.Drawing.Size(270, 31);
            // 
            // nudSoLuong
            // 
            nudSoLuong.Location = new Point(620, 94);
            nudSoLuong.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudSoLuong.Name = "nudSoLuong";
            nudSoLuong.Size = new Size(120, 31);
            nudSoLuong.TabIndex = 4;
            // 
            // cboMaNV
            // 
            cboMaNV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaNV.Location = new Point(120, 131);
            cboMaNV.Name = "cboMaNV";
            cboMaNV.Size = new Size(320, 33);
            cboMaNV.TabIndex = 5;
            // 
            // cboMaTK
            // 
            cboMaTK.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMaTK.Location = new Point(120, 90);
            cboMaTK.Name = "cboMaTK";
            cboMaTK.Size = new Size(320, 33);
            cboMaTK.TabIndex = 6;
            // 
            // dtpNgayDK
            // 
            dtpNgayDK.Format = DateTimePickerFormat.Short;
            dtpNgayDK.Location = new Point(440, 56);
            dtpNgayDK.Name = "dtpNgayDK";
            dtpNgayDK.Size = new Size(160, 31);
            dtpNgayDK.TabIndex = 7;
            // 
            // txtSoPhieu
            // 
            txtSoPhieu.Location = new Point(120, 56);
            txtSoPhieu.Name = "txtSoPhieu";
            txtSoPhieu.Size = new Size(114, 31);
            txtSoPhieu.TabIndex = 8;
            // 
            // lblSearch
            // 
            lblSearch.Location = new Point(12, 184);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(100, 24);
            lblSearch.TabIndex = 9;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // lblCa
            // 
            lblCa.Location = new Point(782, 96);
            lblCa.Name = "lblCa";
            lblCa.Size = new Size(40, 24);
            lblCa.TabIndex = 10;
            lblCa.Text = "Ca:";
            // 
            // lblPhong
            // 
            lblPhong.Location = new Point(511, 135);
            lblPhong.Name = "lblPhong";
            lblPhong.Size = new Size(70, 24);
            lblPhong.TabIndex = 11;
            lblPhong.Text = "Phòng:";
            // 
            // lblSL
            // 
            lblSL.Location = new Point(512, 96);
            lblSL.Name = "lblSL";
            lblSL.Size = new Size(100, 24);
            lblSL.TabIndex = 12;
            lblSL.Text = "Số lượng:";
            // 
            // lblNoiSinh
            // 
            lblNoiSinh.Location = new Point(12, 133);
            lblNoiSinh.Name = "lblNoiSinh";
            lblNoiSinh.Size = new Size(100, 24);
            lblNoiSinh.TabIndex = 13;
            lblNoiSinh.Text = "Nhân viên:";
            // 
            // lblTen
            // 
            lblTen.Location = new Point(12, 92);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(100, 24);
            lblTen.TabIndex = 14;
            lblTen.Text = "Thực khách:";
            // 
            // lblNgay
            // 
            lblNgay.Location = new Point(340, 58);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(100, 24);
            lblNgay.TabIndex = 15;
            lblNgay.Text = "Ngày ĐK:";
            // 
            // lblMa
            // 
            lblMa.Location = new Point(12, 58);
            lblMa.Name = "lblMa";
            lblMa.Size = new Size(100, 24);
            lblMa.TabIndex = 16;
            lblMa.Text = "Số phiếu:";
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(8, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(600, 40);
            lblTitle.TabIndex = 17;
            lblTitle.Text = "QUẢN LÝ ĐẶT TIỆC";
            // 
            // FormDatTiec
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1286, 719);
            Controls.Add(splitMain);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 600);
            Name = "FormDatTiec";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đặt tiệc";
            Load += FormDatTiec_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            pnlButtons.ResumeLayout(false);
            pnlForm.ResumeLayout(false);
            pnlForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.Label lblNgay;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.Label lblNoiSinh;
        private System.Windows.Forms.Label lblSL;
        private System.Windows.Forms.Label lblPhong;
        private System.Windows.Forms.Label lblCa;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSoPhieu;
        private System.Windows.Forms.DateTimePicker dtpNgayDK;
        private System.Windows.Forms.ComboBox cboMaTK;
        private System.Windows.Forms.ComboBox cboMaNV;
        private System.Windows.Forms.NumericUpDown nudSoLuong;
        private System.Windows.Forms.ComboBox cboPhong;
        private System.Windows.Forms.ComboBox cboCa;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnBoQua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnMoi;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnChiTiet;
    }
}
