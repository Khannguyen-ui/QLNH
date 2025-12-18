using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_QLNH
{
    partial class FormQLHoaDon
    {
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
        private Label lblTitle;
        private TextBox txtKeyword;
        private Label label1;

        private RadioButton rdoNgay;
        private RadioButton rdoThang;
        private RadioButton rdoNam;

        private Label lblFrom;
        private Label lblTo;

        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;
        private DateTimePicker dtNgay;   // chỉ để tương thích giao diện
        private ComboBox cboThang;
        private NumericUpDown numNam;

        private Button btnSearch;
        private Button btnClear;
        private Button btnTopMon;
        private Button btnThongKe;

        private DataGridView dgvHD;
        private Label lblCount;
        private Label lblTotal;

        private Button btnView;
        private Button btnPrint;
        private Button btnClose;

        // Món top
        private DataGridView dgvTop;
        private Panel chartTop;

        // Layout
        private TableLayoutPanel tableTop;
        private Panel panelFilters;
        private Panel panelBottom;

        // Columns
        private DataGridViewTextBoxColumn colSoPhieu;
        private DataGridViewTextBoxColumn colNgayTT;
        private DataGridViewTextBoxColumn colTongTam;
        private DataGridViewTextBoxColumn colGiam;
        private DataGridViewTextBoxColumn colThanhTien;
        private DataGridViewTextBoxColumn colNV;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.dgvHD = new System.Windows.Forms.DataGridView();
            this.colSoPhieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongTam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableTop = new System.Windows.Forms.TableLayoutPanel();
            this.dgvTop = new System.Windows.Forms.DataGridView();
            this.chartTop = new System.Windows.Forms.Panel();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.rdoNgay = new System.Windows.Forms.RadioButton();
            this.rdoThang = new System.Windows.Forms.RadioButton();
            this.rdoNam = new System.Windows.Forms.RadioButton();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.dtNgay = new System.Windows.Forms.DateTimePicker();
            this.cboThang = new System.Windows.Forms.ComboBox();
            this.numNam = new System.Windows.Forms.NumericUpDown();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.btnTopMon = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHD)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.tableTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTop)).BeginInit();
            this.panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNam)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.Control;
            this.mainPanel.Controls.Add(this.dgvHD);
            this.mainPanel.Controls.Add(this.panelBottom);
            this.mainPanel.Controls.Add(this.tableTop);
            this.mainPanel.Controls.Add(this.panelFilters);
            this.mainPanel.Controls.Add(this.lblTitle);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(16);
            this.mainPanel.Size = new System.Drawing.Size(1260, 700);
            this.mainPanel.TabIndex = 0;
            // 
            // dgvHD
            // 
            this.dgvHD.AllowUserToAddRows = false;
            this.dgvHD.AllowUserToDeleteRows = false;
            this.dgvHD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHD.ColumnHeadersHeight = 34;
            this.dgvHD.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSoPhieu,
            this.colNgayTT,
            this.colTongTam,
            this.colGiam,
            this.colThanhTien,
            this.colNV});
            this.dgvHD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHD.Location = new System.Drawing.Point(16, 527);
            this.dgvHD.MultiSelect = false;
            this.dgvHD.Name = "dgvHD";
            this.dgvHD.ReadOnly = true;
            this.dgvHD.RowHeadersVisible = false;
            this.dgvHD.RowHeadersWidth = 62;
            this.dgvHD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHD.Size = new System.Drawing.Size(1228, 157);
            this.dgvHD.TabIndex = 0;
            this.dgvHD.DoubleClick += new System.EventHandler(this.dgvHD_DoubleClick);
            // 
            // colSoPhieu
            // 
            this.colSoPhieu.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSoPhieu.DataPropertyName = "SoPhieu";
            this.colSoPhieu.HeaderText = "Số phiếu";
            this.colSoPhieu.MinimumWidth = 8;
            this.colSoPhieu.Name = "colSoPhieu";
            this.colSoPhieu.ReadOnly = true;
            this.colSoPhieu.Width = 108;
            // 
            // colNgayTT
            // 
            this.colNgayTT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colNgayTT.DataPropertyName = "NgayTT";
            this.colNgayTT.HeaderText = "Ngày TT";
            this.colNgayTT.MinimumWidth = 8;
            this.colNgayTT.Name = "colNgayTT";
            this.colNgayTT.ReadOnly = true;
            this.colNgayTT.Width = 103;
            // 
            // colTongTam
            // 
            this.colTongTam.DataPropertyName = "TongTam";
            this.colTongTam.HeaderText = "Tổng tạm";
            this.colTongTam.MinimumWidth = 8;
            this.colTongTam.Name = "colTongTam";
            this.colTongTam.ReadOnly = true;
            // 
            // colGiam
            // 
            this.colGiam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colGiam.DataPropertyName = "GiamPT";
            this.colGiam.HeaderText = "Giảm (%)";
            this.colGiam.MinimumWidth = 8;
            this.colGiam.Name = "colGiam";
            this.colGiam.ReadOnly = true;
            this.colGiam.Width = 111;
            // 
            // colThanhTien
            // 
            this.colThanhTien.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colThanhTien.DataPropertyName = "ThanhTien";
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.MinimumWidth = 8;
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            this.colThanhTien.Width = 120;
            // 
            // colNV
            // 
            this.colNV.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colNV.DataPropertyName = "NVTT";
            this.colNV.HeaderText = "NV TT";
            this.colNV.MinimumWidth = 8;
            this.colNV.Name = "colNV";
            this.colNV.ReadOnly = true;
            this.colNV.Width = 89;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.lblCount);
            this.panelBottom.Controls.Add(this.lblTotal);
            this.panelBottom.Controls.Add(this.btnView);
            this.panelBottom.Controls.Add(this.btnPrint);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBottom.Location = new System.Drawing.Point(16, 471);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(12);
            this.panelBottom.Size = new System.Drawing.Size(1228, 56);
            this.panelBottom.TabIndex = 1;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(8, 8);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(104, 20);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "Có 0 hoá đơn";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(140, 8);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(201, 25);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "TỔNG THÀNH TIỀN: 0";
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(520, 12);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(80, 38);
            this.btnView.TabIndex = 2;
            this.btnView.Text = "Xem";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(608, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 38);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "In";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(696, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 38);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tableTop
            // 
            this.tableTop.ColumnCount = 2;
            this.tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableTop.Controls.Add(this.dgvTop, 0, 0);
            this.tableTop.Controls.Add(this.chartTop, 1, 0);
            this.tableTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableTop.Location = new System.Drawing.Point(16, 111);
            this.tableTop.Margin = new System.Windows.Forms.Padding(12);
            this.tableTop.Name = "tableTop";
            this.tableTop.RowCount = 1;
            this.tableTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTop.Size = new System.Drawing.Size(1228, 360);
            this.tableTop.TabIndex = 2;
            this.tableTop.Visible = false;
            // 
            // dgvTop
            // 
            this.dgvTop.AllowUserToAddRows = false;
            this.dgvTop.AllowUserToDeleteRows = false;
            this.dgvTop.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTop.ColumnHeadersHeight = 34;
            this.dgvTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTop.Location = new System.Drawing.Point(3, 3);
            this.dgvTop.MultiSelect = false;
            this.dgvTop.Name = "dgvTop";
            this.dgvTop.ReadOnly = true;
            this.dgvTop.RowHeadersVisible = false;
            this.dgvTop.RowHeadersWidth = 62;
            this.dgvTop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTop.Size = new System.Drawing.Size(730, 354);
            this.dgvTop.TabIndex = 0;
            // 
            // chartTop
            // 
            this.chartTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chartTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chartTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartTop.Location = new System.Drawing.Point(739, 3);
            this.chartTop.Name = "chartTop";
            this.chartTop.Size = new System.Drawing.Size(486, 354);
            this.chartTop.TabIndex = 1;
            // 
            // panelFilters
            // 
            this.panelFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFilters.Controls.Add(this.label1);
            this.panelFilters.Controls.Add(this.txtKeyword);
            this.panelFilters.Controls.Add(this.rdoNgay);
            this.panelFilters.Controls.Add(this.rdoThang);
            this.panelFilters.Controls.Add(this.rdoNam);
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.dtFrom);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtTo);
            this.panelFilters.Controls.Add(this.dtNgay);
            this.panelFilters.Controls.Add(this.cboThang);
            this.panelFilters.Controls.Add(this.numNam);
            this.panelFilters.Controls.Add(this.btnThongKe);
            this.panelFilters.Controls.Add(this.btnTopMon);
            this.panelFilters.Controls.Add(this.btnSearch);
            this.panelFilters.Controls.Add(this.btnClear);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(16, 16);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(8);
            this.panelFilters.Size = new System.Drawing.Size(1228, 95);
            this.panelFilters.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bộ lọc";
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(60, 6);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(320, 26);
            this.txtKeyword.TabIndex = 1;
            // 
            // rdoNgay
            // 
            this.rdoNgay.AutoSize = true;
            this.rdoNgay.Location = new System.Drawing.Point(12, 42);
            this.rdoNgay.Name = "rdoNgay";
            this.rdoNgay.Size = new System.Drawing.Size(70, 24);
            this.rdoNgay.TabIndex = 2;
            this.rdoNgay.Text = "Ngày";
            this.rdoNgay.CheckedChanged += new System.EventHandler(this.rdoNgay_CheckedChanged);
            // 
            // rdoThang
            // 
            this.rdoThang.AutoSize = true;
            this.rdoThang.Location = new System.Drawing.Point(88, 42);
            this.rdoThang.Name = "rdoThang";
            this.rdoThang.Size = new System.Drawing.Size(79, 24);
            this.rdoThang.TabIndex = 3;
            this.rdoThang.Text = "Tháng";
            this.rdoThang.CheckedChanged += new System.EventHandler(this.rdoThang_CheckedChanged);
            // 
            // rdoNam
            // 
            this.rdoNam.AutoSize = true;
            this.rdoNam.Location = new System.Drawing.Point(173, 42);
            this.rdoNam.Name = "rdoNam";
            this.rdoNam.Size = new System.Drawing.Size(67, 24);
            this.rdoNam.TabIndex = 4;
            this.rdoNam.Text = "Năm";
            this.rdoNam.CheckedChanged += new System.EventHandler(this.rdoNam_CheckedChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(270, 47);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(27, 20);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "Từ";
            this.lblFrom.Click += new System.EventHandler(this.lblFrom_Click);
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(304, 40);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(213, 26);
            this.dtFrom.TabIndex = 6;
            this.dtFrom.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(547, 44);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(40, 20);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "đến:";
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(593, 38);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(182, 26);
            this.dtTo.TabIndex = 8;
            // 
            // dtNgay
            // 
            this.dtNgay.Location = new System.Drawing.Point(304, 38);
            this.dtNgay.Name = "dtNgay";
            this.dtNgay.Size = new System.Drawing.Size(213, 26);
            this.dtNgay.TabIndex = 9;
            this.dtNgay.Visible = false;
            // 
            // cboThang
            // 
            this.cboThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThang.Location = new System.Drawing.Point(317, 40);
            this.cboThang.Name = "cboThang";
            this.cboThang.Size = new System.Drawing.Size(80, 28);
            this.cboThang.TabIndex = 10;
            this.cboThang.Visible = false;
            // 
            // numNam
            // 
            this.numNam.Location = new System.Drawing.Point(384, 44);
            this.numNam.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numNam.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numNam.Name = "numNam";
            this.numNam.Size = new System.Drawing.Size(80, 26);
            this.numNam.TabIndex = 11;
            this.numNam.Value = new decimal(new int[] {
            2025,
            0,
            0,
            0});
            this.numNam.Visible = false;
            // 
            // btnThongKe
            // 
            this.btnThongKe.Location = new System.Drawing.Point(979, 27);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(110, 36);
            this.btnThongKe.TabIndex = 12;
            this.btnThongKe.Text = "Thống Kê";
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // btnTopMon
            // 
            this.btnTopMon.Location = new System.Drawing.Point(1095, 27);
            this.btnTopMon.Name = "btnTopMon";
            this.btnTopMon.Size = new System.Drawing.Size(120, 36);
            this.btnTopMon.TabIndex = 13;
            this.btnTopMon.Text = "Món Top";
            this.btnTopMon.Click += new System.EventHandler(this.btnTopMon_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(793, 27);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 36);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Tìm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(879, 27);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 36);
            this.btnClear.TabIndex = 15;
            this.btnClear.Text = "Xoá";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(167, 48);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Hoá đơn";
            // 
            // FormQLHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 700);
            this.Controls.Add(this.mainPanel);
            this.Name = "FormQLHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hoá đơn/Thống kê";
            this.Load += new System.EventHandler(this.FormQLHoaDon_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHD)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.tableTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTop)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNam)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
