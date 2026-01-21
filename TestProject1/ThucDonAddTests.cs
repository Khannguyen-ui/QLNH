using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestProject1
{
    [TestClass]
    public class ThucDonAddTests
    {
        // =================================================================
        // PHẦN 1: KIỂM TRA LOGIC ĐẦU VÀO (VALIDATION)
        // (Mô phỏng lại validation trong FormThucDon: btnLuu_Click & ReadForm)
        // =================================================================

        // TC1: Mã thực đơn rỗng
        [TestMethod]
        public void ThemTD_MaTD_Rong_TraVeLoi()
        {
            ThucDon td = new ThucDon { MaTD = "", TenMon = "Cơm Gà", GiaTien = 30000 };
            string ketQua = ValidateLogic(td);
            Assert.AreEqual("Vui lòng nhập Mã món.", ketQua);
        }

        // TC2: Tên món rỗng
        [TestMethod]
        public void ThemTD_TenMon_Rong_TraVeLoi()
        {
            ThucDon td = new ThucDon { MaTD = "COMGA", TenMon = "", GiaTien = 30000 };
            string ketQua = ValidateLogic(td);
            Assert.AreEqual("Vui lòng nhập Tên món.", ketQua);
        }

        // TC3: Giá tiền âm (Logic nghiệp vụ)
        [TestMethod]
        public void ThemTD_GiaTien_Am_TraVeLoi()
        {
            ThucDon td = new ThucDon { MaTD = "TEST01", TenMon = "Món Test", GiaTien = -5000 };
            string ketQua = ValidateLogic(td);
            Assert.AreEqual("Giá tiền không được nhỏ hơn 0.", ketQua);
        }

        // TC4: Số lượng tồn âm (Logic nghiệp vụ)
        [TestMethod]
        public void ThemTD_SoLuongTon_Am_TraVeLoi()
        {
            ThucDon td = new ThucDon { MaTD = "TEST01", TenMon = "Món Test", GiaTien = 10000, SoLuongTon = -1 };
            string ketQua = ValidateLogic(td);
            Assert.AreEqual("Số lượng tồn không được nhỏ hơn 0.", ketQua);
        }

        // TC5: Mã thực đơn quá dài (> 20 ký tự - Giả định Database varchar(20))
        [TestMethod]
        public void ThemTD_MaTD_QuaDai_TraVeLoi()
        {
            ThucDon td = new ThucDon
            {
                MaTD = "TD01234567890123456789", // 22 ký tự
                TenMon = "Test",
                GiaTien = 1000
            };
            string ketQua = ValidateLogic(td);
            Assert.IsTrue(ketQua.Contains("không được vượt quá 20 ký tự"));
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA TƯƠNG TÁC DỮ LIỆU (DATABASE)
        // =================================================================

        // TC6: Thêm món ăn thành công (Dữ liệu chuẩn)
       
        [TestMethod]
        public void ThemTD_DuLieuHopLe_ThemThanhCong()
        {
            // 1. Arrange
            // SỬA Ở ĐÂY: Rút ngắn mã xuống dưới 10 ký tự
            string maTest = "TEST_01";

            // Xóa trước để đảm bảo sạch dữ liệu
            ThucDonBLL.Delete(maTest);

            ThucDon td = new ThucDon
            {
                MaTD = maTest,
                TenMon = "Cafe Test Unit",
                DVT = "Ly",
                GiaTien = 25000,
                GhiChu = "Test Data",
                SoLuongTon = 100
            };

            // 2. Act
            bool ketQua = ThucDonBLL.Insert(td);

            // 3. Assert
            Assert.IsTrue(ketQua, "Lẽ ra phải thêm thành công món mới.");

            // Dọn dẹp sau khi test
            ThucDonBLL.Delete(maTest);
        }

        // TC7: Thêm món bị trùng Mã (Duplicate PK)
        // Kiểm tra xem BLL/DAL xử lý việc trùng khóa chính như thế nào
        [TestMethod]
        public void ThemTD_TrungMaTD_TraVeFalse_Hoac_Exception()
        {
            // 1. Arrange: Tạo sẵn 1 món
            string maTest = "TD_DUP";
            ThucDon td1 = new ThucDon { MaTD = maTest, TenMon = "Món Gốc", GiaTien = 10000 };

            // Xóa cũ (nếu có) rồi thêm mới để đảm bảo td1 tồn tại
            ThucDonBLL.Delete(maTest);
            ThucDonBLL.Insert(td1);

            // 2. Act: Cố tình Insert lại mã đó (Gọi thẳng Insert chứ không qua logic Update của Form)
            ThucDon td2 = new ThucDon { MaTD = maTest, TenMon = "Món Trùng", GiaTien = 20000 };

            try
            {
                bool ketQua = ThucDonBLL.Insert(td2);

                // Nếu code DAL bắt try-catch và trả về false:
                Assert.IsFalse(ketQua, "Không được phép thêm trùng Mã thực đơn.");
            }
            catch (Exception)
            {
                // Nếu code DAL ném lỗi SQL ra ngoài:
                // Test Pass vì hệ thống DB đã chặn lại
                Assert.IsTrue(true);
            }

            // Dọn dẹp
            ThucDonBLL.Delete(maTest);
        }

        // =================================================================
        // PHẦN 3: CÁC TEST CASE NÂNG CAO (EDGE CASES & SQL INJECTION)
        // =================================================================

        // TC8: Giá tiền bằng 0 (Món miễn phí/Khuyến mãi) -> Hợp lệ
        [TestMethod]
        public void ThemTD_GiaTienBang0_ThemThanhCong()
        {
            string maTest = "TD_FREE";
            ThucDonBLL.Delete(maTest);

            ThucDon td = new ThucDon
            {
                MaTD = maTest,
                TenMon = "Trà Đá Miễn Phí",
                GiaTien = 0, // Giá = 0
                DVT = "Ly"
            };

            bool ketQua = ThucDonBLL.Insert(td);
            Assert.IsTrue(ketQua, "Hệ thống phải cho phép món ăn có giá = 0 (miễn phí).");

            ThucDonBLL.Delete(maTest);
        }

        // TC9: SQL Injection trong Tên món (Ví dụ: Pizza ' Special)
        // Kiểm tra xem DAL có dùng SqlParameter không
        [TestMethod]
        public void ThemTD_TenMon_ChuaKyTuDacBiet_SQLInjection_ThemThanhCong()
        {
            string maTest = "TDSQLINJ";
            ThucDonBLL.Delete(maTest);

            // Tên chứa dấu nháy đơn (') thường gây lỗi nếu nối chuỗi SQL
            string tenHack = "Pizza 'Deluxe' Special";

            ThucDon td = new ThucDon
            {
                MaTD = maTest,
                TenMon = tenHack,
                GiaTien = 150000
            };

            bool ketQua = ThucDonBLL.Insert(td);

            // [NẾU CODE DAL DÙNG PARAMETER -> PASS]
            // [NẾU CODE DAL NỐI CHUỖI -> FAIL]
            Assert.IsTrue(ketQua, "Lỗi: Hệ thống không xử lý được tên món có dấu nháy đơn (SQL Injection check)!");

            // Verify: Đọc lại xem tên có đúng không
            var list = ThucDonBLL.Search(maTest);
            var item = list.FirstOrDefault(x => x.MaTD == maTest);
            Assert.IsNotNull(item);
            Assert.AreEqual(tenHack, item.TenMon, "Tên món lưu vào DB bị sai lệch.");

            // Cleanup
            ThucDonBLL.Delete(maTest);
        }

        // TC10: Kiểm tra độ chính xác của kiểu Float (Giá tiền lẻ)
        [TestMethod]
        public void ThemTD_GiaTien_SoLe_LuuDung()
        {
            string maTest = "TD_FLOAT";
            ThucDonBLL.Delete(maTest);

            // Giả sử bán theo Gram hoặc USD
            float giaLe = 10.5f;

            ThucDon td = new ThucDon
            {
                MaTD = maTest,
                TenMon = "Test Float",
                GiaTien = giaLe
            };

            ThucDonBLL.Insert(td);

            // Kiểm tra đọc ra
            var result = ThucDonBLL.GetAll().FirstOrDefault(x => x.MaTD == maTest);

            // So sánh số thực phải có độ lệch cho phép (delta)
            Assert.AreEqual(giaLe, result.GiaTien, 0.01, "Giá tiền lưu số lẻ không chính xác.");

            ThucDonBLL.Delete(maTest);
        }

        // -----------------------------------------------------------------
        // HÀM MÔ PHỎNG LOGIC (Helper Method)
        // Tự viết dựa trên logic nên có của Form ThucDon
        // -----------------------------------------------------------------
        private string ValidateLogic(ThucDon td)
        {
            // 1. Mã món
            if (string.IsNullOrWhiteSpace(td.MaTD)) return "Vui lòng nhập Mã món.";
            if (td.MaTD.Length > 20) return "Mã thực đơn không được vượt quá 20 ký tự";

            // 2. Tên món
            if (string.IsNullOrWhiteSpace(td.TenMon)) return "Vui lòng nhập Tên món.";

            // 3. Giá tiền (Trong Form là parse từ chuỗi, ở đây check giá trị số)
            if (td.GiaTien < 0) return "Giá tiền không được nhỏ hơn 0.";

            // 4. Số lượng tồn
            if (td.SoLuongTon < 0) return "Số lượng tồn không được nhỏ hơn 0.";

            return "OK";
        }
        // =================================================================
        // PHẦN 4: KIỂM TRA DANH MỤC (DỮ LIỆU ĐỔ VÀO COMBOBOX DVT)
        // =================================================================

        // TC11: Kiểm tra lấy danh sách Đơn vị tính từ Database
        [TestMethod]
        public void ThemTD_LoadDVT_TraVeDanhSachChinhXac()
        {
            // 1. Act: Gọi hàm lấy danh sách DVT
            var dsDVT = ThucDonBLL.GetListDVT();

            // 2. Assert: Danh sách không được rỗng
            Assert.IsNotNull(dsDVT, "Danh sách đơn vị tính không được null.");
            Assert.IsTrue(dsDVT.Count > 0, "Cơ sở dữ liệu bảng DonViTinh đang trống.");

            // Kiểm tra xem có chứa các đơn vị cơ bản bạn đã INSERT không
            Assert.IsTrue(dsDVT.Contains("Ly") || dsDVT.Contains("Đĩa"), "Thiếu dữ liệu đơn vị tính cơ bản.");
        }

        // TC12: Kiểm tra tính toàn vẹn khi gán DVT vào món ăn
        [TestMethod]
        public void ThemTD_GánDVT_LưuThànhCông()
        {
            string maTest = "TD_DVT_01";
            ThucDonBLL.Delete(maTest);

            // Giả lập chọn đơn vị tính từ ComboBox (lấy cái đầu tiên trong DB)
            string dvtChon = ThucDonBLL.GetListDVT().FirstOrDefault() ?? "Phần";

            ThucDon td = new ThucDon
            {
                MaTD = maTest,
                TenMon = "Món Test DVT",
                DVT = dvtChon, // Gán đơn vị tính từ ComboBox
                GiaTien = 10000
            };

            bool ketQua = ThucDonBLL.Insert(td);
            Assert.IsTrue(ketQua, "Không lưu được món ăn khi chọn đơn vị tính từ danh mục.");

            ThucDonBLL.Delete(maTest);
        }
    }
}