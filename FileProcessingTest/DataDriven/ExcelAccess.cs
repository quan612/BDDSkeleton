using System.Collections.Generic;
using System.Data;
using System.IO;
using FileProcessingTest.Framework.Help;
using OfficeOpenXml;

namespace FileProcessingTest.Framework.DataDriven
{
    public class ExcelAccess
    {
        public static string DataDrivenPath { get; set; }

        public static List<T> GetDataFromExcelToList<T>(string workSheetName) where T:class, new()
        {
            var tempTable = DataTableFromExcel(workSheetName, true);
            return tempTable.DataTableToList<T>();
        }

        private static DataTable DataTableFromExcel(string workSheetName, bool hasHeader = true)
        {
           
            if (DataDrivenPath == null)
            {
                return null;
            }
            else
            {
                using (var pck = new ExcelPackage())
                {
                    using (var stream = File.OpenRead(DataDrivenPath))
                    {
                        pck.Load(stream);
                    }
                    var workbook = pck.Workbook;
                    var ws = workbook.Worksheets[workSheetName];
                    var tbl = new DataTable();

                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    var a = ws.Dimension.End.Row;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        var row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                    return tbl;
                }
            }
        }
    }
}


