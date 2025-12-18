using System;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GUI_QLNH
{
    public static class ExcelExportHelper
    {
        public const string DefaultFileName = "Export.xlsx";

        public static void Export(DataGridView dgv, IWin32Window owner = null)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show(owner, "Không có d? li?u ?? xu?t!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.Title = "L?u file Excel";
                sfd.FileName = DefaultFileName;

                if (sfd.ShowDialog() != DialogResult.OK) return;

                string filePath = sfd.FileName;

                Excel.Application excelApp = null;
                Excel.Workbook wb = null;
                Excel.Worksheet ws = null;

                try
                {
                    excelApp = new Excel.Application();
                    wb = excelApp.Workbooks.Add(Type.Missing);
                    ws = (Excel.Worksheet)wb.ActiveSheet;
                    ws.Name = "Sheet1";

                    int colCount = 0;
                    int excelCol = 1;

                    // header
                    for (int c = 0; c < dgv.Columns.Count; c++)
                    {
                        var col = dgv.Columns[c];
                        if (!col.Visible) continue;
                        if (col is DataGridViewCheckBoxColumn) continue; // skip checkbox column

                        ws.Cells[1, excelCol] = col.HeaderText;
                        colCount++;
                        excelCol++;
                    }

                    // style header
                    Excel.Range headerRange = ws.Range[ws.Cells[1, 1], ws.Cells[1, colCount]];
                    headerRange.Font.Bold = true;
                    headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                    // data
                    int excelRow = 2;
                    for (int r = 0; r < dgv.Rows.Count; r++)
                    {
                        if (dgv.AllowUserToAddRows && r == dgv.Rows.Count - 1) break;

                        excelCol = 1;
                        for (int c = 0; c < dgv.Columns.Count; c++)
                        {
                            var col = dgv.Columns[c];
                            if (!col.Visible) continue;
                            if (col is DataGridViewCheckBoxColumn) continue;

                            object val = dgv.Rows[r].Cells[c].Value;
                            ws.Cells[excelRow, excelCol] = (val == null ? "" : val.ToString());
                            excelCol++;
                        }

                        excelRow++;
                    }

                    // autofit
                    Excel.Range usedRange = ws.Range[ws.Cells[1, 1], ws.Cells[1 + dgv.Rows.Count, colCount]];
                    usedRange.Columns.AutoFit();

                    // save
                    wb.SaveAs(filePath);
                    wb.Close();
                    excelApp.Quit();

                    MessageBox.Show(owner, "Xu?t Excel thành công!\n" + filePath, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try { System.Diagnostics.Process.Start(filePath); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(owner, "L?i xu?t Excel: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try { if (wb != null) wb.Close(false); } catch { }
                    try { if (excelApp != null) excelApp.Quit(); } catch { }
                }
            }
        }
    }
}
