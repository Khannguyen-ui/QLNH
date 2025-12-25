using System;
using System.Collections.Generic;
using System.Data;
using DAL_QLNH;
using DTO_QLNH;

namespace BLL_QLNH
{
    public class DatTiecBLL
    {
        private readonly DatTiecDAL _dal = new DatTiecDAL();

        /* =============== GRID CHÍNH =============== */

        public DataTable GetAll() => _dal.GetAll();

        public DataTable Search(string keyword) => _dal.Search(keyword);

        public bool Delete(string soPhieu) => _dal.Delete(soPhieu);

        /* =============== COMBOBOX NGUỒN DỮ LIỆU =============== */

        public List<LookItem> GetThucKhach() => _dal.GetThucKhach();

        public List<LookItem> GetNhanVien() => _dal.GetNhanVien();

        /* =============== TREEVIEW / CHI TIẾT =============== */

        public DataTable GetByMatk(string matk) => _dal.GetByMatk(matk);

        public List<PhieuItem> GetPhieuByMatk(string matk) => _dal.GetPhieuByMatk(matk);

        public List<CtMonItem> GetCtBySoPhieu(string soPhieu) => _dal.GetCtBySoPhieu(soPhieu);

        // Lấy 1 phiếu (đổ lại form khi mở để thanh toán)
        public DataRow GetBySoPhieu(string soPhieu) => _dal.GetBySoPhieu(soPhieu);

        /* =============== LOGIC PHÒNG / CA =============== */
        // true = phòng-ca trống, false = đã có người
        public bool PhongCaHopLe(DateTime ngayDat, string phong, string ca, string soPhieuDangSua = null)
        {
            bool biTrung = _dal.KiemTraTrungPhongCa(ngayDat, phong, ca, soPhieuDangSua);
            return !biTrung;
        }

        /* =============== THÊM / CẬP NHẬT PHIẾU =============== */

        // Thêm phiếu mới (mặc định đặt trạng thái CHO_TT để có thể thanh toán sau)
        public bool ThemDatTiec(
            string soPhieu,
            DateTime ngayDat,
            string maTK,
            string maNV,
            int soLuongKhach,
            string phong,
            string ca,
            out string messageLoi)
        {
            messageLoi = string.Empty;

            if (string.IsNullOrWhiteSpace(soPhieu))
            {
                messageLoi = "Số phiếu không được rỗng.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(phong))
            {
                messageLoi = "Phòng không được rỗng.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(ca))
            {
                messageLoi = "Ca không được rỗng.";
                return false;
            }

            // phòng-ca đã ai đặt chưa?
            if (!PhongCaHopLe(ngayDat, phong, ca, null))
            {
                messageLoi = $"Phòng {phong} - {ca} ngày {ngayDat:dd/MM/yyyy} đã có người đặt rồi.";
                return false;
            }

            // chèn với trạng thái CHO_TT (để có thể thanh toán sau)
            int rows = _dal.InsertDatTiec(soPhieu, ngayDat, maTK, maNV, soLuongKhach, phong, ca, "CHO_TT");
            if (rows <= 0)
            {
                messageLoi = "Không thêm được phiếu đặt tiệc.";
                return false;
            }
            return true;
        }

        // Cập nhật phiếu (không đổi trạng thái nếu không truyền)
        public bool CapNhatDatTiec(
            string soPhieu,
            DateTime ngayDat,
            string maTK,
            string maNV,
            int soLuongKhach,
            string phong,
            string ca,
            out string messageLoi,
            string trangThai = null)
        {
            messageLoi = string.Empty;

            if (string.IsNullOrWhiteSpace(soPhieu))
            {
                messageLoi = "Thiếu số phiếu.";
                return false;
            }

            // check phòng-ca trùng, loại trừ chính mình
            if (!PhongCaHopLe(ngayDat, phong, ca, soPhieu))
            {
                messageLoi = $"Phòng {phong} - {ca} ngày {ngayDat:dd/MM/yyyy} đã có người đặt rồi.";
                return false;
            }

            int rows = _dal.UpdateDatTiec(soPhieu, ngayDat, maTK, maNV, soLuongKhach, phong, ca, trangThai);
            if (rows <= 0)
            {
                messageLoi = "Không cập nhật được phiếu đặt tiệc.";
                return false;
            }
            return true;
        }

        /* =============== TRẠNG THÁI & DANH SÁCH CHỜ =============== */

        // Đổi trạng thái phiếu (CHO_TT / DA_TT)
        public bool SetTrangThai(string soPhieu, string trangThai)
            => _dal.SetTrangThai(soPhieu, trangThai) > 0;

        // Đánh dấu đã thanh toán (gọi sau khi lập hóa đơn thành công)
        public bool MarkDaThanhToan(string soPhieu)
            => SetTrangThai(soPhieu, "DA_TT");

        // Trả về các phiếu CHỜ THANH TOÁN để form “Lập/Thanh toán hoá” hiển thị
        public DataTable GetPending(string keyword = null)
            => _dal.GetPending(keyword);

        // Kiểm tra phiếu đã có hóa đơn chưa (tránh tạo trùng)
        public bool HasHoaDon(string soPhieu)
            => _dal.HasHoaDon(soPhieu);
    }
}
