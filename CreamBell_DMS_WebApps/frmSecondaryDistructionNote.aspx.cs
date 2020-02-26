using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSecondaryDistructionNote : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        
        string query;
        DataTable dt = new DataTable();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2;
        SqlTransaction transaction;
        string strQuery = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {

                ClearAll();
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                ddlBusinessUnit.Items.Add("All...");
                obj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
                //Session["SecProductDetails"] = null;
                //Session["MarketDistructionDetails"] = null;
                GetProduct();
                //GetCustomerGroup();
            }
        }

        protected void GetCustomerGroup()
        {
            strQuery = "Select CUSTGROUP_CODE+'-'+CUSTGROUP_NAME as Name,CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0";
            ddlCustomerGroup.Items.Clear();
            ddlCustomerGroup.Items.Add("Select...");
            obj.BindToDropDown(ddlCustomerGroup, strQuery, "Name", "CUSTGROUP_CODE");
            ddlCustomer.Items.Add("Select...");
        }

        protected void GetCustomer()
        {
            //strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'" +
            //               " Union  All " +
            //               " Select Name=(Select A.Customer_Code+'-'+A.Customer_Name as Name  from ax.ACXCUSTMASTER A where A.Blocked = 0 and B.SubDistributor=A.Customer_Code AND A.CUST_GROUP='" + ddlCustomerGroup.SelectedValue + "' ),B.SubDistributor as Customer_Code " +
            //               " from ax.ACX_SDLinking B Where B.Other_Site='" + Session["SiteCode"].ToString() + "'";

            //ddlCustomer.Items.Clear();           
            //ddlCustomer.Items.Add("Select...");
            //obj.BindToDropDown(ddlCustomer, strQuery, "Name", "Customer_Code");

            DataTable dt = new DataTable();
            dt = (DataTable)Session["MarketDistructionDetails"];
            var Value = dt.Select("Cust_Group ='" + ddlCustomerGroup.Text.Trim() + "'");
            dt= Value.CopyToDataTable();
            
            var distValue = dt.DefaultView.ToTable(true, "CUSTOMER_CODE", "CUSTOMER_NAME");
            DataTable dtCust = new DataTable();
            dtCust = (DataTable)distValue;
            
            if (dtCust.Rows.Count > 0)
            {
                ddlCustomer.DataSource = dtCust;
                ddlCustomer.DataTextField = "CUSTOMER_NAME";
                ddlCustomer.DataValueField = "CUSTOMER_CODE";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, new ListItem("--Select--", "0"));
            }

        }

        protected void GetProduct()
        {
            #region  Product Details
            string bucode;
            if (ddlBusinessUnit.SelectedIndex==0)
            {
                bucode = "";
            }
            else
                bucode = ddlBusinessUnit.SelectedValue;
            dt = new DataTable();
            if (rdoColdRoomDistruction.Checked==true)
            {
                query = "exec ACX_GETDISTRUCTIONPRODUCTDETAILS '" + Session["SiteCode"].ToString() + "',0,'"+ bucode + "'";
                dt = obj.GetData(query);
            }
            else if (rdoMarketReturnDistruction.Checked==true)
            {
               // query = "exec ACX_GETDISTRUCTIONPRODUCTDETAILS '" + Session["SiteCode"].ToString() + "',1";

                dt = (DataTable)Session["MarketDistructionDetails"];
                if (dt != null)
                {
                    var Value = dt.Select("Cust_Group ='" + ddlCustomerGroup.Text.Trim() + "' AND CUSTOMER_CODE ='" + ddlCustomer.Text.Trim() + "'");
                    dt = Value.CopyToDataTable();

                    var distValue = dt.DefaultView.ToTable(true, "PRODUCT_NAME", "PRODUCTCODE");
                    DataTable dtCust = new DataTable();
                    dtCust = (DataTable)distValue;
                    dt = dtCust;
                }
                
            }
            
           
            if (dt!=null)
            {
                DDLMaterialCode.DataSource = dt;
                DDLMaterialCode.DataTextField = "PRODUCT_NAME";
                DDLMaterialCode.DataValueField = "PRODUCTCODE";
                DDLMaterialCode.DataBind();
                DDLMaterialCode.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            #endregion
        }
      
        protected void rdMarketReturnDistruction_CheckedChanged(object sender, EventArgs e)
        {
            ClearAll();
            lblBU.Visible = false;
            ddlBusinessUnit.Visible = false;
            #region  Customer Group Details
            Session["MarketDistructionDetails"] = null;
           
            query = "exec ACX_GETDISTRUCTIONPRODUCTDETAILS_MARKET '" + Session["SiteCode"].ToString() + "',1";            
            dt = new DataTable();
            dt = obj.GetData(query);
            Session["MarketDistructionDetails"] = dt;

            if (dt.Rows.Count > 0)
            {
                var distValue = dt.DefaultView.ToTable(true, "Cust_Group", "CUSTGROUP_NAME");
                DataTable dtCustGr = new DataTable();
                dtCustGr = (DataTable)distValue;

                if (dtCustGr.Rows.Count > 0)
                {
                    ddlCustomerGroup.DataSource = dtCustGr;
                    ddlCustomerGroup.DataTextField = "Custgroup_Name";
                    ddlCustomerGroup.DataValueField = "Cust_Group";
                    ddlCustomerGroup.DataBind();
                    ddlCustomerGroup.Items.Insert(0, new ListItem("--Select--", "0"));
                }
               
            }
            
            #endregion
        }

        protected void txtPcsQty_TextChanged(object sender, EventArgs e)
        {
            BoxCalculation();
        }

        protected string GetCurrentAllowedClaim()
        {
            string AllowedClaim;
            AllowedClaim = "0";
            DataTable DtAmount = new DataTable();
            if (Session["DistructionDetails"]!=null)
            {
                DtAmount = (DataTable)Session["DistructionDetails"];
                AllowedClaim = Convert.ToString(DtAmount.Compute("Sum(CLAIM_CALCULATION)", "ProductCode ='" + DDLMaterialCode.SelectedItem.Value + "'"));
                if (Session["SecProductDetails"] != null)
                {
                    DataTable dtTotal = new DataTable();
                    dtTotal = (DataTable)Session["SecProductDetails"];
                    if (dtTotal.Rows.Count > 0)
                    {
                        //string RecAmt = Convert.ToString(dtTotal.Compute("Sum(Value)", "ProductCode ='" + DDLMaterialCode.SelectedItem.Value + "'"));
                        string RecAmt = Convert.ToString(dtTotal.Compute("Sum(Value)",""));
                        if (RecAmt == "")
                            RecAmt = "0";
                        AllowedClaim = (Convert.ToDecimal(AllowedClaim) - Convert.ToDecimal(RecAmt)).ToString("0.00");
                    }
                }    
            }
            return AllowedClaim;
        }
        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region  Stock Details

            dt = new DataTable();
            if (rdoColdRoomDistruction.Checked == true)
            {
                query = "exec ACX_GETDISTRUCTIONPRODUCTDETAILS '" + Session["SiteCode"].ToString() + "',0";
                dt = obj.GetData(query);
            }
            else if (rdoMarketReturnDistruction.Checked == true)
            {
                //query = "exec ACX_GETDISTRUCTIONPRODUCTDETAILS_MARKET '" + Session["SiteCode"].ToString() + "',1";
                //dt = obj.GetData(query);
                dt = (DataTable)Session["MarketDistructionDetails"];
                if (dt.Rows.Count > 0)
                {
                    var Value = dt.Select("Cust_Group ='" + ddlCustomerGroup.Text.Trim() + "' AND CUSTOMER_CODE ='" + ddlCustomer.Text.Trim() + "' and PRODUCTCODE = '" + DDLMaterialCode.SelectedItem.Value + "'");
                    dt = Value.CopyToDataTable();
                                       
                }
            }
           
           
            Session["DistructionDetails"] = dt; 

            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select(" PRODUCTCODE = '" + DDLMaterialCode.SelectedItem.Value + "'");
                if (dr.Length > 0)
                {
                    txtBoxQty.Text = "0";
                    txtPcsQty.Text = "0";
                    txtUsedQty.Text = "0";
                    txtStockQty.Text = Convert.ToDecimal(dr[0]["NS_AVL_QTY"]).ToString("0.00");
                    txtBatch.Text = "";
                    txtMDF.Text = "";
                    txtRemark.Text = "";
                    decimal Price = Convert.ToDecimal(dr[0]["BASIC_Rate"].ToString());
                    txtPrice.Text = Price.ToString("0.00");
                    txtValue.Text = "0";
                    txtActualClaim.Text = "0";
                    txtClaimAllowedPer.Text = Convert.ToDecimal(dr[0]["ALLOWED_PER"]).ToString("0.00");
                    txtClaimReceivd.Text = Convert.ToDecimal(dr[0]["CLAIM_RECEIVED"]).ToString("0.00");
                    txtPackSize.Text = Convert.ToDecimal(dr[0]["PRODUCT_PACKSIZE"]).ToString("0.00");
                    txtClaimCalculation.Text = Convert.ToDecimal(GetCurrentAllowedClaim()).ToString("0.00"); // Convert.ToDecimal(dr[0]["Claim_Calculation"]).ToString("0.00");
                }
                else
                {
                    txtBoxQty.Text = "0";
                    txtPcsQty.Text = "0";
                    txtUsedQty.Text = "0.00";
                    txtStockQty.Text = "0.00";
                    txtBatch.Text = "";
                    txtMDF.Text = "";
                    txtRemark.Text = "";
                    txtPrice.Text = "0.00";
                    txtValue.Text = "0.00";
                    txtActualClaim.Text = "0.00";
                    txtClaimAllowedPer.Text = "0.00";
                    txtClaimReceivd.Text = "0.00";
                    txtPackSize.Text = "0";
                    txtClaimCalculation.Text = "0.00";
                }
            }
            else
            {
                txtBoxQty.Text = "0";
                txtPcsQty.Text = "0";
                txtUsedQty.Text = "0";
                txtStockQty.Text = "0";
                txtBatch.Text = "";
                txtMDF.Text = "";
                txtRemark.Text = "";
                txtPrice.Text = "0.00";
                txtValue.Text = "0.00";
                txtActualClaim.Text = "0.00";
                txtClaimAllowedPer.Text = "0.00";
                txtClaimReceivd.Text = "0.00";
                txtPackSize.Text = "0";
                txtClaimCalculation.Text = "0.00";
            }
            #endregion
            GetUsedQty();
        }

        private void GetUsedQty()
        {
            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "--Select--")
            {
                txtUsedQty.Text = "0";
                txtReceiptValue.Text = "0.00";
            }
            else
            {
                if (Session["SecProductDetails"] != null)
                {
                    DataTable dtTotal = new DataTable();
                    dtTotal = (DataTable)Session["SecProductDetails"];
                    if (dtTotal.Rows.Count > 0)
                    {
                        string sqty = Convert.ToString(dtTotal.Compute("Sum(QtyBox)", "ProductCode ='" + DDLMaterialCode.SelectedItem.Value + "'"));
                        if (sqty == "")
                            sqty = "0";

                        decimal sumQty = Convert.ToDecimal(sqty);
                        txtUsedQty.Text = sumQty.ToString("0.00");
                        // Sum of value is actual= sum of claim value
                        decimal sumAmount = Convert.ToDecimal(dtTotal.Compute("Sum(Value)", ""));
                        txtReceiptValue.Text = sumAmount.ToString("0.00");
                    }
                    else
                    {
                        txtUsedQty.Text = "0";
                        txtReceiptValue.Text = "0.00";
                    }
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        protected void ClearAll()
        {
            Session["MarketDistructionDetails"] = null;
            Session["SecProductDetails"] = null;

            ddlCustomerGroup.Items.Clear();
            ddlCustomer.Items.Clear();

            txtBoxQty.Text = string.Empty;
            txtPcsQty.Text = string.Empty;
            txtUsedQty.Text = string.Empty;
            txtStockQty.Text = string.Empty;
            txtBatch.Text = string.Empty;
            txtMDF.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtActualClaim.Text = string.Empty;          
            txtValue.Text = string.Empty;

            txtActualClaim.Text = string.Empty;
            txtClaimReceivd.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            txtPackSize.Text = string.Empty;
            txtClaimCalculation.Text = string.Empty;
            txtClaimAllowedPer.Text = string.Empty;
          
            DDLMaterialCode.Items.Clear();
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            GetProduct();
            AddColumnInDataTable();

            if (rdoMarketReturnDistruction.Checked==true)
            {
                txtStockQty.Visible = false;
                lblStockQty.Visible = false;
                
                lblCustomer.Visible = true;
                lblcustgr.Visible = true;
                ddlCustomerGroup.Visible = true;
                ddlCustomer.Visible = true;
            }
            else
            {
                txtStockQty.Visible = true;
                lblStockQty.Visible = true;
                
                lblCustomer.Visible = false;
                lblcustgr.Visible = false;
                ddlCustomerGroup.Visible = false;
                ddlCustomer.Visible = false;
            }
        }

        protected void ClearAfterAdd()
        {            
            txtPcsQty.Text = string.Empty;
            txtBoxQty.Text = string.Empty;
            txtBatch.Text = string.Empty;
            txtMDF.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtActualClaim.Text = string.Empty;           
        }

        protected void txtBoxQty_TextChanged(object sender, EventArgs e)
        {
            BoxCalculation();
        }

        protected void BoxCalculation()
        {
            #region  Product Details
            if (txtBoxQty.Text == "")
            {
                txtBoxQty.Text = "0";
            }
            if (txtPcsQty.Text == "")
            {
                txtPcsQty.Text = "0";
            }
            if (txtStockQty.Text == "")
            {
                txtStockQty.Text = "0";
            }
            if (txtPackSize.Text == "" || txtPackSize.Text == "0")
            {
                txtPackSize.Text = "1";
            }
            if (Convert.ToDecimal(txtBoxQty.Text) < 0)
            {
                string message = "alert('Distruction Qty box cannot be less than zero.!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtBoxQty.Text = "0";
                txtBoxQty.Focus();
                return;
            }
            if (Convert.ToDecimal(txtPcsQty.Text) < 0)
            {
                string message = "alert('Distruction Qty pcs cannot be less than zero.!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtPcsQty.Text = "0";
                txtPcsQty.Focus();
                return;
            }
            decimal totalBox = Convert.ToDecimal(txtBoxQty.Text) + (Convert.ToDecimal(txtPcsQty.Text) / Convert.ToDecimal(txtPackSize.Text));
            if (rdoColdRoomDistruction.Checked == true)
            {
                if (Convert.ToDecimal(txtStockQty.Text) < totalBox)
                {
                    string message = "alert('Distruction Qty cannot be more than quantity available to move.!!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    txtBoxQty.Text = "0";
                    txtBoxQty.Focus();
                    return;
                }
            }
            txtValue.Text = (Convert.ToDecimal(txtPrice.Text) * totalBox).ToString("0.00");
            

            if (Convert.ToDecimal(txtClaimCalculation.Text) >= Convert.ToDecimal(txtValue.Text))
            {
                txtActualClaim.Text = Convert.ToDecimal(txtValue.Text).ToString("0.00");
            }
            else
            {
                txtActualClaim.Text = Convert.ToDecimal(txtClaimCalculation.Text).ToString("0.00");
            }
            
            #endregion            
        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            bool valid = true;
            valid = ValidateLineItemAdd();
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["SecProductDetails"] as DataTable;
                dt = AddLineItems();
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = true;
                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = false;
                }
                ClearAfterAdd();
            }
        }

        private bool ValidateLineItemAdd()
        {
            bool b = true;

            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "--Select--")
            {
                string message = "alert('Select Product First !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                DDLMaterialCode.Focus();
                b = false;
                return b;
            }
            if (txtBatch.Text == string.Empty)
            {
                string message = "alert('Please Provide Batch No !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtBatch.Focus();
                b = false;
                return b;
            }
            if (txtMDF.Text == string.Empty)
            {
                string message = "alert('Please Provide MDF !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtMDF.Focus();
                b = false;
                return b;
            }
            decimal totalBox = Convert.ToDecimal(txtBoxQty.Text) + (Convert.ToDecimal(txtPcsQty.Text)/ Convert.ToDecimal(txtPackSize.Text));
            if (totalBox ==0)
            {
                b = false;
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtBoxQty.Focus();
                return b;
            }
            if (txtPcsQty.Text == string.Empty)
            {
                txtPcsQty.Text = "0";
            }
            if (txtPackSize.Text == "" || txtPackSize.Text == "0")
            {
                txtPackSize.Text = "1";
            }
            if (Convert.ToDecimal(txtBoxQty.Text) < 0)
            {
                b = false;
                string message = "alert('Distruction qty box cannot be less than zero!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtBoxQty.Focus();
                return b;
            }
            if (Convert.ToDecimal(txtPcsQty.Text) < 0)
            {
                b = false;
                string message = "alert('Distruction qty pcs cannot be less than zero!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtPcsQty.Focus();
                return b;
            }
            if (txtMDF.Text == string.Empty)
            {
                txtMDF.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
            }
            if (rdoColdRoomDistruction.Checked == true)
            {
                foreach (GridViewRow grv in gvDetails.Rows)
                {
                    DataTable dt = new DataTable();
                    if (Session["SecProductDetails"] != null)
                    {
                        dt = (DataTable)Session["SecProductDetails"];
                        if (dt.Rows.Count > 0)
                        {
                            string sqty = Convert.ToString(dt.Compute("Sum(QtyBox)", "ProductCode ='" + DDLMaterialCode.SelectedItem.Value + "'"));
                            if (sqty == "")
                                sqty = "0";
                            decimal sumQty = Convert.ToDecimal(sqty) + Convert.ToDecimal(txtBoxQty.Text) + (Convert.ToDecimal(txtPcsQty.Text) / Convert.ToDecimal(txtPackSize.Text));
                            if (sumQty > Convert.ToDecimal(txtStockQty.Text))
                            {
                                string message = "alert('Distruction Qty cannot be more than quantity available to move.!!');";
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", message, true);
                                b = false;
                                return b;
                            }
                        }

                    }

                }
            }
            if (rdoMarketReturnDistruction.Checked == true)
            {
                if (Convert.ToDecimal(txtActualClaim.Text) <= 0)
                {
                    string message = "alert('Claim amount should be more than zero.!!');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", message, true);
                    b = false;
                    return b;
                }
            }
            return b;
        }

        private void AddColumnInDataTable()
        {
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            column.ColumnName = "SNO";
            //-----------------------------------------------------------//
            dtLineItems = new DataTable("LineItemTable");
            dtLineItems.Columns.Add(column);
            dtLineItems.Columns.Add("ProductCode", typeof(string));
            dtLineItems.Columns.Add("ProductName", typeof(string));
            dtLineItems.Columns.Add("QtyBox", typeof(decimal));
            dtLineItems.Columns.Add("QtyPcs", typeof(int));
            dtLineItems.Columns.Add("BatchNo", typeof(string));
            dtLineItems.Columns.Add("MFD", typeof(DateTime));
            dtLineItems.Columns.Add("Remark", typeof(string));
            dtLineItems.Columns.Add("Price", typeof(decimal));
            dtLineItems.Columns.Add("Value", typeof(decimal));
        }

        private DataTable AddLineItems()
        {
            try
            {
                if (Session["SecProductDetails"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["SecProductDetails"];
                }

                #region Add To Session

                DataRow row;
                row = dtLineItems.NewRow();
                int count = 1 + dtLineItems.Rows.Count;
                row["SNO"] = count;
                row["ProductCode"] = DDLMaterialCode.SelectedItem.Value.ToString();
                row["ProductName"] = DDLMaterialCode.SelectedItem.Text.ToString();
                decimal totalBox = Convert.ToDecimal(txtBoxQty.Text) + (Convert.ToDecimal(txtPcsQty.Text) / Convert.ToDecimal(txtPackSize.Text));
                row["QtyBox"] = totalBox;//Convert.ToDecimal(txtBoxQty.Text.Trim().ToString());
                row["QtyPcs"] = decimal.Parse(txtPcsQty.Text.Trim().ToString());
                row["BatchNo"] = txtBatch.Text.Trim().ToString();
                row["MFD"] = txtMDF.Text;
                row["Remark"] = txtRemark.Text.Trim().ToString();
                row["Price"] = Convert.ToDecimal(txtPrice.Text.Trim().ToString());
                row["Value"] = Convert.ToDecimal(txtActualClaim.Text.Trim().ToString());// Convert.ToDecimal(txtValue.Text.Trim().ToString());
                dtLineItems.Rows.Add(row);
                
                Session["SecProductDetails"] = dtLineItems;
                txtClaimCalculation.Text = Convert.ToDecimal(GetCurrentAllowedClaim()).ToString("0.00");
                #endregion
                GetUsedQty();
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int gvRow = gvDetails.Rows.Count;
            if (gvRow > 0)
            {
                SaveDetails();

            }
        }

        private void SaveDetails()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                //string Number="DT16000001";
                #region  getnTUMBER
                string Number = string.Empty;
                string query = "SELECT ISNULL(MAX(CAST(RIGHT(DISTRUCTION_NO,6) AS INT)),0)+1 FROM [ax].[ACXDISTRUCTIONNOTE] where SITEID='" + Session["SiteCode"].ToString() + "'";
                conn = obj.GetConnection();
                cmd1 = new SqlCommand(query);

                transaction = conn.BeginTransaction();
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.Text;
                object vc = cmd1.ExecuteScalar();


                Number = "DT" + System.DateTime.Today.ToString("yy") + ((int)vc).ToString("000000");
                #endregion

                #region insert into distruction table
                if (Session["SecProductDetails"] != null)
                {
                    //conn = obj.GetConnection();
                    cmd = new SqlCommand("ACX_INSERTDISTRUCTIONNOTE");
                    // transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;

                    int i = 0;
                    foreach (GridViewRow grv in gvDetails.Rows)
                    {
                        i = i + 1;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SITEID", Session["SITECODE"].ToString());
                        if(rdoMarketReturnDistruction.Checked==true)
                        {
                            cmd.Parameters.AddWithValue("@CUSTOMERCODE", ddlCustomer.SelectedItem.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@CUSTOMERCODE","");
                        }                        
                        cmd.Parameters.AddWithValue("@DISTRUCTION_NO", Number);
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@RECEIPT_NO","");
                        cmd.Parameters.AddWithValue("@RECEIPTDATE", "");
                        cmd.Parameters.AddWithValue("@INVOICE_NO", "");
                        cmd.Parameters.AddWithValue("@INVOICEDATE", "");
                        cmd.Parameters.AddWithValue("@LINENO", i);
                        cmd.Parameters.AddWithValue("@PRODUCTCODE", grv.Cells[1].Text);
                        cmd.Parameters.AddWithValue("@BOX", grv.Cells[3].Text);
                        cmd.Parameters.AddWithValue("@BATCHNO", grv.Cells[5].Text.Replace("&nbsp;", ""));
                        cmd.Parameters.AddWithValue("@MFD", grv.Cells[6].Text);
                        cmd.Parameters.AddWithValue("@REMARK", grv.Cells[7].Text.Replace("&nbsp;", ""));
                        cmd.Parameters.AddWithValue("@PRICE", grv.Cells[8].Text);
                        cmd.Parameters.AddWithValue("@VALUE", Convert.ToDecimal(grv.Cells[9].Text));                        
                        if (rdoMarketReturnDistruction.Checked==true)
                        {
                            cmd.Parameters.AddWithValue("@DISTRUCTIONTYPE", 2);
                        }
                        else 
                        {
                            cmd.Parameters.AddWithValue("@DISTRUCTIONTYPE", 1);
                        }                        
                        cmd.ExecuteNonQuery();
                    }
                #endregion

                    #region  insert into transaction table and claim table

                    cmd2 = new SqlCommand("ACX_DISTRUCTION_STOCK_MOVEMENT");
                    cmd2.Connection = conn;
                    cmd2.Transaction = transaction;
                    cmd2.CommandTimeout = 3600;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.Clear();
                    cmd2.Parameters.AddWithValue("@Siteid", Session["SITECODE"].ToString());
                     if(rdoColdRoomDistruction.Checked==true)
                     {
                         cmd2.Parameters.AddWithValue("@DISTRUCTIONTYPE", 1);                    
                     }
                     else
                     {
                         cmd2.Parameters.AddWithValue("@DISTRUCTIONTYPE", 2);                    
                     }                    
                    cmd2.Parameters.AddWithValue("@DISTRUCTION_NO", Number);
                    cmd2.ExecuteNonQuery();

                    #endregion

                    transaction.Commit();
                    ClearAll();
                    if (rdoMarketReturnDistruction.Checked == true)
                        rdMarketReturnDistruction_CheckedChanged(null, null);
                    string message = "alert('Distruction No : " + Number + " Generated Successfully.!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('Error:" + ex.Message + " !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open")
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }
       
        protected void rdoColdRoomDistruction_CheckedChanged(object sender, EventArgs e)
        {
            ClearAll();
            lblBU.Visible = true;
            ddlBusinessUnit.Visible = true;
        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            LinkButton lnk = sender as LinkButton;
            string Product = gvrow.Cells[1].Text;
            string Batch = gvrow.Cells[5].Text.Replace("&nbsp;", "");

            if (gvDetails.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["SecProductDetails"];               
                DataRow[] dr = dt.Select("PRODUCTCODE='" + Product + "' and BatchNo='" + Batch + "'");
                foreach (DataRow drrow in dr)
                {
                    drrow.Delete();
                }
                dt.AcceptChanges();
                txtClaimCalculation.Text = Convert.ToDecimal(GetCurrentAllowedClaim()).ToString("0.00");
                Session["SecProductDetails"] = null;
                Session["SecProductDetails"] = dt;
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }
            GetUsedQty();
        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProduct();
        }

        protected void ddlCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustomer();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProduct();
        }


    }
}