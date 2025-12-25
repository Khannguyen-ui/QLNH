namespace DTO_QLNH
{
    public class CTDatTiec
    {
        public string SoPhieu { get; set; }   // nvarchar(10)
        public string MaTD { get; set; }      // nvarchar(10)
        public double? SoLuong { get; set; }  // float
        public double? GiaBan { get; set; }   // float
    }

    // cho combobox Thực đơn
    public class MonLookup
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public double? Gia { get; set; }
        public override string ToString() => Ten;
    }
}
