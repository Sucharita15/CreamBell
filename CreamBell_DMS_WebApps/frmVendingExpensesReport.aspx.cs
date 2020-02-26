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
    public partial class frmVendingExpensesReport : System.Web.UI.Page
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
                string sqlstr11;
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                   
                    sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    
                }
                else
                { sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' "; }
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                if (ddlState.Items.Count == 2)
                {
                    ddlState.SelectedIndex = 1;
                    ddlState_SelectedIndexChanged(sender, e);
                }
                GetVRSName();
            }
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = Validate();
            if (b)
            {
                ShowReportByFilter();
                uppanel.Update();
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSiteId.Items.Clear();
            drpVRS.Items.Clear();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";

                ddlSiteId.Items.Add("Select...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                ddlSiteId.Items.Add("Select...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");

            }
            if (ddlSiteId.Items.Count == 2)
                ddlSiteId.SelectedIndex = 1;
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  string SiteList = ddlSiteId.SelectedValue.ToString();
            //string SiteList = "";
            //foreach (ListItem item in ddlSiteId.Items)
            //{
            //    if (item.Selected)
            //    {
            //        if (SiteList == "")
            //        {
            //            SiteList += "'" + item.Value.ToString() + "'";
            //        }
            //        else
            //        {
            //            SiteList += ",'" + item.Value.ToString() + "'";
            //        }
            //    }
            //}

            //if (SiteList.Length > 0)
            //{
            //    DataTable dt = new DataTable();
            //    string sqlstr1 = string.Empty;
            //    string sqlstr = @"SELECT DISTINCT  ax.ACXCUSTMASTER.CUSTOMER_CODE , ax.ACXCUSTMASTER.CUSTOMER_NAME FROM ax.INVENTSITE INNER JOIN
            //             ax.ACXCUSTMASTER ON ax.INVENTSITE.SITEID = ax.ACXCUSTMASTER.SITE_CODE INNER JOIN
            //             ax.ACXCUSTGROUPMASTER ON ax.ACXCUSTMASTER.CUST_GROUP = ax.ACXCUSTGROUPMASTER.CUSTGROUP_CODE where
            //             ax.INVENTSITE.STATECODE <>'' and [ax].[ACXCUSTMASTER].CUst_Group='CG0002' and ax.INVENTSITE.SITEID in (" + SiteList + ") ";
            //    dt = baseObj.GetData(sqlstr);
            //    drpVRS.Items.Clear();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        drpVRS.DataSource = dt;
            //        drpVRS.DataTextField = "CUSTOMER_NAME";
            //        drpVRS.DataValueField = "CUSTOMER_CODE";
            //        drpVRS.DataBind();
            //    }
            //}
            //else
            //{
            //    drpVRS.Items.Clear();
            //}
            GetVRSName();
        }

        protected void GetVRSName()
        {
            string SiteList = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                SiteList ="'" +Convert.ToString(Session["SiteCode"])+"'" ;
            }
            else
            {
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
                drpVRS.Items.Clear();
                drpVRS.Items.Add("Select...");
                baseObj.BindToDropDown(drpVRS, sqlstr, "CUSTOMER_NAME", "CUSTOMER_CODE");
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    drpVRS.DataSource = dt;
                //    drpVRS.DataTextField = "CUSTOMER_NAME";
                //    drpVRS.DataValueField = "CUSTOMER_CODE";
                //    drpVRS.DataBind();
                //}
            }
            else
            {
                drpVRS.Items.Clear();
            }
        }
        private bool Validate()
        {
            bool b = true;
            
            LblMessage.Text = string.Empty;
            if (txtMonth.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide Month";
                LblMessage.Visible = true;
            }

            if (ddlSiteId.SelectedItem.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please select Distributor";
                LblMessage.Visible = true;
            }

            return b;
        }

        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlCommand cmd1 = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
                //conn = new SqlConnection(objGlobal.GetConnectionString());
                //conn.Open();
                //cmd = new SqlCommand();
                //cmd.Connection = conn;
                //cmd.CommandTimeout = 100;
                //cmd.CommandType = CommandType.StoredProcedure;
                //query = "[ACX_DAILYSALESSTATEMENT_VD]";
                //cmd.CommandText = query;
                DateTime stDate = Convert.ToDateTime(txtMonth.Text);
                DateTime firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                //cmd.Parameters.AddWithValue("@MONTHNO", stDate.Month);
                //cmd.Parameters.AddWithValue("@YEARNO", stDate.Year);
                ////cmd.Parameters.AddWithValue("@EndDate", lastDayOfMonth);                
                //cmd.Parameters.AddWithValue("@SITECODE", ddlSiteId.SelectedItem.Value);
                //if (drpVRS.SelectedIndex>1)
                //{
                //    cmd.Parameters.AddWithValue("@VRSCODE", drpVRS.SelectedValue.ToString());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@VRSCODE", "");
                //}
                
                dtDataByfilter = new DataTable();
                dtDataByfilter = baseObj.GetData("EXEC ACX_DAILYSALESSTATEMENT_VD '" + drpVRS.SelectedValue.ToString() + "','" + ddlSiteId.SelectedItem.Value + "'," + stDate.Month + ","+ stDate.Year);
               // cmd.ExecuteNonQuery();
                //dtDataByfilter.Load(cmd.ExecuteReader());
                //==============Claim Data===================
               
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
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("SITECODE");                  
                    DataSetParameter.Columns.Add("MONTHNO");
                    DataSetParameter.Columns.Add("VRSCODE");
                    DataSetParameter.Rows.Add();
                   
                    DataSetParameter.Rows[0]["SITECODE"] = ddlSiteId.SelectedItem.Text;
                    DataSetParameter.Rows[0]["MONTHNO"] = txtMonth.Text;                   
                    DataSetParameter.Rows[0]["VRSCODE"] = drpVRS.SelectedItem.Text;                    

                    //DataSetParameter.Rows[0]["VRSName"] = ddlVRS.SelectedItem.Text.ToString() +" "+ ddlVRS.SelectedValue.ToString();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\VendingExpenseReportMonthly.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "Month";
                    FromDate.Values.Add(txtMonth.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
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
    }
}