﻿using System;
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
    public partial class frmFOCSaleInvoiceReport : System.Web.UI.Page
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
                FillState();
            }
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
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
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
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
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
                chkListState.Items[0].Selected = true;
                chkListSite.Items[0].Selected = true;
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
                    sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                }
                else
                {
                    sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                }

                dt = new DataTable();
                // dt = baseObj.GetData(sqlstr1);
                chkListSite.Items.Clear();
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }

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
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }

        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerGroup();
        }

        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            bool b = Validate();
            if (b)
            {
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

                query = "ACX_FOCSALEINVOICEREPORT";

                cmd.CommandText = query;
                //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                cmd.Parameters.AddWithValue("@FROMDATE", Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyy"));
                cmd.Parameters.AddWithValue("@TODATE", Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyy"));
               string ListState = "";
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
                if (ListState.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATE", ListState);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATE", "");
                }

                string DistributorList = "";
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
                if (DistributorList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SITEID", DistributorList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SITEID", "");
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
                    cmd.Parameters.AddWithValue("@CUSTOMERGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTOMERGROUP", "");
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
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FROMDATE");
                    DataSetParameter.Columns.Add("TODATE");
                    DataSetParameter.Columns.Add("STATE");
                    DataSetParameter.Columns.Add("SITEID");
                    DataSetParameter.Columns.Add("CUSTOMERGROUP");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FROMDATE"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["TODATE"] = txtToDate.Text;
                    string ListState = "";
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
                    DataSetParameter.Rows[0]["STATE"] = ListState;
                    string DistributorList = "";
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
                    if (DistributorList == string.Empty)
                    {
                        DistributorList = "ALL";
                    }
                    DataSetParameter.Rows[0]["SITEID"] = DistributorList;

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
                    DataSetParameter.Rows[0]["CUSTOMERGROUP"] = CustomerGroupList;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\FOCSaleInvoiceReport.rdl");
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
