using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmStockMovementReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                fillSiteAndState();
            }
        }
        protected void fillSiteAndState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                ddlState.Items.Add("All...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
                ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
        }

        //protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string strQuery = @"Select PSR_Code +'-'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
        //                   " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
        //                   " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
        //                   " where B.Site_code ='" + Session["SiteCode"].ToString() + "')";
        //    ddlPSR.Items.Clear();
        //    ddlPSR.Items.Add("Select...");
        //    baseObj.BindToDropDown(ddlPSR, strQuery, "PSRName", "PSR_Code");
        //}

        protected void ddlPSR_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dt = new DataTable();
            bool b = Validate();
            if (b == true)
            {
                try
                {

                    ShowReportSummary();
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        private void ShowReportSummary()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
           DataTable dtSetData = null;

            try
            {

                //dtSetData = new DataTable();
                //string query = "ACX_STOCKMOVEMENT_REPORT";
                //List<string> ilist = new List<string>();
                //List<string> item = new List<string>();
                //string StateCode, SiteCode;
                //SiteCode = "";

                //if (ddlSiteId.SelectedIndex >= 0)
                //{
                //    if (ddlSiteId.SelectedItem.Text != "All...")
                //    {
                //        SiteCode = ddlSiteId.SelectedItem.Value;
                //    }
                //}
                //ilist.Add("@SITEID"); item.Add(SiteCode);

                //StateCode = "";
                //if (ddlState.SelectedIndex > 0)
                //{
                //    StateCode = ddlState.SelectedItem.Value;
                //}
                //ilist.Add("@STATE"); item.Add(StateCode);
                //ilist.Add("@FROMDATE"); item.Add(txtFromDate.Text);
                //ilist.Add("@TODATE"); item.Add(txtToDate.Text);

                //dtSetData = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                //if (dtSetData.Rows.Count > 0)
                //{
                //    gridCurrentStcok.DataSource = dtSetData;
                //    gridCurrentStcok.DataBind();

                //}
                //else
                //{


                //} 
                
               string querymy = "";
               if ( BulletinTypeDropDown.Text == "Usable To Non Usable")
                {
                    querymy = "8";
                   
                }
                if (BulletinTypeDropDown.Text == "Non Usable To Usable")
                {
                    querymy = "10";

                }
                if (BulletinTypeDropDown.Text == "Non Usable To Scrap")
                {
                    querymy = "11";

                }
                string SITEID = "";
                SITEID = ddlSiteId.SelectedValue;
                //Session["filter"] = "";
                //Session["filter"] = " DocumentType = '" + querymy + "' and SiteCode = '" + SITEID + "'";
                //Session["filter"] = Session["filter"] + " and CONVERT(datetime,DocumentDate,103) between '" + txtFromDate.Text + "'" + " and '" + txtToDate.Text + "'";
                //string query4 = "select IH.ProductCode,IL.PRODUCT_NAME,IH.DocumentDate,IH.TransLocation,(SELECT TOP 1 SIH.TransLocation FROM AX.ACXINVENTTRANS SIH WHERE SIH.DocumentDate = IH.DocumentDate AND SIH.DocumentNo = IH.DocumentNo AND SIH.ProductCode = IH.ProductCode AND SIH.DocumentType = IH.DocumentType AND SIH.TransId = IH.TransId AND SIH.TransType = IH.TransType AND SIH.TransQty > 0 AND SIH.SiteCode = IH.SiteCode) TOLOCATION,-(IH.TransQty) as TransQty,  ISNULL((SELECT TOP 1 AMOUNT FROM[DBO].[ACX_UDF_GETPRICE](GETDATE(), (select top 1 D1.ACX_PRICEGROUP from[ax].logisticsaddressstate D1 where D1.STATEID = (select statecode from[ax].[inventsite] where siteid = IH.SiteCode)),IH.PRODUCTCODE)),0) AS BASERATE,-(IH.TransQty)*ISNULL((SELECT TOP 1 AMOUNT FROM [DBO].[ACX_UDF_GETPRICE](GETDATE(),(select top 1 D1.ACX_PRICEGROUP from [ax].logisticsaddressstate D1 where D1.STATEID = (select statecode from[ax].[inventsite] IK where siteid=IH.SiteCode)),IH.PRODUCTCODE)),0)  as BaseValue from ax.ACXINVENTTRANS IH LEFT JOIN[AX].[inventtable] IL on IH.ProductCode = IL.itemid WHERE IH.TransQty < 0 AND " + Session["filter"] + "";
                DataTable dt = new DataTable();
                List<string> ilist = new List<string>();
                List<string> item = new List<string>();
                //string query = "ACX_STOCKMOVEMENT_REPORT";
                string query = "ACX_STOCKMOVEMENT_REPORT1";
                ilist.Add("@DocumentType"); item.Add(querymy);
                ilist.Add("@SiteCode"); item.Add(SITEID);
                ilist.Add("@DocumentDate1"); item.Add(txtFromDate.Text);
                ilist.Add("@DocumentDate2"); item.Add(txtToDate.Text);
                dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
               // dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    object sumLtr, SumQtyLtr, sumLtr1;
                    sumLtr = dt.Compute("Sum(TransQty)", "");
                    sumLtr1 = dt.Compute("Sum(BASERATE)", "");
                    SumQtyLtr = dt.Compute("Sum(BaseValue)", "");
                    // dt.Rows.Add("TOTAL", "", "", sumLtr, "", "", "", SumQtyLtr);
                    dt.Rows.Add("", "", "", "", "TOTAL", sumLtr, "0", SumQtyLtr);
                    gridStockMovementReport.DataSource = dt;
                    gridStockMovementReport.DataBind();
                }
                else
                {
                    Label1.Text = "Records does not exits";

                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
       

        


        private void LoadDataInReportViewerDetail(DataTable dtSetData)
        {
            //try
            //{
            //    if (dtSetData.Rows.Count > 0)
            //    {
            //        ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
            //        ReportViewer1.LocalReport.DataSources.Clear();
            //        ReportViewer1.LocalReport.DataSources.Add(RDS1);

            //        ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PSR_DSR.rdl");
            //        ReportViewer1.LocalReport.Refresh();
            //        ReportViewer1.Visible = true;
            //    }
            //    else
            //    {
            //        LblMessage.Text = "No Records Exists !!";
            //        ReportViewer1.Visible = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LblMessage.Text = ex.Message.ToString();
            //}
        }

        private bool Validate()
        {
            bool b;
            if (txtFromDate.Text == string.Empty)
            {
                string message = "alert('Select From Date !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtFromDate.Focus();
                b = false;
                return b;
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }
            if (txtToDate.Text == string.Empty)
            {
                string message = "alert('Select To Date !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtToDate.Focus();
                b = false;
                return b;
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }

            //if (ddlPSR.SelectedItem.Value == "Select..." || ddlPSR.Text == string.Empty)
            //{
            //    string message = "alert('Select PSR Name !');";
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            //    ddlPSR.Focus();
            //    b = false;
            //    return b;
            //}
            if (BulletinTypeDropDown.SelectedItem.Value == "--Select--" || BulletinTypeDropDown.Text == string.Empty)
            {
                string message = "alert('Select TransType !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                BulletinTypeDropDown.Focus();
                b = false;
                return b;
            }           
            if (ddlState.SelectedItem.Value == "Select..." || ddlSiteId.Text == string.Empty)
            {
                string message = "alert('Select State Name !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                ddlState.Focus();
                b = false;
                return b;
            }
            if (ddlSiteId.SelectedItem.Value == "All..." || ddlSiteId.Text == string.Empty)
            {
                string message = "alert('Select Site Name !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                ddlSiteId.Focus();
                b = false;
                return b;
            }
            return b;
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void BtnExportToExel_Click(object sender, EventArgs e)
        {
            if (gridStockMovementReport.Rows.Count > 0)
            {
               
                ExportToExcelNew();
                uppanel.Update();
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
                uppanel.Update();
            }

        }
        private void ExportToExcelNew()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "StockMovementReport" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gridStockMovementReport.GridLines = GridLines.Both;
            gridStockMovementReport.HeaderStyle.Font.Bold = true;
            gridStockMovementReport.RenderControl(htmltextwrtter);            
            //gridStockMovementReport.RenderControl(htmltextwrtter);
            //{
            //    Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
            //}
            Response.Write(strwritter.ToString());
            Response.End();
        }
      
    }
}