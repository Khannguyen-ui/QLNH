using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class NhanVienUpdateTests
    {
        // =================================================================
        // PHẦN 1: KIỂM TRA LOGIC ĐẦU VÀO (VALIDATION)
        // (Mô phỏng lại validation trên Form trước khi Sửa)
        // =================================================================

        // TC_UPDATE_01: Sửa nhân viên nhưng Mã NV rỗng (Lỗi hệ thống không truyền mã)
        [TestMethod]
        public void SuaNV_MaNV_Rong_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "", TenNV = "Test Update", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.AreEqual("Vui lòng chọn nhân viên cần sửa.", ketQua);
        }

        // TC_UPDATE_02: Sửa nhân viên có Mã bắt đầu bằng số (Check tính hợp lệ của ID đầu vào)
        [TestMethod]
        public void SuaNV_MaNV_BatDauBangSo_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "1NV", TenNV = "Test Update", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("phải BẮT ĐẦU bằng chữ cái"));
        }

        // TC_UPDATE_03: Sửa nhân viên có Mã chứa ký tự đặc biệt
        [TestMethod]
        public void SuaNV_MaNV_ChuaKyTuDacBiet_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV@1", TenNV = "Test Update", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("không được chứa ký tự đặc biệt"));
        }

        // TC_UPDATE_04: Sửa nhân viên có Mã quá dài
        [TestMethod]
        public void SuaNV_MaNV_QuaDai_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV01234567890123456789", TenNV = "Test", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("không được vượt quá 20 ký tự"));
        }

        // TC_UPDATE_05: Sửa nhân viên nhưng để Tên rỗng
        [TestMethod]
        public void SuaNV_TenNV_Rong_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV01", TenNV = "", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.AreEqual("Vui lòng nhập Tên nhân viên.", ketQua);
        }

        // TC_UPDATE_06: Sửa tên nhân viên chứa số
        [TestMethod]
        public void SuaNV_TenNV_ChuaSo_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV01", TenNV = "Nguyen Van A 1", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.IsTrue(ketQua.Contains("không được chứa số hoặc ký tự đặc biệt"));
        }

        // TC_UPDATE_07: Sửa nhân viên thiếu thông tin bắt buộc (Nơi sinh, Mật khẩu...)
        [TestMethod]
        public void SuaNV_ThieuNoiSinh_TraVeLoi()
        {
            NhanVien nv = new NhanVien { MaNV = "NV01", TenNV = "Nguyen Van B", NoiSinh = "", MatKhau = "123" };
            string ketQua = ValidateLogic(nv);
            Assert.AreEqual("Vui lòng nhập Nơi sinh.", ketQua);
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA TƯƠNG TÁC DỮ LIỆU (DATABASE)
        // =================================================================

        // TC_UPDATE_08: Sửa nhân viên thành công (Dữ liệu hợp lệ)
        [TestMethod]
        public void SuaNV_DuLieuHopLe_CapNhatThanhCong()
        {
            // 1. Arrange
            string maTest = "NV_UPD_OK";
            NhanVienBLL.Delete(maTest, out _); // Reset
            NhanVien nvGoc = new NhanVien { MaNV = maTest, TenNV = "Ten Cu", NoiSinh = "Hue", MatKhau = "111" };
            NhanVienBLL.Insert(nvGoc, out _);

            NhanVien nvMoi = new NhanVien { MaNV = maTest, TenNV = "Ten Moi", NoiSinh = "Da Nang", MatKhau = "222" };

            // 2. Act
            string err = "";
            bool ketQua = NhanVienBLL.Update(nvMoi, out err);

            // 3. Assert
            Assert.IsTrue(ketQua, "Lỗi: Không cập nhật được dù dữ liệu đúng. Chi tiết: " + err);

            // Cleanup
            NhanVienBLL.Delete(maTest, out _);
        }

        // TC_UPDATE_09: Sửa nhân viên không tồn tại (Sai Mã NV)
        // (Tương ứng với Add trùng mã, thì Update là Sai mã)
        [TestMethod]
        public void SuaNV_MaNV_KhongTonTai_TraVeFalse()
        {
            string maAo = "NV_NOT_EXIST";
            NhanVien nv = new NhanVien { MaNV = maAo, TenNV = "Test", NoiSinh = "HCM", MatKhau = "123" };

            string err = "";
            bool ketQua = NhanVienBLL.Update(nv, out err);

            Assert.IsFalse(ketQua, "Hệ thống phải báo lỗi vì Mã NV không tồn tại trong DB.");
        }

        // =================================================================
        // PHẦN 3: CÁC TEST CASE NÂNG CAO (SẼ FAIL NẾU CODE CHƯA FIX)
        // =================================================================

        // TC_UPDATE_10: Sửa tên thành toàn dấu cách
        [TestMethod]
        public void SuaNV_NhapToanKhoangTrang_TraVeLoi()
        {
            string maTest = "NV_SPACE";
            NhanVienBLL.Delete(maTest, out _);
            NhanVien nvGoc = new NhanVien { MaNV = maTest, TenNV = "User Old", NoiSinh = "A", MatKhau = "1" };
            NhanVienBLL.Insert(nvGoc, out _);

            NhanVien nvSua = new NhanVien { MaNV = maTest, TenNV = "   ", NoiSinh = "   ", MatKhau = "123" };

            string err = "";
            bool ketQua = NhanVienBLL.Update(nvSua, out err);

            // [SỬA ĐỂ FAIL]: Nếu BLL không Trim() dữ liệu, nó sẽ Update thành công -> Test Fail.
            // Kỳ vọng: Hệ thống phải nhận ra đây là dữ liệu rỗng và trả về thông báo lỗi.
            Assert.AreEqual("Vui lòng nhập Tên nhân viên.", err, "Lỗi: Hệ thống cho phép sửa thành tên trắng!");

            NhanVienBLL.Delete(maTest, out _);
        }

        // TC_UPDATE_11: Độ dài biên khi sửa (Tên > 100 ký tự)
        [TestMethod]
        public void SuaNV_TenNV_QuaDai_TraVeLoi()
        {
            string maTest = "NV_LONG";
            NhanVienBLL.Delete(maTest, out _);
            NhanVien nvGoc = new NhanVien { MaNV = maTest, TenNV = "User Old", NoiSinh = "A", MatKhau = "1" };
            NhanVienBLL.Insert(nvGoc, out _);

            string tenDai = new string('A', 101);
            NhanVien nvSua = new NhanVien { MaNV = maTest, TenNV = tenDai, MatKhau = "123" };

            string err = "";
            NhanVienBLL.Update(nvSua, out err);

            // [SỬA ĐỂ FAIL]: Nếu BLL ném Exception SQL -> Test Fail.
            // Kỳ vọng: Thông báo tiếng Việt thân thiện.
            Assert.AreEqual("Tên nhân viên không được vượt quá 100 ký tự", err);

            NhanVienBLL.Delete(maTest, out _);
        }

        // TC_UPDATE_12: SQL Injection khi Sửa (Tên chứa dấu nháy đơn O'Neil)
        [TestMethod]
        public void SuaNV_TenNV_ChuaKyTuGayLoiSQL_TraVeFalse_Hoac_XuLyDung()
        {
            string maTest = "NV_SQL";
            NhanVienBLL.Delete(maTest, out _);
            NhanVien nvGoc = new NhanVien { MaNV = maTest, TenNV = "User Old", NoiSinh = "A", MatKhau = "1" };
            NhanVienBLL.Insert(nvGoc, out _);

            // Tên O'Neil là hợp lệ nhưng hay gây lỗi SQL
            string tenHack = "O'Neil";
            NhanVien nvSua = new NhanVien { MaNV = maTest, TenNV = tenHack, NoiSinh = "Test", MatKhau = "123" };

            string err = "";
            bool ketQua = NhanVienBLL.Update(nvSua, out err);

            // [SỬA ĐỂ FAIL]: Nếu code nối chuỗi SQL thủ công -> Crash/False -> Test Fail.
            // Kỳ vọng: Update thành công (True).
            Assert.IsTrue(ketQua, "Lỗi: Hệ thống không cho phép sửa tên có dấu nháy đơn (O'Neil)!");

            if (ketQua) NhanVienBLL.Delete(maTest, out _);
        }

        // TC_UPDATE_13: Sửa ngày làm việc thành quá xa (Năm 2200)
        [TestMethod]
        public void SuaNV_NgayLamViec_TuongLai_TraVeFalse()
        {
            string maTest = "NV_FUTURE";
            NhanVienBLL.Delete(maTest, out _);
            NhanVien nvGoc = new NhanVien { MaNV = maTest, TenNV = "User Old", NoiSinh = "A", MatKhau = "1" };
            NhanVienBLL.Insert(nvGoc, out _);

            NhanVien nvSua = new NhanVien
            {
                MaNV = maTest,
                TenNV = "Future User",
                NgayLamViec = new DateTime(2200, 1, 1), 
                MatKhau = "123"
            };

            string err = "";
            NhanVienBLL.Update(nvSua, out err);

           
            Assert.AreEqual("Ngày làm việc không hợp lệ (quá tương lai)", err);

            NhanVienBLL.Delete(maTest, out _);
        }

        // -----------------------------------------------------------------
        // HÀM HELPER MÔ PHỎNG LOGIC VALIDATION
        // -----------------------------------------------------------------
        private string ValidateLogic(NhanVien nv)
        {
            // Logic check Mã NV (Dù sửa nhưng vẫn cần validate input đầu vào)
            if (string.IsNullOrWhiteSpace(nv.MaNV)) return "Vui lòng chọn nhân viên cần sửa.";
            if (!char.IsLetter(nv.MaNV[0])) return "Mã nhân viên phải BẮT ĐẦU bằng chữ cái";
            if (nv.MaNV.Any(c => !char.IsLetterOrDigit(c))) return "Mã nhân viên không được chứa ký tự đặc biệt";
            if (nv.MaNV.Length > 20) return "Mã nhân viên không được vượt quá 20 ký tự";

            // Logic check Tên NV
            if (string.IsNullOrWhiteSpace(nv.TenNV)) return "Vui lòng nhập Tên nhân viên.";
            if (nv.TenNV.Length > 100) return "Tên nhân viên không được vượt quá 100 ký tự";
            if (nv.TenNV.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c))) return "Tên nhân viên không được chứa số hoặc ký tự đặc biệt";

            // Logic check thông tin khác
            if (string.IsNullOrWhiteSpace(nv.NoiSinh)) return "Vui lòng nhập Nơi sinh.";
            if (string.IsNullOrWhiteSpace(nv.MatKhau)) return "Vui lòng nhập Mật khẩu.";

            return "OK";
        }
    }
}