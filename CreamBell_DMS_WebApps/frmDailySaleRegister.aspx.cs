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
    public partial class frmDailySaleRegister : System.Web.UI.Page
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
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                string queryCustomerGroup = " Select CUSTGROUP_CODE, CUSTGROUP_NAME, CUSTGROUP_CODE+'-'+ CUSTGROUP_NAME as CUSTGROUP from [ax].[ACXCUSTGROUPMASTER] where DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and BLOCKED<>1 ";
                DDLCustGroupNew.Items.Clear();
                DDLCustGroupNew.Items.Add("ALL");
                obj.BindToDropDownp(DDLCustGroupNew, queryCustomerGroup, "CUSTGROUP", "CUSTGROUP_CODE");

                string queryProductGroup = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE";
                DDLProductGroupNew.Items.Clear();
                DDLProductGroupNew.Items.Add("ALL");
                obj.BindToDropDownp(DDLProductGroupNew, queryProductGroup, "PRODUCT_GROUP", "PRODUCT_GROUP");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DDLCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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
                    DDLCustomersNew.Items.Add("-Select-");
                    obj.BindToDropDownp(DDLCustomersNew, queryCustomer, "CUSTOMER_NAME", "CUSTOMER_CODE");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DDLProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                if (DDLProductGroupNew.Text == "ALL")
                {
                    DDLSubCategoryNew.Items.Clear();
                    DDLProductNew.Items.Clear();
                }
                else
                {
                    string strQuery = " Select distinct  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                                     " ax.INVENTTABLE where replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLProductGroupNew.SelectedItem.Text.ToString() + "' ";


                    DDLSubCategoryNew.Items.Clear();
                    DDLSubCategoryNew.Items.Clear();
                    DDLSubCategoryNew.Items.Add("-Select-");
                    obj.BindToDropDownp(DDLSubCategoryNew, strQuery, "SUBCATEGORY", "SUBCATEGORY");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DDLSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                if (DDLSubCategoryNew.Text == "-Select-")
                {
                    DDLProductNew.Items.Clear();
                }
                else
                {
                    string strQuery = " Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME,PRODUCT_NAME as PRODDESCP, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where " +
                                 " replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLSubCategoryNew.SelectedItem.Text.ToString() + "' ";

                    DDLProductNew.Items.Clear();
                    DDLProductNew.Items.Add("-Select-");
                    obj.BindToDropDownp(DDLProductNew, strQuery, "PRODUCT_NAME", "PRODDESCP");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void imgBtnRefresh_Click(object sender, ImageClickEventArgs e)
        {
            BindFilters();
            DDLCustomersNew.Items.Clear();
            DDLSubCategoryNew.Items.Clear();
            DDLProductNew.Items.Clear();
            txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
            txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);
            LblMessage.Text = string.Empty;
            ReportViewer1.Visible = false;
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
           
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            DataTable dtSetData = null;
            try
            {
                var siteCodes = ucRoleFilters.GetCommaSepartedSiteId();

                string query = " Select NAME from ax.inventsite where siteid IN ("+ siteCodes + ")";
                //string query = " Select NAME from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query);
                dtSetHeader.Columns.Add("FromDate");
                dtSetHeader.Columns.Add("ToDate");
                dtSetHeader.Rows.Add(query);
                dtSetHeader.Rows[0]["FromDate"] = txtFromDate.Text;
                dtSetHeader.Rows[0]["ToDate"] = txtToDate.Text;

                string CustomerGroup = string.Empty;
                string Customer = string.Empty;
                string ProductGroup = string.Empty;
                string ProdSubCategory = string.Empty;
                string Product = string.Empty;
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
                if (DDLProductGroupNew.SelectedIndex > 0)
                {
                    ProductGroup = DDLProductGroupNew.SelectedValue.ToString();
                }
                else
                {
                    ProductGroup = "";
                }
                if (DDLSubCategoryNew.SelectedIndex > 0)
                {
                    ProdSubCategory = DDLSubCategoryNew.SelectedValue.ToString();
                }
                else
                {
                    ProdSubCategory = "";
                }
                if (DDLProductNew.SelectedIndex > 0)
                {
                    Product = DDLProductNew.SelectedValue.ToString();
                }
                else
                {
                    Product = "";
                }
                //FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + CustomerGroup + "','" + Customer + "','" + ProductGroup + "','" + ProdSubCategory + "','" + Product + "'";
                FilterQuery = "EXEC SP_SALEREGISTER '" + siteCodes + "','" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "','" + CustomerGroup + "','" + Customer + "','" + ProductGroup + "','" + ProdSubCategory + "','" + Product + "'";
                dtSetData = new DataTable();
                dtSetData = obj.GetData(FilterQuery);

                LoadDataInReportViewer(dtSetHeader, dtSetData);

                //#region Filter By Both Customer and Product Wise
                //FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "'";

                //if (DDLCustGroup.Text == "ALL" && DDLProductGroup.Text == "ALL")
                //{

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);

                //}

                //if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text == "-Select-" && DDLProductGroup.Text == "ALL")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "','" + DDLCustGroup.SelectedValue + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text != "-Select-" && DDLProductGroup.Text == "ALL")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "','" + DDLCustGroup.SelectedValue + "','" + DDLCustomers.SelectedValue + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text == "-Select-" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text != "-Select-" && DDLProduct.Text != "-Select")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "','" + DDLCustGroup.SelectedValue + "',@productgroup='" + DDLProductGroup.SelectedValue + "',@subcategory='" + DDLSubCategory.SelectedValue + "',@productname='" + DDLProduct.SelectedValue + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //else if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text == "-Select-" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text != "-Select-")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "','" + DDLCustGroup.SelectedValue + "',@productgroup='" + DDLProductGroup.SelectedValue + "',@subcategory='" + DDLSubCategory.SelectedValue + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //else if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text == "-Select-" && DDLProductGroup.Text != "ALL")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "','" + DDLCustGroup.SelectedValue + "',@productgroup='" + DDLProductGroup.SelectedValue + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

           

                //else if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text != "-Select-" && DDLProductGroup.Text != "ALL")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "','" + DDLCustGroup.SelectedValue + "','" + DDLCustomers.SelectedValue + "','" + DDLProductGroup.SelectedValue + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text != "-Select-" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text != "-Select-")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text != "ALL" && DDLCustomers.Text != "-Select-" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text != "-Select-" && DDLProduct.Text != "-Select")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text == "ALL" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text == "-Select-")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text == "ALL" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text != "-Select-" && DDLProduct.Text == "-Select-")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "'";
                    
                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //if (DDLCustGroup.Text == "ALL" && DDLProductGroup.Text != "ALL" && DDLSubCategory.Text != "-Select-" && DDLProduct.Text != "-Select-")
                //{
                //    FilterQuery = "EXEC SP_SALEREGISTER '" + Session["SiteCode"].ToString() + "','" + Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "'";

                //    dtSetData = new DataTable();
                //    dtSetData = obj.GetData(FilterQuery);
                //}

                //#endregion
                

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
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\DailySaleRegister.rdl");
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
                    LblMessage.Text = String.Empty;
                }
                else
                {
                    LblMessage.Text = "No Records Exists !!";
                    ReportViewer1.Visible = false;
                }
            }
            catch(Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();

                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        
    }
}