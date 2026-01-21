using System;
using System.Data;
using System.Text.RegularExpressions;
using DAL_QLNH;

namespace BLL_QLNH
{
    /// <summary>
    /// Business layer cho module Hoá đơn.
    /// Chỉ thực hiện: chuẩn hoá tham số, ràng buộc đầu vào, uỷ quyền xuống DAL.
    /// Không tính toán tiền chi tiết (để DAL/DB đảm nhận).
    /// </summary>
    public class HoaDonBLL
    {
        private readonly HoaDonDAL _dal = new HoaDonDAL();

        // ======= Constants / Options =======
        public const string TRANG_THAI_UNPAID = "UNPAID";
        public const string TRANG_THAI_PAID = "PAID";
        public const string TRANG_THAI_CHO_TT = "CHO_TT";

        /// <summary>
        /// Nếu true: bắt buộc truyền nvtt (mã NV thực hiện) khi tạo/ensure HĐ.
        /// Nếu false: cho phép rỗng và để DAL/DB tự xử lý (trigger, default).
        /// </summary>
        public bool RequireNvttForCreate { get; set; } = false;

        // ======= Helper chuẩn hoá =======
        private static string NormalizeSoPhieu(string soPhieu)
        {
            if (string.IsNullOrWhiteSpace(soPhieu)) return string.Empty;
            var s = soPhieu.Trim().ToUpperInvariant();

            // Nếu bạn có pattern chuẩn cho SoPhieu (ví dụ: P + số), có thể kiểm tra:
            // ví dụ: P20231101xxxxx -> P\d+
            // Không fail cứng, chỉ trả về normalize để tránh phá flow GUI.
            // Regex.IsMatch(s, @"^P\d+$")

            return s;
        }

        private static decimal ClampGiamPt(decimal giamPt)
        {
            if (giamPt < 0m) return 0m;
            if (giamPt > 100m) return 100m;
            return giamPt;
        }

        private void EnsureNvttIfRequired(string nvtt)
        {
            if (!RequireNvttForCreate) return;
            if (string.IsNullOrWhiteSpace(nvtt))
                throw new ArgumentException("Thiếu mã nhân viên (NVTT) cho thao tác tạo/đảm bảo hoá đơn.");
        }

        // ======= Truy vấn chung =======
        /// <summary>Tìm kiếm danh sách hoá đơn theo keyword và/hoặc khoảng ngày.</summary>
        public DataTable Search(string keyword, DateTime? from, DateTime? to)
        {
            return _dal.Search(keyword?.Trim() ?? string.Empty, from, to);
        }

        /// <summary>Lấy chi tiết hoá đơn/phiếu theo số phiếu.</summary>
        public DataTable GetChiTietHoaDon(string soPhieu)
        {
            var sp = NormalizeSoPhieu(soPhieu);
            if (string.IsNullOrEmpty(sp)) return new DataTable();
            return _dal.GetChiTietHoaDon(sp);
        }

        // expose top mon
        public DataTable GetTopMon(DateTime? from, DateTime? to, int top = 10)
        {
            return _dal.GetTopMon(from, to, top);
        }

        // ======= Danh sách CHỜ THANH TOÁN =======
        /// <summary>Danh sách phiếu/hóa đơn ở trạng thái chờ thanh toán (lưới FormLapThanhToan).</summary>
        
        public DataTable ListChoThanhToan(string keyword = "", DateTime? from = null, DateTime? to = null, string maNV = "")
        {
            // Truyền thêm tham số maNV xuống DAL
            return _dal.ListChoThanhToan(keyword?.Trim() ?? string.Empty, from, to, maNV);
        }

        // ======= Lấy hoá đơn theo SỐ PHIẾU =======
        public DataTable GetBySoPhieu(string soPhieu)
        {
            var sp = NormalizeSoPhieu(soPhieu);
            if (string.IsNullOrEmpty(sp)) return new DataTable();
            return _dal.GetBySoPhieu(sp);
        }

        // ======= Tạo / Ensure hoá đơn =======
        /// <summary>
        /// Tạo hoá đơn UNPAID từ số phiếu. Trả về MaHD (rỗng nếu lỗi).
        /// </summary>
        public string CreateFromSoPhieu(string soPhieu, decimal giamPT, string nvtt)
        {
            var sp = NormalizeSoPhieu(soPhieu);
            if (string.IsNullOrEmpty(sp)) return string.Empty;

            EnsureNvttIfRequired(nvtt);

            var g = ClampGiamPt(giamPT);
            return _dal.CreateFromSoPhieu(sp, g, nvtt?.Trim());
        }

        /// <summary>
        /// Bảo đảm tồn tại hoá đơn UNPAID cho số phiếu (nếu có rồi trả về MaHD hiện có).
        /// </summary>
        public string EnsureUnpaidHoaDon(string soPhieu, decimal giamPT, string nvtt)
        {
            var sp = NormalizeSoPhieu(soPhieu);
            if (string.IsNullOrEmpty(sp)) return string.Empty;

            EnsureNvttIfRequired(nvtt);

            var g = ClampGiamPt(giamPT);
            return _dal.EnsureUnpaidHoaDon(sp, g, nvtt?.Trim());
        }

        // ======= Đánh dấu ĐÃ THANH TOÁN =======
        /// <summary>
        /// Đánh dấu PAID theo MaHD; trả số dòng ảnh hưởng (>=1 là OK).
        /// Nên đồng bộ DatTiec.TrangThai = 'DA_TT' ở DAL/DB.
        /// </summary>
        public int SetPaid(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD)) return 0;
            return _dal.SetPaid(maHD.Trim());
        }

        // ======= Bổ trợ tuỳ chọn (không bắt buộc GUI dùng) =======

        /// <summary>
        /// Thử tạo (hoặc lấy) hoá đơn UNPAID cho số phiếu; trả true/false thay vì ném lỗi.
        /// </summary>
        public bool TryEnsureUnpaid(string soPhieu, decimal giamPT, string nvtt, out string maHD, out string error)
        {
            maHD = string.Empty;
            error = string.Empty;
            try
            {
                maHD = EnsureUnpaidHoaDon(soPhieu, giamPT, nvtt);
                if (string.IsNullOrWhiteSpace(maHD))
                {
                    error = "Không tạo/khôi phục được hoá đơn UNPAID.";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Thử đánh dấu PAID; trả true nếu cập nhật >=1 dòng.
        /// </summary>
        public bool TrySetPaid(string maHD, out string error)
        {
            error = string.Empty;
            try
            {
                var n = SetPaid(maHD);
                if (n <= 0)
                {
                    error = "Không có dòng nào được cập nhật.";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

    }
}
