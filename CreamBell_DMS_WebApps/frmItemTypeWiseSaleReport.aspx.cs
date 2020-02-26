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
    public partial class frmItemTypeWiseSaleReport : System.Web.UI.Page
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

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = ValidateInput();
            if (b)
            {
                ShowReport();
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
        private void BindFilters()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string queryCustomerGroup = " Select CUSTGROUP_CODE, CUSTGROUP_NAME, CUSTGROUP_CODE+'-'+ CUSTGROUP_NAME as CUSTGROUP from [ax].[ACXCUSTGROUPMASTER] where DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and BLOCKED<>1 ";
            DDLCustGroupNew.Items.Clear();
            DDLCustGroupNew.Items.Add("ALL");
            obj.BindToDropDownp(DDLCustGroupNew, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE");
        }
        protected void DDLCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            if (DDLCustGroupNew.Text == "ALL")
            {
                string queryALLCustomer = " Select CUSTOMER_CODE, CUSTOMER_NAME from ax.acxcustmaster where  SITE_CODE='" + Session["SiteCode"].ToString() +
                                          "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'and BLOCKED = 0";

                DDLCustomersNew.Items.Clear();
                //DDLCustomers.Items.Add("-Select-");
                //obj.BindToDropDown(DDLCustomers, queryALLCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");

            }
            else
            {
                string queryCustomer = " Select CUSTOMER_CODE, CUSTOMER_NAME from ax.acxcustmaster where CUST_GROUP='" + DDLCustGroupNew.SelectedValue.ToString() + "' " +
                                        " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'and BLOCKED = 0";

                DDLCustomersNew.Items.Clear();
                DDLCustomersNew.Items.Add("--SELECT--");
                obj.BindToDropDownp(DDLCustomersNew, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");
            }

        }

        private void ShowReport()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            DataTable dtSetData = null;
            string CustomerGroup = string.Empty;
            string Customer = string.Empty;
            if (DDLCustGroupNew.SelectedIndex > 0)
            {
                CustomerGroup = DDLCustGroupNew.SelectedValue.ToString();
            }
            else
            {
                CustomerGroup = "";
            }
            if (DDLCustomersNew.SelectedIndex > 0)
            {
                Customer = DDLCustomersNew.SelectedValue.ToString();
            }
            else
            {
                Customer = "";
            }

            try
            {

                string query = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query);


                FilterQuery = "EXEC ACX_USP_ItemTypeWiseSaleReport '" + ucRoleFilters.GetCommaSepartedSiteId() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + CustomerGroup + "','" + Customer + "'";
                dtSetData = new DataTable();
                dtSetData = obj.GetData(FilterQuery);

                LoadDataInReportViewer(dtSetHeader, dtSetData);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }

        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData)
        {
            try
            {
                if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SKUTypeWiseSale.rdl");
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
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }

    }
}