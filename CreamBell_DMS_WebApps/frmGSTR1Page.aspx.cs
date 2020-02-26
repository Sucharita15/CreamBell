
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Web.Services;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmGSTR1Page : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
        }
        
        private void ExcelDownload()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                SqlCommand cmd = new SqlCommand();
                DataTable dtDataByfilter;
                SqlDataAdapter ad;
                DataSet dsDataByfilter = new DataSet();
                string query = string.Empty;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                #region Generating Data For GSTR1_B2B
                cmd.CommandText = "USP_GSTR1_B2B";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("B2B");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_B2CL
                cmd.CommandText = "USP_GSTR1_B2CL";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("B2CL");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_B2CS
                cmd.CommandText = "USP_GSTR1_B2CS";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("B2CS");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_CDNR
                cmd.CommandText = "USP_GSTR1_CDNR";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("CDNR");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_CDNUR
                cmd.CommandText = "USP_GSTR1_CDNUR";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("CDNUR");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_HSN
                cmd.Parameters.Clear();
                cmd.CommandText = "USP_GSTR1_HSN";
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("HSN");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_DOCS
                cmd.CommandText = "USP_GSTR1_DOCS";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("DOCS");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                ExcelCreation(dsDataByfilter);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ExcelDownloadGSTR2()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                SqlCommand cmd = new SqlCommand();
                DataTable dtDataByfilter;
                SqlDataAdapter ad;
                DataSet dsDataByfilter = new DataSet();
                string query = string.Empty;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                #region Generating Data For GSTR1_B2B
                cmd.CommandText = "USP_GSTR2_B2B";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("B2B");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                
                #region Generating Data For GSTR1_CDNR
                cmd.CommandText = "USP_GSTR2_CDNR";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("CDNR");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_B2B
                cmd.CommandText = "USP_GSTR2_HSN";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("hsnsum");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                ExcelCreationGSTR2(dsDataByfilter);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ExcelDownloadGSTR3()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                SqlCommand cmd = new SqlCommand();
                DataTable dtDataByfilter;
                SqlDataAdapter ad;
                DataSet dsDataByfilter = new DataSet();
                string query = string.Empty;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                #region Generating Data For GSTR1_B2B
                cmd.CommandText = "USP_GSTR3_3_1_A";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("GSTR3_3_1_A");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_B2CL
                cmd.CommandText = "USP_GSTR3_3_1_C";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("GSTR3_3_1_C");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                #region Generating Data For GSTR1_B2CL
                cmd.CommandText = "USP_GSTR3_4_A_5";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@FROMMONTH", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TOMONTH", Convert.ToDateTime(txtToDate.Text));
                dtDataByfilter = new DataTable("GSTR3_4_A_5");
                ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                dsDataByfilter.Tables.Add(dtDataByfilter);
                #endregion

                ExcelCreationGSTR3(dsDataByfilter);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ExcelCreation(DataSet dtDataByfilter)
        {
            try
            {
                string myServerPath = Server.MapPath("~/ExcelTemplate/GSTR1TemplateV12.xlsx");
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wbExiting = excel.Workbooks.Open(myServerPath);
                Microsoft.Office.Interop.Excel.Worksheet shB2B = wbExiting.Sheets[2];
                Microsoft.Office.Interop.Excel.Worksheet shB2CL = wbExiting.Sheets[3];
                Microsoft.Office.Interop.Excel.Worksheet shB2CS = wbExiting.Sheets[4];
                Microsoft.Office.Interop.Excel.Worksheet shCDNR = wbExiting.Sheets[5];
                Microsoft.Office.Interop.Excel.Worksheet shCDNUR = wbExiting.Sheets[6];
                Microsoft.Office.Interop.Excel.Worksheet shHSN = wbExiting.Sheets[11];
                Microsoft.Office.Interop.Excel.Worksheet shDOCS = wbExiting.Sheets[12];

                #region Writing Data in Sheet [GSTR1_B2B]
                if (dtDataByfilter != null && dtDataByfilter.Tables["B2B"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["B2B"].Rows.Count; i++)
                    {
                        shB2B.Cells[i + 5, 1] = dtDataByfilter.Tables["B2B"].Rows[i][0];
                        shB2B.Cells[i + 5, 2] = dtDataByfilter.Tables["B2B"].Rows[i][1];
                        shB2B.Cells[i + 5, 3] = dtDataByfilter.Tables["B2B"].Rows[i][2];
                        shB2B.Cells[i + 5, 4] = dtDataByfilter.Tables["B2B"].Rows[i][3];
                        shB2B.Cells[i + 5, 5] = dtDataByfilter.Tables["B2B"].Rows[i][4];
                        shB2B.Cells[i + 5, 6] = dtDataByfilter.Tables["B2B"].Rows[i][5];
                        shB2B.Cells[i + 5, 7] = dtDataByfilter.Tables["B2B"].Rows[i][6];
                        shB2B.Cells[i + 5, 8] = dtDataByfilter.Tables["B2B"].Rows[i][7];
                        shB2B.Cells[i + 5, 9] = dtDataByfilter.Tables["B2B"].Rows[i][8];
                        shB2B.Cells[i + 5, 10] = dtDataByfilter.Tables["B2B"].Rows[i][9];
                        shB2B.Cells[i + 5, 11] = dtDataByfilter.Tables["B2B"].Rows[i][10];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_B2CL]
                if (dtDataByfilter != null && dtDataByfilter.Tables["B2CL"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["B2CL"].Rows.Count; i++)
                    {
                        shB2CL.Cells[i + 5, 1] = dtDataByfilter.Tables["B2CL"].Rows[i][0];
                        shB2CL.Cells[i + 5, 2] = dtDataByfilter.Tables["B2CL"].Rows[i][1];
                        shB2CL.Cells[i + 5, 3] = dtDataByfilter.Tables["B2CL"].Rows[i][2];
                        shB2CL.Cells[i + 5, 4] = dtDataByfilter.Tables["B2CL"].Rows[i][3];
                        shB2CL.Cells[i + 5, 5] = dtDataByfilter.Tables["B2CL"].Rows[i][4];
                        shB2CL.Cells[i + 5, 6] = dtDataByfilter.Tables["B2CL"].Rows[i][5];
                        shB2CL.Cells[i + 5, 7] = dtDataByfilter.Tables["B2CL"].Rows[i][6];
                        shB2CL.Cells[i + 5, 8] = dtDataByfilter.Tables["B2CL"].Rows[i][7];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_B2CS]
                if (dtDataByfilter != null && dtDataByfilter.Tables["B2CS"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["B2CS"].Rows.Count; i++)
                    {
                        shB2CS.Cells[i + 5, 1] = dtDataByfilter.Tables["B2CS"].Rows[i][0];
                        shB2CS.Cells[i + 5, 2] = dtDataByfilter.Tables["B2CS"].Rows[i][1];
                        shB2CS.Cells[i + 5, 3] = dtDataByfilter.Tables["B2CS"].Rows[i][2];
                        shB2CS.Cells[i + 5, 4] = dtDataByfilter.Tables["B2CS"].Rows[i][3];
                        shB2CS.Cells[i + 5, 5] = dtDataByfilter.Tables["B2CS"].Rows[i][4];
                        shB2CS.Cells[i + 5, 6] = dtDataByfilter.Tables["B2CS"].Rows[i][5];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_CDNR]
                if (dtDataByfilter != null && dtDataByfilter.Tables["CDNR"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["CDNR"].Rows.Count; i++)
                    {
                        shCDNR.Cells[i + 5, 1] = dtDataByfilter.Tables["CDNR"].Rows[i][0];
                        shCDNR.Cells[i + 5, 2] = dtDataByfilter.Tables["CDNR"].Rows[i][1];
                        shCDNR.Cells[i + 5, 3] = dtDataByfilter.Tables["CDNR"].Rows[i][2];
                        shCDNR.Cells[i + 5, 4] = dtDataByfilter.Tables["CDNR"].Rows[i][3];
                        shCDNR.Cells[i + 5, 5] = dtDataByfilter.Tables["CDNR"].Rows[i][4];
                        shCDNR.Cells[i + 5, 6] = dtDataByfilter.Tables["CDNR"].Rows[i][5];
                        shCDNR.Cells[i + 5, 7] = dtDataByfilter.Tables["CDNR"].Rows[i][6];
                        shCDNR.Cells[i + 5, 8] = dtDataByfilter.Tables["CDNR"].Rows[i][7];
                        shCDNR.Cells[i + 5, 9] = dtDataByfilter.Tables["CDNR"].Rows[i][8];
                        shCDNR.Cells[i + 5, 10] = dtDataByfilter.Tables["CDNR"].Rows[i][9];
                        shCDNR.Cells[i + 5, 11] = dtDataByfilter.Tables["CDNR"].Rows[i][10];
                        shCDNR.Cells[i + 5, 12] = dtDataByfilter.Tables["CDNR"].Rows[i][11];
                        shCDNR.Cells[i + 5, 13] = dtDataByfilter.Tables["CDNR"].Rows[i][12];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_CDNUR]
                if (dtDataByfilter != null && dtDataByfilter.Tables["CDNUR"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["CDNUR"].Rows.Count; i++)
                    {
                        shCDNUR.Cells[i + 5, 1] = dtDataByfilter.Tables["CDNUR"].Rows[i][0];
                        shCDNUR.Cells[i + 5, 2] = dtDataByfilter.Tables["CDNUR"].Rows[i][1];
                        shCDNUR.Cells[i + 5, 3] = dtDataByfilter.Tables["CDNUR"].Rows[i][2];
                        shCDNUR.Cells[i + 5, 4] = dtDataByfilter.Tables["CDNUR"].Rows[i][3];
                        shCDNUR.Cells[i + 5, 5] = dtDataByfilter.Tables["CDNUR"].Rows[i][4];
                        shCDNUR.Cells[i + 5, 6] = dtDataByfilter.Tables["CDNUR"].Rows[i][5];
                        shCDNUR.Cells[i + 5, 7] = dtDataByfilter.Tables["CDNUR"].Rows[i][6];
                        shCDNUR.Cells[i + 5, 8] = dtDataByfilter.Tables["CDNUR"].Rows[i][7];
                        shCDNUR.Cells[i + 5, 9] = dtDataByfilter.Tables["CDNUR"].Rows[i][8];
                        shCDNUR.Cells[i + 5, 10] = dtDataByfilter.Tables["CDNUR"].Rows[i][9];
                        shCDNUR.Cells[i + 5, 11] = dtDataByfilter.Tables["CDNUR"].Rows[i][10];
                        shCDNUR.Cells[i + 5, 12] = dtDataByfilter.Tables["CDNUR"].Rows[i][11];
                        shCDNUR.Cells[i + 5, 13] = dtDataByfilter.Tables["CDNUR"].Rows[i][12];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_HSN]
                if (dtDataByfilter != null && dtDataByfilter.Tables["HSN"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["HSN"].Rows.Count; i++)
                    {
                        shHSN.Cells[i + 5, 1] = dtDataByfilter.Tables["HSN"].Rows[i][0];
                        shHSN.Cells[i + 5, 2] = dtDataByfilter.Tables["HSN"].Rows[i][1];
                        shHSN.Cells[i + 5, 3] = dtDataByfilter.Tables["HSN"].Rows[i][2];
                        shHSN.Cells[i + 5, 4] = dtDataByfilter.Tables["HSN"].Rows[i][3];
                        shHSN.Cells[i + 5, 5] = dtDataByfilter.Tables["HSN"].Rows[i][4];
                        shHSN.Cells[i + 5, 6] = dtDataByfilter.Tables["HSN"].Rows[i][5];
                        shHSN.Cells[i + 5, 7] = dtDataByfilter.Tables["HSN"].Rows[i][6];
                        shHSN.Cells[i + 5, 8] = dtDataByfilter.Tables["HSN"].Rows[i][7];
                        shHSN.Cells[i + 5, 9] = dtDataByfilter.Tables["HSN"].Rows[i][8];
                        shHSN.Cells[i + 5, 10] = dtDataByfilter.Tables["HSN"].Rows[i][9];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_DOCS]
                if (dtDataByfilter != null && dtDataByfilter.Tables["DOCS"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["DOCS"].Rows.Count; i++)
                    {
                        shDOCS.Cells[i + 5, 1] = dtDataByfilter.Tables["DOCS"].Rows[i][0];
                        shDOCS.Cells[i + 5, 2] = dtDataByfilter.Tables["DOCS"].Rows[i][1];
                        shDOCS.Cells[i + 5, 3] = dtDataByfilter.Tables["DOCS"].Rows[i][2];
                        shDOCS.Cells[i + 5, 4] = dtDataByfilter.Tables["DOCS"].Rows[i][3];
                        shDOCS.Cells[i + 5, 5] = dtDataByfilter.Tables["DOCS"].Rows[i][4];
                    }
                }
                #endregion

                string strUniqueFileName = string.Empty;
                strUniqueFileName = Session["USERID"].ToString();

                string myServerPath1 = Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx");
                FileInfo file = new FileInfo(myServerPath1);
                if (file.Exists)
                {
                    file.Delete();
                }

                wbExiting.SaveAs(Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx"), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                excel.DisplayAlerts = false;
                wbExiting.Close(false, Type.Missing, Type.Missing);
                excel.Quit();

                FileInfo file1 = new FileInfo(myServerPath1);

                if (file1.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=GSTR1TemplateV12.xlsx");
                    Response.AddHeader("Content-Type", "application/Excel");
                    Response.ContentType = "application/vnd.xls";
                    Response.AddHeader("Content-Length", file1.Length.ToString());
                    Response.WriteFile(file1.FullName);
                    // file.Delete();    
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
                }


            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ExcelCreationGSTR2(DataSet dtDataByfilter)
        {
            try
            {
                string myServerPath = Server.MapPath("~/ExcelTemplate/GSTR2TemplateV12.xlsx");
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wbExiting = excel.Workbooks.Open(myServerPath);
                Microsoft.Office.Interop.Excel.Worksheet shB2B = wbExiting.Sheets[3];
                //Microsoft.Office.Interop.Excel.Worksheet shB2CL = wbExiting.Sheets[3];
                //Microsoft.Office.Interop.Excel.Worksheet shB2CS = wbExiting.Sheets[4];
                //Microsoft.Office.Interop.Excel.Worksheet shCDNUR = wbExiting.Sheets[5];
                Microsoft.Office.Interop.Excel.Worksheet shCDNR = wbExiting.Sheets[7];
                Microsoft.Office.Interop.Excel.Worksheet shHSN = wbExiting.Sheets[13];
                //Microsoft.Office.Interop.Excel.Worksheet shDOCS = wbExiting.Sheets[12];

                #region Writing Data in Sheet [GSTR2_B2B]
                if (dtDataByfilter != null && dtDataByfilter.Tables["B2B"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["B2B"].Rows.Count; i++)
                    {
                        shB2B.Cells[i + 5, 1] = dtDataByfilter.Tables["B2B"].Rows[i][0];
                        shB2B.Cells[i + 5, 2] = dtDataByfilter.Tables["B2B"].Rows[i][1];
                        shB2B.Cells[i + 5, 3] = dtDataByfilter.Tables["B2B"].Rows[i][2];
                        shB2B.Cells[i + 5, 4] = dtDataByfilter.Tables["B2B"].Rows[i][3];
                        shB2B.Cells[i + 5, 5] = dtDataByfilter.Tables["B2B"].Rows[i][4];
                        shB2B.Cells[i + 5, 6] = dtDataByfilter.Tables["B2B"].Rows[i][5];
                        shB2B.Cells[i + 5, 7] = dtDataByfilter.Tables["B2B"].Rows[i][6];
                        shB2B.Cells[i + 5, 8] = dtDataByfilter.Tables["B2B"].Rows[i][7];
                        shB2B.Cells[i + 5, 9] = dtDataByfilter.Tables["B2B"].Rows[i][8];
                        shB2B.Cells[i + 5, 10] = dtDataByfilter.Tables["B2B"].Rows[i][9];
                        shB2B.Cells[i + 5, 11] = dtDataByfilter.Tables["B2B"].Rows[i][10];
                        shB2B.Cells[i + 5, 12] = dtDataByfilter.Tables["B2B"].Rows[i][11];
                        shB2B.Cells[i + 5, 13] = dtDataByfilter.Tables["B2B"].Rows[i][12];
                        shB2B.Cells[i + 5, 14] = dtDataByfilter.Tables["B2B"].Rows[i][13];
                        shB2B.Cells[i + 5, 15] = dtDataByfilter.Tables["B2B"].Rows[i][14];
                        shB2B.Cells[i + 5, 16] = dtDataByfilter.Tables["B2B"].Rows[i][15];
                        shB2B.Cells[i + 5, 17] = dtDataByfilter.Tables["B2B"].Rows[i][16];
                        shB2B.Cells[i + 5, 18] = dtDataByfilter.Tables["B2B"].Rows[i][17];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR1_B2CL]
                //if (dtDataByfilter != null && dtDataByfilter.Tables["B2CL"].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtDataByfilter.Tables["B2CL"].Rows.Count; i++)
                //    {
                //        shB2CL.Cells[i + 5, 1] = dtDataByfilter.Tables["B2CL"].Rows[i][0];
                //        shB2CL.Cells[i + 5, 2] = dtDataByfilter.Tables["B2CL"].Rows[i][1];
                //        shB2CL.Cells[i + 5, 3] = dtDataByfilter.Tables["B2CL"].Rows[i][2];
                //        shB2CL.Cells[i + 5, 4] = dtDataByfilter.Tables["B2CL"].Rows[i][3];
                //        shB2CL.Cells[i + 5, 5] = dtDataByfilter.Tables["B2CL"].Rows[i][4];
                //        shB2CL.Cells[i + 5, 6] = dtDataByfilter.Tables["B2CL"].Rows[i][5];
                //        shB2CL.Cells[i + 5, 7] = dtDataByfilter.Tables["B2CL"].Rows[i][6];
                //        shB2CL.Cells[i + 5, 8] = dtDataByfilter.Tables["B2CL"].Rows[i][7];
                //    }
                //}
                //#endregion

                //#region Writing Data in Sheet [GSTR1_B2CS]
                //if (dtDataByfilter != null && dtDataByfilter.Tables["B2CS"].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtDataByfilter.Tables["B2CS"].Rows.Count; i++)
                //    {
                //        shB2CS.Cells[i + 5, 1] = dtDataByfilter.Tables["B2CS"].Rows[i][0];
                //        shB2CS.Cells[i + 5, 2] = dtDataByfilter.Tables["B2CS"].Rows[i][1];
                //        shB2CS.Cells[i + 5, 3] = dtDataByfilter.Tables["B2CS"].Rows[i][2];
                //        shB2CS.Cells[i + 5, 4] = dtDataByfilter.Tables["B2CS"].Rows[i][3];
                //        shB2CS.Cells[i + 5, 5] = dtDataByfilter.Tables["B2CS"].Rows[i][4];
                //        shB2CS.Cells[i + 5, 6] = dtDataByfilter.Tables["B2CS"].Rows[i][5];
                //    }
                //}
                //#endregion

                #region Writing Data in Sheet [GSTR1_CDNR]
                if (dtDataByfilter != null && dtDataByfilter.Tables["CDNR"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["CDNR"].Rows.Count; i++)
                    {
                        shCDNR.Cells[i + 5, 1] = dtDataByfilter.Tables["CDNR"].Rows[i][0];
                        shCDNR.Cells[i + 5, 2] = dtDataByfilter.Tables["CDNR"].Rows[i][1];
                        shCDNR.Cells[i + 5, 3] = dtDataByfilter.Tables["CDNR"].Rows[i][2];
                        shCDNR.Cells[i + 5, 4] = dtDataByfilter.Tables["CDNR"].Rows[i][3];
                        shCDNR.Cells[i + 5, 5] = dtDataByfilter.Tables["CDNR"].Rows[i][4];
                        shCDNR.Cells[i + 5, 6] = dtDataByfilter.Tables["CDNR"].Rows[i][5];
                        shCDNR.Cells[i + 5, 7] = dtDataByfilter.Tables["CDNR"].Rows[i][6];
                        shCDNR.Cells[i + 5, 8] = dtDataByfilter.Tables["CDNR"].Rows[i][7];
                        shCDNR.Cells[i + 5, 9] = dtDataByfilter.Tables["CDNR"].Rows[i][8];
                        shCDNR.Cells[i + 5, 10] = dtDataByfilter.Tables["CDNR"].Rows[i][9];
                        shCDNR.Cells[i + 5, 11] = dtDataByfilter.Tables["CDNR"].Rows[i][10];
                        shCDNR.Cells[i + 5, 12] = dtDataByfilter.Tables["CDNR"].Rows[i][11];
                        shCDNR.Cells[i + 5, 13] = dtDataByfilter.Tables["CDNR"].Rows[i][12];
                        shCDNR.Cells[i + 5, 14] = dtDataByfilter.Tables["CDNR"].Rows[i][13];
                        shCDNR.Cells[i + 5, 15] = dtDataByfilter.Tables["CDNR"].Rows[i][14];
                        shCDNR.Cells[i + 5, 16] = dtDataByfilter.Tables["CDNR"].Rows[i][15];
                        shCDNR.Cells[i + 5, 17] = dtDataByfilter.Tables["CDNR"].Rows[i][16];
                        shCDNR.Cells[i + 5, 18] = dtDataByfilter.Tables["CDNR"].Rows[i][17];
                        shCDNR.Cells[i + 5, 19] = dtDataByfilter.Tables["CDNR"].Rows[i][18];
                        shCDNR.Cells[i + 5, 20] = dtDataByfilter.Tables["CDNR"].Rows[i][19];
                        shCDNR.Cells[i + 5, 21] = dtDataByfilter.Tables["CDNR"].Rows[i][20];
                    }
                }
                #endregion

                //#region Writing Data in Sheet [GSTR1_CDNUR]
                //if (dtDataByfilter != null && dtDataByfilter.Tables["CDNUR"].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtDataByfilter.Tables["CDNUR"].Rows.Count; i++)
                //    {
                //        shCDNUR.Cells[i + 5, 1] = dtDataByfilter.Tables["CDNUR"].Rows[i][0];
                //        shCDNUR.Cells[i + 5, 2] = dtDataByfilter.Tables["CDNUR"].Rows[i][1];
                //        shCDNUR.Cells[i + 5, 3] = dtDataByfilter.Tables["CDNUR"].Rows[i][2];
                //        shCDNUR.Cells[i + 5, 4] = dtDataByfilter.Tables["CDNUR"].Rows[i][3];
                //        shCDNUR.Cells[i + 5, 5] = dtDataByfilter.Tables["CDNUR"].Rows[i][4];
                //        shCDNUR.Cells[i + 5, 6] = dtDataByfilter.Tables["CDNUR"].Rows[i][5];
                //        shCDNUR.Cells[i + 5, 7] = dtDataByfilter.Tables["CDNUR"].Rows[i][6];
                //        shCDNUR.Cells[i + 5, 8] = dtDataByfilter.Tables["CDNUR"].Rows[i][7];
                //        shCDNUR.Cells[i + 5, 9] = dtDataByfilter.Tables["CDNUR"].Rows[i][8];
                //        shCDNUR.Cells[i + 5, 10] = dtDataByfilter.Tables["CDNUR"].Rows[i][9];
                //        shCDNUR.Cells[i + 5, 11] = dtDataByfilter.Tables["CDNUR"].Rows[i][10];
                //        shCDNUR.Cells[i + 5, 12] = dtDataByfilter.Tables["CDNUR"].Rows[i][11];
                //        shCDNUR.Cells[i + 5, 13] = dtDataByfilter.Tables["CDNUR"].Rows[i][12];
                //    }
                //}
                //#endregion

                //#region Writing Data in Sheet [GSTR1_DOCS]
                //if (dtDataByfilter != null && dtDataByfilter.Tables["DOCS"].Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtDataByfilter.Tables["DOCS"].Rows.Count; i++)
                //    {
                //        shDOCS.Cells[i + 5, 1] = dtDataByfilter.Tables["DOCS"].Rows[i][0];
                //        shDOCS.Cells[i + 5, 2] = dtDataByfilter.Tables["DOCS"].Rows[i][1];
                //        shDOCS.Cells[i + 5, 3] = dtDataByfilter.Tables["DOCS"].Rows[i][2];
                //        shDOCS.Cells[i + 5, 4] = dtDataByfilter.Tables["DOCS"].Rows[i][3];
                //        shDOCS.Cells[i + 5, 5] = dtDataByfilter.Tables["DOCS"].Rows[i][4];
                //    }
                //}
                #endregion

                #region Writing Data in Sheet [GSTR2_hsnsum]
                if (dtDataByfilter != null && dtDataByfilter.Tables["hsnsum"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["hsnsum"].Rows.Count; i++)
                    {
                        shHSN.Cells[i + 5, 1] = dtDataByfilter.Tables["hsnsum"].Rows[i][0];
                        shHSN.Cells[i + 5, 2] = dtDataByfilter.Tables["hsnsum"].Rows[i][1];
                        shHSN.Cells[i + 5, 3] = dtDataByfilter.Tables["hsnsum"].Rows[i][2];
                        shHSN.Cells[i + 5, 4] = dtDataByfilter.Tables["hsnsum"].Rows[i][3];
                        shHSN.Cells[i + 5, 5] = dtDataByfilter.Tables["hsnsum"].Rows[i][4];
                        shHSN.Cells[i + 5, 6] = dtDataByfilter.Tables["hsnsum"].Rows[i][5];
                        shHSN.Cells[i + 5, 7] = dtDataByfilter.Tables["hsnsum"].Rows[i][6];
                        shHSN.Cells[i + 5, 8] = dtDataByfilter.Tables["hsnsum"].Rows[i][7];
                        shHSN.Cells[i + 5, 9] = dtDataByfilter.Tables["hsnsum"].Rows[i][8];
                        shHSN.Cells[i + 5, 10] = dtDataByfilter.Tables["hsnsum"].Rows[i][9];
                        //shHSN.Cells[i + 5, 11] = dtDataByfilter.Tables["hsnsum"].Rows[i][10];
                    }
                }
                #endregion

                string strUniqueFileName = string.Empty;
                strUniqueFileName = Session["USERID"].ToString();

                string myServerPath1 = Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx");
                FileInfo file = new FileInfo(myServerPath1);
                if (file.Exists)
                {
                    file.Delete();
                }

                wbExiting.SaveAs(Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx"), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                excel.DisplayAlerts = false;
                wbExiting.Close(false, Type.Missing, Type.Missing);
                excel.Quit();

                FileInfo file1 = new FileInfo(myServerPath1);

                if (file1.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=GSTR2TemplateV12.xlsx");
                    Response.AddHeader("Content-Type", "application/Excel");
                    Response.ContentType = "application/vnd.xls";
                    Response.AddHeader("Content-Length", file1.Length.ToString());
                    Response.WriteFile(file1.FullName);
                    // file.Delete();    
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
                }


            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ExcelCreationGSTR3(DataSet dtDataByfilter)
        {
            try
            {
                string myServerPath = Server.MapPath("~/ExcelTemplate/GSTR3TemplateV12.xlsx");
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wbExiting = excel.Workbooks.Open(myServerPath);
                Microsoft.Office.Interop.Excel.Worksheet GSTR3B = wbExiting.Sheets[2];
                //Microsoft.Office.Interop.Excel.Worksheet shB2CL = wbExiting.Sheets[3];
                //Microsoft.Office.Interop.Excel.Worksheet shB2CS = wbExiting.Sheets[4];
                //Microsoft.Office.Interop.Excel.Worksheet shCDNUR = wbExiting.Sheets[5];
                //Microsoft.Office.Interop.Excel.Worksheet shCDNR = wbExiting.Sheets[6];
                //Microsoft.Office.Interop.Excel.Worksheet shHSN = wbExiting.Sheets[12];
                //Microsoft.Office.Interop.Excel.Worksheet shDOCS = wbExiting.Sheets[12];

                #region Writing Data in Sheet [GSTR3B-GSTR3_3_1_A]
                if (dtDataByfilter != null && dtDataByfilter.Tables["GSTR3_3_1_A"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["GSTR3_3_1_A"].Rows.Count; i++)
                    {
                        GSTR3B.Cells[i + 11, 3] = dtDataByfilter.Tables["GSTR3_3_1_A"].Rows[i][0];
                        GSTR3B.Cells[i + 11, 4] = dtDataByfilter.Tables["GSTR3_3_1_A"].Rows[i][1];
                        GSTR3B.Cells[i + 11, 5] = dtDataByfilter.Tables["GSTR3_3_1_A"].Rows[i][2];
                        GSTR3B.Cells[i + 11, 6] = dtDataByfilter.Tables["GSTR3_3_1_A"].Rows[i][3];
                        GSTR3B.Cells[i + 11, 7] = dtDataByfilter.Tables["GSTR3_3_1_A"].Rows[i][4];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR3B-GSTR3_3_1_C]
                if (dtDataByfilter != null && dtDataByfilter.Tables["GSTR3_3_1_C"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["GSTR3_3_1_C"].Rows.Count; i++)
                    {
                        GSTR3B.Cells[i + 13, 3] = dtDataByfilter.Tables["GSTR3_3_1_C"].Rows[i][0];
                        GSTR3B.Cells[i + 13, 4] = dtDataByfilter.Tables["GSTR3_3_1_C"].Rows[i][1];
                        GSTR3B.Cells[i + 13, 5] = dtDataByfilter.Tables["GSTR3_3_1_C"].Rows[i][2];
                        GSTR3B.Cells[i + 13, 6] = dtDataByfilter.Tables["GSTR3_3_1_C"].Rows[i][3];
                        GSTR3B.Cells[i + 13, 7] = dtDataByfilter.Tables["GSTR3_3_1_C"].Rows[i][4];
                    }
                }
                #endregion

                #region Writing Data in Sheet [GSTR3B-GSTR3_4_A_5]
                if (dtDataByfilter != null && dtDataByfilter.Tables["GSTR3_4_A_5"].Rows.Count > 0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables["GSTR3_4_A_5"].Rows.Count; i++)
                    {
                        GSTR3B.Cells[i + 26, 3] = dtDataByfilter.Tables["GSTR3_4_A_5"].Rows[i][0];
                        GSTR3B.Cells[i + 26, 4] = dtDataByfilter.Tables["GSTR3_4_A_5"].Rows[i][1];
                        GSTR3B.Cells[i + 26, 5] = dtDataByfilter.Tables["GSTR3_4_A_5"].Rows[i][2];
                        GSTR3B.Cells[i + 26, 6] = dtDataByfilter.Tables["GSTR3_4_A_5"].Rows[i][3];
                        //GSTR3B.Cells[i + 6, 6] = dtDataByfilter.Tables["GSTR3_4_A_5"].Rows[i][4];
                    }
                }
                #endregion

                string strUniqueFileName = string.Empty;
                strUniqueFileName = Session["USERID"].ToString();

                string myServerPath1 = Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx");
                FileInfo file = new FileInfo(myServerPath1);
                if (file.Exists)
                {
                    file.Delete();
                }

                wbExiting.SaveAs(Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx"), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                excel.DisplayAlerts = false;
                wbExiting.Close(false, Type.Missing, Type.Missing);
                excel.Quit();

                FileInfo file1 = new FileInfo(myServerPath1);

                if (file1.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=GSTR3TemplateV12.xlsx");
                    Response.AddHeader("Content-Type", "application/Excel");
                    Response.ContentType = "application/vnd.xls";
                    Response.AddHeader("Content-Length", file1.Length.ToString());
                    Response.WriteFile(file1.FullName);
                    // file.Delete();    
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateStaticParameter())
                {
                    if (rdoGSTR1.Checked)
                    {
                        ExcelDownload();
                    }
                    else if (rdoGSTR2.Checked)
                    {
                        ExcelDownloadGSTR2();
                    }
                    else if (rdoGSTR3.Checked)
                    {
                        ExcelDownloadGSTR3();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('"+ex.Message.ToString()+"');", true);
            }
        }

        private bool ValidateStaticParameter()
        {
            bool value = true;
            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
            }
            if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty && Convert.ToDateTime(txtFromDate.Text)<Convert.ToDateTime("01-Jul-2017"))
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('From Date Cannot be less than 01-Jul-2017 !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty && Convert.ToDateTime(txtToDate.Text) < Convert.ToDateTime("01-Jul-2017"))
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('To Date Cannot be less than 01-Jul-2017 !');", true);
            }
            return value;
        }
    }
}