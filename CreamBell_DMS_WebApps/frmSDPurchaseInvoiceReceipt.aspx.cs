using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSDPurchaseInvoiceReceipt : System.Web.UI.Page
    {
        public DataTable dtLineItems;

        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        static string SessionGrid ="GRReceipt";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Page.IsPostBack)
            {
                Session[""] = null;
                if (Request.QueryString["ID"] != null)
                {
                    GetReferencedData(Request.QueryString["ID"]);
                }
            }
        }

        private void GetReferencedData(string queryString)
        {
            if (Request.QueryString["ID"].ToString() != string.Empty)
            {
                string ReferenceNo = Request.QueryString["ID"].ToString();
                string DistCode = Request.QueryString["Dist"].ToString();

                //bool b = CheckPostedStatus(ReferenceNo);
                //if (b == false)
                //{
                    ShowRecords(ReferenceNo,DistCode);
                //}
            }

        }

        private bool CheckPostedStatus(string ReferenceNo)
        {
            bool checkStatus = false;
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                string query = " Select  [STATUS] from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' AND DOCUMENT_NO='" + ReferenceNo + "' ";

                object status = obj.GetScalarValue(query);
                if (status != null)
                {
                    string str = status.ToString();
                    if (str == "1")
                    {
                        checkStatus = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Reference Number Not Found !! Redirecting back to previous page ...');", true);
                        Response.Redirect("frmPurchUnPostList.aspx");
                        return checkStatus;
                    }
                }
                else
                {
                    checkStatus = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Reference Status Not Exists !! Redirecting back to previous page ...');", true);
                    Response.Redirect("frmPurchUnPostList.aspx");
                    return checkStatus;
                }
                return checkStatus;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
                return checkStatus;
            }
        }

        private void ShowRecords(string ReceiptNumber, string DistCode)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtHeader = null;
            DataTable dtLine = null;
            try
            {
                string queryHeader = " Select A.CUSTOMER_CODE, A.INVOICE_NO, A.SO_NO, A.SO_DATE, A.INVOICE_VALUE, A.INVOIC_DATE, A.TRANSPORTER_CODE, A.VEHICAL_NO, " +
                                     " A.DRIVER_CODE, A.DRIVER_MOBILENO, A.SITEID AS DISTRIBUTORCODE, B.SITEID, B.NAME AS DISTRIBUTORNAME,a.DISTGSTINNO,A.DISTGSTINREGDATE,A.DISTCOMPOSITIONSCHEME from ax.[ACXSALEINVOICEHEADER] A INNER JOIN" +
                                     " ax.inventsite B on A.SITEID=B.SITEID where A.INVOICE_NO='" + ReceiptNumber + "' " +
                                     " and A.CUSTOMER_CODE='" + Session["SiteCode"].ToString() + "'  and A.SITEID='" + DistCode + "' AND A.INVOICE_NO NOT IN (SELECT Sale_InvoiceNo FROM AX.ACXPURCHINVRECIEPTHEADER WHERE Site_Code='" + Session["SiteCode"].ToString() + "' AND Supplier_Code ='" + DistCode + "')";

                string queryLine = " Select SIL.LINE_NO, SIL.PRODUCT_CODE, (PRODUCT_CODE+'-'+ PRODUCT_NAME) AS PRODUCTDESC,  SIL.BOX, SIL.CRATES, SIL.LTR, " +
                                   " QUANTITY,SIL.MRP, SIL.RATE, INVT.UOM, TAX_CODE, TAX_AMOUNT, DISC_AMOUNT, ADDTAX_CODE, ADDTAX_AMOUNT, " +
                                   " TDValue SEC_DISC_AMOUNT, LINEAMOUNT, AMOUNT,SIL.HSNCODE,SIL.COMPOSITIONSCHEME,SIL.TAXCOMPONENT,SIL.ADDTAXCOMPONENT from [ax].[ACXSALEINVOICELINE] SIL INNER JOIN  AX.INVENTTABLE INVT " +
                                   " ON SIL.PRODUCT_CODE = INVT.ITEMID where SIL.INVOICE_NO='" + ReceiptNumber + "' and SIL.CUSTOMER_CODE='" + Session["SiteCode"].ToString() + "'  and SIL.SITEID='" + DistCode + "' AND SIL.INVOICE_NO NOT IN (SELECT Sale_InvoiceNo FROM AX.ACXPURCHINVRECIEPTHEADER WHERE Site_Code='" + Session["SiteCode"].ToString() + "' AND Supplier_Code ='" + DistCode + "')";


                dtHeader = obj.GetData(queryHeader);
                if (dtHeader.Rows.Count == 0 || dtHeader == null)
                {
                    return;
                }
                txtIndentNo.Text = string.Empty; 
                txtIndentDate.Text = string.Empty ;
                txtTransporterName.Text = dtHeader.Rows[0]["TRANSPORTER_CODE"].ToString();
                txtvehicleNo.Text = dtHeader.Rows[0]["VEHICAL_NO"].ToString();
                txtcomposition.Text= dtHeader.Rows[0]["DISTCOMPOSITIONSCHEME"].ToString();
                txtGstno.Text= dtHeader.Rows[0]["DISTGSTINNO"].ToString();
                txtRegistrationdate.Text= dtHeader.Rows[0]["DISTGSTINREGDATE"].ToString();
                DateTime ReceiptDate = Convert.ToDateTime(dtHeader.Rows[0]["INVOIC_DATE"].ToString());
                txtReceiptDate.Text = ReceiptDate.ToString("dd-MMM-yyyy");

                txtInvoiceNo.Text = dtHeader.Rows[0]["INVOICE_NO"].ToString();
                txtSONo.Text = dtHeader.Rows[0]["SO_NO"].ToString();
                if (txtSONo.Text != string.Empty)
                {
                    LoadIndent(txtSONo.Text.Trim().ToString());
                }

                txttransporterNo.Text = dtHeader.Rows[0]["DRIVER_CODE"].ToString();
                txtVehicleType.Text = string.Empty;
                decimal RecptValue = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());
                txtReceiptValue.Text = (Math.Round(RecptValue, 2)).ToString();

                txtDistributorCode.Text = dtHeader.Rows[0]["DISTRIBUTORCODE"].ToString();
                txtDistributorName.Text = dtHeader.Rows[0]["DISTRIBUTORNAME"].ToString();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                ilist.Add("@INVOICENO"); litem.Add(ReceiptNumber);
                ilist.Add("@SITEID"); litem.Add(Session["SiteCode"].ToString());
                ilist.Add("@DISTRIBUTOR"); litem.Add(DistCode);

                dtLine = obj.GetData_New("USP_GETPENDING_PURCHRECEIPTLINE", CommandType.StoredProcedure,ilist,litem);
                //dtLine = obj.GetData(queryLine);
                Session[SessionGrid] = dtLine;

                if (dtLine.Rows.Count > 0)
                {
                    GridPurchItems.DataSource = dtLine;
                    GridPurchItems.DataBind();
                    GridPurchItems.Visible = true;
                    GridViewFooterTotalShow(dtLine);
                }
                else
                {
                    LblMessage.Text = "No Line Items Exist";
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void GridViewFooterTotalShow(DataTable dt)
        {
            try
            {
                decimal Box = dt.AsEnumerable().Sum(row => row.Field<decimal>("BOX"));                   //For Total[Sum] Box Show in Footer--//
                GridPurchItems.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[4].Text = "Box: "+Box.ToString("N2");
                GridPurchItems.FooterRow.Cells[4].Font.Bold = true;

                decimal Crate = dt.AsEnumerable().Sum(row => row.Field<decimal>("CRATES"));          //For Total[Sum] Show in Footer--//
                GridPurchItems.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[5].Text = "Crate: " + Crate.ToString("N2");
                GridPurchItems.FooterRow.Cells[5].Font.Bold = true;

                decimal Litre = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTR"));          //For Total[Sum] Litre Show in Footer--//
                GridPurchItems.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[6].Text = "Ltr: " + Litre.ToString("N2");
                GridPurchItems.FooterRow.Cells[6].Font.Bold = true;

                decimal Rate = dt.AsEnumerable().Sum(row => row.Field<decimal>("RATE"));          //For Total[Sum] Show in Footer--//
                GridPurchItems.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[8].Text = "Rate: " + Rate.ToString("N2");
                GridPurchItems.FooterRow.Cells[8].Font.Bold = true;

                decimal TaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));          //For Total[Sum] Show in Footer--//
                GridPurchItems.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[10].Text = "Tax1 Amt: " + TaxAmount.ToString("N2");
                GridPurchItems.FooterRow.Cells[10].Font.Bold = true;

                decimal AddTaxAmnt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADDTAX_AMOUNT"));          //For Total[Sum] Litre Show in Footer--//
                GridPurchItems.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[12].Text = "Tax2 Amt: " + AddTaxAmnt.ToString("N2");
                GridPurchItems.FooterRow.Cells[12].Font.Bold = true;

                decimal Discount = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC_AMOUNT"));          //For Total[Sum] Litre Show in Footer--//
                GridPurchItems.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[13].Text = "Disc: " + Discount.ToString("N2");
                GridPurchItems.FooterRow.Cells[13].Font.Bold = true;

                decimal TDAMOUNT= dt.AsEnumerable().Sum(row => row.Field<decimal>("TDVALUE"));          //For Total[Sum] Litre Show in Footer--//
                GridPurchItems.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[14].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[14].Text = "TD: " + TDAMOUNT.ToString("N2");
                GridPurchItems.FooterRow.Cells[14].Font.Bold = true;
                
                decimal Gross = dt.AsEnumerable().Sum(row => row.Field<decimal>("LINEAMOUNT"));          //For Total[Sum] Price Show in Footer--//
                GridPurchItems.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[15].Text = "Gross:" + Gross.ToString("N2");
                GridPurchItems.FooterRow.Cells[15].Font.Bold = true;

                decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("AMOUNT"));          //For Total[Sum] Value Show in Footer--//
                GridPurchItems.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Left;
                GridPurchItems.FooterRow.Cells[16].ForeColor = System.Drawing.Color.MidnightBlue;
                GridPurchItems.FooterRow.Cells[16].Text = "Net:" + total.ToString("N2");
                GridPurchItems.FooterRow.Cells[16].Font.Bold = true;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadIndent(string SONumber)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string query = "Select CUST_REF_NO from [ax].[ACXSALESHEADER] where SO_NO='"+ SONumber+"' and CUSTOMER_CODE='"+Session["SiteCode"].ToString()+"' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

            try
            {

                object objIndentNo = obj.GetScalarValue(query);
                if (objIndentNo != null)
                {
                    txtIndentNo.Text = objIndentNo.ToString();
                }
                else
                {
                    txtIndentNo.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnPostPurchaseInvoice_Click(object sender, EventArgs e)
        {
            bool b = ValidateInvoicePosting();
            if(b)
            {
                POSTInvoiceData();
            }
        }

        private bool ValidateInvoicePosting()
        {
            bool b = true;

            if (txtInvoiceNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide  Invoice Number . Cannot Post data.";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            string chkInvoice = "select * from [ax].[ACXPURCHINVRECIEPTHEADER] where Sale_InvoiceNo='" + txtInvoiceNo.Text.Trim() + "' and SUPPLIER_CODE='"+txtDistributorCode.Text +"' and DataAreaid='" + Session["DATAAREAID"].ToString() + "' and Site_Code='" + Session["SiteCode"].ToString() + "'";
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtChk = obj.GetData(chkInvoice);
            if (dtChk.Rows.Count > 0)
            {
                string message = "alert('InvoiceNo: " + txtInvoiceNo.Text.Trim() + " Already exists  !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                b = false;
                return b;
            }
            if (txtReceiptValue.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide  Invoice Number . Cannot Post data.";
                txtReceiptValue.Focus();
                b = false;
                return b;
            }

            if (GridPurchItems.Rows.Count <= 0 )
            {
                LblMessage.Text = "► No Items to Post. Cannot Post data.";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }

            return b;
        }

        private void POSTInvoiceData()
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string PostDocumentNo = string.Empty;
            if (Session[SessionGrid]==null)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Session Expired.'); window.location.href='frmPendingPurchaseReciept.aspx';", true);
                return;
            }
            DataTable dtGrid = (DataTable)Session[SessionGrid];
            try
            {
                #region Generate New Posted Invoice Number

                cmd = new SqlCommand();
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                DataTable dtNumSeq = obj.GetNumSequenceNew(2, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                string NUMSEQ = string.Empty;
                if (dtNumSeq != null)
                {
                    PostDocumentNo = dtNumSeq.Rows[0][0].ToString();
                    NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }
                #endregion


                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.Text;
                                
                #region Insert Header
                DataSet ds = new DataSet();
                ds = new DataSet();
                DataTable dtLineItem = new DataTable();
                
                dtLineItem = dtGrid.Copy();
                dtLineItem.TableName = "SDPurchReceiptLine";
                dtLineItem.Columns["REMARKS"].ReadOnly = false;
                for (int i = 0; i < GridPurchItems.Rows.Count; i++)
                {
                    int LineNo = Convert.ToInt16(GridPurchItems.Rows[i].Cells[1].Text.ToString());
                    TextBox txtRemark = (TextBox)GridPurchItems.Rows[i].Cells[17].FindControl("txtRemark");
                    DataRow[] dr= dtLineItem.Select("LINE_NO=" + LineNo);
                    if (dr.Length>0)
                    {
                        dr[0]["REMARKS"] = txtRemark.Text;
                        dtLineItem.AcceptChanges();
                    }
                }

                ds.Tables.Add(dtLineItem);
                string InvoiceLineXml = ds.GetXml();

                string GstRegDate;
                GstRegDate = Convert.ToString(Session["SITEGSTINREGDATE"]) == "" ? null : Convert.ToDateTime(Session["SITEGSTINREGDATE"]).ToString("yyyy-MM-dd");
                cmd.CommandText = "ACX_ACXPURCHINVRECIEPT";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Document_No", PostDocumentNo);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@Document_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@Purchase_Indent_No", txtIndentNo.Text.Trim());
                cmd.Parameters.AddWithValue("@Purchase_Indent_Date", txtIndentDate.Text);
                cmd.Parameters.AddWithValue("@SO_No", txtSONo.Text.Trim());
                cmd.Parameters.AddWithValue("@SO_Date", DateTime.Now);
                cmd.Parameters.AddWithValue("@STATUS", 1);
                cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(txtReceiptValue.Text));
                cmd.Parameters.AddWithValue("@Purchase_Reciept_No", PostDocumentNo);
                cmd.Parameters.AddWithValue("@Sale_InvoiceDate", txtReceiptDate.Text);
                cmd.Parameters.AddWithValue("@Sale_InvoiceNo", txtInvoiceNo.Text.Trim().ToString());
                
                cmd.Parameters.AddWithValue("@Transporter_Code", txtTransporterName.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@VEHICAL_No", txtvehicleNo.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@VEHICAL_Type", txtVehicleType.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@PREDOCUMENT_NO", string.Empty);
                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text.Trim().ToString());
                cmd.Parameters.AddWithValue("@Supplier_Code", txtDistributorCode.Text.Trim());
                cmd.Parameters.AddWithValue("@DISTGSTINNO", Convert.ToString(Session["SITEGSTINNO"]));
                cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", GstRegDate);
                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", (Convert.ToBoolean(Session["SITECOMPOSITIONSCHEME"]) == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@VENDGSTINNO", Convert.ToString(txtGstno.Text));
                cmd.Parameters.AddWithValue("@VENDGSTINREGDATE", Convert.ToString(txtReceiptDate.Text));
                cmd.Parameters.AddWithValue("@VENDCOMPOSITIONSCHEME", (Convert.ToBoolean(txtcomposition.Text) == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@XmlData", InvoiceLineXml);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //sda.Fill(dt);
                #endregion
                transaction.Commit();
                Session[SessionGrid] = null;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');", true);
                //UpdateTransTable(PostDocumentNo, transaction, conn);
                RefreshCompletePage();

            }

            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }

        private string PurchaseTransTax(string PostDocumentNo, int LineNO,string INVOICE_NO, string CUSTOMER_CODE, string ProductCode)
        {
            try
            {
                SqlCommand cmd3 = new SqlCommand();
                cmd3.Connection = conn;
                cmd3.Transaction = transaction;
                cmd3.CommandTimeout = 3600;
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "USP_SDPURCHASETRANSTAX";
                cmd3.Parameters.Clear();
                cmd3.Parameters.AddWithValue("@RECEIPTNO", PostDocumentNo);
                cmd3.Parameters.AddWithValue("@LINENO", LineNO);
                cmd3.Parameters.AddWithValue("@DOCUMENT_NO", INVOICE_NO);
                cmd3.Parameters.AddWithValue("@SITECODE", Session["SiteCode"].ToString());
                cmd3.Parameters.AddWithValue("@CUSTOMER_CODE", CUSTOMER_CODE);
                cmd3.Parameters.AddWithValue("@ITEMCODE", ProductCode);
                cmd3.ExecuteNonQuery();
                return "Success";
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return ex.Message.ToString();
            }
        }
        private void RefreshCompletePage()
        {
            LblMessage.Text = string.Empty;
            txtIndentDate.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txttransporterNo.Text = string.Empty;
            txtvehicleNo.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            txtReceiptDate.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtReceiptValue.Text = string.Empty;
            GridPurchItems.DataSource = null;
            GridPurchItems.Visible = false;
            txtDistributorCode.Text = string.Empty;
            txtDistributorName.Text = string.Empty;
            txtSONo.Text = string.Empty;
            txtIndentNo.Text = string.Empty;
            GridPurchItems.DataSource = null;
            GridPurchItems.Visible = false;
            Session[SessionGrid] = null;
        }

        public void UpdateTransTable(string PostedDocumentNo, SqlTransaction trans, SqlConnection conn)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string TransLocation = "";
                string TransId = string.Empty;

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
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                string st = Session["SiteCode"].ToString();
               if (st.Length <= 6)
               {
                   TransId = st + System.DateTime.Now.ToString("yymmddhhmmss");
               }
               else
               {
                   TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
               }

                for (int p = 0; p < GridPurchItems.Rows.Count; p++)
                {
                    string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 1;                                          // Type 1 for Purchase Invoice Receipt
                    int DocumentType = 1;
                    string DocumentNo = PostedDocumentNo;
                    string productNameCode = GridPurchItems.Rows[p].Cells[2].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    string box = GridPurchItems.Rows[p].Cells[4].Text;
                    
                    decimal TransQty = Convert.ToDecimal(box) * 1;
                    string UOM = GridPurchItems.Rows[p].Cells[6].Text;
                    string Referencedocumentno = PostedDocumentNo;
                    int REcid = p + 1;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TransId", TransId);
                    cmd.Parameters.AddWithValue("@SiteCode", Siteid);
                    cmd.Parameters.AddWithValue("@DATAAREAID", DATAAREAID);
                    cmd.Parameters.AddWithValue("@RECID", p + 1);
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

                    int i = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            #region old Code PROCEDURE ISSUE

            //try
            //{
            //    CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            //    conn = obj.GetConnection();
            //    string TransLocation = "";
            //    string TransId = string.Empty;

            //    string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
            //    DataTable dt = new DataTable();
            //    dt = obj.GetData(query1);
            //    if (dt.Rows.Count > 0)
            //    {
            //        TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
            //    }

            //    string st = Session["SiteCode"].ToString();
            //    if (st.Length <= 6)
            //    {
            //        TransId = st + System.DateTime.Now.ToString("yymmddhhmmss");
            //    }
            //    else
            //    {
            //        TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
            //    }
                

            //    cmd = new SqlCommand();
            //    cmd.Connection = conn;
            //    cmd.CommandTimeout = 100;
            //    cmd.CommandText = string.Empty;
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "[ACX_PURCHINVC_UPDATEINVENTTRANS]";

            //    cmd.Parameters.Clear();

            //    string strSite = Session["SiteCode"].ToString();
            //    string strDAtaArea = Session["DATAAREAID"].ToString();

            //    cmd.Parameters.AddWithValue("@SITECODE", strSite);
            //    cmd.Parameters.AddWithValue("@DOCUMENTPURCHRECEIPTNUMBER", PostedDocumentNo);
            //    cmd.Parameters.AddWithValue("@DATAAREAID", strDAtaArea);
            //    cmd.Parameters.AddWithValue("@TRANSID", TransId);
            //    cmd.Parameters.AddWithValue("@WAREHOUSE", TransLocation);
            //    cmd.Parameters.AddWithValue("@TRANSTYPE", 1);

            //    int i = cmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    LblMessage.Text = "Error: Inventory Update Issue - " + ex.Message.ToString();
            //}
            //finally
            //{
            //    if (conn != null)
            //    {
            //        if (conn.State == ConnectionState.Open)
            //        {
            //            conn.Close();
            //        }
            //    }
            //}

            #endregion
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshCompletePage();
        }



    }
}