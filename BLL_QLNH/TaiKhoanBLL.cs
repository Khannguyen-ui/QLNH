using DAL_QLDT;
using DAL_QLNH;
using DTO_QLNH;

namespace BLL_QLNH
{
    public class TaiKhoanBLL
    {
        // Đăng nhập Admin / Quản lý
        public static bool DangNhapTaiKhoan(TaiKhoan tk, out string vaiTro, out string hoTen)
        {
            return TaiKhoanDAL.KiemTraDangNhap(tk, out vaiTro, out hoTen);
        }

        // Lấy mật khẩu (chỉ dùng cho autofill trong môi trường dev)
        public static string GetPasswordByUser(string tenDangNhap)
        {
            return TaiKhoanDAL.GetPasswordByUser(tenDangNhap);
        }
    }
}
