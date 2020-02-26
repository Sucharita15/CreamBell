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
    public partial class frmPurchaseReversal : System.Web.UI.Page
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
        static string SessionGrid = "GRReversalLine";

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
                    query = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                    DataTable dt = new DataTable();
                    dt = obj.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                    }
                }
                GetReceipt();
                GetReason();
            }

        }
        private void ShowRecords(string PurchReceiptNumber)
        {
            
            DataTable dtLine = null;
            try
            {
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                ilist.Add("@PURCH_RECIEPTNO"); litem.Add(PurchReceiptNumber);
                ilist.Add("@SITEID"); litem.Add(Session["SiteCode"].ToString());
                ilist.Add("@DATAAREAID"); litem.Add(Session["DATAAREAID"].ToString());

                dtLine = obj.GetData_New("usp_Get_PurchaseReversalLine", CommandType.StoredProcedure, ilist, litem);
                Session[SessionGrid] = dtLine;
                if (dtLine.Rows.Count > 0)
                {
                    //ViewState["dtgvDetails"] = dtLine;
                    gvDetails.DataSource = dtLine;
                    gvDetails.DataBind();
                    txtreversalVal.Text = Convert.ToDecimal(dtLine.Compute("SUM(RAMOUNT)", "")).ToString("0.0000");
                    gvDetails.Visible = true;
                }
                else
                {
                    gvDetails.DataSource = null ;
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
                query = "Select distinct([DOCUMENT_NO]),[DOCUMENT_DATE],[PURCH_RECIEPTNO],[SALE_INVOICENO], [SALE_INVOICEDATE]  from [ax].[ACXPURCHINVRECIEPTHEADER]  " +
                        " where [SITE_CODE]='"+ Session["SiteCode"].ToString() +"' and [DATAAREAID]='"+ Session["DATAAREAID"].ToString() +"' AND SALE_INVOICEDATE>='2017-07-01'"+
                        "and PURCH_RECIEPTNO not in (Select Purch_RECIEPTNO FROM [ax].[ACXPURCHRETURNHEADER] where SITE_CODE='" + Session["SiteCode"].ToString() + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "')" +
                        //" and invoice_No not in (select So_No from [ax].[ACXSALEINVOICEHEADER] where [Siteid]='" + lblsite.Text + "' and TranType=2)" +
                        "  order by [DOCUMENT_DATE] desc";

                dt = new DataTable();
                dt = obj.GetData(query);
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

        private void GetReason()
        {
            try
            {
                query = "Select distinct DAMAGEREASON_CODE,DAMAGEREASON_CODE + '-(' + DAMAGEREASON_NAME +')' as RETURNREASON  from [ax].[ACXDAMAGEREASON]";

                dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    drpReturnReason.DataSource = dt;
                    drpReturnReason.DataTextField = "RETURNREASON";
                    drpReturnReason.DataValueField = "DAMAGEREASON_CODE";
                    drpReturnReason.DataBind();
                    drpReturnReason.Items.Insert(0, new ListItem("--Select--", "0"));
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
                object objcheckproductcode = null;
                if (ddllist.ID == "ddlInvoceNo")
                {
                    query = "Select Top 1 A.PURCH_RECIEPTNO from [ax].[ACXPURCHINVRECIEPTHEADER] A where A.[SITE_Code]='"+ Session["SiteCode"].ToString() + "' " +
                        "and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and A.[SALE_INVOICENO]='" + ddlInvoceNo.SelectedItem.Value + "' ";

                     objcheckproductcode = obj.GetScalarValue(query);
                    
                     if (objcheckproductcode == null)
                     {
                         LblMessage.Text = "PURCH_RECIEPTNO Number Not Found";
                         return;
                     }
                     drpRecieptNo.SelectedIndex = drpRecieptNo.Items.IndexOf(drpRecieptNo.Items.FindByText(objcheckproductcode.ToString()));
                }
                else
                {
                //=================Fill Header Part===============
                    string sqlstr = "Select Top 1 [SALE_INVOICENO] from [ax].[ACXPURCHINVRECIEPTHEADER] A where A.[SITE_Code]='" + Session["SiteCode"].ToString() + "' " +
                       "and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and A.PURCH_RECIEPTNO='" + drpRecieptNo.SelectedItem.Text + "' ";

                    objcheckproductcode = obj.GetScalarValue(sqlstr);

                    if (objcheckproductcode == null)
                    {
                        LblMessage.Text = "Invoice Number Not Found";
                        return;
                    }
                    ddlInvoceNo.SelectedIndex = ddlInvoceNo.Items.IndexOf(ddlInvoceNo.Items.FindByText(objcheckproductcode.ToString()));
              
                }

                query = "Select A.[DOCUMENT_NO],CONVERT(VARCHAR(11),A.[DOCUMENT_DATE] ,106) as [DOCUMENT_DATE], A.PURCH_INDENTNO, CONVERT(VARCHAR(11),A.[PURCH_INDENTDATE],106) AS PURCH_INDENTDATE, " +
                      "A.MATERIAL_VALUE, A.PURCH_RECIEPTNO, A.TRANSPORTER_CODE,A.VEHICAL_NO, A.VEHICAL_TYPE, A.[SALE_INVOICENO] AS INVOICENO, " +
                      " CONVERT(VARCHAR(11), A.[SALE_INVOICEDATE],106) AS INVOICEDATE from [ax].[ACXPURCHINVRECIEPTHEADER] A where A.[SITE_Code]='" + Session["SiteCode"].ToString() + "' " +
                      "and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and A.PURCH_RECIEPTNO='" + drpRecieptNo.SelectedItem.Text + "' ";


                dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {

                    txtReceiptDate.Text = dt.Rows[0]["DOCUMENT_DATE"].ToString();
                   
                    //txtInvoiceNo.Text = dt.Rows[0]["INVOICENO"].ToString();
                    txtInvoiceDate.Text = dt.Rows[0]["INVOICEDATE"].ToString();
                    decimal materialValue = Convert.ToDecimal(dt.Rows[0]["MATERIAL_VALUE"].ToString());
                    txtReceiptValue.Text = (Math.Round(materialValue, 2)).ToString();
                    txtIndentNo.Text = dt.Rows[0]["PURCH_INDENTNO"].ToString();
                    txtIndentDate.Text = dt.Rows[0]["PURCH_INDENTDATE"].ToString();
                    txtTransporterName.Text = dt.Rows[0]["TRANSPORTER_CODE"].ToString();
                    txtVehicleNumber.Text = dt.Rows[0]["VEHICAL_NO"].ToString();
                    txtVehicleType.Text = dt.Rows[0]["VEHICAL_TYPE"].ToString();
                    //txtDriverNumber.Text = dt.Rows[0]["PURCH_INDENTDATE"].ToString();
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
            SaveData();
        }

        private void SaveData()
        {
            try
            {
                LblMessage.Text = "";
                bool b = Validation();
                if (b == true)
                {
                    conn = obj.GetConnection();
                    string strCode = string.Empty;
                    cmd = new SqlCommand();
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;


                    DataTable dtNumSeq = obj.GetNumSequenceNew(3, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                    string NUMSEQ = string.Empty;
                    if (dtNumSeq != null)
                    {
                        strCode = dtNumSeq.Rows[0][0].ToString();
                        NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                    }
                    else
                    {
                        return;
                    }
                    cmd.CommandText = string.Empty;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ACX_ACXPURCHINVREVERSAL";


                    #region Header Insert Data

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@SITE_CODE", Session["SiteCode"].ToString());
                    cmd.Parameters.AddWithValue("@PURCH_RETURNNO", strCode);
                    cmd.Parameters.AddWithValue("@PURCH_RECIEPTNO", drpRecieptNo.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtDriverNumber.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_NO", txtVehicleNumber.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_TYPE", txtVehicleType.Text);
                    cmd.Parameters.AddWithValue("@RETURN_REASONCODE", drpReturnReason.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@REMARK", txtRemark.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                    DataTable dtgrid = new DataTable();
                    if (Session[SessionGrid] != null)
                        dtgrid = (DataTable)Session[SessionGrid];
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Session expired! Reselect the receipt no.');", true);
                        LblMessage.Text = "Session expired! Reselect the receipt no.";
                        return;
                    }
                    #endregion
                    DataSet ds = new DataSet();
                    DataTable dtLineItem = new DataTable();
                    dtLineItem = dtgrid.Copy();
                    dtLineItem.TableName = "PurchReturn";
                    ds.Tables.Add(dtLineItem);
                    string ReversalLineXml = ds.GetXml();

                    cmd.Parameters.AddWithValue("@XmlData", ReversalLineXml);
                    cmd.ExecuteNonQuery();
                    //using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    //{
                    //    using (DataTable dtt = new DataTable())
                    //    {
                    //        sda.Fill(dtt);
                    //    }
                    //}
                    //SaveManualPurchaseReturnToInventTransTable(strCode, transaction, conn);
                    transaction.Commit();


                    LblMessage.Text = "Purchase Return Order : " + strCode.ToString() + " Generated Successfully.!";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", " alert('Purchase Return Order : " + strCode.ToString() + " Generated Successfully.!');", true);
                    ClearAll();
                    //LblMessage.Text = "Inventory Affected Successfully.!";
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Inventory Affected Successfully.!');", true);
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string message = "alert('Error:" + ex.Message.Replace("'","") + " !');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", message, true);
                LblMessage.Text = message;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }           

        }

        private void SaveManualPurchaseReturnToInventTransTable(string PurcReturnCode, SqlTransaction trans, SqlConnection conn)
        {

            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";

                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }

                string queryInsert = " Insert Into ax.acxinventTrans " +
                                 "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                 "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno])" +
                                 " Values (@TransId,@SiteCode,@DATAAREAID,@RECID,@InventTransDate,@TransType,@DocumentType,@DocumentNo,@DocumentDate, " +
                                 " @ProductCode,@TransQty,@TransUOM,@TransLocation,@Referencedocumentno)";

               
                cmd = new SqlCommand(queryInsert);
                cmd.Connection = conn;
                cmd.Transaction = trans;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                string st = Session["SiteCode"].ToString();
             

                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                    string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 5;                                          // Type 5 for Manual Purchase Return
                    int DocumentType = 5;
                    string DocumentNo = PurcReturnCode;
                    string productNameCode = gvDetails.Rows[i].Cells[2].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    TextBox txtBoxQty = (TextBox)gvDetails.Rows[i].Cells[4].FindControl("txtQty");
                    string strqty = txtBoxQty.Text;
                    decimal TransQty = Convert.ToDecimal(strqty) * -1;
                    string UOM = gvDetails.Rows[i].Cells[6].Text;                    
                    string Referencedocumentno = PurcReturnCode;
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
                    cmd.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);

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
            drpRecieptNo.SelectedIndex = 0;
            txtReceiptDate.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            txtDriverNumber.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txtVehicleNumber.Text = string.Empty;
            ddlInvoceNo.SelectedIndex = 0;
            drpReturnReason.SelectedIndex = 0;
            txtIndentDate.Text = string.Empty;
            txtInvoiceDate.Text = string.Empty;
            txtDriverNumber.Text = string.Empty;
            txtreversalVal.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtRemark.Text = string.Empty;
            Session["LineItem"] = null;
            gvDetails.DataSource = null;
            gvDetails.Visible = false;
           // GetReceipt();
           
        }

        private bool Validation()
        {
            bool returnvalue = false;

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

            if (drpReturnReason.SelectedItem.Text == "--Select--")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Return Reason  !');", true);
                drpReturnReason.Focus();
                returnvalue = false;
                return returnvalue;
            }
            else
            {
                returnvalue = true;
            }

            return returnvalue;
        }

        protected void chkCompReversal_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtgrid = new DataTable();
            if (Session[SessionGrid] != null)
                dtgrid = (DataTable)Session[SessionGrid];
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Session expired! Reselect the receipt no.');", true);
                return;
            }
            if (dtgrid.Rows.Count > 0)
            {
                decimal RetBox;
                foreach (DataColumn col in dtgrid.Columns)
                    col.ReadOnly = false;
                foreach (DataRow dr in dtgrid.Rows)
                {
                    if (chkCompReversal.Checked)
                    { RetBox = Convert.ToDecimal((Convert.ToDecimal(dr["BOX"]) - Convert.ToDecimal(dr["RETURNQTY"])).ToString("0.0000")); }
                    else
                    { RetBox = Convert.ToDecimal("0.0000"); }
                    string ExpTaxValue = dr["EXPTAXVALUE"].ToString();
                    dr["RAMOUNT"] = (Convert.ToDecimal(dr["AMOUNT"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RDISCOUNT"] = (Convert.ToDecimal(dr["DISCOUNT"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RCRATES"] = (Convert.ToDecimal(dr["CRATES"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RLTR"] = (Convert.ToDecimal(dr["LTR"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RBASICVALUE"] = (Convert.ToDecimal(dr["BASICVALUE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RTRDDISCVALUE"] = (Convert.ToDecimal(dr["TRDDISCVALUE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RPRICE_EQUALVALUE"] = (Convert.ToDecimal(dr["PRICE_EQUALVALUE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RTAXAMOUNT"] = (Convert.ToDecimal(dr["TAXAMOUNT"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RADD_TAX_AMOUNT"] = (Convert.ToDecimal(dr["ADD_TAX_AMOUNT"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RSURCHARGE_AMOUNT"] = (Convert.ToDecimal(dr["RSURCHARGE_AMOUNT"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RYEXP"] = (Convert.ToDecimal(dr["YEXP"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RYCS3"] = (Convert.ToDecimal(dr["YCS3"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    if (ExpTaxValue != null && ExpTaxValue != "")
                    {
                        dr["REXPTAXVALUE"] = (Convert.ToDecimal(dr["EXPTAXVALUE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    }
                    //dr["REXPTAXVALUE"] = (Convert.ToDecimal(dr["EXPTAXVALUE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RVAT_INC_PERCVALUE"] = (Convert.ToDecimal(dr["VAT_INC_PERCVALUE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RGROSSRATE"] = (Convert.ToDecimal(dr["GROSSRATE"]) / Convert.ToDecimal(dr["BOX"])) * RetBox;
                    dr["RBOX"] = RetBox;

                    dtgrid.AcceptChanges();
                }
            }
            Session[SessionGrid] = dtgrid;
            gvDetails.DataSource = dtgrid;
            gvDetails.DataBind();
            txtreversalVal.Text = Convert.ToDecimal(dtgrid.Compute("SUM(RAMOUNT)","")).ToString("0.0000");
            //txtReceiptValue.Text = Convert.ToDecimal()
          //  gvDetails.Visible = true;
         
        }

        protected void txtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;
                HiddenField h = (HiddenField)gvDetails.Rows[row.RowIndex].FindControl("HiddenValueLineNo");
                TextBox txtBox = (TextBox)sender;
                string productCode = row.Cells[2].Text;
                string[] str = productCode.Split('-');
                DataTable dtgrid = new DataTable();
                if (Session[SessionGrid] != null)
                    dtgrid = (DataTable)Session[SessionGrid];
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Session expired! Reselect the receipt no.');", true);
                    return;
                }
                if (txtBox.Text.Trim() == "")
                    txtBox.Text = "0";
                DataRow[] dr = dtgrid.Select("LINE_NO=" + h.Value.ToString());
                if (dr.Length>0)
                {
                    foreach (DataColumn col in dtgrid.Columns)
                        col.ReadOnly = false;
                    if (Convert.ToDecimal(dr[0]["BOX"])< Convert.ToDecimal(dr[0]["RETURNQTY"])+ Convert.ToDecimal(txtBox.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Return box quantity less than invoice quantity for the product " + dr[0]["PRODUCT_CODE"].ToString() +".');", true);
                        txtBox.Text = "0";
                        txtBox_TextChanged(sender, e);
                        return;
                    }
                    string ExpTaxValue = dr[0]["EXPTAXVALUE"].ToString();
                    dr[0]["RAMOUNT"] = (Convert.ToDecimal(dr[0]["AMOUNT"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RDISCOUNT"] = (Convert.ToDecimal(dr[0]["DISCOUNT"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RCRATES"] = (Convert.ToDecimal(dr[0]["CRATES"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RLTR"] = (Convert.ToDecimal(dr[0]["LTR"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RBASICVALUE"] = (Convert.ToDecimal(dr[0]["BASICVALUE"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RTRDDISCVALUE"] = (Convert.ToDecimal(dr[0]["TRDDISCVALUE"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RPRICE_EQUALVALUE"] = (Convert.ToDecimal(dr[0]["PRICE_EQUALVALUE"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RTAXAMOUNT"] = (Convert.ToDecimal(dr[0]["TAXAMOUNT"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RADD_TAX_AMOUNT"] = (Convert.ToDecimal(dr[0]["ADD_TAX_AMOUNT"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);

                    dr[0]["RSURCHARGE_AMOUNT"] = (Convert.ToDecimal(dr[0]["RSURCHARGE_AMOUNT"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RYEXP"] = (Convert.ToDecimal(dr[0]["YEXP"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RYCS3"] = (Convert.ToDecimal(dr[0]["YCS3"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    if (ExpTaxValue != null && ExpTaxValue != "")
                    {
                        dr[0]["REXPTAXVALUE"] = (Convert.ToDecimal(dr[0]["EXPTAXVALUE"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    }
                    dr[0]["RVAT_INC_PERCVALUE"] = (Convert.ToDecimal(dr[0]["VAT_INC_PERCVALUE"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RGROSSRATE"] = (Convert.ToDecimal(dr[0]["GROSSRATE"]) / Convert.ToDecimal(dr[0]["BOX"])) * Convert.ToDecimal(txtBox.Text);
                    dr[0]["RBOX"] = Convert.ToDecimal(txtBox.Text).ToString();

                    dtgrid.AcceptChanges();
                }
                Session[SessionGrid] = dtgrid;
                gvDetails.DataSource = dtgrid;
                gvDetails.DataBind();
                gvDetails.Visible = true;
                txtreversalVal.Text = Convert.ToDecimal(dtgrid.Compute("SUM(RAMOUNT)", "")).ToString("0.0000");
                //   if (txtBox.Text == "" || txtBox.Text == "0" )
                //  {
                //string message = "alert('Qty cannot be left blank..!!');";
                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                //check = false;

                //return;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be left blank..!!');", true);
                //check = false;
                //return;
                //  }
                //else
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('" + ex.Message.ToString().Replace("'","") + ".');", true);
            }
            
        }

        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
    }
}