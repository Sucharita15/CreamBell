using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using ClosedXML.Excel;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSaleRegister : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlConnection conn = null;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
				DDLBusinessUnit.Items.Clear();
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                DDLBusinessUnit.Items.Add("All...");
                baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
                ShowSaleRegister();
            }

        }

        private void ShowSaleRegister()
        {
            try
            {
                //=====================New==================
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                DataTable dt = new DataTable();
                string FromDate = System.DateTime.Today.ToString("dd-MMM-yyyy");
                string ToDate = System.DateTime.Today.ToString("dd-MMM-yyyy");// System.DateTime.Today.AddDays(1).ToString("dd-MMM-yyyy");
                txtFromDate.Text = FromDate;
                txtToDate.Text = ToDate;

                string query = "ACX_USP_SaleRegister";
                List<string> ilist = new List<string>();
                List<string> item = new List<string>();
               
                ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
                ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                ilist.Add("@StartDate"); item.Add(txtFromDate.Text); 
                ilist.Add("@EndDate"); item.Add(txtToDate.Text);
                ilist.Add("@BUCODE");
                if (DDLBusinessUnit.SelectedIndex >= 1)
                {
                    item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                }
                else
                {
                    item.Add("");
                }

                dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                //dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gridSaleRegister.DataSource = dt;
                    gridSaleRegister.DataBind();
                    LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                    Session["SaleRegister"] = dt;
                }
                else
                {
                    LblMessage.Text = string.Empty;
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            bool b = ValidateSearch();

            if (b == true)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                try
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;

                    if (rdoDetailedView.Checked == true)
                    {
                        string query = "ACX_USP_SaleRegister";
                        List<string> ilist = new List<string>();
                        List<string> item = new List<string>();
                        DataTable dt = new DataTable();

                        ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
                        ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                        ilist.Add("@EndDate"); item.Add(txtToDate.Text);
                        ilist.Add("@BUCODE");
                        if (DDLBusinessUnit.SelectedIndex >= 1)
                        {
                            item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            item.Add("");
                        }

                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                        if (dt.Rows.Count > 0)
                        {
                            gridSaleRegisterbottom.Visible = false;
                            gridSaleRegister.Visible = true;
                            gridSaleRegister.DataSource = dt;
                            gridSaleRegister.DataBind();
                            gridSaleRegister.ShowFooter = false;
                            LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                            Session["SaleRegister"] = dt;
                        }
                        else
                        {
                            gridSaleRegisterbottom.Visible = false;
                            LblMessage.Text = string.Empty;
                            gridSaleRegister.DataSource = dt;
                            gridSaleRegister.DataBind();

                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                        }
                    }
                    if (rdoSummarisedView.Checked == true)
                    {
                        string query = "ACX_USP_SaleRegister_Summary";
                        List<string> ilist = new List<string>();
                        List<string> item = new List<string>();
                        DataTable dt = new DataTable();

                        ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
                        //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                        ilist.Add("@EndDate"); item.Add(txtToDate.Text);
                        ilist.Add("@BUCODE");
                        if (DDLBusinessUnit.SelectedIndex >= 1)
                        {
                            item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            item.Add("");
                        }
                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                        if (dt.Rows.Count > 0)
                        {
                            gridSaleRegister.Visible = true;
                            gridSaleRegister.ShowFooter = true;
                            gridSaleRegister.DataSource = dt;
                            gridSaleRegister.DataBind();
                            GridViewFooterCalculate(dt);
                            LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                            Session["SaleRegister"] = dt;
                        }
                        else
                        {

                            LblMessage.Text = string.Empty;
                            gridSaleRegister.DataSource = dt;
                            gridSaleRegister.DataBind();

                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                        }

                        //------------------------------------------------------------------------//

                        string bottomquery = "ACX_USP_SaleRegister_Summary2";
                        List<string> bottomilist = new List<string>();
                        List<string> bottomitem = new List<string>();
                        DataTable bottomdt = new DataTable();

                        bottomilist.Add("@Site_Code"); bottomitem.Add(Session["SiteCode"].ToString());
                        //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        bottomilist.Add("@StartDate"); bottomitem.Add(txtFromDate.Text);
                        bottomilist.Add("@EndDate"); bottomitem.Add(txtToDate.Text);
                        bottomilist.Add("@BUCODE");
                        if (DDLBusinessUnit.SelectedIndex >= 1)
                        {
                            bottomitem.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                        }
                        else
                        {
                            bottomitem.Add("");
                        }

                        bottomdt = obj.GetData_New(bottomquery, CommandType.StoredProcedure, bottomilist, bottomitem);

                        if (bottomdt.Rows.Count > 0)
                        {
                            gridSaleRegisterbottom.Visible = true;
                            gridSaleRegisterbottom.ShowFooter = true;
                            gridSaleRegisterbottom.DataSource = bottomdt;
                            gridSaleRegisterbottom.DataBind();
                            GridViewFooterCalculatebottom(bottomdt);
                            LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                            //Session["PurchaseRegister"] = dt;
                        }
                        else
                        {
                            gridSaleRegisterbottom.Visible = true;
                            LblMessage.Text = string.Empty;
                            gridSaleRegisterbottom.DataSource = bottomdt;
                            gridSaleRegisterbottom.DataBind();
                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exits Between This Date Range !');", true);
                            LblMessage.Text = "No Data Exits Between This Date Range  !";
                            LblMessage.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }            
        }

        private void GridViewFooterCalculatebottom(DataTable dt)
        {
            decimal units = dt.AsEnumerable().Sum(row => row.Field<decimal>("UNITS"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTRS"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASIC VALUE"));
            decimal DiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC AMOUNT"));
            decimal SchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCHEME DISC AMT"));
            decimal AddSchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCHEME DISC AMT"));
            decimal Disc2Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC2 AMT"));
            decimal TdAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD AMT"));
            decimal PeAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("PE AMT"));
            decimal TaxableValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLE VALUE"));

            decimal Gstamt1 = dt.AsEnumerable().Sum(row => row.Field<decimal>("GST AMT1"));
            decimal Gstamt2 = dt.AsEnumerable().Sum(row => row.Field<decimal>("GST AMT2"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));

            //==============================

            gridSaleRegisterbottom.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gridSaleRegisterbottom.FooterRow.Cells[0].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[0].Text = "TOTAL";
            gridSaleRegisterbottom.FooterRow.Cells[0].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[1].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[1].Text = units.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[1].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[2].Text = ltrs.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[2].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[3].Text = BASICVALUE.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[3].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[4].Text = DiscAmount.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[4].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[5].Text = SchemeDiscAmt.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[5].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[6].Text = AddSchemeDiscAmt.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[6].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[7].Text = Disc2Amt.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[7].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[8].Text = TdAmt.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[8].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[9].Text = PeAmt.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[9].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[10].Text = TaxableValue.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[10].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[12].Text = Gstamt1.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[12].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[14].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[14].Text = Gstamt2.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[14].Font.Bold = true;

            gridSaleRegisterbottom.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegisterbottom.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegisterbottom.FooterRow.Cells[15].Text = invvalue.ToString("N2");
            gridSaleRegisterbottom.FooterRow.Cells[15].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }


        private void GridViewFooterCalculatebottomNew(DataTable dt,GridView gvname)
        {
            decimal units = dt.AsEnumerable().Sum(row => row.Field<decimal>("UNITS"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTRS"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASIC VALUE"));
            decimal DiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC AMOUNT"));
            decimal SchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCHEME DISC AMT"));
            decimal AddSchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCHEME DISC AMT"));
            decimal Disc2Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC2 AMT"));
            decimal TdAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD AMT"));
            decimal PeAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("PE AMT"));
            decimal TaxableValue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLE VALUE"));

            decimal Gstamt1 = dt.AsEnumerable().Sum(row => row.Field<decimal>("GST AMT1"));
            decimal Gstamt2 = dt.AsEnumerable().Sum(row => row.Field<decimal>("GST AMT2"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));

            //==============================

            gvname.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //gvname.FooterRow.Cells[0].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[0].Text = "TOTAL";
            gvname.FooterRow.Cells[0].Font.Bold = true;

            gvname.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[1].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[1].Text = units.ToString("N2");
            gvname.FooterRow.Cells[1].Font.Bold = true;

            gvname.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[2].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[2].Text = ltrs.ToString("N2");
            gvname.FooterRow.Cells[2].Font.Bold = true;

            gvname.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[3].Text = BASICVALUE.ToString("N2");
            gvname.FooterRow.Cells[3].Font.Bold = true;

            gvname.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[4].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[4].Text = DiscAmount.ToString("N2");
            gvname.FooterRow.Cells[4].Font.Bold = true;

            gvname.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[5].Text = SchemeDiscAmt.ToString("N2");
            gvname.FooterRow.Cells[5].Font.Bold = true;

            //GridPurchItems.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            //GridPurchItems.FooterRow.Cells[11].ForeColor = System.Drawing.Color.MidnightBlue;
            //GridPurchItems.FooterRow.Cells[11].Text = DiscVal.ToString("N2");
            //GridPurchItems.FooterRow.Cells[11].Font.Bold = true;

            gvname.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[6].Text = AddSchemeDiscAmt.ToString("N2");
            gvname.FooterRow.Cells[6].Font.Bold = true;

            gvname.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[7].Text = Disc2Amt.ToString("N2");
            gvname.FooterRow.Cells[7].Font.Bold = true;

            gvname.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[8].Text = TdAmt.ToString("N2");
            gvname.FooterRow.Cells[8].Font.Bold = true;

            gvname.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[9].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[9].Text = PeAmt.ToString("N2");
            gvname.FooterRow.Cells[9].Font.Bold = true;

            gvname.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[10].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[10].Text = TaxableValue.ToString("N2");
            gvname.FooterRow.Cells[10].Font.Bold = true;

            gvname.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[12].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[12].Text = Gstamt1.ToString("N2");
            gvname.FooterRow.Cells[12].Font.Bold = true;

            gvname.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[14].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[14].Text = Gstamt2.ToString("N2");
            gvname.FooterRow.Cells[14].Font.Bold = true;

            gvname.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Left;
            //gvname.FooterRow.Cells[15].ForeColor = System.Drawing.Color.MidnightBlue;
            gvname.FooterRow.Cells[15].Text = invvalue.ToString("N2");
            gvname.FooterRow.Cells[15].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void GridViewFooterCalculate(DataTable dt)
        {
            decimal TotalQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("TOTAL QTY"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTR"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASIC AMT"));
            decimal DiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC AMOUNT"));
            decimal SchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCHEME DISC AMT"));
            decimal AddSchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCHEME DISC AMT"));
            decimal Disc2Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC2 AMT"));
            decimal TdAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD AMT"));
            decimal PeAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("PE AMT"));
            decimal TaxableAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLE AMT"));
            decimal Tax1Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            decimal Tax2Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));
            //decimal AddSchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCHEME DISC AMT"));
            //decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            //decimal Priceequalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRICE EQUAL VALUE"));
            //decimal secdiscamount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC DISC AMOUNT"));
            //decimal schsplvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCH SPL VAL"));
            //decimal taxablevalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLEVALUE"));

            //decimal Tax1amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            //decimal Tax2amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));


            //==============================

            gridSaleRegister.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Center;
            gridSaleRegister.FooterRow.Cells[16].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[16].Text = "TOTAL";
            gridSaleRegister.FooterRow.Cells[16].Font.Bold = true;

            gridSaleRegister.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[19].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[19].Text = TotalQty.ToString("N2");
            gridSaleRegister.FooterRow.Cells[19].Font.Bold = true;

            gridSaleRegister.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[20].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[20].Text = ltrs.ToString("N2");
            gridSaleRegister.FooterRow.Cells[20].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[21].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[21].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[21].Text = BASICVALUE.ToString("N2");
            gridSaleRegister.FooterRow.Cells[21].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[22].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[22].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[22].Text = DiscAmount.ToString("N2");
            gridSaleRegister.FooterRow.Cells[22].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[23].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[23].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[23].Text = SchemeDiscAmt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[23].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[24].HorizontalAlign = HorizontalAlign.Center;
            gridSaleRegister.FooterRow.Cells[24].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[24].Text = AddSchemeDiscAmt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[24].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[25].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[25].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[25].Text = Disc2Amt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[25].Font.Bold = true;
            
            
            gridSaleRegister.FooterRow.Cells[26].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[26].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[26].Text = TdAmt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[26].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[27].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[27].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[27].Text = PeAmt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[27].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[28].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[28].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[28].Text = TaxableAmt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[28].Font.Bold = true;
            
            gridSaleRegister.FooterRow.Cells[30].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[30].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[30].Text = Tax1Amt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[30].Font.Bold = true;

            gridSaleRegister.FooterRow.Cells[32].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[32].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[32].Text = Tax2Amt.ToString("N2");
            gridSaleRegister.FooterRow.Cells[32].Font.Bold = true;

            gridSaleRegister.FooterRow.Cells[33].HorizontalAlign = HorizontalAlign.Left;
            gridSaleRegister.FooterRow.Cells[33].ForeColor = System.Drawing.Color.MidnightBlue;
            gridSaleRegister.FooterRow.Cells[33].Text = invvalue.ToString("N2");
            gridSaleRegister.FooterRow.Cells[33].Font.Bold = true;

            //if (GridPurchItems.Rows.Count > 0)
            //{
            //    txtReceiptValue.Text = NetValue.ToString("N2");
            //}
            //else
            //{
            //    txtReceiptValue.Text = "0.00";
            //}


        }

        private void GridViewFooterCalculateNew(DataTable dt,GridView gvname)
        {
            decimal TotalQty = dt.AsEnumerable().Sum(row => row.Field<decimal>("TOTAL QTY"));          //For Total[Sum] Value Show in Footer--//
            decimal ltrs = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTR"));
            decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASIC AMT"));
            decimal DiscAmount = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC AMOUNT"));
            decimal SchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCHEME DISC AMT"));
            decimal AddSchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCHEME DISC AMT"));
            decimal Disc2Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("DISC2 AMT"));
            decimal TdAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TD AMT"));
            decimal PeAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("PE AMT"));
            decimal TaxableAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLE AMT"));
            decimal Tax1Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            decimal Tax2Amt = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));
            decimal invvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("INV VALUE"));
            //decimal AddSchemeDiscAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCHEME DISC AMT"));
            //decimal TRDDISCVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("TRDDISCVALUE"));
            //decimal Priceequalvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("PRICE EQUAL VALUE"));
            //decimal secdiscamount = dt.AsEnumerable().Sum(row => row.Field<decimal>("SEC DISC AMOUNT"));
            //decimal schsplvalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("SCH SPL VAL"));
            //decimal taxablevalue = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAXABLEVALUE"));

            //decimal Tax1amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX1 AMT"));
            //decimal Tax2amount = dt.AsEnumerable().Sum(row => row.Field<decimal>("TAX2 AMT"));


            //==============================

            gvname.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Center;
            gvname.FooterRow.Cells[16].Text = "TOTAL";
            gvname.FooterRow.Cells[16].Font.Bold = true;

            gvname.FooterRow.Cells[19].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[19].Text = TotalQty.ToString("N2");
            gvname.FooterRow.Cells[19].Font.Bold = true;

            gvname.FooterRow.Cells[20].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[20].Text = ltrs.ToString("N2");
            gvname.FooterRow.Cells[20].Font.Bold = true;

            gvname.FooterRow.Cells[21].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[21].Text = BASICVALUE.ToString("N2");
            gvname.FooterRow.Cells[21].Font.Bold = true;

            gvname.FooterRow.Cells[22].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[22].Text = DiscAmount.ToString("N2");
            gvname.FooterRow.Cells[22].Font.Bold = true;

            gvname.FooterRow.Cells[23].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[23].Text = SchemeDiscAmt.ToString("N2");
            gvname.FooterRow.Cells[23].Font.Bold = true;

            gvname.FooterRow.Cells[24].HorizontalAlign = HorizontalAlign.Center;
            gvname.FooterRow.Cells[24].Text = AddSchemeDiscAmt.ToString("N2");
            gvname.FooterRow.Cells[24].Font.Bold = true;

            gvname.FooterRow.Cells[25].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[25].Text = Disc2Amt.ToString("N2");
            gvname.FooterRow.Cells[25].Font.Bold = true;


            gvname.FooterRow.Cells[26].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[26].Text = TdAmt.ToString("N2");
            gvname.FooterRow.Cells[26].Font.Bold = true;

            gvname.FooterRow.Cells[27].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[27].Text = PeAmt.ToString("N2");
            gvname.FooterRow.Cells[27].Font.Bold = true;

            gvname.FooterRow.Cells[28].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[28].Text = TaxableAmt.ToString("N2");
            gvname.FooterRow.Cells[28].Font.Bold = true;

            gvname.FooterRow.Cells[30].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[30].Text = Tax1Amt.ToString("N2");
            gvname.FooterRow.Cells[30].Font.Bold = true;

            gvname.FooterRow.Cells[32].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[32].Text = Tax2Amt.ToString("N2");
            gvname.FooterRow.Cells[32].Font.Bold = true;

            gvname.FooterRow.Cells[33].HorizontalAlign = HorizontalAlign.Left;
            gvname.FooterRow.Cells[33].Text = invvalue.ToString("N2");
            gvname.FooterRow.Cells[33].Font.Bold = true;
        }

        private bool ValidateSearch()
        {
            bool value = false;
            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
            }
            if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            return value;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            //if (gridSaleRegister.Rows.Count > 0)
            //{
            //    //ExportToExcel();
                ExportToExcelNew();
            //}
            //else
            //{
            //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
            //}
        }

        private void ExportToExcelNew()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;

                if (rdoDetailedView.Checked == true)
                {
                    GridView gvvc = new GridView();
                    string buunit;
                    if (DDLBusinessUnit.SelectedIndex >= 1)
                    {
                        buunit = (DDLBusinessUnit.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        buunit = ("");
                    }

                    string query = "EXEC ACX_USP_SaleRegister '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "','" + FromDate + "','" + ToDate + "','" + buunit + "'";
                    //string query = "EXEC USP_GETCUSTOMERWISESALEREPORT '" + FromDate + "','" + ToDate + "','" + statesel.ToString() + "'";

                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = obj.GetConnection();
                            cmd.CommandTimeout = 3600;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    wb.Worksheets.Add(dt, "SaleRegisterDetailed");

                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=SaleRegister_Detailed.xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }
                if (rdoSummarisedView.Checked == true)
                {
                    string query = "ACX_USP_SaleRegister_Summary";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    DataTable dt = new DataTable();

                    ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
                    //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@EndDate"); item.Add(txtToDate.Text);
                    ilist.Add("@BUCODE");
                    if (DDLBusinessUnit.SelectedIndex >= 1)
                    {
                        item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        item.Add("");
                    }

                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                    GridView gvexport = new GridView();
                    gvexport.ShowFooter = true;
                    gvexport.DataSource = dt;
                    gvexport.DataBind();
                    GridViewFooterCalculateNew(dt, gvexport);
                    //--------------------------------------------------------------------------//

                    string bottomquery = "ACX_USP_SaleRegister_Summary2";
                    List<string> bottomilist = new List<string>();
                    List<string> bottomitem = new List<string>();
                    DataTable bottomdt = new DataTable();

                    bottomilist.Add("@Site_Code"); bottomitem.Add(Session["SiteCode"].ToString());
                    //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    bottomilist.Add("@StartDate"); bottomitem.Add(txtFromDate.Text);
                    bottomilist.Add("@EndDate"); bottomitem.Add(txtToDate.Text);
                    bottomilist.Add("@BUCODE");
                    if (DDLBusinessUnit.SelectedIndex >= 1)
                    {
                        bottomitem.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        bottomitem.Add("");
                    }

                    bottomdt = obj.GetData_New(bottomquery, CommandType.StoredProcedure, bottomilist, bottomitem);
                    GridView gvexportbottom = new GridView();
                    gvexportbottom.ShowFooter = true;
                    gvexportbottom.DataSource = bottomdt;
                    gvexportbottom.DataBind();
                    GridViewFooterCalculatebottomNew(bottomdt, gvexportbottom);



                    //if (gridSaleRegister.Rows.Count > 0 || gridSaleRegisterbottom.Rows.Count > 0)
                    if (gvexport.Rows.Count > 0 || gvexportbottom.Rows.Count > 0)
                    {
                        string sFileName = "SaleRegister_Summarised.xls";

                        sFileName = sFileName.Replace("/", "");
                        // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                        Response.ContentType = "application/vnd.ms-excel";
                        EnableViewState = false;

                        System.IO.StringWriter objSW = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                        System.IO.StringWriter objSWT = new System.IO.StringWriter();
                        System.Web.UI.HtmlTextWriter objHTWT = new System.Web.UI.HtmlTextWriter(objSWT);

                        foreach (GridViewRow row in gvexport.Rows)
                        {
                            row.Cells[0].CssClass = "textmode";
                            row.Cells[1].CssClass = "textmode"; row.Cells[2].CssClass = "textmode"; row.Cells[3].CssClass = "textmode"; row.Cells[4].CssClass = "textmode";
                            row.Cells[5].CssClass = "textmode"; row.Cells[6].CssClass = "textmode"; row.Cells[7].CssClass = "textmode"; row.Cells[8].CssClass = "textmode";
                            row.Cells[9].CssClass = "textmode"; row.Cells[10].CssClass = "textmode"; row.Cells[11].CssClass = "textmode"; row.Cells[12].CssClass = "textmode";
                            row.Cells[13].CssClass = "textmode"; row.Cells[14].CssClass = "textmode"; row.Cells[15].CssClass = "textmode"; row.Cells[16].CssClass = "textmode";
                            row.Cells[17].CssClass = "textmode"; row.Cells[18].CssClass = "textmode";
                            //row.BackColor = Color.White;
                            //foreach (TableCell cell in row.Cells)
                            //{
                            //    cell.CssClass = "textmode";
                            //}
                        }
                        //foreach (GridViewRow row in gvexportbottom.Rows)
                        //{
                        //    //row.BackColor = Color.White;
                        //    foreach (TableCell cell in row.Cells)
                        //    {
                        //        cell.CssClass = "textmode";
                        //    }
                        //}
                        //dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
                        //dg.RenderControl(objHTW);
                        string name = "Sale Register Summarised Report";

                        //string DistributoName = ddlSiteId.SelectedItem.Value + " - " + ddlSiteId.SelectedItem.Text;
                        Response.Write("<table><tr><td colspan='5' style='width:100px;align-items:center; font:18px;'> <b> " + name + "</b> </td></tr> <tr><td><b>From Date:  " + txtFromDate.Text.Replace(",", "") + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text.Replace(",", "") + "</b></td></tr></table>");
                        Response.Write("<br/>");
                        // STYLE THE SHEET AND WRITE DATA TO IT.
                        Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                            "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                        //gridSaleRegister.RenderControl(objHTW);
                        gvexport.RenderControl(objHTW);
                        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                        Response.Write(style);
                        Response.Write(objSW.ToString());
                        Response.Write("<br/>");
                        Response.Write("<br/>");
                        //gridSaleRegisterbottom.RenderControl(objHTWT);
                        gvexportbottom.RenderControl(objHTWT);
                        //string style1 = @"<style> .textmode { mso-number-format:\@; } </style>";
                        //Response.Write(style1);
                        Response.Write(objSWT.ToString());
                        // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
                        //Response.Write("<table><tr><td></td><td></td><td></td><td><b>Total: </b></td><td></td><td><b>" + units + "</b></td><td><b>" + ltrs + "</b></td><td><b>" + BASICVALUE + "</b></td><td><b>" + TRDDISCVALUE + "</b></td><td><b>" + Priceequalvalue + "</b></td><td><b>" + secdiscamount + "</b></td><td><b>" + schsplvalue + "</b></td><td><b>" + taxablevalue + "</b></td><td></td><td><b>" + Tax1amount + "</b></td><td></td><td><b>" + Tax2amount + "</b></td><td><b>" + invvalue + "</b></td></tr></table>");

                        Response.End();
                        //dg = null;
                    }
                    else
                    {
                        LblMessage.Text = "No Data Exits Between This Date Range  !";
                        LblMessage.Visible = true;
                    }

                }
            }

            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //private void ExportToExcelNew()
        //{
        //    try
        //    {
        //        //bool b = ValidateInput();
        //        //if (b == true)
        //        //{
        //        GridView gvvc = new GridView();
        //        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();


        //        //string siteid = "";
        //        string FromDate = txtFromDate.Text;
        //        string ToDate = txtToDate.Text;
        //        string buunit;
        //        if (DDLBusinessUnit.SelectedIndex >= 1)
        //        {
        //            buunit = (DDLBusinessUnit.SelectedItem.Value.ToString());
        //        }
        //        else
        //        {
        //            buunit = ("");
        //        }

        //        string query = "EXEC ACX_USP_SaleRegister '" + Session["SiteCode"].ToString() + "','" + Session["DATAAREAID"].ToString() + "','" + FromDate + "','" + ToDate + "','" + buunit + "'";
        //        //string query = "EXEC USP_GETCUSTOMERWISESALEREPORT '" + FromDate + "','" + ToDate + "','" + statesel.ToString() + "'";

        //        using (SqlCommand cmd = new SqlCommand(query))
        //        {
        //            using (SqlDataAdapter sda = new SqlDataAdapter())
        //            {
        //                cmd.Connection = obj.GetConnection();
        //                cmd.CommandTimeout = 500;
        //                sda.SelectCommand = cmd;
        //                using (DataTable dt = new DataTable())
        //                {
        //                    sda.Fill(dt);
        //                    using (XLWorkbook wb = new XLWorkbook())
        //                    {
        //                        wb.Worksheets.Add(dt, "SaleRegisterDetailed");

        //                        Response.Clear();
        //                        Response.Buffer = true;
        //                        Response.Charset = "";
        //                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //                        Response.AddHeader("content-disposition", "attachment;filename=SaleRegister_Detailed.xlsx");
        //                        using (MemoryStream MyMemoryStream = new MemoryStream())
        //                        {
        //                            wb.SaveAs(MyMemoryStream);
        //                            MyMemoryStream.WriteTo(Response.OutputStream);
        //                            Response.Flush();
        //                            Response.End();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {                
        //        LblMessage.Text = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //        {
        //            if (conn.State == ConnectionState.Open)
        //            {
        //                conn.Close();
        //                conn.Dispose();
        //            }
        //        }
        //    }
        //}

        //private void ExportToExcelNew()
        //{
        //    //old
        //    //Response.Clear();
        //    //Response.Buffer = true;
        //    //Response.ClearContent();
        //    //Response.ClearHeaders();
        //    //Response.Charset = "";
        //    //string FileName = "SaleRegister" + DateTime.Now + ".xls";
        //    //StringWriter strwritter = new StringWriter();
        //    //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    //Response.ContentType = "application/vnd.ms-excel";
        //    //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        //    //gridSaleRegister.GridLines = GridLines.Both;
        //    //gridSaleRegister.HeaderStyle.Font.Bold = true;
        //    //gridSaleRegister.RenderControl(htmltextwrtter);
        //    //{
        //    //    Response.Write("<table><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
        //    //} 
        //    //Response.Write(strwritter.ToString());
        //    //Response.End();

        //    try
        //    {
        //        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

        //        string FromDate = txtFromDate.Text;
        //        string ToDate = txtToDate.Text;

        //        if (rdoDetailedView.Checked == true)
        //        {

        //            string query = "ACX_USP_SaleRegister";
        //            List<string> ilist = new List<string>();
        //            List<string> item = new List<string>();
        //            DataTable dt = new DataTable();


        //            ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
        //            ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
        //            ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
        //            ilist.Add("@EndDate"); item.Add(txtToDate.Text);
        //            ilist.Add("@BUCODE");
        //            if (DDLBusinessUnit.SelectedIndex >= 1)
        //            {
        //                item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
        //            }
        //            else
        //            {
        //                item.Add("");
        //            }

        //            dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

        //            GridView gvexport = new GridView();
        //           // gvexport.ShowFooter = true;
        //            gvexport.DataSource = dt;
        //            gvexport.DataBind();
        //            //GridViewFooterCalculateNew(dt,gvexport);

        //            //if (gridSaleRegister.Rows.Count > 0)
        //            if(gvexport.Rows.Count>0)
        //            {

        //                string sFileName = "SaleRegister_Detailed.xls";

        //                sFileName = sFileName.Replace("/", "");
        //                // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
        //                Response.ClearContent();
        //                Response.Buffer = true;
        //                Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
        //                Response.ContentType = "application/vnd.ms-excel";
        //                EnableViewState = false;

        //                System.IO.StringWriter objSW = new System.IO.StringWriter();
        //                System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

        //                foreach (GridViewRow row in gvexport.Rows)
        //                {
        //                    row.Cells[0].CssClass = "textmode";
        //                    row.Cells[1].CssClass = "textmode"; row.Cells[2].CssClass = "textmode"; row.Cells[3].CssClass = "textmode"; row.Cells[4].CssClass = "textmode";
        //                    row.Cells[5].CssClass = "textmode"; row.Cells[6].CssClass = "textmode"; row.Cells[7].CssClass = "textmode"; row.Cells[8].CssClass = "textmode";
        //                    row.Cells[9].CssClass = "textmode"; row.Cells[10].CssClass = "textmode"; row.Cells[11].CssClass = "textmode"; row.Cells[12].CssClass = "textmode";
        //                    row.Cells[13].CssClass = "textmode"; row.Cells[14].CssClass = "textmode"; row.Cells[15].CssClass = "textmode"; row.Cells[16].CssClass = "textmode";
        //                    row.Cells[17].CssClass = "textmode"; row.Cells[18].CssClass = "textmode"; row.Cells[19].CssClass = "textmode"; row.Cells[20].CssClass = "textmode";
        //                    row.Cells[21].CssClass = "textmode"; row.Cells[22].CssClass = "textmode"; row.Cells[23].CssClass = "textmode"; row.Cells[24].CssClass = "textmode";
        //                    row.Cells[25].CssClass = "textmode"; row.Cells[26].CssClass = "textmode"; row.Cells[27].CssClass = "textmode"; row.Cells[28].CssClass = "textmode";
        //                    row.Cells[29].CssClass = "textmode"; row.Cells[30].CssClass = "textmode"; row.Cells[31].CssClass = "textmode"; row.Cells[32].CssClass = "textmode";
        //                    row.Cells[33].CssClass = "textmode"; row.Cells[34].CssClass = "textmode"; row.Cells[35].CssClass = "textmode"; row.Cells[36].CssClass = "textmode";
        //                    row.Cells[37].CssClass = "textmode"; row.Cells[38].CssClass = "textmode"; row.Cells[39].CssClass = "textmode"; row.Cells[40].CssClass = "textmode";
        //                    //row.BackColor = Color.White;
        //                    //foreach (TableCell cell in row.Cells)
        //                    //{
        //                    //    cell.CssClass = "textmode";
        //                    //}
        //                }
        //                //System.IO.StringWriter objSWT = new System.IO.StringWriter();
        //                //System.Web.UI.HtmlTextWriter objHTWT = new System.Web.UI.HtmlTextWriter(objSWT);


        //                //dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
        //                //dg.RenderControl(objHTW);
        //                string name = "Sale Register Detailed Report";

        //                //string DistributoName = ddlSiteId.SelectedItem.Value + " - " + ddlSiteId.SelectedItem.Text;
        //                Response.Write("<table><tr><td colspan='5' style='width:100px;align-items:center; font:18px;'> <b> " + name + "</b> </td></tr> <tr><td><b>From Date:  " + txtFromDate.Text.Replace(",", "") + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text.Replace(",", "") + "</b></td></tr></table>");
        //                Response.Write("<br/>");
        //                // STYLE THE SHEET AND WRITE DATA TO IT.
        //                Response.Write("<style> TABLE { border:dotted 1px #999; } " +
        //                    "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
        //                //gridSaleRegister.RenderControl(objHTW);
        //                gvexport.RenderControl(objHTW);
        //                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //                Response.Write(style);
        //                Response.Write(objSW.ToString());
        //                // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
        //                //Response.Write("<table><tr><td></td><td></td><td></td><td><b>Total: </b></td><td></td><td><b>" + units + "</b></td><td><b>" + ltrs + "</b></td><td><b>" + BASICVALUE + "</b></td><td><b>" + TRDDISCVALUE + "</b></td><td><b>" + Priceequalvalue + "</b></td><td><b>" + secdiscamount + "</b></td><td><b>" + schsplvalue + "</b></td><td><b>" + taxablevalue + "</b></td><td></td><td><b>" + Tax1amount + "</b></td><td></td><td><b>" + Tax2amount + "</b></td><td><b>" + invvalue + "</b></td></tr></table>");

        //                Response.End();
        //                //dg = null;
        //            }
        //            else
        //            {
        //                LblMessage.Text = "No Data Exits Between This Date Range  !";
        //                LblMessage.Visible = true;
        //            }
        //        }
        //        if (rdoSummarisedView.Checked == true)
        //        {
        //            string query = "ACX_USP_SaleRegister_Summary";
        //            List<string> ilist = new List<string>();
        //            List<string> item = new List<string>();
        //            DataTable dt = new DataTable();

        //            ilist.Add("@Site_Code"); item.Add(Session["SiteCode"].ToString());
        //            //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
        //            ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
        //            ilist.Add("@EndDate"); item.Add(txtToDate.Text);
        //            ilist.Add("@BUCODE");
        //            if (DDLBusinessUnit.SelectedIndex >= 1)
        //            {
        //                item.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
        //            }
        //            else
        //            {
        //                item.Add("");
        //            }

        //            dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
        //            GridView gvexport = new GridView();
        //            gvexport.ShowFooter = true;
        //            gvexport.DataSource = dt;
        //            gvexport.DataBind();
        //            GridViewFooterCalculateNew(dt, gvexport);
        //            //--------------------------------------------------------------------------//

        //            string bottomquery = "ACX_USP_SaleRegister_Summary2";
        //            List<string> bottomilist = new List<string>();
        //            List<string> bottomitem = new List<string>();
        //            DataTable bottomdt = new DataTable();

        //            bottomilist.Add("@Site_Code"); bottomitem.Add(Session["SiteCode"].ToString());
        //            //ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
        //            bottomilist.Add("@StartDate"); bottomitem.Add(txtFromDate.Text);
        //            bottomilist.Add("@EndDate"); bottomitem.Add(txtToDate.Text);
        //            bottomilist.Add("@BUCODE");
        //            if (DDLBusinessUnit.SelectedIndex >= 1)
        //            {
        //                bottomitem.Add(DDLBusinessUnit.SelectedItem.Value.ToString());
        //            }
        //            else
        //            {
        //                bottomitem.Add("");
        //            }

        //            bottomdt = obj.GetData_New(bottomquery, CommandType.StoredProcedure, bottomilist, bottomitem);
        //            GridView gvexportbottom = new GridView();
        //            gvexportbottom.ShowFooter = true;
        //            gvexportbottom.DataSource = bottomdt;
        //            gvexportbottom.DataBind();
        //            GridViewFooterCalculatebottomNew(bottomdt, gvexportbottom);



        //            //if (gridSaleRegister.Rows.Count > 0 || gridSaleRegisterbottom.Rows.Count > 0)
        //            if(gvexport.Rows.Count>0 || gvexportbottom.Rows.Count>0)
        //            {
        //                string sFileName = "SaleRegister_Summarised.xls";

        //                sFileName = sFileName.Replace("/", "");
        //                // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
        //                Response.ClearContent();
        //                Response.Buffer = true;
        //                Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
        //                Response.ContentType = "application/vnd.ms-excel";
        //                EnableViewState = false;

        //                System.IO.StringWriter objSW = new System.IO.StringWriter();
        //                System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

        //                System.IO.StringWriter objSWT = new System.IO.StringWriter();
        //                System.Web.UI.HtmlTextWriter objHTWT = new System.Web.UI.HtmlTextWriter(objSWT);

        //                foreach (GridViewRow row in gvexport.Rows)
        //                {
        //                    row.Cells[0].CssClass = "textmode";
        //                    row.Cells[1].CssClass = "textmode"; row.Cells[2].CssClass = "textmode"; row.Cells[3].CssClass = "textmode"; row.Cells[4].CssClass = "textmode";
        //                    row.Cells[5].CssClass = "textmode"; row.Cells[6].CssClass = "textmode"; row.Cells[7].CssClass = "textmode"; row.Cells[8].CssClass = "textmode";
        //                    row.Cells[9].CssClass = "textmode"; row.Cells[10].CssClass = "textmode"; row.Cells[11].CssClass = "textmode"; row.Cells[12].CssClass = "textmode";
        //                    row.Cells[13].CssClass = "textmode"; row.Cells[14].CssClass = "textmode"; row.Cells[15].CssClass = "textmode"; row.Cells[16].CssClass = "textmode";
        //                    row.Cells[17].CssClass = "textmode"; row.Cells[18].CssClass = "textmode";
        //                    //row.BackColor = Color.White;
        //                    //foreach (TableCell cell in row.Cells)
        //                    //{
        //                    //    cell.CssClass = "textmode";
        //                    //}
        //                }
        //                //foreach (GridViewRow row in gvexportbottom.Rows)
        //                //{
        //                //    //row.BackColor = Color.White;
        //                //    foreach (TableCell cell in row.Cells)
        //                //    {
        //                //        cell.CssClass = "textmode";
        //                //    }
        //                //}
        //                //dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
        //                //dg.RenderControl(objHTW);
        //                string name = "Sale Register Summarised Report";

        //                //string DistributoName = ddlSiteId.SelectedItem.Value + " - " + ddlSiteId.SelectedItem.Text;
        //                Response.Write("<table><tr><td colspan='5' style='width:100px;align-items:center; font:18px;'> <b> " + name + "</b> </td></tr> <tr><td><b>From Date:  " + txtFromDate.Text.Replace(",", "") + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text.Replace(",", "") + "</b></td></tr></table>");
        //                Response.Write("<br/>");
        //                // STYLE THE SHEET AND WRITE DATA TO IT.
        //                Response.Write("<style> TABLE { border:dotted 1px #999; } " +
        //                    "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
        //                //gridSaleRegister.RenderControl(objHTW);
        //                gvexport.RenderControl(objHTW);
        //                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //                Response.Write(style);
        //                Response.Write(objSW.ToString());
        //                Response.Write("<br/>");
        //                Response.Write("<br/>");
        //                //gridSaleRegisterbottom.RenderControl(objHTWT);
        //                gvexportbottom.RenderControl(objHTWT);
        //                //string style1 = @"<style> .textmode { mso-number-format:\@; } </style>";
        //                //Response.Write(style1);
        //                Response.Write(objSWT.ToString());
        //                // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
        //                //Response.Write("<table><tr><td></td><td></td><td></td><td><b>Total: </b></td><td></td><td><b>" + units + "</b></td><td><b>" + ltrs + "</b></td><td><b>" + BASICVALUE + "</b></td><td><b>" + TRDDISCVALUE + "</b></td><td><b>" + Priceequalvalue + "</b></td><td><b>" + secdiscamount + "</b></td><td><b>" + schsplvalue + "</b></td><td><b>" + taxablevalue + "</b></td><td></td><td><b>" + Tax1amount + "</b></td><td></td><td><b>" + Tax2amount + "</b></td><td><b>" + invvalue + "</b></td></tr></table>");

        //                Response.End();
        //                //dg = null;
        //            }
        //            else
        //            {
        //                LblMessage.Text = "No Data Exits Between This Date Range  !";
        //                LblMessage.Visible = true;
        //            }

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message.ToString();
        //    }
        //}

        private void ExportToPDF()
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
             "attachment;filename=GridViewExport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            gridSaleRegister.AllowPaging = false;
            gridSaleRegister.DataBind();
            gridSaleRegister.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        protected void rdoDetailedView_CheckedChanged(object sender, EventArgs e)
        {
            gridSaleRegister.DataSource = null;
            gridSaleRegister.DataBind();
            gridSaleRegister.Visible = false;
            LblMessage.Text = string.Empty;
            gridSaleRegisterbottom.DataSource = null;
            gridSaleRegisterbottom.DataBind();
            gridSaleRegisterbottom.Visible = false;
        }
    }
}