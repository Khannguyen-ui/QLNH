using DAL_QLNH;
using DTO_QLNH;

using System;
using System.Data;

namespace BLL_QLNH
{
    public class ThucKhachBLL
    {
        private readonly ThucKhachDAL _dal = new ThucKhachDAL();

        public DataTable GetAll() => _dal.GetAll();
        public DataTable Search(string keyword) => _dal.Search(keyword?.Trim() ?? "");

        public bool Insert(ThucKhachDTO t)
        {
            Validate(t, isUpdate: false);
            if (_dal.Exists(t.MaTK))
                throw new ArgumentException($"Mã thực khách '{t.MaTK}' đã tồn tại.");
            return _dal.Insert(t);
        }

        public bool Update(ThucKhachDTO t)
        {
            if (string.IsNullOrWhiteSpace(t.MaTK))
                throw new ArgumentException("Mã thực khách không hợp lệ.");
            Validate(t, isUpdate: true);
            return _dal.Update(t);
        }

        public bool Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                throw new ArgumentException("Mã thực khách không hợp lệ.");
            return _dal.Delete(ma);
        }

        private static void Validate(ThucKhachDTO t, bool isUpdate)
        {
            if (string.IsNullOrWhiteSpace(t.MaTK))
                throw new ArgumentException("Phải nhập Mã thực khách (MATK).");
            if (t.MaTK.Length > 10)
                throw new ArgumentException("Mã thực khách tối đa 10 ký tự.");
            if (string.IsNullOrWhiteSpace(t.TenTK))
                throw new ArgumentException("Phải nhập Tên thực khách.");
            if (t.TenTK.Length > 50)
                throw new ArgumentException("Tên thực khách tối đa 50 ký tự.");
            if (!string.IsNullOrWhiteSpace(t.SoDT) && t.SoDT.Length > 15)
                throw new ArgumentException("Số điện thoại tối đa 15 ký tự.");
            // Có thể bổ sung validate SDT là số nếu cần
        }
    }
}
