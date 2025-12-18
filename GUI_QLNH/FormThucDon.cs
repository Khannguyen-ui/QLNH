using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using DTO_QLNH;
using BLL_QLNH;
using Excel = Microsoft.Office.Interop.Excel;

namespace GUI_QLNH
{
    public partial class FormThucDon : Form
    {
        private const string SelectColName = "Chon";

        public FormThucDon()
        {
            InitializeComponent();
            SetupGrid();
            LoadData();
        }

        // =================== LOAD DỮ LIỆU DB ===================
        private void LoadData()
        {
            dgvThucDon.DataSource = ThucDonBLL.GetAll();
            EnsureSelectColumn();

            // ensure MaTD column is sortable and supports numeric compare
            if (dgvThucDon.Columns.Contains("MaTD"))
            {
                dgvThucDon.Columns["MaTD"].SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        // =================== CẤU HÌNH LƯỚI ===================
        private void SetupGrid()
        {
            dgvThucDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThucDon.MultiSelect = false;
            dgvThucDon.AutoGenerateColumns = true;
            dgvThucDon.ReadOnly = false; // để tick checkbox
            dgvThucDon.AllowUserToAddRows = false;
            dgvThucDon.AllowUserToDeleteRows = false;
            dgvThucDon.EditMode = DataGridViewEditMode.EditOnEnter;
            // KHÔNG ép Anchor tại đây nữa – để Designer đặt ALL sides

            // handle numeric sorting on MaTD
            dgvThucDon.SortCompare -= dgvThucDon_SortCompare;
            dgvThucDon.SortCompare += dgvThucDon_SortCompare;
        }

        // Tạo cột "Chọn" checkbox nếu chưa có
        private void EnsureSelectColumn()
        {
            if (!dgvThucDon.Columns.Contains(SelectColName))
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    Name = SelectColName,
                    HeaderText = "Chọn",
                    Width = 55,
                    ReadOnly = false
                };
                dgvThucDon.Columns.Insert(0, chk);
            }

            // Chỉ cho sửa cột "Chọn", các cột khác readonly
            foreach (DataGridViewColumn col in dgvThucDon.Columns)
                col.ReadOnly = col.Name != SelectColName;

            // Clear tick
            foreach (DataGridViewRow r in dgvThucDon.Rows)
            {
                if (r.Cells[SelectColName] is DataGridViewCheckBoxCell)
                    r.Cells[SelectColName].Value = false;
            }

            // Make MaTD column sortable (numeric comparison will be handled in SortCompare)
            if (dgvThucDon.Columns.Contains("MaTD"))
            {
                dgvThucDon.Columns["MaTD"].SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        // =================== TIỆN ÍCH FORM ===================
        private void ResetForm()
        {
            txtMaTD.Clear();
            txtTenMon.Clear();
            txtDVT.Clear();
            txtGiaTien.Clear();
            txtGhiChu.Clear();
            txtSoLuongTon.Clear();
            txtSearch.Clear();
            txtMaTD.Focus();
        }

        private ThucDon ReadForm()
        {
            // Parse giá an toàn
            float gia = 0;
            var raw = (txtGiaTien.Text ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(raw))
            {
                if (!float.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out gia))
                {
                    float.TryParse(raw, NumberStyles.Float, CultureInfo.CurrentCulture, out gia);
                }
            }

            int soLuongTon = 0;
            int.TryParse((txtSoLuongTon?.Text ?? "").Trim(), out soLuongTon);

            return new ThucDon
            {
                MaTD = txtMaTD.Text.Trim(),
                TenMon = txtTenMon.Text.Trim(),
                DVT = txtDVT.Text.Trim(),
                GiaTien = gia,
                GhiChu = txtGhiChu.Text.Trim(),
                SoLuongTon = soLuongTon
            };
        }

        private void FillFormFromRow(DataGridViewRow row)
        {
            if (row == null) return;

            txtMaTD.Text = Convert.ToString(row.Cells["MaTD"]?.Value);
            txtTenMon.Text = Convert.ToString(row.Cells["TenMon"]?.Value);
            txtDVT.Text = Convert.ToString(row.Cells["DVT"]?.Value);
            txtGiaTien.Text = Convert.ToString(row.Cells["GiaTien"]?.Value);
            txtGhiChu.Text = Convert.ToString(row.Cells["GhiChu"]?.Value);

            object val = null;
            if (dgvThucDon.Columns.Contains("SoLuongTon"))
                val = row.Cells["SoLuongTon"].Value;
            txtSoLuongTon.Text = val == null || val == DBNull.Value ? "0" : Convert.ToString(val);
        }

        // =================== BUTTON HANDLERS ===================
        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtMaTD.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var td = ReadForm();
                if (string.IsNullOrWhiteSpace(td.MaTD) || string.IsNullOrWhiteSpace(td.TenMon))
                {
                    MessageBox.Show("Mã và Tên món không được để trống!");
                    return;
                }

                var existed = ThucDonBLL.GetAll().Find(x => x.MaTD == td.MaTD);
                bool result = existed == null
                    ? ThucDonBLL.Insert(td)
                    : ThucDonBLL.Update(td);

                if (result)
                {
                    MessageBox.Show(existed == null
                        ? "Thêm thành công!"
                        : "Cập nhật thành công!");
                    LoadData();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Thao tác thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTD.Text))
            {
                MessageBox.Show("Chọn một dòng để sửa!");
                return;
            }

            MessageBox.Show("Hãy chỉnh thông tin ở khung trên và nhấn Lưu để cập nhật!");
            txtTenMon.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                dgvThucDon.EndEdit(); // chốt giá trị checkbox trước khi đọc
                var ids = GetCheckedIds();

                if (ids.Count > 0)
                {
                    if (MessageBox.Show(
                        $"Xóa {ids.Count} món ăn đã chọn?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes)
                        return;

                    int ok = 0, fail = 0;
                    foreach (var id in ids)
                    {
                        if (ThucDonBLL.Delete(id)) ok++; else fail++;
                    }

                    LoadData();
                    ResetForm();
                    MessageBox.Show($"Đã xoá: {ok} | Không xoá được: {fail}");
                }
                else
                {
                    var maTD = txtMaTD.Text.Trim();
                    if (string.IsNullOrWhiteSpace(maTD))
                    {
                        MessageBox.Show("Chọn một dòng để xóa hoặc tick chọn trong lưới!");
                        return;
                    }

                    if (MessageBox.Show(
                        "Xoá món ăn này?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    if (ThucDonBLL.Delete(maTD))
                    {
                        MessageBox.Show("Xoá thành công!");
                        LoadData();
                        ResetForm();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xoá!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }
        }

        private void btnMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadData();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            dgvThucDon.DataSource = ThucDonBLL.Search(keyword);
            EnsureSelectColumn();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        // >>>>> XUẤT EXCEL (Ghi file) <<<<<
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportDataGridViewToExcel(dgvThucDon);
        }

        private void ExportDataGridViewToExcel(DataGridView dgv)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.Title = "Lưu file Excel";
                sfd.FileName = "ThucDon.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                string filePath = sfd.FileName;

                Excel.Application excelApp = null;
                Excel.Workbook wb = null;
                Excel.Worksheet ws = null;

                try
                {
                    excelApp = new Excel.Application();
                    wb = excelApp.Workbooks.Add(Type.Missing);   // Add (đúng)
                    ws = (Excel.Worksheet)wb.ActiveSheet;
                    ws.Name = "ThucDon";

                    int colCount = 0;

                    // Ghi header (bỏ cột Chọn)
                    int excelCol = 1;
                    for (int c = 0; c < dgv.Columns.Count; c++)
                    {
                        DataGridViewColumn col = dgv.Columns[c];
                        if (!col.Visible) continue;
                        if (col.Name == SelectColName) continue;

                        ws.Cells[1, excelCol] = col.HeaderText;
                        colCount++;
                        excelCol++;
                    }

                    // Style header
                    Excel.Range headerRange = ws.Range[ws.Cells[1, 1], ws.Cells[1, colCount]];
                    headerRange.Font.Bold = true;
                    headerRange.Interior.Color =
                        System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                    // Ghi data
                    int excelRow = 2;
                    for (int r = 0; r < dgv.Rows.Count; r++)
                    {
                        if (dgv.AllowUserToAddRows && r == dgv.Rows.Count - 1) break;

                        excelCol = 1;
                        for (int c = 0; c < dgv.Columns.Count; c++)
                        {
                            var col = dgv.Columns[c];
                            if (!col.Visible) continue;
                            if (col.Name == SelectColName) continue;

                            object val = dgv.Rows[r].Cells[c].Value;
                            ws.Cells[excelRow, excelCol] = (val == null ? "" : val.ToString());
                            excelCol++;
                        }

                        excelRow++;
                    }

                    // Autofit
                    Excel.Range usedRange = ws.Range[ws.Cells[1, 1], ws.Cells[1 + dgv.Rows.Count, colCount]];
                    usedRange.Columns.AutoFit();

                    // Lưu file
                    wb.SaveAs(filePath);
                    wb.Close();
                    excelApp.Quit();

                    MessageBox.Show("Xuất Excel thành công!\n" + filePath,
                                    "Thành công",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    try { System.Diagnostics.Process.Start(filePath); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất Excel: " + ex.Message,
                                    "Lỗi",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    if (wb != null)
                    {
                        try { wb.Close(false); } catch { }
                    }
                    if (excelApp != null)
                    {
                        try { excelApp.Quit(); } catch { }
                    }
                }
            }
        }

        // >>>>> NHẬP CSV (Đọc file + LƯU VÀO DB) <<<<<
        // Format CSV:
        // MaTD,TenMon,DVT,GiaTien,GhiChu,SoLuongTon
        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn file CSV thực đơn";
                ofd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                // Read using FileStream with FileShare.ReadWrite so files opened by other apps (e.g. Excel)
                // can still be read. Parse lines with a robust splitter that understands quotes.
                try
                {
                    int added = 0;
                    int updated = 0;
                    int failed = 0;

                    using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sr = new StreamReader(fs))
                    {
                        bool firstLine = true;
                        string rawLine;
                        while ((rawLine = sr.ReadLine()) != null)
                        {
                            var line = rawLine.Trim();
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            var cells = SplitCsvLine(line);
                            if (cells == null || cells.Length < 6) continue;

                            if (firstLine)
                            {
                                string h0 = (cells[0] ?? "").Trim().ToLower();
                                if (h0 == "matd" || h0 == "mã món" || h0 == "ma mon" || h0 == "ma td")
                                {
                                    firstLine = false;
                                    continue; // skip header
                                }
                            }
                            firstLine = false;

                            string ma = cells[0].Trim();
                            string ten = cells[1].Trim();
                            string dvt = cells[2].Trim();

                            float gia = 0;
                            if (!float.TryParse(cells[3].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out gia))
                                float.TryParse(cells[3].Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out gia);

                            string note = cells[4].Trim();

                            int ton = 0;
                            int.TryParse(cells[5].Trim(), out ton);

                            var item = new ThucDon
                            {
                                MaTD = ma,
                                TenMon = ten,
                                DVT = dvt,
                                GiaTien = gia,
                                GhiChu = note,
                                SoLuongTon = ton
                            };

                            var existed = ThucDonBLL.GetAll().Find(x => x.MaTD == item.MaTD);
                            bool ok;
                            if (existed == null)
                            {
                                ok = ThucDonBLL.Insert(item);
                                if (ok) added++; else failed++;
                            }
                            else
                            {
                                ok = ThucDonBLL.Update(item);
                                if (ok) updated++; else failed++;
                            }
                        }
                    }

                    LoadData();

                    MessageBox.Show(
                        "Nhập CSV xong:\n"
                        + "- Thêm mới: " + added
                        + "\n- Cập nhật: " + updated
                        + "\n- Lỗi: " + failed,
                        "Kết quả nhập CSV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Không đọc / lưu được file CSV: " + ex.Message,
                        "Lỗi nhập CSV",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        // =================== SỰ KIỆN LƯỚI ===================
        private void dgvThucDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Click vào ô checkbox "Chọn", chỉ cần commit edit
            if (dgvThucDon.Columns[e.ColumnIndex].Name == SelectColName)
            {
                dgvThucDon.CommitEdit(DataGridViewDataErrorContexts.Commit);
                return;
            }

            // Còn lại: fill form
            var row = dgvThucDon.Rows[e.RowIndex];
            FillFormFromRow(row);
        }

        private void dgvThucDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // để trống (nhưng vẫn cần hàm cho event designer)
        }

        // =================== LẤY ID CÁC DÒNG ĐƯỢC CHỌN ===================
        private List<string> GetCheckedIds()
        {
            var ids = new List<string>();
            foreach (DataGridViewRow row in dgvThucDon.Rows)
            {
                if (row.Cells[SelectColName] is DataGridViewCheckBoxCell)
                {
                    bool isChecked = false;
                    try
                    {
                        var val = row.Cells[SelectColName].Value;
                        if (val != null && val != DBNull.Value)
                            isChecked = Convert.ToBoolean(val);
                    }
                    catch { isChecked = false; }

                    if (isChecked)
                    {
                        string id = Convert.ToString(row.Cells["MaTD"]?.Value);
                        if (!string.IsNullOrWhiteSpace(id))
                            ids.Add(id);
                    }
                }
            }
            return ids;
        }

        // Simple CSV line splitter that supports quoted fields and escaped quotes
        private string[] SplitCsvLine(string line)
        {
            if (line == null) return null;
            var cells = new List<string>();
            bool inQuotes = false;
            var cur = new System.Text.StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Escaped quote
                        cur.Append('"');
                        i++; // skip next quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    cells.Add(cur.ToString());
                    cur.Clear();
                }
                else
                {
                    cur.Append(c);
                }
            }
            cells.Add(cur.ToString());
            return cells.ToArray();
        }

        // Custom numeric-aware sort for MaTD column
        private void dgvThucDon_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == null) return;
            if (e.Column.Name != "MaTD") return;

            string s1 = Convert.ToString(e.CellValue1)?.Trim();
            string s2 = Convert.ToString(e.CellValue2)?.Trim();

            int n1, n2;
            bool p1 = int.TryParse(s1, NumberStyles.Integer, CultureInfo.InvariantCulture, out n1);
            bool p2 = int.TryParse(s2, NumberStyles.Integer, CultureInfo.InvariantCulture, out n2);

            if (p1 && p2)
            {
                e.SortResult = n1.CompareTo(n2);
            }
            else
            {
                e.SortResult = String.Compare(s1, s2, StringComparison.CurrentCultureIgnoreCase);
            }

            e.Handled = true;
        }
    }
}
