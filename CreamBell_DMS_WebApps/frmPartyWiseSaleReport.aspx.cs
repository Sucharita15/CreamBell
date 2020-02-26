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
    public partial class frmPartyWiseSaleReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionKeys.USERID] == null || Session[SessionKeys.USERID].ToString() == string.Empty)
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
            //CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string queryCustomerGroup = " Select CUSTGROUP_CODE, CUSTGROUP_NAME, CUSTGROUP_CODE+'-'+ CUSTGROUP_NAME as CUSTGROUP from [ax].[ACXCUSTGROUPMASTER] where DATAAREAID='" + Session[SessionKeys.DATAAREAID].ToString() + "' and BLOCKED<>1 ";
            //DDLCustGroup.Items.Clear();
            //DDLCustGroup.Items.Add("ALL");
            //obj.BindToDropDown(DDLCustGroup, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE");
            CreamBell_DMS_WebApps.App_Code.CreamBellFramework.BindToDropDown(DDLCustGroup, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE", "All");
        }

        protected void DDLCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            if (DDLCustGroup.Text == "ALL")
            {
                string queryALLCustomer = " Select CUSTOMER_CODE, CUSTOMER_NAME from ax.acxcustmaster where  SITE_CODE='" + Session[SessionKeys.SITECODE].ToString() +
                                          "' and DATAAREAID='" + Session[SessionKeys.DATAAREAID].ToString() + "'and BLOCKED = 0";
                DDLCustomers.Items.Clear();
                //DDLCustomers.Items.Add("-Select-");
                //obj.BindToDropDown(DDLCustomers, queryALLCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");
            }
            else
            {
                string queryCustomer = " Select CUSTOMER_CODE, CUSTOMER_NAME from ax.acxcustmaster where CUST_GROUP='" + DDLCustGroup.SelectedValue.ToString() + "' " +
                                        " and SITE_CODE='" + Session[SessionKeys.SITECODE].ToString() + "' and DATAAREAID='" + Session[SessionKeys.DATAAREAID].ToString() + "'and BLOCKED = 0";
                //DDLCustomers.Items.Clear();
                //DDLCustomers.Items.Add("-Select-");
                //obj.BindToDropDown(DDLCustomers, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");
                CreamBell_DMS_WebApps.App_Code.CreamBellFramework.BindToDropDown(DDLCustomers, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE", "-Select-");
            }
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            //bool b = Validate();
            if (Validate())
            {
                ShowReport();
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

        private void ShowReport()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //string FilterQuery = string.Empty;
            //DataTable dtSetHeader = null;
            //DataTable dtSetData = null;

            try
            {
                //string query = "Select NAME from ax.inventsite where SITEID='" + Session[SessionKeys.SITECODE].ToString() + "'";
                DataTable dtSetHeader = new DataTable();
                DataColumn dc = new DataColumn(SessionKeys.NAME);
                dtSetHeader.Columns.Add(dc);
                dtSetHeader.Rows.Add(Session[SessionKeys.NAME].ToString());
                //dtSetHeader = obj.GetData(query);
                string CustomerGroup = string.Empty;
                string Customer = string.Empty;
                if (DDLCustGroup.SelectedIndex > 0)
                {
                    CustomerGroup = DDLCustGroup.SelectedValue.ToString();
                }
                else
                {
                    CustomerGroup = "";
                }
                if (DDLCustomers.SelectedIndex > 0)
                {
                    Customer = DDLCustomers.SelectedValue.ToString();
                }
                else
                {
                    Customer = "";
                }

                //FilterQuery = " SELECT SITEID, SCS.CUSTOMER_NAME,C.CUST_GROUP, SCS.NAMEALIAS, AMOUNT , " +
                //              "  isnull(BOXQty,'0') as Box,  isnull(PCSQTY,'0') as PCS, isnull(BOXPCS,'0') as [TotalBoxPCS],  BOX as TotalQtyConv ,SCS.LTR " +
                //              " from [ACX_SKUWISE_COMPLETESALE] SCS" +
                //              " INNER JOIN AX.INVENTTABLE INVT ON SCS.PRODUCT_CODE = INVT.ITEMID " +
                //               " LEFT JOIN [ax].[ACXCUSTMASTER] C on SCS.Customer_Code = C.CUSTOMER_CODE" +
                //              " where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >=" +
                //              " '" + Convert.ToDateTime(txtFromDate.Text) + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text) + "' " +
                //              " ORDER BY CUSTOMER_NAME ASC ";

                string FilterQuery = "ACX_USP_PARTYWISESALE";
                //string FilterQuery = "ACX_USP_PARTYWISESALE '" 
                //+ Session["SiteCode"].ToString() + "','" 
                //+ Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" 
                //+ Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" 
                //+ CustomerGroup + "','" + Customer + "'";
                List<string> param = new List<string>();
                List<string> paramVal = new List<string>();
                param.Add("@SITEID");
                paramVal.Add(Session["SiteCode"].ToString());

                param.Add("@INITIALDATE");
                paramVal.Add(Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"));

                param.Add("@FINALDATE");
                paramVal.Add(Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"));

                param.Add("@customergroupname");
                paramVal.Add(CustomerGroup);

                param.Add("@customername");
                paramVal.Add(Customer);

                param.Add("@BUCODE");
                paramVal.Add("");

                DataTable dtSetData = obj.GetData_New(FilterQuery, CommandType.StoredProcedure, param, paramVal);

                //dtSetData = new DataTable();
                //DataTable dtSetData = obj.GetData(FilterQuery);

                LoadDataInReportViewer(dtSetHeader, dtSetData);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData)
        {
            try
            {
                if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PartyItemWiseSale.rdl");
                    ReportViewer1.AsyncRendering = true;
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
                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtSetHeader);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportViewer1.ShowPrintButton = true;
                    ReportViewer1.Visible = true;
                    ReportViewer1.ZoomPercent = 100;
                    this.ReportViewer1.LocalReport.Refresh();
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