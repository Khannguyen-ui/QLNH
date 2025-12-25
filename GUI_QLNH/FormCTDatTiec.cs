using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BLL_QLNH;
using DTO_QLNH;

namespace GUI_QLNHS
{
    public partial class FormCTDatTiec : Form
    {
        private readonly CTDatTiecBLL _bll = new CTDatTiecBLL();
        public string SoPhieu { get; }

        public FormCTDatTiec(string soPhieu)
        {
            InitializeComponent();
            SoPhieu = soPhieu?.Trim();
            lblPhieu.Text = "Phiếu: " + SoPhieu;
        }

        private void FormCTDatTiec_Load(object sender, EventArgs e)
        {
            // nạp món
            var mons = _bll.LoadMon();
            cboMon.DisplayMember = "Ten";
            cboMon.ValueMember = "Ma";
            cboMon.DataSource = mons;

            if (mons.Count > 0) cboMon.SelectedIndex = 0;

            LoadGrid();
            numSL.Value = 1;
        }

        private void LoadGrid()
        {
            dgv.DataSource = _bll.GetByPhieu(SoPhieu);
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["SOPHIEU"].HeaderText = "Số phiếu";
                dgv.Columns["MATD"].HeaderText = "Mã món";
                if (dgv.Columns.Contains("TENMON")) dgv.Columns["TENMON"].HeaderText = "Tên món";
                dgv.Columns["SOLUONG"].HeaderText = "Số lượng";
                dgv.Columns["GIABAN"].HeaderText = "Giá bán";
                if (dgv.Columns.Contains("THANHTIEN")) dgv.Columns["THANHTIEN"].HeaderText = "Thành tiền";
            }
        }

        private void cboMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMon.SelectedItem is MonLookup m)
            {
                txtMaTD.Text = m.Ma;
                if (string.IsNullOrWhiteSpace(txtGia.Text))  // chỉ gợi ý khi chưa nhập
                    txtGia.Text = (m.Gia ?? 0).ToString();
                UpdateThanhTien();
            }
        }

        private void numSL_ValueChanged(object sender, EventArgs e) => UpdateThanhTien();
        private void txtGia_TextChanged(object sender, EventArgs e) => UpdateThanhTien();

        private void UpdateThanhTien()
        {
            double sl = (double)numSL.Value;
            if (!double.TryParse(txtGia.Text, out var gia)) gia = 0;
            lblThanhTien.Text = "Thành tiền: " + (sl * gia).ToString("N0");
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];
            txtMaTD.Text = Convert.ToString(r.Cells["MATD"].Value)?.Trim();
            // chọn lại combobox theo mã
            cboMon.SelectedValue = txtMaTD.Text;
            if (double.TryParse(Convert.ToString(r.Cells["SOLUONG"].Value), out var sl))
                numSL.Value = (decimal)Math.Max(0, sl);
            txtGia.Text = Convert.ToString(r.Cells["GIABAN"].Value);
            UpdateThanhTien();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var x = new CTDatTiec
                {
                    SoPhieu = SoPhieu,
                    MaTD = txtMaTD.Text.Trim(),
                    SoLuong = (double)numSL.Value,
                    GiaBan = double.TryParse(txtGia.Text, out var g) ? g : 0
                };
                if (_bll.Upsert(x))
                {
                    LoadGrid();
                    MessageBox.Show("Đã lưu chi tiết.");
                }
                else MessageBox.Show("Không lưu được.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTD.Text))
            { MessageBox.Show("Chọn 1 dòng hoặc nhập Mã món để xóa."); return; }
            if (MessageBox.Show("Xóa món này khỏi phiếu?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                if (_bll.Delete(SoPhieu, txtMaTD.Text.Trim()))
                { LoadGrid(); MessageBox.Show("Đã xóa."); }
                else MessageBox.Show("Không xóa được.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e) => Close();
    }
}
