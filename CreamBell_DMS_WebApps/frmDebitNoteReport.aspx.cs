using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Data;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmDebitNoteReport : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();
        public static string ParameterName = string.Empty;
        System.Collections.Generic.List<byte[]> bytelist = new List<byte[]>();
        HashSet<string> h1 = null;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CreamBell_DMS_WebApps.App_Code.Global baseobj = new CreamBell_DMS_WebApps.App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
        DataTable dtHeader = null;
        DataTable dtLinetest = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                GridDetail();
                //FillPSR();
                CalendarExtender1.StartDate = Convert.ToDateTime("01-July-2017");
                txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                //ShowReport("SaleInvoice", "0000000004");
            }
            LblMessage.Text = "";
        }
        private void GridDetail()
        {

            string sitecode1;
            try
            {
                sitecode1 = Session["SiteCode"].ToString();
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();
                string str = "Select A.PURCH_RETURNNO as INVOICE_NO , A.PURCH_RETURNDATE as INVOIC_DATE, A.Purch_recieptno as SO_NO,case when CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103)= '01/01/1900' then '' else CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103) end  AS SO_DATE, ('[' +A.SITE_CODE +']' + ' ' + B.NAME) as CUSTOMER , A.Return_docvalue as INVOICE_VALUE" +
                                               " from ax.[ACXPURCHRETURNHEADER]  A  INNER JOIN ax.inventsite B   ON A.SITE_CODE=B.SITEID" +
                                               " where  A.SITE_CODE= '" + sitecode1 + "' and PURCH_RETURNDATE>=DateAdd(Day,-1,getdate()) and PURCH_RETURNDATE<=getdate() order by A.PURCH_RETURNDATE desc,A.PURCH_RETURNNO desc";
                adp1 = new SqlDataAdapter("Select A.PURCH_RETURNNO as INVOICE_NO , A.PURCH_RETURNDATE as INVOIC_DATE, A.Purch_recieptno as SO_NO,case when CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103)= '01/01/1900' then '' else CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103) end  AS SO_DATE, ('[' +A.SITE_CODE +']' + ' ' + B.NAME) as CUSTOMER , A.Return_docvalue as INVOICE_VALUE" +
                                               " from ax.[ACXPURCHRETURNHEADER]  A  INNER JOIN ax.inventsite B   ON A.SITE_CODE=B.SITEID" +
                                               " where  A.SITE_CODE= '" + sitecode1 + "' and PURCH_RETURNDATE>=DateAdd(Day,-1,getdate()) and PURCH_RETURNDATE<=getdate() order by A.PURCH_RETURNDATE desc,A.PURCH_RETURNNO desc", conn);

                ds2.Clear();
                adp1.Fill(ds2, "dtl");

                if (ds2.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                    {
                        GridView1.DataSource = ds2.Tables["dtl"];
                        GridView1.DataBind();
                    }
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
     
        protected void drpPSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillBeat();
            //  uppanel.Update();
        }

        protected void drpBeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillCustomer();
        }
        
        protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Enabled = false;
            if (ddlSearchNew.SelectedIndex >= 0)
            { txtSearch.Enabled = true; txtSearch.Focus(); txtSearch.Attributes.Remove("disabled"); }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();

                if (txtFromDate.Text.Trim().Length > 0)
                {
                    if (!baseObj.IsDate(txtFromDate.Text))
                    {
                        string message = "alert('Invalid From Date !!!');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        txtFromDate.Focus();
                        return;
                    }
                }
                if (txtToDate.Text.Trim().Length > 0)
                {
                    if (!baseObj.IsDate(txtToDate.Text))
                    {
                        string message = "alert('Invalid To Date !!!');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                        txtToDate.Focus();
                        return;
                    }
                }

                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                bool b = ValidateSearch();
                GridView1.DataSource = null;
                GridView2.DataSource = null;
                GridView2.DataBind();
                if (b == true)
                {
                    DateTime endDate = Convert.ToDateTime(this.txtToDate.Text);
                    //endDate.AddDays(1);
                    //DateTime end = endDate.AddDays(1);

                    string FromDate = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy");
                    string ToDate = endDate.ToString("dd-MMM-yyyy");

                    //string query = "Select A.PURCH_RETURNNO as INVOICE_NO , A.PURCH_RETURNDATE as INVOIC_DATE, A.Purch_recieptno as SO_NO,case when CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103)= '01/01/1900' then '' else CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103) end  AS SO_DATE, ('[' + A.SITE_CODE +']' + ' ' + B.NAME) as CUSTOMER , A.Return_docvalue as INVOICE_VALUE" +
                    //               " from ax.[ACXPURCHRETURNHEADER]  A  INNER JOIN ax.inventsite B   ON A.SITE_CODE=B.SITEID" +
                    //                " where  A.SITE_CODE= '" + Session["SiteCode"].ToString() + "' and PURCH_RETURNDATE>='" + FromDate + "' and PURCH_RETURNDATE<='" + ToDate + "'";

                    string query = "Select A.PURCH_RETURNNO as INVOICE_NO , A.PURCH_RETURNDATE as INVOIC_DATE, A.Purch_recieptno as SO_NO,case when CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103)= '01/01/1900' then '' else CONVERT(VARCHAR(10),PURCH_RECIEPTDATE , 103) end  AS SO_DATE, ('[' + A.SITE_CODE +']' + ' ' + B.NAME) as CUSTOMER , A.Return_docvalue as INVOICE_VALUE" +
                                   " from ax.[ACXPURCHRETURNHEADER]  A  INNER JOIN ax.inventsite B   ON A.SITE_CODE=B.SITEID" +
                                    " where  A.SITE_CODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") and PURCH_RETURNDATE>='" + FromDate + "' and PURCH_RETURNDATE<='" + ToDate + "'";

                    //if (txtInvoiceNoStart.Text != string.Empty && txtInvoiceNoEnd.Text != string.Empty)
                    //{
                    //    try
                    //    {
                    //        string[] strInvoicefromsplit = txtInvoiceNoStart.Text.Split('-');
                    //        string[] strInvoiceTosplit = txtInvoiceNoEnd.Text.Split('-');
                    //        query = query + "and cast(Right(A.invoice_no,6) as int) between " + strInvoicefromsplit[1] + " and " + strInvoiceTosplit[1] + " ";
                    //    }
                    //    catch
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Invoice number is not in correct format !! ');", true);
                    //        GridView1.DataSource = null;
                    //        GridView1.DataBind();
                    //        return;
                    //    }
                    //
                    //
                    //}
                    //if (drpPSR.SelectedItem.Text != string.Empty && drpPSR.SelectedItem.Text != "Select...")
                    //{
                    //    query = query + "and B.psr_code='" + drpPSR.SelectedItem.Value + "' ";
                    //
                    //    if (drpBeat.SelectedItem.Text != string.Empty && drpBeat.SelectedItem.Text != "Select...")
                    //    {
                    //        query = query + "and B.psr_beat='" + drpBeat.SelectedItem.Value + "' ";
                    //
                    //        if (drpCustomer.SelectedItem.Text != string.Empty && drpCustomer.Text != "Select...")
                    //        {
                    //            query = query + "and A.CUSTOMER_CODE='" + drpCustomer.SelectedItem.Value + "' ";
                    //        }
                    //    }
                    //}
                    if (ddlSearchNew.Text == "Sales Invoice No" && txtSearch.Text != "")
                    {
                        query = query + "AND A.PURCH_RETURNNO like '%" + txtSearch.Text.Trim().ToString() + "%'";
                    }
                    //if (ddlSearch.Text == "Customer")
                    //{
                    //    query = query + " and (A.CUSTOMER_CODE like '%" + txtSearch.Text.Trim().ToString() + "%' or B.CUSTOMER_NAME like '%" + txtSearch.Text.Trim().ToString() + "%') ";
                    //}
                    query = query + "  ORDER BY A.PURCH_RETURNDATE DESC, A.PURCH_RETURNNO DESC";

                    DataTable dtFilter = obj.GetData(query);
                    if (dtFilter.Rows.Count > 0)
                    {

                        GridView1.DataSource = dtFilter;
                        GridView1.DataBind();

                        CheckBox chk = (CheckBox)GridView1.HeaderRow.FindControl("checkAll");
                        chk.Checked = true;


                        foreach (GridViewRow grv in GridView1.Rows)
                        {
                            CheckBox chkAll = (CheckBox)grv.Cells[0].FindControl("chklist");
                            chkAll.Checked = true;

                        }
                        // uppanel.Update();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('No Data Exist !! ');", true);
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                        //GridDetail();
                        txtSearch.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateSearch()
        {
            bool value = false;
            if (txtFromDate.Text == string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Provide The Date Range Parameter !";
                LblMessage.Visible = true;
                //uppanel.Update();
                //UpdatePanel1.Update();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The Date Range Parameter !');", true);
                //string message = "alert('Please Provide The Date Range Parameter !');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Provide The TO Date Range Parameter !";
                LblMessage.Visible = true;
                //uppanel.Update();
                //UpdatePanel1.Update();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The TO Date Range Parameter !');", true);
                //string message = "alert('Please Provide The TO Date Range Parameter !');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Provide The FROM Date Range Parameter !";
                LblMessage.Visible = true;
                //  uppanel.Update();
                //UpdatePanel1.Update();
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
                //string message = "alert('Please Provide The FROM Date Range Parameter !');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            //if (txtInvoiceNoStart.Text == string.Empty && txtInvoiceNoEnd.Text != string.Empty)
            //{
            //    value = false;
            //    LblMessage.Text = "Please Provide The From Invoice No Range Parameter !";
            //    LblMessage.Visible = true;
            //    //  uppanel.Update();
            //    //UpdatePanel1.Update();
            //    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The From Invoice No Range Parameter !');", true);
            //    //string message = " alert('Please Provide The From Invoice No Range Parameter  !');";
            //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            //
            //}
            //if (txtInvoiceNoStart.Text != string.Empty && txtInvoiceNoEnd.Text == string.Empty)
            //{
            //    value = false;
            //    LblMessage.Text = "Please Provide The To Invoice No Range Parameter !";
            //    LblMessage.Visible = true;
            //    //  uppanel.Update();
            //    //UpdatePanel1.Update();
            //    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The To Invoice No Range Parameter !');", true);
            //    //string message = " alert('Please Provide The To Invoice No Range Parameter !');";
            //    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            //
            //}
            return value;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                string multipleInvoice = string.Empty;

                foreach (GridViewRow grv in GridView1.Rows)
                {
                    CheckBox chklist1 = (CheckBox)grv.Cells[0].FindControl("chklist");
                    LinkButton lnkBtn = (LinkButton)grv.Cells[0].FindControl("lnkbtn");
                    if (chklist1.Checked)
                    {
                        // ShowReportSaleInvoice(string.Empty, chklist1.Text);
                        if (multipleInvoice == string.Empty)
                        {
                            multipleInvoice = "'" + lnkBtn.Text + "'";
                        }
                        else
                        {
                            multipleInvoice += ",'" + lnkBtn.Text + "'";
                        }
                    }
                }
                if (multipleInvoice == "")
                {
                    LblMessage.Text = "Please select Invoice No.. !";
                    //UpdatePanel1.Update();
                    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select Invoice No.. !');", true);
                    //string message = " alert('Please select Invoice No.. !');";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

                    return;
                }

                //==================
                //List<string> ilist = new List<string>();
                //List<string> litem = new List<string>();

                //string queryHeader = "Acx_SaleInvoiceHeaderReport";
                //string queryLine = "Acx_SaleInvoiceLineReport";
                string queryHeader = "USP_GETDEBITNOTEHEADER" + "'" + multipleInvoice + "','" + Session["SiteCode"].ToString() + "'";
                string queryLine = "USP_GETDEBITNOTELINE" + "'" + multipleInvoice + "','" + Session["SiteCode"].ToString() + "'";
                //string queryHeader = " Select SHD.INVOICE_NO, IST.ACXADDRESS1 +' '+ACXADDRESS2   AS SITEADDRESS, IST.AcxCity,IST.ACXzipcode,IST.STATECODE AS SITESTATE, IST.ACXVAT AS SITEVAT," +
                //                        " IST.ACXMOBILE AS SITEMOBILE,IST.ACXTELEPHONE AS SitePhoneNo, CONVERT(nvarchar(15), SHD.INVOIC_DATE,105) as INVOIC_DATE, SHD.SITEID, " +
                //                        " USM.User_Name, USM.State, SHD.CUSTGROUP_CODE,SHD.CUSTOMER_CODE, CUS.CUSTOMER_NAME, CUS.ADDRESS1,CUS.ADDRESS2,CUS.CITY, " +
                //                        " CUS.AREA,CUS.DISTRICT, CUS.STATE, CUS.MOBILE_NO, CUS.PHONE_NO,CUS.VAT,SHD.SO_NO, CONVERT(nvarchar(15),SHD.SO_DATE,105) as SO_DATE ," +
                //                        " SHD.LOADSHEET_NO, CONVERT(nvarchar(15),SHD.LOADSHEET_DATE,105) as LOADSHEET_DATE ,SHD.TRANSPORTER_CODE, SHD.VEHICAL_NO, " +
                //                        " SHD.DRIVER_CODE, SHD.DRIVER_MOBILENO, SHD.INVOICE_VALUE, SHD.LOADSHEETQTY, " +
                //                        " CASE WHEN  CUS.VAT='' THEN 'RETAIL INVOICE' ELSE 'TAX INVOICE' END AS REPORTTYPE , Remark" +
                //                        " from ax.ACXSALEINVOICEHEADER SHD " +
                //                        " INNER JOIN ax.ACXCUSTMASTER CUS ON SHD.CUSTOMER_CODE=CUS.CUSTOMER_CODE " +
                //                        " INNER JOIN ax.ACXUSERMASTER USM ON SHD.SITEID= USM.SITE_CODE " +
                //                        " INNER JOIN AX.INVENTSITE IST ON IST.SITEID = SHD.SITEID " +
                //                         "where INVOICE_NO in (" + multipleInvoice + ") and SHD.SITEID='" + Session["SiteCode"].ToString() + "' order by SHD.INVOICE_NO  ";

                //            string queryLine = @"Select ROW_NUMBER() over (ORDER BY SINVL.Amount Desc,PROD.PRODUCT_SUBCATEGORY, PRODUCT_NAME) AS SRNO,case when amount<>0 then 'A' else 'B' end as SortFilter
                //                                        ,PROD.PRODUCT_SUBCATEGORY ,SINVL.Invoice_No, SINVL.PRODUCT_CODE,
                //                                     PRODUCT_NAME as PRODUCT_NAME,
                //                                    CAST(PROD.PRODUCT_PACKSIZE as decimal(9,2)) as PRODUCT_PACKSIZE , 
                //                                    CAST(SINVL.BOXPCS as decimal(9,2)) as BOX,CAST(SINVL.BOX as decimal(9,2)) as BoxConv,CAST(SINVL.MRP as decimal(9,2)) as MRP, 
                //                                    CAST((SINVL.BOX * SINVL.MRP) as decimal(9,2)) as MRPValue, 
                //                                    CAST(SINVL.LTR as decimal(9,2)) as LTR, 
                //                                    CAST(SINVL.TAX_CODE as decimal(9,2)) as TAX_CODE,  CAST(SINVL.CRATES as decimal(9,2)) as CRATES, 
                //                                    CAST(SINVL.AMOUNT as decimal(9,2)) as AMOUNT,  CAST(SINVL.TAX_AMOUNT as decimal(9,2)) as TAX_AMOUNT, 
                //                                    CAST(SINVL.LINEAMOUNT as decimal(9,2)) as LINEAMOUNT,  CAST(SINVL.ADDTAX_AMOUNT as decimal(9,2)) as ADDTAX_AMOUNT, 
                //                                    CAST(SINVL.ADDTAX_CODE as decimal(9,2)) as ADDTAX_CODE, CAST(SINVL.DISC_AMOUNT as decimal(9,2)) as DISC_AMOUNT
                //                                    ,SINVL.SEC_DISC_AMOUNT,isnull(SINVL.PEVAlue,0) as PEValue,isnull(TDValue,0) as TDValue ,                                    
                //                                    CAST(SINVL.AMOUNT as Decimal(9,2)) -
                //									CAST(SINVL.TAX_AMOUNT as Decimal(9,2)) -  
                //									CAST(SINVL.ADDTAX_AMOUNT as decimal(9,2)) + isnull(TDValue,0) - 
                //									Isnull(SINVL.PEVAlue,0) + CAST(SINVL.DISC_AMOUNT as decimal(9,2)) + SINVL.SEC_DISC_AMOUNT  as basic,
                //                                    Case when SINVL.BOX = 0 then 0.00 else (
                //									Cast((CAST(SINVL.AMOUNT as Decimal(9,2)) -
                //									CAST(SINVL.TAX_AMOUNT as Decimal(9,2)) -  
                //									CAST(SINVL.ADDTAX_AMOUNT as decimal(9,2)) + isnull(TDValue,0) - 
                //									Isnull(SINVL.PEVAlue,0) + CAST(SINVL.DISC_AMOUNT as decimal(9,2)) + SINVL.SEC_DISC_AMOUNT) / CAST(SINVL.BOX as decimal(9,2)) as decimal(9,2)) 
                //                                    ) End as RATE,
                //
                //							        CAST(SINVL.AMOUNT as Decimal(9,2)) - CAST(SINVL.TAX_AMOUNT as Decimal(9,2)) - CAST(SINVL.ADDTAX_AMOUNT as decimal(9,2)) as TaxableAmt 
                //                                    from ax.ACXSALEINVOICELINE SINVL 
                //                                    INNER JOIN  ax.inventtable PROD ON SINVL.PRODUCT_CODE=PROD.ITEMID " +
                //                                    " where INVOICE_NO in (" + multipleInvoice + ") and SINVL.SITEID='" + Session["SiteCode"].ToString() + "' Order By  INVOICE_NO ,SortFilter,Product_SubCategory ";

                DataTable dtMainHeader1 = new DataTable();
                dtMainHeader1 = obj.GetData(queryHeader);
                DataTable dtLine1 = new DataTable();
                dtLine1 = obj.GetData(queryLine);

                string[] strSavePDFID = new string[dtMainHeader1.Rows.Count + 1];


                //for merging a pdf into single one,I want extra pdf and for this i have called SaveInvoicePDf funtion
                //for single invoice PDF i have created 2 pdf and murge into one
                SaveInvoicePDf(dtMainHeader1, dtLine1, 0, 0, strSavePDFID);
                //

                for (int i = 0; i < dtMainHeader1.Rows.Count; i++)
                {
                    SaveInvoicePDf(dtMainHeader1, dtLine1, i, i + 1, strSavePDFID);
                }

                if (strSavePDFID.Length > 0)
                {
                    string des = Server.MapPath("Downloads\\" + strSavePDFID[0] + ".pdf");
                    string[] source = new string[strSavePDFID.Length - 1];
                    for (int i = 0; i < strSavePDFID.Length - 1; i++)
                    {
                        source[i] = Server.MapPath("Downloads\\" + strSavePDFID[i + 1] + ".pdf");
                    }

                    MergeFiles(des, source);

                    string FilePath = Server.MapPath("Downloads\\" + strSavePDFID[0] + ".pdf");
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(FilePath);
                    //Delete the files from system..........
                    if (strSavePDFID.Length > 0)
                    {
                        for (int i = 0; i < strSavePDFID.Length; i++)
                        {
                            string path = Server.MapPath("Downloads\\" + strSavePDFID[i] + ".pdf");
                            FileInfo file = new FileInfo(path);
                            if (file.Exists)//check file exsit or not
                            {
                                file.Delete();
                            }
                        }
                    }

                    if (FileBuffer != null)
                    {
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + strSavePDFID[0] + ".pdf");
                        Response.BinaryWrite(FileBuffer);
                        Response.Flush();
                        Response.End();
                    }
                }

                // murgebytges();     
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void SaveInvoicePDf(DataTable dtMainHeader1, DataTable dtLine1, int i, int j, string[] strSavePDFID)
        {
            try
            {
                var Hrows = (from row in dtMainHeader1.AsEnumerable()
                             where row.Field<string>("INVOICE_NO").Trim() == dtMainHeader1.Rows[i]["INVOICE_NO"].ToString()
                             select row);
                var Header = Hrows.CopyToDataTable();
                dtHeader = Header;

                var rows = (from row in dtLine1.AsEnumerable()
                            where row.Field<string>("INVOICE_NO").Trim() == dtMainHeader1.Rows[i]["INVOICE_NO"].ToString()
                            orderby row.Field<string>("SortFilter").Trim(), row.Field<string>("Product_SubCategory").Trim()
                            select row);
                var lines = rows.CopyToDataTable();
                dtLinetest = lines;

                //DataRow[] rows = dtLinetest.Select("Invoice_No = + dtHeader.Rows[i]["INVOICE_NO"].ToString()");
                string strId = DateTime.Now.ToString("yyyyMMddHHmmss");
                strSavePDFID[j] = strId;
                ShowReportSaleInvoice_New(dtHeader, dtLinetest, strId);
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        private void ShowReportSaleInvoice_New(DataTable dtHeader, DataTable dtLinetest, string strIdSavePDF)
        {
            try
            {
                decimal GlobalTaxAmount = 0;

                DataTable dtAmountWords = null;
                DataTable dtLine = new DataTable();
                CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new App_Code.AmountToWords();
                decimal TotalAmount = 0;
                string InvoiceNo = dtHeader.Rows[0]["INVOICE_NO"].ToString();
                if (dtLinetest.Rows.Count > 0)
                {
                    dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
                    dtLine.Columns.Add("BOX"); dtLine.Columns.Add("BoxConv"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
                    dtLine.Columns.Add("TAX_CODE"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
                    dtLine.Columns.Add("TAX_AMOUNT"); dtLine.Columns.Add("LINEAMOUNT"); dtLine.Columns.Add("DISC_AMOUNT"); dtLine.Columns.Add("SEC_DISC_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_AMOUNT");
                    dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
                    dtLine.Columns.Add("TDValue");
                    dtLine.Columns.Add("TaxableAmt");
                    dtLine.Columns.Add("basic");



                    h1 = new HashSet<string>();
                    for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    {
                        if (!h1.Contains(dtLinetest.Rows[i]["TAX_CODE"].ToString()))
                        {
                            h1.Add(dtLinetest.Rows[i]["TAX_CODE"].ToString());
                        }
                        dtLine.Rows.Add();
                        dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
                        dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
                        dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
                        dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
                        dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
                        dtLine.Rows[i]["BoxConv"] = Convert.ToDecimal(dtLinetest.Rows[i]["BoxConv"]);
                        dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
                        dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
                        dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
                        dtLine.Rows[i]["TAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_CODE"]);
                        dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
                        dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        dtLine.Rows[i]["TAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                        GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

                        dtLine.Rows[i]["LINEAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["LINEAMOUNT"]);
                        dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
                        dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
                        dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
                        dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
                        dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
                        dtLine.Rows[i]["TaxableAmt"] = Convert.ToDecimal(dtLinetest.Rows[i]["TaxableAmt"]);
                        dtLine.Rows[i]["basic"] = Convert.ToDecimal(dtLinetest.Rows[i]["basic"]);
                        TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);

                    }

                    //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

                    decimal HeaderAmount = TotalAmount;
                    decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
                    //decimal TotalTaxAmount = 12 ;
                    decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

                    //---Calculating Round Off Value for the Sale Invoice Bill---//
                    decimal RoundOffValue = 0;
                    decimal FinalValueForWords = 0;

                    double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
                    if (decimalpoints > 0.5)
                    {
                        RoundOffValue = (decimal)Math.Round(TotalNetValue);
                        FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
                        decimalpoints = 1 - decimalpoints;    // if Rounding Value is greater than 0.50 then plus sign with decimal points.
                    }
                    else
                    {
                        decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
                        RoundOffValue = (decimal)Math.Floor(TotalNetValue);
                        FinalValueForWords = RoundOffValue;
                    }

                    if (dtLinetest.Rows.Count > 0)
                    {
                        // h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
                        int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
                        int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
                        int totalrec = dtlinecount % 41;

                        if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
                        {
                            totalrec = dtlinecount % 32;
                            int totalcount = 32 - totalrec;  ///------Total Rows on a single page is 25
                            if (totalrec != 0)
                            {
                                //for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
                                //{
                                //    DataRow dr = dtLine.NewRow();

                                //    dr[0] = ""; dr[1] = ""; dr[2] = "";
                                //    dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                //    dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                //    dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                //    dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                //    dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0;
                                //    dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);

                                //}
                            }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));

                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + dtLinetest.Rows[0]["Invoice_No"].ToString() + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);

                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleReturnGST.rdl");
                            ReportViewer1.ShowPrintButton = true;

                        }
                        else
                        {
                            totalrec = dtlinecount % 41;
                            int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
                            if (totalrec != 0)
                            {
                                //for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
                                //{
                                //    DataRow dr = dtLine.NewRow();

                                //    dr[0] = ""; dr[1] = ""; dr[2] = "";
                                //    dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
                                //    dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
                                //    dr[11] = 0; dr[12] = 0; dr[13] = 0;
                                //    dr[14] = 0; dr[15] = 0; dr[16] = 0;
                                //    dr[17] = 0; dr[18] = 0; dr[19] = 0; dr[20] = 0; dr[21] = 0;

                                //    dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
                                //}
                            }
                            string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                            string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + dtLinetest.Rows[0]["Invoice_No"].ToString() + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                            dtAmountWords = obj.GetData(queryAmountWords);

                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(RDS1);
                            ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                            ReportViewer1.LocalReport.DataSources.Add(RDS2);
                            ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                            ReportViewer1.LocalReport.DataSources.Add(RDS3);
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PurchaseReturnGST.rdl");
                            ReportViewer1.ShowPrintButton = true;
                        }
                    }
                }

                ReportViewer1.LocalReport.DisplayName = dtLinetest.Rows[0]["Invoice_No"].ToString();

                //ReportViewer1.LocalReport.Refresh();
                //UpdatePanel1.Update();

                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + strIdSavePDF + ".pdf");


                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");


                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    stream.Write(Bytes, 0, Bytes.Length);
                }


                //Warning[] warnings;
                //string[] streamIds;
                //string mimeType = string.Empty;
                //string encoding = string.Empty;
                //string extension = string.Empty;

                //byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                //bytelist.Add(Bytes);

                #endregion
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        public static void MergeFiles(string destinationFile, string[] sourceFiles)
        {
            try
            {
                int f = 0;
                // we create a reader for a certain document
                PdfReader reader = new PdfReader(sourceFiles[f]);
                // we retrieve the total number of pages
                int n = reader.NumberOfPages;
                //  Console.WriteLine("There are " + n + " pages in the original file.");
                // step 1: creation of a document-object
                Document document = new Document(reader.GetPageSizeWithRotation(1));
                // step 2: we create a writer that listens to the document
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
                // step 3: we open the document
                document.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;
                int rotation;
                // step 4: we add content
                while (f < sourceFiles.Length)
                {
                    int i = 0;
                    while (i < n)
                    {
                        i++;
                        document.SetPageSize(reader.GetPageSizeWithRotation(i));
                        document.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        rotation = reader.GetPageRotation(i);
                        if (rotation == 90 || rotation == 270)
                        {
                            cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                        }
                        else
                        {
                            cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                        }
                        // Console.WriteLine("Processed page " + i);
                    }
                    f++;
                    if (f < sourceFiles.Length)
                    {
                        reader = new PdfReader(sourceFiles[f]);
                        // we retrieve the total number of pages
                        n = reader.NumberOfPages;
                        //   Console.WriteLine("There are " + n + " pages in the original file.");
                    }
                }
                // step 5: we close the document
                document.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                ErrorSignal.FromCurrentContext().Raise(e);
            }

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    String SaleInvoiceNo = string.Empty;
                    String strRetVal2 = "DebitNote";
                    LinkButton LnkBtnInvoice;
                    HyperLink HL;
                    HL = new HyperLink();
                    int i = e.Row.RowIndex;

                    string SaleInvoice = e.Row.Cells[0].Text;
                    LnkBtnInvoice = (LinkButton)e.Row.FindControl("lnkbtn");
                    SaleInvoiceNo = LnkBtnInvoice.Text;

                    HL = (HyperLink)e.Row.FindControl("HPLinkPrint");

                    //HL.CssClass = "iframe";
                    HL.NavigateUrl = "#";
                    //HL.NavigateUrl = "frmReport.aspx?SaleInvoiceNo=" + SaleInvoiceNo + "&Type=" + strRetVal2 + "";
                    HL.Font.Bold = true;
                    HL.Attributes.Add("onclick", "window.open('frmReport.aspx?SaleInvoiceNo=" + SaleInvoiceNo + "&Type=" + strRetVal2 + "','_newtab');");
                    //HL.Attributes.Add("onclick", "setFrame();");


                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkbox = (CheckBox)sender;
                if (chkbox.Checked)
                {
                    foreach (GridViewRow grv in GridView1.Rows)
                    {
                        CheckBox chkboxTest = (CheckBox)grv.Cells[0].FindControl("chkSONO");
                        chkboxTest.Checked = true;
                    }
                }
                else
                {
                    foreach (GridViewRow grv in GridView1.Rows)
                    {
                        CheckBox chkboxTest = (CheckBox)grv.Cells[0].FindControl("chkSONO");
                        chkboxTest.Checked = false;
                    }
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;

                //adp1 = new SqlDataAdapter("select a.CUSTOMER_CODE,a.INVOICE_NO,a.LINE_NO,a.PRODUCT_CODE,a.AMOUNT,a.BOX,a.CRATES,a.LTR,a.QUANTITY," +
                //                        " a.MRP,a.RATE,a.UOM,a.TAX_CODE,a.TAX_AMOUNT,a.DISC_AMOUNT,a.SEC_DISC_AMOUNT,b.product_group,b.product_name" +
                //                         " from ax.ACXSALEINVOICELINE a, ax.ACXProductMaster b " +
                //                          " where a.INVOICE_NO = '" + lnk.Text + "' and a.product_code = b.product_code ", conn);

                adp1 = new SqlDataAdapter("select a.CUSTOMER_CODE,a.INVOICE_NO,a.LINE_NO,a.PRODUCT_CODE,a.AMOUNT,a.BOX,a.CRATES,a.LTR,a.QUANTITY," +
                                        " a.MRP,a.RATE,a.UOM,a.TAX_CODE,a.TAX_AMOUNT,a.DISC_AMOUNT,a.SEC_DISC_AMOUNT,b.product_group,b.product_name,BOXQTY,PCSQty,BOXPCS " +
                                         " from ax.ACXSALEINVOICELINE a, ax.InventTable b " +
                                          " where a.INVOICE_NO = '" + lnk.Text + "' and  a.product_code = b.ItemId and a.SiteID='" + Session["SiteCode"].ToString() + "' ", conn);

                ds1.Clear();
                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    GridView2.DataSource = ds1;
                    GridView2.DataBind();

                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            finally
            {
                conn.Close();
                conn.Dispose();

            }
        }


        //Todo Commented by Vijay
        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(txtFromDate.Text) < Convert.ToDateTime("01-Jul-2017"))
            {
                txtFromDate.Text = "01-Jul-2017";
            }
        }
    }

}