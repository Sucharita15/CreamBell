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
    public partial class frmMobileAppSyncReport : System.Web.UI.Page
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
                fillState();
                fillUserType();
                fillSyncVersion();
            }
        }
        
        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                ShowReportByFilter();
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                query = "dbo.ACX_USP_MobileSyncStatus";

                cmd.CommandText = query;
                //sate
                if (ddlState.SelectedIndex > 0 || ddlState.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@STATE", ddlState.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATE", "");
                }
                //UserTupe


                if (ddlUserType.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@UserType", ddlUserType.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@UserType", "");
                }
                if (ddlUser.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@USERID", ddlUser.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USERID", "");
                }
                if (ddlSyncVersion.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@SYNCVERSION", ddlSyncVersion.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SYNCVERSION", "");
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
        private void LoadDataInReportViewer(DataTable dtSetData)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\MobileAppSyncReport.rdl");
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                 //   ReportDataSource RDS2 = new ReportDataSource("DataSet1", DataSetParameter);
                 //   ReportViewer1.LocalReport.DataSources.Add(RDS2);
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
  
        protected void fillState()
        {

                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct STATE Code,STATE as Name from [ax].[acxConfigInfo] where UserId<>'' and STATE<>'' ";
                ddlState.Items.Add("All...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
        }
        protected void fillUserType()
        {
            ddlUserType.Items.Clear();
            string sqlstr1 = "Select Distinct case when UserType = 0 then 'PSR' when UserType = 1 then 'VRS' end as Code, " +
                            " case when UserType = 0 then 'PSR' when UserType = 1 then 'VRS' end as Name from [ax].[acxConfigInfo] where UserId<>'' and STATE<>''";
            ddlUserType.Items.Add("All...");
            baseObj.BindToDropDown(ddlUserType, sqlstr1, "Name", "Code");
        }
        protected void fillUser()
        {
            if (ddlState.SelectedIndex > 0)
            {
                ddlUser.Items.Clear();
                string sqlstr1 = "Select Distinct UserId as Code,UserName+'/'+UserId as Name from [ax].[acxConfigInfo] where UserId<>'' and STATE='" + ddlState.SelectedValue.ToString() + "'";
                ddlUser.Items.Add("All...");
                baseObj.BindToDropDown(ddlUser, sqlstr1, "Name", "Code");
            }
        }
        protected void fillSyncVersion()
        {
                ddlSyncVersion.Items.Clear();
                string sqlstr1 = "Select Distinct RTRIM(SyncVersion) Code,RTRIM(SyncVersion) as Name from [ax].[acxSyncLog] where psrcode<>''";
                ddlSyncVersion.Items.Add("All...");
                baseObj.BindToDropDown(ddlSyncVersion, sqlstr1, "Name", "Code");

        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillUser();
        }
    }
}