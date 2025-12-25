using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DTO_QLNH;

namespace DAL_QLNH
{
    /// <summary>
    /// NhanVienDAL (static) – an toàn với DB hiện tại.
    /// - Hỗ trợ cả MatKhau (plain) và PasswordHash (SHA-256).
    /// - Dùng SP nếu có, không có thì chạy SQL trực tiếp.
    /// - Gán tham số SP “tùy tồn tại” để tránh lỗi khi SP chưa cập nhật.
    /// </summary>
    public static class NhanVienDAL
    {
        private static readonly string ConnStr =
            "Data Source=LAPTOP-KRERKDGK\\SQLEXPRESS02;Initial Catalog=QLNHS;Integrated Security=True;TrustServerCertificate=True";

        // ========= Helpers =========
        private static string Sha256(string s)
        {
            using (var sha = SHA256.Create())
            {
                var b = sha.ComputeHash(Encoding.UTF8.GetBytes(s ?? ""));
                var sb = new StringBuilder(b.Length * 2);
                foreach (var x in b) sb.Append(x.ToString("x2"));
                return sb.ToString();
            }
        }

        private static DateTime ClampSmallDateTime(DateTime d)
        {
            var min = new DateTime(1900, 1, 1);
            var max = new DateTime(2079, 6, 6);
            if (d < min) d = min; if (d > max) d = max;
            return d;
        }

        private static object ToDbDateOrNull(DateTime dt) =>
            dt == DateTime.MinValue ? (object)DBNull.Value : ClampSmallDateTime(dt);

        private static DateTime FromDbDateOrMin(object dbVal) =>
            dbVal == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dbVal);

        private static bool ObjectExists(SqlConnection cn, string name, string type) // type: 'P' proc, 'U' table
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = "SELECT 1 FROM sys.objects WHERE name=@n AND type=@t";
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@t", type);
                return cmd.ExecuteScalar() != null;
            }
        }

        private static bool ColumnExists(SqlConnection cn, string table, string col)
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT 1
                                FROM sys.columns c
                                WHERE c.object_id = OBJECT_ID(@tb) AND c.name = @col";
                cmd.Parameters.AddWithValue("@tb", table);
                cmd.Parameters.AddWithValue("@col", col);
                return cmd.ExecuteScalar() != null;
            }
        }

        private static bool ProcHasParam(SqlConnection cn, string procName, string paramName) // paramName: "@matkhau"
        {
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT 1
                                FROM sys.parameters
                                WHERE object_id = OBJECT_ID(@p) AND name = @n";
                cmd.Parameters.AddWithValue("@p", procName);
                cmd.Parameters.AddWithValue("@n", paramName.TrimStart('@'));
                return cmd.ExecuteScalar() != null;
            }
        }

        private static NhanVien MapNv(IDataRecord r, bool hasMatKhau)
        {
            var nv = new NhanVien
            {
                MaNV = r["MANV"].ToString(),
                TenNV = r["TENNV"] == DBNull.Value ? null : r["TENNV"].ToString(),
                NoiSinh = r["NOISINH"] == DBNull.Value ? null : r["NOISINH"].ToString(),
                NgayLamViec = FromDbDateOrMin(r["NGAYLAMVIEC"])
            };
            if (hasMatKhau)
                nv.MatKhau = r["MatKhau"] == DBNull.Value ? null : r["MatKhau"].ToString();
            return nv;
        }

        // helper to set password column (plain or hash) explicitly
        private static void SetPasswordDirect(string maNV, string matKhau, bool hasMatKhau, bool hasPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(maNV) || string.IsNullOrEmpty(matKhau)) return;

            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    if (hasMatKhau)
                    {
                        cmd.CommandText = "UPDATE dbo.NhanVien SET MatKhau=@mk WHERE MANV=@ma";
                        cmd.Parameters.AddWithValue("@mk", matKhau);
                    }
                    else if (hasPasswordHash)
                    {
                        cmd.CommandText = "UPDATE dbo.NhanVien SET PasswordHash=@h WHERE MANV=@ma";
                        cmd.Parameters.AddWithValue("@h", Sha256(matKhau));
                    }
                    else
                    {
                        return;
                    }
                    cmd.Parameters.AddWithValue("@ma", maNV.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ========= READ =========
        public static List<NhanVien> GetAll()
        {
            var list = new List<NhanVien>();
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();

                bool hasMatKhau = ColumnExists(cn, "dbo.NhanVien", "MatKhau");
                bool useSp = ObjectExists(cn, "sp_NhanVien_GetAll", "P");

                using (var cmd = cn.CreateCommand())
                {
                    if (useSp)
                    {
                        cmd.CommandText = "sp_NhanVien_GetAll";
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        cmd.CommandText = hasMatKhau
                            ? "SELECT MANV,TENNV,NOISINH,NGAYLAMVIEC,MatKhau FROM dbo.NhanVien ORDER BY MANV"
                            : "SELECT MANV,TENNV,NOISINH,NGAYLAMVIEC FROM dbo.NhanVien ORDER BY MANV";
                    }

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read()) list.Add(MapNv(r, hasMatKhau));
                    }
                }
            }
            return list;
        }

        public static NhanVien GetByMaNV(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return null;
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();

                bool hasMatKhau = ColumnExists(cn, "dbo.NhanVien", "MatKhau");

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = hasMatKhau
                        ? @"SELECT MANV,TENNV,NOISINH,NGAYLAMVIEC,MatKhau FROM dbo.NhanVien WHERE MANV=@ma"
                        : @"SELECT MANV,TENNV,NOISINH,NGAYLAMVIEC FROM dbo.NhanVien WHERE MANV=@ma";
                    cmd.Parameters.AddWithValue("@ma", maNV.Trim());

                    using (var r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        return r.Read() ? MapNv(r, hasMatKhau) : null;
                    }
                }
            }
        }

        // ========= CREATE / UPDATE / DELETE =========
        public static bool Insert(NhanVien nv)
        {
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();
                bool hasMatKhau = ColumnExists(cn, "dbo.NhanVien", "MatKhau");
                bool hasPasswordHash = ColumnExists(cn, "dbo.NhanVien", "PasswordHash");
                bool useSp = ObjectExists(cn, "sp_NhanVien_Insert", "P");

                using (var cmd = cn.CreateCommand())
                {
                    if (useSp)
                    {
                        cmd.CommandText = "sp_NhanVien_Insert";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@manv", SqlDbType.NVarChar, 10).Value = nv.MaNV?.Trim();
                        cmd.Parameters.Add("@tennv", SqlDbType.NVarChar, 50).Value = (object)nv.TenNV ?? DBNull.Value;
                        cmd.Parameters.Add("@noisinh", SqlDbType.NVarChar, 50).Value =
                            string.IsNullOrWhiteSpace(nv.NoiSinh) ? (object)DBNull.Value : nv.NoiSinh;
                        cmd.Parameters.Add("@ngaylamviec", SqlDbType.SmallDateTime).Value = ToDbDateOrNull(nv.NgayLamViec);

                        // chỉ gán nếu SP có tham số và DB có cột
                        if (hasMatKhau && ProcHasParam(cn, "sp_NhanVien_Insert", "@matkhau"))
                            cmd.Parameters.Add("@matkhau", SqlDbType.NVarChar, 100).Value =
                                (object)nv.MatKhau ?? DBNull.Value;
                        else if (!hasMatKhau && hasPasswordHash && ProcHasParam(cn, "sp_NhanVien_Insert", "@passwordhash"))
                            cmd.Parameters.Add("@passwordhash", SqlDbType.NVarChar, 256).Value = (object)Sha256(nv.MatKhau) ?? DBNull.Value;
                    }
                    else
                    {
                        if (hasMatKhau)
                        {
                            cmd.CommandText = @"INSERT INTO dbo.NhanVien(MANV,TENNV,NOISINH,NGAYLAMVIEC,MatKhau)
                                        VALUES(@manv,@tennv,@noisinh,@ngaylamviec,@matkhau)";
                            cmd.Parameters.AddWithValue("@matkhau", (object)nv.MatKhau ?? DBNull.Value);
                        }
                        else if (hasPasswordHash)
                        {
                            cmd.CommandText = @"INSERT INTO dbo.NhanVien(MANV,TENNV,NOISINH,NGAYLAMVIEC,PasswordHash)
                                        VALUES(@manv,@tennv,@noisinh,@ngaylamviec,@passwordhash)";
                            cmd.Parameters.AddWithValue("@passwordhash", (object)Sha256(nv.MatKhau) ?? DBNull.Value);
                        }
                        else
                        {
                            cmd.CommandText = @"INSERT INTO dbo.NhanVien(MANV,TENNV,NOISINH,NGAYLAMVIEC)
                                        VALUES(@manv,@tennv,@noisinh,@ngaylamviec)";
                        }

                        cmd.Parameters.AddWithValue("@manv", nv.MaNV?.Trim());
                        cmd.Parameters.AddWithValue("@tennv", (object)nv.TenNV ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@noisinh",
                            string.IsNullOrWhiteSpace(nv.NoiSinh) ? (object)DBNull.Value : nv.NoiSinh);
                        cmd.Parameters.Add("@ngaylamviec", SqlDbType.SmallDateTime).Value = ToDbDateOrNull(nv.NgayLamViec);
                    }

                    try
                    {
                        var rows = cmd.ExecuteNonQuery();
                        if (rows > 0) return true;

                        // Some stored procedures may return 0 even when they performed insert.
                        // Verify by selecting the row back.
                        if (!string.IsNullOrWhiteSpace(nv.MaNV))
                        {
                            var exists = GetByMaNV(nv.MaNV);
                            if (exists != null)
                            {
                                // if password column exists but password not set by SP, set it explicitly
                                if (!string.IsNullOrEmpty(nv.MatKhau))
                                {
                                    bool storedHasPwd = !string.IsNullOrEmpty(exists.MatKhau);
                                    if (!storedHasPwd)
                                    {
                                        SetPasswordDirect(nv.MaNV, nv.MatKhau, hasMatKhau, hasPasswordHash);
                                    }
                                }

                                return true;
                            }
                        }

                        return false;
                    }
                    catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                    {
                        throw new Exception("Mã nhân viên đã tồn tại.", ex);
                    }
                }
            }
        }

        public static bool Update(NhanVien nv)
        {
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();
                bool hasMatKhau = ColumnExists(cn, "dbo.NhanVien", "MatKhau");
                bool hasPasswordHash = ColumnExists(cn, "dbo.NhanVien", "PasswordHash");
                bool useSp = ObjectExists(cn, "sp_NhanVien_Update", "P");

                using (var cmd = cn.CreateCommand())
                {
                    if (useSp)
                    {
                        cmd.CommandText = "sp_NhanVien_Update";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@manv", SqlDbType.NVarChar, 10).Value = nv.MaNV?.Trim();
                        cmd.Parameters.Add("@tennv", SqlDbType.NVarChar, 50).Value = (object)nv.TenNV ?? DBNull.Value;
                        cmd.Parameters.Add("@noisinh", SqlDbType.NVarChar, 50).Value =
                            string.IsNullOrWhiteSpace(nv.NoiSinh) ? (object)DBNull.Value : nv.NoiSinh;
                        cmd.Parameters.Add("@ngaylamviec", SqlDbType.SmallDateTime).Value = ToDbDateOrNull(nv.NgayLamViec);

                        if (hasMatKhau && ProcHasParam(cn, "sp_NhanVien_Update", "@matkhau"))
                            cmd.Parameters.Add("@matkhau", SqlDbType.NVarChar, 100).Value =
                                (object)nv.MatKhau ?? DBNull.Value;
                        else if (!hasMatKhau && hasPasswordHash && ProcHasParam(cn, "sp_NhanVien_Update", "@passwordhash"))
                            cmd.Parameters.Add("@passwordhash", SqlDbType.NVarChar, 256).Value = (object)Sha256(nv.MatKhau) ?? DBNull.Value;
                    }
                    else
                    {
                        if (hasMatKhau)
                        {
                            cmd.CommandText = @"UPDATE dbo.NhanVien
                                        SET TENNV=@tennv, NOISINH=@noisinh, NGAYLAMVIEC=@ngaylamviec, MatKhau=@matkhau
                                        WHERE MANV=@manv";
                            cmd.Parameters.AddWithValue("@matkhau", (object)nv.MatKhau ?? DBNull.Value);
                        }
                        else if (hasPasswordHash)
                        {
                            cmd.CommandText = @"UPDATE dbo.NhanVien
                                        SET TENNV=@tennv, NOISINH=@noisinh, NGAYLAMVIEC=@ngaylamviec, PasswordHash=@passwordhash
                                        WHERE MANV=@manv";
                            cmd.Parameters.AddWithValue("@passwordhash", (object)Sha256(nv.MatKhau) ?? DBNull.Value);
                        }
                        else
                        {
                            cmd.CommandText = @"UPDATE dbo.NhanVien
                                        SET TENNV=@tennv, NOISINH=@noisinh, NGAYLAMVIEC=@ngaylamviec
                                        WHERE MANV=@manv";
                        }

                        cmd.Parameters.AddWithValue("@manv", nv.MaNV?.Trim());
                        cmd.Parameters.AddWithValue("@tennv", (object)nv.TenNV ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@noisinh",
                            string.IsNullOrWhiteSpace(nv.NoiSinh) ? (object)DBNull.Value : nv.NoiSinh);
                        cmd.Parameters.Add("@ngaylamviec", SqlDbType.SmallDateTime).Value = ToDbDateOrNull(nv.NgayLamViec);
                    }

                    var rows = cmd.ExecuteNonQuery();
                    if (rows > 0) return true;

                    // If no rows affected, verify target record exists (update may have not changed values)
                    if (!string.IsNullOrWhiteSpace(nv.MaNV))
                    {
                        var exists = GetByMaNV(nv.MaNV);
                        if (exists != null)
                        {
                            // Ensure password is set if SP didn't handle it
                            if (!string.IsNullOrEmpty(nv.MatKhau))
                            {
                                bool storedHasPwd = !string.IsNullOrEmpty(exists.MatKhau);
                                if (!storedHasPwd)
                                {
                                    SetPasswordDirect(nv.MaNV, nv.MatKhau, hasMatKhau, hasPasswordHash);
                                }
                            }
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        public static bool Delete(string maNV)
        {
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();
                bool useSp = ObjectExists(cn, "sp_NhanVien_Delete", "P");

                using (var cmd = cn.CreateCommand())
                {
                    if (useSp)
                    {
                        cmd.CommandText = "sp_NhanVien_Delete";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@manv", SqlDbType.NVarChar, 10).Value = maNV?.Trim();
                    }
                    else
                    {
                        cmd.CommandText = "DELETE FROM dbo.NhanVien WHERE MANV=@manv";
                        cmd.Parameters.AddWithValue("@manv", maNV?.Trim());
                    }

                    var rows = cmd.ExecuteNonQuery();
                    if (rows > 0) return true;

                    // Some SPs may not return affected rows; verify the record no longer exists
                    var exists = GetByMaNV(maNV);
                    return exists == null;
                }
            }
        }

        // ========= SEARCH =========
        public static List<NhanVien> SearchOnServer(string keyword)
        {
            var list = new List<NhanVien>();
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();

                bool hasMatKhau = ColumnExists(cn, "dbo.NhanVien", "MatKhau");
                bool useSp = ObjectExists(cn, "sp_NhanVien_Search", "P");

                using (var cmd = cn.CreateCommand())
                {
                    if (useSp)
                    {
                        cmd.CommandText = "sp_NhanVien_Search";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@keyword", SqlDbType.NVarChar, 100).Value = keyword ?? string.Empty;
                    }
                    else
                    {
                        cmd.CommandText = hasMatKhau
                            ? @"DECLARE @k NVARCHAR(110)=N'%'+ISNULL(@kw,N'')+N'%';
                        SELECT MANV,TENNV,NOISINH,NGAYLAMVIEC,MatKhau
                        FROM dbo.NhanVien
                        WHERE MANV LIKE @k OR TENNV LIKE @k OR NOISINH LIKE @k
                        ORDER BY MANV;"
                            : @"DECLARE @k NVARCHAR(110)=N'%'+ISNULL(@kw,N'')+N'%';
                        SELECT MANV,TENNV,NOISINH,NGAYLAMVIEC
                        FROM dbo.NhanVien
                        WHERE MANV LIKE @k OR TENNV LIKE @k OR NOISINH LIKE @k
                        ORDER BY MANV;";
                        cmd.Parameters.AddWithValue("@kw", (keyword ?? "").Trim());
                    }

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read()) list.Add(MapNv(r, hasMatKhau));
                    }
                }
            }
            return list;
        }

        // ========= AUTH =========
        public static bool TryLoginNhanVien(string maNv, string pass, out string tenNv)
        {
            tenNv = null;
            if (string.IsNullOrWhiteSpace(maNv) || string.IsNullOrWhiteSpace(pass)) return false;

            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();

                bool hasHash = ColumnExists(cn, "dbo.NhanVien", "PasswordHash");
                bool hasPlain = ColumnExists(cn, "dbo.NhanVien", "MatKhau");

                using (var cmd = cn.CreateCommand())
                {
                    if (hasHash)
                    {
                        cmd.CommandText = @"SELECT TENNV FROM dbo.NhanVien 
                                    WHERE MANV=@ma AND PasswordHash=@h";
                        cmd.Parameters.AddWithValue("@ma", maNv.Trim());
                        cmd.Parameters.AddWithValue("@h", Sha256(pass));
                    }
                    else if (hasPlain)
                    {
                        cmd.CommandText = @"SELECT TENNV FROM dbo.NhanVien 
                                    WHERE MANV=@ma AND MatKhau=@mk";
                        cmd.Parameters.AddWithValue("@ma", maNv.Trim());
                        cmd.Parameters.AddWithValue("@mk", pass.Trim());
                    }
                    else
                    {
                        // không có cột nào -> luôn fail
                        return false;
                    }

                    var o = cmd.ExecuteScalar();
                    if (o == null || o == DBNull.Value) return false;
                    tenNv = o.ToString();
                    return true;
                }
            }
        }

        public static string GetPasswordByMaNV(string maNv)
        {
            if (string.IsNullOrWhiteSpace(maNv)) return null;
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();

                bool hasPlain = ColumnExists(cn, "dbo.NhanVien", "MatKhau");
                if (!hasPlain) return null;

                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandText = "SELECT MatKhau FROM dbo.NhanVien WHERE MANV=@ma";
                    cmd.Parameters.AddWithValue("@ma", maNv.Trim());
                    var o = cmd.ExecuteScalar();
                    return (o == null || o == DBNull.Value) ? null : o.ToString();
                }
            }
        }

        // ========= CHANGE PASSWORD =========
        public static bool ChangePassword(string maNV, string currentPassword, string newPassword)
        {
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();

                bool hasHash = ColumnExists(cn, "dbo.NhanVien", "PasswordHash");
                bool hasPlain = ColumnExists(cn, "dbo.NhanVien", "MatKhau");

                using (var cmd = cn.CreateCommand())
                {
                    if (hasHash)
                    {
                        cmd.CommandText = @"UPDATE dbo.NhanVien
                                    SET PasswordHash=@newHash
                                    WHERE MANV=@ma AND PasswordHash=@oldHash";
                        cmd.Parameters.AddWithValue("@newHash", Sha256(newPassword ?? ""));
                        cmd.Parameters.AddWithValue("@oldHash", Sha256(currentPassword ?? ""));
                    }
                    else if (hasPlain)
                    {
                        cmd.CommandText = @"UPDATE dbo.NhanVien
                                    SET MatKhau=@newPwd
                                    WHERE MANV=@ma AND MatKhau=@oldPwd";
                        cmd.Parameters.AddWithValue("@newPwd", newPassword ?? "");
                        cmd.Parameters.AddWithValue("@oldPwd", currentPassword ?? "");
                    }
                    else
                    {
                        return false;
                    }

                    cmd.Parameters.AddWithValue("@ma", (maNV ?? "").Trim());
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
