using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;

namespace TestProject1
{
    [TestClass]
    public class NhanVienDeleteTests
    {
        // TC_DEL_01: Mã rỗng
        [TestMethod]
        public void XoaNV_MaRong_TraVeLoi()
        {
            string err = "";
            bool ketQua = NhanVienBLL.Delete("", out err);

            Assert.IsFalse(ketQua);
            Assert.AreEqual("Thiếu mã nhân viên.", err);
        }

        // TC_DEL_02: Mã null
        [TestMethod]
        public void XoaNV_MaNull_TraVeLoi()
        {
            string err = "";
            bool ketQua = NhanVienBLL.Delete(null, out err);

            Assert.IsFalse(ketQua);
            Assert.AreEqual("Thiếu mã nhân viên.", err);
        }

        // TC_DEL_03: Xóa thành công (Dữ liệu sạch, không ràng buộc)
        [TestMethod]
        public void XoaNV_NhanVienMoi_XoaThanhCong()
        {
            // Arrange
            string maTest = "NVTEST01"; // Mã không chứa ký tự đặc biệt
            NhanVienBLL.Delete(maTest, out _); // Cleanup trước

            NhanVien nv = new NhanVien { MaNV = maTest, TenNV = "Test User", MatKhau = "123", NoiSinh = "Test" };

            // Insert phải thành công thì mới test xóa được
            string errInsert;
            bool insertOk = NhanVienBLL.Insert(nv, out errInsert);
            Assert.IsTrue(insertOk, "Setup Insert thất bại: " + errInsert);

            // Act
            string err = "";
            bool ketQua = NhanVienBLL.Delete(maTest, out err);

            // Assert
            Assert.IsTrue(ketQua, "Lỗi xóa nhân viên hợp lệ: " + err);
            Assert.IsNull(NhanVienBLL.GetByMaNV(maTest), "Dữ liệu vẫn còn trong DB!");
        }

        // TC_DEL_04: Xóa không tồn tại
        [TestMethod]
        public void XoaNV_KhongTonTai_TraVeLoi()
        {
            string maAo = "GHOST_9999";
            string err = "";
            bool ketQua = NhanVienBLL.Delete(maAo, out err);

            Assert.IsFalse(ketQua);
            Assert.AreEqual("Nhân viên không tồn tại.", err);
        }

        // TC_DEL_05: Chặn xóa Admin
        [TestMethod]
        public void XoaNV_TaiKhoanAdmin_TraVeLoi()
        {
        
            string maAdmin = "admin";
            string err = "";

            // Act
            bool ketQua = NhanVienBLL.Delete(maAdmin, out err);

            
            Assert.IsFalse(ketQua, "Hệ thống đang cho phép xóa Admin (Lỗ hổng bảo mật)!");

            Assert.IsTrue(err.Contains("Admin") || err.Contains("Không thể xóa"),
                          "Thông báo lỗi chưa đúng: " + err);
        }

        // TC_DEL_06: Xóa nhân viên dính khóa ngoại (Foreign Key)
        [TestMethod]
        public void XoaNV_DinhKhoaNgoai_TraVeLoiTiengViet()
        {
            // Arrange: 'NV01' là mã giả định đã có Hóa đơn/Phiếu nhập
            string maDinhKhoa = "NV01";

            // Act
            string err = "";
            bool ketQua = NhanVienBLL.Delete(maDinhKhoa, out err);

            // Assert
            Assert.IsFalse(ketQua, "Hệ thống phải chặn xóa khi dính khóa ngoại.");

            // Kiểm tra thông báo lỗi tiếng Việt (đã được catch và xử lý ở DAL/BLL)
            bool msgOk = err.Contains("dữ liệu liên quan") || err.Contains("Không thể xóa") || err.Contains("phát sinh");
            Assert.IsTrue(msgOk, $"Thông báo lỗi chưa thân thiện hoặc chưa bắt đúng lỗi SQL: {err}");
        }
    }
}