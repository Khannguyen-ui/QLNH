using BLL_QLNH;
using DTO_QLNH;
using GUI_QLNH.Common;
using GUI_QLNHS;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI_QLNH
{
    public partial class FormDatTiec : Form, IExportable
    {
        private readonly DatTiecBLL _bll = new DatTiecBLL();

        private enum Mode { Idle, Adding, Editing }
        private Mode _mode = Mode.Idle;
                private const string SelectCol = "Chon";

        public FormDatTiec()
        {
            InitializeComponent();


            ConfigureGrid();
            EnsureExportButtonExists();
            txtSoPhieu.ReadOnly = true;
        }

        private void EnsureExportButtonExists()
        {
            try
            {
                if (this.Controls.Find("btnExport", true).Length > 0) return;

                Control host = null;
                var candidateNames = new[] { "panelBottom", "panelButtons", "pnlBottom", "panel1", "panelControls" };
                foreach (var n in candidateNames)
                {
                    var f = this.Controls.Find(n, true);
                    if (f != null && f.Length > 0) { host = f[0]; break; }
                }
                if (host == null)
                {
                    var refBtn = this.Controls.Find("btnThoat", true).OfType<Control>().FirstOrDefault()
                                         ?? this.Controls.Find("btnXoa", true).OfType<Control>().FirstOrDefault();
                    host = refBtn?.Parent ?? this;
                }

                var btnExport = new Button { Name = "btnExport", Text = "Xuất", Size = new Size(90, 32), Anchor = AnchorStyles.Bottom | AnchorStyles.Right };
                host.Controls.Add(btnExport);
                btnExport.Top = Math.Max(8, host.Height - btnExport.Height - 10);
                btnExport.Left = Math.Max(8, host.Width - btnExport.Width - 12);
                host.Resize -= Host_Resize_ForExport; host.Resize += Host_Resize_ForExport;
                btnExport.Click += btnExport_Click;
            }
            catch { }
        }

        private void Host_Resize_ForExport(object sender, EventArgs e)
        {
            try
            {
                var host = sender as Control;
                var btn = host?.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "btnExport");
                if (btn != null)
                {
                    btn.Top = Math.Max(8, host.Height - btn.Height - 10);
                    btn.Left = Math.Max(8, host.Width - btn.Width - 12);
                }
            }
            catch { }
        }

        // IExportable
        public string ExportKey => "DatTiec";
        public string DefaultExportFileName => "DatTiec";
        public DataTable GetExportData() => ExportHelper.DataTableFromGrid(dgv);

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportHelper.ExportIfAllowed(this, this);
        }

        private void FormDatTiec_Load(object sender, EventArgs e)
        {
            // tooltip
            var tip = new ToolTip();
            tip.SetToolTip(txtSoPhieu, "Nhập số phiếu (duy nhất)");
            tip.SetToolTip(dtpNgayDK, "Chọn ngày đặt");
            tip.SetToolTip(cboMaTK, "Chọn thực khách");
            tip.SetToolTip(cboMaNV, "Chọn nhân viên");
            tip.SetToolTip(nudSoLuong, "Số lượng khách");
            tip.SetToolTip(cboPhong, "Chọn phòng/ sảnh");
            tip.SetToolTip(cboCa, "Chọn ca (CA1 / CA2 / ...)");

            // backup item cho Ca nếu Designer chưa có
            if (cboCa.Items.Count == 0)
            {
                cboCa.Items.Add("CA1");
                cboCa.Items.Add("CA2");
                cboCa.Items.Add("CA3");
            }

            BindCombos();
            LoadGrid();
            ResetForm();
            SetMode(Mode.Idle);

            // TreeView (đã có sẵn trong Designer) – chỉ cần nạp gốc
            LoadTreeRoots();
        }

        private void BindCombos()
        {
            var tks = _bll.GetThucKhach();
            var nvs = _bll.GetNhanVien();
            var phongs = _bll.GetListPhong();

            cboMaTK.DisplayMember = "Ten";
            cboMaTK.ValueMember = "Ma";
            cboMaTK.DataSource = tks;

            cboMaNV.DisplayMember = "Ten";
            cboMaNV.ValueMember = "Ma";
            cboMaNV.DataSource = nvs;

            cboPhong.DisplayMember = "Ten"; // Hiển thị tên (VD: Phòng VIP 1)
            cboPhong.ValueMember = "Ma";    // Giá trị ngầm (VD: P01)
            cboPhong.DataSource = phongs;
            cboPhong.SelectedIndex = -1;    // Mặc định không chọn gì
        }

        private void ConfigureGrid()
        {
            dgv.AutoGenerateColumns = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.RowHeadersVisible = false;

            // Make columns auto-size and readable
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dgv.CellClick += dgv_CellClick;
            dgv.DataBindingComplete -= Dgv_DataBindingComplete;
            dgv.DataBindingComplete += Dgv_DataBindingComplete;
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Ensure checkbox column exists
                EnsureSelectColumn();

                // Friendly headers and readonly flags
                if (dgv.Columns.Contains("SOPHIEU")) { dgv.Columns["SOPHIEU"].HeaderText = "Số phiếu"; dgv.Columns["SOPHIEU"].ReadOnly = true; dgv.Columns["SOPHIEU"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("NGAYDK")) { dgv.Columns["NGAYDK"].HeaderText = "Ngày ĐK"; dgv.Columns["NGAYDK"].ReadOnly = true; dgv.Columns["NGAYDK"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("MATK")) { dgv.Columns["MATK"].HeaderText = "Mã TK"; dgv.Columns["MATK"].ReadOnly = true; dgv.Columns["MATK"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("TENTK")) { dgv.Columns["TENTK"].HeaderText = "Thực khách"; dgv.Columns["TENTK"].ReadOnly = true; dgv.Columns["TENTK"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
                if (dgv.Columns.Contains("MANV")) { dgv.Columns["MANV"].HeaderText = "Mã NV"; dgv.Columns["MANV"].ReadOnly = true; dgv.Columns["MANV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("TENNV")) { dgv.Columns["TENNV"].HeaderText = "Nhân viên"; dgv.Columns["TENNV"].ReadOnly = true; dgv.Columns["TENNV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
                if (dgv.Columns.Contains("SOLUONGKHACH")) { dgv.Columns["SOLUONGKHACH"].HeaderText = "Số lượng"; dgv.Columns["SOLUONGKHACH"].ReadOnly = true; dgv.Columns["SOLUONGKHACH"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("PHONG")) { dgv.Columns["PHONG"].HeaderText = "Phòng"; dgv.Columns["PHONG"].ReadOnly = true; dgv.Columns["PHONG"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("Ca")) { dgv.Columns["Ca"].HeaderText = "Ca"; dgv.Columns["Ca"].ReadOnly = true; dgv.Columns["Ca"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
                if (dgv.Columns.Contains("TRANGTHAI")) { dgv.Columns["TRANGTHAI"].HeaderText = "TRANGTHAI"; dgv.Columns["TRANGTHAI"].ReadOnly = true; dgv.Columns["TRANGTHAI"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }

                // Ensure checkbox column width and position
                if (dgv.Columns.Contains(SelectCol))
                {
                    var chk = dgv.Columns[SelectCol];
                    chk.ReadOnly = false;
                    chk.DisplayIndex = 0;
                    chk.Width = 50;
                    chk.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }

                // After autosizing some columns, set remaining to Fill so grid uses available space
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Clear any selected cell to avoid focus border
                if (dgv.Rows.Count > 0) dgv.CurrentCell = null;
            }
            catch { /* ignore UI adjust errors */ }
        }

        private void EnsureSelectColumn()
        {
            if (!dgv.Columns.Contains(SelectCol))
            {
                var c = new DataGridViewCheckBoxColumn
                {
                    Name = SelectCol,
                    HeaderText = "Chọn",
                    Width = 55,
                    ReadOnly = false
                };
                dgv.Columns.Insert(0, c);
            }

            foreach (DataGridViewColumn col in dgv.Columns)
                col.ReadOnly = col.Name != SelectCol;

            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (r.IsNewRow) continue;
                try { r.Cells[SelectCol].Value = r.Cells[SelectCol].Value ?? false; } catch { }
            }
        }

        private void LoadGrid()
        {
            dgv.DataSource = _bll.GetAll();
            // Header text and sizing handled in DataBindingComplete
        }

        private void ResetForm()
        {
            txtSoPhieu.Clear();
            dtpNgayDK.Value = DateTime.Today;

            if (cboMaTK.Items.Count > 0) cboMaTK.SelectedIndex = 0;
            if (cboMaNV.Items.Count > 0) cboMaNV.SelectedIndex = 0;
            if (cboCa.Items.Count > 0) cboCa.SelectedIndex = 0;

            nudSoLuong.Value = 0;
            cboPhong.SelectedIndex = -1; // <-- Thêm dòng này
            txtSearch.Clear();
            txtSoPhieu.Focus();
        }

        private void SetMode(Mode m)
        {
            _mode = m;
            bool editing = m == Mode.Editing;
            bool adding = m == Mode.Adding;

            btnThem.Enabled = !adding;
            btnLuu.Enabled = adding || editing;
            btnBoQua.Enabled = adding || editing;
            btnSua.Enabled = editing;
            btnXoa.Enabled = true;
            btnMoi.Enabled = true;

            txtSoPhieu.ReadOnly = editing; // không đổi PK khi sửa
        }

        private DatTiecDTO ReadForm()
        {
            return new DatTiecDTO
            {
                SoPhieu = txtSoPhieu.Text.Trim(),
                NgayDk = dtpNgayDK.Value,
                MaTk = cboMaTK.SelectedValue?.ToString(),
                MaNv = cboMaNV.SelectedValue?.ToString(),
                SoLuongKhach = (int)nudSoLuong.Value,
                Phong = cboPhong.SelectedValue?.ToString(),
                Ca = cboCa.Text.Trim()
            };
        }

        private void FillForm(DataGridViewRow r)
        {
            txtSoPhieu.Text = Convert.ToString(r.Cells["SOPHIEU"].Value)?.Trim();

            if (DateTime.TryParse(Convert.ToString(r.Cells["NGAYDK"].Value), out var d))
                dtpNgayDK.Value = d;
            else
                dtpNgayDK.Value = DateTime.Today;

            cboMaTK.SelectedValue = Convert.ToString(r.Cells["MATK"].Value)?.Trim();
            cboMaNV.SelectedValue = Convert.ToString(r.Cells["MANV"].Value)?.Trim();

            if (int.TryParse(Convert.ToString(r.Cells["SOLUONGKHACH"].Value), out var sl))
            {
                if (sl < (int)nudSoLuong.Minimum) sl = (int)nudSoLuong.Minimum;
                if (sl > (int)nudSoLuong.Maximum) sl = (int)nudSoLuong.Maximum;
                nudSoLuong.Value = sl;
            }
            else
            {
                nudSoLuong.Value = 0;
            }

            string maPhong = Convert.ToString(r.Cells["PHONG"].Value);
            cboPhong.SelectedValue = maPhong;

            if (dgv.Columns.Contains("Ca"))
            {
                string caValue = Convert.ToString(r.Cells["Ca"].Value)?.Trim();
                if (!string.IsNullOrEmpty(caValue))
                {
                    if (!cboCa.Items.Contains(caValue))
                        cboCa.Items.Add(caValue);
                    cboCa.SelectedItem = caValue;
                }
            }
        }

        private bool ValidateInput(DatTiecDTO x, bool isAdd)
        {
            if (string.IsNullOrWhiteSpace(x.SoPhieu))
            { MessageBox.Show("Vui lòng nhập số phiếu."); txtSoPhieu.Focus(); return false; }

            if (x.SoPhieu.Length > 10)
            { MessageBox.Show("Số phiếu tối đa 10 ký tự."); txtSoPhieu.Focus(); return false; }

            if (x.NgayDk == null)
            { MessageBox.Show("Vui lòng chọn ngày đặt."); dtpNgayDK.Focus(); return false; }

            
            if (isAdd && x.NgayDk.Value.Date < DateTime.Today.Date)
            {
                MessageBox.Show("Ngày đặt không được nhỏ hơn hôm nay.");
                dtpNgayDK.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(x.MaTk))
            { MessageBox.Show("Vui lòng chọn thực khách."); cboMaTK.Focus(); return false; }

            if (string.IsNullOrWhiteSpace(x.MaNv))
            { MessageBox.Show("Vui lòng chọn nhân viên."); cboMaNV.Focus(); return false; }

            if (!x.SoLuongKhach.HasValue || x.SoLuongKhach.Value <= 0)
            { MessageBox.Show("Số lượng khách phải lớn hơn 0."); nudSoLuong.Focus(); return false; }

            if (string.IsNullOrWhiteSpace(x.Phong))
            {
                MessageBox.Show("Vui lòng chọn phòng."); // Sửa thông báo
                cboPhong.Focus(); // Focus vào combo box
                return false;
            }

            if (string.IsNullOrWhiteSpace(x.Ca))
            { MessageBox.Show("Vui lòng chọn ca."); cboCa.Focus(); return false; }

            if (isAdd)
            {
                var tb = _bll.GetAll();
                foreach (DataRow row in tb.Rows)
                {
                    string soPhieuExisting = Convert.ToString(row["SOPHIEU"]).Trim();
                    if (string.Equals(soPhieuExisting, x.SoPhieu, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Số phiếu đã tồn tại.");
                        txtSoPhieu.Focus();
                        return false;
                    }
                }
            }

            return true;
        }

        // ====== grid events ======
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgv.Columns[e.ColumnIndex].Name == SelectCol)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                return;
            }

            var r = dgv.Rows[e.RowIndex];
            if (r.Cells["SOPHIEU"].Value == null) return;

            FillForm(r);
            SetMode(Mode.Editing);
        }

        // ====== buttons ======
        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetForm();          // Xóa trắng các ô
            SetMode(Mode.Adding); // Chuyển sang chế độ Thêm

            // ===> THÊM DÒNG NÀY ĐỂ TỰ ĐỘNG SINH MÃ <===
            txtSoPhieu.Text = _bll.TaoSoPhieuTuDong();
            // ==========================================
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhieu.Text))
            {
                MessageBox.Show("Chọn một dòng trong lưới để sửa.");
                return;
            }
            SetMode(Mode.Editing);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var x = ReadForm();

                if (_mode == Mode.Adding)
                {
                    if (!ValidateInput(x, true)) return;

                    string err;
                    bool ok = _bll.ThemDatTiec(
                        soPhieu: x.SoPhieu,
                        ngayDat: x.NgayDk.Value,
                        maTK: x.MaTk,
                        maNV: x.MaNv,
                        soLuongKhach: x.SoLuongKhach.Value,
                        phong: x.Phong,
                        ca: x.Ca,
                        messageLoi: out err
                    );

                    if (!ok)
                    {
                        MessageBox.Show(err, "Không thể thêm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show("Đã thêm phiếu đặt tiệc.");
                    LoadGrid();
                    ResetForm();
                    SetMode(Mode.Idle);
                    LoadTreeRoots();
                }
                else if (_mode == Mode.Editing)
                {
                    if (!ValidateInput(x, false)) return;

                    string err;
                    bool ok = _bll.CapNhatDatTiec(
                        soPhieu: x.SoPhieu,
                        ngayDat: x.NgayDk.Value,
                        maTK: x.MaTk,
                        maNV: x.MaNv,
                        soLuongKhach: x.SoLuongKhach.Value,
                        phong: x.Phong,
                        ca: x.Ca,
                        messageLoi: out err
                    );

                    if (!ok)
                    {
                        MessageBox.Show(err, "Không thể cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show("Đã cập nhật.");
                    LoadGrid();
                    ResetForm();
                    SetMode(Mode.Idle);
                    LoadTreeRoots();
                }
                else
                {
                    MessageBox.Show("Không ở chế độ thêm/sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(Mode.Idle);
        }

        private List<string> GetCheckedIds()
        {
            var ids = new List<string>();
            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (r.IsNewRow) continue;

                var cell = r.Cells[SelectCol];
                if (cell != null && cell.Value is bool b && b)
                {
                    var id = Convert.ToString(r.Cells["SOPHIEU"].Value);
                    if (!string.IsNullOrWhiteSpace(id))
                        ids.Add(id.Trim());
                }
            }
            return ids;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                dgv.EndEdit();
                var ids = GetCheckedIds(); // Lấy danh sách các mã SoPhieu được tích chọn

                // TRƯỜNG HỢP 1: XÓA HÀNG LOẠT
                if (ids.Any())
                {
                    if (MessageBox.Show($"Bạn đã chọn {ids.Count} phiếu. Hệ thống sẽ xóa các phiếu hợp lệ và giữ lại các phiếu đã lập hóa đơn. Tiếp tục?",
                        "Xác nhận xóa hàng loạt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;

                    int okCnt = 0;
                    List<string> lockedIds = new List<string>(); // Danh sách chứa các mã bị dính khóa

                    foreach (var id in ids)
                    {
                        try
                        {
                            // Gọi BLL để xóa. Hàm Delete trong DAL của bạn đã có check HoaDon
                            if (_bll.Delete(id))
                            {
                                okCnt++;
                            }
                            else
                            {
                                // Nếu trả về false nghĩa là dính khóa (có hóa đơn)
                                lockedIds.Add(id);
                            }
                        }
                        catch (Exception)
                        {
                            // Nếu dính lỗi SQL ngoại lệ cũng đưa vào danh sách lỗi
                            lockedIds.Add(id);
                        }
                    }

                    // TỔNG HỢP THÔNG BÁO
                    string message = $"Đã xóa thành công: {okCnt} phiếu.";
                    if (lockedIds.Count > 0)
                    {
                        message += $"\n\n[CẢNH BÁO CHO QUẢN LÝ]: {lockedIds.Count} phiếu sau đã có hóa đơn nên không thể xóa:\n"
                                   + string.Join(", ", lockedIds);
                    }

                    MessageBox.Show(message, "Kết quả xử lý", MessageBoxButtons.OK,
                        lockedIds.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                    LoadGrid();
                    ResetForm();
                    SetMode(Mode.Idle);
                    LoadTreeRoots();
                }
                // TRƯỜNG HỢP 2: XÓA ĐƠN LẺ (Khi không tích checkbox nào)
                else
                {
                    var id = txtSoPhieu.Text.Trim();
                    if (string.IsNullOrEmpty(id))
                    {
                        MessageBox.Show("Vui lòng chọn một phiếu trên lưới hoặc tích chọn để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (MessageBox.Show($"Xóa phiếu đặt tiệc {id}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    if (_bll.Delete(id))
                    {
                        MessageBox.Show("Xóa thành công!", "Thông báo");
                        LoadGrid();
                        ResetForm();
                        SetMode(Mode.Idle);
                        LoadTreeRoots();
                    }
                    else
                    {
                        MessageBox.Show($"[CẢNH BÁO]: Phiếu {id} đã lập hóa đơn hoặc không tồn tại. Không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(Mode.Idle);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                dgv.DataSource = _bll.Search(txtSearch.Text.Trim());
                EnsureSelectColumn();
                SetMode(Mode.Idle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Là form con, Close() sẽ đóng tab hiện tại trong pnlContent
            Close();
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoPhieu.Text))
            {
                MessageBox.Show("Vui lòng chọn một phiếu đặt tiệc để xem/sửa chi tiết!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            var f = new FormCTDatTiec(txtSoPhieu.Text.Trim());
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);

            LoadGrid();
            LoadTreeRoots();
        }

        // ===== TreeView =====
        private void LoadTreeRoots()
        {
            tv.BeginUpdate();
            tv.Nodes.Clear();

            foreach (var tk in _bll.GetThucKhach())
            {
                var root = new TreeNode($"{tk.Ten} ({tk.Ma})")
                {
                    Name = tk.Ma,  // MATK
                    Tag = "KH"
                };
                root.Nodes.Add(new TreeNode("...")); // dummy để lazy-load
                tv.Nodes.Add(root);
            }

            tv.EndUpdate();
        }

        private void tv_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var n = e.Node;

            // lazy-load
            if (n.Nodes.Count == 1 && n.Nodes[0].Text == "...")
                n.Nodes.Clear();
            else
                return;

            if ((string)n.Tag == "KH")
            {
                var list = _bll.GetPhieuByMatk(n.Name);
                foreach (var p in list)
                {
                    string text = $"{p.SoPhieu}  -  {(p.NgayDk.HasValue ? p.NgayDk.Value.ToString("dd/MM/yyyy") : "")}  -  {p.Phong ?? ""}";
                    var child = new TreeNode(text)
                    {
                        Name = p.SoPhieu,
                        Tag = "PHIEU"
                    };
                    child.Nodes.Add(new TreeNode("...")); // chi tiết món
                    n.Nodes.Add(child);
                }
            }
            else if ((string)n.Tag == "PHIEU")
            {
                var items = _bll.GetCtBySoPhieu(n.Name);
                foreach (var ct in items)
                {
                    string text = $"{(ct.TenMon ?? "(không tên)")} (x{(ct.SoLuong?.ToString() ?? "?")}) - {(ct.GiaBan?.ToString("0,0") ?? "?")}";
                    var leaf = new TreeNode(text) { Tag = "MON" };
                    n.Nodes.Add(leaf);
                }
            }
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var n = e.Node;

            if ((string)n.Tag == "KH")
            {
                dgv.DataSource = _bll.GetByMatk(n.Name);
                EnsureSelectColumn();
                SetMode(Mode.Idle);
            }
            else if ((string)n.Tag == "PHIEU")
            {
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    var v = Convert.ToString(r.Cells["SOPHIEU"].Value);
                    if (!string.IsNullOrEmpty(v) && v.Trim() == n.Name)
                    {
                        r.Selected = true;
                        dgv.CurrentCell = r.Cells["SOPHIEU"];
                        FillForm(r);
                        SetMode(Mode.Editing);
                        break;
                    }
                }
            }
        }

    
    }
}
