using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel; 
using System.Data; 
using System.Web.UI.WebControls; 
using NPOI.XSSF.UserModel;

namespace Elifarma.WebReport.Code
{
    public class WebUtils
    {
        public static string ReportPath(string reportname)
        {
            return Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/Reports"), reportname);
        }

        public static string get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static MemoryStream ExportDataTableToExcel(DataTable adt_DataTable,string title)
        {
            try
            {
                // file stream object
                MemoryStream stream = new MemoryStream();
                if (adt_DataTable == null)
                {
                    return stream;
                }
                // Open the Excel object
                HSSFWorkbook workbook = new HSSFWorkbook();

                // Excel Sheet object
                var sheet = workbook.CreateSheet(title);

                //set date format
                var cellStyleDate = workbook.CreateCellStyle();
                var format = workbook.CreateDataFormat();
                cellStyleDate.DataFormat = format.GetFormat("dd/MM/yyyy");

                // NPOI operation using Excel table
                var row = sheet.CreateRow(0);
                int count = 0;
                for (int i = 0; i < adt_DataTable.Columns.Count; i++) // row to generate a first sheet name 
                {
                    var cell = row.CreateCell(count++);
                    cell.SetCellValue(adt_DataTable.Columns[i].Caption);
                }
                // import table data to excel
                for (int i = 0; i < adt_DataTable.Rows.Count; i++)
                {
                    var rows = sheet.CreateRow(i + 1);
                    count = 0;
                    for (int j = 0; j < adt_DataTable.Columns.Count; j++)
                    {
                        var cell = rows.CreateCell(count++);
                        Type type = adt_DataTable.Rows[i][j].GetType();
                        if (type == typeof(int) || type == typeof(Int16)
                            || type == typeof(Int32) || type == typeof(Int64))
                        {
                            cell.SetCellValue(Convert.ToInt32(adt_DataTable.Rows[i][j]));
                        }
                        else
                        {
                            if (type == typeof(float) || type == typeof(double) || type == typeof(Double))
                            {
                                cell.SetCellValue((Double)adt_DataTable.Rows[i][j]);
                            }
                            else
                            {
                                if (type == typeof(DateTime))
                                {
                                    cell.SetCellValue(((DateTime)adt_DataTable.Rows[i][j]).ToString("yyyy-MM-dd HH:mm"));
                                }
                                else
                                {
                                    if (type == typeof(bool) || type == typeof(Boolean))
                                    {
                                        cell.SetCellValue((bool)adt_DataTable.Rows[i][j]);
                                    }
                                    else
                                    {
                                        cell.SetCellValue(adt_DataTable.Rows[i][j].ToString());
                                    }
                                }
                            }
                        }
                    }
                }

                // Save the excel document
                sheet.ForceFormulaRecalculation = true;

                workbook.Write(stream);
                workbook = null; 

                return stream;
            }
            catch (Exception ex)
            {
                return new MemoryStream();
            }
        }

        public static void ExportToExcel(DataTable data, string FileName)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow rowHead = sheet.CreateRow(0);

            //Fill in the header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //Fill in the content
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }

            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (FileStream stream = File.OpenWrite(FileName))
            {
                workbook.Write(stream);
                stream.Close();
            }
            GC.Collect();
        }
        }
    }