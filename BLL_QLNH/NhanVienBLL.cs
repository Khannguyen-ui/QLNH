using System;
using System.Collections.Generic;
using System.Linq;
using DAL_QLNH;
using DTO_QLNH;

namespace BLL_QLNH
{
    /// <summary>
    /// Nghiệp vụ Nhân Viên: validate dữ liệu và gọi DAL.
    /// Dùng từ mã nguồn hiện có bằng các phương thức static.
    /// </summary>
    public static class NhanVienBLL
    {
        // ================== API PUBLIC (static để tương thích code cũ) ==================
        public static NhanVien GetByMaNV(string maNV)
        {
            return NhanVienDAL.GetByMaNV(maNV);
        }

        public static List<NhanVien> GetAll()
        {
            return NhanVienDAL.GetAll();
        }

        public static bool Insert(NhanVien nv)
        {
            string err;
            return Insert(nv, out err);
        }

        public static bool Insert(NhanVien nv, out string error)
        {
            error = null;
            if (!ValidateBasic(nv, requireName: true, out error)) return false;
            try { return NhanVienDAL.Insert(nv); }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public static bool Update(NhanVien nv)
        {
            string err;
            return Update(nv, out err);
        }

        public static bool Update(NhanVien nv, out string error)
        {
            error = null;
            if (!ValidateBasic(nv, requireName: false, out error)) return false;
            try { return NhanVienDAL.Update(nv); }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public static bool Delete(string maNV)
        {
            string err;
            return Delete(maNV, out err);
        }

        public static bool Delete(string maNV, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(maNV)) { error = "Thiếu mã nhân viên."; return false; }
            try { return NhanVienDAL.Delete(maNV); }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        /// <summary>
        /// Cập nhật thông tin cơ bản (yêu cầu có họ tên).
        /// </summary>
        public static bool UpdateBasic(NhanVien nv)
        {
            string err;
            return UpdateBasic(nv, out err);
        }

        public static bool UpdateBasic(NhanVien nv, out string error)
        {
            error = null;
            if (!ValidateBasic(nv, requireName: true, out error)) return false;
            try { return NhanVienDAL.Update(nv); }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        /// <summary>
        /// Đổi mật khẩu theo cột MatKhau (plain-text).
        /// </summary>
        public static bool ChangePassword(string maNV, string currentPassword, string newPassword)
        {
            string err;
            return ChangePassword(maNV, currentPassword, newPassword, out err);
        }

        public static bool ChangePassword(string maNV, string currentPassword, string newPassword, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(maNV)) { error = "Thiếu mã nhân viên."; return false; }
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6) { error = "Mật khẩu mới tối thiểu6 ký tự."; return false; }
            try
            {
                var ok = NhanVienDAL.ChangePassword(maNV, currentPassword, newPassword);
                if (!ok) error = "Mật khẩu hiện tại không đúng.";
                return ok;
            }
            catch (Exception ex) { error = ex.Message; return false; }
        }

        public static List<NhanVien> Search(string keyword)
        {
            var all = GetAll();
            keyword = (keyword ?? "").ToLowerInvariant();

            return all.Where(nv =>
                   (nv.MaNV ?? "").ToLowerInvariant().Contains(keyword)
                || (nv.TenNV ?? "").ToLowerInvariant().Contains(keyword)
                || (nv.NoiSinh ?? "").ToLowerInvariant().Contains(keyword))
                .ToList();
        }

        public static List<NhanVien> SearchOnServer(string keyword)
        {
            return NhanVienDAL.SearchOnServer(keyword);
        }

        public static bool DangNhapNhanVien(string maNv, string pass, out string tenNv)
        {
            return NhanVienDAL.TryLoginNhanVien(maNv, pass, out tenNv);
        }

        public static string GetPasswordByMaNV(string maNv)
        {
            return NhanVienDAL.GetPasswordByMaNV(maNv);
        }

        // ================== VALIDATION ==================
        private static bool ValidateBasic(NhanVien nv, bool requireName, out string error)
        {
            error = null;
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV))
            {
                error = "Thiếu mã nhân viên.";
                return false;
            }
            if (requireName && string.IsNullOrWhiteSpace(nv.TenNV))
            {
                error = "Họ tên không được để trống.";
                return false;
            }
            return true;
        }
    }
}
