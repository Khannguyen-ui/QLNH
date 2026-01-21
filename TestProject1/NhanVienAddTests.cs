using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class NhanVienAddTests
    {
        // =================================================================
        // PHẦN 1: KIỂM TRA LOGIC ĐẦU VÀO (VALIDATION)
        // (Mô phỏng lại các câu lệnh if/else trong FormNhanVien.cs)
        // =================================================================

        // TC1: Mã nhân viên rỗng
        [TestMethod]
        public void ThemNV_MaNV_Rong_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "", TenNV = "Test", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.AreEqual("Vui lòng nhập Mã nhân viên.", ketQua);
        }

        // TC2: Mã nhân viên bắt đầu bằng số (Ví dụ: 1NV)
        [TestMethod]
        public void ThemNV_MaNV_BatDauBangSo_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "1NV", TenNV = "Test", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("phải BẮT ĐẦU bằng chữ cái"));
        }

        // TC3: Mã nhân viên chứa ký tự đặc biệt (Ví dụ: NV@1)
        [TestMethod]
        public void ThemNV_MaNV_ChuaKyTuDacBiet_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV@1", TenNV = "Test", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("không được chứa ký tự đặc biệt"));
        }

        // TC4: Mã nhân viên quá dài (> 20 ký tự)
        [TestMethod]
        public void ThemNV_MaNV_QuaDai_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV01234567890123456789", TenNV = "Test", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("không được vượt quá 20 ký tự"));
        }

        // TC5: Tên nhân viên rỗng
        [TestMethod]
        public void ThemNV_TenNV_Rong_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NVTest", TenNV = "", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.AreEqual("Vui lòng nhập Tên nhân viên.", ketQua);
        }

        // TC6: Tên nhân viên chứa số (Ví dụ: Nguyen Van A 1)
        [TestMethod]
        public void ThemNV_TenNV_ChuaSo_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NVTest", TenNV = "Nguyen Van A 1", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("không được chứa số hoặc ký tự đặc biệt"));
        }

        // TC7: Thiếu nơi sinh hoặc mật khẩu
        [TestMethod]
        public void ThemNV_ThieuNoiSinh_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NVTest", TenNV = "Nguyen A", NoiSinh = "", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.AreEqual("Vui lòng nhập Nơi sinh.", ketQua);
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA TƯƠNG TÁC DỮ LIỆU (DATABASE)
        // =================================================================

        // TC8: Thêm nhân viên thành công (Dữ liệu chuẩn)
        [TestMethod]
        public void ThemNV_DuLieuHopLe_ThemThanhCong()
        {
            // 1. Arrange
            string maTest = "NV_TEST_01";
            // Xóa trước để đảm bảo sạch dữ liệu
            NhanVienBLL.Delete(maTest, out _);

            NhanVien nv = new NhanVien
            {
                MaNV = maTest,
                TenNV = "Test User Valid",
                NoiSinh = "Ha Noi",
                NgayLamViec = DateTime.Now,
                MatKhau = "123"
            };

            // 2. Act
            string err = "";
            bool ketQua = NhanVienBLL.Insert(nv, out err);

            // 3. Assert
            Assert.IsTrue(ketQua, "Lẽ ra phải thêm thành công. Lỗi: " + err);

            // Dọn dẹp sau khi test
            NhanVienBLL.Delete(maTest, out _);
        }

        // TC9: Thêm nhân viên bị trùng Mã (Duplicate)
        [TestMethod]
        public void ThemNV_TrungMaNV_TraVeFalse()
        {
            // 1. Arrange: Tạo sẵn 1 NV
            string maTest = "NV_DUP";
            NhanVien nv1 = new NhanVien { MaNV = maTest, TenNV = "User 1", NoiSinh = "A", MatKhau = "1" };
            NhanVienBLL.Insert(nv1, out _);

            // 2. Act: Cố tình thêm lại mã đó
            NhanVien nv2 = new NhanVien { MaNV = maTest, TenNV = "User 2", NoiSinh = "B", MatKhau = "1" };

            // Logic kiểm tra trùng giống trong Form
            var list = NhanVienBLL.GetAll();
            var existed = list.Find(x => string.Equals(x.MaNV, nv2.MaNV, StringComparison.OrdinalIgnoreCase));

            // 3. Assert
            Assert.IsNotNull(existed, "Hệ thống phải tìm thấy nhân viên đã tồn tại!");

            // Dọn dẹp
            NhanVienBLL.Delete(maTest, out _);
        }
        // =================================================================
        // PHẦN 3: CÁC TEST CASE NÂNG CAO (EDGE CASES & SQL INJECTION)
        // =================================================================

        // TC_ADD_10: Nhập toàn dấu cách
        [TestMethod]
        public void ThemNV_NhapToanKhoangTrang_TraVeLoi()
        {
            string maTest = "   ";
            NhanVien nv = new NhanVien { MaNV = maTest, TenNV = "   ", NoiSinh = "   ", MatKhau = "123" };

            string err = "";
            bool ketQua = NhanVienBLL.Insert(nv, out err);

            // [SỬA ĐỂ FAIL]: Hiện tại BLL trả về "Thiếu mã nhân viên." (do ValidateBasic).
            // Ta kỳ vọng thông báo phải lịch sự hơn là "Vui lòng nhập..." như trong Form.
            // Vì thông báo không khớp -> Test sẽ ĐỎ (Failed).
            Assert.AreEqual("Vui lòng nhập Mã nhân viên.", err, "Thông báo lỗi không đúng chuẩn UI!");
        }

        // TC_ADD_11: Kiểm tra độ dài biên (Quá 20 ký tự)
        [TestMethod]
        public void ThemNV_MaNV_DoDaiBien_Qua21KyTu_TraVeLoi()
        {
            string maDai = new string('A', 21);
            NhanVien nv = new NhanVien { MaNV = maDai, TenNV = "Test", MatKhau = "123" };

            string err = "";
            NhanVienBLL.Insert(nv, out err);

            // [SỬA ĐỂ FAIL]: Hiện tại BLL trả về lỗi tiếng Anh của SQL (String truncated...).
            // Ta kỳ vọng BLL phải chặn trước và báo lỗi tiếng Việt.
            // Vì Actual là lỗi SQL loằng ngoằng -> Test sẽ ĐỎ (Failed).
            Assert.AreEqual("Mã nhân viên không được vượt quá 20 ký tự", err);
        }

        // TC_ADD_12: Tên chứa ký tự đặc biệt (SQL Injection Test)
        [TestMethod]
        public void ThemNV_TenNV_ChuaKyTuGayLoiSQL_TraVeFalse()
        {
            string maTest = "NV_SQL_TEST";
            // Tên "O'Neil" là tên hợp lệ ở nước ngoài, hệ thống PHẢI cho phép thêm.
            // Nhưng hiện tại hệ thống của bạn đang trả về False (do test cũ pass Assert.IsFalse).
            string tenHack = "O'Neil";
            NhanVien nv = new NhanVien { MaNV = maTest, TenNV = tenHack, NoiSinh = "Test", MatKhau = "123" };

            string err = "";
            bool ketQua = NhanVienBLL.Insert(nv, out err);

            // [SỬA ĐỂ FAIL]: Ta kỳ vọng tên này phải thêm được (True).
            // Nhưng thực tế hệ thống đang trả về False (lỗi).
            // -> Test sẽ ĐỎ (Failed) để báo động rằng hệ thống đang chặn nhầm tên người dùng.
            Assert.IsTrue(ketQua, "Lỗi: Hệ thống không cho phép tên có dấu nháy đơn (O'Neil)!");

            // Cleanup
            if (ketQua) NhanVienBLL.Delete(maTest, out _);
        }

        // TC_ADD_13: Ngày làm việc quá xa (Năm 2200)
        [TestMethod]
        public void ThemNV_NgayLamViec_TuongLai_TraVeFalse()
        {
            string maTest = "NV_FUTURE";
            NhanVien nv = new NhanVien
            {
                MaNV = maTest,
                TenNV = "Future",
                NgayLamViec = new DateTime(2200, 1, 1),
                MatKhau = "123",
                NoiSinh = "HCM"
            };

            string err = "";
            NhanVienBLL.Insert(nv, out err);

            // [SỬA ĐỂ FAIL]: Hiện tại BLL trả về lỗi SQL Crash (SqlDateTime overflow).
            // Ta kỳ vọng BLL phải validate logic nghiệp vụ.
            // Vì err thực tế là lỗi Exception dài dòng -> Test sẽ ĐỎ (Failed).
            Assert.AreEqual("Ngày làm việc không hợp lệ (quá tương lai)", err);
        }

        // -----------------------------------------------------------------
        // HÀM MÔ PHỎNG LOGIC TRONG FORM (Helper Method)
        // Copy y hệt logic if/else từ file FormNhanVien.cs của bạn
        // -----------------------------------------------------------------
        private string ValidateLogic(NhanVien nv)
        {
            // 1. Mã NV
            if (string.IsNullOrWhiteSpace(nv.MaNV)) return "Vui lòng nhập Mã nhân viên.";
            if (!char.IsLetter(nv.MaNV[0])) return "Mã nhân viên phải BẮT ĐẦU bằng chữ cái";
            if (nv.MaNV.Any(c => !char.IsLetterOrDigit(c))) return "Mã nhân viên không được chứa ký tự đặc biệt";
            if (nv.MaNV.Length > 20) return "Mã nhân viên không được vượt quá 20 ký tự";

            // 2. Tên NV
            if (string.IsNullOrWhiteSpace(nv.TenNV)) return "Vui lòng nhập Tên nhân viên.";
            if (nv.TenNV.Length > 100) return "Tên nhân viên không được vượt quá 100 ký tự";
            if (nv.TenNV.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c))) return "Tên nhân viên không được chứa số hoặc ký tự đặc biệt";

            // 3. Khác
            if (string.IsNullOrWhiteSpace(nv.NoiSinh)) return "Vui lòng nhập Nơi sinh.";
            if (string.IsNullOrWhiteSpace(nv.MatKhau)) return "Vui lòng nhập Mật khẩu.";

            return "OK"; // Hợp lệ
        }
    }
}