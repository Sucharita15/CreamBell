using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCollectionReport : System.Web.UI.Page
    {
        string query = string.Empty;
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                fillCustomerGroup();     
            }
        }

        public void fillCustomerGroup()
        {
            try
            {
                query = "Select CUSTGROUP_CODE+'-'+CUSTGROUP_NAME as Name,CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0";
                drpCustomerGroupNew.Items.Clear();
                drpCustomerGroupNew.Items.Add("--Select--");
                obj.BindToDropDownp(drpCustomerGroupNew, query, "Name", "CUSTGROUP_CODE");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (gvDetails.Rows.Count > 0)
            {
                //ExportToExcel();
                ExportToExcel();
            }
            else
            {
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
                LblMessage.Text = "Cannot Export Data due to No Records available.  !";
                LblMessage.Visible = true;
                uppanel.Update();
            }
        }

        private void ExportToExcel()
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                string FileName = "CollectionReport" + DateTime.Now + ".xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                gvDetails.GridLines = GridLines.Both;
                gvDetails.HeaderStyle.Font.Bold = true;
                gvDetails.RenderControl(htmltextwrtter);
                if (drpCustomerGroupNew.SelectedItem.Text != "--Select--")
                {
                    Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td> <td></td> <td><b>Customer Group: " + drpCustomerGroupNew.SelectedItem.Text + "</b></td></tr></table>");
                }
                else
                {
                    Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
                }
                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bool b = ValidateSearch();

            if (b == true)
            {
                try
                {
                    ShowCollectionDetails();
                    uppanel.Update();
                }
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        private void ShowCollectionDetails()
        {
            try
            {
                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;

                string condition = string.Empty;
                if (drpCustomerGroupNew.SelectedItem != null && drpCustomerGroupNew.Text != "--Select--")
                {
                    condition = " and B.Cust_Group='" + drpCustomerGroupNew.SelectedItem.Value + "' ";
                }
                query = "select A.[Document_No],A.[Collection_Date],A.[Customer_Code],B.[CUSTOMER_NAME],B.[ADDRESS1]+','+B.[ADDRESS2]+','+B.[CITY]+','+B.[AREA]" +
                         " +','+B.[DISTRICT]+','+B.[STATE]+','+B.[ZIPCODE] as Address,C.Instrument_Description,A.Instrument_No,A.Ref_DocumentNo,A.Collection_Amount " +
                         " FROM [ax].[ACXCOLLECTIONENTRY] A" +
                         //or uat//" Inner Join [ax].[ACXCUSTMASTER] B on B.Customer_Code=A.[Customer_Code] and A.Dataareaid=B.Dataareaid "+
                         //" Inner Join [ax].[ACXCUSTMASTER] B on B.Customer_Code=A.[Customer_Code] and A.SiteCode=B.Site_Code and A.Dataareaid=B.Dataareaid " +
                         " Inner Join VW_CUSTOMERMASTER_HISTORY B on B.Customer_Code=A.[Customer_Code] and A.SiteCode=B.Site_Code and A.Dataareaid=B.Dataareaid " +
                         " Inner join [ax].[ACXINSTRUMENTMASTER] C on C.Instrument_Code=A.Instrument_Code and A.Dataareaid=C.Dataareaid " +
                         " Where  A.SiteCode IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")  and A.Dataareaid='" + Session["DATAAREAID"].ToString() + "' and A.[Collection_Date] between '" + FromDate + "' and '" + ToDate + "' " + condition +
                         " order by A.[Customer_Code],A.[Document_No] ";
                //query = "select A.[Document_No],A.[Collection_Date],A.[Customer_Code],B.[CUSTOMER_NAME],B.[ADDRESS1]+','+B.[ADDRESS2]+','+B.[CITY]+','+B.[AREA]" +
                //         " +','+B.[DISTRICT]+','+B.[STATE]+','+B.[ZIPCODE] as Address,C.Instrument_Description,A.Instrument_No,A.Ref_DocumentNo,A.Collection_Amount "+
                //         " FROM [ax].[ACXCOLLECTIONENTRY] A"+
                //         " Inner Join [ax].[ACXCUSTMASTER] B on B.Customer_Code=A.[Customer_Code] and A.SiteCode=B.Site_Code and A.Dataareaid=B.Dataareaid "+
                //         " Inner join [ax].[ACXINSTRUMENTMASTER] C on C.Instrument_Code=A.Instrument_Code and A.Dataareaid=C.Dataareaid "+
                //         " Where  A.SiteCode='" + Session["SiteCode"].ToString() + "' and A.Dataareaid='" + Session["DATAAREAID"].ToString() + "' and A.[Collection_Date] between '" + FromDate + "' and '" + ToDate + "' " + condition +
                //         " order by A.[Customer_Code],A.[Document_No] ";

                DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                    Session["SaleRegister"] = dt;
                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    LblMessage.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateSearch()
        {
            bool value = false;
            try
            {
                if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
                {
                    value = false;
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
                    LblMessage.Text = "Please Provide The Date Range Parameter  !";
                    LblMessage.Visible = true;
                    uppanel.Update();
                }
                if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
                {
                    value = false;
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
                    LblMessage.Text = "Please Provide The TO Date Range Parameter  !";
                    LblMessage.Visible = true;
                    uppanel.Update();
                }
                if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
                {
                    value = false;
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
                    LblMessage.Text = "Please Provide The FROM Date Range Parameter  !";
                    LblMessage.Visible = true;
                    uppanel.Update();
                }
                if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                {
                    value = true;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return value;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}