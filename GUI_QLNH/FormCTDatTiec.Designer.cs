namespace GUI_QLNHS
{
    partial class FormCTDatTiec
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPhieu = new System.Windows.Forms.Label();
            this.lblMon = new System.Windows.Forms.Label();
            this.lblMa = new System.Windows.Forms.Label();
            this.lblSL = new System.Windows.Forms.Label();
            this.lblGia = new System.Windows.Forms.Label();
            this.lblThanhTien = new System.Windows.Forms.Label();

            this.cboMon = new System.Windows.Forms.ComboBox();
            this.txtMaTD = new System.Windows.Forms.TextBox();
            this.numSL = new System.Windows.Forms.NumericUpDown();
            this.txtGia = new System.Windows.Forms.TextBox();

            this.dgv = new System.Windows.Forms.DataGridView();

            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.numSL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();

            // Title
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(14, 9);
            this.lblTitle.Size = new System.Drawing.Size(760, 34);
            this.lblTitle.Text = "CHI TIẾT ĐẶT TIỆC";

            this.lblPhieu.Location = new System.Drawing.Point(18, 45);
            this.lblPhieu.Size = new System.Drawing.Size(300, 22);
            this.lblPhieu.Text = "Phiếu: ";

            this.lblMon.Location = new System.Drawing.Point(18, 78);
            this.lblMon.Size = new System.Drawing.Size(80, 22);
            this.lblMon.Text = "Món:";

            this.cboMon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMon.Location = new System.Drawing.Point(100, 76);
            this.cboMon.Size = new System.Drawing.Size(280, 23);
            this.cboMon.SelectedIndexChanged += new System.EventHandler(this.cboMon_SelectedIndexChanged);

            this.lblMa.Location = new System.Drawing.Point(390, 78);
            this.lblMa.Size = new System.Drawing.Size(80, 22);
            this.lblMa.Text = "Mã món:";
            this.txtMaTD.Location = new System.Drawing.Point(460, 76);
            this.txtMaTD.Size = new System.Drawing.Size(100, 23);
            this.txtMaTD.ReadOnly = true;

            this.lblSL.Location = new System.Drawing.Point(18, 108);
            this.lblSL.Size = new System.Drawing.Size(80, 22);
            this.lblSL.Text = "Số lượng:";
            this.numSL.Location = new System.Drawing.Point(100, 106);
            this.numSL.Maximum = 100000;
            this.numSL.Minimum = 0;
            this.numSL.Value = 1;
            this.numSL.Size = new System.Drawing.Size(120, 23);
            this.numSL.ValueChanged += new System.EventHandler(this.numSL_ValueChanged);

            this.lblGia.Location = new System.Drawing.Point(240, 108);
            this.lblGia.Size = new System.Drawing.Size(60, 22);
            this.lblGia.Text = "Giá bán:";
            this.txtGia.Location = new System.Drawing.Point(300, 106);
            this.txtGia.Size = new System.Drawing.Size(120, 23);
            this.txtGia.TextChanged += new System.EventHandler(this.txtGia_TextChanged);

            this.lblThanhTien.Location = new System.Drawing.Point(440, 108);
            this.lblThanhTien.Size = new System.Drawing.Size(220, 22);
            this.lblThanhTien.Text = "Thành tiền: 0";

            // Grid
            this.dgv.Location = new System.Drawing.Point(18, 145);
            this.dgv.Size = new System.Drawing.Size(740, 300);
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.MultiSelect = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.RowHeadersVisible = false;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);

            // Buttons
            this.btnLuu.Location = new System.Drawing.Point(18, 455);
            this.btnLuu.Size = new System.Drawing.Size(90, 28);
            this.btnLuu.Text = "Lưu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            this.btnXoa.Location = new System.Drawing.Point(114, 455);
            this.btnXoa.Size = new System.Drawing.Size(90, 28);
            this.btnXoa.Text = "Xóa";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);

            this.btnThoat.Location = new System.Drawing.Point(210, 455);
            this.btnThoat.Size = new System.Drawing.Size(90, 28);
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 495);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết Đặt tiệc";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Load += new System.EventHandler(this.FormCTDatTiec_Load);

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitle, this.lblPhieu, this.lblMon, this.cboMon, this.lblMa, this.txtMaTD,
                this.lblSL, this.numSL, this.lblGia, this.txtGia, this.lblThanhTien,
                this.dgv, this.btnLuu, this.btnXoa, this.btnThoat
            });

            ((System.ComponentModel.ISupportInitialize)(this.numSL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle, lblPhieu, lblMon, lblMa, lblSL, lblGia, lblThanhTien;
        private System.Windows.Forms.ComboBox cboMon;
        private System.Windows.Forms.TextBox txtMaTD, txtGia;
        private System.Windows.Forms.NumericUpDown numSL;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnLuu, btnXoa, btnThoat;
    }
}
