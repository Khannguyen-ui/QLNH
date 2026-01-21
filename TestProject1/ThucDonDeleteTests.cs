using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Linq;

namespace TestProject1
{
    [TestClass]
    public class ThucDonDeleteTests
    {
        // =================================================================
        // PHẦN 1: KIỂM TRA LOGIC ĐẦU VÀO & TRƯỜNG HỢP CƠ BẢN
        // =================================================================

        // TC_DEL_0: Người dùng nhấn Xóa khi chưa chọn món (Mã rỗng)
        [TestMethod]
        public void XoaTD_ChuaChonMon_TraVeFalse()
        {
            // Arrange
            string maRong = ""; // Tương đương với việc txtMaTD.Text trống

            // Act
            bool ketQua = ThucDonBLL.Delete(maRong);

            // Assert
            // Hàm BLL hoặc DAL phải trả về False để GUI biết đường hiện MessageBox cảnh báo
            Assert.IsFalse(ketQua, "Hệ thống không được xóa khi không có mã định danh.");
        }
        // TC_DEL_01: Kiểm tra truyền mã rỗng
        [TestMethod]
        public void XoaTD_MaRong_TraVeFalse()
        {
            // Act
            bool ketQua = ThucDonBLL.Delete("");

            // Assert
            Assert.IsFalse(ketQua, "Hàm Delete không được trả về True khi mã rỗng.");
        }

        // TC_DEL_02: Kiểm tra xóa mã không tồn tại
        [TestMethod]
        public void XoaTD_MaKhongTonTai_TraVeFalse()
        {
            // Act
            bool ketQua = ThucDonBLL.Delete("GHOST_999");

            // Assert
            Assert.IsFalse(ketQua, "Hệ thống không nên báo thành công khi mã không có trong DB.");
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA NGHIỆP VỤ XÓA THÀNH CÔNG (HAPPY PATH)
        // =================================================================

        // TC_DEL_03: Xóa một món ăn hợp lệ vừa mới thêm
        [TestMethod]
        public void XoaTD_DuLieuSach_XoaThanhCong()
        {
            // 1. Arrange: Tạo một món ăn tạm để xóa
            string maTest = "TD_DEL_01";
            ThucDonBLL.Delete(maTest); // Cleanup trước

            ThucDon td = new ThucDon { MaTD = maTest, TenMon = "Món Test Xóa", GiaTien = 1000 };
            ThucDonBLL.Insert(td);

            // 2. Act: Thực hiện xóa
            bool ketQua = ThucDonBLL.Delete(maTest);

            // 3. Assert
            Assert.IsTrue(ketQua, "Lẽ ra phải xóa thành công món ăn hợp lệ.");

            // Kiểm tra lại trong DB xem còn tồn tại không
            var tonTai = ThucDonBLL.GetAll().Any(x => x.MaTD == maTest);
            Assert.IsFalse(tonTai, "Dữ liệu vẫn còn tồn tại trong Database sau khi xóa!");
        }

        // =================================================================
        // PHẦN 3: KIỂM TRA RÀNG BUỘC DỮ LIỆU (DATA INTEGRITY)
        // =================================================================

        // TC_DEL_04: Xóa món ăn đang có trong Hóa đơn (Ràng buộc khóa ngoại)
        // Lưu ý: Giả định mã 'TD01' đã được tạo và gán vào một Hóa đơn trong SQL
        [TestMethod]
        public void XoaTD_DangCoHoaDon_ChặnXóaThànhCông()
        {
            // Arrange: Mã món ăn này đã phát sinh giao dịch
            string maDangBan = "TD01";

            // Act
            bool ketQua = ThucDonBLL.Delete(maDangBan);

            // Assert
            // Mong đợi là False vì SQL Server sẽ quăng lỗi Foreign Key và DAL trả về false
            Assert.IsFalse(ketQua, "Hệ thống đang cho phép xóa món ăn đã có lịch sử bán hàng (Lỗi ràng buộc)!");
        }

        // =================================================================
        // PHẦN 4: KIỂM TRA AN TOÀN HỆ THỐNG
        // =================================================================

        // TC_DEL_05: Kiểm tra SQL Injection qua tham số xóa
        [TestMethod]
        public void XoaTD_SQLInjection_ChặnThànhCông()
        {
            // Act: Truyền chuỗi phá hoại SQL
            string maDocHai = "' OR 1=1 --";
            bool ketQua = ThucDonBLL.Delete(maDocHai);

            // Assert
            Assert.IsFalse(ketQua, "Hệ thống bị lỗi bảo mật SQL Injection khi xóa!");
        }

        // TC_DEL_06: Kiểm tra xóa hàng loạt (Bulk Delete logic)
        [TestMethod]
        public void XoaTD_DanhSachNhieuMa_XoaDungSoLuong()
        {
            // 1. Arrange: Tạo 2 món tạm
            string m1 = "BULK1", m2 = "BULK2";
            ThucDonBLL.Insert(new ThucDon { MaTD = m1, TenMon = "Món 1", GiaTien = 1000 });
            ThucDonBLL.Insert(new ThucDon { MaTD = m2, TenMon = "Món 2", GiaTien = 1000 });

            string[] listDelete = { m1, m2 };

            // 2. Act
            int countSuccess = 0;
            foreach (var id in listDelete)
            {
                if (ThucDonBLL.Delete(id)) countSuccess++;
            }

            // 3. Assert
            Assert.AreEqual(2, countSuccess, "Xóa hàng loạt không đủ số lượng mong muốn.");
        }
    }
}