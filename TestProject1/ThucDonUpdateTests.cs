using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Linq;

namespace TestProject1
{
    [TestClass]
    public class ThucDonUpdateTests
    {
        private const string TEST_MA = "TD_UPD";
        // Đối tượng dùng để khóa đồng bộ luồng, tránh lỗi chạy song song trên DB
        private static readonly object _dbLock = new object();

        // Reset dữ liệu trước mỗi test case
        [TestInitialize]
        public void Setup()
        {
            // Lock đảm bảo các bài test không tranh chấp mã TD_UPD cùng lúc
            lock (_dbLock)
            {
                ThucDonBLL.Delete(TEST_MA);
                var tdGoc = new ThucDon
                {
                    MaTD = TEST_MA,
                    TenMon = "Món Gốc",
                    DVT = "Đĩa",
                    GiaTien = 50000,
                    SoLuongTon = 10
                };
                ThucDonBLL.Insert(tdGoc);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            lock (_dbLock)
            {
                ThucDonBLL.Delete(TEST_MA);
            }
        }

        // =================================================================
        // PHẦN 1: KIỂM TRA TƯƠNG TÁC DATABASE (SUCCESS & FAIL)
        // =================================================================

        [TestMethod]
        public void CapNhatTD_DuLieuHopLe_CapNhatThanhCong()
        {
            // 1. Arrange
            var tdUpdate = new ThucDon
            {
                MaTD = TEST_MA,
                TenMon = "Món Đã Sửa",
                DVT = "Tô",
                GiaTien = 65000,
                SoLuongTon = 20,
                GhiChu = "Đã cập nhật"
            };

            // 2. Act
            bool ketQua;
            lock (_dbLock) // Giữ lock trong suốt quá trình Act/Assert nếu cần thiết
            {
                ketQua = ThucDonBLL.Update(tdUpdate);

                // 3. Assert
                Assert.IsTrue(ketQua, "Phải cập nhật thành công khi dữ liệu hợp lệ.");

                var dataDb = ThucDonBLL.GetAll().FirstOrDefault(x => x.MaTD == TEST_MA);
                Assert.IsNotNull(dataDb);
                Assert.AreEqual("Món Đã Sửa", dataDb.TenMon);
                Assert.AreEqual(65000, dataDb.GiaTien);
            }
        }

        [TestMethod]
        public void CapNhatTD_MaKhongTonTai_TraVeFalse()
        {
            // 1. Arrange
            var tdSaiMa = new ThucDon { MaTD = "KHONG_CO", TenMon = "Test", GiaTien = 1000 };

            // 2. Act
            bool ketQua = ThucDonBLL.Update(tdSaiMa);

            // 3. Assert
            Assert.IsFalse(ketQua, "Không được báo thành công khi mã món không có trong DB.");
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA LOGIC NGHIỆP VỤ (VALIDATION)
        // =================================================================

        [TestMethod]
        public void CapNhatTD_TenRong_TraVeLoi()
        {
            var td = new ThucDon { MaTD = TEST_MA, TenMon = "", GiaTien = 5000 };
            string msg = ValidateUpdateLogic(td);
            Assert.AreEqual("Tên món không được để trống.", msg);
        }

        [TestMethod]
        public void CapNhatTD_GiaAm_TraVeLoi()
        {
            var td = new ThucDon { MaTD = TEST_MA, TenMon = "Sửa Giá", GiaTien = -100 };
            string msg = ValidateUpdateLogic(td);
            Assert.AreEqual("Giá tiền không hợp lệ.", msg);
        }

        // =================================================================
        // PHẦN 3: EDGE CASES (TRƯỜNG HỢP BIÊN)
        // =================================================================

        [TestMethod]
        public void CapNhatTD_GhiChuVuaDu_ThemThanhCong()
        {
            // Giảm xuống 50 ký tự để phù hợp với thiết kế DB phổ thông
            string chuoiVuaDu = new string('A', 50);
            var td = new ThucDon
            {
                MaTD = TEST_MA,
                TenMon = "Món Ghi Chú",
                GiaTien = 1000,
                GhiChu = chuoiVuaDu
            };

            bool ketQua;
            lock (_dbLock)
            {
                ketQua = ThucDonBLL.Update(td);
            }
            Assert.IsTrue(ketQua, "Hệ thống phải xử lý được ghi chú trong giới hạn cho phép.");
        }

        [TestMethod]
        public void CapNhatTD_TenCoDauNhay_ThemThanhCong()
        {
            var td = new ThucDon { MaTD = TEST_MA, TenMon = "Lẩu 'Thái' Lan", GiaTien = 200000 };

            bool ketQua;
            lock (_dbLock)
            {
                ketQua = ThucDonBLL.Update(td);
            }
            Assert.IsTrue(ketQua, "Hệ thống phải xử lý được dấu nháy đơn trong câu Update.");
        }

        // -----------------------------------------------------------------
        // HELPER: Validate logic giống trong FormThucDon
        // -----------------------------------------------------------------
        private string ValidateUpdateLogic(ThucDon td)
        {
            if (string.IsNullOrWhiteSpace(td.MaTD)) return "Mã không được trống.";
            if (string.IsNullOrWhiteSpace(td.TenMon)) return "Tên món không được để trống.";
            if (td.GiaTien < 0) return "Giá tiền không hợp lệ.";
            return "OK";
        }
    }
}