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
    public partial class frmSaleRegisterItemWise : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindFilters();
                txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
                txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);
            }
        }

        private void BindFilters()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string queryCustomerGroup = " Select CUSTGROUP_CODE, CUSTGROUP_NAME, CUSTGROUP_CODE+'-'+ CUSTGROUP_NAME as CUSTGROUP from [ax].[ACXCUSTGROUPMASTER] where DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and BLOCKED<>1 ";
            DDLCustGroupNew.Items.Clear();
            DDLCustGroupNew.Items.Add("ALL");
            obj.BindToDropDownp(DDLCustGroupNew, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE");
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = Validate();
            if (b)
            {
                ShowReport();
            }
        }

        private void ShowReport()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            DataTable dtSetData = null;

            try
            {

                string query = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query);
                if (DDLCustGroupNew.SelectedValue.ToString() == "ALL")
                {
                    FilterQuery = "EXEC SP_SALEREGISTERITEMWISE '" + ucRoleFilters.GetCommaSepartedSiteId() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + "'";
                }
                else
                {
                    FilterQuery = "EXEC SP_SALEREGISTERITEMWISE '" + ucRoleFilters.GetCommaSepartedSiteId() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + DDLCustGroupNew.SelectedValue.ToString() + "'";
                }
                dtSetData = new DataTable();
                dtSetData = obj.GetData(FilterQuery);

                LoadDataInReportViewer(dtSetHeader, dtSetData);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
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

        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData)
        {
            try
            {
                if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleRegisterItemWise.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportParameter[] parameter = new ReportParameter[2];
                    parameter[1] = FromDate;
                    parameter[0] = ToDate;
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtSetHeader);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportViewer1.ShowPrintButton = true;
                    this.ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;
                    ReportViewer1.ZoomPercent = 100;
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