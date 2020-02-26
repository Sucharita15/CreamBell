using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmInvoiceSave : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2;
        SqlTransaction transaction;
        string strQuery = string.Empty;
        int intApplicable;
        Boolean boolDiscAvalMRP = false;
        DataTable Msg = new DataTable();
        public DataTable dtSchemeLineItems;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    #region SO Load
                    btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
                    Session["InvSchemeLineItem"] = null;
                    Session["InvLineItem"] = null;
                    dtLineItems = null;
                    ProductGroup();
                    FillProductCode();

                    if (Session["SO_NOList"] != null)
                    {
                        string sono = Session["SO_NOList"].ToString();// Cache["SO_NO"].ToString();

                        if (Session["SiteCode"] != null)
                        {
                            string siteid = Session["SiteCode"].ToString();
                            if (lblsite.Text == "")
                            {
                                lblsite.Text = siteid;
                            }
                            //==================For Warehouse Location==============
                            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                            string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                            DataTable dt = new DataTable();
                            dt = obj.GetData(query1);
                            if (dt.Rows.Count > 0)
                            {
                                Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                            }
                        }
                        else
                        {
                            string siteid = "Select A.SiteId From ax.ACXLOADSHEETHEADER A where A.SO_NO in (select id from [dbo].[CommaDelimitedToTable]('" + sono + "' ,','))";
                            DataTable dt = new DataTable();
                            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                            dt = obj.GetData(siteid);
                            if (dt.Rows.Count > 0)
                            {
                                lblsite.Text = dt.Rows[0]["SiteId"].ToString();
                            }
                        }


                        if (sono != "")
                        {
                            ShowData(sono);
                        }

                        txtInvoiceDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);

                        // For Shceme Seletion
                        DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                        if (dtApplicable.Rows.Count > 0)
                        {
                            intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                        }

                        if (intApplicable == 1 || intApplicable == 3)
                        {
                            this.BindSchemeGrid();
                        }

                        string strQuery = @"Select SchemeCode,Product_Code as ItemCode,cast(BOXQty as decimal(10,2)) as Qty,cast(PCSQty as decimal(10,2)) as QtyPcs,Slab from [ax].[ACXSALESLINE] 
                                    Where SO_NO in ('" + sono + "') and SchemeCode<>'' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                        DataTable dtScheme = baseObj.GetData(strQuery);

                        if (dtScheme.Rows.Count > 0)
                        {
                            for (int i = 0; i <= dtScheme.Rows.Count - 1; i++)
                            {
                                GetSelectedShemeItemChecked(dtScheme.Rows[i]["SchemeCode"].ToString(), dtScheme.Rows[i]["ItemCode"].ToString(), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["Qty"].ToString())), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["Slab"].ToString())), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["QtyPcs"].ToString())));

                            }
                        }
                        if (intApplicable == 1 || intApplicable == 3)
                        {
                            this.SchemeDiscCalculation();
                            DataTable dt1 = new DataTable();
                            if (Session["InvLineItem"] == null)
                            {
                                AddColumnInDataTable();
                            }
                            else
                            {
                                dt1 = (DataTable)Session["InvLineItem"];
                            }
                           
                            GridViewFooterCalculate(dt1);
                        }
                    }
                    #endregion
                }
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }
        private void ShowData(string sono)
        {
            DataTable dt = new DataTable();
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

            string query = " select A.SO_No,CONVERT(VARCHAR(11),A.SO_Date,106) as SO_Date ,A.Loadsheet_No,CONVERT(VARCHAR(11),L.LoadSheet_Date,106) as LoadSheet_Date, " +
                       "B.CUST_GROUP,Customer_Group=(Select C.CUSTGROUP_NAME from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=B.CUST_GROUP)," +
                       "B.Customer_Code,concat(B.Customer_Code,'-', B.Customer_Name) as Customer_Name," +
                       " B.Address1,B.Mobile_No,B.VAT,B.GSTINNO,B.GSTREGISTRATIONDATE,B.COMPOSITIONSCHEME,B.STATE  " +
                       " from [ax].[ACXSALESHEADER] A left join ax.Acxcustmaster B on A.Customer_Code=B.Customer_Code  " +
                       "  left join ax.ACXLOADSHEETHEADER L on L.LoadSheet_No=A.LoadSheet_No and L.Siteid=A.siteid " +
                //  " where A.[Siteid]='" + lblsite.Text + "' and A.SO_NO in (" + sono + " )";
                       " where A.[Siteid]='" + lblsite.Text + "' and A.SO_NO in (select id from [dbo].[CommaDelimitedToTable]('" + sono + "' ,','))";

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
                chkCompositionScheme.Checked = Convert.ToBoolean(dt.Rows[0]["COMPOSITIONSCHEME"]);
                txtBillToState.Text = dt.Rows[0]["STATE"].ToString();

                query = "EXEC USP_CUSTSHIPTOADDRESS '" + drpCustomerCode.SelectedValue + "'";
                ddlShipToAddress.Items.Clear();
                baseObj.BindToDropDown(ddlShipToAddress, query, "SHIPTOADDRESS", "SHIPTOADDRESS");
                
                /*----------------GST Implementation End-------------------------- */
            }
            else
            {
                txtGSTtin.Text = "";
                txtGSTtinRegistration.Text = "";          
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
            } 
            #region 
            
            //query = "select GSTINNO,GSTREGISTRATIONDATE,COMPOSITIONSCHEME from VW_CUSTOMERGSTINFO where CUSTOMER_CODE='" + drpCustomerCode.SelectedValue + "'";           
            //DataTable dt1 = baseObj.GetData(query);
            //if (dt1.Rows.Count > 0)
            //{
            //    txtGSTtin.Text = dt1.Rows[0]["GSTINNO"].ToString();
            //    txtGSTtinRegistration.Text = dt1.Rows[0]["GSTREGISTRATIONDATE"].ToString();
            //    chkCompositionScheme.Checked = Convert.ToBoolean(dt1.Rows[0]["COMPOSITIONSCHEME"]);                
            //}
            //else
            //{
            //    txtGSTtin.Text = "";
            //    txtGSTtinRegistration.Text = "";                
            //}

            
            #endregion
            
            conn = baseObj.GetConnection();
            cmd2 = new SqlCommand("AX.ACX_SOTOINVOICECREATION");
            //transaction = conn.BeginTransaction();
            cmd2.Connection = conn;
            //cmd2.Transaction = transaction;
            cmd2.CommandTimeout = 0;
            cmd2.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            // dt = obj.GetData(query);
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@TransLocation", Session["TransLocation"].ToString());
            string s = Session["TransLocation"].ToString();
            cmd2.Parameters.AddWithValue("@siteid", lblsite.Text);
            cmd2.Parameters.AddWithValue("@so_no", sono);
            dt.Load(cmd2.ExecuteReader());

            //transaction.Commit();

            if (conn != null)
            {
                if (conn.State.ToString() == "Open")
                {
                    conn.Close();
                    conn.Dispose();
                }
            }


            //dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                Session["InvLineItem"] = dt;
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                GridViewFooterCalculate(dt);
            }
            else
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
            }
            TDApplicable();
        }
        private void TDApplicable()
        {
            string strTD = "select TDPEAPPLICABLE from [ax].[ACXCUSTGROUPMASTER] Where CustGroup_Code='" + drpCustomerGroup.SelectedItem.Value + "'";
            DataTable dtTDApplicable = new DataTable();
            dtTDApplicable = baseObj.GetData(strTD);
            if (dtTDApplicable != null)
            {
                if (dtTDApplicable.Rows[0]["TDPEAPPLICABLE"].ToString() == "1")
                {
                    //gvDetails.Columns[11].Visible = false;
                    txtTDValue.Enabled = true;
                    btnApply.Enabled = true;
                }
                else
                {
                    //gvDetails.Columns[11].Visible = true;
                    txtTDValue.Enabled = false;
                    btnApply.Enabled = false;
                }
            }
        }
        protected void txtRate_TextChanged(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //TextBox txtRate = (TextBox)sender;
            //GridViewRow row = (GridViewRow)txtRate.Parent.Parent;
            //decimal SchemeDiscVal = 0;
            //int idx = row.RowIndex;
            //TextBox qty = (TextBox)row.Cells[0].FindControl("txtBox");
            //Label soqty = (Label)row.Cells[0].FindControl("SO_Qty");
            //Label amt = (Label)row.Cells[0].FindControl("Amount");
            //Label tax = (Label)row.Cells[0].FindControl("Tax");
            //Label taxvalue = (Label)row.Cells[0].FindControl("TaxValue");
            //Label lblSchemeDiscVal = (Label)row.Cells[0].FindControl("lblSchemeDiscVal");
            //if (lblSchemeDiscVal.Text.Trim().Length > 0)
            //{
            //    SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
            //}

            //if (Convert.ToDecimal(txtRate.Text) <= 0)
            //{

            //    return;
            //}
            //if (qty.Text == "")
            //{
            //    decimal taxValue = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(soqty.Text) * (Convert.ToDecimal(tax.Text) / 100));
            //    decimal amount = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(soqty.Text)) + taxValue;

            //    taxvalue.Text = taxValue.ToString("#.##");
            //    amt.Text = amount.ToString("#.##");

            //}
            //else
            //{
            //    decimal taxValue = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(qty.Text) * (Convert.ToDecimal(tax.Text) / 100));
            //    decimal amount = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(qty.Text)) + taxValue;

            //    // decimal amount = Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(qty.Text);
            //    taxvalue.Text = taxValue.ToString("#.##");
            //    amt.Text = amount.ToString("#.##");
            //}
            //UpdateSession();
        }
        protected void txtBox_TextChanged(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            TextBox txtB = (TextBox)sender;
            decimal SchemeDiscVal = 0;
            GridViewRow row = (GridViewRow)txtB.Parent.Parent;
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            string query;
            int idx = row.RowIndex;

            // Label amt = (Label)row.Cells[0].FindControl("Amount");

            HiddenField ddlPrCode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
            HiddenField discType = (HiddenField)row.FindControl("hDiscType");
            HiddenField discCalculation = (HiddenField)row.FindControl("hDiscCalculationType");
            HiddenField hClaimDiscAmt = (HiddenField)row.FindControl("hClaimDiscAmt");
            Label lblLtr = (Label)row.FindControl("LTR");
            Label amount = (Label)row.FindControl("Amount");
            TextBox txtRate = (TextBox)row.FindControl("txtRate");
            Label tax = (Label)row.FindControl("Tax");
            Label taxvalue = (Label)row.FindControl("TaxValue");
            Label AddtaxPer = (Label)row.FindControl("AddTax");
            Label AddTaxValue = (Label)row.FindControl("AddTaxValue");
            Label StockQty = (Label)row.FindControl("StockQty");
            Label SO_Qty = (Label)row.FindControl("SO_Qty");
            Label DiscPer = (Label)row.FindControl("Disc");
            Label DiscVal = (Label)row.FindControl("DiscValue");
            Label lblMRP = (Label)row.FindControl("MRP");
            Label lblTotal = (Label)row.FindControl("Total");
            TextBox SDiscPer = (TextBox)row.FindControl("SecDisc");
            TextBox SDiscVal = (TextBox)row.FindControl("SecDiscValue");
            boolDiscAvalMRP = false;
            TextBox txtBoxQtyGrid = (TextBox)row.FindControl("txtBoxQtyGrid");
            TextBox txtPcsQtyGrid = (TextBox)row.FindControl("txtPcsQtyGrid");
            TextBox txtBox = (TextBox)row.FindControl("txtBox");

            Label txtBoxPcs = (Label)row.FindControl("txtBoxPcs");

            HiddenField hdnTaxableammount = (HiddenField)row.FindControl("hdnTaxableAmount");
            Label lblPack = (Label)row.FindControl("Pack");
            Label lblSchemeDiscVal = (Label)row.FindControl("lblSchemeDiscVal");
            if (lblSchemeDiscVal.Text.Trim().Length > 0)
            {
                SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
            }

            if ((Convert.ToDecimal(txtBoxQtyGrid.Text) + Convert.ToDecimal(txtPcsQtyGrid.Text)) <= 0)
            {
                lblLtr.Text = "0.00";
                taxvalue.Text = "0.00";
                amount.Text = "0.00";
                lblTotal.Text = "0.00";
                AddTaxValue.Text = "0.00";
                DiscVal.Text = "0.00";
                SDiscPer.Text = "0.00";
                SDiscVal.Text = "0.00";
                txtBoxPcs.Text = "0.00";
                txtBox.Text = "0.00";
                hdnTaxableammount.Value = "0.00";
                row.BackColor = System.Drawing.Color.White;
            }
            else
            {
                if (Convert.ToDecimal(txtPcsQtyGrid.Text) > Convert.ToDecimal(lblPack.Text))
                {
                    int pcs = Convert.ToInt16(Convert.ToDecimal(txtPcsQtyGrid.Text));// Convert.ToInt16(txtPcsQtyGrid.Text); Math.Floor(pcsQty / packSize)
                    int pac = Convert.ToInt16(Convert.ToDecimal(lblPack.Text));
                    int addbox = pcs / pac;//Convert.ToInt16(Convert.ToDecimal(lblPack.Text));
                    int remainder = pcs % pac;
                    if (remainder == 0)
                    {
                        txtPcsQtyGrid.Text = "0";
                    }
                    else
                    {
                        txtPcsQtyGrid.Text = Convert.ToString(remainder);
                    }
                    txtBoxQtyGrid.Text = Convert.ToString(Convert.ToInt16(Convert.ToDouble(txtBoxQtyGrid.Text)) + addbox);
                }
                int BoxQty = (txtBoxQtyGrid.Text.Trim() == "" ? 0 : Convert.ToInt16(Convert.ToDouble(txtBoxQtyGrid.Text)));
                //txtPcsQtyGrid.Text = (txtPcsQtyGrid.Text == "0.00" ? "0" : txtPcsQtyGrid.Text);
                txtPcsQtyGrid.Text = (Convert.ToInt16(Convert.ToDouble(txtPcsQtyGrid.Text)) == 0 ? "0" : txtPcsQtyGrid.Text);
                string BoxPcs = Convert.ToString(BoxQty) + '.' + (txtPcsQtyGrid.Text.Length == 1 ? "0" : "") + Convert.ToString(txtPcsQtyGrid.Text);
                txtBoxPcs.Text = BoxPcs;
                decimal SecDiscPer = Convert.ToDecimal(SDiscPer.Text);
                decimal SecDiscValue = Convert.ToDecimal(SDiscVal.Text);
                decimal decTotAmount = 0;
                if (Convert.ToDecimal(txtB.Text) > Convert.ToDecimal(StockQty.Text))
                {

                    row.BackColor = System.Drawing.Color.Red;
                    txtB.Text = SO_Qty.Text;
                    

                    string message = "alert('Please Enter a valid number or input quantity less than stock quantity!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    txtBoxQtyGrid.Text = Convert.ToInt16(Convert.ToDecimal(SO_Qty.Text)).ToString();
                    decimal PcsQty, stPack, stSOQty;
                    PcsQty = stPack = stSOQty = 0;
                    stPack = Convert.ToInt16(Convert.ToDecimal(lblPack.Text));
                    stSOQty = Convert.ToInt16(Convert.ToDecimal(SO_Qty.Text));
                    PcsQty = (stSOQty - Convert.ToDecimal(SO_Qty.Text)) * stPack;
                    if (PcsQty < 0)
                    {
                        txtBoxQtyGrid.Text = Convert.ToInt16(Convert.ToDecimal(SO_Qty.Text) - 1).ToString();
                        PcsQty = PcsQty * -1;
                    }

                    txtPcsQtyGrid.Text = PcsQty.ToString();
                }
                else
                {
                    row.BackColor = System.Drawing.Color.White;
                }

                if (Convert.ToDecimal(txtB.Text) > 0)
                {
                    if (SecDiscPer != 0)
                    {
                        SecDiscValue = (SecDiscPer * (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text)) / 100);
                    }
                    DataTable dtscheme = new DataTable();
                    dtscheme = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + ddlPrCode.Value.ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtB.Text) + "," + Convert.ToDecimal(txtRate.Text) + ",'" + Session["SITELOCATION"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    if (dtscheme.Rows.Count > 0)
                    {
                        if (dtscheme.Rows[0]["RETMSG"].ToString().IndexOf("TRUE") >= 0)
                        {
                            DiscVal.Text = dtscheme.Rows[0]["DISCVAL"].ToString();
                            hClaimDiscAmt.Value = dtscheme.Rows[0]["CLAIMDISCAMT"].ToString();
                        }
                    }
                    //if (discType.Value == "0")  // Percentage
                    //{
                    //    DiscVal.Text = (Convert.ToDecimal(DiscPer.Text) * Convert.ToDecimal(txtB.Text) * Convert.ToDecimal(txtRate.Text) / 100).ToString("0.00");
                    //}
                }
                //if (discType.Value == "1") // value
                //{
                //    DiscVal.Text = (Convert.ToDecimal(DiscPer.Text) * Convert.ToDecimal(txtB.Text)).ToString("0.00");
                //    decimal discVal = 0;
                //    decimal tax1 = 0, tax2 = 0;
                //    //dt = new DataTable();
                //    //dt = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ddlPrCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                //    //if (txtBillToState.Text == "JK")
                //    //{
                //    //    if (dt.Rows.Count > 0)
                //    //    {                           
                //    //        tax1 = Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString());
                //    //        tax2 = Convert.ToDecimal(dt.Rows[0]["ACXADDITIONALTAXVALUE"].ToString());
                //    //        if (Convert.ToInt16(dt.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
                //    //        {
                //    //            discVal = Convert.ToDecimal(DiscVal.Text) / (1 + (tax1 + (tax1 * tax2) / 100) / 100);
                //    //        }
                //    //        else  // Tax On Taxable Amount
                //    //        {
                //    //            discVal = Convert.ToDecimal(DiscVal.Text) / (1 + ((tax1 + tax2) / 100));
                //    //        }
                //    //        DiscVal.Text = Convert.ToString(discVal.ToString("0.00"));
                //    //    }
                //    //}
                //    //else
                //    //{
                //    discVal = Convert.ToDecimal(DiscVal.Text) / (1 + ((Convert.ToDecimal(tax.Text) + Convert.ToDecimal(AddtaxPer.Text)) / 100));
                //    DiscVal.Text = Convert.ToString(discVal.ToString("0.00"));
                //    //}
                //}
                
                query = "select  CEILING(" + txtB.Text + "/(CASE WHEN ISNULL(A.Product_Crate_PackSize,0)=0 THEN 1 ELSE A.Product_Crate_PackSize END)) as Crates,(" + txtB.Text + "*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.InventTable A where A.ItemId='" + ddlPrCode.Value + "'";

                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    
                    decimal l;
                    //decimal rate, amt;                   
                    l = Convert.ToDecimal(dt.Rows[0]["Ltr"].ToString());
                    lblLtr.Text = l.ToString("#.##");
                    decimal taxV = 0;
                    //if (boolDiscAvalMRP == true)
                    //{ taxV = ((decTotAmount - Convert.ToDecimal(DiscVal.Text) - SecDiscValue) * (Convert.ToDecimal(tax.Text) / 100)); }
                    //else
                    { taxV = ((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal) * (Convert.ToDecimal(tax.Text) / 100)); }
                    DataTable dtTax = new DataTable();
                    decimal AddtaxV = 0;
                    //if (txtBillToState.Text == "JK")
                    //{
                    //    dtTax = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ddlPrCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                    //    if (dtTax.Rows.Count > 0)
                    //    {
                    //        if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
                    //        {
                    //            AddtaxV = (taxV * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //        }
                    //        else if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 1) // Tax On Taxable Amount
                    //        {
                    //            if (boolDiscAvalMRP == true)
                    //            {
                    //                AddtaxV = ((decTotAmount - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal) * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //            }
                    //            else
                    //            {
                    //                AddtaxV = ((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal) * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //            }

                    //        }
                    //        else if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 2) // Tax On MRP
                    //        {
                    //            AddtaxV = ((Convert.ToDecimal(lblMRP.Text) * Convert.ToDecimal(txtB.Text) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal) * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //        }
                    //        else
                    //        {
                    //            AddtaxV = ((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal) * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //        }
                    //    }                        
                    //}
                    //else
                    //{
                    AddtaxV = ((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal) * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //}
                    decimal amt = 0;
                    //if (boolDiscAvalMRP == true)
                    //{  amt = (decTotAmount - Convert.ToDecimal(DiscVal.Text) - SecDiscValue + taxV + AddtaxV);}
                    //else
                    { amt = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text)) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal + taxV + AddtaxV; }
                    hdnTaxableammount.Value = ((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text)) - Convert.ToDecimal(DiscVal.Text) - SecDiscValue - SchemeDiscVal).ToString();
                    taxvalue.Text = taxV.ToString("#.##");
                    AddTaxValue.Text = AddtaxV.ToString("#.##");
                    amount.Text = amt.ToString("#.##");
                    lblTotal.Text = amt.ToString("#.##");
                    //SDiscVal.Text = SecDiscValue.ToString("#.##");
                    SDiscVal.Text = String.Format("{0:0.00}", SecDiscValue); //SecDiscValue.ToString("#.##");
                    SDiscPer.Text = String.Format("{0:0.00}", SecDiscPer); //SecDiscPer.ToString("#.##");
                    //        }
                }
                else
                {
                    lblLtr.Text = "0.00";
                    taxvalue.Text = "0.00";
                    amount.Text = "0.00";
                    lblTotal.Text = "0.00";
                    AddTaxValue.Text = "0.00";
                    DiscVal.Text = "0.00";
                    SDiscPer.Text = "0.00";
                    SDiscVal.Text = "0.00";
                    hdnTaxableammount.Value = "0.00";
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                    string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
            }
            //have to update session 
            TDCalculation();
            GrandTotal();
            UpdateSession();
            // For Shceme Seletion
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
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
            DataTable dt1 = new DataTable();
            if (Session["InvLineItem"] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dt1 = (DataTable)Session["InvLineItem"];
            }
            
            GridViewFooterCalculate(dt1);
            // BindSchemeGrid();
        }

        //protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvDetails.PageIndex = e.NewPageIndex;
        //    BindGridview();
        //}

        public void GrandTotal()
        {
            //==================All gridview data into a datatable==========
            DataTable dtLine = new DataTable();

            dtLine.Columns.Add("SO_Qty", typeof(decimal));
            dtLine.Columns.Add("Invoice_Qty", typeof(decimal));
            dtLine.Columns.Add("Box_Qty", typeof(decimal));
            dtLine.Columns.Add("Pcs_Qty", typeof(decimal));
            dtLine.Columns.Add("LTR", typeof(decimal));
            dtLine.Columns.Add("DiscVal", typeof(decimal));
            dtLine.Columns.Add("TaxValue", typeof(decimal));
            dtLine.Columns.Add("ADDTaxValue", typeof(decimal));
            dtLine.Columns.Add("Amount", typeof(decimal));
            dtLine.Columns.Add("TD", typeof(decimal));
            dtLine.Columns.Add("Total", typeof(decimal));
            dtLine.Columns.Add("SecDiscAmount", typeof(decimal));

            foreach (GridViewRow gvr in gvDetails.Rows)
            {
                Label SO_Qty = (Label)gvr.FindControl("SO_Qty");
                TextBox Invoice_Qty = (TextBox)gvr.FindControl("txtBox");
                TextBox Box_Qty = (TextBox)gvr.FindControl("txtBoxQtyGrid");
                TextBox Pcs_Qty = (TextBox)gvr.FindControl("txtPcsQtyGrid");
                Label Ltr = (Label)gvr.FindControl("LTR");
                Label DiscVal = (Label)gvr.FindControl("DiscValue");
                Label taxvalue = (Label)gvr.FindControl("TaxValue");
                Label AddTaxValue = (Label)gvr.FindControl("AddTaxValue");
                Label amount = (Label)gvr.FindControl("Amount");
                Label lblTD = (Label)gvr.FindControl("TD");
                Label lblTotal = (Label)gvr.FindControl("Total");
                TextBox SecDiscVal = (TextBox)gvr.FindControl("SecDiscValue");
                if (Invoice_Qty.Text == null || Invoice_Qty.Text == string.Empty)
                {
                    Invoice_Qty.Text = "0.00";
                }
                if (Box_Qty.Text == null || Box_Qty.Text == string.Empty)
                {
                    Box_Qty.Text = "0.00";
                }
                if (Pcs_Qty.Text == null || Pcs_Qty.Text == string.Empty)
                {
                    Pcs_Qty.Text = "0.00";
                }
                if (AddTaxValue.Text == string.Empty)
                {
                    AddTaxValue.Text = "0.00";
                }
                if (DiscVal.Text == string.Empty)
                {
                    DiscVal.Text = "0.00";
                }
                if (taxvalue.Text == "")
                {
                    taxvalue.Text = "0.00";
                }
                if (Ltr.Text == "")
                {
                    Ltr.Text = "0";
                }
                if (SO_Qty.Text == "")
                {
                    SO_Qty.Text = "0";
                }
                if (amount.Text == "")
                {
                    amount.Text = "0.00";
                }
                if (lblTD.Text == string.Empty)
                {
                    lblTD.Text = "0.00";
                }
                if (lblTotal.Text == string.Empty)
                {
                    lblTotal.Text = "0.00";
                }
                if (SecDiscVal.Text == string.Empty)
                {
                    SecDiscVal.Text = "0.00";
                }
                DataRow r;
                r = dtLine.NewRow();
                r["So_Qty"] = Convert.ToDecimal(SO_Qty.Text);
                r["Invoice_Qty"] = Convert.ToDecimal(Invoice_Qty.Text);
                r["Box_Qty"] = Convert.ToDecimal(Box_Qty.Text);
                r["Pcs_Qty"] = Convert.ToDecimal(Pcs_Qty.Text);
                r["Ltr"] = Convert.ToDecimal(Ltr.Text);
                r["DiscVal"] = Convert.ToDecimal(DiscVal.Text);
                r["TaxValue"] = Convert.ToDecimal(taxvalue.Text);
                r["ADDTaxValue"] = Convert.ToDecimal(AddTaxValue.Text);
                r["Amount"] = Convert.ToDecimal(amount.Text);
                r["TD"] = Convert.ToDecimal(lblTD.Text);
                r["Total"] = Convert.ToDecimal(lblTotal.Text);
                r["SecDiscAmount"] = Convert.ToDecimal(SecDiscVal.Text);
                dtLine.Rows.Add(r);

            }
            GridViewFooterCalculate(dtLine);
            

        }
        protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label StockQty = (Label)e.Row.FindControl("StockQty");
                TextBox BoxQty = (TextBox)e.Row.FindControl("txtBox");
                if (Convert.ToDecimal(BoxQty.Text) > Convert.ToDecimal(StockQty.Text))
                {
                    e.Row.BackColor = System.Drawing.Color.Tomato;
                }
                // Pcs Billing Applicable
                HiddenField hdnPrCode = (HiddenField)e.Row.FindControl("HiddenField2");
                TextBox PcsQty = (TextBox)e.Row.FindControl("txtPcsQtyGrid");
                DataTable dt = baseObj.GetData("Select Product_Group from ax.InventTable where ItemId='" + hdnPrCode.Value + "'");
                if (dt.Rows.Count > 0)
                {
                    dt = baseObj.GetPcsBillingApplicability(Session["SCHSTATE"].ToString(), dt.Rows[0][0].ToString());
                    string ProductGroupApplicable = string.Empty;

                    if (dt.Rows.Count > 0)
                    {
                        ProductGroupApplicable = dt.Rows[0][1].ToString();
                    }
                    if (ProductGroupApplicable == "Y")
                    {
                        PcsQty.Enabled = true;
                    }
                    else
                    {
                        PcsQty.Enabled = false;
                    }
                }
            }
        }
        protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        public bool ValidateSchemeQty1(string SelectedShemeCode)
        {
            bool returnType = true;
            if (gvScheme.Rows.Count > 0)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("SchemeCode", typeof(string));
                dt.Columns.Add("Slab", typeof(int));
                dt.Columns.Add("FreeQty", typeof(int));
                dt.Columns.Add("FreeQtyPcs", typeof(int));
                dt.Columns.Add("SetNO", typeof(int));

                DataRow dr = null;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    dr = dt.NewRow();
                    dr["SchemeCode"] = rw.Cells[1].Text;
                    dr["Slab"] = Convert.ToInt16(rw.Cells[6].Text);
                    dr["SetNo"] = Convert.ToInt16(rw.Cells[7].Text);
                    if (rw.Cells[8].Text == "&nbsp;")
                    { rw.Cells[8].Text = ""; }
                    if (rw.Cells[11].Text == "&nbsp;")
                    { rw.Cells[11].Text = ""; }
                    if (rw.Cells[8].Text != "")
                    {
                        dr["FreeQty"] = Convert.ToInt16(Convert.ToInt16(rw.Cells[8].Text));
                        dr["FreeQtyPcs"] = 0;
                    }
                    if (Convert.ToString(rw.Cells[11].Text) != "")
                    {
                        dr["FreeQtyPcs"] = Convert.ToInt16(Convert.ToInt16(rw.Cells[11].Text));
                        dr["FreeQty"] = 0;
                    }

                    dt.Rows.Add(dr);

                }
                DataView dv = new DataView(dt);
                DataTable distinctTableValue = (DataTable)dv.ToTable(true, "SchemeCode", "Slab", "FreeQty", "FreeQtyPcs", "SetNo");

                DataView dv1 = new DataView(distinctTableValue);
                dv1.RowFilter = "[SchemeCode]='" + SelectedShemeCode + "'";
                if (dv1.Count > 0)
                {

                    decimal TotalQty = 0, TotalQtyPcs = 0;
                    decimal Slab = 0;
                    decimal FreeQty = 0;
                    decimal FreeQtyPcs = 0;
                    decimal SetNo = 0;
                    string SchemeCode = string.Empty;
                    foreach (DataRowView drow in dv1)
                    {

                        TotalQty = 0;
                        TotalQtyPcs = 0;
                        SchemeCode = drow["SchemeCode"].ToString();
                        Slab = Convert.ToInt16(drow["Slab"]);
                        FreeQty = Convert.ToInt16(drow["FreeQty"]);
                        FreeQtyPcs = Convert.ToInt16(drow["FreeQtyPcs"]);

                        foreach (GridViewRow rw in gvScheme.Rows)
                        {
                            //HiddenField hdnCalculationOn = (HiddenField)rw.FindControl("hdnCalculationOn");                                
                            CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                            if (chkBx.Checked == true)
                            {
                                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                                TextBox txtQtyPcs = (TextBox)rw.FindControl("txtQtyPcs");
                                txtQty.Text = (txtQty.Text.Trim().Length == 0 ? "0" : txtQty.Text);
                                txtQtyPcs.Text = (txtQtyPcs.Text.Trim().Length == 0 ? "0" : txtQtyPcs.Text);

                                if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab && Convert.ToInt16(rw.Cells[7].Text) == SetNo)
                                {
                                    TotalQty += Convert.ToInt16(txtQty.Text);
                                }
                                if (!string.IsNullOrEmpty(txtQtyPcs.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab && Convert.ToInt16(rw.Cells[7].Text) == SetNo)
                                {
                                    TotalQtyPcs += Convert.ToInt16(txtQtyPcs.Text);
                                }
                            }
                        }

                        if (TotalQty != FreeQty && Slab == Convert.ToInt16(drow["Slab"]) && SetNo == Convert.ToInt16(drow["SetNo"]) && TotalQtyPcs != FreeQtyPcs)
                        {
                            returnType = false;
                        }

                    }
                }
                else
                {
                    returnType = false;
                }
            }
            return returnType;

        }
        public bool ValidateSchemeQtyNew()
        {
            bool returnType = true;
            if (gvScheme.Rows.Count > 0)
            {

                DataTable dt = new DataTable();
                dt.Columns.Add("SchemeCode", typeof(string));
                dt.Columns.Add("Slab", typeof(int));
                dt.Columns.Add("FreeQty", typeof(int));

                DataRow dr = null;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    dr = dt.NewRow();
                    dr["SchemeCode"] = rw.Cells[1].Text;
                    dr["Slab"] = Convert.ToInt16(rw.Cells[6].Text);
                    dr["FreeQty"] = Convert.ToInt16(Convert.ToInt16(rw.Cells[7].Text));

                    dt.Rows.Add(dr);

                }
                DataView dv = new DataView(dt);
                DataTable distinctTableValue = (DataTable)dv.ToTable(true, "SchemeCode", "Slab", "FreeQty");
                decimal TotalQty = 0;
                decimal Slab = 0;
                decimal FreeQty = 0;
                string SchemeCode = string.Empty;
                foreach (DataRow drow in distinctTableValue.Rows)
                {
                    TotalQty = 0;
                    SchemeCode = drow["SchemeCode"].ToString();
                    Slab = Convert.ToInt16(drow["Slab"]);
                    FreeQty = Convert.ToInt16(drow["FreeQty"]);

                    foreach (GridViewRow rw in gvScheme.Rows)
                    {

                        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                        if (chkBx.Checked == true)
                        {

                            TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                            if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt16(rw.Cells[6].Text) == Slab)
                            {
                                TotalQty += Convert.ToInt16(txtQty.Text);
                            }

                        }
                    }


                    if (TotalQty != FreeQty && Slab == Convert.ToInt16(drow["Slab"]))
                    {
                        returnType = false;

                    }

                }
            }
            return returnType;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {                
                if (drpSONumber.Text=="")
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

                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
                {
                    IsSchemeValidate = baseObj.ValidateSchemeQty(SelectedSchemeCode, ref gvScheme); // ValidateSchemeQty(SelectedSchemeCode);
                }
                else
                {
                    IsSchemeValidate = true;
                }

                if (IsSchemeValidate == true)
                {
                    if (b == true)
                    {
                        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                        if (strmessage == string.Empty)
                        {
                            SaveHeader();                
                            string message = "alert('Invoice No: " + txtInvoiceNo.Text + " Saved Successfully!!'); window.location.href='frmInvoicePrepration.aspx'; ";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);                            
                            ClearAll();
                        }
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
                        dtStock = baseObj.GetData(strStcok);
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
                        using (DataTable dtFreeItem = baseObj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + gv.Cells[4].Text.ToString() + "'"))
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
                        dtStock = baseObj.GetData(strStcok);
                        decimal curstock = 0;
                        if (dtStock.Rows.Count > 0)
                        {
                            curstock =  Convert.ToDecimal(dtStock.Rows[0]["TransQty"].ToString());
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
                dtStock = baseObj.GetData(strSQL);
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

                
                DataTable dt = new DataTable();
                //======Save Header===================
                conn = obj.GetConnection();
                cmd = new SqlCommand("ACX_SaleInvoice_Header");
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
                decimal TDValue = 0;
                if (txtTDValue.Text == string.Empty)
                {
                    TDValue = 0;
                    txtTDValue.Text = "0";
                }
                else
                {
                    TDValue = Global.ConvertToDecimal(txtTDValue.Text);
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
                /* ---------- GST Implementation End -----------*/
                //========Save Line================
     

                int i = 0;
                decimal totamt = 0;

                //===================New===============
                cmd1 = new SqlCommand("ACX_Insert_SaleInvoiceLine");
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 3600;
                cmd1.CommandType = CommandType.StoredProcedure;

                int count = gvDetails.Rows.Count;
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    i = i + 1;

                    HiddenField Tproduct = (HiddenField)gv.FindControl("HiddenField2");
                    Label Amt = (Label)gv.FindControl("Amount");
                    //totamt = totamt + Convert.ToDecimal(Amt.Text);

                    TextBox box = (TextBox)gv.FindControl("txtBox");
                    if (box.Text.Trim().Length == 0) { box.Text = "0"; }
                    Label ltr = (Label)gv.FindControl("ltr");
                    if (ltr.Text.Trim().Length == 0) { ltr.Text = "0"; }
                    Label mrp = (Label)gv.FindControl("MRP");
                    if (mrp.Text.Trim().Length == 0) { mrp.Text = "0"; }
                    TextBox rate = (TextBox)gv.FindControl("txtRate");
                    if (rate.Text.Trim().Length == 0) { rate.Text = "0"; }
                    Label taxvalue = (Label)gv.FindControl("TaxValue");
                    if (taxvalue.Text.Trim().Length == 0) { taxvalue.Text = "0"; }
                    Label taxper = (Label)gv.FindControl("Tax");
                    if (taxper.Text.Trim().Length == 0) { taxper.Text = "0"; }
                    Label AddtaxPer = (Label)gv.FindControl("AddTax");
                    if (AddtaxPer.Text.Trim().Length == 0) { AddtaxPer.Text = "0"; }
                    Label AddTaxValue = (Label)gv.FindControl("AddTaxValue");
                    if (AddTaxValue.Text.Trim().Length == 0) { AddTaxValue.Text = "0"; }

                    //if (AddTaxValue.Text.Trim().Length == 0) { AddTaxValue.Text = "0"; }
                    //if (Global.ConvertToDecimal(txtTDValue.Text) > 0)
                    //{
                    //    taxvalue = (Label)gv.FindControl("VatAfterPE");
                    //    if (taxvalue.Text.Trim().Length == 0) { taxvalue.Text = "0"; }
                    //}
                    //else
                    //{
                        //taxvalue = (Label)gv.FindControl("TaxValue");
                        //if (taxvalue.Text.Trim().Length == 0) { taxvalue.Text = "0"; }
                    //}
                    
                    Label DiscVal = (Label)gv.FindControl("DiscValue");
                    if (DiscVal.Text.Trim().Length == 0) { DiscVal.Text = "0"; }

                    Label Disc = (Label)gv.FindControl("Disc");
                    if (Disc.Text.Trim().Length == 0) { Disc.Text = "0"; }
                    Label SchemeCode = (Label)gv.FindControl("lblScheme");
                    HiddenField hDT = (HiddenField)gv.FindControl("hDiscType");
                    HiddenField hCalculationBase = (HiddenField)gv.FindControl("hDiscCalculationType");
                    HiddenField hClaimDiscAmt = (HiddenField)gv.FindControl("hClaimDiscAmt");
                    HiddenField hTax1 = (HiddenField)gv.FindControl("hTax1");
                    HiddenField hTax2 = (HiddenField)gv.FindControl("hTax2");
                    HiddenField hTax1component = (HiddenField)gv.FindControl("hTax1component");
                    HiddenField hTax2component = (HiddenField)gv.FindControl("hTax2component");
                    //TextBox SDiscPer = (TextBox)gvDetails.Rows[i].FindControl("SecDisc");
                    TextBox SDiscVal = (TextBox)gv.FindControl("SecDiscValue");
                    if (SDiscVal.Text.Trim().Length == 0) { SDiscVal.Text = "0"; }

                    Label lblTD = (Label)gv.FindControl("TD");
                    if (lblTD.Text.Trim().Length == 0) { lblTD.Text = "0"; }
                    Label lblPE = (Label)gv.FindControl("PE");
                    if (lblPE.Text.Trim().Length == 0) { lblPE.Text = "0"; }
                    Label lblTotal = (Label)gv.FindControl("Total");
                    if (lblTotal.Text.Trim().Length == 0) { lblTotal.Text = "0"; }

                    totamt = totamt + Global.ConvertToDecimal(lblTotal.Text);

                    HiddenField hdnTaxableammount = (HiddenField)gv.FindControl("hdnTaxableAmount");
                    HiddenField hdnBasePrice = (HiddenField)gv.FindControl("hdnBasePrice");
                    TextBox txtBoxQtyGrid = (TextBox)gv.FindControl("txtBoxQtyGrid");
                    if (txtBoxQtyGrid.Text.Trim().Length == 0) { txtBoxQtyGrid.Text = "0"; }
                    TextBox txtPcsQtyGrid = (TextBox)gv.FindControl("txtPcsQtyGrid");
                    if (txtPcsQtyGrid.Text.Trim().Length == 0) { txtPcsQtyGrid.Text = "0"; }
                    Label txtBoxPcs = (Label)gv.FindControl("txtBoxPcs");
                    if (txtBoxPcs.Text.Trim().Length == 0) { txtBoxPcs.Text = "0"; }

                    Label lblHSNCODE = (Label)gv.FindControl("lblHSNCODE");
                    Label lblTAXCOMPONENT = (Label)gv.FindControl("lblTAXCOMPONENT");
                    Label lblADDTAXCOMPONENT = (Label)gv.FindControl("lblADDTAXCOMPONENT");
                    Label lblSchemeDiscPer = (Label)gv.FindControl("lblSchemeDiscPer");
                    if (lblSchemeDiscPer.Text.Trim().Length == 0) { lblSchemeDiscPer.Text = "0"; }
                    Label lblSchemeDiscVal = (Label)gv.FindControl("lblSchemeDiscVal");
                    if (lblSchemeDiscVal.Text.Trim().Length == 0) { lblSchemeDiscVal.Text = "0"; }

                    if (lblTD.Text == string.Empty)
                    {
                        lblTD.Text = "0.00";
                    }
                    if (lblPE.Text == string.Empty)
                    {
                        lblPE.Text = "0.00";
                    }

                    if (hDT.Value == "")
                    {
                        hDT.Value = "2";
                    }
                    if (rate.Text == "")
                    {
                        rate.Text = "0";
                    }
                    if (Disc.Text == "")
                    {
                        Disc.Text = "0";
                    }
                    if (DiscVal.Text == "0")
                    {
                        DiscVal.Text = "0";
                    }
                    decimal Tamount, Tbox, Tltr, Tmrp, Trate;//, decFinalAmount = 0;
                    Tamount = Global.ConvertToDecimal(Amt.Text);
                    Tbox = Global.ConvertToDecimal(box.Text);
                    Tltr = Global.ConvertToDecimal(ltr.Text);
                    Tmrp = Global.ConvertToDecimal(mrp.Text);
                    Trate = Global.ConvertToDecimal(rate.Text);
                   
                    decimal LineAmount = 0;
                    //taxvalue.Text = (taxvalue.Text.Trim().Length == 0 ? "0" : taxvalue.Text);
                    //decimal taxamt = Global.ConvertToDecimal(taxvalue.Text);
                    LineAmount = Trate * Tbox;
                    //totamt = totamt + Tamount;
                    if (Tbox > 0)
                    {
                        //cmd1.CommandText = "";
                        cmd1.Parameters.Clear();
                        cmd1.Parameters.AddWithValue("@status", "Insert");
                        cmd1.Parameters.AddWithValue("@SITEID", lblsite.Text);
                        cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd1.Parameters.AddWithValue("@RECID", "");
                        cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", drpCustomerCode.SelectedItem.Value);
                        cmd1.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceNo.Text);
                        cmd1.Parameters.AddWithValue("@LINE_NO", i);
                        cmd1.Parameters.AddWithValue("@PRODUCT_CODE", Tproduct.Value);
                        cmd1.Parameters.AddWithValue("@PRODUCTGROUP_CODE", "");
                        cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(lblTotal.Text.Trim()));
                        cmd1.Parameters.AddWithValue("@BOX", Convert.ToDecimal(Tbox.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@CRATES", 0);
                        cmd1.Parameters.AddWithValue("@LTR", Convert.ToDecimal(Tltr.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@QUANTITY", 0);
                        cmd1.Parameters.AddWithValue("@MRP", Convert.ToDecimal(Tmrp.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(Trate.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@TAX_CODE", Convert.ToDecimal(taxper.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@TAX_AMOUNT", Convert.ToDecimal(taxvalue.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@ADDTAX_CODE", Convert.ToDecimal(AddtaxPer.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@ADDTAX_AMOUNT", Convert.ToDecimal(AddTaxValue.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@DISC_AMOUNT", Convert.ToDecimal(DiscVal.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@SEC_DISC_AMOUNT", Convert.ToDecimal(SDiscVal.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@TranType", 1);
                        cmd1.Parameters.AddWithValue("@DiscType", hDT.Value.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@Disc", Convert.ToDecimal(Disc.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@SchemeCode", SchemeCode.Text.ToString().Trim());
                        //cmd1.Parameters.AddWithValue("@SchemeCode", "");
                        cmd1.Parameters.AddWithValue("@LINEAMOUNT", Convert.ToDecimal(LineAmount.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@DiscCalculationBase", hCalculationBase.Value.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@CLAIM_DISC_AMT", hClaimDiscAmt.Value.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@Tax1", hTax1.Value.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@Tax2", hTax2.Value.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@Tax1component", hTax1component.Value.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@Tax2component", hTax2component.Value.ToString().Trim());

                        cmd1.Parameters.AddWithValue("@TDValue", Convert.ToDecimal(lblTD.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@PEValue", Convert.ToDecimal(lblPE.Text.ToString().Trim()));

                        cmd1.Parameters.AddWithValue("@BasePrice", Convert.ToDecimal(hdnBasePrice.Value.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@BOXQty", Convert.ToDecimal(txtBoxQtyGrid.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@PcsQty", Convert.ToDecimal(txtPcsQtyGrid.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@BOXPcs", Convert.ToDecimal(txtBoxPcs.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@TaxableAmount", Math.Round(Convert.ToDecimal(hdnTaxableammount.Value),2));
                                              
                        /*-------- GST IMPLEMENTATION START --------*/
                        cmd1.Parameters.AddWithValue("@HSNCODE", lblHSNCODE.Text.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                        cmd1.Parameters.AddWithValue("@TAXCOMPONENT", lblTAXCOMPONENT.Text.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", lblADDTAXCOMPONENT.Text.ToString().Trim());
                        cmd1.Parameters.AddWithValue("@DOCTYPE", 6);
                        cmd1.Parameters.AddWithValue("@SchemeDiscPer", Convert.ToDecimal(lblSchemeDiscPer.Text.ToString().Trim()));
                        cmd1.Parameters.AddWithValue("@SchemeDiscVal", Convert.ToDecimal(lblSchemeDiscVal.Text.ToString().Trim()));
                        /*-------- GST IMPLEMENTATION END ---------*/                        
                        cmd1.ExecuteNonQuery();
                    }
                }
                
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
                int TransType = 2;//1 for Slae invoice 
                int DocumentType = 2;
                string DocumentNo = txtInvoiceNo.Text;
                string DocumentDate = txtInvoiceDate.Text;
                string uom = "BOX";
                string Referencedocumentno = drpSONumber.SelectedItem.Text;
                string TransLocation = Session["TransLocation"].ToString();

                int l = 0;

                //============Loop For LineItem==========
                for (int k = 0; k < gvDetails.Rows.Count; k++)
                {
                    Label Product = (Label)gvDetails.Rows[k].Cells[2].FindControl("Product");
                    TextBox box = (TextBox)gvDetails.Rows[k].Cells[6].FindControl("txtBox");

                    string productNameCode = Product.Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    string Qty = box.Text;
                    decimal TransQty = Global.ConvertToDecimal(Qty);
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
                        cmd2.Parameters.AddWithValue("@TransQty", -TransQty);
                        cmd2.Parameters.AddWithValue("@uom", uom);
                        cmd2.Parameters.AddWithValue("@TransLocation", TransLocation);
                        cmd2.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);
                        cmd2.Parameters.AddWithValue("@TransLineNo", l);
                        cmd2.ExecuteNonQuery();
                        l += 1;
                    }

                }                
                //============Loop For Scheme LineItem==========
                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        //  i = i + 1;
                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        TextBox txtQtyToAvailPcs = (TextBox)gv.FindControl("txtQtyPcs");
                        txtQtyToAvail.Text = (txtQtyToAvail.Text.Trim().Length == 0 ? "0" : txtQtyToAvail.Text);
                        txtQtyToAvailPcs.Text = (txtQtyToAvailPcs.Text.Trim().Length == 0 ? "0" : txtQtyToAvailPcs.Text);
                        
                        
                        //TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");                        
                        string Sproduct = gv.Cells[4].Text;                        
                       
                        decimal packSize = 1, boxqty = 0, pcsQty = 0;
                        decimal billQty = 0;
                        string boxPcs = "";

                        using (DataTable dtFreeItem = baseObj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + gv.Cells[4].Text.ToString() + "'"))
                        {
                            if (dtFreeItem.Rows.Count > 0)
                            {
                                string strPack = dtFreeItem.Rows[0]["Product_PackSize"].ToString();
                                packSize = Global.ConvertToDecimal(strPack);

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

                       // boxqty = (pcsQty >= packSize ? Convert.ToInt32(pcsQty / packSize) : 0);
                        boxqty = (pcsQty >= packSize ? Convert.ToInt32(Math.Floor(pcsQty / packSize)) : 0);
                        pcsQty = Convert.ToInt32(txtQtyToAvailPcs.Text) - (boxqty * packSize);

                        boxqty = boxqty + Convert.ToInt32(txtQtyToAvail.Text);
                        billQty = Math.Round((boxqty + (pcsQty / packSize)), 2);// Math.Round(Convert.ToDecimal((boxqty + (pcsQty / packSize))), 2);
                        boxPcs = boxqty.ToString() + "." + (pcsQty.ToString().Length == 1 ? "0" : "") + pcsQty.ToString();


                        decimal TransQty = billQty;

                       // totamt = 0;

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
                            cmd2.Parameters.AddWithValue("@ProductCode", Sproduct);
                            cmd2.Parameters.AddWithValue("@TransQty", -TransQty);
                            cmd2.Parameters.AddWithValue("@uom", uom);
                            cmd2.Parameters.AddWithValue("@TransLocation", TransLocation);
                            cmd2.Parameters.AddWithValue("@Referencedocumentno", Referencedocumentno);
                            cmd2.Parameters.AddWithValue("@TransLineNo", l);
                            cmd2.ExecuteNonQuery();

                            l += 1;
                        }
                    }

                }

                //===================================================
                // Insert For Scheme Line
               
                i = gvDetails.Rows.Count;
                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        i = i + 1;

                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        TextBox txtQtyToAvailPcs = (TextBox)gv.FindControl("txtQtyPcs");
                        txtQtyToAvail.Text = (txtQtyToAvail.Text.Trim().Length == 0 ? "0" : txtQtyToAvail.Text);
                        txtQtyToAvailPcs.Text = (txtQtyToAvailPcs.Text.Trim().Length == 0 ? "0" : txtQtyToAvailPcs.Text);
                       

                        HiddenField pr = (HiddenField)gv.Cells[2].FindControl("HiddenField2");
                        HiddenField hScTax1component = (HiddenField)gv.Cells[2].FindControl("hScTax1component");
                        HiddenField hScTax2component = (HiddenField)gv.Cells[2].FindControl("hScTax2component");
                        HiddenField hScTax1 = (HiddenField)gv.Cells[2].FindControl("hScTax1");
                        HiddenField hScTax2 = (HiddenField)gv.Cells[2].FindControl("hScTax2");
                        
                        decimal decQtyToAvail = Global.ConvertToDecimal(txtQtyToAvail.Text);
                       // totamt = 0;

                        {
                            #region For Box Pcs Conv
                            

                            decimal packSize = 1, boxqty = 0, pcsQty = 0;
                            decimal billQty = 0;
                            string boxPcs = "";

                            using (DataTable dtFreeItem = baseObj.GetData("Select Cast(ISNULL(Product_PackSize,1) as int) AS Product_PackSize From AX.InventTable Where ItemId='" + gv.Cells[4].Text.ToString() + "'"))
                            {
                                if (dtFreeItem.Rows.Count > 0)
                                {
                                    string strPack = dtFreeItem.Rows[0]["Product_PackSize"].ToString();
                                    packSize = Global.ConvertToDecimal(strPack);

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

                            string[] calValue = baseObj.CalculatePrice1(gv.Cells[4].Text, drpCustomerCode.SelectedItem.Value, billQty, "Box");
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
                                cmd1.Parameters.Clear();
                                cmd1.Parameters.AddWithValue("@status", "Insert");
                                cmd1.Parameters.AddWithValue("@SITEID", lblsite.Text);
                                cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                                cmd1.Parameters.AddWithValue("@RECID", "");
                                cmd1.Parameters.AddWithValue("@CUSTOMER_CODE", drpCustomerCode.SelectedItem.Value);
                                cmd1.Parameters.AddWithValue("@INVOICE_NO", txtInvoiceNo.Text);
                                cmd1.Parameters.AddWithValue("@LINE_NO", i);
                                cmd1.Parameters.AddWithValue("@PRODUCT_CODE", gv.Cells[4].Text);
                                cmd1.Parameters.AddWithValue("@PRODUCTGROUP_CODE", "");
                                cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(gv.Cells[24].Text));                                
                                cmd1.Parameters.AddWithValue("@BOX", billQty);                                
                                cmd1.Parameters.AddWithValue("@BOXPcs", boxPcs);
                                cmd1.Parameters.AddWithValue("@BOXQty", txtQtyToAvail.Text);
                                cmd1.Parameters.AddWithValue("@PcsQty", txtQtyToAvailPcs.Text);
                                cmd1.Parameters.AddWithValue("@BasePrice", Convert.ToDecimal(gv.Cells[15].Text));
                                cmd1.Parameters.AddWithValue("@TaxableAmount", Convert.ToDecimal(gv.Cells[19].Text));
                                //================================
                                cmd1.Parameters.AddWithValue("@CRATES", 0);
                                cmd1.Parameters.AddWithValue("@LTR", strLtr);
                                cmd1.Parameters.AddWithValue("@QUANTITY", 0);
                                cmd1.Parameters.AddWithValue("@MRP", 0);
                                cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(gv.Cells[15].Text));
                                cmd1.Parameters.AddWithValue("@TAX_CODE", Convert.ToDecimal(gv.Cells[20].Text));
                                cmd1.Parameters.AddWithValue("@TAX_AMOUNT", Convert.ToDecimal(gv.Cells[21].Text));
                                cmd1.Parameters.AddWithValue("@ADDTAX_CODE", Convert.ToDecimal(gv.Cells[22].Text));
                                cmd1.Parameters.AddWithValue("@ADDTAX_AMOUNT", Convert.ToDecimal(gv.Cells[23].Text));
                                cmd1.Parameters.AddWithValue("@DISC_AMOUNT", 0);
                                cmd1.Parameters.AddWithValue("@SEC_DISC_AMOUNT", 0);
                                cmd1.Parameters.AddWithValue("@TranType", 1);
                                cmd1.Parameters.AddWithValue("@DiscType", 2);
                                cmd1.Parameters.AddWithValue("@Disc", 0);
                                cmd1.Parameters.AddWithValue("@SchemeCode", gv.Cells[1].Text);
                                cmd1.Parameters.AddWithValue("@LINEAMOUNT", Convert.ToDecimal(gv.Cells[16].Text));
                                cmd1.Parameters.AddWithValue("@DiscCalculationBase", 2);
                                cmd1.Parameters.AddWithValue("@TDValue", 0);
                                cmd1.Parameters.AddWithValue("@PEValue", 0);
                                cmd1.Parameters.AddWithValue("@HSNCODE", gv.Cells[25].Text);
                                cmd1.Parameters.AddWithValue("@COMPOSITIONSCHEME", Convert.ToInt32(Session["SITECOMPOSITIONSCHEME"].ToString()));
                                cmd1.Parameters.AddWithValue("@TAXCOMPONENT", gv.Cells[26].Text);
                                cmd1.Parameters.AddWithValue("@ADDTAXCOMPONENT", gv.Cells[27].Text);
                                cmd1.Parameters.AddWithValue("@DOCTYPE", 6);
                                cmd1.Parameters.AddWithValue("@SchemeDiscPer", Convert.ToDecimal(gv.Cells[17].Text));
                                cmd1.Parameters.AddWithValue("@SchemeDiscVal", Convert.ToDecimal(gv.Cells[18].Text));
                                cmd1.Parameters.AddWithValue("@Tax1", hScTax1.Value.ToString().Trim());
                                cmd1.Parameters.AddWithValue("@Tax2", hScTax2.Value.ToString().Trim());
                                cmd1.Parameters.AddWithValue("@Tax1component", hScTax1component.Value.ToString().Trim());
                                cmd1.Parameters.AddWithValue("@Tax2component", hScTax2component.Value.ToString().Trim());
                                cmd1.ExecuteNonQuery();
                                totamt = totamt + Convert.ToDecimal(gv.Cells[24].Text); 
                            }
                        }
                    }
                }
                //==============Remaining Part Of Header-===============
                cmd.Parameters.AddWithValue("@INVOICE_VALUE", totamt);
                cmd.ExecuteNonQuery();
                //============Commit All Data================
                transaction.Commit();
                Session["SO_NOList"] = null;                
                dtLineItems = null;
                Session["InvLineItem"] = null;
                Session["InvSchemeLineItem"] = null;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lblMessage.Text = ex.Message.ToString();                
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                txtInvoiceNo.Text = "";
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
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
                    decimal TransQty = Global.ConvertToDecimal(Qty);
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
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void ClearAll()
        {
            txtViewTotalBox.Text = "";
            txtViewTotalPcs.Text = "";
            txtBoxPcs.Text = "";
            //gvDetails.Columns[11].Visible = true;
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

            txtSecDiscPer.Text = "0.00";
            txtSecDiscValue.Text = "0.00";
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
        protected void lnkbtn_Click(object sender, EventArgs e)
        {

        }
        protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


        }
        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            //Cache.Remove("SO_NO");
            Session["SO_NOList"] = null;
            Response.Redirect("~/frmInvoicePrepration.aspx");
        }
        private bool Validation()
        {
            bool returnvalue = true;

            try
            {
                // Check For Valid Delivery Date
                if (txtAddress.Text.Trim().Length == 0)
                {
                    string message = "alert('Customers Bill To Address Required !!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;                    
                    return returnvalue;
                }

                if (txtBillToState.Text.Trim().Length == 0)
                {
                    string message = "alert('Customer Bill To State Required !!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;                    
                    return returnvalue;
                }

                if (ddlShipToAddress.SelectedValue == null || ddlShipToAddress.SelectedValue.ToString().Trim().Length == 0)
                {
                    string message = "alert('Customers Ship To Address Required !!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;                    
                    return returnvalue;
                }

                if (drpCustomerCode.Text == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Customer Name !');", true);
                    string message = "alert('Please Provide Customer Name !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    drpCustomerGroup.Focus();
                    returnvalue = false;
                    return returnvalue;
                }

                else if (drpCustomerGroup.Text == "")
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Customer Group !');", true);
                    string message = "alert('Please Provide Customer Group !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    drpCustomerGroup.Focus();
                    returnvalue = false;
                    return returnvalue;
                }

                else if (drpSONumber.Text == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Sale Order Number !');", true);
                    string message = "alert('Please Provide Sale Order Number !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    drpSONumber.Focus();
                    returnvalue = false;
                    return returnvalue;
                }

                else if (txtInvoiceDate.Text == "")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Invoice Date !');", true);
                    txtInvoiceDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);

                    //string message = "alert('Please Provide Invoice Date !');";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    //txtInvoiceDate.Focus();
                    //returnvalue = false;
                    //return returnvalue;
                }
                //===============================
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();
                //=============So checking its already in the invoice table?========
                if (Session["SO_NOList"] == null)
                {
                    string message = "alert('Invoice_save session expired ,Please select Invoice No ...'); window.location.href='frmInvoicePrepration.aspx'; ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
                string sono = Session["SO_NOList"].ToString();// Cache["SO_NO"].ToString();
                string query1 = "select * from ax.ACXSALEINVOICEHEADER " +
                    "where [Siteid]='" + lblsite.Text + "' and SO_NO in (select id from [dbo].[CommaDelimitedToTable]('" + sono + "' ,',')) and invoice_no not in (select so_no from ax.ACXSALEINVOICEHEADER B where [Siteid]='" + lblsite.Text + "' and trantype=2)";
                dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    string message = "alert('Already Created Invoice No: " + txtInvoiceNo.Text + " againts the SO NO: " + drpSONumber.SelectedItem.Text + " !')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                }
                //============Check Stock Item Line By Line======
                dt = new DataTable();
                int valrow = 0;
                decimal totalInvoiceValue = 0;
                Label lblTotal;

                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    lblTotal = (Label)gvDetails.Rows[i].Cells[2].FindControl("Total");
                    totalInvoiceValue += Global.ConvertToDecimal(lblTotal.Text);
                    Label Product = (Label)gvDetails.Rows[i].Cells[2].FindControl("Product");
                    string productNameCode = Product.Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    TextBox box = (TextBox)gvDetails.Rows[i].Cells[6].FindControl("txtBox");
                    string Qty = box.Text;
                    decimal TransQty = Global.ConvertToDecimal(Qty);

                    if (TransQty > 0)
                    {
                        valrow = valrow + 1;
                    }

                    string query = "select ProductCode from ax.acxinventTrans group by sitecode,translocation,ProductCode " +
                                   " Having sitecode='" + Session["SiteCode"].ToString() + "' and translocation='" + Session["TransLocation"].ToString() + "' and ProductCode='" + ProductCode + "' and sum(TransQty)>=" + TransQty + "";

                    dt = obj.GetData(query);
                    if (Convert.ToDecimal(Qty)>0)
                    {
                        if (dt.Rows.Count <= 0)
                        {
                            string message = "alert('Product :" + productNameCode + " is out of stock !');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            returnvalue = false;
                            return returnvalue;
                        }
                    }
                }

                if (totalInvoiceValue <= 0)
                {
                    string message = "alert('Invoice value should not be less than zero!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                }

                if (valrow <= 0)
                {
                    string message = "alert('Please enter valid data !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                string message = "alert('" + ex.Message + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                returnvalue = false;
                ErrorSignal.FromCurrentContext().Raise(ex);
                return returnvalue;
            }
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
        public void BindSchemeGrid()
        {
            DataTable dt = GetDatafromSP(@"ACX_SCHEME");
            if (dt != null)
            {


                foreach (DataRow row in dt.Rows)
                {
                    DataTable dtscheme = new DataTable();
                    dtscheme = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + row["Free Item Code"].ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "',1," + Convert.ToDecimal(row["Rate"].ToString()) + ",'" + Session["SITELOCATION"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
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
            gvScheme.DataSource = dt;
            gvScheme.DataBind();

            //Session["InvSchemeLineItem"] = null;
            //AddColumnInSchemeDataTable();
            //DataTable dt = GetDatafromSP(@"ACX_SCHEME");
            //gvScheme.DataSource = dt;
            //gvScheme.DataBind();
            //Session["InvSchemeLineItem"] = dt;
           
        }
        public DataTable GetDatafromSP(string SPName)
        {
            conn = baseObj.GetConnection();
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
                    dt.Columns.Add("TotalFreeQty", typeof(System.Int16));
                    dt.Columns.Add("TotalFreeQtyPCS", typeof(System.Int16));
                    DataTable dt1 = dt.Clone();
                    dt1.Clear();

                    Int16 TotalQtyofGroupItem = 0;
                    Int16 TotalPCSQtyofGroupItem = 0;
                    //decimal TotalQtyofGroupItem = 0;
                    Int16 TotalQtyofItem = 0;
                    Int16 TotalPCSQtyofItem = 0;
                    //decimal TotalQtyofItem = 0;
                    Int16 TotalMaxQtyofItem = 0;
                    Int16 TotalMaxPCSQtyofItem = 0;
                    //decimal TotalMaxQtyofItem = 0;

                    decimal TotalValueofGroupItem = 0;
                    decimal TotalValueofItem = 0;
                    decimal TotalMaxValueofItem = 0;

                    foreach (DataRow dr in dt.Rows)
                    {

                        #region Picking Scheme For Bill Qty
                        if (dr["Scheme Type"].ToString() == "0") // For Qty
                        {
                            if (Convert.ToInt16(dr["MINIMUMQUANTITY"]) > 0)
                            {
                                TotalQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString(), "BOX");
                                TotalQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString(), "BOX");
                                TotalMaxQtyofItem = GetMaxQtyofItem("BOX");
                                #region Picking Scheme For Box Qty Free
                                if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                                {
                                    if (dr["Scheme Item Type"].ToString() == "Group" && TotalQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITY"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        dt1.ImportRow(dr);
                                    }

                                    if (dr["Scheme Item Type"].ToString() == "Item" && TotalQtyofItem >= Convert.ToInt16(dr["MINIMUMQUANTITY"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofItem / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        dt1.ImportRow(dr);
                                    }
                                    if (dr["Scheme Item Type"].ToString() == "All" && TotalQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITY"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        dt1.ImportRow(dr);
                                    }
                                }
                                #endregion

                                #region Picking Scheme For PCS Qty Free
                                if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                                {
                                    if (dr["Scheme Item Type"].ToString() == "Group" && TotalQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITY"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }

                                    if (dr["Scheme Item Type"].ToString() == "Item" && TotalQtyofItem >= Convert.ToInt16(dr["MINIMUMQUANTITY"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofItem / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }
                                    if (dr["Scheme Item Type"].ToString() == "All" && TotalQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITY"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITY"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }
                                }
                                #endregion
                            }

                            if (Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]) > 0)
                            {
                                TotalPCSQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString(), "PCS");
                                TotalPCSQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString(), "PCS");
                                TotalMaxPCSQtyofItem = GetMaxQtyofItem("PCS");

                                #region Picking Scheme For Box Free Qty
                                if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                                {
                                    if (dr["Scheme Item Type"].ToString() == "Group" && TotalPCSQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        dt1.ImportRow(dr);
                                    }

                                    if (dr["Scheme Item Type"].ToString() == "Item" && TotalPCSQtyofItem >= Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofItem / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        dt1.ImportRow(dr);
                                    }
                                    if (dr["Scheme Item Type"].ToString() == "All" && TotalPCSQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        dt1.ImportRow(dr);
                                    }
                                }
                                #endregion

                                #region Picking Scheme For PCS Free Qty
                                if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                                {
                                    if (dr["Scheme Item Type"].ToString() == "Group" && TotalPCSQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }

                                    if (dr["Scheme Item Type"].ToString() == "Item" && TotalPCSQtyofItem >= Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofItem / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }
                                    if (dr["Scheme Item Type"].ToString() == "All" && TotalPCSQtyofGroupItem >= Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(dr["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region Picking Scheme For Value
                        if (dr["Scheme Type"].ToString() == "1") // For Value
                        {
                            TotalValueofGroupItem = GetValueofGroupItem(dr["Scheme Item group"].ToString());
                            TotalValueofItem = GetValueofItem(dr["Scheme Item group"].ToString());
                            TotalMaxValueofItem = GetMaxValueofItem();
                            if (Convert.ToDecimal(dr["MINIMUMVALUE"]) > 0)
                            {
                                if (Convert.ToInt16(dr["FREEQTY"]) > 0)
                                {
                                    if (dr["Scheme Item Type"].ToString() == "Group" && TotalValueofGroupItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        //dr["FREEQTY"] = dr["TotalFreeQty"];
                                        dt1.ImportRow(dr);
                                    }

                                    if (dr["Scheme Item Type"].ToString() == "Item" && TotalValueofItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        //dr["FREEQTY"] = dr["TotalFreeQty"];
                                        dt1.ImportRow(dr);
                                    }
                                    if (dr["Scheme Item Type"].ToString() == "All" && TotalValueofGroupItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                                    {
                                        dr["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTY"]));
                                        //dr["FREEQTY"] = dr["TotalFreeQty"];
                                        dt1.ImportRow(dr);
                                    }
                                }
                                if (Convert.ToInt16(dr["FREEQTYPCS"]) > 0)
                                {
                                    if (dr["Scheme Item Type"].ToString() == "Group" && TotalValueofGroupItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }

                                    if (dr["Scheme Item Type"].ToString() == "Item" && TotalValueofItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }
                                    if (dr["Scheme Item Type"].ToString() == "All" && TotalValueofGroupItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                                    {
                                        dr["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt16(dr["FREEQTYPCS"]));
                                        dt1.ImportRow(dr);
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    DataTable dt3 = dt1.Clone();

                    #region For Qty
                    DataView view = new DataView(dt1);
                    view.RowFilter = "[Scheme Type]=0";
                    DataTable distinctTable = (DataTable)view.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    DataView dvSort = new DataView(distinctTable);
                    dvSort.Sort = "SCHEMECODE ASC, MINIMUMQUANTITY DESC";

                    DataView Dv1 = null;
                    int intCalRemFreeQty = 0;
                    Int16 RemainingQty = 0;
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
                                    TotalQtyofGroupItem = GetQtyofGroupItem(drv2["Scheme Item group"].ToString(), "BOX");
                                    TotalQtyofItem = GetQtyofItem(drv2["Scheme Item group"].ToString(), "BOX");
                                    TotalMaxQtyofItem = GetMaxQtyofItem("BOX");

                                    if (drv["Scheme Item Type"].ToString() == "Group" && TotalQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                        dt3.ImportRow(drv2.Row);
                                        //  RemainingQty = Convert.ToInt16(TotalQtyofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                    }

                                    if (drv["Scheme Item Type"].ToString() == "Item" && TotalQtyofItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofItem / Convert.ToInt16(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                        dt3.ImportRow(drv2.Row);
                                        //   RemainingQty = Convert.ToInt16(TotalQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                    }

                                    if (drv["Scheme Item Type"].ToString() == "All" && TotalQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                        dt3.ImportRow(drv2.Row);
                                        //  RemainingQty = Convert.ToInt16(TotalMaxQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                    }
                                }
                                #endregion

                                #region Scheme Free Lines For PCS Free Qty
                                // PCS Qty Scheme Filter
                                Dv1 = new DataView(dt1);
                                Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY='" + drv.Row["MINIMUMQUANTITY"] + "' AND FREEQTYPCS>0";

                                foreach (DataRowView drv2 in Dv1)
                                {
                                    TotalQtyofGroupItem = GetQtyofGroupItem(drv2["Scheme Item group"].ToString(), "BOX");
                                    TotalQtyofItem = GetQtyofItem(drv2["Scheme Item group"].ToString(), "BOX");
                                    TotalMaxQtyofItem = GetMaxQtyofItem("BOX");

                                    if (drv["Scheme Item Type"].ToString() == "Group" && TotalQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                        dt3.ImportRow(drv2.Row);
                                        //RemainingQty = Convert.ToInt16(TotalPCSQtyofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                    }

                                    if (drv["Scheme Item Type"].ToString() == "Item" && TotalQtyofItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofItem / Convert.ToInt16(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                        dt3.ImportRow(drv2.Row);
                                        //RemainingQty = Convert.ToInt16(TotalQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                    }

                                    if (drv["Scheme Item Type"].ToString() == "All" && TotalQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                                    {
                                        drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                        dt3.ImportRow(drv2.Row);
                                        //RemainingQty = Convert.ToInt16(TotalMaxQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
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
                    RemainingQty = 0;
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
                                TotalPCSQtyofGroupItem = GetQtyofGroupItem(drv2["Scheme Item group"].ToString(), "PCS");
                                TotalPCSQtyofItem = GetQtyofItem(drv2["Scheme Item group"].ToString(), "PCS");
                                TotalMaxPCSQtyofItem = GetMaxQtyofItem("PCS");

                                if (drv["Scheme Item Type"].ToString() == "Group" && TotalPCSQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                    dt3.ImportRow(drv2.Row);
                                    //RemainingQty = Convert.ToInt16(TotalQtyofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                }

                                if (drv["Scheme Item Type"].ToString() == "Item" && TotalPCSQtyofItem >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofItem / Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                    dt3.ImportRow(drv2.Row);
                                    //RemainingQty = Convert.ToInt16(TotalQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                }

                                if (drv["Scheme Item Type"].ToString() == "All" && TotalPCSQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                    dt3.ImportRow(drv2.Row);
                                    //RemainingQty = Convert.ToInt16(TotalMaxQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                }
                            }
                            #endregion

                            #region Scheme Free Lines PCS Free Qty
                            Dv1 = new DataView(dt1);
                            Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITYPCS='" + drv.Row["MINIMUMQUANTITYPCS"] + "' AND FREEQTYPCS>0 AND MINIMUMQUANTITYPCS>0";

                            foreach (DataRowView drv2 in Dv1)
                            {
                                TotalPCSQtyofGroupItem = GetQtyofGroupItem(drv2["Scheme Item group"].ToString(), "PCS");
                                TotalPCSQtyofItem = GetQtyofItem(drv2["Scheme Item group"].ToString(), "PCS");
                                TotalMaxPCSQtyofItem = GetMaxQtyofItem("PCS");

                                if (drv["Scheme Item Type"].ToString() == "Group" && TotalPCSQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                    dt3.ImportRow(drv2.Row);
                                    //RemainingQty = Convert.ToInt16(TotalQtyofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                }

                                if (drv["Scheme Item Type"].ToString() == "Item" && TotalPCSQtyofItem >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofItem / Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                    dt3.ImportRow(drv2.Row);
                                    //RemainingQty = Convert.ToInt16(TotalQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                                }

                                if (drv["Scheme Item Type"].ToString() == "All" && TotalPCSQtyofGroupItem >= Convert.ToInt16(drv["MINIMUMQUANTITYPCS"]))
                                {
                                    drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalPCSQtyofGroupItem / Convert.ToInt16(drv2["MINIMUMQUANTITYPCS"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                    dt3.ImportRow(drv2.Row);
                                    //RemainingQty = Convert.ToInt16(TotalMaxQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
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
                    Int16 RemainingQtyValue = 0;
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
                                TotalValueofGroupItem = GetValueofGroupItem(drv2["Scheme Item group"].ToString());
                                TotalValueofItem = GetValueofItem(drv2["Scheme Item group"].ToString());
                                TotalMaxValueofItem = GetMaxValueofItem();

                                if (Convert.ToInt16(drv2["FREEQTY"]) > 0)
                                {
                                    if (drv["Scheme Item Type"].ToString() == "Group" && TotalValueofGroupItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                        dt3.ImportRow(drv2.Row);
                                        RemainingQtyValue = Convert.ToInt16(TotalValueofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMVALUE"]));
                                    }

                                    if (drv["Scheme Item Type"].ToString() == "Item" && TotalValueofItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                        dt3.ImportRow(drv2.Row);
                                        RemainingQtyValue = Convert.ToInt16(TotalValueofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMVALUE"]));
                                    }
                                    if (drv["Scheme Item Type"].ToString() == "All" && TotalValueofGroupItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                        dt3.ImportRow(drv2.Row);
                                        RemainingQtyValue = Convert.ToInt16(TotalValueofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMVALUE"]));
                                    }
                                }
                                if (Convert.ToInt16(drv2["FREEQTYPCS"]) > 0)
                                {
                                    if (drv["Scheme Item Type"].ToString() == "Group" && TotalValueofGroupItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                        dt3.ImportRow(drv2.Row);
                                    }

                                    if (drv["Scheme Item Type"].ToString() == "Item" && TotalValueofItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
                                        dt3.ImportRow(drv2.Row);
                                    }
                                    if (drv["Scheme Item Type"].ToString() == "All" && TotalValueofGroupItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                                    {
                                        drv2["TotalFreeQtyPCS"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTYPCS"]));
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
        public DataTable GetDatafromSPDiscount(string SPName, string strItem)
        {
            DataTable dt = new DataTable();
            conn = baseObj.GetConnection();
            try
            {
                string ItemGroup = string.Empty;
                string ItemCode = string.Empty;

                string strSite = Session["SiteCode"].ToString();
                string strLocation = Session["SCHSTATE"].ToString();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                cmd.Parameters.AddWithValue("@PLACESITE", strSite);
                cmd.Parameters.AddWithValue("@PLACESTATE", strLocation);
                cmd.Parameters.AddWithValue("@PLACEALL", "");
                cmd.Parameters.AddWithValue("@CUSCODECUS", drpCustomerCode.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEGROUP", drpCustomerGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CUSCODEALL", "");
                cmd.Parameters.AddWithValue("@ITEMITEM", strItem);
                cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                //cmd.Parameters.AddWithValue("@ITEMGROUP",DDLProductGroup.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@ITEMALL", "");

                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dt;
        }
        public Int16 GetQtyofGroupItem(string Group, string BoxPCS)        
        {
            DataTable dt;
            if (Group.Trim().Length == 0 || Group.Trim().ToUpper() == "ALL")
            {
                dt = baseObj.GetData("Select  DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            else
            {
                dt = baseObj.GetData("Select DISTINCT ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            Int16 Qty = 0;
            foreach (DataRow dtdr in dt.Rows)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    TextBox txtQty = (TextBox)gvrow.Cells[6].FindControl("txtBoxQtyGrid");
                    TextBox txtQtyPcs = (TextBox)gvrow.Cells[6].FindControl("txtPcsQtyGrid");
                    Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                    string[] arritem = lblItem.Text.Split('-');
                    string strItemCode = arritem[0].ToString().Trim();
                    if (dtdr[0].ToString() == strItemCode)
                    {
                        if (BoxPCS == "BOX")
                        {
                            Qty += Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                        }
                        else
                        {
                            Qty += Convert.ToInt16(Convert.ToDouble(txtQtyPcs.Text));
                        }
                        //Qty += Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                    }
                }
            }
            return Qty;
        }
        public decimal GetValueofGroupItem(string Group)
        {
            DataTable dt;
            if (Group.Trim().Length == 0 || Group.Trim().ToUpper() == "ALL")
            {
                dt = baseObj.GetData("Select ITEMID From AX.ACXFreeItemGroupTable Where DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            else
            {
                dt = baseObj.GetData("Select ITEMID From AX.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
            }
            decimal Value = 0;
            foreach (DataRow dtdr in dt.Rows)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    Label lblAmount = (Label)gvrow.Cells[6].FindControl("Amount");
                    Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                    Label lblSchemeDiscVal = (Label)gvrow.Cells[2].FindControl("lblSchemeDiscVal");
                    
                    string[] arritem = lblItem.Text.Split('-');
                    string strItemCode = arritem[0].ToString().Trim();

                    if (dtdr[0].ToString() == strItemCode)
                    {
                        Value += Convert.ToDecimal(lblAmount.Text) + Convert.ToDecimal(lblSchemeDiscVal.Text);
                    }
                }
            }
            return Value;
        }        
        public Int16 GetQtyofItem(string Item, string BoxPCS)    
        {
            Int16 Qty = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                TextBox txtQty = (TextBox)gvrow.Cells[6].FindControl("txtBoxQtyGrid");
                TextBox txtQtyPcs = (TextBox)gvrow.Cells[6].FindControl("txtPcsQtyGrid");
                Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                string[] arritem = lblItem.Text.Split('-');
                string strItemCode = arritem[0].ToString().Trim();

                if (strItemCode == Item)
                {
                   // Qty = Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                    if (BoxPCS == "BOX")
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                    }
                    else
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(txtQtyPcs.Text));
                    }
                }
            }
            return Qty;
        }
        public decimal GetValueofItem(string Item)
        {
            decimal Value = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                Label lblAmount = (Label)gvrow.Cells[6].FindControl("Total");
                Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                Label lblSchemeDiscVal = (Label)gvrow.Cells[2].FindControl("lblSchemeDiscVal");

                string[] arritem = lblItem.Text.Split('-');
                string strItemCode = arritem[0].ToString().Trim();

                if (strItemCode == Item)
                {
                    //Value = Convert.ToDecimal(lblAmount.Text);
                    Value = Convert.ToDecimal(lblAmount.Text) + Convert.ToDecimal(lblSchemeDiscVal.Text);

                }
            }
            return Value;
        }       
        public Int16 GetMaxQtyofItem(string BoxPCS)       
        {
            Int16 Qty = 0;
            Int16[] arrQty = new Int16[gvDetails.Rows.Count];
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                TextBox txtQty = (TextBox)gvrow.Cells[6].FindControl("txtBoxQtyGrid");
                TextBox txtQtyPcs = (TextBox)gvrow.Cells[6].FindControl("txtPcsQtyGrid");
               // arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                if (BoxPCS == "BOX")
                {
                    arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDecimal(txtQty.Text));
                }
                else
                {
                    arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDecimal(txtQtyPcs.Text));
                }

            }
            Qty = arrQty.Max();
            return Qty;
        }
        public decimal GetMaxValueofItem()
        {
            decimal Value = 0;
            decimal[] arrValue = new decimal[gvDetails.Rows.Count];
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                Label lblAmount = (Label)gvrow.Cells[12].FindControl("Amount");
                Label lblSchemeDiscVal = (Label)gvrow.Cells[2].FindControl("lblSchemeDiscVal");

                //arrValue[gvrow.RowIndex] = Convert.ToDecimal(lblAmount.Text);
                arrValue[gvrow.RowIndex] = Convert.ToDecimal(lblAmount.Text) + Convert.ToDecimal(lblSchemeDiscVal.Text);
            }
            Value = arrValue.Max();
            return Value;
        }

        //private bool GetPrevSelection(string SchemeCode, string SetNo)
        //{
        //    foreach (GridViewRow rw in gvScheme.Rows)
        //    {
        //        CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
        //        if (chkBx.Checked && rw.Cells[1].Text != SchemeCode)
        //        {
        //            return true;
        //        }
        //        if (chkBx.Checked && rw.Cells[1].Text == SchemeCode && rw.Cells[7].Text != SetNo)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            GridViewRow row1 = (GridViewRow)activeCheckBox.Parent.Parent;
            String SchemeCode1 = row1.Cells[1].Text;
            String SetNo1 = row1.Cells[7].Text;
            if (Global.GetPrevSelection(SchemeCode1, SetNo1, ref gvScheme,"0")) { activeCheckBox.Checked = false; return; }
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
                if (SetNo1 == "0")
                {
                    txtQty = (TextBox)row1.FindControl("txtQty");
                    txtQtyPcs = (TextBox)row1.FindControl("txtQtyPcs");
                    txtQty.Text = "";
                    txtQtyPcs.Text = "";
                }
                #endregion
            }
            #endregion
            if (activeCheckBox.Checked)
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
            SchemeDiscCalculation();
            DataTable dt1 = new DataTable();
            if (Session["InvLineItem"] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dt1 = (DataTable)Session["InvLineItem"];
            }
           
            GridViewFooterCalculate(dt1);
        }
        public void GetSelectedShemeItemChecked(string SchemeCode, string FreeitemCode, Int16 Qty, Int16 Slab,Int16 QtyPcs)
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
                    if (chkBx.Checked==false)
                    {
                        txtQty.Text = "0";
                        txtQtyPcs.Text = "0";
                    }
                    //chkSelect_CheckedChanged(this, new EventArgs());
                }
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
            this.SchemeDiscCalculation();
            DataTable dt1 = new DataTable();
            if (Session["InvLineItem"] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dt1 = (DataTable)Session["InvLineItem"];
            }
            
            GridViewFooterCalculate(dt1);
        }
        private void GridViewFooterCalculate(DataTable dt)
        {
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
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Amount"));
            decimal TD = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD"));
            decimal AllTotal = dt.AsEnumerable().Sum(row => row.Field<decimal>("Total"));

            gvDetails.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[4].Text = "TOTAL";
            gvDetails.FooterRow.Cells[4].Font.Bold = true;

            //gvDetails.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[18].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[18].Text = total.ToString("N2");
            //gvDetails.FooterRow.Cells[18].Font.Bold = true;

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


            //gvDetails.FooterRow.Cells[23].HorizontalAlign = HorizontalAlign.Right;
            //gvDetails.FooterRow.Cells[23].ForeColor = System.Drawing.Color.MidnightBlue;
            //gvDetails.FooterRow.Cells[23].Text = TD.ToString("N2");
            //gvDetails.FooterRow.Cells[23].Font.Bold = true;

            gvDetails.FooterRow.Cells[28].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[28].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[28].Text = AllTotal.ToString("N2");
            gvDetails.FooterRow.Cells[28].Font.Bold = true;

            gvDetails.FooterRow.Cells[22].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[22].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[22].Text = tSecDiscAmount.ToString("N2");
            gvDetails.FooterRow.Cells[22].Font.Bold = true;
            CalculateInvoiceValue();
        }

        
        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                       + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'  and P.block=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            FillProductCode();
            DDLProductSubCategory.Focus();
            PcsBillingApplicable();
        }

        protected void DDLProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            strQuery = "Select distinct P.ITEMID+'-'+P.Product_Name as Name,P.ITEMID from ax.InventTable P where Product_Group='" + DDLProductGroup.SelectedValue + "' and P.block=0 and P.PRODUCT_SUBCATEGORY ='" + DDLProductSubCategory.SelectedItem.Value + "'"; //--AND SITE_CODE='657546'";
            DDLMaterialCode.DataSource = null;
            DDLMaterialCode.Items.Clear();
            // DDLMaterialCode.Items.Add("Select...");
            baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Name", "ITEMID");
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            DDLMaterialCode.Enabled = true;
            DDLMaterialCode.SelectedIndex = 0;
            //DDLMaterialCode.Attributes.Add("onChange", "location.href = this.options[this.selectedIndex].value;");
            DDLMaterialCode.Focus();
            /* Modified  29-03-2017     */
            string a = sender.ToString();
            string b = e.ToString();

            txtQtyBox.Text = "0";
            txtBoxqty.Text = "0";
            txtQtyCrates.Text = "0";
            txtLtr.Text = "0.00";
            txtPrice.Text = "0.00";
            txtValue.Text = "0.00";
            txtEnterQty.Text = "0.00";
            txtViewTotalBox.Text = "0.00";
            txtBoxPcs.Text = "0.00";

            DataTable dt = new DataTable();
            strQuery = " Select ISNULL(coalesce(cast(sum(F.TransQty) as decimal(10,2)),0),0) as TransQty,cast(G.Product_PackSize as decimal(10,2)) as Product_PackSize,cast(G.Product_MRP as decimal(10,2)) as Product_MRP ,Cast(G.Product_Crate_PackSize as decimal(10,2)) as Product_CrateSize" +
                       " from [ax].[ACXINVENTTRANS] F " +
                       " Left Join ax.InventTable G on G.ItemId=F.[ProductCode] " +
                       " where F.[SiteCode]='" + Session["SiteCode"].ToString() + "' and G.block=0 and F.[ProductCode]='" + DDLMaterialCode.SelectedItem.Value + "' and F.[TransLocation]='" + Session["TransLocation"].ToString() + "' " +
                       " group by G.Product_PackSize,G.Product_MRP,G.Product_Crate_PackSize ";

            dt = baseObj.GetData(strQuery);
            if (dt.Rows.Count > 0)
            {
                txtStockQty.Text = Convert.ToDecimal(dt.Rows[0]["TransQty"].ToString()).ToString();
                txtPack.Text = dt.Rows[0]["Product_PackSize"].ToString();
                txtMRP.Text = dt.Rows[0]["Product_MRP"].ToString();
                txtCrateSize.Text = dt.Rows[0]["Product_CrateSize"].ToString();
                ProductSubCategory();
            }
            else
            {
                txtStockQty.Text = "0.00";
                txtPack.Text = "0";
                txtMRP.Text = "0.00";
                txtCrateSize.Text = "0.00";
            }
            txtQtyBox.Focus();
            // DataTable dt = new DataTable();
            string query = "select Product_Group,PRODUCT_SUBCATEGORY  from ax.INVENTTABLE where ItemId='" + DDLMaterialCode.SelectedItem.Value + "'  and block=0 order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                DDLProductSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
            }

            txtEnterQty.Focus();
             PcsBillingApplicable();
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = sender.ToString();
            string b = e.ToString();

            txtQtyBox.Text = "0";
            txtBoxqty.Text = "0";
            txtQtyCrates.Text = "0";
            txtLtr.Text = "0.00";
            txtPrice.Text = "0.00";
            txtValue.Text = "0.00";
            txtEnterQty.Text = "0.00";
            txtViewTotalBox.Text = "0.00";
            txtBoxPcs.Text = "0.00";

            DataTable dt = new DataTable();
            strQuery = " Select ISNULL(coalesce(cast(sum(F.TransQty) as decimal(10,2)),0),0) as TransQty,cast(G.Product_PackSize as decimal(10,2)) as Product_PackSize,cast(G.Product_MRP as decimal(10,2)) as Product_MRP ,Cast(G.Product_Crate_PackSize as decimal(10,2)) as Product_CrateSize" +
                       " from [ax].[ACXINVENTTRANS] F " +
                       " Left Join ax.InventTable G on G.ItemId=F.[ProductCode] " +
                       " where F.[SiteCode]='" + Session["SiteCode"].ToString() + "' and G.block=0 and F.[ProductCode]='" + DDLMaterialCode.SelectedItem.Value + "' and F.[TransLocation]='" + Session["TransLocation"].ToString() + "' " +
                       " group by G.Product_PackSize,G.Product_MRP,G.Product_Crate_PackSize ";

            dt = baseObj.GetData(strQuery);
            if (dt.Rows.Count > 0)
            {
                txtStockQty.Text = Convert.ToDecimal(dt.Rows[0]["TransQty"].ToString()).ToString();
                txtPack.Text = dt.Rows[0]["Product_PackSize"].ToString();
                txtMRP.Text = dt.Rows[0]["Product_MRP"].ToString();
                txtCrateSize.Text = dt.Rows[0]["Product_CrateSize"].ToString();
                ProductSubCategory();
            }
            else
            {
                txtStockQty.Text = "0.00";
                txtPack.Text = "0";
                txtMRP.Text = "0.00";
                txtCrateSize.Text = "0.00";
            }
            try
            {
                string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, drpCustomerCode.SelectedItem.Value, 1, ddlEntryType.SelectedItem.Value);
                if (calValue[0] == "")
                {
                    lblMessage.Text = "Price Not Define !";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Price not defined!');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }
            txtQtyBox.Focus();
            // DataTable dt = new DataTable();
            string query = "select Product_Group,PRODUCT_SUBCATEGORY  from ax.INVENTTABLE where ItemId='" + DDLMaterialCode.SelectedItem.Value + "' order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                DDLProductSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
            }

            txtEnterQty.Focus();
            PcsBillingApplicable();
        }
        public void ProductSubCategory()
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'  and P.block=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            // FillProductCode();
            DDLProductSubCategory.Focus();
        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
            DDLMaterialCode.Focus();
            lblMessage.Text = string.Empty;
           
            //=============Validation=================
            foreach (GridViewRow grv in gvDetails.Rows)
            {
                //string product = grv.Cells[0].Text;
                HiddenField hdnproduct = (HiddenField)grv.Cells[0].FindControl("HiddenField2");
                string product = hdnproduct.Value;
                if (DDLMaterialCode.SelectedItem.Value == product)
                {
                    txtEnterQty.Text = "";
                    txtQtyBox.Text = "";
                    txtQtyCrates.Text = "";
                    txtLtr.Text = "";
                    txtPrice.Text = "";
                    txtValue.Text = "";
                    txtMRP.Text = "";
                    txtPack.Text = "";
                    txtStockQty.Text = "";
                    txtCrateQty.Text = "";
                    txtBoxqty.Text = "";
                    txtPCSQty.Text = "";


                   // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');", true);
                    string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return;
                }

            }
            //============================
            bool valid = true;
            valid = ValidateLineItemAdd();
            //DataTable dt = new DataTable();
            //dt = Session["ItemTable"] as DataTable;
            DataTable dt = new DataTable();
            if (Session["InvLineItem"] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dt = (DataTable)Session["InvLineItem"];
            }

            if (valid == true)
            {
                
                dt = AddLineItems();
                if (Msg.Rows.Count > 0)
                {
                    string message = "alert('Error: Invalid Operation...!!!! ');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('" + message.ToString().Replace("'","''") + "'", true);
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = true;
                    if (txtSecDiscPer.Text.Trim().Length > 0 || txtSecDiscValue.Text.Trim().Length > 0)
                    { btnGO_Click(sender, e); }
                    if (txtTDValue.Text.Trim().Length>0)
                    { btnApply_Click(sender, e); }
                    GridViewFooterCalculate(dt);

                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = false;
                }
            }
            //else
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //    gvDetails.Visible = true;
            //                }

            //if (dt.Rows.Count > 0)
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //    gvDetails.Visible = true;

            //    GridViewFooterCalculate(dt);

            //}
            //else
            //{
            //    gvDetails.DataSource = dt;
            //    gvDetails.DataBind();
            //    gvDetails.Visible = false;
            //}
            txtEnterQty.Text = string.Empty;
            DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
            if (dtApplicable.Rows.Count > 0)
            {
                intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
            }
            if (intApplicable == 1 || intApplicable == 3)
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
            DDLMaterialCode.Enabled = true;
            GridViewFooterCalculate(dt);

            //DDLMaterialCode.SelectedIndex = 0;           
            //            DDLMaterialCode.Focus();
        }
        public void ProductGroup()
        {

            string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>'' AND A.BLOCK=0 order by a.Product_Group";
            DDLProductGroup.Items.Clear();
            DDLProductGroup.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        }
        protected void txtQtyBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, drpCustomerCode.SelectedItem.Value, int.Parse(txtEnterQty.Text), ddlEntryType.SelectedItem.Value);
                if (calValue[0] != "")
                {
                    if (calValue[5] == "Box")
                    {
                        txtQtyBox.Text = txtEnterQty.Text;
                        txtQtyCrates.Text = calValue[0];
                        if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                            string message = "alert('Please Enter a valid number!!');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                            return;
                        }
                    }
                    if (calValue[5] == "Crate")
                    {
                        txtQtyCrates.Text = txtEnterQty.Text;
                        txtQtyBox.Text = calValue[0];
                    }
                    txtLtr.Text = calValue[1];

                    txtPrice.Text = calValue[2];
                    txtValue.Text = calValue[3];
                    lblHidden.Text = calValue[4];

                    BtnAddItem.Focus();
                    BtnAddItem.CausesValidation = false;

                  //  BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");
                }
                else
                {
                    lblMessage.Text = "Price Not Define !";
                }
            }
            catch (Exception ex)
            {
                //                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price Not Defined !');", true);
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + ex.Message.ToString() + "');", true);
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }

        }
        private bool ValidateLineItemAdd()
        {
            bool b = true;

            if (DDLProductGroup.Text == "Select..." || DDLProductGroup.Text == "")
            {
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                string message = "alert('Select Material Group !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                DDLProductGroup.Focus();
                b = false;
                return b;
            }
            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "Select...")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Product First !');", true);

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

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be left blank !');", true);
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
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be zero !');", true);

                string message = "alert('Stock Qty cannot be zero !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Box Qty should not greater than Stock Qty !');", true);
                string message = "alert('Box Qty should not greater than Stock Qty !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtQtyCrates.Text == string.Empty)
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price cannot be left blank !');", true);
                string message = "alert('Price cannot be left blank !');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                return b;
            }
            if (txtLtr.Text == string.Empty || txtLtr.Text == "0")
            {
                b = false;
               // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('ltr cannot be left blank !');", true);
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
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price is cannot be left blank !');", true);

                    string message = "alert('Price is cannot be left blank !');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                    return b;
                }
                if (Convert.ToDecimal(txtPrice.Text) <= 0)
                {
                    b = false;
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price is cannot be left blank !');", true);

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
        private DataTable AddLineItems()
        {
            try
            {
                if (Session["InvLineItem"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["InvLineItem"];
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
                        return dtLineItems;
                    }
                    #endregion

                    DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                    if (dtApplicable.Rows.Count > 0)
                    {
                        intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                    }

                    #region  Discount
                    /////////////////////////////////// For /////////////////////////////////////////////////////

                    #endregion

                    int count = gvDetails.Rows.Count;
                    count = count + 1;
                    DataTable dt = new DataTable();
                    /* ----------------Gst Implementation----------------*/                   
                    dt = baseObj.GetData("EXEC USP_ACX_GetSalesLineCalcGST '" + DDLMaterialCode.SelectedValue.ToString() + "','" + drpCustomerCode.SelectedValue.ToString() + "','" + Session["SiteCode"].ToString() + "'," + Convert.ToDecimal(txtQtyBox.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",'" + Session["SITELOCATION"].ToString() + "','" + drpCustomerGroup.SelectedValue.ToString() + "','" + Session["DATAAREAID"].ToString() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["RETMSG"].ToString().IndexOf("FALSE") >= 0)
                        {
                            string message = "alert('" + dt.Rows[0]["RETMSG"].ToString().Replace("FALSE|", "") + "');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                            DDLMaterialCode.Focus();
                            return dtLineItems;
                        }

                        DataRow row;
                        row = dtLineItems.NewRow();

                        row["Product_Code"] = DDLMaterialCode.SelectedValue.ToString();
                        row["Line_No"] = count;
                        row["Product"] = DDLMaterialCode.SelectedItem.Text.ToString();
                        row["Pack"] = txtPack.Text;
                        row["So_Qty"] = "0";
                        row["Invoice_Qty"] = decimal.Parse(txtQtyBox.Text.Trim().ToString());
                        row["Box_Qty"] = Convert.ToDecimal(txtViewTotalBox.Text.Trim().ToString()); //Math.Truncate(decimal.Parse(txtQtyBox.Text.Trim().ToString()));
                        row["Pcs_Qty"] = Convert.ToDecimal(txtViewTotalPcs.Text.Trim().ToString());// (decimal.Parse(txtQtyBox.Text.Trim().ToString()) - Math.Truncate(decimal.Parse(txtQtyBox.Text.Trim().ToString()))) * Global.ParseDecimal(txtPack.Text);//decimal.Parse(txtPCSQty.Text.Trim().ToString());
                        row["BoxPcs"] = Convert.ToDecimal(txtBoxPcs.Text.Trim());
                        row["Ltr"] = Convert.ToDecimal(txtLtr.Text.Trim().ToString());
                        //row["Rate"] = Convert.ToDecimal(txtPrice.Text.Trim().ToString());
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
                        row["DiscVal"] = Convert.ToDecimal(dt.Rows[0]["DISCVAL"]).ToString("0.00");
                        row["SchemeDisc"] = new decimal(0);
                        row["SchemeDiscVal"] = new decimal(0);
                       
                        //==========For Tax===============
                        
                        //row["TaxPer"] = Convert.ToDecimal(dt.Rows[0]["TAX_CODE"]);
                        row["TaxPer"] = Convert.ToDecimal(dt.Rows[0]["TAX_PER"]);
                        row["TaxValue"] = Convert.ToDecimal(dt.Rows[0]["TAX_AMOUNT"]).ToString("0.00");
                        //row["ADDTaxPer"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_CODE"]);
                        row["ADDTaxPer"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_PER"]);
                        row["ADDTaxValue"] = Convert.ToDecimal(dt.Rows[0]["ADDTAX_AMOUNT"]).ToString("0.00");
                        row["MRP"] = Convert.ToDecimal(dt.Rows[0]["MRP"]);
                        row["TaxableAmount"] = Convert.ToDecimal(dt.Rows[0]["TaxableAmount"]);

                        row["StockQty"] = decimal.Parse(txtStockQty.Text.Trim().ToString());
                        row["SchemeCode"] = "";
                        row["Total"] = Convert.ToDecimal(dt.Rows[0]["VALUE"]).ToString("0.00"); //TotalValue.ToString("0.00");
                        row["TD"] = "0.00";
                        row["PE"] = "0.00";
                        row["SecDiscPer"] = "0.00";
                        row["SecDiscAmount"] = "0.00";
                        row["SchemeDisc"] = "0.00";
                        row["SchemeDiscVal"] = "0.00";
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
                       
                        /*  ----- GST Implementation End */

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

                        dtLineItems.Rows.Add(row);

                        //Update session table
                        Session["InvLineItem"] = dtLineItems;

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
                }
             
            }
            catch (Exception ex)
            {
               
                string message = "alert('Error: " + ex.Message + " !');";
                Msg.Columns.Add("Mssg");
                Msg.Rows.Add(Msg.NewRow()["Mssg"] = message);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                ErrorSignal.FromCurrentContext().Raise(ex);
                return Msg;
            }
           return dtLineItems;
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
            // dtLineItems.Columns.Add("MaterialGroup", typeof(string));
            dtLineItems.Columns.Add("Product_Code", typeof(string));
            dtLineItems.Columns.Add("Line_No", typeof(decimal));
            dtLineItems.Columns.Add("Product", typeof(string));
            dtLineItems.Columns.Add("Pack", typeof(string));
           // dtLineItems.Columns.Add("Mrp", typeof(string));
            dtLineItems.Columns.Add("So_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Invoice_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Box_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Pcs_Qty", typeof(decimal));
            dtLineItems.Columns.Add("BoxPcs", typeof(decimal));


            //dtLineItems.Columns.Add("QtyCrates", typeof(decimal));
            //dtLineItems.Columns.Add("QtyBox", typeof(int));
            dtLineItems.Columns.Add("Ltr", typeof(decimal));
            dtLineItems.Columns.Add("Rate", typeof(decimal));
            dtLineItems.Columns.Add("BasePrice", typeof(decimal));
            dtLineItems.Columns.Add("Amount", typeof(decimal));
            //dtLineItems.Columns.Add("UOM", typeof(string));
            /*-------Discount---------*/
            dtLineItems.Columns.Add("DISCTYPE", typeof(string));
            dtLineItems.Columns.Add("Disc", typeof(decimal));
            dtLineItems.Columns.Add("DiscVal", typeof(decimal));
            dtLineItems.Columns.Add("SchemeDisc", typeof(decimal));
            dtLineItems.Columns.Add("SchemeDiscVal", typeof(decimal));
            //===========Tax==============================
            dtLineItems.Columns.Add("TaxPer", typeof(decimal));
            dtLineItems.Columns.Add("TaxValue", typeof(decimal));
            dtLineItems.Columns.Add("AddTaxPer", typeof(decimal));
            dtLineItems.Columns.Add("AddTaxValue", typeof(decimal));
            dtLineItems.Columns.Add("MRP", typeof(decimal));
            dtLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            dtLineItems.Columns.Add("StockQty", typeof(decimal));   //mrp or price
            dtLineItems.Columns.Add("SchemeCode", typeof(string));
            dtLineItems.Columns.Add("Total", typeof(decimal));
            dtLineItems.Columns.Add("TD", typeof(decimal));
            dtLineItems.Columns.Add("SecDiscPer", typeof(decimal));
            dtLineItems.Columns.Add("SecDiscAmount", typeof(decimal));
            dtLineItems.Columns.Add("CALCULATIONBASE", typeof(string));
            dtLineItems.Columns.Add("CalculationOn", typeof(string));
            dtLineItems.Columns.Add("DiscCalculationBase", typeof(string));
            // New Fields for GST
            dtLineItems.Columns.Add("HSNCode", typeof(string));
            dtLineItems.Columns.Add("TaxComponent", typeof(string));
            dtLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtLineItems.Columns.Add("PE", typeof(decimal));
            dtLineItems.Columns.Add("CLAIMDISCAMT", typeof(decimal));
            dtLineItems.Columns.Add("TAX1", typeof(decimal));
            dtLineItems.Columns.Add("TAX2", typeof(decimal));
            dtLineItems.Columns.Add("TAX1COMPONENT", typeof(string));
            dtLineItems.Columns.Add("TAX2COMPONENT", typeof(string));

        }

        private void AddColumnInSchemeDataTable()
        {
            dtSchemeLineItems = new DataTable("SchemeLineItemTable");
            dtSchemeLineItems.Columns.Add("Srno", typeof(int));
            dtSchemeLineItems.Columns.Add("Avail", typeof(bool));
            dtSchemeLineItems.Columns.Add("SchemeCode", typeof(string));
            dtSchemeLineItems.Columns.Add("SchemeName", typeof(string));
            dtSchemeLineItems.Columns.Add("ProductGroup", typeof(string));
            dtSchemeLineItems.Columns.Add("ProductCode", typeof(string));
            dtSchemeLineItems.Columns.Add("ProductName", typeof(string));
            dtSchemeLineItems.Columns.Add("Slab", typeof(int));
            dtSchemeLineItems.Columns.Add("Set", typeof(int));
            dtSchemeLineItems.Columns.Add("FreeBoxQty", typeof(int));
            dtSchemeLineItems.Columns.Add("AvailBoxQty", typeof(int));
            dtSchemeLineItems.Columns.Add("FreePCSQty", typeof(int));
            dtSchemeLineItems.Columns.Add("AvailPCSQty", typeof(int));
            dtSchemeLineItems.Columns.Add("PackSize", typeof(int));
            dtSchemeLineItems.Columns.Add("ConvQty", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Rate", typeof(decimal));
            dtSchemeLineItems.Columns.Add("QtyLtr", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Price", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Value", typeof(decimal));
            dtSchemeLineItems.Columns.Add("UOM", typeof(string));
            //===========Discount=========================
            dtSchemeLineItems.Columns.Add("DiscType", typeof(string));
            dtSchemeLineItems.Columns.Add("Disc", typeof(decimal));
            dtSchemeLineItems.Columns.Add("DiscVal", typeof(decimal));
            dtSchemeLineItems.Columns.Add("SchemeDisc", typeof(decimal));
            dtSchemeLineItems.Columns.Add("SchemeDiscVal", typeof(decimal));
            //===========Tax==============================
            dtSchemeLineItems.Columns.Add("Tax_Code", typeof(decimal));
            dtSchemeLineItems.Columns.Add("Tax_Amount", typeof(decimal));
            dtSchemeLineItems.Columns.Add("AddTax_Code", typeof(decimal));
            dtSchemeLineItems.Columns.Add("AddTax_Amount", typeof(decimal));
            dtSchemeLineItems.Columns.Add("MRP", typeof(decimal));
            dtSchemeLineItems.Columns.Add("CalculationBase", typeof(string));
            dtSchemeLineItems.Columns.Add("Calculationon", typeof(string));   //mrp or price
            dtSchemeLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            // New Fields for GST
            dtSchemeLineItems.Columns.Add("HSNCode", typeof(string));
            dtSchemeLineItems.Columns.Add("TaxComponent", typeof(string));
            dtSchemeLineItems.Columns.Add("AddTaxComponent", typeof(string));
            dtSchemeLineItems.Columns.Add("CLAIMDISCAMT", typeof(decimal));
            dtSchemeLineItems.Columns.Add("TAX1", typeof(decimal));
            dtSchemeLineItems.Columns.Add("TAX2", typeof(decimal));
            dtSchemeLineItems.Columns.Add("TAX1COMPONENT", typeof(string));
            dtSchemeLineItems.Columns.Add("TAX2COMPONENT", typeof(string));
        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                decimal SecDiscPer = 0, SecDiscValue = 0, SchemeDiscVal = 0;
                if (string.IsNullOrEmpty(txtSecDiscPer.Text))
                {
                    txtSecDiscPer.Text = "0";
                }
                if (string.IsNullOrEmpty(txtSecDiscValue.Text))
                {
                    txtSecDiscValue.Text = "0";
                }
                DataTable dt = (DataTable)Session["InvLineItem"];
                dt.Columns["SecDiscPer"].ReadOnly = false;
                dt.Columns["SecDiscAmount"].ReadOnly = false;
                dt.Columns["TaxValue"].ReadOnly = false;
                dt.Columns["AddTaxValue"].ReadOnly = false;
                dt.Columns["TaxableAmount"].ReadOnly = false;
                dt.Columns["Total"].ReadOnly = false;

                //dt.Rows[i]["TaxValue"] = taxvalue.Text;
                //dt.Rows[i]["AddTaxValue"] = AddTaxVal.Text;
                //dt.Rows[i]["TaxableAmount"] = hdnTaxableammount.Value;
                //dt.Rows[i]["Total"] = NetAmount.ToString("0.00");

                //foreach (GridViewRow gv in gvDetails.Rows)
                for (int i = 0; gvDetails.Rows.Count > i; i++)
                {
                    HiddenField discType = (HiddenField)gvDetails.Rows[i].FindControl("hDiscType");
                    HiddenField discCalculation = (HiddenField)gvDetails.Rows[i].FindControl("hDiscCalculationType");
                    Label lblMRP = (Label)gvDetails.Rows[i].FindControl("MRP");

                    HiddenField ProductCode = (HiddenField)gvDetails.Rows[i].FindControl("HiddenField2");
                    SecDiscPer = Convert.ToDecimal(txtSecDiscPer.Text);
                    SecDiscValue = Convert.ToDecimal(txtSecDiscValue.Text);

                    TextBox Invoice_Qty = (TextBox)gvDetails.Rows[i].FindControl("txtBox");
                    TextBox txtRate = (TextBox)gvDetails.Rows[i].FindControl("txtRate");
                    Label DiscPer = (Label)gvDetails.Rows[i].FindControl("Disc");
                    Label DiscVal = (Label)gvDetails.Rows[i].FindControl("DiscValue");
                    Label tax = (Label)gvDetails.Rows[i].FindControl("Tax");
                    Label taxvalue = (Label)gvDetails.Rows[i].FindControl("TaxValue");
                    Label AddtaxPer = (Label)gvDetails.Rows[i].FindControl("AddTax");
                    Label AddTaxVal = (Label)gvDetails.Rows[i].FindControl("AddTaxValue");
                    Label netamount = (Label)gvDetails.Rows[i].FindControl("Amount");
                    TextBox SDiscPer = (TextBox)gvDetails.Rows[i].FindControl("SecDisc");
                    TextBox SDiscVal = (TextBox)gvDetails.Rows[i].FindControl("SecDiscValue");

                    Label lblSchemeDiscVal = (Label)gvDetails.Rows[i].FindControl("lblSchemeDiscVal");
                    if (lblSchemeDiscVal.Text.Trim().Length > 0)
                    {
                        SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
                    }
                    Label lblTotal = (Label)gvDetails.Rows[i].FindControl("Total");

                    HiddenField hdnTaxableammount = (HiddenField)gvDetails.Rows[i].FindControl("hdnTaxableAmount");

                    decimal Basic = Convert.ToDecimal(Invoice_Qty.Text) * Convert.ToDecimal(txtRate.Text);
                    if (SecDiscPer != 0)
                    {
                        SecDiscValue = (SecDiscPer * Basic) / 100;
                    }

                    decimal LineAmount = Basic - (SecDiscValue + (SchemeDiscVal) + Convert.ToDecimal(DiscVal.Text));
                    decimal TaxValue = (LineAmount * Convert.ToDecimal(tax.Text)) / 100;
                    decimal AddTaxValue = 0;// (LineAmount * Convert.ToDecimal(AddtaxPer.Text)) / 100;
                    DataTable dtTax = new DataTable();
                    string s = ProductCode.Value;
                    /*----------GST Implement-----------------*/
                    //if (txtBillToState.Text == "JK")
                    //{
                    //    dtTax = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ProductCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                    //    if (dtTax.Rows.Count > 0)
                    //    {
                    //        if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
                    //        {
                    //            AddTaxValue = (TaxValue * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //        }
                    //        else if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 1) // Tax On Taxable Amount
                    //        {
                    //            AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //        }
                    //        else
                    //        {
                    //            AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    //        }
                    //    }
                    //}
                    //else
                    //{
                        AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                    ////}
                    decimal NetAmount = LineAmount + TaxValue + AddTaxValue;
                    if (NetAmount > 0)
                    {
                        hdnTaxableammount.Value = LineAmount.ToString("0.00");
                        taxvalue.Text = TaxValue.ToString("0.00");
                        AddTaxVal.Text = AddTaxValue.ToString("0.00");
                        SDiscPer.Text = SecDiscPer.ToString("0.00");
                        SDiscVal.Text = SecDiscValue.ToString("0.00");
                        // Sec Discount Value 
                        dt.Rows[i]["SecDiscPer"] = SDiscPer.Text;
                        dt.Rows[i]["SecDiscAmount"] = SDiscVal.Text;
                        dt.Rows[i]["TaxValue"] = taxvalue.Text;
                        dt.Rows[i]["AddTaxValue"] = AddTaxVal.Text;
                        dt.Rows[i]["TaxableAmount"] = hdnTaxableammount.Value;
                        dt.Rows[i]["Total"] = NetAmount.ToString("0.00");
                        netamount.Text = NetAmount.ToString("0.00");
                        lblTotal.Text = NetAmount.ToString("0.00");
                    }
                    else
                    {
                        if (Basic > 0)
                        {
                            txtSecDiscValue.Text = "0.00";
                            txtSecDiscPer.Text = "0.00";
                            string message = "alert('Invoice Line value should not be less than zero!!');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        }
                    }
                    if (txtTDValue.Text != "")
                    {
                        TDCalculation();
                    }

                    GrandTotal();
                    UpdateSession();
                    DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                    if (dtApplicable.Rows.Count > 0)
                    {
                        intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                    }
                    if (intApplicable == 1 || intApplicable == 3)
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
                        DataTable dt1 = new DataTable();
                        if (Session["InvLineItem"] == null)
                        {
                            AddColumnInDataTable();
                        }
                        else
                        {
                            dt1 = (DataTable)Session["InvLineItem"];
                        }
                        
                        GridViewFooterCalculate(dt1);


                    }
                    //btnApply_Click(sender, e);
                }
                //dt = (DataTable)Session["InvLineItem"];
                //Session["InvLineItem"] = dt;
                

                //DataTable dt = new DataTable();
               

                Upnel.Update();
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }

        }      
        protected void txtSecDiscPer_TextChanged(object sender, EventArgs e)
        {
            if (txtSecDiscValue.Text != string.Empty)
            {
                txtSecDiscValue.Text = string.Empty;
                btnGO.Focus();
            }
        }
        protected void txtSecDiscValue_TextChanged(object sender, EventArgs e)
        {
            if (txtSecDiscPer.Text != string.Empty)
            {
                txtSecDiscPer.Text = string.Empty;
                btnGO.Focus();
            }
        }
        protected void SecCalculation()
        {
           // decimal SecDiscPer = 0, SecDiscValue = 0;

        }
        protected void SecDisc_TextChanged(object sender, EventArgs e)
        {
            decimal SecDiscPer = 0, SecDiscValue = 0, SchemeDiscVal = 0;

            TextBox SDiscPer = (TextBox)sender;
            GridViewRow gv = (GridViewRow)SDiscPer.Parent.Parent;

            if (string.IsNullOrEmpty(SDiscPer.Text))
            {
                SDiscPer.Text = "0";
            }

            SecDiscPer = Convert.ToDecimal(SDiscPer.Text);

            int idx = gv.RowIndex;

            HiddenField discType = (HiddenField)gv.FindControl("hDiscType");
            HiddenField discCalculation = (HiddenField)gv.FindControl("hDiscCalculationType");
            Label lblMRP = (Label)gv.FindControl("MRP");

            HiddenField ProductCode = (HiddenField)gv.FindControl("HiddenField2");
            TextBox Invoice_Qty = (TextBox)gv.FindControl("txtBox");
            TextBox txtRate = (TextBox)gv.FindControl("txtRate");
            Label DiscPer = (Label)gv.FindControl("Disc");
            Label DiscVal = (Label)gv.FindControl("DiscValue");
            Label tax = (Label)gv.FindControl("Tax");
            Label taxvalue = (Label)gv.FindControl("TaxValue");
            Label AddtaxPer = (Label)gv.FindControl("AddTax");
            Label AddTaxVal = (Label)gv.FindControl("AddTaxValue");
            Label netamount = (Label)gv.FindControl("Amount");
            TextBox SDiscVal = (TextBox)gv.FindControl("SecDiscValue");
            Label lblSchemeDiscVal = (Label)gv.FindControl("lblSchemeDiscVal");
            if (lblSchemeDiscVal.Text.Trim().Length > 0)
            {
                SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
            }
            Label lblTotal = (Label)gv.FindControl("Total");
            boolDiscAvalMRP = false;

            HiddenField hdnTaxableammount = (HiddenField)gv.FindControl("hdnTaxableAmount");
            decimal Basic = Convert.ToDecimal(Invoice_Qty.Text) * Convert.ToDecimal(txtRate.Text);
            if (SecDiscPer != 0)
            {
                SecDiscValue = (SecDiscPer * Basic) / 100;
            }
       
            //==============================
            decimal LineAmount = Basic - (SecDiscValue + (SchemeDiscVal) + Convert.ToDecimal(DiscVal.Text));
            decimal TaxValue = (LineAmount * Convert.ToDecimal(tax.Text)) / 100;
            hdnTaxableammount.Value = LineAmount.ToString();
            decimal AddTaxValue = 0;
            DataTable dtTax = new DataTable();
            /*----------------GST IMPLEMENTATION-------------------*/
            //if (txtBillToState.Text == "JK")
            //{

            //    dtTax = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ProductCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
            //    if (dtTax.Rows.Count > 0)
            //    {
            //        if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
            //        {
            //            AddTaxValue = (TaxValue * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            //        }
            //        else if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 1) // Tax On Taxable Amount
            //        {
            //            AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            //        }
            //        else
            //        {
            //            AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            //        }
            //    }
            //}
            //else
            //{
                AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            //}

            // (LineAmount * Convert.ToDecimal(AddTaxVal.Text)) / 100;

            decimal NetAmount = LineAmount + TaxValue + AddTaxValue;
            if (NetAmount > 0)
            {
                taxvalue.Text = TaxValue.ToString("0.00");
                AddTaxVal.Text = AddTaxValue.ToString("0.00");
                SDiscPer.Text = SecDiscPer.ToString("0.00");
                SDiscVal.Text = SecDiscValue.ToString("0.00");
                netamount.Text = NetAmount.ToString("0.00");
                lblTotal.Text = NetAmount.ToString("0.00");
            }
            else
            {
                if (Basic > 0)
                {
                    SDiscPer.Text = "0.00";
                    SDiscVal.Text = "0.00";
                    SecDiscValue_TextChanged(sender, e);
                    string message = "alert('Invoice Line Value should not be less than zero!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
            }
            if (txtTDValue.Text != "")
            {
                TDCalculation();
            }
            GrandTotal();
            UpdateSession();
        }
        protected void SecDiscValue_TextChanged(object sender, EventArgs e)
        {
            decimal SecDiscPer = 0, SecDiscValue = 0, SchemeDiscVal = 0;

            TextBox SDiscVal = (TextBox)sender;
            GridViewRow gv = (GridViewRow)SDiscVal.Parent.Parent;

            if (string.IsNullOrEmpty(SDiscVal.Text))
            {
                SDiscVal.Text = "0";
            }

            SecDiscValue = Convert.ToDecimal(SDiscVal.Text);

            int idx = gv.RowIndex;
            HiddenField discType = (HiddenField)gv.FindControl("hDiscType");
            HiddenField discCalculation = (HiddenField)gv.FindControl("hDiscCalculationType");
            Label lblMRP = (Label)gv.FindControl("MRP");

            HiddenField ProductCode = (HiddenField)gv.FindControl("HiddenField2");
            TextBox Invoice_Qty = (TextBox)gv.FindControl("txtBox");
            TextBox txtRate = (TextBox)gv.FindControl("txtRate");
            Label DiscPer = (Label)gv.FindControl("Disc");
            Label DiscVal = (Label)gv.FindControl("DiscValue");
            Label tax = (Label)gv.FindControl("Tax");
            Label taxvalue = (Label)gv.FindControl("TaxValue");
            Label AddtaxPer = (Label)gv.FindControl("AddTax");
            Label AddTaxVal = (Label)gv.FindControl("AddTaxValue");
            Label netamount = (Label)gv.FindControl("Amount");
            TextBox SDiscPer = (TextBox)gv.FindControl("SecDisc");
            Label lblSchemeDiscVal = (Label)gv.FindControl("lblSchemeDiscVal");
            if (lblSchemeDiscVal.Text.Trim().Length > 0)
            {
                SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
            }
            Label lblTotal = (Label)gv.FindControl("Total");

            decimal Basic = Convert.ToDecimal(Invoice_Qty.Text) * Convert.ToDecimal(txtRate.Text);


            decimal LineAmount = Basic - (SecDiscValue + (SchemeDiscVal) + Convert.ToDecimal(DiscVal.Text));
            HiddenField hdnTaxableammount = (HiddenField)gv.FindControl("hdnTaxableAmount");
            hdnTaxableammount.Value = LineAmount.ToString();
            decimal TaxValue = (LineAmount * Convert.ToDecimal(tax.Text)) / 100;

            decimal AddTaxValue = 0;
            //if (txtBillToState.Text == "JK")
            //{

                DataTable dtTax = new DataTable();
            ////    dtTax = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ProductCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");

            ////    if (dtTax.Rows.Count > 0)
            ////    {
            ////        if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
            ////        {
            ////            AddTaxValue = (TaxValue * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            ////        }
            ////        else if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 1) // Tax On Taxable Amount
            ////        {
            ////            AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            ////        }
            ////        else
            ////        {
            ////            AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            ////        }
            ////    }
            ////}
            ////else
            ////{
                AddTaxValue = (LineAmount * (Convert.ToDecimal(AddtaxPer.Text) / 100));
            //}
            decimal NetAmount = (LineAmount + TaxValue + AddTaxValue);
            if (NetAmount > 0)
            {
                taxvalue.Text = TaxValue.ToString("0.00");
                AddTaxVal.Text = AddTaxValue.ToString("0.00");
                SDiscPer.Text = SecDiscPer.ToString("0.00");
                SDiscVal.Text = SecDiscValue.ToString("0.00");
                netamount.Text = NetAmount.ToString("0.00");
                lblTotal.Text = NetAmount.ToString("0.00");
            }
            else
            {
                if (Basic > 0)
                {
                    SDiscVal.Text = "0.00";
                    string message = "alert('Invoice Line Value should not be less than zero!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                }
            }

            if (txtTDValue.Text != "")
            {
                TDCalculation();
            }
            GrandTotal();
            UpdateSession();
        }

        protected void txtTDValue_TextChanged(object sender, EventArgs e)
        {

        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
           // btnGO_Click(sender,e);
            if (txtTDValue.Text == "")
            {
                txtTDValue.Text = "0";
            }
            if (txtTDValue.Text != "")
            {            
                TDCalculation();
                GrandTotal();
                UpdateSession();
                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
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
                    DataTable dt1 = new DataTable();
                    if (Session["InvLineItem"] == null)
                    {
                        AddColumnInDataTable();
                    }
                    else
                    {
                        dt1 = (DataTable)Session["InvLineItem"];
                    }
                    
                    GridViewFooterCalculate(dt1);
                }
               
            }
            else
            { }

        }

        public void TDCalculation()
        {
            try
            {
                decimal totalBasicValue = 0, SchemeDiscVal = 0;
                //=========For calculate TD % ================
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    TextBox Invoice_Qty = (TextBox)gv.FindControl("txtBox");
                    TextBox txtRate = (TextBox)gv.FindControl("txtRate");
                    HiddenField ProductCode = (HiddenField)gv.FindControl("HiddenField2");

                    HiddenField discType = (HiddenField)gv.FindControl("hDiscType");
                    HiddenField discCalculation = (HiddenField)gv.FindControl("hDiscCalculationType");
                    Label lblMRP = (Label)gv.FindControl("MRP");
                 
                    totalBasicValue = totalBasicValue + (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(Invoice_Qty.Text));
                  //totalBasicValue = totalBasicValue + (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(Invoice_Qty.Text));
                }
                decimal TDPercentage = Convert.ToDecimal(txtTDValue.Text) / totalBasicValue;
                //=================Apply TD Percentage=================
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    HiddenField discType = (HiddenField)gv.FindControl("hDiscType");
                    HiddenField discCalculation = (HiddenField)gv.FindControl("hDiscCalculationType");
                    Label lblMRP = (Label)gv.FindControl("MRP");

                    HiddenField ProductCode = (HiddenField)gv.FindControl("HiddenField2");

                    TextBox Invoice_Qty = (TextBox)gv.FindControl("txtBox");
                    TextBox txtRate = (TextBox)gv.FindControl("txtRate");
                    Label tax = (Label)gv.FindControl("Tax");
                    decimal TaxPer = Convert.ToDecimal(tax.Text);
                    Label DiscVal = (Label)gv.FindControl("DiscValue");
                    TextBox txtSecDiscVal = (TextBox)gv.FindControl("SecDiscValue");
                    Label lblTD = (Label)gv.FindControl("TD");
                    Label lblPE = (Label)gv.FindControl("PE");
                    Label lblTotal = (Label)gv.FindControl("Total");
                    Label lblToatlBeforeTax = (Label)gv.FindControl("ToatlBeforeTax");
                    Label lblVatAfterPE = (Label)gv.FindControl("VatAfterPE");
                    Label lblSchemeDiscVal = (Label)gv.FindControl("lblSchemeDiscVal");
                    Label taxValue = (Label)gv.FindControl("TaxValue");
                    Label addTaxValue = (Label)gv.FindControl("AddTaxValue");

                    if (lblSchemeDiscVal.Text.Trim().Length > 0)
                    {
                        SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
                    }

                    HiddenField hdnTaxableammount = (HiddenField)gv.FindControl("hdnTaxableAmount");
                    Label Addtax = (Label)gv.FindControl("AddTax");
                    decimal AddTaxPer = Convert.ToDecimal(Addtax.Text);

                    if (txtSecDiscVal.Text == string.Empty)
                    {
                        txtSecDiscVal.Text = "0";
                    }
                    if (Convert.ToDecimal(Invoice_Qty.Text) > 0)
                    {
                        decimal BasicValue = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(Invoice_Qty.Text));
                
                        decimal TD = (BasicValue * TDPercentage);
                        lblTD.Text = TD.ToString("0.0000");
                        decimal PE = 0;// TD * (((TaxPer + AddTaxPer) / 100) / (1 + ((TaxPer + AddTaxPer) / 100)));
                        DataTable dtTax = new DataTable();
                        dtTax = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE,H.ACXADDITIONALTAXBASIS from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ProductCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                        //if (txtBillToState.Text == "JK")
                        //{                            
                        //    if (dtTax.Rows.Count > 0)
                        //    {
                        //        if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
                        //        {
                        //            PE = TD * (((TaxPer + TaxPer * AddTaxPer / 100) / 100) / (1 + ((TaxPer + TaxPer * AddTaxPer / 100) / 100)));

                        //        }
                        //        else  // Tax On Taxable Amount (ACXADDITIONALTAXBASIS=1 or 2)
                        //        {
                        //            PE = TD * (((TaxPer + AddTaxPer) / 100) / (1 + ((TaxPer + AddTaxPer) / 100)));
                        //        }
                        //    }
                        //}
                        //else
                        //{
                            PE = TD * (((TaxPer + AddTaxPer) / 100) / (1 + ((TaxPer + AddTaxPer) / 100)));
                        ////}
                        lblPE.Text = PE.ToString("0.00");
                        decimal BeforeTaxTotal = BasicValue - SchemeDiscVal - Convert.ToDecimal(DiscVal.Text) - Convert.ToDecimal(txtSecDiscVal.Text) - TD + PE;
                        hdnTaxableammount.Value = BeforeTaxTotal.ToString();
                        lblToatlBeforeTax.Text = BeforeTaxTotal.ToString("0.00");
                        taxValue.Text = Convert.ToDecimal((BeforeTaxTotal * TaxPer) / 100).ToString("0.00");
                        addTaxValue.Text = Convert.ToDecimal((BeforeTaxTotal * AddTaxPer) / 100).ToString("0.00");
                        decimal VatAfterPE = 0;// BeforeTaxTotal * (TaxPer + AddTaxPer) / 100;
                        //if (txtBillToState.Text == "JK")
                        //{
                        //    if (dtTax.Rows.Count > 0)
                        //    {
                        //        if (Convert.ToInt16(dtTax.Rows[0]["ACXADDITIONALTAXBASIS"].ToString()) == 0) // Tax On Tax
                        //        {
                        //            VatAfterPE = BeforeTaxTotal * (TaxPer + TaxPer * AddTaxPer / 100) / 100;
                        //        }
                        //        else  // Tax On Taxable Amount (ACXADDITIONALTAXBASIS=1 or 2)
                        //        {
                        //            VatAfterPE = BeforeTaxTotal * (TaxPer + AddTaxPer) / 100;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                            VatAfterPE = BeforeTaxTotal * (TaxPer + AddTaxPer) / 100;
                        ////}
                        lblVatAfterPE.Text = VatAfterPE.ToString("0.0000");
                        decimal total = BeforeTaxTotal + VatAfterPE;
                        lblTotal.Text = total.ToString("0.0000");
                        if (Convert.ToDecimal(txtTDValue.Text) == 0 || txtTDValue.Text == string.Empty)
                        {
                            lblToatlBeforeTax.Text = "0.0000";
                            lblVatAfterPE.Text = "0.0000";
                        }
                    }
                    else
                    {
                        lblTD.Text = "0.00";
                        lblPE.Text = "0.00";
                        lblToatlBeforeTax.Text = "0.00";
                        lblVatAfterPE.Text = "0.00";
                        lblTotal.Text = "0.00";
                    }
                }
                //Also Update session
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }
        protected void drpCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void FillProductCode()
        {
            DDLMaterialCode.Items.Clear();
            // DDLMaterialCode.Items.Add("Select...");
            if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
            {
                strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE WHERE BLOCK=0  order by Product_Name";
                DDLMaterialCode.Items.Clear();
                DDLMaterialCode.Items.Add("Select...");
                baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
                DDLMaterialCode.Focus();
            }
        }
        private void CalculateInvoiceValue()
        {
            DataTable dtsum = new DataTable();
            decimal TotalInvValue = 0;
            if (Session["InvLineItem"] != null)
            {
                dtsum = (DataTable)Session["InvLineItem"];
                TotalInvValue = Convert.ToDecimal(dtsum.Compute("Sum(total)", ""));
            }
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                {
                    TotalInvValue += Convert.ToDecimal(rw.Cells[24].Text);
                }
            }
            txtinvoicevalue.Text = TotalInvValue.ToString("0.00");
        }
        protected void UpdateSession()
        {
            decimal SchemeDiscVal = 0;
            foreach (GridViewRow row in gvDetails.Rows)
            {
                HiddenField hdnTaxableAmount = (HiddenField)row.Cells[0].FindControl("hdnTaxableAmount");
                HiddenField hdfLineNo = (HiddenField)row.Cells[0].FindControl("hdfLineNo");
                HiddenField hClaimDiscAmt = (HiddenField)row.FindControl("hClaimDiscAmt");
                Label lblSOQty = (Label)row.Cells[0].FindControl("SO_Qty");
                TextBox txtBox = (TextBox)row.Cells[0].FindControl("txtBox");
                Label lblStockQty = (Label)row.Cells[0].FindControl("StockQty");
                Label lblLtr = (Label)row.Cells[0].FindControl("Ltr");
                TextBox txtRate = (TextBox)row.FindControl("txtRate");
                Label tax = (Label)row.FindControl("Tax");
                Label taxvalue = (Label)row.FindControl("TaxValue");
                Label AddtaxPer = (Label)row.FindControl("AddTax");
                Label AddTaxValue = (Label)row.FindControl("AddTaxValue");
                Label DiscPer = (Label)row.FindControl("Disc");
                Label DiscVal = (Label)row.FindControl("DiscValue");
                TextBox SDiscPer = (TextBox)row.FindControl("SecDisc");
                TextBox SDiscVal = (TextBox)row.FindControl("SecDiscValue");
                Label amount = (Label)row.FindControl("Amount");
                Label lblTotal = (Label)row.FindControl("Total");
                TextBox txtBoxQtyGrid = (TextBox)row.FindControl("txtBoxQtyGrid");
                TextBox txtPcsQtyGrid = (TextBox)row.FindControl("txtPcsQtyGrid");
                Label txtBoxPcs = (Label)row.FindControl("txtBoxPcs");

                Label tdAmount = (Label)row.FindControl("TD");
                Label peAmount = (Label)row.FindControl("PE");
                if (peAmount.Text.Trim().Length == 0) { peAmount.Text = "0.00"; }
                Label lblSchemeDiscVal = (Label)row.FindControl("lblSchemeDiscVal");
                if (lblSchemeDiscVal.Text.Trim().Length > 0)
                {
                    SchemeDiscVal = Convert.ToDecimal(lblSchemeDiscVal.Text);
                }

                decimal SecDiscPer = Convert.ToDecimal(SDiscPer.Text);
                decimal SecDiscValue = Convert.ToDecimal(SDiscVal.Text);

                dtLineItems = (DataTable)Session["InvLineItem"];
                foreach (DataColumn DC in dtLineItems.Columns)
                {
                    DC.ReadOnly = false;
                }
                dtLineItems.Select(string.Format("[Line_No] = '{0}'", hdfLineNo.Value.ToString()))
                .ToList<DataRow>()
                .ForEach(r =>
                {
                    r["So_Qty"] = lblSOQty.Text;
                    r["Invoice_Qty"] = txtBox.Text;
                    r["Box_Qty"] = txtBoxQtyGrid.Text;
                    r["Pcs_Qty"] = txtPcsQtyGrid.Text;
                    r["BoxPcs"] = txtBoxPcs.Text;
                    r["StockQty"] = lblStockQty.Text;
                    r["Ltr"] = lblLtr.Text;
                    r["Rate"] = txtRate.Text;
                    r["DiscVal"] = DiscVal.Text;
                    r["Amount"] = amount.Text;
                    r["TaxValue"] = taxvalue.Text;
                    r["ADDTaxValue"] = AddTaxValue.Text;
                    r["TaxableAmount"] = hdnTaxableAmount.Value.ToString();
                    r["Total"] = lblTotal.Text;
                    r["SecDiscPer"] = SDiscPer.Text;
                    r["SecDiscAmount"] = SDiscVal.Text;
                    r["SchemeDiscVal"] = SchemeDiscVal;
                    r["TD"] = tdAmount.Text;
                    r["PE"] = peAmount.Text;
                    r["ClaimDiscAmt"] = hClaimDiscAmt.Value;
                });
            }
            CalculateInvoiceValue();
        }
        protected void txtLtr_TextChanged(object sender, EventArgs e)
        {

        }
        protected void txtCrateQty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
        }
        protected void txtBoxqty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
        }
        protected void txtPCSQty_TextChanged(object sender, EventArgs e)
        {
            CalculateQtyAmt(sender);
        }
        public void CalculateQtyAmt(Object sender)
        {
            decimal dblTotalQty = 0, crateQty = 0, boxQty = 0, pcsQty = 0, crateSize = 0, boxPackSize = 0;

            crateQty = Global.ParseDecimal(txtCrateQty.Text);
            boxQty = Global.ParseDecimal(txtBoxqty.Text);
            pcsQty = Global.ParseDecimal(txtPCSQty.Text);
            crateSize = Global.ParseDecimal(txtCrateSize.Text);
            boxPackSize = Global.ParseDecimal(txtPack.Text);

            txtCrateQty.Text = Convert.ToString(crateQty);            
            txtBoxqty.Text = Convert.ToString(boxQty);            
            txtPCSQty.Text = Convert.ToString(pcsQty);
            
            dblTotalQty = crateQty * crateSize + boxQty + (pcsQty / (boxPackSize == 0 ? 1 : boxPackSize));  //Total qty (Create+box+pcs) with decimal factor
            //txtEnterQty.Text = dblTotalQty.ToString("0.00");          //Total Qty 
            txtQtyBox.Text = dblTotalQty.ToString("0.000000");              // txtEnterQty.Text; //Total Qty

            txtStockQty.Text = (txtStockQty.Text.Trim().Length == 0 ? "0" : txtStockQty.Text);

            decimal TotalBox = 0, TotalPcs = 0;
            TotalBox = Math.Truncate(dblTotalQty);                          //Extract Only Box Qty From Total Qty
            TotalPcs = Math.Round((dblTotalQty - TotalBox) * boxPackSize);  //Extract Only Pcs Qty From Total Qty
            string  BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);
            // Call CalculatePrice 
            txtStockQty.Text = (txtStockQty.Text.Trim().Length == 0 ? "0" : txtStockQty.Text);
            txtQtyBox.Text = (txtQtyBox.Text.Trim().Length == 0 ? "0" : txtQtyBox.Text);

            try
            {
                string[] calValue = baseObj.CalculatePrice1(DDLMaterialCode.SelectedItem.Value, drpCustomerCode.SelectedItem.Value, dblTotalQty, "Box");
                if (calValue[0] != "")
                {
                    
                    if (calValue[5] == "Box")
                    {
                        txtBoxPcs.Text = BoxPcs;
                        txtViewTotalBox.Text = TotalBox.ToString("0.00");
                        txtViewTotalPcs.Text = TotalPcs.ToString("0.00");
                       // txtQtyBox.Text = txtEnterQty.Text;
                        txtQtyCrates.Text = calValue[0];
                        if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
                        {                            
                            string message = "alert('Stock not available !!');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);

                            return;
                        }
                    }

                    txtLtr.Text = calValue[1];
                    txtPrice.Text = calValue[2];
                    txtValue.Text = calValue[3];
                    lblHidden.Text = calValue[4];
                    BtnAddItem.Focus();
                    BtnAddItem.CausesValidation = false;
                }
                else
                {
                    lblMessage.Text = "Price Not Define !";
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
            }
        }
        protected void txtBoxQtyGrid_TextChanged(object sender, EventArgs e)
        {
         CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            TextBox txtBoxQty = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtBoxQty.Parent.Parent;
            
            HiddenField ddlPrCode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
            TextBox txtPcs = (TextBox)row.Cells[0].FindControl("txtPcsQtyGrid");
            TextBox textTotal = (TextBox)row.Cells[0].FindControl("txtBox");
            if(txtBoxQty.Text=="")
            {
                txtBoxQty.Text = "0";
            }
            decimal TotalBox;
            TotalBox = obj.GetTotalBox(ddlPrCode.Value, 0, Convert.ToDecimal(txtPcs.Text), Convert.ToDecimal(txtBoxQty.Text));
            textTotal.Text = Convert.ToString(TotalBox);

            
            //if (dtLineItems != null && dtLineItems.Rows.Count > 0)
            //{
            //    DataRow[] dr = dtLineItems.Select("Line_No=" + hdfLineNo.Value.ToString());
                
            //}
            
            txtBox_TextChanged((object)textTotal, e);
        }
        protected void txtPcsQtyGrid_TextChanged(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            TextBox txtPcsQty = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtPcsQty.Parent.Parent;
           
            HiddenField ddlPrCode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
            TextBox txtBoxQty = (TextBox)row.Cells[0].FindControl("txtBoxQtyGrid");
            TextBox textTotal = (TextBox)row.Cells[0].FindControl("txtBox");

            decimal TotalBox;
            TotalBox = obj.GetTotalBox(ddlPrCode.Value, 0, Convert.ToDecimal(txtPcsQty.Text), Convert.ToDecimal(txtBoxQty.Text));
         
            textTotal.Text = Convert.ToString(TotalBox);
            txtBox_TextChanged((object)textTotal, e);

        }        
        protected void GridLevelCalculation()
        {

        }
        public void PcsBillingApplicable()
        {

            DataTable dt = baseObj.GetPcsBillingApplicability(Session["SCHSTATE"].ToString(), DDLProductGroup.SelectedItem.Value);  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
            string ProductGroupApplicable = string.Empty;

            if (dt.Rows.Count > 0)
            {
                ProductGroupApplicable = dt.Rows[0][1].ToString();
            }
            if (ProductGroupApplicable == "Y")
            {
                txtPCSQty.Enabled = true;
            }
            else
            {
                txtPCSQty.Enabled = false;
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
            this.SchemeDiscCalculation();
            DataTable dt1 = new DataTable();
            if (Session["InvLineItem"] == null)
            {
                AddColumnInDataTable();
            }
            else
            {
                dt1 = (DataTable)Session["InvLineItem"];
            }
            
            GridViewFooterCalculate(dt1);
        }

        protected void ddlShipToAddress_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected decimal SchemeLineCalculation(decimal discPer)
        {
            decimal Rate = 0, Qty = 0, BasicAmount = 0, DiscPer = 0, packSize = 0;
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
                    packSize = Convert.ToDecimal(rw.Cells[13].Text);

                    Qty = Convert.ToDecimal(txtQty.Text) + (Convert.ToDecimal(txtQtyPCS.Text) > 0 ? Convert.ToDecimal(txtQtyPCS.Text) / packSize : 0);
                    Rate = Convert.ToDecimal(rw.Cells[15].Text);
                    rw.Cells[14].Text = Convert.ToString(Qty.ToString("0.0000"));
                    rw.Cells[16].Text = Convert.ToString((Rate * Qty).ToString("0.0000"));
                    BasicAmount = (Rate * Qty);
                    //rw.Cells[16].Text = Convert.ToString(Rate * Qty);
                    rw.Cells[17].Text = (discPer * 100).ToString("0.00");
                    DiscAmt = BasicAmount * discPer;
                    rw.Cells[18].Text = DiscAmt.ToString("0.00");
                    rw.Cells[19].Text = (BasicAmount - DiscAmt).ToString("0.0000");
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
                    TaxAmount = Math.Round(((BasicAmount - DiscAmt) * TaxPer) / 100, 2);
                    AddTaxAmount = Math.Round(((BasicAmount - DiscAmt) * AddTaxPer) / 100, 2);
                    rw.Cells[21].Text = TaxAmount.ToString("0.00");
                    rw.Cells[23].Text = AddTaxAmount.ToString("0.00");
                    rw.Cells[24].Text = (BasicAmount + TaxAmount + AddTaxAmount - DiscAmt).ToString("0.00");
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
                dtLineItems = (DataTable)Session["InvLineItem"];
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
                dtLineItems.Columns["TaxableAmount"].ReadOnly = false;
                dtLineItems.Columns["TaxValue"].ReadOnly = false;
                dtLineItems.Columns["AddTaxValue"].ReadOnly = false;
                dtLineItems.Columns["TaxableAmount"].ReadOnly = false;
                dtLineItems.Columns["Total"].ReadOnly = false;
                dtLineItems.Columns["SchemeDisc"].ReadOnly = false;
                dtLineItems.Columns["SchemeDiscVal"].ReadOnly = false;
                for (int i = 0; i < dtLineItems.Rows.Count; i++)
                {
                    lineAmount = Convert.ToDecimal(dtLineItems.Rows[i]["Invoice_Qty"].ToString()) * Convert.ToDecimal(dtLineItems.Rows[i]["Rate"].ToString());
                    DiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["DiscVal"].ToString());
                    SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SecDiscAmount"].ToString());
                    TDAmount = Convert.ToDecimal(dtLineItems.Rows[i]["TD"].ToString());
                    PEAmount = Convert.ToDecimal(dtLineItems.Rows[i]["PE"].ToString());
                    //dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                    SchemeDiscPer = (DiscPer * 100);
                    SchemeDiscAmount = Math.Round((lineAmount * DiscPer), 6);
                    dtLineItems.Rows[i]["SchemeDisc"] = (DiscPer * 100).ToString("0.000000");
                    dtLineItems.Rows[i]["SchemeDiscVal"] = SchemeDiscAmount.ToString("0.000000");//.ToString("0.000000");
                    TaxableAmount = (lineAmount - DiscAmount - SchemeDiscAmount - SecDiscAmount - TDAmount + PEAmount);
                    dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.00");
                    if (dtLineItems.Rows[i]["TaxPer"].ToString().Trim().Length == 0)
                    {
                        TaxPer = 0;
                    }
                    else
                    {
                        TaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["TaxPer"]);
                    }
                    if (dtLineItems.Rows[i]["AddTaxPer"].ToString().Trim().Length == 0)
                    {
                        AddTaxPer = 0;
                    }
                    else
                    {
                        AddTaxPer = Convert.ToDecimal(dtLineItems.Rows[i]["AddTaxPer"]);
                    }
                    
                    TaxAmount = Math.Round(TaxableAmount * TaxPer / 100, 6);
                    AddTaxAmount = Math.Round(TaxableAmount * AddTaxPer / 100, 6);
                    //if (dtLineItems.Rows[i]["SecDiscAmount"].ToString().Trim().Length == 0)
                    //{
                    //    SecDiscAmount = new decimal(0);
                    //}
                    //else
                    //{
                    //    SecDiscAmount = Convert.ToDecimal(dtLineItems.Rows[i]["SecDiscAmount"]);
                    //}
                    dtLineItems.Rows[i]["TaxValue"] = TaxAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["AddTaxValue"] = AddTaxAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["TaxableAmount"] = TaxableAmount.ToString("0.000000");
                    dtLineItems.Rows[i]["Total"] = (TaxableAmount + TaxAmount + AddTaxAmount).ToString("0.000000");
                }
                dtLineItems.AcceptChanges();
                Session["InvLineItem"] = dtLineItems;
                gvDetails.DataSource = dtLineItems;
                gvDetails.DataBind();
                //#endregion
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

    }

}