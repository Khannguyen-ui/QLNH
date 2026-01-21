using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using DAL_QLNH; // Để dùng DAL xóa dữ liệu test

namespace TestProject1
{
    [TestClass]
    public class DatTiecAddTests
    {
        private DatTiecBLL _bll;
        private DatTiecDAL _dal; // Dùng để dọn dẹp dữ liệu test
        private const string CONN_STR = "Server=VANKHAN;Database=QLNHS;Trusted_Connection=True;TrustServerCertificate=True";


        [TestInitialize]
        public void Setup()
        {
            _bll = new DatTiecBLL();
            _dal = new DatTiecDAL();
        }

        // =================================================================
        // PHẦN 1: KIỂM TRA VALIDATION (RỖNG)
        // =================================================================

        // TC01: Số phiếu rỗng
        [TestMethod]
        public void ThemDT_SoPhieuRong_TraVeFalse()
        {
            string err = "";
            bool kq = _bll.ThemDatTiec("", DateTime.Now, "TK01", "NV01", 5, "P1", "Ca1", out err);

            Assert.IsFalse(kq);
            Assert.AreEqual("Số phiếu không được rỗng.", err);
        }

        // TC02: Phòng rỗng
        [TestMethod]
        public void ThemDT_PhongRong_TraVeFalse()
        {
            string err = "";
            bool kq = _bll.ThemDatTiec("P_TEST_01", DateTime.Now, "TK01", "NV01", 5, "", "Ca1", out err);

            Assert.IsFalse(kq);
            Assert.AreEqual("Phòng không được rỗng.", err);
        }

        // TC03: Ca rỗng
        [TestMethod]
        public void ThemDT_CaRong_TraVeFalse()
        {
            string err = "";
            bool kq = _bll.ThemDatTiec("P_TEST_01", DateTime.Now, "TK01", "NV01", 5, "P1", "", out err);

            Assert.IsFalse(kq);
            Assert.AreEqual("Ca không được rỗng.", err);
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA LOGIC NGHIỆP VỤ & DATABASE
        // =================================================================

        // TC04: Thêm thành công (Happy Path)
        [TestMethod]
        public void ThemDT_HopLe_ThanhCong()
        {
            // 1. Arrange
            string soPhieu = "DT_TEST_OK";
            DateTime ngay = DateTime.Today; // Dùng Today để tránh lệch giờ phút giây
            string phong = "Ban_Test_VIP";
            string ca = "Ca_Test";

            // [QUAN TRỌNG] Xóa sạch bất kỳ phiếu nào đang chiếm chỗ Phòng+Ca+Ngày này
            // (Bất kể nó có mã phiếu là gì)
            CleanUpConflict(ngay, phong, ca);

            // Xóa phiếu test chính chủ (đề phòng)
            _dal.Delete(soPhieu);

            // 2. Act
            string err = "";
            bool kq = _bll.ThemDatTiec(soPhieu, ngay, "TK01", "NV01", 10, phong, ca, out err);

            // 3. Assert
            Assert.IsTrue(kq, "Lỗi thêm mới: " + err);

            // Kiểm tra lại trong DB
            DataRow row = _bll.GetBySoPhieu(soPhieu);
            Assert.IsNotNull(row, "Không tìm thấy phiếu vừa thêm trong DB.");
            Assert.AreEqual(phong, row["PHONG"].ToString());

            // 4. Cleanup
            _dal.Delete(soPhieu);
        }


        // TC05: Trùng lịch đặt (Phòng + Ca + Ngày đã có người)
        [TestMethod]
        public void ThemDT_TrungLich_TraVeFalse()
        {
            // 1. Arrange: Tạo 1 phiếu chiếm chỗ trước
            string soPhieu1 = "DT_GOC";
            string soPhieu2 = "DT_TRUNG";
            DateTime ngay = DateTime.Today; // Cùng ngày
            string phong = "Ban_DuyNhat";
            string ca = "Ca_Toi";

            _dal.Delete(soPhieu1);
            _dal.Delete(soPhieu2);

            // Thêm phiếu 1 thành công
            _bll.ThemDatTiec(soPhieu1, ngay, "TK01", "NV01", 5, phong, ca, out _);

            // 2. Act: Cố tình thêm phiếu 2 vào cùng Ngày + Phòng + Ca đó
            string err = "";
            bool kq = _bll.ThemDatTiec(soPhieu2, ngay, "TK02", "NV02", 2, phong, ca, out err);

            // 3. Assert
            Assert.IsFalse(kq, "Hệ thống phải chặn trùng lịch.");
            // Kiểm tra thông báo lỗi có chứa thông tin ngày/phòng không
            Assert.IsTrue(err.Contains("đã có người đặt"), "Thông báo lỗi không đúng.");

            // 4. Cleanup
            _dal.Delete(soPhieu1);
            _dal.Delete(soPhieu2);
        }
        private void CleanUpConflict(DateTime ngay, string phong, string ca)
        {
            using (var cn = new SqlConnection(CONN_STR))
            {
                cn.Open();
                string sql = @"DELETE FROM DatTiec 
                               WHERE NgayDatNgay = @ngay 
                               AND Phong = @phong 
                               AND Ca = @ca";
                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                    cmd.Parameters.AddWithValue("@phong", phong);
                    cmd.Parameters.AddWithValue("@ca", ca);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}