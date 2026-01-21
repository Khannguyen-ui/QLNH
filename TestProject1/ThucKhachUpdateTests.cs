using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Data;

namespace TestProject1
{
    [TestClass]
    public class ThucKhachUpdateTests
    {
        private ThucKhachBLL _bll;

        [TestInitialize]
        public void Setup()
        {
            _bll = new ThucKhachBLL();
        }

        // =================================================================
        // NHÓM 1: KIỂM TRA RỖNG
        // =================================================================

        [TestMethod]
        public void SuaTK_MaRong_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "", TenTK = "Ten Hop Le", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Mã rỗng.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Mã thực khách không hợp lệ.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_TenRong_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Tên rỗng.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Phải nhập Tên thực khách.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_SDTRong_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Ten Hop Le", SoDT = "" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi SĐT rỗng.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Phải nhập Số điện thoại.", ex.Message); }
        }

        // =================================================================
        // NHÓM 2: KIỂM TRA ĐỊNH DẠNG & KÝ TỰ ĐẶC BIỆT
        // =================================================================

        [TestMethod]
        public void SuaTK_MaChuaKyTuDacBiet_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK@123", TenTK = "Test", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Mã chứa ký tự đặc biệt.");
            }
            catch (ArgumentException ex) { Assert.IsTrue(ex.Message.Contains("không được chứa ký tự đặc biệt")); }
        }

        [TestMethod]
        public void SuaTK_Ma_CoGachDuoi_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK_01", TenTK = "Test", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Mã chứa gạch dưới.");
            }
            catch (ArgumentException ex) { Assert.IsTrue(ex.Message.Contains("không được chứa ký tự đặc biệt")); }
        }

        [TestMethod]
        public void SuaTK_TenChuaSo_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Nguyen A 1", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Tên chứa số.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Tên thực khách không được chứa số.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_Ten_CoGachDuoi_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Nguyen_A", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Tên chứa gạch dưới.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Tên thực khách không được chứa dấu gạch dưới (_).", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_SDT_ChuaChu_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "0905abc123" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT chứa chữ.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Số điện thoại chỉ được chứa số.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_SDT_CoGachDuoi_NemLoi()
        {
            
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "090123456_" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi SĐT chứa gạch dưới.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Số điện thoại chỉ được chứa số.", ex.Message);
            }
        }

        [TestMethod]
        public void SuaTK_DiaChi_CoGachDuoi_NemLoi()
        {
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "0901234567", DiaChi = "Ha_Noi" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Lẽ ra phải ném lỗi khi Địa chỉ chứa gạch dưới.");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Địa chỉ không được chứa dấu gạch dưới (_).", ex.Message); }
        }

        // =================================================================
        // NHÓM 3: KIỂM TRA ĐỘ DÀI BIÊN
        // =================================================================

        [TestMethod]
        public void SuaTK_MaQuaDai_NemLoi()
        {
            string maDai = "TK123456789012"; // 14 ký tự
            var tk = new ThucKhachDTO { MaTK = maDai, TenTK = "Test", SoDT = "0901234567" };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Mã quá dài (>13).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Mã thực khách tối đa 13 ký tự.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_SDT_QuaNgan_NemLoi()
        {
            string sdtNgan = "123456789"; // 9 số
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = sdtNgan };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi SĐT quá ngắn (<10).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Số điện thoại phải từ 10 đến 13 số.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_SDT_QuaDai_NemLoi()
        {
            string sdtDai = "01234567890123"; // 14 số
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = sdtDai };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi SĐT quá dài (>13).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Số điện thoại phải từ 10 đến 13 số.", ex.Message); }
        }

        [TestMethod]
        public void SuaTK_DiaChi_QuaDai_NemLoi()
        {
            string diaChiDai = new string('D', 101); // 101 ký tự
            var tk = new ThucKhachDTO { MaTK = "TK01", TenTK = "Test", SoDT = "0901234567", DiaChi = diaChiDai };
            try
            {
                _bll.Update(tk);
                Assert.Fail("Không bắt lỗi Địa chỉ quá dài (>100).");
            }
            catch (ArgumentException ex) { Assert.AreEqual("Địa chỉ tối đa 100 ký tự.", ex.Message); }
        }

        // =================================================================
        // NHÓM 4: DATABASE & LOGIC NGHIỆP VỤ
        // =================================================================

        [TestMethod]
        public void SuaTK_DuLieuHopLe_ThanhCong()
        {
            string maTest = "TKUPDFULL";
            try { _bll.Delete(maTest); } catch { }

            _bll.Insert(new ThucKhachDTO { MaTK = maTest, TenTK = "Ten Cu", SoDT = "0901234567", DiaChi = "Hue" });

            var tkMoi = new ThucKhachDTO
            {
                MaTK = maTest,
                TenTK = "Ten Moi",
                SoDT = "0901234567890", // 13 số (Hợp lệ)
                DiaChi = "Da Nang"
            };

            bool ketQua = _bll.Update(tkMoi);
            Assert.IsTrue(ketQua, "Update thất bại dù dữ liệu hợp lệ.");

            DataTable dt = _bll.Search(maTest);
            Assert.AreEqual("Ten Moi", dt.Rows[0]["TENTK"].ToString());
            Assert.AreEqual("0901234567890", dt.Rows[0]["SODT"].ToString());

            _bll.Delete(maTest);
        }

        [TestMethod]
        public void SuaTK_KhongTonTai_TraVeFalse()
        {
            string maAo = "TKGHOST99";
            var tk = new ThucKhachDTO { MaTK = maAo, TenTK = "Test", SoDT = "0901234567" };

            bool ketQua = _bll.Update(tk);
            Assert.IsFalse(ketQua, "Phải trả về False khi Mã TK không tồn tại.");
        }
    }
}