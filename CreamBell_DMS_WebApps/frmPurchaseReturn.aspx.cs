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
using System.Drawing;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchaseReturn : System.Web.UI.Page
    {
        public DataTable dtLineItems;
        public DataTable dtTempLineItems;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        SqlDataAdapter adp1, adp2;
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string strmessage = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["USERID"] == null || Session["USERID"].ToString()==string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                //CalendarExtender1.StartDate = DateTime.Now;
                //CalendarExtender2.StartDate = DateTime.Now;

                this.txtEntryValue.Attributes.Add("onkeypress", "button_click(this,'" + this.BtnAddItem.ClientID + "')");
                FillMaterialGroup();
                FillReturnReasonType();
                Session["LineItem"] = null;
                ImgBtnSearch.Visible = false;           //Temporary Visiblilty false
            }
        }

        private void FillMaterialGroup()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //string strQuery = "Select distinct PRODUCT_GROUP from ax.acxproductmaster";
            string strQuery = "Select distinct PRODUCT_GROUP from ax.INVENTTABLE";
            DDLMaterialGroup.Items.Clear();
            DDLMaterialGroup.Items.Add("-Select-");
            obj.BindToDropDown(DDLMaterialGroup, strQuery, "PRODUCT_GROUP", "PRODUCT_GROUP");

        }

        private void FillReturnReasonType()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string strQuery = "Select distinct DAMAGEREASON_CODE,DAMAGEREASON_CODE + '-(' + DAMAGEREASON_NAME +')' as RETURNREASON  from [ax].[ACXDAMAGEREASON]";
            DDLReturnReason.Items.Clear();
            DDLReturnReason.Items.Add("-Select-");
            obj.BindToDropDown(DDLReturnReason, strQuery, "RETURNREASON", "DAMAGEREASON_CODE");
        }

        protected void DDLMaterialGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMaterialGroup.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                

                //string strQuery = " Select distinct  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                //               " ax.acxproductmaster where replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLMaterialGroup.SelectedItem.Text.ToString()+"' ";

                string strQuery = " Select distinct  replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '')  as SUBCATEGORY from  " +
                                  " ax.INVENTTABLE where replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '" + DDLMaterialGroup.SelectedItem.Text.ToString() + "' ";
                

                DDLProductSubCategory.Items.Clear();
                DDLMaterialCode.Items.Clear();
                DDLProductSubCategory.Items.Add("-Select-");
                obj.BindToDropDown(DDLProductSubCategory, strQuery, "SUBCATEGORY", "SUBCATEGORY");

            }
        }


        private DataTable AddLineItems()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

                if (Session["LineItem"] == null)
                {
                    #region Runtime Datatable for Line Items

                    DataColumn column = new DataColumn();
                    column.DataType = System.Type.GetType("System.Int32");
                    column.AutoIncrement = true;
                    column.AutoIncrementSeed = 1;
                    column.AutoIncrementStep = 1;
                    column.ColumnName = "SNO";
                    //-----------------------------------------------------------//
                    dtLineItems = new DataTable("LineItemTable");
                    dtLineItems.Columns.Add(column);
                    dtLineItems.Columns.Add("ProductGroup", typeof(string));
                    dtLineItems.Columns.Add("ProductSubCategory", typeof(string));
                    //dtLineItems.Columns.Add("ProductCode", typeof(string));
                    dtLineItems.Columns.Add("ProductName", typeof(string));
                    dtLineItems.Columns.Add("QtyBox", typeof(int));
                    dtLineItems.Columns.Add("QtyCrates", typeof(decimal));
                    dtLineItems.Columns.Add("QtyLtr", typeof(decimal));
                    dtLineItems.Columns.Add("UOM", typeof(string));
                    dtLineItems.Columns.Add("Price", typeof(decimal));
                    dtLineItems.Columns.Add("Discount", typeof(decimal));
                    dtLineItems.Columns.Add("Tax%", typeof(decimal));
                    dtLineItems.Columns.Add("TaxAmount", typeof(decimal));
                    dtLineItems.Columns.Add("Value", typeof(decimal));

                    #endregion
                }
                else
                {
                    dtLineItems = (DataTable)Session["LineItem"];
                }

                #region Check Duplicate Entry of Line Items through LINQ
                bool _exits =false;
                try
                {
                    if (dtLineItems.Rows.Count > 0)
                    {
                        var rowColl = dtLineItems.AsEnumerable();

                        var record = (from r in rowColl
                                      where r.Field<string>("ProductGroup") == DDLMaterialGroup.SelectedItem.Text &&
                                      r.Field<string>("ProductSubCategory") == DDLProductSubCategory.SelectedItem.Text.ToString()
                                      && r.Field<string>("ProductName") == DDLMaterialCode.SelectedItem.Text.ToString()
                                      select r.Field<int>("SNO")).First<int>();
                        if(record>=1)
                        {
                            _exits = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Error: You are adding duplicate product !');", true);
                        }
                        else
                        {
                            _exits = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _exits = false;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }

                #endregion

                if (_exits == false)
                {
                    #region Add New Row in Datatable with values
                    DataRow row;
                    row = dtLineItems.NewRow();


                    string[] arr = obj.CalculatePrice1(DDLMaterialCode.SelectedValue.ToString(), string.Empty, Convert.ToDecimal(txtEntryValue.Text), DDLEntryType.SelectedItem.Text.ToString());       //Get Calculation Value for Crate,Ltr//

                    if (arr != null)
                    {
                        #region add Line Items in Datatable

                        row["ProductGroup"] = DDLMaterialGroup.SelectedItem.Text.ToString();
                        row["ProductSubCategory"] = DDLProductSubCategory.SelectedItem.Text.ToString();
                        row["ProductName"] = DDLMaterialCode.SelectedItem.Text.ToString();

                        if (arr[5].ToString() == "Crate")
                        {
                            //row["QtyBox"] = Math.Round(decimal.Parse(arr[0].ToString()));
                            //row["QtyCrates"] = Math.Round(decimal.Parse(txtEntryValue.Text.Trim().ToString()));
                            //row["QtyLtr"] = arr[1].ToString();
                            //row["UOM"] = arr[4].ToString();
                            //row["Price"] = arr[2].ToString();
                            //row["Tax"] = "12.5";                                                               //by default it is 12.23
                            //row["Discount"] = "0";                                                              //by default it is 0
                            //row["Value"] = arr[3].ToString();

                            string[] CalculatedValueArray = CalculateNetValue(Convert.ToDecimal(txtPrice.Text), decimal.Parse(arr[0].ToString()));

                            row["QtyBox"] = Math.Round(decimal.Parse(arr[0].ToString()));
                            row["QtyCrates"] = Math.Round(decimal.Parse(txtEntryValue.Text.Trim().ToString()));
                            row["QtyLtr"] = arr[1].ToString();
                            row["UOM"] = arr[4].ToString();
                            row["Price"] = txtPrice.Text.ToString();
                            row["Discount"] = txtDiscount.Text.ToString();  
                            row["Tax%"] = txtTaxPerc.Text.ToString();
                            row["TaxAmount"] = CalculatedValueArray[1].ToString();
                            //row["Value"] = (decimal.Parse(arr[0].ToString()) * Convert.ToDecimal(CalculatedValueArray[2].ToString())).ToString();
                            row["Value"] = CalculatedValueArray[3].ToString();
                             
                        }

                        if (arr[5].ToString() == "Box")
                        {
                            //row["QtyBox"] = Math.Round(decimal.Parse(txtEntryValue.Text.Trim().ToString()));
                            //row["QtyCrates"] = Math.Round(decimal.Parse(arr[0].ToString()));
                            //row["QtyLtr"] = arr[1].ToString();
                            //row["UOM"] = arr[4].ToString();
                            //row["Price"] = arr[2].ToString();
                            //row["Tax"] = "12.5";                                                               
                            //row["Discount"] = "0";                                                              
                            //row["Value"] = arr[3].ToString();

                            string[] CalculatedValueArray = CalculateNetValue(Convert.ToDecimal(txtPrice.Text), decimal.Parse(txtEntryValue.Text.Trim().ToString()));

                            row["QtyBox"] = Math.Round(decimal.Parse(txtEntryValue.Text.Trim().ToString()));
                            row["QtyCrates"] = Math.Round(decimal.Parse(arr[0].ToString()));
                            row["QtyLtr"] = arr[1].ToString();
                            row["UOM"] = arr[4].ToString();
                            row["Price"] = txtPrice.Text.ToString();
                            row["Discount"] = txtDiscount.Text.ToString();
                            row["Tax%"] = txtTaxPerc.Text.ToString();
                            row["TaxAmount"] = CalculatedValueArray[1].ToString();
                            //row["Value"] = (decimal.Parse(txtEntryValue.Text.Trim().ToString()) * Convert.ToDecimal(CalculatedValueArray[2].ToString())).ToString();
                            row["Value"] = CalculatedValueArray[3].ToString();
                        }


                        dtLineItems.Rows.Add(row);
                        #endregion

                    }

                    #endregion
                }

                //Update session table
                Session["LineItem"] = dtLineItems;
                FillMaterialGroup();
                DDLMaterialCode.Items.Clear();
                DDLProductSubCategory.Items.Clear();
                txtEntryValue.Text = string.Empty;
                LiteralEntryType.Text = "Enter Box";
                DDLEntryType.SelectedIndex = 0;
                txtPrice.Text = string.Empty;
                txtTaxPerc.Text = string.Empty;
                
                return dtLineItems;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return dtLineItems;
        }

        private bool ValidateLineItemAdd()
        {
            bool b = false;

            if (DDLMaterialGroup.Text == "-Select-")
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Material Group !');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Group !');", true);
                DDLMaterialGroup.Focus();
                b = false;
                return b;
            }
            if (DDLProductSubCategory.Text == "-Select-")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Sub Category First !');", true);
                DDLProductSubCategory.Focus();
                b = false;
                return b;
            }

            if (DDLMaterialCode.Text == "-Select-")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Product Name First !');", true);
                DDLMaterialCode.Focus();
                b = false;
                return b;
            }
            
            if (txtEntryValue.Text == string.Empty)
            {
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Entry Value !');", true);
                ScriptManager.RegisterStartupScript(this,this.GetType(), "Alert", " alert('Please Provide Entry Value !');", true);
                txtEntryValue.Focus();
                b = false;
                return b;
            }


            if (txtEntryValue.Text.Length > 0 && txtEntryValue.Text != string.Empty)
            {
                int qty;
                if (!int.TryParse(txtEntryValue.Text, out qty))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Entered " + DDLEntryType.SelectedItem.Text.ToString() + " is not valid !');", true);
                    txtEntryValue.Focus();
                    b = false;
                    return b;
                }
                else
                {
                    if (Convert.ToDecimal(txtEntryValue.Text) == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' Entered " + DDLEntryType.SelectedItem.Text.ToString() + " cannot be 0 !');", true);
                        txtEntryValue.Focus();
                        b = false;
                        return b;
                    }
                    else
                    {
                        b = true;
                        return b;
                    }
                    //b = true;
                    //return b;
                }

                #region 
                //else
                //{
                //    if (txtPrice.Text == string.Empty)
                //    {
                //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price cannot be left blank !');", true);
                //        txtPrice.Focus();
                //        b = false;
                //        return b;
                //    }
                //    else
                //    {
                //        if (txtTaxPerc.Text == string.Empty)
                //        {
                //            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide TAX % !');", true);
                //            txtTaxPerc.Focus();
                //            b = false;
                //            return b;
                //        }
                //        else
                //        {
                //            b = true;
                //            return b;
                //        }
                //    }

                //}

                #endregion

            }

            #region
            //else
            //{
            //    b = false;
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Box Qty cannot be left blank !');", true);
            //    return b;
            //}

            //if (txtPrice.Text.Length > 0 && txtPrice.Text != string.Empty)
            //{
            //    decimal amount;
            //    if (!decimal.TryParse(txtPrice.Text, out amount))
            //    {
            //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price is not valid !');", true);
            //        txtPrice.Focus();
            //        b = false;
            //        return b;
            //    }
            //    else
            //    {
            //        if (txtTaxPerc.Text == string.Empty)
            //        {
            //            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide TAX % !');", true);
            //            txtTaxPerc.Focus();
            //            b = false;
            //            return b;
            //        }
            //        else
            //        {
            //            if (txtDiscount.Text == string.Empty)
            //            {
            //                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Discount Amount !');", true);
            //                txtDiscount.Focus();
            //                b = false;
            //                return b;
            //            }
            //            else
            //            {
            //                b = true;
            //                return b;
            //            }
            //        }
            //    }

            //}
            //else
            //{
            //    b = false;
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Price is cannot be left blank !');", true);
            //    return b;
            //}

            //if (txtTaxPerc.Text.Length > 0 && txtTaxPerc.Text != string.Empty)
            //{
            //    decimal amount;
            //    if (!decimal.TryParse(txtTaxPerc.Text, out amount))
            //    {
            //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Tax Perc is not valid !');", true);
            //        txtTaxPerc.Focus();
            //        b = false;
            //        return b;
            //    }
            //    else
            //    {
            //        if (txtDiscount.Text == string.Empty)
            //        {
            //            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Discount Amount !');", true);
            //            txtDiscount.Focus();
            //            b = false;
            //            return b;
            //        }
            //        else
            //        {
            //            b = true;
            //            return b;
            //        }
            //    }

            //}
            //else
            //{
            //    b = false;
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Tax % is cannot be left blank !');", true);
            //    return b;
            //}

            //if (txtValue.Text.Length > 0)
            //{
            //    decimal amount;
            //    if (!decimal.TryParse(txtValue.Text, out amount))
            //    {
            //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Value is not valid !');", true);
            //        txtValue.Focus();
            //        b = false;
            //        return b;
            //    }
            //    else
            //    {
            //        b = true;
            //        return b;
            //    }

            //}

            #endregion

            return b;

        }

        protected void BtnAddItem_Click(object sender, EventArgs e)
        {

            bool valid = ValidateLineItemAdd();
            if (valid == true)
            {
                DataTable dt = new DataTable();
                dt = Session["ItemTable"] as DataTable;
                if(!string.IsNullOrEmpty(txtTaxPerc.Text.Trim()))
                {
                    try
                    {
                        decimal strTaxAmount =Convert.ToDecimal(txtTaxPerc.Text.Trim());
                    }
                    catch (Exception ex)
                    {                        
                        if(ex.Message.Contains("nput string was not in a correct format"))
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid Tax Amount!');", true);
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            return;
                        }
                    }
                }
                else
                {
                    txtTaxPerc.Text = "0";
                }
                dt = AddLineItems();
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = true;
                    //gvDetails.Columns[9].Visible = false;
                    //gvDetails.Columns[10].Visible = false;
                    //gvDetails.Columns[11].Visible = false;
                    //gvDetails.Columns[12].Visible = false;

                    GridViewFooterTotalShow(dt);

                }
                else
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                    gvDetails.Visible = false;
                }
            }
        }

        private void GridViewFooterTotalShow(DataTable dt)
        {
            decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));          //For Total[Sum] Value Show in Footer--//
            gvDetails.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[13].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[13].Text = "Total:" + total.ToString("N2");
            gvDetails.FooterRow.Cells[13].Font.Bold = true;

            decimal price = dt.AsEnumerable().Sum(row => row.Field<decimal>("Price"));          //For Total[Sum] Price Show in Footer--//
            gvDetails.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[9].Text = "Total:" + price.ToString("N2");
            gvDetails.FooterRow.Cells[9].Font.Bold = true;

            int Box = dt.AsEnumerable().Sum(row => row.Field<int>("QtyBox"));                   //For Total[Sum] Box Show in Footer--//
            gvDetails.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[5].Text = "Tot:" + Box.ToString();
            gvDetails.FooterRow.Cells[5].Font.Bold = true;

            decimal Crate = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyCrates"));          //For Total[Sum] Show in Footer--//
            gvDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[6].Text = "Tot:" + Crate.ToString();
            gvDetails.FooterRow.Cells[6].Font.Bold = true;

            decimal Litre = dt.AsEnumerable().Sum(row => row.Field<decimal>("QtyLtr"));          //For Total[Sum] Litre Show in Footer--//
            gvDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            gvDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvDetails.FooterRow.Cells[7].Text = "Tot:" + Litre.ToString("N2");
            gvDetails.FooterRow.Cells[7].Font.Bold = true;

            //gvDetails.Columns[0].Visible = false;
            //gvDetails.AutoGenerateDeleteButton = true;
            //UpdatePanel4.Update();
        }

        protected void txtQtyCrates_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    int qty = string.IsNullOrEmpty(txtQtyCrates.Text) ? 0 : Convert.ToInt32(txtQtyCrates.Text);
            //    decimal Price = string.IsNullOrEmpty(txtPrice.Text) ? 0 : Convert.ToDecimal(txtPrice.Text);

            //    txtValue.Text = (qty * Price).ToString();
            //}
            //catch (Exception ex)
            //{

            //}
        }

        protected void txtPrice_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    //int qty = string.IsNullOrEmpty(txtQtyCrates.Text) ? 0 : Convert.ToInt32(txtQtyCrates.Text);
            //    decimal Price = string.IsNullOrEmpty(txtPrice.Text) ? 0 : Convert.ToDecimal(txtPrice.Text);
            //    decimal TaxPercent = string.IsNullOrEmpty(txtTaxPerc.Text) ? 0 : Convert.ToDecimal(txtTaxPerc.Text);
                
            //    string[] CalculatedValueArray = CalculateNetValue(Price);
            //    if (CalculatedValueArray != null)
            //    {
            //        //txtValue = CalculatedValueArray[]
            //    }
                
            //}
            //catch (Exception ex)
            //{

            //}
        }

        protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
        }

        protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
            
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            SavePurchaseReturn();
        }

        private void SavePurchaseReturn()
        {
            try
            {
                bool b = ValidatePurchaseReturnHeaderData();
                if (b == true)
                {
                    if (Session["LineItem"] != null)
                    {

                        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                        conn = obj.GetConnection();

                        string _query = "SELECT ISNULL(MAX(CAST(RIGHT(PURCH_RETURNNO,7) AS INT)),0)+1 FROM [ax].[ACXPURCHRETURNHEADER] where SITE_CODE='" + Session["SiteCode"].ToString() + "'";

                        cmd = new SqlCommand(_query);
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.Text;
                        object vc = cmd.ExecuteScalar();

                        
                        DataTable dtNumSeq = obj.GetNumSequenceNew(3, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                        string NUMSEQ = string.Empty;
                        string strCode = string.Empty;
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
                        cmd.CommandText = "ACX_ACXPURCHRETURNHEADER";


                        #region Header Insert Data

                        //Random rnd = new Random();
                        //int PurchaseReturnNo = rnd.Next(10000, 1000000);
                        //String PurchaseReturnNo = GeneratePurchaseReturnNo();
                        //int ReturnDocValue = rnd.Next(8000, 80000);

                        DataTable dt = Session["LineItem"] as DataTable;
                        decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));                  //For Header ReturnDocValue

                        string queryRecID = "Select Count(RECID) as RECID from [ax].[ACXPURCHRETURNHEADER]";
                        Int64 Recid = Convert.ToInt64(obj.GetScalarValue(queryRecID));

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SITE_CODE", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@PURCH_RETURNNO", strCode);
                        cmd.Parameters.AddWithValue("@PURCH_RECIEPTNO", txtReceiptNo.Text.Trim().ToString());
                        cmd.Parameters.AddWithValue("@PURCH_RECIEPTDATE", txtReceiptDate.Text);
                        cmd.Parameters.AddWithValue("@TRANSPORTER_CODE", txtDriverNumber.Text);
                        cmd.Parameters.AddWithValue("@SALE_INVOICENO", txtInvoiceNo.Text);
                        cmd.Parameters.AddWithValue("@SALE_INVOICEDATE", txtInvoiceDate.Text);
                        cmd.Parameters.AddWithValue("@VEHICAL_NO", txtVehicleNumber.Text);
                        cmd.Parameters.AddWithValue("@STATUS", 1);
                        cmd.Parameters.AddWithValue("@VEHICAL_TYPE", txtVehicleType.Text);
                        cmd.Parameters.AddWithValue("@RETURN_REASONCODE", DDLReturnReason.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@REMARK", txtRemark.Text.Trim().ToString());
                        cmd.Parameters.AddWithValue("@RETURN_DOCVALUE", total);
                        cmd.Parameters.AddWithValue("@RECID", Recid + 1);
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@NUMSEQ",NUMSEQ);

                        #endregion


                        #region Line Insert Data
                        cmd1 = new SqlCommand("ACX_ACXPURCHRETURNLINE");
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;

                        int i = 0;
                        foreach (GridViewRow grv in gvDetails.Rows)
                        {
                            i = i + 1;
                            cmd1.Parameters.Clear();


                            string productNameCode = grv.Cells[4].Text;
                            string[] str = productNameCode.Split('-');

                            string productCode = str[0].ToString();
                            string QtyBox = grv.Cells[5].Text;
                            string QtyCrate = grv.Cells[6].Text;
                            string QtyLtr = grv.Cells[7].Text;
                            string UOM = grv.Cells[8].Text;
                            string Price = grv.Cells[9].Text;
                            string Discount = grv.Cells[10].Text;
                            string TaxPerc = grv.Cells[11].Text;
                            string TaxAmount = grv.Cells[12].Text;
                            string Value = grv.Cells[13].Text;

                            cmd1.Parameters.AddWithValue("@PURCH_RETURNNO", strCode);
                            cmd1.Parameters.AddWithValue("@PRODUCT_CODE", productCode);
                            cmd1.Parameters.AddWithValue("@BOX", QtyBox);
                            cmd1.Parameters.AddWithValue("@CRATES", QtyCrate);
                            cmd1.Parameters.AddWithValue("@LTR", QtyLtr);
                            cmd1.Parameters.AddWithValue("@RATE", Convert.ToDecimal(Price));
                            cmd1.Parameters.AddWithValue("@UOM", UOM);
                            cmd1.Parameters.AddWithValue("@DISCOUNT", Convert.ToDecimal(Discount));
                            cmd1.Parameters.AddWithValue("@TAX", Convert.ToDecimal(TaxPerc));
                            cmd1.Parameters.AddWithValue("@TAXAMOUNT", Convert.ToDecimal(TaxAmount));
                            cmd1.Parameters.AddWithValue("@AMOUNT", Convert.ToDecimal(Value));
                            cmd1.Parameters.AddWithValue("@LINE_NO", i);
                            cmd1.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                            cmd1.Parameters.AddWithValue("@RECID", i);
                            cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());



                            cmd1.ExecuteNonQuery();
                        }

                        #endregion

                        cmd.ExecuteNonQuery();


                        SaveManualPurchaseReturnToInventTransTable(strCode, transaction, conn);
                        
                        transaction.Commit();
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Purchase Return Order : " + strCode.ToString() + " Generated Successfully.!');", true);
                        ResetAllControls();
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Inventory Affected Successfully.!');", true);
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Add Line Items First !');", true);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Error: Transaction Not Completed !');", true);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }   
        }

        private bool ValidatePurchaseReturnHeaderData()
        {
            bool returnvalue = false;

            //if (txtReceiptNo.Text == string.Empty)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Receipt No !');", true);
            //    txtReceiptNo.Focus();
            //    returnvalue = false;
            //    return returnvalue;
            //}
            //if (txtReceiptDate.Text == string.Empty)
            //{
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Receipt Date!');", true);
            //    txtReceiptDate.Focus();
            //    returnvalue = false;
            //    return returnvalue;
            //}
            if (txtInvoiceNo.Text == string.Empty)
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide Invoice No!');", true);
                txtInvoiceNo.Focus();
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
               
            if (DDLReturnReason.Text == "-Select-")
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Return Reason  !');", true);
                DDLReturnReason.Focus();
                returnvalue = false;
                return returnvalue;
            }
            else
            {
                returnvalue = true;
            }

            return returnvalue;
        }

        private void ResetAllControls()
        {
            txtReceiptNo.Text = string.Empty;
            txtReceiptDate.Text = string.Empty;
            txtDriverNumber.Text = string.Empty;
            txtTransporterName.Text = string.Empty;
            txtVehicleNumber.Text = string.Empty;
            //txtReturnReason.Text = string.Empty;
            txtInvoiceNo.Text = string.Empty;
            txtInvoiceDate.Text = string.Empty;
            txtDriverNumber.Text = string.Empty;
            txtVehicleType.Text = string.Empty;
            txtRemark.Text = string.Empty;
            Session["LineItem"] = null;
            gvDetails.DataSource = null;
            gvDetails.Visible = false;
            FillMaterialGroup();
            DDLMaterialCode.Items.Clear();
            DDLEntryType.SelectedIndex = 0;
            txtEntryValue.Text = string.Empty;
            LiteralEntryType.Text = string.Empty;
            //txtQtyCrates.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtTaxPerc.Text = string.Empty;
            //txtValue.Text = string.Empty;
            FillReturnReasonType();

            //txtDiscount.Text = string.Empty;
        }

        protected void DDLMaterialCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMaterialCode.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();


                //string strQuery = " Select PRODUCT_MRP as PRICE, PRODUCT_PACKSIZE as BOX,PRODUCT_CRATE_PACKSIZE as CRATE , LTR from ax.acxproductmaster where " +
                //                  " replace(replace(PRODUCT_CODE, char(9), ''), char(13) + char(10), '') = '" + DDLMaterialCode.SelectedValue.ToString() + "'";

                string strQuery = " Select PRODUCT_MRP as PRICE, PRODUCT_PACKSIZE as BOX,PRODUCT_CRATE_PACKSIZE as CRATE , LTR from ax.INVENTTABLE where " +
                                  " replace(replace(ITEMID, char(9), ''), char(13) + char(10), '') = '" + DDLMaterialCode.SelectedValue.ToString() + "'";

                DataTable dt= obj.GetData(strQuery);
                if (dt.Rows.Count > 0)
                {
                    decimal Ltr = Convert.ToDecimal(dt.Rows[0]["LTR"].ToString());
                    decimal Crate = Convert.ToDecimal(dt.Rows[0]["CRATE"].ToString());
                    decimal price = Convert.ToDecimal(dt.Rows[0]["PRICE"].ToString());

                    //txtLtr.Text = Math.Round(Ltr,2).ToString();
                    //txtQtyCrates.Text = Math.Round(Crate,2).ToString();
                    //txtPrice.Text = Math.Round(price, 2).ToString();
                }
            }
           


        }

        protected void txtQtyBox_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    int qty = string.IsNullOrEmpty(txtQtyBox.Text) ? 0 : Convert.ToInt32(txtQtyBox.Text);
            //    decimal Price = string.IsNullOrEmpty(txtPrice.Text) ? 0 : Convert.ToDecimal(txtPrice.Text);

            //    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //    string[] arr = obj.CalculatePrice(DDLMaterialCode.SelectedValue.ToString(), Convert.ToInt32(txtQtyBox.Text));

            //    txtQtyCrates.Text = arr[0].ToString();
            //    txtLtr.Text = arr[1].ToString();
            //    txtPrice.Text = arr[2].ToString();
            //    txtValue.Text = arr[3].ToString();
                
            //}
            //catch (Exception ex)
            //{

            //}
        }

        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            HiddenField hiddenField = (HiddenField)gvrow.FindControl("HiddenField2");
            LinkButton lnk = sender as LinkButton;

            if (Session["LineItem"] != null)
            {
                DataTable dt = Session["LineItem"] as DataTable;
                dt.AsEnumerable().Where(r => r.Field<int>("SNO") == Convert.ToInt32(hiddenField.Value)).ToList().ForEach(row => row.Delete());
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                GridViewFooterTotalShow(dt);

                Session["LineItem"] = dt;
            }
        }

        public string GeneratePurchaseReturnNo()
        {
            try
            {

                string IndNo = string.Empty;
                string Number = string.Empty;
                int intTotalRec;
                string strQuery = "Select coalesce(max(PURCH_RETURNNO),0) as NewPurchreturnNo from [ax].[ACXPURCHRETURNHEADER] where SITE_CODE='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                dt = obj.GetData(strQuery);
                intTotalRec = dt.Rows.Count;

                if (dt.Rows[0]["NewPurchreturnNo"].ToString() != "0")
                {
                    string st = dt.Rows[0]["NewPurchreturnNo"].ToString();
                    if (st.Length < 10)
                    {
                        int len = st.Length;
                        int plen = 10 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                    }
                    Number = st.Substring(6, 4);
                    int intnumber = Convert.ToInt32(Number) + 1;
                    Number = intnumber.ToString().PadLeft(4, '0');
                    st = Session["SiteCode"].ToString();
                    if (st.Length < 10)
                    {
                        int len = st.Length;
                        int plen = 10 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                    }
                    IndNo = st.Substring(4, 6) + Number;
                    return IndNo;
                }
                else
                {
                    int intnumber = 1;
                    Number = intnumber.ToString().PadLeft(4, '0');
                    string st = Session["SiteCode"].ToString();
                    if (st.Length < 10)
                    {
                        int len = st.Length;
                        int plen = 10 - len;
                        for (int i = 0; i < plen; i++)
                        {
                            st = "0" + st;
                        }
                    }
                    IndNo = st.Substring(4, 6) + Number;
                    return IndNo;
                }
            }
            catch (Exception ex)
            {
                strmessage = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
                return strmessage;
            }

        }

        public string ReturnProductName(string ProductCode)
        {
            string ProductName = string.Empty;
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string query = "Select distinct PRODUCT_NAME from ax.ACXPRODUCTMASTER where PRODUCT_CODE='" + ProductCode + "'";
                ProductName = obj.GetScalarValue(query);
                return ProductName;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return ProductName;
            }
        }

        protected void DDLProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLProductSubCategory.Text != "-Select-")
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                //string strQuery = " Select PRODUCT_NAME, PRODUCT_CODE,PRODUCT_GROUP from ax.acxproductmaster where " +
                //                  " replace(replace(PRODUCT_GROUP, char(9), ''), char(13) + char(10), '') = '"+DDLMaterialGroup.SelectedItem.Text.ToString() +"' ";

                //string strQuery = " Select PRODUCT_CODE +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, PRODUCT_CODE,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.acxproductmaster where " +
                //                  " replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLProductSubCategory.SelectedItem.Text.ToString() + "' ";

                string strQuery = " Select ITEMID +'-(' + PRODUCT_NAME+')' as PRODUCT_NAME, ITEMID,PRODUCT_GROUP, PRODUCT_SUBCATEGORY from ax.INVENTTABLE where " +
                                 " replace(replace(PRODUCT_SUBCATEGORY, char(9), ''), char(13) + char(10), '') = '" + DDLProductSubCategory.SelectedItem.Text.ToString() + "' ";

                DDLMaterialCode.Items.Clear();
                DDLMaterialCode.Items.Add("-Select-");
                obj.BindToDropDown(DDLMaterialCode, strQuery, "PRODUCT_NAME", "ITEMID");
            }
        }

        protected void DDLEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLEntryType.SelectedItem.Text.ToString() == "-Select-")
            {
                LiteralEntryType.Text = "";
            }
            if (DDLEntryType.SelectedItem.Text.ToString() == "Crate")
            {
                LiteralEntryType.Text = "Enter Crate";
            }
            if (DDLEntryType.SelectedItem.Text.ToString() == "Box")
            {
                LiteralEntryType.Text = "Enter Box";
            }
            

        }

        protected void BtnNew_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvDetails.AllowPaging = false;
                DataTable dt = Session["LineItem"] as DataTable;
                gvDetails.DataSource = dt;
                gvDetails.DataBind();

                gvDetails.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvDetails.HeaderRow.Cells)
                {
                    cell.BackColor = gvDetails.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvDetails.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvDetails.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvDetails.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvDetails.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        
            
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void txtEntryValue_TextChanged(object sender, EventArgs e)
        {
            if (txtEntryValue.Text != string.Empty)
            {
                BtnAddItem.Focus();
            }
        }

        private string[] CalculateNetValue(decimal price, decimal box)
        {
            string[] returnResult = null;
            try
            {
               
                decimal priceAfterTax;
                decimal taxPerc;
                decimal taxAmount;
                decimal NetValue;

                //decimal Rate = price * Convert.ToDecimal(txtEntryValue.Text);
                decimal Rate = price * box;

                taxPerc = (Convert.ToDecimal(txtTaxPerc.Text) / 100);
                taxAmount = (Convert.ToDecimal(taxPerc * Rate));
                priceAfterTax = (Convert.ToDecimal(Rate + taxAmount));
                NetValue = priceAfterTax;

                returnResult = new string[4];
                returnResult[0] = taxPerc.ToString();
                returnResult[1] = taxAmount.ToString();
                returnResult[2] = priceAfterTax.ToString();
                returnResult[3] = NetValue.ToString();
                return returnResult;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return returnResult;
            }
            
        }

        protected void txtTaxPerc_TextChanged(object sender, EventArgs e)
        {
            BtnAddItem.Focus();
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
                //transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                string st = Session["SiteCode"].ToString();
                string TransId = st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yymmddhhmmss");

                for (int i = 0; i < gvDetails.Rows.Count; i++)
                {
                    string Siteid = Session["SiteCode"].ToString();
                    string DATAAREAID = Session["DATAAREAID"].ToString();
                    int TransType = 5;                                          // Type 5 for Manual Purchase Return
                    int DocumentType = 5;
                    string DocumentNo = PurcReturnCode;                                
                    string productNameCode = gvDetails.Rows[i].Cells[4].Text;
                    string[] str = productNameCode.Split('-');
                    string ProductCode = str[0].ToString();
                    decimal TransQty = Convert.ToDecimal(gvDetails.Rows[i].Cells[5].Text) * -1;
                    string UOM = gvDetails.Rows[i].Cells[8].Text;
                    //string TransLocation = st.Substring(st.Length - 6);
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
                //transaction.Commit();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Inventory Affected Successfully.!');", true);
                //cmd.Dispose();
                //conn.Close();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }


        }

    }
}