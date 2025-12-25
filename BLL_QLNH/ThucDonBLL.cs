using System.Collections.Generic;
using System.Linq;
using DTO_QLNH;
using DAL_QLDT;

namespace BLL_QLNH
{
    public class ThucDonBLL
    {
        public static List<ThucDon> GetAll()
        {
            return ThucDonDAL.GetAll();
        }

        public static bool Insert(ThucDon td)
        {
            return ThucDonDAL.Insert(td);
        }

        public static bool Update(ThucDon td)
        {
            return ThucDonDAL.Update(td);
        }

        public static bool Delete(string maTD)
        {
            return ThucDonDAL.Delete(maTD);
        }

        // Thêm phương thức Search
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
    }
}