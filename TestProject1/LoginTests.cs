using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;

namespace TestProject1
{
    [TestClass]
    public class TaiKhoanTests
    {
        // =============================================================
        // NHÓM 1: TEST ĐĂNG NHẬP THẤT BẠI (VALIDATION & DATA)
        // =============================================================

        // TC_01: Nhập sai mật khẩu
        [TestMethod]
        public void DangNhap_DungTaiKhoan_SaiMatKhau_TraVeFalse()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "admin";
            tk.MatKhau = "mat_khau_sai_123";

            string vaiTro = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTro, out hoTen);

            Assert.IsFalse(ketQua);
        }

        // TC_02: Nhập sai tài khoản (Tài khoản không tồn tại)
        // Đây chính là hàm bạn vừa yêu cầu sửa đổi
        [TestMethod]
        public void DangNhap_TaiKhoanKhongTonTai_TraVeFalse()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "user9999"; // Tên này chắc chắn không có trong DB
            tk.MatKhau = "123";

            string vaiTro = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTro, out hoTen);

            Assert.IsFalse(ketQua, "Lỗi: Tài khoản không tồn tại !");
        }

        // TC_03: Bỏ trống Tên đăng nhập
        [TestMethod]
        public void DangNhap_BoTrongTenDangNhap_TraVeFalse()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "";
            tk.MatKhau = "123";

            string vaiTro = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTro, out hoTen);

            Assert.IsFalse(ketQua);
        }

        // TC_04: Bỏ trống Mật khẩu
        [TestMethod]
        public void DangNhap_BoTrongMatKhau_TraVeFalse()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "admin";
            tk.MatKhau = "";

            string vaiTro = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTro, out hoTen);

            Assert.IsFalse(ketQua);
        }

        // TC_05: Bỏ trống cả hai
        [TestMethod]
        public void DangNhap_BoTrongCaHai_TraVeFalse()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "";
            tk.MatKhau = "";

            string vaiTro = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTro, out hoTen);

            Assert.IsFalse(ketQua);
        }

        // =============================================================
        // NHÓM 2: TEST ĐĂNG NHẬP THÀNH CÔNG (PHÂN QUYỀN)
        // =============================================================

        // TC_06: Admin đăng nhập đúng
        [TestMethod]
        public void DangNhap_Admin_TaiKhoanKhaDung_TraVeTrue_Va_DungQuyen()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "admin";
            tk.MatKhau = "123";

            string vaiTroMongDoi = "Admin";
            string vaiTroThucTe = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTroThucTe, out hoTen);

            Assert.IsTrue(ketQua, "Đăng nhập Admin thất bại");
            Assert.AreEqual(vaiTroMongDoi, vaiTroThucTe, "Sai quyền Admin");
        }

        // TC_07: Nhân viên đăng nhập đúng
        [TestMethod]
        public void DangNhap_NhanVien_TaiKhoanKhaDung_TraVeTrue_Va_DungQuyen()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "NV01";
            tk.MatKhau = "123";

            string vaiTroMongDoi = "NhanVien";
            string vaiTroThucTe = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTroThucTe, out hoTen);

            Assert.IsTrue(ketQua, "Đăng nhập Nhân viên thất bại");
            Assert.AreEqual(vaiTroMongDoi, vaiTroThucTe, "Sai quyền Nhân viên");
        }

        // =============================================================
        // NHÓM 3: TEST KIỂM TRA CHÉO QUYỀN (SECURITY)
        // =============================================================

        // TC_09: Admin không được nhận nhầm là Nhân viên
        [TestMethod]
        public void DangNhap_Admin_KiemTraQuyen_KhongPhaiNhanVien()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "admin";
            tk.MatKhau = "123";

            string quyenNhanVien = "NhanVien";
            string vaiTroThucTe = "";
            string hoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTroThucTe, out hoTen);

            Assert.IsTrue(ketQua);
            Assert.AreNotEqual(quyenNhanVien, vaiTroThucTe);
        }

        // TC_10: Nhân viên không được nhận nhầm là Admin
        [TestMethod]
        public void DangNhap_NhanVien_KiemTraQuyen_KhongPhaiAdmin()
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TenDangNhap = "NV01";
            tk.MatKhau = "123";

            string quyenAdmin = "Admin";
            string vaiTroThucTe = "";
            // Sử dụng cú pháp tương thích cũ nếu out _ bị lỗi
            string dummyHoTen = "";

            bool ketQua = TaiKhoanBLL.DangNhapTaiKhoan(tk, out vaiTroThucTe, out dummyHoTen);

            Assert.IsTrue(ketQua);
            Assert.AreNotEqual(quyenAdmin, vaiTroThucTe);
        }
    }
}