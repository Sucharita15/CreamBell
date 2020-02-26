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
    public partial class frmSaleInvoiceReturn : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        string query;
        DataTable dt = new DataTable();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1,cmd2;
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
                    btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
                    if (Session["SiteCode"] != null)
                    {
                        string siteid = Session["SiteCode"].ToString();
                        if (lblsite.Text == "")
                        {
                            lblsite.Text = siteid;
                        }
                        //==================For Warehouse Location==============                        
                        query = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                        DataTable dt = new DataTable();
                        dt = obj.GetData(query);
                        if (dt.Rows.Count > 0)
                        {
                            Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                        }
                    }
                    btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
                    Session["dtGrid"] = null;
                    txtInvoiceReturnDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);
                    //CalendarExtender3.StartDate = DateTime.Now;
                    GetInvoice();
            }

        }
        private void GetInvoice()
        {
            try
            {
                // query = "select distinct(Invoice_No) as Invoice_No from [ax].[ACXSALEINVOICEHEADER] where [SITEID]='" + lblsite.Text + "' and TranType=1";                
                //query = "select distinct(invoice_No) from [ax].[ACXSALEINVOICEHEADER] where [Siteid]='" + lblsite.Text + "' and TranType=1 and invoic_date>='1-Jul-2017' " +
                //        " and invoice_No not in (select So_No from [ax].[ACXSALEINVOICEHEADER] where [Siteid]='" + lblsite.Text + "' and TranType=2)" +
                //        " order by invoice_No desc";                
                //
                //dt = new DataTable();
                //dt = obj.GetData(query);
                conn = obj.GetConnection();
                cmd = new SqlCommand("GetInvoiceNoForSaleReveral");
                //transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                //cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Siteid", lblsite.Text);
                //cmd.Parameters.AddWithValue("@Customercode", drpCustomerCode.SelectedValue.ToString());
                //cmd.Parameters.AddWithValue("@InvoiceType", type);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Dispose();

                if (dt.Rows.Count > 0)
                {
                    drpInvNo.DataSource = dt;
                    drpInvNo.DataTextField = "Invoice_No";
                    drpInvNo.DataValueField = "Invoice_No";
                    drpInvNo.DataBind();
                    drpInvNo.Items.Insert(0, new ListItem("--Select--", "0"));
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }     
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //string strInvoiceNo = obj.GetNumSequence(7, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());
                bool b = Validation();
                if (b == true)
                {
                    if (strmessage == string.Empty)
                    {
                        SaveHeader();
                        
                        GetInvoice();
                        ClearAll();
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public string InvoiceNo()
        {
            try
            {
                string IndNo = string.Empty;
                string Number = string.Empty;
                int intTotalRec;               
               // string strQuery = "Select ISNULL(MAX(CAST(RIGHT(Invoice_No,8) AS INT)),0)+1 as new_InvoiceNo from [ax].[ACXSALEINVOICEHEADER] where [Siteid]='" + lblsite.Text + "' and TranType=2";
                string strQuery = "Select ISNULL(MAX(CAST(RIGHT(Invoice_No,7) AS INT)),0)+1 as new_InvoiceNo from [ax].[ACXSALEINVOICEHEADER] where [Siteid]='" + lblsite.Text + "' and TranType=2";
                DataTable dt = new DataTable();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                dt = obj.GetData(strQuery);
                intTotalRec = dt.Rows.Count;

                if (dt.Rows[0]["new_InvoiceNo"].ToString() != "0")
                {
                    string st = dt.Rows[0]["new_InvoiceNo"].ToString();
                    if (st.Length < 7)
                    {
                        int len = st.Length;
                        int plen = 7 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                        IndNo = "RT-"+ st;
                        return IndNo;
                    }
                }
                else
                {
                    string st = "1";
                    if (st.Length < 7)
                    {
                        int len = st.Length;
                        int plen = 7 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                        IndNo = "RT-" + st;
                        return IndNo;
                    }

                }

                return IndNo;                
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                strmessage = ex.Message.ToString();
                return strmessage;
            }

        }
        private bool Validation()
        {
            bool returnvalue = false;
            try { 
            if (drpCustomerCode.Text == "")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Customer Name !');", true);
                drpCustomerGroup.Focus();
                returnvalue = false;
                return returnvalue;
            }
            else if (drpCustomerGroup.Text == "")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Customer Group !');", true);
                drpCustomerGroup.Focus();
                returnvalue = false;
                return returnvalue;
            }

            else if (txtInvoiceReturnDate.Text == "")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Invoice Return Date !');", true);
                txtInvoiceReturnDate.Focus();
                returnvalue = false;
                return returnvalue;
            }
            DataTable dtLineTable = (DataTable)Session["dtGrid"];
            if (dtLineTable == null )
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Return Quantity !');", true);
                returnvalue = false;
                return returnvalue;
            }
            if (dtLineTable.Rows.Count == 0)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Return Quantity !');", true);
                returnvalue = false;
                return returnvalue;
            }
            decimal dectotretqty = 0;
            dectotretqty =Convert.ToDecimal(dtLineTable.Compute("sum(Total_ReturnQty)", ""));
            if (dectotretqty <=0)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please add atleast one return quantity !');", true);
                returnvalue = false;
                return returnvalue;
            }
            //else
            //{
            //    returnvalue = true;
            //}
            //============Check Stock Item Line By Line======

            for (int i = 0; i < gvDetails.Rows.Count; i++)
            {
                //Label Product = (Label)gvDetails.Rows[i].Cells[2].FindControl("Product");
                //string productNameCode = Product.Text;
                //string[] str = productNameCode.Split('-');
                //string ProductCode = str[0].ToString();
                Label box = (Label)gvDetails.Rows[i].Cells[5].FindControl("txtBox");
                string Qty = box.Text;
                decimal TransQty = Convert.ToDecimal(Qty);


                if (TransQty > 0)
                {
                    returnvalue = true;
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please check the Invoice Qty !');", true);
                    returnvalue = false;
                    return returnvalue;
                }
            }  
                      
            return returnvalue;
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + ex.Message.Replace("'","''") + "');", true);
                returnvalue = false;
                return returnvalue;
            }
        }
        public void UpdateTransTable()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                //==================Common Data-==============
                string st = Session["SiteCode"].ToString();
                string Siteid = Session["SiteCode"].ToString();
                string DATAAREAID = Session["DATAAREAID"].ToString();
                int TransType = 4;//1 for Slae invoice 
                int DocumentType = 4;
                string DocumentNo = txtInvoiceReturnNo.Text;
                string DocumentDate = txtInvoiceReturnDate.Text;
                string uom = "BOX";
                string Referencedocumentno = drpInvNo.SelectedItem.Text;
                string TransLocation = Session["TransLocation"].ToString();
                //============Loop For LineItem==========
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    Label Product = (Label)gvDetails.Rows[i].Cells[2].FindControl("Product");
                    Label box = (Label)gvDetails.Rows[i].Cells[5].FindControl("txtBox");

                    string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                    string productNameCode = Product.Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    string Qty = box.Text;
                    decimal TransQty = Convert.ToDecimal(Qty);
                    int REcid = i + 1;

                    if (TransQty > 0)
                    {
                        string query = " Insert Into ax.acxinventTrans " +
                                "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno])" +
                                " Values ('" + TransId + "','" + Siteid + "','" + DATAAREAID + "'," + REcid + ",getdate()," + TransType + "," + DocumentType + ",'" + DocumentNo + "'," +
                                " '" + DocumentDate + "','" + ProductCode + "'," + TransQty + ",'" + uom + "','" + TransLocation + "','" + Referencedocumentno + "')";
                        obj.ExecuteCommand(query);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void SaveData()  
        {
            try
            {
                DataTable dtNumSeq = obj.GetNumSequenceNew(7, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");               
                string NUMSEQ = string.Empty;
                if (dtNumSeq != null)
                {
                    txtInvoiceReturnNo.Text= dtNumSeq.Rows[0][0].ToString();
                    NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }
                conn = obj.GetConnection();
                cmd = new SqlCommand("ACX_SALEREVERSAL");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@INVOICENONEW", txtInvoiceReturnNo.Text);
                cmd.Parameters.AddWithValue("@NumSeq", NUMSEQ);
                cmd.Parameters.AddWithValue("@INVOICENOOLD", drpInvNo.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@SITEID", lblsite.Text);
                cmd.ExecuteNonQuery();
                transaction.Commit();


                cmd = new SqlCommand("ACX_UPDATESALEREVERSAL");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SITEID", lblsite.Text);
                cmd.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceReturnNo.Text);
                cmd.ExecuteNonQuery();
                return;
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Try Again!!');", true);
                txtInvoiceReturnNo.Text = "";
            }
            finally
            {

            }
        }
        public void ClearAll()
        {
         
            drpCustomerCode.Items.Clear();
            drpCustomerCode.Items.Clear();
            txtInvoiceDate.Text = "";
            txtTIN.Text = "";
            txtAddress.Text = "";
            txtMobileNO.Text = "";           
            txtTransporterName.Text = "";
            txtDriverContact.Text = "";
            txtDriverName.Text = "";
            txtVehicleNo.Text = "";
            txtInvoiceReturnNo.Text = "";
            txtInvoiceReturnDate.Text = "";
            chkCompReversal.Checked = false;
            txtInvoiceReturnDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);
            //CalendarExtender3.StartDate = DateTime.Now;

            gvDetails.DataSource = null;
            gvDetails.DataBind();

        }
        protected void txtTransporterName_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtDriverName_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtDriverContact_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtVehicleNo_TextChanged(object sender, EventArgs e)
        {

        }     
        protected void drpInvNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //=================Fill Header Part===============
                query = "Select A.[SITEID],CONVERT(VARCHAR(11),A.[INVOIC_DATE],106) as [INVOIC_DATE]," +
                    " B.CUST_GROUP,Customer_Group=(Select C.CUSTGROUP_NAME from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=B.CUST_GROUP)," +
                    " B.Customer_Code,concat(B.Customer_Code,'-', B.Customer_Name) as Customer_Name,B.[MOBILE_NO],B.Address1,B.Mobile_No,B.VAT" +
                    " from [ax].[ACXSALEINVOICEHEADER] A " +
                    " Inner Join [ax].[ACXCUSTMASTER] B on B.[CUSTOMER_CODE]=A.[CUSTOMER_CODE] " +
                    " where A.[SITEID]='" + lblsite.Text + "' and A.[INVOICE_NO]='" + drpInvNo.SelectedItem.Text + "' and TranType=1 and A.[INVOIC_DATE] >= '1-Jul-2017' ";

                dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    drpCustomerCode.Items.Clear();
                    drpCustomerGroup.Items.Clear();
                    drpCustomerGroup.Items.Insert(0, new ListItem(dt.Rows[0]["Customer_Group"].ToString(), dt.Rows[0]["CUST_GROUP"].ToString()));
                    drpCustomerCode.Items.Insert(0, new ListItem(dt.Rows[0]["Customer_Name"].ToString(), dt.Rows[0]["Customer_Code"].ToString()));
                    txtTIN.Text = dt.Rows[0]["VAT"].ToString();
                    txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                    txtMobileNO.Text = dt.Rows[0]["Mobile_No"].ToString();
                    txtInvoiceDate.Text = dt.Rows[0]["INVOIC_DATE"].ToString();
                }
                else
                {
                    drpCustomerCode.Items.Clear();
                    drpCustomerGroup.Items.Clear();
                    txtTIN.Text = "";
                    txtAddress.Text = "";
                    txtMobileNO.Text = "";
                    txtInvoiceDate.Text = "";
                }
                //================Fill Line Item=============
                query = "EXEC USP_GETSALEINVOICERETURNLINE '" + drpInvNo.SelectedItem.Text + "','" + lblsite.Text + "'";

                    //query = "select cast(A.BOX as decimal(10, 2))-cast((select isnull(sum(box), 0)"+
                    //        "from ax.ACXSALEINVOICEline aa "+
                    //        "left join ax.ACXSALEINVOICEheader bb on aa.INVOICE_NO = bb.INVOICE_NO"+
                    //        " and aa.SITEID = bb.SITEID where aa.trantype = 2 and aa.SITEID = A.[SITEID]"+
                    //        "and bb.SO_NO = A.INVOICE_NO) as decimal(10,2)) Balance_Qty, 0.0 [Rerturn_Qty_Box], 0.0 [Rerturn_Qty_Pcs], A.HSNCODE," +
                    //        "0.0 [Total_ReturnQty],E.ItemId as Product_Code,A.Line_No,E.ItemId + '-' + E.Product_Name as Product, "+
                    //        "cast(E.Product_PackSize as decimal(10, 2)) as Pack,cast(E.Product_MRP as decimal(10, 2)) as MRP, "+
                    //        "cast(A.BOX as decimal(10, 2)) as Invoice_Qty,cast(A.LTR as decimal(10, 2)) as R_Ltr,'0.00'[Ltr],"+
                    //        "coalesce(cast(A.Rate as decimal(10, 2)),0) as Rate,cast(A.Tax_Code as decimal(10, 2)) as Tax_Code,"+
                    //        "cast(A.[TAX_AMOUNT] as decimal(10, 2)) as R_TaxValue,'0.00'[TaxValue], cast(A.LineAmount as decimal(10, 2)) as [R_LineAMOUNT], '0.00'[LineAMOUNT],"+
                    //        "cast(A.AddTax_Code as decimal(10, 2)) as AddTax_Code,cast(A.AddTax_Amount as decimal(10, 2)) as R_AddTax_Amount,'0.00'[AddTax_Amount],"+
                    //        "cast(A.Disc as decimal(10, 2)) as Disc,cast(A.Disc_Amount as decimal(10, 2)) as R_Disc_Amount,'0.00'[Disc_Amount],"+
                    //        "cast(A.[AMOUNT] as decimal(10, 2)) as R_Amount,'0.00' Amount , A.SchemeCode,A.DiscType,A.DiscCalculationBase," +
                    //        "cast(A.TDValue as decimal(10, 2)) as R_TDValue,'0.00'[TDValue],cast(A.PEValue as decimal(10, 2)) as R_PEValue ,'0.00'[PEValue],"+
                    //        " cast(A.Sec_Disc_Amount as decimal(10, 2)) as R_Sec_Disc_Amount,'0.00'[Sec_Disc_Amount], TAXCOMPONENT,ADDTAXCOMPONENT,cast(A.SCHEMEDISCPER as decimal(10, 2)) as SCHEMEDISCPER ,cast(A.SCHEMEDISCVALUE as decimal(10, 2)) as R_SCHEMEDISCVALUE,'0.00'[SCHEMEDISCVALUE],cast(A.taxableamount as decimal(10, 2)) as R_TaxableAmount,'0.00' [TaxableAmount],A.BasePrice  from [ax].[ACXSALEINVOICELINE] A" +
                    //        " left Join ax.InventTable E on A.Product_COde=E.ItemId " +
                    //        " where A.[SITEID]='" + lblsite.Text + "' and A.[INVOICE_NO]='" + drpInvNo.SelectedItem.Text + "' and TranType=1 order by a.line_no asc";

                dt = new DataTable();
                dt = obj.GetData(query);
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
                chkCompReversal.Checked = false;
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    col.ReadOnly = false;
                    //string temp = col.DataType.ToString();
                    if (col.DataType.Name == "String")
                    {
                        col.MaxLength = 500;
                    }
                }
                Session["dtGrid"] = dt;
                //dt = (DataTable)Session["dtGrid"];
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');window.location=window.location;";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }
        }

        protected void txtRetQtyBox_TextChanged(object sender, EventArgs e)
        {
            DataTable dtOrignalGrid = (DataTable)Session["dtGrid"];
            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            TextBox txtReturnBox = (TextBox)row.FindControl("txtRetQtyBox");
            TextBox txtReturnPcs = (TextBox)row.FindControl("txtRetQtyBPcs");
            HiddenField hfRetQtyBox = (HiddenField)row.FindControl("hfRetQtyBox");
            HiddenField hfReturnPcs = (HiddenField)row.FindControl("hfRetQtyBPcs");
            Label lblTotalReturnBox = (Label)row.FindControl("txtTotalReturnQty");
            Label lblBalanceQty = (Label)row.FindControl("txtBalQty");
            Label lblPackSize = (Label)row.FindControl("Pack");
            //Label lblLTR = (Label)row.FindControl("LTR");
            //Label lblTaxValue = (Label)row.FindControl("TaxValue");
            //Label lblAddTaxAmount = (Label)row.FindControl("AddTaxValue");
            //Label lblDiscAmount = (Label)row.FindControl("DiscValue");
            //Label lblSecDiscAmount = (Label)row.FindControl("SecDiscValue");
            //Label lblTDValue = (Label)row.FindControl("lblTDValue");
            //Label lblPEValue = (Label)row.FindControl("lblPEValue");
            //Label lblAmount = (Label)row.FindControl("Amount");
            Label lblLineNo = (Label)row.FindControl("Line_No");
            //Label lblInvNo = (Label)row.FindControl("txtBox");
            //Label lblSCHEMEDISCPER = (Label)row.FindControl("lblSCHEMEDISCPER");
            //Label lblSCHEMEDISCVALUE = (Label)row.FindControl("lblSCHEMEDISCVALUE");
            DataTable dtGrid = new DataTable();
            dt = (DataTable)gvDetails.DataSource;
            decimal BalQty = Convert.ToDecimal(lblBalanceQty.Text);
            decimal newreturnbal;
            if (txtReturnBox.Text == "")
                txtReturnBox.Text = "0";
            if (txtReturnPcs.Text == "")
                txtReturnPcs.Text = "0";
            decimal oldReturnBal = Convert.ToDecimal(lblTotalReturnBox.Text);
            newreturnbal = (Convert.ToDecimal(txtReturnBox.Text == "" ? "0" : txtReturnBox.Text) + (lblPackSize.Text == "0" ? 1 : (Convert.ToDecimal(txtReturnPcs.Text == "" ? "0" : txtReturnPcs.Text) / Convert.ToDecimal(lblPackSize.Text))));

            if ((newreturnbal - oldReturnBal) > BalQty)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "javascript:BoxQtyError();", true);
                //Response.Write("<script>alert('Return Quantity cannot exceed the Balance Quantity');</script>");
                txtReturnBox.Text = hfRetQtyBox.Value.ToString();
                txtReturnPcs.Text = hfReturnPcs.Value.ToString();
            }
            else
            {
                DataRow[] rw = dtOrignalGrid.Select("Line_NO=" + lblLineNo.Text);
                if (rw.Length > 0)
                {
                    rw[0]["Balance_Qty"] = (Convert.ToDecimal(lblBalanceQty.Text) - (newreturnbal - oldReturnBal)).ToString("0.0000");
                    rw[0]["Rerturn_Qty_Box"] = Convert.ToDecimal(txtReturnBox.Text);
                    rw[0]["Rerturn_Qty_Pcs"] = (Convert.ToDecimal(txtReturnPcs.Text));
                    rw[0]["Total_ReturnQty"] = newreturnbal.ToString("0.0000");
                    rw[0]["R_Ltr"] = Convert.ToDecimal(rw[0]["Ltr"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal;
                    rw[0]["R_Tax_AMOUNT"] = (Convert.ToDecimal(rw[0]["TAX_AMOUNT"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_LineAMOUNT"] = (Convert.ToDecimal(rw[0]["LineAMOUNT"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_AddTax_Amount"] = (Convert.ToDecimal(rw[0]["AddTax_Amount"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_Disc_Amount"] = (Convert.ToDecimal(rw[0]["Disc_Amount"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_Amount"] = (Convert.ToDecimal(rw[0]["Amount"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_TDValue"] = (Convert.ToDecimal(rw[0]["TDValue"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_PEValue"] = (Convert.ToDecimal(rw[0]["PEValue"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_Sec_Disc_Amount"] = (Convert.ToDecimal(rw[0]["Sec_Disc_Amount"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_SCHEMEDISCVALUE"] = (Convert.ToDecimal(rw[0]["SCHEMEDISCVALUE"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    rw[0]["R_TaxableAmount"] = (Convert.ToDecimal(rw[0]["TaxableAmount"]) / Convert.ToDecimal(rw[0]["Invoice_Qty"]) * newreturnbal).ToString("0.000000");
                    dtOrignalGrid.AcceptChanges();
                    Session["dtGrid"] = dtOrignalGrid;
                }
                dtGrid = (DataTable)Session["dtGrid"];
                
                gvDetails.DataSource = dtGrid;
                gvDetails.DataBind();
                hfRetQtyBox.Value = txtReturnBox.Text;
                hfReturnPcs.Value = txtReturnPcs.Text;
                //Session["dtGrid"] = dtGrid;

            }

        }
        public void SaveHeader()
        {
            try
            {
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                //string query;                
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                DataTable dtNumSEQ = obj.GetNumSequenceNew(7, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                string SO_NO = string.Empty;
                string NUMSEQ = string.Empty;
                if (dtNumSEQ != null)
                {
                    txtInvoiceReturnNo.Text = dtNumSEQ.Rows[0][0].ToString();
                    NUMSEQ = dtNumSEQ.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }


                DataTable dt = new DataTable();
                //======Save Header===================
                conn = obj.GetConnection();
                cmd = new SqlCommand("ACX_SaleInvoice_Header");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                //string sono = Session["SO_NOList"].ToString();//Cache["SO_NO"].ToString(); 
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@SITEID", lblsite.Text);
                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", drpCustomerCode.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@ACTUALINVOICE_NO", drpInvNo.Text);
                cmd.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceReturnNo.Text);
                //cmd.Parameters.AddWithValue("@SO_NO", drpSONumber.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@SO_NO", drpInvNo.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@INVOIC_DATE", txtInvoiceReturnDate.Text);
                cmd.Parameters.AddWithValue("@SO_DATE", txtInvoiceDate.Text);
                cmd.Parameters.AddWithValue("@CUSTGROUP_CODE", drpCustomerGroup.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@LOADSHEET_NO", txtLoadSheetNumber.Text);
                //cmd.Parameters.AddWithValue("@LOADSHEET_DATE", txtLoadsheetDate.Text);
                cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtTransporterName.Text);
                cmd.Parameters.AddWithValue("@VEHICAL_NO", txtVehicleNo.Text);
                cmd.Parameters.AddWithValue("@DRIVER_CODE", txtDriverName.Text);
                cmd.Parameters.AddWithValue("@DRIVER_MOBILENO", txtDriverContact.Text);
                cmd.Parameters.AddWithValue("@status", "INSERT");
                cmd.Parameters.AddWithValue("@TranType", 2);
                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                
                //cmd.Parameters.AddWithValue("@Remark", txtRemark.Text);


                //========Save Line================


                int i = 0;
                decimal totamt = 0;

                //===================New===============
                cmd1 = new SqlCommand("Acx_Insert_SaleInvoiceLine");
                //string abc = drpInvNo.Text.ToString();
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;
                DataTable dtLineTable = (DataTable)Session["dtGrid"];
                int count = dtLineTable.Rows.Count;
                foreach (DataRow row in dtLineTable.Rows)
                {
                    i = i + 1;
                    //totamt = totamt + Tamount;
                    if (Convert.ToDecimal(row["Total_ReturnQty"]) > 0)
                    {
                        //cmd1.CommandText = "";
                        cmd1.Parameters.Clear();
                        cmd1.Parameters.AddWithValue("@status", "Insert");
                        cmd1.Parameters.AddWithValue("@SITEID", lblsite.Text);
                        cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd1.Parameters.AddWithValue("@RECID", "");
                        cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", drpCustomerCode.SelectedItem.Value);
                        cmd1.Parameters.AddWithValue("@ACTUALINVOICE_NO", drpInvNo.Text);
                        cmd1.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceReturnNo.Text);
                        cmd1.Parameters.AddWithValue("@LINE_NO", i);
                        cmd1.Parameters.AddWithValue("@PRODUCT_CODE", row["Product_Code"]);
                        cmd1.Parameters.AddWithValue("@PRODUCTGROUP_CODE", "");
                        cmd1.Parameters.AddWithValue("@AMOUNT", row["Amount"]);
                        cmd1.Parameters.AddWithValue("@BOX", Convert.ToDecimal(row["Total_ReturnQty"]));
                        cmd1.Parameters.AddWithValue("@CRATES", Convert.ToDecimal(0));
                        cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(row["Ltr"]));
                        cmd1.Parameters.AddWithValue("@QUANTITY", Convert.ToDecimal(0));
                        cmd1.Parameters.AddWithValue("@MRP", row["MRP"]);
                        cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(row["Rate"]));
                        cmd1.Parameters.AddWithValue("@TAX_CODE", Convert.ToDecimal(row["Tax_Code"]));
                        cmd1.Parameters.AddWithValue("@TAX_AMOUNT", Convert.ToDecimal(row["R_TAX_AMOUNT"]));
                        cmd1.Parameters.AddWithValue("@ADDTAX_CODE", Convert.ToDecimal(row["AddTax_Code"]));
                        cmd1.Parameters.AddWithValue("@ADDTAX_AMOUNT", Convert.ToDecimal(row["R_AddTax_Amount"]));
                        cmd1.Parameters.AddWithValue("@DISC_AMOUNT", Convert.ToDecimal(row["R_Disc_Amount"]));
                        cmd1.Parameters.AddWithValue("@SEC_DISC_AMOUNT", Convert.ToDecimal(row["R_Sec_Disc_Amount"]));
                        cmd1.Parameters.AddWithValue("@TranType", 2);
                        cmd1.Parameters.AddWithValue("@DiscType",Convert.ToInt16(row["DiscType"]));
                        cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(row["Disc"]));
                        cmd1.Parameters.AddWithValue("@SchemeCode", Convert.ToString(row["SCHEMECODE"]));
                        cmd1.Parameters.AddWithValue("@LINEAMOUNT", Convert.ToDecimal(row["R_LineAmount"]));
                        cmd1.Parameters.AddWithValue("@DiscCalculationBase",Convert.ToInt16(row["DiscCalculationBase"]));
                        cmd1.Parameters.AddWithValue("@TDValue",Convert.ToDecimal(row["R_TDValue"]));
                        cmd1.Parameters.AddWithValue("@PEValue",Convert.ToDecimal(row["R_PEValue"]));


                        cmd1.Parameters.AddWithValue("@BasePrice", Convert.ToDecimal(row["BASEPRICE"]));
                        cmd1.Parameters.AddWithValue("@BOXQty", Convert.ToDecimal(row["Rerturn_Qty_Box"]));
                        cmd1.Parameters.AddWithValue("@PcsQty", Convert.ToDecimal(row["Rerturn_Qty_Pcs"].ToString()==""?"0": row["Rerturn_Qty_Pcs"]));
                        string boxPcs = "";
                        boxPcs = Convert.ToDecimal(row["Rerturn_Qty_Pcs"].ToString() == "" ? "0" : row["Rerturn_Qty_Pcs"]).ToString("0");
                        boxPcs = Convert.ToDecimal(row["Rerturn_Qty_Box"]).ToString("0") + "." + (boxPcs.Length > 1 ? boxPcs : "0" + boxPcs);
                        cmd1.Parameters.AddWithValue("@BOXPcs", boxPcs);
                        cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(row["R_TAXABLEAMOUNT"]));
                        cmd1.Parameters.AddWithValue("@HSNCODE", row["HSNCODE"]);
                        cmd1.Parameters.AddWithValue("@TAXCOMPONENT", row["TAXCOMPONENT"]);
                        cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", row["ADDTAXCOMPONENT"]);
                        cmd1.Parameters.AddWithValue("@SO_NO", drpInvNo.SelectedItem.Text);

                        cmd1.Parameters.AddWithValue("@SchemeDiscPer", Convert.ToDecimal(row["SCHEMEDISCPER"]));
                        cmd1.Parameters.AddWithValue("@SchemeDiscVal", Convert.ToDecimal(row["R_SCHEMEDISCVALUE"]));

                        totamt = totamt + Convert.ToDecimal(row["R_Amount"]); 
                        cmd1.ExecuteNonQuery();
                      }
                  }
                //==============Remaining Part Of Header-===============
                cmd.Parameters.AddWithValue("@INVOICE_VALUE", totamt);
                cmd.ExecuteNonQuery();

                //===============Update Transaction Table===============

                cmd2 = new SqlCommand("ACX_Insert_InventTransTable"); //new SqlCommand();
                cmd2.Connection = conn;
                cmd2.Transaction = transaction;
                cmd2.CommandTimeout = 3600;
                cmd2.CommandType = CommandType.StoredProcedure; //CommandType.Text;
                //==================Common Data-==============
                string st = Session["SiteCode"].ToString();
                string Siteid = Session["SiteCode"].ToString();
                string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                string DATAAREAID = Session["DATAAREAID"].ToString();
                int TransType = 4;//1 for Slae invoice 
                int DocumentType = 4;
                string DocumentNo = txtInvoiceReturnNo.Text;
                string DocumentDate = txtInvoiceDate.Text;
                string uom = "BOX";
                //string Referencedocumentno = drpSONumber.SelectedItem.Text;
                string TransLocation = Session["TransLocation"].ToString();

                int l = 0;

                //============Loop For LineItem==========
                for (int k = 0; k < gvDetails.Rows.Count; k++)
                {
                    Label Product = (Label)gvDetails.Rows[k].Cells[2].FindControl("Product");
                    Label box = (Label)gvDetails.Rows[k].Cells[6].FindControl("txtTotalReturnQty");

                    string productNameCode = Product.Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    string Qty = box.Text;
                    decimal TransQty = App_Code.Global.ConvertToDecimal(Qty);
                    int REcid = k + 1;

                    if (TransQty > 0)
                    {

                        cmd2.Parameters.Clear();
                        cmd2.Parameters.AddWithValue("@status", "Insert");
                        cmd2.Parameters.AddWithValue("@SITEID", lblsite.Text);
                        cmd2.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd2.Parameters.AddWithValue("@RECID", "");
                        cmd2.Parameters.AddWithValue("@TransId", TransId);
                        cmd2.Parameters.AddWithValue("@TransType", TransType);
                        cmd2.Parameters.AddWithValue("@DocumentType", DocumentType);
                        cmd2.Parameters.AddWithValue("@DocumentNo", DocumentNo);
                        cmd2.Parameters.AddWithValue("@DocumentDate", DocumentDate);
                        cmd2.Parameters.AddWithValue("@ProductCode", ProductCode);
                        cmd2.Parameters.AddWithValue("@TransQty", TransQty);
                        cmd2.Parameters.AddWithValue("@uom", uom);
                        cmd2.Parameters.AddWithValue("@TransLocation", TransLocation);
                        //cmd2.Parameters.AddWithValue("@SO_NO", drpInvNo.SelectedItem.Text);

                        // cmd2.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);
                        cmd2.Parameters.AddWithValue("@TransLineNo", l);
                        
                        cmd2.ExecuteNonQuery();

                        l += 1;
                    }

                }
                //============Loop For Scheme LineItem==========


                //===================================================
                // Insert For Scheme Line

                

                //============Commit All Data================
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Sale Reversal No: " + txtInvoiceReturnNo.Text + " Saved Successfully!!');window.location=window.location;", true);
                transaction.Commit();
                SqlCommand cmd3 = new SqlCommand("ACX_UPDATESALEREVERSAL");
                
                cmd3.Connection = conn;
                cmd3.CommandTimeout = 3600;
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@SITEID", lblsite.Text);
                cmd3.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceReturnNo.Text);
                cmd3.ExecuteNonQuery();

                Session["SO_NOList"] = null;
                dtLineItems = null;
                Session["LineItem"] = null;
                Session["dtGrid"] = null;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //lblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');window.location=window.location;";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtInvoiceReturnNo.Text = "";
            }
            finally
            {
                conn.Close();
            }
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            GridViewRow row1 = (GridViewRow)activeCheckBox.Parent.Parent;
            String SchemeCode1 = row1.Cells[1].Text;
            String SetNo1 = row1.Cells[7].Text;
            TextBox txtQty = (TextBox)row1.FindControl("txtQty");
            TextBox txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");

            if (App_Code.Global.GetPrevSelection(SchemeCode1, SetNo1, ref gvScheme,"0")) { activeCheckBox.Checked = false; return; }
            #region For Selection
            if (activeCheckBox.Checked)
            {
                GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                string SchemeCode = row.Cells[1].Text;
                string SetNo = row.Cells[7].Text;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    #region Same Scheme Validation 1st Check
                    //CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    //if (chkBx.Checked)
                    //{
                    //    if (rw.Cells[1].Text != SchemeCode)
                    //    {
                    //        activeCheckBox.Checked = false;

                    //        string message = "alert('You can select only one scheme items !');";
                    //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    //       return;
                    //    }
                    //    else if(rw.Cells[1].Text == SchemeCode && SetNo != rw.Cells[7].Text)
                    //    {
                    //        activeCheckBox.Checked = false;

                    //        string message = "alert('You can select only one scheme items with same set!');";
                    //        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                    //        return;
                    //    }
                    //}
                    #endregion

                    #region 2nd Check Same scheme and same set setno > 0 Auto select all scheme
                    CheckBox chkBxNew = (CheckBox)rw.FindControl("chkSelect");  //Auto Select All Same Set No
                    if (Convert.ToString(rw.Cells[7].Text) == SetNo && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(SetNo) > 0)
                    {

                        chkBxNew.Checked = true;
                        txtQty = (TextBox)rw.FindControl("txtQty");
                        txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                        HiddenField hdnTotalFreeQty = (HiddenField)rw.FindControl("hdnTotalFreeQty");
                        HiddenField hdnTotalFreeQtyPcs = (HiddenField)rw.FindControl("hdnTotalFreeQtyPcs");
                        txtQty.Text = hdnTotalFreeQty.Value;
                        txtQtyPcs.Text = hdnTotalFreeQtyPcs.Value;
                        txtQty.Enabled = false;
                        txtQtyPcs.Enabled = false;
                    }

                    #endregion
                }

                #region FreeBox Readonly validation
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    txtQty = (TextBox)rw.FindControl("txtQty");
                    if (chkBx.Checked)
                    {
                        txtQty.ReadOnly = false;
                    }
                    else
                    {
                        txtQty.Text = string.Empty;
                        txtQty.ReadOnly = true;
                    }
                }
                #endregion
                #endregion

                #region  checkbox enable for set no 0 only

                if (SetNo1 == "0")
                {
                    txtQty = (TextBox)row1.FindControl("txtQty");
                    txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
                    HiddenField hdnTotalFreeQty = (HiddenField)row1.FindControl("hdnTotalFreeQty");
                    HiddenField hdnTotalFreeQtyPcs = (HiddenField)row1.FindControl("hdnTotalFreeQtyPcs");

                    if (hdnTotalFreeQty.Value != "")
                    {
                        txtQty.ReadOnly = false;
                    }
                    else
                    {
                        txtQty.ReadOnly = true;
                    }
                    if (Session["ApplicableOnState"].ToString() == "Y")
                    {
                        if (hdnTotalFreeQtyPcs.Value != "")
                        {
                            txtQtyPcs.ReadOnly = false;
                        }
                        else
                        {
                            txtQtyPcs.ReadOnly = true;
                        }
                    }
                    else { txtQtyPcs.ReadOnly = true; }
                }
                #endregion

            }
            else
            {
                #region  For unchecked
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                    string SchemeCode = row.Cells[1].Text;
                    string SetNo = row.Cells[7].Text;
                    if (SchemeCode == SchemeCode1 && SetNo == SetNo1 && Convert.ToInt16(SetNo) > 0)
                    {

                        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                        chkBx.Checked = false;
                        txtQty = (TextBox)rw.FindControl("txtQty");
                        txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                        HiddenField hdnTotalFreeQty = (HiddenField)rw.FindControl("hdnTotalFreeQty");
                        HiddenField hdnTotalFreeQtyPcs = (HiddenField)rw.FindControl("hdnTotalFreeQtyPcs");
                        txtQty.Text = "";
                        txtQtyPcs.Text = "";
                    }
                }
                if (SetNo1 == "0")
                {
                    txtQty = (TextBox)row1.FindControl("txtQty");
                    txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPcs.Text = "";
                }
                #endregion
            }
            if (activeCheckBox.Checked)
            {
                if (Convert.ToInt32(SetNo1) > 0)
                {
                    txtQty.Enabled = false;
                    txtQtyPcs.Enabled = false;
                }
                else
                {
                    txtQty.Enabled = true;
                    txtQtyPcs.Enabled = true;
                }
            }
            else
            {
                txtQty.Enabled = false;
                txtQtyPcs.Enabled = false;
                txtQty.Text = "0";
                txtQtyPcs.Text = "0";
            }
            SchemeDiscCalculation();
        }

        protected decimal SchemeLineCalculation(decimal discPer)
        {
            decimal Rate = 0, Qty = 0, BasicAmount = 0, packSize = 0;
            decimal TotalBasicAmount = 0;
            decimal TaxPer = 0;
            decimal AddTaxPer = 0;
            decimal TaxAmount = 0;
            decimal AddTaxAmount = 0;

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                if (chkBx.Checked == true)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                    txtQtyPCS.Text = (txtQtyPCS.Text.Trim().Length == 0 ? "0" : txtQtyPCS.Text);
                    packSize = Convert.ToDecimal(rw.Cells[13].Text);

                    Qty = Convert.ToDecimal(txtQty.Text) + (Convert.ToDecimal(txtQtyPCS.Text) > 0 ? Convert.ToDecimal(txtQtyPCS.Text) / packSize : 0);
                    Rate = Convert.ToDecimal(rw.Cells[15].Text);
                    rw.Cells[14].Text = Convert.ToString(Qty);
                    rw.Cells[16].Text = Convert.ToString(Rate * Qty);
                    BasicAmount = (Rate * Qty);
                    rw.Cells[16].Text = Convert.ToString(Rate * Qty);
                    rw.Cells[17].Text = discPer.ToString("0.00");
                    rw.Cells[18].Text = (BasicAmount * discPer).ToString("0.00");
                    rw.Cells[19].Text = BasicAmount.ToString("0.00");
                    if (rw.Cells[20].Text.Trim().Length == 0)
                    {
                        TaxPer = 0;
                    }
                    else
                    {
                        TaxPer = Convert.ToDecimal(rw.Cells[20].Text);
                    }
                    if (rw.Cells[22].Text.Trim().Length == 0)
                    {
                        AddTaxPer = 0;
                    }
                    else
                    {
                        AddTaxPer = Convert.ToDecimal(rw.Cells[22].Text);
                    }
                    TaxAmount = Math.Round((BasicAmount * TaxPer) / 100, 2);
                    AddTaxAmount = Math.Round((BasicAmount * AddTaxPer) / 100, 2);
                    rw.Cells[21].Text = TaxAmount.ToString("0.00");
                    rw.Cells[23].Text = AddTaxAmount.ToString("0.00");
                    rw.Cells[24].Text = (BasicAmount + TaxAmount + AddTaxAmount).ToString("0.00");
                    TotalBasicAmount = TotalBasicAmount + BasicAmount;
                }
            }
            return TotalBasicAmount;
        }

        protected void SchemeDiscCalculation()
        {
            try
            {
                /*@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    Don't change rounding places of any field {decimal number}
                  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ */

                decimal TotalSchemeBasicAmount = 0;
                decimal TotalBasicAmount = 0;
                DataTable dtLineItems;
                decimal DiscPer = 0;

                TotalSchemeBasicAmount = this.SchemeLineCalculation(0);
                dtLineItems = null;
                dtLineItems = (DataTable)Session["LineItem"];
                TotalBasicAmount = dtLineItems.AsEnumerable().Sum(row => row.Field<decimal>("QtyBox") * row.Field<decimal>("Price"));
                TotalBasicAmount = TotalBasicAmount + TotalSchemeBasicAmount;
                DiscPer = Math.Round(TotalSchemeBasicAmount / TotalBasicAmount, 6);
                this.SchemeLineCalculation(DiscPer);

                //#region Update Grid
                //foreach (GridViewRow rw in gvDetails.Rows)
                decimal lineAmount = 0;
                decimal DiscAmount = 0;
                decimal SchemeDiscAmount = 0;
                decimal TaxableAmount = 0;
                decimal TaxPer = 0;
                decimal TaxAmount = 0;
                decimal AddTaxPer = 0;
                decimal AddTaxAmount = 0;

                for (int i = 0; i < dtLineItems.Rows.Count; i++)
                {
                    lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["QtyBox"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Price"].ToString());
                    DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                    dtLineItems.Rows[i]["SchemeDisc"] = DiscPer.ToString("0.000000");
                    SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                    dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["TaxableAmount"] = (lineAmount - DiscAmount - SchemeDiscAmount).ToString("0.00");
                    if (dtLineItems.Rows[i]["Tax_Code"].ToString().Trim().Length == 0)
                    {
                        TaxPer = 0;
                    }
                    else
                    {
                        TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["Tax_Code"]);
                    }
                    if (dtLineItems.Rows[i]["AddTax_Code"].ToString().Trim().Length == 0)
                    {
                        AddTaxPer = 0;
                    }
                    else
                    {
                        AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTax_Code"]);
                    }
                    TaxableAmount = lineAmount - DiscAmount - SchemeDiscAmount;
                    TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                    AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                    dtLineItems.Rows[i]["Tax_Amount"] = TaxAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["AddTax_Amount"] = AddTaxAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["Value"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                }
                dtLineItems.AcceptChanges();
                Session["LineItem"] = dtLineItems;
                gvDetails.DataSource = dtLineItems;
                gvDetails.DataBind();
                //#endregion
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }


        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            int TotalQty = 0;
            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int AvlFreeQty = Convert.ToInt16(row.Cells[8].Text);
            int Slab = Convert.ToInt16(row.Cells[6].Text);
            CheckBox chkBx1 = (CheckBox)row.FindControl("chkSelect");
            TextBox txtQty1 = (TextBox)row.FindControl("txtQty");

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                if (chkBx.Checked == true)
                {
                    //For Box

                    if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                    {
                        TotalQty += Convert.ToInt16(txtQty.Text);
                        if (getBoxPcsPicQty(SchemeCode, Slab, "P") > 0)
                        {
                            txtQty1.Text = "0";
                            chkBx1.Checked = false;
                            txtQty1.ReadOnly = false;
                            string message = "alert('Free Qty should not greater than available free qty !');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            return;
                        }
                    }

                    if (TotalQty > AvlFreeQty)
                    {
                        txtQty1.Text = "0";
                        chkBx1.Checked = false;
                        txtQty1.ReadOnly = false;
                        string message = "alert('Free Qty Box should not greater than available free qty box!');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQty.Text = "0";
                }
            }
            this.SchemeDiscCalculation();
        }

        protected void chkCompReversal_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtOrignalGrid = (DataTable)Session["dtGrid"];
            foreach(DataRow dr in dtOrignalGrid.Rows)
            {
                
                if (chkCompReversal.Checked == true)
                {
                    dr["Balance_Qty"] = Convert.ToDecimal(dr["Balance_Qty"]) + Convert.ToDecimal(dr["Total_ReturnQty"]);
                    if ((Convert.ToDecimal(dr["Balance_Qty"]) - Convert.ToInt32(dr["Balance_Qty"])) >= 0)
                    {
                        dr["Rerturn_Qty_Box"] = Convert.ToInt32(dr["Balance_Qty"]);
                        dr["Rerturn_Qty_Pcs"] = (Convert.ToDecimal(dr["Balance_Qty"]) - Convert.ToInt32(dr["Balance_Qty"])) * Convert.ToDecimal(dr["Pack"]);
                    }
                    else
                    {
                        dr["Rerturn_Qty_Box"] = Convert.ToInt32(dr["Balance_Qty"]) - 1;
                        dr["Rerturn_Qty_Pcs"] = Convert.ToInt32((Convert.ToDecimal(dr["Balance_Qty"]) - (Convert.ToInt32(dr["Balance_Qty"]) - 1)) * Convert.ToDecimal(dr["Pack"]));
                    }
                    dr["Total_ReturnQty"] = Convert.ToDecimal(dr["Balance_Qty"]);
                    if (Convert.ToDecimal(dr["Invoice_Qty"]) == Convert.ToDecimal(dr["Balance_Qty"]))
                    {

                        dr["R_Ltr"] = Convert.ToDecimal(dr["Ltr"]);
                        dr["R_TAX_AMOUNT"] = Convert.ToDecimal(dr["TAX_AMOUNT"]) ;
                        dr["R_LineAMOUNT"] = Convert.ToDecimal(dr["LineAMOUNT"]) ;
                        dr["R_AddTax_Amount"] = Convert.ToDecimal(dr["AddTax_Amount"]);
                        dr["R_Disc_Amount"] = Convert.ToDecimal(dr["Disc_Amount"]);
                        dr["R_Amount"] = Convert.ToDecimal(dr["Amount"]);
                        dr["R_TDValue"] = Convert.ToDecimal(dr["TDValue"]);
                        dr["R_PEValue"] = Convert.ToDecimal(dr["PEValue"]);
                        dr["R_Sec_Disc_Amount"] = Convert.ToDecimal(dr["Sec_Disc_Amount"]);
                        dr["R_SCHEMEDISCVALUE"] = Convert.ToDecimal(dr["SCHEMEDISCVALUE"]);
                        dr["R_TaxableAmount"] = Convert.ToDecimal(dr["TaxableAmount"]);
                        dr["Balance_Qty"] = "0";
                    }
                    else
                    {
                        dr["R_Ltr"] = Convert.ToDecimal(dr["Ltr"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_TAX_AMOUNT"] = Convert.ToDecimal(dr["TAX_AMOUNT"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_LineAMOUNT"] = Convert.ToDecimal(dr["LineAMOUNT"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_AddTax_Amount"] = Convert.ToDecimal(dr["AddTax_Amount"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_Disc_Amount"] = Convert.ToDecimal(dr["Disc_Amount"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_Amount"] = Convert.ToDecimal(dr["Amount"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_TDValue"] = Convert.ToDecimal(dr["TDValue"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_PEValue"] = Convert.ToDecimal(dr["PEValue"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_Sec_Disc_Amount"] = Convert.ToDecimal(dr["Sec_Disc_Amount"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_SCHEMEDISCVALUE"] = Convert.ToDecimal(dr["SCHEMEDISCVALUE"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["R_TaxableAmount"] = Convert.ToDecimal(dr["TaxableAmount"]) / Convert.ToDecimal(dr["Invoice_Qty"]) * Convert.ToDecimal(dr["Balance_Qty"]);
                        dr["Balance_Qty"] = "0";
                    }
                }
                else
                {
                    dr["Balance_Qty"] = Convert.ToDecimal(dr["Balance_Qty"]) + Convert.ToDecimal(dr["Total_ReturnQty"]);
                    dr["Rerturn_Qty_Box"] = "0";
                    dr["Rerturn_Qty_Pcs"] = "0";
                    dr["Total_ReturnQty"] = "0";
                    dr["R_Ltr"] = "0";
                    dr["R_TAX_AMOUNT"] = "0";
                    dr["R_LineAMOUNT"] = "0";
                    dr["R_AddTax_Amount"] = "0";
                    dr["R_Disc_Amount"] = "0";
                    dr["R_Amount"] = "0";
                    dr["R_TDValue"] = "0";
                    dr["R_PEValue"] = "0";
                    dr["R_Sec_Disc_Amount"] = "0";
                    dr["R_SCHEMEDISCVALUE"] = "0";
                    dr["R_TaxableAmount"] = "0";
                }
                dr.AcceptChanges();
            }
            gvDetails.DataSource = dtOrignalGrid;
            gvDetails.DataBind();
            Session["dtGrid"] = dtOrignalGrid;
            
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
            }
        }

        protected Int32 getBoxPcsPicQty(string SchemeCode, int slab, string BoxPcs)
        {
            Int16 TotalQty = 0;
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                if (txtQty.Text.Trim().Length == 0)
                {
                    txtQty.Text = "0";
                }
                if (txtQtyPcs.Text.Trim().Length == 0)
                {
                    txtQtyPcs.Text = "0";
                }
                if (chkBx.Checked == true)
                {
                    if (rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[7].Text) == 0) //Convert.ToInt16(rw.Cells[6].Text) == slab &&
                    {
                        if (BoxPcs == "B")
                        {
                            TotalQty += Convert.ToInt16(txtQty.Text);
                        }
                        else
                        {
                            TotalQty += Convert.ToInt16(txtQtyPcs.Text);
                        }
                    }
                }
            }
            return TotalQty;
        }

        protected void txtQtyPcs_TextChanged(object sender, EventArgs e)
        {
            int TotalQtyPcs = 0;
            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int AvlFreeQtyPcs = Convert.ToInt16(row.Cells[11].Text);
            int Slab = Convert.ToInt16(row.Cells[6].Text);
            CheckBox chkBx1 = (CheckBox)row.FindControl("chkSelect");
            TextBox txtQtyPcs1 = (TextBox)row.FindControl("txtQtyPcs");

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                if (chkBx.Checked == true)
                {
                    //For Pcs                    
                    if (!string.IsNullOrEmpty(txtQtyPcs.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                    {
                        TotalQtyPcs += Convert.ToInt16(txtQtyPcs.Text);
                        if (getBoxPcsPicQty(SchemeCode, Slab, "B") > 0)
                        {
                            txtQtyPcs1.Text = "0";
                            chkBx1.Checked = false;
                            txtQtyPcs1.ReadOnly = false;
                            string message = "alert('Free Qty Pcs should not greater than available free qty pcs !');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            return;
                        }
                    }
                    if (TotalQtyPcs > AvlFreeQtyPcs)
                    {
                        txtQtyPcs1.Text = "0";
                        chkBx1.Checked = false;
                        txtQtyPcs1.ReadOnly = false;
                        string message = "alert('Free Qty Pcs should not greater than available free qty pcs !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQtyPcs.Text = "0";
                }
            }
            this.SchemeDiscCalculation();
        }
    }
}