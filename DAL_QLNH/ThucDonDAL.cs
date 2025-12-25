using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_QLNH;

namespace DAL_QLNH
{
    public class ThucDonDAL
    {
        private static readonly string connectionString =
            @"Data Source=VANKHAN;Initial Catalog=QLNHS;Integrated Security=True;TrustServerCertificate=True";

        private static SqlConnection Conn() => new SqlConnection(connectionString);

        // ===== Helpers =====
        private static ThucDon Map(SqlDataReader rd)
        {
            var td = new ThucDon();

            td.MaTD = rd["MATD"] != DBNull.Value ? rd["MATD"].ToString().Trim() : string.Empty;
            td.TenMon = rd["TENMON"] != DBNull.Value ? rd["TENMON"].ToString() : string.Empty;
            td.DVT = rd["DVT"] != DBNull.Value ? rd["DVT"].ToString() : string.Empty;
            td.GiaTien = rd["GIATIEN"] != DBNull.Value ? Convert.ToSingle(rd["GIATIEN"]) : 0f;
            td.GhiChu = rd["GHICHU"] != DBNull.Value ? rd["GHICHU"].ToString() : null;
            td.SoLuongTon = rd["SoLuongTon"] != DBNull.Value ? Convert.ToInt32(rd["SoLuongTon"]) : 0;

            return td;
        }

        // ===== GET ALL =====
        public static List<ThucDon> GetAll()
        {
            var list = new List<ThucDon>();
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucDon_GetAll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read()) list.Add(Map(rd));
                }
            }
            return list;
        }

        // ===== SEARCH (SP) =====
        public static List<ThucDon> Search(string keyword)
        {
            var list = new List<ThucDon>();
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucDon_Search", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@keyword", (object)(keyword ?? string.Empty));
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read()) list.Add(Map(rd));
                }
            }
            return list;
        }

        // ===== INSERT =====
        public static bool Insert(ThucDon td)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucDon_Insert", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matd", td.MaTD ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@tenmon", td.TenMon ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dvt", (object)td.DVT ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@giatien", (object)td.GiaTien);
                cmd.Parameters.AddWithValue("@ghichu", (object)td.GhiChu ?? DBNull.Value);
                // n?u SP h? tr? SoLuongTon, truy?n thêm (fallback0)
                cmd.Parameters.AddWithValue("@soluongton", td.SoLuongTon);

                con.Open();
                object x = cmd.ExecuteScalar(); // SELECT @@ROWCOUNT AS Affected
                return x != null && Convert.ToInt32(x) > 0;
            }
        }

        // ===== UPDATE =====
        public static bool Update(ThucDon td)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucDon_Update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matd", td.MaTD ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@tenmon", td.TenMon ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dvt", (object)td.DVT ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@giatien", (object)td.GiaTien);
                cmd.Parameters.AddWithValue("@ghichu", (object)td.GhiChu ?? DBNull.Value);
                // n?u SP h? tr? SoLuongTon, truy?n thêm
                cmd.Parameters.AddWithValue("@soluongton", td.SoLuongTon);

                con.Open();
                object x = cmd.ExecuteScalar();
                return x != null && Convert.ToInt32(x) > 0;
            }
        }

        // ===== DELETE =====
        public static bool Delete(string maTD)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucDon_Delete", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matd", maTD ?? string.Empty);

                con.Open();
                object x = cmd.ExecuteScalar();
                return x != null && Convert.ToInt32(x) > 0;
            }
        }

        // Gi?m t?n kho: tr? v? true n?u thành công (?? s? l??ng), false n?u không ?? ho?c l?i
        public static bool DecreaseStock(SqlConnection cn, SqlTransaction tx, string maTD, int qty)
        {
            using (var cmd = new SqlCommand(@"
 UPDATE ThucDon
 SET SoLuongTon = SoLuongTon - @qty
 WHERE MaTD = @ma AND SoLuongTon >= @qty", cn, tx))
            {
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@ma", maTD);
                int affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }

        // Update stock standalone (admin UI)
        public static bool UpdateStock(string maTD, int newQty)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand(@"UPDATE ThucDon SET SoLuongTon = @q WHERE MaTD = @ma", con))
            {
                cmd.Parameters.AddWithValue("@q", newQty);
                cmd.Parameters.AddWithValue("@ma", maTD);
                con.Open();
                int affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }
    }
}
