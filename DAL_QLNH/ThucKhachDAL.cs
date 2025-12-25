using DTO_QLNH;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_QLNH
{
    public class ThucKhachDAL
    {
        // Chuỗi kết nối của bạn (giữ nguyên máy bạn)
        private const string _cn =
            @"Data Source=LAPTOP-KRERKDGK\SQLEXPRESS02;Initial Catalog=QLNHS;Integrated Security=True;TrustServerCertificate=True";

        private SqlConnection Conn() => new SqlConnection(_cn);

        // ============ GET ALL ============
        public DataTable GetAll()
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucKhach_GetAll", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var tb = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(tb);
                }
                return tb;
            }
        }

        // ============ SEARCH ============
        public DataTable Search(string keyword)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucKhach_Search", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@keyword", keyword ?? string.Empty);

                var tb = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(tb);
                }
                return tb;
            }
        }

        // ============ INSERT ============
        public bool Insert(ThucKhachDTO t)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucKhach_Insert", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matk", (object)t.MaTK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tentk", (object)t.TenTK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@diachi", (object)t.DiaChi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sodt", (object)t.SoDT ?? DBNull.Value);

                con.Open();
                // SP trả SELECT @@ROWCOUNT AS Affected
                object x = cmd.ExecuteScalar();
                return x != null && Convert.ToInt32(x) > 0;
            }
        }

        // ============ UPDATE ============
        public bool Update(ThucKhachDTO t)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucKhach_Update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matk", (object)t.MaTK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@tentk", (object)t.TenTK ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@diachi", (object)t.DiaChi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sodt", (object)t.SoDT ?? DBNull.Value);

                con.Open();
                object x = cmd.ExecuteScalar();
                return x != null && Convert.ToInt32(x) > 0;
            }
        }

        // ============ DELETE ============
        public bool Delete(string ma)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucKhach_Delete", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matk", ma ?? string.Empty);

                con.Open();
                object x = cmd.ExecuteScalar();
                return x != null && Convert.ToInt32(x) > 0;
            }
        }

        // ============ EXISTS ============
        public bool Exists(string ma)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_ThucKhach_Exists", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@matk", ma ?? string.Empty);

                con.Open();
                object x = cmd.ExecuteScalar();
                return x != null && Convert.ToInt32(x) == 1;
            }
        }
    }
}
