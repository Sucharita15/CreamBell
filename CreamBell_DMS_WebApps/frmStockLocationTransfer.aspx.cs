using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Elmah;

namespace CreamBell_DMS_WebApps
{

    public partial class frmStockLocationTransfer : System.Web.UI.Page
    {
        public DataTable dtLineItems;
        public SqlCommand cmd;
        public SqlTransaction transaction;
        public SqlConnection conn;
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                //CalendarExtender1.StartDate = DateTime.Now;
                //CalendarExtender2.StartDate = DateTime.Now;

                string datestring = DateTime.Now.ToString("yyyy-MM-dd");
                txtDate.Text = datestring;
                this.txtStockMoveQty.Attributes.Add("onkeypress", "button_click(this,'" + this.BtnAdd.ClientID + "')");
                FillReturnReasonType();
                FillBU();
                LoadWarehouse();
                ProductGroup();
                ProductSubCategory();
                FillProductCode();
                Session["LineItem"] = null;
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetProductDescription(string prefixText)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string query = "Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where " +
                           "replace(replace(ITEMID, char(9), ''), char(13) + char(10), '') Like @ProductCode+'%'";
            
            SqlConnection conn = obj.GetConnection();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductCode", prefixText);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<string> ProductDetails = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProductDetails.Add(dt.Rows[i]["ITEMID"].ToString());
            }
            return ProductDetails;
        }


        public void FillBU()
        {
            DDLBusinessUnit.Items.Clear();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            DDLBusinessUnit.Items.Add("All...");
            baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
        }
        public void ProductGroup()
        {
            if (DDLBusinessUnit.SelectedItem.Text == "All...")
            {
                DDLProductGroup.Items.Clear();
                string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' and A.block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
                //if (rdStock.Checked == true)
                //{
                //    strProductGroup += " AND a.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                //}
                strProductGroup += " order by a.Product_Group";
                DDLProductGroup.Items.Clear();
                DDLProductGroup.Items.Add("Select...");
                baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

            }
            else
            {
                DDLProductGroup.Items.Clear();
                string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  and A.block=0 and BU_CODE in ('" + DDLBusinessUnit.SelectedItem.Value.ToString() + "') ";
                //if (rdStock.Checked == true)
                //{
                //    strProductGroup += " AND a.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                //}
                DDLProductGroup.Items.Clear();
                strProductGroup += " order by a.Product_Group";
                DDLProductGroup.Items.Add("Select...");
                baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");
            }
            //string BU;
            //if (DDLBusinessUnit.SelectedIndex == 0)
            //{
            //    BU = "";
            //}
            //else
            //{
            //    BU = DDLBusinessUnit.SelectedValue.ToString();
            //}
            //string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where BU_CODE like CASE WHEN LEN('"+ BU +"')>0 THEN '"+ BU +"' ELSE '%' END and a.Product_Group <>''";
            //strProductGroup += " order by a.Product_Group";
            //DDLProductGroup.Items.Clear();
            //DDLProductGroup.Items.Add("Select...");
            //baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        }
        public void ProductSubCategory()
        {
            string strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'";
            DDLProdSubCategory.Items.Clear();
            DDLProdSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProdSubCategory, strQuery, "Name", "Code");
        }

        public void Reset()
        {
            DDLBusinessUnit.SelectedIndex = 0;
            DDLProductGroup.SelectedIndex = 0;
            DDLProdSubCategory.SelectedIndex = 0;
            DDLProductDesc.SelectedIndex = 0;
            txtstock.Text = "0.00";
        }

        public void FillProductCode()
        {
            string strQuery = string.Empty;
            if (DDLBusinessUnit.SelectedItem.Text == "All...")
            {
                DDLProductDesc.Items.Clear();
                // DDLMaterialCode.Items.Add("Select...");
                if (DDLProductGroup.Text == "Select..." && DDLProdSubCategory.Text == "Select..." || DDLProdSubCategory.Text == "")
                {
                    strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
                    //if (rdStock.Checked == true)
                    //{
                    //    strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                    //}
                    strQuery += "  order by Product_Name";
                    DDLProductDesc.Items.Clear();
                    DDLProductDesc.Items.Add("Select...");
                    //DataTable dt = baseObj.GetData(strQuery);

                    baseObj.BindToDropDown(DDLProductDesc, strQuery, "Product_Name", "Product_Code");
                    DDLProductDesc.Focus();
                }
            }
            else
            {
                DDLProductDesc.Items.Clear();
                // DDLMaterialCode.Items.Add("Select...");
                if (DDLProductGroup.Text == "Select..." && DDLProdSubCategory.Text == "Select..." || DDLProdSubCategory.Text == "")
                {
                    strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0"
                        + " and BU_CODE in('" + DDLBusinessUnit.SelectedItem.Value.ToString() + "')";
                    //if (rdStock.Checked == true)
                    //{
                    //    strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                    //}
                    strQuery += " order by Product_Name";
                    DDLProductDesc.Items.Clear();
                    DDLProductDesc.Items.Add("Select...");
                    //DataTable dt = baseObj.GetData(strQuery);

                    baseObj.BindToDropDown(DDLProductDesc, strQuery, "Product_Name", "Product_Code");
                    DDLProductDesc.Focus();
                }
            }
            //DDLProductDesc.Items.Clear();
            //// DDLMaterialCode.Items.Add("Select...");
            //string BU;
            //if (DDLBusinessUnit.SelectedIndex == 0)
            //{
            //    BU = "";
            //}
            //else
            //{
            //    BU = DDLBusinessUnit.SelectedValue.ToString();
            //}
            //
            //if (DDLProductGroup.Text == "Select..." && DDLProdSubCategory.Text == "Select..." || DDLProdSubCategory.Text == "")
            //{
            //    string strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE";
            //    strQuery += " where BU_CODE LIKE CASE WHEN LEN('"+ BU +"')>0 THEN '"+ BU +"' ELSE '%' END order by Product_Name";
            //    DDLProductDesc.Items.Clear();
            //    DDLProductDesc.Items.Add("Select...");
            //    baseObj.BindToDropDown(DDLProductDesc, strQuery, "Product_Name", "Product_Code");
            //    DDLProductDesc.Focus();
            //}
        }
        protected void BtnGetProductDetails_Click(object sender, EventArgs e)
        {
            
            if (txtProductCode.Text != string.Empty)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                string queryFillProductDetails = "Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY " +
                                                 "from ax.INVENTTABLE where replace(replace(ITEMID, char(9), ''), char(13) + char(10), '')= '" + txtProductCode.Text.Trim().ToString() + "'";
                DataTable dtProductDetails = obj.GetData(queryFillProductDetails);
                if (dtProductDetails.Rows.Count > 0 && dtProductDetails.Rows.Count == 1)
                {
                    
                    DDLProductGroup.Items.Clear();
                    DDLProdSubCategory.Items.Clear();
                    DDLProductDesc.Items.Clear();

                    DDLProductGroup.Items.Add(dtProductDetails.Rows[0]["PRODUCT_GROUP"].ToString());
                    DDLProductGroup.SelectedItem.Text = dtProductDetails.Rows[0]["PRODUCT_GROUP"].ToString();

                    DDLProdSubCategory.Items.Add(dtProductDetails.Rows[0]["PRODUCT_SUBCATEGORY"].ToString());
                    DDLProdSubCategory.SelectedItem.Text = dtProductDetails.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();

                    DDLProductDesc.Items.Add(dtProductDetails.Rows[0]["PRODUCT_NAME"].ToString());
                    DDLProductDesc.SelectedItem.Text = dtProductDetails.Rows[0]["PRODUCT_NAME"].ToString();

                    
                    DDLProductGroup.Enabled = false;
                    DDLProdSubCategory.Enabled = false;
                    DDLProductDesc.Enabled = false;
                    DDLReason.Focus();
                }
                if (dtProductDetails.Rows.Count > 1)
                {
                    DDLProductGroup.Items.Clear();
                    DDLProdSubCategory.Items.Clear();
                    DDLProductDesc.Items.Clear();
                    string message = "alert('Product Code Issue: We Have Duplicate Records for this Product Code !');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product Code Issue: We Have Duplicate Records for this Product Code !');", true);
                }
                if (dtProductDetails.Rows.Count == 0)
                {
                    DDLProductGroup.Items.Clear();
                    DDLProdSubCategory.Items.Clear();
                    DDLProductDesc.Items.Clear();
                    string message = "alert('Product Code Issue: No Such Produt Code Exist !');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product Code Issue: No Such Produt Code Exist !');", true);
                }

            }
            else
            {
                txtProductCode.Focus();
                string message = "alert('Please Provide Product Code Here !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Product Code Here !');", true);
            }
        }

        private void FillReturnReasonType()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string strQuery = "Select distinct DAMAGEREASON_CODE,DAMAGEREASON_CODE + '-(' + DAMAGEREASON_NAME +')' as RETURNREASON  from [ax].[ACXDAMAGEREASON]";
            DDLReason.Items.Clear();
            DDLReason.Items.Add("-Select-");
            obj.BindToDropDown(DDLReason, strQuery, "RETURNREASON", "DAMAGEREASON_CODE");
        }

        private void LoadWarehouse()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string query1 = "SELECT INVENTLOCATIONID,NAME FROM AX.inventlocation WHERE INVENTSITEID='" + Session["SiteCode"].ToString() + "'";

            DDLWarehouseFrom.Items.Clear();
            DDLWarehouseFrom.Items.Add("-Select-");
            obj.BindToDropDown(DDLWarehouseFrom, query1, "NAME", "INVENTLOCATIONID");
            DDLWarehouseTo.Items.Clear();
            DDLWarehouseTo.Items.Add("-Select-");
            obj.BindToDropDown(DDLWarehouseTo, query1, "NAME", "INVENTLOCATIONID");
        }

        protected void BtnAdd_Click(object sender, EventArgs e)                 // To add Items in Grid After Successfull Validations//
        {
            bool valid = ValidateLineItemAdd();
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                dt = AddLineItems();
                if (dt.Rows.Count > 0)
                {
                    gridStockTransferItems.DataSource = dt;
                    gridStockTransferItems.DataBind();
                    gridStockTransferItems.Visible = true; 
                }
                else
                {
                    gridStockTransferItems.DataSource = dt;
                    gridStockTransferItems.DataBind();
                    gridStockTransferItems.Visible = false;
                }
                Reset();
            }
          //  Reset();
        }

        private bool ValidateLineItemAdd()                                     // Add Items Validation Logic //
        {
            bool b = false;
            LblProductMessage.Text = string.Empty;
            if (DDLWarehouseFrom.Text == "-Select-")
            {
                this.LblMessage.Text = "► Please Select the Warehouse From Location First !";
                DDLWarehouseFrom.Focus();
                b = false;
                return b;
            }
            if (DDLWarehouseTo.Text == "-Select-")
            {
                this.LblMessage.Text = "► Please Select the Warehouse To Location First !";
                DDLWarehouseFrom.Focus();
                b = false;
                return b;
            }
            if (DDLWarehouseTo.Text.ToString().Trim() == DDLWarehouseFrom.Text.ToString().Trim())
            {
                this.LblMessage.Text = "► Please Warehouse From and Warehouse To Location different!";
                DDLWarehouseTo.Focus();
                b = false;
                return b;
            }
            if (DDLProductGroup.Text == string.Empty)
            {
                this.LblMessage.Text = "► Product Group Not Available." + Environment.NewLine + " First Type Product Code to Get Details for Product Group !";
                txtProductCode.Focus();
                b = false;
                return b;
            }
            if (DDLProdSubCategory.Text == string.Empty)
            {
                this.LblMessage.Text = "► Product Sub Category Group Not Available For This Item !";
                DDLProdSubCategory.Focus();
                b = false;
                return b;
            }

            if (DDLProductDesc.Text == string.Empty)
            {
                this.LblMessage.Text = "► Product Description Not Available For This Item !";
                DDLProductDesc.Focus();
                b = false;
                return b;
            }
            if (DDLReason.Text == "-Select-")
            {
                this.LblMessage.Text = "► Select Reason Type !";
                DDLReason.Focus();
                b = false;
                return b;
            }

            if (txtStockMoveQty.Text == string.Empty)
            {
                this.LblMessage.Text = "► Please Provide Stock Moving Quantity Value !";
                txtStockMoveQty.Focus();
                b = false;
                return b;
            }

            if (txtStockMoveQty.Text.Length > 0 && txtStockMoveQty.Text != string.Empty)
            {
                if (Convert.ToDecimal(txtStockMoveQty.Text) == 0)
                {
                    this.LblMessage.Text = "► Stock Moving Quantity Value should be greater than zero (0)!";
                    txtStockMoveQty.Focus();
                    b = false;
                    return b;
                }
                else
                {
                    LblMessage.Text = string.Empty;
                    b = true;
                    return b;
                }
            }

            return b;
        }                                   
        private DataTable AddLineItems()                  // Create Runtime Datatable for Items and Add in Datatable and then In Grid Finally//
        {
            try
            {
                App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

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
                    dtLineItems.Columns.Add("MoveQty", typeof(decimal));

                    #endregion
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                }

                #region Check Duplicate Entry of Line Items through LINQ
                bool _exits = false;
                try
                {
                    if (dtLineItems.Rows.Count > 0)
                    {
                        var rowColl = dtLineItems.AsEnumerable();

                        var record = (from  r in rowColl
                                      where r.Field<string>("ProductGroup") == DDLProductGroup.SelectedValue &&
                                            r.Field<string>("ProductSubCategory") == DDLProdSubCategory.SelectedValue.ToString() &&
                                            r.Field<string>("ProductDesc") == DDLProductDesc.SelectedValue.ToString() && 
                                            r.Field<decimal>("MoveQty") == Convert.ToDecimal(txtStockMoveQty.Text.ToString())
                                     select r.Field<int>("SNO")).First<int>();
                        if (record >= 1)
                        {
                            _exits = true;
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: You are adding duplicate Entry !');", true);
                            LblMessage.Text = "► Error: You are adding duplicate Entry !";
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

                #endregion


                #region Stock Availability Check For That Product

                //string productNameCode = DDLProductDesc.SelectedItem.Text.ToString();
                //string[] str = productNameCode.Split('-');
                //string productCode = str[0].ToString();
                string productCode = DDLProductDesc.SelectedValue.ToString();
                bool _stockCheck = ValidateStock(productCode, Convert.ToDecimal(txtStockMoveQty.Text));



                #endregion


                if (_exits == false && _stockCheck == true)
                {
                    #region Add New Row in Datatable with values

                    DataRow row;
                    row = dtLineItems.NewRow();

                    row["ProductGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                    row["ProductSubCategory"] = DDLProdSubCategory.SelectedItem.Text.ToString();
                    row["ProductDesc"] = DDLProductDesc.SelectedValue.ToString();   // DDLProductDesc.SelectedItem.Text.ToString();
                    row["UOM"] = DDLEntryType.SelectedItem.Text.ToString();
                    row["Reason"] = DDLReason.SelectedValue.ToString();
                    row["MoveQty"] = txtStockMoveQty.Text.ToString();

                    dtLineItems.Rows.Add(row);

                    #endregion
                }

                Session["LineItem"] = dtLineItems;
                //ClearProductDropDown();
                DDLReason.SelectedIndex = 0;
                DDLEntryType.SelectedIndex = 0;
                txtStockMoveQty.Text = string.Empty;
                txtProductCode.Text = string.Empty;
                return dtLineItems;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
        }

        private bool ValidateStock(string ProductCode, decimal MoveQty)
        {
            bool stock = true;

            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string queryStock = " Select isnull(SUM(INVTS.TRANSQTY),0) AS STOCK From AX.ACXINVENTTRANS INVTS INNER JOIN AX.INVENTTABLE INV  ON INV.ITEMID= INVTS.PRODUCTCODE " +
                           " WHERE INVTS.DATAAREAID='" + Session["DATAAREAID"] + "' AND INVTS.SiteCode='" + Session["SiteCode"] + "' " +
                           " AND INVTS.TRANSLOCATION='" + DDLWarehouseFrom.SelectedValue.Trim().ToString() + "' " +
                           " AND INVTS.PRODUCTCODE ='" + ProductCode + "'";

            object stockValue = obj.GetScalarValue(queryStock);
            if (stockValue != null)
            {
                decimal AvailableStock = Math.Round(Convert.ToDecimal(stockValue.ToString()), 2);
                if (MoveQty <= AvailableStock)
                {
                    if (MoveQty + AvailableStock < 0)
                    {
                        stock = false;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Stock Negative Issue : Final Stock cannot be in negative figure. !');", true);
                        LblMessage.Text = "Stock Negative Issue : Final Stock cannot be in negative figure. !";
                    }
                    else
                    {
                        stock = true;
                    }

                }
                else
                {
                    stock = false;
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Stock Issue : Entered Value cannot be more than the available Stock !');", true);
                    LblMessage.Text = "Stock Issue : Entered Value cannot be more than the available Stock !";
                }
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
            return stock;
        }

        private void ClearProductDropDown()             // Clears all the Items for Prodcut Group, Sub-Category, Desc after Adding Line Items//
        {
            DDLProductGroup.Items.Clear();
            DDLProdSubCategory.Items.Clear();
            DDLProductDesc.Items.Clear();
        }

        private void ResetAllControls()
        {
            //ClearProductDropDown();
            FillReturnReasonType();
            txtStockMoveQty.Text = string.Empty;
            Session["ItemTable"] = null;
            Session["LineItem"] = null;
            gridStockTransferItems.DataSource = null;
            gridStockTransferItems.Visible = false;
            txtDate.Text = string.Empty;
            string datestring = DateTime.Now.ToString("yyyy-MM-dd");
            txtDate.Text = datestring;
            LoadWarehouse();
            txtProductCode.Text = string.Empty;
            LblMessage.Text = string.Empty;
            //DDLProductDesc.SelectedItem.Text = "Select...";
            //DDLProdSubCategory.SelectedItem.Text = "Select...";
            //DDLProductGroup.SelectedItem.Text = "Select...";

           // LoadWarehouse();
            FillBU();
            ProductGroup();
            ProductSubCategory();
            FillProductCode();
            Session["LineItem"] = null;
            txtStockMoveQty.Text = string.Empty;
            txtstock.Text = string.Empty;

        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            HiddenField hiddenField = (HiddenField)gvrow.FindControl("HiddenField2");
            LinkButton lnk = sender as LinkButton;

            if (Session["LineItem"] != null)
            {
                DataTable dt = Session["LineItem"] as DataTable;
                dt.AsEnumerable().Where(r => r.Field<int>("SNO") == Convert.ToInt32(hiddenField.Value)).ToList().ForEach(row => row.Delete());
                gridStockTransferItems.DataSource = dt;
                gridStockTransferItems.DataBind();
                Session["LineItem"] = dt;
            }
        }

        private bool ValidateStockTransferSave()                    // Validates all required values for Saving the Data to Database INVENTRANS//
        {
            bool _value = false;
            if (gridStockTransferItems.Rows.Count <= 0 && Session["ItemTable"] == null)
            {
                _value = false;
                this.LblMessage.Text = "►  Please Add Stock Transfer Product !";
                
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Add Items for Adjustment Entry !');", true);
            }
            else
            {
                string SqlQuery = " SELECT DISTINCT SITECODE,PRODUCTCODE,ISNULL(SUM(TRANSQTY),0) TRANSQTY FROM AX.ACXINVENTTRANS IT WHERE SITECODE='" + Session["SiteCode"].ToString() + "'" +
                    " AND TRANSLOCATION='" + DDLWarehouseFrom.SelectedValue.ToString() + "' GROUP BY SITECODE,PRODUCTCODE HAVING ISNULL(SUM(TRANSQTY),0)>0 ";
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                DataTable dtPrdStock = obj.GetData(SqlQuery);
                DataRow[] drrow;

                for (int i = 0; i < gridStockTransferItems.Rows.Count; i++)
                {
                    string productNameCode = gridStockTransferItems.Rows[i].Cells[3].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    drrow = dtPrdStock.Select("PRODUCTCODE='" + ProductCode + "'");
                    if (drrow.Length == 0)
                    {
                        _value = false;
                        this.LblMessage.Text = "►  " + productNameCode + " stock not available. Please remove !";
                        return _value;
                    }
                    else
                    {
                        decimal TransQty = Convert.ToDecimal(gridStockTransferItems.Rows[i].Cells[6].Text);
                        decimal stockqty=Convert.ToDecimal(drrow[0]["TRANSQTY"]);

                        if (stockqty<TransQty)
                        {
                            _value = false;
                            this.LblMessage.Text = "►  " + productNameCode + " stock not available. Please remove !";
                            return _value;
                        }
                    }
                }
                _value = true;
            }
            return _value;
        }

        protected void BtnSaveStockTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateStockTransferSave();
                if (b == true)
                {
                    SaveStockTransferToDB();
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void SaveStockTransferToDB()
        {
            try
            {
                //var watch = System.Diagnostics.Stopwatch.StartNew();
                // the code that you want to measure comes here

                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                conn = obj.GetConnection();

                string queryInsert = " Insert Into AX.ACXINVENTTRANS " +
                                     " ([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                     " [DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno])" +
                                     " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate, " +
                                     " @ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno)";


                string st = Session["SiteCode"].ToString();
                string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");        //SiteCode[6 Character] plus yymmddhhss of current time//

                //string TransId = obj.GetNumSequence(10, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());

                #region Generate New Code for Stock Transfer

                string _query = "SELECT ISNULL(MAX(CAST(RIGHT(DocumentNo,10) AS INT)),0)+1 FROM [ax].[ACXINVENTTRANS] " +
                                "where SITECODE='" + Session["SiteCode"].ToString() + "' and TransType=6";

                cmd = new SqlCommand(_query);
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;
                object vc = cmd.ExecuteScalar();

                string strCodeforStockTransfer = "ST-" + ((int)vc).ToString("0000000000");          //--Stock Transfer Number [ST-0000000001] ---// 

                #endregion

                cmd.CommandText = string.Empty;
                cmd.CommandText = queryInsert;

                #region Grid Data Insert for Warehouse A

                for (int i = 0; i < gridStockTransferItems.Rows.Count; i++)
                {

                    string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 6;                                          // For Stock Transfer TransType //
                    int DocumentType = 6;                                       // For Stock Transfer TransType //
                    string DocumentNo = strCodeforStockTransfer;               // Stock Transfer Number as generated by Code---//
                    
                    string productNameCode = gridStockTransferItems.Rows[i].Cells[3].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gridStockTransferItems.Rows[i].Cells[6].Text);
                    string UOM = gridStockTransferItems.Rows[i].Cells[4].Text;
                    
                    int RecID = i + 1;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TransId", TransId);
                    cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                    cmd.Parameters.AddWithValue("@RECID", RecID);
                    cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TransType", TransType);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                    cmd.Parameters.AddWithValue("@Referencedocumentno", "");
                    cmd.Parameters.AddWithValue("@DocumentDate", Convert.ToDateTime(txtDate.Text));
                    cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                    cmd.Parameters.AddWithValue("@TransQty", TransQty * -1);            // for Stock less from WareHouse From Location//
                    cmd.Parameters.AddWithValue("@TransUOM", UOM);
                    cmd.Parameters.AddWithValue("@TransLocation", DDLWarehouseFrom.SelectedValue.ToString());

                    cmd.ExecuteNonQuery();
                    ViewState["AdjustmentNo"] = TransId;
                }

                #endregion

                            

                StockTransferTOWareHouse(strCodeforStockTransfer, transaction, conn);

                //obj.UpdateLastNumSequence(10, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);

                transaction.Commit();
                //watch.Stop();
                //var elapsedMs = watch.ElapsedMilliseconds;
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Transfer No: " + strCodeforStockTransfer +  " .Stock Transferred from "+DDLWarehouseTo.Text.ToString()+ " to "+DDLWarehouseTo.Text+ " Successfully !!. ');", true);

                string message = " alert('Transfer No: " + strCodeforStockTransfer + " .Stock Transferred from " + DDLWarehouseFrom.Text.ToString() + " to " + DDLWarehouseTo.Text + " Successfully !!. ');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                ResetAllControls();
                cmd.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = "► Stock Not Transffered. " + " Error: " +  ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }


        }


        private void StockTransferTOWareHouse(string StockTransferCode, SqlTransaction trans, SqlConnection conn)      // Save Same Stock to WAREHOUSE OTHER LOCATION B //
        {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                //conn = obj.GetConnection();

                #region Stock Transfer Save for Warehouse TO Location

                string queryInsert = " Insert Into AX.ACXINVENTTRANS " +
                                     " ([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                     " [DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno])" +
                                     " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate, " +
                                     " @ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno)";


                string st = Session["SiteCode"].ToString();
                string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.AddSeconds(2).ToString("yymmddhhmmss");        //SiteCode[6 Character] plus yymmddhhss of current time//

                cmd = new SqlCommand(queryInsert);
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                #region Grid Data Insert Section

                for (int i = 0; i < gridStockTransferItems.Rows.Count; i++)
                {

                    string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 6;                                          // For Stock Transfer TransType //
                    int DocumentType = 6;                                       // For Stock Transfer TransType //
                    string DocumentNo = StockTransferCode;               // Stock Transfer Number as generated by Code---//
                    string productNameCode = gridStockTransferItems.Rows[i].Cells[3].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gridStockTransferItems.Rows[i].Cells[6].Text);
                    string UOM = gridStockTransferItems.Rows[i].Cells[4].Text;
                    
                    int RecID = i + 1;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TransId", TransId);
                    cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                    cmd.Parameters.AddWithValue("@RECID", RecID);
                    cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TransType", TransType);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                    cmd.Parameters.AddWithValue("@Referencedocumentno", "");
                    cmd.Parameters.AddWithValue("@DocumentDate", Convert.ToDateTime(txtDate.Text));
                    cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                    cmd.Parameters.AddWithValue("@TransQty", TransQty);            // for Stock add from WareHouse TO Location//
                    cmd.Parameters.AddWithValue("@TransUOM", UOM);
                    cmd.Parameters.AddWithValue("@TransLocation", DDLWarehouseTo.SelectedValue.ToString());

                    cmd.ExecuteNonQuery();

                }

                #endregion

                #endregion

            //}
            //catch (Exception ex)
            //{
            //    transaction.Rollback();
            //}
        }
        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            ResetAllControls();
        }

        protected void DDLProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                         + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'";
            DDLProdSubCategory.Items.Clear();
            DDLProdSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProdSubCategory, strQuery, "Name", "Code");
            DDLProdSubCategory.Focus();
            strQuery = "SELECT distinct inv.ITEMID+'-'+inv.Product_Name as Name,inv.ITEMID from ax.INVENTTABLE inv where inv.Product_Group='" + DDLProductGroup.SelectedValue + "' ";
            DDLProductDesc.DataSource = null;
            DDLProductDesc.Items.Clear();
            DDLProductDesc.Items.Add("-Select-");
            baseObj.BindToDropDown(DDLProductDesc, strQuery, "Name", "ITEMID");
        }

        protected void DDLProdSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = "Select distinct P.ITEMID+'-'+P.Product_Name as Name,P.ITEMID from ax.InventTable P where Product_Group='" + DDLProductGroup.SelectedValue + "' and P.PRODUCT_SUBCATEGORY ='" + DDLProdSubCategory.SelectedItem.Value + "'"; //--AND SITE_CODE='657546'";
            DDLProductDesc.DataSource = null;
            DDLProductDesc.Items.Clear();
            baseObj.BindToDropDown(DDLProductDesc, strQuery, "Name", "ITEMID");
            DDLProductDesc.Items.Add("Select...");
            DDLProductDesc.Focus();
            if (DDLProductDesc.SelectedItem.Text != "Select...")
            {
                DDLProductDesc_SelectedIndexChanged(null, null);
            }
        }

        protected void DDLProductDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            //===========Fill Product Group and Product Sub Cat========
            DataTable dt = new DataTable();
            string query = "select Product_Group,PRODUCT_SUBCATEGORY  from ax.INVENTTABLE where ItemId='" + DDLProductDesc.SelectedValue.ToString() + "' order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                //=============For Product Sub Cat======
                DDLProdSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
                
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string queryStock = " Select isnull(SUM(INVTS.TRANSQTY),0) AS STOCK From AX.ACXINVENTTRANS INVTS INNER JOIN AX.INVENTTABLE INV  ON INV.ITEMID= INVTS.PRODUCTCODE " +
                               " WHERE INVTS.DATAAREAID='" + Session["DATAAREAID"] + "' AND INVTS.SiteCode='" + Session["SiteCode"] + "' " +
                               " AND INVTS.TRANSLOCATION='" + DDLWarehouseFrom.SelectedValue.Trim().ToString() + "' " +
                               " AND INVTS.PRODUCTCODE ='" + DDLProductDesc.SelectedValue.ToString() + "'";

                object stockValue = obj.GetScalarValue(queryStock);
                if (stockValue != null)
                {
                    txtstock.Text= Convert.ToDecimal(stockValue.ToString()).ToString("0.00");
                }
            }
        }

        protected void DDLBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLProdSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();

        }

        protected void DDLWarehouseFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string query2 = "SELECT INVENTLOCATIONID,NAME FROM AX.inventlocation WHERE INVENTSITEID='" + Session["SiteCode"].ToString() + "' and INVENTLOCATIONID <> '" + DDLWarehouseFrom.SelectedValue + "'";
            DDLWarehouseTo.Items.Clear();
            DDLWarehouseTo.Items.Add("-Select-");
            obj.BindToDropDown(DDLWarehouseTo, query2, "NAME", "INVENTLOCATIONID");
        }
    }
}