using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmServiceInvoiceReversal : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global baseobj = new CreamBell_DMS_WebApps.App_Code.Global();
        string query;
        DataTable dt = new DataTable();
        DataTable dtG = new DataTable();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2;
        SqlTransaction transaction;
        string strQuery = string.Empty;
        //on Page Load 
        protected void Page_Load(object sender, EventArgs e)

        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)

            {
                
                txtInvoiceReturnDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
                BindInvoiceNo();

            }

        }

        public void BindInvoiceNo()
        {

            drpInvNo.Items.Add("Select...");
            string query1 = "EXEC USP_GETSERVICEREVERSALINVOICE '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "'";
            baseobj.BindToDropDown(drpInvNo, query1, "INVOICENO", "INVOICENO");
        }

        //on Button Save insert data in Table
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (drpInvNo.SelectedItem.Value == "Select...")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Please Select Invoice Number');", true); return;
            }
            else
            {
                try
                {
                    DataTable reversal = ViewState["UpdatedReversalLine"] as DataTable;
                    if (reversal == null)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('No Line Amount is changed ');", true); return;
                    }
                    else
                    {
                        reversal.TableName = "ServiceLineItems";
                        string result = string.Empty;
                        using (StringWriter sw = new StringWriter())
                        {
                            reversal.WriteXml(sw);
                            result = sw.ToString();
                        }
                        conn = obj.GetConnection();

                        cmd = new SqlCommand("USP_INS_REVSERVICEBILL");
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@REFRENCEDOCNO", drpInvNo.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@REMARK", txtRemarks.Text);
                        cmd.Parameters.AddWithValue("@USERCODE", Session["USERID"].ToString());
                        cmd.Parameters.AddWithValue("@XmlData", result);
                        cmd.Parameters.AddWithValue("@DOCUMENTNO", "");
                        DataTable _dt = new DataTable();
                        _dt.Load(cmd.ExecuteReader());
                        Object _documentNum = _dt.Rows[0][0];
                        string _documentNumber = _documentNum.ToString();
                        conn.Dispose();
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + "Document Number:" + " " + _documentNumber + "');", true);

                        drpInvNo.Items.Clear();
                        BindInvoiceNo();

                        clearAll();
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + ex.Message.ToString().Replace("'", "''") + "');", true); return;
                }

            }
        }

        //Populate Data according to INVOICE Number
        public void drpGetInvoiceData(object sender, EventArgs e)

        {
            try
            {

                conn = obj.GetConnection();
                cmd = new SqlCommand("USP_GETSERVICEREVERSALINVOICE");
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Siteid", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@INVOICENO", drpInvNo.SelectedItem.Value);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Dispose();
                Session["INVOICENO"] = drpInvNo.SelectedItem.Value;
                foreach (DataRow dtRow in dt.Rows)
                {
                    dtRow.ToString();

                    txtInvoiceDate.Text = dtRow["INVOICEDATE"].ToString();

                    txtBalanceAmount.Text = dtRow["BALANCEAMOUNT"].ToString();
                    txtAddress.Text = dtRow["BILLTOADDRESS"].ToString();
                    txtState.Text = dtRow["BILLTOSTATE"].ToString();
                }

                SqlConnection conn1 = null;
                conn1 = obj.GetConnection();
                cmd = new SqlCommand("USP_GETSERVICEREVERSALLINE");
                cmd.Connection = conn1;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Siteid", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@INVOICENO", drpInvNo.SelectedItem.Value);
                dtG = new DataTable();
                dtG.Load(cmd.ExecuteReader());
                conn1.Dispose();
                ViewState["ReversalLine"] = dtG;
                if (dtG.Rows.Count > 0)
                {
                    gvDetails.DataSource = dtG;
                    gvDetails.DataBind();
                    
                }
                if(CheckBox1.Checked)
                CheckBox1.Checked = false;

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //On Rate Change Calculate Amount
        protected void onRateChange(object sender, EventArgs e)
         {
            TextBox ddl = (TextBox)sender;
            GridViewRow grv = (GridViewRow)ddl.Parent.Parent;
            int ri = grv.RowIndex;
            DataTable reversal = ViewState["ReversalLine"] as DataTable;
            foreach (DataColumn dtC in reversal.Columns)
            {
                dtC.ReadOnly = false;
                reversal.AcceptChanges();
            }
           
                TextBox _Rate = (TextBox)grv.Cells[7].FindControl("RATE");
                decimal rate = Convert.ToDecimal(_Rate.Text);
                Label _Amount = (Label)grv.Cells[12].FindControl("Amount");
                decimal amount = Convert.ToDecimal(_Amount.Text);
                Label _BalanceAmount = (Label)grv.Cells[13].FindControl("Balance_Amount");
                decimal balanceamount = Convert.ToDecimal(_BalanceAmount.Text);
                Label Tax1 = (Label)grv.Cells[8].FindControl("TAX1");
                decimal tax1 = Convert.ToDecimal(Tax1.Text);
                Label Tax2 = (Label)grv.Cells[11].FindControl("TAX2");
                decimal tax2 = Convert.ToDecimal(Tax2.Text);
                decimal _tax1Amount = ((rate * tax1) / 100);
                decimal _tax2Amount = ((rate * tax2) / 100);
                Label Tax1Amount = (Label)grv.Cells[9].FindControl("Tax1_AMT");
                Tax1Amount.Text = Math.Round(_tax1Amount,2).ToString();
                Label Tax2Amount = (Label)grv.Cells[12].FindControl("Tax2_AMT");
                Tax2Amount.Text = Math.Round(_tax2Amount,2).ToString();
                decimal _amount = rate + _tax1Amount + _tax2Amount;
                if (balanceamount <= _amount)
                {
                decimal default_value = 0;
                _Rate.Text = default_value.ToString();
                _Amount.Text = default_value.ToString();
                Tax1Amount.Text = default_value.ToString();
                Tax2Amount.Text = default_value.ToString();
                _Rate.Focus();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Calculated Amount should be less then Balance Amount');", true); return;
                }
                else
                    {
                        if (rate >= balanceamount)
                            {
                            decimal default_value = 0;
                            _Rate.Text = default_value.ToString();
                            _Amount.Text = default_value.ToString();
                            Tax1Amount.Text = default_value.ToString();
                            Tax2Amount.Text = default_value.ToString();
                            _Rate.Focus();
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Line Amount should be less then Balance Amount');", true); return;
                        }
                        else {
                          _Amount.Text = Math.Round(_amount, 2).ToString();
                        }
                    }
                    reversal.Rows[ri].SetField("TAX1AMT", _tax1Amount);
                    reversal.Rows[ri].SetField("TAX2AMT", _tax2Amount);
                    reversal.Rows[ri].SetField("AMOUNT", _amount);
                    reversal.Rows[ri].SetField("RATE", rate);
                    DataTable d = reversal;             
                    reversal.AcceptChanges();
                    ViewState["UpdatedReversalLine"] = reversal;
        }

        //Clear all fields after saving
        protected void clearAll()
        {
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            txtAddress.Text = "";
            txtBalanceAmount.Text = "";
            txtInvoiceDate.Text = "";
            txtState.Text = "";
            txtRemarks.Text = "";
            drpInvNo.SelectedIndex = 0;
            if (CheckBox1.Checked)
             CheckBox1.Checked = false;
        }

        // On Complete Reversal
        protected void onCompleteReversal(object sender, EventArgs e)
        {
            int i = 0;
            if (drpInvNo.SelectedItem.Value == "Select...") {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Please Select Invoice Number');", true); return;

            }
            else
            {
                if (CheckBox1.Checked == true)
                {
                    DataTable reversal = ViewState["ReversalLine"] as DataTable;
                    foreach (DataColumn dtC in reversal.Columns)
                    {
                        dtC.ReadOnly = false;
                        reversal.AcceptChanges();
                    }
                    foreach (GridViewRow grv in gvDetails.Rows)
                    {
                        TextBox _Rate = (TextBox)grv.Cells[7].FindControl("RATE");
                        _Rate.ReadOnly = true;
                        Label _Amount = (Label)grv.Cells[12].FindControl("Amount");
                        decimal amount = Convert.ToDecimal(_Amount.Text);
                        Label _BalanceAmount = (Label)grv.Cells[13].FindControl("Balance_Amount");
                        decimal balanceamount = Convert.ToDecimal(_BalanceAmount.Text);
                        Label Tax1 = (Label)grv.Cells[8].FindControl("TAX1");
                        decimal tax1 = Convert.ToDecimal(Tax1.Text);
                        Label Tax2 = (Label)grv.Cells[11].FindControl("TAX2");
                        decimal tax2 = Convert.ToDecimal(Tax2.Text);
                        decimal rate;
                        if (tax1 + tax2>0)
                        {
                            rate = Math.Round( (balanceamount * (100 / (100+tax1 + tax2))), 2);
                        }
                        else
                        {
                            rate =balanceamount ;

                        }

                        decimal _tax1AMT = (rate * (tax1/100));
                        decimal _tax2AMT = (rate * (tax2/100));

                        decimal _amt = rate + _tax1AMT + _tax2AMT;
                        Label Tax1Amount = (Label)grv.Cells[9].FindControl("Tax1_AMT");
                        Tax1Amount.Text = Math.Round(_tax1AMT, 2).ToString();
                        Label Tax2Amount = (Label)grv.Cells[12].FindControl("Tax2_AMT");
                        Tax2Amount.Text = Math.Round(_tax2AMT, 2).ToString();

                        _Rate.Text = Convert.ToDecimal(rate).ToString();

                        if (_amt <= balanceamount)
                        {
                            _Amount.Text = Math.Round(_amt, 2).ToString();

                        }
                        reversal.Rows[i].SetField("TAX1AMT", _tax1AMT);
                        reversal.Rows[i].SetField("TAX2AMT", _tax2AMT);
                        reversal.Rows[i].SetField("AMOUNT", _amt);
                        reversal.Rows[i].SetField("RATE", rate);
                        i = i + 1;
                    }
                    reversal.AcceptChanges();

                    ViewState["UpdatedReversalLine"] = reversal;

                }
                else
                {
                    foreach (GridViewRow grv in gvDetails.Rows)
                    {
                        TextBox _Rate = (TextBox)grv.Cells[7].FindControl("RATE");
                        Label _Amount = (Label)grv.Cells[12].FindControl("Amount");
                        Label Tax1Amount = (Label)grv.Cells[9].FindControl("Tax1_AMT");
                        Label Tax2Amount = (Label)grv.Cells[12].FindControl("Tax2_AMT");
                        
                        _Rate.Text = "0.00";
                        _Amount.Text = "0.00";
                        Tax1Amount.Text ="0.00";
                        Tax2Amount.Text = "0.00";
                        _Rate.ReadOnly = false;
                    }

                }
            }

        }
    }
}