using System;

namespace DTO_QLNH
{
    public class DatTiecDTO
    {
        public string SoPhieu { get; set; }          // nvarchar(10) PK
        public DateTime? NgayDk { get; set; }        // datetime
        public string MaTk { get; set; }             // nvarchar(10)
        public string MaNv { get; set; }             // nvarchar(10)
        public int? SoLuongKhach { get; set; }       // int
        public string Phong { get; set; }            // nvarchar(50)
        public string Ca { get; set; }               // nvarchar(20) NEW
    }

    // item dùng cho combobox ThựcKhách / NhânViên
    public class LookItem
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public override string ToString() => Ten;
    }

    // item dùng TreeView cấp 2 (các phiếu theo khách)
    public class PhieuItem
    {
        public string SoPhieu { get; set; }
        public DateTime? NgayDk { get; set; }
        public string Phong { get; set; }
    }

    // item dùng TreeView cấp 3 (các món trong phiếu)
    public class CtMonItem
    {
        public string TenMon { get; set; }
        public double? SoLuong { get; set; }
        public double? GiaBan { get; set; }
    }
}
