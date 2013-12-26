using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aspose.Cells;
using System.Drawing;

namespace K12.LogView.Modules
{
    //匯出DataGridView內容的小工具
    internal class DataGridViewExport
    {
        Workbook _workbook;
        Worksheet _worksheet;
        List<int> _colIndexes;

        /// <summary>
        /// 匯出DataGridView內容的小工具
        /// </summary>
        /// <param name="dgv">傳入DataGridView即可</param>
        public DataGridViewExport(DataGridView dgv)
        {
            _workbook = new Workbook();
            _workbook.Worksheets.Clear();
            _worksheet = _workbook.Worksheets[_workbook.Worksheets.Add()];
            _worksheet.Name = "Sheet1";
            _colIndexes = new List<int>();

            int sheetRowIndex = 0;
            int sheetColIndex = 0;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible == false) continue;
                _colIndexes.Add(col.Index);
                _worksheet.Cells[sheetRowIndex, sheetColIndex++].PutValue(col.HeaderText);
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                sheetRowIndex++;
                sheetColIndex = 0;
                foreach (int colIndex in _colIndexes)
                {
                    //string superstring = "";
                    //for (int x = 0; x < 15000; x++)
                    //{
                    //    superstring += x.ToString();
                    //    if (superstring.Length > 32001)
                    //        break;
                    //}
                    string Cellvalue = "" + row.Cells[colIndex].Value;
                    if (Cellvalue.Length > 32000)
                    {
                        //string cell1 = Cellvalue.Substring(0, 15000);
                        //string cell2 = Cellvalue.Substring(15001, Cellvalue.Length);
                        //_worksheet.Cells[sheetRowIndex, sheetColIndex++].PutValue(cell1);
                        //_worksheet.Cells[sheetRowIndex, sheetColIndex++].PutValue(cell2);
                        _worksheet.Cells[sheetRowIndex, _colIndexes.Count].PutValue("欄位內容超過32,000字元容許度!!");
                        _worksheet.Cells[sheetRowIndex, _colIndexes.Count].Style.Font.Color = Color.Red;

                        _worksheet.Cells[sheetRowIndex, sheetColIndex++].PutValue(Cellvalue.Substring(0, 32000));
                    }
                    else
                    {
                        //_worksheet.Cells[sheetRowIndex, _colIndexes.Count].PutValue(superstring);
                        _worksheet.Cells[sheetRowIndex, sheetColIndex++].PutValue(Cellvalue);
                    }
                }
            }

            _worksheet.AutoFitColumns();
        }

        public void Save(string path)
        {
            try
            {
                _workbook.Save(path, FileFormatType.Excel2003);

            }
            catch (Exception ex)
            {
                MessageBox.Show("匯出失敗：" + ex.Message);
            }
        }
    }
}
