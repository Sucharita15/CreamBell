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
    public partial class frmVersionReleaseRegister : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataTable dt = new DataTable();
        CreamBell_DMS_WebApps.App_Code.Global baseobj = new Global();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                FillDdlList();
            }
        }
        private void FillDdlList()
        {
            string query = string.Empty;
            DataTable dt = new DataTable();

            //ddlVersionCode.Items.Clear();
            query = @"SELECT 'Un-Assigned' AS VersionName UNION SELECT VersionName FROM ACX_VW_GetVersionInfo Order By VersionName";
            dt = baseobj.GetData(query);
            chkListVersion.Items.Clear();
            chkListVersion.Items.Add("All...");
            chkListVersion.DataSource = dt;
            chkListVersion.DataTextField = "VersionName";
            chkListVersion.DataValueField = "VersionName";
            chkListVersion.DataBind();

            query = @"SELECT STATEID,NAME FROM AX.LOGISTICSADDRESSSTATE ORDER BY NAME";
            //baseobj.BindToDropDown(ddlState, query, "Name", "STATEID");
            dt = baseobj.GetData(query);
            chkListState.Items.Clear();
            chkListState.Items.Add("All...");
            chkListState.DataSource = dt;
            chkListState.DataTextField = "NAME";
            chkListState.DataValueField = "STATEID";
            chkListState.DataBind();
            
            query = @"SELECT 'VRS' AS NAME UNION SELECT 'PSR' AS NAME ORDER BY NAME";
            dt = baseobj.GetData(query);
            chkListUserType.Items.Clear();
            chkListUserType.Items.Add("All...");
            chkListUserType.DataSource = dt;
            chkListUserType.DataTextField = "NAME";
            chkListUserType.DataValueField = "NAME";
            chkListUserType.DataBind();

            query = @"SELECT 'YES' AS NAME UNION SELECT 'NO' AS NAME ORDER BY NAME";
            dt = baseobj.GetData(query);
            chkListBlock.Items.Clear();
            chkListBlock.Items.Add("All...");
            chkListBlock.DataSource = dt;
            chkListBlock.DataTextField = "NAME";
            chkListBlock.DataValueField = "NAME";
            chkListBlock.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string sqlCond = string.Empty;
            string strValues = string.Empty;
            SqlDataAdapter dap = null;
            DataTable dt = null;
            string strStateFilter = string.Empty;
            string strDistributorFilter = string.Empty;
            string strUserTypeFilter = string.Empty;
            string strUserCodeFilter = string.Empty;
            string strVersionFilter = string.Empty;
            string strBlockFilter = string.Empty;

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            sqlCond = "EXEC ACX_USP_GETVERSIONRELEASE ";
            strValues = "";

            foreach (ListItem item in chkListState.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            strStateFilter = strValues.Replace("'","");
            sqlCond = sqlCond + strValues;

            if (txtDistributor.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtDistributor.Text + "',";
                strDistributorFilter = txtDistributor.Text;
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (ListItem item in chkListUserType.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues;
            strUserTypeFilter = strValues.Replace("'", "");

            if (txtUserCode.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtUserCode.Text + "',";
                strUserCodeFilter = txtUserCode.Text;
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (ListItem item in chkListVersion.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues.Replace("Un-Assigned", "");
            strVersionFilter = strValues.Replace("'", "");

            strValues = "";
            foreach (ListItem item in chkListBlock.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "'"; }
            else { sqlCond = sqlCond + "'All'"; }
            sqlCond = sqlCond + strValues;
            strBlockFilter = strValues.Replace("'", "");

            dap = new SqlDataAdapter(sqlCond, conn);
            dt = new DataTable("dtTemp");
            dap.Fill(dt);
            LoadDataInReportViewer(dt,strStateFilter,strDistributorFilter,strUserTypeFilter,strUserCodeFilter,strVersionFilter,strBlockFilter);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        private void LoadDataInReportViewer(DataTable dtSetData,string stateFilterStr,string distributorFilterStr,string userTypeFilterStr,string userCodeFilterStr,string versionFilterStr,string blockStatusFilterStr)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");
                    DataSetParameter.Columns.Add("Version");
                    DataSetParameter.Columns.Add("Block");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["StateCode"] = (stateFilterStr.Trim().Length == 0 ? "All" : stateFilterStr);
                    DataSetParameter.Rows[0]["Distributor"] = (distributorFilterStr.Trim().Length == 0 ? "All" : distributorFilterStr);
                    DataSetParameter.Rows[0]["UserType"] = (userTypeFilterStr.Trim().Length == 0 ? "All" : userTypeFilterStr); 
                    DataSetParameter.Rows[0]["UserCode"] = (userCodeFilterStr.Trim().Length == 0 ? "All" : userCodeFilterStr); 
                    DataSetParameter.Rows[0]["Version"] = (versionFilterStr.Trim().Length == 0 ? "All" : versionFilterStr); 
                    DataSetParameter.Rows[0]["Block"] = (blockStatusFilterStr.Trim().Length == 0 ? "All" : blockStatusFilterStr);
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\CurrentVersionReleaseReport.rdl");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DateSetParameter", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
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

        private void LoadDataInReportViewerHistory(DataTable dtSetData, string stateFilterStr, string distributorFilterStr, string userTypeFilterStr, string userCodeFilterStr, string versionFilterStr)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");
                    DataSetParameter.Columns.Add("Version");
                    DataSetParameter.Columns.Add("Block");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["StateCode"] = (stateFilterStr.Trim().Length == 0 ? "All" : stateFilterStr);
                    DataSetParameter.Rows[0]["Distributor"] = (distributorFilterStr.Trim().Length == 0 ? "All" : distributorFilterStr);
                    DataSetParameter.Rows[0]["UserType"] = (userTypeFilterStr.Trim().Length == 0 ? "All" : userTypeFilterStr);
                    DataSetParameter.Rows[0]["UserCode"] = (userCodeFilterStr.Trim().Length == 0 ? "All" : userCodeFilterStr) ;
                    DataSetParameter.Rows[0]["Version"] = (versionFilterStr.Trim().Length == 0 ? "All" : versionFilterStr);
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\VersionReleaseHistoryReport.rdl");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DateSetParameter", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
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
        protected void btnVersionHistory_Click(object sender, EventArgs e)
        {
            string sqlCond = string.Empty;
            string strValues = string.Empty;
            SqlDataAdapter dap = null;
            DataTable dt = null;
            string strStateFilter = string.Empty;
            string strDistributorFilter = string.Empty;
            string strUserTypeFilter = string.Empty;
            string strUserCodeFilter = string.Empty;
            string strVersionFilter = string.Empty;

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            sqlCond = "EXEC ACX_USP_GETVERSIONRELEASEHISTORY ";
            strValues = "";

            foreach (ListItem item in chkListState.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            strStateFilter = strValues.Replace("'", "");
            sqlCond = sqlCond + strValues;

            if (txtDistributor.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtDistributor.Text + "',";
                strDistributorFilter = txtDistributor.Text;
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (ListItem item in chkListUserType.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues;
            strUserTypeFilter = strValues.Replace("'", "");

            if (txtUserCode.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtUserCode.Text + "',";
                strUserCodeFilter = txtUserCode.Text;
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (ListItem item in chkListVersion.Items)
            {
                if (item.Selected)
                {
                    strValues += "''" + item.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "'"; }
            else { sqlCond = sqlCond + "'All'"; }
            sqlCond = sqlCond + strValues.Replace("Un-Assigned", "");
            strVersionFilter = strValues.Replace("'", "");

            dap = new SqlDataAdapter(sqlCond, conn);
            dt = new DataTable("dtTemp");
            dap.Fill(dt);
            LoadDataInReportViewerHistory(dt, strStateFilter, strDistributorFilter, strUserTypeFilter, strUserCodeFilter, strVersionFilter);

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStateAll.Checked == true)
            {
                for (int i = 0; i < chkListState.Items.Count; i++)
                {
                    chkListState.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < chkListState.Items.Count; i++)
                {
                    chkListState.Items[i].Selected = false;
                }
            }
        }

        protected void chkUserTypeAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUserTypeAll.Checked == true)
            {
                for (int i = 0; i < chkListUserType.Items.Count; i++)
                {
                    chkListUserType.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < chkListUserType.Items.Count; i++)
                {
                    chkListUserType.Items[i].Selected = false;
                }
            }
        }

        protected void chkVersionAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVersionAll.Checked == true)
            {
                for (int i = 0; i < chkListVersion.Items.Count; i++)
                {
                    chkListVersion.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < chkListVersion.Items.Count; i++)
                {
                    chkListVersion.Items[i].Selected = false;
                }
            }
        }

        protected void chkBlockAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBlockAll.Checked == true)
            {
                for (int i = 0; i < chkListBlock.Items.Count; i++)
                {
                    chkListBlock.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < chkListBlock.Items.Count; i++)
                {
                    chkListBlock.Items[i].Selected = false;
                }
            }
        }
    }
}