using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using System.Globalization;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmVenderPayment : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        public DataTable dt;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string query = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillPlant();
                txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
                txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);
                txtPaymentDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();               
            }
        }

        public void fillPlant()
        {
            //query = "select CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTCODE] ELSE A.[ACXPLANTNAME] END [ACXPLANTCODE], CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTNAME] ELSE (SELECT TOP 1 NAME FROM AX.INVENTSITE WHERE SITEID=A.ACXPLANTNAME) END [ACXPLANTNAME] from ax.inventsite A Where A.SITEID='" + Session["SITECODE"].ToString() + "' union select SITEID,NAME from AX.INVENTSITE A Where A.SITEID IN (SELECT DISTINCT OTHER_SITE FROM AX.ACX_SDLinking WHERE SubDistributor='" + Session["SITECODE"].ToString() + "' AND BLOCKED=0)";
            //drpPlant.Items.Clear();
            //drpPlant.Items.Add("--All--");
            //baseObj.BindToDropDown(drpPlant, query, "ACXPLANTNAME", "ACXPLANTCODE");

            query = "select distinct isnull(supplier_code,'7200') as suppliercode,isnull(supplier_code,'7200') + '-' +  isnull(IVS.NAME,'Devyani Food Industries Limited') AS suppliername from [ax].[ACXPURCHINVRECIEPTHEADER] PH left join [ax].[INVENTSITE] IVS on IVS.siteid=PH.supplier_code WHERE PH.SITE_CODE='" + Session["SITECODE"].ToString() + "' ";
            drpPlant.Items.Clear();
            drpPlant.Items.Add("--All--");
            baseObj.BindToDropDown(drpPlant, query, "suppliername", "suppliercode");
        }

        protected void drpPlant_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void tnShow_Click(object sender, EventArgs e)
        {
            BindGridview();
            chkboxSelectAll.Checked = false;
            uppanel.Update();
        }

        protected void BindGridview()
        {
            try
            {
                //string condition = string.Empty;
              
                //if (drpPlant.SelectedItem.Value != "--All--")
                //{
                //    condition = " Where A.[ACXPLANTCODE]='" + drpPlant.SelectedItem.Value + "'";
                //}
                //else
                //{

                //}
                //query = "Select ACXPLANTCODE,ACXPLANTCODE+'-'+ACXPLANTNAME as PlantName from (" +
                //    " Select CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTCODE] ELSE A.[ACXPLANTNAME] END [ACXPLANTCODE],  " +
                //    " CASE WHEN LEN(A.[ACXPLANTCODE])>0 THEN A.[ACXPLANTNAME]  " +
                //            " ELSE (SELECT TOP 1 NAME FROM AX.INVENTSITE WHERE SITEID=A.ACXPLANTNAME) END [ACXPLANTNAME] " +
                //    " From ax.inventsite A Where A.SITEID='" + Session["SITECODE"].ToString() + "' " +
                //    " Union " +
                //    " Select SITEID,NAME " +
                //    " From AX.INVENTSITE A " +
                //    " Where A.SITEID IN (SELECT DISTINCT OTHER_SITE FROM AX.ACX_SDLinking WHERE SubDistributor='" + Session["SITECODE"].ToString() + "' AND BLOCKED=0) ) A " + condition;

                string query = string.Empty;

                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                
                if (drpPlant.SelectedItem.Text != "--All--")
                {
                    query = "acx_getpendingpaymententry " + "'" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "'," + "'" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'," + "'" + drpPlant.SelectedItem.Value + "','" + Session["SITECODE"] + "'";
                    //cmd.Parameters.AddWithValue("@SupplierCode", drpPlant.SelectedItem.Value);
                }
                else
                {
                    query = "acx_getpendingpaymententry " + "'" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "'," + "'" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'," + "'','" + Session["SITECODE"] + "'";
                    //cmd.Parameters.AddWithValue("@SupplierCode", "");
                }
                cmd.CommandText = query;
                dt = new DataTable();
                dt = baseObj.GetData(query);


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
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                Console.Write(ex.Message);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validation();

                if (b == true)
                {
                    SaveDetails();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool Validation()
        {
            bool b = true;
            LblMessage.Text = string.Empty;
            LblMessage.Visible = false;

            if (drpPlant.Text == "--Select--" || drpPlant.Text == "")
            {
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer Group !');", true);
                LblMessage.Text = "Select Plant Name ";
                LblMessage.Visible = true;
                uppanel.Update();
                drpPlant.Focus();
                b = false;
                return b;
            }

            if (txtPaymentDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Payment Date cannot be left blank !";
                LblMessage.Visible = true;
                uppanel.Update();
                return b;
            }
            //==========For Grivew Detail===============
            dt = new DataTable();
            foreach (GridViewRow gv in gvDetails.Rows)
            {
                DropDownList InstrumentCode = (DropDownList)gv.Cells[4].FindControl("drpInstrument");
                TextBox InstrumentNo = (TextBox)gv.Cells[5].FindControl("txtInstrument");
                DropDownList RefDocument_No = (DropDownList)gv.Cells[6].FindControl("drpRefDocument");
                Label RefDocument_Date = (Label)gv.Cells[7].FindControl("lblRefDocDate");
                Label PendingAmount = (Label)gv.Cells[8].FindControl("lblPendingAmount");
                TextBox Amount = (TextBox)gv.Cells[9].FindControl("txtAmount");

                decimal amount;
                if (Amount.Text == string.Empty)
                {
                    amount = 0;
                }
                else
                {
                    amount = Convert.ToDecimal(Amount.Text);
                }


                if (amount > 0)
                {
                    if (InstrumentCode.SelectedIndex == 0)
                    {
                        LblMessage.Text = "Select Instrument !";
                        LblMessage.Visible = true;
                        uppanel.Update();
                        InstrumentCode.Focus();
                        b = false;
                        return b;
                    }
                    else
                    {
                        b = true;
                    }


                    //if (RefDocument_No.Text == "--Select--")
                    //{
                    //    LblMessage.Text = "'Please Select RefDocument_No !";
                    //    LblMessage.Visible = true;
                    //    uppanel.Update();
                    //    RefDocument_No.Focus();
                    //    b = false;
                    //    return b;
                    //}
                }
                else
                { 
                    b = false; 
                }
                if(b==true)
                {
                    return b;
                }
            }
            return b;

        }

        public void SaveDetails()
        {
            try
            {

                dt = new DataTable();

                //============Generate Doc Number=============
                #region  getnTUMBER
                string DocumentNo = string.Empty;
                string query = "SELECT ISNULL(MAX(CAST(RIGHT([Document_No],6) AS INT)),0)+1 FROM [ax].[ACXPAYMENTENTRY] where SITECODE='" + Session["SiteCode"].ToString() + "'";
                conn = obj.GetConnection();
                cmd1 = new SqlCommand(query);

                transaction = conn.BeginTransaction();
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.Text;
                object vc = cmd1.ExecuteScalar();
                int NUMSEQ = (int)vc;
                DocumentNo = "PY" + "-" + ((int)vc).ToString("000000");

                #endregion


                conn = baseObj.GetConnection();
                cmd = new SqlCommand("ACX_PaymentEntry");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd1 = new SqlCommand("ACX_PaymentEntry");
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;


                //======Save PaymentEntry===================

                decimal pendingAmount = 0;
                string ReferenceDocDate, ReferenceDocNo;
                int i = 0;
                int count = 0;
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    i = i + 1;

                    HiddenField Suppliercode = (HiddenField)gv.Cells[0].FindControl("HiddenField2");
                    DropDownList InstrumentCode = (DropDownList)gv.Cells[2].FindControl("drpInstrument");
                    TextBox InstrumentNo = (TextBox)gv.Cells[3].FindControl("txtInstrument");
                    //DropDownList RefDocument_No = (DropDownList)gv.Cells[5].FindControl("drpRefDocument");
                    //Label RefDocument_Date = (Label)gv.Cells[6].FindControl("lblRefDocDate");
                    //Label PendingAmount = (Label)gv.Cells[7].FindControl("lblPendingAmount");
                    TextBox Amount = (TextBox)gv.Cells[8].FindControl("txtAmount");
                    TextBox Remark = gv.FindControl("txtRemark") as TextBox;

                    string PendingAmount = (gv.Cells[9].Text);

                    ReferenceDocDate = (gv.Cells[7].Text);


                    ReferenceDocNo = (gv.Cells[6].Text);

                    decimal amount;
                    if (Amount.Text == string.Empty)
                    {
                        amount = 0;
                    }
                    else
                    {
                        amount = Convert.ToDecimal(Amount.Text);
                    }
                    if (PendingAmount == string.Empty)
                    {
                        pendingAmount = 0;
                    }
                    else
                    {
                        pendingAmount = Convert.ToDecimal(PendingAmount);
                    }


                    if (amount > 0 && InstrumentCode.SelectedItem.Text != "--Select--")
                    {
                        string sitecode = Session["SiteCode"].ToString();
                        string DATAAREAID = Session["DATAAREAID"].ToString();
                        string Plant_Code = Suppliercode.Value.ToString();
                        count = count + 1;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@Plant_Code", Suppliercode.Value);
                        cmd.Parameters.AddWithValue("@Document_No", DocumentNo);
                        cmd.Parameters.AddWithValue("@Instrument_Code", InstrumentCode.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@Instrument_No", InstrumentNo.Text);
                        cmd.Parameters.AddWithValue("@Ref_DocumentNo", ReferenceDocNo.ToString());
                        cmd.Parameters.AddWithValue("@Ref_DocumentDate", ReferenceDocDate.ToString());
                        cmd.Parameters.AddWithValue("@Payment_Amount", amount);
                        cmd.Parameters.AddWithValue("@Payment_Date", txtPaymentDate.Text);
                        cmd.Parameters.AddWithValue("@Status", "INSERT");
                        cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                        cmd.Parameters.AddWithValue("@Remark", Remark.Text.Trim());

                        cmd.ExecuteNonQuery();

                        //===============Update Payment Ledger/Transaction Table===============
                        //cmd1.Parameters.Clear();
                        //string sitecode = Session["SiteCode"].ToString();
                        //double amt = Convert.ToDouble(pendingAmount-amount);
                        //cmd1.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                        //cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        //cmd1.Parameters.AddWithValue("@Document_No", ReferenceDocNo.ToString());
                        //cmd1.Parameters.AddWithValue("@RemainingAmount", pendingAmount - amount);
                        //cmd1.Parameters.AddWithValue("@Status", "UPDATE");
                        //cmd1.Parameters.AddWithValue("@Remark", Remark.Text.Trim());
                        //
                        //cmd1.ExecuteNonQuery();

                    }
                    else
                    {

                    }


                }
                //============Commit All Data================

                transaction.Commit();
                if (count > 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Payment Entry Document No : " + DocumentNo + " Generated Successfully.!');", true);
                }

                BindGridview();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LblMessage.Text = "'Please Try Again!! !";
                LblMessage.Visible = true;
                uppanel.Update();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
            }
          }

        protected void drpRefDocument_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                DropDownList RefDoc = (DropDownList)sender;
                GridViewRow row = (GridViewRow)RefDoc.Parent.Parent;
                HiddenField customercode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
                Label RefDate = (Label)row.Cells[0].FindControl("lblRefDocDate");
                Label PendingAmount = (Label)row.Cells[0].FindControl("lblPendingAmount");
                string strDocumentNo = RefDoc.SelectedItem.Text.ToString();
                if (strDocumentNo != "--Select--")
                {
                    //query = "select [Document_No],CONVERT(nvarchar(20), [Document_Date],106) as [Document_Date], coalesce(cast([RemainingAmount] as decimal(10,2)),0) as [RemainingAmount]  from ax.ACXVENDORTRANS" +
                    //     " where  Document_No='" + strDocumentNo + "' and [SiteCode]='" + Session["SiteCode"].ToString() + "'  ";
                    query = @"SELECT T.[Document_No],CONVERT(nvarchar(20), T.[Document_Date],106) as [Document_Date],coalesce(cast((ISNULL(TR.RemainingAmount,0)+T.RemainingAmount)as decimal(10,2)),0) as [RemainingAmount] FROM ax.ACXVENDORTRANS T LEFT JOIN AX.ACXVENDORTRANS TR 
                            ON T.DOCUMENT_NO=TR.REFNO_DOCUMENTNO AND T.SITECODE=TR.SITECODE
                            AND T.Dealer_Code=TR.Dealer_Code AND TR.DOCUMENTTYPE=2 AND T.DOCUMENTTYPE=1
                            WHERE T.DOCUMENTTYPE=1 AND (ISNULL(TR.RemainingAmount,0)+T.RemainingAmount)>0
	                        AND T.DOCUMENT_NO='" + strDocumentNo + "' AND T.SITECODE='" + Session["SiteCode"].ToString() + "'";

                    dt = baseObj.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        RefDate.Text = dt.Rows[0]["Document_Date"].ToString();
                        PendingAmount.Text = dt.Rows[0]["RemainingAmount"].ToString();
                    }
                }
                else
                {
                    RefDate.Text = string.Empty;
                    PendingAmount.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            TextBox txtAmt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtAmt.Parent.Parent;

            int nRow = row.RowIndex;
            try
            {
                double amount = Convert.ToDouble(txtAmt.Text);
                double PendingAmount = Convert.ToDouble(gvDetails.Rows[nRow].Cells[9].Text);
                //double abc = Convert.ToDouble(gvDetails.Rows[nRow].Cells[8].Text);
                //double abc1 = Convert.ToDouble(gvDetails.Rows[nRow].Cells[9].Text);
                //double abc2 = Convert.ToDouble(gvDetails.Rows[nRow].Cells[11].Text);
                if ((amount > PendingAmount) || (amount < 0))
                {
                    txtAmt.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Entered amount cannot be nagative or greater than Pending amount');", true);
                    //gvDetails.Rows[nRow].Cells[10].Text = "";
                    //ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Entered amount cannot be greater than remaining amount');", true);
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Payment Entry Document No :  Generated Successfully.!');", true); 
                }

              
                else
                {
                    if (nRow + 1 < gvDetails.Rows.Count)
                    {
                        gvDetails.Rows[++nRow].Cells[0].FindControl("txtAmount").Focus();
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            chkboxSelectAll.Checked = false;
        }

        //protected void txtAmount_KeyUp(object sender, EventArgs e)
        //{
        //    TextBox txtAmt = (TextBox)sender;
        //    GridViewRow row = (GridViewRow)txtAmt.Parent.Parent;
        //
        //    int nRow = row.RowIndex;
        //    double amount = Convert.ToDouble(txtAmt.Text);
        //    double PendingAmount = Convert.ToDouble(gvDetails.Rows[nRow].Cells[9].Text);
        //
        //    if (amount > PendingAmount)
        //    {
        //        txtAmt.Text = string.Empty;
        //        //gvDetails.Rows[nRow].Cells[10].Text = "";
        //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Entered amount cannot be greater than remaining amount');", true);
        //        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Payment Entry Document No :  Generated Successfully.!');", true);
        //    }
        //    else
        //    {
        //
        //    }
        //}
        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //============ Fill Data to dropdown of Gridview===================
                    DropDownList ddList = (DropDownList)e.Row.FindControl("drpInstrument");
                    DataTable dt = new DataTable();
                    List<string> ilist = new List<string>();
                    List<string> litem = new List<string>();
                    string query;

                    //==========Instrument

                    query = "Select [Instrument_Code],[Instrument_Description] from ax.ACXINSTRUMENTMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0 order by [Instrument_Description]";

                    dt = baseObj.GetData_New(query, CommandType.Text, ilist, litem);

                    ddList.DataSource = dt;
                    ddList.DataTextField = "Instrument_Description";
                    ddList.DataValueField = "Instrument_Code";
                    ddList.DataBind();
                    ddList.Items.Insert(0, new ListItem("--Select--", "0"));

                    //==========Ref Doc Number
                    //HiddenField plantcode = (HiddenField)e.Row.FindControl("HiddenField2");
                    //DropDownList ddRefNo = (DropDownList)e.Row.FindControl("drpRefDocument");
                    //dt = new DataTable();
                    //
                    ////query = " Select VND.[Document_No] from ax.ACXVENDORTRANS VND WHERE VND.RefNo_DocumentNo is null and VND.[Document_No] not in (select ax.ACXVENDORTRANS.RefNo_DocumentNo from ax.ACXVENDORTRANS where ax.ACXVENDORTRANS.RefNo_DocumentNo is not null)  " +
                    ////        " and VND.RemainingAmount<>0 and VND.[SiteCode]='" + Session["SiteCode"].ToString() + "' and vnd.[Dealer_Code]='" + plantcode.Value + "' " +
                    ////        " order by VND.[Document_No] desc ";
                    //query = @"SELECT T.[Document_No] FROM ax.ACXVENDORTRANS T LEFT JOIN AX.ACXVENDORTRANS TR 
                    //        ON T.DOCUMENT_NO=TR.REFNO_DOCUMENTNO AND T.SITECODE=TR.SITECODE
                    //        AND T.Dealer_Code=TR.Dealer_Code AND TR.DOCUMENTTYPE=2 AND T.DOCUMENTTYPE=1
                    //        WHERE T.DOCUMENTTYPE=1 AND (ISNULL(TR.RemainingAmount,0)+T.RemainingAmount)>0
	                //        AND T.SITECODE='" + Session["SiteCode"].ToString() + "' AND T.Dealer_Code='" + plantcode.Value + "' order by t.Document_No desc";
                    //
                    //dt = baseObj.GetData_New(query, CommandType.Text, ilist, litem);
                    //
                    //
                    //if (dt.Rows.Count > 0)
                    //{
                    //    ddRefNo.DataSource = dt;
                    //    ddRefNo.DataTextField = "Document_No";
                    //    ddRefNo.DataValueField = "Document_No";
                    //    ddRefNo.DataBind();
                    //}
                    //ddRefNo.Items.Insert(0, new ListItem("--Select--", "0"));

                }
            }
            catch (Exception ex) {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + ex.Message.ToString().Replace("'", "") + "');", true);
            }

        }

        protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            
            if (chkboxSelectAll.Checked)
            {
                foreach (GridViewRow row in gvDetails.Rows)
                {
                    string PendingAmount = (row.Cells[9].Text);
                    PendingAmount = PendingAmount == "" ? "0" : PendingAmount;
                    double pendingamt = Convert.ToDouble(PendingAmount);
                    TextBox txtAmount = (TextBox)row.Cells[10].FindControl("txtAmount");
                    txtAmount.Text = pendingamt.ToString("0.00");
                }
            }
            else
            {
                foreach (GridViewRow row in gvDetails.Rows)
                {
                    string PendingAmount = (row.Cells[9].Text);
                    PendingAmount = PendingAmount == "" ? "0" : PendingAmount;
                    double pendingamt = Convert.ToDouble(PendingAmount);
                    TextBox txtAmount = (TextBox)row.Cells[10].FindControl("txtAmount");
                    txtAmount.Text = "";
                }
            }
        }
    }
}