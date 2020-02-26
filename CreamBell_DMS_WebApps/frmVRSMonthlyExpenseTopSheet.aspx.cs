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
    public partial class frmVRSMonthlyExpenseTopSheet : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();

        
        protected override void OnLoad(EventArgs e)
        {
            ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            ucRoleFilters.ListStateChanged += UcRoleFilters_ListStateChange;
            base.OnLoad(e);
            
        }
        private void UcRoleFilters_ListStateChange(object sender, EventArgs e)
        {
            SiteChange();
        }

        private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        {
            SiteChange();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                string sqlstr11; 
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                    sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                else
                    sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                if (ddlState.Items.Count == 2)
                {
                    ddlState.SelectedIndex = 1;
                    ddlState_SelectedIndexChanged(sender, e);
                    
                }
            }
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                phState.Visible = false;
                ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            }
        }
        private bool ValidateInput()
        {
            bool b;
            if (txtFromDate.Text == string.Empty )
            {
                b = false;
                LblMessage.Text = "Please Provide Month";
            }
            else if (ddlBusinessUnit.SelectedItem.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Select Business Unit";
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }
            return b;
        }

        private void StateChange() {
            ddlVRS.Items.Clear();
            ddlBusinessUnit.Items.Clear();
            if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                SiteChange();
            } else {
                ddlSiteId.Items.Clear();

                if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";

                    ddlSiteId.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");

                }
                else
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    ddlSiteId.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");

                }
                if (ddlSiteId.Items.Count == 2)
                {
                    ddlSiteId.SelectedIndex = 1;
                    SiteChange();
                }

            }
                
        }


        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSiteId.Items.Clear();
            ddlVRS.Items.Clear();
            ddlBusinessUnit.Items.Clear();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
               
                ddlSiteId.Items.Add("Select...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");

            }
            else
            {
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                ddlSiteId.Items.Add("Select...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");

            }
            if (ddlSiteId.Items.Count == 2)
            {
                ddlSiteId.SelectedIndex = 1;
                ddlSiteId_SelectedIndexChanged(sender, e);
            }
        }
        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = ValidateInput();
            if (b)
            {               
                ShowReportByFilter();
                uppanel.Update();
            }
        }
        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new CreamBell_DMS_WebApps.App_Code.Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlCommand cmd1 = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
               // conn = new SqlConnection(objGlobal.GetConnectionString());
              //  conn.Open();
                cmd = new SqlCommand();
               // cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;                
                query = "[ax].[ACX_VRS_MonthlySummerySheet]";
                cmd.CommandText = query;
                DateTime stDate = Convert.ToDateTime(txtFromDate.Text);               
                DateTime firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                cmd.Parameters.AddWithValue("@StartDate", firstDayOfMonth);
                cmd.Parameters.AddWithValue("@EndDate", lastDayOfMonth);
                if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                    cmd.Parameters.AddWithValue("@SiteId", ucRoleFilters.GetCommaSepartedSiteId());
                }
                else
                {
                    if (ddlSiteId.SelectedIndex >= 1)
                    {
                        cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SiteId", "0");
                    }
                }
                cmd.Parameters.AddWithValue("@VRSCode", ddlVRS.SelectedValue.ToString());              
                dtDataByfilter = new DataTable();
                cmd.Connection = objGlobal.GetConnection();
                dtDataByfilter.Load(cmd.ExecuteReader());
                //==============Claim Data===================
                DataTable dtClaim = new DataTable();
                cmd1 = new SqlCommand();
                //cmd1.Connection = conn;                
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;
                query = "[ax].[ACX_VRS_MonthlySummerySheet_CLAIM]";
                cmd1.CommandText = query;               
                cmd1.Parameters.AddWithValue("@StartDate", firstDayOfMonth);
                cmd1.Parameters.AddWithValue("@EndDate", lastDayOfMonth);
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    cmd1.Parameters.AddWithValue("@SiteId", ucRoleFilters.GetCommaSepartedSiteId());
                }
                else
                {
                    if (ddlSiteId.SelectedIndex >= 1)
                    {
                        cmd1.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                    }
                    else
                    {
                        cmd1.Parameters.AddWithValue("@SiteId", "0");
                    }
                }
                if (ddlBusinessUnit.SelectedIndex >= 1)
                {
                    cmd1.Parameters.AddWithValue("@BUCODE", ddlBusinessUnit.SelectedItem.Value.ToString());
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@BUCODE", "");
                }
                cmd1.Parameters.AddWithValue("@VRSCode", ddlVRS.SelectedValue.ToString());
                dtClaim = new DataTable();
                cmd1.Connection = objGlobal.GetConnection();
                dtClaim.Load(cmd1.ExecuteReader());

                LoadDataInReportViewer(dtDataByfilter,dtClaim);
            }
            catch (Exception ex)
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                objGlobal.CloseSqlConnection();
            }
        }
        private void LoadDataInReportViewer(DataTable dtSetData, DataTable dtSetDataClaim)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("SiteName");
                    DataSetParameter.Columns.Add("City");
                    DataSetParameter.Columns.Add("State");
                    DataSetParameter.Columns.Add("Month");
                    DataSetParameter.Columns.Add("VRSName");
                    DataSetParameter.Rows.Add();
                    if (ddlSiteId.SelectedIndex > 0)
                    {
                        DataSetParameter.Rows[0]["SiteName"] = ddlSiteId.SelectedItem.Text;
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["SiteName"] = "";
                    }

                    if (ddlState.SelectedIndex>0)
                    {
                        DataTable dtCity = new DataTable();
                        dtCity = baseObj.GetData("select AcxCity from ax.inventsite where siteid='"+ ddlSiteId.SelectedItem.Value +"'");
                        if (dtCity.Rows.Count>0)
                        {
                            DataSetParameter.Rows[0]["City"] = dtCity.Rows[0]["AcxCity"];
                        }
                        DataSetParameter.Rows[0]["State"] = ddlState.SelectedItem.Text;
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["City"] = "";
                        DataSetParameter.Rows[0]["State"] = "";
                    }
  
                    DataSetParameter.Rows[0]["Month"] = txtFromDate.Text;
                    if (ddlVRS.SelectedIndex > 0)
                    {
                        DataSetParameter.Rows[0]["VRSName"] = ddlVRS.SelectedItem.Text;
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["VRSName"] = "";
                    }
                    //DataSetParameter.Rows[0]["VRSName"] = ddlVRS.SelectedItem.Text.ToString() +" "+ ddlVRS.SelectedValue.ToString();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\VRSMonthlyExpenseMonitoringSummary.rdl");
                    ReportViewer1.AsyncRendering = true;
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);                    
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    ReportDataSource RDS1 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DSetDataClaim", dtSetDataClaim);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportDataSource RDS3 = new ReportDataSource("DataSetHeader", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS3);
                    this.ReportViewer1.LocalReport.Refresh();

                    ReportViewer1.Visible = true;                    
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

        private void SiteChange() {
            string SiteList = "";
            if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                SiteList = ucRoleFilters.GetCommaSepartedSiteId();
            } else {
                foreach (ListItem item in ddlSiteId.Items)
                {
                    if (item.Selected)
                    {
                        if (SiteList == "")
                        {
                            SiteList += "'" + item.Value.ToString() + "'";
                        }
                        else
                        {
                            SiteList += ",'" + item.Value.ToString() + "'";
                        }
                    }
                }
            }
                

            if (SiteList.Length > 0)
            {
                DataTable dt = new DataTable();
                string sqlstr1 = string.Empty;
                string sqlstr = @"SELECT DISTINCT  ax.ACXCUSTMASTER.CUSTOMER_CODE , ax.ACXCUSTMASTER.CUSTOMER_NAME FROM ax.INVENTSITE INNER JOIN
                         ax.ACXCUSTMASTER ON ax.INVENTSITE.SITEID = ax.ACXCUSTMASTER.SITE_CODE INNER JOIN
                         ax.ACXCUSTGROUPMASTER ON ax.ACXCUSTMASTER.CUST_GROUP = ax.ACXCUSTGROUPMASTER.CUSTGROUP_CODE where
                         ax.INVENTSITE.STATECODE <>'' and [ax].[ACXCUSTMASTER].CUst_Group='CG0002' and ax.INVENTSITE.SITEID in (" + SiteList + ") ";
                dt = baseObj.GetData(sqlstr);
                ddlVRS.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlVRS.DataSource = dt;
                    ddlVRS.DataTextField = "CUSTOMER_NAME";
                    ddlVRS.DataValueField = "CUSTOMER_CODE";
                    ddlVRS.DataBind();
                }
            }
            else
            {
                ddlVRS.Items.Clear();
            }
            ddlBusinessUnit.Items.Clear();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID in (" + SiteList + ")";
            ddlBusinessUnit.Items.Add("Select...");
            baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            SiteChange();
            
        }
    }
}