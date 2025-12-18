using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL_QLNH;
using DTO_QLNH;
using GUI_QLNH.Dialogs;

namespace GUI_QLNH
{
    public partial class FormThongTinNhanVien : Form
    {
        public FormThongTinNhanVien()
        {
            InitializeComponent();
            // Không làm gì động tới DB/Session ở đây để Designer an toàn
        }

        // Nhận biết chế độ Design an toàn cho Designer
        private static bool IsDesignMode()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return true;
            try
            {
                var p = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                return p.Equals("devenv", StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (IsDesignMode()) return; // tuyệt đối không chạm DB khi Designer render
            _ = LoadDataAsync();
        }

        // ================= DATA =================
        private async Task LoadDataAsync()
        {
            try
            {
                var ma = AppSession.CurrentMaNV; // có thể null
                lblMaNvCurrent.Text = string.IsNullOrWhiteSpace(ma) ? "NV???" : ma;

                var nv = await Task.Run(() => NhanVienBLL.GetByMaNV(ma));
                if (nv == null) { ClearFields(); return; }
                BindToForm(nv);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindToForm(NhanVien nv)
        {
            txtMaNV.Text = nv.MaNV;
            txtHoTen.Text = nv.TenNV;
            txtNoiSinh.Text = nv.NoiSinh;

            if (nv.NgayLamViec == DateTime.MinValue)
            {
                dtpNgayLam.Checked = false;
            }
            else
            {
                dtpNgayLam.Checked = true;
                dtpNgayLam.Value = nv.NgayLamViec;
            }
        }

        private void ClearFields()
        {
            txtMaNV.Text = txtHoTen.Text = txtNoiSinh.Text = "";
            dtpNgayLam.Checked = false;
        }

        private NhanVien Collect()
        {
            errProvider.Clear();

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                errProvider.SetError(txtHoTen, "Họ tên không được trống.");

            if (!string.IsNullOrEmpty(errProvider.GetError(txtHoTen)))
                return null;

            return new NhanVien
            {
                MaNV = txtMaNV.Text?.Trim(),
                TenNV = txtHoTen.Text?.Trim(),
                NoiSinh = txtNoiSinh.Text?.Trim(),
                // MinValue => NULL khi ghi DB (đã xử lý ở DAL)
                NgayLamViec = dtpNgayLam.Checked ? dtpNgayLam.Value.Date : DateTime.MinValue
            };
        }

        // ================= EVENTS =================
        private async void btnReload_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            await LoadDataAsync();
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;

            var nv = Collect();
            if (nv == null) return;

            string err = null;
            var ok = await Task.Run(() =>
            {
                string e1;
                var res = NhanVienBLL.UpdateBasic(nv, out e1);
                if (!res) err = e1;
                return res;
            });

            if (!ok)
            {
                MessageBox.Show(err ?? "Cập nhật thất bại.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Đã cập nhật thông tin.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await LoadDataAsync();
        }

        private void btnChangePwd_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;

            using (var dlg = new FrmChangePassword())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                string err;
                var ok = NhanVienBLL.ChangePassword(AppSession.CurrentMaNV,
                                             dlg.CurrentPassword,
                                             dlg.NewPassword, out err);
                if (!ok)
                {
                    MessageBox.Show(err ?? "Đổi mật khẩu thất bại.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            MessageBox.Show("Đổi mật khẩu thành công!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (IsDesignMode()) return;
            FindForm()?.Close();
        }
    }
}
