using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DTO_QLNH;

namespace DAL_QLNH
{
    public class DatTiecDAL
    {
        private readonly string _cnStr =
            "Server=VANKHAN;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";

        /* ======================= GRID CHÍNH ======================= */

        // Lấy toàn bộ phiếu đặt tiệc + tên khách + tên NV
        public DataTable GetAll()
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var da = new SqlDataAdapter(@"
                SELECT 
                    d.SoPhieu      AS SOPHIEU,
                    d.NgayDK       AS NGAYDK,
                    d.MATK         AS MATK,
                    tk.TENTK       AS TENTK,
                    d.MANV         AS MANV,
                    nv.TENNV       AS TENNV,
                    d.SoLuongKhach AS SOLUONGKHACH,
                    d.PHONG        AS PHONG,
                    d.Ca           AS Ca,
                    d.TrangThai    AS TRANGTHAI
                FROM DatTiec d
                LEFT JOIN ThucKhach tk ON tk.MATK = d.MATK
                LEFT JOIN NhanVien  nv ON nv.MANV = d.MANV
                ORDER BY d.NgayDK DESC, d.SoPhieu", cn))
                {
                    var tb = new DataTable();
                    da.Fill(tb);
                    return tb;
                }
            }
        }

        // Tìm theo từ khóa (SoPhieu / TenKhach / Phong / NV)
        public DataTable Search(string keyword)
        {
            string kw = keyword ?? "";
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var da = new SqlDataAdapter(@"
                SELECT 
                    d.SoPhieu      AS SOPHIEU,
                    d.NgayDK       AS NGAYDK,
                    d.MATK         AS MATK,
                    tk.TENTK       AS TENTK,
                    d.MANV         AS MANV,
                    nv.TENNV       AS TENNV,
                    d.SoLuongKhach AS SOLUONGKHACH,
                    d.PHONG        AS PHONG,
                    d.Ca           AS Ca,
                    d.TrangThai    AS TRANGTHAI
                FROM DatTiec d
                LEFT JOIN ThucKhach tk ON tk.MATK = d.MATK
                LEFT JOIN NhanVien  nv ON nv.MANV = d.MANV
                WHERE d.SoPhieu LIKE @kw
                   OR tk.TENTK  LIKE @kw
                   OR d.PHONG   LIKE @kw
                   OR nv.TENNV  LIKE @kw
                ORDER BY d.NgayDK DESC, d.SoPhieu", cn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@kw", "%" + kw + "%");
                    var tb = new DataTable();
                    da.Fill(tb);
                    return tb;
                }
            }
        }

        // Xoá phiếu (trả false nếu đang có chi tiết hoặc FK ràng buộc)
        public bool Delete(string soPhieu)
        {
            if (string.IsNullOrWhiteSpace(soPhieu)) return false;

            using (var cn = new SqlConnection(_cnStr))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        // If there is a HoaDon for this SoPhieu, do not delete
                        using (var cmdChk = new SqlCommand("SELECT COUNT(1) FROM HoaDon WHERE SoPhieu = @SoPhieu", cn, tx))
                        {
                            cmdChk.Parameters.AddWithValue("@SoPhieu", soPhieu);
                            var cntObj = cmdChk.ExecuteScalar();
                            int cnt = cntObj == null ? 0 : Convert.ToInt32(cntObj);
                            if (cnt > 0)
                            {
                                tx.Rollback();
                                return false; // there is an invoice, do not delete
                            }
                        }

                        // Delete child CTDatTiec rows first (if any)
                        using (var cmdDelCt = new SqlCommand("DELETE FROM CTDatTiec WHERE SoPhieu = @SoPhieu", cn, tx))
                        {
                            cmdDelCt.Parameters.AddWithValue("@SoPhieu", soPhieu);
                            cmdDelCt.ExecuteNonQuery();
                        }

                        // Then delete the DatTiec row
                        using (var cmd = new SqlCommand("DELETE FROM DatTiec WHERE SoPhieu = @SoPhieu", cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                            int affected = cmd.ExecuteNonQuery();
                            tx.Commit();
                            return affected > 0;
                        }
                    }
                    catch (SqlException)
                    {
                        try { tx.Rollback(); } catch { }
                        return false;
                    }
                    catch (Exception)
                    {
                        try { tx.Rollback(); } catch { }
                        return false;
                    }
                }
            }
        }

        /* ======================= COMBOBOX ======================= */

        public List<LookItem> GetThucKhach()
        {
            var list = new List<LookItem>();
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                SELECT MATK, TENTK FROM ThucKhach ORDER BY TENTK, MATK", cn))
                {
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new LookItem
                            {
                                Ma = rd["MATK"].ToString().Trim(),
                                Ten = rd["TENTK"].ToString().Trim()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<LookItem> GetNhanVien()
        {
            var list = new List<LookItem>();
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                SELECT MANV, TENNV FROM NhanVien ORDER BY TENNV, MANV", cn))
                {
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new LookItem
                            {
                                Ma = rd["MANV"].ToString().Trim(),
                                Ten = rd["TENNV"].ToString().Trim()
                            });
                        }
                    }
                }
            }
            return list;
        }

        /* ======================= TREEVIEW / CHI TIẾT ======================= */

        public DataTable GetByMatk(string matk)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var da = new SqlDataAdapter(@"
                SELECT 
                    d.SoPhieu      AS SOPHIEU,
                    d.NgayDK       AS NGAYDK,
                    d.MATK         AS MATK,
                    tk.TENTK       AS TENTK,
                    d.MANV         AS MANV,
                    nv.TENNV       AS TENNV,
                    d.SoLuongKhach AS SOLUONGKHACH,
                    d.PHONG        AS PHONG,
                    d.Ca           AS Ca,
                    d.TrangThai    AS TRANGTHAI
                FROM DatTiec d
                LEFT JOIN ThucKhach tk ON tk.MATK = d.MATK
                LEFT JOIN NhanVien  nv ON nv.MANV = d.MANV
                WHERE d.MATK = @matk
                ORDER BY d.NgayDK DESC, d.SoPhieu", cn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@matk", matk);
                    var tb = new DataTable();
                    da.Fill(tb);
                    return tb;
                }
            }
        }

        // danh sách node phiếu của1 khách
        public List<PhieuItem> GetPhieuByMatk(string matk)
        {
            var list = new List<PhieuItem>();
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                SELECT SoPhieu, NgayDK, PHONG
                FROM DatTiec
                WHERE MATK = @matk
                ORDER BY NgayDK DESC, SoPhieu", cn))
                {
                    cmd.Parameters.AddWithValue("@matk", matk);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new PhieuItem
                            {
                                SoPhieu = rd["SoPhieu"].ToString().Trim(),
                                NgayDk = rd["NgayDK"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["NgayDK"]),
                                Phong = rd["PHONG"].ToString().Trim()
                            });
                        }
                    }
                }
            }
            return list;
        }

        // chi tiết món của1 phiếu
        public List<CtMonItem> GetCtBySoPhieu(string soPhieu)
        {
            var list = new List<CtMonItem>();
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                SELECT td.TenMon, ct.SoLuong, ct.GiaBan
                FROM CTDatTiec ct
                LEFT JOIN ThucDon td ON td.MaTD = ct.MaTD
                WHERE ct.SoPhieu = @sophieu
                ORDER BY td.TenMon", cn))
                {
                    cmd.Parameters.AddWithValue("@sophieu", soPhieu);
                    cn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new CtMonItem
                            {
                                TenMon = rd["TenMon"] as string,
                                SoLuong = rd["SoLuong"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["SoLuong"]),
                                GiaBan = rd["GiaBan"] == DBNull.Value ? (double?)null : Convert.ToDouble(rd["GiaBan"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        // lấy1 phiếu theo SoPhieu (dùng để mở lại hóa đơn/đổ form)
        public DataRow GetBySoPhieu(string soPhieu)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var da = new SqlDataAdapter(@"
                SELECT TOP1
                    d.SoPhieu, d.NgayDK, d.NgayDatNgay, d.MATK, tk.TENTK,
                    d.MANV, nv.TENNV, d.SoLuongKhach, d.PHONG, d.Ca, d.TrangThai
                FROM DatTiec d
                LEFT JOIN ThucKhach tk ON tk.MATK = d.MATK
                LEFT JOIN NhanVien nv ON nv.MANV = d.MANV
                WHERE d.SoPhieu = @SoPhieu", cn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@SoPhieu", soPhieu);
                    var tb = new DataTable();
                    da.Fill(tb);
                    return tb.Rows.Count > 0 ? tb.Rows[0] : null;
                }
            }
        }

        /* ======================= PHÒNG/CA ======================= */

        // true = đã có người đặt (bị trùng). (BLL sẽ negation)
        public bool KiemTraTrungPhongCa(DateTime ngayDat, string phong, string ca, string soPhieuBoQua = null)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM DatTiec
                WHERE NgayDatNgay = @ngay
                  AND PHONG       = @phong
                  AND Ca          = @ca
                  AND (@skip IS NULL OR SoPhieu <> @skip)", cn))
                {
                    cmd.Parameters.AddWithValue("@ngay", ngayDat.Date);
                    cmd.Parameters.AddWithValue("@phong", phong?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@ca", ca?.Trim() ?? "");
                    cmd.Parameters.AddWithValue("@skip", string.IsNullOrWhiteSpace(soPhieuBoQua) ? (object)DBNull.Value : soPhieuBoQua.Trim());
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /* ======================= INSERT / UPDATE ======================= */

        public int InsertDatTiec(string soPhieu, DateTime ngayDat,
                                 string maTK, string maNV,
                                 int soLuongKhach, string phong, string ca,
                                 string trangThai = "CHO_TT")
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                INSERT INTO DatTiec 
                    (SoPhieu, NgayDK, NgayDatNgay, MATK, MANV, SoLuongKhach, PHONG, Ca, TrangThai)
                VALUES
                    (@SoPhieu, @NgayDK, @NgayDatNgay, @MATK, @MANV, @SoLuongKhach, @PHONG, @Ca, @TrangThai)", cn))

                {
                    cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                    cmd.Parameters.AddWithValue("@NgayDK", ngayDat);
                    cmd.Parameters.AddWithValue("@NgayDatNgay", ngayDat.Date);
                    cmd.Parameters.AddWithValue("@MATK", maTK);
                    cmd.Parameters.AddWithValue("@MANV", maNV);
                    cmd.Parameters.AddWithValue("@SoLuongKhach", soLuongKhach);
                    cmd.Parameters.AddWithValue("@PHONG", phong);
                    cmd.Parameters.AddWithValue("@Ca", ca);
                    cmd.Parameters.AddWithValue("@TrangThai", string.IsNullOrWhiteSpace(trangThai) ? "CHO_TT" : trangThai);

                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int UpdateDatTiec(string soPhieu, DateTime ngayDat,
                                 string maTK, string maNV,
                                 int soLuongKhach, string phong, string ca,
                                 string trangThai = null)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                UPDATE DatTiec
                   SET NgayDK = @NgayDK,
                       NgayDatNgay = @NgayDatNgay,
                       MATK = @MATK,
                       MANV = @MANV,
                       SoLuongKhach = @SoLuongKhach,
                       PHONG = @PHONG,
                       Ca = @Ca
                 WHERE SoPhieu = @SoPhieu;

                -- optional cập nhật trạng thái
                IF @TrangThai IS NOT NULL
                    UPDATE DatTiec SET TrangThai=@TrangThai WHERE SoPhieu=@SoPhieu;", cn))

                {
                    cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                    cmd.Parameters.AddWithValue("@NgayDK", ngayDat);
                    cmd.Parameters.AddWithValue("@NgayDatNgay", ngayDat.Date);
                    cmd.Parameters.AddWithValue("@MATK", maTK);
                    cmd.Parameters.AddWithValue("@MANV", maNV);
                    cmd.Parameters.AddWithValue("@SoLuongKhach", soLuongKhach);
                    cmd.Parameters.AddWithValue("@PHONG", phong);
                    cmd.Parameters.AddWithValue("@Ca", ca);
                    cmd.Parameters.AddWithValue("@TrangThai", (object)trangThai ?? DBNull.Value);

                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /* ======================= TRẠNG THÁI & DANH SÁCH CHỜ ======================= */

        // Đổi trạng thái (CHO_TT / DA_TT)
        public int SetTrangThai(string soPhieu, string trangThai)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                UPDATE DatTiec SET TrangThai = @TrangThai
                WHERE SoPhieu = @SoPhieu", cn))
                {
                    cmd.Parameters.AddWithValue("@SoPhieu", soPhieu);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Kiểm tra đã có hoá đơn cho phiếu này chưa
        public bool HasHoaDon(string soPhieu)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var cmd = new SqlCommand(@"
                SELECT COUNT(*) FROM HoaDon WHERE SoPhieu = @sp", cn))
                {
                    cmd.Parameters.AddWithValue("@sp", soPhieu);
                    cn.Open();
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        // Danh sách phiếu CHỜ THANH TOÁN (để form Lập/Thanh toán hoá nạp lên)
        public DataTable GetPending(string keyword = null)
        {
            using (var cn = new SqlConnection(_cnStr))
            {
                using (var da = new SqlDataAdapter(@"
                DECLARE @k NVARCHAR(110) = N'%'+ISNULL(@kw,N'')+N'%';
                SELECT d.SoPhieu, d.NgayDK, d.PHONG, d.Ca, d.SoLuongKhach,
                       tk.TENTK, nv.TENNV
                FROM DatTiec d
                LEFT JOIN ThucKhach tk ON tk.MATK = d.MATK
                LEFT JOIN NhanVien  nv ON nv.MANV = d.MANV
                WHERE d.TrangThai = N'CHO_TT'
                  AND (@kw IS NULL OR d.SoPhieu LIKE @k OR tk.TENTK LIKE @k OR d.PHONG LIKE @k OR nv.TENNV LIKE @k)
                ORDER BY d.NgayDK DESC, d.SoPhieu", cn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@kw", (object)keyword ?? DBNull.Value);
                    var tb = new DataTable();
                    da.Fill(tb);
                    return tb;
                }
            }
        }
    }
}
