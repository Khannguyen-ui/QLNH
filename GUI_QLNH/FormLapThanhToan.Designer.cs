using System.Windows.Forms;
using System.Drawing;

namespace GUI_QLNH
{
    partial class FormLapThanhToan
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgv;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnThanhToan;
        private Button btnClose;
        private Label lblCount;
        private CheckBox chkDate;
        private DateTimePicker dtFrom;
        private DateTimePicker dtTo;
        private Label label1;
        private Label label2;
        private Panel panelTop;
        private Panel panelBottom;

        private DataGridViewTextBoxColumn colMaHD;
        private DataGridViewTextBoxColumn colSoPhieu;
        private DataGridViewTextBoxColumn colNgayTT;
        private DataGridViewTextBoxColumn colTongTam;
        private DataGridViewTextBoxColumn colGiamPT;
        private DataGridViewTextBoxColumn colTongCuoi;
        private DataGridViewTextBoxColumn colNVTT;
        private DataGridViewTextBoxColumn colMaNV;
        private DataGridViewTextBoxColumn colTrangThai;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.colMaHD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoPhieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongTam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiamPT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongCuoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNVTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaNV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.chkDate = new System.Windows.Forms.CheckBox();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.ColumnHeadersHeight = 34;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaHD,
            this.colSoPhieu,
            this.colNgayTT,
            this.colTongTam,
            this.colGiamPT,
            this.colTongCuoi,
            this.colNVTT,
            this.colMaNV,
            this.colTrangThai});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 98);
            this.dgv.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 62;
            this.dgv.Size = new System.Drawing.Size(1183, 563);
            this.dgv.TabIndex = 0;
            // 
            // colMaHD
            // 
            this.colMaHD.DataPropertyName = "MaHD";
            this.colMaHD.FillWeight = 90F;
            this.colMaHD.HeaderText = "Mã HĐ";
            this.colMaHD.MinimumWidth = 8;
            this.colMaHD.Name = "colMaHD";
            // 
            // colSoPhieu
            // 
            this.colSoPhieu.DataPropertyName = "SoPhieu";
            this.colSoPhieu.FillWeight = 110F;
            this.colSoPhieu.HeaderText = "Số phiếu";
            this.colSoPhieu.MinimumWidth = 8;
            this.colSoPhieu.Name = "colSoPhieu";
            // 
            // colNgayTT
            // 
            this.colNgayTT.DataPropertyName = "NgayTT";
            dataGridViewCellStyle2.Format = "dd/MM/yyyy HH:mm";
            this.colNgayTT.DefaultCellStyle = dataGridViewCellStyle2;
            this.colNgayTT.FillWeight = 120F;
            this.colNgayTT.HeaderText = "Ngày TT";
            this.colNgayTT.MinimumWidth = 8;
            this.colNgayTT.Name = "colNgayTT";
            // 
            // colTongTam
            // 
            this.colTongTam.DataPropertyName = "TongTam";
            this.colTongTam.FillWeight = 110F;
            this.colTongTam.HeaderText = "Tổng tạm";
            this.colTongTam.MinimumWidth = 8;
            this.colTongTam.Name = "colTongTam";
            // 
            // colGiamPT
            // 
            this.colGiamPT.DataPropertyName = "GiamPT";
            this.colGiamPT.FillWeight = 80F;
            this.colGiamPT.HeaderText = "Giảm (%)";
            this.colGiamPT.MinimumWidth = 8;
            this.colGiamPT.Name = "colGiamPT";
            // 
            // colTongCuoi
            // 
            this.colTongCuoi.DataPropertyName = "TongCuoi";
            this.colTongCuoi.FillWeight = 120F;
            this.colTongCuoi.HeaderText = "Tổng cuối";
            this.colTongCuoi.MinimumWidth = 8;
            this.colTongCuoi.Name = "colTongCuoi";
            // 
            // colNVTT
            // 
            this.colNVTT.DataPropertyName = "NVTT";
            this.colNVTT.HeaderText = "NV TT";
            this.colNVTT.MinimumWidth = 8;
            this.colNVTT.Name = "colNVTT";
            // 
            // colMaNV
            // 
            this.colMaNV.DataPropertyName = "MaNV";
            this.colMaNV.FillWeight = 90F;
            this.colMaNV.HeaderText = "Mã NV";
            this.colMaNV.MinimumWidth = 8;
            this.colMaNV.Name = "colMaNV";
            // 
            // colTrangThai
            // 
            this.colTrangThai.DataPropertyName = "TrangThai";
            this.colTrangThai.FillWeight = 90F;
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.MinimumWidth = 8;
            this.colTrangThai.Name = "colTrangThai";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(15, 27);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(282, 26);
            this.txtSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(306, 24);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(96, 36);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Tìm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(410, 24);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 36);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThanhToan.Location = new System.Drawing.Point(1749, 13);
            this.btnThanhToan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(206, 37);
            this.btnThanhToan.TabIndex = 1;
            this.btnThanhToan.Text = "Thanh toán hóa đơn";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1967, 13);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 37);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Đóng";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(15, 21);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(134, 20);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "Có 0 hoá đơn chờ";
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(527, 29);
            this.chkDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(113, 24);
            this.chkDate.TabIndex = 3;
            this.chkDate.Text = "Theo ngày:";
            this.chkDate.CheckedChanged += new System.EventHandler(this.chkDate_CheckedChanged);
            // 
            // dtFrom
            // 
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(675, 27);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(192, 26);
            this.dtFrom.TabIndex = 5;
            // 
            // dtTo
            // 
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(926, 27);
            this.dtTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(192, 26);
            this.dtTo.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(636, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Từ:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(881, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Đến:";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Controls.Add(this.chkDate);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.dtFrom);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.dtTo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
            this.panelTop.Size = new System.Drawing.Size(1183, 98);
            this.panelTop.TabIndex = 2;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.lblCount);
            this.panelBottom.Controls.Add(this.btnThanhToan);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 661);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
            this.panelBottom.Size = new System.Drawing.Size(1183, 86);
            this.panelBottom.TabIndex = 1;
            // 
            // FormLapThanhToan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 747);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormLapThanhToan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách phiếu chờ thanh toán";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
