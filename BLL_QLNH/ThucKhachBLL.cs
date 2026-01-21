using DAL_QLNH;
using DTO_QLNH;
using System;
using System.Data;
using System.Linq; // BẮT BUỘC CÓ để dùng hàm .Any() và .Contains()

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
            // Update cần check sơ bộ mã trước khi vào Validate chi tiết
            if (string.IsNullOrWhiteSpace(t.MaTK))
                throw new ArgumentException("Mã thực khách không hợp lệ.");

            Validate(t, isUpdate: true);
            return _dal.Update(t);
        }

        // Cập nhật hàm Delete để chặn mã rác ngay từ đầu (Khớp với Unit Test Xóa)
        public bool Delete(string ma)
        {
            // 1. Kiểm tra Rỗng
            if (string.IsNullOrWhiteSpace(ma))
                throw new ArgumentException("Mã thực khách không hợp lệ.");

            // 2. Kiểm tra Độ dài biên (Max 13)
            if (ma.Length > 13)
                throw new ArgumentException("Mã thực khách tối đa 13 ký tự.");

            // 3. Kiểm tra Ký tự đặc biệt & Gạch dưới (_)
            // IsLetterOrDigit trả về false nếu gặp dấu _, nên dòng này đã chặn _ luôn rồi.
            if (ma.Any(c => !char.IsLetterOrDigit(c)))
                throw new ArgumentException("Mã thực khách không được chứa ký tự đặc biệt hoặc dấu gạch dưới.");

            return _dal.Delete(ma);
        }

        // --- HÀM VALIDATE CHUẨN (ĐÃ CẬP NHẬT ĐẦY ĐỦ RÀNG BUỘC) ---
        private static void Validate(ThucKhachDTO t, bool isUpdate)
        {
            // 1. Validate MÃ TK
            if (string.IsNullOrWhiteSpace(t.MaTK))
                throw new ArgumentException("Phải nhập Mã thực khách (MATK).");
            if (t.MaTK.Length > 13)
                throw new ArgumentException("Mã thực khách tối đa 13 ký tự.");

            // Check ký tự đặc biệt (Bao gồm cả _)
            if (t.MaTK.Any(c => !char.IsLetterOrDigit(c)))
                throw new ArgumentException("Mã thực khách không được chứa ký tự đặc biệt hoặc dấu gạch dưới.");

            // 2. Validate TÊN TK
            if (string.IsNullOrWhiteSpace(t.TenTK))
                throw new ArgumentException("Phải nhập Tên thực khách.");
            if (t.TenTK.Length > 50)
                throw new ArgumentException("Tên thực khách tối đa 50 ký tự.");

            if (t.TenTK.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
            {
                throw new ArgumentException("Tên thực khách chỉ được chứa chữ cái và khoảng trắng, không được chứa số hoặc ký tự đặc biệt.");
            }

            // Chặn dấu gạch dưới
            if (t.TenTK.Contains("_"))
                throw new ArgumentException("Tên thực khách không được chứa dấu gạch dưới (_).");

            // 3. Validate SỐ ĐIỆN THOẠI
            // Bắt buộc nhập (Check rỗng trước tiên)
            if (string.IsNullOrWhiteSpace(t.SoDT))
                throw new ArgumentException("Phải nhập Số điện thoại.");

            // Check độ dài 10-13
            if (t.SoDT.Length < 10 || t.SoDT.Length > 13)
                throw new ArgumentException("Số điện thoại phải từ 10 đến 13 số.");

            // Check chỉ chứa số (hàm này cũng chặn luôn dấu _ và chữ cái)
            if (t.SoDT.Any(c => !char.IsDigit(c)))
                throw new ArgumentException("Số điện thoại chỉ được chứa số.");

            // 4. Validate ĐỊA CHỈ
            // 4. Validate ĐỊA CHỈ
            if (!string.IsNullOrWhiteSpace(t.DiaChi))
            {
                if (t.DiaChi.Length > 100)
                    throw new ArgumentException("Địa chỉ tối đa 100 ký tự.");

                // CÁC KÝ TỰ ĐƯỢC PHÉP: Chữ cái, Số, Khoảng trắng và / , . -
                char[] allowedChars = { '/', ',', '.', '-' };

                if (t.DiaChi.Any(c => !char.IsLetterOrDigit(c) &&
                                     !char.IsWhiteSpace(c) &&
                                     !allowedChars.Contains(c)))
                {
                    throw new ArgumentException("Địa chỉ không được chứa các ký hiệu đặc biệt (ngoại trừ / , . -).");
                }
            }
        }
    }
}