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
    public partial class frmItemWisePartySaleSummary : System.Web.UI.Page
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
            bool b = Validate();
            if (b)
            {
                ShowReport();
            }
        }
        private void BindFilters()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string queryCustomerGroup = " Select CUSTGROUP_CODE, CUSTGROUP_NAME, CUSTGROUP_CODE+'-'+ CUSTGROUP_NAME as CUSTGROUP from [ax].[ACXCUSTGROUPMASTER] where DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and BLOCKED<>1 ";
            DDLCustGroupNew.Items.Clear();
            DDLCustGroupNew.Items.Add("ALL");
            obj.BindToDropDownp(DDLCustGroupNew, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE");

            //string queryProductGroup = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE";
            //DDLProductGroup.Items.Clear();
            //DDLProductGroup.Items.Add("ALL");
            //obj.BindToDropDown(DDLProductGroup, queryProductGroup, "PRODUCT_GROUP", "PRODUCT_GROUP");
        }

        protected void DDLCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

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
                DDLCustomersNew.Items.Add("-Select-");
                obj.BindToDropDownp(DDLCustomersNew, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");
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

            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            DataTable dtSetData = null;

            try
            {
                string query = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query);
                string CustomerGroup = string.Empty;
                string CustomerName = string.Empty;
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
                    CustomerName = DDLCustomersNew.SelectedValue.ToString();
                }
                else
                {
                    CustomerName = "";
                }


                // FilterQuery = " SELECT A.SITEID, A.CUSTOMER_NAME,C.CUST_GROUP, A.NAMEALIAS, A.AMOUNT"
                //    + ",isnull(BOXQty,'0') as Box,  isnull(PCSQTY,'0') as PCS, isnull(BOXPCS,'0') as [TotalBoxPCS], BOX as TotalQtyConv,A.LTR"
                //     + " from[ACX_SKUWISE_COMPLETESALE] A INNER JOIN AX.INVENTTABLE I ON I.ITEMID= A.PRODUCT_CODE" +
                //     " LEFT JOIN [ax].[ACXCUSTMASTER] C on A.Customer_Code = C.CUSTOMER_CODE" +
                //" where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >=" +
                //" '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "' " +
                //" ORDER BY CUSTOMER_NAME ASC ";
                //" SELECT SITEID, CUSTOMER_NAME, NAMEALIAS, AMOUNT, BOX, LTR  from [ACX_SKUWISE_COMPLETESALE] " +

                FilterQuery = "ACX_USP_ItemWisePartySaleSummary '" + ucRoleFilters.GetCommaSepartedSiteId() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + CustomerGroup+"','"+ CustomerName+"'";
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
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ItemWisePartySaleSummary.rdl");
                    ReportParameter SITEID = new ReportParameter();
                    SITEID.Name = "SITEID";
                    SITEID.Values.Add(Session["SiteCode"].ToString());
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    //ReportParameter CUST_GROUP = new ReportParameter();
                    //CUST_GROUP.Name = "CUST_GROUP";
                    //if (DDLCustGroup.SelectedIndex > 0)
                    //{ CUST_GROUP.Values.Add(DDLCustGroup.SelectedValue.ToString()); }
                    //else
                    //CUST_GROUP.Values.Add("");

                    //ReportParameter CUSTOMER_CODE = new ReportParameter();
                    //CUSTOMER_CODE.Name = "CUSTOMER_CODE";
                    //if (DDLCustomers.SelectedIndex > 0)
                    //{ CUSTOMER_CODE.Values.Add(DDLCustomers.SelectedValue.ToString()); }
                    //else
                    //    CUSTOMER_CODE.Values.Add("");

                    ReportParameter[] parameter = new ReportParameter[2];
                    //parameter[2] = SITEID;
                    parameter[0] = FromDate;
                    parameter[1] = ToDate;
                    //parameter[3] = CUST_GROUP;
                    //parameter[4] = CUSTOMER_CODE;
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