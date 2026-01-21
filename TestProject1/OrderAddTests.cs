using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL_QLNH; // Sử dụng BLL thật
using DAL_QLNH; // Sử dụng DAL thật
using DTO_QLNH; // Sử dụng DTO thật

namespace TestProject1
{
    [TestClass]
    public class OrderAddTests
    {
        // =================================================================
        // DTO HELPER
        // =================================================================
        public class OrderInput
        {
            public string MaNV { get; set; }
            public bool IsOrderLe { get; set; }
            public string MaTK { get; set; }
            public string Phong { get; set; }
            public string Ca { get; set; }
            public List<ChiTietMon> Items { get; set; } = new List<ChiTietMon>();
            public decimal GiamGiaPercent { get; set; }
        }

        public class ChiTietMon
        {
            public string MaTD { get; set; }
            public int SLDat { get; set; }
            public int SLTon { get; set; }
            public decimal Gia { get; set; }
        }

        // =================================================================
        // PHẦN 1: LOGIC VALIDATION (Không đổi)
        // =================================================================

        [TestMethod]
        public void ThemPhieu_KhongChonMon_TraVeLoi()
        {
            var input = new OrderInput { MaNV = "NV01", Items = new List<ChiTietMon>() };
            string ketQua = ValidateLogic(input);
            Assert.AreEqual("Chưa chọn món nào.", ketQua);
        }

        [TestMethod]
        public void ThemPhieu_ThieuMaNV_TraVeLoi()
        {
            var input = new OrderInput
            {
                MaNV = "",
                Items = new List<ChiTietMon> { new ChiTietMon { MaTD = "M01", SLDat = 1 } }
            };
            string ketQua = ValidateLogic(input);
            Assert.AreEqual("Thiếu mã nhân viên.", ketQua);
        }

        [TestMethod]
        public void ThemPhieu_DatTiec_ThieuKhach_TraVeLoi()
        {
            var input = new OrderInput
            {
                MaNV = "NV01",
                IsOrderLe = false, // Đặt tiệc
                MaTK = "",
                Items = new List<ChiTietMon> { new ChiTietMon { MaTD = "M01", SLDat = 1 } }
            };
            string ketQua = ValidateLogic(input);
            Assert.AreEqual("Chọn khách hoặc nhập tên khách.", ketQua);
        }

        [TestMethod]
        public void ThemPhieu_DatTiec_ThieuPhong_TraVeLoi()
        {
            var input = new OrderInput
            {
                MaNV = "NV01",
                IsOrderLe = false,
                MaTK = "KH01",
                Phong = "",
                Items = new List<ChiTietMon> { new ChiTietMon { MaTD = "M01", SLDat = 1 } }
            };
            string ketQua = ValidateLogic(input);
            Assert.AreEqual("Chọn Phòng/Sảnh.", ketQua);
        }

        [TestMethod]
        public void ThemPhieu_OrderLe_KhongCanPhong_TraVeOK()
        {
            var input = new OrderInput
            {
                MaNV = "NV01",
                IsOrderLe = true,
                Phong = "",
                Ca = "",
                Items = new List<ChiTietMon> { new ChiTietMon { MaTD = "M01", SLDat = 1 } }
            };
            string ketQua = ValidateLogic(input);
            Assert.AreEqual("OK", ketQua);
        }

        // =================================================================
        // PHẦN 2: LOGIC TÍNH TOÁN (Không đổi)
        // =================================================================

        [TestMethod]
        public void ThemPhieu_QuaTonKho_TraVeLoi()
        {
            var input = new OrderInput
            {
                Items = new List<ChiTietMon>
                {
                    new ChiTietMon { MaTD = "M01", SLDat = 10, SLTon = 5 }
                }
            };
            string ketQua = CheckTonKho(input);
            Assert.IsTrue(ketQua.Contains("Không đủ tồn kho"), "Hệ thống phải chặn khi đặt quá số lượng tồn.");
        }

        [TestMethod]
        public void TinhTien_KhongGiamGia_TraVeDung()
        {
            var input = new OrderInput
            {
                GiamGiaPercent = 0,
                Items = new List<ChiTietMon>
                {
                    new ChiTietMon { Gia = 10000, SLDat = 2 }, // 20k
                    new ChiTietMon { Gia = 5000, SLDat = 1 }   // 5k
                }
            };
            decimal total = CalculateTotal(input);
            Assert.AreEqual(25000m, total);
        }

        [TestMethod]
        public void TinhTien_GiamGia10Percent_TraVeDung()
        {
            var input = new OrderInput
            {
                GiamGiaPercent = 10,
                Items = new List<ChiTietMon>
                {
                    new ChiTietMon { Gia = 100000, SLDat = 1 }
                }
            };
            decimal total = CalculateTotal(input);
            Assert.AreEqual(90000m, total);
        }

        // =================================================================
        // PHẦN 3: TEST DATABASE (ĐÃ SỬA LỖI ĐỘ DÀI CHUỖI)
        // =================================================================

        // TC_ADD_07: Check trùng lịch
        [TestMethod]
        public void ThemPhieu_TrungLich_TraVeFalse()
        {
            // 1. Arrange
            // [FIXED]: Rút ngắn SoPhieu (vd: BUSY_5912345) ~ 12 ký tự
            string soPhieu = "BUSY_" + DateTime.Now.ToString("mmssfff");
            string phong = "P_TEST_FULL";
            string ca = "Ca Sang";
            DateTime ngay = DateTime.Today;

            DatTiecBLL bll = new DatTiecBLL();
            string err = "";

            // Xóa trước để chắc chắn
            bll.Delete(soPhieu);

            // Chèn 1 phiếu đặt trước để chiếm chỗ
            // Lưu ý: Đảm bảo MaNV, MaTK tồn tại trong DB, nếu không sẽ lỗi khoá ngoại
            bll.ThemDatTiec(soPhieu, ngay, "TK01", "NV01", 1, phong, ca, out err);

            // 2. Act: Kiểm tra xem phòng đó có hợp lệ (trống) không
            bool isHopLe = bll.PhongCaHopLe(ngay, phong, ca);

            // 3. Assert
            Assert.IsFalse(isHopLe, "Hệ thống phải trả về False (Không hợp lệ) vì phòng đã có người đặt.");

            // Cleanup
            bll.Delete(soPhieu);
        }

        // TC_ADD_10: Thêm phiếu thành công & Kiểm tra tồn kho
        [TestMethod]
        public void ThemPhieu_DuLieuHopLe_ThemThanhCong()
        {
            // 1. Arrange
            // [FIXED]: Rút ngắn SoPhieu (vd: OK_5912345) ~ 10 ký tự
            string soPhieu = "OK_" + DateTime.Now.ToString("mmssfff");
            string maTDTest = "MON_TEST";
            int tonKhoBanDau = 100;
            int soLuongDat = 1;

            // Đảm bảo món ăn tồn tại (Dùng DAL thật)
            EnsureThucDonExist(maTDTest, tonKhoBanDau);

            DatTiecBLL bll = new DatTiecBLL();
            string err = "";

            // 2. Act
            // Bước A: Thêm phiếu đặt tiệc (Dùng BLL thật)
            bool ketQua = bll.ThemDatTiec(soPhieu, DateTime.Now, "TK01", "NV01", 1, "Ban Le", "Tai Cho", out err);

            // Bước B: Mô phỏng hành động trừ tồn kho của Form
            if (ketQua)
            {
                ThucDonDAL.UpdateStock(maTDTest, tonKhoBanDau - soLuongDat);
            }

            // 3. Assert
            Assert.IsTrue(ketQua, "Lẽ ra phải thêm phiếu thành công. Lỗi: " + err);

            // Check DB: Tồn kho phải giảm còn 99
            var listMon = ThucDonBLL.Search(maTDTest);
            var monDb = listMon.FirstOrDefault(x => x.MaTD == maTDTest);

            Assert.IsNotNull(monDb, "Không tìm thấy món ăn trong DB.");
            Assert.AreEqual(99, monDb.SoLuongTon, "Tồn kho chưa được trừ đúng!");

            // Cleanup
            bll.Delete(soPhieu);
        }
        // TC_ADD_12: Kiểm tra lỗi khi Số phiếu quá dài (Edge Case)
        // KẾT QUẢ DỰ KIẾN: FAIL (Do code BLL chưa chặn độ dài, bị SQL ném Exception)
        [TestMethod]
        public void ThemPhieu_SoPhieuQuaDai_TraVeLoi()
        {
            // 1. Arrange
            // Tạo chuỗi dài 50 ký tự (Giả sử DB chỉ cho phép 20)
            string soPhieuDai = "TEST_OVERFLOW_" + new string('X', 50);
            DatTiecBLL bll = new DatTiecBLL();
            string err = "";

            // 2. Act
            // Nếu code BLL không kiểm tra độ dài, dòng này sẽ văng Exception từ SQL
            bool ketQua = false;
            try
            {
                ketQua = bll.ThemDatTiec(soPhieuDai, DateTime.Now, "TK01", "NV01", 1, "Phong Vip", "Ca Sang", out err);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                // Nếu chạy vào đây tức là Test đã phát hiện ra lỗi hệ thống (Crash)
                Assert.Fail("Lỗi: Hệ thống bị Crash do SQL Exception (Chưa validate độ dài chuỗi). Chi tiết: " + ex.Message);
            }

            // 3. Assert (Kỳ vọng code phải xử lý êm đẹp)
            Assert.IsFalse(ketQua, "Hệ thống phải trả về False khi dữ liệu quá dài.");
            Assert.AreEqual("Số phiếu không được vượt quá 20 ký tự.", err, "Thông báo lỗi không đúng mong đợi.");
        }

        // -----------------------------------------------------------------
        // HÀM HELPER 
        // -----------------------------------------------------------------

        private string ValidateLogic(OrderInput input)
        {
            if (input.Items == null || input.Items.Count == 0) return "Chưa chọn món nào.";
            if (string.IsNullOrWhiteSpace(input.MaNV)) return "Thiếu mã nhân viên.";
            if (!input.IsOrderLe)
            {
                if (string.IsNullOrWhiteSpace(input.MaTK)) return "Chọn khách hoặc nhập tên khách.";
                if (string.IsNullOrWhiteSpace(input.Phong)) return "Chọn Phòng/Sảnh.";
                if (string.IsNullOrWhiteSpace(input.Ca)) return "Chọn Ca phục vụ.";
            }
            return "OK";
        }

        private string CheckTonKho(OrderInput input)
        {
            foreach (var item in input.Items)
            {
                if (item.SLDat > item.SLTon) return $"Không đủ tồn kho cho món {item.MaTD}.";
            }
            return "OK";
        }

        private decimal CalculateTotal(OrderInput input)
        {
            decimal sum = 0;
            foreach (var item in input.Items) sum += item.Gia * item.SLDat;
            decimal giamGia = input.GiamGiaPercent;
            if (giamGia < 0) giamGia = 0;
            if (giamGia > 100) giamGia = 100;
            return sum * (1 - giamGia / 100m);
        }

        private void EnsureThucDonExist(string ma, int sl)
        {
            var list = ThucDonBLL.Search(ma);
            if (list.Count == 0)
            {
                // Dùng tên ngắn cho Mã
                ThucDonDAL.Insert(new ThucDon { MaTD = ma, TenMon = "TestMon", GiaTien = 5000, SoLuongTon = sl, DVT = "Cai" });
            }
            else
            {
                ThucDonDAL.UpdateStock(ma, sl);
            }
        }
    }
}