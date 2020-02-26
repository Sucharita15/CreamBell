
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCreditDebitNotePurchaser : System.Web.UI.Page
    {
        public DataTable dtLineItems;
        public SqlCommand cmd;
        public SqlCommand cmd1;
        public SqlCommand cmd2;
        public SqlCommand cmd3;
        public SqlTransaction transaction;
        public SqlConnection conn;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                this.txtAdjQty.Attributes.Add("onkeypress", "button_click(this,'" + this.BtnAdd.ClientID + "')");
                FillInvoiceValue();
                FillMaterialGroup();
                FillReturnReasonType();
                LoadWarehouse();
                Session["LineItem"] = null;
                //imgBtDate.Visible = false;
            }
        }

        private void FillMaterialGroup()
        {
            try{
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string strQuery = string.Empty;
            DDLProdSubCategory.Items.Clear();
            DDLProdSubCategory.Items.Add("-Select-");

            DDLProductDesc.Items.Clear();
            DDLProductDesc.Items.Add("-Select-");
            if (ddlInvoiceNo.SelectedItem.Text == "-Select-")
            {
                strQuery = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE WHERE BLOCK=0";
            }
            else
            {
                if (DDLReason.SelectedItem.Text.Contains("SKU"))
                {
                    strQuery = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE WHERE BLOCK=0";
                }
                else
                {
                    strQuery = " Select distinct PRODUCT_GROUP from ax.INVENTTABLE where BLOCK=0 AND " +
                                  " ax.INVENTTABLE.ITEMID in (select PRODUCT_CODE from ax.[ACXPURCHINVRECIEPTLINE] where PURCH_RECIEPTNO ='" + ddlInvoiceNo.SelectedValue.ToString() + "' and SiteId='"+ Session["SiteCode"].ToString() +"' )";
                }
            }
            DDLProductGroup.Items.Clear();
            DDLProductGroup.Items.Add("-Select-");
            obj.BindToDropDown(DDLProductGroup, strQuery, "PRODUCT_GROUP", "PRODUCT_GROUP");
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadWarehouse()
        {
            try{
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
            DataTable dt = new DataTable();
            dt = obj.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                //LblWarehouse.Text = Session["TransLocation"].ToString() + " - " + "MAIN WAREHOUSE";
            }
            else
            {
                //LblWarehouse.Text = "MAIN WAREHOUSE";
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void FillReturnReasonType()
        {
            try{
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string strQuery = "Select distinct DAMAGEREASON_CODE,DAMAGEREASON_CODE + '-(' + DAMAGEREASON_NAME +')' as RETURNREASON  from [ax].[ACXDAMAGEREASON]";
            DDLReason.Items.Clear();
            DDLReason.Items.Add("-Select-");
            obj.BindToDropDown(DDLReason, strQuery, "RETURNREASON", "DAMAGEREASON_CODE");
        }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void FillInvoiceValue()
        {
            try{
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string strQuery = "select (Document_no +' ('+ Sale_InvoiceNo+')') as Sale_InvoiceNo , Document_no  from [ax].[ACXPURCHINVRECIEPTHEADER] where Site_Code='" + Session["SiteCode"].ToString() + "' ";
            ddlInvoiceNo.Items.Clear();
            ddlInvoiceNo.Items.Add("-Select-");
            obj.BindToDropDown(ddlInvoiceNo, strQuery, "Sale_InvoiceNo", "Document_no");
        }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DDLProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try{
            if (DDLProductGroup.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string strQuery = string.Empty;
                if (ddlInvoiceNo.SelectedItem.Text == "-Select-")
                {
                    strQuery = " Select distinct  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                                      " ax.INVENTTABLE where  BLOCK=0 AND replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLProductGroup.SelectedItem.Text.ToString() + "' ";
                }
                else
                {
                    if (DDLReason.SelectedItem.Text.Contains("SKU"))
                    {
                        strQuery = " Select distinct  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                                     " ax.INVENTTABLE where BLOCK=0 AND replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLProductGroup.SelectedItem.Text.ToString() + "' ";
                    }
                    else
                    {
                        strQuery = " Select distinct replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                                         " ax.INVENTTABLE where BLOCK=0 AND ax.INVENTTABLE.ITEMID in (select PRODUCT_CODE from ax.[ACXPURCHINVRECIEPTLINE] where PURCH_RECIEPTNO ='" + ddlInvoiceNo.SelectedValue.ToString() + "' ) and replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLProductGroup.SelectedItem.Text.ToString() + "' ";
                    }
                }
                DDLProdSubCategory.Items.Clear();
                DDLProductDesc.Items.Clear();
                DDLProdSubCategory.Items.Add("-Select-");
                obj.BindToDropDown(DDLProdSubCategory, strQuery, "SUBCATEGORY", "SUBCATEGORY");
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DDLProdSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try{
            LblMessage.Text = string.Empty;
            if (DDLReason.Text == "-Select-")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Reason !');", true);
                DDLReason.Focus();
                return;
            }

            if (DDLProdSubCategory.Text != "-Select-")
            {
                string strQuery = string.Empty;
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                if (ddlInvoiceNo.SelectedItem.Text == "-Select-")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Invoice No. !');", true);
                    ddlInvoiceNo.Focus();
                    return;
                }
                if (DDLReason.SelectedItem.Text.Contains("SKU"))
                {
                    strQuery = " Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where BLOCK=0 AND " +
                                " replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLProdSubCategory.SelectedItem.Text.ToString() + "' ";
                }
                else
                {
                    strQuery = " Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where BLOCK=0 AND " +
                              " ax.INVENTTABLE.ITEMID in (select PRODUCT_CODE from ax.[ACXPURCHINVRECIEPTLINE] where PURCH_RECIEPTNO ='" + ddlInvoiceNo.SelectedValue.ToString() + "' and SiteId='" + Session["SiteCode"].ToString() + "') and  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLProdSubCategory.SelectedItem.Text.ToString() + "' ";
                }
                DDLProductDesc.Items.Clear();
                DDLProductDesc.Items.Add("-Select-");
                obj.BindToDropDown(DDLProductDesc, strQuery, "PRODUCT_NAME", "ITEMID");
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtAdjValue_TextChanged(object sender, EventArgs e)
        {
            if (txtAdjQty.Text != string.Empty)
            {
                BtnAdd.Focus();
            }
        }

        private bool ValidateLineItemAdd()
        {
            bool b = true;
            try{
            lblPreBoxQty.Text = (lblPreBoxQty.Text.Trim().Length == 0 ? "0" : lblPreBoxQty.Text);
            if (DDLProductGroup.Text == "-Select-")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Group !');", true);
                DDLProductGroup.Focus();
                b = false;
                return b;
            }
            if (DDLProdSubCategory.Text == "-Select-")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Sub Category First !');", true);
                DDLProdSubCategory.Focus();
                b = false;
                return b;
            }

            if (DDLProductDesc.Text == "-Select-")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product First !');", true);
                DDLProductDesc.Focus();
                b = false;
                return b;
            }
            if (DDLReason.Text == "-Select-")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Reason Type !');", true);
                DDLReason.Focus();
                b = false;
                return b;
            }

            if (txtAdjQty.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Provide Adjustment Qty !');", true);
                txtAdjQty.Focus();
                b = false;
                return b;
            }

            if (txtAdjQty.Text.Length > 0 && txtAdjQty.Text != string.Empty)
            {
                if (Convert.ToDecimal(txtAdjQty.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('  Adjustment Value cannot be 0 !');", true);
                    txtAdjQty.Focus();
                    b = false;
                    return b;
                }
                if (Convert.ToDecimal(txtAdjQty.Text.Trim()) < 0 && (DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX")))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Negative Quantity Not Allowed For PRICE & TAX Mismatch !');", true);
                    txtAdjQty.Focus();
                    b = false;
                    return b;
                }
                if (Convert.ToDecimal(txtAdjQty.Text.Trim()) != Convert.ToDecimal(lblPreBoxQty.Text.Trim())  && (DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX")))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Invalid Quantity For PRICE & TAX Mismatch !');", true);
                    txtAdjQty.Focus();
                    b = false;
                    return b;
                }
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string strSQL = "SELECT COUNT(*) FROM [ax].ACXADJUSTMENTENTRYHEADER HD " +
                                " INNER JOIN [ax].[ACXADJUSTMENTENTRYLINE] HL ON HD.DOCUMENT_NO = HL.DOCUMENT_NO AND HD.SITE_CODE = HL.SITEID " +
                                " WHERE HD.PURCH_RECIEPTNO = '"+ ddlInvoiceNo.SelectedValue.ToString()  + "' and HD.SITE_CODE = '"+ Session["SiteCode"].ToString()  + "' AND HL.PRODUCT_CODE = '"+ DDLProductDesc.SelectedValue.ToString()  + "' AND HL.DAMAGEREASON = '"+ DDLReason.SelectedValue.ToString()  + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(strSQL);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Adjustment Already Done !');", true);
                        txtAdjQty.Focus();
                        b = false;
                        return b;
                    }
                }
                //else if ((DDLReason.SelectedItem.Text.Contains("SKU") || DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX")) && Convert.ToDecimal(txtAdjQty.Text) > 0)
                //{
                //    DataTable dt = (DataTable)Session["LineItem"];
                //    if (Session["LineItem"] != null &&  dt.Rows.Count>0)
                //    {
                //        DataTable dtcheck = (DataTable)Session["LineItem"];
                //        DataRow[] drrow = dtcheck.Select("Reason='" + DDLReason.SelectedValue.ToString() + "' and PRODUCT_CODE='" + DDLProductDesc.SelectedValue.ToString() + "'");
                //        if (drrow.Length > 0)
                //        {
                //            decimal AlreadyAddQty = Convert.ToDecimal(dtcheck.Compute("Sum(AdjustmentValue)", " Reason='" + DDLReason.SelectedValue.ToString() + "' and PRODUCT_CODE='" + DDLProductDesc.SelectedValue.ToString() + "'"));
                //            if (AlreadyAddQty + Convert.ToDecimal(txtAdjQty.Text) > 0)
                //            {
                //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('In case of SKU Missmatch, Price Missmatch or Tax Missmatch, Adjustment qty cannot be greater than reduced qty!');", true);
                //                txtAdjQty.Focus();
                //                b = false;
                //                return b;
                //            }
                //            else
                //            {
                //                b = true;
                //                return b;
                //            }
                //        }
                //        else
                //        {
                //            b = true;
                //            return b;
                //        }
                //        //else
                //        //{
                //        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('In case of SKU Missmatch, Price Missmatch or Tax Missmatch first add negative stock of given product!');", true);
                //        //    txtAdjQty.Focus();
                //        //    b = false;
                //        //    return b;
                //        //}

                //    }
                //    else
                //    {
                //        b = true;
                //        return b;
                //    }
                //}
                //else if ((DDLReason.SelectedItem.Text.Contains("SKU") || DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX")) && Convert.ToDecimal(txtAdjQty.Text) > 0)
                //{
                //    DataTable dtcheck = (DataTable)Session["LineItem"];
                //    decimal AlreadyAddQty = Convert.ToDecimal(dtcheck.Compute("Sum(AdjustmentValue)", " ReasonName='" + DDLReason.Text.ToString() + "' and PRODUCT_CODE='" + DDLProductDesc.Text.ToString() + "'"));
                //    if (AlreadyAddQty + Convert.ToDecimal(txtAdjQty.Text) >= 0)
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('In case of SKU Missmatch, Price Missmatch or Tax Missmatch, Adjustment qty cannot be greater than reduced qty!');", true);
                //        txtAdjQty.Focus();
                //        b = false;
                //        return b;
                //    }
                //    else
                //    {
                //        b = true;
                //        return b;
                //    }
                //}
                //else
                //{
                //    b = true;
                //    return b;
                //}
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return b;
        }

        private DataTable AddLineItems()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                decimal strLtr = 0;
                decimal strPrice = 0;
                decimal strBasicValue = 0;
                decimal taxCode = 0;

                if (DDLReason.SelectedItem.Text.Contains("SKU"))
                {
                    CreamBell_DMS_WebApps.App_Code.Global baseObj = new App_Code.Global();

                    string querySearch = "select * from ax.[LOGISTICSADDRESSSTATE] where Stateid in (Select distinct StateCode from [ax].[INVENTSITE] where StateCode !='' and SITEID='" + Session["SiteCode"].ToString() + "') ";
                    DataTable dt = baseObj.GetData(querySearch);
                    string[] calValue = baseObj.CalculatePrice1(DDLProductDesc.SelectedValue.ToString(), Session["SiteCode"].ToString(), int.Parse(txtAdjQty.Text.Trim()), "Box","", 0);
                    if (Convert.ToString(calValue[2]).Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                        return Session["LineItem"] as DataTable;
                    }
                    if (Convert.ToDecimal(calValue[2]) < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                        return Session["LineItem"] as DataTable;
                    }
                    if (calValue[1] != null)
                    {
                        strLtr = Convert.ToDecimal(calValue[1].ToString());
                    }

                    if (calValue[2] != null)
                    {
                        strPrice = Convert.ToDecimal(calValue[2].ToString());
                        strBasicValue = (Convert.ToDecimal(txtAdjQty.Text.Trim()) * Convert.ToDecimal(strPrice));
                    }

                    string strUOM = string.Empty;
                    if (calValue[4] != null)
                    {
                        strUOM = calValue[4].ToString();
                    }
                    string srtTaxCode = " Select  H.TaxValue,H.ACXADDITIONALTAXVALUE, (case when H.ACXADDITIONALTAXBASIS=0 then (H.TaxValue+H.ACXADDITIONALTAXVALUE) else H.TaxValue+(H.TaxValue*H.ACXADDITIONALTAXVALUE/100) end ) as TaxPer  from  [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup  where H.ItemId='" + DDLProductDesc.SelectedValue.ToString() + "' and G.Siteid='" + Session["SITECODE"].ToString() + "'";

                    DataTable dtTax = new DataTable();

                    dtTax = baseObj.GetData(srtTaxCode);
                    if (dtTax.Rows.Count != 0)
                    {
                        taxCode = Convert.ToDecimal(dtTax.Rows[0]["TaxPer"].ToString());
                    }
                    else
                    {
                        taxCode = 0;
                    }
                }
                else if (DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX"))
                {
                    if (Convert.ToDecimal(txtAdjQty.Text.Trim()) > 0)
                    {
                        CreamBell_DMS_WebApps.App_Code.Global baseObj = new App_Code.Global();
                        string StatePriceGroup = "";
                        StatePriceGroup = obj.GetScalarValue("SELECT PRICEGROUP from[ax].[inventsite] where siteid = '" + Session["SiteCode"] + "'");
                        if(StatePriceGroup == null)
                        { StatePriceGroup = ""; }
                        if (StatePriceGroup.Length == 0)
                        {
                            StatePriceGroup = obj.GetScalarValue("SELECT TOP 1 ACX_PRICEGROUP FROM AX.LOGISTICSADDRESSSTATE WHERE STATEID = (SELECT STATECODE FROM AX.INVENTSITE WHERE SITEID = '" + Session["SiteCode"] + "')");
                        }
                        StatePriceGroup = Convert.ToString(StatePriceGroup).Trim() == "" ? "-" : StatePriceGroup;
                        string querySearch = "select * from ax.[LOGISTICSADDRESSSTATE] where Stateid in (Select distinct StateCode from [ax].[INVENTSITE] where StateCode !='' and SITEID='" + Session["SiteCode"].ToString() + "') ";
                        DataTable dt = baseObj.GetData(querySearch);
                        //string[] calValue = baseObj.CalculatePrice1(DDLProductDesc.SelectedValue.ToString(), dt.Rows[0]["ACX_PRICEGROUP"].ToString(), int.Parse(txtAdjQty.Text.Trim()), "Box","", 0);
                        string[] calValue = baseObj.CalculatePrice1(DDLProductDesc.SelectedValue.ToString(), StatePriceGroup.ToString(), int.Parse(txtAdjQty.Text.Trim()), "Box", "", 0);
                        if (Convert.ToString(calValue[2]).Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                            return Session["LineItem"] as DataTable;
                        }
                        if (Convert.ToDecimal(calValue[2]) < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                            return Session["LineItem"] as DataTable;
                        }
                        if (calValue[1] != null)
                        {
                            strLtr = Convert.ToDecimal(calValue[1].ToString());
                        }

                        if (calValue[2] != null)
                        {
                            strPrice = Convert.ToDecimal(calValue[2].ToString());
                            strBasicValue = (Convert.ToDecimal(txtAdjQty.Text.Trim()) * Convert.ToDecimal(strPrice));
                        }
                        string strUOM = string.Empty;
                        if (calValue[4] != null)
                        {
                            strUOM = calValue[4].ToString();
                        }
                        string srtTaxCode = " Select  H.TaxValue,H.ACXADDITIONALTAXVALUE, (case when H.ACXADDITIONALTAXBASIS=0 then (H.TaxValue+H.ACXADDITIONALTAXVALUE) else H.TaxValue+(H.TaxValue*H.ACXADDITIONALTAXVALUE/100) end ) as TaxPer  from  [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup  where H.ItemId='" + DDLProductDesc.SelectedValue.ToString() + "' and G.Siteid='" + Session["SITECODE"].ToString() + "'";

                        DataTable dtTax = new DataTable();

                        dtTax = baseObj.GetData(srtTaxCode);
                        if (dtTax.Rows.Count !=0)
                        {
                            taxCode = Convert.ToDecimal(dtTax.Rows[0]["TaxPer"].ToString());
                        }
                        else
                        {
                            taxCode = 0;
                        }
                    }
                }

                if (Session["LineItem"] == null)
                {
                    #region Runtime Datatable for Line Items
                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.AutoIncrement = true;
                    column.AutoIncrementSeed = 1;
                    column.AutoIncrementStep = 1;
                    column.ColumnName = "SNO";
                    //-----------------------------------------------------------//
                    dtLineItems = new DataTable("LineItemTable");
                    dtLineItems.Columns.Add(column);
                    dtLineItems.Columns.Add("ProductGroup", typeof(string));
                    dtLineItems.Columns.Add("ProductSubCategory", typeof(string));
                    dtLineItems.Columns.Add("ProductDesc", typeof(string));
                    dtLineItems.Columns.Add("UOM", typeof(string));
                    dtLineItems.Columns.Add("Reason", typeof(string));
                    dtLineItems.Columns.Add("AdjustmentValue", typeof(decimal));//BOX Value
                    //--------------------NEW-------------------------------
                    dtLineItems.Columns.Add("Sale_InvoiceNo", typeof(string));
                    dtLineItems.Columns.Add("PRODUCT_CODE", typeof(string));
                    dtLineItems.Columns.Add("PreBoxQty", typeof(decimal));
                    dtLineItems.Columns.Add("PreRate", typeof(decimal));
                    dtLineItems.Columns.Add("PreTax", typeof(decimal));
                    dtLineItems.Columns.Add("Amount", typeof(decimal));
                    dtLineItems.Columns.Add("PreBasicValue", typeof(decimal));
                    dtLineItems.Columns.Add("CreditDebitValue", typeof(decimal));
                    dtLineItems.Columns.Add("PURCH_RECIEPTNO", typeof(string));
                    dtLineItems.Columns.Add("ReasonName", typeof(string));
                    #endregion
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                }

                #region Check Duplicate Entry of Line Items through LINQ
                bool _exits = false;
                if (DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX"))
                {
                    _exits = false;
                }
                else
                {
                    try
                    {
                        if (dtLineItems.Rows.Count > 0)
                        {
                            var rowColl = dtLineItems.AsEnumerable();

                            var record = (from r in rowColl
                                          where r.Field<string>("ProductGroup") == DDLProductGroup.SelectedItem.Text &&
                                          r.Field<string>("ProductSubCategory") == DDLProdSubCategory.SelectedItem.Text.ToString()
                                          && r.Field<string>("ProductDesc") == DDLProductDesc.SelectedItem.Text.ToString() && r.Field<decimal>("AdjustmentValue") == Convert.ToDecimal(txtAdjQty.Text.ToString())
                                          select r.Field<int>("SNO")).First<int>();
                            if (record >= 1)
                            {
                                _exits = true;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: You are adding duplicate adjustment Entry !');", true);
                            }
                            else
                            {
                                _exits = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _exits = false;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
                #endregion

                #region Stock Availability Check For That Product
                string productNameCode = DDLProductDesc.SelectedItem.Text.ToString();
                string[] str = productNameCode.Split('-');
                string productCode = str[0].ToString();
                bool _stockCheck = false;
                if (DDLReason.SelectedItem.Text.Contains("SKU"))
                {
                    _stockCheck = true;
                }
                else
                {
                    _stockCheck = ValidateStock(productCode, Convert.ToDecimal(txtAdjQty.Text));
                }
                #endregion


                if (_exits == false && _stockCheck == true)
                {
                    #region Add New Row in Datatable with values

                    DataRow row;
                    row = dtLineItems.NewRow();
                    row["ProductGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                    row["ProductSubCategory"] = DDLProdSubCategory.SelectedItem.Text.ToString();
                    row["ProductDesc"] = DDLProductDesc.SelectedItem.Text.ToString();
                    row["UOM"] = DDLEntryType.SelectedItem.Text.ToString();
                    row["Reason"] = DDLReason.SelectedValue.ToString();
                    row["AdjustmentValue"] = Convert.ToDecimal(txtAdjQty.Text).ToString("0.00");
                    //-------NEW-----------------------
                    row["Sale_InvoiceNo"] = ddlInvoiceNo.SelectedValue.ToString();
                    string strPURCH_RECIEPTNO = ddlInvoiceNo.SelectedItem.Text.ToString();
                    string[] words = strPURCH_RECIEPTNO.Split('(');
                    foreach (string word in words)
                    {
                        Console.WriteLine(word);
                    }
                    row["PURCH_RECIEPTNO"] = words[0].ToString();
                    row["ReasonName"] = DDLReason.SelectedItem.Text.ToString();
                    row["PRODUCT_CODE"] = DDLProductDesc.SelectedValue.ToString();
                    if ((DDLReason.SelectedItem.Text.Contains("SKU") || DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX")) && Convert.ToDecimal(txtAdjQty.Text.ToString()) > 0)
                    {
                        row["PreBoxQty"] = 0;//Convert.ToDecimal(txtAdjQty.Text.Trim());
                        row["PreRate"] = strPrice;
                        row["PreTax"] = taxCode;
                        row["Amount"] = 0;//(Convert.ToDecimal(txtAdjQty.Text.Trim()) * strPrice + (Convert.ToDecimal(txtAdjQty.Text.Trim()) * strPrice * taxCode) / 100);
                        row["PreBasicValue"] = 0;//Convert.ToDecimal(txtAdjQty.Text.Trim()) * strPrice;
                        Decimal strRate = Convert.ToDecimal(txtAdjQty.Text.ToString()) * Convert.ToDecimal(strPrice);
                        row["CreditDebitValue"] = Convert.ToDecimal(strRate + (((strRate * Convert.ToDecimal(taxCode)) / 100))).ToString("0.00");
                    }
                    else
                    {
                        if (lblPreBoxQty.Text.Trim() != string.Empty)
                        {
                            row["PreBoxQty"] = Convert.ToDecimal(lblPreBoxQty.Text).ToString("0.00");
                        }
                        else
                        {
                            row["PreBoxQty"] = 0;
                        }
                        if (lblRate.Text.Trim() != string.Empty)
                        {
                            row["PreRate"] = Convert.ToDecimal(lblRate.Text.ToString()).ToString("0.00");
                        }
                        else
                        {
                            row["PreRate"] = 0;
                        }
                        if (lblTaxPer.Text.Trim() != string.Empty)
                        {
                            row["PreTax"] = Convert.ToDecimal(lblTaxPer.Text.ToString()).ToString("0.00");
                        }
                        else
                        {
                            row["PreTax"] = 0;
                        }
                        if (lblAmount.Text.Trim() != string.Empty)
                        {
                            row["Amount"] = Convert.ToDecimal(lblAmount.Text.ToString()).ToString("0.00");
                        }
                        else
                        {
                            row["Amount"] = 0;
                        }
                        if (lblBasicValue.Text.Trim() != string.Empty)
                        {
                            row["PreBasicValue"] = Convert.ToDecimal(lblBasicValue.Text.ToString()).ToString("0.00");
                        }
                        else
                        {
                            row["PreBasicValue"] = 0;
                        }
                        Decimal strRate = Convert.ToDecimal(txtAdjQty.Text.ToString()) * Convert.ToDecimal(lblRate.Text.ToString());
                        row["CreditDebitValue"] = Convert.ToDecimal(strRate + (((strRate * Convert.ToDecimal(lblTaxPer.Text.Trim())) / 100))).ToString("0.00");
                    }

                    dtLineItems.Rows.Add(row);
                    #endregion
                }
                //Update session table
                Session["LineItem"] = dtLineItems;
                //FillMaterialGroup();
                //////  DDLProdSubCategory.Items.Clear();
                //DDLProductDesc.Items.Clear();
                //DDLReason.SelectedIndex = 0;
                DDLEntryType.SelectedIndex = 0;
                txtAdjQty.Text = string.Empty;

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
        }

        private DataTable AddLineItemsForPrice_Tax_Mismatch()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                decimal strLtr = 0;
                decimal strPrice = 0;
                decimal strBasicValue = 0;
                decimal taxCode = 0;

               if (DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX"))
                {

                    if (Convert.ToDecimal(txtAdjQty.Text.Trim()) < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Negative Quantity Not Allowed For PRICE & TAX Mismatch !');", true);
                        return Session["LineItem"] as DataTable;
                    }

                    CreamBell_DMS_WebApps.App_Code.Global baseObj = new App_Code.Global();
                    string querySearch = "Select * from ax.[LOGISTICSADDRESSSTATE] where Stateid in (Select distinct StateCode from [ax].[INVENTSITE] where StateCode !='' and SITEID='" + Session["SiteCode"].ToString() + "') ";
                    DataTable dt = baseObj.GetData(querySearch);

                    string[] calValue = baseObj.CalculatePrice1(DDLProductDesc.SelectedValue.ToString(), Session["SiteCode"].ToString(), int.Parse(txtAdjQty.Text.Trim()), "Box","", 0);

                    if (Convert.ToString(calValue[2]).Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                        return Session["LineItem"] as DataTable;
                    }
                    if (Convert.ToDecimal(calValue[2]) < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                        return Session["LineItem"] as DataTable;
                    }
                    if (calValue[1] != null)
                    {
                        strLtr = Convert.ToDecimal(calValue[1].ToString());
                    }

                    if (calValue[2] != null)
                    {
                        strPrice = Convert.ToDecimal(calValue[2].ToString());
                        strBasicValue = (Convert.ToDecimal(txtAdjQty.Text.Trim()) * Convert.ToDecimal(strPrice));
                    }
                    string StatePriceGroup = "";
                    StatePriceGroup = obj.GetScalarValue("SELECT PRICEGROUP from[ax].[inventsite] where siteid = '" + Session["SiteCode"] + "'");
                    if(StatePriceGroup == null)
                    { StatePriceGroup = ""; }
                    if (StatePriceGroup.Length == 0)
                    {
                        StatePriceGroup = obj.GetScalarValue("SELECT TOP 1 ACX_PRICEGROUP FROM AX.LOGISTICSADDRESSSTATE WHERE STATEID = (SELECT STATECODE FROM AX.INVENTSITE WHERE SITEID = '" + Session["SiteCode"] + "')");
                    }
                    StatePriceGroup = Convert.ToString(StatePriceGroup).Trim() == "" ? "-" : StatePriceGroup;
                    //DataTable dtPrice = baseObj.GetData("Select Amount From [DBO].[ACX_UDF_GETPRICE](GETDATE(),'" + dt.Rows[0]["ACX_PRICEGROUP"].ToString() + "','" + DDLProductDesc.SelectedValue.ToString() + "')");
                    DataTable dtPrice = baseObj.GetData("Select Amount From [DBO].[ACX_UDF_GETPRICE](GETDATE(),'" + StatePriceGroup.ToString() + "','" + DDLProductDesc.SelectedValue.ToString() + "')");
                    if (dtPrice.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtPrice.Rows[0]["Amount"]) < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                            return Session["LineItem"] as DataTable;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: Price not defined !');", true);
                        return Session["LineItem"] as DataTable;
                    }

                    strPrice = Math.Round(Convert.ToDecimal(dtPrice.Rows[0]["Amount"]),2);
                    strBasicValue = Math.Round((Convert.ToDecimal(txtAdjQty.Text.Trim()) * Convert.ToDecimal(strPrice)),2);

                    string strUOM = string.Empty;
                    if (calValue[4] != null)
                    {
                        strUOM = calValue[4].ToString();
                    }

                    string srtTaxCode = " Select  H.TaxValue,H.ACXADDITIONALTAXVALUE, (case when H.ACXADDITIONALTAXBASIS=0 then (H.TaxValue+H.ACXADDITIONALTAXVALUE) else H.TaxValue+(H.TaxValue*H.ACXADDITIONALTAXVALUE/100) end ) as TaxPer  from  [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup  where H.ItemId='" + DDLProductDesc.SelectedValue.ToString() + "' and G.Siteid='" + Session["SITECODE"].ToString() + "'";

                    DataTable dtTax = new DataTable();

                    dtTax = baseObj.GetData(srtTaxCode);
                    if (dtTax.Rows.Count != 0)
                    {
                        taxCode = Math.Round(Convert.ToDecimal(dtTax.Rows[0]["TaxPer"].ToString()),2);
                    }
                    else
                    {
                        taxCode = 0;
                    }
                    if (Session["LineItem"] == null)
                    {
                        #region Runtime Datatable for Line Items
                        DataColumn column = new DataColumn();

                        column.DataType = System.Type.GetType("System.Int32");
                        column.AutoIncrement = true;
                        column.AutoIncrementSeed = 1;
                        column.AutoIncrementStep = 1;
                        column.ColumnName = "SNO";
                        //-----------------------------------------------------------//
                        dtLineItems = new DataTable("LineItemTable");
                        dtLineItems.Columns.Add(column);
                        dtLineItems.Columns.Add("ProductGroup", typeof(string));
                        dtLineItems.Columns.Add("ProductSubCategory", typeof(string));
                        dtLineItems.Columns.Add("ProductDesc", typeof(string));
                        dtLineItems.Columns.Add("UOM", typeof(string));
                        dtLineItems.Columns.Add("Reason", typeof(string));
                        dtLineItems.Columns.Add("AdjustmentValue", typeof(decimal));//BOX Value
                        //--------------------NEW-------------------------------
                        dtLineItems.Columns.Add("Sale_InvoiceNo", typeof(string));
                        dtLineItems.Columns.Add("PRODUCT_CODE", typeof(string));
                        dtLineItems.Columns.Add("PreBoxQty", typeof(decimal));
                        dtLineItems.Columns.Add("PreRate", typeof(decimal));
                        dtLineItems.Columns.Add("PreTax", typeof(decimal));
                        dtLineItems.Columns.Add("Amount", typeof(decimal));
                        dtLineItems.Columns.Add("PreBasicValue", typeof(decimal));
                        dtLineItems.Columns.Add("CreditDebitValue", typeof(decimal));
                        dtLineItems.Columns.Add("PURCH_RECIEPTNO", typeof(string));
                        dtLineItems.Columns.Add("ReasonName", typeof(string));
                        #endregion
                    }
                    else
                    {
                        dtLineItems = (DataTable)Session["LineItem"];
                    }
                    DataRow row;
                    int rowlineStatus=0; // 0 for as per GRN Line (negative), 1 for Price/Tax Adjustment (Actual)
                    while (rowlineStatus <= 1)
                    {
                        row = dtLineItems.NewRow();
                        row["ProductGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                        row["ProductSubCategory"] = DDLProdSubCategory.SelectedItem.Text.ToString();
                        row["ProductDesc"] = DDLProductDesc.SelectedItem.Text.ToString();
                        row["UOM"] = DDLEntryType.SelectedItem.Text.ToString();
                        row["Reason"] = DDLReason.SelectedValue.ToString();
                        row["AdjustmentValue"] = Convert.ToDecimal(txtAdjQty.Text).ToString("0.00");
                        //-------NEW-----------------------
                        row["Sale_InvoiceNo"] = ddlInvoiceNo.SelectedValue.ToString();
                        string strPURCH_RECIEPTNO = ddlInvoiceNo.SelectedItem.Text.ToString();
                        string[] words = strPURCH_RECIEPTNO.Split('(');
                        foreach (string word in words)
                        {
                            Console.WriteLine(word);
                        }
                        row["PURCH_RECIEPTNO"] = words[0].ToString();
                        row["ReasonName"] = DDLReason.SelectedItem.Text.ToString();
                        row["PRODUCT_CODE"] = DDLProductDesc.SelectedValue.ToString();
                        if (rowlineStatus == 1) // Insert row for Price/Tax diff
                        {
                            row["AdjustmentValue"] = Convert.ToDecimal(txtAdjQty.Text).ToString("0.00");
                            row["PreBoxQty"] = 0;//Convert.ToDecimal(txtAdjQty.Text.Trim());
                            row["PreRate"] = strPrice;
                            row["PreTax"] = taxCode;
                            row["Amount"] = 0;//(Convert.ToDecimal(txtAdjQty.Text.Trim()) * strPrice + (Convert.ToDecimal(txtAdjQty.Text.Trim()) * strPrice * taxCode) / 100);
                            row["PreBasicValue"] = 0;//Convert.ToDecimal(txtAdjQty.Text.Trim()) * strPrice;
                            Decimal strRate = Convert.ToDecimal(txtAdjQty.Text.ToString()) * Convert.ToDecimal(strPrice);
                            row["CreditDebitValue"] = Convert.ToDecimal(strRate + (((strRate * Convert.ToDecimal(taxCode)) / 100))).ToString("0.00");
                        }
                        else
                        {

                            row["AdjustmentValue"] = (Convert.ToDecimal(txtAdjQty.Text)*-1).ToString("0.00");
                            if (lblPreBoxQty.Text.Trim() != string.Empty)
                            {
                                row["PreBoxQty"] = Convert.ToDecimal(lblPreBoxQty.Text).ToString("0.00");
                            }
                            else
                            {
                                row["PreBoxQty"] = 0;
                            }
                            if (lblRate.Text.Trim() != string.Empty)
                            {
                                row["PreRate"] = Convert.ToDecimal(lblRate.Text.ToString()).ToString("0.00");
                            }
                            else
                            {
                                row["PreRate"] = 0;
                            }
                            if (lblTaxPer.Text.Trim() != string.Empty)
                            {
                                row["PreTax"] = Convert.ToDecimal(lblTaxPer.Text.ToString()).ToString("0.00");
                            }
                            else
                            {
                                row["PreTax"] = 0;
                            }
                            if (lblAmount.Text.Trim() != string.Empty)
                            {
                                row["Amount"] = Convert.ToDecimal(lblAmount.Text.ToString()).ToString("0.00");
                            }
                            else
                            {
                                row["Amount"] = 0;
                            }
                            if (lblBasicValue.Text.Trim() != string.Empty)
                            {
                                row["PreBasicValue"] = Convert.ToDecimal(lblBasicValue.Text.ToString()).ToString("0.00");
                            }
                            else
                            {
                                row["PreBasicValue"] = 0;
                            }
                            Decimal strRate = Convert.ToDecimal(txtAdjQty.Text.ToString())*-1* Convert.ToDecimal(lblRate.Text.ToString());
                            row["CreditDebitValue"] = Convert.ToDecimal(strRate + (((strRate * Convert.ToDecimal(lblTaxPer.Text.Trim())) / 100))).ToString("0.00");
                        }
                        dtLineItems.Rows.Add(row);
                        rowlineStatus += 1;
                    }
                }
                //Update session table
                Session["LineItem"] = dtLineItems;
                DDLEntryType.SelectedIndex = 0;
                txtAdjQty.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            try{
            bool valid = ValidateLineItemAdd();
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                
                if (DDLProductDesc.Text == "-Select-")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product First !');", true);
                    DDLProductDesc.Focus();
                    return;
                }              
                string strProductId = DDLProductDesc.SelectedValue.ToString();
                decimal strAdjQty = 0;
                    try
                    {
                        strAdjQty = Convert.ToDecimal(txtAdjQty.Text.Trim());
                        if (strAdjQty < 0)
                        {
                            string query1 = " Select ITEMID from ax.INVENTTABLE where BLOCK=0 AND" +
                                  " ax.INVENTTABLE.ITEMID in (select PRODUCT_CODE from ax.[ACXPURCHINVRECIEPTLINE] where PRODUCT_CODE='" + DDLProductDesc.SelectedValue.ToString() + "' and PURCH_RECIEPTNO ='" + ddlInvoiceNo.SelectedValue.ToString() + "' )" +
                                  "and  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLProdSubCategory.SelectedItem.Text.ToString() + "' ";
                            DataTable dt1 = new DataTable();
                            dt1 = obj.GetData(query1);
                            if (dt1.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Item not exist in seleted invoice. Please enter valid Qty!');", true);
                                txtAdjQty.Focus();
                                return;
                            }
                        }
                        if (DDLReason.SelectedItem.Text.Contains("Shortage"))
                        {
                            if (strAdjQty > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Shortage always enter less then zero!. Please enter valid Qty!');", true);
                                txtAdjQty.Focus();
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please enter Valid Qty !');", true);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        return;
                    }
                if (DDLReason.SelectedItem.Text.Contains("PRICE") || DDLReason.SelectedItem.Text.Contains("TAX"))
                {
                    dt = AddLineItemsForPrice_Tax_Mismatch();
                }
                else
                {
                    dt = AddLineItems();
                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        gridAdjusment.DataSource = dt;
                        gridAdjusment.DataBind();
                        gridAdjusment.Visible = true;
                    }
                    else
                    {
                        gridAdjusment.DataSource = dt;
                        gridAdjusment.DataBind();
                        gridAdjusment.Visible = false;
                    }
                }
            }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }                
        }

        private bool ValidateAdjustmentSave()
        {
            bool _value = false;
            
            if (gridAdjusment.Rows.Count <= 0 && Session["ItemTable"] == null)
            {
                _value = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Add Items for Adjustment Entry !');", true);
            }
            if (ddlInvoiceNo.SelectedItem.Text == string.Empty)
            {
                _value = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Provide the Invoice Number !');", true);
                ddlInvoiceNo.Focus();
            }
            else
            {
                _value = true;
            }
            return _value;
        }

        private void SaveAdjustmentToTrans()
        {
            try
            {
                //validate stock changed by amol 03-Feb-2017
                Dictionary<string, decimal> distinctProduct_Qty = new Dictionary<string, decimal>();
                for (int i = 0; i < gridAdjusment.Rows.Count; i++)
                {
                    string productNameCode = gridAdjusment.Rows[i].Cells[7].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gridAdjusment.Rows[i].Cells[15].Text);
                    if (distinctProduct_Qty.ContainsKey(ProductCode))
                    {
                        decimal value = 0;
                        value = distinctProduct_Qty[ProductCode];
                        distinctProduct_Qty.Remove(ProductCode);
                        distinctProduct_Qty.Add(ProductCode, value + TransQty);
                    }
                    else
                    {
                        distinctProduct_Qty.Add(ProductCode, TransQty);
                    }

                }

                foreach (KeyValuePair<string, decimal> keyValue in distinctProduct_Qty)
                {
                    bool _stockcheck = ValidateStock(keyValue.Key, keyValue.Value);
                    if (!_stockcheck)
                    {
                        return;
                    }
                }

                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";
                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }
                DataTable dtNumSeq = obj.GetNumSequenceNew(15, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                string NUMSEQ = string.Empty;
                string TransId = string.Empty;
                if (dtNumSeq != null && dtNumSeq.Rows.Count > 0)
                {
                    TransId = dtNumSeq.Rows[0][0].ToString();
                    NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }

                string queryInsert = " Insert Into ax.acxinventTrans " +
                                 "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                 "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],[NUMSEQ],[TransLineNo])" +
                                 " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType, " +
                                 " @DocumentNo,@DocumentDate,@ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno,@NUMSEQ,@TransLineNo)";

                string queryInsert1 = "Insert Into [ax].ACXADJUSTMENTENTRYHEADER " +
                                "([DOCUMENT_NO],[DOCUMENT_DATE],[SITE_CODE],[PURCH_RECIEPTNO],[PURCH_INVOICENO],[BASIC_AMOUNT],[ADJ_AMOUNT]," +
                                "[CREATEDDATETIME],[NUMSEQ],[SUPPLIERCODE],[RECID],[DATAAREAID])" +
                                " Values (@DOCUMENT_NO,@DOCUMENT_DATE,@SITE_CODE,@PURCH_RECIEPTNO,@PURCH_INVOICENO,@BASIC_AMOUNT,@ADJ_AMOUNT, " +
                                " @CREATEDDATETIME,@NUMSEQ,@SUPPLIERCODE,@RECID,@DATAAREAID)";

                string queryInsert2 = "Insert Into [ax].[ACXADJUSTMENTENTRYLINE] " +
                             "([DOCUMENT_NO],[DATAAREAID],[RECID]," +
                             "[LINE_NO],[PRODUCT_CODE],[RATE],[UOM],[BASICVALUE],[AMOUNT],[GRNQTY],[ADJQTY],[TAX],[TAXAMOUNT],[ADJVALUE],[SITEID]," +
                            " [TRANSACTIONID],[TRANSDATE],[Remark],[DAMAGEREASON])" +
                             " Values (@DOCUMENT_NO,@DATAAREAID,@RECID,@LINE_NO,@PRODUCT_CODE,@RATE,@UOM,@BASICVALUE,@AMOUNT,@GRNQTY,@ADJQTY,@TAX," +
                             " @TAXAMOUNT,@ADJVALUE,@SITEID,@TRANSACTIONID,@TRANSDATE,@Remark,@DAMAGEREASON)";

                string queryInsert3 = "update [ax].[ACXVENDORTRANS] SET RemainingAmount=RemainingAmount+@ADJ_AMOUNT " +
                                "where Document_No='" + ddlInvoiceNo.SelectedValue.ToString() + "' and SiteCode='" + Session["SiteCode"].ToString() + "' and DocumentType=1";


                string queryLineRecID = "Select Count(RECID) as RECID from [ax].[ACXADJUSTMENTENTRYLINE]";
                Int64 recid = Convert.ToInt64(obj.GetScalarValue(queryLineRecID));

                decimal strBASIC_AMOUNT = 0;
                decimal strADJ_AMOUNT = 0;

                conn = obj.GetConnection();
                cmd = new SqlCommand(queryInsert);
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                cmd1 = new SqlCommand(queryInsert1);

                if (transaction == null)
                {
                    transaction = conn.BeginTransaction();
                }
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.Text;


                cmd2 = new SqlCommand(queryInsert2);
                cmd2.Connection = conn;
                if (transaction == null)
                {
                    transaction = conn.BeginTransaction();
                }
                cmd2.Transaction = transaction;
                cmd2.CommandTimeout = 3600;
                cmd2.CommandType = CommandType.Text;


                cmd3 = new SqlCommand(queryInsert3);
                cmd3.Connection = conn;
                if (transaction == null)
                {
                    transaction = conn.BeginTransaction();
                }
                cmd3.Transaction = transaction;
                cmd3.CommandTimeout = 3600;
                cmd3.CommandType = CommandType.Text;

                string Siteid = Session["SiteCode"].ToString();
                string st = Session["SiteCode"].ToString();
                for (int i = 0; i < gridAdjusment.Rows.Count; i++)
                {

                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 3;//1 for purchase invoice receipt
                    int DocumentType = 3;
                    //Temporary Number Insert//
                    string productNameCode = gridAdjusment.Rows[i].Cells[7].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gridAdjusment.Rows[i].Cells[15].Text);
                    string uom = gridAdjusment.Rows[i].Cells[8].Text;
                    //string TransLocation = st.Substring(st.Length - 6);
                    string Referencedocumentno = gridAdjusment.Rows[i].Cells[5].Text;
                    int REcid = i + 1;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TransId", TransId);
                    cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                    cmd.Parameters.AddWithValue("@RECID", i + 1);
                    cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TransType", TransType);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    cmd.Parameters.AddWithValue("@DocumentNo", dtNumSeq.Rows[0]["NEXTNUMBER"]);
                    cmd.Parameters.AddWithValue("@DocumentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                    cmd.Parameters.AddWithValue("@TransQty", TransQty);
                    cmd.Parameters.AddWithValue("@TransUOM", uom);
                    cmd.Parameters.AddWithValue("@TransLocation", TransLocation);
                    cmd.Parameters.AddWithValue("@Referencedocumentno", ddlInvoiceNo.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                    cmd.Parameters.AddWithValue("@TransLineNo", i + 1);
                    cmd.ExecuteNonQuery();
                    //-----Line---
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@DOCUMENT_NO", TransId);
                    cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd2.Parameters.AddWithValue("@RECID", recid + 1);
                    cmd2.Parameters.AddWithValue("@LINE_NO", i + 1);
                    cmd2.Parameters.AddWithValue("@PRODUCT_CODE", ProductCode);
                    cmd2.Parameters.AddWithValue("@RATE", gridAdjusment.Rows[i].Cells[11].Text);
                    cmd2.Parameters.AddWithValue("@UOM", uom);
                    strBASIC_AMOUNT = strBASIC_AMOUNT + Convert.ToDecimal(TransQty * Convert.ToDecimal(gridAdjusment.Rows[i].Cells[11].Text));
                    strADJ_AMOUNT = strADJ_AMOUNT + Convert.ToDecimal(gridAdjusment.Rows[i].Cells[16].Text);
                    Decimal strTaxAmount = (((TransQty * Convert.ToDecimal(gridAdjusment.Rows[i].Cells[11].Text) * Convert.ToDecimal(gridAdjusment.Rows[i].Cells[12].Text))) / 100);
                    Decimal strBasicValue = Convert.ToDecimal(TransQty * Convert.ToDecimal(gridAdjusment.Rows[i].Cells[11].Text));
                    string strDamagedesc = Convert.ToString(gridAdjusment.Rows[i].Cells[9].Text);
                    strDamagedesc = strDamagedesc.Substring(0, strDamagedesc.IndexOf("-")).Trim();
                    cmd2.Parameters.AddWithValue("@BASICVALUE", strBasicValue);
                    cmd2.Parameters.AddWithValue("@AMOUNT", gridAdjusment.Rows[i].Cells[14].Text);
                    cmd2.Parameters.AddWithValue("@GRNQTY", gridAdjusment.Rows[i].Cells[10].Text);
                    cmd2.Parameters.AddWithValue("@ADJQTY", TransQty);
                    cmd2.Parameters.AddWithValue("@TAX", gridAdjusment.Rows[i].Cells[12].Text);
                    cmd2.Parameters.AddWithValue("@TAXAMOUNT", strTaxAmount);
                    cmd2.Parameters.AddWithValue("@ADJVALUE", gridAdjusment.Rows[i].Cells[16].Text);
                    cmd2.Parameters.AddWithValue("@SITEID", Siteid);
                    cmd2.Parameters.AddWithValue("@TRANSACTIONID", TransId);
                    cmd2.Parameters.AddWithValue("@TRANSDATE", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@Remark", "");
                    cmd2.Parameters.AddWithValue("@DAMAGEREASON", strDamagedesc);
                    cmd2.ExecuteNonQuery();
                    ViewState["AdjustmentNo"] = TransId;
                }

                string strPURCH_RECIEPTNO = ddlInvoiceNo.SelectedItem.Text.ToString();
                string[] words = strPURCH_RECIEPTNO.Split('(');
                string[] words1 = words[1].Split(')');

                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@DOCUMENT_NO", TransId);
                cmd1.Parameters.AddWithValue("@DOCUMENT_DATE", DateTime.Now);
                cmd1.Parameters.AddWithValue("@SITE_CODE", Siteid);
                cmd1.Parameters.AddWithValue("@PURCH_RECIEPTNO", ddlInvoiceNo.SelectedValue.ToString());
                cmd1.Parameters.AddWithValue("@PURCH_INVOICENO", words1[0].ToString());
                cmd1.Parameters.AddWithValue("@BASIC_AMOUNT", strBASIC_AMOUNT);
                cmd1.Parameters.AddWithValue("@ADJ_AMOUNT", strADJ_AMOUNT);
                cmd1.Parameters.AddWithValue("@CREATEDDATETIME", DateTime.Now);
                cmd1.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                cmd1.Parameters.AddWithValue("@SUPPLIERCODE", lblSupplierName.Text.Trim());
                cmd1.Parameters.AddWithValue("@RECID", recid + 1);
                cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd1.ExecuteNonQuery();

                cmd3.Parameters.Clear();
                cmd3.Parameters.AddWithValue("@ADJ_AMOUNT", strADJ_AMOUNT);
                cmd3.ExecuteNonQuery();
                //obj.UpdateLastNumSequence(9, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);

                transaction.Commit();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Adjustment Number : " + TransId + " Generated Successfully.!');", true);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "alert('Adjustment Number : " + TransId + " Generated Successfully.!');", true);
                resetPage();
                LblMessage.Text = "Adjustment entry saved successfully. Generated adjustment entry no is " + TransId;

                cmd.Dispose();
                conn.Close();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }                
        }

        private void resetPage()
        {
            ddlInvoiceNo.SelectedIndex = 0;
            FillMaterialGroup();
            FillReturnReasonType();
            txtAdjQty.Text = string.Empty;
            DDLProductGroup.SelectedIndex = 0;
            DDLProdSubCategory.Items.Clear();
            DDLProductDesc.Items.Clear();

            lblInvoiceDate.Text = string.Empty;
            lblInvoiceValue.Text = string.Empty;
            lblSupplierName.Text = string.Empty;
            lblRate.Text = string.Empty;
            lblTaxPer.Text = string.Empty;
            lblAmount.Text = string.Empty;
            lblBasicValue.Text = string.Empty;
            lblPreBoxQty.Text = string.Empty;
            lblProductName.Text = string.Empty;
            DataTable dtBlank = new DataTable();
            gridAdjusment.DataSource = dtBlank;
            gridAdjusment.DataBind();
            DDLReason.SelectedIndex = 0;
            ddlInvoiceNo.SelectedIndex = 0;

            Session["ItemTable"] = null;
            Session["LineItem"] = null;
            gridAdjusment.DataSource = null;
            gridAdjusment.Visible = false;
            // txtAdjustmentRefNo.Text = string.Empty;
            ViewState["AdjustmentNo"] = null;
            LblMessage.Text = string.Empty;
            //txtSearchAdjustmentNo.Text = string.Empty;
        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            try{
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            HiddenField hiddenField = (HiddenField)gvrow.FindControl("HiddenField2");
            LinkButton lnk = sender as LinkButton;

            if (Session["LineItem"] != null)
            {
                DataTable dt = Session["LineItem"] as DataTable;
                dt.AsEnumerable().Where(r => r.Field<int>("SNO") == Convert.ToInt32(hiddenField.Value)).ToList().ForEach(row => row.Delete());
                gridAdjusment.DataSource = dt;
                gridAdjusment.DataBind();
                Session["LineItem"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {

                }
                else
                {
                    DDLProductDesc.SelectedIndex = 0;
                    DDLProductDesc_SelectedIndexChanged(null, null);
                }
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BtnSaveAdjustment_Click1(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateAdjustmentSave();
                if (b == true)
                {
                    SaveAdjustmentToTrans();
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateStock(string ProductCode, decimal AdjusmentValue)
        {
            bool stock = true;
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                string queryStock = " Select ISNULL(SUM(INVTS.TRANSQTY),0) AS STOCK From AX.ACXINVENTTRANS INVTS INNER JOIN AX.INVENTTABLE INV  ON INV.ITEMID= INVTS.PRODUCTCODE " +
                               " WHERE INVTS.DATAAREAID='" + Session["DATAAREAID"] + "' AND INV.BLOCK=0 AND INVTS.SiteCode='" + Session["SiteCode"] + "' AND INVTS.TRANSLOCATION='" + Session["TransLocation"].ToString() + "' " +
                               " AND INVTS.PRODUCTCODE ='" + ProductCode + "'";

                object stockValue = string.Empty;
                stockValue = obj.GetScalarValue(queryStock);
                if (stockValue == null || stockValue.ToString() == string.Empty)
                {
                    stock = false;
                }
                if (stockValue != null && stockValue.ToString() != string.Empty)
                {
                    decimal AvailableStock = Math.Round(Convert.ToDecimal(stockValue.ToString()), 2);
                    //if (AdjusmentValue <= AvailableStock)
                    //{
                    if (AdjusmentValue + AvailableStock < 0)
                    {
                        stock = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Stock Negative Issue  :-Product Code: " + ProductCode + "  Final Stock cannot be in negative figure. !');", true);
                    }
                    else
                    {
                        stock = true;
                    }

                    //}
                    //else
                    //{
                    //stock = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Stock Issue : Entered Value cannot be more than the available Stock !');", true);
                    //}


                    //if (AdjusmentValue <= AvailableStock)
                    //{
                    //    stock = true;
                    //}
                    //else
                    //{
                    //    stock = false;
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Stock Issue : Entered Value cannot be more than the available Stock !');", true);
                    //}
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return stock;
        }

        //protected void BtnSearch_Click(object sender, EventArgs e)
        //{
        //    bool b = validateSearch();
        //    if (b == true)
        //    {
        //        ShowAdjusmentEntryDetails(txtSearchAdjustmentNo.Text.Trim().ToString());
        //    }
        //}

        //protected void DDlSearchType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (DDlSearchType.SelectedItem.Text.ToString() == "Adjustment No")
        //    {
        //        imgBtDate.Visible = false;
        //    }
        //    //if (DDlSearchType.SelectedItem.Text.ToString() == "Date")
        //    //{
        //    //    imgBtDate.Visible = true;
        //    //}

        //}

        //private bool validateSearch()
        //{
        //    bool search = false;
        //    if (txtSearchAdjustmentNo.Text == string.Empty)
        //    {
        //        search = false;
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Provide Search Text !');", true);
        //    }
        //    else
        //    {
        //        search = true;
        //    }
        //    return search;
        //}

        private void ShowAdjusmentEntryDetails(string Value)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string querySearch = string.Empty;

                querySearch = "Select A.RECID as SNO, INV.PRODUCT_GROUP as ProductGroup,INV.PRODUCT_SUBCATEGORY as ProductSubCategory, " +
                                     "  A.ProductCode + '-' + '('+INV.PRODUCT_NAME+')' AS ProductDesc,  A.TransUOM as UOM , CAST(a.TransQty as decimal(16,2)) as AdjustmentValue, " +
                                     " A.ReferenceDocumentNo as Reason , A.TransLocation  from ax.acxinventtrans A  INNER JOIN AX.INVENTTABLE INV ON INV.ITEMID= A.ProductCode " +
                                     "  where A.DocumentNo='" + Value + "' and A.SiteCode='" + Session["SiteCode"].ToString() + "'";

                DataTable dt = obj.GetData(querySearch);
                if (dt.Rows.Count > 0)
                {
                    gridAdjusment.DataSource = dt;
                    gridAdjusment.DataBind();
                    panelAdjustmentEntry.Enabled = false;
                    PanelGrid.Enabled = false;
                    gridAdjusment.Visible = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Records Not Found!');", true);
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            panelAdjustmentEntry.Enabled = true;
            PanelGrid.Enabled = true;
            // txtSearchAdjustmentNo.Text = string.Empty;
            resetPage();
            lblInvoiceDate.Text = string.Empty;
            lblInvoiceValue.Text = string.Empty;
            lblSupplierName.Text = string.Empty;
            lblRate.Text = string.Empty;
            lblTaxPer.Text = string.Empty;
            lblAmount.Text = string.Empty;
            lblBasicValue.Text = string.Empty;
            lblPreBoxQty.Text = string.Empty;
            lblProductName.Text = string.Empty;
            DataTable dt = new DataTable();
            gridAdjusment.DataSource = dt;
            gridAdjusment.DataBind();

        }

        protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInvoiceDate.Text = string.Empty;
            lblInvoiceValue.Text = string.Empty;
            lblSupplierName.Text = string.Empty;
            lblRate.Text = string.Empty;
            lblTaxPer.Text = string.Empty;
            lblAmount.Text = string.Empty;
            lblBasicValue.Text = string.Empty;
            lblPreBoxQty.Text = string.Empty;
            lblProductName.Text = string.Empty;
            DataTable dtBlank = new DataTable();
            gridAdjusment.DataSource = dtBlank;
            gridAdjusment.DataBind();
            DDLReason.SelectedIndex = 0;
            LblMessage.Text = string.Empty;
            
            try{
            if (ddlInvoiceNo.SelectedItem.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string querySearch = string.Empty;
                querySearch = "select Document_Date,cast(Material_Value as decimal(18,4)) as Material_Value, (CASE   WHEN Supplier_Code = '' or Supplier_Code is null THEN PLANT_NAME  WHEN PLANT_NAME = '' or PLANT_NAME is null THEN  (select top 1 Name from [ax].[INVENTSITE] where Siteid=Supplier_Code )  ELSE 'z'" +
                          "END) as Supplier_Code from [ax].[ACXPURCHINVRECIEPTHEADER] where DOCUMENT_NO='" + ddlInvoiceNo.SelectedValue.ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";

                DataTable dt = obj.GetData(querySearch);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblInvoiceDate.Text = Convert.ToDateTime(dt.Rows[0]["Document_Date"]).ToString("dd-MMM-yyyy");
                    lblInvoiceValue.Text = Convert.ToDecimal(dt.Rows[0]["Material_Value"]).ToString("0.00");
                    lblSupplierName.Text = dt.Rows[0]["Supplier_Code"].ToString();
                }
                DDLReason.Focus();
                FillMaterialGroup();
                DDLProdSubCategory.Items.Clear();
                DDLProdSubCategory.Items.Add("-Select-");
                DDLProductDesc.Items.Clear();
                DDLProductDesc.Items.Add("-Select-");
            }
            else
            {
                FillMaterialGroup();
                DDLProdSubCategory.Items.Clear();
                DDLProdSubCategory.Items.Add("-Select-");
                DDLProductDesc.Items.Clear();
                DDLProductDesc.Items.Add("-Select-");
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DDLProductDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt;

            LblMessage.Text = string.Empty;
            lblRate.Text = string.Empty;
            lblTaxPer.Text = string.Empty;
            //lblAddTex.Text = string.Empty;
            lblBasicValue.Text = string.Empty;
            lblPreBoxQty.Text = string.Empty;
            lblProductName.Text = string.Empty;
            lblAmount.Text = string.Empty;
            if (DDLProductDesc.SelectedItem.Text != "-Select-")
            {
                try
                {
                    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                    string querySearch = string.Empty;
                    querySearch = "select PRODUCT_CODE, cast(Box as decimal(18,4)) Box, cast(Rate as decimal(18,4)) as Rate, cast(VAT_INC_PERC as decimal(18,4)) as Tax, cast(AMOUNT as decimal(18,4)) as AMOUNT, cast(BasicValue as decimal(18,4)) as BasicValue from ax.[ACXPURCHINVRECIEPTLINE] inner join [ax].[ACXPURCHINVRECIEPTHEADER] on [ax].[ACXPURCHINVRECIEPTHEADER].purch_recieptno=[ax].[ACXPURCHINVRECIEPTLINE].purch_recieptno where [ax].[ACXPURCHINVRECIEPTLINE].PURCH_RECIEPTNO='" + ddlInvoiceNo.SelectedValue.ToString() + "' and [ax].[ACXPURCHINVRECIEPTLINE].PRODUCT_CODE='" + DDLProductDesc.SelectedValue.ToString() + "'  and [ax].[ACXPURCHINVRECIEPTHEADER].Site_Code='" + Session["SiteCode"].ToString() + "'";
                    dt = obj.GetData(querySearch);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        lblPreBoxQty.Text = Convert.ToDecimal(dt.Rows[0]["Box"]).ToString("0.00");
                        lblRate.Text = Convert.ToDecimal(dt.Rows[0]["Rate"]).ToString("0.00");
                        lblTaxPer.Text = Convert.ToDecimal(dt.Rows[0]["Tax"]).ToString("0.00");
                        // lblAddTex.Text = dt.Rows[0]["ADD_TAX_PERC"].ToString();
                        lblBasicValue.Text = Convert.ToDecimal(dt.Rows[0]["BasicValue"]).ToString("0.00");
                        lblAmount.Text = Convert.ToDecimal(dt.Rows[0]["AMOUNT"]).ToString("0.00");
                        lblProductName.Text = DDLProductDesc.SelectedItem.Text.ToString();
                    }
                    //else
                    //{
                    //    querySearch = "EXEC AX_USP_GetItemTaxRate '" + Session["SiteCode"].ToString() + "','" + DDLProductDesc.SelectedValue.ToString() + "'";
                    //    dt = obj.GetData(querySearch);
                    //    if (dt != null && dt.Rows.Count > 0)
                    //    {
                    //        lblPreBoxQty.Text = "0";
                    //        lblRate.Text = Convert.ToDecimal(dt.Rows[0]["Rate"]).ToString("0.00");
                    //        lblTaxPer.Text = Convert.ToDecimal(dt.Rows[0]["Tax"]).ToString("0.00");
                    //        lblProductName.Text = DDLProductDesc.SelectedItem.Text.ToString();
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            else
            {

            }
        }

        protected void DDLReason_SelectedIndexChanged(object sender, EventArgs e)
        {

            FillMaterialGroup();
        }
    }
}