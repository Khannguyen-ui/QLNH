using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using DAL_QLNH;
using System;
using System.Linq;

namespace TestProject1
{
    [TestClass]
    public class ThucKhachTests
    {
        private ThucKhachBLL _bll;

        [TestInitialize]
        public void Setup()
        {
            _bll = new ThucKhachBLL();
        }

        // =================================================================
        // PHẦN 1: KIỂM TRA RỖNG (MANDATORY FIELDS)
        // =================================================================

        [TestMethod]
        public void ThemTK_MaRong_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "", TenTK = "Nguyen Van A", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Mã rỗng.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Phải nhập Mã thực khách (MATK).", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_TenRong_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Tên rỗng.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Phải nhập Tên thực khách.", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_SDTRong_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT rỗng.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Phải nhập Số điện thoại.", ex.Message); }
        }

        // =================================================================
        // PHẦN 2: KIỂM TRA ĐỊNH DẠNG & KÝ TỰ ĐẶC BIỆT
        // =================================================================

        [TestMethod]
        public void ThemTK_MaChuaKyTuDacBiet_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK@123", TenTK = "Test", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Mã chứa ký tự đặc biệt.");
            }
            catch (ArgumentException ex) { Assert.IsTrue(ex.Message.Contains("không được chứa ký tự đặc biệt")); }
        }

        [TestMethod]
        public void ThemTK_Ma_CoGachDuoi_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK_01", TenTK = "Test", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Mã chứa gạch dưới.");
            }
            catch (ArgumentException ex) { Assert.IsTrue(ex.Message.Contains("không được chứa ký tự đặc biệt")); }
        }

        [TestMethod]
        public void ThemTK_TenChuaSo_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Nguyen A 1", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Tên chứa số.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Tên thực khách không được chứa số.", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_Ten_CoGachDuoi_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Nguyen_A", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Tên chứa gạch dưới.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Tên thực khách không được chứa dấu gạch dưới (_).", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_SDT_ChuaChu_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "0905abc123" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT chứa chữ.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Số điện thoại chỉ được chứa số.", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_SDT_CoGachDuoi_NemLoi()
        {
           
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "090123456_" };

            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT chứa gạch dưới.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Số điện thoại chỉ được chứa số.", ex.Message);
            }
        }


        [TestMethod]
        public void ThemTK_DiaChi_CoGachDuoi_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "0901234567", DiaChi = "Ha_Noi" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Địa chỉ chứa gạch dưới.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Địa chỉ không được chứa dấu gạch dưới (_).", ex.Message); }
        }

        // =================================================================
        // PHẦN 3: KIỂM TRA ĐỘ DÀI BIÊN
        // =================================================================

        [TestMethod]
        public void ThemTK_MaQuaDai_NemLoi()
        {
            string maDai = "TK123456789012"; // 14 ký tự
            var tk = new ThucKhachDTO { MaTK = maDai, TenTK = "Test", SoDT = "0901234567" };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Mã quá dài (>13).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Mã thực khách tối đa 13 ký tự.", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_SDT_QuaNgan_NemLoi()
        {
            string sdtNgan = "123456789"; // 9 số
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = sdtNgan };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT quá ngắn (<10).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Số điện thoại phải từ 10 đến 13 số.", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_SDT_QuaDai_NemLoi()
        {
            string sdtDai = "01234567890123"; // 14 số
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = sdtDai };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT quá dài (>13).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Số điện thoại phải từ 10 đến 13 số.", ex.Message); }
        }

        [TestMethod]
        public void ThemTK_DiaChi_QuaDai_NemLoi()
        {
            string diaChiDai = new string('D', 101); // 101 ký tự
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "0901234567", DiaChi = diaChiDai };
            try
            {
                _bll.Insert(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Địa chỉ quá dài.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Địa chỉ tối đa 100 ký tự.", ex.Message); }
        }

        // =================================================================
        // PHẦN 4: DATABASE & LOGIC NGHIỆP VỤ
        // =================================================================

        [TestMethod]
        public void ThemTK_HopLe_ThanhCong()
        {
            string maTest = "TKTESTOK";
            try { _bll.Delete(maTest); } catch { }

            var tk = new ThucKhachDTO
            {
                MaTK = maTest,
                TenTK = "Nguyen Van A",
                SoDT = "0905123456", // Hợp lệ (10 số)
                DiaChi = "Da Nang"
            };

            bool kq = _bll.Insert(tk);
            Assert.IsTrue(kq, "Lỗi khi thêm thực khách hợp lệ.");

            // Kiểm tra tồn tại
            var dt = _bll.Search(maTest);
            Assert.IsTrue(dt.Rows.Count > 0, "Không tìm thấy khách vừa thêm trong DB.");

            _bll.Delete(maTest);
        }

        [TestMethod]
        public void ThemTK_TrungMa_NemLoi()
        {
            string maTest = "TKDUP";
            try { _bll.Delete(maTest); } catch { }

            _bll.Insert(new ThucKhachDTO { MaTK = maTest, TenTK = "A", SoDT = "0901234567" });

            try
            {
                _bll.Insert(new ThucKhachDTO { MaTK = maTest, TenTK = "B", SoDT = "0901234567" });
                Assert.Fail("Không bắt được lỗi trùng mã.");
            }
            catch (ArgumentException ex) { Assert.IsTrue(ex.Message.Contains("đã tồn tại")); }

            _bll.Delete(maTest);
        }
    }
}