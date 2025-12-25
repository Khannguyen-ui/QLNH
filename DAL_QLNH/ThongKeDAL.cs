using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_QLNH
{
    public class ThongKeDAL
    {
        private const string CONN =
            "Server=LAPTOP-KRERKDGK\\SQLEXPRESS02;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";

        private static DataTable Fill(SqlCommand cmd)
        {
            var da = new SqlDataAdapter(cmd);
            var tb = new DataTable();
            da.Fill(tb);
            return tb;
        }

        // Thống kê theo hóa đơn
        public DataTable TkHoaDon(DateTime? from, DateTime? to)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT hd.SoPhieu, hd.NgayTT, ISNULL(hd.TongTam,0) AS TongTam,
       ISNULL(hd.GiamPT,0) AS GiamPT, ISNULL(hd.TongCuoi,0) AS TongCuoi,
       ISNULL(dt.Phong, N'') AS MaBan
FROM   HoaDon hd
LEFT   JOIN DatTiec dt ON dt.SoPhieu = hd.SoPhieu
WHERE  (@from IS NULL OR CONVERT(date, hd.NgayTT) >= @from)
  AND  (@to   IS NULL OR CONVERT(date, hd.NgayTT) <= @to)
ORDER BY hd.NgayTT DESC, hd.SoPhieu DESC";
                cmd.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);
                return Fill(cmd);
            }
        }

        // Thống kê theo "bàn ăn" (dùng DatTiec.Phong làm mã bàn)
        public DataTable TkBanAn(DateTime? from, DateTime? to)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT
    ISNULL(dt.Phong, N'')              AS MaBanAn,
    td.TenMon,
    SUM(ISNULL(ct.SoLuong,0))          AS SoLuongMon,
    SUM(ISNULL(ct.SoLuong,0) * ISNULL(ct.GiaBan,0)) AS GiaTong,
    MIN(CONVERT(date, hd.NgayTT))      AS ThoiGian
FROM CTDatTiec ct
JOIN HoaDon   hd ON hd.SoPhieu = ct.SoPhieu
LEFT JOIN DatTiec  dt ON dt.SoPhieu = ct.SoPhieu
LEFT JOIN ThucDon  td ON td.MaTD    = ct.MaTD
WHERE  (@from IS NULL OR CONVERT(date, hd.NgayTT) >= @from)
  AND  (@to   IS NULL OR CONVERT(date, hd.NgayTT) <= @to)
GROUP BY dt.Phong, td.TenMon
ORDER BY GiaTong DESC, TenMon";
                cmd.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);
                return Fill(cmd);
            }
        }

        // Top món trong khoảng
        public DataTable TopMon(DateTime? from, DateTime? to, int topN)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
WITH X AS(
  SELECT td.TenMon,
         SUM(ISNULL(ct.SoLuong,0)) AS TongSL,
         SUM(ISNULL(ct.SoLuong,0)*ISNULL(ct.GiaBan,0)) AS DoanhThu
  FROM CTDatTiec ct
  JOIN HoaDon hd ON hd.SoPhieu = ct.SoPhieu
  LEFT JOIN ThucDon td ON td.MaTD = ct.MaTD
  WHERE (@from IS NULL OR CONVERT(date, hd.NgayTT) >= @from)
    AND (@to   IS NULL OR CONVERT(date, hd.NgayTT) <= @to)
  GROUP BY td.TenMon
)
SELECT TOP (@topN) TenMon, TongSL, DoanhThu
FROM X
ORDER BY DoanhThu DESC, TenMon ASC";
                cmd.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@topN", topN);
                return Fill(cmd);
            }
        }

        // Tổng doanh thu trong khoảng
        public decimal TongDoanhThu(DateTime? from, DateTime? to)
        {
            using (var cn = new SqlConnection(CONN))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
SELECT SUM(ISNULL(hd.TongCuoi,0))
FROM   HoaDon hd
WHERE  (@from IS NULL OR CONVERT(date, hd.NgayTT) >= @from)
  AND  (@to   IS NULL OR CONVERT(date, hd.NgayTT) <= @to)";
                cmd.Parameters.AddWithValue("@from", (object)from ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@to", (object)to ?? DBNull.Value);
                cn.Open();
                object v = cmd.ExecuteScalar();
                return v == null || v == DBNull.Value ? 0m : Convert.ToDecimal(v);
            }
        }
    }
}
