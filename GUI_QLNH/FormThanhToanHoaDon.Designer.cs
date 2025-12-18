using System;
using System.Data;
using System.Windows.Forms;

namespace GUI_QLNH
{
    partial class FormThanhToanHoaDon
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSP;
        private System.Windows.Forms.TextBox txtSoPhieu;
        private System.Windows.Forms.Label lblNgay;
        private System.Windows.Forms.DateTimePicker dtNgayTT;

        private System.Windows.Forms.GroupBox grpKhach;
        private System.Windows.Forms.TextBox txtKhach;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label lblDiaChi;

        private System.Windows.Forms.GroupBox grpPhieu;
        private System.Windows.Forms.TextBox txtNhanVien;
        private System.Windows.Forms.TextBox txtPhong;
        private System.Windows.Forms.TextBox txtSLKhach;
        private System.Windows.Forms.Label lblNV;
        private System.Windows.Forms.Label lblPhong;
        private System.Windows.Forms.Label lblSLK;

        private System.Windows.Forms.DataGridView dgvCT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaTD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenMon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSL;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;

        private System.Windows.Forms.Label lblGiam;
        private System.Windows.Forms.NumericUpDown numGiamGia;
        private System.Windows.Forms.Label lblKhachDua;
        private System.Windows.Forms.NumericUpDown numKhachDua;
        private System.Windows.Forms.Label lblTongTam;
        private System.Windows.Forms.TextBox txtTongTam;
        private System.Windows.Forms.Label lblPhaiTra;
        private System.Windows.Forms.TextBox txtPhaiTra;
        private System.Windows.Forms.Label lblTienThoi;
        private System.Windows.Forms.TextBox txtTienThoi;

        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnDong;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSP = new System.Windows.Forms.Label();
            this.txtSoPhieu = new System.Windows.Forms.TextBox();
            this.lblNgay = new System.Windows.Forms.Label();
            this.dtNgayTT = new System.Windows.Forms.DateTimePicker();
            this.grpKhach = new System.Windows.Forms.GroupBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtKhach = new System.Windows.Forms.TextBox();
            this.lblSDT = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.grpPhieu = new System.Windows.Forms.GroupBox();
            this.lblNV = new System.Windows.Forms.Label();
            this.txtNhanVien = new System.Windows.Forms.TextBox();
            this.lblPhong = new System.Windows.Forms.Label();
            this.txtPhong = new System.Windows.Forms.TextBox();
            this.lblSLK = new System.Windows.Forms.Label();
            this.txtSLKhach = new System.Windows.Forms.TextBox();
            this.dgvCT = new System.Windows.Forms.DataGridView();
            this.colMaTD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenMon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblGiam = new System.Windows.Forms.Label();
            this.numGiamGia = new System.Windows.Forms.NumericUpDown();
            this.lblKhachDua = new System.Windows.Forms.Label();
            this.numKhachDua = new System.Windows.Forms.NumericUpDown();
            this.lblTongTam = new System.Windows.Forms.Label();
            this.txtTongTam = new System.Windows.Forms.TextBox();
            this.lblPhaiTra = new System.Windows.Forms.Label();
            this.txtPhaiTra = new System.Windows.Forms.TextBox();
            this.lblTienThoi = new System.Windows.Forms.Label();
            this.txtTienThoi = new System.Windows.Forms.TextBox();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.grpKhach.SuspendLayout();
            this.grpPhieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGiamGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKhachDua)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(18, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HÓA ĐƠN";
            // 
            // lblSP
            // 
            this.lblSP.AutoSize = true;
            this.lblSP.Location = new System.Drawing.Point(22, 52);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(76, 20);
            this.lblSP.TabIndex = 1;
            this.lblSP.Text = "Số phiếu:";
            // 
            // txtSoPhieu
            // 
            this.txtSoPhieu.Location = new System.Drawing.Point(112, 49);
            this.txtSoPhieu.Name = "txtSoPhieu";
            this.txtSoPhieu.Size = new System.Drawing.Size(180, 26);
            this.txtSoPhieu.TabIndex = 2;
            this.txtSoPhieu.Leave += new System.EventHandler(this.txtSoPhieu_Leave);
            // 
            // lblNgay
            // 
            this.lblNgay.AutoSize = true;
            this.lblNgay.Location = new System.Drawing.Point(355, 52);
            this.lblNgay.Name = "lblNgay";
            this.lblNgay.Size = new System.Drawing.Size(71, 20);
            this.lblNgay.TabIndex = 3;
            this.lblNgay.Text = "Ngày TT:";
            // 
            // dtNgayTT
            // 
            this.dtNgayTT.Location = new System.Drawing.Point(432, 48);
            this.dtNgayTT.Name = "dtNgayTT";
            this.dtNgayTT.Size = new System.Drawing.Size(210, 26);
            this.dtNgayTT.TabIndex = 4;
            // 
            // grpKhach
            // 
            this.grpKhach.Controls.Add(this.lblTen);
            this.grpKhach.Controls.Add(this.txtKhach);
            this.grpKhach.Controls.Add(this.lblSDT);
            this.grpKhach.Controls.Add(this.txtSDT);
            this.grpKhach.Controls.Add(this.lblDiaChi);
            this.grpKhach.Controls.Add(this.txtDiaChi);
            this.grpKhach.Location = new System.Drawing.Point(22, 84);
            this.grpKhach.Name = "grpKhach";
            this.grpKhach.Size = new System.Drawing.Size(538, 118);
            this.grpKhach.TabIndex = 5;
            this.grpKhach.TabStop = false;
            this.grpKhach.Text = "Khách hàng";
            // 
            // lblTen
            // 
            this.lblTen.AutoSize = true;
            this.lblTen.Location = new System.Drawing.Point(12, 28);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(66, 20);
            this.lblTen.TabIndex = 0;
            this.lblTen.Text = "Tên KH:";
            // 
            // txtKhach
            // 
            this.txtKhach.Location = new System.Drawing.Point(84, 24);
            this.txtKhach.Name = "txtKhach";
            this.txtKhach.ReadOnly = true;
            this.txtKhach.Size = new System.Drawing.Size(200, 26);
            this.txtKhach.TabIndex = 1;
            // 
            // lblSDT
            // 
            this.lblSDT.AutoSize = true;
            this.lblSDT.Location = new System.Drawing.Point(301, 27);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(45, 20);
            this.lblSDT.TabIndex = 2;
            this.lblSDT.Text = "SĐT:";
            this.lblSDT.Click += new System.EventHandler(this.lblSDT_Click);
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(352, 24);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.ReadOnly = true;
            this.txtSDT.Size = new System.Drawing.Size(180, 26);
            this.txtSDT.TabIndex = 3;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(12, 66);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(61, 20);
            this.lblDiaChi.TabIndex = 4;
            this.lblDiaChi.Text = "Địa chỉ:";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(70, 62);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.ReadOnly = true;
            this.txtDiaChi.Size = new System.Drawing.Size(440, 26);
            this.txtDiaChi.TabIndex = 5;
            // 
            // grpPhieu
            // 
            this.grpPhieu.Controls.Add(this.lblNV);
            this.grpPhieu.Controls.Add(this.txtNhanVien);
            this.grpPhieu.Controls.Add(this.lblPhong);
            this.grpPhieu.Controls.Add(this.txtPhong);
            this.grpPhieu.Controls.Add(this.lblSLK);
            this.grpPhieu.Controls.Add(this.txtSLKhach);
            this.grpPhieu.Location = new System.Drawing.Point(574, 84);
            this.grpPhieu.Name = "grpPhieu";
            this.grpPhieu.Size = new System.Drawing.Size(500, 118);
            this.grpPhieu.TabIndex = 6;
            this.grpPhieu.TabStop = false;
            this.grpPhieu.Text = "Thông tin phiếu";
            // 
            // lblNV
            // 
            this.lblNV.AutoSize = true;
            this.lblNV.Location = new System.Drawing.Point(14, 28);
            this.lblNV.Name = "lblNV";
            this.lblNV.Size = new System.Drawing.Size(83, 20);
            this.lblNV.TabIndex = 0;
            this.lblNV.Text = "Nhân viên:";
            // 
            // txtNhanVien
            // 
            this.txtNhanVien.Location = new System.Drawing.Point(103, 25);
            this.txtNhanVien.Name = "txtNhanVien";
            this.txtNhanVien.ReadOnly = true;
            this.txtNhanVien.Size = new System.Drawing.Size(160, 26);
            this.txtNhanVien.TabIndex = 1;
            // 
            // lblPhong
            // 
            this.lblPhong.AutoSize = true;
            this.lblPhong.Location = new System.Drawing.Point(269, 28);
            this.lblPhong.Name = "lblPhong";
            this.lblPhong.Size = new System.Drawing.Size(59, 20);
            this.lblPhong.TabIndex = 2;
            this.lblPhong.Text = "Phòng:";
            // 
            // txtPhong
            // 
            this.txtPhong.Location = new System.Drawing.Point(334, 24);
            this.txtPhong.Name = "txtPhong";
            this.txtPhong.ReadOnly = true;
            this.txtPhong.Size = new System.Drawing.Size(160, 26);
            this.txtPhong.TabIndex = 3;
            // 
            // lblSLK
            // 
            this.lblSLK.AutoSize = true;
            this.lblSLK.Location = new System.Drawing.Point(14, 66);
            this.lblSLK.Name = "lblSLK";
            this.lblSLK.Size = new System.Drawing.Size(102, 20);
            this.lblSLK.TabIndex = 4;
            this.lblSLK.Text = "Số lượng KH:";
            // 
            // txtSLKhach
            // 
            this.txtSLKhach.Location = new System.Drawing.Point(122, 62);
            this.txtSLKhach.Name = "txtSLKhach";
            this.txtSLKhach.ReadOnly = true;
            this.txtSLKhach.Size = new System.Drawing.Size(160, 26);
            this.txtSLKhach.TabIndex = 5;
            // 
            // dgvCT
            // 
            this.dgvCT.AllowUserToAddRows = false;
            this.dgvCT.AllowUserToDeleteRows = false;
            this.dgvCT.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCT.ColumnHeadersHeight = 34;
            this.dgvCT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaTD,
            this.colTenMon,
            this.colDVT,
            this.colSL,
            this.colGia,
            this.colThanhTien});
            this.dgvCT.Location = new System.Drawing.Point(22, 216);
            this.dgvCT.MultiSelect = false;
            this.dgvCT.Name = "dgvCT";
            this.dgvCT.ReadOnly = true;
            this.dgvCT.RowHeadersVisible = false;
            this.dgvCT.RowHeadersWidth = 62;
            this.dgvCT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCT.Size = new System.Drawing.Size(1052, 362);
            this.dgvCT.TabIndex = 7;
            // 
            // colMaTD
            // 
            this.colMaTD.DataPropertyName = "MaTD";
            this.colMaTD.FillWeight = 70F;
            this.colMaTD.HeaderText = "Mã";
            this.colMaTD.MinimumWidth = 8;
            this.colMaTD.Name = "colMaTD";
            this.colMaTD.ReadOnly = true;
            // 
            // colTenMon
            // 
            this.colTenMon.DataPropertyName = "TenMon";
            this.colTenMon.FillWeight = 220F;
            this.colTenMon.HeaderText = "Tên món";
            this.colTenMon.MinimumWidth = 8;
            this.colTenMon.Name = "colTenMon";
            this.colTenMon.ReadOnly = true;
            // 
            // colDVT
            // 
            this.colDVT.DataPropertyName = "DVT";
            this.colDVT.FillWeight = 80F;
            this.colDVT.HeaderText = "ĐVT";
            this.colDVT.MinimumWidth = 8;
            this.colDVT.Name = "colDVT";
            this.colDVT.ReadOnly = true;
            // 
            // colSL
            // 
            this.colSL.DataPropertyName = "SoLuong";
            dataGridViewCellStyle1.Format = "n0";
            this.colSL.DefaultCellStyle = dataGridViewCellStyle1;
            this.colSL.FillWeight = 70F;
            this.colSL.HeaderText = "SL";
            this.colSL.MinimumWidth = 8;
            this.colSL.Name = "colSL";
            this.colSL.ReadOnly = true;
            // 
            // colGia
            // 
            this.colGia.DataPropertyName = "GiaBan";
            dataGridViewCellStyle2.Format = "n0";
            this.colGia.DefaultCellStyle = dataGridViewCellStyle2;
            this.colGia.FillWeight = 110F;
            this.colGia.HeaderText = "Giá";
            this.colGia.MinimumWidth = 8;
            this.colGia.Name = "colGia";
            this.colGia.ReadOnly = true;
            // 
            // colThanhTien
            // 
            this.colThanhTien.DataPropertyName = "ThanhTien";
            dataGridViewCellStyle3.Format = "n0";
            this.colThanhTien.DefaultCellStyle = dataGridViewCellStyle3;
            this.colThanhTien.FillWeight = 130F;
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 8;
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            // 
            // lblGiam
            // 
            this.lblGiam.AutoSize = true;
            this.lblGiam.Location = new System.Drawing.Point(22, 592);
            this.lblGiam.Name = "lblGiam";
            this.lblGiam.Size = new System.Drawing.Size(100, 20);
            this.lblGiam.TabIndex = 8;
            this.lblGiam.Text = "Giảm giá (%)";
            // 
            // numGiamGia
            // 
            this.numGiamGia.Location = new System.Drawing.Point(128, 589);
            this.numGiamGia.Name = "numGiamGia";
            this.numGiamGia.Size = new System.Drawing.Size(80, 26);
            this.numGiamGia.TabIndex = 9;
            this.numGiamGia.ValueChanged += new System.EventHandler(this.numGiamGia_ValueChanged);
            // 
            // lblKhachDua
            // 
            this.lblKhachDua.AutoSize = true;
            this.lblKhachDua.Location = new System.Drawing.Point(225, 590);
            this.lblKhachDua.Name = "lblKhachDua";
            this.lblKhachDua.Size = new System.Drawing.Size(85, 20);
            this.lblKhachDua.TabIndex = 10;
            this.lblKhachDua.Text = "Khách đưa";
            // 
            // numKhachDua
            // 
            this.numKhachDua.Location = new System.Drawing.Point(316, 588);
            this.numKhachDua.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numKhachDua.Name = "numKhachDua";
            this.numKhachDua.Size = new System.Drawing.Size(133, 26);
            this.numKhachDua.TabIndex = 11;
            this.numKhachDua.ThousandsSeparator = true;
            this.numKhachDua.ValueChanged += new System.EventHandler(this.numKhachDua_ValueChanged);
            // 
            // lblTongTam
            // 
            this.lblTongTam.AutoSize = true;
            this.lblTongTam.Location = new System.Drawing.Point(455, 595);
            this.lblTongTam.Name = "lblTongTam";
            this.lblTongTam.Size = new System.Drawing.Size(76, 20);
            this.lblTongTam.TabIndex = 12;
            this.lblTongTam.Text = "Tổng tạm";
            // 
            // txtTongTam
            // 
            this.txtTongTam.Location = new System.Drawing.Point(537, 589);
            this.txtTongTam.Name = "txtTongTam";
            this.txtTongTam.ReadOnly = true;
            this.txtTongTam.Size = new System.Drawing.Size(120, 26);
            this.txtTongTam.TabIndex = 13;
            this.txtTongTam.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTongTam.TextChanged += new System.EventHandler(this.txtTongTam_TextChanged);
            // 
            // lblPhaiTra
            // 
            this.lblPhaiTra.AutoSize = true;
            this.lblPhaiTra.Location = new System.Drawing.Point(663, 592);
            this.lblPhaiTra.Name = "lblPhaiTra";
            this.lblPhaiTra.Size = new System.Drawing.Size(63, 20);
            this.lblPhaiTra.TabIndex = 14;
            this.lblPhaiTra.Text = "Phải trả";
            // 
            // txtPhaiTra
            // 
            this.txtPhaiTra.Location = new System.Drawing.Point(732, 586);
            this.txtPhaiTra.Name = "txtPhaiTra";
            this.txtPhaiTra.ReadOnly = true;
            this.txtPhaiTra.Size = new System.Drawing.Size(120, 26);
            this.txtPhaiTra.TabIndex = 15;
            this.txtPhaiTra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTienThoi
            // 
            this.lblTienThoi.AutoSize = true;
            this.lblTienThoi.Location = new System.Drawing.Point(879, 592);
            this.lblTienThoi.Name = "lblTienThoi";
            this.lblTienThoi.Size = new System.Drawing.Size(69, 20);
            this.lblTienThoi.TabIndex = 16;
            this.lblTienThoi.Text = "Tiền thối";
            // 
            // txtTienThoi
            // 
            this.txtTienThoi.Location = new System.Drawing.Point(954, 586);
            this.txtTienThoi.Name = "txtTienThoi";
            this.txtTienThoi.ReadOnly = true;
            this.txtTienThoi.Size = new System.Drawing.Size(120, 26);
            this.txtTienThoi.TabIndex = 17;
            this.txtTienThoi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Location = new System.Drawing.Point(574, 630);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(128, 30);
            this.btnThanhToan.TabIndex = 18;
            this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(720, 630);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(76, 30);
            this.btnIn.TabIndex = 19;
            this.btnIn.Text = "In";
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(812, 630);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(90, 30);
            this.btnExportExcel.TabIndex = 20;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(908, 630);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(87, 30);
            this.btnDong.TabIndex = 21;
            this.btnDong.Text = "Đóng";
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // FormThanhToanHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSP);
            this.Controls.Add(this.txtSoPhieu);
            this.Controls.Add(this.lblNgay);
            this.Controls.Add(this.dtNgayTT);
            this.Controls.Add(this.grpKhach);
            this.Controls.Add(this.grpPhieu);
            this.Controls.Add(this.dgvCT);
            this.Controls.Add(this.lblGiam);
            this.Controls.Add(this.numGiamGia);
            this.Controls.Add(this.lblKhachDua);
            this.Controls.Add(this.numKhachDua);
            this.Controls.Add(this.lblTongTam);
            this.Controls.Add(this.txtTongTam);
            this.Controls.Add(this.lblPhaiTra);
            this.Controls.Add(this.txtPhaiTra);
            this.Controls.Add(this.lblTienThoi);
            this.Controls.Add(this.txtTienThoi);
            this.Controls.Add(this.btnThanhToan);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnDong);
            this.Name = "FormThanhToanHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh Toán Hóa đơn";
            this.Load += new System.EventHandler(this.FormHoaDon_Load);
            this.grpKhach.ResumeLayout(false);
            this.grpKhach.PerformLayout();
            this.grpPhieu.ResumeLayout(false);
            this.grpPhieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGiamGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKhachDua)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
