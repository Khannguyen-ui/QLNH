using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;

namespace GUI_QLNH
{
    public partial class FormOrderChoKhach : Form
    {
        // =============== CẤU HÌNH KẾT NỐI ===============
        private const string CONN =
            "Server=LAPTOP-KRERKDGK\\SQLEXPRESS02;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";

        // Mã nhân viên hiện tại (FormMenu set trước khi Show)
        public string CurrentManv { get; set; } = "NV01";

        // =============== CTOR ===============
        public FormOrderChoKhach()
        {
            InitializeComponent();

            // Grid
            if (dgvTD != null)
            {
                dgvTD.AutoGenerateColumns = false;
                dgvTD.EditMode = DataGridViewEditMode.EditOnEnter;

                // đảm bảo gắn events (tránh double subscribe)
                dgvTD.SelectionChanged -= dgvTD_SelectionChanged;
                dgvTD.SelectionChanged += dgvTD_SelectionChanged;

                dgvTD.CellEndEdit -= dgvTD_CellEndEdit;
                dgvTD.CellEndEdit += dgvTD_CellEndEdit;
            }

            // Checkbox Order lẻ
            if (chkOrderLe != null)
            {
                chkOrderLe.CheckedChanged -= chkOrderLe_CheckedChanged;
                chkOrderLe.CheckedChanged += chkOrderLe_CheckedChanged;
            }

            // Giảm giá thay đổi => cập nhật tổng
            if (numGiamGia != null)
            {
                numGiamGia.ValueChanged -= numGiamGia_ValueChanged;
                numGiamGia.ValueChanged += numGiamGia_ValueChanged;
            }

            // Nút/tìm kiếm
            if (btnTim != null)
            {
                btnTim.Click -= btnTim_Click;
                btnTim.Click += btnTim_Click;
            }
            if (txtTim != null)
            {
                txtTim.KeyDown -= txtTim_KeyDown;
                txtTim.KeyDown += txtTim_KeyDown;
            }
            if (btnChonAll != null)
            {
                btnChonAll.Click -= btnChonAll_Click;
                btnChonAll.Click += btnChonAll_Click;
            }
            if (btnBoChon != null)
            {
                btnBoChon.Click -= btnBoChon_Click;
                btnBoChon.Click += btnBoChon_Click;
            }
            if (btnMoi != null)
            {
                btnMoi.Click -= btnMoi_Click;
                btnMoi.Click += btnMoi_Click;
            }
            if (btnApplySL != null)
            {
                btnApplySL.Click -= btnApplySL_Click;
                btnApplySL.Click += btnApplySL_Click;
            }
            if (btnTaoPhieu != null)
            {
                btnTaoPhieu.Click -= btnTaoPhieu_Click;
                btnTaoPhieu.Click += btnTaoPhieu_Click;
            }
            if (btnThoat != null)
            {
                btnThoat.Click -= btnThoat_Click;
                btnThoat.Click += btnThoat_Click;
            }

            // Phòng / Ca / Ngày change events -> update status
            if (cbPhong != null)
            {
                cbPhong.SelectedIndexChanged -= cbPhong_SelectedIndexChanged;
                cbPhong.SelectedIndexChanged += cbPhong_SelectedIndexChanged;
            }
            if (cbCa != null)
            {
                cbCa.SelectedIndexChanged -= cbCa_SelectedIndexChanged;
                cbCa.SelectedIndexChanged += cbCa_SelectedIndexChanged;
            }
            if (dtNgay != null)
            {
                dtNgay.ValueChanged -= dtNgay_ValueChanged;
                dtNgay.ValueChanged += dtNgay_ValueChanged;
            }

            // Đảm bảo Load form
            this.Load -= FormOrderChoKhach_Load;
            this.Load += FormOrderChoKhach_Load;
        }

        // Cho form cha (FormMenu) gọi để set nhân viên đăng nhập
        public void ApplyNhanVienFilter(string manv)
        {
            CurrentManv = manv;
            if (lblNhanVien != null) lblNhanVien.Text = "Nhân viên: " + manv;
        }

        // =============== LOAD FORM ===============
        private void FormOrderChoKhach_Load(object sender, EventArgs e)
        {
            try
            {
                if (dtNgay != null) dtNgay.Value = DateTime.Now;
                if (numSoLuongKhach != null) numSoLuongKhach.Value = 1;
                if (numGiamGia != null) numGiamGia.Value = 0;
                if (txtTongTien != null) txtTongTien.Text = "0";

                LoadThucDon();
                LoadKhachHang();
                LoadPhongVaCa();

                if (lblNhanVien != null) lblNhanVien.Text = "Nhân viên: " + CurrentManv;

                chkOrderLe_CheckedChanged(null, null);
                UpdateTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // =============== LOAD PHÒNG & CA (mặc định nếu DB bạn chưa có bảng) ===============
        private void LoadPhongVaCa()
        {
            if (cbPhong != null)
            {
                cbPhong.Items.Clear();
                cbPhong.Items.AddRange(new object[] { "Phòng 1", "Phòng 2", "Phòng 3", "VIP 1", "VIP 2" });
                cbPhong.SelectedIndex = -1;
            }

            if (cbCa != null)
            {
                cbCa.Items.Clear();
                cbCa.Items.AddRange(new object[] { "Ca sáng", "Ca trưa", "Ca tối" });
                cbCa.SelectedIndex = -1;
            }
        }

        // =============== UTILS ===============
        private static int AsInt(object v, int def = 0)
        {
            if (v == null || v == DBNull.Value) return def;
            int n; return int.TryParse(Convert.ToString(v), out n) ? n : def;
        }

        private static decimal AsDec(object v, decimal def = 0m)
        {
            if (v == null || v == DBNull.Value) return def;
            decimal d; return decimal.TryParse(Convert.ToString(v), out d) ? d : def;
        }

        private static string AsStr(object v) => (v == null || v == DBNull.Value) ? "" : v.ToString();

        private static bool ColumnExists(SqlConnection cn, SqlTransaction tx, string table, string column)
        {
            using (var cmd = new SqlCommand(@"
SELECT 1 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME=@t AND COLUMN_NAME=@c;", cn, tx))
            {
                cmd.Parameters.AddWithValue("@t", table);
                cmd.Parameters.AddWithValue("@c", column);
                return cmd.ExecuteScalar() != null;
            }
        }

        private static int GetColMaxLen(SqlConnection cn, SqlTransaction tx, string table, string column, int fallback = 20)
        {
            using (var cmd = new SqlCommand(@"
SELECT CHARACTER_MAXIMUM_LENGTH 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME=@t AND COLUMN_NAME=@c;", cn, tx))
            {
                cmd.Parameters.AddWithValue("@t", table);
                cmd.Parameters.AddWithValue("@c", column);
                object o = cmd.ExecuteScalar();
                return (o == null || o == DBNull.Value) ? fallback : Convert.ToInt32(o);
            }
        }

        // =============== CHECKBOX ORDER LẺ ===============
        private void chkOrderLe_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOrderLe == null) return;
            bool modeLe = chkOrderLe.Checked;

            if (cbKhach != null) cbKhach.Enabled = !modeLe;
            if (dtNgay != null) dtNgay.Enabled = !modeLe;
            if (numSoLuongKhach != null) numSoLuongKhach.Enabled = !modeLe;
            if (cbPhong != null) cbPhong.Enabled = !modeLe;
            if (cbCa != null) cbCa.Enabled = !modeLe;
            if (txtGhiChu != null) txtGhiChu.Enabled = !modeLe;

            if (modeLe)
            {
                if (numSoLuongKhach != null) numSoLuongKhach.Value = 1;
                if (cbPhong != null) cbPhong.SelectedIndex = -1;
                if (cbCa != null) cbCa.SelectedIndex = -1;
                if (txtGhiChu != null) txtGhiChu.Text = "";
            }
        }

        // =============== LOAD THỰC ĐƠN ===============
        private void LoadThucDon(string keyword = "")
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT MaTD, TenMon, DVT, GiaTien, GhiChu, SoLuongTon
FROM ThucDon
WHERE (@kw = '' OR TenMon LIKE '%'+@kw+'%' OR MaTD LIKE '%'+@kw+'%')
ORDER BY TenMon;";
                cmd.Parameters.AddWithValue("@kw", (keyword ?? "").Trim());

                var da = new SqlDataAdapter(cmd);
                var tb = new DataTable();
                da.Fill(tb);

                if (!tb.Columns.Contains("Chon")) tb.Columns.Add("Chon", typeof(bool));
                if (!tb.Columns.Contains("SL")) tb.Columns.Add("SL", typeof(int));

                foreach (DataRow r in tb.Rows)
                {
                    if (r["Chon"] == DBNull.Value) r["Chon"] = false;
                    if (r["SL"] == DBNull.Value) r["SL"] = 1;
                }

                if (dgvTD != null) dgvTD.DataSource = tb;
                if (lblCount != null) lblCount.Text = "Có " + tb.Rows.Count + " món";
            }

            UpdateTongTien();
        }

        // =============== LOAD KHÁCH HÀNG ===============
        private void LoadKhachHang()
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT MaTK, TenTK FROM ThucKhach ORDER BY TenTK;";
                var da = new SqlDataAdapter(cmd);
                var tb = new DataTable();
                da.Fill(tb);

                if (cbKhach != null)
                {
                    cbKhach.DataSource = tb;
                    cbKhach.DisplayMember = "TenTK";
                    cbKhach.ValueMember = "MaTK";
                    cbKhach.SelectedIndex = -1; // chưa chọn
                }
            }
        }

        // =============== SỰ KIỆN NHỎ (UI) ===============
        private void btnTim_Click(object sender, EventArgs e) => LoadThucDon(txtTim?.Text);

        private void txtTim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && btnTim != null) btnTim.PerformClick();
        }

        private void btnChonAll_Click(object sender, EventArgs e)
        {
            var dt = dgvTD?.DataSource as DataTable;
            if (dt == null) return;
            foreach (DataRow r in dt.Rows) r["Chon"] = true;
            UpdateTongTien();
        }

        private void btnBoChon_Click(object sender, EventArgs e)
        {
            var dt = dgvTD?.DataSource as DataTable;
            if (dt == null) return;
            foreach (DataRow r in dt.Rows) r["Chon"] = false;
            UpdateTongTien();
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            txtGhiChu?.Clear();
            if (cbPhong != null) cbPhong.SelectedIndex = -1;
            if (cbCa != null) cbCa.SelectedIndex = -1;
            if (numSoLuongKhach != null) numSoLuongKhach.Value = 1;
            if (dtNgay != null) dtNgay.Value = DateTime.Now;
            if (numGiamGia != null) numGiamGia.Value = 0;

            if (cbKhach != null && cbKhach.Items.Count > 0) cbKhach.SelectedIndex = -1;

            var dt = dgvTD?.DataSource as DataTable;
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    r["Chon"] = false;
                    r["SL"] = 1;
                }
            }

            txtMaTD?.Clear();
            txtTenMon?.Clear();
            txtDVT?.Clear();
            txtGia?.Clear();
            if (numSLItem != null) numSLItem.Value = 1;

            UpdateTongTien();
        }

        private void dgvTD_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTD?.CurrentRow == null) return;

            txtMaTD.Text = Convert.ToString(dgvTD.CurrentRow.Cells["colMa"].Value ?? "");
            txtTenMon.Text = Convert.ToString(dgvTD.CurrentRow.Cells["colTen"].Value ?? "");
            txtDVT.Text = Convert.ToString(dgvTD.CurrentRow.Cells["colDVT"].Value ?? "");

            decimal gia = AsDec(dgvTD.CurrentRow.Cells["colGia"].Value, 0m);
            txtGia.Text = gia.ToString("n0");

            int sl = AsInt(dgvTD.CurrentRow.Cells["colSL"].Value, 1);
            if (sl < 1) sl = 1;

            int ton = AsInt(dgvTD.CurrentRow.Cells["colTon"].Value, 9999);
            if (ton < 1) ton = 1;

            numSLItem.Maximum = ton;
            if (sl > ton) sl = ton;
            numSLItem.Value = sl;
        }

        private void btnApplySL_Click(object sender, EventArgs e)
        {
            if (dgvTD?.CurrentRow == null) return;

            int slMoi = (int)numSLItem.Value;
            if (slMoi < 1) slMoi = 1;

            int ton = AsInt(dgvTD.CurrentRow.Cells["colTon"].Value, 9999);
            if (slMoi > ton) slMoi = ton;

            dgvTD.CurrentRow.Cells["colSL"].Value = slMoi;
            dgvTD.CurrentRow.Cells["colChon"].Value = true;

            UpdateTongTien();
        }

        private void dgvTD_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvTD == null) return;
            if (dgvTD.Columns[e.ColumnIndex].Name != "colSL") return;

            var row = dgvTD.Rows[e.RowIndex];

            int userSL = AsInt(row.Cells["colSL"].Value, 1);
            if (userSL < 1) userSL = 1;

            int ton = AsInt(row.Cells["colTon"].Value, 9999);
            if (userSL > ton) userSL = ton;

            row.Cells["colSL"].Value = userSL;
            row.Cells["colChon"].Value = true;

            UpdateTongTien();
        }

        private void numGiamGia_ValueChanged(object sender, EventArgs e) => UpdateTongTien();

        // =============== TÍNH TỔNG TIỀN ===============
        private void UpdateTongTien()
        {
            var dt = dgvTD?.DataSource as DataTable;
            if (dt == null) { if (txtTongTien != null) txtTongTien.Text = "0"; return; }

            decimal sum = 0m;
            foreach (DataRow r in dt.Rows)
            {
                bool chon = r["Chon"] != DBNull.Value && (bool)r["Chon"];
                if (!chon) continue;

                int sl = Math.Max(1, AsInt(r["SL"], 1));
                decimal gia = AsDec(r["GiaTien"], 0m);

                sum += gia * sl;
            }

            decimal giamPT = (numGiamGia != null) ? numGiamGia.Value : 0;
            if (giamPT < 0) giamPT = 0;
            if (giamPT > 100) giamPT = 100;

            decimal after = sum * (1 - giamPT / 100m);

            if (txtTongTien != null)
                txtTongTien.Text = Math.Round(after, MidpointRounding.AwayFromZero).ToString("n0");
        }

        // =============== LƯU PHIẾU + (CÓ THỂ) TẠO HÓA ĐƠN ===============
        private void btnTaoPhieu_Click(object sender, EventArgs e)
        {
            bool isOrderLe = chkOrderLe != null && chkOrderLe.Checked;

            var dtGrid = dgvTD?.DataSource as DataTable;
            if (dtGrid == null) return;

            var items = dtGrid.AsEnumerable()
                .Where(r => (r.Field<bool?>("Chon") ?? false))
                .Select(r => new
                {
                    MaTD = AsStr(r["MaTD"]),
                    SL = Math.Max(1, AsInt(r["SL"], 1)),
                    Gia = AsDec(r["GiaTien"], 0m)
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.MaTD))
                .ToList();

            if (items.Count == 0)
            {
                MessageBox.Show("Chưa chọn món nào.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentManv))
            {
                MessageBox.Show("Thiếu mã nhân viên.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy thông tin form
            string phong = cbPhong?.Text?.Trim() ?? "";
            string caDat = cbCa?.Text?.Trim() ?? "";
            string ghiChu = txtGhiChu?.Text?.Trim() ?? "";
            int soLuongKhach = (int)(numSoLuongKhach?.Value ?? 1);
            DateTime ngay = dtNgay?.Value ?? DateTime.Now;

            string maTK = cbKhach?.SelectedValue == null ? null : cbKhach.SelectedValue.ToString();
            string tenTK = cbKhach?.Text == null ? null : cbKhach.Text.Trim();

            if (!isOrderLe)
            {
                if (string.IsNullOrWhiteSpace(maTK) && string.IsNullOrWhiteSpace(tenTK))
                {
                    MessageBox.Show("Chọn khách hoặc nhập tên khách.",
                        "Thiếu thông tin khách", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(phong))
                {
                    MessageBox.Show("Chọn Phòng/Sảnh.", "Thiếu dữ liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbPhong?.Focus(); return;
                }
                if (string.IsNullOrWhiteSpace(caDat))
                {
                    MessageBox.Show("Chọn Ca phục vụ.", "Thiếu dữ liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbCa?.Focus(); return;
                }
            }
            else
            {
                // Order lẻ (ăn tại chỗ)
                phong = "Bàn lẻ";
                caDat = "Tại chỗ";
                ghiChu = "(Order lẻ tại chỗ)";
                soLuongKhach = 1;
                ngay = DateTime.Now;
                if (string.IsNullOrWhiteSpace(maTK) && string.IsNullOrWhiteSpace(tenTK))
                    maTK = null; // khách vãng lai
            }

            string soPhieu = null;
            string maHD = null;

            using (var cn = new SqlConnection(CONN))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        // Nếu khách chưa có mã -> tạo khách tạm
                        if (string.IsNullOrEmpty(maTK) && !string.IsNullOrWhiteSpace(tenTK))
                            maTK = TryEnsureKhachHang(cn, tx, tenTK);

                        bool hasCa = ColumnExists(cn, tx, "DatTiec", "Ca");
                        bool hasNgayDatNgay = ColumnExists(cn, tx, "DatTiec", "NgayDatNgay");
                        bool hasGhiChu = ColumnExists(cn, tx, "DatTiec", "GhiChu");
                        bool hasTrangThai = ColumnExists(cn, tx, "DatTiec", "TrangThai");

                        // Check trùng theo NgayDatNgay (nếu có)
                        if (!isOrderLe && hasCa && hasNgayDatNgay)
                        {
                            if (IsPhongCaBusy(cn, tx, ngay.Date, phong, caDat))
                            {
                                MessageBox.Show(
                                    $"Phòng {phong} - {caDat} ngày {ngay:dd/MM/yyyy} đã có khách đặt.\n" +
                                    "Vui lòng chọn Phòng/Ca khác!",
                                    "Trùng lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tx.Rollback();
                                return;
                            }
                        }

                        // Tạo SoPhieu unique
                        int maxLen = GetColMaxLen(cn, tx, "DatTiec", "SoPhieu", 20);
                        string baseId = (isOrderLe ? "O" : "P") + DateTime.Now.ToString("yyMMddHHmmssff");
                        soPhieu = baseId.Length <= maxLen
                            ? baseId
                            : (isOrderLe ? "O" : "P") + baseId.Substring(baseId.Length - (maxLen - 1));

                        // Build INSERT DatTiec động (bao gồm NgayDatNgay + TrangThai nếu có)
                        string sqlInsertDT = @"
INSERT INTO DatTiec(SoPhieu, NgayDK{0}, MaTK, MaNV, SoLuongKhach, Phong{1}{2}{3})
VALUES(@SoPhieu, @NgayDK{4}, @MaTK, @MaNV, @SLK, @Phong{5}{6}{7})";

                        string colNgayDatNgay = hasNgayDatNgay ? ", NgayDatNgay" : "";
                        string valNgayDatNgay = hasNgayDatNgay ? ", @NgayDatNgay" : "";

                        string colCa = hasCa ? ", Ca" : "";
                        string colGC = hasGhiChu ? ", GhiChu" : "";
                        string colTT = hasTrangThai ? ", TrangThai" : "";

                        string valCa = hasCa ? ", @Ca" : "";
                        string valGC = hasGhiChu ? ", @GhiChu" : "";
                        string valTT = hasTrangThai ? ", N'CHO_TT'" : ""; // set trạng thái chờ

                        sqlInsertDT = string.Format(sqlInsertDT,
                            colNgayDatNgay, colCa, colGC, colTT,
                            valNgayDatNgay, valCa, valGC, valTT);

                        using (var cmd = new SqlCommand(sqlInsertDT, cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                            cmd.Parameters.AddWithValue("@NgayDK", ngay);
                            if (hasNgayDatNgay) cmd.Parameters.AddWithValue("@NgayDatNgay", ngay.Date);
                            cmd.Parameters.AddWithValue("@MaTK", (object)(maTK ?? (string)null) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaNV", CurrentManv);
                            cmd.Parameters.AddWithValue("@SLK", soLuongKhach);
                            cmd.Parameters.AddWithValue("@Phong", phong);
                            if (hasCa) cmd.Parameters.AddWithValue("@Ca", caDat);
                            if (hasGhiChu) cmd.Parameters.AddWithValue("@GhiChu", ghiChu);
                            cmd.ExecuteNonQuery();
                        }

                        // Thêm CTDatTiec + trừ tồn kho
                        foreach (var it in items)
                        {
                            // check tồn kho
                            using (var chkCmd = new SqlCommand(
                                "SELECT SoLuongTon FROM ThucDon WHERE MaTD=@ma;", cn, tx))
                            {
                                chkCmd.Parameters.AddWithValue("@ma", it.MaTD);
                                int current = Convert.ToInt32(chkCmd.ExecuteScalar() ?? 0);
                                if (current < it.SL)
                                {
                                    tx.Rollback();
                                    MessageBox.Show(
                                        $"Không đủ tồn kho cho món {it.MaTD}. Tồn hiện tại: {current}.",
                                        "Lỗi tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            // trừ tồn
                            using (var decCmd = new SqlCommand(@"
UPDATE ThucDon SET SoLuongTon = SoLuongTon - @qty 
WHERE MaTD=@ma AND SoLuongTon >= @qty;", cn, tx))
                            {
                                decCmd.Parameters.AddWithValue("@qty", it.SL);
                                decCmd.Parameters.AddWithValue("@ma", it.MaTD);
                                if (decCmd.ExecuteNonQuery() <= 0)
                                {
                                    tx.Rollback();
                                    MessageBox.Show(
                                        $"Không thể cập nhật tồn kho cho món {it.MaTD}.",
                                        "Lỗi tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            // insert CTDatTiec
                            using (var cmd = new SqlCommand(@"
INSERT INTO CTDatTiec(SoPhieu, MaTD, SoLuong, GiaBan)
VALUES(@SoPhieu, @MaTD, @SoLuong, @GiaBan);", cn, tx))
                            {
                                cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                                cmd.Parameters.AddWithValue("@MaTD", it.MaTD);
                                cmd.Parameters.AddWithValue("@SoLuong", it.SL);
                                cmd.Parameters.AddWithValue("@GiaBan", it.Gia);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        try { tx.Rollback(); } catch { }
                        var msg = ex.Message ?? "";
                        if (msg.Contains("UX_DatTiec_Ngay_Phong_Ca") || msg.ToLower().Contains("duplicate key"))
                        {
                            MessageBox.Show(
                                "Phòng / Ca này đã có khách đặt trong ngày đã chọn.\nVui lòng chọn phòng hoặc ca khác!",
                                "Trùng lịch đặt tiệc", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Lỗi lưu phiếu: " + msg, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }
                }
            }

            // Tạo hóa đơn từ phiếu (nếu có proc)
            maHD = TryCreateHoaDonForSoPhieu(soPhieu, (decimal)(numGiamGia?.Value ?? 0), CurrentManv);

            // Báo OK
            MessageBox.Show(
                "Đã tạo phiếu " + soPhieu +
                (string.IsNullOrEmpty(maHD) ? "" : ("\nHoá đơn: " + maHD)),
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Hỏi thanh toán luôn
            var dr = MessageBox.Show(
                "Mở hoá đơn để thanh toán ngay bây giờ?",
                "Hoá đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (var f = new FormThanhToanHoaDon(soPhieu, CurrentManv, (decimal)(numGiamGia?.Value ?? 0)))
                    {
                        f.StartPosition = FormStartPosition.CenterParent;
                        var rs = f.ShowDialog(this);

                        bool isPaid = false;
                        try
                        {
                            var prop = f.GetType().GetProperty("IsPaid");
                            if (prop != null) isPaid = (bool)(prop.GetValue(f) ?? false);
                        }
                        catch { }

                        if (isPaid) SetTrangThaiPhieu(soPhieu, "DA_TT");

                        GoQLHoaDonWithSoPhieu(soPhieu);
                    }
                }
                catch
                {
                    // nếu chưa có FormHoaDon thì bỏ qua
                }
            }
            else
            {
                // Không thanh toán ngay: giữ trạng thái CHO_TT để xử lý sau
                SetTrangThaiPhieu(soPhieu, "CHO_TT");

                // Mở danh sách chờ thanh toán và focus ngay phiếu vừa tạo
                try
                {
                    var frm = new FormLapThanhToan();
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.Show(this);
                    frm.FocusBySoPhieu(soPhieu);
                }
                catch { /* nếu chưa add form này thì bỏ qua */ }
            }

            // Reset form + reload tồn
            btnMoi?.PerformClick();
            LoadThucDon();
        }

        // =============== HELPER: check trùng Phòng–Ca–Ngày ===============
        private bool IsPhongCaBusy(SqlConnection cn, SqlTransaction tx, DateTime ngay, string phong, string ca)
        {
            using (var cmd = new SqlCommand(@"
SELECT COUNT(*) 
FROM DatTiec
WHERE NgayDatNgay = @ngay AND Phong = @phong AND Ca = @ca;", cn, tx))
            {
                cmd.Parameters.AddWithValue("@ngay", ngay);
                cmd.Parameters.AddWithValue("@phong", phong);
                cmd.Parameters.AddWithValue("@ca", ca);
                int n = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                return n > 0;
            }
        }

        // =============== HELPER: set trạng thái phiếu ===============
        private void SetTrangThaiPhieu(string soPhieu, string trangThai)
        {
            try
            {
                using (var cn = new SqlConnection(CONN))
                using (var cmd = new SqlCommand(@"
IF COL_LENGTH('dbo.DatTiec','TrangThai') IS NOT NULL
    UPDATE dbo.DatTiec SET TrangThai=@tt WHERE SoPhieu=@sp;", cn))
                {
                    cmd.Parameters.AddWithValue("@tt", trangThai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sp", soPhieu);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* bỏ qua */ }
        }

        // =============== TẠO HÓA ĐƠN TỪ STORED PROC (nếu có) ===============
        // Proc: dbo.sp_HD_CreateFromSoPhieu(@SoPhieu, @GiamPT, @NVTT, @MaHD OUT NVARCHAR(20))
        private string TryCreateHoaDonForSoPhieu(string soPhieu, decimal giamPT, string nvtt)
        {
            try
            {
                using (var cn = new SqlConnection(CONN))
                {
                    cn.Open();

                    // kiểm tra proc có tồn tại không
                    using (var chk = new SqlCommand(@"
SELECT 1 FROM sys.objects 
WHERE object_id = OBJECT_ID(N'dbo.sp_HD_CreateFromSoPhieu') 
  AND type = 'P';", cn))
                    {
                        var ok = chk.ExecuteScalar();
                        if (ok == null) return null;
                    }

                    using (var cmd = new SqlCommand("dbo.sp_HD_CreateFromSoPhieu", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                        cmd.Parameters.AddWithValue("@GiamPT", giamPT);
                        cmd.Parameters.AddWithValue("@NVTT", nvtt);

                        var pOut = new SqlParameter("@MaHD", SqlDbType.NVarChar, 20)
                        { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(pOut);

                        cmd.ExecuteNonQuery();
                        return Convert.ToString(pOut.Value ?? "");
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        // =============== ĐIỀU HƯỚNG VỀ QL HÓA ĐƠN (nếu FormMenu hỗ trợ) ===============
        private void GoQLHoaDonWithSoPhieu(string soPhieu)
        {
            var menu = this.ParentForm as FormMenu;
            if (menu != null)
            {
                try
                {
                    var mi = menu.GetType().GetMethod(
                        "OpenQLHoaDonWithSoPhieu",
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    );
                    if (mi != null) mi.Invoke(menu, new object[] { soPhieu });
                }
                catch { }
            }
        }

        // =============== TẠO KHÁCH HÀNG TẠM ===============
        private string TryEnsureKhachHang(SqlConnection cn, SqlTransaction tx, string tenTk)
        {
            string ma = "TK" + DateTime.Now.ToString("mmssff");

            using (var cmd = new SqlCommand(@"
INSERT INTO ThucKhach(MaTK, TenTK, DiaChi, SoDT)
VALUES(@Ma, @Ten, N'', N'');", cn, tx))
            {
                cmd.Parameters.AddWithValue("@Ma", ma);
                cmd.Parameters.AddWithValue("@Ten", tenTk);
                cmd.ExecuteNonQuery();
            }

            return ma;
        }

        // =============== LẶT VẶT ===============
        private void btnThoat_Click(object sender, EventArgs e) => this.Close();

        private void labelB_SL_Click(object sender, EventArgs e) { /* optional */ }

        private void cbPhong_SelectedIndexChanged(object sender, EventArgs e) => UpdatePhongCaStatus();
        private void cbCa_SelectedIndexChanged(object sender, EventArgs e) => UpdatePhongCaStatus();
        private void dtNgay_ValueChanged(object sender, EventArgs e) => UpdatePhongCaStatus();

        // Update room/shift status label by querying DatTiec table for selected date/room/ca
        private void UpdatePhongCaStatus()
        {
            try
            {
                if (lblPhongCaStatus == null) return;

                if (chkOrderLe != null && chkOrderLe.Checked)
                {
                    lblPhongCaStatus.Text = "Order lẻ (không cần đặt phòng)";
                    lblPhongCaStatus.ForeColor = Color.DarkGreen;
                    return;
                }

                string phong = cbPhong?.Text?.Trim() ?? "";
                string ca = cbCa?.Text?.Trim() ?? "";
                DateTime ngay = dtNgay?.Value ?? DateTime.Now;

                if (string.IsNullOrWhiteSpace(phong))
                {
                    lblPhongCaStatus.Text = "";
                    return;
                }

                using (var cn = new SqlConnection(CONN))
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();

                    // If a specific ca is selected, check that slot
                    if (!string.IsNullOrWhiteSpace(ca))
                    {
                        cmd.CommandText = @"SELECT COUNT(*) FROM DatTiec WHERE NgayDatNgay = @ngay AND Phong = @phong AND Ca = @ca";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                        cmd.Parameters.AddWithValue("@phong", phong);
                        cmd.Parameters.AddWithValue("@ca", ca);
                        int n = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        if (n > 0)
                        {
                            lblPhongCaStatus.Text = "Đã đặt";
                            lblPhongCaStatus.ForeColor = Color.Red;
                        }
                        else
                        {
                            lblPhongCaStatus.Text = "Còn";
                            lblPhongCaStatus.ForeColor = Color.DarkGreen;
                        }
                        return;
                    }

                    // No specific ca selected -> check how many ca already booked for this room on the date
                    // Use the items available in cbCa as the set of possible shifts
                    int totalCa = (cbCa != null) ? cbCa.Items.Count : 0;
                    if (totalCa <= 0)
                    {
                        // fallback: check any booking exists
                        cmd.CommandText = @"SELECT COUNT(*) FROM DatTiec WHERE NgayDatNgay = @ngay AND Phong = @phong";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                        cmd.Parameters.AddWithValue("@phong", phong);
                        int any = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                        lblPhongCaStatus.Text = any > 0 ? "Phòng có lịch" : "Phòng còn";
                        lblPhongCaStatus.ForeColor = any > 0 ? Color.OrangeRed : Color.DarkGreen;
                        return;
                    }

                    // Count distinct booked shifts for that room/date
                    cmd.CommandText = @"SELECT COUNT(DISTINCT Ca) FROM DatTiec WHERE NgayDatNgay = @ngay AND Phong = @phong";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                    cmd.Parameters.AddWithValue("@phong", phong);
                    int booked = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);

                    if (booked >= totalCa)
                    {
                        lblPhongCaStatus.Text = "Đã đặt hết";
                        lblPhongCaStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblPhongCaStatus.Text = $"Còn {totalCa - booked} ca";
                        lblPhongCaStatus.ForeColor = Color.DarkGreen;
                    }
                }
            }
            catch
            {
                // ignore errors; keep label empty
                try { lblPhongCaStatus.Text = ""; } catch { }
            }
        }

        private void btnTaoPhieu_Click_1(object sender, EventArgs e)
        {

        }
    }
}
