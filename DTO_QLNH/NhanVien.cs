using System;

namespace DTO_QLNH
{
    // DTO thống nhất với GUI: MaNV, TenNV, NoiSinh, NgayLamViec
    public class NhanVien
    {
        public string MaNV { get; set; }          // PK (NVARCHAR(10))
        public string TenNV { get; set; }         // NVARCHAR(50)
        public string NoiSinh { get; set; }       // NVARCHAR(50) NULL
        public DateTime NgayLamViec { get; set; } // SMALLDATETIME NULL represented by DateTime.MinValue

        // (tuỳ DB) – nếu có cột mật khẩu
        public string MatKhau { get; set; }       // NVARCHAR(...) hoặc PasswordHash
    }
}
