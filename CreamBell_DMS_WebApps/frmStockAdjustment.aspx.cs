using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmStockAdjustment : System.Web.UI.Page
    {
        public DataTable dtLineItems;
        public SqlCommand cmd;
        public SqlTransaction transaction;
        public SqlConnection conn;
        App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                BtnSaveAdjustment.Attributes.Add("onclick", "javascript: if (Page_ClientValidate() ){" + BtnSaveAdjustment.ClientID + ".disabled=true;}" + ClientScript.GetPostBackEventReference(BtnSaveAdjustment, ""));
                this.txtAdjValue.Attributes.Add("onkeypress", "button_click(this,'" + this.BtnAdd.ClientID + "')");
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                DDLBusinessUnit.Items.Add("All...");
                baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
                FillMaterialGroup();
                FillReturnReasonType();
                FillProductCode();
                //FillBU();
                LoadWarehouse();
                FillState();
                hdfPacksize.Value = "0";
                Session["LineItem"] = null;
                imgBtDate.Visible = false;
            }

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
                    strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
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
                    strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv "
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
            //string strQuery = string.Empty;
            //DDLProductDesc.Items.Clear();
            //
            //string BU;
            //if (DDLBusinessUnit.SelectedIndex == 0)
            //{
            //    BU = "";
            //}
            //else
            //{
            //    BU = DDLBusinessUnit.SelectedValue.ToString();
            //}
            //string a = DDLProductGroup.Text;
            //string b = DDLProdSubCategory.Text;
            //if (DDLProductGroup.Text == "Select..." && DDLProdSubCategory.Text == "Select..." || DDLProdSubCategory.Text == "")
            //{
            //    strQuery = "SELECT DISTINCT(ITEMID) AS Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) AS Product_Name FROM AX.INVENTTABLE inv";
            //    strQuery += " where BU_CODE LIKE CASE WHEN LEN('" + BU + "')>0 THEN '" + BU + "' ELSE '%' END";
            //    strQuery += " Order By Product_Name";
            //    DDLProductDesc.Items.Clear();
            //    DDLProductDesc.Items.Add("-Select-");
            //    baseObj.BindToDropDown(DDLProductDesc, strQuery, "Product_Name", "Product_Code");
            //    DDLProductDesc.Focus();
            //}
        }
        protected void FillBU()
        {
            DDLBusinessUnit.Items.Clear();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            DDLBusinessUnit.Items.Add("All...");
            baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
        }
        private void LoadWarehouse()
        {
            string sitecode = ddldistributor.SelectedValue.ToString();
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string query1 = "select MainWarehouse from ax.inventsite where siteid='" + sitecode + "'";
            DataTable dt = new DataTable();
            dt = obj.GetData(query1);
            if (dt.Rows.Count > 0)
            {
                Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                LblWarehouse.Text = Session["TransLocation"].ToString() + " - " + "MAIN WAREHOUSE";
            }
            else
            {
                LblWarehouse.Text = "";
            }
        }
        private void FillMaterialGroup()
        {
            if (DDLBusinessUnit.SelectedItem.Text == "All...")
            {
                DDLProductGroup.Items.Clear();
                string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
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
                string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' and BU_CODE in ('" + DDLBusinessUnit.SelectedItem.Value.ToString() + "') ";
                //if (rdStock.Checked == true)
                //{
                //    strProductGroup += " AND a.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                //}
                DDLProductGroup.Items.Clear();
                strProductGroup += " order by a.Product_Group";
                DDLProductGroup.Items.Add("Select...");
                baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");
            }
            //CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            //string BU;        
            //if (DDLBusinessUnit.SelectedIndex == 0)
            //{
            //    BU = "";
            //}
            //else
            //{
            //    BU = DDLBusinessUnit.SelectedValue.ToString();
            //}
            //string strQuery = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE WHERE BLOCK=0";
            //strQuery += " and BU_CODE LIKE CASE WHEN LEN('" + BU + "')>0 THEN '" + BU + "' ELSE '%' END";
            //DDLProductGroup.Items.Clear();
            //DDLProductGroup.Items.Add("-Select-");
            //obj.BindToDropDown(DDLProductGroup, strQuery, "PRODUCT_GROUP", "PRODUCT_GROUP");
        }
        private void FillReturnReasonType()
        {
            App_Code.Global obj = new App_Code.Global();

            string strQuery = "Select distinct DAMAGEREASON_CODE,DAMAGEREASON_CODE + '-(' + DAMAGEREASON_NAME +')' as RETURNREASON  from [ax].[ACXDAMAGEREASON]";
            DDLReason.Items.Clear();
            DDLReason.Items.Add("-Select-");
            obj.BindToDropDown(DDLReason, strQuery, "RETURNREASON", "DAMAGEREASON_CODE");
        }

        protected void DDLProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = string.Empty;
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                         + "where  P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'";
            DDLProdSubCategory.Items.Clear();
            DDLProdSubCategory.Items.Add("-Select-");
            baseObj.BindToDropDown(DDLProdSubCategory, strQuery, "Name", "Code");
            DDLProdSubCategory.Focus();
            strQuery = "SELECT distinct inv.ITEMID+'-'+inv.Product_Name as Name,inv.ITEMID from ax.INVENTTABLE inv where inv.Product_Group='" + DDLProductGroup.SelectedValue + "' ";
            DDLProductDesc.DataSource = null;
            DDLProductDesc.Items.Clear();
            DDLProductDesc.Items.Add("-Select-");
            baseObj.BindToDropDown(DDLProductDesc, strQuery, "Name", "ITEMID");

            txtPcsQty.Text = "";
            txtPcsQty.Text = "";
            txtBoxPcs.Text = "";
            txtAdjValue.Text = "";
            hdfPacksize.Value = "0";
            PcsBillingApplicable();
        }

        protected void DDLProdSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = string.Empty;
            strQuery = "SELECT Distinct inv.ITEMID+'-'+inv.Product_Name AS Name,inv.ITEMID FROM AX.INVENTTABLE INV WHERE INV.Product_Group='" + DDLProductGroup.SelectedValue + "' and inv.PRODUCT_SUBCATEGORY ='" + DDLProdSubCategory.SelectedItem.Value + "' ";

            DDLProductDesc.DataSource = null;
            DDLProductDesc.Items.Clear();
            DDLProductDesc.Items.Add("-Select-");
            baseObj.BindToDropDown(DDLProductDesc, strQuery, "Name", "ITEMID");
            txtBoxQty.Text = "";
            txtPcsQty.Text = "";
            txtBoxPcs.Text = "";
            txtAdjValue.Text = "";
            hdfPacksize.Value = "0";
            DDLProductDesc.Enabled = true;
            DDLProductDesc.Focus();

            if (DDLProductDesc.SelectedItem.Text != "Select...")
            {
                DDLProductDesc_SelectedIndexChanged(null, null);
            }

            PcsBillingApplicable();
        }

        protected void txtAdjValue_TextChanged(object sender, EventArgs e)
        {
            if (txtAdjValue.Text != string.Empty)
            {
                BtnAdd.Focus();
            }
        }

        private bool ValidateLineItemAdd()
        {
            bool b = false;

            if (DDLProductGroup.Text.LastIndexOf("Select") >= 0)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Group !');", true);
                DDLProductGroup.Focus();
                b = false;
                return b;
            }
            if (DDLProdSubCategory.Text.LastIndexOf("Select") >= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Sub Category First !');", true);
                DDLProdSubCategory.Focus();
                b = false;
                return b;
            }

            if (DDLProductDesc.Text.LastIndexOf("Select") >= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product First !');", true);
                DDLProductDesc.Focus();
                b = false;
                return b;
            }

            if (DDLReason.Text.LastIndexOf("Select") >= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Reason Type !');", true);
                DDLReason.Focus();
                b = false;
                return b;
            }

            if (txtAdjValue.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Provide Adjustment Value !');", true);
                txtAdjValue.Focus();
                b = false;
                return b;
            }

            if (txtAdjValue.Text.Length > 0 && txtAdjValue.Text != string.Empty)
            {
                if (Convert.ToDecimal(txtAdjValue.Text) == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Adjustment Value cannot be 0 !');", true);
                    txtAdjValue.Focus();
                    b = false;
                    return b;
                }
                else
                {
                    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                    if (Convert.ToDecimal(obj.GetScalarValue("SELECT COUNT(*) FROM AX.INVENTTABLE WHERE ITEMID='" + DDLProductDesc.SelectedValue.ToString() + "' AND BLOCK=1")) > 0 && Convert.ToDecimal(txtAdjValue.Text) > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Product blocked. So positive adjustment not allowed !!');", true);
                        txtAdjValue.Focus();
                        b = false;
                        return b;
                    }
                    b = true;
                    return b;
                }
            }

            return b;
        }

        private DataTable AddLineItems()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

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
                    dtLineItems.Columns.Add("AdjustmentValue", typeof(decimal));
                    dtLineItems.Columns.Add("BoxQty", typeof(decimal));
                    dtLineItems.Columns.Add("PcsQty", typeof(decimal));
                    dtLineItems.Columns.Add("BoxPcs", typeof(string));
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
                        for (int i = 0; i < dtLineItems.Rows.Count; i++)
                        {
                            if (dtLineItems.Rows[i]["ProductDesc"].ToString() == DDLProductDesc.SelectedItem.Text.ToString())
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: You are adding duplicate adjustment Entry !');", true);
                                _exits = true;
                                break;
                            }

                        }

                        if (_exits)
                        {
                            return dtLineItems;
                        }
                        //var rowColl = dtLineItems.AsEnumerable();
                        //var record = (from r in rowColl
                        //              where r.Field<string>("ProductGroup") == DDLProductGroup.SelectedItem.Text &&
                        //              r.Field<string>("ProductSubCategory") == DDLProdSubCategory.SelectedItem.Text.ToString()
                        //              && r.Field<string>("ProductDesc") == DDLProductDesc.SelectedItem.Text.ToString() && r.Field<decimal>("AdjustmentValue") == Convert.ToDecimal(txtAdjValue.Text.ToString())
                        //              select r.Field<int>("SNO")).First<int>();
                        //if (record >= 1)
                        //{
                        //    _exits = true;
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: You are adding duplicate adjustment Entry !');", true);
                        //}
                        //else
                        //{
                        //    _exits = false;
                        //}
                    }
                }
                catch (Exception ex)
                {
                    _exits = false;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }

                #endregion

                #region Stock Availability Check For That Product

                string productNameCode = DDLProductDesc.SelectedItem.Text.ToString();
                string[] str = productNameCode.Split('-');
                string productCode = str[0].ToString();

                bool _stockCheck = ValidateStock(productCode, Convert.ToDecimal(txtAdjValue.Text));

                #endregion

                if (_exits == false && _stockCheck == true)
                {
                    #region Add New Row in Datatable with values
                    DataRow row;
                    row = dtLineItems.NewRow();

                    row["ProductGroup"] = DDLProductGroup.SelectedItem.Text.ToString();
                    row["ProductSubCategory"] = DDLProdSubCategory.SelectedItem.Text.ToString();
                    row["ProductDesc"] = DDLProductDesc.SelectedItem.Text.ToString();
                    row["UOM"] = "Box";
                    row["Reason"] = DDLReason.SelectedValue.ToString();
                    row["AdjustmentValue"] = App_Code.Global.ConvertToDecimal(txtAdjValue.Text).ToString();
                    row["BoxQty"] = App_Code.Global.ConvertToDecimal(txtBoxQty.Text).ToString();
                    row["PcsQty"] = App_Code.Global.ConvertToDecimal(txtPcsQty.Text).ToString();
                    row["BoxPcs"] = txtBoxPcs.Text.ToString();
                    dtLineItems.Rows.Add(row);
                    #endregion
                }
                //Update session table
                Session["LineItem"] = dtLineItems;
                Reset();
                return dtLineItems;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
        }

        private void Reset()
        {
            FillMaterialGroup();
            DDLProdSubCategory.SelectedIndex = 0;
            DDLProductDesc.SelectedIndex = 0;
            DDLReason.SelectedIndex = 0;
            DDLBusinessUnit.SelectedIndex = 0;
            txtAdjValue.Text = string.Empty;
            txtBoxQty.Text = string.Empty;
            txtPcsQty.Text = string.Empty;
            txtBoxPcs.Text = string.Empty;
        }
        public void ProductSubCategory()
        {
            string strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'";
            DDLProdSubCategory.Items.Clear();
            DDLProdSubCategory.Items.Add("-Select-");
            baseObj.BindToDropDown(DDLProdSubCategory, strQuery, "Name", "Code");
            // FillProductCode();
            DDLProdSubCategory.Focus();
        }
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            bool valid = ValidateLineItemAdd();
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                dt = AddLineItems();
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

        private bool ValidateAdjustmentSave()
        {
            bool _value = false;
            if (ddlstate.Text == "Select...")
            {
                string message = "alert('Please select state !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return false;
            }
            if (ddldistributor.Text == "Select...")
            {
                string message = "alert('Please select Distributor !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return false;
            }

            if (txtAdjustmentRefNo.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Provide the Reference Number !');", true);
                txtAdjustmentRefNo.Focus();
                return false;
            }
            if (gridAdjusment.Rows.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please add line items to grid !');", true);
                return false;
            }
            else
            {
                _value = true;
            }
            return _value;
        }

        private bool ValidateAdjustmentSaveFromExcel()
        {
            bool _value = false;
            if (ddlstate.Text == "Select...")
            {
                string message = "alert('Please select state !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return false;
            }
            if (ddldistributor.Text == "Select...")
            {
                string message = "alert('Please select Distributor !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return false;

            }

            if (txtAdjustmentRefNo.Text == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please Provide the Reference Number !');", true);
                txtAdjustmentRefNo.Focus();
                return false;
            }
            else
            {
                _value = true;
            }
            return _value;
        }

        protected void QtyCalcualtion()
        {
            try
            {
                string query = @"Select I.ITEMID,I.PRODUCT_PACKSIZE,I.PRODUCT_CRATE_PACKSIZE,I.LTR from ax.InventTable I "
                             + "Where I.ITEMID ='" + DDLProductDesc.SelectedItem.Value + "'  ";
                DataTable dtItem = new DataTable();
                dtItem = baseObj.GetData(query);
                if (dtItem.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString()) <= 0)
                    {
                        string message = "alert('Please Check Product Pack Size !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                    decimal TotalQty = 0, boxQty = 0, pcsQty = 0, pacSize = 0;
                    boxQty = App_Code.Global.ParseDecimal(txtBoxQty.Text);
                    pcsQty = App_Code.Global.ParseDecimal(txtPcsQty.Text);
                    pacSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                    TotalQty = boxQty + (pcsQty / (pacSize == 0 ? 1 : pacSize));  //Total qty (Create+box+pcs) with decimal factor
                    decimal TotalBox = 0, TotalPcs = 0;
                    string BoxPcs = "";
                    TotalBox = Math.Truncate(TotalQty);                     //Extract Only Box Qty From Total Qty
                    TotalPcs = Math.Round((TotalQty - TotalBox) * pacSize); //Extract Only Pcs Qty From Total Qty
                    BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);
                    txtBoxQty.Text = TotalBox.ToString("0");
                    txtPcsQty.Text = TotalPcs.ToString("0");
                    txtBoxPcs.Text = BoxPcs;
                    txtAdjValue.Text = TotalQty.ToString("0.00");
                }
                else
                {
                    string message = "alert('Please Check Product Pack Size !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }

        }
        private void SaveAdjustmentToTrans()
        {
            string sitecode = ddldistributor.SelectedValue.ToString();
            try
            {
                //validate stock changed by amol 03-Feb-2017
                for (int i = 0; i < gridAdjusment.Rows.Count; i++)
                {
                    string productNameCode = gridAdjusment.Rows[i].Cells[3].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gridAdjusment.Rows[i].Cells[6].Text);
                    bool _stockcheck = ValidateStock(ProductCode, TransQty);
                    if (!_stockcheck)
                    {
                        return;
                    }
                }

                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";

                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + sitecode + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }
                else
                {
                    string message = "alert('Mainwarehouse not defined please check!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }

                DataTable dtNumSeq = obj.GetNumSequenceNew(9, sitecode, Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                string NUMSEQ = string.Empty;
                string TransId = string.Empty;
                if (dtNumSeq != null)
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
                                 "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],[NUMSEQ])" +
                                 " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate, " +
                                 " @ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno,@NUMSEQ)";


                conn = obj.GetConnection();
                cmd = new SqlCommand(queryInsert);
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                // string st = Session["SiteCode"].ToString();


                for (int i = 0; i < gridAdjusment.Rows.Count; i++)
                {
                    // string a = ddldistributor.SelectedValue.ToString();
                    // string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 3;//1 for purchase invoice receipt
                    int DocumentType = 3;
                    string productNameCode = gridAdjusment.Rows[i].Cells[3].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gridAdjusment.Rows[i].Cells[9].Text);
                    string uom = gridAdjusment.Rows[i].Cells[4].Text;
                    //string TransLocation = st.Substring(st.Length - 6);
                    string Referencedocumentno = gridAdjusment.Rows[i].Cells[5].Text;
                    int REcid = i + 1;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TransId", TransId);
                    cmd.Parameters.AddWithValue("@SiteCode", sitecode);
                    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                    cmd.Parameters.AddWithValue("@RECID", i + 1);
                    cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TransType", TransType);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    cmd.Parameters.AddWithValue("@DocumentNo", txtAdjustmentRefNo.Text);
                    cmd.Parameters.AddWithValue("@DocumentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                    cmd.Parameters.AddWithValue("@TransQty", TransQty);
                    cmd.Parameters.AddWithValue("@TransUOM", uom);
                    cmd.Parameters.AddWithValue("@TransLocation", TransLocation);
                    cmd.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);
                    cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);

                    cmd.ExecuteNonQuery();
                    ViewState["AdjustmentNo"] = TransId;
                }
                //obj.UpdateLastNumSequence(9, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);
                transaction.Commit();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Adjustment Number : " + TransId + " Generated Successfully.!');", true);
                resetPage();
                cmd.Dispose();
                conn.Close();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Adjustment Number : " + TransId + " Generated Successfully.!');", true);
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
            DDLProdSubCategory.Items.Clear();
            FillBU();
            FillState();
            FillMaterialGroup();
            FillProductCode();
            FillReturnReasonType();
            txtAdjValue.Text = string.Empty;
            DDLProdSubCategory.Items.Clear();
            //DDLProductDesc.Items.Clear();
            Session["ItemTable"] = null;
            Session["LineItem"] = null;
            gridAdjusment.DataSource = null;
            gridAdjusment.DataBind();
            gridAdjusment.Visible = false;
            txtAdjustmentRefNo.Text = string.Empty;
            ViewState["AdjustmentNo"] = null;
            txtSearchAdjustmentNo.Text = string.Empty;
            LblWarehouse.Text = "";
            LblMessage.Text = "";
            // ddlstate.Text = string.Empty;
            //  ddldistributor.Text = string.Empty;
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
                 gridAdjusment.DataSource = dt;
                 gridAdjusment.DataBind();
                 Session["LineItem"] = dt;
            }
        }

        protected void rdoManualEntry_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoManualEntry")
            {
                #region Manual Entry

                
                gridAdjusment.DataSource = null;
                gridAdjusment.DataBind();
                panelAdjustmentEntry.Visible = true;
                AsyncFileUpload1.Visible = false;
                btnUpload.Visible = false;
                DDLBusinessUnit.Visible = true;
                DDLBusinessUnit.SelectedIndex = 0;
                DDLProductGroup.Visible = true;
                DDLProductGroup.SelectedIndex = 0;
                txtBoxQty.Text = string.Empty;
                txtPcsQty.Text = string.Empty;
                txtBoxPcs.Text = string.Empty;
                txtAdjValue.Text = string.Empty;

                BtnAdd.Visible = true;
                gridAdjusment.DataSource = null;
                gridAdjusment.DataBind();

                Session["LineItem"] = null;
                Session["SchemeLineItem"] = null;
                Session["PreSoNO"] = null;
                Session["SONO"] = null;

                #endregion
            }
            if (rdo.ID == "rdoExcelEntry")
            {
                #region Excel Entry

                panelAdjustmentEntry.Visible = false;
                AsyncFileUpload1.Visible = true;
                btnUpload.Visible = true;
                gridAdjusment.DataSource = null;
                gridAdjusment.DataBind();
                gridviewRecordNotExist.DataSource = null;
                gridviewRecordNotExist.DataBind();
                DDLBusinessUnit.Visible = false;
                DDLProductGroup.Visible = false;
                DDLProductDesc.SelectedIndex = 0;
                DDLReason.SelectedIndex = 0;
                txtBoxQty.Text = string.Empty;
                txtPcsQty.Text = string.Empty;
                txtBoxPcs.Text = string.Empty;
                txtAdjValue.Text = string.Empty;
                BtnAdd.Visible = false;

                Session["LineItem"] = null;
                Session["SchemeLineItem"] = null;
                Session["PreSoNO"] = null;
                Session["SONO"] = null;
                #endregion

            }

        }

        protected void ImDnldTemp_Click(object sender, ImageClickEventArgs e)
        {
           Response.Redirect("ExcelTemplate/StockAdjustmentExcelTemplate.xlsx");
        }

        private void dtChangeZeroToNull(DataTable dataTable)
        {
            List<string> dcNames = dataTable.Columns
                                    .Cast<DataColumn>()
                                    .Select(x => x.ColumnName)
                                    .ToList(); //This querying of the Column Names, you could do with LINQ
            foreach (DataRow row in dataTable.Rows) //This is the part where you update the cell one by one
                foreach (string columnName in dcNames)
                    row[columnName] = row[columnName] == DBNull.Value ? 0 : row[columnName];
        }

        protected void fileUploadValidation(object sender, EventArgs args)
        {
            //System.IO.FileStream fs = new FileStream(pathSource, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        protected void UploadQtyCalculation(DataTable dt, int index)
        {

            try
            {
                if (ValidateExcelData(dt, index) == true)
                {

                    string query = @"Select I.ITEMID,I.PRODUCT_PACKSIZE,I.PRODUCT_CRATE_PACKSIZE,I.LTR from ax.InventTable I "
                                 + "Where I.ITEMID ='" + Session["ProductCode"] + "'  ";
                    DataTable dtItem = new DataTable();
                    dtItem = baseObj.GetData(query);
                    if (dtItem.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString()) <= 0)
                        {
                            string message = "alert('Please Check Product Pack Size !');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            return;
                        }
                        decimal TotalQty = 0, boxQty = 0, pcsQty = 0, pacSize = 0;
                        boxQty = App_Code.Global.ParseDecimal(dt.Rows[index]["BoxQty"].ToString());
                        pcsQty = App_Code.Global.ParseDecimal(dt.Columns["PcsQty"].ToString());
                        pacSize = App_Code.Global.ParseDecimal(dtItem.Rows[0]["PRODUCT_PACKSIZE"].ToString());
                        TotalQty = boxQty + (pcsQty / (pacSize == 0 ? 1 : pacSize));  //Total qty (Create+box+pcs) with decimal factor
                        decimal TotalBox = 0, TotalPcs = 0;
                        string BoxPcs = "";
                        TotalBox = Math.Truncate(TotalQty);                     //Extract Only Box Qty From Total Qty
                        TotalPcs = Math.Round((TotalQty - TotalBox) * pacSize); //Extract Only Pcs Qty From Total Qty
                        BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);
                        //txtBoxQty.Text = TotalBox.ToString("0");
                        //txtPcsQty.Text = TotalPcs.ToString("0");
                        //txtBoxPcs.Text = BoxPcs;
                        //txtAdjValue.Text = TotalQty.ToString("0.00");
                        dt.Rows[index]["BoxPcs"] = Convert.ToDouble(BoxPcs);
                        dt.Rows[index]["AdjustmentValue"] = Convert.ToDouble(TotalQty.ToString("0.00"));
                    }
                    else
                    {
                        string message = "alert('Please Check Product Pack Size !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }

                    
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }


        }

        protected bool ValidateExcelData(DataTable dtExceltable , int i)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string query = string.Empty;

            conn = new SqlConnection(obj.GetConnectionString());
            conn.Open();
            cmd = new SqlCommand("ACX_USP_PCSBillingApplicable", conn);
            cmd.Connection = conn;
            cmd.CommandTimeout = 3600;
            cmd.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            

            cmd.Parameters.AddWithValue("@StateCode", ddlstate.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@ProductGroup", dtExceltable.Rows[i]["ProductGroup"]);

            dt.Load(cmd.ExecuteReader());
            // bool returnVal = true;
            if (dt.Rows[0]["ApplicableOnProdGrp"].ToString() == "N" && Session["BoxQty"].ToString() == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        protected void btnUpload_Click(object sender, EventArgs e)

        {
            try
            {
                bool b = ValidateAdjustmentSaveFromExcel();
                if (b)
                {
                    if (AsyncFileUpload1.HasFile)
                    {


                        //#region
                        gridAdjusment.Visible = true;
                        string fileName = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);

                        AsyncFileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                        string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        string conString = string.Empty;
                        string extension = Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);
                        DataTable dtExcelData = new DataTable();

                        //  excel upload
                        dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLS(Server.MapPath("~/Uploads/" + fileName), true);

                        dtChangeZeroToNull(dtExcelData);

                        dtExcelData.TableName = "AdjustmentUpload";
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dtExcelData);
                        string StockAdjXml = ds.GetXml();

                        conn = baseObj.GetConnection();
                        cmd = new SqlCommand("ACX_AdjExcelUpload");
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;

                        
                        cmd.Parameters.Clear();
                        
                        cmd.Parameters.AddWithValue("@SITEID", ddldistributor.SelectedValue);

                        cmd.Parameters.AddWithValue("@XmlData", StockAdjXml);

                        //cmd.ExecuteNonQuery();
                        SqlDataAdapter da = new SqlDataAdapter();
                        
                        DataSet ds1 = new DataSet();
                        da.SelectCommand = cmd;
                        da.Fill(ds1);
                        if (ds1.Tables[0].Rows.Count>0)
                        { 
                            gridAdjusment.DataSource = ds1.Tables[0];
                            gridAdjusment.DataBind();
                            gridAdjusment.Visible = true;
                        }
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            gridviewRecordNotExist.DataSource = ds1.Tables[1];
                            gridviewRecordNotExist.DataBind();
                            ModalPopupExtender1.Show();
                        }
                        Session["LineItem"] = ds1.Tables[0];
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = ex.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);//LblMessage1.Visible = true;
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open")
                    {
                        conn.Close();
                    }
                }
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
            string sitecode = ddldistributor.SelectedValue.ToString();
            bool stock = true;

            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            string queryStock = " Select ISNULL(SUM(INVTS.TRANSQTY),0) AS STOCK From AX.ACXINVENTTRANS INVTS INNER JOIN AX.INVENTTABLE INV  ON INV.ITEMID= INVTS.PRODUCTCODE " +
                           " WHERE INVTS.DATAAREAID='" + Session["DATAAREAID"] + "' AND INVTS.SiteCode='" + sitecode + "' AND INVTS.TRANSLOCATION='" + Session["TransLocation"].ToString() + "' " +
                           " AND INVTS.PRODUCTCODE ='" + ProductCode + "'";

            object stockValue = obj.GetScalarValue(queryStock);
            if (stockValue == null || Convert.ToString(stockValue) == string.Empty)
            {
                stock = false;
            }
            if (stockValue != null && Convert.ToString(stockValue) != string.Empty)
            {
                decimal AvailableStock = Math.Round(Convert.ToDecimal(stockValue.ToString()), 2);
                //if (AdjusmentValue <= AvailableStock)
                //{
                if (AdjusmentValue + AvailableStock < 0)
                {
                    stock = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Stock Negative Issue : Final Stock cannot be in negative figure. !');", true);
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
            return stock;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bool b = validateSearch();
            if (b == true)
            {
                ShowAdjusmentEntryDetails(txtSearchAdjustmentNo.Text.Trim().ToString());
            }
        }

        protected void DDlSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlSearchType.SelectedItem.Text.ToString() == "Adjustment No")
            {
                imgBtDate.Visible = false;
            }
            //if (DDlSearchType.SelectedItem.Text.ToString() == "Date")
            //{
            //    imgBtDate.Visible = true;
            //}

        }

        private bool validateSearch()
        {
            bool search = false;
            if (txtSearchAdjustmentNo.Text == string.Empty)
            {
                search = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Provide Search Text !');", true);
            }
            else
            {
                search = true;
            }
            return search;
        }

        private void ShowAdjusmentEntryDetails(string Value)
        {

            string sitecode = ddldistributor.SelectedValue.ToString();

            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                string querySearch = string.Empty;

                querySearch = "Select A.RECID AS SNO, INV.PRODUCT_GROUP as ProductGroup,INV.PRODUCT_SUBCATEGORY AS ProductSubCategory, " +
                    " A.ProductCode + '-' + '('+INV.PRODUCT_NAME+')' AS ProductDesc, A.TransUOM AS UOM,CAST(a.TransQty as decimal(16,2)) AS AdjustmentValue, " +
                    " A.ReferenceDocumentNo as Reason , A.TransLocation,(SELECT BoxQty FROM dbo.udf_GetPCSDetails(a.TransQty,INV.PRODUCT_PACKSIZE)) AS BOXQTY,(SELECT PCSQty FROM dbo.udf_GetPCSDetails(a.TransQty,INV.PRODUCT_PACKSIZE)) AS PCSQTY,(SELECT BOXPRINT FROM dbo.udf_GetPCSDetails(a.TransQty,INV.PRODUCT_PACKSIZE)) AS BOXPCS  " +
                    " From AX.acxinventtrans A INNER JOIN AX.INVENTTABLE INV ON INV.ITEMID= A.ProductCode " +
                    " Where A.DocumentNo = '" + Value + "' And A.SiteCode='" + sitecode + "'";

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
            txtSearchAdjustmentNo.Text = string.Empty;
            resetPage();
        }

        protected void txtBoxQty_TextChanged(object sender, EventArgs e)
        {
            QtyCalcualtion();
        }

        protected void DDLProductDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBoxQty.Text = "";
            txtPcsQty.Text = "";
            txtAdjValue.Text = "";
            txtBoxPcs.Text = "";
            //===========Fill Product Group and Product Sub Cat========
            DataTable dt = new DataTable();
            string query = "Select Product_Group,PRODUCT_SUBCATEGORY,cast(Product_PackSize as decimal(10,2)) as Product_PackSize,Cast(Product_Crate_PackSize as decimal(10,2)) as Product_CrateSize    from ax.INVENTTABLE where ItemId='" + DDLProductDesc.SelectedItem.Value + "' order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                //=============For Product Sub Cat======
                DDLProdSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
            }
            txtBoxQty.Focus();
            PcsBillingApplicable();
        }
        public void PcsBillingApplicable()
        {
            string sitestate = ddlstate.SelectedValue.ToString();
            DataTable dt = baseObj.GetPcsBillingApplicability(sitestate, DDLProductGroup.SelectedItem.Value);  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
            string ProductGroupApplicable = string.Empty;

            if (dt.Rows.Count > 0)
            {
                ProductGroupApplicable = dt.Rows[0][1].ToString();
            }
            if (ProductGroupApplicable == "Y")
            {
                txtPcsQty.Enabled = true;
            }
            else
            {
                txtPcsQty.Enabled = false;
            }
        }

        protected void DDLBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLProdSubCategory.Items.Clear();
            FillMaterialGroup();
            FillProductCode();
        }


        protected void FillState()
        {
            DataTable dt = new DataTable();
            dt = new DataTable();
            ddlstate.Items.Clear();
            string sqlstr11;

            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                sqlstr11 = "Select Distinct I.StateCode,LS.Name StateName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY LS.Name ";
            }
            else
            {
                sqlstr11 = @"Select I.StateCode StateCode,LS.Name as StateName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
            }
            ddlstate.Items.Add("Select...");
            baseObj.BindToDropDown(ddlstate, sqlstr11, "StateName", "StateCode");
            if (ddlstate.Items.Count == 2)
                ddlstate.SelectedIndex = 1;
            ddlstate_SelectedIndexChanged(null, null);
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr1;
            ddldistributor.Items.Clear();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlstate.SelectedItem.Value + "' ORDER BY NAME";
            }
            else
            {
                sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
            }
            ddldistributor.Items.Add("Select...");
            baseObj.BindToDropDown(ddldistributor, sqlstr1, "Name", "Code");

            if (ddldistributor.Items.Count == 2)
                ddldistributor.SelectedIndex = 1;
            //  ddldistributor_SelectedIndexChanged(sender, e);

        }

        protected void ddldistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                string sqlstr1 = @"SELECT i.INVENTLOCATIONID as Code,i.NAME FROM ax.INVENTLOCATION i WHERE i.INVENTSITEID='" + ddldistributor.SelectedValue.ToString() + "';";
                //           ddcbWarehouse.Items.Clear();
                DataTable dt = new DataTable();
                dt = baseObj.GetData(sqlstr1);
                string sitecode = ddldistributor.SelectedValue.ToString();
                LoadWarehouse();
                //         ddcbWarehouse.DataSource = dt;
                //    string  = "Name";
                //     ddcbWarehouse.DataValueField = "Code";
                //   ddcbWarehouse.DataBind();

            }
        }
    }

}
