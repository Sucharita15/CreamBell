using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSaleRegisterDateWiseReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                //string sqlstr11 = "Select Distinct StateCode Code,StateCode Name from [ax].[INVENTSITE] where STATECODE <>'' ";
                //ddlState.Items.Add("Select...");
                //baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
				DDLBusinessUnit.Items.Clear();
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                DDLBusinessUnit.Items.Add("All...");
                baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");

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
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+' -'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                ddlState.Items.Add("All...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId +' -'+ I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
            }
        }
        private bool ValidateInput()
        {
            bool b;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide From Date and To Date";
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }
          
            return b;
        }

        private void ShowReportSummary()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            DataTable dtSetData = null;

            try
            {
                //string query = "Select Top 1 NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
//                object obj1 = obj.GetScalarValue(query);

//                string sqlstr = @"Select  IH.INVOIC_DATE,Case when IH.TranType = 1 then 'SV' when IH.TranType = 2 then 'PS'  end as TransType,
//                                   IH.INVOICE_NO,C.Customer_Name,IH.Customer_Code, CASE IH.TRANTYPE WHEN 1 THEN IL.BOX WHEN 2 THEN -IL.BOX END BOX,
//                                    CASE IH.TRANTYPE WHEN 1 THEN IL.LTR WHEN 2 THEN -IL.LTR END LTR,CASE IH.TRANTYPE WHEN 1 THEN IL.LINEAMOUNT WHEN 2 THEN -IL.LINEAMOUNT END LINEAMOUNT,
//                                    CASE IH.TRANTYPE WHEN 1 THEN IL.DISC_AMOUNT WHEN 2 THEN -IL.DISC_AMOUNT END DISC_AMOUNT,
//                                    CASE IH.TRANTYPE WHEN 1 THEN IL.TAX_AMOUNT WHEN 2 THEN -IL.TAX_AMOUNT END TAX_AMOUNT,
//                                    CASE IH.TRANTYPE WHEN 1 THEN IL.ADDTAX_AMOUNT WHEN 2 THEN -IL.ADDTAX_AMOUNT END ADDTAX_AMOUNT,
//                                    '' as Surcharge ,  CASE IH.TRANTYPE WHEN 1 THEN IL.AMOUNT WHEN 2 THEN -IL.AMOUNT END AMOUNT , '' as FOCAMOUNT , '' as SchemeAmount,* 
//                                   from [ax].[ACXSALEINVOICEHEADER] IH
//                                   Left Join [ax].[ACXSALEINVOICELINE] IL on IH.SITEID = IL.SITEID and IL.INVOICE_NO = IH.INVOICE_NO
//                                   left join [ax].[ACXCUSTMASTER] C on IH.Customer_Code = C.CUSTOMER_CODE
//                                   and IL.DATAAREAID = IH.DATAAREAID
//                                   where  IH.INVOIC_DATE  >= " +
//                                   " '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and IH.INVOIC_DATE <= '" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'  ";
                


//                if (ddlSiteId.SelectedIndex != -1)
//                {
//                    if (ddlSiteId.SelectedItem.Text != "Select...")
//                    {
//                        sqlstr += "and IH.SITEID = '" + ddlSiteId.SelectedItem.Value + "'  ";
//                    }
//                }
//                sqlstr += " Order by IH.INVOIC_DATE ";


             

                dtSetData = new DataTable();

                string query = "ACX_SALEREGISTER_DATEWISE";
                List<string> ilist = new List<string>();
                List<string> item = new List<string>();
                string StateCode,SiteCode;
                SiteCode = "";

                if (ddlSiteId.SelectedIndex >=0)
                {
                    if (ddlSiteId.SelectedItem.Text != "All...")
                    {
                        SiteCode = ddlSiteId.SelectedItem.Value;
                    }                    
                }
                ilist.Add("@SITEID"); item.Add(SiteCode);
                
                StateCode = "";
                if (ddlState.SelectedIndex>0)
                {
                    StateCode = ddlState.SelectedItem.Value;
                }
                ilist.Add("@STATE"); item.Add(StateCode);
                ilist.Add("@FROMDATE"); item.Add(txtFromDate.Text);
                ilist.Add("@TODATE"); item.Add(txtToDate.Text);
                ilist.Add("@BUCODE");
                if (DDLBusinessUnit.SelectedIndex >= 1)
                {
                    item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                }
                else
                {
                    item.Add("");
                }
                dtSetData = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                //dtSetData = obj.GetData(sqlstr);
                LoadDataInReportViewerDetail(dtSetData);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadDataInReportViewerDetail(DataTable dtSetData)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {

                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FromDate");
                    DataSetParameter.Columns.Add("ToDate");
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    if (ddlState.SelectedItem.Text == "Select...")
                    {
                        DataSetParameter.Rows[0]["StateCode"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "Select...")
                        {
                            DataSetParameter.Rows[0]["Distributor"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Text;
                        }
                    }
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleRegisterDateWiseReport.rdl");
                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;
                }
                else
                {
                    LblMessage.Text = "No Records Exists !!";
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";

            //ddlSiteId.Items.Add("Select...");
            //baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");


            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,SiteId +' -'+ NAME Name from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
                ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,SiteId +' -'+ NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                //ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            DataTable dt = new DataTable();
            bool b = ValidateInput();
            if (b == true)
            {
                try
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;

                  //  string query = "ACX_USP_SaleRegisterReport";
                  //  List<string> ilist = new List<string>();
                  //  List<string> item = new List<string>();
                    //ilist.Add("@Site_Code");
                    //if (ddlSiteId.SelectedIndex != -1)
                    //{
                    //    if (ddlSiteId.SelectedItem.Text != "Select...")
                    //    {
                    //        item.Add(ddlSiteId.SelectedItem.Value);
                    //    }
                    //    else
                    //    {
                    //        item.Add("0");
                    //    }
                    //}
                    //else
                    //{
                    //    item.Add("0");
                    //}

                    //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    //ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                    //ilist.Add("@EndDate"); item.Add(txtToDate.Text);

                    //dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                    //if (dt.Rows.Count > 0)
                    //{
                       // LoadDataInReportViewerDetail(dt);
                    ShowReportSummary();
                  //  }
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