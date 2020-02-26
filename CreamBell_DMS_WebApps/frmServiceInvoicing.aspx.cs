using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps
{
    public partial class frmServiceInvoicing : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlTransaction transaction;
        string InvoiceNo = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            InvoiceNo = string.Empty;
            txtMonth.Attributes.Add("readonly", "readonly");
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                Session["ServiceLineItem"] = null;
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                Session["SerInvLine"] = null;
                FillSupplierDetails();
                FillCustomers();
                FillProduct();
            }
        }

        protected void rdoSelf_CheckedChanged(object sender, EventArgs e)
        {
            ResetData();
            RadioButton rdo = (RadioButton)sender;
            if (rdo.ID == "rdoSelf")
            {
                #region Self Entry
                lblCustomer.Visible = false;
                drpCustomer.Visible = false;
                #endregion
            }
            else
            {
                #region Customer Entry
                lblCustomer.Visible = true;
                drpCustomer.Visible = true;
                #endregion
            }
        }

        protected void FillCustomers()
        {
            try
            {
                string query = string.Empty;
                ddlCustomers.Items.Clear();
                //query = "select CUSTOMERCODE,CUSTOMERNAME from VW_GetSrvInvoiceSiteCustList WHERE SITEID = '" + Session["SITECODE"].ToString() + "'";
                query = "SELECT CUSTOMERCODE,CUSTOMERNAME FROM VW_GetSrvInvoiceSiteCustList WHERE SITEID = '" + Session["SITECODE"].ToString() + "' UNION " + "SELECT C.[CUSTOMER_CODE], C.[CUSTOMER_CODE] + ' - ' + ISNULL(C.[CUSTOMER_NAME], '') AS Customer FROM AX.ACXCUSTMASTER C JOIN AX.ACX_SDLINKING SD ON SD.SUBDISTRIBUTOR = C.CUSTOMER_CODE WHERE SD.OTHER_SITE = '" + Session["SiteCode"].ToString() + "' AND C.CUST_GROUP != 'CG0001'";
                ddlCustomers.Items.Add("Select...");
                baseObj.BindToDropDown(ddlCustomers, query, "CUSTOMERNAME", "CUSTOMERCODE");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                lblMessage.Visible = true;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillProduct()
        {
            try
            {
                string query = string.Empty;
                ddlProduct.Items.Clear();
                query = "select HSNCODE,HSNCODE + ' - ' + PRODUCTDESCRIPTION AS DESCRIPTION from ax.AcxProductCode where STATUS = 1";
                ddlProduct.Items.Add("Select...");
                baseObj.BindToDropDown(ddlProduct, query, "DESCRIPTION", "HSNCODE");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                lblMessage.Visible = true;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillSupplierDetails()
        {
            try
            {
                ddlCompany.Items.Clear();
                string sqlstr = string.Empty;

                sqlstr = "select SUPPLIERCODE,SUPPLIERCODE + '-' + SUPPLIERNAME as SUPPLIERNAME from VW_GetSupplierList WHERE SITEID = '" + Session["SITECODE"].ToString() + "' or (SITEID = '' and [STATEID]='" + Session["SITELOCATION"] + "')";
                ddlCompany.Items.Add("Select...");
                baseObj.BindToDropDown(ddlCompany, sqlstr, "SUPPLIERNAME", "SUPPLIERCODE");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                lblMessage.Visible = true;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (ddlCompany.SelectedItem.Text != "Select...")
            {
                if (txtAmount.Text != "")
                {
                    if (Convert.ToDecimal(txtAmount.Text) > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = baseObj.GetData("select * from VW_GetSupplierList WHERE (SITEID = '" + Session["SITECODE"].ToString() + "' or SITEID = '') AND SUPPLIERCODE = '" + ddlCompany.SelectedItem.Value.ToString() + "'");

                        if (dt.Rows.Count > 0)
                        {
                            Session["SerInvLine"] = null;
                            DataTable dtNew = new DataTable();
                            string strQuery = "EXEC USP_ACX_GETTAXRATECALCGST_SERVICEINVOICING '" + Session["SITELOCATION"].ToString() + "','" + dt.Rows[0]["STATEID"].ToString() + "','" + Convert.ToDecimal(txtAmount.Text) + "','" + dt.Rows[0]["COMPOSITIONSCHEME"].ToString() + "','" + dt.Rows[0]["GSTINNO"].ToString() + "'";
                            dtNew = baseObj.GetData(strQuery);

                            if (dtNew.Rows.Count > 0)
                            {
                                char sp = '|';
                                string[] retmsg = dtNew.Rows[0]["RETMSG"].ToString().Split(sp);
                                if (retmsg[0].ToUpper() == "TRUE")

                                    if (dtNew.Rows[0]["RETMSG"].ToString().IndexOf("FALSE") >= 0)
                                {
                                    string message = "alert('" + dtNew.Rows[0]["RETMSG"].ToString().Replace("FALSE|", "") + "');";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                                    return;
                                }
                                else
                                {
                                    Session["SerInvLine"] = dtNew;
                                    txtTotTax.Text = Math.Round(Convert.ToDecimal(dtNew.Rows[0]["TOTTAXAMOUNT"].ToString()), 2).ToString();
                                    txtFAmount.Text = Math.Round(Convert.ToDecimal(dtNew.Rows[0]["FINALAMOUNT"].ToString()), 2).ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        string message = "alert('Amount Entered Should be Greater Than Zero...');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        txtAmount.Text = "";
                        txtTotTax.Text = "";
                        txtFAmount.Text = "";
                        return;
                    }
                }
                else
                {
                    txtTotTax.Text = "";
                    txtFAmount.Text = "";
                }
            }
            else
            {
                string message = "alert('Please Select Company Before Entering Amount...');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtAmount.Text = "";
                txtTotTax.Text = "";
                txtFAmount.Text = "";
                return;
            }
        }

        private bool ValidateInput()
        {
            bool b;

            if (ddlCompany.SelectedItem.Text == "Select...")
            {
                b = false;
                lblMessage.Text = "Please Select Company...";
                lblMessage.Visible = true;
            }
            else if (txtMonth.Text == string.Empty)
            {
                b = false;
                lblMessage.Text = "Please Select Month...";
                lblMessage.Visible = true;
            }
            else if (drpCustomer.Visible == true && ddlCustomers.SelectedItem.Text == "Select...")
            {
                b = false;
                lblMessage.Text = "Please Select Customer...";
                lblMessage.Visible = true;
            }
            else if(gvDetails.Rows.Count == 0)
            {
                b = false;
                lblMessage.Text = "Please Add Lines Before Saving...";
                lblMessage.Visible = true;
            }
            else
            {
                b = true;
                lblMessage.Text = string.Empty;
                lblMessage.Visible = false;
            }
            return b;
        }

        private bool ValidateInputBeforeAdd()
        {
            bool b;

            if (ddlCompany.SelectedItem.Text == "Select...")
            {
                b = false;
                lblMessage.Text = "Please Select Company...";
                lblMessage.Visible = true;
            }
            else if (txtMonth.Text == string.Empty)
            {
                b = false;
                lblMessage.Text = "Please Select Month...";
                lblMessage.Visible = true;
            }
            else if (txtAmount.Text == "" || Convert.ToDecimal(txtAmount.Text) == 0)
            {
                b = false;
                lblMessage.Text = "Amount Cannot be Blank or Zero...";
                lblMessage.Visible = true;
            }
            else if (txtFAmount.Text == "" || Convert.ToDecimal(txtFAmount.Text) == 0)
            {
                b = false;
                lblMessage.Text = "Final Amount Cannot be Blank or Zero...";
                lblMessage.Visible = true;
            }
            else if (drpCustomer.Visible == true && ddlCustomers.SelectedItem.Text == "Select...")
            {
                b = false;
                lblMessage.Text = "Please Select Customer...";
                lblMessage.Visible = true;
            }
            else if (ddlProduct.SelectedItem.Text == "Select...")
            {
                b = false;
                lblMessage.Text = "Please Select Product...";
                lblMessage.Visible = true;
            }
            else
            {
                b = true;
                lblMessage.Text = string.Empty;
                lblMessage.Visible = false;
            }
            return b;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    SaveData();
                    string message = "alert('Service Invoice : " + InvoiceNo + " Generated Successfully.!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    uppanel.Update();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }


        private void SaveData()
        {
            string query = string.Empty;
            try
            {
                if (Session["ServiceLineItem"] != null)
                {
                    DataTable dt = new DataTable();
                    DataSet lds = new DataSet();
                    dt = (DataTable)Session["ServiceLineItem"];
                    dt.TableName = "ServiceLineItems";
                    lds.Tables.Add(dt);
                    string ls_xml = lds.GetXml();
                    conn = new SqlConnection(baseObj.GetConnectionString());
                    conn.Open();
                    cmd = new SqlCommand();
                    transaction = conn.BeginTransaction();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 3600;
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.StoredProcedure;
                    query = "USP_INS_SERVICEBILL";
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                    cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                    cmd.Parameters.AddWithValue("@COMPANYCODE", ddlCompany.SelectedItem.Value.ToString());
                    if (drpCustomer.Visible == false)
                    {
                        cmd.Parameters.AddWithValue("@CUSTOMERCODE", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CUSTOMERCODE", ddlCustomers.SelectedItem.Value.ToString());
                    }
                    cmd.Parameters.AddWithValue("@REFRENCEDOCNO", "");
                    cmd.Parameters.AddWithValue("@REFRENCEDATE", Convert.ToDateTime(txtMonth.Text.ToString()));
                    cmd.Parameters.AddWithValue("@REMARK", "");
                    cmd.Parameters.AddWithValue("@USERCODE", Session["USERID"].ToString());
                    cmd.Parameters.AddWithValue("@XmlData", ls_xml);
                    SqlParameter DocumentNo = new SqlParameter();
                    DocumentNo.ParameterName = "@DOCUMENTNO";
                    DocumentNo.SqlDbType = SqlDbType.NVarChar;
                    DocumentNo.Size = 20;
                    DocumentNo.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(DocumentNo);
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    InvoiceNo = cmd.Parameters["@DOCUMENTNO"].Value.ToString();
                    ResetData();
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "► Cannot insert data because session in Empty";
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
            }
        }
        private void ResetData()
        {
            lblMessage.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtTotTax.Text = string.Empty;
            txtFAmount.Text = string.Empty;
            string InvoiceNo = string.Empty;
            FillSupplierDetails();
            FillCustomers();
            FillProduct();
            Session["ServiceLineItem"] = null;
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            Session["SerInvLine"] = null;
        }

        protected void btnSavePrint_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    SaveData();
                    uppanel.Update();
                    //string message = "debugger; alert('Service Invoice: " + InvoiceNo + " Generated Successfully!!');  window.location.href='frmServiceInvoicing.aspx';" +
                    //                 " var printWindow = window.open('frmReport.aspx?ServiceInvoiceNo=" + InvoiceNo + "&Type=ServiceInvoice&docType=PDF&Site=" + Session["SiteCode"].ToString() + "','_newtab');";
                    string message = " alert('Service Invoice: " + InvoiceNo + " Generated Successfully!!');  window.location.href='frmServiceInvoicing.aspx';" +
                                     " var printWindow = window.open('frmReport.aspx?ServiceInvoiceNo=" + InvoiceNo + "&Type=ServiceInvoice&docType=PDF&Site=" + Session["SiteCode"].ToString() + "','_newtab');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            Session["ServiceLineItem"] = null;
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }

       
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtTotTax.Text = string.Empty;
            txtFAmount.Text = string.Empty;
            string InvoiceNo = string.Empty;
            FillCustomers();
            FillProduct();
            Session["ServiceLineItem"] = null;
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            Session["SerInvLine"] = null;
        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            //  HiddenField hiddenField = (HiddenField)gvrow.FindControl("HiddenField2");
            LinkButton lnk = sender as LinkButton;

            if (Session["ServiceLineItem"] != null)
            {
                DataTable dt = Session["LineItem"] as DataTable;
                //  dt.AsEnumerable().Where(r => r.Field<int>("SNO") == Convert.ToInt32(hiddenField.Value)).ToList().ForEach(row => row.Delete());
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                Session["ServiceLineItem"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {

                }
                else
                {
                  
                }
            }

        }
        protected void txtMonth_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInputBeforeAdd();
                DataTable dtDublicate = Session["ServiceLineItem"] as DataTable;
                if (dtDublicate != null)
                {
                    if (dtDublicate.Rows.Count > 0)
                    {
                        if (dtDublicate.Select("ProductCodeName='" + ddlProduct.SelectedValue.ToString() + "'").Length > 0)
                        {
                            string message = "alert('" + ddlProduct.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                            lblMessage.Text = string.Empty;
                            txtAmount.Text = string.Empty;
                            txtTotTax.Text = string.Empty;
                            txtFAmount.Text = string.Empty;
                            Session["SerInvLine"] = null;
                            ScriptManager.RegisterStartupScript((sender as Control), this.GetType(), "alert", message, true);
                            ddlProduct.Focus();
                            return;
                        }
                    }
                }
                if (b)
                {
                    AddDataInGrid();
                    txtAmount.Text = string.Empty;
                    txtTotTax.Text = string.Empty;
                    txtFAmount.Text = string.Empty;
                    uppanel.Update();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void AddDataInGrid()
        {
            DataTable dt = new DataTable();
            dtLineItems = new DataTable();
            if (Session["ServiceLineItem"] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session["ServiceLineItem"];
            }
            if (Session["SerInvLine"] != null)
            {
                dt = (DataTable)Session["SerInvLine"];
            }
            DataRow row;
            row = dtLineItems.NewRow();
            row["ProductCodeName"] = ddlProduct.SelectedItem.Value;
            row["Product_Name"] = ddlProduct.SelectedItem.Text;

            row["Tax_Per"] = dt.Rows[0]["TAXPER"].ToString();
            row["Tax_Amount"] = dt.Rows[0]["TAXAMOUNT"].ToString();
            row["AddTax_Per"] = dt.Rows[0]["ADDTAXPER"].ToString();
            row["AddTax_Amount"] = dt.Rows[0]["ADDTAXAMOUNT"].ToString();
            row["TaxableAmount"] = dt.Rows[0]["AMOUNT"].ToString();
            row["HSNCode"] = ddlProduct.SelectedItem.Value;
            row["TaxComponent"] = dt.Rows[0]["TAX1COMPONENT"].ToString();
            row["AddTaxComponent"] = dt.Rows[0]["TAX2COMPONENT"].ToString();
            row["Amount"] = dt.Rows[0]["FINALAMOUNT"].ToString();
            dtLineItems.Rows.Add(row);

            Session["SerInvLine"] = null;
            Session["ServiceLineItem"] = dtLineItems;
            gvDetails.DataSource = dtLineItems;
            gvDetails.DataBind();
        }

        private void AddColumnInDataTable()
        {
            //dtLineItems = new DataTable();
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            column.ColumnName = "SNO";
            //-----------------------------------------------------------//
            dtLineItems = new DataTable("LineItemTable");
            dtLineItems.Columns.Add(column);
            dtLineItems.Columns.Add("ProductCodeName", typeof(string));
            dtLineItems.Columns.Add("Product_Name", typeof(string));
            //===========Tax==============================
            dtLineItems.Columns.Add("Tax_Per", typeof(decimal));
            dtLineItems.Columns.Add("Tax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Per", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            // New Fields for GST
            dtLineItems.Columns.Add("HSNCode", typeof(string));
            dtLineItems.Columns.Add("TaxComponent", typeof(string));
            dtLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtLineItems.Columns.Add("Amount", typeof(string));
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtTotTax.Text = string.Empty;
            txtFAmount.Text = string.Empty;
            Session["SerInvLine"] = null;
        }
    }
}