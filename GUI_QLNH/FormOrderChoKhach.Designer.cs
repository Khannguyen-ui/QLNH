using System.Windows.Forms;

namespace GUI_QLNH
{
    partial class FormOrderChoKhach
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        
        #region Windows Form Designer generated code

        // ====== THÊM MỚI 2 PANEL KHUNG ======
        private Panel pnlMain;
        private Panel pnlCenter;

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnBoChon = new System.Windows.Forms.Button();
            this.btnChonAll = new System.Windows.Forms.Button();
            this.btnTim = new System.Windows.Forms.Button();
            this.txtTim = new System.Windows.Forms.TextBox();
            this.lblNhanVien = new System.Windows.Forms.Label();
            this.dgvTD = new System.Windows.Forms.DataGridView();
            this.colChon = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDVT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlRightCard = new System.Windows.Forms.Panel();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnMoi = new System.Windows.Forms.Button();
            this.btnTaoPhieu = new System.Windows.Forms.Button();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.labelTongTien = new System.Windows.Forms.Label();
            this.numGiamGia = new System.Windows.Forms.NumericUpDown();
            this.labelGiamGia = new System.Windows.Forms.Label();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.labelGhiChu = new System.Windows.Forms.Label();
            this.lblPhongCaStatus = new System.Windows.Forms.Label();
            this.cbCa = new System.Windows.Forms.ComboBox();
            this.labelCa = new System.Windows.Forms.Label();
            this.cbPhong = new System.Windows.Forms.ComboBox();
            this.labelPhong = new System.Windows.Forms.Label();
            this.numSoLuongKhach = new System.Windows.Forms.NumericUpDown();
            this.labelSoLuongKhach = new System.Windows.Forms.Label();
            this.dtNgay = new System.Windows.Forms.DateTimePicker();
            this.labelNgay = new System.Windows.Forms.Label();
            this.cbKhach = new System.Windows.Forms.ComboBox();
            this.labelKhach = new System.Windows.Forms.Label();
            this.chkOrderLe = new System.Windows.Forms.CheckBox();
            this.lblRightTitle = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnApplySL = new System.Windows.Forms.Button();
            this.numSLItem = new System.Windows.Forms.NumericUpDown();
            this.labelB_SL = new System.Windows.Forms.Label();
            this.txtGia = new System.Windows.Forms.TextBox();
            this.labelB_Gia = new System.Windows.Forms.Label();
            this.txtDVT = new System.Windows.Forms.TextBox();
            this.labelB_DVT = new System.Windows.Forms.Label();
            this.txtTenMon = new System.Windows.Forms.TextBox();
            this.labelB_Ten = new System.Windows.Forms.Label();
            this.txtMaTD = new System.Windows.Forms.TextBox();
            this.labelB_Ma = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTD)).BeginInit();
            this.pnlRightCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGiamGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuongKhach)).BeginInit();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSLItem)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTop.Controls.Add(this.btnBoChon);
            this.pnlTop.Controls.Add(this.btnChonAll);
            this.pnlTop.Controls.Add(this.btnTim);
            this.pnlTop.Controls.Add(this.txtTim);
            this.pnlTop.Controls.Add(this.lblNhanVien);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(12, 12);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(962, 71);
            this.pnlTop.TabIndex = 2;
            // 
            // btnBoChon
            // 
            this.btnBoChon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBoChon.Location = new System.Drawing.Point(820, 8);
            this.btnBoChon.Name = "btnBoChon";
            this.btnBoChon.Size = new System.Drawing.Size(110, 40);
            this.btnBoChon.TabIndex = 0;
            this.btnBoChon.Text = "Bỏ chọn";
            // 
            // btnChonAll
            // 
            this.btnChonAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChonAll.Location = new System.Drawing.Point(709, 8);
            this.btnChonAll.Name = "btnChonAll";
            this.btnChonAll.Size = new System.Drawing.Size(90, 38);
            this.btnChonAll.TabIndex = 1;
            this.btnChonAll.Text = "Chọn";
            // 
            // btnTim
            // 
            this.btnTim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTim.Location = new System.Drawing.Point(347, 14);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(81, 34);
            this.btnTim.TabIndex = 2;
            this.btnTim.Text = "Tìm";
            // 
            // txtTim
            // 
            this.txtTim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTim.Location = new System.Drawing.Point(454, 14);
            this.txtTim.Name = "txtTim";
            this.txtTim.Size = new System.Drawing.Size(212, 26);
            this.txtTim.TabIndex = 3;
            // 
            // lblNhanVien
            // 
            this.lblNhanVien.AutoSize = true;
            this.lblNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNhanVien.Location = new System.Drawing.Point(12, 14);
            this.lblNhanVien.Name = "lblNhanVien";
            this.lblNhanVien.Size = new System.Drawing.Size(136, 28);
            this.lblNhanVien.TabIndex = 4;
            this.lblNhanVien.Text = "Nhân viên: --";
            // 
            // dgvTD
            // 
            this.dgvTD.BackgroundColor = System.Drawing.Color.White;
            this.dgvTD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChon,
            this.colSL,
            this.colMa,
            this.colTen,
            this.colDVT,
            this.colGia,
            this.colTon});
            this.dgvTD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTD.Location = new System.Drawing.Point(12, 83);
            this.dgvTD.Name = "dgvTD";
            this.dgvTD.RowHeadersVisible = false;
            this.dgvTD.RowHeadersWidth = 62;
            this.dgvTD.RowTemplate.Height = 28;
            this.dgvTD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTD.Size = new System.Drawing.Size(962, 398);
            this.dgvTD.TabIndex = 0;
            // 
            // colChon
            // 
            this.colChon.DataPropertyName = "Chon";
            this.colChon.HeaderText = "Chọn";
            this.colChon.MinimumWidth = 8;
            this.colChon.Name = "colChon";
            this.colChon.Width = 55;
            // 
            // colSL
            // 
            this.colSL.DataPropertyName = "SL";
            this.colSL.HeaderText = "SL";
            this.colSL.MinimumWidth = 8;
            this.colSL.Name = "colSL";
            this.colSL.Width = 45;
            // 
            // colMa
            // 
            this.colMa.DataPropertyName = "MaTD";
            this.colMa.HeaderText = "Mã";
            this.colMa.MinimumWidth = 8;
            this.colMa.Name = "colMa";
            this.colMa.Width = 80;
            // 
            // colTen
            // 
            this.colTen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTen.DataPropertyName = "TenMon";
            this.colTen.HeaderText = "Tên món";
            this.colTen.MinimumWidth = 8;
            this.colTen.Name = "colTen";
            // 
            // colDVT
            // 
            this.colDVT.DataPropertyName = "DVT";
            this.colDVT.HeaderText = "ĐVT";
            this.colDVT.MinimumWidth = 8;
            this.colDVT.Name = "colDVT";
            this.colDVT.Width = 80;
            // 
            // colGia
            // 
            this.colGia.DataPropertyName = "GiaTien";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "n0";
            this.colGia.DefaultCellStyle = dataGridViewCellStyle1;
            this.colGia.HeaderText = "Giá tiền";
            this.colGia.MinimumWidth = 8;
            this.colGia.Name = "colGia";
            this.colGia.Width = 110;
            // 
            // colTon
            // 
            this.colTon.DataPropertyName = "SoLuongTon";
            this.colTon.HeaderText = "Tồn";
            this.colTon.MinimumWidth = 8;
            this.colTon.Name = "colTon";
            this.colTon.Width = 70;
            // 
            // pnlRightCard
            // 
            this.pnlRightCard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlRightCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRightCard.Controls.Add(this.btnThoat);
            this.pnlRightCard.Controls.Add(this.btnMoi);
            this.pnlRightCard.Controls.Add(this.btnTaoPhieu);
            this.pnlRightCard.Controls.Add(this.txtTongTien);
            this.pnlRightCard.Controls.Add(this.labelTongTien);
            this.pnlRightCard.Controls.Add(this.numGiamGia);
            this.pnlRightCard.Controls.Add(this.labelGiamGia);
            this.pnlRightCard.Controls.Add(this.txtGhiChu);
            this.pnlRightCard.Controls.Add(this.labelGhiChu);
            this.pnlRightCard.Controls.Add(this.lblPhongCaStatus);
            this.pnlRightCard.Controls.Add(this.cbCa);
            this.pnlRightCard.Controls.Add(this.labelCa);
            this.pnlRightCard.Controls.Add(this.cbPhong);
            this.pnlRightCard.Controls.Add(this.labelPhong);
            this.pnlRightCard.Controls.Add(this.numSoLuongKhach);
            this.pnlRightCard.Controls.Add(this.labelSoLuongKhach);
            this.pnlRightCard.Controls.Add(this.dtNgay);
            this.pnlRightCard.Controls.Add(this.labelNgay);
            this.pnlRightCard.Controls.Add(this.cbKhach);
            this.pnlRightCard.Controls.Add(this.labelKhach);
            this.pnlRightCard.Controls.Add(this.chkOrderLe);
            this.pnlRightCard.Controls.Add(this.lblRightTitle);
            this.pnlRightCard.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRightCard.Location = new System.Drawing.Point(998, 12);
            this.pnlRightCard.Name = "pnlRightCard";
            this.pnlRightCard.Padding = new System.Windows.Forms.Padding(12);
            this.pnlRightCard.Size = new System.Drawing.Size(393, 736);
            this.pnlRightCard.TabIndex = 1;
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(230, 515);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(80, 36);
            this.btnThoat.TabIndex = 0;
            this.btnThoat.Text = "Thoát";
            // 
            // btnMoi
            // 
            this.btnMoi.Location = new System.Drawing.Point(144, 517);
            this.btnMoi.Name = "btnMoi";
            this.btnMoi.Size = new System.Drawing.Size(80, 36);
            this.btnMoi.TabIndex = 1;
            this.btnMoi.Text = "Mới";
            // 
            // btnTaoPhieu
            // 
            this.btnTaoPhieu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTaoPhieu.Location = new System.Drawing.Point(16, 515);
            this.btnTaoPhieu.Name = "btnTaoPhieu";
            this.btnTaoPhieu.Size = new System.Drawing.Size(121, 36);
            this.btnTaoPhieu.TabIndex = 2;
            this.btnTaoPhieu.Text = "Tạo phiếu";
            this.btnTaoPhieu.Click += new System.EventHandler(this.btnTaoPhieu_Click_1);
            // 
            // txtTongTien
            // 
            this.txtTongTien.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.txtTongTien.Location = new System.Drawing.Point(116, 468);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.ReadOnly = true;
            this.txtTongTien.Size = new System.Drawing.Size(164, 34);
            this.txtTongTien.TabIndex = 3;
            this.txtTongTien.Text = "0";
            this.txtTongTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelTongTien
            // 
            this.labelTongTien.AutoSize = true;
            this.labelTongTien.Location = new System.Drawing.Point(16, 474);
            this.labelTongTien.Name = "labelTongTien";
            this.labelTongTien.Size = new System.Drawing.Size(79, 20);
            this.labelTongTien.TabIndex = 4;
            this.labelTongTien.Text = "Tổng tiền:";
            // 
            // numGiamGia
            // 
            this.numGiamGia.Location = new System.Drawing.Point(128, 424);
            this.numGiamGia.Name = "numGiamGia";
            this.numGiamGia.Size = new System.Drawing.Size(120, 26);
            this.numGiamGia.TabIndex = 5;
            // 
            // labelGiamGia
            // 
            this.labelGiamGia.AutoSize = true;
            this.labelGiamGia.Location = new System.Drawing.Point(13, 424);
            this.labelGiamGia.Name = "labelGiamGia";
            this.labelGiamGia.Size = new System.Drawing.Size(94, 20);
            this.labelGiamGia.TabIndex = 6;
            this.labelGiamGia.Text = "Giảm giá %:";
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Location = new System.Drawing.Point(116, 347);
            this.txtGhiChu.Multiline = true;
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(164, 64);
            this.txtGhiChu.TabIndex = 7;
            // 
            // labelGhiChu
            // 
            this.labelGhiChu.AutoSize = true;
            this.labelGhiChu.Location = new System.Drawing.Point(13, 350);
            this.labelGhiChu.Name = "labelGhiChu";
            this.labelGhiChu.Size = new System.Drawing.Size(68, 20);
            this.labelGhiChu.TabIndex = 8;
            this.labelGhiChu.Text = "Ghi chú:";
            // 
            // lblPhongCaStatus
            // 
            this.lblPhongCaStatus.Location = new System.Drawing.Point(236, 303);
            this.lblPhongCaStatus.Name = "lblPhongCaStatus";
            this.lblPhongCaStatus.Size = new System.Drawing.Size(140, 24);
            this.lblPhongCaStatus.TabIndex = 21;
            this.lblPhongCaStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbCa
            // 
            this.cbCa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCa.Location = new System.Drawing.Point(116, 300);
            this.cbCa.Name = "cbCa";
            this.cbCa.Size = new System.Drawing.Size(104, 28);
            this.cbCa.TabIndex = 9;
            // 
            // labelCa
            // 
            this.labelCa.AutoSize = true;
            this.labelCa.Location = new System.Drawing.Point(13, 303);
            this.labelCa.Name = "labelCa";
            this.labelCa.Size = new System.Drawing.Size(33, 20);
            this.labelCa.TabIndex = 10;
            this.labelCa.Text = "Ca:";
            // 
            // cbPhong
            // 
            this.cbPhong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPhong.Location = new System.Drawing.Point(116, 249);
            this.cbPhong.Name = "cbPhong";
            this.cbPhong.Size = new System.Drawing.Size(104, 28);
            this.cbPhong.TabIndex = 11;
            // 
            // labelPhong
            // 
            this.labelPhong.AutoSize = true;
            this.labelPhong.Location = new System.Drawing.Point(13, 250);
            this.labelPhong.Name = "labelPhong";
            this.labelPhong.Size = new System.Drawing.Size(59, 20);
            this.labelPhong.TabIndex = 12;
            this.labelPhong.Text = "Phòng:";
            // 
            // numSoLuongKhach
            // 
            this.numSoLuongKhach.Location = new System.Drawing.Point(116, 212);
            this.numSoLuongKhach.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSoLuongKhach.Name = "numSoLuongKhach";
            this.numSoLuongKhach.Size = new System.Drawing.Size(164, 26);
            this.numSoLuongKhach.TabIndex = 13;
            this.numSoLuongKhach.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelSoLuongKhach
            // 
            this.labelSoLuongKhach.AutoSize = true;
            this.labelSoLuongKhach.Location = new System.Drawing.Point(13, 212);
            this.labelSoLuongKhach.Name = "labelSoLuongKhach";
            this.labelSoLuongKhach.Size = new System.Drawing.Size(33, 20);
            this.labelSoLuongKhach.TabIndex = 14;
            this.labelSoLuongKhach.Text = "SL:";
            // 
            // dtNgay
            // 
            this.dtNgay.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtNgay.Location = new System.Drawing.Point(116, 170);
            this.dtNgay.Name = "dtNgay";
            this.dtNgay.Size = new System.Drawing.Size(190, 26);
            this.dtNgay.TabIndex = 15;
            // 
            // labelNgay
            // 
            this.labelNgay.AutoSize = true;
            this.labelNgay.Location = new System.Drawing.Point(13, 173);
            this.labelNgay.Name = "labelNgay";
            this.labelNgay.Size = new System.Drawing.Size(75, 20);
            this.labelNgay.TabIndex = 16;
            this.labelNgay.Text = "Ngày ĐK:";
            // 
            // cbKhach
            // 
            this.cbKhach.Location = new System.Drawing.Point(116, 125);
            this.cbKhach.Name = "cbKhach";
            this.cbKhach.Size = new System.Drawing.Size(190, 28);
            this.cbKhach.TabIndex = 17;
            // 
            // labelKhach
            // 
            this.labelKhach.AutoSize = true;
            this.labelKhach.Location = new System.Drawing.Point(13, 126);
            this.labelKhach.Name = "labelKhach";
            this.labelKhach.Size = new System.Drawing.Size(58, 20);
            this.labelKhach.TabIndex = 18;
            this.labelKhach.Text = "Khách:";
            // 
            // chkOrderLe
            // 
            this.chkOrderLe.AutoSize = true;
            this.chkOrderLe.Location = new System.Drawing.Point(5, 82);
            this.chkOrderLe.Name = "chkOrderLe";
            this.chkOrderLe.Size = new System.Drawing.Size(152, 24);
            this.chkOrderLe.TabIndex = 19;
            this.chkOrderLe.Text = "Order lẻ (tại chỗ)";
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.AutoSize = true;
            this.lblRightTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRightTitle.Location = new System.Drawing.Point(9, 10);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(183, 28);
            this.lblRightTitle.TabIndex = 20;
            this.lblRightTitle.Text = "Thông tin đặt tiệc";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.btnApplySL);
            this.pnlBottom.Controls.Add(this.numSLItem);
            this.pnlBottom.Controls.Add(this.labelB_SL);
            this.pnlBottom.Controls.Add(this.txtGia);
            this.pnlBottom.Controls.Add(this.labelB_Gia);
            this.pnlBottom.Controls.Add(this.txtDVT);
            this.pnlBottom.Controls.Add(this.labelB_DVT);
            this.pnlBottom.Controls.Add(this.txtTenMon);
            this.pnlBottom.Controls.Add(this.labelB_Ten);
            this.pnlBottom.Controls.Add(this.txtMaTD);
            this.pnlBottom.Controls.Add(this.labelB_Ma);
            this.pnlBottom.Controls.Add(this.lblCount);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(12, 481);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(962, 243);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnApplySL
            // 
            this.btnApplySL.Location = new System.Drawing.Point(777, 18);
            this.btnApplySL.Name = "btnApplySL";
            this.btnApplySL.Size = new System.Drawing.Size(90, 32);
            this.btnApplySL.TabIndex = 0;
            this.btnApplySL.Text = "Áp SL";
            // 
            // numSLItem
            // 
            this.numSLItem.Location = new System.Drawing.Point(827, 74);
            this.numSLItem.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numSLItem.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSLItem.Name = "numSLItem";
            this.numSLItem.Size = new System.Drawing.Size(60, 26);
            this.numSLItem.TabIndex = 1;
            this.numSLItem.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelB_SL
            // 
            this.labelB_SL.AutoSize = true;
            this.labelB_SL.Location = new System.Drawing.Point(781, 77);
            this.labelB_SL.Name = "labelB_SL";
            this.labelB_SL.Size = new System.Drawing.Size(29, 20);
            this.labelB_SL.TabIndex = 2;
            this.labelB_SL.Text = "SL";
            // 
            // txtGia
            // 
            this.txtGia.Location = new System.Drawing.Point(644, 78);
            this.txtGia.Name = "txtGia";
            this.txtGia.ReadOnly = true;
            this.txtGia.Size = new System.Drawing.Size(131, 26);
            this.txtGia.TabIndex = 3;
            this.txtGia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelB_Gia
            // 
            this.labelB_Gia.AutoSize = true;
            this.labelB_Gia.Location = new System.Drawing.Point(604, 84);
            this.labelB_Gia.Name = "labelB_Gia";
            this.labelB_Gia.Size = new System.Drawing.Size(34, 20);
            this.labelB_Gia.TabIndex = 4;
            this.labelB_Gia.Text = "Giá";
            // 
            // txtDVT
            // 
            this.txtDVT.Location = new System.Drawing.Point(501, 81);
            this.txtDVT.Name = "txtDVT";
            this.txtDVT.ReadOnly = true;
            this.txtDVT.Size = new System.Drawing.Size(80, 26);
            this.txtDVT.TabIndex = 5;
            // 
            // labelB_DVT
            // 
            this.labelB_DVT.AutoSize = true;
            this.labelB_DVT.Location = new System.Drawing.Point(454, 81);
            this.labelB_DVT.Name = "labelB_DVT";
            this.labelB_DVT.Size = new System.Drawing.Size(41, 20);
            this.labelB_DVT.TabIndex = 6;
            this.labelB_DVT.Text = "ĐVT";
            // 
            // txtTenMon
            // 
            this.txtTenMon.Location = new System.Drawing.Point(224, 78);
            this.txtTenMon.Name = "txtTenMon";
            this.txtTenMon.ReadOnly = true;
            this.txtTenMon.Size = new System.Drawing.Size(205, 26);
            this.txtTenMon.TabIndex = 7;
            // 
            // labelB_Ten
            // 
            this.labelB_Ten.AutoSize = true;
            this.labelB_Ten.Location = new System.Drawing.Point(182, 78);
            this.labelB_Ten.Name = "labelB_Ten";
            this.labelB_Ten.Size = new System.Drawing.Size(36, 20);
            this.labelB_Ten.TabIndex = 8;
            this.labelB_Ten.Text = "Tên";
            // 
            // txtMaTD
            // 
            this.txtMaTD.Location = new System.Drawing.Point(48, 75);
            this.txtMaTD.Name = "txtMaTD";
            this.txtMaTD.ReadOnly = true;
            this.txtMaTD.Size = new System.Drawing.Size(100, 26);
            this.txtMaTD.TabIndex = 9;
            // 
            // labelB_Ma
            // 
            this.labelB_Ma.AutoSize = true;
            this.labelB_Ma.Location = new System.Drawing.Point(13, 78);
            this.labelB_Ma.Name = "labelB_Ma";
            this.labelB_Ma.Size = new System.Drawing.Size(31, 20);
            this.labelB_Ma.TabIndex = 10;
            this.labelB_Ma.Text = "Mã";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(16, 12);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(77, 20);
            this.lblCount.TabIndex = 11;
            this.lblCount.Text = "Có 0 món";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlCenter);
            this.pnlMain.Controls.Add(this.pnlRightCard);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(12);
            this.pnlMain.Size = new System.Drawing.Size(1403, 760);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlCenter
            // 
            this.pnlCenter.Controls.Add(this.dgvTD);
            this.pnlCenter.Controls.Add(this.pnlBottom);
            this.pnlCenter.Controls.Add(this.pnlTop);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(12, 12);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Padding = new System.Windows.Forms.Padding(12);
            this.pnlCenter.Size = new System.Drawing.Size(986, 736);
            this.pnlCenter.TabIndex = 0;
            // 
            // FormOrderChoKhach
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1403, 760);
            this.Controls.Add(this.pnlMain);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormOrderChoKhach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhân viên (đặt tiệc)";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTD)).EndInit();
            this.pnlRightCard.ResumeLayout(false);
            this.pnlRightCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGiamGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSoLuongKhach)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSLItem)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        public System.Windows.Forms.Label lblNhanVien;
        private System.Windows.Forms.TextBox txtTim;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Button btnChonAll;
        private System.Windows.Forms.Button btnBoChon;

        private System.Windows.Forms.DataGridView dgvTD;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSL;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTon;

        private System.Windows.Forms.Panel pnlRightCard;
        private System.Windows.Forms.Label lblRightTitle;
        private System.Windows.Forms.Label lblPhongCaStatus;
        private System.Windows.Forms.CheckBox chkOrderLe;
        private System.Windows.Forms.Label labelKhach;
        private System.Windows.Forms.ComboBox cbKhach;
        private System.Windows.Forms.Label labelNgay;
        private System.Windows.Forms.DateTimePicker dtNgay;
        private System.Windows.Forms.Label labelSoLuongKhach;
        private System.Windows.Forms.NumericUpDown numSoLuongKhach;
        private System.Windows.Forms.Label labelPhong;
        private System.Windows.Forms.ComboBox cbPhong;
        private System.Windows.Forms.Label labelCa;
        private System.Windows.Forms.ComboBox cbCa;
        private System.Windows.Forms.Label labelGhiChu;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label labelGiamGia;
        private System.Windows.Forms.NumericUpDown numGiamGia;
        private System.Windows.Forms.Label labelTongTien;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.Button btnTaoPhieu;
        private System.Windows.Forms.Button btnMoi;
        private System.Windows.Forms.Button btnThoat;

        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label labelB_Ma;
        private System.Windows.Forms.TextBox txtMaTD;
        private System.Windows.Forms.Label labelB_Ten;
        private System.Windows.Forms.TextBox txtTenMon;
        private System.Windows.Forms.Label labelB_DVT;
        private System.Windows.Forms.TextBox txtDVT;
        private System.Windows.Forms.Label labelB_Gia;
        private System.Windows.Forms.TextBox txtGia;
        private System.Windows.Forms.Label labelB_SL;
        private System.Windows.Forms.NumericUpDown numSLItem;
        private System.Windows.Forms.Button btnApplySL;
    }
}
