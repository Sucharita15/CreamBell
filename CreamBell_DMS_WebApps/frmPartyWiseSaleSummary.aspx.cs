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
using CreamBell_DMS_WebApps.App_Code;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPartyWiseSaleSummary : System.Web.UI.Page
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
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string queryCustomerGroup = " Select CUSTGROUP_CODE, CUSTGROUP_NAME, CUSTGROUP_CODE+'-'+ CUSTGROUP_NAME as CUSTGROUP from [ax].[ACXCUSTGROUPMASTER] where DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and BLOCKED<>1 ";
            //DDLCustGroup.Items.Clear();
            //DDLCustGroup.Items.Add("ALL");
            //obj.BindToDropDown(DDLCustGroup, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE");
            CreamBell_DMS_WebApps.App_Code.CreamBellFramework.BindToDropDown(DDLCustGroup, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE","ALL");
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            //DDLBusinessUnit.Items.Clear();
            //DDLBusinessUnit.Items.Add("All...");
            //obj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
            CreamBell_DMS_WebApps.App_Code.CreamBellFramework.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code", "All...");
        }
        protected void DDLCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            if (DDLCustGroup.Text == "ALL")
            {
                string queryALLCustomer = " Select CUSTOMER_CODE, CUSTOMER_NAME from ax.acxcustmaster where  SITE_CODE='" + Session["SiteCode"].ToString() +
                                          "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'" +
                                          " UNION " +
                                          "SELECT SUBDISTRIBUTOR AS Customer_Code, SUBDISTRIBUTOR + '-' + NAME AS CUSTOMER_NAME  from ax.ACX_SDLINKING where OTHER_SITE='" + Session["SiteCode"].ToString() +
                                          "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                DDLCustomers.Items.Clear();
                //DDLCustomers.Items.Add("-Select-");
                //obj.BindToDropDown(DDLCustomers, queryALLCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");

            }
            else
            {
                string queryCustomer = " Select CUSTOMER_CODE, CUSTOMER_NAME from ax.acxcustmaster where CUST_GROUP='" + DDLCustGroup.SelectedValue.ToString() + "' " +
                                        " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'" +
                                        " UNION " +
                                        "SELECT SUBDISTRIBUTOR AS Customer_Code, SUBDISTRIBUTOR + '-' + NAME AS CUSTOMER_NAME  from ax.ACX_SDLINKING where CUSTGROUP='" + DDLCustGroup.SelectedValue.ToString() + "' " +
                                        " and OTHER_SITE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                //DDLCustomers.Items.Clear();
                //DDLCustomers.Items.Add("-Select-");
                //obj.BindToDropDown(DDLCustomers, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");
                CreamBell_DMS_WebApps.App_Code.CreamBellFramework.BindToDropDown(DDLCustomers, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE", "-Select-");
            }

        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            //bool b = ValidateInput();
            if (ValidateInput())
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

        private void ShowReport()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            //DataTable dtSetHeader = null;
            //DataTable dtSetData = null;
            //DataTable dtTotalInvoiceNo = null;

            try
            {

                //string query = "Select NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                DataTable dtSetHeader = new DataTable();
                DataColumn dc = new DataColumn(SessionKeys.NAME);
                dtSetHeader.Columns.Add(dc);
                dtSetHeader.Rows.Add(Session[SessionKeys.NAME].ToString());
                //dtSetHeader = obj.GetData(query);
                string CustomerGroup = string.Empty;
                string Customer = string.Empty;
                string BU = string.Empty;
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
                if (DDLBusinessUnit.SelectedIndex >= 1)
                {
                    BU = DDLBusinessUnit.SelectedItem.Value.ToString();
                }
                else
                {
                    BU = "";
                }

                //FilterQuery = "EXEC SP_PARTYWISESALESUMMARY '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + CustomerGroup + "','" + Customer + "','" + BU + "'";
                FilterQuery = "[SP_PARTYWISESALESUMMARY_TI_Version]"; //"SP_PARTYWISESALESUMMARY"; 
                    //+ Session["SiteCode"].ToString() + "','" 
                    //+ Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" 
                    //+ Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" 
                    //+ CustomerGroup + "','" 
                    //+ Customer + "','" 
                    //+ BU + "'";

                //FilterQuery = " SELECT SITEID, INVOICE_NO, SIW.CUSTOMER_NAME,C.CUST_GROUP,"
                //             + " isnull(BOXQty,'0') as Box,  isnull(PCSQTY,'0') as PCS, isnull(BOXPCS,'0') as [TotalBoxPCS],  BOX as TotalQtyConv, SIW.LTR, LINEAMOUNT, DISC_AMOUNT, SEC_DISC_AMOUNT, DISC, TAX_CODE, TAX_AMOUNT,  ADDTAX_CODE, "
                //             + " ADDTAX_AMOUNT, AMOUNT,TD_Per,PE_Per,tdvalue FROM ACX_SALESUMMARY_PARTY_ITEM_WISE SIW"
                //             + " INNER JOIN AX.INVENTTABLE INVT ON SIW.PRODUCT_CODE = INVT.ITEMID "
                //             + " LEFT JOIN [ax].[ACXCUSTMASTER] C on SIW.Customer_Code = C.CUSTOMER_CODE" 
                //             + " where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >=" +
                //              " '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "' " +
                //              " ORDER BY INVOICE_DATE ASC , INVOICE_NO ASC ";
                List<string> param = new List<string>();
                List<string> paramVal = new List<string>();
                param.Add("@SiteId");
                paramVal.Add(Session["SiteCode"].ToString());

                param.Add("@StartDate");
                paramVal.Add(Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"));

                param.Add("@EndDate");
                paramVal.Add(Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"));

                param.Add("@customergroupname");
                paramVal.Add(CustomerGroup);

                param.Add("@customername");
                paramVal.Add(Customer);

                param.Add("@BUCODE");
                paramVal.Add(BU);

                DataTable dtSetData = obj.GetData_New(FilterQuery, CommandType.StoredProcedure, param, paramVal);
                //dtSetData = new DataTable();
                //dtSetData = obj.GetData(FilterQuery);

                string queryTotInv = " Select Count(Distinct INVOICE_NO) as InvoiceNo FROM ACX_SALESUMMARY_PARTY_ITEM_WISE " +
                                     " where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >=" +
                                     " '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "' group by CUSTOMER_NAME";
                DataTable dtTotalInvoiceNo = obj.GetData(queryTotInv);
                LoadDataInReportViewer(dtSetHeader, dtSetData ,dtTotalInvoiceNo);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData, DataTable dtTotalInvoiceNo)
        {
            try
            {
                if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PartyWiseSaleSummary.rdl");
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
                    ReportDataSource RDS3 = new ReportDataSource("TotalDistinctSumGroupbyInvoiceNo", dtTotalInvoiceNo);
                    ReportViewer1.LocalReport.DataSources.Add(RDS3);
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