using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmFOCPurchaseInvoice : System.Web.UI.Page
    {
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        SqlDataAdapter adp2, adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();

        public static decimal ProductWeight;
        public static decimal ProductLitre;

        SqlDataAdapter da = new SqlDataAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!Page.IsPostBack)
            {
                string datestring = DateTime.Now.ToString("yyyy-MM-dd");
                txtReceiptDate.Text = datestring;

                FillIndentNo();
                Session["LineItem"] = null;
                BtnUpdateHeader.Visible = false;

                txtIndentDate.Attributes.Add("readonly", "readonly");
                txtInvoiceDate.Attributes.Add("readonly", "readonly");

                if (Request.QueryString["PreNo"] != null)
                {
                    GetUnPostedReferenceData(Request.QueryString["PreNo"]);
                }
            }
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetProductDescription(string prefixText)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
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

        protected void BtnGetProductDetails_Click(object sender, EventArgs e)
        {
            try { 
            LblMessage.Text = "";
            if (txtProductCode.Text != string.Empty)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                ProductLitre = 0;
                ProductWeight= 0;

                string queryFillProductDetails = "Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY," +
                                                 " CAST(ROUND(PRODUCT_MRP,2) as NUMERIC(10,2)) as PRODUCT_MRP ,CAST(ROUND(LTR,2) as NUMERIC(10,2)) as LTR, " +
                                                 " CAST(ROUND(NETWEIGHTPCS,2) as NUMERIC(10,2)) as NETWEIGHTPCS " +
                                                 "from ax.INVENTTABLE where replace(replace(ITEMID, char(9), ''), char(13) + char(10), '')= '" + txtProductCode.Text.Trim().ToString() + "'";

                DataTable dtProductDetails = obj.GetData(queryFillProductDetails);
                if (dtProductDetails.Rows.Count > 0 && dtProductDetails.Rows.Count == 1)
                {
                    txtProductDesc.Text = dtProductDetails.Rows[0]["PRODUCT_NAME"].ToString();
                    txtMRP.Text = dtProductDetails.Rows[0]["PRODUCT_MRP"].ToString();
                    txtWeight.Text = dtProductDetails.Rows[0]["NETWEIGHTPCS"].ToString();
                    txtVolume.Text = dtProductDetails.Rows[0]["LTR"].ToString();
                    ProductWeight = Convert.ToDecimal(dtProductDetails.Rows[0]["NETWEIGHTPCS"].ToString());
                    ProductLitre = Convert.ToDecimal(dtProductDetails.Rows[0]["LTR"].ToString());
                }
                if (dtProductDetails.Rows.Count > 1)
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product Code Issue: We Have Duplicate Records for this Product Code !');", true);

                    LblMessage.Text = "Product Code Issue: We Have Duplicate Records for this Product Code !";

                   // string message = "alert('Product Code Issue: We Have Duplicate Records for this Product Code !');";
                    //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
                if (dtProductDetails.Rows.Count == 0)
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product Code Issue: No Such Produt Code Exist !');", true);

                    LblMessage.Text = "Product Code Issue: No Such Produt Code Exist !";

                    //string message = "alert('Product Code Issue: No Such Produt Code Exist !');";
                    //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }

            }
            else
            {
                txtProductCode.Focus();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Product Code Here !');", true);

                LblMessage.Text = "Please Provide Product Code Here !";
                    // string message = " alert('Please Provide Product Code Here !');";
                    //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateManualEntry()
        {
            bool b = false;

            if (txtInvoiceNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice No.";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            if (txtInvoiceDate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice Date.";
                txtInvoiceDate.Focus();
                b = false;
                return b;
            }
            if (txtProductCode.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Product Code:";
                txtProductCode.Focus();
                b = false;
                return b;
            }

            if (txtProductDesc.Text == string.Empty)
            {
                LblMessage.Text = "► Product Description Not Available:";
                txtProductDesc.Focus();
                b = false;
                return b;
            }
            if (txtMRP.Text == string.Empty)
            {
                LblMessage.Text = "► Product MRP Not Available:";
                txtMRP.Focus();
                b = false;
                return b;
            }

            if (txtEntryValue.Text == string.Empty || Convert.ToDecimal(txtEntryValue.Text) == 0)
            {
                LblMessage.Text = "► Please Provide " + LTEntryType.Text + " Value : and its cannot be zero.";
                txtEntryValue.Focus();
                b = false;
                return b;
            }
           
            if (txtWeight.Text == string.Empty)
            {
                LblMessage.Text = "► Weight Cannot be empty or 0.";
                txtWeight.Focus();
                b = false;
                return b;
            }
            if (txtVolume.Text == string.Empty)
            {
                LblMessage.Text = "► Volume Cannot be empty or 0:";
                txtVolume.Focus();
                b = false;
                return b;
            }
            else
            {
                LblMessage.Text = string.Empty;
                b = true;
            }
            return b;
        }

        /*
        private string[] CalculateManualEntryMethod(decimal RateAmount, decimal EntryValueQty)
        {
            
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string[] returnResult = null;

            try
            {
                if (DDLEntryType.SelectedItem.Text.ToString() == "Crate")
                {
                    string[] ReturnArray = obj.CalculatePrice1(txtProductCode.Text, string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());

                    EntryValueQty = Convert.ToDecimal(ReturnArray[0].ToString());
                }

                decimal RateValue = 0;
                decimal TRDPerc = Convert.ToDecimal(txtTRDDiscPerc.Text);
                decimal TRDValue = 0;
                decimal PriceEqualValue = Convert.ToDecimal(txtPriceEqualValue.Text);
                decimal VATPerc = Convert.ToDecimal(txtVATAddTAXPerc.Text);
                decimal VATValue = 0;
                decimal GrossRate = Convert.ToDecimal(txtGrossRate.Text);
                decimal NetValue = 0;
                decimal Weight = 0;
                decimal Ltr = 0;

                RateValue = Math.Round(RateAmount * EntryValueQty, 2);

                TRDValue = Math.Round((RateValue * TRDPerc) / 100, 2);

                VATValue = Math.Round((((RateValue - TRDValue) + PriceEqualValue) * VATPerc) / 100, 2);

                NetValue = Math.Round(GrossRate * EntryValueQty, 2);

                Weight = Math.Round((Convert.ToDecimal(txtWeight.Text) * EntryValueQty) / 1000, 2);

                Ltr = Math.Round((Convert.ToDecimal(txtVolume.Text) * EntryValueQty) / 1000, 2);

                returnResult = new string[6];
                returnResult[0] = RateValue.ToString();
                returnResult[1] = TRDValue.ToString();
                returnResult[2] = VATValue.ToString();
                returnResult[3] = NetValue.ToString();
                returnResult[4] = Weight.ToString();
                returnResult[5] = Ltr.ToString();
                return returnResult;
            }
            catch (Exception)
            {
                return returnResult;
            }
            
        }

         */
        private string[] GetBOXCRATELITRECalculatedValueFromGlobal()
        {
            string[] ReturnArray = null;
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            try
            {
                ReturnArray = obj.CalculatePrice1(txtProductCode.Text, string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());

                return ReturnArray;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
                return ReturnArray;
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            ResetPageUnPostedData();
        }

        private void ResetPageUnpostedDataHeader()
        {
            LblMessage.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtMRP.Text = string.Empty;
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtVolume.Text = string.Empty;

            
        }

        private void ResetPageUnPostedData()
        {
            LblMessage.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtMRP.Text = string.Empty;
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtVolume.Text = string.Empty;

            //free the Header Textbox
            txtIndentDate.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txttransporterNo.Text = string.Empty;
            txtvehicleNo.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            txtInvoiceDate.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtReceiptValue.Text = "0";
            GridFOCPurchItems.DataSource = null;
            GridFOCPurchItems.Visible = false;
            txtPurchDocumentNo.Text = string.Empty;
            //Emtry grid view data
            GridFOCPurchItems.DataSource = null;
            GridFOCPurchItems.DataBind();

        }

        private void FillIndentNo()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                string strQuery = " select distinct INDENT_NO as INDENT_NO from [ax].[ACXPURCHINDENTHEADER] PINDH where  NOT EXISTS (Select PURCH_INDENTNO " +
                                  " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH where PINDH.INDENT_NO=PIH.PURCH_INDENTNO AND PINDH.SITEID = PIH.SITE_CODE) and  PINDH.SITEID='" + Session["SiteCode"].ToString() +
                                  "' and PINDH.STATUS=1  order by INDENT_NO desc";

                DrpIndentNo.Items.Clear();
                DrpIndentNo.Items.Add("-Select-");
                obj.BindToDropDown(DrpIndentNo, strQuery, "INDENT_NO", "INDENT_NO");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void DrpIndentNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtIndentNo.Text = DrpIndentNo.Text;
                if (DrpIndentNo.Text != "-Select-")
                {
                    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                    string strQuery = " select indent_date,invoice_no from [ax].[ACXPURCHINDENTHEADER] where indent_no = '" + DrpIndentNo.SelectedValue.ToString() + "'";

                    DataTable dt = obj.GetData(strQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtIndentDate.Text = Convert.ToDateTime(dt.Rows[0]["indent_date"]).ToString("dd-MMM-yyyy");
                        txtInvoiceNo.Text = dt.Rows[0]["invoice_no"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BtnSavePreData_Click(object sender, EventArgs e)
        {
            bool b = ValidateManualEntry();
            if (b)
            {
                PRESavePurchaseInvoiceReceiptToDB();
            }
        }

        private void PRESavePurchaseInvoiceReceiptToDB()
        {
            try
            {
                string strCode = string.Empty;

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (txtPurchDocumentNo.Text == string.Empty)
                {
                    #region PO  Number Generate

                    cmd = new SqlCommand();
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.Text;

                    DataTable dtNumSeq = obj.GetNumSequenceNew(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
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

                    strCode = obj.GetNumSequence(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                    cmd.CommandText = string.Empty;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ACX_ACXPURCHINVRECIEPTHEADERPRE";

                    #endregion

                    #region Header Insert Data

                    string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADERPRE]";
                    Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                    cmd.Parameters.Clear();
                    string indentno = string.Empty;
                    if (DrpIndentNo.SelectedItem.Text.ToString() == "-Select-")
                    {
                        indentno = "";
                    }
                    else
                    {
                        indentno = DrpIndentNo.SelectedItem.Text.ToString();
                    }
                    cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                    cmd.Parameters.AddWithValue("@Purchase_Indent_No", indentno);
                    cmd.Parameters.AddWithValue("@Purchase_Indent_Date", txtIndentDate.Text);
                    cmd.Parameters.AddWithValue("@Transporter_Code", txtTransporterName.Text);
                    cmd.Parameters.AddWithValue("@Document_No", strCode);
                    cmd.Parameters.AddWithValue("@DocumentDate", txtReceiptDate.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_No", txtvehicleNo.Text);
                    cmd.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                    cmd.Parameters.AddWithValue("@Sale_InvoiceNo", txtInvoiceNo.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@Sale_InvoiceDate", txtInvoiceDate.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_Type", txtVehicleType.Text);
                    cmd.Parameters.AddWithValue("@SO_No", string.Empty);
                    cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(txtReceiptValue.Text));
                    cmd.Parameters.AddWithValue("@recid", Recid + 1);
                    cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd.Parameters.AddWithValue("@STATUS", 0);                  // for UnPosted Purchase Invoice Receipt Status//
                    cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                    cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text.Trim().ToString());
                    cmd.ExecuteNonQuery();
                    txtPurchDocumentNo.Text = strCode;
                    //transaction.Commit();
                    #endregion


                }

                #region Line Insert Data on Same PURCH Order Number

                cmd1 = new SqlCommand("[ACX_ACXPURCHINVRECIEPTLINEPRE]");
                cmd1.Connection = conn;
                if (transaction == null)
                {
                    transaction = conn.BeginTransaction();
                }
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;

                string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINEPRE]";
                Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));


                #endregion

                string productNameCode = txtProductDesc.Text;
                string[] str = productNameCode.Split('-');
                string productCode = str[0].ToString();

                strCode = txtPurchDocumentNo.Text;

                string[] ReturnArray = null;
                ReturnArray = obj.CalculatePrice1(txtProductCode.Text, string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());
                if (ReturnArray != null)
                {
                    cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1);
                    cmd1.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                    cmd1.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                    cmd1.Parameters.AddWithValue("@PRODUCT_CODE", productCode);
                    cmd1.Parameters.AddWithValue("@LINE_NO", RecidLine + 1);
                    cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                    #region Crate Box Data Insert

                    if (ReturnArray[5].ToString() == "Box")
                    {
                        cmd1.Parameters.AddWithValue("@BOX", txtEntryValue.Text.Trim().ToString());
                        cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
                        cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                        cmd1.Parameters.AddWithValue("@RATE", 0);
                        cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                        cmd1.Parameters.AddWithValue("@BASICVALUE", 0);
                        cmd1.Parameters.AddWithValue("@TRDDISCPERC", 0);
                        cmd1.Parameters.AddWithValue("@TRDDISCVALUE", 0);
                        cmd1.Parameters.AddWithValue("@PRICEEQUALVALUE", 0);
                        cmd1.Parameters.AddWithValue("@VAT_INC_PERC", 0);
                        cmd1.Parameters.AddWithValue("@VAT_INC_PERC_VALUE", 0);
                        cmd1.Parameters.AddWithValue("@GROSSAMOUNT", 0);
                        cmd1.Parameters.AddWithValue("@DISCOUNT", 0);
                        cmd1.Parameters.AddWithValue("@TAX", 0);
                        cmd1.Parameters.AddWithValue("@TAXAMOUNT", 0);
                        cmd1.Parameters.AddWithValue("@AMOUNT", 0);
                        cmd1.Parameters.AddWithValue("@Remark", "FOC");
                        cmd1.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    txtPurchDocumentNo.Text = strCode;
                    ShowRecords(txtPurchDocumentNo.Text.ToString());
                    UpdateAndShowTotalMaterialValue(txtPurchDocumentNo.Text.ToString());
                    ResetPageUnpostedDataHeader();
                    BtnUpdateHeader.Visible = true;

                    #endregion

                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
            }
        }

        private void UpdateAndShowTotalMaterialValue(string PurchNo)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            try
            {
                string query = "Select * from [ax].[ACXPURCHINVRECIEPTLINEPRE] WHERE PURCH_RECIEPTNO='" + PurchNo + "' " +
                               "and SITEID='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";


                DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    decimal totalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("AMOUNT"));
                    txtReceiptValue.Text = Math.Round(totalvalue, 2).ToString();

                    string UpdateMaterialValue = "UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET MATERIAL_VALUE = " + totalvalue + " where PURCH_RECIEPTNO='" + PurchNo + "' " +
                                                " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    obj.ExecuteCommand(UpdateMaterialValue);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }

        private void ShowRecords(string PurchReceiptNumber)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtHeader = null;
            DataTable dtLine = null;
            try
            {
                string queryHeader = "Select * from [ax].[ACXPURCHINVRECIEPTHEADERPRE] where PURCH_RECIEPTNO='" + PurchReceiptNumber + "' " +
                                     " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                string queryLine = "Select PURCH_RECIEPTNO, LINE_NO,PRODUCT_CODE, BOX,CRATES,A.LTR,A.UOM,RATE,BASICVALUE,TRDDISCPERC,TRDDISCVALUE,PRICE_EQUALVALUE," +
                                   "VAT_INC_PERC,VAT_INC_PERCVALUE,GROSSRATE,AMOUNT,(PRODUCT_CODE+'-'+ PRODUCT_NAME) AS PRODUCTDESC,PRODUCT_MRP  from [ax].[ACXPURCHINVRECIEPTLINEPRE] A " +
                                   " INNER JOIN AX.INVENTTABLE B  ON A.PRODUCT_CODE = B.ITEMID   WHERE PURCH_RECIEPTNO='" + PurchReceiptNumber + "' " +
                                   " and SITEID='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                //"dd-MMM-yyyy"



                dtHeader = obj.GetData(queryHeader);
                if (dtHeader.Rows[0]["PURCH_INDENTNO"].ToString() == string.Empty || dtHeader.Rows[0]["PURCH_INDENTNO"].ToString() == null)
                {
                    DrpIndentNo.SelectedIndex = 0;
                }
                else
                {
                    DrpIndentNo.SelectedItem.Text = dtHeader.Rows[0]["PURCH_INDENTNO"].ToString();
                }

                
                DateTime IndentDate = Convert.ToDateTime(dtHeader.Rows[0]["PURCH_INDENTDATE"].ToString());
                txtIndentDate.Text = IndentDate.ToString("dd-MMM-yyyy");
                txtTransporterName.Text = dtHeader.Rows[0]["TRANSPORTER_CODE"].ToString();
                txtvehicleNo.Text = dtHeader.Rows[0]["VEHICAL_NO"].ToString();
                DateTime ReceiptDate = Convert.ToDateTime(dtHeader.Rows[0]["DOCUMENT_DATE"].ToString());
                txtReceiptDate.Text = ReceiptDate.ToString("dd-MMM-yyyy");
                txtInvoiceNo.Text = dtHeader.Rows[0]["SALE_INVOICENO"].ToString();
                DateTime InvcDate = Convert.ToDateTime(dtHeader.Rows[0]["SALE_INVOICEDATE"].ToString());
                txtInvoiceDate.Text = InvcDate.ToString("dd-MMM-yyyy");
                txttransporterNo.Text = dtHeader.Rows[0]["TRANSPORTER_CODE"].ToString();
                txtVehicleType.Text = dtHeader.Rows[0]["VEHICAL_TYPE"].ToString();
                decimal RecptValue = Convert.ToDecimal(dtHeader.Rows[0]["MATERIAL_VALUE"].ToString());
                txtReceiptValue.Text = (Math.Round(RecptValue, 2)).ToString();
                txtPurchDocumentNo.Text = PurchReceiptNumber;

                dtLine = obj.GetData(queryLine);

                if (dtLine.Rows.Count > 0)
                {
                    GridFOCPurchItems.DataSource = dtLine;
                    GridFOCPurchItems.DataBind();
                    GridFOCPurchItems.Visible = true;
                }
                else
                {
                    LblMessage.Text = "No Line Items Exist";
                    BtnUpdateHeader.Visible = false;
                    GridFOCPurchItems.DataSource = null;
                    GridFOCPurchItems.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }


        protected void BtnUpdateHeader_Click(object sender, EventArgs e)
        {
            UpdateHeader(txtPurchDocumentNo.Text);
        }


        private void UpdateHeader(string PurchInvcNo)
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            try
            {
                if (txtPurchDocumentNo.Text != string.Empty)
                {
                    #region Update Header

                    string QueryUpdate = " UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET PURCH_INDENTNO= @PurchIndentNo," +
                                         " PURCH_INDENTDATE= @PURCH_INDENTDATE, SALE_INVOICEDATE=@SALE_INVOICEDATE, SALE_INVOICENO = @SALE_INVOICENO," +
                                         " TRANSPORTER_CODE= @TRANSPORTER_CODE, VEHICAL_NO = @VEHICAL_NO, VEHICAL_TYPE= @VEHICAL_TYPE  " +
                                         " where PURCH_RECIEPTNO='" + txtPurchDocumentNo.Text + "' " +
                                         " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    cmd = new SqlCommand(QueryUpdate);
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.Transaction = transaction;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.Text;

                    string strindentNo = string.Empty;
                    if (DrpIndentNo.SelectedItem.Text == "-Select-")
                    {
                        strindentNo = "";
                    }
                    else
                    {
                        strindentNo = DrpIndentNo.SelectedItem.Text;
                    }

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@PurchIndentNo", strindentNo);
                    cmd.Parameters.AddWithValue("@PURCH_INDENTDATE", txtIndentDate.Text);
                    cmd.Parameters.AddWithValue("@SALE_INVOICEDATE", txtInvoiceDate.Text);
                    cmd.Parameters.AddWithValue("@SALE_INVOICENO", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtTransporterName.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_NO", txtvehicleNo.Text);
                    cmd.Parameters.AddWithValue("@VEHICAL_TYPE", txtVehicleType.Text);
                    cmd.ExecuteNonQuery();
                    transaction.Commit();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }

        }

        protected void GridFOCPurchItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                HiddenField hiddenfield = (HiddenField)GridFOCPurchItems.Rows[e.RowIndex].FindControl("HiddenValueLineNo");

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                string query = "Delete from [ax].[ACXPURCHINVRECIEPTLINEPRE] where PURCH_RECIEPTNO='" + txtPurchDocumentNo.Text + "' " +
                               " and LINE_NO = " + hiddenfield.Value + " and SITEID='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                obj.ExecuteCommand(query);
                ShowRecords(txtPurchDocumentNo.Text);
                UpdateAndShowTotalMaterialValue(txtPurchDocumentNo.Text);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnPostPurchaseInvoice_Click(object sender, EventArgs e)
        {
            POSTPurchaseInvoiceReceipt(txtPurchDocumentNo.Text);
        }

        private void POSTPurchaseInvoiceReceipt(string PrePurchReceiptNo)
        {
            try
            {
                string PostDocumentNo = string.Empty;

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (txtPurchDocumentNo.Text != string.Empty)
                {
                    #region Get PreSaved Data Header and Line

                    string queryPreHeader = "Select * from [ax].[ACXPURCHINVRECIEPTHEADERPRE] where PURCH_RECIEPTNO='" + PrePurchReceiptNo + "' " +
                                            " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    string queryPreLine = "Select * from [ax].[ACXPURCHINVRECIEPTLINEPRE] WHERE PURCH_RECIEPTNO='" + PrePurchReceiptNo + "' " +
                                          " and SITEID='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    DataTable dtHeaderPre = obj.GetData(queryPreHeader);
                    DataTable dtLinePre = obj.GetData(queryPreLine);

                    #endregion

                    if (dtHeaderPre.Rows.Count > 0 && dtLinePre.Rows.Count > 0)
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

                        string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADER]";
                        Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                        cmd.CommandText = string.Empty;
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = " INSERT INTO [ax].[ACXPURCHINVRECIEPTHEADER] ( Document_No,DATAAREAID,RECID,Document_Date,Purch_IndentNo,Purch_IndentDate, " +
                                          " SO_No,SO_Date,STATUS,Material_Value,Purch_RecieptNo,Sale_InvoiceDate,Sale_InvoiceNo,Site_Code,Transporter_Code,VEHICAL_No," +
                                          " VEHICAL_Type,PREDOCUMENT_NO,NUMSEQ) values ( @Document_No,@DATAAREAID,@RECID,@Document_Date,@Purch_IndentNo,@Purch_IndentDate, " +
                                          " @SO_No,@SO_Date,@STATUS,@Material_Value,@Purch_RecieptNo,@Sale_InvoiceDate,@Sale_InvoiceNo,@Site_Code,@Transporter_Code,@VEHICAL_No," +
                                          " @VEHICAL_Type,@PREDOCUMENT_NO,@NUMSEQ)";

                        #region Insert Header

                        for (int i = 0; i < dtHeaderPre.Rows.Count; i++)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@Document_No", PostDocumentNo);
                            cmd.Parameters.AddWithValue("@DATAAREAID", dtHeaderPre.Rows[0]["DATAAREAID"].ToString());
                            cmd.Parameters.AddWithValue("@RECID", Recid + 1);
                            cmd.Parameters.AddWithValue("@Document_Date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Purch_IndentNo", dtHeaderPre.Rows[0]["Purch_IndentNo"].ToString());
                            cmd.Parameters.AddWithValue("@Purch_IndentDate", dtHeaderPre.Rows[0]["Purch_IndentDate"].ToString());
                            cmd.Parameters.AddWithValue("@SO_No", dtHeaderPre.Rows[0]["SO_No"].ToString());
                            cmd.Parameters.AddWithValue("@SO_Date", dtHeaderPre.Rows[0]["SO_Date"].ToString());
                            cmd.Parameters.AddWithValue("@STATUS", 1);
                            cmd.Parameters.AddWithValue("@Material_Value", Convert.ToDecimal(dtHeaderPre.Rows[0]["Material_Value"].ToString()));
                            cmd.Parameters.AddWithValue("@Purch_RecieptNo", PostDocumentNo);
                            cmd.Parameters.AddWithValue("@Sale_InvoiceDate", dtHeaderPre.Rows[0]["Sale_InvoiceDate"].ToString());
                            cmd.Parameters.AddWithValue("@Sale_InvoiceNo", dtHeaderPre.Rows[0]["Sale_InvoiceNo"].ToString());
                            cmd.Parameters.AddWithValue("@Site_Code", dtHeaderPre.Rows[0]["Site_Code"].ToString());
                            cmd.Parameters.AddWithValue("@Transporter_Code", dtHeaderPre.Rows[0]["Transporter_Code"].ToString());
                            cmd.Parameters.AddWithValue("@VEHICAL_No", dtHeaderPre.Rows[0]["VEHICAL_No"].ToString());
                            cmd.Parameters.AddWithValue("@VEHICAL_Type", dtHeaderPre.Rows[0]["VEHICAL_Type"].ToString());
                            cmd.Parameters.AddWithValue("@PREDOCUMENT_NO", PrePurchReceiptNo);
                            cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);

                            cmd.ExecuteNonQuery();
                        }

                        #endregion

                        cmd1 = new SqlCommand();
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.Text;

                        string queryLineRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINE]";
                        Int64 recid = Convert.ToInt64(obj.GetScalarValue(queryLineRecID));

                        cmd1.CommandText = string.Empty;
                        cmd1.CommandText = " INSERT INTO [ax].[ACXPURCHINVRECIEPTLINE] ( Purch_RecieptNo,DATAAREAID,RECID,Amount,Box,Crates,Discount,Line_No, " +
                                           " Ltr,Product_Code,Rate,	Siteid,TAX,TAXAMOUNT,UOM,BASICVALUE,TRDDISCPERC,TRDDISCVALUE,PRICE_EQUALVALUE,VAT_INC_PERC, " +
                                           " VAT_INC_PERCVALUE,GROSSRATE,Remark ) Values ( @Purch_RecieptNo,@DATAAREAID,@RECID,@Amount,@Box,@Crates,@Discount,@Line_No," +
                                           " @Ltr,@Product_Code,@Rate, @Siteid,@TAX,@TAXAMOUNT,@UOM,@BASICVALUE,@TRDDISCPERC,@TRDDISCVALUE,@PRICE_EQUALVALUE, " +
                                           " @VAT_INC_PERC,@VAT_INC_PERCVALUE,@GROSSRATE,@Remark )";

                        #region Line Insert

                        for (int p = 0; p < dtLinePre.Rows.Count; p++)
                        {
                            cmd1.Parameters.Clear();

                            cmd1.Parameters.AddWithValue("@Purch_RecieptNo", PostDocumentNo);
                            cmd1.Parameters.AddWithValue("@DATAAREAID", dtLinePre.Rows[p]["DATAAREAID"].ToString());
                            cmd1.Parameters.AddWithValue("@RECID", p + recid + 1);
                            cmd1.Parameters.AddWithValue("@Amount", Convert.ToDecimal(dtLinePre.Rows[p]["Amount"].ToString()));
                            cmd1.Parameters.AddWithValue("@Box", Convert.ToDecimal(dtLinePre.Rows[p]["Box"].ToString()));
                            cmd1.Parameters.AddWithValue("@Crates", Convert.ToDecimal(dtLinePre.Rows[p]["Crates"].ToString()));
                            cmd1.Parameters.AddWithValue("@Discount", Convert.ToDecimal(dtLinePre.Rows[p]["Discount"].ToString()));
                            cmd1.Parameters.AddWithValue("@Line_No", p + recid + 1);
                            cmd1.Parameters.AddWithValue("@Ltr", Convert.ToDecimal(dtLinePre.Rows[p]["Ltr"].ToString()));
                            cmd1.Parameters.AddWithValue("@Product_Code", dtLinePre.Rows[p]["Product_Code"].ToString());
                            cmd1.Parameters.AddWithValue("@Rate", Convert.ToDecimal(dtLinePre.Rows[p]["Rate"].ToString()));
                            cmd1.Parameters.AddWithValue("@Siteid", dtLinePre.Rows[p]["Siteid"].ToString());
                            cmd1.Parameters.AddWithValue("@TAX", Convert.ToDecimal(dtLinePre.Rows[p]["TAX"].ToString()));
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", Convert.ToDecimal(dtLinePre.Rows[p]["TAXAMOUNT"].ToString()));
                            cmd1.Parameters.AddWithValue("@UOM", dtLinePre.Rows[p]["UOM"].ToString());
                            cmd1.Parameters.AddWithValue("@BASICVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["BASICVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@TRDDISCPERC", Convert.ToDecimal(dtLinePre.Rows[p]["TRDDISCPERC"].ToString()));
                            cmd1.Parameters.AddWithValue("@TRDDISCVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["TRDDISCVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@PRICE_EQUALVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["PRICE_EQUALVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@VAT_INC_PERC", Convert.ToDecimal(dtLinePre.Rows[p]["VAT_INC_PERC"].ToString()));
                            cmd1.Parameters.AddWithValue("@VAT_INC_PERCVALUE", Convert.ToDecimal(dtLinePre.Rows[p]["VAT_INC_PERCVALUE"].ToString()));
                            cmd1.Parameters.AddWithValue("@GROSSRATE", Convert.ToDecimal(dtLinePre.Rows[p]["GROSSRATE"].ToString()));
                            cmd1.Parameters.AddWithValue("@Remark", dtLinePre.Rows[p]["Remark"].ToString());

                            cmd1.ExecuteNonQuery();

                        }

                        #endregion


                        transaction.Commit();
                       //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Purchase Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');", true);
                        LblMessage.Text = "Purchase Invoice Posted Successfully !  Document Number : " + PostDocumentNo + "";
                       // string message = "alert('Purchase Invoice Posted Successfully !  Document Number : " + PostDocumentNo + " ');";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                        UpdatePostedStatusInPreTable();
                        UpdateTransTable(PostDocumentNo);
                        txtPurchDocumentNo.Text = string.Empty;
                        RefreshCompletePage();


                    }
                }
                else
                {
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' No Data to Save. Please Add Line Items and Header Details First !! ');", true);

                    LblMessage.Text = "No Data to Save. Please Add Line Items and Header Details First !! ";

                    //string message = "alert(' No Data to Save. Please Add Line Items and Header Details First !! ');";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
            }
        }

        private void RefreshCompletePage()
        {
            LblMessage.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtMRP.Text = string.Empty;
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            txtWeight.Text = string.Empty;
            txtVolume.Text = string.Empty;
            BtnUpdateHeader.Visible = false;
            DrpIndentNo.SelectedIndex = 0;
            FillIndentNo();
            txtIndentDate.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txttransporterNo.Text = string.Empty;
            txtvehicleNo.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            txtInvoiceDate.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtReceiptValue.Text = "0";
            GridFOCPurchItems.DataSource = null;
            GridFOCPurchItems.Visible = false;
            txtPurchDocumentNo.Text = string.Empty;
        }

        private void UpdatePostedStatusInPreTable()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            try
            {
                if (txtPurchDocumentNo.Text != string.Empty)
                {
                    #region Update Pre Header Status

                    string QueryUpdate = " UPDATE [ax].[ACXPURCHINVRECIEPTHEADERPRE] SET STATUS = 1  where PURCH_RECIEPTNO='" + txtPurchDocumentNo.Text + "' " +
                                         " and SITE_CODE='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                    obj.ExecuteCommand(QueryUpdate);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }

        }

        public void UpdateTransTable(string PostedDocumentNo)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                conn = obj.GetConnection();
                string TransLocation = "";

                string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    TransLocation = dt.Rows[0]["MainWarehouse"].ToString();
                }

                string st = Session["SiteCode"].ToString();
                string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[ACX_PURCHINVC_UPDATEINVENTTRANS]";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@SITECODE", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@DOCUMENTPURCHRECEIPTNUMBER", PostedDocumentNo);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@TRANSID", TransId);
                cmd.Parameters.AddWithValue("@WAREHOUSE", TransLocation);
                cmd.Parameters.AddWithValue("@TRANSTYPE", 1);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = "Error: Inventory Update Issue - " + ex.Message.ToString();
            }
        }

        private void GetUnPostedReferenceData(string queryString)
        {
            if (Request.QueryString["PreNo"].ToString() != string.Empty)
            {
                string ReferenceNo = Request.QueryString["PreNo"].ToString();

                bool b = CheckPostedStatus(ReferenceNo);
                if (b == false)
                {
                    ShowRecords(ReferenceNo);
                }
            }

        }

        private bool CheckPostedStatus(string ReferenceNo)
        {
            bool checkStatus = false;

            LblMessage.Text = "";
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                string query = " Select  [STATUS] from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' AND DOCUMENT_NO='" + ReferenceNo + "' ";

                object status = obj.GetScalarValue(query);
                if (status != null || status != string.Empty)
                {
                    string str = status.ToString();
                    if (str == "1")
                    {
                        checkStatus = true;
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Reference Number Not Found !! Redirecting back to previous page ...');", true);
                        LblMessage.Text = "Reference Number Not Found !! Redirecting back to previous page ...";
                        //string message = "alert(' Reference Number Not Found !! Redirecting back to previous page ...');";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        Response.Redirect("frmPurchUnPostList.aspx");
                        return checkStatus;
                    }
                }
                else
                {
                    checkStatus = true;
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Reference Status Not Exists !! Redirecting back to previous page ...');", true);

                    LblMessage.Text = "Reference Status Not Exists !! Redirecting back to previous page ...";
                    //string message = "alert(' Reference Status Not Exists !! Redirecting back to previous page ...');";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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

        private bool ValidateManualEntryExcel()
        {
            bool b = true;

            if (txtInvoiceNo.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice No.";
                txtInvoiceNo.Focus();
                b = false;
                return b;
            }
            if (txtInvoiceDate.Text == string.Empty)
            {
                LblMessage.Text = "► Please Provide Invoice Date.";
                txtInvoiceDate.Focus();
                b = false;
                return b;
            }
            //if (txtReceiptValue.Text == string.Empty)
            //{
            //    LblMessage.Text = "► Please Provide Receipt Value.";
            //    txtReceiptValue.Focus();
            //    b = false;
            //    return b;
            //}

            return b;
        }

        protected void btnUplaod_Click(object sender, EventArgs e)
        {
            try
            {
                LblMessage.Text = "";
                bool b = ValidateManualEntryExcel();
                if (b)
                {
                    if (AsyncFileUpload1.HasFile)
                    {
                        //  #region

                        //bool contains = true;
                        string fileName = System.IO.Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        AsyncFileUpload1.PostedFile.SaveAs(Server.MapPath("~/Uploads/" + fileName));
                        string excelPath = Server.MapPath("~/Uploads/") + Path.GetFileName(AsyncFileUpload1.PostedFile.FileName);
                        string conString = string.Empty;
                        string extension = Path.GetExtension(AsyncFileUpload1.PostedFile.FileName);
                        DataTable dtExcelData = new DataTable();

                        //excel upload
                        dtExcelData = CreamBell_DMS_WebApps.App_Code.ExcelUpload.ImportExcelXLS(Server.MapPath("~/Uploads/" + fileName), true);
                        string strCode = string.Empty;
                        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                        conn = obj.GetConnection();

                        #region PO  Number Generate

                        cmd = new SqlCommand();
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.Text;

                        DataTable dtNumSeq = obj.GetNumSequenceNew(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
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

                        strCode = obj.GetNumSequence(1, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                        cmd.CommandText = string.Empty;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "[ACX_ACXPURCHINVRECIEPTHEADERPRE]";

                        #endregion

                        #region Header Insert Data


                        string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTHEADERPRE]";
                        Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                        cmd.Parameters.Clear();
                        string indentno = string.Empty;
                        if (DrpIndentNo.SelectedItem.Text.ToString() == "-Select-")
                        {
                            indentno = "";
                        }
                        else
                        {
                            indentno = DrpIndentNo.SelectedItem.Text.ToString();
                        }
                        cmd.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@Purchase_Indent_No", indentno);
                        cmd.Parameters.AddWithValue("@Purchase_Indent_Date", txtIndentDate.Text);
                        cmd.Parameters.AddWithValue("@Transporter_Code", txtTransporterName.Text);
                        cmd.Parameters.AddWithValue("@Document_No", strCode);
                        cmd.Parameters.AddWithValue("@DocumentDate", txtReceiptDate.Text);
                        cmd.Parameters.AddWithValue("@VEHICAL_No", txtvehicleNo.Text);
                        cmd.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                        cmd.Parameters.AddWithValue("@Sale_InvoiceNo", txtInvoiceNo.Text.Trim().ToString());
                        cmd.Parameters.AddWithValue("@Sale_InvoiceDate", txtInvoiceDate.Text);
                        cmd.Parameters.AddWithValue("@VEHICAL_Type", txtVehicleType.Text);
                        cmd.Parameters.AddWithValue("@SO_No", string.Empty);
                        cmd.Parameters.AddWithValue("@Material_Value", "0");
                        cmd.Parameters.AddWithValue("@recid", Recid + 1);
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@STATUS", 0);                                          // for UnPosted Purchase Invoice Receipt Status//
                        cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                        cmd.Parameters.AddWithValue("@DRIVERNAME", txttransporterNo.Text.Trim().ToString());

                        #endregion


                        #region Line Insert Data on Same PURCH Order Number

                        cmd1 = new SqlCommand("[ACX_ACXPURCHINVRECIEPTLINEPRE]");
                        cmd1.Connection = conn;
                        if (transaction == null)
                        {
                            transaction = conn.BeginTransaction();
                        }
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;
                        string queryRecidLine = "Select Count(RECID) as RECID from [ax].[ACXPURCHINVRECIEPTLINEPRE]";
                        Int64 RecidLine = Convert.ToInt64(obj.GetScalarValue(queryRecidLine));
                        #endregion

                        string productNameCode = txtProductDesc.Text;
                        string[] str = productNameCode.Split('-');
                        string productCode = str[0].ToString();
                        //strCode = txtPurchDocumentNo.Text;

                        DataTable dtForShownUnuploadData = new DataTable();
                        dtForShownUnuploadData.Columns.Add("ProductCode");
                        dtForShownUnuploadData.Columns.Add("Qty");
                        //dtForShownUnuploadData.Columns.Add("Rate");
                        //dtForShownUnuploadData.Columns.Add("TRD");
                        //dtForShownUnuploadData.Columns.Add("Value");
                        //dtForShownUnuploadData.Columns.Add("Vat");
                        //dtForShownUnuploadData.Columns.Add("Remark");
                        int j = 0;
                        int no = 0;
                        for (int i = 0; i < dtExcelData.Rows.Count; i++)
                        {
                            string sqlstr = "select ItemID from ax.inventTable where ItemID = '" + dtExcelData.Rows[i]["ProductCode"].ToString() + "'";
                            object objcheckproductcode = obj.GetScalarValue(sqlstr);
                            if (objcheckproductcode == null)
                            {
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Rate"] = dtExcelData.Rows[i]["Rate"].ToString();
                                //dtForShownUnuploadData.Rows[j]["TRD"] = dtExcelData.Rows[i]["TRD"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Value"] = dtExcelData.Rows[i]["Value"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Vat"] = dtExcelData.Rows[i]["Vat"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Remark"] = dtExcelData.Rows[i]["Remark"].ToString();
                                j += 1;
                                continue;
                            }

                            try
                            {
                                decimal Qty = Convert.ToDecimal(dtExcelData.Rows[i]["Qty"]);
                                //decimal Rate = Convert.ToDecimal(dtExcelData.Rows[i]["Rate"]);
                                //decimal TRD = Convert.ToDecimal(dtExcelData.Rows[i]["TRD"]);
                                //decimal Value = Convert.ToDecimal(dtExcelData.Rows[i]["Value"]);
                                //decimal Vat = Convert.ToDecimal(dtExcelData.Rows[i]["Vat"]);

                                if (Qty == 0)
                                {
                                    dtForShownUnuploadData.Rows.Add();
                                    dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                    //dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                    //dtForShownUnuploadData.Rows[j]["Rate"] = dtExcelData.Rows[i]["Rate"].ToString();
                                    //dtForShownUnuploadData.Rows[j]["TRD"] = dtExcelData.Rows[i]["TRD"].ToString();
                                    //dtForShownUnuploadData.Rows[j]["Value"] = dtExcelData.Rows[i]["Value"].ToString();
                                    //dtForShownUnuploadData.Rows[j]["Vat"] = dtExcelData.Rows[i]["Vat"].ToString();
                                    //dtForShownUnuploadData.Rows[j]["Remark"] = dtExcelData.Rows[i]["Remark"].ToString();
                                    j += 1;
                                    continue;
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorSignal.FromCurrentContext().Raise(ex);
                                dtForShownUnuploadData.Rows.Add();
                                dtForShownUnuploadData.Rows[j]["ProductCode"] = dtExcelData.Rows[i]["ProductCode"].ToString();
                                dtForShownUnuploadData.Rows[j]["Qty"] = dtExcelData.Rows[i]["Qty"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Rate"] = dtExcelData.Rows[i]["Rate"].ToString();
                                //dtForShownUnuploadData.Rows[j]["TRD"] = dtExcelData.Rows[i]["TRD"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Value"] = dtExcelData.Rows[i]["Value"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Vat"] = dtExcelData.Rows[i]["Vat"].ToString();
                                //dtForShownUnuploadData.Rows[j]["Remark"] = dtExcelData.Rows[i]["Remark"].ToString();
                                j += 1;
                                continue;

                            }



                            cmd1.Parameters.Clear();


                            string[] ReturnArray = null;
                            ReturnArray = obj.CalculatePrice1(dtExcelData.Rows[i]["ProductCode"].ToString(), string.Empty, Convert.ToDecimal(dtExcelData.Rows[i]["Qty"].ToString()), DDLEntryType.SelectedItem.Text.ToString());
                            if (ReturnArray != null)
                            {

                                cmd1.Parameters.AddWithValue("@RECID", RecidLine + 1 + i);
                                cmd1.Parameters.AddWithValue("@Site_Code", Session["SiteCode"].ToString());
                                cmd1.Parameters.AddWithValue("@Purchase_Reciept_No", strCode);
                                cmd1.Parameters.AddWithValue("@PRODUCT_CODE", dtExcelData.Rows[i]["ProductCode"].ToString());
                                cmd1.Parameters.AddWithValue("@LINE_NO", RecidLine + 1 + i);
                                cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());

                                #region Crate Box Data Insert

                                if (ReturnArray[5].ToString() == "Box")
                                {
                                    cmd1.Parameters.AddWithValue("@BOX", dtExcelData.Rows[i]["Qty"].ToString());
                                    cmd1.Parameters.AddWithValue("@CRATES", ReturnArray[0].ToString());
                                    cmd1.Parameters.AddWithValue("@LTR", ReturnArray[1].ToString());
                                    cmd1.Parameters.AddWithValue("@RATE", 0);
                                    cmd1.Parameters.AddWithValue("@UOM", ReturnArray[4].ToString());
                                    
                                    cmd1.Parameters.AddWithValue("@BASICVALUE", 0);
                                    cmd1.Parameters.AddWithValue("@TRDDISCPERC", 0);
                                   
                                    
                                    cmd1.Parameters.AddWithValue("@TRDDISCVALUE", 0);
                                    cmd1.Parameters.AddWithValue("@PRICEEQUALVALUE", 0);
                                    cmd1.Parameters.AddWithValue("@VAT_INC_PERC",0);
                                   
                                    cmd1.Parameters.AddWithValue("@VAT_INC_PERC_VALUE", 0);
                                    cmd1.Parameters.AddWithValue("@GROSSAMOUNT", 0);
                                    cmd1.Parameters.AddWithValue("@DISCOUNT", 0);
                                    cmd1.Parameters.AddWithValue("@TAX", 0);
                                    cmd1.Parameters.AddWithValue("@TAXAMOUNT", 0);
                                   
                                    cmd1.Parameters.AddWithValue("@AMOUNT", 0);
                                    cmd1.Parameters.AddWithValue("@Remark", "FOC");
                                    cmd1.ExecuteNonQuery();
                                    no += 1;

                                }
                                #endregion

                            }

                        }


                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                        ShowRecords(strCode);
                        UpdateAndShowTotalMaterialValue(strCode);

                        BtnUpdateHeader.Visible = true;
                        LblMessage.Text = "Records Uploaded Successfully. Total Records : " + dtExcelData.Rows.Count + ". Uploaded : " + no + " Records.";

                        if (dtForShownUnuploadData.Rows.Count > 0)
                        {
                            gridviewRecordNotExist.DataSource = dtForShownUnuploadData;
                            gridviewRecordNotExist.DataBind();
                            ModalPopupExtender1.Show();
                        }
                        else
                        {
                            gridviewRecordNotExist.DataSource = null;
                            gridviewRecordNotExist.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                transaction.Rollback();
                LblMessage.Text = ex.Message.ToString();
            }
        }

        protected void rdoManualEntry_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoManualEntry")
            {
                panelAddLine.Visible = true;
                AsyncFileUpload1.Visible = false;
                btnUplaod.Visible = false;
                LblMessage.Text = "";

                txtIndentDate.Text = string.Empty;
                txtTransporterName.Text = string.Empty;
                txttransporterNo.Text = string.Empty;
                txtvehicleNo.Text = string.Empty;
                txtInvoiceNo.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                txtVehicleType.Text = string.Empty;
                GridFOCPurchItems.DataSource = null;
                GridFOCPurchItems.Visible = false;
                txtPurchDocumentNo.Text = string.Empty;


                GridFOCPurchItems.DataSource = null;
                GridFOCPurchItems.DataBind();
                txtPurchDocumentNo.Text = "";
            }
            else
            {
                panelAddLine.Visible = false;
                AsyncFileUpload1.Visible = true;
                btnUplaod.Visible = true;
                LblMessage.Text = "";
                txtIndentDate.Text = string.Empty;
                txtTransporterName.Text = string.Empty;
                txttransporterNo.Text = string.Empty;
                txtvehicleNo.Text = string.Empty;
                txtInvoiceNo.Text = string.Empty;
                txtInvoiceDate.Text = string.Empty;
                txtVehicleType.Text = string.Empty;
                
                GridFOCPurchItems.DataSource = null;
                GridFOCPurchItems.Visible = false;
                txtPurchDocumentNo.Text = string.Empty;
                txtPurchDocumentNo.Text = "";
                GridFOCPurchItems.DataSource = null;
                GridFOCPurchItems.DataBind();

            }

        }

        protected void txtEntryValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                decimal EntryValueQty = 0;
                
                EntryValueQty = Convert.ToDecimal(txtEntryValue.Text);
                if (EntryValueQty != 0)
                {
                    txtWeight.Text = Convert.ToString(Math.Round((ProductWeight * EntryValueQty) / 1000, 2));
                    txtVolume.Text = Convert.ToString(Math.Round((ProductLitre * EntryValueQty) / 1000, 2));
                }
                else
                {
                    LblMessage.Text = "Entered Quantity always greater than zero..!!";
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }
    }
}