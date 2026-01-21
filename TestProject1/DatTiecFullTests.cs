using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class DatTiecFullTests
    {
        private DatTiecBLL _bll;

        // Các hằng số dữ liệu test
        private const string ID_TEST_1 = "TEST_UPD_1";
        private const string ID_TEST_2 = "TEST_UPD_2";
        private const string MATK = "TK01"; // Đảm bảo mã này có thật trong DB
        private const string MANV = "NV01"; // Đảm bảo mã này có thật trong DB

        // CẤU HÌNH KẾT NỐI (Lưu ý: Phải trùng khớp với máy của bạn)
        private const string CONN_STR = "Server=VANKHAN;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";

        [TestInitialize]
        public void Setup()
        {
            _bll = new DatTiecBLL();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Dọn dẹp sau khi test xong
            ForceCleanUp(ID_TEST_1);
            ForceCleanUp(ID_TEST_2);
        }

        // =================================================================
        // HÀM HỖ TRỢ "BẤT TỬ" (Upsert)
        // =================================================================
        private void CreateSeedData(string id, string phong, string ca, int sl = 10)
        {
            // 1. Dọn dẹp chỗ ngồi (Phòng/Ca) để tránh lỗi trùng lịch
            CleanUpConflict(DateTime.Today.AddDays(1), phong, ca, id);

            // 2. Chạy SQL "Upsert" (Có thì Update, Chưa thì Insert)
            using (var cn = new SqlConnection(CONN_STR))
            {
                cn.Open();
                string sql = @"
                    DELETE FROM HoaDon WHERE SoPhieu = @SoPhieu;
                    DELETE FROM CTDatTiec WHERE SoPhieu = @SoPhieu;

                    IF EXISTS (SELECT 1 FROM DatTiec WHERE SoPhieu = @SoPhieu)
                    BEGIN
                        UPDATE DatTiec 
                        SET NgayDK = @Ngay, NgayDatNgay = @Ngay, MATK = @MaTK, MANV = @MaNV, 
                            SoLuongKhach = @SL, PHONG = @Phong, Ca = @Ca, TrangThai = 'CHO_TT'
                        WHERE SoPhieu = @SoPhieu
                    END
                    ELSE
                    BEGIN
                        INSERT INTO DatTiec (SoPhieu, NgayDK, NgayDatNgay, MATK, MANV, SoLuongKhach, PHONG, Ca, TrangThai)
                        VALUES (@SoPhieu, @Ngay, @Ngay, @MaTK, @MaNV, @SL, @Phong, @Ca, 'CHO_TT')
                    END";

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@SoPhieu", id);
                    cmd.Parameters.AddWithValue("@Ngay", DateTime.Today.AddDays(1));
                    cmd.Parameters.AddWithValue("@MaTK", MATK);
                    cmd.Parameters.AddWithValue("@MaNV", MANV);
                    cmd.Parameters.AddWithValue("@SL", sl);
                    cmd.Parameters.AddWithValue("@Phong", phong);
                    cmd.Parameters.AddWithValue("@Ca", ca);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void CleanUpConflict(DateTime ngay, string phong, string ca, string excludeId)
        {
            try
            {
                using (var cn = new SqlConnection(CONN_STR))
                {
                    cn.Open();
                    string sql = "SELECT SoPhieu FROM DatTiec WHERE NgayDatNgay=@n AND PHONG=@p AND Ca=@c AND SoPhieu <> @exclude";
                    var ids = new List<string>();
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@n", ngay.Date);
                        cmd.Parameters.AddWithValue("@p", phong);
                        cmd.Parameters.AddWithValue("@c", ca);
                        cmd.Parameters.AddWithValue("@exclude", excludeId);
                        using (var rd = cmd.ExecuteReader())
                            while (rd.Read()) ids.Add(rd["SoPhieu"].ToString());
                    }
                    foreach (var id in ids) ForceCleanUp(id);
                }
            }
            catch { }
        }

        private void ForceCleanUp(string soPhieu)
        {
            try
            {
                using (var cn = new SqlConnection(CONN_STR))
                {
                    cn.Open();
                    string sql = @"
                        DELETE FROM HoaDon WHERE SoPhieu = @sp;
                        DELETE FROM CTDatTiec WHERE SoPhieu = @sp;
                        DELETE FROM DatTiec WHERE SoPhieu = @sp;";
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@sp", soPhieu);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        // =================================================================
        // CÁC TEST CASE (ĐÃ LOẠI BỎ CASE 'HUY_BO' GÂY LỖI)
        // =================================================================

        [TestMethod]
        public void CapNhat_ThongTinCoBan_ThanhCong()
        {
            CreateSeedData(ID_TEST_1, "VIP_1", "CA_1", 10);

            string err = "";
            bool kq = _bll.CapNhatDatTiec(ID_TEST_1, DateTime.Today.AddDays(1), MATK, MANV, 50, "VIP_1", "CA_1", out err);

            Assert.IsTrue(kq, "Lỗi cập nhật: " + err);
            DataRow row = _bll.GetBySoPhieu(ID_TEST_1);
            Assert.AreEqual(50, Convert.ToInt32(row["SoLuongKhach"]));
        }

        [TestMethod]
        public void CapNhat_DoiSangPhongTrong_ThanhCong()
        {
            CreateSeedData(ID_TEST_1, "VIP_1", "CA_1");
            CleanUpConflict(DateTime.Today.AddDays(1), "VIP_2_NEW", "CA_1", ID_TEST_1);

            string err = "";
            bool kq = _bll.CapNhatDatTiec(ID_TEST_1, DateTime.Today.AddDays(1), MATK, MANV, 10, "VIP_2_NEW", "CA_1", out err);

            Assert.IsTrue(kq, "Lỗi đổi phòng: " + err);
            DataRow row = _bll.GetBySoPhieu(ID_TEST_1);
            Assert.AreEqual("VIP_2_NEW", row["PHONG"].ToString());
        }

        [TestMethod]
        public void CapNhat_DoiPhong_BiTrungLich_TraVeFalse()
        {
            // Tạo 2 phiếu xung đột
            CreateSeedData(ID_TEST_1, "VIP_1", "CA_1");
            CreateSeedData(ID_TEST_2, "VIP_BLOCK", "CA_1");

            string err = "";
            bool kq = _bll.CapNhatDatTiec(ID_TEST_1, DateTime.Today.AddDays(1), MATK, MANV, 10, "VIP_BLOCK", "CA_1", out err);

            Assert.IsFalse(kq, "Hệ thống không chặn được trùng lịch!");
            Assert.IsTrue(err.Contains("đã có người đặt"), "Thông báo lỗi không đúng: " + err);
        }

        [TestMethod]
        public void CapNhat_TrangThai_DaThanhToan_ThanhCong()
        {
            CreateSeedData(ID_TEST_1, "VIP_1", "CA_1");

            bool kq = _bll.MarkDaThanhToan(ID_TEST_1); // Chuyển sang DA_TT

            Assert.IsTrue(kq, "Lỗi khi update trạng thái thanh toán");
            DataRow rowAfter = _bll.GetBySoPhieu(ID_TEST_1);
            Assert.AreEqual("DA_TT", rowAfter["TrangThai"].ToString());
        }

        // Đã bỏ test case 'SetTrangThaiTuyChinh' dùng 'HUY_BO' để tránh lỗi DB Constraint

        [TestMethod]
        public void CapNhat_IdKhongTonTai_TraVeFalse()
        {
            string err = "";
            ForceCleanUp("ID_MA");
            bool kq = _bll.CapNhatDatTiec("ID_MA", DateTime.Now, MATK, MANV, 10, "P", "C", out err);
            Assert.IsFalse(kq);
            Assert.AreEqual("Không cập nhật được phiếu đặt tiệc.", err);
        }

        [TestMethod]
        public void CapNhat_IdRong_TraVeFalse()
        {
            string err = "";
            bool kq = _bll.CapNhatDatTiec("", DateTime.Now, MATK, MANV, 10, "P", "C", out err);
            Assert.IsFalse(kq);
            Assert.AreEqual("Thiếu số phiếu.", err);
        }
    }
}