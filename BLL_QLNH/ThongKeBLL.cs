using System;
using System.Data;
using DAL_QLNH;

namespace BLL_QLNH
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL _dal = new ThongKeDAL();

        public DataTable TkHoaDon(DateTime? from, DateTime? to) { return _dal.TkHoaDon(from, to); }
        public DataTable TkBanAn(DateTime? from, DateTime? to) { return _dal.TkBanAn(from, to); }
        public DataTable TopMon(DateTime? from, DateTime? to, int topN) { return _dal.TopMon(from, to, topN); }
        public decimal TongDoanhThu(DateTime? from, DateTime? to) { return _dal.TongDoanhThu(from, to); }
    }
}
