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
    public partial class frmOpenSalesOrderReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                fillSiteAndState();
//                ShowDetails();
            }
        }

        private void ShowDetails()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string query = "ACX_USP_OpenSaleOrder";                                  
            List<string> ilist = new List<string>();
            List<string> item = new List<string>();
            DataTable dt = new DataTable();

            ilist.Add("@Site_Code");item.Add(Session["SiteCode"].ToString());
            ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
            ilist.Add("@StartDate");item.Add(txtFromDate.Text);
            ilist.Add("@EndDate"); item.Add(txtToDate.Text);

            dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
            
            if (dt.Rows.Count > 0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                Session["OpenSO"] = dt;
            }
            else
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                LblMessage.Text = string.Empty;
            }

        }

        protected void fillSiteAndState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = obj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY Name ";
                ddlState.Items.Add("Select...");
                obj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();

                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId= '" + Session["SiteCode"].ToString() + "'  ORDER BY LS.Name";
                obj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                obj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = obj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' Order By NAME";
                // ddlSiteId.Items.Add("All...");
                obj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order By NAME";
                //ddlSiteId.Items.Add("All...");
                obj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
        }
        private bool ValidateSearch()
        {
            bool value = false;
            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
                LblMessage.Text = "Please Provide The Date Range Parameter !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            else if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
                LblMessage.Text = "Please Provide The TO Date Range Parameter !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            else if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
                LblMessage.Text = "Please Provide The FROM Date Range Parameter  !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            else if (ddlState.SelectedItem.Text == "Select...")
            {
                value = false;
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
                LblMessage.Text = "Please Select State  !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
            else
            {
                value = true;
            }
            return value;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
                ExportToExcelNew();
                uppanel.Update();
        }

        //private void ExportToExcelNew()
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.Charset = "";
        //    string FileName = "OpenSalesOrder" + DateTime.Now + ".xls";
        //    StringWriter strwritter = new StringWriter();
        //    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        //    gvDetails.GridLines = GridLines.Both;
        //    gvDetails.HeaderStyle.Font.Bold = true;
        //    gvDetails.RenderControl(htmltextwrtter);
        //    {
        //        Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
        //    } 
        //    Response.Write(strwritter.ToString());
        //    Response.End();
        //}

        private void ExportToExcelNew()
        {
            bool b = ValidateSearch();

            if (b == true)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                try
                {

                    string query = "ACX_USP_OpenSaleOrder";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    DataTable dt = new DataTable();

                    if (ddlSiteId.SelectedItem.Text == "")
                    {
                        ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
                    }
                    else
                    {
                        ilist.Add("@Site_Code"); item.Add(ddlSiteId.SelectedItem.Value.ToString());
                    }
                    ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                   
                    //string FromDate = txtFromDate.Text;                  
                    //string ToDate = txtToDate.Text;
                 
                    if (dt.Rows.Count > 0)
                    {
                        GridView gv = new GridView();
                        gv.DataSource = dt;
                        gv.DataBind();
                        string FileName = "OpenSalesOrder" + DateTime.Now + ".xls";
                        StringWriter strwritter = new StringWriter();
                        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                        LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                        gv.GridLines = GridLines.Both;
                        gv.HeaderStyle.Font.Bold = true;
                        gv.RenderControl(htmltextwrtter);
                        {
                            Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
                        }
                        Response.Write(strwritter.ToString());
                        Response.End();

                    }
                    else
                    {

                        LblMessage.Text = string.Empty;

                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                        LblMessage.Text = "No Data Exits Between This Date Range ! !";
                        LblMessage.Visible = true;
                        uppanel.Update();

                    }

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }            
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bool b = ValidateSearch();

            if (b == true)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                try
                {

                    string query = "ACX_USP_OpenSaleOrder";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    DataTable dt = new DataTable();

                    if (ddlSiteId.SelectedItem.Text == "")
                    {
                        ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
                    }
                    else
                    {
                        ilist.Add("@Site_Code"); item.Add(ddlSiteId.SelectedItem.Value.ToString());
                    }
                    ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                   
                    //string FromDate = txtFromDate.Text;                  
                    //string ToDate = txtToDate.Text;
                 
                    if (dt.Rows.Count > 0)
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                        LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                        gvDetails.Visible = true;
                        Session["OpenSO"] = dt;
                    }
                    else
                    {

                        LblMessage.Text = string.Empty;
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();

                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                        LblMessage.Text = "No Data Exits Between This Date Range ! !";
                        LblMessage.Visible = true;
                        uppanel.Update();

                    }

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }            
        }
    }
}