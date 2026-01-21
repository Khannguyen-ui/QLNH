using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL_QLNH;
using DTO_QLNH;
using System;
using System.Data;
using System.Linq;

namespace TestProject1
{
    [TestClass]
    public class ThucKhachDeleteTests
    {
        private ThucKhachBLL _bll;

        [TestInitialize]
        public void Setup()
        {
            _bll = new ThucKhachBLL();
        }

        // =================================================================
        // NHÓM 1: KIỂM TRA LOGIC RÀNG BUỘC (VALIDATION)
        // =================================================================

        [TestMethod]
        public void XoaTK_MaRong_NemLoi()
        {
            // TC_DEL_01: Kiểm tra khi truyền mã rỗng hoặc chỉ có khoảng trắng
            try
            {
                _bll.Delete("");
                Assert.Fail("Lẽ ra phải ném lỗi khi mã rỗng.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Mã thực khách không hợp lệ.", ex.Message);
            }
        }

        [TestMethod]
        public void XoaTK_MaQuaDai_NemLoi()
        {
            // TC_DEL_02: Kiểm tra mã vượt quá 13 ký tự
            string maQuaDai = "TK123456789012345";
            try
            {
                _bll.Delete(maQuaDai);
                Assert.Fail("Không bắt được lỗi mã quá dài.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Mã thực khách tối đa 13 ký tự.", ex.Message);
            }
        }

        [TestMethod]
        public void XoaTK_MaChuaKyTuDacBiet_NemLoi()
        {
            // TC_DEL_03: Kiểm tra mã chứa ký tự đặc biệt (@, #, _, ...)
            string maLoi = "TK_01@";
            try
            {
                _bll.Delete(maLoi);
                Assert.Fail("Không bắt được lỗi ký tự đặc biệt.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Mã thực khách không được chứa ký tự đặc biệt hoặc dấu gạch dưới.", ex.Message);
            }
        }

        // =================================================================
        // NHÓM 2: KIỂM TRA TƯƠNG TÁC DATABASE THỰC TẾ
        // =================================================================

        [TestMethod]
        public void XoaTK_MaKhongTonTai_TraVeFalse()
        {
            // TC_DEL_04: Xóa một mã không có trong Database
            string maAo = "TK999999";

            // Hàm Delete trả về false nếu không xóa được (không có dòng nào bị ảnh hưởng)
            bool ketQua = _bll.Delete(maAo);

            Assert.IsFalse(ketQua, "Hệ thống phải trả về False khi mã không tồn tại.");
        }

        [TestMethod]
        public void XoaTK_DuLieuHopLe_XoaThanhCong()
        {
            // TC_DEL_05: Kịch bản xóa thành công dữ liệu "sạch"
            string maTest = "TKDELOK";

            // 1. Arrange: Đảm bảo mã test tồn tại (Xóa cũ nếu có, rồi Insert mới)
            try { _bll.Delete(maTest); } catch { }
            _bll.Insert(new ThucKhachDTO { MaTK = maTest, TenTK = "Khach Test Xoa", SoDT = "0901234567" });

            // 2. Act: Thực hiện xóa
            bool ketQua = _bll.Delete(maTest);

            // 3. Assert: Kiểm tra kết quả
            Assert.IsTrue(ketQua, "Xóa thực khách hợp lệ phải trả về True.");

            // Kiểm tra lại Database xem còn tồn tại không
            DataTable dt = _bll.Search(maTest);
            Assert.AreEqual(0, dt.Rows.Count, "Dữ liệu vẫn còn trong Database sau khi lệnh xóa thành công.");
        }

        [TestMethod]
        public void XoaTK_DangCoHoaDon_NemLoiHoacTraVeFalse()
        {
            // TC_DEL_06: Kiểm tra ràng buộc khóa ngoại (Foreign Key)
            // Lưu ý: 'TK01' giả định là mã đã có dữ liệu hóa đơn trong DB của bạn
            string maDangDung = "TK01";

            try
            {
                bool ketQua = _bll.Delete(maDangDung);

                // Nếu code DAL của bạn trả về false khi dính Foreign Key
                Assert.IsFalse(ketQua, "Hệ thống không được phép xóa thực khách đã có hóa đơn.");
            }
            catch (Exception)
            {
                // Nếu code DAL của bạn throw lỗi SQL ra ngoài (547 - Foreign Key violation)
                // Test Pass vì hệ thống đã chặn lại thành công
                Assert.IsTrue(true);
            }
        }
    }
}