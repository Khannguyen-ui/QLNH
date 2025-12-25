using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL_QLNH;
using DTO_QLNH;
using GUI_QLNH.Common;

namespace GUI_QLNHS
{
    public partial class FormThucKhach : Form, GUI_QLNH.Common.IExportable
    {
        private readonly ThucKhachBLL _bll = new ThucKhachBLL();

        private enum FormMode { Idle, Adding, Editing }
        private FormMode _mode = FormMode.Idle;

        private const string SelectCol = "Chon";

        public FormThucKhach()
        {
            InitializeComponent();

            // Configure DataGridView for nicer, responsive UI
            dgv.AutoGenerateColumns = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = false; // some columns will be set readonly later
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;

            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // expand columns to fill available width
            dgv.AllowUserToResizeRows = false;
            dgv.EnableHeadersVisualStyles = false;

            // Visual polish
            dgv.BackgroundColor = Color.WhiteSmoke;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 245, 250);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            dgv.CellClick += dgv_CellClick;
            dgv.DataBindingComplete += Dgv_DataBindingComplete; // ensure columns/checkbox after bind

            EnsureExportButtonExists();
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

        private void FormThucKhach_Load(object sender, EventArgs e)
        {
            LoadGrid();
            ResetForm();
            SetMode(FormMode.Idle);
        }

        private void LoadGrid()
        {
            // Bind data
            dgv.DataSource = null;
            dgv.DataSource = _bll.GetAll();

            // Ensure select column and adjust widths will be handled in DataBindingComplete
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                EnsureSelectColumn();

                // Make the newly inserted select column narrow and not autosized by Fill
                if (dgv.Columns.Contains(SelectCol))
                {
                    var c = dgv.Columns[SelectCol];
                    c.ReadOnly = false;
                    c.Width = 50;
                    c.Frozen = false;
                    // Put checkbox column at left-most
                    c.DisplayIndex = 0;
                }

                // Ensure certain known database columns are readonly and have friendly headers
                if (dgv.Columns.Contains("MATK")) { dgv.Columns["MATK"].HeaderText = "Mã"; dgv.Columns["MATK"].ReadOnly = true; }
                if (dgv.Columns.Contains("TENTK")) { dgv.Columns["TENTK"].HeaderText = "Họ tên"; dgv.Columns["TENTK"].ReadOnly = false; }
                if (dgv.Columns.Contains("DIACHI")) { dgv.Columns["DIACHI"].HeaderText = "Địa chỉ"; dgv.Columns["DIACHI"].ReadOnly = false; }
                if (dgv.Columns.Contains("SODT")) { dgv.Columns["SODT"].HeaderText = "Điện thoại"; dgv.Columns["SODT"].ReadOnly = false; }

                // Re-apply AutoSizeColumnsMode.Fill to distribute width after possible manual widths
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Small polish: alternate row colors
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
            catch { /* ignore UI adjustments errors */ }
        }

        private void EnsureSelectColumn()
        {
            if (dgv.Columns.Contains(SelectCol)) return;

            var c = new DataGridViewCheckBoxColumn
            {
                Name = SelectCol,
                HeaderText = "Chọn",
                Width = 50,
                ReadOnly = false,
                TrueValue = true,
                FalseValue = false
            };

            // Insert at index0
            dgv.Columns.Insert(0, c);

            // Ensure other columns are readonly except select
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Name == SelectCol) continue;
                // keep data columns editable by default (except ID) - we'll set ID as readonly in DataBindingComplete
                col.ReadOnly = false;
            }

            // Initialize checkbox values safely
            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (r.IsNewRow) continue;
                if (r.Cells.Count > 0 && r.Cells[SelectCol] != null)
                {
                    try
                    {
                        if (r.Cells[SelectCol].Value == null || r.Cells[SelectCol].Value == DBNull.Value)
                            r.Cells[SelectCol].Value = false;
                    }
                    catch { /* ignore per-row */ }
                }
            }
        }

        private void ResetForm()
        {
            txtMa.Clear();
            txtTen.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtTim.Clear();
            txtMa.Focus();
        }

        private void SetMode(FormMode m)
        {
            _mode = m;
            bool adding = m == FormMode.Adding;
            bool editing = m == FormMode.Editing;

            btnThem.Enabled = !adding;
            btnLuu.Enabled = adding || editing;
            btnBoQua.Enabled = adding || editing;
            btnSua.Enabled = editing;
            btnXoa.Enabled = true;

            txtMa.ReadOnly = editing; // khi sửa không cho đổi mã
        }

        private ThucKhachDTO ReadForm()
        {
            return new ThucKhachDTO
            {
                MaTK = txtMa.Text.Trim(),
                TenTK = txtTen.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                SoDT = txtSDT.Text.Trim()
            };
        }

        private void FillForm(DataGridViewRow r)
        {
            txtMa.Text = Convert.ToString(r.Cells["MATK"].Value);
            txtTen.Text = Convert.ToString(r.Cells["TENTK"].Value);
            txtDiaChi.Text = Convert.ToString(r.Cells["DIACHI"].Value);
            txtSDT.Text = Convert.ToString(r.Cells["SODT"].Value);
        }

        // ====== EVENTS ======
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgv.Columns[e.ColumnIndex].Name == SelectCol)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                return;
            }

            var r = dgv.Rows[e.RowIndex];
            if (r.Cells["MATK"].Value == null) return;

            FillForm(r);
            SetMode(FormMode.Editing);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(FormMode.Adding);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var t = ReadForm();

                if (_mode == FormMode.Adding)
                {
                    if (_bll.Insert(t))
                    {
                        MessageBox.Show("Đã thêm thực khách.");
                        LoadGrid(); ResetForm(); SetMode(FormMode.Idle);
                    }
                    else MessageBox.Show("Không thể thêm.");
                }
                else if (_mode == FormMode.Editing)
                {
                    if (_bll.Update(t))
                    {
                        MessageBox.Show("Đã cập nhật.");
                        LoadGrid(); ResetForm(); SetMode(FormMode.Idle);
                    }
                    else MessageBox.Show("Không thể cập nhật.");
                }
                else MessageBox.Show("Không ở chế độ thêm/sửa.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(FormMode.Idle);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            { MessageBox.Show("Chọn một dòng trong lưới để sửa."); return; }
            MessageBox.Show("Sửa thông tin bên trên rồi bấm Lưu.");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure any pending checkbox edits are committed before collecting checked ids
                dgv.EndEdit();
                try { dgv.CommitEdit(DataGridViewDataErrorContexts.Commit); } catch { }
                try { if (dgv.CurrentCell != null) dgv.CurrentCell = null; } catch { }

                var ids = GetCheckedIds();

                if (ids.Count > 0)
                {
                    if (MessageBox.Show($"Xóa {ids.Count} thực khách đã chọn?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

                    int ok = 0, fail = 0;
                    foreach (var id in ids)
                    {
                        try { if (_bll.Delete(id)) ok++; else fail++; }
                        catch { fail++; }
                    }
                    LoadGrid(); ResetForm(); SetMode(FormMode.Idle);
                    MessageBox.Show($"Đã xóa: {ok} | Lỗi: {fail}");
                }
                else
                {
                    var ma = txtMa.Text.Trim();
                    if (string.IsNullOrEmpty(ma))
                    { MessageBox.Show("Chọn một dòng để xóa hoặc tick trong lưới!"); return; }

                    if (MessageBox.Show("Xóa thực khách này?", "Xác nhận",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                    if (_bll.Delete(ma))
                    {
                        LoadGrid(); ResetForm(); SetMode(FormMode.Idle);
                        MessageBox.Show("Đã xóa!");
                    }
                    else MessageBox.Show("Không thể xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            SetMode(FormMode.Idle);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                dgv.DataSource = _bll.Search(txtTim.Text.Trim());
                // After search DataBindingComplete will run to ensure select column
                SetMode(FormMode.Idle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e) => Close();

        // IExportable
        public string ExportKey => "ThucKhach";
        public string DefaultExportFileName => "DanhSachThucKhach";
        public DataTable GetExportData() => GUI_QLNH.Common.ExportHelper.DataTableFromGrid(dgv);

        private void btnExport_Click(object sender, EventArgs e)
        {
            GUI_QLNH.Common.ExportHelper.ExportIfAllowed(this, this);
        }

        private List<string> GetCheckedIds()
        {
            var ids = new List<string>();
            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (r.IsNewRow) continue;
                if (r.Cells.Count > 0 && dgv.Columns.Contains(SelectCol) && r.Cells[SelectCol] != null)
                {
                    bool isChecked = false;
                    try
                    {
                        var val = r.Cells[SelectCol].Value;
                        if (val != null && val != DBNull.Value)
                        {
                            // Convert robustly to boolean for different underlying types
                            isChecked = Convert.ToBoolean(val);
                        }
                    }
                    catch { isChecked = false; }

                    if (isChecked)
                    {
                        var id = Convert.ToString(r.Cells["MATK"].Value);
                        if (!string.IsNullOrWhiteSpace(id)) ids.Add(id.Trim());
                    }
                }
            }
            return ids;
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
