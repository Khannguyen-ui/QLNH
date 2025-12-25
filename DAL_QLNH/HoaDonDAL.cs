using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL_QLNH
{
    public class HoaDonDAL
    {
        private const string CONN =
            "Server=VANKHAN;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";

        // ================== TÌM KIẾM & CHI TIẾT ==================
        public DataTable Search(string keyword, DateTime? from, DateTime? to)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT MaHD, SoPhieu, NgayTT, TongTam, GiamPT, ThanhTien, TongCuoi, NVTT, MaNV, TrangThai
FROM HoaDon
WHERE (@kw = N'' OR SoPhieu LIKE N'%'+@kw+N'%' OR MaNV LIKE N'%'+@kw+N'%' OR NVTT LIKE N'%'+@kw+N'%')
  AND (@from IS NULL OR (NgayTT IS NOT NULL AND CONVERT(date, NgayTT) >= @from))
  AND (@to   IS NULL OR (NgayTT IS NOT NULL AND CONVERT(date, NgayTT) <= @to))
ORDER BY CAST(ISNULL(NgayTT,'1900-01-01') AS datetime) DESC, MaHD DESC;";
                cmd.Parameters.AddWithValue("@kw", (keyword ?? "").Trim());
                cmd.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);

                var da = new SqlDataAdapter(cmd);
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        public DataTable GetChiTietHoaDon(string soPhieu)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT ct.SoPhieu, ct.MaTD, td.TenMon, td.DVT,
       ct.SoLuong, ct.GiaBan, (ct.SoLuong * ct.GiaBan) AS ThanhTien
FROM   CTDatTiec ct
JOIN   ThucDon td ON td.MaTD = ct.MaTD
WHERE  ct.SoPhieu = @sp
ORDER BY td.TenMon;";
                cmd.Parameters.AddWithValue("@sp", soPhieu);

                var da = new SqlDataAdapter(cmd);
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        // ================== DANH SÁCH CHỜ THANH TOÁN ==================
        // Lấy từ DatTiec để cả các phiếu CHO_TT chưa có bản ghi HoaDon cũng xuất hiện
        public DataTable ListChoThanhToan(string keyword, DateTime? from, DateTime? to)
        {
            using (var cn = new SqlConnection(CONN))
            using (var da = new SqlDataAdapter(@"
SELECT 
    hd.MaHD,
    d.SoPhieu,
    hd.NgayTT,
    hd.GiamPT,
    hd.TongTam,
    hd.ThanhTien,
    hd.TongCuoi,
    hd.NVTT,
    hd.MaNV,
    hd.TrangThai     AS TrangThaiHoaDon,
    d.NgayDK,
    d.Phong,
    d.Ca,
    d.TrangThai      AS TrangThaiPhieu,
    tk.TenTK,
    nv.TENNV         AS NhanVienLap
FROM DatTiec d
LEFT JOIN HoaDon    hd ON hd.SoPhieu = d.SoPhieu
LEFT JOIN ThucKhach tk ON tk.MaTK    = d.MaTK
LEFT JOIN NhanVien  nv ON nv.MaNV    = d.MaNV
WHERE (
        @q = N'' 
        OR d.SoPhieu LIKE N'%'+@q+N'%'
        OR ISNULL(hd.SoPhieu,N'') LIKE N'%'+@q+N'%'
        OR ISNULL(hd.MaHD,N'')    LIKE N'%'+@q+N'%'
        OR ISNULL(tk.TenTK,N'')   LIKE N'%'+@q+N'%'
        OR ISNULL(hd.NVTT,N'')    LIKE N'%'+@q+N'%'
      )
  -- Lọc ngày chỉ áp cho những dòng có NgayTT; nếu NULL vẫn giữ (phiếu chưa lập hóa đơn)
  AND (@from IS NULL OR hd.NgayTT IS NULL OR CONVERT(date, hd.NgayTT) >= @from)
  AND (@to   IS NULL OR hd.NgayTT IS NULL OR CONVERT(date, hd.NgayTT) < DATEADD(day,1,@to))
ORDER BY 
  CAST(COALESCE(hd.NgayTT, d.NgayDK, '1900-01-01') AS datetime) DESC,
  ISNULL(hd.MaHD, N'') DESC;", cn))
            {
                da.SelectCommand.Parameters.AddWithValue("@q", (keyword ?? "").Trim());
                da.SelectCommand.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                da.SelectCommand.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);

                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        // ================== LẤY/MỞ HÓA ĐƠN THEO SỐ PHIẾU ==================
        public DataTable GetBySoPhieu(string soPhieu)
        {
            using (var cn = new SqlConnection(CONN))
            using (var da = new SqlDataAdapter(@"
SELECT TOP 1 *
FROM HoaDon
WHERE SoPhieu = @sp
ORDER BY NgayTT DESC;", cn))
            {
                da.SelectCommand.Parameters.AddWithValue("@sp", soPhieu);
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }

        // Bảo đảm có hoá đơn UNPAID cho SoPhieu (trả về MaHD)
        public string EnsureUnpaidHoaDon(string soPhieu, decimal giamPT, string nvtt)
        {
            var exist = GetBySoPhieu(soPhieu);
            if (exist.Rows.Count > 0) return Convert.ToString(exist.Rows[0]["MaHD"]);
            return CreateFromSoPhieu(soPhieu, giamPT, nvtt);
        }

        // ================== TẠO HÓA ĐƠN TỪ SỐ PHIẾU ==================
        public string CreateFromSoPhieu(string soPhieu, decimal giamPT, string nvtt)
        {
            // 1) Ưu tiên stored procedure nếu DB đã có
            using (var cn = new SqlConnection(CONN))
            using (var chk = new SqlCommand(@"
SELECT 1 FROM sys.objects 
WHERE object_id = OBJECT_ID(N'dbo.sp_HD_CreateFromSoPhieu') 
  AND type = N'P';", cn))
            {
                cn.Open();
                var ok = chk.ExecuteScalar();
                if (ok != null)
                {
                    using (var cmd = new SqlCommand("dbo.sp_HD_CreateFromSoPhieu", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                        cmd.Parameters.AddWithValue("@GiamPT", giamPT);
                        var nvVal = string.IsNullOrWhiteSpace(nvtt) ? "" : nvtt.Trim();
                        cmd.Parameters.AddWithValue("@NVTT", nvVal);

                        var pOut = new SqlParameter("@MaHD", SqlDbType.NVarChar, 20)
                        { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(pOut);
                        cmd.ExecuteNonQuery();
                        return Convert.ToString(pOut.Value ?? "");
                    }
                }
            }

            // 2) Fallback: tự tính và chèn trong transaction
            using (var cn = new SqlConnection(CONN))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        decimal tongTam = 0m;
                        using (var cmd = new SqlCommand(@"
SELECT SUM(ISNULL(SoLuong,0) * ISNULL(GiaBan,0)) 
FROM CTDatTiec WHERE SoPhieu = @sp;", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@sp", soPhieu);
                            var o = cmd.ExecuteScalar();
                            tongTam = (o == null || o == DBNull.Value) ? 0m : Convert.ToDecimal(o);
                        }

                        var g = (giamPT < 0 ? 0 : (giamPT > 100 ? 100 : giamPT));
                        decimal thanhTien = Math.Round(tongTam * (1 - g / 100m), 2);

                        string maHD = "HD" + DateTime.Now.ToString("yyMMddHHmmssff");

                        using (var ins = new SqlCommand(@"
INSERT INTO HoaDon(MaHD, SoPhieu, NgayTT, GiamPT, TongTam, ThanhTien, NVTT, TrangThai)
VALUES(@MaHD, @SoPhieu, GETDATE(), @Giam, @TongTam, @Thanh, @NVTT, N'UNPAID');", cn, tx))
                        {
                            ins.Parameters.AddWithValue("@MaHD", maHD);
                            ins.Parameters.AddWithValue("@SoPhieu", soPhieu);
                            ins.Parameters.AddWithValue("@Giam", giamPT);
                            ins.Parameters.AddWithValue("@TongTam", tongTam);
                            ins.Parameters.AddWithValue("@Thanh", thanhTien);
                            var nvVal2 = string.IsNullOrWhiteSpace(nvtt) ? "" : nvtt.Trim();
                            ins.Parameters.AddWithValue("@NVTT", nvVal2);
                            ins.ExecuteNonQuery();
                        }

                        using (var up = new SqlCommand(
                            @"UPDATE DatTiec SET TrangThai = N'CHO_TT' WHERE SoPhieu = @sp;", cn, tx))
                        {
                            up.Parameters.AddWithValue("@sp", soPhieu);
                            up.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return maHD;
                    }
                    catch
                    {
                        try { tx.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        // ================== ĐÁNH DẤU ĐÃ THANH TOÁN ==================
        public int SetPaid(string maHD)
        {
            // 1) Dùng proc nếu có
            using (var cn = new SqlConnection(CONN))
            using (var chk = new SqlCommand(
                @"SELECT 1 FROM sys.objects WHERE object_id=OBJECT_ID(N'dbo.sp_HD_SetPaid') AND type=N'P';", cn))
            {
                cn.Open();
                var ok = chk.ExecuteScalar();
                if (ok != null)
                {
                    using (var cmd = new SqlCommand("dbo.sp_HD_SetPaid", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaHD", maHD);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }

            // 2) Fallback: cập nhật trong transaction
            using (var cn = new SqlConnection(CONN))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        string soPhieu = null;
                        using (var get = new SqlCommand("SELECT SoPhieu FROM HoaDon WHERE MaHD=@id;", cn, tx))
                        {
                            get.Parameters.AddWithValue("@id", maHD);
                            soPhieu = Convert.ToString(get.ExecuteScalar() ?? "");
                        }
                        if (string.IsNullOrWhiteSpace(soPhieu))
                            throw new Exception("Không tìm thấy hóa đơn.");

                        int a1, a2;

                        using (var up1 = new SqlCommand(@"
UPDATE HoaDon 
SET TrangThai = N'PAID', NgayTT = GETDATE() 
WHERE MaHD = @id;", cn, tx))
                        {
                            up1.Parameters.AddWithValue("@id", maHD);
                            a1 = up1.ExecuteNonQuery();
                        }

                        using (var up2 = new SqlCommand(@"
UPDATE DatTiec 
SET TrangThai = N'DA_TT' 
WHERE SoPhieu = @sp;", cn, tx))
                        {
                            up2.Parameters.AddWithValue("@sp", soPhieu);
                            a2 = up2.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return a1 + a2;
                    }
                    catch
                    {
                        try { tx.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        // ===== TOP MÓN =====
        /// <summary>
        /// Trả về danh sách món (TenMon) và tổng SoLuong trong khoảng ngày đặt (DatTiec.NgayDK).
        /// </summary>
        public DataTable GetTopMon(DateTime? from, DateTime? to, int top = 10)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT TOP(@top) td.TenMon AS TenMon, SUM(ISNULL(ct.SoLuong,0)) AS SoLuong
FROM CTDatTiec ct
JOIN ThucDon td ON td.MaTD = ct.MaTD
JOIN DatTiec d ON d.SoPhieu = ct.SoPhieu
WHERE (@from IS NULL OR CONVERT(date, d.NgayDK) >= @from)
  AND (@to IS NULL OR CONVERT(date, d.NgayDK) <= @to)
GROUP BY td.TenMon
ORDER BY SUM(ISNULL(ct.SoLuong,0)) DESC;";
                cmd.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@top", top);

                var da = new SqlDataAdapter(cmd);
                var tb = new DataTable();
                da.Fill(tb);
                return tb;
            }
        }
    }
}
