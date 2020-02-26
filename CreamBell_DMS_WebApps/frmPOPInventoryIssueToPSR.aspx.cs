using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPOPInventoryIssueToPSR : System.Web.UI.Page
    {
        string strmessage = string.Empty;
        int slno = 0;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        SqlDataAdapter adp1, adp2;
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new App_Code.Global();
        string strQuery = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            btnAdd.Focus();
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["SiteCode"] != null)
                {
                    string siteid = Session["SiteCode"].ToString();
                    if (lblsite.Text == "")
                    {
                        lblsite.Text = siteid;
                    }
                    Session["LineItem"] = null;
                }
                FillItemCategory();
                FillItemCode();
                FillPSRName();
               

            }
        }

       private void FillItemCode()
        {
            DrpItemDescription.Items.Clear();
            // DDLMaterialCode.Items.Add("Select...");
            if (drpItemCategory.Text == "Select..." && drpItemCode.Text == "Select..." || drpItemCode.Text == "")
            {
                strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE where block=0 and PRODUCT_GROUP ='POP' order by Product_Name";
                DrpItemDescription.Items.Clear();
                DrpItemDescription.Items.Add("Select...");
                baseObj.BindToDropDown(DrpItemDescription, strQuery, "Product_Name", "Product_Code");
                DrpItemDescription.Focus();
            }
        }
        private void FillPSRName()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = "select PSRCODE,PSRCODE +' - ' + PSR_Name+'' as PSR_Name from [ax].[ACXPSRSITELinkingMaster] A JOIN [ax].[ACXPSRMaster] B ON A.PSRCODE=B.PSR_Code where site_code='" + Session["SiteCode"] + "' and blocked=0";
            drpPSRName.Items.Clear();
            drpPSRName.Items.Add("-Select-");
            obj.BindToDropDown(drpPSRName, strQuery, "PSR_Name", "PSRCODE");
           }

        public void ProductSubCategory()
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P WHERE P.BLOCK=0"
                        + " and P.PRODUCT_GROUP='POP'";
            drpItemCode.Items.Clear();
            drpItemCode.Items.Add("Select...");
            baseObj.BindToDropDown(drpItemCode, strQuery, "Name", "Code");
            // FillProductCode();
            drpItemCode.Focus();
        }
        private void FillPSRContact()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            
            string strQuery = "select ContactNo FROM [ax].[ACXPSRMaster] where PSR_CODE  = '" + drpPSRName.SelectedItem.Value + "' ";
            string strCon = obj.GetScalarValue(strQuery);
            txtPSRContact.Text = strCon;
           

        }

        private void FillItemCategory()
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string strQuery = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE WHERE PRODUCT_GROUP='POP' AND BLOCK=0";
            drpItemCategory.Items.Clear();
            drpItemCategory.Items.Add("-Select-");
            obj.BindToDropDown(drpItemCategory, strQuery, "PRODUCT_GROUP", "PRODUCT_GROUP");

        }

        protected void drpItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P WHERE P.BLOCK=0"
                          + " and P.PRODUCT_GROUP='POP'";
            drpItemCode.Items.Clear();
            drpItemCode.Items.Add("Select...");
            DrpItemDescription.Items.Clear();
            DrpItemDescription.Items.Add("Select...");
            baseObj.BindToDropDown(drpItemCode, strQuery, "Name", "Code");
            // FillProductCode();
            drpItemCode.Focus();


        }

        protected void drpItemCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = "Select distinct P.ITEMID+'-'+P.Product_Name as Name,P.ITEMID from ax.InventTable P where Product_Group='POP' AND P.BLOCK=0 and P.PRODUCT_SUBCATEGORY ='" + drpItemCode.SelectedItem.Value + "'"; //--AND SITE_CODE='657546'";
            DrpItemDescription.DataSource = null;
            DrpItemDescription.Items.Clear();
            //DDLMaterialCode.Items.Add("Select...");
            baseObj.BindToDropDown(DrpItemDescription, strQuery, "Name", "ITEMID");
            DrpItemDescription.Items.Add("Select...");
            txtAvailableQuantity.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtQTYPcs.Text = string.Empty;
            
            DrpItemDescription.Enabled = true;
            //DDLMaterialCode.SelectedIndex = 0;
            //DDLMaterialCode_SelectedIndexChanged(sender, e);            
            DrpItemDescription.Focus();
            if (DrpItemDescription.SelectedItem.Text != "Select...")
            {
                DrpItemDescription_SelectedIndexChanged(null, null);
            }


        }

        protected void drpPSRName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPSRContact();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            //=============Validation=================
            foreach (GridViewRow grv in gvDetails.Rows)
            {

                HiddenField lblProduct = (HiddenField)grv.FindControl("HiddenValueItemCode");
                if (DrpItemDescription.SelectedItem.Value == lblProduct.Value)
                {

                    string message = "alert('" + DrpItemDescription.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);


                    return;
                }

            }

            //=================================
            //==============================================

          

            if (drpItemCode.Text == "Select..." || drpItemCode.Text == "")
            {
                string message = "alert('Select Item Sub Group First !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                drpItemCode.Focus();
                return;
            }
            if (drpItemCategory.Text == string.Empty || drpItemCategory.Text == "Select...")
            {
                string message = "alert('Select Item Group First !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                drpItemCategory.Focus();
                return;
            }

            string strQuery = "select isnull(sum(TransQty),0) from [ax].[ACXINVENTTRANS] where ProductCode='" + DrpItemDescription.SelectedItem.Value + "' and SiteCode='" + Session["SiteCode"] + "' and DocumentType='9'";
            string strTransQty = obj.GetScalarValue(strQuery);

            if (Convert.ToDecimal(txtQTYPcs.Text) >= Convert.ToDecimal(strTransQty))
            {

                string message = "alert('" + DrpItemDescription.SelectedItem.Value + " is not available in stock.Enter Quantity below than " + Math.Round(Convert.ToDecimal(strTransQty), 2) + "!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                txtQTYPcs.Focus();

                return;
            }
            if (txtQTYPcs.Text == string.Empty || txtQTYPcs.Text == "0")
            {
                string message = "alert(' Invalid Qty !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                return;
            }
            if (drpPSRName.Text == string.Empty || drpPSRName.Text == "Select...")
            {
                string message = "alert('Select PSR Name First !');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                drpItemCategory.Focus();
                return;
            }
            DataTable dt = new DataTable();
            dt = Session["ItemTable"] as DataTable;
            if (Session["LineItem"] == null)
            {
                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.AutoIncrement = true;
                column.AutoIncrementSeed = 1;
                column.AutoIncrementStep = 1;
                column.ColumnName = "Sr_No";
                //-----------------------------------------------------------//

                dtLineItems = new DataTable("LineItemTable");
                dtLineItems.Columns.Add(column);

                dtLineItems.Columns.Add("Item_Category", typeof(string));
                dtLineItems.Columns.Add("Item_SubCategory", typeof(string));
                dtLineItems.Columns.Add("Item_Code", typeof(string));
                dtLineItems.Columns.Add("Item_Name", typeof(string));
                dtLineItems.Columns.Add("QTY", typeof(string));
              

            }
            else
            {
                dtLineItems = (DataTable)Session["LineItem"];

            }
            DataRow row;
            row = dtLineItems.NewRow();

            row["Item_Category"] = drpItemCategory.Text;
            row["Item_SubCategory"] = drpItemCode.SelectedItem.Value;
            row["Item_Code"] = DrpItemDescription.SelectedItem.Value;
            row["Item_Name"] = DrpItemDescription.SelectedItem.Text;
            row["QTY"] = txtQTYPcs.Text;
           
            dtLineItems.Rows.Add(row);
            
            Session["LineItem"] = dtLineItems;
            dt = dtLineItems; ;

            if (dt.Rows.Count > 0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();

            }
            else
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();

            }



            btnAdd.Enabled = true;
            btnAdd.Attributes.Add("onclick", " this.disabled = false; " + ClientScript.GetPostBackEventReference(btnAdd, null) + ";");
            drpItemCategory.Focus();

        }


        public void clear()
        {
            txtPSRContact.Text = txtRemark.Text = txtQTYPcs.Text = "";
            txtAvailableQuantity.Text = "";
            txtRemark.Text = "";
            txtQTYPcs.Text="";
            drpPSRName.SelectedIndex = drpItemCategory.SelectedIndex = drpItemCode.SelectedIndex = DrpItemDescription.SelectedIndex = 0;
            drpItemCode.Items.Clear();
            gvDetails.DataSource = null;
            gvDetails.DataBind();


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    if (Session["LineItem"] != null)
                    {
                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                        conn = obj.GetConnection();
                        //when number seq is created then open the comment code
                        DataTable dtNumSeq = obj.GetNumSequenceNew(13, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");

                        string NUMSEQ = string.Empty;
                        //  string NUMSEQ = "POP-1617-000000";      //Hard Code
                        string Code = string.Empty;
                        if (dtNumSeq != null)
                        {
                            Code = dtNumSeq.Rows[0][0].ToString();
                            NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                        }
                        else
                        {
                            return;
                        }

                        cmd1 = new SqlCommand("[dbo].[ACX_POPISSUE_Line]");
                        transaction = conn.BeginTransaction();
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;

                        //=============Save Line=============
                        int i = 0;
                        foreach (GridViewRow grv in gvDetails.Rows)
                        {

                            HiddenField lblProduct = (HiddenField)grv.FindControl("HiddenValueItemCode");
                            Label QTY = (Label)grv.Cells[4].FindControl("QTY");
                          

                            i = i + 1;

                            string st = Session["SiteCode"].ToString();
                            string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");

                            cmd1.Parameters.Clear();
                            cmd1.Parameters.AddWithValue("@TransId", TransId);
                            cmd1.Parameters.AddWithValue("@Site_Code", lblsite.Text);
                            cmd1.Parameters.AddWithValue("@Line_No", i);
                            cmd1.Parameters.AddWithValue("@DOC_NO", Code);
                            cmd1.Parameters.AddWithValue("@ITEM_CODE", lblProduct.Value);
                            cmd1.Parameters.AddWithValue("@QTY_IN_PCS", QTY.Text);
                            cmd1.Parameters.AddWithValue("@NumSeq", NUMSEQ);
                            cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                            cmd1.ExecuteNonQuery();
                        }

                        // ==============Save Header=========
                        List<string> ilist = new List<string>();
                        List<string> litem = new List<string>();

                        cmd = new SqlCommand("[dbo].[ACX_InventoryIssue_POPHeader]");
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@PSR_CODE", drpPSRName.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@DOC_NO", Code);
                        cmd.Parameters.AddWithValue("@Remark", txtRemark.Text);
                        cmd.Parameters.AddWithValue("@NumSeq", NUMSEQ);
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        lblMessage.Text = "Data Saved Successfully. POP Issue Slip No is:" + Code + "";
                        clear();

                    }
                    else
                    {

                        string message = "alert('Please Add Line Items First !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string message = "alert('Error:" + ex.Message + " !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                Session["LineItem"] = null;
                if (conn.State.ToString() == "Open") { conn.Close(); }
            }
        }

        protected void txtQTYPcs_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            try
            {
                string strQuery = "select isnull(sum(TransQty),0) from [ax].[ACXINVENTTRANS] where ProductCode='" + DrpItemDescription.SelectedItem.Value + "' and SiteCode='" + Session["SiteCode"] + "' and DocumentType='9'";
                string strTransQty = obj.GetScalarValue(strQuery);
                if(strTransQty != "")
                {
                    if (Convert.ToDecimal(txtQTYPcs.Text) >= Convert.ToDecimal(strTransQty))
                    {
                      
                        string message = "alert('" + DrpItemDescription.SelectedItem.Value + " is not available in stock.Enter Quantity below than " + Math.Round(Convert.ToDecimal(strTransQty),2) + "!!');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        txtQTYPcs.Focus();
                       
                        return;
                    }
                    btnAdd.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                lblMessage.Text = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DrpItemDescription_SelectedIndexChanged(object sender, EventArgs e)
        { //string a = sender.ToString();
            //string b = e.ToString();
            //txtQtyBox.Focus();
            txtRemark.Text = string.Empty;
            txtQTYPcs.Text = string.Empty;
           

            //===========Fill Product Group and Product Sub Cat========
            DataTable dt = new DataTable();
            string query = "select Product_Group,PRODUCT_SUBCATEGORY  from ax.INVENTTABLE where ItemId='" + DrpItemDescription.SelectedItem.Value + "' order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                drpItemCategory.Text = dt.Rows[0]["Product_Group"].ToString();
                ProductSubCategory();
                //=============For Product Sub Cat======
                drpItemCode.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
         }
         FillAvailableQuantity();
        }
    
        public void  FillAvailableQuantity()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = "select Round(convert(decimal(18,2),isnull(sum(TransQty),0)),2) from [ax].[ACXINVENTTRANS] where ProductCode='" + DrpItemDescription.SelectedItem.Value + "' and SiteCode='" + Session["SiteCode"] + "' and DocumentType='9'";
            string strTransQty = obj.GetScalarValue(strQuery);
            txtAvailableQuantity.Text =strTransQty;
       }

       
    }
}