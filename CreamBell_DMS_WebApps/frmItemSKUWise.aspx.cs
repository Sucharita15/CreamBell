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
    public partial class frmItemSKUWise : System.Web.UI.Page
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

            string queryProductGroup = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE";
            DDLProductGroupNew.Items.Clear();
            DDLProductGroupNew.Items.Add("ALL");
            obj.BindToDropDownp(DDLProductGroupNew, queryProductGroup, "PRODUCT_GROUP", "PRODUCT_GROUP");
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

        protected void DDLProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            if (DDLProductGroupNew.Text == "ALL")
            {
                DDLSubCategoryNew.Items.Clear();
                //DDLProduct.Items.Clear();
            }
            else
            {
                string strQuery = " Select distinct  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                                 " ax.INVENTTABLE where replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLProductGroupNew.SelectedItem.Text.ToString() + "' ";
                
              
                DDLSubCategoryNew.Items.Clear();
                DDLSubCategoryNew.Items.Add("-Select-");
                obj.BindToDropDownp(DDLSubCategoryNew, strQuery, "SUBCATEGORY", "SUBCATEGORY");
            }


        }

        protected void DDLSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            if (DDLSubCategoryNew.Text == "-Select-")
            {
             //   DDLProduct.Items.Clear();
            }
            else
            {
                string strQuery = " Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME,PRODUCT_NAME as PRODDESCP, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where " +
                             " replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLSubCategoryNew.SelectedItem.Text.ToString() + "' ";

               // DDLProduct.Items.Clear();
                //DDLProduct.Items.Add("-Select-");
                //obj.BindToDropDown(DDLProduct, strQuery, "PRODUCT_NAME", "PRODDESCP");
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
                string query = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query);
                string CUST_GROUP = string.Empty;
                string CUSTOMER_NAME = string.Empty;
                string PRODUCT_GROUP = string.Empty;
                string PRODUCT_SUBCATEGORY = string.Empty;
                //string Product = string.Empty;
                if (DDLCustGroupNew.SelectedIndex > 0)
                {
                    CUST_GROUP = DDLCustGroupNew.SelectedValue.ToString();
                }
                else
                {
                    CUST_GROUP = "";
                }
                if (DDLCustomersNew.SelectedIndex > 0)
                {
                    CUSTOMER_NAME = DDLCustomersNew.SelectedValue.ToString();
                }
                else
                {
                    CUSTOMER_NAME = "";
                }
                if (DDLProductGroupNew.SelectedIndex > 0)
                {
                    PRODUCT_GROUP = DDLProductGroupNew.SelectedValue.ToString();
                }
                else
                {
                    PRODUCT_GROUP = "";
                }
                if (DDLSubCategoryNew.SelectedIndex > 0)
                {
                    PRODUCT_SUBCATEGORY = DDLSubCategoryNew.SelectedValue.ToString();
                }
                else
                {
                    PRODUCT_SUBCATEGORY = "";
                }

             

               // FilterQuery = "select ASS.SITEID, ASS.NAMEALIAS, ASS.PRODUCT_NAME,c.CUST_GROUP, c.CUSTOMER_NAME,"+
                     // "INVT.PRODUCT_GROUP,INVT.PRODUCT_SUBCATEGORY, "+
                 //     "BOXQTY as BOX, PCSQTY as PCS,  isnull(BOXPCS, '0') as [TotalBoxPCS],"+
                   //   "BOX as TotalQtyConv, ASS.LTR, AMOUNT from ACX_SKUWISE_SALE ASS  "+ 
                     // "INNER JOIN AX.INVENTTABLE INVT ON ASS.PRODUCT_CODE = INVT.ITEMID  "+
                   //   "INNER JOIN ax.acxcustmaster C on ASS.SITEID = c.SITE_CODE "+
                     // "   where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >='" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "'"+
                      //" and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "' and " +
                      //"  CUST_GROUP ='" + CUST_GROUP + "' and  CUSTOMER_NAME='" + CUSTOMER_NAME + "'  and  PRODUCT_GROUP='" + PRODUCT_GROUP + "'  and  PRODUCT_SUBCATEGORY='" + PRODUCT_SUBCATEGORY + "' ORDER BY NAMEALIAS ASC ";
                FilterQuery = "EXEC Usp_ITEMSKUWISE '" + ucRoleFilters.GetCommaSepartedSiteId() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + CUST_GROUP + "','" + CUSTOMER_NAME + "','" + PRODUCT_GROUP + "','" + PRODUCT_SUBCATEGORY + "'";

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
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SKUWiseSale.rdl");
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
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtSetHeader);
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