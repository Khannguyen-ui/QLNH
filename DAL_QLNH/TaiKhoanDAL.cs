using System;
using System.Data.SqlClient;
using DTO_QLNH;

namespace DAL_QLNH
{
    public class TaiKhoanDAL
    {
        private static readonly string connectionString =
            "Data Source=LAPTOP-KRERKDGK\\SQLEXPRESS02;Initial Catalog=QLNHS;Integrated Security=True;TrustServerCertificate=True";

        // Đăng nhập Admin / Quản lý (bảng TaiKhoan)
        // Trả về true nếu đúng user/pass. Gửi ngược ra vaiTro và hoTen.
        public static bool KiemTraDangNhap(TaiKhoan tk, out string vaiTro, out string hoTen)
        {
            vaiTro = "";
            hoTen = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT VaiTro, TenDangNhap AS HoTen
                FROM TaiKhoan
                WHERE TenDangNhap = @user AND MatKhau = @pass", conn))
            {
                cmd.Parameters.AddWithValue("@user", tk.TenDangNhap);
                cmd.Parameters.AddWithValue("@pass", tk.MatKhau);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vaiTro = reader["VaiTro"] == DBNull.Value ? "" : reader["VaiTro"].ToString();
                        hoTen = reader["HoTen"] == DBNull.Value ? "" : reader["HoTen"].ToString();
                        return true;
                    }
                }
            }

            return false;
        }

        // Lấy mật khẩu (để autofill) — chỉ dùng trong môi trường dev. Không nên lưu/hiển thị mật khẩu plaintext trong sản phẩm thực tế.
        public static string GetPasswordByUser(string tenDangNhap)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap)) return null;

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"SELECT MatKhau FROM TaiKhoan WHERE TenDangNhap = @user", conn))
            {
                cmd.Parameters.AddWithValue("@user", tenDangNhap.Trim());
                conn.Open();
                var obj = cmd.ExecuteScalar();
                if (obj == null || obj == DBNull.Value) return null;
                return obj.ToString();
            }
        }
    }
}
