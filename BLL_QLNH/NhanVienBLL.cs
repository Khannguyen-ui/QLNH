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

        // Trong BLL_QLNH/NhanVienBLL.cs

        public static bool Delete(string maNV, out string error)
        {
            error = null;

            // 1. Kiểm tra Rỗng/Null (TC_DEL_01, TC_DEL_02)
            if (string.IsNullOrWhiteSpace(maNV))
            {
                error = "Thiếu mã nhân viên.";
                return false;
            }

            // 2. [CẬP NHẬT] ĐƯA LÊN TRƯỚC: Chặn xóa Admin (TC_DEL_05)
            // Phải chặn ngay từ khóa "admin" trước khi kiểm tra trong DB
            if (maNV.Trim().Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                error = "Không thể xóa tài khoản Admin hệ thống.";
                return false;
            }

            // 3. Kiểm tra tồn tại (TC_DEL_04)
            // Chỉ kiểm tra tồn tại nếu mã đó KHÔNG PHẢI là admin
            var exists = GetByMaNV(maNV);
            if (exists == null)
            {
                error = "Nhân viên không tồn tại.";
                return false;
            }

            // 4. Gọi DAL xóa (TC_DEL_03 và TC_DEL_06)
            try
            {
                bool deleted = NhanVienDAL.Delete(maNV);

                // Kiểm tra lại lần nữa cho chắc (trường hợp Delete trả về true nhưng rows=0)
                // Tuy nhiên với logic DAL trên thì deleted luôn là true trừ khi Exception
                return deleted;
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ DAL ném lên (bao gồm lỗi khóa ngoại 547)
                error = ex.Message;
                return false;
            }
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
