using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace GUI_QLNH
{
    public partial class FormThanhToanHoaDon : Form
    {
        private const string CONN =
            "Server=LAPTOP-KRERKDGK\\SQLEXPRESS02;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";

        public string SoPhieu { get; private set; }
        /// <summary> Mã NV đang đăng nhập/thanh toán (ghi vào NVTT hoặc MaNV nếu có cột) </summary>
        public string MaNV { get; private set; }
        public decimal GiamMacDinh { get; private set; }

        // trả về MaHD (nếu tạo) và flag IsPaid để caller biết
        private string _maHD = "";
        private bool _isPaid = false;
        public string MaHD => _maHD;
        public bool IsPaid => _isPaid;

        private bool _readOnly = false;
        public bool ReadOnlyMode
        {
            get => _readOnly;
            set { _readOnly = value; ApplyReadOnly(); }
        }

        public FormThanhToanHoaDon()
        {
            InitializeComponent();
            if (dgvCT != null) dgvCT.AutoGenerateColumns = false;
        }

        public FormThanhToanHoaDon(string soPhieu, string maNV, decimal giamPT = 0m) : this()
        {
            SoPhieu = soPhieu;
            MaNV = maNV;
            GiamMacDinh = Math.Max(0, Math.Min(100, giamPT));
        }

        private void FormHoaDon_Load(object sender, EventArgs e)
        {
            dtNgayTT.Value = DateTime.Now;
            txtSoPhieu.Text = SoPhieu ?? "";

            if (!string.IsNullOrWhiteSpace(SoPhieu))
            {
                LoadThongTin();
                LoadChiTiet();
            }

            numGiamGia.Value = GiamMacDinh;
            UpdateTong();
        }

        private void ApplyReadOnly()
        {
            bool canEdit = !_readOnly;
            numGiamGia.Enabled = canEdit;
            numKhachDua.Enabled = canEdit;
            btnThanhToan.Enabled = canEdit;
        }

        // =====================================================
        // =============== LOAD DỮ LIỆU HÓA ĐƠN ================
        // =====================================================
        private void LoadThongTin()
        {
            try
            {
                // (1) Thông tin phiếu + khách (theo DatTiec)
                using (var cn = new SqlConnection(CONN))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT dt.SoPhieu, dt.NgayDK, dt.MaTK, dt.MaNV, ISNULL(dt.SoLuongKhach,0) AS SoLuongKhach,
       ISNULL(dt.Phong,'') AS Phong,
       tk.TenTK, ISNULL(tk.SoDT,'') AS SoDT, ISNULL(tk.DiaChi,'') AS DiaChi,
       nv.TenNV
FROM   DatTiec dt
LEFT JOIN ThucKhach tk ON tk.MaTK = dt.MaTK
LEFT JOIN NhanVien nv  ON nv.MaNV = dt.MaNV
WHERE  dt.SoPhieu = @sp";
                    cmd.Parameters.AddWithValue("@sp", SoPhieu);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            txtKhach.Text = Convert.ToString(rd["TenTK"] ?? "");
                            txtSDT.Text = Convert.ToString(rd["SoDT"] ?? "");
                            txtDiaChi.Text = Convert.ToString(rd["DiaChi"] ?? "");
                            txtNhanVien.Text = Convert.ToString(rd["TenNV"] ?? ""); // nhân viên lập phiếu đặt tiệc
                            txtPhong.Text = Convert.ToString(rd["Phong"] ?? "");
                            txtSLKhach.Text = Convert.ToString(rd["SoLuongKhach"] ?? "0");
                        }
                    }
                }

                // (2) Nếu hóa đơn đã tồn tại → đọc thông tin hóa đơn
                if (HoaDonDaTonTai(SoPhieu))
                {
                    ReadOnlyMode = true;

                    bool hasDaNhan = ColExists("HoaDon", "DaNhan");
                    bool hasTienThoi = ColExists("HoaDon", "TienThoi");
                    bool hasThanhTien = ColExists("HoaDon", "ThanhTien");

                    // Không có TongCuoi ở DB của bạn → không select.
                    string sql = "SELECT NgayTT, GiamPT, TongTam";
                    if (hasThanhTien) sql += ", ThanhTien";
                    if (hasDaNhan) sql += ", DaNhan";
                    if (hasTienThoi) sql += ", TienThoi";
                    sql += " FROM HoaDon WHERE SoPhieu=@sp";

                    using (var cn = new SqlConnection(CONN))
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@sp", SoPhieu);
                        cn.Open();
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                if (DateTime.TryParse(Convert.ToString(rd["NgayTT"]), out DateTime ngay))
                                    dtNgayTT.Value = ngay;
                                if (decimal.TryParse(Convert.ToString(rd["GiamPT"]), out decimal gpt))
                                    numGiamGia.Value = Math.Max(0, Math.Min(100, gpt));

                                decimal tongTam = AsDec(rd["TongTam"], 0m);
                                txtTongTam.Text = tongTam.ToString("n0");

                                // Phải trả: ưu tiên số đã chốt ở ThanhTien; nếu chưa có thì tính từ TongTam & % giảm.
                                decimal phaiTra;
                                if (hasThanhTien)
                                {
                                    var tt = AsDec(rd["ThanhTien"], -1m);
                                    phaiTra = (tt >= 0m)
                                        ? tt
                                        : Math.Round(tongTam * (1 - numGiamGia.Value / 100m), 0, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    phaiTra = Math.Round(tongTam * (1 - numGiamGia.Value / 100m), 0, MidpointRounding.AwayFromZero);
                                }
                                txtPhaiTra.Text = phaiTra.ToString("n0");

                                if (hasDaNhan)
                                    numKhachDua.Value = AsDec(rd["DaNhan"], 0m);
                                else
                                    numKhachDua.Value = 0;

                                if (hasTienThoi)
                                    txtTienThoi.Text = ToMoney(rd["TienThoi"]);
                                else
                                    txtTienThoi.Text = "0";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTiet()
        {
            try
            {
                using (var cn = new SqlConnection(CONN))
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT ct.MaTD, td.TenMon, td.DVT, ct.SoLuong, ct.GiaBan,
       (ct.SoLuong * ct.GiaBan) AS ThanhTien
FROM   CTDatTiec ct
JOIN   ThucDon td ON td.MaTD = ct.MaTD
WHERE  ct.SoPhieu = @sp
ORDER BY td.TenMon";
                    cmd.Parameters.AddWithValue("@sp", SoPhieu);

                    var da = new SqlDataAdapter(cmd);
                    var tb = new DataTable();
                    da.Fill(tb);

                    if (tb.Columns.Contains("ThanhTien"))
                        tb.Columns["ThanhTien"].ReadOnly = true;

                    dgvCT.DataSource = tb;
                }
                UpdateTong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // =================== TÍNH TIỀN =======================
        // =====================================================
        private void UpdateTong()
        {
            decimal tongTam = 0m;
            if (dgvCT?.DataSource is DataTable tb)
            {
                foreach (DataRow r in tb.Rows)
                    tongTam += AsDec(r["ThanhTien"], 0m);
            }
            txtTongTam.Text = tongTam.ToString("n0");

            decimal giam = numGiamGia.Value;
            decimal phaiTra = Math.Round(
                tongTam * (1 - giam / 100m),
                0,
                MidpointRounding.AwayFromZero);
            txtPhaiTra.Text = phaiTra.ToString("n0");

            decimal khachDua = numKhachDua.Value;
            decimal thoi = khachDua - phaiTra;
            txtTienThoi.Text = (thoi < 0 ? 0 : thoi).ToString("n0");
        }

        private void numGiamGia_ValueChanged(object sender, EventArgs e) => UpdateTong();
        private void numKhachDua_ValueChanged(object sender, EventArgs e) => UpdateTong();

        // =====================================================
        // ================= THANH TOÁN =======================
        // =====================================================
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SoPhieu))
            {
                MessageBox.Show("Chưa có Số phiếu.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (HoaDonDaTonTai(SoPhieu))
            {
                MessageBox.Show("Phiếu này đã thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReadOnlyMode = true;
                return;
            }

            decimal tongTam = ParseMoney(txtTongTam.Text);
            decimal phaiTra = ParseMoney(txtPhaiTra.Text);
            decimal giamPT = numGiamGia.Value;
            decimal khachDua = numKhachDua.Value;
            decimal tienThoi = Math.Max(0, khachDua - phaiTra);

            using (var cn = new SqlConnection(CONN))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        bool hasDaNhan = ColExists(cn, tx, "HoaDon", "DaNhan");
                        bool hasTienThoi = ColExists(cn, tx, "HoaDon", "TienThoi");

                        // Xác định cột nhân viên để ghi: ưu tiên NVTT, sau đó MaNV; nếu không có thì không ghi.
                        string empCol = null;
                        if (ColExists(cn, tx, "HoaDon", "NVTT")) empCol = "NVTT";
                        else if (ColExists(cn, tx, "HoaDon", "MaNV")) empCol = "MaNV";

                        var cols = "SoPhieu, NgayTT, TongTam, GiamPT";
                        var vals = "@SoPhieu, @NgayTT, @TongTam, @GiamPT";

                        if (hasDaNhan) { cols += ", DaNhan"; vals += ", @DaNhan"; }
                        if (hasTienThoi) { cols += ", TienThoi"; vals += ", @TienThoi"; }
                        if (!string.IsNullOrEmpty(empCol)) { cols += ", " + empCol; vals += ", @Emp"; }

                        if (ColExists(cn, tx, "HoaDon", "ThanhTien"))
                        {
                            cols += ", ThanhTien";
                            vals += ", @ThanhTien";
                        }

                        string sql = $"INSERT INTO HoaDon({cols}) VALUES({vals})";

                        using (var cmd = new SqlCommand(sql, cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@SoPhieu", SoPhieu);
                            cmd.Parameters.AddWithValue("@NgayTT", dtNgayTT.Value);
                            cmd.Parameters.AddWithValue("@TongTam", tongTam);
                            cmd.Parameters.AddWithValue("@GiamPT", giamPT);

                            if (hasDaNhan) cmd.Parameters.AddWithValue("@DaNhan", khachDua);
                            if (hasTienThoi) cmd.Parameters.AddWithValue("@TienThoi", tienThoi);
                            if (!string.IsNullOrEmpty(empCol))
                                cmd.Parameters.AddWithValue("@Emp",
                                    (object)(MaNV ?? (string)null) ?? DBNull.Value);

                            if (ColExists(cn, tx, "HoaDon", "ThanhTien"))
                                cmd.Parameters.AddWithValue("@ThanhTien", phaiTra);

                            cmd.ExecuteNonQuery();
                        }

                        // Lấy MaHD vừa tạo (top1 theo NgayTT)
                        using (var get = new SqlCommand(@"SELECT TOP(1) MaHD FROM HoaDon WHERE SoPhieu=@sp ORDER BY NgayTT DESC, MaHD DESC", cn, tx))
                        {
                            get.Parameters.AddWithValue("@sp", SoPhieu);
                            var o = get.ExecuteScalar();
                            _maHD = (o == null || o == DBNull.Value) ? "" : Convert.ToString(o);
                        }

                        // Cập nhật trạng thái DatTiec = DA_TT
                        using (var up = new SqlCommand(@"UPDATE DatTiec SET TrangThai = N'DA_TT' WHERE SoPhieu = @sp;", cn, tx))
                        {
                            up.Parameters.AddWithValue("@sp", SoPhieu);
                            up.ExecuteNonQuery();
                        }

                        tx.Commit();
                        _isPaid = true;
                        MessageBox.Show("Thanh toán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ReadOnlyMode = true;
                    }
                    catch (Exception ex)
                    {
                        try { tx.Rollback(); } catch { }
                        MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // =====================================================
        // ================== IN HÓA ĐƠN ======================
        // =====================================================
        private void btnIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SoPhieu)) return;
            MessageBox.Show("Giả lập in hoá đơn " + SoPhieu + ". Gắn RDLC/Crystal sau.",
                "In hoá đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =====================================================
        // ================= XUẤT EXCEL =======================
        // =====================================================
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvCT == null || dgvCT.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu chi tiết để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                string soPhieuNow = (txtSoPhieu?.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(soPhieuNow))
                    soPhieuNow = "HoaDon";

                sfd.Title = "Lưu file Excel";
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = "HoaDon_" + soPhieuNow + ".xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                ExportToExcel(filePath);

                MessageBox.Show("Đã xuất file:\n" + filePath,
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Ghi file Excel bằng CSV (tab-separated) thay vì dùng Interop để tránh phụ thuộc COM ở thiết kế.
        /// </summary>
        private void ExportToExcel(string filePath)
        {
            try
            {
                using (var sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    // Header
                    sw.WriteLine("HÓA ĐƠN");
                    sw.WriteLine();

                    sw.WriteLine($"Số phiếu:\t{txtSoPhieu.Text}\tNgày TT:\t{dtNgayTT.Value:dd/MM/yyyy}");
                    sw.WriteLine($"Khách hàng:\t{txtKhach.Text}\tSĐT:\t{txtSDT.Text}");
                    sw.WriteLine($"Địa chỉ:\t{txtDiaChi.Text}");
                    sw.WriteLine($"Nhân viên:\t{txtNhanVien.Text}\tPhòng:\t{txtPhong.Text}");
                    sw.WriteLine($"Số lượng KH:\t{txtSLKhach.Text}");
                    sw.WriteLine();

                    // Table header
                    sw.WriteLine("Mã\tTên món\tĐVT\tSL\tGiá\tThành tiền");

                    DataTable tb = dgvCT.DataSource as DataTable;
                    if (tb != null)
                    {
                        foreach (DataRow dr in tb.Rows)
                        {
                            var ma = dr["MaTD"]?.ToString() ?? "";
                            var ten = dr["TenMon"]?.ToString() ?? "";
                            var dvt = dr["DVT"]?.ToString() ?? "";
                            var sl = dr["SoLuong"]?.ToString() ?? "";
                            var gia = dr["GiaBan"]?.ToString() ?? "";
                            var tt = dr.Table.Columns.Contains("ThanhTien") ? dr["ThanhTien"]?.ToString() ?? "" : "";
                            sw.WriteLine($"{ma}\t{ten}\t{dvt}\t{sl}\t{gia}\t{tt}");
                        }
                    }
                    else
                    {
                        foreach (DataGridViewRow gr in dgvCT.Rows)
                        {
                            if (gr.IsNewRow) continue;
                            var ma = gr.Cells["colMaTD"].Value?.ToString() ?? "";
                            var ten = gr.Cells["colTenMon"].Value?.ToString() ?? "";
                            var dvt = gr.Cells["colDVT"].Value?.ToString() ?? "";
                            var sl = gr.Cells["colSL"].Value?.ToString() ?? "";
                            var gia = gr.Cells["colGia"].Value?.ToString() ?? "";
                            var tt = gr.Cells["colThanhTien"].Value?.ToString() ?? "";
                            sw.WriteLine($"{ma}\t{ten}\t{dvt}\t{sl}\t{gia}\t{tt}");
                        }
                    }

                    sw.WriteLine();
                    sw.WriteLine($"\t\t\tTổng tạm:\t{txtTongTam.Text}");
                    sw.WriteLine($"\t\t\tGiảm giá (%):\t{numGiamGia.Value}");
                    sw.WriteLine($"\t\t\tPhải trả:\t{txtPhaiTra.Text}");
                    sw.WriteLine($"\t\t\tKhách đưa:\t{numKhachDua.Value:n0}");
                    sw.WriteLine($"\t\t\tTiền thối:\t{txtTienThoi.Text}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất file thất bại:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // =================== KHÁC ===========================
        // =====================================================
        private void btnDong_Click(object sender, EventArgs e) => Close();

        private void txtSoPhieu_Leave(object sender, EventArgs e)
        {
            string newSP = (txtSoPhieu.Text ?? "").Trim();
            if (!string.Equals(newSP, SoPhieu ?? "", StringComparison.OrdinalIgnoreCase))
            {
                SoPhieu = newSP;
                if (!string.IsNullOrWhiteSpace(SoPhieu))
                {
                    LoadThongTin();
                    LoadChiTiet();
                }
            }
        }

        // =====================================================
        // =================== TIỆN ÍCH =======================
        // =====================================================
        private static decimal AsDec(object v, decimal def = 0m)
        {
            if (v == null || v == DBNull.Value) return def;
            return decimal.TryParse(Convert.ToString(v), out decimal d) ? d : def;
        }

        private static bool ColExists(SqlConnection cn, SqlTransaction tx, string table, string col)
        {
            using (var cmd = new SqlCommand(
                "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@t AND COLUMN_NAME=@c",
                cn, tx))
            {
                cmd.Parameters.AddWithValue("@t", table);
                cmd.Parameters.AddWithValue("@c", col);
                return cmd.ExecuteScalar() != null;
            }
        }

        private static bool ColExists(string table, string col)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = new SqlCommand(
                "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@t AND COLUMN_NAME=@c", cn))
            {
                cmd.Parameters.AddWithValue("@t", table);
                cmd.Parameters.AddWithValue("@c", col);
                cn.Open();
                return cmd.ExecuteScalar() != null;
            }
        }

        private bool HoaDonDaTonTai(string soPhieu)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(1) FROM HoaDon WHERE SoPhieu=@sp";
                cmd.Parameters.AddWithValue("@sp", soPhieu);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private static string ToMoney(object v)
        {
            decimal.TryParse(Convert.ToString(v),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.CurrentCulture, out decimal d);
            return d.ToString("n0");
        }

        private static decimal ParseMoney(string s)
        {
            if (decimal.TryParse(
                s,
                System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.CurrentCulture,
                out decimal d))
                return d;

            decimal.TryParse((s ?? "").Replace(",", ""), out d);
            return d;
        }

        private void lblSDT_Click(object sender, EventArgs e)
        {

        }

        private void txtTongTam_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
