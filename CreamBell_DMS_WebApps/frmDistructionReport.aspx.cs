using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Elmah;
using Microsoft.Reporting.WebForms;

namespace CreamBell_DMS_WebApps
{   
    public partial class frmDistructionReport : System.Web.UI.Page
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
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID ='" + ucRoleFilters.GetCommaSepartedSiteId() + "' ";
                DDLBusinessUnitNew.Items.Clear();
                DDLBusinessUnitNew.Items.Add("All...");
                baseObj.BindToDropDownp(DDLBusinessUnitNew, query, "bu_desc", "bu_code");
                fillStateCode();
                fillSiteCode();
            }
        }

        private DataTable fillStateCode()
        {
            DataTable dt = new DataTable();
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string strQuery = "Select distinct I.StateCode as Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dt = obj.GetData(strQuery);
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return dt;
        }

        private DataTable fillSiteCode()
        {
            DataTable dt = new DataTable();
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string strQuery = "Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dt = obj.GetData(strQuery);
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return dt;
        }
        //protected void fillSiteAndState()
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlState.Items.Clear();
        //        string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
        //        ddlState.Items.Add("All...");
        //        baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlState.Items.Clear();
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
        //        baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
        //    }
        //}

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

        //protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
        //        ddlSiteId.Items.Add("All...");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
        //        //ddlSiteId.Items.Add("All...");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //    }
        //}

        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    ShowReportByFilter();
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new CreamBell_DMS_WebApps.App_Code.Global();
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
                query = "[dbo].[ACX_DESTRUCTIONREPORT]";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@FROMDATE", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TODATE", Convert.ToDateTime(txtToDate.Text));

                DataTable dtState = fillStateCode();
                if (dtState.Rows.Count > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", dtState.Rows[0]["Code"]);
                }
                DataTable dtSite = fillSiteCode();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    cmd.Parameters.AddWithValue("@SITEID", ucRoleFilters.GetCommaSepartedSiteId());
                }
                else {
                    
                    if (dtState.Rows.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@SITEID", dtSite.Rows[0]["Code"]);
                    }
                }
                


                //TypeReport
                string ReportType = "";
                string TypeName = "";
                foreach (ListItem item in chkSelectionNew.Items)
                {
                    if (item.Selected)
                    {
                        if (ReportType == "")
                        {
                            ReportType += "" + item.Value.ToString() + "";
                            TypeName += "" + item.Text.ToString() + "";
                        }
                        else
                        {
                            ReportType += "," + item.Value.ToString() + "";
                            TypeName += "," + item.Text.ToString() + "";
                        }
                    }
                }
                //if (chkAllUnit.Checked == true)
                //{
                //    TypeName = "All";
                //}
                if (ReportType.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@ReportType", ReportType);
                }
                else
                {                   
                    TypeName = "All";
                    cmd.Parameters.AddWithValue("@ReportType", "");
                }
                if (DDLBusinessUnitNew.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", DDLBusinessUnitNew.SelectedItem.Value.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }

                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());

                if (dtDataByfilter.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FROMDATE");
                    DataSetParameter.Columns.Add("TODATE");
                    DataSetParameter.Columns.Add("STATECODE");
                    DataSetParameter.Columns.Add("SITEID");
                    DataSetParameter.Columns.Add("TypeName");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FROMDATE"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["TODATE"] = txtToDate.Text;
                    DataSetParameter.Rows[0]["STATECODE"] = dtState.Rows[0]["Name"];
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataSetParameter.Rows[0]["SITEID"] = ucRoleFilters.GetCommaSepartedSiteId();
                    }
                    else {
                        DataSetParameter.Rows[0]["SITEID"] = dtSite.Rows[0]["Name"];
                    }
                    
                    DataSetParameter.Rows[0]["TypeName"] = TypeName;


                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\DistructionReport.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtDataByfilter);
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
                
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
            }
        }

        //protected void chkAllUnit_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkAllUnit.Checked)
        //    {
        //        for (int i = 0; i < chkSelectionNew.Items.Count; i++)
        //        {
        //            chkSelectionNew.Items[i].Selected = true;
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < chkSelectionNew.Items.Count; i++)
        //        {
        //            chkSelectionNew.Items[i].Selected = false;
        //        }
        //    }
        //}

        protected void chkSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chkSelectionNew.Items.Count; i++)
            {
                if (chkSelectionNew.Items[i].Selected == false)
                {
                   // chkAllUnit.Checked = false;
                }
            }
        }
    }
}