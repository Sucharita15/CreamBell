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
    public partial class frmMrpDiscountClaim : System.Web.UI.Page
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
                FillState();
                chkListSite_SelectedIndexChanged(null, null);
            }

            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                phState.Visible = false;
                ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            }
        }

        private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        {
            FillCustomerGroup();
        }

        protected void FillState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            DataTable dt = new DataTable();

            dt = new DataTable();
            if (objcheckSitecode != null)
            {
                chkListState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                dt = baseObj.GetData(sqlstr11);
                chkListState.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "NAME";
                    chkListState.DataValueField = "Code";
                    chkListState.DataBind();
                }
            }
            else
            {
                chkListState.Items.Clear();
                chkListSite.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "StateName";
                    chkListState.DataValueField = "StateCode";
                    chkListState.DataBind();

                    chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }
                if (dt.Rows.Count > 0)
                {
                    chkListState.Items[0].Selected = true;
                    chkListSite.Items[0].Selected = true;
                }
            }
        }


        protected void FillSite()
        {
            string StateList = "";
            foreach (ListItem item in chkListState.Items)
            {
                if (item.Selected)
                {
                    if (StateList == "")
                    {
                        StateList += "'" + item.Value.ToString() + "'";
                    }
                    else
                    {
                        StateList += ",'" + item.Value.ToString() + "'";
                    }
                }
            }
            if (StateList.Length > 0)
            {
                DataTable dt = new DataTable();
                string sqlstr1 = string.Empty;
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName,NAME from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                }
                else
                {
                    sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                }
                dt = new DataTable();
                // dt = baseObj.GetData(sqlstr1);
                chkListSite.Items.Clear();
                dt = baseObj.GetData(sqlstr1);
                chkListSite.DataSource = dt;
                chkListSite.DataTextField = "SiteName";
                chkListSite.DataValueField = "SiteId";
                chkListSite.DataBind();
            }
            else
            {
                chkListSite.Items.Clear();
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
        protected void FillBusinessGroups()
        {
            string buunit = "";
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                buunit = ucRoleFilters.GetCommaSepartedSiteId();
            }
            else {
                foreach (ListItem items in chkListSite.Items)
                {
                    if (items.Selected)
                    {
                        if (buunit == "")
                        {
                            buunit += "" + items.Value.ToString() + "";
                        }
                        else
                        {
                            buunit += "," + items.Value.ToString() + "";
                        }
                    }
                }
            }
               
            DataTable dt = new DataTable();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID in ('" + buunit + "')";
            dt = new DataTable();
            dt = baseObj.GetData(query);
            Chkbu.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Chkbu.DataSource = dt;
                Chkbu.DataTextField = "bu_desc";
                Chkbu.DataValueField = "bu_code";
                Chkbu.DataBind();
            }
        }
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListCustomerGroup.Items.Clear();
            Chkbu.Items.Clear();
            FillSite();
        }

        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillCustomerGroup();
            //if (chkListSite.SelectedItem.Value.ToString() == "")
            //{
            //    Chkbu.Items.Clear();
            //}
            //else
            //{
            //    FillBusinessGroups();
            //}
            for (int i = 0; i < chkListSite.Items.Count; i++)
            {
                if (chkListSite.Items[i].Selected == true)
                {
                    FillBusinessGroups();
                    FillCustomerGroup();
                    break;

                }


                else
                {
                    Chkbu.Items.Clear();
                    chkListCustomerGroup.Items.Clear();
                }
                //else {
                //    FillBusinessGroup();
                //}
            }
        }

        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            bool b = ValidateInput();
            if (b)
            {
                ShowReportByFilter();
            }

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
                //conn.Open();
                cmd = new SqlCommand();
                //cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "[ax].[ACX_MRPDiscountClaimReport]";

                cmd.CommandText = query;
                //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyy"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyy"));
               string ListState = "";
                string DistributorList = "";
                if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                    ListState = ucRoleFilters.GetCommaSepartedStateId();
                    DistributorList = ucRoleFilters.GetCommaSepartedSiteId();

                } else {
                    foreach (ListItem item in chkListState.Items)
                    {
                        if (item.Selected)
                        {
                            if (ListState == "")
                            {
                                ListState += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                ListState += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    
                    foreach (ListItem item in chkListSite.Items)
                    {
                        if (item.Selected)
                        {
                            if (DistributorList == "")
                            {
                                DistributorList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                DistributorList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                }
                    
                if (ListState.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", ListState);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }

                
                if (DistributorList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", DistributorList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }
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
                    cmd.Parameters.AddWithValue("@CUSTOMER_CODE", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTOMER_CODE", "");
                }

                string buunits = "";
                foreach (ListItem items in Chkbu.Items)
                {
                    if (items.Selected)
                    {
                        if (buunits == "")
                        {
                            buunits += "" + items.Value.ToString() + "";
                        }
                        else
                        {
                            buunits += "," + items.Value.ToString() + "";
                        }
                    }
                }
                if (buunits.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", buunits);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }

                dtDataByfilter = new DataTable();
                cmd.Connection = objGlobal.GetConnection();  
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
                //if (conn != null)
                //{
                //    if (conn.State.ToString() == "Open") { conn.Close(); }
                //}
                objGlobal.CloseSqlConnection();
            }
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
                    DataSetParameter.Columns.Add("STATECODE");
                    DataSetParameter.Columns.Add("SiteId");
                    DataSetParameter.Columns.Add("CUSTOMER_CODE");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    string ListState = "";
                    string DistributorList = "";
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                        ListState = ucRoleFilters.GetCommaSepartedStateId();
                        DistributorList = ucRoleFilters.GetCommaSepartedSiteId();
                    } else {
                        foreach (ListItem item in chkListState.Items)
                        {
                            if (item.Selected)
                            {
                                if (ListState == "")
                                {
                                    ListState += "" + item.Text.ToString() + "";
                                }
                                else
                                {
                                    ListState += "," + item.Text.ToString() + "";
                                }
                            }
                        }

                        
                        foreach (ListItem item in chkListSite.Items)
                        {
                            if (item.Selected)
                            {
                                if (DistributorList == "")
                                {
                                    DistributorList += "" + item.Text.ToString() + "";
                                }
                                else
                                {
                                    DistributorList += "," + item.Text.ToString() + "";
                                }
                            }

                        }
                    }
                       
                    DataSetParameter.Rows[0]["STATECODE"] = ListState;
                   
                    if (DistributorList == string.Empty)
                    {
                        DistributorList = "ALL";
                    }
                    DataSetParameter.Rows[0]["SiteId"] = DistributorList;

                    string CustomerGroupList = "";
                    foreach (ListItem item in chkListCustomerGroup.Items)
                    {
                        if (item.Selected)
                        {
                            if (CustomerGroupList == "")
                            {
                                CustomerGroupList += "" + item.Text.ToString() + "";
                            }
                            else
                            {
                                CustomerGroupList += "," + item.Text.ToString() + "";
                            }
                        }

                    }
                    if (CustomerGroupList == string.Empty)
                    {
                        CustomerGroupList = "ALL";
                    }
                    DataSetParameter.Rows[0]["CUSTOMER_CODE"] = CustomerGroupList;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\MrpDiscountClaim.rdl");
                    ReportParameter FromDate = new ReportParameter();
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
    
