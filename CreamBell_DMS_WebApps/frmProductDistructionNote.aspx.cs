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
    public partial class frmProductDistructionNote : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        string query;
        DataTable dt = new DataTable();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd,cmd1,cmd2;
        SqlTransaction transaction;
        string strQuery = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
           //LblMessage.Text = "";
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Session["ProductDetails"] = null;
                GetReceipt();
            }
        }

        private void GetReceipt()
        {
            try
            {               
                query = "EXEC ACX_GET_NONSALABLEINVOICE '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "',0";

                dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    drpRecieptNo.DataSource = dt;
                    drpRecieptNo.DataTextField = "PURCH_RECIEPTNO";
                    drpRecieptNo.DataValueField = "PURCH_RECIEPTNO";
                    drpRecieptNo.DataBind();
                    drpRecieptNo.Items.Insert(0, new ListItem("--Select--", "0"));

                    drpInvoceNo.DataSource = dt;
                    drpInvoceNo.DataTextField = "SALE_INVOICENO";
                    drpInvoceNo.DataValueField = "SALE_INVOICENO";
                    drpInvoceNo.DataBind();
                    drpInvoceNo.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
                
                App_Code.Global obj = new App_Code.Global();
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

                            
                Number = "DT"+ System.DateTime.Today.ToString("yy") + ((int)vc).ToString("000000");  
                #endregion

                #region insert into distruction table
                if (Session["ProductDetails"] != null)
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
                        cmd.Parameters.AddWithValue("@DISTRUCTION_NO", Number);
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@RECEIPT_NO", drpRecieptNo.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@RECEIPTDATE", txtReceiptDate.Text);
                        cmd.Parameters.AddWithValue("@INVOICE_NO", drpInvoceNo.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@INVOICEDATE", txtInvoiceDate.Text);
                        cmd.Parameters.AddWithValue("@LINENO", i);                        
                        cmd.Parameters.AddWithValue("@PRODUCTCODE", grv.Cells[1].Text);
                        cmd.Parameters.AddWithValue("@BOX", grv.Cells[3].Text);                      
                        cmd.Parameters.AddWithValue("@BATCHNO", grv.Cells[5].Text.Replace("&nbsp;",""));                        
                        cmd.Parameters.AddWithValue("@MFD", grv.Cells[6].Text);
                        cmd.Parameters.AddWithValue("@REMARK", grv.Cells[7].Text.Replace("&nbsp;", ""));
                        cmd.Parameters.AddWithValue("@PRICE", grv.Cells[8].Text);
                        cmd.Parameters.AddWithValue("@VALUE",Convert.ToDecimal(grv.Cells[9].Text));
                        cmd.Parameters.AddWithValue("@DISTRUCTIONTYPE", 0);
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
                    cmd2.Parameters.AddWithValue("@DISTRUCTIONTYPE", 0);                    
                    cmd2.Parameters.AddWithValue("@DISTRUCTION_NO", Number);

                    cmd2.ExecuteNonQuery();
                   
                    #endregion
                    
                    transaction.Commit();
                    ClearAll();
                    string message = "alert('Distruction No : " + Number + " Generated Successfully.!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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

        protected void drpInvoceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpRecieptNo.SelectedIndex = drpInvoceNo.SelectedIndex;
            GetInvoiceDetails();
            ClearData();
        }

        protected void GetInvoiceDetails()
        {
            #region  getHeaderDetails
            query = "Select A.[DOCUMENT_NO],CONVERT(VARCHAR(11),A.[DOCUMENT_DATE] ,106) as [DOCUMENT_DATE], A.PURCH_INDENTNO, CONVERT(VARCHAR(11),A.[PURCH_INDENTDATE],106) AS PURCH_INDENTDATE, " +
                      "A.MATERIAL_VALUE, A.PURCH_RECIEPTNO, A.TRANSPORTER_CODE,A.VEHICAL_NO, A.VEHICAL_TYPE, A.[SALE_INVOICENO] AS INVOICENO, " +
                      " CONVERT(VARCHAR(11), A.[SALE_INVOICEDATE],106) AS INVOICEDATE from [ax].[ACXPURCHINVRECIEPTHEADER] A where A.[SITE_Code]='" + Session["SiteCode"].ToString() + "' " +
                      "and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and A.PURCH_RECIEPTNO='" + drpRecieptNo.SelectedItem.Text + "' ";

            dt = new DataTable();
            dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                txtReceiptDate.Text = dt.Rows[0]["DOCUMENT_DATE"].ToString();
                txtInvoiceDate.Text = dt.Rows[0]["INVOICEDATE"].ToString();
               // decimal materialValue = Convert.ToDecimal(dt.Rows[0]["MATERIAL_VALUE"].ToString());
                txtReceiptValue.Text ="0.00";
            }

            #endregion

            #region  Product Details
            query = "exec ACX_GET_NONSALABLEINVOICEDETAIL '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "','" + drpRecieptNo.SelectedItem.Text + "',0";
            dt = new DataTable();
            dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLMaterialCode.DataSource = dt;
                DDLMaterialCode.DataTextField = "PRODUCTDESC";
                DDLMaterialCode.DataValueField = "PRODUCT_CODE";
                DDLMaterialCode.DataBind();
                DDLMaterialCode.Items.Insert(0, new ListItem("--Select--", "0"));
            }
            #endregion
        }

        protected void drpRecieptNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpInvoceNo.SelectedIndex = drpRecieptNo.SelectedIndex;
            GetInvoiceDetails();
            ClearData();
        
        }
        protected void ClearData()
        {
            Session["ProductDetails"] = null;
            DataTable dt = new DataTable();
            dt = null;
            gvDetails.DataSource = dt;
            gvDetails.DataBind();
            txtBoxQty.Text = "0";
            txtPcsQty.Text = "0";
            txtUsedQty.Text = "0";
            txtStockQty.Text = "0";
            txtBatch.Text = "";
            txtMDF.Text = "";
            txtRemark.Text = "";
            txtPrice.Text = "0";
            txtTaxAmt.Text = "0";
            txtValue.Text = "0";
            txtReceiptValue.Text = "0";
        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            bool valid = true;           
            valid = ValidateLineItemAdd();           
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["ProductDetails"] as DataTable;
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

        private DataTable AddLineItems()
        {
            try
            {
                if (Session["ProductDetails"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["ProductDetails"];
                }

                #region Add To Session

                DataRow row;
                row = dtLineItems.NewRow();
                int count = 1 + dtLineItems.Rows.Count;
                row["SNO"] = count;
                row["ProductCode"] = DDLMaterialCode.SelectedItem.Value.ToString();              
                row["ProductName"] = DDLMaterialCode.SelectedItem.Text.ToString();
                row["QtyBox"] = Convert.ToDecimal(txtBoxQty.Text.Trim().ToString());
                row["QtyPcs"] = decimal.Parse(txtPcsQty.Text.Trim().ToString());
                row["BatchNo"] = txtBatch.Text.Trim().ToString();
                row["MFD"] = txtMDF.Text;
                row["Remark"] =txtRemark.Text.Trim().ToString();
                row["Price"] = Convert.ToDecimal(txtPrice.Text.Trim().ToString());
                row["Value"] = Convert.ToDecimal(txtValue.Text.Trim().ToString());              
                dtLineItems.Rows.Add(row);
                Session["ProductDetails"] = dtLineItems;
                                
                #endregion
                GetUsedQty();
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
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
                if (Session["ProductDetails"]!=null)
                {
                    DataTable dtTotal = new DataTable();
                    dtTotal = (DataTable)Session["ProductDetails"];
                    if (dtTotal.Rows.Count > 0)
                    {
                        string sqty = Convert.ToString(dtTotal.Compute("Sum(QtyBox)", "ProductCode ='" + DDLMaterialCode.SelectedItem.Value + "'"));
                        if (sqty == "")
                            sqty = "0";
                        decimal sumQty = Convert.ToDecimal(sqty);
                        txtUsedQty.Text = sumQty.ToString();
                        decimal sumAmount = Convert.ToDecimal(dtTotal.Compute("Sum(Value)",""));
                        txtReceiptValue.Text = sumAmount.ToString("0.00");
                    }
                }                                
            }
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
            if (txtValue.Text == string.Empty)
            {
                txtValue.Text = "0";
            }
            if (txtBoxQty.Text == string.Empty || txtBoxQty.Text == "0")
            {
                b = false;                
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtBoxQty.Focus();
                return b;
            }   
            if (txtPcsQty.Text == string.Empty)
            {
                txtPcsQty.Text="0";               
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
            foreach (GridViewRow grv in gvDetails.Rows)
            {
                DataTable dt = new DataTable();
                if (Session["ProductDetails"]!=null)
                {
                    dt = (DataTable)Session["ProductDetails"];
                    if(dt.Rows.Count>0)
                    {
                        string sqty = Convert.ToString(dt.Compute("Sum(QtyBox)", "ProductCode ='" + DDLMaterialCode.SelectedItem.Value + "'"));
                        if (sqty == "")
                            sqty = "0";
                        decimal sumQty = Convert.ToDecimal(sqty) + Convert.ToDecimal(txtBoxQty.Text);
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
            if (Convert.ToDecimal(txtValue.Text) <= 0)
            {
                string message = "alert('Claim value should be more than zero.!!');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", message, true);
                b = false;
                return b;
            }
            return b;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        protected void ClearAll()
        {
            txtBatch.Text = string.Empty;
            txtBoxQty.Text = string.Empty;
            txtInvoiceDate.Text = string.Empty;
            txtMDF.Text = string.Empty;
            txtPcsQty.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtReceiptDate.Text = string.Empty;
            txtStockQty.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            txtPerBoxValue.Text = string.Empty;
            txtTotalTax.Text = string.Empty;
            Session["ProductDetails"] = null;
            DDLMaterialCode.Items.Clear();
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            GetReceipt();
            txtTaxAmt.Text = string.Empty;
            txtActualQty.Text = string.Empty;
            txtUsedQty.Text = string.Empty;
        }

        protected void ClearAfterAdd()
        {
            txtBatch.Text = string.Empty;
            txtBoxQty.Text = string.Empty;            
            txtMDF.Text = string.Empty;
            txtPcsQty.Text = string.Empty;           
            txtValue.Text = string.Empty;
            txtRemark.Text = string.Empty;           
            txtTaxAmt.Text = string.Empty;
            
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region  Stock Details
            query = "exec ACX_GET_NONSALABLEINVOICEDETAIL '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "','" + drpRecieptNo.SelectedItem.Text + "',0";
            dt = new DataTable();
            dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DataRow[] dr = dt.Select(" PRODUCT_CODE = '" + DDLMaterialCode.SelectedItem.Value + "'");
                if (dr.Length > 0)
                {
                    txtBoxQty.Text = "0";
                    txtPcsQty.Text = "0";
                    txtUsedQty.Text = "0";
                    txtBatch.Text = "";
                    txtMDF.Text = "";
                    txtRemark.Text = "";

                     txtStockQty.Text=dr[0]["ValidateBoxQty"].ToString() ;
                     decimal Price= Convert.ToDecimal(dr[0]["Rate"].ToString());
                     txtPrice.Text = Price.ToString("0.00");
                     txtPerBoxValue.Text = dr[0]["PER_UT_RATE"].ToString();
                     txtTotalTax.Text = dr[0]["VAT_INC_PERCVALUE"].ToString();
                     txtActualQty.Text = dr[0]["ACTUAL_BOX"].ToString();    
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
                    txtPrice.Text = "0";
                    txtPerBoxValue.Text = "0";
                    txtTaxAmt.Text = "0";
                    txtTotalTax.Text = "0";
                    txtActualQty.Text = "0";
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
                txtPrice.Text = "0";
                txtPerBoxValue.Text = "0";
                txtTaxAmt.Text = "0";
                txtTotalTax.Text = "0";
                txtActualQty.Text = "0";
            }
            #endregion
            GetUsedQty();
        }

        protected void txtBoxQty_TextChanged(object sender, EventArgs e)
        {
            #region  Product Details   
            if (txtBoxQty.Text=="")
            {
                txtBoxQty.Text = "0";
            }
            if (txtStockQty.Text=="")
            {
                txtStockQty.Text = "0";
            }
            if (txtPcsQty.Text == "")
            {
                txtPcsQty.Text = "0";
            }
            if (Convert.ToDecimal(txtBoxQty.Text) < 0)
            {
             
                string message = "alert('Distruction qty box cannot be less than zero!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtBoxQty.Focus();
                return ;
            }
            if (Convert.ToDecimal(txtPcsQty.Text) < 0)
            {
                string message = "alert('Distruction qty pcs cannot be less than zero!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                if (txtPcsQty.Visible==true)
                txtPcsQty.Focus();
                return;
            }
            if (Convert.ToDouble(txtStockQty.Text) < Convert.ToDouble(txtBoxQty.Text))
            {
                string message = "alert('Distruction Qty cannot be more than quantity available to move.!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                txtBoxQty.Text = "0";
                txtBoxQty.Focus();
            }
            else
            {
                txtValue.Text = (Convert.ToDecimal(txtPerBoxValue.Text) * Convert.ToDecimal(txtBoxQty.Text.Trim())).ToString("0.00");
                if (Convert.ToDecimal(txtActualQty.Text) > 0)
                {
                    txtTaxAmt.Text = ((Convert.ToDecimal(txtTotalTax.Text) / (Convert.ToDecimal(txtActualQty.Text))) * Convert.ToDecimal(txtBoxQty.Text.Trim())).ToString("0.00");
                }
                else
                {
                    txtTaxAmt.Text = "0";
                }
            }                 
            #endregion            
        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;           
            LinkButton lnk = sender as LinkButton;
            string Product = gvrow.Cells[1].Text;
            string Batch = gvrow.Cells[5].Text.Replace("&nbsp;", "");

            if (gvDetails.Rows.Count>0)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ProductDetails"];
                //dt.AsEnumerable().Where(r => r.Field<string>("PRODUCTCODE") == Product).ToList().ForEach(row => row.Delete());
               // dt.AsEnumerable().Where(r => r.Field<string>("PRODUCTCODE") == Product).Concat(r.Field<string>("PRODUCTCODE") == Product)).ToList().ForEach(row => row.Delete());
                DataRow[] dr = dt.Select("PRODUCTCODE='"+Product+"' and BatchNo='"+ Batch +"'");
                foreach(DataRow drrow in dr)
                {
                    drrow.Delete();
                }
                dt.AcceptChanges();
                Session["ProductDetails"] = null;
                Session["ProductDetails"] = dt;
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }
            GetUsedQty();

        }

        protected void txtUsedQty_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
