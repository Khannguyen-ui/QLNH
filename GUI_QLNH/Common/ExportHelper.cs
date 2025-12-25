// GUI_QLNH/Common/ExportHelper.cs
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace GUI_QLNH.Common
{
    public static class ExportHelper
    {
        /// <summary>
        /// Export a DataTable to Excel .xlsx using Microsoft.Office.Interop.Excel.
        /// REQUIREMENT: Microsoft Excel installed and reference to Microsoft.Office.Interop.Excel.
        /// If Excel is not installed, falls back to CSV automatically.
        /// </summary>
        public static void ExportToExcel(DataTable table, string suggestedFileName)
        {
            if (table == null || table.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv",
                FileName = suggestedFileName
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                var ext = Path.GetExtension(sfd.FileName).ToLowerInvariant();
                if (ext == ".csv")
                {
                    ExportCsv(table, sfd.FileName);
                    return;
                }

                // Excel interop - fallback to CSV when Excel is not available
                try
                {
                    Type excelType = Type.GetTypeFromProgID("Excel.Application");
                    if (excelType == null)
                    {
                        // Excel not installed -> fallback to CSV with same base name
                        var csvPath = Path.ChangeExtension(sfd.FileName, ".csv");
                        ExportCsv(table, csvPath);
                        MessageBox.Show($"Excel không được cài đặt. Đã xuất sang file CSV:\n{csvPath}", "Xuất CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    dynamic excel = Activator.CreateInstance(excelType);
                    excel.Visible = false;
                    dynamic wb = excel.Workbooks.Add();
                    dynamic ws = wb.ActiveSheet;

                    // Header
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        ws.Cells[1, c + 1].Value = table.Columns[c].ColumnName;
                    }

                    // Data
                    for (int r = 0; r < table.Rows.Count; r++)
                    {
                        for (int c = 0; c < table.Columns.Count; c++)
                        {
                            ws.Cells[r + 2, c + 1].Value = table.Rows[r][c];
                        }
                    }

                    wb.SaveAs(sfd.FileName);
                    wb.Close();
                    excel.Quit();

                    // Release COM
                    try { Marshal.FinalReleaseComObject(ws); } catch { }
                    try { Marshal.FinalReleaseComObject(wb); } catch { }
                    try { Marshal.FinalReleaseComObject(excel); } catch { }

                    MessageBox.Show("Xuất Excel thành công!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // On any error, try to fallback to CSV
                    try
                    {
                        var csvPath = Path.ChangeExtension(sfd.FileName, ".csv");
                        ExportCsv(table, csvPath);
                        MessageBox.Show($"Không thể xuất Excel ({ex.Message}). Đã xuất sang CSV:\n{csvPath}", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Xuất Excel thất bại: " + ex2.Message, "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private static void ExportCsv(DataTable table, string path)
        {
            using (var sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
            {
                // header
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    if (c > 0) sw.Write(",");
                    sw.Write(EscapeCsv(table.Columns[c].ColumnName));
                }
                sw.WriteLine();

                // rows
                foreach (DataRow row in table.Rows)
                {
                    for (int c = 0; c < table.Columns.Count; c++)
                    {
                        if (c > 0) sw.Write(",");
                        var val = row[c]?.ToString() ?? "";
                        sw.Write(EscapeCsv(val));
                    }
                    sw.WriteLine();
                }
            }
            MessageBox.Show("Đã xuất CSV.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string EscapeCsv(string input)
        {
            if (input == null) return "";
            bool needQuote = input.Contains(",") || input.Contains("\"") || input.Contains("\n") || input.Contains("\r");
            if (!needQuote) return input;
            return "\"" + input.Replace("\"", "\"\"") + "\"";
        }

        // -------------------------------------------------------------
        // Utility: convert DataGridView to DataTable (visible columns only)
        public static DataTable DataTableFromGrid(DataGridView dgv, bool includeInvisible = false)
        {
            if (dgv == null) return null;
            var dt = new DataTable();

            // columns
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!includeInvisible && !col.Visible) continue;
                string colName = string.IsNullOrWhiteSpace(col.HeaderText) ? col.Name : col.HeaderText;
                if (string.IsNullOrEmpty(colName)) colName = col.Name ?? "Col" + col.Index;
                // ensure unique
                string unique = colName;
                int attempt = 1;
                while (dt.Columns.Contains(unique)) { unique = colName + "_" + attempt; attempt++; }
                dt.Columns.Add(unique, typeof(string));
            }

            // rows
            foreach (DataGridViewRow r in dgv.Rows)
            {
                if (dgv.AllowUserToAddRows && r.IsNewRow) continue;
                var vals = new object[dt.Columns.Count];
                int ci = 0;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (!includeInvisible && !col.Visible) continue;
                    var v = r.Cells[col.Index]?.Value;
                    vals[ci++] = v == null || v == DBNull.Value ? string.Empty : v.ToString();
                }
                dt.Rows.Add(vals);
            }

            return dt;
        }

        // -------------------------------------------------------------
        // Check policy and export an IExportable
        public static void ExportIfAllowed(IExportable exportable, IWin32Window owner = null)
        {
            if (exportable == null) throw new ArgumentNullException(nameof(exportable));

            try
            {
                bool allowed = ExportPolicy.IsExportAllowed(exportable.ExportKey);
                if (!allowed)
                {
                    MessageBox.Show(owner, "Bạn không có quyền xuất báo cáo này.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dt = exportable.GetExportData();
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show(owner, "Không có dữ liệu để xuất.", "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ExportToExcel(dt, exportable.DefaultExportFileName ?? exportable.ExportKey ?? "Export");
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, "Lỗi xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
