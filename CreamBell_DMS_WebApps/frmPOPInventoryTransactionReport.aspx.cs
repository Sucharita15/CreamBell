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
    public partial class frmPOPInventoryTransactionReport : System.Web.UI.Page
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
            DataTable dt = new DataTable();

            dt = new DataTable();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                ddlState.Items.Add("Select...");
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


        protected void drpState_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
            ddlSiteId.Items.Clear();
            ddlSiteId.Items.Add("Select...");
            baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
         }

        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dt = new DataTable();
            bool b = Validate();
            if (b == true)
            {
                try
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;

                    string query = "[dbo].[ACX_POP_InventoryTransaction]";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    ilist.Add("@FromDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@ToDate"); item.Add(txtToDate.Text);
                    ilist.Add("@STATECODE");
                    if (ddlState.SelectedIndex != -1)
                    {
                        if (ddlState.SelectedItem.Text != "Select...")
                        {
                            item.Add(ddlState.SelectedItem.Value);
                        }
                        else
                        {
                            item.Add("0");
                        }
                    }
                    else
                    {
                        item.Add("0");
                    }

                    
                    ilist.Add("@SiteId");
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text != "Select...")
                        {
                            item.Add(ddlSiteId.SelectedItem.Value);
                        }
                        else
                        {
                            item.Add("0");
                        }
                    }
                    else
                    {
                        item.Add("0");
                    }

                   // ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                   
                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                    if (dt.Rows.Count > 0)
                    {
                        LoadDataInReportViewerDetail(dt);
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
                    DataSetParameter.Columns.Add("STATECODE");
                    DataSetParameter.Columns.Add("SiteId");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    if (ddlState.SelectedItem.Text == "Select...")
                    {
                        DataSetParameter.Rows[0]["STATECODE"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["STATECODE"] = ddlState.SelectedItem.Text;
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "Select...")
                        {
                            DataSetParameter.Rows[0]["SiteId"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["SiteId"] = ddlSiteId.SelectedItem.Text;
                        }
                    }

                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\POPInventoryTransactionReport.rdl");
                    ReportViewer1.AsyncRendering = true;
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

    }
}