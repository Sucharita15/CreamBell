using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchaseRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);// DateTime.Today.ToString();
                txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today); //string.Format("{0:dd/MMM/yyyy }", DateTime.Today.AddDays(1));// DateTime.Today.ToString();
                ShowPurchaseRegister();
            }

        }

        private void ShowPurchaseRegister()
        {
            App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
           
            //======old=====================
            //string query = "Select PIL.PURCH_RECIEPTNO,PIH.DOCUMENT_NO, CONVERT(nvarchar(20), PIH.DOCUMENT_DATE,105) as RECEIPTDATE , PIH.SALE_INVOICENO, " +
            //               " CONVERT(nvarchar(20), PIH.SALE_INVOICEDATE,105) as SALE_INVOICEDATE , PIH.PURCH_INDENTNO, " +
            //               " INVT.PRODUCT_GROUP, (PIL.PRODUCT_CODE +'-'+ INVT.PRODUCT_NAME) as PRODUCT, PIL.CRATES, PIL.BOX, PIL.LTR " +
            //               " from ax.ACXPURCHINVRECIEPTLINE PIL INNER JOIN ax.ACXPURCHINVRECIEPTHEADER PIH ON PIL.PURCH_RECIEPTNO=PIH.DOCUMENT_NO " +
            //               " INNER JOIN AX.INVENTTABLE INVT ON PIL.PRODUCT_CODE=INVT.ITEMID " +
            //               " WHERE PIL.SITEID='" + Session["SiteCode"].ToString() + "' and PIL.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and PIH.DOCUMENT_DATE between CONVERT(VARCHAR, GETDATE(), 23) and dateadd(dd,1,getdate()) ";

            //=====new-==============
            //string query = "Select PIL.PURCH_RECIEPTNO,PIH.DOCUMENT_NO, CONVERT(nvarchar(20), PIH.DOCUMENT_DATE,105) as RECEIPTDATE , PIH.SALE_INVOICENO, " +
            //              " CONVERT(nvarchar(20), PIH.SALE_INVOICEDATE,105) as SALE_INVOICEDATE , PIH.PURCH_INDENTNO, " +
            //              " INVT.PRODUCT_GROUP, PIL.PRODUCT_CODE,(PIL.PRODUCT_CODE +'-'+ INVT.PRODUCT_NAME) as PRODUCT, PIL.CRATES,PIL.LTR " +
            //              " , case when PIL.BasicValue!=0 and PIL.[Remark]!='FOC' then PIL.BOX else 0 end as BOXQTY"+
            //              ", case when PIL.BasicValue=0 and PIL.[Remark]='FOC' then PIL.BOX else 0 end as FOCQTY"+
            //              ", PIL.BOX as TOTALQty"+
            //              ", [BASICVALUE],[TRDDISCPERC],[TRDDISCVALUE],[PRICE_EQUALVALUE],[TAX],[TAXAMOUNT],[AMOUNT]"+
            //              " from ax.ACXPURCHINVRECIEPTLINE PIL INNER JOIN ax.ACXPURCHINVRECIEPTHEADER PIH ON PIL.PURCH_RECIEPTNO=PIH.DOCUMENT_NO and pil.siteid=pih.site_code " +
            //              " INNER JOIN AX.INVENTTABLE INVT ON PIL.PRODUCT_CODE=INVT.ITEMID " +
            //              " WHERE PIL.SITEID='" + Session["SiteCode"].ToString() + "' and PIL.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and PIH.DOCUMENT_DATE between CONVERT(VARCHAR, GETDATE(), 23) and dateadd(dd,1,getdate()) ";
            //DataTable dt = obj.GetData(query);

            string query = "ACX_USP_PurchaseRegister";
            List<string> ilist = new List<string>();
            List<string> item = new List<string>();
            DataTable dt = new DataTable();

            ilist.Add("@Site_Code"); item.Add(ucRoleFilters.GetCommaSepartedSiteId());
            ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
            ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
            ilist.Add("@EndDate"); item.Add(txtToDate.Text);

            dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
            
            if (dt.Rows.Count > 0)
            {
                gridPurchaseRegister.DataSource = dt;
                gridPurchaseRegister.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                Session["PurchaseRegister"] = dt;
            }
            else
            {
                LblMessage.Text = string.Empty;
            }

        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            //if (gridPurchaseRegister.Rows.Count > 0)
            //{
                //ExportToExcel();
                ExportToExcelNew();
                uppanel.Update();
            //}
            //else
            //{
            //    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
            //    LblMessage.Text = "Cannot Export Data due to No Records available. !";
            //    LblMessage.Visible = true;
            //    uppanel.Update();
            //}
        }

        private void ExportToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PurchaseRegister.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gridPurchaseRegister.AllowPaging = false;
                DataTable dt = Session["PurchaseRegister"] as DataTable;
                gridPurchaseRegister.DataSource = dt;
                gridPurchaseRegister.DataBind();

                gridPurchaseRegister.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gridPurchaseRegister.HeaderRow.Cells)
                {
                    cell.BackColor = gridPurchaseRegister.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gridPurchaseRegister.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gridPurchaseRegister.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gridPurchaseRegister.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gridPurchaseRegister.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        private void ExportToExcelNew()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            // create dataset
            try
            {
                if (rdoDetailedView.Checked == true)
                {
                    //CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                    string query = "ACX_USP_PurchaseRegister";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    DataTable dt = new DataTable();

                    ilist.Add("@Site_Code"); item.Add(ucRoleFilters.GetCommaSepartedSiteId());
                    ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                    //=================Create Excel==========

                    string attachment = "attachment; filename=PurchaseRegister_Detailed.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dt.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dt.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();
                }
                if (rdoSummarizedView.Checked == true)
                {

                    string query = "ACX_USP_PurchaseRegister_SummaryView";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    DataTable dt = new DataTable();

                    ilist.Add("@Site_Code"); item.Add(ucRoleFilters.GetCommaSepartedSiteId());
                    //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                    GridView gvexport = new GridView();
                    gvexport.ShowFooter = true;
                    gvexport.DataSource = dt;
                    gvexport.DataBind();
                    GridViewFooterCalculateNew(dt, gvexport);

                    //------------------------------------------------------------------------------//

                    string bottomquery = "ACX_USP_PurchaseRegister_SummaryView2";
                    List<string> bottomilist = new List<string>();
                    List<string> bottomitem = new List<string>();
                    DataTable bottomdt = new DataTable();

                    bottomilist.Add("@Site_Code"); bottomitem.Add(ucRoleFilters.GetCommaSepartedSiteId());
                    //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    bottomilist.Add("@StartDate"); bottomitem.Add(txtFromDate.Text);
                    bottomilist.Add("@EndDate"); bottomitem.Add(txtToDate.Text);

                    bottomdt = obj.GetData_New(bottomquery, CommandType.StoredProcedure, bottomilist, bottomitem);
                    GridView gvexportbottom = new GridView();
                    gvexportbottom.ShowFooter = true;
                    gvexportbottom.DataSource = bottomdt;
                    gvexportbottom.DataBind();
                    GridViewFooterCalculatebottomNew(bottomdt, gvexportbottom);

                    //if (gridPurchaseRegister.Rows.Count > 0 || gridpurchaseregisterbottom.Rows.Count > 0)
                    if (gvexport.Rows.Count > 0 || gvexportbottom.Rows.Count > 0)
                    {
                        string sFileName = "PurchaseRegister_Summarised.xls";

                        sFileName = sFileName.Replace("/", "");
                        // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                        Response.ContentType = "application/vnd.ms-excel";
                        EnableViewState = false;

                        System.IO.StringWriter objSW = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                        System.IO.StringWriter objSWT = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter objHTWT = new System.Web.UI.HtmlTextWriter(objSWT);


                        //dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
                        //dg.RenderControl(objHTW);
                        string name = "Purchase Register Summarised Report";

                        //string DistributoName = ddlSiteId.SelectedItem.Value + " - " + ddlSiteId.SelectedItem.Text;
                        Response.Write("<table><tr><td colspan='5' style='width:100px;align-items:center; font:18px;'> <b> " + name + "</b> </td></tr> <tr><td><b>From Date:  " + txtFromDate.Text.Replace(",", "") + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text.Replace(",", "") + "</b></td></tr></table>");
                        Response.Write("<br/>");
                        // STYLE THE SHEET AND WRITE DATA TO IT.
                        Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                            "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                        //gridPurchaseRegister.RenderControl(objHTW);
                        gvexport.RenderControl(objHTW);
                        Response.Write(objSW.ToString());
                        Response.Write("<br/>");
                        Response.Write("<br/>");
                        gvexportbottom.RenderControl(objHTWT);
                        Response.Write(objSWT.ToString());
                        // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
                        //Response.Write("<table><tr><td></td><td></td><td></td><td><b>Total: </b></td><td></td><td><b>" + units + "</b></td><td><b>" + ltrs + "</b></td><td><b>" + BASICVALUE + "</b></td><td><b>" + TRDDISCVALUE + "</b></td><td><b>" + Priceequalvalue + "</b></td><td><b>" + secdiscamount + "</b></td><td><b>" + schsplvalue + "</b></td><td><b>" + taxablevalue + "</b></td><td></td><td><b>" + Tax1amount + "</b></td><td></td><td><b>" + Tax2amount + "</b></td><td><b>" + invvalue + "</b></td></tr></table>");

                        Response.End();
                        //dg = null;
                    }
                    else
                    {
                        LblMessage.Text = "No Data Exits Between This Date Range  !";
                        LblMessage.Visible = true;
                    }

                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
           

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Charset = "";
            //string FileName = "PurchaseRegister" + DateTime.Now + ".xls";
            //StringWriter strwritter = new StringWriter();
            //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            //gridPurchaseRegister.GridLines = GridLines.Both;
            //gridPurchaseRegister.HeaderStyle.Font.Bold = true;
            //gridPurchaseRegister.RenderControl(htmltextwrtter);
            //Response.Write(strwritter.ToString());
            //Response.End();      
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private bool ValidateSearch()
        {
            bool value = false;
            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
                LblMessage.Text = "Please Provide The Date Range Parameter !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
                LblMessage.Text = "Please Provide The TO Date Range Parameter !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
                LblMessage.Text = "Please Provide The FROM Date Range Parameter !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            return value;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bool b =  ValidateSearch();

            if (b == true)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                try
                {
                    DateTime endDate = Convert.ToDateTime(this.txtToDate.Text);
                    endDate.AddDays(1);
                    DateTime end = endDate.AddDays(1);
                                      
                    string FromDate = txtFromDate.Text;
                    string ToDate =  end.ToString("yyyy-MM-dd");//txtToDate.Text;


                    //string query = "Select PIL.PURCH_RECIEPTNO,PIH.DOCUMENT_NO, CONVERT(nvarchar(20), PIH.DOCUMENT_DATE,105) as RECEIPTDATE , PIH.SALE_INVOICENO, " +
                    //               " CONVERT(nvarchar(20), PIH.SALE_INVOICEDATE,105) as SALE_INVOICEDATE , PIH.PURCH_INDENTNO, " +
                    //               " INVT.PRODUCT_GROUP, (PIL.PRODUCT_CODE +'-'+ INVT.PRODUCT_NAME) as PRODUCT, PIL.CRATES, PIL.BOX, PIL.LTR " +
                    //               " from ax.ACXPURCHINVRECIEPTLINE PIL INNER JOIN ax.ACXPURCHINVRECIEPTHEADER PIH ON PIL.PURCH_RECIEPTNO=PIH.DOCUMENT_NO " +
                    //               " INNER JOIN AX.INVENTTABLE INVT ON PIL.PRODUCT_CODE=INVT.ITEMID " +
                    //               " WHERE PIL.SITEID='" + Session["SiteCode"].ToString() + "' and PIL.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and " +
                    //               " PIH.DOCUMENT_DATE BETWEEN '" + FromDate + "' and '" + ToDate + "' Order By PIH.DOCUMENT_DATE ";

                    //=================new=================

                    //string query = "Select PIL.PURCH_RECIEPTNO,PIH.DOCUMENT_NO, CONVERT(nvarchar(20), PIH.DOCUMENT_DATE,105) as RECEIPTDATE , PIH.SALE_INVOICENO, " +
                    //      " CONVERT(nvarchar(20), PIH.SALE_INVOICEDATE,105) as SALE_INVOICEDATE , PIH.PURCH_INDENTNO, " +
                    //      " INVT.PRODUCT_GROUP, PIL.PRODUCT_CODE,(PIL.PRODUCT_CODE +'-'+ INVT.PRODUCT_NAME) as PRODUCT, PIL.CRATES,PIL.LTR " +
                    //      " , case when PIL.BasicValue!=0 and PIL.[Remark]!='FOC' then PIL.BOX else 0 end as BOXQTY" +
                    //      ", case when PIL.BasicValue=0 and PIL.[Remark]='FOC' then PIL.BOX else 0 end as FOCQTY" +
                    //      ", PIL.BOX as TOTALQty" +
                    //      ", [BASICVALUE],[TRDDISCPERC],[TRDDISCVALUE],[PRICE_EQUALVALUE],[TAX],[TAXAMOUNT],[AMOUNT]" +
                    //      " from ax.ACXPURCHINVRECIEPTLINE PIL INNER JOIN ax.ACXPURCHINVRECIEPTHEADER PIH ON PIL.PURCH_RECIEPTNO=PIH.DOCUMENT_NO " +
                    //      " INNER JOIN AX.INVENTTABLE INVT ON PIL.PRODUCT_CODE=INVT.ITEMID " +
                    //      " WHERE PIL.SITEID='" + Session["SiteCode"].ToString() + "' and PIL.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and " +
                    //      " PIH.DOCUMENT_DATE BETWEEN '" + FromDate + "' and '" + ToDate + "' Order By PIH.DOCUMENT_DATE ";

                    //DataTable dt = obj.GetData(query);

                    if (rdoDetailedView.Checked == true)
                    {

                        string query = "ACX_USP_PurchaseRegister";
                        List<string> ilist = new List<string>();
                        List<string> item = new List<string>();
                        DataTable dt = new DataTable();

                        ilist.Add("@Site_Code"); item.Add(ucRoleFilters.GetCommaSepartedSiteId());
                        ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                        ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                        if (dt.Rows.Count > 0)
                        {
                            gridpurchaseregisterbottom.Visible = false;
                            gridPurchaseRegister.ShowFooter = false;
                            gridPurchaseRegister.DataSource = dt;
                            gridPurchaseRegister.DataBind();
                            LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                            Session["PurchaseRegister"] = dt;
                        }
                        else
                        {
                            gridpurchaseregisterbottom.Visible = false;
                            LblMessage.Text = string.Empty;
                            gridPurchaseRegister.DataSource = dt;
                            gridPurchaseRegister.DataBind();
                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                            LblMessage.Text = "No Data Exits Between This Date Range  !";
                            LblMessage.Visible = true;
                            uppanel.Update();
                        }
                    }
                    if(rdoSummarizedView.Checked == true)
                    {
                        string query = "ACX_USP_PurchaseRegister_SummaryView";
                        List<string> ilist = new List<string>();
                        List<string> item = new List<string>();
                        DataTable dt = new DataTable();

                        ilist.Add("@Site_Code"); item.Add(ucRoleFilters.GetCommaSepartedSiteId());
                        //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                        ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                        if (dt.Rows.Count > 0)
                        {
                            gridPurchaseRegister.ShowFooter = true;
                            gridPurchaseRegister.DataSource = dt;
                            gridPurchaseRegister.DataBind();
                            GridViewFooterCalculate(dt);
                            LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                            Session["PurchaseRegister"] = dt;
                        }
                        else
                        {
                            LblMessage.Text = string.Empty;
                            gridPurchaseRegister.DataSource = dt;
                            gridPurchaseRegister.DataBind();
                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                            LblMessage.Text = "No Data Exits Between This Date Range  !";
                            LblMessage.Visible = true;
                            uppanel.Update();
                        }

                        //------------------------------------------------------------------------//

                        string bottomquery = "ACX_USP_PurchaseRegister_SummaryView2";
                        List<string> bottomilist = new List<string>();
                        List<string> bottomitem = new List<string>();
                        DataTable bottomdt = new DataTable();

                        bottomilist.Add("@Site_Code"); bottomitem.Add(ucRoleFilters.GetCommaSepartedSiteId());
                        //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        bottomilist.Add("@StartDate"); bottomitem.Add(txtFromDate.Text);
                        bottomilist.Add("@EndDate"); bottomitem.Add(txtToDate.Text);

                        bottomdt = obj.GetData_New(bottomquery, CommandType.StoredProcedure, bottomilist, bottomitem);

                        if (bottomdt.Rows.Count > 0)
                        {
                            gridpurchaseregisterbottom.Visible = true;
                            gridpurchaseregisterbottom.ShowFooter = true;
                            gridpurchaseregisterbottom.DataSource = bottomdt;
                            gridpurchaseregisterbottom.DataBind();
                            GridViewFooterCalculatebottom(bottomdt);
                            LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                            //Session["PurchaseRegister"] = dt;
                        }
                        else
                        {
                            gridpurchaseregisterbottom.Visible = true;
                            LblMessage.Text = string.Empty;
                            gridpurchaseregisterbottom.DataSource = bottomdt;
                            gridpurchaseregisterbottom.DataBind();
                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                            LblMessage.Text = "No Data Exits Between This Date Range  !";
                            LblMessage.Visible = true;
                            uppanel.Update();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }         
        }

        private void GridViewFooterCalculatebottom(DataTable dt)
        {
            int units = dt.AsEnumerable().Sum(row => row.Field<int>("UNITS"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTRS"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASICVALUE"));
            decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            decimal Priceequalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRICE EQUAL VALUE"));
            decimal secdiscamount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC DISC AMOUNT"));
            decimal schsplvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCH SPL VAL"));
            decimal taxablevalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLEVALUE"));

            decimal Tax1amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            decimal Tax2amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));

            //==============================

            gridpurchaseregisterbottom.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gridpurchaseregisterbottom.FooterRow.Cells[0].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[0].Text = "TOTAL";
            gridpurchaseregisterbottom.FooterRow.Cells[0].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[1].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[1].Text = units.ToString();
            gridpurchaseregisterbottom.FooterRow.Cells[1].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[2].Text = ltrs.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[2].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[3].Text = BASICVALUE.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[3].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[4].Text = TRDDISCVALUE.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[4].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[5].Text = Priceequalvalue.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[5].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[6].Text = secdiscamount.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[6].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[7].Text = schsplvalue.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[7].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[8].Text = taxablevalue.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[8].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[10].Text = Tax1amount.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[10].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[12].Text = Tax2amount.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[12].Font.Bold = true;

            gridpurchaseregisterbottom.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
            gridpurchaseregisterbottom.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            gridpurchaseregisterbottom.FooterRow.Cells[13].Text = invvalue.ToString("N2");
            gridpurchaseregisterbottom.FooterRow.Cells[13].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void GridViewFooterCalculatebottomNew(DataTable dt,GridView gvname)
        {
            int units = dt.AsEnumerable().Sum(row => row.Field<int>("UNITS"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTRS"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASICVALUE"));
            decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            decimal Priceequalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRICE EQUAL VALUE"));
            decimal secdiscamount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC DISC AMOUNT"));
            decimal schsplvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCH SPL VAL"));
            decimal taxablevalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLEVALUE"));

            decimal Tax1amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            decimal Tax2amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));

            //==============================

            gvname.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //gvname.FooterRow.Cells[0].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[0].Text = "TOTAL";
            gvname.FooterRow.Cells[0].Font.Bold = true;

            gvname.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[1].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[1].Text = units.ToString();
            gvname.FooterRow.Cells[1].Font.Bold = true;

            gvname.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[2].Text = ltrs.ToString("N2");
            gvname.FooterRow.Cells[2].Font.Bold = true;

            gvname.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[3].Text = BASICVALUE.ToString("N2");
            gvname.FooterRow.Cells[3].Font.Bold = true;

            gvname.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[4].Text = TRDDISCVALUE.ToString("N2");
            gvname.FooterRow.Cells[4].Font.Bold = true;

            gvname.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[5].Text = Priceequalvalue.ToString("N2");
            gvname.FooterRow.Cells[5].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            gvname.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[6].Text = secdiscamount.ToString("N2");
            gvname.FooterRow.Cells[6].Font.Bold = true;

            gvname.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[7].Text = schsplvalue.ToString("N2");
            gvname.FooterRow.Cells[7].Font.Bold = true;

            gvname.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[8].Text = taxablevalue.ToString("N2");
            gvname.FooterRow.Cells[8].Font.Bold = true;

            gvname.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[10].Text = Tax1amount.ToString("N2");
            gvname.FooterRow.Cells[10].Font.Bold = true;

            gvname.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[12].Text = Tax2amount.ToString("N2");
            gvname.FooterRow.Cells[12].Font.Bold = true;

            gvname.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[13].Text = invvalue.ToString("N2");
            gvname.FooterRow.Cells[13].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void GridViewFooterCalculate(DataTable dt)
        {
            int units = dt.AsEnumerable().Sum(row => row.Field<int>("UNITS"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTRS"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASICVALUE"));
            decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            decimal Priceequalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRICE EQUAL VALUE"));
            decimal secdiscamount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC DISC AMOUNT"));
            decimal schsplvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCH SPL VAL"));
            decimal taxablevalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLEVALUE"));

            decimal Tax1amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            decimal Tax2amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));

            //==============================

            gridPurchaseRegister.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            gridPurchaseRegister.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[4].Text = "TOTAL";
            gridPurchaseRegister.FooterRow.Cells[4].Font.Bold = true;
            
            gridPurchaseRegister.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[6].Text = units.ToString();
            gridPurchaseRegister.FooterRow.Cells[6].Font.Bold = true;
            
            gridPurchaseRegister.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[7].Text = ltrs.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[7].Font.Bold = true;
            
            gridPurchaseRegister.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[8].Text = BASICVALUE.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[8].Font.Bold = true;
            
            gridPurchaseRegister.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[9].Text = TRDDISCVALUE.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[9].Font.Bold = true;
            
            gridPurchaseRegister.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[10].Text = Priceequalvalue.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[10].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            gridPurchaseRegister.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[11].Text = secdiscamount.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[11].Font.Bold = true;

            gridPurchaseRegister.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[12].Text = schsplvalue.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[12].Font.Bold = true;

            gridPurchaseRegister.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[13].Text = taxablevalue.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[13].Font.Bold = true;

            gridPurchaseRegister.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[15].Text = Tax1amount.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[15].Font.Bold = true;

            gridPurchaseRegister.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[17].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[17].Text = Tax2amount.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[17].Font.Bold = true;

            gridPurchaseRegister.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Left;
            gridPurchaseRegister.FooterRow.Cells[18].ForeColor = System.Drawing.Color.MidnightBlue;
            gridPurchaseRegister.FooterRow.Cells[18].Text = invvalue.ToString("N2");
            gridPurchaseRegister.FooterRow.Cells[18].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void GridViewFooterCalculateNew(DataTable dt,GridView gvname)
        {
            int units = dt.AsEnumerable().Sum(row => row.Field<int>("UNITS"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTRS"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASICVALUE"));
            decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            decimal Priceequalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRICE EQUAL VALUE"));
            decimal secdiscamount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC DISC AMOUNT"));
            decimal schsplvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCH SPL VAL"));
            decimal taxablevalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLEVALUE"));

            decimal Tax1amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            decimal Tax2amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));

            //==============================

            gvname.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            //gvname.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[4].Text = "TOTAL";
            gvname.FooterRow.Cells[4].Font.Bold = true;

            gvname.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[6].Text = units.ToString();
            gvname.FooterRow.Cells[6].Font.Bold = true;

            gvname.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[7].Text = ltrs.ToString("N2");
            gvname.FooterRow.Cells[7].Font.Bold = true;

            gvname.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[8].Text = BASICVALUE.ToString("N2");
            gvname.FooterRow.Cells[8].Font.Bold = true;

            gvname.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[9].Text = TRDDISCVALUE.ToString("N2");
            gvname.FooterRow.Cells[9].Font.Bold = true;

            gvname.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[10].Text = Priceequalvalue.ToString("N2");
            gvname.FooterRow.Cells[10].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            gvname.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[11].Text = secdiscamount.ToString("N2");
            gvname.FooterRow.Cells[11].Font.Bold = true;

            gvname.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[12].Text = schsplvalue.ToString("N2");
            gvname.FooterRow.Cells[12].Font.Bold = true;

            gvname.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[13].Text = taxablevalue.ToString("N2");
            gvname.FooterRow.Cells[13].Font.Bold = true;

            gvname.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[15].Text = Tax1amount.ToString("N2");
            gvname.FooterRow.Cells[15].Font.Bold = true;

            gvname.FooterRow.Cells[17].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[17].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[17].Text = Tax2amount.ToString("N2");
            gvname.FooterRow.Cells[17].Font.Bold = true;

            gvname.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[18].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[18].Text = invvalue.ToString("N2");
            gvname.FooterRow.Cells[18].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void ExportToPDF()
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
             "attachment;filename=GridViewExport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridPurchaseRegister.AllowPaging = false;
            gridPurchaseRegister.DataBind();
            gridPurchaseRegister.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End(); 
        }

        protected void rdoDetailedView_CheckedChanged(object sender, EventArgs e)
        {
            gridPurchaseRegister.DataSource = null;
            gridPurchaseRegister.DataBind();
            LblMessage.Text = string.Empty;
            gridpurchaseregisterbottom.DataSource = null;
            gridpurchaseregisterbottom.DataBind();
        }
    }
}