using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmInvoiceGeneration : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        int intApplicable;
        static string SessionGrid = "InvGenDataGrid";
        static string SessionSchGrid = "InvGenSchGrid";
        static string SessionProductInfo = "ProductInfo";
        static Boolean BindingGrid = false;
        string strmessage;
        SqlConnection conn = null;
        SqlCommand cmd, cmd2;
        SqlTransaction transaction;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            int gofocus = Convert.ToInt32(Session["btngofocus"]);
            int plusfocus = Convert.ToInt32(Session["plusfocus"]);
            int tdfocus = Convert.ToInt32(Session["tdfocus"]);
            if (plusfocus == 2)
            {
                BtnAddItem.Focus();
            }
            if (gofocus == 1)
            {
                btnGO.Focus();
            }
            if (tdfocus == 3)
            {
                btnApply.Focus();
            }
            Session["plusfocus"] = null;
            Session["btngofocus"] = null;
            Session["tdfocus"] = null;
            lblMessage.Text = "";
            if (!IsPostBack)
            {
                try
                {
                    BindingGrid = true;
                    #region SO Load
                    btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
                    Session[SessionSchGrid] = null;
                    Session[SessionGrid] = null;
                    string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                    ddlBusinessUnit.Items.Add("All...");
                    obj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
                    ProductGroup();
                    FillProductCode();
                    if (Session["SO_NOList"] != null)
                    {
                        string sono = Session["SO_NOList"].ToString();
                        if (Session["SiteCode"] != null)
                        {
                            string siteid = Session["SiteCode"].ToString();
                            if (lblsite.Text == "")
                            {
                                lblsite.Text = siteid;
                            }
                            //==================For Warehouse Location==============
                            string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                            DataTable dt = new DataTable();
                            dt = obj.GetData(query1);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["MainWarehouse"].ToString().Trim().Length > 0)
                                { Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString(); }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('Warehouse setup not defined.')", true);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('Warehouse setup not defined.')", true);
                                return;
                            }
                        }
                        else
                        {
                            Response.Redirect("Login.aspx");
                            return;
                        }

                        if (sono != "")
                        {
                            ShowData(sono);
                        }
                        txtInvoiceDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);
                        #region // For Scheme Seletion
                        //DataTable dtApplicable = obj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                        //if (dtApplicable.Rows.Count > 0)
                        //{ intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString()); }
                        //Session["intApplicable"] = intApplicable;
                        //if (intApplicable == 1 || intApplicable == 3)
                        //{
                            this.BindSchemeGrid();
                        //}

                        string strQuery = @"Select SchemeCode,Product_Code as ItemCode,cast(BOXQty as decimal(10,2)) as Qty,cast(PCSQty as decimal(10,2)) as QtyPcs,Slab from [ax].[ACXSALESLINE] 
                                    Where SO_NO in ('" + sono + "') and SchemeCode<>'' and siteid='" + Session["SiteCode"].ToString() + "' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                        DataTable dtScheme = obj.GetData(strQuery);

                        if (dtScheme.Rows.Count > 0)
                        {
                            for (int i = 0; i <= dtScheme.Rows.Count - 1; i++)
                            {
                                GetSelectedShemeItemChecked(dtScheme.Rows[i]["SchemeCode"].ToString(), dtScheme.Rows[i]["ItemCode"].ToString(), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["Qty"].ToString())), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["Slab"].ToString())), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["QtyPcs"].ToString())));
                            }
                        }
                        else
                        {
                            #region %scheme selection
                            strQuery = @"SELECT REFSCHEMECODE,ISNULL(SCHEMETYPE,-1) SCHEMETYPE,SCHREFRECID FROM [AX].[ACXSALESHEADER] WHERE  REFSCHEMECODE<>'' AND 
                                    SO_NO in ('" + sono + "') AND SITEID='" + Session["SiteCode"].ToString() + "'  and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                            dtScheme = obj.GetData(strQuery);
                            if (dtScheme.Rows.Count > 0)
                            {
                                for (int i = 0; i <= dtScheme.Rows.Count - 1; i++)
                                {
                                    foreach (GridViewRow rw in gvScheme.Rows)
                                    {
                                        HiddenField hdnSchemeLine = (HiddenField)rw.FindControl("hdnSchemeLine");
                                        HiddenField hdnSchemeType = (HiddenField)rw.FindControl("hdnSchemeType");
                                        if (rw.Cells[1].Text == dtScheme.Rows[i]["REFSCHEMECODE"].ToString() && Convert.ToInt16(hdnSchemeType.Value) == Convert.ToInt16(dtScheme.Rows[i]["SCHEMETYPE"].ToString()) && hdnSchemeLine.Value.ToString() == dtScheme.Rows[i]["SCHREFRECID"].ToString())
                                        {
                                            CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                                            chkBx.Checked = true;
                                            chkSelect_CheckedChanged(chkBx, new EventArgs());
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                        //if (intApplicable == 1 || intApplicable == 3)
                        //{
                            BindingGrid = true;
                            this.SchemeDiscCalculation();
                        //}
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
                }
            }

        }
        public void GetSelectedShemeItemChecked(string SchemeCode, string FreeitemCode, Int16 Qty, Int16 Slab, Int16 QtyPcs)
        {
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                if (rw.Cells[1].Text == SchemeCode && rw.Cells[4].Text == FreeitemCode && Convert.ToInt16(Convert.ToDouble(rw.Cells[6].Text)) == Slab)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    chkBx.Checked = true;
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = Convert.ToString(Qty);
                    txtQtyPcs.Text = Convert.ToString(QtyPcs);
                    chkSelect_CheckedChanged(chkBx, new EventArgs());
                    if (chkBx.Checked == false)
                    {
                        txtQty.Text = "0";
                        txtQtyPcs.Text = "0";
                    }
                    //chkSelect_CheckedChanged(this, new EventArgs());
                }
            }
        }
        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            GridViewRow row1 = (GridViewRow)activeCheckBox.Parent.Parent;
            String SchemeCode1 = row1.Cells[1].Text;
            String SetNo1 = row1.Cells[7].Text;
            HiddenField hdnSchType = (HiddenField)row1.FindControl("hdnSchemeType");
            HiddenField hdnSchSrlNo = (HiddenField)row1.FindControl("hdnSchSrlNo");
            if (App_Code.Global.GetPrevSelection(SchemeCode1, SetNo1, ref gvScheme, hdnSchSrlNo.Value.ToString())) { activeCheckBox.Checked = false; return; }
            TextBox txtQty = (TextBox)row1.FindControl("txtQty");
            TextBox txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
            string SchemeCode;
            string SetNo;

            //CheckBox activeCheckBox = sender as CheckBox;
            #region For Selection If checked


            if (activeCheckBox.Checked)
            {
                //GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                //string SchemeCode = row.Cells[1].Text;
                GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;

                SchemeCode = row.Cells[1].Text;
                SetNo = row.Cells[7].Text;

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
                    //        return;
                    //    }
                    //    else if (rw.Cells[1].Text == SchemeCode && SetNo != rw.Cells[7].Text)
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
                    HiddenField hdnSchemeType = (HiddenField)rw.FindControl("hdnSchemetype");
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
                    if (hdnSchemeType.Value.ToString() == "2" || hdnSchemeType.Value.ToString() == "3")
                    {
                        txtQty.ReadOnly = true;
                        txtQtyPcs.ReadOnly = true;
                    }
                }
                #endregion

                #region  checkbox enable for set no 0 only
                if (SetNo1 == "0")
                {
                    if ((hdnSchType.Value.ToString() == "2" || hdnSchType.Value.ToString() == "3"))
                    {

                    }
                    else
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
                }
                #endregion

            }
            else
            {
                #region  For unchecked
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                    SchemeCode = row.Cells[1].Text;
                    SetNo = row.Cells[7].Text;
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
                        txtQtyPcs.ReadOnly = true;
                        txtQty.ReadOnly = true;
                    }
                }
                if (SetNo1 == "0" && (hdnSchType.Value.ToString() == "2" || hdnSchType.Value.ToString() == "3"))
                {
                    txtQty = (TextBox)row1.FindControl("txtQty");
                    txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPcs.Text = "";
                }
                #endregion
            }
            #endregion
            if (activeCheckBox.Checked && (hdnSchType.Value.ToString() == "0" || hdnSchType.Value.ToString() == "1"))
            {
                if (Convert.ToInt32(SetNo1) > 0)
                {
                    foreach (GridViewRow rw in gvScheme.Rows)
                    {
                        if (Convert.ToString(rw.Cells[7].Text) == SetNo1 && rw.Cells[1].Text == SchemeCode1)
                        {
                            txtQty = (TextBox)rw.FindControl("txtQty");
                            txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                            txtQty.Enabled = false;
                            txtQtyPcs.Enabled = false;
                        }
                    }
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
            BindingGrid = true;
            SchemeDiscCalculation();

            GridViewFooterCalculate();
        }
        private void ShowData(string sono)
        {
            DataTable dt = new DataTable();
            string query = "SELECT A.SO_No,CONVERT(VARCHAR(11),A.SO_Date,106) as SO_Date ,A.Loadsheet_No,A.SHIPTOADDRESS," +
                        "(SELECT TOP 1 CONVERT(VARCHAR(11), L.LoadSheet_Date, 106) FROM ax.ACXLOADSHEETHEADER L WHERE L.LoadSheet_No = A.LoadSheet_No and L.Siteid = A.siteid ) as LoadSheet_Date, " +
                       "B.CUST_GROUP,Customer_Group=(Select C.CUSTGROUP_NAME from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=B.CUST_GROUP)," +
                       "B.Customer_Code,concat(B.Customer_Code,'-', B.Customer_Name) as Customer_Name," +
                       "B.Address1,B.Mobile_No,B.VAT,B.GSTINNO,B.GSTREGISTRATIONDATE,B.COMPOSITIONSCHEME,B.STATE  " +
                       "from [ax].[ACXSALESHEADER] A left join ax.Acxcustmaster B on A.Customer_Code=B.Customer_Code " +
                       // "left join ax.ACXLOADSHEETHEADER L on L.LoadSheet_No=A.LoadSheet_No and L.Siteid=A.siteid " +
                       "where A.[Siteid]='" + lblsite.Text + "' and A.SO_NO in (select id from [dbo].[CommaDelimitedToTable]('" + sono + "' ,','))";

            dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                drpCustomerGroup.Items.Insert(0, new ListItem(dt.Rows[0]["Customer_Group"].ToString(), dt.Rows[0]["CUST_GROUP"].ToString()));
                drpCustomerCode.Items.Insert(0, new ListItem(dt.Rows[0]["Customer_Name"].ToString(), dt.Rows[0]["Customer_Code"].ToString()));
                txtTIN.Text = dt.Rows[0]["VAT"].ToString();
                txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                txtMobileNO.Text = dt.Rows[0]["Mobile_No"].ToString();
                txtLoadSheetNumber.Text = dt.Rows[0]["Loadsheet_No"].ToString();
                drpSONumber.Items.Insert(0, new ListItem(dt.Rows[0]["So_NO"].ToString(), dt.Rows[0]["So_NO"].ToString()));
                txtSODate.Text = dt.Rows[0]["SO_Date"].ToString();
                txtLoadsheetDate.Text = dt.Rows[0]["LoadSheet_Date"].ToString();

                /*   ---------- GST Implementation Start ----------- */
                txtGSTtin.Text = dt.Rows[0]["GSTINNO"].ToString();
                txtGSTtinRegistration.Text = dt.Rows[0]["GSTREGISTRATIONDATE"].ToString();
                chkCompositionScheme.Checked = Convert.ToBoolean((Convert.ToString(dt.Rows[0]["COMPOSITIONSCHEME"]) == string.Empty ? 0 : dt.Rows[0]["COMPOSITIONSCHEME"]));
                txtBillToState.Text = dt.Rows[0]["STATE"].ToString();

                String qquery = "Exec [dbo].[usp_GetCustomerOutstandingDetail] '" + txtBillToState.Text + "','" + Session["SITECODE"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "'";

                DataTable dtt = obj.GetData(qquery);
                if (dtt.Rows.Count > 0)
                {
                    txtoutstanding.Text = Convert.ToDecimal(dtt.Rows[0]["OutstandingAmount"].ToString()).ToString("0.00");
                    //lblMessage.Visible = true;
                }
                else
                {
                    txtoutstanding.Text = "0.00";
                    //lblMessage.Visible = false;
                }

                query = "EXEC USP_CUSTSHIPTOADDRESS '" + drpCustomerCode.SelectedValue + "'";
                ddlShipToAddress.Items.Clear();
                obj.BindToDropDown(ddlShipToAddress, query, "SHIPTOADDRESS", "SHIPTOADDRESS");
                if (dt.Rows[0]["SHIPTOADDRESS"].ToString() != "")
                {
                    ddlShipToAddress.SelectedValue = dt.Rows[0]["SHIPTOADDRESS"].ToString();
                }
                DataTable dtProductInfo = new DataTable();
                dtProductInfo = obj.GetData("EXEC USP_GETPRODUCTINFO '" + Session["SCHSTATE"].ToString() + "','" + drpCustomerCode.SelectedValue + "','" + lblsite.Text + "','" + Session["TransLocation"].ToString() + "'");
                Session[SessionProductInfo] = dtProductInfo;
                /*----------------GST Implementation End-------------------------- */
            }
            else
            {
                txtGSTtin.Text = "";
                txtGSTtinRegistration.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
            }

            conn = obj.GetConnection();
            cmd2 = new SqlCommand("AX.ACX_SOTOINVOICECREATION");
            cmd2.Connection = conn;
            cmd2.CommandTimeout = 0;
            cmd2.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@TransLocation", Session["TransLocation"].ToString());
            string s = Session["TransLocation"].ToString();
            cmd2.Parameters.AddWithValue("@siteid", lblsite.Text);
            cmd2.Parameters.AddWithValue("@so_no", sono);
            dt.Load(cmd2.ExecuteReader());

            if (conn != null)
            {
                if (conn.State.ToString() == "Open")
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            Session[SessionGrid] = dt;
            if (dt.Rows.Count > 0)
            { txtHdnTDValue.Text = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD")).ToString(); }
            BindGrid();
            //TDApplicable();
        }
        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label Line_No = (Label)e.Row.FindControl("Line_No");
                Label lblStockQty = (Label)e.Row.FindControl("StockQty");
                TextBox txtInvBox = (TextBox)e.Row.FindControl("txtBox");
                HiddenField hProductCode = (HiddenField)e.Row.FindControl("HiddenField2");
                if (Convert.ToDecimal(lblStockQty.Text) < Convert.ToDecimal(txtInvBox.Text))
                {
                    e.Row.BackColor = System.Drawing.Color.Tomato;
                }

                TextBox PcsQty = (TextBox)e.Row.FindControl("txtPcsQtyGrid");
                DataTable dt = (DataTable)Session[SessionProductInfo];
                DataRow[] dr = dt.Select("ITEMID='" + hProductCode.Value.ToString() + "'");
                if (dr.Length > 0)
                {
                    if (dr[0]["PCSBillingApplicable"].ToString().ToUpper() == "Y")
                    { PcsQty.Enabled = true; }
                    else
                    { PcsQty.Enabled = false; }
                }
            }
        }
        private bool Validation()
        {
            bool returnvalue = true;
            string message = "";
            decimal totalInvoiceValue;
            try
            {
                // Check For Valid Delivery Date
                if (txtAddress.Text.Trim().Length == 0)
                {
                    message = "alert('Customers Bill To Address Required !!!');";
                    returnvalue = false;
                }
                else if (txtBillToState.Text.Trim().Length == 0)
                {
                    message = "alert('Customer Bill To State Required !!!');";
                    returnvalue = false;
                }
                else if (ddlShipToAddress.SelectedValue == null || ddlShipToAddress.SelectedValue.ToString().Trim().Length == 0)
                {
                    message = "alert('Customers Ship To Address Required !!!');";
                    returnvalue = false;
                }
                else if (drpCustomerCode.Text == "")
                {
                    message = "alert('Please Provide Customer Name !!!');";
                    drpCustomerGroup.Focus();
                    returnvalue = false;
                }
                else if (drpCustomerGroup.Text == "")
                {
                    message = "alert('Please Provide Customer Group !');";
                    drpCustomerGroup.Focus();
                    returnvalue = false;
                }
                else if (drpSONumber.Text == "")
                {
                    message = "alert('Please Provide Sale Order Number !');";
                    drpSONumber.Focus();
                    returnvalue = false;
                }
                else if (txtInvoiceDate.Text == "")
                {
                    txtInvoiceDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);
                }
                if (message.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return returnvalue;
                }
                //===============================
                if (Session["SO_NOList"] == null)
                {
                    message = "alert('Invoice genration session expired ,Please select sales order no ...'); window.location.href='frmInvoicePrepration.aspx'; ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
                string sono = Session["SO_NOList"].ToString();
                string query1 = "select * from ax.ACXSALEINVOICEHEADER WHERE [Siteid]='" + lblsite.Text + "' and " +
                    "SO_NO in (select id from [dbo].[CommaDelimitedToTable]('" + sono + "' ,',')) and ISNULL(invoice_no,'')=''";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    message = "alert('Already Created Invoice No: " + txtInvoiceNo.Text + " againts the SO NO: " + drpSONumber.SelectedItem.Text + "!')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                }
                DataTable dtGrid = (DataTable)Session[SessionGrid];
                if (Session[SessionGrid] == null)
                {
                    message = "alert('Invoice genration session expired ,Please select sales order no ...'); window.location.href='frmInvoicePrepration.aspx'; ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
                totalInvoiceValue = Convert.ToDecimal(dtGrid.Compute("sum(Amount)", ""));
                if (totalInvoiceValue <= 0)
                {
                    message = "alert('Invoice value should not be less than zero!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                message = "alert('" + ex.Message.Replace("'", "''") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                returnvalue = false;
                ErrorSignal.FromCurrentContext().Raise(ex);
                return returnvalue;
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            ClearAll();
            Session["SO_NOList"] = null;
            Response.Redirect("~/frmInvoicePrepration.aspx");
        }
        private void BindGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session[SessionGrid];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {

                        DataView dv = dt.DefaultView;
                        if (rdAsc.Checked == true)
                        {
                            dv.Sort = "Line_No " + rdAsc.Text.ToString();
                        }
                        else if (rdDesc.Checked == true)
                        {
                            dv.Sort = "Line_No " + rdDesc.Text.ToString();
                        }
                        Session[SessionGrid] = dv.ToTable();
                        gvDetails.DataSource = dv.ToTable();
                        gvDetails.DataBind();
                        gvDetails.Visible = true;
                        GridViewFooterCalculate();
                    }
                    else
                    {
                        gvDetails.DataSource = dt;
                        gvDetails.DataBind();
                        gvDetails.Visible = false;
                    }
                }
                else
                {
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                    gvDetails.Visible = false;
                }
                //Session[SessionGrid] = dt;
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                //GridViewFooterCalculate();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "''") + "')", true);
            }
        }
        public void ProductGroup()
        {
            string strQuery = string.Empty;
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string query = "USP_DropdownGetProductGroup";
            ilist.Add("@USERCODE"); litem.Add(Session["USERID"].ToString());
            if (ddlBusinessUnit.SelectedItem.Text == "All...")
            {
                ilist.Add("@BU_CODE"); litem.Add("");
            }
            else
            {
                ilist.Add("@BU_CODE"); litem.Add(ddlBusinessUnit.SelectedItem.Value.ToString());
            }
            if (rdStock.Checked == true)
            {
                ilist.Add("@IsStock"); litem.Add("1");
            }
            else
            {
                ilist.Add("@IsStock"); litem.Add("0");
            }
            ilist.Add("@Block"); litem.Add("1");
            DataTable dtBinddropdown = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);

            DDLProductGroup.DataSource = dtBinddropdown;
            DDLProductGroup.DataTextField = "CODE";
            DDLProductGroup.DataValueField = "CODE_DESC";
            DDLProductGroup.DataBind();
            DDLProductGroup.Items.Insert(0, new ListItem("--Select--", ""));
        }
        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = string.Empty;
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string query = "USP_DropdownGetProductSubGroup";
            ilist.Add("@USERCODE"); litem.Add(Session["USERID"].ToString());
            ilist.Add("@IsStock"); litem.Add("0");
            ilist.Add("@Block"); litem.Add("1");
            ilist.Add("@ProductGroup"); litem.Add(DDLProductGroup.SelectedValue.ToString());
            DataTable dtBinddropdown = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
            DDLProductSubCategory.DataSource = dtBinddropdown;
            DDLProductSubCategory.DataTextField = "CODE";
            DDLProductSubCategory.DataValueField = "CODE_DISC";
            DDLProductSubCategory.DataBind();
            DDLProductSubCategory.Items.Insert(0, new ListItem("--Select--", ""));
            FillProductCode();
            DDLProductSubCategory.Focus();
        }
        public void FillProductCode()
        {
            string strQuery = string.Empty;
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            string query = "USP_DropdownGetProductDesc";
            ilist.Add("@USERCODE"); litem.Add(Session["USERID"].ToString());
            if (ddlBusinessUnit.SelectedItem.Text == "All...")
            {
                ilist.Add("@BU_CODE"); litem.Add("");
            }
            else
            {
                ilist.Add("@BU_CODE"); litem.Add(ddlBusinessUnit.SelectedItem.Value.ToString());
            }
            if (rdStock.Checked == true)
            {
                ilist.Add("@IsStock"); litem.Add("1");
            }
            else
            {
                ilist.Add("@IsStock"); litem.Add("0");
            }
            ilist.Add("@Block"); litem.Add("1");
            ilist.Add("@ProductGroup"); litem.Add(DDLProductGroup.SelectedValue.ToString());
            ilist.Add("@ProductSubGroup"); litem.Add(DDLProductSubCategory.SelectedValue.ToString());

            DataTable dtBinddropdown = obj.GetData_New(query, CommandType.StoredProcedure, ilist, litem);
            DDLMaterialCode.DataSource = dtBinddropdown;
            DDLMaterialCode.DataTextField = "CODE_DISC";
            DDLMaterialCode.DataValueField = "CODE";
            DDLMaterialCode.DataBind();
            DDLMaterialCode.Items.Insert(0, new ListItem("--Select--", ""));
        }
        protected void txtTransporterName_TextChanged(object sender, EventArgs e)
        {
            txtDriverName.Focus();
        }
        protected void txtDriverName_TextChanged(object sender, EventArgs e)
        {
            txtDriverContact.Focus();
        }
        protected void txtDriverContact_TextChanged(object sender, EventArgs e)
        {
            txtVehicleNo.Focus();
        }
        protected void ddlShipToAddress_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public DataTable GetDatafromSP(string SPName)
        {
            conn = obj.GetConnection();
            try
            {
                string ItemGroup = string.Empty;
                string ItemCode = string.Empty;

                foreach (GridViewRow row in gvDetails.Rows)
                {
                    Label lblItem = (Label)row.Cells[2].FindControl("Product");
                    string[] arritem = lblItem.Text.Split('-');
                    string strItemCode = arritem[0].ToString().Trim();
                    if (row.RowIndex == 0)
                    {
                        ItemCode = "" + strItemCode + "";
                    }
                    else
                    {
                        ItemCode += "," + strItemCode + "";
                    }
                }

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                cmd.Parameters.AddWithValue("@PLACESITE", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@PLACESTATE", Session["SCHSTATE"].ToString());
                cmd.Parameters.AddWithValue("@PLACEALL", "");
                cmd.Parameters.AddWithValue("@CUSCODECUS", drpCustomerCode.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEGROUP", drpCustomerGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEALL", "");
                cmd.Parameters.AddWithValue("@ITEMITEM", ItemCode);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                //cmd.Parameters.AddWithValue("@ITEMGROUP",DDLProductGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@ITEMALL", "");

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    #region Scheme Found
                    dt.Columns.Add("TotalFreeQty", typeof(System.Int16));
                    dt.Columns.Add("TotalFreeQtyPCS", typeof(System.Int16));
                    dt.Columns.Add("TotalSchemeValueoff", typeof(System.Decimal));
                    DataTable dt1 = dt.Clone();
                    dt1.Clear();

                    Int16 TotalQtyofGroupItem = 0;
                    Int16 TotalPCSQtyofGroupItem = 0;
                    Int16 TotalQtyofItem = 0;
                    Int16 TotalPCSQtyofItem = 0;

                    decimal TotalValueofGroupItem = 0;
                    decimal TotalValueofItem = 0;
                    dt.Columns["SCHBOX"].ReadOnly = false;
                    dt.Columns["SCHPCS"].ReadOnly = false;
                    dt.Columns["SCHVALUE"].ReadOnly = false;
                    dt.Columns["MINBOX"].ReadOnly = false;
                    dt.Columns["MINPCS"].ReadOnly = false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["TotalFreeQty"] = Convert.ToInt16("0");
                        dr["TotalFreeQtyPCS"] = Convert.ToInt16("0");
                        dr["TotalSchemeValueoff"] = Convert.ToDecimal("0.0");
                        #region Minimum Qty
                        if (Convert.ToInt16(dr["MINIMUMQUANTITY"]) > 0)
                        {
                            TotalQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString(), "BOX");
                            TotalQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString(), "BOX");
                            dr["SCHBOX"] = TotalQtyofGroupItem + TotalQtyofItem;
                            if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                            {
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                dr["TotalSchemeValueoff"] = Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"])) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]);
                                //dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalQtyofGroupItem + TotalQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
                            }
                        }
                        #endregion
                        #region Minimum PCS
                        if (Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]) > 0)
                        {
                            TotalPCSQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString(), "PCS");
                            TotalPCSQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString(), "PCS");
                            dr["SCHPCS"] = TotalPCSQtyofGroupItem + TotalPCSQtyofItem;
                            if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                            {
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                dr["TotalSchemeValueoff"] = Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"])) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]);
                                // dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalPCSQtyofGroupItem + TotalPCSQtyofItem) / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
                            }
                        }
                        #endregion
                        #region Minimum Value
                        if (Convert.ToInt16(dr["MINIMUMVALUE"]) > 0)
                        {
                            TotalValueofGroupItem = GetValueofGroupItem(dr["Scheme Item group"].ToString());
                            TotalValueofItem = GetValueofItem(dr["Scheme Item group"].ToString());
                            dr["SCHVALUE"] = TotalValueofGroupItem + TotalValueofItem;
                            if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                            {
                                dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                            }
                            if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                            {
                                dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTY"]));
                            }
                            if (dr["Schemetype"].ToString() == "3")
                            {
                                dr["TotalSchemeValueoff"] = Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"])) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]);
                                // dr["TotalSchemeValueoff"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal((TotalValueofGroupItem + TotalValueofItem) / Convert.ToInt16(dr["MINIMUMVALUE"]))) * Convert.ToDecimal(dr["SCHEMEVALUEOFF"]));
                            }
                        }
                        #endregion
                        #region Minimum Purchase Type Check
                        if (Convert.ToInt16(dr["MINIMUMPURCHASEITEMTYPE"]) == 0)
                        {
                            dr["MINBOX"] = "0";
                            dr["MINPCS"] = "0";
                        }
                        if (Convert.ToInt16(dr["MINIMUMPURCHASEBOX"]) > 0 && Convert.ToInt16(dr["MINIMUMPURCHASEITEMTYPE"]) > 0)
                        {
                            TotalQtyofGroupItem = GetQtyofGroupItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "BOX");
                            TotalQtyofItem = GetQtyofItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "BOX");
                            dr["MINBOX"] = TotalQtyofGroupItem + TotalQtyofItem;
                        }
                        if (Convert.ToInt16(dr["MINIMUMPURCHASEPCS"]) > 0 && Convert.ToInt16(dr["MINIMUMPURCHASEITEMTYPE"]) > 0)
                        {
                            TotalPCSQtyofGroupItem = GetQtyofGroupItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "PCS");
                            TotalPCSQtyofItem = GetQtyofItem(dr["MINIMUMPURCHASEITEMGROUP"].ToString(), "PCS");
                            dr["MINPCS"] = TotalPCSQtyofGroupItem + TotalPCSQtyofItem;
                        }
                        #endregion
                        dt.AcceptChanges();
                    }
                    DataRow[] drSchFound = dt.Select("(MINIMUMQUANTITY<=SCHBOX OR MINIMUMVALUE<=SCHVALUE OR MINIMUMQUANTITYPCS<=SCHPCS) AND (MINIMUMPURCHASEBOX<=MINBOX OR MINIMUMPURCHASEPCS<=MINPCS)");
                    if (drSchFound.Length > 0)
                    {
                        dt1 = drSchFound.CopyToDataTable();
                    }
                    else
                    {
                        return null;
                    }
                    //dt1 = dt.Select("(MINIMUMQUANTITY<=SCHBOX OR MINIMUMVALUE<=SCHVALUE OR MINIMUMQUANTITYPCS<=SCHPCS) AND (MINIMUMPURCHASEBOX<=MINBOX OR MINIMUMPURCHASEPCS<=MINPCS)").CopyToDataTable();

                    //dt1 = dt.Select("(MINIMUMPURCHASEBOX<=MINBOX OR MINIMUMPURCHASEPCS<=MINPCS)").CopyToDataTable();
                    DataTable dt3 = dt1.Clone();

                    #region For Qty
                    DataView view = new DataView(dt1);
                    view.RowFilter = "[Scheme Type]=0";
                    DataTable distinctTable = (DataTable)view.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    DataView dvSort = new DataView(distinctTable);
                    dvSort.Sort = "SCHEMECODE ASC, MINIMUMQUANTITY DESC";

                    DataView Dv1 = null;
                    int intCalRemFreeQty = 0;
                    string schemeCode = string.Empty;

                    foreach (DataRowView drv in dvSort)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            intCalRemFreeQty = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }

                        if (intCalRemFreeQty == 0)
                        {
                            // Qty Scheme Filter
                            if (Convert.ToInt16(drv.Row["MINIMUMQUANTITY"]) > 0)
                            {
                                #region Scheme Free Lines BOX Free Qty
                                Dv1 = new DataView(dt1);
                                Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY='" + drv.Row["MINIMUMQUANTITY"] + "' AND FREEQTY>0";

                                foreach (DataRowView drv2 in Dv1)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                                #endregion

                                #region Scheme Free Lines For PCS Free Qty
                                // PCS Qty Scheme Filter
                                Dv1 = new DataView(dt1);
                                Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY='" + drv.Row["MINIMUMQUANTITY"] + "' AND FREEQTYPCS>0";

                                foreach (DataRowView drv2 in Dv1)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                                #endregion
                            }
                            intCalRemFreeQty = +1;
                        }
                    }
                    #endregion

                    #region For PCS Qty

                    view = new DataView(dt1);
                    view.RowFilter = "[Scheme Type]=0";
                    distinctTable = (DataTable)view.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITYPCS");

                    dvSort = new DataView(distinctTable);
                    dvSort.Sort = "SCHEMECODE ASC,MINIMUMQUANTITYPCS DESC";

                    Dv1 = null;
                    intCalRemFreeQty = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSort)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            intCalRemFreeQty = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (intCalRemFreeQty == 0)
                        {
                            #region Scheme Free Lines BOX Free Qty
                            Dv1 = new DataView(dt1);
                            Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS='" + drv.Row["MINIMUMQUANTITYPCS"] + "' AND FREEQTY>0 AND MINIMUMQUANTITYPCS>0";

                            foreach (DataRowView drv2 in Dv1)
                            {
                                if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    dt3.ImportRow(drv2.Row);
                                }
                            }
                            #endregion

                            #region Scheme Free Lines PCS Free Qty
                            Dv1 = new DataView(dt1);
                            Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS='" + drv.Row["MINIMUMQUANTITYPCS"] + "' AND FREEQTYPCS>0 AND MINIMUMQUANTITYPCS>0";

                            foreach (DataRowView drv2 in Dv1)
                            {
                                if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    dt3.ImportRow(drv2.Row);
                                }
                            }
                            #endregion
                        }
                        intCalRemFreeQty += 1;
                    }

                    #endregion
                    #region for Value

                    DataView viewValue = new DataView(dt1);
                    viewValue.RowFilter = "[Scheme Type]=1";
                    DataTable distinctTableValue = (DataTable)viewValue.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortValue = new DataView(distinctTableValue);
                    dvSortValue.Sort = "SCHEMECODE ASC,MINIMUMVALUE DESC";

                    DataView Dv1Value = null;
                    Int16 IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortValue)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE >='" + drv.Row["MINIMUMVALUE"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["FREEQTY"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                                if (Convert.ToInt16(drv2["FREEQTYPCS"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }

                    #endregion
                    #region Scheme Percentage
                    DataView viewScheme = new DataView(dt1);
                    viewScheme.RowFilter = "[Scheme Type]=2";
                    DataTable distinctTableScheme = (DataTable)viewScheme.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortScheme = new DataView(distinctTableScheme);
                    dvSortScheme.Sort = "SCHEMECODE ASC,MINIMUMVALUE DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortScheme)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE >='" + drv.Row["MINIMUMVALUE"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMVALUE"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv2["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewScheme = new DataView(dt1);
                    viewScheme.RowFilter = "[Scheme Type]=2";
                    distinctTableScheme = (DataTable)viewScheme.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    dvSortScheme = new DataView(distinctTableScheme);
                    dvSortScheme.Sort = "SCHEMECODE ASC,MINIMUMQUANTITY DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortScheme)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY >='" + drv.Row["MINIMUMQUANTITY"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITY"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewScheme = new DataView(dt1);
                    viewScheme.RowFilter = "[Scheme Type]=2";
                    distinctTableScheme = (DataTable)viewScheme.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITYPCS");

                    dvSortScheme = new DataView(distinctTableScheme);
                    dvSortScheme.Sort = "SCHEMECODE ASC,MINIMUMQUANTITYPCS DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortScheme)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS >='" + drv.Row["MINIMUMQUANTITYPCS"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]) > 0)
                                {

                                    if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITYPCS"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    #endregion
                    #region Scheme Valueoff
                    DataView viewSchemeValueoff = new DataView(dt1);
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=3";
                    DataTable distinctTableSchemeValueOff = (DataTable)viewSchemeValueoff.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortSchemeValueoff = new DataView(distinctTableSchemeValueOff);
                    dvSortSchemeValueoff.Sort = "SCHEMECODE ASC,MINIMUMVALUE DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortSchemeValueoff)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE >='" + drv.Row["MINIMUMVALUE"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMVALUE"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHVALUE"]) >= Convert.ToInt16(drv2["MINIMUMVALUE"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewSchemeValueoff = new DataView(dt1);
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=3";
                    distinctTableSchemeValueOff = (DataTable)viewSchemeValueoff.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    dvSortSchemeValueoff = new DataView(distinctTableSchemeValueOff);
                    dvSortSchemeValueoff.Sort = "SCHEMECODE ASC,MINIMUMQUANTITY DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortSchemeValueoff)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY >='" + drv.Row["MINIMUMQUANTITY"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITY"]) > 0)
                                {
                                    if (Convert.ToDecimal(drv2["SCHBOX"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITY"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    viewSchemeValueoff = new DataView(dt1);
                    viewSchemeValueoff.RowFilter = "[Scheme Type]=2";
                    distinctTableSchemeValueOff = (DataTable)viewSchemeValueoff.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITYPCS");

                    dvSortSchemeValueoff = new DataView(distinctTableSchemeValueOff);
                    dvSortSchemeValueoff.Sort = "SCHEMECODE ASC,MINIMUMQUANTITYPCS DESC";

                    Dv1Value = null;
                    IntCalRemFreeValue = 0;
                    schemeCode = "";

                    foreach (DataRowView drv in dvSortSchemeValueoff)
                    {
                        if (schemeCode != drv.Row["SCHEMECODE"].ToString())
                        {
                            IntCalRemFreeValue = 0;
                            schemeCode = drv.Row["SCHEMECODE"].ToString();
                        }
                        if (IntCalRemFreeValue == 0)
                        {
                            Dv1Value = new DataView(dt1);
                            Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' AND [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS >='" + drv.Row["MINIMUMQUANTITYPCS"] + "'";

                            foreach (DataRowView drv2 in Dv1Value)
                            {
                                if (Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]) > 0)
                                {

                                    if (Convert.ToDecimal(drv2["SCHPCS"]) >= Convert.ToDecimal(drv2["MINIMUMQUANTITYPCS"]))
                                    {
                                        dt3.ImportRow(drv2.Row);
                                    }
                                }
                            }
                            IntCalRemFreeValue += 1;
                        }
                    }
                    #endregion
                    // Update Slab Qty For FreeQTYPCS
                    foreach (DataColumn col in dt3.Columns)
                    {
                        if (col.ColumnName == "FREEQTY")
                        {
                            col.ReadOnly = false;
                        }
                    }
                    for (int i = 0; i <= dt3.Rows.Count - 1; i++)
                    {
                        if (Convert.ToDecimal(dt3.Rows[i]["FREEQTYPCS"].ToString()) > 0)
                        {
                            dt3.Rows[i]["FREEQTY"] = Convert.ToInt32(dt3.Rows[i]["FREEQTYPCS"]);
                        }
                    }
                    dt3.AcceptChanges();
                    DataView dv = dt3.DefaultView;
                    dv.Sort = "SCHEMECODE ASC,SetNo ASC, FREEQTY ASC";
                    DataTable sortedDT = dv.ToTable();
                    return sortedDT;
                    #endregion
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public Int16 GetQtyofGroupItem(string Group, string BoxPCS)
        {
            DataTable dt;
            if (Group.Trim().Length == 0 || Group.Trim().ToUpper() == "ALL")
            {
                dt = obj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS'");
            }
            else
            {
                dt = obj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and (DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS')");
            }
            string Product_Code = "";
            foreach (DataRow row in dt.Rows)
            {
                if (Product_Code.Length > 0)
                { Product_Code = Product_Code + ",'" + row["ItemId"].ToString() + "'"; }
                else
                { Product_Code = Product_Code + "'" + row["ItemId"].ToString() + "'"; }
            }
            DataTable dtLineItems = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session[SessionGrid];
            }
            Int16 Qty = 0;
            string SrchField = "";
            if (BoxPCS == "BOX")
            {
                SrchField = "BOX_Qty";
            }
            else
            {
                SrchField = "PCS_Qty";
            }
            try
            {
                Qty = Convert.ToInt16(dtLineItems.Compute("Sum(" + SrchField + ")", "Product_Code in (" + Product_Code + ")"));
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return Qty;
        }
        public decimal GetValueofGroupItem(string Group)
        {
            DataTable dt;
            if (Group.Trim().Length == 0 || Group.Trim().ToUpper() == "ALL")
            {
                dt = obj.GetData("Select ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS'");
            }
            else
            {
                dt = obj.GetData("Select ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and (DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS')");
            }
            decimal Value = 0;
            string Product_Code = "";
            foreach (DataRow row in dt.Rows)
            {
                if (Product_Code.Length > 0)
                { Product_Code = Product_Code + ",'" + row["ItemId"].ToString() + "'"; }
                else
                { Product_Code = Product_Code + "'" + row["ItemId"].ToString() + "'"; }
            }
            DataTable dtLineItems = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session[SessionGrid];
            }
            DataTable dtSchCheck = new DataTable();
            try
            {
                dtSchCheck = dtLineItems.Select("Product_Code in (" + Product_Code + ")").CopyToDataTable();
            }
            catch (Exception ex){ ErrorSignal.FromCurrentContext().Raise(ex); }

            if (dtSchCheck != null)
            {
                if (dtSchCheck.Rows.Count > 0)
                {
                    try
                    {
                        var objQty = dtSchCheck.AsEnumerable().Sum(r => r.Field<decimal>("Amount") + r.Field<decimal>("SCHEMEDISCVAL") + +r.Field<decimal>("ADDSCHDISCAMT"));
                        if (objQty.ToString().Length > 0)
                        { Value = Convert.ToDecimal(objQty); }
                    }
                    catch (Exception ex){ ErrorSignal.FromCurrentContext().Raise(ex); }
                }
            }
            return Value;
        }
        public Int16 GetQtyofItem(string Item, string BoxPCS)
        {
            Int16 Qty = 0;
            DataTable dtLineItems = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session[SessionGrid];
            }
            string SrchField = "";
            if (BoxPCS == "BOX")
            {
                SrchField = "BOX_Qty";
            }
            else
            {
                SrchField = "PCS_Qty";
            }
            try
            {
                var objQty = dtLineItems.Compute("Sum(" + SrchField + ")", "Product_Code='" + Item + "'");
                if (objQty.ToString().Length > 0)
                { Qty = Convert.ToInt16(objQty); }
            }
            catch (Exception ex){ ErrorSignal.FromCurrentContext().Raise(ex); }
            return Qty;
        }
        public decimal GetValueofItem(string Item)
        {
            decimal Value = 0;
            DataTable dtLineItems = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session[SessionGrid];
            }
            DataTable dtSchCheck = new DataTable();
            try
            {
                dtSchCheck = dtLineItems.Select("Product_Code in ('" + Item + "')").CopyToDataTable();
            }
            catch(Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            if (dtSchCheck != null)
            {
                if (dtSchCheck.Rows.Count > 0)
                {
                    try
                    {
                        var objQty = dtSchCheck.AsEnumerable().Sum(r => r.Field<decimal>("Amount") + r.Field<decimal>("SCHEMEDISCVAL") + r.Field<decimal>("ADDSCHDISCAMT"));
                        //var objQty = Convert.ToDecimal(dtLineItems.Compute("Sum(Amount+SCHEMEDISCVALUE)", "Product_Code in ('" + Item + "')"));
                        if (objQty.ToString().Length > 0)
                        { Value = Convert.ToDecimal(objQty); }
                    }
                    catch (Exception ex){ ErrorSignal.FromCurrentContext().Raise(ex); }
                }
            }
            return Value;
        }
        public Int16 GetMaxQtyofItem(string BoxPCS)
        {
            Int16 Qty = 0;
            DataTable dtLineItems = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session[SessionGrid];
            }
            string SrchField = "";
            if (BoxPCS == "BOX")
            {
                SrchField = "BOX_Qty";
            }
            else
            {
                SrchField = "PCS_Qty";
            }
            try
            {
                var objQty = Convert.ToInt16(dtLineItems.Compute("MAX(" + SrchField + ")", ""));
                if (objQty.ToString().Length > 0)
                { Qty = Convert.ToInt16(objQty); }
            }
            catch(Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return Qty;
        }
        public decimal GetMaxValueofItem()
        {
            decimal Value = 0;
            DataTable dtLineItems = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dtLineItems = (DataTable)Session[SessionGrid];
            }
            try
            {
                var objQty = dtLineItems.AsEnumerable().Max(r => r.Field<decimal>("Amount") + r.Field<decimal>("SCHEMEDISCVAL"));
                //var objQty = Convert.ToDecimal(dtLineItems.Compute("MAX(Amount+SCHEMEDISCVALUE)", ""));
                if (objQty.ToString().Length > 0)
                { Value = Convert.ToDecimal(objQty); }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return Value;
        }
        public void BindSchemeGrid()
        {
            Session[SessionSchGrid] = null;
            DataTable dt = GetDatafromSP(@"ACX_SCHEME");
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataTable dtscheme = new DataTable();
                    dtscheme = obj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + row["Free Item Code"].ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "',1," + Convert.ToDecimal(row["Rate"].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    if (dtscheme.Rows.Count > 0)
                    {
                        dt.Columns["Tax1Per"].ReadOnly = false;
                        dt.Columns["Tax2Per"].ReadOnly = false;
                        dt.Columns["Tax1"].ReadOnly = false;
                        dt.Columns["Tax2"].ReadOnly = false;
                        dt.Columns["Tax1Component"].ReadOnly = false;
                        dt.Columns["Tax2Component"].ReadOnly = false;
                        dt.Columns["Tax2Component"].MaxLength = 20;
                        dt.Columns["Tax1Component"].MaxLength = 20;
                        if (dtscheme.Rows[0]["RETMSG"].ToString().IndexOf("TRUE") >= 0)
                        {
                            row["Tax1Per"] = dtscheme.Rows[0]["TAX_PER"];
                            row["Tax2Per"] = dtscheme.Rows[0]["ADDTAX_PER"];
                            row["Tax1"] = dtscheme.Rows[0]["Tax1"];
                            row["Tax2"] = dtscheme.Rows[0]["Tax2"];
                            row["Tax1Component"] = dtscheme.Rows[0]["TAX1COMPONENT"];
                            row["Tax2Component"] = dtscheme.Rows[0]["TAX2COMPONENT"];
                            dt.AcceptChanges();
                        }
                    }
                }
            }
            Session[SessionSchGrid] = dt;
            gvScheme.DataSource = dt;
            gvScheme.DataBind();
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
                if (chkBx.Checked == true)
                {
                    if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                    {
                        TotalQty += Convert.ToInt16(txtQty.Text);
                        if (getBoxPcsPicQty(SchemeCode, Slab, "P") > 0)
                        {
                            txtQty1.Text = "0";
                            chkBx1.Checked = false;
                            txtQty1.ReadOnly = false;
                            string message = "alert('Free Qty should not greater than available free qty !');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            return;
                        }
                    }
                    if (TotalQty > AvlFreeQty)
                    {
                        txtQty1.Text = "0";
                        chkBx1.Checked = false;
                        txtQty1.ReadOnly = false;
                        string message = "alert('Free Qty should not greater than available free qty !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQty.Text = "0";
                }
            }
            BindingGrid = true;
            this.SchemeDiscCalculation();
            DataTable dt1 = new DataTable();
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
                    if (rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[7].Text) == 0)
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
        protected void DDLProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillProductCode();
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            DDLMaterialCode.Enabled = true;
            DDLMaterialCode.SelectedIndex = 0;
            DDLMaterialCode.Focus();
            /* Modified  29-03-2017     */
            string a = sender.ToString();
            string b = e.ToString();

            txtQtyBox.Text = "0";
            txtBoxqty.Text = "";
            txtQtyCrates.Text = "0";
            txtLtr.Text = "0.00";
            txtPrice.Text = "0.00";
            txtValue.Text = "0.00";
            txtEnterQty.Text = "0.00";
            txtViewTotalBox.Text = "0.00";
            txtBoxPcs.Text = "0.00";
            DDLMaterialCode.Focus();
        }
        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string a = sender.ToString();
                string b = e.ToString();
                txtQtyBox.Text = "0";
                txtBoxqty.Text = "";
                txtQtyCrates.Text = "0";
                txtLtr.Text = "0.00";
                txtPrice.Text = "0.00";
                txtValue.Text = "0.00";
                txtEnterQty.Text = "0.00";
                txtViewTotalBox.Text = "0.00";
                txtBoxPcs.Text = "0.00";

                DataTable dt = new DataTable();
                if (Session[SessionProductInfo] != null)
                { dt = (DataTable)Session[SessionProductInfo]; }
                else
                {
                    dt = obj.GetData("EXEC USP_GETPRODUCTINFO '" + Session["SCHSTATE"].ToString() + "','" + drpCustomerCode.SelectedValue + "','" + lblsite.Text + "','" + Session["TransLocation"].ToString() + "'");
                    Session[SessionProductInfo] = dt;
                }
                txtStockQty.Text = "0.00";
                txtPack.Text = "0";
                txtMRP.Text = "0.00";
                txtCrateSize.Text = "0.00";
                txtPCSQty.Enabled = false;
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("ITEMID='" + DDLMaterialCode.SelectedValue.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        if (Convert.ToDecimal(dr[0]["AMOUNT"]) <= 0)
                        {
                            lblMessage.Text = "Price not defined!!!";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Price not defined!!!');", true);
                            ResetTextBoxes();
                            return;
                        }
                        if (Convert.ToDecimal(dr[0]["Product_PackSize"]) <= 0)
                        {
                            lblMessage.Text = "Price size not define!!!";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Pack size not defined!!!');", true);
                            ResetTextBoxes();
                            return;
                        }
                        if (Convert.ToDecimal(dr[0]["Product_MRP"]) <= 0)
                        {
                            lblMessage.Text = "MRP not define!!!";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('MRP not defined!!!');", true);
                            ResetTextBoxes();
                            return;
                        }

                        txtPrice.Text = dr[0]["AMOUNT"].ToString();
                        txtPack.Text = dr[0]["Product_PackSize"].ToString();
                        txtMRP.Text = dr[0]["Product_MRP"].ToString();
                        txtCrateSize.Text = dr[0]["PRODUCT_CRATE_PACKSIZE"].ToString();
                        txtStockQty.Text = dr[0]["StockQty"].ToString();
                        DDLProductGroup.SelectedValue = dr[0]["PRODUCT_GROUP"].ToString();
                        ddlProductGroup_SelectedIndexChanged(sender, e);
                        DDLProductSubCategory.SelectedValue = dr[0]["PRODUCT_SUBCATEGORY"].ToString();
                        DDLMaterialCode.SelectedValue = dr[0]["ItemId"].ToString();
                        txtStockQty.Text = Convert.ToDecimal(dr[0]["StockQty"].ToString()).ToString();
                        if (dt.Rows[0]["PCSBillingApplicable"].ToString().ToUpper() == "Y")
                        {
                            txtPCSQty.Enabled = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString().Replace("'", "") + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }

            txtBoxqty.Focus();
        }
        protected void txtCrateQty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
            Session["plusfocus"] = 2;
        }
        protected void txtBoxqty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
            Session["plusfocus"] = 2;
        }
        protected void txtPCSQty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
            Session["plusfocus"] = 2;
        }
        protected void txtSecDiscPer_TextChanged(object sender, EventArgs e)
        {
            if (txtSecDiscPer.Text != string.Empty)
            {
                txtSecDiscValue.Text = string.Empty;
                Session["btngofocus"] = 1;
            }
        }
        protected void txtSecDiscValue_TextChanged(object sender, EventArgs e)
        {
            if (txtSecDiscValue.Text != string.Empty)
            {
                txtSecDiscPer.Text = string.Empty;
                Session["btngofocus"] = 1;
            }
        }

        private void SecDisCalculation()
        {
            try
            {
                decimal SecDiscPer = 0, SecDiscValue = 0;
                if (string.IsNullOrEmpty(txtSecDiscPer.Text))
                {
                    SecDiscValue = Convert.ToDecimal(txtSecDiscValue.Text);
                    //txtSecDiscPer.Text = "0";
                }
                if (string.IsNullOrEmpty(txtSecDiscValue.Text))
                {
                    SecDiscPer = Convert.ToDecimal(txtSecDiscPer.Text);
                    //txtSecDiscValue.Text = "0";
                }
                DataTable dt = (DataTable)Session[SessionGrid];
                dt.Columns["SecDiscPer"].ReadOnly = false;
                dt.Columns["SecDiscAmount"].ReadOnly = false;
                dt.Columns["TaxValue"].ReadOnly = false;
                dt.Columns["AddTaxValue"].ReadOnly = false;
                dt.Columns["TaxableAmount"].ReadOnly = false;
                dt.Columns["Total"].ReadOnly = false;
                dt.Columns["Amount"].ReadOnly = false;



                decimal TD, TaxableAmount, Tax1, Tax2, PE, DiscValue, SchemeDiscValue, AddSchDiscAmt, Basic = 0;
                foreach (DataRow row in dt.Rows)
                {
                    TD = TaxableAmount = Tax1 = Tax2 = PE = DiscValue = SchemeDiscValue = Basic = AddSchDiscAmt = 0;
                    Basic = Convert.ToDecimal(row["Invoice_Qty"]) * Convert.ToDecimal(row["Rate"]);
                    if (SecDiscPer != 0)
                    {
                        SecDiscValue = (SecDiscPer * Basic) / 100;
                    }

                    TD = Convert.ToDecimal(row["TD"]);

                    DiscValue = Convert.ToDecimal(row["DiscVal"]);

                    SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);
                    AddSchDiscAmt = Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                    PE = Convert.ToDecimal(row["PE"]);
                    TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD + PE - AddSchDiscAmt;
                    Tax1 = TaxableAmount * Convert.ToDecimal(row["TAXPer"]) / 100;
                    Tax2 = TaxableAmount * Convert.ToDecimal(row["ADDTAXPer"]) / 100;
                    row["SecDiscPer"] = SecDiscPer;
                    row["SecDiscAmount"] = SecDiscValue;
                    row["TAXABLEAMOUNT"] = TaxableAmount;
                    row["TaxValue"] = Tax1;
                    row["AddTaxValue"] = Tax2;
                    row["Total"] = TaxableAmount + Tax1 + Tax2;
                    row["Amount"] = TaxableAmount + Tax1 + Tax2;
                    if (TaxableAmount + Tax1 + Tax2 < 0)
                    {
                        if (Basic > 0)
                        {
                            txtSecDiscValue.Text = "0.00";
                            txtSecDiscPer.Text = "0.00";
                            string message = "alert('Invoice Line value should not be less than zero!! At Line no " + row["Line_No"].ToString() + "');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        }
                    }
                    dt.AcceptChanges();
                }
                Session[SessionGrid] = dt;
                if (BindingGrid)
                { BindGrid(); }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
            }
            finally
            {

            }
        }
        protected void btnGO_Click(object sender, EventArgs e)
        {
            BindingGrid = true;
            SecDisCalculation();
            //if (Convert.ToInt16(Session["intApplicable"]) == 1 || Convert.ToInt16(Session["intApplicable"]) == 3)
            {
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPCS.Text = "";
                    chkgrd.Checked = false;
                    txtSecDiscPer.Text = "";
                    txtSecDiscValue.Text = "";
                }
                this.SchemeDiscCalculation();
                this.BindSchemeGrid();
                BindingGrid = true;
                this.SchemeDiscCalculation();
            }
            if (txtSecDiscPer.Text != "" || txtSecDiscPer.Text != string.Empty)
            {
                txtSecDiscPer.Text = "";
            }
            if (txtSecDiscValue.Text != "" || txtSecDiscValue.Text != string.Empty)
            {
                txtSecDiscValue.Text = "";
            }
        }
        protected void SecDisc_TextChanged(object sender, EventArgs e)
        {
            decimal SecDiscPer = 0, SecDiscValue = 0;

            TextBox SDiscPer = (TextBox)sender;
            GridViewRow gv = (GridViewRow)SDiscPer.Parent.Parent;
            TextBox SDiscPerNew = (TextBox)gv.FindControl("SecDisc");
            if (string.IsNullOrEmpty(SDiscPerNew.Text))
            {
                SDiscPer.Text = "0";
                SDiscPerNew.Text = "0";
            }

            //SecDiscPer = Convert.ToDecimal(SDiscPer.Text);
            SecDiscPer = Convert.ToDecimal(SDiscPerNew.Text);
            Label lblLineNo = (Label)gv.FindControl("Line_No");
            TextBox txtSDiscValue = (TextBox)gv.FindControl("SecDiscValue");
            if (string.IsNullOrEmpty(txtSDiscValue.Text))
            {
                txtSDiscValue.Text = "0";
            }
            if (txtSDiscValue.Text == "0")
            {
                txtSDiscValue.Text = "0";
            }
            DataTable dt = (DataTable)Session[SessionGrid];

            dt.Columns["SecDiscPer"].ReadOnly = false;
            dt.Columns["SecDiscAmount"].ReadOnly = false;
            dt.Columns["TaxValue"].ReadOnly = false;
            dt.Columns["AddTaxValue"].ReadOnly = false;
            dt.Columns["TaxableAmount"].ReadOnly = false;
            dt.Columns["Total"].ReadOnly = false;
            dt.Columns["Amount"].ReadOnly = false;
            DataRow[] Drrow = dt.Select("Line_No=" + lblLineNo.Text);

            SecDiscValue = Convert.ToDecimal(txtSDiscValue.Text);
            BindingGrid = false;
            decimal TD, TaxableAmount, Tax1, Tax2, PE, DiscValue, SchemeDiscValue, Basic, AddSchDiscAmt = 0;
            foreach (DataRow row in Drrow)
            {
                TD = TaxableAmount = Tax1 = Tax2 = PE = DiscValue = SchemeDiscValue = Basic = AddSchDiscAmt = 0;
                Basic = Convert.ToDecimal(row["Invoice_Qty"]) * Convert.ToDecimal(row["Rate"]);

                if (SecDiscPer == 0 && SecDiscValue > 0)
                { }
                else
                {
                    SecDiscValue = (SecDiscPer * Basic) / 100;
                }
                TD = Convert.ToDecimal(row["TD"]);

                DiscValue = Convert.ToDecimal(row["DiscVal"]);

                SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);
                AddSchDiscAmt = Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                PE = Convert.ToDecimal(row["PE"]);
                TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD + PE - AddSchDiscAmt;
                Tax1 = TaxableAmount * Convert.ToDecimal(row["TAXPER"]) / 100;
                Tax2 = TaxableAmount * Convert.ToDecimal(row["ADDTAXPER"]) / 100;
                row["SecDiscPer"] = SecDiscPer;
                row["SecDiscAmount"] = SecDiscValue;
                row["TAXABLEAMOUNT"] = TaxableAmount;
                row["TaxValue"] = Tax1;
                row["AddTaxValue"] = Tax1;
                row["Total"] = TaxableAmount + Tax1 + Tax2;
                row["Amount"] = TaxableAmount + Tax1 + Tax2;
                if (TaxableAmount + Tax1 + Tax2 < 0)
                {
                    if (Basic > 0)
                    {
                        SDiscPer.Text = "0.00";
                        txtSDiscValue.Text = "0.00";
                        SecDiscValue_TextChanged(sender, e);
                        string message = "alert('Invoice Line value should not be less than zero!! At Line no " + row["Line_No"].ToString() + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    }
                }
                dt.AcceptChanges();
            }
            Session[SessionGrid] = dt;

            if (txtHdnTDValue.Text != "")
            {
                TDCalculation();
            }
            //if (Convert.ToInt16(Session["intApplicable"]) == 1 || Convert.ToInt16(Session["intApplicable"]) == 3)
            {
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPCS.Text = "";
                    chkgrd.Checked = false;
                }
                this.SchemeDiscCalculation();

                BindSchemeGrid();
                this.SchemeDiscCalculation();
            }
            BindingGrid = true;
            if (BindingGrid)
            { BindGrid(); }
        }
        protected void SecDiscValue_TextChanged(object sender, EventArgs e)
        {
            decimal SecDiscPer = 0, SecDiscValue = 0;

            TextBox SDiscVal = (TextBox)sender;
            GridViewRow gv = (GridViewRow)SDiscVal.Parent.Parent;

            if (string.IsNullOrEmpty(SDiscVal.Text))
            {
                SDiscVal.Text = "0";
            }
            SecDiscValue = Convert.ToDecimal(SDiscVal.Text);

            TextBox SDiscPer = (TextBox)gv.FindControl("SecDisc");
            if (string.IsNullOrEmpty(SDiscPer.Text))
            {
                SDiscPer.Text = "0";
            }
            SDiscPer.Text = "0";
            SecDiscPer = Convert.ToDecimal(SDiscPer.Text);
            Label lblLineNo = (Label)gv.FindControl("Line_No");
            TextBox txtSDiscValue = (TextBox)gv.FindControl("SecDiscValue");
            if (string.IsNullOrEmpty(txtSDiscValue.Text))
            {
                txtSDiscValue.Text = "0";
            }
            DataTable dt = (DataTable)Session[SessionGrid];

            dt.Columns["SecDiscPer"].ReadOnly = false;
            dt.Columns["SecDiscAmount"].ReadOnly = false;
            dt.Columns["TaxValue"].ReadOnly = false;
            dt.Columns["AddTaxValue"].ReadOnly = false;
            dt.Columns["TaxableAmount"].ReadOnly = false;
            dt.Columns["Total"].ReadOnly = false;
            dt.Columns["Amount"].ReadOnly = false;
            DataRow[] Drrow = dt.Select("Line_No=" + lblLineNo.Text);
            BindingGrid = false;
            decimal TD, TaxableAmount, Tax1, Tax2, PE, DiscValue, SchemeDiscValue, Basic, AddSchDiscAmt = 0;
            foreach (DataRow row in Drrow)
            {
                TD = TaxableAmount = Tax1 = Tax2 = PE = DiscValue = SchemeDiscValue = Basic = AddSchDiscAmt = 0;
                Basic = Convert.ToDecimal(row["Invoice_Qty"]) * Convert.ToDecimal(row["Rate"]);
                if (SecDiscPer != 0)
                {
                    SecDiscValue = (SecDiscPer * Basic) / 100;
                }

                TD = Convert.ToDecimal(row["TD"]);

                DiscValue = Convert.ToDecimal(row["DiscVal"]);
                AddSchDiscAmt = Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                SchemeDiscValue = Convert.ToDecimal(row["SchemeDiscVal"]);
                PE = Convert.ToDecimal(row["PE"]);
                TaxableAmount = Basic - DiscValue - SecDiscValue - SchemeDiscValue - TD + PE - AddSchDiscAmt;
                Tax1 = TaxableAmount * Convert.ToDecimal(row["TAXPER"]) / 100;
                Tax2 = TaxableAmount * Convert.ToDecimal(row["ADDTAXPER"]) / 100;
                row["SecDiscPer"] = SecDiscPer;
                row["SecDiscAmount"] = SecDiscValue;
                row["TAXABLEAMOUNT"] = TaxableAmount;
                row["TaxValue"] = Tax1;
                row["AddTaxValue"] = Tax1;
                row["Total"] = TaxableAmount + Tax1 + Tax2;
                row["Amount"] = TaxableAmount + Tax1 + Tax2;
                if (TaxableAmount + Tax1 + Tax2 < 0)
                {
                    if (Basic > 0)
                    {
                        SDiscPer.Text = "0.00";
                        SDiscVal.Text = "0.00";
                        string message = "alert('Invoice Line value should not be less than zero!! At Line no " + row["Line_No"].ToString() + "');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    }
                }
                dt.AcceptChanges();
            }
            Session[SessionGrid] = dt;
            if (txtTDValue.Text != "")
            {
                TDCalculation();
            }

            //if (Convert.ToInt16(Session["intApplicable"]) == 1 || Convert.ToInt16(Session["intApplicable"]) == 3)
            {
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPCS.Text = "";
                    chkgrd.Checked = false;
                }

                this.SchemeDiscCalculation();
                BindSchemeGrid();
                this.SchemeDiscCalculation();
            }
            BindingGrid = true;
            if (BindingGrid)
            { BindGrid(); }
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
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            return;
                        }
                    }
                    if (TotalQtyPcs > AvlFreeQtyPcs)
                    {
                        txtQtyPcs1.Text = "0";
                        chkBx1.Checked = false;
                        txtQtyPcs1.ReadOnly = false;
                        string message = "alert('Free Qty Pcs should not greater than available free qty pcs !');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return;
                    }
                }
                else
                {
                    txtQtyPcs.Text = "0";
                }
            }
            BindingGrid = true;
            this.SchemeDiscCalculation();
            GridViewFooterCalculate();
        }
        public void CalculateQtyAmt(Object sender)
        {
            decimal dblTotalQty = 0, crateQty = 0, boxQty = 0, pcsQty = 0, crateSize = 0, boxPackSize = 0;
            DataTable dt = new DataTable();
            if (Session[SessionProductInfo] != null)
            { dt = (DataTable)Session[SessionProductInfo]; }
            else
            {
                dt = obj.GetData("EXEC USP_GETPRODUCTINFO '" + Session["SCHSTATE"].ToString() + "','" + drpCustomerCode.SelectedValue + "','" + lblsite.Text + "','" + Session["TransLocation"].ToString() + "'");
                Session[SessionProductInfo] = dt;
            }
            crateQty = App_Code.Global.ParseDecimal(txtCrateQty.Text);
            boxQty = App_Code.Global.ParseDecimal(txtBoxqty.Text);
            pcsQty = App_Code.Global.ParseDecimal(txtPCSQty.Text);
            crateSize = App_Code.Global.ParseDecimal(txtCrateSize.Text);
            boxPackSize = App_Code.Global.ParseDecimal(txtPack.Text);

            txtCrateQty.Text = Convert.ToString(crateQty);
            txtBoxqty.Text = Convert.ToString(boxQty);
            txtPCSQty.Text = Convert.ToString(pcsQty);

            dblTotalQty = crateQty * crateSize + boxQty + (pcsQty / (boxPackSize == 0 ? 1 : boxPackSize));
            txtQtyBox.Text = dblTotalQty.ToString("0.000000");
            DataRow[] dr = dt.Select("ITEMID='" + DDLMaterialCode.SelectedValue.ToString() + "'");
            if (dr.Length > 0)
            {
                txtQtyCrates.Text = Math.Round(Math.Ceiling(dblTotalQty / (Convert.ToDecimal(dr[0]["PRODUCT_CRATE_PACKSIZE"]) == 0 ? 1 : Convert.ToDecimal(dr[0]["PRODUCT_CRATE_PACKSIZE"]))), 2).ToString();
                txtLtr.Text = Math.Round(((dblTotalQty * Convert.ToDecimal(dr[0]["LTR"]) * Convert.ToDecimal(dr[0]["PRODUCT_PACKSIZE"])) / 1000), 2).ToString();
                txtPrice.Text = Math.Round(Convert.ToDecimal(dr[0]["AMOUNT"]), 2).ToString();
                txtValue.Text = Math.Round((Convert.ToDecimal(dr[0]["AMOUNT"]) * dblTotalQty), 2).ToString();
                txtStockQty.Text = Math.Round((Convert.ToDecimal(dr[0]["StockQty"])), 2).ToString();
            }

            txtStockQty.Text = (txtStockQty.Text.Trim().Length == 0 ? "0" : txtStockQty.Text);

            decimal TotalBox = 0, TotalPcs = 0;
            TotalBox = Math.Truncate(dblTotalQty);                          //Extract Only Box Qty From Total Qty
            TotalPcs = Math.Round((dblTotalQty - TotalBox) * boxPackSize);  //Extract Only Pcs Qty From Total Qty
            string BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);
            // Call CalculatePrice 
            txtStockQty.Text = (txtStockQty.Text.Trim().Length == 0 ? "0" : txtStockQty.Text);
            txtQtyBox.Text = (txtQtyBox.Text.Trim().Length == 0 ? "0" : txtQtyBox.Text);
            if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
            {
                string message = "alert('Stock not available!!!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }
            txtBoxPcs.Text = BoxPcs;
            txtViewTotalBox.Text = TotalBox.ToString("0.00");
            txtViewTotalPcs.Text = TotalPcs.ToString("0.00");
            BtnAddItem.Focus();
            //BtnAddItem.CausesValidation = false;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpSONumber.Text == "")
                {
                    Session["SO_NOList"] = null;
                    Response.Redirect("~/frmInvoicePrepration.aspx");
                }
                bool b = Validation();
                if (!b)
                {
                    return;
                }
                bool SchemeStock = checkStock();
                if (SchemeStock == false)
                {
                    string message = "alert('Please check stock qty for scheme product!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }
                string SelectedSchemeCode = string.Empty;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chk = (CheckBox)rw.FindControl("ChkSelect");
                    if (chk.Checked)
                    {
                        SelectedSchemeCode = rw.Cells[1].Text;
                        break;
                    }
                }

                bool IsSchemeValidate = false;

                //DataTable dtApplicable = obj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                //if (dtApplicable.Rows.Count > 0)
                //{
                //    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                //}
                //if (intApplicable == 1 || intApplicable == 3)
                //{
                    IsSchemeValidate = obj.ValidateSchemeQty(SelectedSchemeCode, ref gvScheme); // ValidateSchemeQty(SelectedSchemeCode);
               // }
                //else
                //{
                    //IsSchemeValidate = true;
                //}

                if (IsSchemeValidate == true)
                {
                    if (b == true)
                    {


                        //if (strmessage == string.Empty)
                        //{
                        SaveHeader();
                        string message = "alert('Invoice No: " + txtInvoiceNo.Text + " Saved Successfully!!'); window.location.href='frmInvoicePrepration.aspx'; ";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        ClearAll();
                        //}
                    }
                }
                else
                {
                    string message = "alert('Free Quantity should be Equal !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void rdAll_CheckedChanged(object sender, EventArgs e)
        {
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }
        protected void btnSavePrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (drpSONumber.Text == "")
                {
                    Session["SO_NOList"] = null;
                    Response.Redirect("~/frmInvoicePrepration.aspx");
                }
                bool b = Validation();
                if (!b)
                {
                    return;
                }
                bool SchemeStock = checkStock();
                if (SchemeStock == false)
                {
                    string message = "alert('Please check stock qty for scheme product!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }
                string SelectedSchemeCode = string.Empty;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chk = (CheckBox)rw.FindControl("ChkSelect");
                    if (chk.Checked)
                    {
                        SelectedSchemeCode = rw.Cells[1].Text;
                        break;
                    }
                }

                bool IsSchemeValidate = false;

                //DataTable dtApplicable = obj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                //if (dtApplicable.Rows.Count > 0)
                //{
                //    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                //}
                //if (intApplicable == 1 || intApplicable == 3)
                //{
                    IsSchemeValidate = obj.ValidateSchemeQty(SelectedSchemeCode, ref gvScheme); // ValidateSchemeQty(SelectedSchemeCode);
                //}
                //else
                //{
                 //   IsSchemeValidate = true;
              //  }

                if (IsSchemeValidate == true)
                {
                    if (b == true)
                    {


                        //if (strmessage == string.Empty)
                        //{
                        SaveHeader();
                        //string message = "debugger; alert('Invoice No: " + txtInvoiceNo.Text + " Saved Successfully!!');  window.location.href='frmInvoicePrepration.aspx';" +
                        //             " var printWindow = window.open('frmReport.aspx?SaleInvoiceNo=" + txtInvoiceNo.Text + "&Type=SaleInvoice&docType=PDF&Site=" + Session["SiteCode"].ToString() + "','_newtab').print();";
                        string message = "alert('Invoice No: " + txtInvoiceNo.Text + " Saved Successfully!!');  window.location.href='frmInvoicePrepration.aspx';" +
                                     " var printWindow = window.open('frmReport.aspx?SaleInvoiceNo=" + txtInvoiceNo.Text + "&Type=SaleInvoice&docType=PDF&Site=" + Session["SiteCode"].ToString() + "','_newtab').print();";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        ClearAll();
                        //}
                    }
                }
                else
                {
                    string message = "alert('Free Quantity should be Equal !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public bool checkSchemeStock()
        {
            bool result = false;

            DataTable dtStock = new DataTable();
            if (gvScheme.Rows.Count > 0)
            {
                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        string Sproduct = gv.Cells[4].Text;
                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        string strStcok = "Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]='" + Session["SiteCode"].ToString() + "' and F.[ProductCode]='" + Sproduct + "' and F.[TransLocation]='" + Session["TransLocation"].ToString() + "'  ";
                        dtStock = obj.GetData(strStcok);
                        decimal curstock = 0;
                        if (dtStock.Rows.Count > 0)
                        {
                            curstock = Convert.ToDecimal(dtStock.Rows[0]["TransQty"].ToString());
                        }
                        if (curstock < Convert.ToDecimal(txtQtyToAvail.Text))
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }
        public bool checkStock()
        {
            bool result = true;
            DataTable dtLine = new DataTable();
            DataTable dtStock = new DataTable();
            DataColumn column = new DataColumn();

            //-----------------------------------------------------------//
            dtLine = new DataTable("dtLine");
            //dtLine.Columns.Add(column);
            // dtLineItems.Columns.Add("MaterialGroup", typeof(string));
            dtLine.Columns.Add("Product_Code", typeof(string));
            dtLine.Columns.Add("StockQty", typeof(decimal));
            dtLine.Columns.Add("InvoiceQty", typeof(decimal));

            foreach (GridViewRow gvD in gvDetails.Rows)
            {
                TextBox InvoiceQty = (TextBox)gvD.FindControl("txtBox");
                Label lblStock = (Label)gvD.FindControl("StockQty");
                HiddenField product = (HiddenField)gvD.FindControl("HiddenField2");
                DataRow row;
                row = dtLine.NewRow();

                row["Product_Code"] = product.Value;
                row["StockQty"] = lblStock.Text;
                row["InvoiceQty"] = InvoiceQty.Text;

                dtLine.Rows.Add(row);

            }

            if (gvScheme.Rows.Count > 0)
            {
                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        string Sproduct = gv.Cells[4].Text;
                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        TextBox txtQtyToAvailPcs = (TextBox)gv.FindControl("txtQtyPcs");
                        txtQtyToAvail.Text = (txtQtyToAvail.Text.Trim().Length == 0 ? "0" : txtQtyToAvail.Text);
                        txtQtyToAvailPcs.Text = (txtQtyToAvailPcs.Text.Trim().Length == 0 ? "0" : txtQtyToAvailPcs.Text);

                        int packSize = 1, boxqty = 0, pcsQty = 0;
                        decimal billQty = 0;
                        using (DataTable dtFreeItem = obj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + gv.Cells[4].Text.ToString() + "'"))
                        {
                            if (dtFreeItem.Rows.Count > 0)
                            {
                                string strPack = dtFreeItem.Rows[0]["Product_PackSize"].ToString();
                                packSize = Convert.ToInt16(strPack);

                            }
                        }
                        if (Convert.ToInt16(txtQtyToAvailPcs.Text) > 0)
                        {
                            pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text);
                        }

                        else
                        {
                            pcsQty = 0;
                        }

                        boxqty = Convert.ToInt32(pcsQty / packSize);
                        pcsQty = pcsQty - (boxqty * packSize);

                        boxqty = boxqty + Convert.ToInt32(txtQtyToAvail.Text);
                        billQty = Math.Round(Convert.ToDecimal((boxqty + (pcsQty / packSize))), 2);

                        string strStcok = "Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]='" + Session["SiteCode"].ToString() + "' and F.[ProductCode]='" + Sproduct + "' and F.[TransLocation]='" + Session["TransLocation"].ToString() + "'  ";
                        dtStock = obj.GetData(strStcok);
                        decimal curstock = 0;
                        if (dtStock.Rows.Count > 0)
                        {
                            curstock = Convert.ToDecimal(dtStock.Rows[0]["TransQty"].ToString());
                        }
                        DataRow row;
                        row = dtLine.NewRow();
                        row["Product_Code"] = gv.Cells[4].Text;
                        row["StockQty"] = curstock;
                        row["InvoiceQty"] = billQty;// decimal.Parse(txtQtyToAvail.Text);

                        dtLine.Rows.Add(row);

                    }
                }
            }

            var stkresult = dtLine.AsEnumerable()
                .GroupBy(r => new
                {
                    ProductCode = r.Field<String>("Product_Code"),
                })

                .Select(g =>
                {
                    var row = g.First();
                    row.SetField("InvoiceQty", g.Sum(r => r.Field<Decimal>("InvoiceQty")));
                    return row;
                })
                .CopyToDataTable();

            dtLine = stkresult;
            string strSQL = "";
            for (int i = 0; i < dtLine.Rows.Count; i++)
            {
                // ReCalculate Product Stock
                strSQL = "Select coalesce(CAST(SUM(F.TransQty) AS decimal(10,2)),0) AS TransQty FROM [AX].[ACXINVENTTRANS] F WHERE F.[SiteCode]='" + Session["SiteCode"].ToString() + "' AND F.[ProductCode]='" + dtLine.Rows[i]["Product_Code"].ToString() + "' AND F.[TransLocation]='" + Session["TransLocation"].ToString() + "'  ";
                dtStock = obj.GetData(strSQL);
                decimal curstock = 0;
                if (dtStock.Rows.Count > 0)
                {
                    curstock = Convert.ToDecimal(dtStock.Rows[0]["TransQty"].ToString());
                }
                else
                { curstock = 0; }
                //decimal curStock = Convert.ToDecimal(dtLine.Rows[i]["StockQty"].ToString());
                decimal invQty = Convert.ToDecimal(dtLine.Rows[i]["InvoiceQty"].ToString());
                if (curstock < invQty)
                {
                    result = false;
                }

            }
            return result;
        }
        public string InvoiceNo()
        {
            try
            {
                string IndNo = string.Empty;
                string Number = string.Empty;
                int intTotalRec;
                string strQuery = "SELECT ISNULL(MAX(CAST(RIGHT(Invoice_No,7) AS INT)),0)+1 AS new_InvoiceNo FROM [AX].[ACXSALEINVOICEHEADER] WHERE [Siteid]='" + lblsite.Text + "' AND TranType = 1";

                DataTable dt = new DataTable();
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
                        if (txtTIN.Text != "")
                        {
                            IndNo = "VI-" + st;
                        }
                        else
                        {
                            IndNo = "RI-" + st;
                        }


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
                        if (txtTIN.Text != "")
                        {
                            IndNo = "VI-" + st;
                        }
                        else
                        {
                            IndNo = "RI-" + st;
                        }
                        //                        IndNo = "RI-" + st;
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
        public void SaveHeader()
        {
            try
            {
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                //string query;                
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                DataTable dtNumSEQ = obj.GetNumSequenceNew(6, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                string SO_NO = string.Empty;
                string NUMSEQ = string.Empty;
                if (dtNumSEQ != null)
                {
                    txtInvoiceNo.Text = dtNumSEQ.Rows[0][0].ToString();
                    NUMSEQ = dtNumSEQ.Rows[0][1].ToString();
                }
                else
                {
                    return;
                }

                DataSet ds = new DataSet();
                DataTable dtSessionTbl = new DataTable();
                DataTable dtLineItem = new DataTable();
                dtSessionTbl = (DataTable)Session[SessionGrid];
                dtLineItem = dtSessionTbl.Copy();

                string RefSchemeCode, RefSchemeLineNo;
                int RefSchemeType;
                RefSchemeCode = "";
                RefSchemeLineNo = "0";
                RefSchemeType = -1;

                //===================================================
                // Insert For Scheme Line
                #region Scheme
                DataTable dtProdInfo = new DataTable();
                dtProdInfo = (DataTable)Session[SessionProductInfo];

                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        TextBox txtQtyToAvailPcs = (TextBox)gv.FindControl("txtQtyPcs");
                        txtQtyToAvail.Text = (txtQtyToAvail.Text.Trim().Length == 0 ? "0" : txtQtyToAvail.Text);
                        txtQtyToAvailPcs.Text = (txtQtyToAvailPcs.Text.Trim().Length == 0 ? "0" : txtQtyToAvailPcs.Text);
                        HiddenField pr = (HiddenField)gv.Cells[2].FindControl("HiddenField2");
                        HiddenField hScTax1component = (HiddenField)gv.Cells[2].FindControl("hScTax1component");
                        HiddenField hScTax2component = (HiddenField)gv.Cells[2].FindControl("hScTax2component");
                        HiddenField hScTax1 = (HiddenField)gv.Cells[2].FindControl("hScTax1");
                        HiddenField hScTax2 = (HiddenField)gv.Cells[2].FindControl("hScTax2");
                        HiddenField hdnSchemeLine = (HiddenField)gv.FindControl("hdnSchemeLine");
                        HiddenField hdnSchemeType = (HiddenField)gv.FindControl("hdnSchemeType");
                        RefSchemeCode = gv.Cells[1].Text.ToString();
                        RefSchemeLineNo = hdnSchemeLine.Value.ToString();
                        RefSchemeType = Convert.ToInt16(hdnSchemeType.Value);

                        decimal decQtyToAvail = App_Code.Global.ConvertToDecimal(txtQtyToAvail.Text);
                        {
                            #region For Box Pcs Conv
                            decimal packSize = 1, boxqty = 0, pcsQty = 0;
                            decimal billQty = 0;
                            string boxPcs = "";

                            using (DataTable dtFreeItem = obj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + gv.Cells[4].Text.ToString() + "'"))
                            {
                                if (dtFreeItem.Rows.Count > 0)
                                {
                                    string strPack = dtFreeItem.Rows[0]["Product_PackSize"].ToString();
                                    packSize = App_Code.Global.ConvertToDecimal(strPack);
                                }
                            }
                            if (Convert.ToInt16(txtQtyToAvailPcs.Text) > 0)
                            {
                                pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text);
                            }
                            else
                            {
                                pcsQty = 0;
                            }

                            //boxqty = (pcsQty >= packSize ? Convert.ToInt32(pcsQty / packSize) : 0);
                            boxqty = (pcsQty >= packSize ? Convert.ToInt32(Math.Floor(pcsQty / packSize)) : 0);
                            pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text) - (boxqty * packSize);

                            boxqty = boxqty + Convert.ToInt32(txtQtyToAvail.Text);
                            billQty = Math.Round((boxqty + (pcsQty / packSize)), 4);// Math.Round(Convert.ToDecimal((boxqty + (pcsQty / packSize))), 2);
                            boxPcs = boxqty.ToString() + "." + (pcsQty.ToString().Length == 1 ? "0" : "") + pcsQty.ToString();

                            string[] calValue = obj.CalculatePrice1(gv.Cells[4].Text, drpCustomerCode.SelectedItem.Value, billQty, "Box", Session["SiteCode"].ToString());
                            string strLtr = string.Empty;
                            if (calValue[1] != null)
                            {
                                strLtr = calValue[1].ToString();
                            }
                            string strUOM = string.Empty;
                            if (calValue[4] != null)
                            {
                                strUOM = calValue[4].ToString();
                            }
                            #endregion

                            //totamt = totamt + Global.ConvertToDecimal(lblTotal.Text);
                            if (billQty > 0)
                            {
                                #region Add Scheme Line
                                DataRow[] dr = dtProdInfo.Select("ItemId='" + gv.Cells[4].Text.ToString() + "'");
                                decimal SchPer, SchAmt, Tax1Amt, Tax2Amt, TaxableAmt, Amount;
                                SchPer = Convert.ToDecimal(dtSessionTbl.Compute("Max(SchemeDisc)", ""));
                                SchAmt = (billQty * Convert.ToDecimal(gv.Cells[16].Text)) * SchPer / 100;
                                TaxableAmt = (billQty * Convert.ToDecimal(gv.Cells[16].Text)) - SchAmt;
                                Tax1Amt = TaxableAmt * Convert.ToDecimal(gv.Cells[21].Text) / 100;
                                Tax2Amt = TaxableAmt * Convert.ToDecimal(gv.Cells[23].Text) / 100;
                                Amount = TaxableAmt + Tax1Amt + Tax2Amt;
                                DataRow row;
                                row = dtLineItem.NewRow();
                                row["Product_Code"] = gv.Cells[4].Text.ToString();
                                row["Line_No"] = dtLineItem.Rows.Count + 1;
                                row["Product"] = "";
                                row["Pack"] = dr[0]["Product_PackSize"];
                                row["So_Qty"] = new decimal(0.0);
                                row["Invoice_Qty"] = billQty;
                                row["Box_Qty"] = Convert.ToDecimal(txtQtyToAvail.Text.ToString());
                                row["Pcs_Qty"] = Convert.ToDecimal(txtQtyToAvailPcs.Text.ToString());
                                row["BoxPcs"] = boxPcs;
                                row["Ltr"] = Convert.ToDecimal(strLtr);
                                row["Rate"] = Convert.ToDecimal(gv.Cells[16].Text);
                                row["BasePrice"] = Convert.ToDecimal(gv.Cells[16].Text);
                                //row["Amount"] = Convert.ToDecimal(gv.Cells[24].Text);
                                row["Amount"] = Amount;
                                row["DiscType"] = 2;
                                row["ClaimDiscAmt"] = Convert.ToDecimal(0);
                                row["Disc"] = Convert.ToDecimal(0);
                                row["DiscVal"] = Convert.ToDecimal(0);
                                //==========For Tax===============
                                row["TaxPer"] = Convert.ToDecimal(gv.Cells[21].Text);
                                row["TaxValue"] = Tax1Amt;
                                //row["TaxValue"] = Convert.ToDecimal(gv.Cells[21].Text);
                                row["ADDTaxPer"] = Convert.ToDecimal(gv.Cells[23].Text);
                                row["ADDTaxValue"] = Tax2Amt;
                                //row["ADDTaxValue"] = Convert.ToDecimal(gv.Cells[23].Text);
                                row["MRP"] = dr[0]["PRODUCT_MRP"];
                                row["TaxableAmount"] = TaxableAmt;
                                //row["TaxableAmount"] = Convert.ToDecimal(gv.Cells[19].Text);
                                row["StockQty"] = "0";
                                row["SchemeCode"] = gv.Cells[1].Text;
                                row["Total"] = Convert.ToDecimal(gv.Cells[25].Text);
                                row["TD"] = new decimal(0);
                                row["PE"] = new decimal(0);
                                row["SecDiscPer"] = new decimal(0);
                                row["SecDiscAmount"] = new decimal(0);
                                row["SchemeDisc"] = SchPer;
                                row["SchemeDiscVal"] = SchAmt;
                                row["ADDSCHDISCPER"] = Convert.ToDecimal(0);
                                row["ADDSCHDISCVAL"] = Convert.ToDecimal(0);
                                row["ADDSCHDISCAMT"] = Convert.ToDecimal(0);
                                //row["SchemeDisc"] = Convert.ToDecimal(gv.Cells[17].Text);
                                //row["SchemeDiscVal"] = Convert.ToDecimal(gv.Cells[18].Text);
                                /*  ----- GST Implementation Start */
                                row["TaxComponent"] = gv.Cells[27].Text;
                                row["AddTaxComponent"] = gv.Cells[28].Text;
                                row["HSNCode"] = gv.Cells[26].Text;
                                row["TAX1COMPONENT"] = hScTax1component.Value.ToString().Trim();
                                row["TAX2COMPONENT"] = hScTax2component.Value.ToString().Trim();
                                row["TAX2"] = hScTax2.Value.ToString().Trim();
                                row["TAX1"] = hScTax1.Value.ToString().Trim();
                                row["TD_Per"] = new decimal(0);
                                row["PE_Per"] = new decimal(0);
                                int intCalculation = 2;
                                row["CalculationOn"] = "2";
                                row["DiscCalculationBase"] = intCalculation.ToString();
                                dtLineItem.Rows.Add(row);
                                #endregion
                            }
                        }
                    }
                }
                #endregion 
                dtLineItem = dtLineItem.Select("INVOICE_QTY<>0").CopyToDataTable();
                dtLineItem.TableName = "InvvoiceLine";
                ds.Tables.Add(dtLineItem);
                string InvoiceLineXml = ds.GetXml();

                DataTable dt = new DataTable();
                //======Save Header===================
                conn = obj.GetConnection();
                cmd = new SqlCommand("ACX_INV_GENERATION");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                string sono = Session["SO_NOList"].ToString();//Cache["SO_NO"].ToString(); 
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@SITEID", lblsite.Text);
                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", drpCustomerCode.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceNo.Text);
                //cmd.Parameters.AddWithValue("@SO_NO", drpSONumber.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@SO_NO", sono);
                cmd.Parameters.AddWithValue("@INVOIC_DATE", txtInvoiceDate.Text);
                cmd.Parameters.AddWithValue("@SO_DATE", txtSODate.Text);
                cmd.Parameters.AddWithValue("@CUSTGROUP_CODE", drpCustomerGroup.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@LOADSHEET_NO", txtLoadSheetNumber.Text);
                cmd.Parameters.AddWithValue("@LOADSHEET_DATE", txtLoadsheetDate.Text);
                cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtTransporterName.Text);
                cmd.Parameters.AddWithValue("@VEHICAL_NO", txtVehicleNo.Text);
                cmd.Parameters.AddWithValue("@DRIVER_CODE", txtDriverName.Text);
                cmd.Parameters.AddWithValue("@DRIVER_MOBILENO", txtDriverContact.Text);
                cmd.Parameters.AddWithValue("@status", "INSERT");
                cmd.Parameters.AddWithValue("@TranType", 1);
                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                cmd.Parameters.AddWithValue("@Remark", txtRemark.Text);
                decimal TotalInvoice_Value = 0;
                TotalInvoice_Value = Convert.ToDecimal(dtLineItem.Compute("sum(total)", ""));
                cmd.Parameters.AddWithValue("@INVOICE_VALUE", TotalInvoice_Value);
                decimal TDValue = 0;
                if (txtTDValue.Text == string.Empty)
                {
                    TDValue = 0;
                    //txtTDValue.Text = "0";
                }
                else
                {
                    TDValue = App_Code.Global.ConvertToDecimal(txtTDValue.Text);
                }
                cmd.Parameters.AddWithValue("@TDValue", TDValue);
                /* ---------- GST Implementation Start -----------*/
                cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

                if (Session["SITEGSTINREGDATE"] != null && Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0 && Convert.ToDateTime(Session["SITEGSTINREGDATE"]).Year != 1900)
                {
                    cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Convert.ToDateTime(Session["SITEGSTINREGDATE"].ToString()));
                }

                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);

                if (txtGSTtinRegistration.Text != null && txtGSTtinRegistration.Text.Trim().Length > 0 && Convert.ToDateTime(txtGSTtinRegistration.Text).Year != 1900)
                {
                    cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", Convert.ToDateTime(txtGSTtinRegistration.Text));
                }
                cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
                cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@BILLTOSTATE", txtBillToState.Text);
                cmd.Parameters.AddWithValue("@XmlData", InvoiceLineXml);
                cmd.Parameters.AddWithValue("@REFSCHEMECODE", RefSchemeCode);
                cmd.Parameters.AddWithValue("@SCHREFRECID", RefSchemeLineNo);
                cmd.Parameters.AddWithValue("@SCHEMETYPE", RefSchemeType);
                //txtInvoiceNo.Text = Convert.ToString(cmd.ExecuteScalar());
                cmd.ExecuteNonQuery();
                //cmd.ExecuteReader();
                //using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //{
                //    using (DataTable dtt = new DataTable())
                //    {
                //        sda.Fill(dtt);
                //    }
                //}
                ///* ---------- GST Implementation End -----------*/
                //========Save Line================



                //============Commit All Data================
                transaction.Commit();

                Session["SO_NOList"] = null;
                Session[SessionGrid] = null;
                Session[SessionSchGrid] = null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lblMessage.Text = ex.Message.ToString().Replace("\r\n", "").Replace("'", "").Replace("\r", "").Replace("\n", "");
                string message = "alert('" + ex.Message.ToString().Replace("\r\n", "").Replace("'", "").Replace("\r", "").Replace("\n", "") + "');";
                lblMessage.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtInvoiceNo.Text = "";
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public Boolean SavePreview()
        {
            try
            {
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                //string query;                
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                DataTable dtNumSEQ = obj.GetNumSequenceNew(6, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                string SO_NO = string.Empty;
                string NUMSEQ = string.Empty;
                if (dtNumSEQ != null)
                {
                    txtInvoiceNo.Text = dtNumSEQ.Rows[0][0].ToString();
                    NUMSEQ = dtNumSEQ.Rows[0][1].ToString();
                }
                else
                {
                    return false;
                }

                DataSet ds = new DataSet();
                DataTable dtSessionTbl = new DataTable();
                DataTable dtLineItem = new DataTable();
                dtSessionTbl = (DataTable)Session[SessionGrid];
                dtLineItem = dtSessionTbl.Copy();

                string RefSchemeCode, RefSchemeLineNo;
                int RefSchemeType;
                RefSchemeCode = "";
                RefSchemeLineNo = "0";
                RefSchemeType = -1;

                //===================================================
                // Insert For Scheme Line
                #region Scheme
                DataTable dtProdInfo = new DataTable();
                dtProdInfo = (DataTable)Session[SessionProductInfo];

                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        TextBox txtQtyToAvailPcs = (TextBox)gv.FindControl("txtQtyPcs");
                        txtQtyToAvail.Text = (txtQtyToAvail.Text.Trim().Length == 0 ? "0" : txtQtyToAvail.Text);
                        txtQtyToAvailPcs.Text = (txtQtyToAvailPcs.Text.Trim().Length == 0 ? "0" : txtQtyToAvailPcs.Text);
                        HiddenField pr = (HiddenField)gv.Cells[2].FindControl("HiddenField2");
                        HiddenField hScTax1component = (HiddenField)gv.Cells[2].FindControl("hScTax1component");
                        HiddenField hScTax2component = (HiddenField)gv.Cells[2].FindControl("hScTax2component");
                        HiddenField hScTax1 = (HiddenField)gv.Cells[2].FindControl("hScTax1");
                        HiddenField hScTax2 = (HiddenField)gv.Cells[2].FindControl("hScTax2");
                        HiddenField hdnSchemeLine = (HiddenField)gv.FindControl("hdnSchemeLine");
                        HiddenField hdnSchemeType = (HiddenField)gv.FindControl("hdnSchemeType");
                        RefSchemeCode = gv.Cells[1].Text.ToString();
                        RefSchemeLineNo = hdnSchemeLine.Value.ToString();
                        RefSchemeType = Convert.ToInt16(hdnSchemeType.Value);

                        decimal decQtyToAvail = App_Code.Global.ConvertToDecimal(txtQtyToAvail.Text);
                        {
                            #region For Box Pcs Conv
                            decimal packSize = 1, boxqty = 0, pcsQty = 0;
                            decimal billQty = 0;
                            string boxPcs = "";

                            using (DataTable dtFreeItem = obj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + gv.Cells[4].Text.ToString() + "'"))
                            {
                                if (dtFreeItem.Rows.Count > 0)
                                {
                                    string strPack = dtFreeItem.Rows[0]["Product_PackSize"].ToString();
                                    packSize = App_Code.Global.ConvertToDecimal(strPack);
                                }
                            }
                            if (Convert.ToInt16(txtQtyToAvailPcs.Text) > 0)
                            {
                                pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text);
                            }
                            else
                            {
                                pcsQty = 0;
                            }

                            //boxqty = (pcsQty >= packSize ? Convert.ToInt32(pcsQty / packSize) : 0);
                            boxqty = (pcsQty >= packSize ? Convert.ToInt32(Math.Floor(pcsQty / packSize)) : 0);
                            pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text) - (boxqty * packSize);

                            boxqty = boxqty + Convert.ToInt32(txtQtyToAvail.Text);
                            billQty = Math.Round((boxqty + (pcsQty / packSize)), 4);// Math.Round(Convert.ToDecimal((boxqty + (pcsQty / packSize))), 2);
                            boxPcs = boxqty.ToString() + "." + (pcsQty.ToString().Length == 1 ? "0" : "") + pcsQty.ToString();

                            string[] calValue = obj.CalculatePrice1(gv.Cells[4].Text, drpCustomerCode.SelectedItem.Value, billQty, "Box", Session["SiteCode"].ToString());
                            string strLtr = string.Empty;
                            if (calValue[1] != null)
                            {
                                strLtr = calValue[1].ToString();
                            }
                            string strUOM = string.Empty;
                            if (calValue[4] != null)
                            {
                                strUOM = calValue[4].ToString();
                            }
                            #endregion

                            //totamt = totamt + Global.ConvertToDecimal(lblTotal.Text);
                            if (billQty > 0)
                            {
                                #region Add Scheme Line
                                DataRow[] dr = dtProdInfo.Select("ItemId='" + gv.Cells[4].Text.ToString() + "'");
                                decimal SchPer, SchAmt, Tax1Amt, Tax2Amt, TaxableAmt, Amount;
                                SchPer = Convert.ToDecimal(dtSessionTbl.Compute("Max(SchemeDisc)", ""));
                                SchAmt = (billQty * Convert.ToDecimal(gv.Cells[16].Text)) * SchPer / 100;
                                TaxableAmt = (billQty * Convert.ToDecimal(gv.Cells[16].Text)) - SchAmt;
                                Tax1Amt = TaxableAmt * Convert.ToDecimal(gv.Cells[21].Text) / 100;
                                Tax2Amt = TaxableAmt * Convert.ToDecimal(gv.Cells[23].Text) / 100;
                                Amount = TaxableAmt + Tax1Amt + Tax2Amt;
                                DataRow row;
                                row = dtLineItem.NewRow();
                                row["Product_Code"] = gv.Cells[4].Text.ToString();
                                row["Line_No"] = dtLineItem.Rows.Count + 1;
                                row["Product"] = "";
                                row["Pack"] = dr[0]["Product_PackSize"];
                                row["So_Qty"] = new decimal(0.0);
                                row["Invoice_Qty"] = billQty;
                                row["Box_Qty"] = Convert.ToDecimal(txtQtyToAvail.Text.ToString());
                                row["Pcs_Qty"] = Convert.ToDecimal(txtQtyToAvailPcs.Text.ToString());
                                row["BoxPcs"] = boxPcs;
                                row["Ltr"] = Convert.ToDecimal(strLtr);
                                row["Rate"] = Convert.ToDecimal(gv.Cells[16].Text);
                                row["BasePrice"] = Convert.ToDecimal(gv.Cells[16].Text);
                                //row["Amount"] = Convert.ToDecimal(gv.Cells[24].Text);
                                row["Amount"] = Amount;
                                row["DiscType"] = 2;
                                row["ClaimDiscAmt"] = Convert.ToDecimal(0);
                                row["Disc"] = Convert.ToDecimal(0);
                                row["DiscVal"] = Convert.ToDecimal(0);
                                //==========For Tax===============
                                row["TaxPer"] = Convert.ToDecimal(gv.Cells[21].Text);
                                row["TaxValue"] = Tax1Amt;
                                //row["TaxValue"] = Convert.ToDecimal(gv.Cells[21].Text);
                                row["ADDTaxPer"] = Convert.ToDecimal(gv.Cells[23].Text);
                                row["ADDTaxValue"] = Tax2Amt;
                                //row["ADDTaxValue"] = Convert.ToDecimal(gv.Cells[23].Text);
                                row["MRP"] = dr[0]["PRODUCT_MRP"];
                                row["TaxableAmount"] = TaxableAmt;
                                //row["TaxableAmount"] = Convert.ToDecimal(gv.Cells[19].Text);
                                row["StockQty"] = "0";
                                row["SchemeCode"] = gv.Cells[1].Text;
                                row["Total"] = Convert.ToDecimal(gv.Cells[25].Text);
                                row["TD"] = new decimal(0);
                                row["PE"] = new decimal(0);
                                row["SecDiscPer"] = new decimal(0);
                                row["SecDiscAmount"] = new decimal(0);
                                row["SchemeDisc"] = SchPer;
                                row["SchemeDiscVal"] = SchAmt;
                                row["ADDSCHDISCPER"] = Convert.ToDecimal(0);
                                row["ADDSCHDISCVAL"] = Convert.ToDecimal(0);
                                row["ADDSCHDISCAMT"] = Convert.ToDecimal(0);
                                //row["SchemeDisc"] = Convert.ToDecimal(gv.Cells[17].Text);
                                //row["SchemeDiscVal"] = Convert.ToDecimal(gv.Cells[18].Text);
                                /*  ----- GST Implementation Start */
                                row["TaxComponent"] = gv.Cells[27].Text;
                                row["AddTaxComponent"] = gv.Cells[28].Text;
                                row["HSNCode"] = gv.Cells[26].Text;
                                row["TAX1COMPONENT"] = hScTax1component.Value.ToString().Trim();
                                row["TAX2COMPONENT"] = hScTax2component.Value.ToString().Trim();
                                row["TAX2"] = hScTax2.Value.ToString().Trim();
                                row["TAX1"] = hScTax1.Value.ToString().Trim();
                                row["TD_Per"] = new decimal(0);
                                row["PE_Per"] = new decimal(0);
                                int intCalculation = 2;
                                row["CalculationOn"] = "2";
                                row["DiscCalculationBase"] = intCalculation.ToString();
                                dtLineItem.Rows.Add(row);
                                #endregion
                            }
                        }
                    }
                }
                #endregion 
                dtLineItem = dtLineItem.Select("INVOICE_QTY<>0").CopyToDataTable();
                dtLineItem.TableName = "InvvoiceLine";
                ds.Tables.Add(dtLineItem);
                string InvoiceLineXml = ds.GetXml();

                DataTable dt = new DataTable();
                //======Save Header===================
                conn = obj.GetConnection();
                cmd = new SqlCommand("ACX_INVPRV_GENERATION");
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                string sono = Session["SO_NOList"].ToString();//Cache["SO_NO"].ToString(); 
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                cmd.Parameters.AddWithValue("@SITEID", lblsite.Text);
                cmd.Parameters.AddWithValue("@CUSTOMER_CODE", drpCustomerCode.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@INVOICE_NO", "Temp-001");
                //cmd.Parameters.AddWithValue("@SO_NO", drpSONumber.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@SO_NO", sono);
                cmd.Parameters.AddWithValue("@INVOIC_DATE", txtInvoiceDate.Text);
                cmd.Parameters.AddWithValue("@SO_DATE", txtSODate.Text);
                cmd.Parameters.AddWithValue("@CUSTGROUP_CODE", drpCustomerGroup.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@LOADSHEET_NO", txtLoadSheetNumber.Text);
                cmd.Parameters.AddWithValue("@LOADSHEET_DATE", txtLoadsheetDate.Text);
                cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtTransporterName.Text);
                cmd.Parameters.AddWithValue("@VEHICAL_NO", txtVehicleNo.Text);
                cmd.Parameters.AddWithValue("@DRIVER_CODE", txtDriverName.Text);
                cmd.Parameters.AddWithValue("@DRIVER_MOBILENO", txtDriverContact.Text);
                cmd.Parameters.AddWithValue("@status", "INSERT");
                cmd.Parameters.AddWithValue("@TranType", 1);
                cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                cmd.Parameters.AddWithValue("@Remark", txtRemark.Text);
                decimal TotalInvoice_Value = 0;
                TotalInvoice_Value = Convert.ToDecimal(dtLineItem.Compute("sum(total)", ""));
                cmd.Parameters.AddWithValue("@INVOICE_VALUE", TotalInvoice_Value);
                decimal TDValue = 0;
                if (txtTDValue.Text == string.Empty)
                {
                    TDValue = 0;
                    //txtTDValue.Text = "0";
                }
                else
                {
                    TDValue = App_Code.Global.ConvertToDecimal(txtTDValue.Text);
                }
                cmd.Parameters.AddWithValue("@TDValue", TDValue);
                /* ---------- GST Implementation Start -----------*/
                cmd.Parameters.AddWithValue("@DISTGSTINNO", Session["SITEGSTINNO"]);

                if (Session["SITEGSTINREGDATE"] != null && Session["SITEGSTINREGDATE"].ToString().Trim().Length > 0 && Convert.ToDateTime(Session["SITEGSTINREGDATE"]).Year != 1900)
                {
                    cmd.Parameters.AddWithValue("@DISTGSTINREGDATE", Convert.ToDateTime(Session["SITEGSTINREGDATE"].ToString()));
                }

                cmd.Parameters.AddWithValue("@DISTCOMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                cmd.Parameters.AddWithValue("@CUSTGSTINNO", txtGSTtin.Text);

                if (txtGSTtinRegistration.Text != null && txtGSTtinRegistration.Text.Trim().Length > 0 && Convert.ToDateTime(txtGSTtinRegistration.Text).Year != 1900)
                {
                    cmd.Parameters.AddWithValue("@CUSTGSTINREGDATE", Convert.ToDateTime(txtGSTtinRegistration.Text));
                }
                cmd.Parameters.AddWithValue("@CUSTCOMPOSITIONSCHEME", (chkCompositionScheme.Checked == true ? 1 : 0));
                cmd.Parameters.AddWithValue("@BILLTOADDRESS", txtAddress.Text);
                cmd.Parameters.AddWithValue("@SHIPTOADDRESS", ddlShipToAddress.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@BILLTOSTATE", txtBillToState.Text);
                cmd.Parameters.AddWithValue("@XmlData", InvoiceLineXml);
                cmd.Parameters.AddWithValue("@REFSCHEMECODE", RefSchemeCode);
                cmd.Parameters.AddWithValue("@SCHREFRECID", RefSchemeLineNo);
                cmd.Parameters.AddWithValue("@SCHEMETYPE", RefSchemeType);
                cmd.ExecuteNonQuery();
                //cmd.ExecuteReader();
                //using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                //{
                //    using (DataTable dtt = new DataTable())
                //    {
                //        sda.Fill(dtt);
                //    }
                //}
                ///* ---------- GST Implementation End -----------*/
                //========Save Line================




                //========Preview=====================




                //============Commit All Data================
                transaction.Commit();
                return true;
                //Session["SO_NOList"] = null;
                //Session[SessionGrid] = null;
                //Session[SessionSchGrid] = null;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
                string message = "alert('" + ex.Message.ToString().Replace("'", "") + "');";

                transaction.Rollback();
                lblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        private bool ValidateLineItemAdd()
        {
            bool b = true;

            if (DDLProductGroup.Text == "Select..." || DDLProductGroup.Text == "")
            {
                string message = "alert('Select Material Group !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                DDLProductGroup.Focus();
                b = false;
                return b;
            }
            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "Select...")
            {
                string message = "alert('Select Product First !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                DDLMaterialCode.Focus();
                b = false;
                return b;
            }
            if (txtQtyBox.Text == string.Empty || txtQtyBox.Text == "0")
            {
                b = false;
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (Convert.ToDecimal(txtQtyBox.Text.Trim()) == 0)
            {
                b = false;
                string message = "alert('Please enter Qty!!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtStockQty.Text == string.Empty || txtStockQty.Text == "0")
            {
                b = false;
                string message = "alert('Stock Qty cannot be zero !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
            {
                b = false;
                string message = "alert('Box Qty should not greater than Stock Qty !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtCrateQty.Text == string.Empty)
            {
                b = false;
                string message = "alert('Crate cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtLtr.Text == string.Empty || txtLtr.Text == "0")
            {
                b = false;
                string message = "alert('LTR cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }

            if (txtPrice.Text == string.Empty || txtPrice.Text == "0")
            {
                decimal amount;
                if (!decimal.TryParse(txtPrice.Text, out amount))
                {
                    b = false;
                    string message = "alert('Price is cannot be left blank !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return b;
                }
                if (Convert.ToDecimal(txtPrice.Text) <= 0)
                {
                    b = false;
                    string message = "alert('Please Check Product Price !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return b;
                }
                else
                {
                    b = true;
                }

            }
            return b;

        }

        private Boolean AddLineItems()
        {
            try
            {
                DataTable dtLineItems = new DataTable();
                if (Session[SessionGrid] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session[SessionGrid];
                }
                DataRow[] dataPerDay = (from myRow in dtLineItems.AsEnumerable()
                                        where myRow.Field<string>("Product_Code") == DDLMaterialCode.SelectedValue.ToString()
                                        select myRow).ToArray();
                if (dataPerDay.Count() == 0)
                {
                    #region  check Bill to Address
                    if (txtBillToState.Text.Trim().Length == 0)
                    {
                        string message = "alert('Please select Customer Bill To State!');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        return false;
                    }
                    #endregion

                    //DataTable dtApplicable = obj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                    //if (dtApplicable.Rows.Count > 0)
                    //{
                    //    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                    //}

                    #region  Discount
                    /////////////////////////////////// For /////////////////////////////////////////////////////

                    #endregion

                    int count = gvDetails.Rows.Count;
                    count = count + 1;
                    DataTable dt = new DataTable();
                    /* ----------------Gst Implementation----------------*/
                    dt = obj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["RETMSG"].ToString().IndexOf("FALSE") >= 0)
                        {
                            string message = "alert('" + dt.Rows[0]["RETMSG"].ToString().Replace("FALSE|", "") + "');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            DDLMaterialCode.Focus();
                            ResetTextBoxes();
                            return false;
                        }

                        DataRow row;
                        row = dtLineItems.NewRow();

                        row["Product_Code"] = DDLMaterialCode.SelectedValue.ToString();
                        row["Line_No"] = count;
                        row["Product"] = DDLMaterialCode.SelectedItem.Text.ToString();
                        row["Pack"] = txtPack.Text;
                        row["So_Qty"] = new decimal(0);
                        row["Invoice_Qty"] = decimal.Parse(txtQtyBox.Text.Trim().ToString());
                        row["Box_Qty"] = Convert.ToDecimal(txtViewTotalBox.Text.Trim().ToString());
                        row["Pcs_Qty"] = Convert.ToDecimal(txtViewTotalPcs.Text.Trim().ToString());
                        row["BoxPcs"] = Convert.ToDecimal(txtBoxPcs.Text.Trim());
                        row["Ltr"] = Convert.ToDecimal(txtLtr.Text.Trim().ToString());
                        row["Rate"] = Convert.ToDecimal(dt.Rows[0]["Rate"]);
                        row["BasePrice"] = Convert.ToDecimal(txtPrice.Text.Trim());
                        row["Amount"] = Convert.ToDecimal(dt.Rows[0]["VALUE"]); //TotalValue.ToString("0.00");
                        if (dt.Rows[0]["DISCTYPE"].ToString() == "")
                        {
                            row["DiscType"] = 2;
                        }
                        else if (dt.Rows[0]["DISCTYPE"].ToString() == "Per")
                        {
                            row["DiscType"] = 0;
                        }
                        else if (dt.Rows[0]["DISCTYPE"].ToString() == "Val")
                        {
                            row["DiscType"] = 1;
                        }
                        row["ClaimDiscAmt"] = Convert.ToDecimal(dt.Rows[0]["ClaimDiscAmt"]);
                        row["Disc"] = Convert.ToDecimal(dt.Rows[0]["DISC"]);
                        row["DiscVal"] = Convert.ToDecimal(dt.Rows[0]["DISCVAL"]);
                        row["SchemeDisc"] = new decimal(0);
                        row["SchemeDiscVal"] = new decimal(0);

                        //==========For Tax===============
                        row["TaxPer"] = Convert.ToDecimal(dt.Rows[0]["TAX_PER"]);
                        row["TaxValue"] = Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]);
                        row["ADDTaxPer"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]);
                        row["ADDTaxValue"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]);
                        row["MRP"] = Convert.ToDecimal(dt.Rows[0]["MRP"]);
                        row["TaxableAmount"] = Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]);

                        row["StockQty"] = decimal.Parse(txtStockQty.Text.Trim().ToString());
                        row["SchemeCode"] = "";
                        row["Total"] = Convert.ToDecimal(dt.Rows[0]["VALUE"]);
                        row["Amount"] = Convert.ToDecimal(dt.Rows[0]["VALUE"]);
                        row["TD"] = new decimal(0);
                        row["PE"] = new decimal(0);
                        row["TD_Per"] = new decimal(0);
                        row["PE_Per"] = new decimal(0);
                        row["SecDiscPer"] = new decimal(0);
                        row["SecDiscAmount"] = new decimal(0);
                        row["SchemeDisc"] = new decimal(0);
                        row["SchemeDiscVal"] = new decimal(0);
                        /*  ----- GST Implementation Start */
                        row["TaxComponent"] = dt.Rows[0]["TAX_CODE"].ToString();
                        row["AddTaxComponent"] = dt.Rows[0]["ADDTAX_CODE"].ToString();
                        row["HSNCode"] = dt.Rows[0]["HSNCODE"].ToString();
                        if (dt.Columns.Contains("CLAIMDISCAMT"))
                        { row["CLAIMDISCAMT"] = dt.Rows[0]["CLAIMDISCAMT"].ToString(); }
                        if (dt.Columns.Contains("TAX1COMPONENT"))
                        { row["TAX1COMPONENT"] = dt.Rows[0]["TAX1COMPONENT"].ToString(); }
                        if (dt.Columns.Contains("TAX2COMPONENT"))
                        { row["TAX2COMPONENT"] = dt.Rows[0]["TAX2COMPONENT"].ToString(); }
                        if (dt.Columns.Contains("TAX2"))
                        { row["TAX2"] = dt.Rows[0]["TAX2"].ToString(); }
                        if (dt.Columns.Contains("TAX2"))
                        { row["TAX1"] = dt.Rows[0]["TAX1"].ToString(); }

                        int intCalculation = 2;
                        row["CalculationOn"] = dt.Rows[0]["DISCTYPE"].ToString();
                        if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
                        {
                            intCalculation = 0;
                        }
                        else if (dt.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
                        {
                            intCalculation = 1;
                        }
                        else
                        {
                            intCalculation = 2;
                        }
                        row["DiscCalculationBase"] = intCalculation.ToString();
                        row["ADDSCHDISCAMT"] = new decimal(0);
                        row["ADDSCHDISCVAL"] = new decimal(0);
                        row["ADDSCHDISCPER"] = new decimal(0);
                        dtLineItems.Rows.Add(row);

                        //Update session table
                        Session[SessionGrid] = dtLineItems;

                        txtQtyCrates.Text = string.Empty;
                        txtQtyBox.Text = string.Empty;
                        txtLtr.Text = string.Empty;
                        txtPrice.Text = string.Empty;
                        lblHidden.Text = string.Empty;
                        txtPack.Text = "";
                        txtMRP.Text = "";
                        txtStockQty.Text = "";
                        txtValue.Text = string.Empty;
                        txtCrateQty.Text = "";
                        txtBoxqty.Text = "";
                        txtPCSQty.Text = "";
                        txtViewTotalBox.Text = "";
                        txtViewTotalPcs.Text = "";
                        txtBoxPcs.Text = "";
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string message = "alert('Error: " + ex.Message.Replace("'", "''") + " !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                ResetTextBoxes();
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }
        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            DDLMaterialCode.Focus();
            lblMessage.Text = string.Empty;
            DataTable dt = new DataTable();
            Boolean ResetMsg;
            ResetMsg = false;
            if (Session[SessionGrid] != null)
            {
                dt = (DataTable)Session[SessionGrid];

                DataRow[] drSec = dt.Select("SecDiscAmount>0");
                if (drSec.Length > 0)
                    ResetMsg = true;
                dt.Columns["SecDiscPer"].ReadOnly = false;
                dt.Columns["SecDiscAmount"].ReadOnly = false;
                //foreach (DataRow drs in dt.Rows)
                //{
                //    drs["SecDiscPer"] = 0;
                //    drs["SecDiscAmount"] = 0;
                //}

                Session[SessionGrid] = dt;
                //txtSecondaryDiscPer.Text = "";
                //txtSecondaryDiscValue.Text = "";
                //txtSecDiscPer.Text = "0";
                //txtSecDiscValue.Text = "0";
                //txtTDValue.Text = "0";
            }
            else
            {
                dt = AddColumnInDataTable();
            }
            DataRow[] drSec1 = dt.Select("TD>0");
            if (drSec1.Length > 0)
                ResetMsg = true;
            DataRow[] dr = dt.Select("Product_Code='" + DDLMaterialCode.SelectedValue.ToString() + "'");
            if (dr.Length > 0)
            {
                string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                ResetTextBoxes();
                return;
            }

            //============================
            bool valid = true;
            valid = ValidateLineItemAdd();
            if (!valid)
            {
                ResetTextBoxes();
                return;
            }

            if (valid == true)
            {

                if (AddLineItems())
                {
                    if (Session[SessionGrid] != null)
                        dt = (DataTable)Session[SessionGrid];
                }
                BindingGrid = false;
                //gvDetails.DataSource = dt;
                //gvDetails.DataBind();
                if (dt.Rows.Count > 0)
                {
                    gvDetails.Visible = true;
                    //if (txtTDValue.Text == "")
                    //{
                    //    txtTDValue.Text = "0";
                    //}
                    if (txtSecDiscPer.Text.Trim().Length > 0 || txtSecDiscValue.Text.Trim().Length > 0)
                    { SecDisCalculation(); }
                    if (txtTDValue.Text.Trim().Length > 0)
                    { TDCalculation(); }
                }
                else
                {
                    gvDetails.Visible = false;
                }
                if (ResetMsg)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Allowed Secondary Discount/Trade Discount value will be reset due to change in quantity or new product addition')", true);
                }
            }

            txtEnterQty.Text = string.Empty;
            btnApply_Click(sender, e);
            //txtSecDiscValue.Text = "0";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('All Secondary discounts were refreshed')", true);
           // if (Convert.ToInt16(Session["intApplicable"]) == 1 || Convert.ToInt16(Session["intApplicable"]) == 3)
            {
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPCS.Text = "";
                    chkgrd.Checked = false;
                }
                this.SchemeDiscCalculation();
                this.BindSchemeGrid();
            }
            this.SchemeDiscCalculation();

            BindingGrid = true;
            BindGrid();
            DDLMaterialCode.Enabled = true;
            GridViewFooterCalculate();
            ResetTextBoxes();
        }
        private void GridViewFooterCalculate()
        {
            DataTable dt = new DataTable();
            if (Session[SessionGrid] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dt = (DataTable)Session[SessionGrid];
            }
            //For Total[Sum] Value Show in Footer--//
            string st = Convert.ToString(dt.Rows[0]["TD"].GetType());
            decimal tSoQtyBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("So_Qty"));
            decimal tInvoiceQtyBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("Invoice_Qty"));
            decimal tBoxQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("Box_Qty"));
            decimal tPcsQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("Pcs_Qty"));
            decimal tLtr = dt.AsEnumerable().Sum(row => row.Field<decimal>("Ltr"));
            decimal tDiscValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("DiscVal"));
            decimal tTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TaxValue"));
            decimal tAddTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADDTaxValue"));
            decimal tSecDiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SecDiscAmount"));
            decimal tAddSecDiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADDSCHDISCAMT"));
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Amount"));
            decimal TD = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD"));
            decimal tSchemeDiscValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SchemeDiscVal"));
            decimal AllTotal = dt.AsEnumerable().Sum(row => row.Field<decimal>("Total"));
            decimal tMRP = dt.AsEnumerable().Sum(r => r.Field<decimal>("Invoice_Qty") * r.Field<decimal>("MRP"));

            gvDetails.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[3].Text = "TOTAL";
            gvDetails.FooterRow.Cells[3].Font.Bold = true;

            gvDetails.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[4].Text = tMRP.ToString("N2");
            gvDetails.FooterRow.Cells[4].Font.Bold = true;

            gvDetails.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[5].Text = tSoQtyBox.ToString("N2");
            gvDetails.FooterRow.Cells[5].Font.Bold = true;

            gvDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[6].Text = tBoxQty.ToString("N2");
            gvDetails.FooterRow.Cells[6].Font.Bold = true;

            gvDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[7].Text = tPcsQty.ToString("N2");
            gvDetails.FooterRow.Cells[7].Font.Bold = true;

            gvDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[8].Text = tInvoiceQtyBox.ToString("N2");
            gvDetails.FooterRow.Cells[8].Font.Bold = true;

            gvDetails.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[11].Text = tLtr.ToString("N2");
            gvDetails.FooterRow.Cells[11].Font.Bold = true;

            gvDetails.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[14].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[14].Text = tTaxAmount.ToString("N2");
            gvDetails.FooterRow.Cells[14].Font.Bold = true;

            gvDetails.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[16].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[16].Text = tAddTaxAmount.ToString("N2");
            gvDetails.FooterRow.Cells[16].Font.Bold = true;

            gvDetails.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[18].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[18].Text = tDiscValue.ToString("N2");
            gvDetails.FooterRow.Cells[18].Font.Bold = true;


            gvDetails.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[20].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[20].Text = tSecDiscAmount.ToString("N2");
            gvDetails.FooterRow.Cells[20].Font.Bold = true;

            gvDetails.FooterRow.Cells[24].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[24].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[24].Text = TD.ToString("N2");
            gvDetails.FooterRow.Cells[24].Font.Bold = true;

            gvDetails.FooterRow.Cells[26].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[26].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[26].Text = AllTotal.ToString("N2");
            gvDetails.FooterRow.Cells[26].Font.Bold = true;

            gvDetails.FooterRow.Cells[32].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[32].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[32].Text = tSchemeDiscValue.ToString("N2");
            gvDetails.FooterRow.Cells[32].Font.Bold = true;

            gvDetails.FooterRow.Cells[34].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[34].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[34].Text = tAddSecDiscAmount.ToString("N2");
            gvDetails.FooterRow.Cells[34].Font.Bold = true;
            CalculateInvoiceValue();
        }

        private void CalculateInvoiceValue()
        {
            DataTable dtsum = new DataTable();
            decimal TotalInvValue = 0;
            if (Session[SessionGrid] != null)
            {
                dtsum = (DataTable)Session[SessionGrid];
                TotalInvValue = Convert.ToDecimal(dtsum.Compute("Sum(total)", ""));
            }
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                {
                    TotalInvValue += Convert.ToDecimal(rw.Cells[25].Text);
                }
            }
            txtinvoicevalue.Text = TotalInvValue.ToString("0.00");
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            // btnGO_Click(sender,e);
            if (txtTDValue.Text != "")
            {
                txtHdnTDValue.Text = txtTDValue.Text;
                txtTDValue.Text = "";
                BindingGrid = true;
                TDCalculation();
                //if (Convert.ToInt32(Session["intApplicable"]) == 1 || Convert.ToInt32(Session["intApplicable"]) == 3)
                {
                    foreach (GridViewRow rw in gvScheme.Rows)
                    {
                        CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                        TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                        TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                        txtQty.Text = "";
                        txtQtyPCS.Text = "";
                        chkgrd.Checked = false;
                        txtTDValue.Text = "";
                    }
                    this.SchemeDiscCalculation();
                    BindSchemeGrid();
                    this.SchemeDiscCalculation();
                }
            }
            else if (txtHdnTDValue.Text != "")
            {
                txtTDValue.Text = "";
                BindingGrid = true;
                TDCalculation();
//if (Convert.ToInt32(Session["intApplicable"]) == 1 || Convert.ToInt32(Session["intApplicable"]) == 3)
                {
                    foreach (GridViewRow rw in gvScheme.Rows)
                    {
                        CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                        TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                        TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                        txtQty.Text = "";
                        txtQtyPCS.Text = "";
                        chkgrd.Checked = false;
                        txtTDValue.Text = "";
                    }
                    this.SchemeDiscCalculation();
                    BindSchemeGrid();
                    this.SchemeDiscCalculation();
                }
            }
            if (txtTDValue.Text != "" || txtTDValue.Text != string.Empty)
            {
                txtTDValue.Text = "";
            }
        }

        public void TDCalculation()
        {
            try
            {
                DataTable dtLineItems = new DataTable();
                if (Session[SessionGrid] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session[SessionGrid];
                }
                decimal totalBasicValue = 0;
                totalBasicValue = dtLineItems.AsEnumerable().Sum(r => r.Field<decimal>("Invoice_Qty") * r.Field<decimal>("Rate"));
                //totalBasicValue = Convert.ToDecimal(dtLineItems.Compute("sum(Invoice_Qty*Rate)", ""));
                //=========For calculate TD % ================
                decimal TDPercentage = Convert.ToDecimal(txtHdnTDValue.Text) / totalBasicValue;
                foreach (DataColumn dc in dtLineItems.Columns)
                {
                    dc.ReadOnly = false;
                }
                foreach (DataRow row in dtLineItems.Rows)
                {
                    decimal BasicValue, TD, TaxableAmount, Tax1, Tax2, PE, PE_PER, TOTALTAX;
                    BasicValue = (Convert.ToDecimal(row["Invoice_Qty"]) * Convert.ToDecimal(row["Rate"]));
                    TD = (BasicValue * TDPercentage);
                    TOTALTAX = Convert.ToDecimal(row["TaxPer"]) + Convert.ToDecimal(row["AddTaxPer"]);
                    PE_PER = TOTALTAX / (1 + (TOTALTAX / 100));
                    //PE = (TD * Convert.ToDecimal(15.25)/100);
                    PE = TD * PE_PER / 100;

                    TaxableAmount = (Convert.ToDecimal(row["Invoice_Qty"]) * Convert.ToDecimal(row["Rate"])) - Convert.ToDecimal(row["DiscVal"]) - Convert.ToDecimal(row["SecDiscAmount"]) - Convert.ToDecimal(row["SchemeDiscVal"]) - TD + PE - Convert.ToDecimal(row["ADDSCHDISCAMT"]);
                    //TaxableAmount = Convert.ToDecimal(row["TAXABLEAMOUNT"]) - TD + PE;
                    Tax1 = TaxableAmount * Convert.ToDecimal(row["TaxPer"]) / 100;
                    Tax2 = TaxableAmount * Convert.ToDecimal(row["AddTaxPer"]) / 100;
                    row["TD_Per"] = TDPercentage;
                    row["PE_Per"] = PE_PER;
                    row["TD"] = TD;
                    row["PE"] = PE;
                    row["TAXABLEAMOUNT"] = TaxableAmount;
                    row["TaxValue"] = Tax1;
                    row["AddTaxValue"] = Tax2;
                    row["Total"] = TaxableAmount + Tax1 + Tax2;
                    row["Amount"] = TaxableAmount + Tax1 + Tax2;
                }
                dtLineItems.AcceptChanges();
                Session[SessionGrid] = dtLineItems;
                if (BindingGrid)
                { BindGrid(); }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
            }
            finally
            {

            }
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
                dtLineItems = (DataTable)Session[SessionGrid];
                TotalBasicAmount = dtLineItems.AsEnumerable().Sum(row => row.Field<decimal>("Invoice_Qty") * row.Field<decimal>("Rate"));
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
                decimal SecDiscAmount = 0;
                decimal TDAmount = 0;
                decimal PEAmount = 0;
                decimal SchemeDiscPer = 0;
                decimal AddSchemeDiscount = 0;
                decimal AddSchemePerc = 0;
                decimal AddSchemeValue = 0;
                dtLineItems.Columns["TaxableAmount"].ReadOnly = false;
                dtLineItems.Columns["TaxValue"].ReadOnly = false;
                dtLineItems.Columns["AddTaxValue"].ReadOnly = false;
                dtLineItems.Columns["TaxableAmount"].ReadOnly = false;
                dtLineItems.Columns["Total"].ReadOnly = false;
                dtLineItems.Columns["Amount"].ReadOnly = false;
                dtLineItems.Columns["SchemeDisc"].ReadOnly = false;
                dtLineItems.Columns["SchemeDiscVal"].ReadOnly = false;
                dtLineItems.Columns["ADDSCHDISCPER"].ReadOnly = false;
                dtLineItems.Columns["ADDSCHDISCVAL"].ReadOnly = false;
                dtLineItems.Columns["ADDSCHDISCAMT"].ReadOnly = false;
                Boolean SchemeDiscApply = false, AddSchemeDiscApply = false;
                string SchSrlNo = "0", AddSchemeGroup = "", AddSchSrlNo = "0";
                SchemeDiscApply = AddSchemeDiscApply = false;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    HiddenField hdnSchemetype = (HiddenField)rw.FindControl("hdnSchemetype");
                    HiddenField hdnSchSrlNo = (HiddenField)rw.FindControl("hdnSchSrlNo");
                    Label lblSchemeDiscPer = (Label)rw.FindControl("lblSchemeDiscPer");
                    HiddenField hdnAddSchType = (HiddenField)rw.FindControl("hdnAddSchType");
                    Label lblAddSchemePer = (Label)rw.FindControl("lblAddSchemePer");
                    Label lblAddSchemeVal = (Label)rw.FindControl("lblAddSchemeVal");
                    Label lblAddSchemeGroup = (Label)rw.FindControl("lblAddSchemeGroup");
                    HiddenField hdntotSchemeValueoff = (HiddenField)rw.FindControl("hdntotSchemeValueoff");
                    if (chkBx.Checked && hdnAddSchType.Value != "0")
                    {
                        AddSchemeDiscApply = true;
                        AddSchemePerc = Convert.ToDecimal(lblAddSchemePer.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblAddSchemePer.Text));
                        AddSchemeValue = Convert.ToDecimal(lblAddSchemeVal.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblAddSchemeVal.Text));
                        AddSchemeGroup = lblAddSchemeGroup.Text;
                        AddSchSrlNo = hdnSchSrlNo.Value.ToString();
                    }

                    if (chkBx.Checked && hdnSchemetype.Value.ToString() == "2")
                    {
                        SchemeDiscApply = true;
                        DiscPer = lblSchemeDiscPer.Text.Trim() == "" ? 0 : Convert.ToDecimal(lblSchemeDiscPer.Text);
                        DiscPer = DiscPer / 100;
                        SchSrlNo = hdnSchSrlNo.Value.ToString();
                        break;
                    }
                    if (chkBx.Checked && hdnSchemetype.Value.ToString() == "3")
                    {
                        SchemeDiscApply = true;
                        #region Scheme Basic Value Check
                        DataTable dtSchValue = new DataTable();
                        if (Session[SessionSchGrid] != null)
                        { dtSchValue = (DataTable)Session[SessionSchGrid]; }
                        DiscPer = 0;
                        if (dtSchValue.Rows.Count > 0)
                        {
                            SchSrlNo = hdnSchSrlNo.Value.ToString();
                            DataRow[] dr = dtSchValue.Select("SrNo=" + SchSrlNo);
                            DataTable dt = new DataTable();
                            string[] ItemArray = new string[1];
                            if (dr.Length > 0)
                            {
                                #region Get Scheme Free Product List
                                if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "ALL" && (dr[0]["Scheme Item Group"].ToString().ToUpper() == "ALL" || dr[0]["Scheme Item Group"].ToString().ToUpper() == ""))
                                {
                                    dt = obj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS'");
                                }
                                else if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "GROUP")
                                {
                                    dt = obj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "' and (DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS')");
                                }
                                else
                                {
                                    dt = obj.GetData("Select  ITEMID From AX.INVENTTABLE Where ITEMID='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "'");
                                }
                                #endregion
                                #region Get Basic Value
                                if (dt.Rows.Count > 0)
                                {
                                    lineAmount = 0;
                                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                                    {
                                        DataRow[] drItem = dt.Select("ItemId='" + dtLineItems.Rows[i]["Product_Code"].ToString() + "'");
                                        if (drItem.Length > 0)
                                        {
                                            lineAmount += Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Rate"].ToString());
                                        }
                                    }
                                }
                                #endregion
                                DiscPer = Convert.ToDecimal(hdntotSchemeValueoff.Value.Trim() == "" ? 0 : Convert.ToDecimal(hdntotSchemeValueoff.Value)) / lineAmount;

                            }

                            break;
                        }
                        #endregion
                    }
                }
                if (AddSchemeDiscApply == true && (AddSchemePerc > 0 || AddSchemeValue > 0))
                {
                    #region Apply Additional Scheme of Percent or Value Off
                    DataTable dtAddSch = new DataTable();
                    if (Session[SessionSchGrid] != null)
                    { dtAddSch = (DataTable)Session[SessionSchGrid]; }

                    if (dtAddSch.Rows.Count > 0)
                    {
                        DataRow[] dr = dtAddSch.Select("SrNo=" + AddSchSrlNo);
                        DataTable dt = new DataTable();
                        string[] ItemArray = new string[1];
                        if (dr.Length > 0)
                        {
                            if (dr[0]["ADDITIONDISCOUNTITEMTYPE"].ToString().ToUpper() == "2")
                            {
                                dt = obj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + dr[0]["ADDITIONDISCOUNTITEMGROUP"].ToString().ToUpper() + "' and (DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS')");
                            }
                            else
                            {
                                dt = obj.GetData("Select  ITEMID From AX.INVENTTABLE Where ITEMID='" + dr[0]["ADDITIONDISCOUNTITEMGROUP"].ToString().ToUpper() + "'");
                            }

                            if (dt.Rows.Count > 0)
                            {

                                for (int i = 0; i < dtLineItems.Rows.Count; i++)
                                {
                                    #region Update Additional Scheme Discount
                                    DataRow[] drItem = dt.Select("ItemId='" + dtLineItems.Rows[i]["Product_Code"].ToString() + "'");
                                    if (drItem.Length > 0)
                                    {
                                        #region Update datarow
                                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Rate"].ToString());
                                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SecDiscAmount"].ToString());
                                        dtLineItems.Rows[i]["ADDSCHDISCPER"] = AddSchemePerc;
                                        dtLineItems.Rows[i]["ADDSCHDISCVAL"] = AddSchemeValue;
                                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TD"].ToString());
                                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                                        SchemeDiscPer = Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDisc"].ToString());
                                        SchemeDiscAmount = Math.Round((lineAmount * SchemeDiscPer), 6);
                                        //SchemeDiscPer = (DiscPer * 100);
                                        //SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                                        //dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                                        if (AddSchemePerc > 0)
                                        { dtLineItems.Rows[i]["ADDSCHDISCAMT"] = Math.Round((lineAmount * (AddSchemePerc / 100)), 6); }
                                        if (AddSchemeValue > 0)
                                        {
                                            dtLineItems.Rows[i]["ADDSCHDISCAMT"] = Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * AddSchemeValue;
                                        }
                                        AddSchemeDiscount = Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"]);
                                        TaxableAmount = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount);
                                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                                        if (dtLineItems.Rows[i]["TaxPer"].ToString().Trim().Length == 0)
                                        { TaxPer = 0; }
                                        else
                                        { TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["TaxPer"]); }
                                        if (dtLineItems.Rows[i]["AddTaxPer"].ToString().Trim().Length == 0)
                                        { AddTaxPer = 0; }
                                        else
                                        { AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTaxPer"]); }

                                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                                        dtLineItems.Rows[i]["TaxValue"] = TaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["AddTaxValue"] = AddTaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["Total"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                                        dtLineItems.Rows[i]["Amount"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                                        #endregion
                                    }
                                    #endregion
                                }
                                dtLineItems.AcceptChanges();
                                Session["LineItem"] = dtLineItems;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Remove Additional Scheme of Percent or Value off
                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                    {
                        #region Update datarow
                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Rate"].ToString());
                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SecDiscAmount"].ToString());
                        dtLineItems.Rows[i]["ADDSCHDISCPER"] = AddSchemePerc;
                        dtLineItems.Rows[i]["ADDSCHDISCVAL"] = AddSchemeValue;
                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TD"].ToString());
                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                        SchemeDiscPer = Convert.ToDecimal(dtLineItems.Rows[i]["SchemeDisc"].ToString());
                        SchemeDiscAmount = Math.Round((lineAmount * SchemeDiscPer), 6);
                        //dtLineItems.Rows[i]["SchemeDisc"] = (SchemeDiscPer * 100).ToString("0.000000");
                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["ADDSCHDISCAMT"] = 0;
                        AddSchemeDiscount = 0;
                        TaxableAmount = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount);
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        if (dtLineItems.Rows[i]["TaxPer"].ToString().Trim().Length == 0)
                        { TaxPer = 0; }
                        else
                        { TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["TaxPer"]); }
                        if (dtLineItems.Rows[i]["AddTaxPer"].ToString().Trim().Length == 0)
                        { AddTaxPer = 0; }
                        else
                        { AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTaxPer"]); }

                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                        dtLineItems.Rows[i]["TaxValue"] = TaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["AddTaxValue"] = AddTaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["Total"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                        dtLineItems.Rows[i]["Amount"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                        #endregion
                    }
                    dtLineItems.AcceptChanges();
                    Session["LineItem"] = dtLineItems;
                    #endregion
                }
                if (SchemeDiscApply == true && DiscPer > 0)
                {
                    DataTable dtSch = new DataTable();
                    if (Session[SessionSchGrid] != null)
                    { dtSch = (DataTable)Session[SessionSchGrid]; }
                    if (dtSch.Rows.Count > 0)
                    {
                        DataRow[] dr = dtSch.Select("SrNo=" + SchSrlNo);
                        DataTable dt = new DataTable();
                        string[] ItemArray = new string[1];
                        if (dr.Length > 0)
                        {
                            if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "ALL" && (dr[0]["Scheme Item Group"].ToString().ToUpper() == "ALL" || dr[0]["Scheme Item Group"].ToString().ToUpper() == ""))
                            {
                                dt = obj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS'");
                            }
                            else if (dr[0]["Scheme Item Type"].ToString().ToUpper() == "GROUP")
                            {
                                dt = obj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "' and (DATAAREAID='" + Session["DATAAREAID"] + "' or DATAAREAID='VMAS')");
                            }
                            else
                            {
                                dt = obj.GetData("Select  ITEMID From AX.INVENTTABLE Where ITEMID='" + dr[0]["Scheme Item Group"].ToString().ToUpper() + "'");
                            }
                            if (dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtLineItems.Rows.Count; i++)
                                {
                                    DataRow[] drItem = dt.Select("ItemId='" + dtLineItems.Rows[i]["Product_Code"].ToString() + "'");
                                    if (drItem.Length > 0)
                                    {
                                        #region Update datarow
                                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Rate"].ToString());
                                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SecDiscAmount"].ToString());
                                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TD"].ToString());
                                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                                        SchemeDiscPer = (DiscPer * 100);
                                        SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                                        AddSchemeDiscount = Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"].ToString());
                                        dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                                        TaxableAmount = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount);
                                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                                        if (dtLineItems.Rows[i]["TaxPer"].ToString().Trim().Length == 0)
                                        { TaxPer = 0; }
                                        else
                                        { TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["TaxPer"]); }
                                        if (dtLineItems.Rows[i]["AddTaxPer"].ToString().Trim().Length == 0)
                                        { AddTaxPer = 0; }
                                        else
                                        { AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTaxPer"]); }

                                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                                        dtLineItems.Rows[i]["TaxValue"] = TaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["AddTaxValue"] = AddTaxAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                                        dtLineItems.Rows[i]["Total"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                                        dtLineItems.Rows[i]["Amount"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                                        #endregion
                                    }
                                }
                                dtLineItems.AcceptChanges();
                                Session[SessionGrid] = dtLineItems;
                                if (BindingGrid)
                                { BindGrid(); }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dtLineItems.Rows.Count; i++)
                    {
                        #region update Line
                        lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Rate"].ToString());
                        DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                        SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SecDiscAmount"].ToString());
                        TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TD"].ToString());
                        PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                        SchemeDiscPer = (DiscPer * 100);
                        SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                        AddSchemeDiscount = Convert.ToDecimal(dtLineItems.Rows[i]["ADDSCHDISCAMT"]);
                        dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                        dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");
                        TaxableAmount = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount - AddSchemeDiscount);
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        if (dtLineItems.Rows[i]["TaxPer"].ToString().Trim().Length == 0)
                        { TaxPer = 0; }
                        else
                        { TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["TaxPer"]); }
                        if (dtLineItems.Rows[i]["AddTaxPer"].ToString().Trim().Length == 0)
                        { AddTaxPer = 0; }
                        else
                        { AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTaxPer"]); }

                        TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                        AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                        dtLineItems.Rows[i]["TaxValue"] = TaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["AddTaxValue"] = AddTaxAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                        dtLineItems.Rows[i]["Total"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                        dtLineItems.Rows[i]["Amount"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                        #endregion
                    }
                    dtLineItems.AcceptChanges();
                    Session[SessionGrid] = dtLineItems;
                    if (BindingGrid)
                    { BindGrid(); }
                }

                //#endregion
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
            }
        }
        protected decimal SchemeLineCalculation(decimal discPer)
        {
            decimal Rate = 0, Qty = 0, BasicAmount = 0, packSize = 0;
            decimal TotalBasicAmount = 0;
            decimal TaxPer = 0;
            decimal DiscAmt = 0;
            decimal AddTaxPer = 0;
            decimal TaxAmount = 0;
            decimal AddTaxAmount = 0;

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                //if (chkBx.Checked == true)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                    txtQtyPCS.Text = (txtQtyPCS.Text.Trim().Length == 0 ? "0" : txtQtyPCS.Text);
                    packSize = Convert.ToDecimal(rw.Cells[14].Text);

                    Qty = Convert.ToDecimal(txtQty.Text) + (Convert.ToDecimal(txtQtyPCS.Text) > 0 ? Convert.ToDecimal(txtQtyPCS.Text) / packSize : 0);
                    Rate = Convert.ToDecimal(rw.Cells[16].Text);
                    rw.Cells[15].Text = Convert.ToString(Qty.ToString("0.0000"));
                    rw.Cells[17].Text = Convert.ToString((Rate * Qty).ToString("0.0000"));
                    BasicAmount = (Rate * Qty);
                    //rw.Cells[16].Text = Convert.ToString(Rate * Qty);
                    rw.Cells[18].Text = (discPer * 100).ToString("0.00");
                    DiscAmt = BasicAmount * discPer;
                    rw.Cells[19].Text = DiscAmt.ToString("0.00");
                    rw.Cells[20].Text = (BasicAmount - DiscAmt).ToString("0.0000");
                    if (rw.Cells[21].Text.Trim().Length == 0)
                    {
                        TaxPer = 0;
                    }
                    else
                    {
                        TaxPer = Convert.ToDecimal(rw.Cells[21].Text);
                    }
                    if (rw.Cells[23].Text.Trim().Length == 0)
                    {
                        AddTaxPer = 0;
                    }
                    else
                    {
                        AddTaxPer = Convert.ToDecimal(rw.Cells[23].Text);
                    }
                    TaxAmount = Math.Round(((BasicAmount - DiscAmt) * TaxPer) / 100, 2);
                    AddTaxAmount = Math.Round(((BasicAmount - DiscAmt) * AddTaxPer) / 100, 2);
                    rw.Cells[22].Text = TaxAmount.ToString("0.00");
                    rw.Cells[24].Text = AddTaxAmount.ToString("0.00");
                    rw.Cells[25].Text = (BasicAmount + TaxAmount + AddTaxAmount - DiscAmt).ToString("0.00");
                    TotalBasicAmount = TotalBasicAmount + BasicAmount;
                }
            }
            return TotalBasicAmount;
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
                int TransType = 2;//1 for Slae invoice 
                int DocumentType = 2;
                string DocumentNo = txtInvoiceNo.Text;
                string DocumentDate = txtInvoiceDate.Text;
                string uom = "BOX";
                string Referencedocumentno = drpSONumber.SelectedItem.Text;
                string TransLocation = Session["TransLocation"].ToString();
                //============Loop For LineItem==========
                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    Label Product = (Label)gvDetails.Rows[i].Cells[2].FindControl("Product");
                    TextBox box = (TextBox)gvDetails.Rows[i].Cells[6].FindControl("txtBox");

                    string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                    string productNameCode = Product.Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    string Qty = box.Text;
                    decimal TransQty = App_Code.Global.ConvertToDecimal(Qty);
                    int REcid = i + 1;

                    if (TransQty > 0)
                    {
                        string query = " Insert Into ax.acxinventTrans " +
                                "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno])" +
                                " Values ('" + TransId + "','" + Siteid + "','" + DATAAREAID + "'," + REcid + ",getdate()," + TransType + "," + DocumentType + ",'" + DocumentNo + "'," +
                                " '" + DocumentDate + "','" + ProductCode + "'," + -TransQty + ",'" + uom + "','" + TransLocation + "','" + Referencedocumentno + "')";
                        obj.ExecuteCommand(query);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "')", true);
            }
        }

        private DataTable AddColumnInDataTable()
        {
            DataTable dtLineItems;
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            column.ColumnName = "Line_No";
            //-----------------------------------------------------------//
            dtLineItems = new DataTable("LineItemTable");
            dtLineItems.Columns.Add(column);
            //dtLineItems.Columns.Add("Line_No", typeof(decimal));
            dtLineItems.Columns.Add("Product_Code", typeof(string));
            dtLineItems.Columns.Add("Product", typeof(string));
            dtLineItems.Columns.Add("Pack", typeof(string));
            dtLineItems.Columns.Add("MRP", typeof(decimal));
            dtLineItems.Columns.Add("DiscCalculationBase", typeof(string));
            dtLineItems.Columns.Add("So_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Ltr", typeof(decimal));
            dtLineItems.Columns.Add("Invoice_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Box_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Pcs_Qty", typeof(decimal));
            dtLineItems.Columns.Add("BoxPcs", typeof(decimal));
            dtLineItems.Columns.Add("Rate", typeof(decimal));
            dtLineItems.Columns.Add("BasePrice", typeof(decimal));
            dtLineItems.Columns.Add("Amount", typeof(decimal));
            dtLineItems.Columns.Add("Total", typeof(decimal));
            dtLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            dtLineItems.Columns.Add("TD", typeof(decimal));
            dtLineItems.Columns.Add("TaxPer", typeof(decimal));
            dtLineItems.Columns.Add("TaxValue", typeof(decimal));
            dtLineItems.Columns.Add("AddTaxPer", typeof(decimal));
            dtLineItems.Columns.Add("AddTaxValue", typeof(decimal));
            dtLineItems.Columns.Add("StockQty", typeof(decimal));
            dtLineItems.Columns.Add("Disc", typeof(decimal));
            dtLineItems.Columns.Add("DiscVal", typeof(decimal));
            dtLineItems.Columns.Add("DISCTYPE", typeof(string));
            dtLineItems.Columns.Add("SchemeCode", typeof(string));
            dtLineItems.Columns.Add("CalculationOn", typeof(string));
            dtLineItems.Columns.Add("SecDiscPer", typeof(decimal));
            dtLineItems.Columns.Add("SecDiscAmount", typeof(decimal));
            dtLineItems.Columns.Add("HSNCode", typeof(string));
            dtLineItems.Columns.Add("TaxComponent", typeof(string));
            dtLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtLineItems.Columns.Add("SchemeDisc", typeof(decimal));
            dtLineItems.Columns.Add("SchemeDiscVal", typeof(decimal));
            dtLineItems.Columns.Add("PE", typeof(decimal));
            dtLineItems.Columns.Add("CLAIMDISCAMT", typeof(decimal));
            dtLineItems.Columns.Add("TAX1COMPONENT", typeof(string));
            dtLineItems.Columns.Add("TAX2COMPONENT", typeof(string));
            dtLineItems.Columns.Add("TAX1", typeof(decimal));
            dtLineItems.Columns.Add("TAX2", typeof(decimal));
            dtLineItems.Columns.Add("TD_Per", typeof(decimal));
            dtLineItems.Columns.Add("TE_Per", typeof(decimal));
            dtLineItems.Columns.Add("ADDSCHDISCPER", typeof(decimal));
            dtLineItems.Columns.Add("ADDSCHDISCVAL", typeof(decimal));
            dtLineItems.Columns.Add("ADDSCHDISCAMT", typeof(decimal));
            //dtLineItems.Columns.Add("Crate", typeof(decimal));
            return dtLineItems;
        }

        public void ResetTextBoxes()
        {
            txtCrateQty.Text = "";
            txtBoxqty.Text = "";
            txtPCSQty.Text = "";
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtQtyBox.Text = "";
            txtBoxPcs.Text = "";
            txtLtr.Text = "";
            txtPrice.Text = "";
            txtValue.Text = "";
            txtStockQty.Text = "";
            DDLProductGroup.Items.Clear();
            DDLProductSubCategory.Items.Clear();
            DDLMaterialCode.Items.Clear();
            ProductGroup();
            FillProductCode();
            //DDLProductGroup.SelectedIndex = 0;
            //DDLProductSubCategory.DataSource = null;
            //DDLProductSubCategory.DataBind();
            //DDLProductSubCategory.SelectedIndex = 0;
            //DDLMaterialCode.SelectedIndex = 0;
        }

        public void ClearAll()
        {
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtBoxPcs.Text = "";
            drpCustomerCode.Items.Clear();
            drpCustomerCode.Items.Clear();
            txtTIN.Text = "";
            txtAddress.Text = "";
            txtMobileNO.Text = "";
            txtLoadSheetNumber.Text = "";
            txtLoadsheetDate.Text = "";
            drpSONumber.Items.Clear();
            txtSODate.Text = "";
            txtTransporterName.Text = "";
            txtDriverContact.Text = "";
            txtDriverName.Text = "";
            txtVehicleNo.Text = "";
            txtInvoiceNo.Text = "";
            txtInvoiceDate.Text = "";
            gvDetails.DataSource = null;
            gvDetails.DataBind();
            gvScheme.DataSource = null;
            gvScheme.DataBind();
            Session[SessionGrid] = null;
            Session[SessionProductInfo] = null;
            Session[SessionSchGrid] = null;
            txtSecDiscPer.Text = "0.00";
            txtSecDiscValue.Text = "0.00";
            txtTDValue.Text = "";
            txtHdnTDValue.Text = "";

            txtRemark.Text = string.Empty;

            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtStockQty.Text = string.Empty;
            txtPack.Text = string.Empty;
            txtMRP.Text = string.Empty;
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
        }
        protected void txtBoxQtyGrid_TextChanged(object sender, EventArgs e)
        {
            TextBox txtBoxQty = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtBoxQty.Parent.Parent;

            HiddenField ddlPrCode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
            TextBox txtPcs = (TextBox)row.Cells[0].FindControl("txtPcsQtyGrid");
            TextBox textTotal = (TextBox)row.Cells[0].FindControl("txtBox");
            TextBox txtSecondaryDiscPer = (TextBox)row.FindControl("SecDisc");
            TextBox txtSecondaryDiscValue = (TextBox)row.FindControl("SecDiscValue");
            if (txtBoxQty.Text == "")
            {
                txtBoxQty.Text = "0";
            }
            Session["Status"] = null;
            decimal TotalBox;
            DataTable dt = new DataTable();
            dt = (DataTable)Session[SessionGrid];
            Boolean ResetMsg;
            ResetMsg = false;
            DataRow[] drSec = dt.Select("SecDiscAmount>0");
            if (drSec.Length > 0)
            {
                ResetMsg = true;
                dt.Columns["SecDiscPer"].ReadOnly = false;
                dt.Columns["SecDiscAmount"].ReadOnly = false;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    dr["SecDiscPer"] = 0;
                //    dr["SecDiscAmount"] = 0;
                //}
                Session[SessionGrid] = dt;
            }
            TotalBox = obj.GetTotalBox(ddlPrCode.Value, 0, Convert.ToDecimal(txtPcs.Text), Convert.ToDecimal(txtBoxQty.Text));
            textTotal.Text = Convert.ToString(TotalBox);
            //txtSecondaryDiscPer.Text = "";
            //txtSecondaryDiscValue.Text = "";
            txtSecDiscPer.Text = "";
            txtSecDiscValue.Text = "";
            //txtTDValue.Text = "0";
            txtTotBoxGrid_TextChanged((object)textTotal, e);
            SecDisc_TextChanged((object)textTotal, e);
            if (ResetMsg)
            {
                //string status = "OpenConfirmDialog()";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "OpenConfirmDialog();", true);
                //string text = Hidden1.Value;
                //if(text.ToString()=="false")
                //{
                //    return;
                //}

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Allowed Secondary/Trade Discount value will be reset due to change in quantity or new product addition')", true);
            }

        }
        protected void txtPcsQtyGrid_TextChanged(object sender, EventArgs e)
        {
            TextBox txtPcsQty = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtPcsQty.Parent.Parent;

            HiddenField ddlPrCode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
            TextBox txtBoxQty = (TextBox)row.Cells[0].FindControl("txtBoxQtyGrid");
            TextBox textTotal = (TextBox)row.Cells[0].FindControl("txtBox");

            decimal TotalBox;
            TotalBox = obj.GetTotalBox(ddlPrCode.Value, 0, Convert.ToDecimal(txtPcsQty.Text), Convert.ToDecimal(txtBoxQty.Text));
            DataTable dt = new DataTable();
            dt = (DataTable)Session[SessionGrid];
            Boolean ResetMsg;
            ResetMsg = false;
            DataRow[] drSec = dt.Select("SecDiscAmount>0");
            if (drSec.Length > 0)
            {
                ResetMsg = true;

                dt.Columns["SecDiscPer"].ReadOnly = false;
                dt.Columns["SecDiscAmount"].ReadOnly = false;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    dr["SecDiscPer"] = 0;
                //    dr["SecDiscAmount"] = 0;
                //}
                Session[SessionGrid] = dt;
            }
            textTotal.Text = Convert.ToString(TotalBox);
            //txtSecDiscPer.Text = "0";
            //txtSecDiscValue.Text = "0";
            //txtTDValue.Text = "0";
            txtTotBoxGrid_TextChanged((object)textTotal, e);
            //txtSecDiscValue.Text = "0";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('All Secondary discounts were refreshed')", true);
            if (ResetMsg)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('Allowed Secondary/Trade Discount value will be reset due to change in quantity or new product addition')", true);
            }
        }
        protected void txtTotBoxGrid_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            TextBox txtB = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtB.Parent.Parent;
            Boolean ReCalculate = false;
            int idx = row.RowIndex;
            if (Session[SessionGrid] != null)
                dt = (DataTable)Session[SessionGrid];

            TextBox txtBoxQtyGrid = (TextBox)row.FindControl("txtBoxQtyGrid");
            Label lblLine_No = (Label)row.FindControl("Line_No");
            TextBox txtPcsQtyGrid = (TextBox)row.FindControl("txtPcsQtyGrid");
            TextBox txtBox = (TextBox)row.FindControl("txtBox");
            TextBox txtSecondaryDiscPer = (TextBox)row.FindControl("SecDisc");
            TextBox txtSecondaryDiscValue = (TextBox)row.FindControl("SecDiscValue");
            Label txtBoxPcs = (Label)row.FindControl("txtBoxPcs");
            Label lblLtr = (Label)row.FindControl("LTR");
            DataRow[] dr = dt.Select("Line_No=" + lblLine_No.Text);
            if (dr.Length > 0)
            {
                DataTable dtProdInfo = (DataTable)Session[SessionProductInfo];
                if ((Convert.ToDecimal(txtBoxQtyGrid.Text) + Convert.ToDecimal(txtPcsQtyGrid.Text)) <= 0)
                {
                    ReCalculate = false;
                    row.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    DataRow[] drProd = dtProdInfo.Select("ItemId='" + dr[0]["Product_Code"].ToString() + "'");
                    string[] Quantity = GetQuantity(App_Code.Global.ConvertToDecimal("0"), App_Code.Global.ConvertToDecimal(txtBoxQtyGrid.Text), App_Code.Global.ConvertToDecimal(txtPcsQtyGrid.Text), App_Code.Global.ConvertToDecimal(drProd[0]["PRODUCT_PackSIZE"]), App_Code.Global.ConvertToDecimal(drProd[0]["PRODUCT_CRATE_PACKSIZE"]), App_Code.Global.ConvertToDecimal(drProd[0]["LTR"]));
                    txtBoxQtyGrid.Text = Quantity[1];
                    txtPcsQtyGrid.Text = Quantity[2];
                    txtBox.Text = Quantity[3];
                    txtB.Text = Quantity[3];
                    txtBoxPcs.Text = Quantity[4];
                    lblLtr.Text = Quantity[5];
                    if (Convert.ToDecimal(txtB.Text) > Convert.ToDecimal(dr[0]["StockQty"]))
                    {
                        ReCalculate = false;
                        row.BackColor = System.Drawing.Color.Red;
                        txtB.Text = "0.0000";
                        txtBox.Text = "0.0000";
                        txtBoxQtyGrid.Text = "0";
                        txtPcsQtyGrid.Text = "0";
                        string message = "alert('Please Enter a valid number or input quantity less than stock quantity!!');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        row.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        row.BackColor = System.Drawing.Color.White;
                        ReCalculate = true;
                    }
                }
            }
            else
            {
                string message = "alert('Invalid Operation performed!!');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }
            if (Convert.ToDecimal(txtBox.Text) <= 0)
            {
                ReCalculate = false;
            }
            foreach (DataColumn dc in dt.Columns)
            {
                dc.ReadOnly = false;
            }
            BindingGrid = false;
            if (ReCalculate)
            {
                DataTable dtReCal = new DataTable();
                dtReCal = obj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + dr[0]["Product_Code"].ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtBox.Text) + "," + Convert.ToDecimal(dr[0]["BasePrice"]) + ",'" + Session["SITELOCATION"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                if (dtReCal.Rows.Count > 0)
                {
                    if (dtReCal.Rows[0]["RETMSG"].ToString().IndexOf("TRUE") >= 0)
                    {
                        dr[0]["LTR"] = Convert.ToDecimal(lblLtr.Text);
                        if (dtReCal.Rows[0]["DISCTYPE"].ToString() == "")
                        {
                            dr[0]["DiscType"] = 2;
                        }
                        else if (dt.Rows[0]["DISCTYPE"].ToString() == "Per")
                        {
                            dr[0]["DiscType"] = 0;
                        }
                        else if (dt.Rows[0]["DISCTYPE"].ToString() == "Val")
                        {
                            dr[0]["DiscType"] = 1;
                        }
                        dr[0]["CalculationOn"] = dtReCal.Rows[0]["CALCULATIONBASE"].ToString();
                        if (dtReCal.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "PRICE")
                        {
                            dr[0]["DiscCalculationBase"] = 0;
                        }
                        else if (dtReCal.Rows[0]["CALCULATIONBASE"].ToString().ToUpper() == "MRP")
                        {
                            dr[0]["DiscCalculationBase"] = 1;
                        }
                        else
                        {
                            dr[0]["DiscCalculationBase"] = 2;
                        }


                        dr[0]["Invoice_Qty"] = Convert.ToDecimal(txtBox.Text);
                        dr[0]["BOX_Qty"] = Convert.ToDecimal(txtBoxQtyGrid.Text);
                        dr[0]["PCS_Qty"] = Convert.ToDecimal(txtPcsQtyGrid.Text);
                        dr[0]["BOXPCS"] = Convert.ToString(txtBoxPcs.Text);
                        dr[0]["DiscVal"] = Convert.ToDecimal(dtReCal.Rows[0]["DISCVAL"]);
                        dr[0]["Rate"] = Convert.ToDecimal(dtReCal.Rows[0]["Rate"]);
                        dr[0]["TaxPer"] = Convert.ToDecimal(dtReCal.Rows[0]["TAX_Per"]);
                        dr[0]["AddTaxPer"] = Convert.ToDecimal(dtReCal.Rows[0]["ADDTAX_Per"]);
                        dr[0]["ClaimDiscAmt"] = Convert.ToDecimal(dtReCal.Rows[0]["CLAIMDISCAMT"]);
                        dr[0]["TAX1"] = Convert.ToDecimal(dtReCal.Rows[0]["Tax1"]);
                        dr[0]["TAX2"] = Convert.ToDecimal(dtReCal.Rows[0]["Tax2"]);
                        dr[0]["TAX1COMPONENT"] = Convert.ToString(dtReCal.Rows[0]["TAX1COMPONENT"]);
                        dr[0]["TAX2COMPONENT"] = Convert.ToString(dtReCal.Rows[0]["TAX2COMPONENT"]);
                        decimal BasicValue, TD, TaxableAmount, Tax1, Tax2, PE, DiscValue, SecondaryDiscValue, SchemeDiscValue;
                        BasicValue = (Convert.ToDecimal(txtBox.Text) * Convert.ToDecimal(dtReCal.Rows[0]["Rate"]));
                        TD = Convert.ToDecimal(dr[0]["TD"]);
                        DiscValue = Convert.ToDecimal(dr[0]["DiscVal"]);
                        if (txtSecondaryDiscPer.Text.Trim().Length == 0 || txtSecondaryDiscPer.Text == "0")
                            txtSecondaryDiscPer.Text = "0";
                        if (txtSecondaryDiscValue.Text.Trim().Length == 0 || txtSecondaryDiscPer.Text == "0")
                            txtSecondaryDiscValue.Text = "0";
                        SecondaryDiscValue = 0;
                        if (Convert.ToDecimal(txtSecondaryDiscPer.Text) > 0)
                        {
                            SecondaryDiscValue = 0;//= BasicValue * Convert.ToDecimal(txtSecondaryDiscPer.Text) / 100;
                        }
                        else
                        {
                            if (Convert.ToDecimal(txtSecondaryDiscValue.Text) > 0)
                            {
                                SecondaryDiscValue = 0;// = Convert.ToDecimal(txtSecondaryDiscValue.Text);
                            }
                        }
                        SchemeDiscValue = Convert.ToDecimal(dr[0]["SchemeDiscVal"]);

                        //SecondaryDiscValue = Convert.ToDecimal(dr[0]["SecondaryDiscValue"]);
                        PE = Convert.ToDecimal(dr[0]["PE"]);
                        TaxableAmount = BasicValue - DiscValue - SecondaryDiscValue - SchemeDiscValue - TD + PE + Convert.ToDecimal(dr[0]["ADDSCHDISCAMT"]);
                        Tax1 = TaxableAmount * Convert.ToDecimal(dtReCal.Rows[0]["TAX_Per"]) / 100;
                        Tax2 = TaxableAmount * Convert.ToDecimal(dtReCal.Rows[0]["ADDTAX_Per"]) / 100;
                        dr[0]["TAXABLEAMOUNT"] = TaxableAmount;
                        dr[0]["TaxValue"] = Tax1;
                        dr[0]["AddTaxValue"] = Tax1;
                        dr[0]["Total"] = TaxableAmount + Tax1 + Tax2;
                        dr[0]["Amount"] = TaxableAmount + Tax1 + Tax2;
                        dt.AcceptChanges();
                        Session[SessionGrid] = dt;

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + dtReCal.Rows[0]["RETMSG"].ToString().Replace("'", "''") + "');", true);
                        ReCalculate = false;
                    }
                }
                else
                {
                    ReCalculate = false;
                }
            }
            if (!ReCalculate)
            {
                dr[0]["LTR"] = new decimal(0);
                dr[0]["DiscCalculationBase"] = new decimal(0);
                dr[0]["Invoice_Qty"] = new decimal(0);
                dr[0]["BOX_Qty"] = new decimal(0);
                dr[0]["PCS_Qty"] = new decimal(0);
                dr[0]["BOXPCS"] = "0.00";
                dr[0]["AMOUNT"] = new decimal(0);
                dr[0]["TOTAL"] = new decimal(0);
                dr[0]["TAXABLEAMOUNT"] = new decimal(0);
                dr[0]["TaxValue"] = new decimal(0);
                dr[0]["AddTaxValue"] = new decimal(0);
                dr[0]["DiscVal"] = new decimal(0);
                dr[0]["DiscVal"] = new decimal(0);
                dr[0]["SecDiscAmount"] = new decimal(0);
                dr[0]["SchemeDiscVal"] = new decimal(0);
                dr[0]["ADDSCHDISCAMT"] = new decimal(0);
                dr[0]["ADDSCHDISCVAL"] = new decimal(0);
                dr[0]["ADDSCHDISCPER"] = new decimal(0);
                dr[0]["PE"] = new decimal(0);
                dr[0]["CLAIMDISCAMT"] = new decimal(0);
                dr[0]["TD"] = new decimal(0);
                dt.AcceptChanges();
                Session[SessionGrid] = dt;
            }
            //Label lblSchemeDiscVal = (Label)row.FindControl("lblSchemeDiscVal");
            //if (lblSchemeDiscVal.Text.Trim().Length > 0)
            //{
            //    SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
            //}

            //have to update session 
            //if (txtTDValue.Text == "")
            //{
            //    txtTDValue.Text = "0";
            //}
            if (txtHdnTDValue.Text.Trim().Length > 0 && Convert.ToDecimal(txtHdnTDValue.Text) > 0)
            { TDCalculation(); }
            //if (txtSecDiscPer.Text.Trim().Length > 0 || txtSecDiscValue.Text.Trim().Length > 0)
            //{ btnGO_Click(sender, e); }
            // For Shceme Seletion
          //  if (Convert.ToInt32(Session["intApplicable"]) == 1 || Convert.ToInt32(Session["intApplicable"]) == 3)
            {
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkgrd = (CheckBox)rw.FindControl("chkSelect");
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    TextBox txtQtyPCS = (TextBox)rw.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPCS.Text = "";
                    chkgrd.Checked = false;
                }
                this.SchemeDiscCalculation();
                BindSchemeGrid();
            }
            this.SchemeDiscCalculation();
            BindingGrid = true;
            if (BindingGrid)
            { BindGrid(); }
        }

        protected void txtTDValue_TextChanged(object sender, EventArgs e)
        {
            if (txtTDValue.Text != string.Empty)
            {
                txtHdnTDValue.Text = txtTDValue.Text;
                Session["tdfocus"] = 3;
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            bool b = Validation();
            if (!b)
            {
                return;
            }
            if (SavePreview())
            {

                //string message = "debugger; alert('Preview Generated Successfully!!');  " +
                //                         " var printWindow = window.open('frmReport.aspx?SaleInvoiceNo=Temp-001&Type=SaleInvoicePreview','_newtab').print();";
                string message = " alert('Preview Generated Successfully!!');  " +
                                         " var printWindow = window.open('frmReport.aspx?SaleInvoiceNo=Temp-001&Type=SaleInvoicePreview','_newtab').print();";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            }
            //string url = "frmReport.aspx?SaleInvoiceNo=Temp-001&Type=SaleInvoicePreview";
            //Response.Write("<script type='text/javascript'>window.open('" + url + "','_blank');</script>");
            //Response.Redirect("frmReport.aspx?SaleInvoiceNo=Temp-001&Type=SaleInvoicePreview");
            //btnPreview.OnClientClick = "window.open('frmReport.aspx?SaleInvoiceNo=Temp-001&Type=SaleInvoicePreview','frmReport')";

        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDLProductSubCategory.Items.Clear();
            ProductGroup();
            FillProductCode();
        }

        private string[] GetQuantity(decimal CrateQty, decimal BoxQty, decimal PcsQty, decimal PackSize, decimal CratePackSize, decimal Ltr)
        {
            string[] Quantity = new string[6] { "0", "0", "0", "0", "0", "0" };
            decimal TotalQty;
            TotalQty = (CrateQty * CratePackSize) + BoxQty + (PcsQty / PackSize);
            //CrateQty = CrateQty <= 0 ? 1 : CrateQty;
            Quantity[0] = (((TotalQty / CratePackSize) - Math.Round(TotalQty / CratePackSize, 0) > 0 ? Math.Round(TotalQty / CratePackSize, 0) + 1 : Math.Round(TotalQty / CratePackSize, 0))).ToString();                           //CrateQty
            Quantity[1] = Math.Round(TotalQty, 0).ToString();                          //Box
            Quantity[2] = Math.Round(((TotalQty - Math.Round(TotalQty, 0)) * PackSize), 0).ToString(); //Pcs
            Quantity[3] = TotalQty.ToString("0.000000");
            Quantity[4] = Quantity[1] + "." + (Quantity[2].Length > 1 ? Quantity[2] : "0" + Quantity[2]);
            Quantity[5] = ((TotalQty * PackSize * Ltr) / 1000).ToString("0.000000");
            return Quantity;
        }

        protected void rdAsc_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        //private void BindingGird()
        //{

        //    if (Session["LineItem"] == null)
        //    {
        //        gvDetails.DataSource = null;
        //        gvDetails.DataBind();
        //        gvDetails.Visible = false;
        //    }
        //    DataTable dt = (DataTable)Session["LineItem"];
        //    if (dt != null)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {

        //            DataView dv = dt.DefaultView;
        //            if (rdAsc.Checked == true)
        //            {
        //                dv.Sort = "Line_No " + rdAsc.Text.ToString();
        //            }
        //            else if (rdDesc.Checked == true)
        //            {
        //                dv.Sort = "Line_No " + rdDesc.Text.ToString();
        //            }
        //            GridViewFooterCalculate();
        //            //GridViewFooterCalculate(dt);
        //            gvDetails.DataSource = dv.ToTable();                    
        //            gvDetails.DataBind();

        //            gvDetails.Visible = true;
        //        }
        //        else
        //        {
        //            gvDetails.DataSource = dt;
        //            gvDetails.DataBind();
        //            gvDetails.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        gvDetails.DataSource = null;
        //        gvDetails.DataBind();
        //        gvDetails.Visible = false;
        //    }
        //}
        //protected void ddlsorting_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    BindingGird();
        //}
    }
}