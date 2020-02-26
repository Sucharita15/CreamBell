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
    public partial class frmFOCInvoiceSave : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        string strmessage = string.Empty;
        public DataTable dtLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1, cmd2, cmd3;
        SqlTransaction transaction;
        string strQuery = string.Empty;
        int intApplicable;
        Boolean boolDiscAvalMRP = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            BtnAddItem.Focus();
            if (!IsPostBack)
            {
                Session["LineItem"] = null;
                ProductGroup();
                FillProductCode();
                txtInvoiceDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");


                //                if (Cache["SO_NO"] != null)
                //                {
                //                    string sono = Cache["SO_NO"].ToString();
                //                    //Cache.Remove("SO_NO");
                //                    if (Session["SiteCode"] != null)
                //                    {
                //                        string siteid = Session["SiteCode"].ToString();
                //                        if (lblsite.Text == "")
                //                        {
                //                            lblsite.Text = siteid;
                //                        }

                //                        //==================For Warehouse Location==============
                //                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                //                        string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                //                        DataTable dt = new DataTable();
                //                        dt = obj.GetData(query1);
                //                        if (dt.Rows.Count > 0)
                //                        {
                //                            Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                //                        }
                //                    }
                //                    else
                //                    {
                //                        string siteid = "Select A.SiteId From ax.ACXLOADSHEETHEADER A where So_No in " + sono + "";
                //                        DataTable dt = new DataTable();
                //                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                //                        dt = obj.GetData(siteid);
                //                        if (dt.Rows.Count > 0)
                //                        {
                //                            lblsite.Text = dt.Rows[0]["SiteId"].ToString();
                //                        }

                //                    }

                //                    if (sono != "")
                //                    {
                //                        ShowData(sono);
                //                    }

                //                    txtInvoiceDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);
                //                    //CalendarExtender3.StartDate = DateTime.Now;

                //                    // For Shceme Seletion
                //                    DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                //                    if (dtApplicable.Rows.Count > 0)
                //                    {
                //                        intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                //                    }
                //                    if (intApplicable == 1 || intApplicable == 3)
                //                    {
                //                        BindSchemeGrid();
                //                    }

                //                    string strQuery = @"Select SchemeCode,Product_Code as ItemCode,cast(BOX as decimal(10,2)) as Qty,Slab from [ax].[ACXSALESLINE] 
                //                                    Where SO_NO in ('" + sono + "') and SchemeCode<>'' and DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";
                //                    DataTable dtScheme = baseObj.GetData(strQuery);

                //                    if (dtScheme.Rows.Count > 0)
                //                    {
                //                        for (int i = 0; i <= dtScheme.Rows.Count - 1; i++)
                //                        {
                //                            GetSelectedShemeItemChecked(dtScheme.Rows[i]["SchemeCode"].ToString(), dtScheme.Rows[i]["ItemCode"].ToString(), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["Qty"].ToString())), Convert.ToInt16(Convert.ToDouble(dtScheme.Rows[i]["Slab"].ToString())));
                //                        }
                //                    }
                //                }
                //                else
                //                {
                if (Session["SiteCode"] != null)
                {
                    string siteid = Session["SiteCode"].ToString();
                    if (lblsite.Text == "")
                    {
                        lblsite.Text = siteid;
                    }
                    CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                    string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session["SiteCode"].ToString() + "'";
                    DataTable dt = new DataTable();
                    dt = obj.GetData(query1);
                    if (dt.Rows.Count > 0)
                    {
                        Session["TransLocation"] = dt.Rows[0]["MainWarehouse"].ToString();
                    }
                    ShowDataForFOC();
                }
                //}
            }
        }

        #region|BIND CUSTGROUP|
        private void ShowDataForFOC()
        {
            string strProductGroup = "Select distinct CUSTGROUP_CODE +'-'+ CUSTGROUP_NAME as CUSTGROUP_NAME, CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER";
            drpCustomerGroup.Items.Clear();
            drpCustomerGroup.Items.Add("Select...");
            baseObj.BindToDropDown(drpCustomerGroup, strProductGroup, "CUSTGROUP_NAME", "CUSTGROUP_CODE");
            drpCustomerGroup.Focus();
        }
        protected void drpCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strProductGroup = "Select distinct CUSTOMER_CODE, CUSTOMER_CODE + '-' + CUSTOMER_NAME as Name from ax.Acxcustmaster where Blocked = 0 AND CUST_GROUP='" + drpCustomerGroup.SelectedValue.ToString() + "' and SITE_CODE ='" + Session["SiteCode"].ToString() + "' ";
            drpCustomerCode.Items.Clear();
            drpCustomerCode.Items.Add("Select...");
            baseObj.BindToDropDown(drpCustomerCode, strProductGroup, "Name", "CUSTOMER_CODE");
            drpCustomerCode.Focus();
        }
        #endregion

        private void ShowData(string sono)
        {
            DataTable dt = new DataTable();
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

            string query = " select A.SO_No,CONVERT(VARCHAR(11),A.SO_Date,106) as SO_Date ,A.Loadsheet_No,CONVERT(VARCHAR(11),L.LoadSheet_Date,106) as LoadSheet_Date, " +
                       "B.CUST_GROUP,Customer_Group=(Select C.CUSTGROUP_NAME from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=B.CUST_GROUP)," +
                       "B.Customer_Code,concat(B.Customer_Code,'-', B.Customer_Name) as Customer_Name," +
                       " B.Address1,B.Mobile_No,B.VAT" +
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
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
            }


            //query = "select Row_Number() over (order by Product_Code,Rate) Line_No, Product_Code,Product,Pack,Mrp,DiscCalculationBase,sum(SO_qty) as SO_qty,sum(Ltr)  ltr,sum(Invoice_Qty)  Invoice_Qty,Rate,Sum(Amount) Amount,Sum(Total) Total,Sum(TD) as TD,TaxPer,Sum(TaxValue) TaxValue,AddTaxPer,Sum(AddTaxValue) AddTaxValue,StockQty,Disc,Sum(DiscVal) DiscVal,DiscType,Schemecode from (" +
            //            "Select E.ItemId as Product_Code,A.Line_No,E.ItemId+'-'+E.Product_Name as Product,coalesce(cast(E.Product_PackSize as decimal(10,2)),0) as Pack," +
            //            "coalesce(cast(E.Product_MRP as decimal(10,2)),0) as MRP,A.DiscCalculationBase,coalesce(cast(A.BOX as decimal(10,2)),0) as So_Qty,coalesce(cast(A.Ltr as decimal(10,2)),0) as Ltr ,coalesce(cast(A.BOX as decimal(10,2)),0) as Invoice_Qty  " +
            //            ", Rate=(select top 1 coalesce(cast(t.Amount as decimal(10,2)),0) from DBO.PriceDiscTable t where  A.product_Code=t.ITEmRelation and t.ACCOUNTRELATION = D.PriceGroup and t.FromDate<=getdate() order by t.FromDate desc) " +
            //            " , cast(A.amount as decimal(10,2)) as Amount " +
            //            " , cast(A.amount as decimal(10,2)) as Total ,TD=0.00 " +
            //            " ,coalesce(cast(A.Tax_Code as decimal(10,2)),0) as TaxPer   " +
            //            " ,TaxValue= coalesce(cast(A.Tax_Amount as decimal(10,2)),0)  " +
            //            " ,coalesce(cast(A.ADDTax_Code as decimal(10,2)),0) as ADDTaxPer  " +
            //            " ,ADDTaxValue= coalesce(cast(A.ADDTax_Amount as decimal(10,2)),0) " +
            //            " ,StockQty=(Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]=A.SITEID and F.[ProductCode]=A.Product_COde and F.[TransLocation]='" + Session["TransLocation"].ToString() + "')   " +
            //            " , A.SchemeCode,coalesce(cast(A.Disc as decimal(10,2)),0) As Disc,coalesce(cast(A.DiscVal as decimal(10,2)),0) DiscVal,DiscType  " +
            //            "  from [ax].[ACXSALESLINE] A " +
            //            " left join [ax].[ACXSALESHEADER] B on A.SO_No=B.SO_No and A.SITEID = B.SITEID " +
            //            " left join ax.Acxcustmaster D on D.Customer_Code=B.Customer_Code " +
            //            " left Join ax.InventTable E on A.Product_COde=E.ItemId  " +
            //            " inner join [ax].inventsite G on G.Siteid=A.siteid " +
            //            " Left join InventTax H on G.TaxGroup=H.TaxGroup and H.ItemId=E.ItemId" +
            //            " Where B.SO_NO in (" + sono + ") and A.SITEID='" + lblsite.Text + "'  and SchemeCode='') A " +
            //            " group by Product_Code,Product,Pack,Mrp,DiscCalculationBase,Rate,TaxPer,AddTaxPer,StockQty,Disc,DiscType,Schemecode ";

            conn = baseObj.GetConnection();
            cmd2 = new SqlCommand("AX.ACX_SOTOINVOICECREATION");
            transaction = conn.BeginTransaction();
            cmd2.Connection = conn;
            cmd2.Transaction = transaction;
            cmd2.CommandTimeout = 3600;
            cmd2.CommandType = CommandType.StoredProcedure;
            dt = new DataTable();
            // dt = obj.GetData(query);
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@TransLocation", Session["TransLocation"].ToString());
            cmd2.Parameters.AddWithValue("@siteid", lblsite.Text);
            cmd2.Parameters.AddWithValue("@so_no", sono);
            dt.Load(cmd2.ExecuteReader());

            transaction.Commit();

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
                Session["LineItem"] = dt;
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

        protected void BindGridview()
        {
            try
            {

                DataTable dt = new DataTable();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                string query;

                query = "Select E.ItemId as Product_Code,A.Line_No,E.ItemId+'-'+E.Product_Name as Product,cast(E.Product_PackSize as decimal(10,2)) as Pack," +
                              "cast(E.Product_MRP as decimal(10,2)) as MRP,cast(A.BOX as decimal(10,2)) as So_Qty,cast(A.Ltr as decimal(10,2)) as Ltr ,cast(A.BOX as decimal(10,2)) as Invoice_Qty " +
                    //",coalesce(cast(t.Amount as decimal(10,2)),0) as Rate,cast(t.Amount*A.BOX*.125 as decimal(10,2)) as TaxValue,cast((t.Amount*A.BOX)  + (t.Amount*A.BOX*.125) as decimal(10,2)) Amount " +
                             " ,Rate=(select top 1 cast(t.Amount as decimal(10,2)) from DBO.PriceDiscTable t where  A.product_Code=t.ITEmRelation and t.ACCOUNTRELATION = D.PriceGroup and getdate() between t.FromDate and t.ToDate order by t.ModifiedDatetime desc) " +
                             " ,TaxValue=(select top 1 cast(t.Amount*A.BOX*.125 as decimal(10,2))  from DBO.PriceDiscTable t where A.product_Code=t.ITEmRelation and t.ACCOUNTRELATION = D.PriceGroup and getdate() between t.FromDate and t.ToDate order by t.ModifiedDatetime desc)" +
                             " ,Amount=(select top 1 cast(t.Amount*A.BOX as decimal(10,2)) from DBO.PriceDiscTable t where A.product_Code=t.ITEmRelation and t.ACCOUNTRELATION = D.PriceGroup and getdate() between t.FromDate and t.ToDate order by t.ModifiedDatetime desc)" +
                              " ,StockQty=(Select cast(sum(F.TransQty) as decimal(10,2)) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]=A.SITEID and F.[ProductCode]=A.Product_COde and F.[TransLocation]='" + Session["TransLocation"].ToString() + "')   " +
                             "  from [ax].[ACXSALESLINE] A " +
                              " left join [ax].[ACXSALESHEADER] B on A.SO_No=B.SO_No " +
                              " left join ax.Acxcustmaster D on D.Customer_Code=B.Customer_Code " +
                              " left Join ax.InventTable E on A.Product_COde=E.ItemId  " +
                    // " inner JOin DBO.PriceDiscTable t on A.product_Code=t.ITEmRelation and t.ACCOUNTRELATION = D.PriceGroup" +
                              " Where B.SO_NO='" + drpSONumber.SelectedItem.Text + "' and A.SITEID='" + lblsite.Text + "'" +
                    //" and getdate() between t.FromDate and t.ToDate " +
                              " Order By A.SO_No,A.Line_No";


                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    GridViewFooterCalculate(dt);
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    int columncount = gvDetails.Rows[0].Cells.Count;
                    gvDetails.Rows[0].Cells.Clear();
                    gvDetails.Rows[0].Cells.Add(new TableCell());
                    gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                    gvDetails.Rows[0].Cells[0].Text = "No Records Found";
                }

            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }

        }

        //protected void txtRate_TextChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    TextBox txtRate = (TextBox)sender;
        //    GridViewRow row = (GridViewRow)txtRate.Parent.Parent;

        //    int idx = row.RowIndex;
        //    TextBox qty = (TextBox)row.Cells[0].FindControl("txtBox");
        //    Label soqty = (Label)row.Cells[0].FindControl("SO_Qty");
        //    Label amt = (Label)row.Cells[0].FindControl("Amount");
        //    Label tax = (Label)row.Cells[0].FindControl("Tax");
        //    Label taxvalue = (Label)row.Cells[0].FindControl("TaxValue");


        //    if (Convert.ToDecimal(txtRate.Text) <= 0)
        //    {

        //        return;
        //    }
        //    if (qty.Text == "")
        //    {
        //        decimal taxValue = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(soqty.Text) * (Convert.ToDecimal(tax.Text) / 100));
        //        decimal amount = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(soqty.Text)) + taxValue;

        //        taxvalue.Text = taxValue.ToString("#.##");
        //        amt.Text = amount.ToString("#.##");

        //    }
        //    else
        //    {
        //        decimal taxValue = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(qty.Text) * (Convert.ToDecimal(tax.Text) / 100));
        //        decimal amount = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(qty.Text)) + taxValue;

        //        // decimal amount = Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(qty.Text);
        //        taxvalue.Text = taxValue.ToString("#.##");
        //        amt.Text = amount.ToString("#.##");
        //    }


        //}

        protected void txtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                TextBox txtB = (TextBox)sender;
                GridViewRow row = (GridViewRow)txtB.Parent.Parent;
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                string query;
                int idx = row.RowIndex;
                // Label amt = (Label)row.Cells[0].FindControl("Amount");HiddenFieldLineNo
                HiddenField HiddenFieldLineNo = (HiddenField)row.Cells[0].FindControl("HiddenFieldLineNo");

                string lblLine_No = HiddenFieldLineNo.Value.ToString();
                Label hdInvoice_Qty = (Label)row.FindControl("hdInvoice_Qty");

                HiddenField ddlPrCode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
                HiddenField discType = (HiddenField)row.FindControl("hDiscType");
                HiddenField discCalculation = (HiddenField)row.FindControl("hDiscCalculationType");
                Label lblLtr = (Label)row.FindControl("LTR");
                Label amount = (Label)row.FindControl("Amount");
                Label txtRate = (Label)row.FindControl("txtRate");
                Label tax = (Label)row.FindControl("Tax");
                Label taxvalue = (Label)row.FindControl("TaxValue");
                Label AddtaxPer = (Label)row.FindControl("AddTax");
                Label AddTaxValue = (Label)row.FindControl("AddTaxValue");
                Label StockQty = (Label)row.FindControl("StockQty");
                Label SO_Qty = (Label)row.FindControl("SO_Qty");
                Label DiscPer = (Label)row.FindControl("Disc");
                Label DiscVal = (Label)row.FindControl("DiscValue");
                Label lblMRP = (Label)row.FindControl("MRP");
                Label lblVALUE = (Label)row.FindControl("MRPVALUE");
                Label lblTotal = (Label)row.FindControl("Total");

                TextBox txtBoxQtyGrid = (TextBox)row.FindControl("txtBoxQtyGrid");
                TextBox txtPcsQtyGrid = (TextBox)row.FindControl("txtPcsQtyGrid");
                TextBox txtBoxTotal = (TextBox)row.FindControl("txtBox"); //TotalBox
                Label lblPack = (Label)row.FindControl("Pack");
                //if (Convert.ToDecimal(txtPcsQtyGrid.Text) > Convert.ToDecimal(lblPack.Text))
                //{
                int pcs = Convert.ToInt32(Convert.ToDecimal(txtPcsQtyGrid.Text));
                int pac = Convert.ToInt32(Convert.ToDecimal(lblPack.Text));
                int addbox = pcs / pac;
                int remainder = pcs % pac;
                if (remainder == 0)
                {
                    txtPcsQtyGrid.Text = "0";
                }
                else
                {
                    txtPcsQtyGrid.Text = Convert.ToString(remainder);
                }
                //txtBoxQtyGrid.Text = Convert.ToString(Convert.ToInt16(txtBoxQtyGrid.Text) + addbox);
                txtBoxQtyGrid.Text = Convert.ToString(Convert.ToInt32(Convert.ToDouble(txtBoxQtyGrid.Text)) + addbox);
                //}

                Label txtBoxPcs = (Label)row.FindControl("txtBoxPcs");
                int BoxQty = (txtBoxQtyGrid.Text.Trim() == "" ? 0 : Convert.ToInt32(Convert.ToDouble(txtBoxQtyGrid.Text)));
                txtPcsQtyGrid.Text = (Convert.ToInt32(Convert.ToDouble(txtPcsQtyGrid.Text)) == 0 ? "0" : txtPcsQtyGrid.Text);


                decimal TotalBox = 0, TotalPcs = 0;
                TotalBox = Math.Truncate(Convert.ToDecimal(BoxQty));                          //Extract Only Box Qty From Total Qty
                TotalPcs = Math.Truncate(Convert.ToDecimal(txtPcsQtyGrid.Text));  //Extract Only Pcs Qty From Total Qty
                string BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);

                //string BoxPcs = Convert.ToString(BoxQty) + '.' + (Convert.ToInt32(txtPcsQtyGrid.Text).ToString().Length == 1 ? "0" : "") + Convert.ToString(Convert.ToInt32(txtPcsQtyGrid.Text));
                txtBoxPcs.Text = BoxPcs;

                //string BoxPcs = txtBoxQtyGrid.Text + '.' + (txtPcsQtyGrid.Text.Length == 1 ? "0" : "") + Convert.ToString(txtPcsQtyGrid.Text);
                //txtBoxPcs.Text = BoxPcs;

                if (string.IsNullOrEmpty(txtB.Text.Trim()))
                {
                    string message = "alert('Box Qty can not be empty !');";
                    // string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                if (Convert.ToDecimal(txtB.Text.Trim()) == 0)
                {
                    string message = "alert('Box Qty can not be zero!');";
                    txtB.Text = "0";
                    // string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    // return;
                }
                if (string.IsNullOrEmpty(StockQty.Text.Trim()))
                {
                    string message = "alert('Stock Qty can not be empty !');";
                    // string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }
                if (Convert.ToDecimal(txtB.Text) > Convert.ToDecimal(StockQty.Text))
                {
                    //lblLtr.Text = lblLtr.Text;
                    //taxvalue.Text = taxvalue.Text;
                    //amount.Text = amount.Text;
                    row.BackColor = System.Drawing.Color.Red;
                    txtB.Text = SO_Qty.Text;
                    string message = "alert('Box Qty should not greater than Stock Qty !');";
                    // string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                    //return;
                }
                //else
                //{
                //    row.BackColor = System.Drawing.Color.White;
                //}

                if (Convert.ToDecimal(txtB.Text) > 0)
                {

                    if (discType.Value == "0")
                    {
                        if (discCalculation.Value == "0")
                        {
                            DiscVal.Text = (Convert.ToDecimal(DiscPer.Text) * Convert.ToDecimal(txtB.Text) * Convert.ToDecimal(txtRate.Text) / 100).ToString("0.00");
                        }
                        else
                        {
                            DiscVal.Text = (Convert.ToDecimal(DiscPer.Text) * Convert.ToDecimal(txtB.Text) * Convert.ToDecimal(lblMRP.Text) / 100).ToString("0.00");
                        }
                    }
                    if (discType.Value == "1")
                    {
                        DiscVal.Text = (Convert.ToDecimal(DiscPer.Text) * Convert.ToDecimal(txtB.Text)).ToString("0.00");
                        dt = new DataTable();
                        dt = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + ddlPrCode.Value + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            decimal discVal = 0;
                            //discVal = Convert.ToDecimal(DiscVal.Text) - ((Convert.ToDecimal(DiscVal.Text) * Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString())) / 100);
                            discVal = Convert.ToDecimal(DiscVal.Text) / (1 + (Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString()) / 100));
                            DiscVal.Text = Convert.ToString(discVal.ToString("0.00"));
                        }
                    }
                    query = "select  CEILING(" + txtB.Text + "/(CASE WHEN ISNULL(A.Product_Crate_PackSize,0)=0 THEN 1 ELSE A.Product_Crate_PackSize END)) as Crates,(" + txtB.Text + "*A.Product_PackSize*A.Ltr)/1000 as Ltr from ax.InventTable A where A.ItemId='" + ddlPrCode.Value + "'";
                    dt = obj.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        decimal l;
                        //decimal rate, amt;                   
                        l = Convert.ToDecimal(dt.Rows[0]["Ltr"].ToString());
                        lblLtr.Text = l.ToString("#.##");


                        decimal rateBox = Convert.ToDecimal((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text)).ToString("0.00"));
                        decimal taxV = ((rateBox - Convert.ToDecimal(DiscVal.Text)) * (Convert.ToDecimal(tax.Text) / 100));

                        decimal AddtaxV = ((Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(txtB.Text) - Convert.ToDecimal(DiscVal.Text)) * (Convert.ToDecimal(AddtaxPer.Text) / 100));
                        decimal amt = (rateBox) - Convert.ToDecimal(DiscVal.Text) + taxV + AddtaxV;
                        taxvalue.Text = taxV.ToString("#.##");
                        AddTaxValue.Text = AddtaxV.ToString("#.##");
                        amount.Text = amt.ToString("#.##");
                        lblTotal.Text = amt.ToString("#.##");

                        //
                        string Box = txtBoxQtyGrid.Text.ToString();
                        string PCS = txtPcsQtyGrid.Text.ToString();
                        string Total_Box = txtBoxTotal.Text;
                        string BoxPcs1 = txtBoxPcs.Text.ToString();



                        //


                        if (Session["LineItem"] != null)
                        {
                            DataTable dtSession = new DataTable();
                            dtSession = Session["LineItem"] as DataTable;
                            foreach (DataRow row123 in dtSession.Rows)
                            {
                                if (Convert.ToString(row123["SNO"]) == lblLine_No)
                                {
                                    //   row123["Invoice_Qty"] = txtB.Text.Trim();    //Changed by Amol 13-FEb-2017

                                    //Box
                                    row123["Box_Qty"] = Box;
                                    //PCS
                                    row123["Pcs_Qty"] = PCS;
                                    //Total_Box
                                    row123["Invoice_Qty"] = Total_Box;   //Total Box Qty conv
                                                                         //BOxPcs
                                    row123["BOXPCS"] = BoxPcs1;
                                    //ltr 
                                    row123["LTR"] = lblLtr.Text.Trim();
                                    //DiscValue
                                    row123["DiscVal"] = DiscVal.Text.Trim();

                                    //row123["Total"] =Convert.ToString(lblTotal.Text.Trim();
                                    //row123["MRPVALUE"] = txtB.Text.Trim();
                                }
                            }
                        }
                    }

                }
                else
                {
                    lblLtr.Text = "0.00";
                    taxvalue.Text = "0.00";
                    amount.Text = "0.00";
                    lblTotal.Text = "0.00";
                    AddTaxValue.Text = "0.00";
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                    string message = "alert('Please Enter a valid number!!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                }
                TDCalculation();
                GrandTotal();
                // For Shceme Seletion
                DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                if (dtApplicable.Rows.Count > 0)
                {
                    intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                }
                if (intApplicable == 1 || intApplicable == 3)
                {
                    // BindSchemeGrid();
                }

                // BindSchemeGrid();
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        //protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvDetails.PageIndex = e.NewPageIndex;
        //    BindGridview();
        //}

        public void GrandTotal()
        {
            try
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
                dtLine.Columns.Add("MRP", typeof(decimal));
                dtLine.Columns.Add("MRPVALUE", typeof(decimal));


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
                    Label lblMRP = (Label)gvr.FindControl("MRP");
                    Label lblMRPVALUE = (Label)gvr.FindControl("MRPVALUE");

                    if (Invoice_Qty.Text == null || Invoice_Qty.Text == string.Empty)
                    {
                        Invoice_Qty.Text = "0.00";
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
                    if (lblMRP.Text == string.Empty)
                    {
                        lblMRP.Text = "0.00";
                    }
                    if (lblMRPVALUE.Text == string.Empty)
                    {
                        lblMRPVALUE.Text = "0.00";
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
                    r["MRP"] = Convert.ToDecimal(lblMRP.Text);
                    r["MRPVALUE"] = Convert.ToDecimal(lblMRP.Text) * Convert.ToDecimal(Invoice_Qty.Text);
                    dtLine.Rows.Add(r);
                }
                GridViewFooterCalculate(dtLine);
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        //protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{

        //}
        //protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{

        //}
        //protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label StockQty = (Label)e.Row.FindControl("StockQty");
                    TextBox BoxQty = (TextBox)e.Row.FindControl("txtBox");
                    HiddenField hdnPrCode = (HiddenField)e.Row.FindControl("HiddenField2");
                    TextBox PcsQty = (TextBox)e.Row.FindControl("txtPcsQtyGrid");

                    if (Convert.ToDecimal(BoxQty.Text) > Convert.ToDecimal(StockQty.Text))
                    {
                        e.Row.BackColor = System.Drawing.Color.Tomato;
                    }

                    DataTable dt = baseObj.GetData("Select Product_Group from ax.InventTable where ItemId='" + hdnPrCode.Value + "' ");
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        //protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        //{

        //}

        public bool ValidateSchemeQty(string SelectedShemeCode)
        {
            bool returnType = true;
            try
            {
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
                        dr["Slab"] = Convert.ToInt32(rw.Cells[6].Text);
                        dr["FreeQty"] = Convert.ToInt32(Convert.ToInt32(rw.Cells[7].Text));

                        dt.Rows.Add(dr);

                    }
                    DataView dv = new DataView(dt);
                    DataTable distinctTableValue = (DataTable)dv.ToTable(true, "SchemeCode", "Slab", "FreeQty");

                    DataView dv1 = new DataView(distinctTableValue);
                    dv1.RowFilter = "[SchemeCode]='" + SelectedShemeCode + "'";
                    if (dv1.Count > 0)
                    {

                        decimal TotalQty = 0;
                        decimal Slab = 0;
                        decimal FreeQty = 0;
                        string SchemeCode = string.Empty;
                        foreach (DataRowView drow in dv1)
                        {
                            TotalQty = 0;
                            SchemeCode = drow["SchemeCode"].ToString();
                            Slab = Convert.ToInt32(drow["Slab"]);
                            FreeQty = Convert.ToInt32(drow["FreeQty"]);

                            foreach (GridViewRow rw in gvScheme.Rows)
                            {
                                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                                if (chkBx.Checked == true)
                                {

                                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                                    if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt32(rw.Cells[6].Text) == Slab)
                                    {
                                        TotalQty += Convert.ToInt32(txtQty.Text);
                                    }

                                }
                            }


                            if (TotalQty != FreeQty && Slab == Convert.ToInt32(drow["Slab"]))
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
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return returnType;
        }

        public bool ValidateSchemeQtyNew()
        {
            bool returnType = true;
            try
            {
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
                        dr["Slab"] = Convert.ToInt32(rw.Cells[6].Text);
                        dr["FreeQty"] = Convert.ToInt32(Convert.ToInt32(rw.Cells[7].Text));

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
                        Slab = Convert.ToInt32(drow["Slab"]);
                        FreeQty = Convert.ToInt32(drow["FreeQty"]);

                        foreach (GridViewRow rw in gvScheme.Rows)
                        {

                            CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                            if (chkBx.Checked == true)
                            {

                                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                                if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt32(rw.Cells[6].Text) == Slab)
                                {
                                    TotalQty += Convert.ToInt32(txtQty.Text);
                                }

                            }
                        }


                        if (TotalQty != FreeQty && Slab == Convert.ToInt32(drow["Slab"]))
                        {
                            returnType = false;

                        }

                    }
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return returnType;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validation();
                //checkStock();               
                bool SchemeStock = checkStock();
                if (SchemeStock == false)
                {
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please check stock qty for scheme product!!');", true);

                    string message = "alert('Please check stock qty for scheme product!!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

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
                    IsSchemeValidate = ValidateSchemeQty(SelectedSchemeCode);
                }
                else
                {
                    IsSchemeValidate = true;
                }

                //if (IsSchemeValidate == true)
                //{
                if (b == true)
                {
                    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                    if (strmessage == string.Empty)
                    {
                        SaveHeader();
                        // Cache.Remove("SO_NO");

                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Invoice No: " + txtInvoiceNo.Text + " Saved Successfully!!');", true);

                        string message = "alert('Invoice No: " + txtInvoiceNo.Text + " Saved Successfully!!');";

                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                        Session["ItemTable"] = null;
                        ClearAll();
                        ShowDataForFOC();
                        Session["LineItem"] = null;
                        drpCustomerGroup_SelectedIndexChanged(null, null);
                        ProductGroup();
                        ddlProductGroup_SelectedIndexChanged(null, null);
                        DDLProductSubCategory_SelectedIndexChanged(null, null);
                        drpCustomerGroup.Focus();
                        ClearDataEntryField();

                    }
                }
                //}
                //else
                //{
                //    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Free Quantity should be Equal !');", true);
                //    string message = "alert('Free Quantity should be Equal !');";
                //    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                //    return;
                //}
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
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
            try
            {
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
                            string strStcok = "Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty from [ax].[ACXINVENTTRANS] F where F.[SiteCode]='" + Session["SiteCode"].ToString() + "' and F.[ProductCode]='" + Sproduct + "' and F.[TransLocation]='" + Session["TransLocation"].ToString() + "'  ";
                            dtStock = baseObj.GetData(strStcok);
                            decimal curstock = 0;
                            if (dtStock.Rows.Count > 0)
                            {
                                curstock = Convert.ToDecimal(dtStock.Rows[0]["TransQty"].ToString());
                            }
                            DataRow row;
                            row = dtLine.NewRow();
                            row["Product_Code"] = gv.Cells[4].Text;
                            row["StockQty"] = curstock;
                            row["InvoiceQty"] = decimal.Parse(txtQtyToAvail.Text);

                            dtLine.Rows.Add(row);

                        }
                    }
                }

                var stkresult = dtLine.AsEnumerable()
                                         .GroupBy(r => new
                                         {
                                             ProductCode = r.Field<String>("Product_Code"),
                                             //StockQty = r.Field<Decimal>("StockQty"),
                                             // Qty = r.Field<Double>("Qty"),
                                         })

                                            .Select(g =>
                                            {
                                                var row = g.First();
                                                row.SetField("InvoiceQty", g.Sum(r => r.Field<Decimal>("InvoiceQty")));
                                                return row;
                                            })
                                                .CopyToDataTable();

                dtLine = stkresult;

                for (int i = 0; i < dtLine.Rows.Count; i++)
                {
                    decimal curStock = Convert.ToDecimal(dtLine.Rows[i]["StockQty"].ToString());
                    decimal invQty = Convert.ToDecimal(dtLine.Rows[i]["InvoiceQty"].ToString());
                    if (curStock < invQty)
                    {
                        result = false;
                    }

                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return result;
        }

        public string InvoiceNo()
        {
            try
            {
                string IndNo = string.Empty;
                string Number = string.Empty;
                int intTotalRec;
                string strQuery = "Select ISNULL(MAX(CAST(RIGHT(Invoice_No,7) AS INT)),0)+1 as new_InvoiceNo from [ax].[ACXSALEINVOICEHEADER] where [Siteid]='" + lblsite.Text + "' and TranType=1";

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
                strmessage = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                string sono = "";
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
                cmd.Parameters.AddWithValue("@TranType", 3);
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
                    TDValue = Convert.ToDecimal(txtTDValue.Text);
                }
                cmd.Parameters.AddWithValue("@TDValue", TDValue);

                //========Save Line================
                //cmd1 = new SqlCommand();
                //cmd1.Connection = conn;
                //cmd1.Transaction = transaction;
                //cmd1.CommandTimeout = 100;
                //cmd1.CommandType = CommandType.Text;

                int i = 0;
                decimal totamt = 0;

                //===================New===============
                cmd1 = new SqlCommand("Acx_Insert_SaleInvoiceLine");
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
                    Label ltr = (Label)gv.FindControl("ltr");
                    Label mrp = (Label)gv.FindControl("MRP");
                    Label rate = (Label)gv.FindControl("txtRate");
                    Label taxvalue = (Label)gv.FindControl("TaxValue");
                    Label taxper = (Label)gv.FindControl("Tax");
                    Label AddtaxPer = (Label)gv.FindControl("AddTax");
                    Label AddTaxValue = (Label)gv.FindControl("AddTaxValue");
                    if (Convert.ToDecimal(txtTDValue.Text) > 0)
                    {
                        taxvalue = (Label)gv.FindControl("VatAfterPE");
                    }
                    else
                    {
                        taxvalue = (Label)gv.FindControl("TaxValue");
                    }

                    Label DiscVal = (Label)gv.FindControl("DiscValue");
                    Label Disc = (Label)gv.FindControl("Disc");
                    Label SchemeCode = (Label)gv.FindControl("lblScheme");
                    HiddenField hDT = (HiddenField)gv.FindControl("hDiscType");
                    HiddenField hCalculationBase = (HiddenField)gv.FindControl("hDiscCalculationType");
                    //TextBox SDiscPer = (TextBox)gvDetails.Rows[i].FindControl("SecDisc");
                    TextBox SDiscVal = (TextBox)gv.FindControl("SecDiscValue");

                    Label lblTD = (Label)gv.FindControl("TD");
                    Label lblPE = (Label)gv.FindControl("PE");
                    Label lblTotal = (Label)gv.FindControl("Total");
                    totamt = totamt + Convert.ToDecimal(lblTotal.Text);
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
                    Tamount = Convert.ToDecimal(Amt.Text);
                    Tbox = Convert.ToDecimal(box.Text);
                    Tltr = Convert.ToDecimal(ltr.Text);
                    Tmrp = Convert.ToDecimal(mrp.Text);
                    Trate = Convert.ToDecimal(rate.Text);
                   
                    TextBox txtBoxQtyGrid = (TextBox)gv.FindControl("txtBoxQtyGrid");
                    TextBox txtPcsQtyGrid = (TextBox)gv.FindControl("txtPcsQtyGrid");
                    Label txtBoxPcs = (Label)gv.FindControl("txtBoxPcs");

                    decimal LineAmount = 0;
                   

                    decimal taxamt = Convert.ToDecimal(taxvalue.Text);
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
                        cmd1.Parameters.AddWithValue("@AMOUNT", lblTotal.Text);
                        cmd1.Parameters.AddWithValue("@BOX", Tbox);
                        cmd1.Parameters.AddWithValue("@CRATES", 0);
                        cmd1.Parameters.AddWithValue("@LTR", Tltr);
                        cmd1.Parameters.AddWithValue("@QUANTITY", 0);
                        cmd1.Parameters.AddWithValue("@MRP", Tmrp);
                        cmd1.Parameters.AddWithValue("@RATE", Trate);
                        cmd1.Parameters.AddWithValue("@TAX_CODE", taxper.Text);
                        cmd1.Parameters.AddWithValue("@TAX_AMOUNT", taxamt);
                        cmd1.Parameters.AddWithValue("@ADDTAX_CODE", AddtaxPer.Text);
                        cmd1.Parameters.AddWithValue("@ADDTAX_AMOUNT", AddTaxValue.Text);
                        cmd1.Parameters.AddWithValue("@DISC_AMOUNT", DiscVal.Text);
                        cmd1.Parameters.AddWithValue("@SEC_DISC_AMOUNT", SDiscVal.Text);
                        cmd1.Parameters.AddWithValue("@TranType", 3);
                        cmd1.Parameters.AddWithValue("@DiscType", hDT.Value);
                        cmd1.Parameters.AddWithValue("@Disc", Disc.Text);
                        cmd1.Parameters.AddWithValue("@SchemeCode", SchemeCode.Text);
                        cmd1.Parameters.AddWithValue("@LINEAMOUNT", LineAmount);
                        cmd1.Parameters.AddWithValue("@DiscCalculationBase", hCalculationBase.Value);
                        cmd1.Parameters.AddWithValue("@TDValue", lblTD.Text);
                        cmd1.Parameters.AddWithValue("@PEValue", lblPE.Text);

                        cmd1.Parameters.AddWithValue("@BasePrice", Trate);
                        cmd1.Parameters.AddWithValue("@BOXQty", txtBoxQtyGrid.Text);
                        cmd1.Parameters.AddWithValue("@PcsQty", txtPcsQtyGrid.Text);
                        cmd1.Parameters.AddWithValue("@BOXPcs", txtBoxPcs.Text);
                        cmd1.Parameters.AddWithValue("@TaxableAmount", lblTotal.Text);
                   
                        cmd1.ExecuteNonQuery();

                    }
                }
                //==============Remaining Part Of Header-===============
                cmd.Parameters.AddWithValue("@INVOICE_VALUE", totamt);
                cmd.ExecuteNonQuery();

                //===============Update Transaction Table===============

                cmd2 = new SqlCommand();
                cmd2.Connection = conn;
                cmd2.Transaction = transaction;
                cmd2.CommandTimeout = 3600;
                cmd2.CommandType = CommandType.Text;
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
                string Referencedocumentno = "";
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
                    decimal TransQty = Convert.ToDecimal(Qty);
                    int REcid = k + 1;

                    if (TransQty > 0)
                    {
                        cmd2.CommandText = "";
                        string Tquery = " Insert Into ax.acxinventTrans " +
                                "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],TransLineNo)" +
                                " Values ('" + TransId + "','" + Siteid + "','" + DATAAREAID + "',(select coalesce(max(Recid),0)+1 as Recid from  ax.acxinventTrans),getdate()," + TransType + "," + DocumentType + ",'" + DocumentNo + "'," +
                                " '" + DocumentDate + "','" + ProductCode + "'," + -TransQty + ",'" + uom + "','" + TransLocation + "','" + Referencedocumentno + "'," + l + ")";
                        //obj.ExecuteCommand(query);
                        cmd2.CommandText = Tquery;
                        cmd2.ExecuteNonQuery();
                        l += 1;
                    }

                }
                //    string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");
                //============Loop For Scheme LineItem==========
                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        //  i = i + 1;

                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");


                        string Sproduct = gv.Cells[4].Text;
                        //decimal TransQty = Convert.ToDecimal(gv.Cells[6].Text);
                        decimal TransQty = Convert.ToDecimal(txtQtyToAvail.Text);
                        totamt = 0;

                        {
                            cmd2.CommandText = "";
                            string Tquery = " Insert Into ax.acxinventTrans " +
                                    "([TransId],[SiteCode],[DATAAREAID],[RECID],[InventTransDate],[TransType],[DocumentType]," +
                                    "[DocumentNo],[DocumentDate],[ProductCode],[TransQty],[TransUOM],[TransLocation],[Referencedocumentno],TransLineNo)" +
                                    " Values ('" + TransId + "','" + Siteid + "','" + DATAAREAID + "',(select coalesce(max(Recid),0)+1 as Recid from  ax.acxinventTrans),getdate()," + TransType + "," + DocumentType + ",'" + DocumentNo + "'," +
                                    " '" + DocumentDate + "','" + Sproduct + "'," + -TransQty + ",'" + uom + "','" + TransLocation + "','" + Referencedocumentno + "'," + l + ")";
                            cmd2.CommandText = Tquery;
                            cmd2.ExecuteNonQuery();
                            l += 1;
                        }
                    }

                }

                //===================================================
                // Insert For Scheme Line
                cmd3 = new SqlCommand();
                cmd3.Connection = conn;
                cmd3.Transaction = transaction;
                cmd3.CommandTimeout = 3600;
                cmd3.CommandType = CommandType.Text;
                i = gvDetails.Rows.Count;
                foreach (GridViewRow gv in gvScheme.Rows)
                {
                    if (((CheckBox)gv.FindControl("chkSelect")).Checked)
                    {
                        i = i + 1;

                        HiddenField pr = (HiddenField)gv.Cells[2].FindControl("HiddenField2");
                        TextBox txtQtyToAvail = (TextBox)gv.FindControl("txtQty");
                        decimal decQtyToAvail = Convert.ToDecimal(txtQtyToAvail.Text);
                        totamt = 0;

                        string[] calValue = baseObj.CalculatePrice1(gv.Cells[4].Text, drpCustomerCode.SelectedItem.Value, int.Parse(txtQtyToAvail.Text), "Box");
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

                        string srtTaxCode = "Select  H.TaxValue,H.ACXADDITIONALTAXVALUE from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + gv.Cells[4].Text + "' and G.Siteid='" + Session["SITECODE"].ToString() + "'";
                        DataTable dtTax = new DataTable();
                        string taxCode = string.Empty;
                        dtTax = obj.GetData(srtTaxCode);
                        if (dtTax != null)
                        {
                            taxCode = dtTax.Rows[0]["TaxValue"].ToString();
                        }
                        else
                        {
                            taxCode = "0";
                        }

                        {
                            cmd3.CommandText = "";
                            string query = " INSERT INTO [ax].[ACXSALEINVOICELINE] " +
                                                    "([SITEID],[DATAAREAID],[RECID],[CUSTOMER_CODE],[INVOICE_NO],[LINE_NO],[PRODUCT_CODE],[PRODUCTGROUP_CODE]" +
                                                    ",[AMOUNT],[BOX],[CRATES],[LTR],[QUANTITY],[MRP],[RATE],[TAX_CODE],[TAX_AMOUNT],[DISC_AMOUNT],[SEC_DISC_AMOUNT],[TranType],DiscType,Disc,SchemeCode)" +
                                                    "VALUES('" + lblsite.Text + "','" + Session["DATAAREAID"].ToString() + "',(select coalesce(max(Recid),0)+1 as Recid from  [ax].[ACXSALEINVOICELINE])," +
                                                     "'" + drpCustomerCode.SelectedItem.Value + "','" + txtInvoiceNo.Text + "'," + i + ",'" + gv.Cells[4].Text + "'," +
                                                     "' '," + 0 + "," + decQtyToAvail + "," + 0 + "," + strLtr + ", " + 0 + ", " + 0 + "" +
                                                     "," + 0 + ",'" + taxCode + "'," + 0 + "," + 0 + "," + 0 + "," + 1 + "," + 0 + "," + 0 + ",'" + gv.Cells[1].Text + "')";
                            cmd3.CommandText = query;
                            cmd3.ExecuteNonQuery();

                        }
                    }
                }


                //============Commit All Data================

                //int a = obj.UpdateLastNumSequence(6, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);
                transaction.Commit();
                Session["LineItem"] = null;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lblMessage.Text = ex.Message.ToString();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + ex.Message.ToString() + "');", true);
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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
                    decimal TransQty = Convert.ToDecimal(Qty);
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
            //txtInvoiceDate.Text = "";
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
            DataTable dtBlank = new DataTable();
            gvDetails.DataSource = dtBlank;
            gvDetails.DataBind();
            gvScheme.DataSource = dtBlank;
            gvDetails.DataBind();
            TxtSetFocus.Text = "0";
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
            Response.Redirect("~/frmInvoicePrepration.aspx");
        }

        private bool Validation()
        {
            bool returnvalue = false;

            if (gvDetails.Rows.Count <= 0)
            {
                string message = "alert('Please Add the Line ......!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                returnvalue = false;
                return returnvalue;
            }

            if (drpCustomerCode.Text == "")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Customer Name !');", true);

                string message = "alert('Please Provide Customer Name...!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                drpCustomerGroup.Focus();
                returnvalue = false;
                return returnvalue;
            }
            else if (drpCustomerGroup.Text == "")
            {
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Customer Group !');", true);

                string message = "alert('Please Provide Customer Group...!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                drpCustomerGroup.Focus();
                returnvalue = false;
                return returnvalue;
            }
           
            else if (txtInvoiceDate.Text == "")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Invoice Date !');", true);

                string message = "alert('Please Provide Invoice Date...!!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtInvoiceDate.Focus();
                returnvalue = false;
                return returnvalue;
            }
            else
            {
                returnvalue = true;
            }
            //===============================
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            DataTable dt = new DataTable();
          
            dt = new DataTable();
            int valrow = 0;
            for (int i = 0; i < gvDetails.Rows.Count; i++)
            {
                Label Product = (Label)gvDetails.Rows[i].Cells[2].FindControl("Product");
                string productNameCode = Product.Text;
                string[] str = productNameCode.Split('-');
                string ProductCode = str[0].ToString();
                TextBox box = (TextBox)gvDetails.Rows[i].Cells[6].FindControl("txtBox");
                string Qty = box.Text;
                decimal TransQty = Convert.ToDecimal(Qty);

                if (TransQty == 0)
                {
                    string message = "alert('Total Box Qty should not be Zero(0),Row No :-"+i+"...!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    returnvalue = false;
                    return returnvalue;
                   
                }


                if (TransQty > 0)
                {
                    valrow = valrow + 1;
                }

                string query = "select ProductCode from ax.acxinventTrans group by sitecode,translocation,ProductCode " +
                               " Having sitecode='" + Session["SiteCode"].ToString() + "' and translocation='" + Session["TransLocation"].ToString() + "' and ProductCode='" + ProductCode + "' and sum(TransQty)>=" + TransQty + "";

                dt = obj.GetData(query);
                if (Qty != "0")
                {
                    if (dt.Rows.Count > 0)
                    {
                        returnvalue = true;
                    }
                    else
                    {
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Product :" + productNameCode + " is out of stock !');", true);

                        string message = "alert('Product :" + productNameCode + " is out of stock !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                        returnvalue = false;
                        return returnvalue;
                    }
                }
            }
            if (valrow <= 0)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid data !');", true);

                string message = "alert('Please enter valid data !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                returnvalue = false;
                return returnvalue;
            }
            return returnvalue;
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
            gvScheme.DataSource = dt;
            gvScheme.DataBind();
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
                    string strItemCode = arritem[0].ToString();
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
                    DataTable dt1 = dt.Clone();
                    dt1.Clear();
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (col.ColumnName == "FREEQTY")
                        {
                            col.ReadOnly = false;
                        }
                    }

                    Int16 TotalQtyofGroupItem = 0;
                    Int16 TotalQtyofItem = 0;
                    Int16 TotalMaxQtyofItem = 0;
                    decimal TotalValueofGroupItem = 0;
                    decimal TotalValueofItem = 0;
                    decimal TotalMaxValueofItem = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Scheme Type"].ToString() == "0") // For Qty
                        {
                            TotalQtyofGroupItem = GetQtyofGroupItem(dr["Scheme Item group"].ToString());
                            TotalQtyofItem = GetQtyofItem(dr["Scheme Item group"].ToString());
                            TotalMaxQtyofItem = GetMaxQtyofItem();

                            if (dr["Scheme Item Type"].ToString() == "Group" && TotalQtyofGroupItem >= Convert.ToInt32(dr["MINIMUMQUANTITY"]))
                            {
                                dr["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt32(dr["MINIMUMQUANTITY"]))) * Convert.ToInt32(dr["FREEQTY"]));
                                //dr["FREEQTY"] = dr["TotalFreeQty"];
                                dt1.ImportRow(dr);
                            }

                            if (dr["Scheme Item Type"].ToString() == "Item" && TotalQtyofItem >= Convert.ToInt32(dr["MINIMUMQUANTITY"]))
                            {
                                dr["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalQtyofItem / Convert.ToInt32(dr["MINIMUMQUANTITY"]))) * Convert.ToInt32(dr["FREEQTY"]));
                                //dr["FREEQTY"] = dr["TotalFreeQty"];
                                dt1.ImportRow(dr);
                            }
                            if (dr["Scheme Item Type"].ToString() == "All" && TotalMaxQtyofItem >= Convert.ToInt32(dr["MINIMUMQUANTITY"]))
                            {
                                dr["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalMaxQtyofItem / Convert.ToInt32(dr["MINIMUMQUANTITY"]))) * Convert.ToInt32(dr["FREEQTY"]));
                                //dr["FREEQTY"] = dr["TotalFreeQty"];
                                dt1.ImportRow(dr);
                            }
                        }

                        if (dr["Scheme Type"].ToString() == "1") // For Value
                        {
                            TotalValueofGroupItem = GetValueofGroupItem(dr["Scheme Item group"].ToString());
                            TotalValueofItem = GetValueofItem(dr["Scheme Item group"].ToString());
                            TotalMaxValueofItem = GetMaxValueofItem();

                            if (dr["Scheme Item Type"].ToString() == "Group" && TotalValueofGroupItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                            {
                                dr["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt32(dr["FREEQTY"]));
                                //dr["FREEQTY"] = dr["TotalFreeQty"];
                                dt1.ImportRow(dr);
                            }

                            if (dr["Scheme Item Type"].ToString() == "Item" && TotalValueofItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                            {
                                dr["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalValueofItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt32(dr["FREEQTY"]));
                                //dr["FREEQTY"] = dr["TotalFreeQty"];
                                dt1.ImportRow(dr);
                            }
                            if (dr["Scheme Item Type"].ToString() == "All" && TotalMaxValueofItem >= Convert.ToDecimal(dr["MINIMUMVALUE"]))
                            {
                                dr["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalMaxValueofItem / Convert.ToDecimal(dr["MINIMUMVALUE"]))) * Convert.ToInt32(dr["FREEQTY"]));
                                //dr["FREEQTY"] = dr["TotalFreeQty"];
                                dt1.ImportRow(dr);
                            }
                        }


                    }

                    DataTable dt3 = dt1.Clone();

                    #region For Qty

                    DataView view = new DataView(dt1);
                    view.RowFilter = "[Scheme Type]=0";
                    DataTable distinctTable = (DataTable)view.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMQUANTITY");

                    DataView dvSort = new DataView(distinctTable);
                    dvSort.Sort = "MINIMUMQUANTITY DESC";

                    DataView Dv1 = null;
                    int intCalRemFreeQty = 0;
                    Int16 RemainingQty = 0;

                    foreach (DataRowView drv in dvSort)
                    {
                        if (intCalRemFreeQty > 0)
                        {
                            TotalQtyofGroupItem = RemainingQty;
                            TotalQtyofItem = RemainingQty;
                            TotalMaxQtyofItem = RemainingQty;
                        }


                        Dv1 = new DataView(dt1);
                        Dv1.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMQUANTITY='" + drv.Row["MINIMUMQUANTITY"] + "'";

                        foreach (DataRowView drv2 in Dv1)
                        {
                            if (drv["Scheme Item Type"].ToString() == "Group" && TotalQtyofGroupItem >= Convert.ToInt32(drv["MINIMUMQUANTITY"]))
                            {
                                drv2["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalQtyofGroupItem / Convert.ToInt32(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt32(drv2["FREEQTY"]));
                                dt3.ImportRow(drv2.Row);
                                RemainingQty = Convert.ToInt16(TotalQtyofGroupItem - (Convert.ToInt32(drv2["TotalFreeQty"]) / Convert.ToInt32(drv2["FREEQTY"])) * Convert.ToInt32(drv2["MINIMUMQUANTITY"]));
                            }

                            if (drv["Scheme Item Type"].ToString() == "Item" && TotalQtyofItem >= Convert.ToInt32(drv["MINIMUMQUANTITY"]))
                            {
                                drv2["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalQtyofItem / Convert.ToInt32(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt32(drv2["FREEQTY"]));
                                dt3.ImportRow(drv2.Row);
                                RemainingQty = Convert.ToInt16(TotalQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                            }
                            if (drv["Scheme Item Type"].ToString() == "All" && TotalMaxQtyofItem >= Convert.ToInt16(drv["MINIMUMQUANTITY"]))
                            {
                                drv2["TotalFreeQty"] = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(TotalMaxQtyofItem / Convert.ToInt32(drv2["MINIMUMQUANTITY"]))) * Convert.ToInt32(drv2["FREEQTY"]));
                                dt3.ImportRow(drv2.Row);
                                RemainingQty = Convert.ToInt16(TotalMaxQtyofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMQUANTITY"]));
                            }
                            intCalRemFreeQty += 1;
                        }

                    }

                    #endregion

                    #region for Value

                    DataView viewValue = new DataView(dt1);
                    viewValue.RowFilter = "[Scheme Type]=1";
                    DataTable distinctTableValue = (DataTable)viewValue.ToTable(true, "SCHEMECODE", "Scheme Item Type", "MINIMUMVALUE");

                    DataView dvSortValue = new DataView(distinctTableValue);
                    dvSortValue.Sort = "MINIMUMVALUE DESC";

                    DataView Dv1Value = null;
                    Int16 IntCalRemFreeValue = 0;
                    Int16 RemainingQtyValue = 0;
                    foreach (DataRowView drv in dvSortValue)
                    {
                        if (IntCalRemFreeValue > 0)
                        {
                            TotalValueofGroupItem = RemainingQtyValue;
                            TotalValueofItem = RemainingQtyValue;
                            TotalMaxValueofItem = RemainingQtyValue;
                        }


                        Dv1Value = new DataView(dt1);
                        Dv1Value.RowFilter = "SCHEMECODE='" + drv.Row["SCHEMECODE"] + "' and [Scheme Item Type]='" + drv.Row["Scheme Item Type"] + "' and MINIMUMVALUE='" + drv.Row["MINIMUMVALUE"] + "'";

                        foreach (DataRowView drv2 in Dv1Value)
                        {
                            if (drv["Scheme Item Type"].ToString() == "Group" && TotalValueofGroupItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                            {
                                drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofGroupItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                dt3.ImportRow(drv2.Row);
                                RemainingQtyValue = Convert.ToInt16(TotalValueofGroupItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMVALUE"]));
                            }

                            if (drv["Scheme Item Type"].ToString() == "Item" && TotalValueofItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                            {
                                drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalValueofItem / Convert.ToInt16(drv["MINIMUMVALUE"]))) * Convert.ToInt16(drv["FREEQTY"]));
                                dt3.ImportRow(drv2.Row);
                                RemainingQtyValue = Convert.ToInt16(TotalValueofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMVALUE"]));
                            }
                            if (drv["Scheme Item Type"].ToString() == "All" && TotalMaxValueofItem >= Convert.ToInt16(drv["MINIMUMVALUE"]))
                            {
                                drv2["TotalFreeQty"] = Convert.ToInt16(System.Math.Floor(Convert.ToDecimal(TotalMaxValueofItem / Convert.ToInt16(drv2["MINIMUMVALUE"]))) * Convert.ToInt16(drv2["FREEQTY"]));
                                dt3.ImportRow(drv2.Row);
                                RemainingQtyValue = Convert.ToInt16(TotalMaxValueofItem - (Convert.ToInt16(drv2["TotalFreeQty"]) / Convert.ToInt16(drv2["FREEQTY"])) * Convert.ToInt16(drv2["MINIMUMVALUE"]));
                            }
                            IntCalRemFreeValue += 1;
                        }

                    }

                    #endregion
                    return dt3;
                }
                else
                {
                    return null;
                }


                #region Coment
                /*
                 DataView dv = null;
                 DataTable dt2 = new DataTable();
                 dt2 = dt1.Clone();
                 var groupedData = from r in dt1.AsEnumerable()
                                   group r by new
                                   {
                                       FreeItemCode = r.Field<string>("Free Item Code"),
                                   } into g
                                   select new
                                   {
                                       g.Key.FreeItemCode,
                                       Max = g.Max(r => r.Field<decimal>("MINIMUMQUANTITY"))

                                   };

                 foreach (var grp in groupedData)
                 {
                     dv = new DataView(dt1);
                     dv.RowFilter = "[Free Item Code]='" + grp.FreeItemCode + "' and MINIMUMQUANTITY=" + grp.Max + "";
                     foreach (DataRowView drv in dv)
                     {
                         DataRow dr = drv.Row;
                         dt2.ImportRow(dr);
                     }

                 }
                 */
                #endregion


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
            conn = baseObj.GetConnection();
            DataTable dt = new DataTable();
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

        public Int16 GetQtyofGroupItem(string Group)
        {
            DataTable dt = baseObj.GetData("select ITEMID from ax.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
            Int16 Qty = 0;
            foreach (DataRow dtdr in dt.Rows)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    TextBox txtQty = (TextBox)gvrow.Cells[6].FindControl("txtBox");
                    Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                    string[] arritem = lblItem.Text.Split('-');
                    string strItemCode = arritem[0].ToString();
                    if (dtdr[0].ToString() == strItemCode)
                    {
                        Qty += Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                    }
                }
            }
            return Qty;
        }

        public decimal GetValueofGroupItem(string Group)
        {
            DataTable dt = baseObj.GetData("select ITEMID from ax.ACXFreeItemGroupTable Where [GROUP] ='" + Group + "' and DATAAREAID='" + Session["DATAAREAID"] + "'");
            decimal Value = 0;
            foreach (DataRow dtdr in dt.Rows)
            {
                foreach (GridViewRow gvrow in gvDetails.Rows)
                {
                    Label lblAmount = (Label)gvrow.Cells[6].FindControl("Amount");
                    Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                    string[] arritem = lblItem.Text.Split('-');
                    string strItemCode = arritem[0].ToString();

                    if (dtdr[0].ToString() == strItemCode)
                    {
                        Value += Convert.ToDecimal(lblAmount.Text);
                    }
                }
            }
            return Value;
        }

        public Int16 GetQtyofItem(string Item)
        {
            Int16 Qty = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                TextBox txtQty = (TextBox)gvrow.Cells[6].FindControl("txtBox");
                Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                string[] arritem = lblItem.Text.Split('-');
                string strItemCode = arritem[0].ToString();

                if (strItemCode == Item)
                {
                    Qty = Convert.ToInt16(Convert.ToDouble(txtQty.Text));
                }
            }
            return Qty;
        }

        public decimal GetValueofItem(string Item)
        {
            decimal Value = 0;
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                Label lblAmount = (Label)gvrow.Cells[6].FindControl("Amount");
                Label lblItem = (Label)gvrow.Cells[2].FindControl("Product");
                string[] arritem = lblItem.Text.Split('-');
                string strItemCode = arritem[0].ToString();

                if (strItemCode == Item)
                {
                    Value = Convert.ToDecimal(lblAmount.Text);
                }
            }
            return Value;
        }

        public Int16 GetMaxQtyofItem()
        {
            Int16 Qty = 0;
            Int16[] arrQty = new Int16[gvDetails.Rows.Count];
            foreach (GridViewRow gvrow in gvDetails.Rows)
            {
                TextBox txtQty = (TextBox)gvrow.Cells[6].FindControl("txtBox");
                arrQty[gvrow.RowIndex] = Convert.ToInt16(Convert.ToDouble(txtQty.Text));
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
                arrValue[gvrow.RowIndex] = Convert.ToDecimal(lblAmount.Text);
            }
            Value = arrValue.Max();
            return Value;
        }

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            if (activeCheckBox.Checked)
            {
                GridViewRow row = (GridViewRow)(((CheckBox)sender)).NamingContainer;
                string SchemeCode = row.Cells[1].Text;
                foreach (GridViewRow rw in gvScheme.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    if (chkBx.Checked)
                    {
                        if (rw.Cells[1].Text != SchemeCode)
                        {
                            activeCheckBox.Checked = false;
                            string message = "alert('You can select only one scheme items !');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('You can select only one scheme items !');", true);
                            return;
                        }
                    }
                }
            }


            //CheckBox activeCheckBox = sender as CheckBox;
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                TextBox txtQty = (TextBox)rw.FindControl("txtQty");
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

        }

        public void GetSelectedShemeItemChecked(string SchemeCode, string FreeitemCode, Int16 Qty, Int16 Slab)
        {
            foreach (GridViewRow rw in gvScheme.Rows)
            {
                if (rw.Cells[1].Text == SchemeCode && rw.Cells[4].Text == FreeitemCode && Convert.ToInt32(Convert.ToDouble(rw.Cells[6].Text)) == Slab)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                    chkBx.Checked = true;
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    txtQty.Text = Convert.ToString(Qty);
                }
            }

        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            int TotalQty = 0;

            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int AvlFreeQty = Convert.ToInt32(row.Cells[7].Text);
            int Slab = Convert.ToInt32(row.Cells[6].Text);

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                if (chkBx.Checked == true)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode && Convert.ToInt32(rw.Cells[6].Text) == Slab)
                    {
                        TotalQty += Convert.ToInt32(txtQty.Text);
                    }

                    if (TotalQty > AvlFreeQty)
                    {
                        txtQty.Text = string.Empty;
                        chkBx.Checked = false;

                        string message = "alert('Free Qty should not greater than available free qty !');";
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Free Qty should not greater than available free qty !');", true);
                        return;
                    }

                }

            }


            /*
            int TotalQty = 0;

            GridViewRow row = (GridViewRow)(((TextBox)sender)).NamingContainer;
            string SchemeCode = row.Cells[1].Text;
            int LineQty = Convert.ToInt16(row.Cells[6].Text);

            foreach (GridViewRow rw in gvScheme.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkSelect");
                if (chkBx.Checked == true)
                {
                    TextBox txtQty = (TextBox)rw.FindControl("txtQty");
                    if (!string.IsNullOrEmpty(txtQty.Text) && rw.Cells[1].Text == SchemeCode)
                    {
                        TotalQty += Convert.ToInt16(txtQty.Text);
                    }

                    if (TotalQty > LineQty)
                    {
                        txtQty.Text = string.Empty;
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Free Qty should not greater than available free qty !');", true);
                        return;
                    }

                }

            }
            */


        }

        private void GridViewFooterCalculate(DataTable dt)
        {
            //For Total[Sum] Value Show in Footer--//
            string st = Convert.ToString(dt.Rows[0]["TD"].GetType());
            decimal tSoQtyBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("So_Qty"));
            decimal tInvoiceQtyBox = dt.AsEnumerable().Sum(row => row.Field<decimal>("Invoice_Qty"));
            decimal tGridBoxQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("Box_Qty"));
            decimal tGridPcsQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("Pcs_Qty"));
            decimal tMRP = dt.AsEnumerable().Sum(row => row.Field<decimal>("MRP"));

            decimal tMRPVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("MRPVALUE"));
            // MRP VALUE= MRP X Invoice_QTY
            decimal tCalCulatedMRP = 0;
            if (tInvoiceQtyBox > 0)
            {
                tCalCulatedMRP = tMRP * tInvoiceQtyBox;
            }
            else
            {
                tCalCulatedMRP = tMRP;
            }

            decimal tLtr = dt.AsEnumerable().Sum(row => row.Field<decimal>("Ltr"));
            decimal tDiscValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("DiscVal"));
            decimal tTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TaxValue"));
            decimal tAddTaxAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADDTaxValue"));
            //decimal tSecDiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("DiscVal"));
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Amount"));
            decimal TD = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD"));
            decimal AllTotal = dt.AsEnumerable().Sum(row => row.Field<decimal>("Total"));

            //decimal MRP = dt.AsEnumerable().Sum(row => row.Field<decimal>("MRP"));

            gvDetails.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[3].Text = "TOTAL";
            gvDetails.FooterRow.Cells[3].Font.Bold = true;

           // gvDetails.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
           // gvDetails.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
           // gvDetails.FooterRow.Cells[4].Text = tMRPVALUE.ToString("N2");
          //  gvDetails.FooterRow.Cells[4].Font.Bold = true;

            gvDetails.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[5].Text = tSoQtyBox.ToString("N2");
            gvDetails.FooterRow.Cells[5].Font.Bold = true;

            gvDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[6].Text = tGridBoxQty.ToString("N2");
            gvDetails.FooterRow.Cells[6].Font.Bold = true;

            gvDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            gvDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[7].Text = tGridPcsQty.ToString("N2");
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

            gvDetails.FooterRow.Cells[21].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[21].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[21].Text = total.ToString("N2");
            gvDetails.FooterRow.Cells[21].Font.Bold = true;
            
            gvDetails.FooterRow.Cells[24].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[24].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[24].Text = TD.ToString("N2");
            gvDetails.FooterRow.Cells[24].Font.Bold = true;

            gvDetails.FooterRow.Cells[28].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[28].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[28].Text = AllTotal.ToString("N2");
            gvDetails.FooterRow.Cells[28].Font.Bold = true;

            gvDetails.FooterRow.Cells[29].HorizontalAlign = HorizontalAlign.Right;
            gvDetails.FooterRow.Cells[29].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[29].Text = tMRPVALUE.ToString("N2");
            gvDetails.FooterRow.Cells[29].Font.Bold = true;
        }

        protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                       + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'  AND P.BLOCK=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            FillProductCode();
            DDLProductSubCategory.Focus();
            PcsBillingApplicable();
        }

        protected void DDLProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            strQuery = "Select distinct P.ITEMID+'-'+P.Product_Name as Name,P.ITEMID from ax.InventTable P where Product_Group='" + DDLProductGroup.SelectedValue + "' and P.PRODUCT_SUBCATEGORY ='" + DDLProductSubCategory.SelectedItem.Value + "' AND P.BLOCK=0"; //--AND SITE_CODE='657546'";
            DDLMaterialCode.DataSource = null;
            DDLMaterialCode.Items.Clear();
             DDLMaterialCode.Items.Add("Select...");
            baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Name", "ITEMID");
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            DDLMaterialCode.Enabled = true;
            // DDLMaterialCode.SelectedIndex = 0;
            //DDLMaterialCode.Attributes.Add("onChange", "location.href = this.options[this.selectedIndex].value;");
            DDLMaterialCode.Focus();
            //DDLMaterialCode_SelectedIndexChanged(null, null);
            //DDLMaterialCode_SelectedIndexChanged(null, null);
            BindDDLProductCode();
            PcsBillingApplicable();
            ClearDataEntryField();
            
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDataEntryField();
            BindDDLProductCode();
            PcsBillingApplicable();
        }

        private void BindDDLProductCode()
        {
            txtQtyBox.Text = string.Empty;
            txtQtyCrates.Text = string.Empty;
            txtLtr.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtEnterQty.Text = string.Empty;
            txtCrateSize.Text = string.Empty;
            txtPrice.Text = string.Empty;
            DataTable dt = new DataTable();
            if (string.IsNullOrEmpty(Convert.ToString(Session["TransLocation"])))
            {
                return;
            }
            if (drpCustomerCode.SelectedIndex == 0 || drpCustomerCode.SelectedIndex < 0)
            {
                string message = "alert('Please Select Customer Code First !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                DDLMaterialCode.SelectedIndex = -1;
                return;
            }

            strQuery = " Select coalesce(cast(sum(F.TransQty) as decimal(10,2)),0) as TransQty,cast(G.Product_PackSize as decimal(10,2)) as Product_PackSize, " +
                       " cast(G.Product_MRP as decimal(10,2)) as Product_MRP,Cast(G.Product_Crate_PackSize as decimal(10,2)) as Product_CrateSize,CAST(t.AMOUNT as decimal(10,2)) AS Product_Price  " +
                       " from [ax].[ACXINVENTTRANS] F (NOLOCK) " +
                       " Left Join ax.InventTable G (NOLOCK) on G.ItemId=F.[ProductCode] " +
                       " Inner join DBO.ACX_UDF_GETPRICE(getdate(),(select PriceGroup from ax.ACXCUSTMASTER (NOLOCK) where CUSTOMER_CODE = '" + drpCustomerCode.SelectedItem.Value + "'),'" + DDLMaterialCode.SelectedItem.Value + "') t on G.ItemId =t.ITEmRelation" +
                       " Where F.[SiteCode]='" + Session["SiteCode"].ToString() + "' and F.[ProductCode]='" + DDLMaterialCode.SelectedItem.Value + "' and F.[TransLocation]='" + Session["TransLocation"].ToString() + "' " +
                       " group by G.Product_PackSize,G.Product_MRP,G.Product_Crate_PackSize,t.AMOUNT ";

            dt = baseObj.GetData(strQuery);
            if (dt.Rows.Count > 0)
            {
                txtStockQty.Text = dt.Rows[0]["TransQty"].ToString();
                txtPack.Text = dt.Rows[0]["Product_PackSize"].ToString();
                txtCrateSize.Text = dt.Rows[0]["Product_CrateSize"].ToString();
                txtPrice.Text = dt.Rows[0]["Product_Price"].ToString();
                txtMRP.Text = dt.Rows[0]["Product_MRP"].ToString();
                ProductSubCategory();
            }
            else
            {
                txtStockQty.Text = "0.00";
                txtPack.Text = "0";
                txtMRP.Text = "0.00";
                txtCrateSize.Text = "0.00";
                txtPrice.Text = "0.00";
            }

            //txtCrateQty.Focus();
            // DataTable dt = new DataTable();
            string query = "select Product_Group,PRODUCT_SUBCATEGORY  from ax.INVENTTABLE where ItemId='" + DDLMaterialCode.SelectedItem.Value + "' order by Product_Name";
            dt = baseObj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                DDLProductGroup.Text = dt.Rows[0]["PRODUCT_GROUP"].ToString();
                ProductSubCategory();
                DDLProductSubCategory.Text = dt.Rows[0]["PRODUCT_SUBCATEGORY"].ToString();
            }
            txtCrateQty.Focus();
        }

        public void CalculateQtyAmt(Object sender)
        {
            decimal dblTotalQty = 0,crateQty = 0, boxQty = 0, pcsQty = 0,crateSize=0,boxSize=0;

            crateQty = Global.ParseDecimal(txtCrateQty.Text);
            txtCrateQty.Text = Convert.ToString(crateQty);

            boxQty = Global.ParseDecimal(txtBoxqty.Text);
            txtBoxqty.Text = Convert.ToString(boxQty);

            pcsQty = Global.ParseDecimal(txtPCSQty.Text);
            txtPCSQty.Text = Convert.ToString(pcsQty);

            crateSize = Global.ParseDecimal(txtCrateSize.Text);
            boxSize = Global.ParseDecimal(txtPack.Text);

            dblTotalQty = crateQty * crateSize + boxQty + (pcsQty / (boxSize == 0 ? 1 : boxSize));
            txtEnterQty.Text = dblTotalQty.ToString("0.00");
            txtQtyBox.Text = txtEnterQty.Text;
            txtBoxqty.Text = Convert.ToString(boxQty);
            txtPCSQty.Text = Convert.ToString(pcsQty);

            decimal TotalBox = 0, TotalPcs = 0;
            TotalBox = Math.Truncate(dblTotalQty);                          //Extract Only Box Qty From Total Qty
            TotalPcs = Math.Round((dblTotalQty - TotalBox) * boxSize);  //Extract Only Pcs Qty From Total Qty
            string BoxPcs = Convert.ToString(TotalBox) + '.' + (Convert.ToString(TotalPcs).Length == 1 ? "0" : "") + Convert.ToString(TotalPcs);

            // Call CalculatePrice 
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

                        txtQtyBox.Text = txtEnterQty.Text;
                        txtQtyCrates.Text = calValue[0];
                        if (string.IsNullOrEmpty(txtStockQty.Text.Trim()))
                        {
                            string message = "alert('Stock Not available!!');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            return;
                        }
                        if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                            string message = "alert('Box Qty should not greater than Stock Qty !');";
                            // string message = "alert('Please Enter a valid number!!');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
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
                    SetFocus(BtnAddItem);
                }
                else
                {
                    lblMessage.Text = "Price Not Define !";
                }
            }
            catch (Exception ex)
            {
                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void ProductSubCategory()
        {
            strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                        + "where P.PRODUCT_GROUP='" + DDLProductGroup.SelectedItem.Value + "'  AND P.BLOCK=0";
            DDLProductSubCategory.Items.Clear();
            DDLProductSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductSubCategory, strQuery, "Name", "Code");
            // FillProductCode();
            DDLProductSubCategory.Focus();
        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {
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
                    //txtPrice.Text = "";
                    txtValue.Text = "";
                    txtMRP.Text = "";
                    txtPack.Text = "";
                    //txtStockQty.Text = "";
                    DDLMaterialCode.Focus();
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');", true);
                    string message = "alert('" + DDLMaterialCode.SelectedItem.Text + " is already exists in the list .Please Select Another Product !!');";
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                    return;
                }

            }
            //============================
            bool valid = true;
            valid = ValidateLineItemAdd();
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                dt = AddLineItems();
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = true;
                    GridViewFooterCalculate(dt);

                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = false;
                }
            }
            txtEnterQty.Text = string.Empty;
            DDLMaterialCode.Focus();
            if (intApplicable == 1 || intApplicable == 3)
            {
                BindSchemeGrid();
            }
        }

        public void ProductGroup()
        {

            string strProductGroup = "Select Distinct a.Product_Group from ax.InventTable a where a.Product_Group <>''  AND a.BLOCK=0 order by a.Product_Group";
            DDLProductGroup.Items.Clear();
            DDLProductGroup.Items.Add("Select...");
            baseObj.BindToDropDown(DDLProductGroup, strProductGroup, "Product_Group", "Product_Group");

        }

        protected void txtBoxqty_TextChanged(object sender, EventArgs e)
        {
            if (DDLMaterialCode.SelectedItem.Text == "Select...")
            {
                string message = "alert('Please select product...!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                txtBoxqty.Text = "";
                return;
            }
            CalculateQtyAmt(sender);
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
                        if (string.IsNullOrEmpty(txtStockQty.Text.Trim()))
                        {
                            string message = "alert('Stock Not available!!');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                            return;
                        }
                        if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Enter a valid number!!');", true);
                            string message = "alert('Box Qty should not greater than Stock Qty !');";
                            // string message = "alert('Please Enter a valid number!!');";
                            ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
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
                    SetFocus(BtnAddItem);
                    //TxtSetFocus.Text = "1";
                    //TxtSetFocus.Focus();
                    //BtnAddItem.Focus();
                    // BtnAddItem.Focus();
                    //BtnAddItem.CausesValidation = false;
                    //  BtnAddItem.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtnAddItem, null) + ";");
                    //  upanel.Update();
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

                string message = "alert('" + ex.Message.ToString() + "');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
            
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        private bool ValidateLineItemAdd()
        {
            bool b = true;

            if (drpCustomerGroup.Text == "Select..." || drpCustomerGroup.Text == "")
            {
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                string message = "alert('Select Customer Group!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                drpCustomerGroup.Focus();
                b = false;
                return b;
            }

            if (drpCustomerCode.Text == "Select..." || drpCustomerCode.Text == "")
            {
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                string message = "alert('Select Customer Code!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                drpCustomerCode.Focus();
                b = false;
                return b;
            }

            if (DDLProductGroup.Text == "Select..." || DDLProductGroup.Text == "")
            {
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                string message = "alert('Select Product Group !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                DDLProductGroup.Focus();
                b = false;
                return b;
            }

            if (DDLMaterialCode.Text == string.Empty || DDLMaterialCode.Text == "Select...")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Product First !');", true);

                string message = "alert('Select Product First !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                DDLMaterialCode.Focus();
                b = false;
                return b;
            }
            if (txtQtyBox.Text == string.Empty || txtQtyBox.Text == "0")
            {
                b = false;
                string message = "alert('Qty cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be left blank !');", true);
                return b;
            }
            if (txtStockQty.Text == string.Empty || txtStockQty.Text == "0")
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Qty cannot be zero !');", true);

                string message = "alert('Qty cannot be zero !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (Convert.ToDecimal(txtQtyBox.Text) > Convert.ToDecimal(txtStockQty.Text))
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Box Qty should not greater than Stock Qty !');", true);
                string message = "alert('Box Qty should not greater than Stock Qty !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return b;
            }
            if (txtQtyCrates.Text == string.Empty)
            {
                b = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price cannot be left blank !');", true);
                string message = "alert('Price cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                return b;
            }
            if (txtLtr.Text == string.Empty || txtLtr.Text == "0")
            {
                b = false;
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('ltr cannot be left blank !');", true);
                string message = "alert('ltr cannot be left blank !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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
                if (Session["LineItem"] == null)
                {
                    AddColumnInDataTable();
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                }
                DataRow[] dataPerDay = (from myRow in dtLineItems.AsEnumerable()
                                        where myRow.Field<string>("Product_Code") == DDLMaterialCode.SelectedValue.ToString()
                                        select myRow).ToArray();
                if (dataPerDay.Count() == 0)
                {
                    #region  Discount
                    /////////////////////////////////// For /////////////////////////////////////////////////////

                    string DiscType = string.Empty;
                    string DiscCalculationBase = string.Empty;
                    decimal discMRP = 0;
                    decimal disc = 0;
                    decimal discValue = 0;
                    decimal TotalValue = 0;
                    decimal decFinalAmount = 0;
                    decimal pack = 0;
                    DataTable dt = new DataTable();
                    DataTable dt1 = baseObj.GetData("SELECT PRODUCT_MRP,ITEMID,coalesce(cast(Product_PackSize as decimal(10,2)),0) as Pack FROM AX.INVENTTABLE WHERE ITEMID = '" + DDLMaterialCode.SelectedValue.ToString() + "'");
                    discMRP = Convert.ToDecimal(dt1.Rows[0]["PRODUCT_MRP"].ToString());

                    DataTable dtApplicable = baseObj.GetData("SELECT APPLICABLESCHEMEDISCOUNT from  AX.ACXCUSTMASTER WHERE CUSTOMER_CODE = '" + drpCustomerCode.SelectedValue.ToString() + "'");
                    if (dtApplicable.Rows.Count > 0)
                    {
                        intApplicable = Convert.ToInt32(dtApplicable.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString());
                    }
                    intApplicable = 0;
                    if (intApplicable == 0)
                    {
                        dt = GetDatafromSPDiscount("[AX].[ACX_DISCOUNTFORFOC]", DDLMaterialCode.SelectedValue.ToString());


                        pack = Convert.ToDecimal(dt1.Rows[0]["Pack"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            DiscType = dt.Rows[0]["Calculation Type"].ToString();
                            DiscCalculationBase = dt.Rows[0]["CalculationBase"].ToString();
                            disc = Convert.ToDecimal(dt.Rows[0]["VALUE"].ToString());
                            if (DiscType == "Per")
                            {
                                if (DiscCalculationBase == "Price")
                                {
                                    //  disc = Convert.ToDecimal(disc.ToString("00"));
                                    disc = Convert.ToDecimal(disc);
                                    discValue = (Convert.ToDecimal(txtValue.Text.Trim().ToString()) * disc) / 100;
                                }
                                else
                                {

                                    if (dt.Rows.Count > 0)
                                    {
                                        decimal TaxCode1 = 0;
                                        dt = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + DDLMaterialCode.SelectedValue.ToString() + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                                        if (dt.Rows.Count > 0)
                                        {
                                            TaxCode1 = Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString());
                                        }

                                        // disc = Convert.ToDecimal(disc.ToString("00"));
                                        disc = Convert.ToDecimal(disc);
                                        decFinalAmount = (Convert.ToDecimal(txtQtyBox.Text.Trim().ToString()) * discMRP) / (1 + TaxCode1 / 100);
                                        discValue = (decFinalAmount * disc / 100);
                                        boolDiscAvalMRP = true;

                                        //disc = Convert.ToDecimal(disc.ToString("00"));
                                        //discValue = ((Convert.ToDecimal(txtQtyBox.Text.Trim().ToString()) * discMRP) * disc / 100);
                                    }
                                }
                            }
                            else
                            {
                                // disc = Convert.ToDecimal(disc.ToString("0.00"));
                                disc = Convert.ToDecimal(disc);
                                discValue = decimal.Parse(txtQtyBox.Text.Trim().ToString()) * disc;

                                if (DiscType == "Val")
                                {
                                    dt = new DataTable();
                                    dt = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + DDLMaterialCode.SelectedValue.ToString() + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                                    if (dt != null)
                                    {
                                        //discValue = discValue - ((discValue * Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString())) / 100);
                                        discValue = discValue / (1 + (Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString()) / 100));
                                    }
                                }
                            }
                        }
                        else
                        {
                            DiscCalculationBase = "0";
                            DiscType = "";
                            disc = 0;
                            discValue = 0;
                        }
                    }
                    else
                    {
                        DiscCalculationBase = "0";
                        DiscType = "";
                        disc = 0;
                        discValue = 0;
                    }
                    if (boolDiscAvalMRP == true)
                    {
                        TotalValue = decFinalAmount - discValue;
                    }
                    else
                    {
                        TotalValue = Convert.ToDecimal(txtValue.Text.Trim().ToString()) - discValue;
                    }


                    ////===============For Tax==================

                    decimal TaxCode = 0, TaxAmount = 0, AddTaxCode = 0, AddTaxAmount = 0;
                    dt = new DataTable();
                    dt = baseObj.GetData("Select  H.TaxValue,H.ACXADDITIONALTAXVALUE from [ax].inventsite G inner join InventTax H on G.TaxGroup=H.TaxGroup where H.ItemId='" + DDLMaterialCode.SelectedValue.ToString() + "' and G.Siteid='" + Session["SiteCode"].ToString() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        TaxCode = Convert.ToDecimal(dt.Rows[0]["TaxValue"].ToString());
                        TaxAmount = (TotalValue * TaxCode) / 100;

                        AddTaxCode = Convert.ToDecimal(dt.Rows[0]["ACXADDITIONALTAXVALUE"].ToString());
                        AddTaxAmount = (TotalValue * AddTaxCode) / 100;
                    }
                    TotalValue = TotalValue + TaxAmount + AddTaxAmount;
                    //========================================
                    int count = gvDetails.Rows.Count;
                    count = count + 1;
                    #endregion

                    DataRow row;
                    row = dtLineItems.NewRow();

                    row["Product_Code"] = DDLMaterialCode.SelectedValue.ToString();
                    row["Line_No"] = count;
                    row["Product"] = DDLMaterialCode.SelectedItem.Text.ToString();
                    row["Pack"] = pack;
                    row["So_Qty"] = "0";

                    row["Ltr"] = Convert.ToDecimal(txtLtr.Text.Trim().ToString());
                    row["Rate"] = Convert.ToDecimal(txtPrice.Text.Trim().ToString());
                    //row["Value"] = Convert.ToDecimal(txtValue.Text.Trim().ToString());
                    row["Amount"] = TotalValue.ToString("0.00");
                    if (DiscType == "")
                    {
                        row["DiscType"] = 2;
                    }
                    else if (DiscType == "Per")
                    {
                        row["DiscType"] = 0;
                    }
                    else if (DiscType == "Val")
                    {
                        row["DiscType"] = 1;
                    }
                    row["Disc"] = disc.ToString("0.00");
                    row["DiscVal"] = discValue.ToString("0.00");
                    //==========For Tax===============
                    row["TaxPer"] = TaxCode.ToString("0.00");
                    row["TaxValue"] = TaxAmount.ToString("0.00");
                    row["ADDTaxPer"] = AddTaxCode.ToString("0.00");
                    row["ADDTaxValue"] = AddTaxAmount.ToString("0.00");
                    row["Invoice_Qty"] = decimal.Parse(txtQtyBox.Text.Trim().ToString());
                    row["Box_Qty"] = Convert.ToDecimal(txtViewTotalBox.Text.Trim().ToString());// Math.Truncate(decimal.Parse(txtQtyBox.Text.Trim().ToString()));
                    row["Pcs_Qty"] = Convert.ToDecimal(txtViewTotalPcs.Text.Trim().ToString()); //(decimal.Parse(txtQtyBox.Text.Trim().ToString()) - Math.Truncate(decimal.Parse(txtQtyBox.Text.Trim().ToString()))) * Global.ParseDecimal(txtPack.Text);//decimal.Parse(txtPCSQty.Text.Trim().ToString());
                    row["BoxPcs"] = Convert.ToDecimal(txtBoxPcs.Text.Trim());
                    row["MRP"] = discMRP.ToString("0.00");//decimal.Parse(txtMRP.Text.Trim().ToString());
                    row["MRPVALUE"] = decimal.Parse(discMRP.ToString("0.00")) * decimal.Parse(txtQtyBox.Text.Trim().ToString());
                    row["StockQty"] = decimal.Parse(txtStockQty.Text.Trim().ToString());
                    row["SchemeCode"] = "";
                    row["Total"] = TotalValue.ToString("0.00");
                    row["TD"] = "0.00";
                    
                    row["BasePrice"] = Convert.ToDecimal(txtPrice.Text.Trim().ToString());
                    row["TaxableAmount"] = TotalValue;
                    int intCalculation = 0;

                    if (DiscCalculationBase == "Price")
                    {
                        intCalculation = 0;
                    }
                    else if (DiscCalculationBase == "MRP")
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
                        Session["LineItem"] = dtLineItems;

                        txtEnterQty.Text = string.Empty;
                       // txtStockQty.Text = string.Empty;
                        txtPCSQty.Text = string.Empty;
                        txtBoxqty.Text = string.Empty;
                        txtCrateQty.Text = string.Empty;
                        txtQtyCrates.Text = string.Empty;
                        txtQtyBox.Text = string.Empty;
                        txtLtr.Text = string.Empty;
                        txtPrice.Text = "0";
                        lblHidden.Text = string.Empty;
                        txtPack.Text = "";
                        txtMRP.Text = "";
                        txtStockQty.Text = "0";
                        txtValue.Text = string.Empty;
                        DDLMaterialCode.Focus();
                        txtViewTotalBox.Text = "";
                        txtViewTotalPcs.Text = "";
                        txtBoxPcs.Text = "";
                        DDLMaterialCode.SelectedIndex = 0;
                        DDLProductGroup.SelectedIndex = 0;
                        DDLProductSubCategory.SelectedIndex = 0;
                }
                return dtLineItems;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
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
            dtLineItems.Columns.Add("ProductCodeName", typeof(string));
            dtLineItems.Columns.Add("Pack", typeof(string));
            // dtLineItems.Columns.Add("Mrp", typeof(string));
            dtLineItems.Columns.Add("SOQty", typeof(decimal));
            dtLineItems.Columns.Add("InvoiceQty", typeof(decimal));
            dtLineItems.Columns.Add("Box_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Pcs_Qty", typeof(decimal));
            //===========================
            dtLineItems.Columns.Add("BoxPcs", typeof(decimal));
            dtLineItems.Columns.Add("BasePrice", typeof(decimal));
            dtLineItems.Columns.Add("TaxableAmount", typeof(decimal));
            //================================
            //dtLineItems.Columns.Add("InvoiceQty", typeof(decimal));
            dtLineItems.Columns.Add("QtyCrates", typeof(decimal));
            dtLineItems.Columns.Add("QtyBox", typeof(int));
            dtLineItems.Columns.Add("QtyLtr", typeof(decimal));
            dtLineItems.Columns.Add("Price", typeof(decimal));
            dtLineItems.Columns.Add("Value", typeof(decimal));
            dtLineItems.Columns.Add("UOM", typeof(string));
            dtLineItems.Columns.Add("DiscType", typeof(string));
            dtLineItems.Columns.Add("Disc", typeof(decimal));
            dtLineItems.Columns.Add("DiscVal", typeof(decimal));
            //===========Tax==============================
            dtLineItems.Columns.Add("Tax_Code", typeof(decimal));
            dtLineItems.Columns.Add("Tax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Code", typeof(decimal));
            dtLineItems.Columns.Add("AddTax_Amount", typeof(decimal));
            dtLineItems.Columns.Add("MRP", typeof(decimal));
            dtLineItems.Columns.Add("CalculationBase", typeof(string));
            //-------------
            dtLineItems.Columns.Add("Product_Code", typeof(string));
            dtLineItems.Columns.Add("Line_No", typeof(string));

            dtLineItems.Columns.Add("Product", typeof(string));
            dtLineItems.Columns.Add("So_Qty", typeof(decimal));
            dtLineItems.Columns.Add("Ltr", typeof(decimal));
            dtLineItems.Columns.Add("Rate", typeof(decimal));
            dtLineItems.Columns.Add("Amount", typeof(decimal));
            //dtLineItems.Columns.Add("DiscType", typeof(string));
            //dtLineItems.Columns.Add("Disc", typeof(string));
            //dtLineItems.Columns.Add("DiscVal", typeof(string));
            dtLineItems.Columns.Add("TaxPer", typeof(decimal));
            dtLineItems.Columns.Add("TaxValue", typeof(decimal));
            dtLineItems.Columns.Add("ADDTaxPer", typeof(decimal));
            dtLineItems.Columns.Add("ADDTaxValue", typeof(decimal));
            //dtLineItems.Columns.Add("MRP", typeof(string));
            dtLineItems.Columns.Add("StockQty", typeof(string));
            dtLineItems.Columns.Add("SchemeCode", typeof(string));
            dtLineItems.Columns.Add("Total", typeof(decimal));
            dtLineItems.Columns.Add("TD", typeof(decimal));
            dtLineItems.Columns.Add("DiscCalculationBase", typeof(string));
            dtLineItems.Columns.Add("Invoice_Qty", typeof(decimal));
            dtLineItems.Columns.Add("MRPVALUE", typeof(decimal));


        }

        protected void btnGO_Click(object sender, EventArgs e)
        {
            try
            {
                decimal SecDiscPer = 0, SecDiscValue = 0;
                if (string.IsNullOrEmpty(txtSecDiscPer.Text))
                {
                    txtSecDiscPer.Text = "0";
                }
                if (string.IsNullOrEmpty(txtSecDiscValue.Text))
                {
                    txtSecDiscValue.Text = "0";
                }


                //foreach (GridViewRow gv in gvDetails.Rows)
                for (int i = 0; gvDetails.Rows.Count > i; i++)
                {
                    //TextBox Invoice_Qty = (TextBox)gv.FindControl("txtBox");
                    //TextBox txtRate = (TextBox)gv.FindControl("txtRate");
                    //Label DiscPer = (Label)gv.FindControl("Disc");
                    //Label DiscVal = (Label)gv.FindControl("DiscValue");
                    //Label tax = (Label)gv.FindControl("Tax");
                    //Label taxvalue = (Label)gv.FindControl("TaxValue");
                    //Label AddtaxPer = (Label)gv.FindControl("AddTax");
                    //Label AddTaxVal = (Label)gv.FindControl("AddTaxValue");
                    //Label netamount = (Label)gv.FindControl("Amount");
                    //Label SDiscPer = (Label)gv.FindControl("SecDisc");
                    //Label SDiscVal = (Label)gv.FindControl("SecDiscValue");

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
                    Label lblTotal = (Label)gvDetails.Rows[i].FindControl("Total");

                    decimal Basic = Convert.ToDecimal(Invoice_Qty.Text) * Convert.ToDecimal(txtRate.Text);
                    if (SecDiscPer != 0)
                    {
                        SecDiscValue = (SecDiscPer * Basic) / 100;
                    }
                    decimal LineAmount = Basic - (SecDiscValue + Convert.ToDecimal(DiscVal.Text));
                    decimal TaxValue = (LineAmount * Convert.ToDecimal(tax.Text)) / 100;
                    decimal AddTaxValue = (LineAmount * Convert.ToDecimal(AddTaxVal.Text)) / 100;
                    decimal NetAmount = LineAmount + TaxValue + AddTaxValue;

                    taxvalue.Text = TaxValue.ToString("0.00");
                    AddTaxVal.Text = AddTaxValue.ToString("0.00");
                    SDiscPer.Text = SecDiscPer.ToString("0.00");
                    SDiscVal.Text = SecDiscValue.ToString("0.00");
                    netamount.Text = NetAmount.ToString("0.00");
                    lblTotal.Text = NetAmount.ToString("0.00");

                    if (txtTDValue.Text != "")
                    {
                        TDCalculation();
                    }
                    GrandTotal();
                }
                //Upnel.Update();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }

        }

        //protected void txtSecDiscPer_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtSecDiscValue.Text != string.Empty)
        //    {
        //        txtSecDiscValue.Text = string.Empty;
        //        btnGO.Focus();
        //    }
        //}
        //protected void txtSecDiscValue_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtSecDiscPer.Text != string.Empty)
        //    {
        //        txtSecDiscPer.Text = string.Empty;
        //        btnGO.Focus();
        //    }
        //}

        protected void SecDisc_TextChanged(object sender, EventArgs e)
        {
            decimal SecDiscPer = 0, SecDiscValue = 0;

            TextBox SDiscPer = (TextBox)sender;
            GridViewRow gv = (GridViewRow)SDiscPer.Parent.Parent;

            if (string.IsNullOrEmpty(SDiscPer.Text))
            {
                SDiscPer.Text = "0";
            }

            SecDiscPer = Convert.ToDecimal(SDiscPer.Text);

            int idx = gv.RowIndex;
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
            Label lblTotal = (Label)gv.FindControl("Total");

            decimal Basic = Convert.ToDecimal(Invoice_Qty.Text) * Convert.ToDecimal(txtRate.Text);
            if (SecDiscPer != 0)
            {
                SecDiscValue = (SecDiscPer * Basic) / 100;
            }
            decimal LineAmount = Basic - (SecDiscValue + Convert.ToDecimal(DiscVal.Text));
            decimal TaxValue = (LineAmount * Convert.ToDecimal(tax.Text)) / 100;
            decimal AddTaxValue = (LineAmount * Convert.ToDecimal(AddTaxVal.Text)) / 100;
            decimal NetAmount = LineAmount + TaxValue + AddTaxValue;

            taxvalue.Text = TaxValue.ToString("0.00");
            AddTaxVal.Text = AddTaxValue.ToString("0.00");
            SDiscPer.Text = SecDiscPer.ToString("0.00");
            SDiscVal.Text = SecDiscValue.ToString("0.00");
            netamount.Text = NetAmount.ToString("0.00");
            lblTotal.Text = NetAmount.ToString("0.00");

            if (txtTDValue.Text != "")
            {
                TDCalculation();
            }
            GrandTotal();
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

            int idx = gv.RowIndex;
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
            Label lblTotal = (Label)gv.FindControl("Total");

            decimal Basic = Convert.ToDecimal(Invoice_Qty.Text) * Convert.ToDecimal(txtRate.Text);

            decimal LineAmount = Basic - (SecDiscValue + Convert.ToDecimal(DiscVal.Text));
            decimal TaxValue = (LineAmount * Convert.ToDecimal(tax.Text)) / 100;
            decimal AddTaxValue = (LineAmount * Convert.ToDecimal(AddTaxVal.Text)) / 100;
            decimal NetAmount = LineAmount + TaxValue + AddTaxValue;

            taxvalue.Text = TaxValue.ToString("0.00");
            AddTaxVal.Text = AddTaxValue.ToString("0.00");
            SDiscPer.Text = SecDiscPer.ToString("0.00");
            SDiscVal.Text = SecDiscValue.ToString("0.00");
            netamount.Text = NetAmount.ToString("0.00");
            lblTotal.Text = NetAmount.ToString("0.00");

            if (txtTDValue.Text != "")
            {
                TDCalculation();
            }
            GrandTotal();
        }

        //protected void txtTDValue_TextChanged(object sender, EventArgs e)
        //{

        //}

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (txtTDValue.Text != "")
            {
                // gvDetails.Columns[11].Visible = false;
                TDCalculation();
                GrandTotal();
            }
            else
            {
                //gvDetails.Columns[11].Visible = true;
            }

        }

        public void TDCalculation()
        {
            try
            {

                decimal totalBasicValue = 0;
                //=========For calculate TD % ================
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    TextBox Invoice_Qty = (TextBox)gv.FindControl("txtBox");
                    TextBox txtRate = (TextBox)gv.FindControl("txtRate");

                    totalBasicValue = totalBasicValue + (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(Invoice_Qty.Text));
                }
                decimal TDPercentage = Convert.ToDecimal(txtTDValue.Text) / totalBasicValue;
                //=================Apply TD Percentage=================
                foreach (GridViewRow gv in gvDetails.Rows)
                {
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

                    if (txtSecDiscVal.Text == string.Empty)
                    {
                        txtSecDiscVal.Text = "0";
                    }
                    if (Convert.ToDecimal(Invoice_Qty.Text) > 0)
                    {
                        decimal BasicValue = (Convert.ToDecimal(txtRate.Text) * Convert.ToDecimal(Invoice_Qty.Text));
                        decimal TD = (BasicValue * TDPercentage);
                        lblTD.Text = TD.ToString("0.00");
                        decimal PE = TD * ((TaxPer / 100) / (1 + (TaxPer / 100)));
                        lblPE.Text = PE.ToString("0.00");
                        decimal BeforeTaxTotal = BasicValue - Convert.ToDecimal(DiscVal.Text) - Convert.ToDecimal(txtSecDiscVal.Text) - TD + PE;
                        lblToatlBeforeTax.Text = BeforeTaxTotal.ToString("0.00");
                        decimal VatAfterPE = BeforeTaxTotal * TaxPer / 100;
                        lblVatAfterPE.Text = VatAfterPE.ToString("0.00");
                        decimal total = BeforeTaxTotal + VatAfterPE;
                        lblTotal.Text = total.ToString("0.00");
                        if (Convert.ToDecimal(txtTDValue.Text) == 0 || txtTDValue.Text == string.Empty)
                        {
                            lblToatlBeforeTax.Text = "0.00";
                            lblVatAfterPE.Text = "0.00";

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

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        public void ClearDataEntryField()
        {
            txtCrateQty.Text = txtBoxqty.Text = txtPCSQty.Text =txtViewTotalBox.Text = txtViewTotalPcs.Text = txtEnterQty.Text ="0" ;
            txtBoxPcs.Text = txtLtr.Text = txtPrice.Text = txtValue.Text = txtStockQty.Text = "0";
        }

        public void FillProductCode()
        {
            DDLMaterialCode.Items.Clear();
            // DDLMaterialCode.Items.Add("Select...");
            if (DDLProductGroup.Text == "Select..." && DDLProductSubCategory.Text == "Select..." || DDLProductSubCategory.Text == "")
            {
                strQuery = "select distinct(ItemId) as Product_Code,concat([ITEMID],' - ',Product_Name) as Product_Name from ax.INVENTTABLE where block=0 order by Product_Name";
                DDLMaterialCode.Items.Clear();
                DDLMaterialCode.Items.Add("Select...");
                baseObj.BindToDropDown(DDLMaterialCode, strQuery, "Product_Name", "Product_Code");
                DDLMaterialCode.Focus();
            }
        }

        protected void TextCrateQty_TextChanged(object sender, EventArgs e)
        {
            if (DDLMaterialCode.SelectedItem.Text == "Select...")
            {
                string message = "alert('Please select product...!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                txtCrateQty.Text = "";
                return;
            }
            CalculateQtyAmt(sender);
        }

        protected void txtPCSQty_TextChanged(object sender, EventArgs e)
        {
            if (DDLMaterialCode.SelectedItem.Text == "Select...")
            {
                string message = "alert('Please select product...!!');";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                txtPCSQty.Text = "";
                return;
            }
            CalculateQtyAmt(sender);
        }

        #region

        private void ShowCustomerDetail(string strCustomerId)
        {
            DataTable dt = new DataTable();
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

            string query = "Select * from ax.Acxcustmaster where CUSTOMER_CODE='" + strCustomerId + "' and SITE_CODE ='" + Session["SiteCode"].ToString() + "' ";

            dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                //drpCustomerGroup.Items.Insert(0, new ListItem(dt.Rows[0]["Customer_Group"].ToString(), dt.Rows[0]["CUST_GROUP"].ToString()));
                //drpCustomerCode.Items.Insert(0, new ListItem(dt.Rows[0]["Customer_Name"].ToString(), dt.Rows[0]["Customer_Code"].ToString()));
                txtTIN.Text = dt.Rows[0]["VAT"].ToString();
                txtAddress.Text = dt.Rows[0]["Address1"].ToString();
                txtMobileNO.Text = dt.Rows[0]["Mobile_No"].ToString();
                txtTransporterName.Focus();
                //txtLoadSheetNumber.Text = dt.Rows[0]["Loadsheet_No"].ToString();
                //drpSONumber.Items.Insert(0, new ListItem(dt.Rows[0]["So_NO"].ToString(), dt.Rows[0]["So_NO"].ToString()));
                //txtSODate.Text = dt.Rows[0]["SO_Date"].ToString();
                // txtLoadsheetDate.Text = dt.Rows[0]["LoadSheet_Date"].ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
            }

        }
        protected void drpCustomerCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowCustomerDetail(drpCustomerCode.SelectedValue.ToString());
        }
        #endregion

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt1 = null;
            dt1 = (DataTable)Session["LineItem"];
            if (dt1.Rows.Count > 0)
            {
                dt1.Rows[e.RowIndex].Delete();
                gvDetails.DataSource = dt1;
                gvDetails.DataBind();
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    GridViewFooterCalculate(dt1);
                }
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
            if (txtBoxQty.Text == "")
            {
                txtBoxQty.Text = "0";
            }
            if (txtPcs.Text == "")
            {
                txtPcs.Text = "0";
            }


            decimal TotalBox;
            TotalBox = obj.GetTotalBox(ddlPrCode.Value, 0, Convert.ToDecimal(txtPcs.Text), Convert.ToDecimal(txtBoxQty.Text));
            textTotal.Text = Convert.ToString(TotalBox);
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

            if (txtBoxQty.Text == "")
            {
                txtBoxQty.Text = "0";
            }
            if (txtPcsQty.Text == "")
            {
                txtPcsQty.Text = "0";
            }

            decimal TotalBox;
            TotalBox = obj.GetTotalBox(ddlPrCode.Value, 0, Convert.ToDecimal(txtPcsQty.Text), Convert.ToDecimal(txtBoxQty.Text));
            textTotal.Text = Convert.ToDecimal(TotalBox).ToString("0.00");
            txtBox_TextChanged((object)textTotal, e);
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
    }
}
