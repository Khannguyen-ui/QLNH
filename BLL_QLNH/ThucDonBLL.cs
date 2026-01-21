using System;
using System.Collections.Generic;
using System.Linq;
using DTO_QLNH;
using DAL_QLNH;

namespace BLL_QLNH
{
    public class ThucDonBLL
    {
        private static void ValidateThucDon(ThucDon td)
        {
            if (string.IsNullOrWhiteSpace(td.MaTD))
                throw new Exception("Vui lòng nhập Mã món.");

            if (string.IsNullOrWhiteSpace(td.TenMon))
                throw new Exception("Vui lòng nhập Tên món.");

            if (td.MaTD.Length > 10)
                throw new Exception("Mã thực đơn không được vượt quá 10 ký tự.");

            if (td.GiaTien < 0)
                throw new Exception("Giá tiền không được nhỏ hơn 0.");

            if (td.SoLuongTon < 0)
                throw new Exception("Số lượng tồn không được nhỏ hơn 0.");

            if (td.GhiChu != null && td.GhiChu.Length > 100)
                throw new Exception("Ghi chú quá dài (tối đa 100 ký tự).");
                      
            if (td.TenMon.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c) && c != '\''))
            {
                throw new Exception("Tên món không được chứa ký tự đặc biệt (ngoại trừ dấu nháy đơn).");
            }
        }

        public static List<ThucDon> GetAll()
        {
            return ThucDonDAL.GetAll();
        }

        // --- CẬP NHẬT HÀM INSERT: CHẶN TRÙNG MÃ ---
        public static bool Insert(ThucDon td)
        {
            // 1. Kiểm tra các logic cơ bản (rỗng, độ dài, số âm)
            ValidateThucDon(td);

            // 2. KIỂM TRA TRÙNG MÃ: 
            // Lấy tất cả dữ liệu và tìm xem mã đã tồn tại chưa
            var all = GetAll();
            bool isExisted = all.Any(x => x.MaTD.Trim().Equals(td.MaTD.Trim(), StringComparison.OrdinalIgnoreCase));

            if (isExisted)
            {
                throw new Exception($"Mã món '{td.MaTD}' đã tồn tại trong hệ thống. Vui lòng nhập mã khác.");
            }

            // 3. Nếu không trùng mới cho phép gọi xuống DAL để Insert
            return ThucDonDAL.Insert(td);
        }

        public static bool Update(ThucDon td)
        {
            ValidateThucDon(td);
            return ThucDonDAL.Update(td);
        }

        public static bool Delete(string maTD)
        {
            if (string.IsNullOrWhiteSpace(maTD)) return false;
            return ThucDonDAL.Delete(maTD);
        }

        public static List<ThucDon> Search(string keyword)
        {
            var all = GetAll();
            if (string.IsNullOrWhiteSpace(keyword)) return all;
            keyword = keyword.ToLower();
            return all.Where(td =>
                (td.MaTD != null && td.MaTD.ToLower().Contains(keyword)) ||
                (td.TenMon != null && td.TenMon.ToLower().Contains(keyword)) ||
                (td.DVT != null && td.DVT.ToLower().Contains(keyword)) ||
                (td.GhiChu != null && td.GhiChu.ToLower().Contains(keyword)) ||
                td.GiaTien.ToString().Contains(keyword)
            ).ToList();
        }
        // Thêm hàm này vào trong class ThucDonBLL
        public static List<string> GetListDVT()
        {
            return ThucDonDAL.GetListDVT();
        }
    }
}