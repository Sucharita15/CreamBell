using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CreamBell_DMS_WebApps.App_Code
{
    public class ExcelExport
    {
        public void ExportXLSB(HttpResponse response, DataTable dataTable, string tableName, string fileName, string tempXlsbFilePath)
        {
            response.ContentType = "application/vnd.ms-excel.sheet.binary.macroEnabled.12"; //xlsb
            response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsb");
            ExportToXL(tempXlsbFilePath, dataTable, tableName);
            response.WriteFile(tempXlsbFilePath);
        }

        public void ExportXLSX(HttpResponse response, DataTable dataTable, string tableName, string fileName)
        {
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; //xlsx
            response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xlsx");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, tableName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(response.OutputStream);
                }
            }
        }

        public void ExportXLS(HttpResponse response, DataTable dataTable, string tableName, string fileName)
        {
            response.ContentType = "application/vnd.ms-excel"; // xls
            response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, tableName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(response.OutputStream);
                }
            }
        }

        private void ExportToXL(string sheetToCreate, DataTable dtToExport, string tableName)
        {
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in dtToExport.Rows) rows.Add(row);
            ExportToXL(sheetToCreate, rows, dtToExport, tableName);
        }

        private void ExportToXL(string sheetToCreate, List<DataRow> selectedRows, DataTable origDataTable, string tableName)
        {
            char Space = ' ';
            string dest = sheetToCreate;

            if (File.Exists(dest))
            {
                File.Delete(dest);
            }

            sheetToCreate = dest;

            if (tableName == null)
            {
                tableName = string.Empty;
            }

            tableName = tableName.Trim().Replace(Space, '_');
            if (tableName.Length == 0)
            {
                tableName = origDataTable.TableName.Replace(Space, '_');
            }

            if (tableName.Length == 0)
            {
                tableName = "NoTableName";
            }

            if (tableName.Length > 30)
            {
                tableName = tableName.Substring(0, 30);
            }

            //Excel names are less than 31 chars
            string queryCreateExcelTable = "CREATE TABLE [" + tableName + "] (";
            Dictionary<string, string> colNames = new Dictionary<string, string>();

            foreach (DataColumn dc in origDataTable.Columns)
            {
                //Cause the query to name each of the columns to be created.
                string modifiedcolName = dc.ColumnName;//.Replace(Space, '_').Replace('.', '#');
                string origColName = dc.ColumnName;
                colNames.Add(modifiedcolName, origColName);

                queryCreateExcelTable += "[" + modifiedcolName + "]" + " text,";

            }

            queryCreateExcelTable = queryCreateExcelTable.TrimEnd(new char[] { Convert.ToChar(",") }) + ")";

            //adds the closing parentheses to the query string
            if (selectedRows.Count > 65000 && sheetToCreate.ToLower().EndsWith(".xls"))
            {
                //use Excel 2007 for large sheets.
                sheetToCreate = sheetToCreate.ToLower().Replace(".xls", string.Empty) + ".xlsx";
            }

            string strCn = string.Empty;
            string ext = System.IO.Path.GetExtension(sheetToCreate).ToLower();
            if (ext == ".xls") strCn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sheetToCreate + "; Extended Properties='Excel 8.0;HDR=YES'";
            if (ext == ".xlsx") strCn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sheetToCreate + ";Extended Properties='Excel 12.0 Xml;HDR=YES' ";
            if (ext == ".xlsb") strCn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sheetToCreate + ";Extended Properties='Excel 12.0;HDR=YES' ";
            if (ext == ".xlsm") strCn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sheetToCreate + ";Extended Properties='Excel 12.0 Macro;HDR=YES' ";

            System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection(strCn);
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(queryCreateExcelTable, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + tableName + "]", cn);
            System.Data.OleDb.OleDbCommandBuilder cb = new System.Data.OleDb.OleDbCommandBuilder(da);

            //creates the INSERT INTO command
            cb.QuotePrefix = "[";
            cb.QuoteSuffix = "]";
            cmd = cb.GetInsertCommand();

            //gets a hold of the INSERT INTO command.
            foreach (DataRow row in selectedRows)
            {
                foreach (System.Data.OleDb.OleDbParameter param in cmd.Parameters)
                {
                    param.Value = row[colNames[param.SourceColumn.Replace('#', '.')]];
                }

                cmd.ExecuteNonQuery(); //INSERT INTO command.
            }
            cn.Close();
            cn.Dispose();
            da.Dispose();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }
    }
}