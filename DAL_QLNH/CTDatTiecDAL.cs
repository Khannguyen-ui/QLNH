using DTO_QLNH;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_QLNH
{
    public class CTDatTiecDAL
    {
        // Kết nối của bạn
        private const string CN =
            @"Data Source=LAPTOP-KRERKDGK\SQLEXPRESS02;Initial Catalog=QLNHS;Integrated Security=True;TrustServerCertificate=True";

        private SqlConnection Conn() => new SqlConnection(CN);

        // ========== GET BY PHIẾU ==========
        public DataTable GetByPhieu(string soPhieu)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_CTDatTiec_GetByPhieu", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);

                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        // ========== EXISTS ==========
        public bool Exists(string soPhieu, string maTd)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_CTDatTiec_Exists", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                cmd.Parameters.AddWithValue("@MaTD", maTd);

                con.Open();
                object result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }

        // ========== INSERT ==========
        public bool Insert(CTDatTiec x)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_CTDatTiec_Insert", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SoPhieu", x.SoPhieu);
                cmd.Parameters.AddWithValue("@MaTD", x.MaTD);

                // SoLuong DECIMAL(18,2)
                var pSL = cmd.Parameters.Add("@SoLuong", SqlDbType.Decimal);
                pSL.Precision = 18; pSL.Scale = 2;
                pSL.Value = x.SoLuong.HasValue ? (object)Convert.ToDecimal(x.SoLuong.Value) : DBNull.Value;

                // GiaBan DECIMAL(18,2)
                var pGB = cmd.Parameters.Add("@GiaBan", SqlDbType.Decimal);
                pGB.Precision = 18; pGB.Scale = 2;
                pGB.Value = x.GiaBan.HasValue ? (object)Convert.ToDecimal(x.GiaBan.Value) : DBNull.Value;

                con.Open();
                // SP trả SELECT @@ROWCOUNT AS Affected
                object scalar = cmd.ExecuteScalar();
                return scalar != null && Convert.ToInt32(scalar) > 0;
            }
        }

        // ========== UPDATE ==========
        public bool Update(CTDatTiec x)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_CTDatTiec_Update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SoPhieu", x.SoPhieu);
                cmd.Parameters.AddWithValue("@MaTD", x.MaTD);

                var pSL = cmd.Parameters.Add("@SoLuong", SqlDbType.Decimal);
                pSL.Precision = 18; pSL.Scale = 2;
                pSL.Value = x.SoLuong.HasValue ? (object)Convert.ToDecimal(x.SoLuong.Value) : DBNull.Value;

                var pGB = cmd.Parameters.Add("@GiaBan", SqlDbType.Decimal);
                pGB.Precision = 18; pGB.Scale = 2;
                pGB.Value = x.GiaBan.HasValue ? (object)Convert.ToDecimal(x.GiaBan.Value) : DBNull.Value;

                con.Open();
                object scalar = cmd.ExecuteScalar();
                return scalar != null && Convert.ToInt32(scalar) > 0;
            }
        }

        // ========== DELETE ==========
        public bool Delete(string soPhieu, string maTd)
        {
            using (var con = Conn())
            using (var cmd = new SqlCommand("dbo.sp_CTDatTiec_Delete", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                cmd.Parameters.AddWithValue("@MaTD", maTd);

                con.Open();
                object scalar = cmd.ExecuteScalar();
                return scalar != null && Convert.ToInt32(scalar) > 0;
            }
        }

        // ========== LOAD MÓN (không cần SP) ==========
        public List<MonLookup> LoadMon()
        {
            using (var cn = Conn())
            using (var cmd = new SqlCommand(
                "SELECT MATD, TENMON, GIATIEN FROM ThucDon ORDER BY TENMON", cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    var list = new List<MonLookup>();
                    while (rd.Read())
                    {
                        double? gia = null;
                        if (!rd.IsDBNull(2))
                        {
                            // GIATIEN có thể là money/decimal → chuyển an toàn sang double
                            object v = rd.GetValue(2);
                            gia = Convert.ToDouble(v);
                        }

                        list.Add(new MonLookup
                        {
                            Ma = rd.GetString(0).Trim(),
                            Ten = rd.IsDBNull(1) ? rd.GetString(0).Trim() : rd.GetString(1),
                            Gia = gia
                        });
                    }
                    return list;
                }
            }
        }
    }
}
