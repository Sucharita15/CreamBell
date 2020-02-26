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
    public partial class frmStockMoveToNonSaleble : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        string query;
        DataTable dt = new DataTable();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd;
        SqlTransaction transaction;
        string strQuery = string.Empty;
     
        protected void Page_Load(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["SiteCode"] != null)
                {
                    //==================For Warehouse Location==============                        
                    query = "SELECT TOP 1 MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                    DataTable dt = new DataTable();
                    dt = obj.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                    }
                }
                GetReceipt();
            }
        }

        private void ShowRecords(string PurchReceiptNumber)
        {

            DataTable dtLine = null;
            try
            {

                Int32 Movetype;
                Movetype = rdprimarytononsalable.Checked == true ? 1 : 0;    
                string  queryLine = "EXEC ACX_GET_NONSALABLEINVOICEDETAIL '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "','" + PurchReceiptNumber + "'," + Movetype;
                dtLine = obj.GetData(queryLine);

                if (dtLine.Rows.Count > 0)
                {
                    ViewState["dtgvDetails"] = dtLine;
                    gvDetails.DataSource = dtLine;
                    gvDetails.DataBind();
                    gvDetails.Visible = true;

                }
                else
                {
                    ViewState["dtgvDetails"] = null;
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                    LblMessage.Text = "No Line Items Exist";
                    // BtnUpdateHeader.Visible = false;
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void GetReceipt()
        {
            try
            {
                txtReceiptDate.Text = string.Empty;
                txtReceiptValue.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                Session["LineItem"] = null;
                gvDetails.DataSource = null;
                gvDetails.Visible = false;
                ViewState["dtgvDetails"] = null;

                Int32 Movetype;
                Movetype = rdprimarytononsalable.Checked == true ? 1 : 0;

                query = "EXEC ACX_GET_NONSALABLEINVOICE '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "'," + Movetype;
                dt = new DataTable();
                dt = obj.GetData(query);
                drpRecieptNo.DataSource = dt;
                drpRecieptNo.DataBind();
                ddlInvoceNo.DataSource = dt;
                ddlInvoceNo.DataBind();
                if (dt.Rows.Count > 0)
                {
                    drpRecieptNo.DataSource = dt;
                    drpRecieptNo.DataTextField = "PURCH_RECIEPTNO";
                    drpRecieptNo.DataValueField = "PURCH_RECIEPTNO";
                    drpRecieptNo.DataBind();
                    drpRecieptNo.Items.Insert(0, new ListItem("--Select--", "0"));

                    ddlInvoceNo.DataSource = dt;
                    ddlInvoceNo.DataTextField = "SALE_INVOICENO";
                    ddlInvoceNo.DataValueField = "SALE_INVOICENO";
                    ddlInvoceNo.DataBind();
                    ddlInvoceNo.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void drpRecieptNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddllist = (DropDownList)sender;
                if (ddllist.ID == "ddlInvoceNo")
                {
                    drpRecieptNo.SelectedIndex = ddlInvoceNo.SelectedIndex;
                }
                else
                {
                    ddlInvoceNo.SelectedIndex = drpRecieptNo.SelectedIndex;
                }

                query = "Select A.[DOCUMENT_NO],CONVERT(VARCHAR(11),A.[DOCUMENT_DATE] ,106) as [DOCUMENT_DATE], A.PURCH_INDENTNO, CONVERT(VARCHAR(11),A.[PURCH_INDENTDATE],106) AS PURCH_INDENTDATE, " +
                      " A.MATERIAL_VALUE, A.PURCH_RECIEPTNO, A.TRANSPORTER_CODE,A.VEHICAL_NO, A.VEHICAL_TYPE, A.[SALE_INVOICENO] AS INVOICENO, " +
                      " CONVERT(VARCHAR(11), A.[SALE_INVOICEDATE],106) AS INVOICEDATE from [ax].[ACXPURCHINVRECIEPTHEADER] A where A.[SITE_Code]='" + Session["SiteCode"].ToString() + "' " +
                      " and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and A.PURCH_RECIEPTNO='" + drpRecieptNo.SelectedItem.Text + "' ";

                dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    txtReceiptDate.Text = dt.Rows[0]["DOCUMENT_DATE"].ToString();
                    txtInvoiceDate.Text = dt.Rows[0]["INVOICEDATE"].ToString();
                    decimal materialValue = Convert.ToDecimal(dt.Rows[0]["MATERIAL_VALUE"].ToString());
                    txtReceiptValue.Text = (Math.Round(materialValue, 2)).ToString();
                }
                //================Fill Line Item=============
                ShowRecords(drpRecieptNo.SelectedItem.Text);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }


        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LblMessage.Text = "";
                bool b = Validation();
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    TextBox txtBoxQty = (TextBox)gvDetails.Rows[i].Cells[4].FindControl("txtQty");

                    if (txtBoxQty.Text == "")
                    {
                        string message = "alert('Qty cannot be left blank or Zero..!!');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        b = false;
                        return;
                    }
                    else if (Convert.ToDecimal(txtBoxQty.Text) < 0)
                    {
                        string message = "alert('Qty must be greater than or equal to zero..!!');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        b= false;
                        return;
                    }

                }
                if (b == true)
                {
                    conn = obj.GetConnection();
                    string strCode = string.Empty;
                    cmd = new SqlCommand();
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ACX_INVOICE_NSSTOCK_TRANSFER";
                    Int32 Movetype;
                    Movetype = rdprimarytononsalable.Checked == true ? 1 : 0;
                    string fromlocation, Tolocation;
                    fromlocation = Tolocation = "";
                    string query1;
                    DataTable dtloc = new DataTable();
                    query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "' ";
                    dtloc = obj.GetData(query1);
                    if (dtloc.Rows.Count > 0)
                    {
                        if (Movetype == 1)
                        { fromlocation = dtloc.Rows[0]["MainWarehouse"].ToString(); }
                        else
                        { Tolocation = dtloc.Rows[0]["MainWarehouse"].ToString(); }
                    }
                    query1 = "Select INVENTLOCATIONID from Ax.inventlocation where InventSiteid = '" + Session["SiteCode"].ToString() + "'  and ACX_WAREHOUSETYPE = 1";
                    dtloc = obj.GetData(query1);
                    if (dtloc.Rows.Count > 0)
                    {
                        if (Movetype == 1)
                        { Tolocation = dtloc.Rows[0]["INVENTLOCATIONID"].ToString(); }
                        else
                        { fromlocation = dtloc.Rows[0]["INVENTLOCATIONID"].ToString(); }
                    }

                    for (int i = 0; i < gvDetails.Rows.Count; i++)
                    {
                        TextBox txtBoxQty = (TextBox)gvDetails.Rows[i].Cells[4].FindControl("txtQty");
                        string strqty = txtBoxQty.Text;

                        if (Convert.ToDecimal(txtBoxQty.Text) > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                            cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                            cmd.Parameters.AddWithValue("@TransType", Movetype.ToString());
                            string productNameCode = gvDetails.Rows[i].Cells[2].Text;
                            string[] str = productNameCode.Split('-');

                            Label lbluom = (Label)gvDetails.Rows[i].Cells[7].FindControl("UT");
                            
                            cmd.Parameters.AddWithValue("@ProductCode", str[0].ToString());
                            cmd.Parameters.AddWithValue("@TransQty", Convert.ToDecimal(strqty).ToString());
                            cmd.Parameters.AddWithValue("@TransUOM", gvDetails.Rows[i].Cells[8].Text.ToString());
                            cmd.Parameters.AddWithValue("@TransFromLocation", fromlocation.ToString());
                            cmd.Parameters.AddWithValue("@TransToLocation", Tolocation.ToString());
                            cmd.Parameters.AddWithValue("@Referencedocumentno", ddlInvoceNo.Text.ToString());
                            cmd.ExecuteNonQuery();
                        }
                    }
                    
                    
                    transaction.Commit();
                    //transaction.Rollback();
                    ClearAll();
                    LblMessage.Text = "Stock Moved Successfully ..!";
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void SaveManualPurchaseReturnToInventTransTable(string PurcReturnCode, SqlTransaction trans, SqlConnection conn,int Qty)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";

                if (Qty < 0)
                {
                    
                    string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "' ";
                    DataTable dt = new DataTable();
                    dt = obj.GetData(query1);
                    if (dt.Rows.Count > 0)
                    {
                        TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                    }
                }
                else
                {
                    string query1 = "Select INVENTLOCATIONID from Ax.inventlocation where InventSiteid = '" + Session["SiteCode"].ToString() + "'  and ACX_WAREHOUSETYPE = 1";
                    DataTable dt = new DataTable();
                    dt = obj.GetData(query1);
                    if (dt.Rows.Count > 0)
                    {
                        TransLocation = dt.Rows[0]["INVENTLOCATIONID"].ToString();
                    }
                }
               

                string queryInsert = " Insert Into ax.acxinventTrans " +
                                 "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                 "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno])" +
                                 " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate, " +
                                 " @ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno)";

                cmd = new SqlCommand(queryInsert);
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                string st = Session["SiteCode"].ToString();

                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                    string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 8;                                          // Type 5 for Manual Purchase Return
                    int DocumentType = 8;    
                    string DocumentNo = PurcReturnCode;
                    string productNameCode = gvDetails.Rows[i].Cells[2].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    TextBox txtBoxQty = (TextBox)gvDetails.Rows[i].Cells[4].FindControl("txtQty");
                    string strqty = txtBoxQty.Text;
                    decimal TransQty = Convert.ToDecimal(strqty) * Qty;
                    string UOM = gvDetails.Rows[i].Cells[7].Text;
                    string Referencedocumentno = ddlInvoceNo.Text;// PurcReturnCode;
                    int REcid = i + 1;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TransId", TransId);
                    cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                    cmd.Parameters.AddWithValue("@RECID", i + 1);
                    cmd.Parameters.AddWithValue("@InventTransDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TransType", TransType);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType);
                    cmd.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                    cmd.Parameters.AddWithValue("@DocumentDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ProductCode", ProductCode);
                    cmd.Parameters.AddWithValue("@TransQty", TransQty);
                    cmd.Parameters.AddWithValue("@TransUOM", UOM);
                    cmd.Parameters.AddWithValue("@TransLocation", TransLocation);
                    cmd.Parameters.AddWithValue("@Referencedocumentno", ddlInvoceNo.SelectedItem.Value);
                    cmd.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        private void ClearAll()
        {
            // drpRecieptNo.Text = string.Empty;
            txtReceiptDate.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            ddlInvoceNo.SelectedIndex = 0;
            txtInvoiceDate.Text = string.Empty;
            Session["LineItem"] = null;
            gvDetails.DataSource = null;
            gvDetails.Visible = false;
            GetReceipt();
            ViewState["dtgvDetails"] = null;

        }

        private bool Validation()
        {
            bool returnvalue = true;

            if (drpRecieptNo.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Receipt No !');", true);

                drpRecieptNo.Focus();
                returnvalue = false;
                return returnvalue;
            }
            if (txtReceiptDate.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Receipt Date!');", true);
                txtReceiptDate.Focus();
                returnvalue = false;
                return returnvalue;
            }
            if (ddlInvoceNo.Text == "-Select-")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Invoice No!');", true);
                ddlInvoceNo.Focus();
                returnvalue = false;
                return returnvalue;
            }
            if (txtInvoiceDate.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Invoice Date!');", true);
                txtInvoiceDate.Focus();
                returnvalue = false;
                return returnvalue;
            }


            return returnvalue;
        }

        protected void txtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool check = true;
                GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;
                HiddenField h = (HiddenField)gvDetails.Rows[row.RowIndex].FindControl("HiddenValueLineNo");
                TextBox txtBox = (TextBox)sender;
                string productCode = row.Cells[2].Text;
                string[] str = productCode.Split('-');

                if (txtBox.Text == "" )
                {
                    string message = "alert('Qty cannot be left blank..!!');";
                    txtBox.Text = "0";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    check = false;
                }
                else if (Convert.ToDecimal(txtBox.Text) < 0)
                {
                    string message = "alert('Qty must be greater than zero..!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    txtBox.Text = "0";
                    check = false;
                }
                else
                {
                    #region  get data
                    DataTable dt = new DataTable();
                    if (ViewState["dtgvDetails"] != null)
                    {
                        dt = (DataTable)ViewState["dtgvDetails"];
                    }
                    else
                    {
                        Int32 Movetype;
                        Movetype = rdprimarytononsalable.Checked == true ? 1 : 0;
                        query = "EXEC ACX_GET_NONSALABLEINVOICE '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "'," + Movetype;
                        dt = obj.GetData(query);
                        ViewState["dtgvDetails"] = dt;
                    }
                    #endregion
                    #region Validate
                    DataRow[] dr = dt.Select("Line_no='" + h.Value + "' AND PRODUCT_CODE = '" + str[0] + "'");
                    if (dr.Length > 0)
                    {
                        if (Convert.ToDouble(dr[0]["ValidateBoxQty"]) < Convert.ToDouble(txtBox.Text))
                        {
                            string message = "alert('Moveable Qty cannot be more than quantity available to move.!!');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            txtBox.Text = "0";
                            txtBox.Focus();
                            check = false;
                        }
                        else
                        { 
                            check = true;
                            //dr[0]["BASICVALUE"] = Convert.ToString((Convert.ToDouble(dr[0]["BASICVALUE"])/Convert.ToDouble(dr[0]["ACTUAL_BOX"])) * Convert.ToDouble(txtBox.Text));
                            //dt.AcceptChanges();
                            //ViewState["dtgvDetails"] = dt;
                            row.Cells[9].Text = Convert.ToString(Math.Round(Convert.ToDouble(dr[0]["LTR"]),2));
                            //row.Cells[16].Text = Convert.ToString(Math.Round(Convert.ToDouble(dr[0]["VAT_INC_PERCVALUE"]), 2));
                            row.Cells[11].Text = Convert.ToString(Convert.ToDouble(dr[0]["PER_UT_RATE"]) * Convert.ToDouble(txtBox.Text));
                            
                        }
                    }
                    else
                    {
                        string message = "alert('Invalid quantity to move.!!');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        txtBox.Text = "0";
                        txtBox.Focus();
                        check = false;
                    }
                    #endregion
                }
                if (check == true)
                {


                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this,typeof(Page),"Validate","alert('" + ex.Message.ToString().Replace("'","") + "');",true);
                return;
            }
        }

        protected void rdprimarytononsalable_CheckedChanged(object sender, EventArgs e)
        {
            GetReceipt();
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
     

    }
}