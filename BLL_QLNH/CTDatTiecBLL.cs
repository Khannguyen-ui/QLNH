using System;
using System.Data;
using System.Collections.Generic;
using DAL_QLNH;
using DTO_QLNH;

namespace BLL_QLNH
{
    public class CTDatTiecBLL
    {
        private readonly CTDatTiecDAL _dal = new CTDatTiecDAL();

        // ✅ Lấy danh sách theo số phiếu (cho DataGridView)
        public DataTable GetByPhieu(string soPhieu)
        {
            if (string.IsNullOrWhiteSpace(soPhieu))
                throw new ArgumentException("Thiếu số phiếu.");
            return _dal.GetByPhieu(soPhieu.Trim());
        }

      
        public List<MonLookup> LoadMon() => _dal.LoadMon();

        // ✅ Upsert: nếu có rồi → Update, chưa có → Insert
        public bool Upsert(CTDatTiec x)
        {
            Validate(x);

            if (_dal.Exists(x.SoPhieu, x.MaTD))
                return _dal.Update(x);

            return _dal.Insert(x);
        }

        // ✅ Xoá
        public bool Delete(string soPhieu, string maTd)
        {
            if (string.IsNullOrWhiteSpace(soPhieu) || string.IsNullOrWhiteSpace(maTd))
                throw new ArgumentException("Thiếu khóa cần xóa.");

            return _dal.Delete(soPhieu.Trim(), maTd.Trim());
        }

        // ✅ Kiểm tra hợp lệ
        private static void Validate(CTDatTiec x)
        {
            if (string.IsNullOrWhiteSpace(x.SoPhieu))
                throw new ArgumentException("Phải có Số phiếu.");
            if (string.IsNullOrWhiteSpace(x.MaTD))
                throw new ArgumentException("Phải chọn Món.");
            if (x.SoLuong != null && x.SoLuong.Value < 0)
                throw new ArgumentException("Số lượng không hợp lệ.");
            if (x.GiaBan != null && x.GiaBan.Value < 0)
                throw new ArgumentException("Giá bán không hợp lệ.");
        }
    }
}
