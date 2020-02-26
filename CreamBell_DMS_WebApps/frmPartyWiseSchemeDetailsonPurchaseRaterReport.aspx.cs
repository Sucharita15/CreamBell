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
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPartyWiseSchemeDetailsonPurchaseRaterReport : System.Web.UI.Page
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
                //string sqlstr11 = "Select Distinct StateCode Code,StateCode Name from [ax].[INVENTSITE] where STATECODE <>'' ";
                //ddlState.Items.Add("Select...");
                //baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                fillSiteAndState();
                FillCustomerGroup();
               // fillSchemeode();
            }
        }
        protected void fillSiteAndState()
        {
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
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
        protected void fillSchemeode()
        {
                DataTable dt = new DataTable();
                chkScheme.Items.Clear();
                string sqlstr = string.Empty;
                sqlstr = "Select Distinct Schemecode,[Scheme Description] AS SchemeName From ACXAllSCHEMEVIEW Where ((StartingDate between '" + txtFromDate.Text.ToString() + "' And '" + txtToDate.Text.ToString() + "') or (ENDINGDATE between '" + txtFromDate.Text.ToString() + "' and '" + txtToDate.Text.ToString() + "')) ";
                //((StartingDate<='01-Feb-2017' and ENDINGDATE>='01-Feb-2017') Or (StartingDate<='28-Feb-2017' and ENDINGDATE>='28-Feb-2017'))
                if (ddlState.SelectedIndex > 0 && ddlSiteId.SelectedIndex<0)
                {
                    sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type] IN ('State','SITE') and ([SalesCode] in ('" + ddlState.SelectedValue.ToString() + "') OR [SalesCode] in (SELECT SITEID FROM AX.INVENTSITE WHERE STATECODE='" + ddlState.SelectedValue.ToString() + "'))))";
                }
                if (ddlSiteId.SelectedIndex > 0)
                {
                    sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type]='State' and [SalesCode]='" + ddlState.SelectedValue.ToString() + "') OR ([Sales Type]='SITE' AND [SalesCode]='" + ddlSiteId.SelectedValue.ToString() + "'))";
                }
                else if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type]='State' and [SalesCode]='" + ddlState.SelectedValue.ToString() + "') OR ([Sales Type]='SITE' AND [SalesCode]='" + ddlSiteId.SelectedValue.ToString() + "'))";
                }
                dt = new DataTable();
                dt = baseObj.GetData(sqlstr);
                chkScheme.Items.Clear();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                    chkScheme.DataSource = dt;
                    chkScheme.DataTextField ="SchemeName";
                    chkScheme.DataValueField = "Schemecode";
                    chkScheme.DataBind();
                    Scheme.Update();
             //   }       
        }
        private bool Validate()
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
        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = Validate();
            if (b)
            {
                //ShowReport();
                ShowReportByFilter();
            }
        }
        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                string qr = "";
                query = "ax.ACX_PartyWiseSchemeDetailsonPurchaseRaterReport";

                cmd.CommandText = query;
                DateTime now = Convert.ToDateTime(txtToDate.Text);
                now = now.AddMonths(1);
                //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));
                //if (ddlSiteId.SelectedIndex >= 1)
                //{
                //    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SiteId", "0");
                //}

                // site
                if (ddlSiteId.SelectedIndex < 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", "0");
                }
                else if (ddlSiteId.SelectedIndex > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else if (ddlSiteId.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "0");
                }
                // CustGroup
                string CustGroupList = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroupList == "")
                        {
                            CustGroupList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            CustGroupList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (CustGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", "");
                }

                // Scheme Code
                string SchemeList = "";
                foreach (ListItem item in chkScheme.Items)
                {
                    if (item.Selected)
                    {
                        if (SchemeList == "")
                        {
                            SchemeList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            SchemeList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (SchemeList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Scheme", SchemeList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Scheme", "");
                }

                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
                LoadDataInReportViewer(dtDataByfilter);              
            }
            catch (Exception ex)
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
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
                //ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            fillSchemeode();
        }
        private void LoadDataInReportViewer(DataTable dtSetData)
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

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PartyWiseSaleSummaryonDiscountPurchase.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet1", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    this.ReportViewer1.LocalReport.Refresh();

                    ReportViewer1.Visible = true;
                    //ReportViewer1.ZoomPercent = 100;
                    LblMessage.Text = String.Empty;
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
        protected void FillCustomerGroup()
        {
            DataTable dt = new DataTable();
            string sqlstr = "select Distinct CustGroup_Name as Name,Custgroup_Code as Code from ax.ACXCUSTGROUPMASTER ";
            dt = new DataTable();
            dt = baseObj.GetData(sqlstr);
            chkListCustomerGroup.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chkListCustomerGroup.DataSource = dt;
                chkListCustomerGroup.DataTextField = "NAME";
                chkListCustomerGroup.DataValueField = "Code";
                chkListCustomerGroup.DataBind();
            }
        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            if (txtFromDate.Text!=string.Empty && txtToDate.Text!=string.Empty )
            {
                fillSchemeode();
            }            
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                fillSchemeode();
            }   
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillSchemeode();
        }
    }
}