using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmItemWiseSaleDetail : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();
        public static string ParameterName = string.Empty;
        List<byte[]> bytelist = new List<byte[]>();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        App_Code.Global baseobj = new App_Code.Global();

        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
            //    FillCustomer();
                GridDetail();
                FillPSR();
                txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
                txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);

                //ShowReport("SaleInvoice", "0000000004");
            }
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            }
            LblMessage.Text = "";
            
        }

        private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        {
            FillPSR();
        }
        //protected void BtnShowReport_Click(object sender, EventArgs e)
        //{
        //    bool b = Validate();
        //    if (b)
        //    {
        //        ShowReport();
        //    }
        //}

        //private void ShowReport()
        //{
        //    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

        //    string FilterQuery = string.Empty;
        //    DataTable dtSetHeader = null;
        //    DataTable dtSetData = null;

        //    try
        //    {

        //        string query = "Select NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
        //        dtSetHeader = new DataTable();
        //        dtSetHeader = obj.GetData(query);

        //        FilterQuery = " SELECT SITEID,TRANSTYPE, INVOICE_DATE, INVOICE_NO, PRODUCT_NAME,PRODUCT_MRP, RATE,  BOX, LINEAMOUNT,  AMOUNT FROM ACX_SALESUMMARY_PARTY_ITEM_WISE "+
        //                      " where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >=" +
        //                      " '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'" +
        //                      //" ORDER BY INVOICE_DATE ASC , INVOICE_NO ASC ";


        //        dtSetData = new DataTable();
        //        dtSetData = obj.GetData(FilterQuery);

        //        LoadDataInReportViewer(dtSetHeader, dtSetData);
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message.ToString();
        //    }

        //}

        //private bool Validate()
        //{
        //    bool b;
        //    if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
        //    {
        //        b = false;
        //        LblMessage.Text = "Please Provide From Date and To Date";

        //    }
        //    else
        //    {
        //        b = true;
        //        LblMessage.Text = string.Empty;
        //    }
        //    return b;
        //}

        //private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData)
        //{
        //    try
        //    {

        //        if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
        //        {
        //            dtSetHeader.Columns.Add("Invoice_No");
        //            DataView dView = new DataView(dtSetData);
        //            DataTable dname = dView.ToTable(true, "Invoice_No");
        //            string strInovoiceNo = string.Empty;

        //            for (int i = 0; i < dname.Rows.Count; i++)
        //            {
        //                strInovoiceNo += dname.Rows[i]["Invoice_No"].ToString() + ";";
        //            }

        //            dtSetHeader.Rows[0]["Invoice_No"] = strInovoiceNo;





        //            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ItemWiseSaleDetail.rdl");
        //            ReportParameter FromDate = new ReportParameter();
        //            FromDate.Name = "FromDate";
        //            FromDate.Values.Add(txtFromDate.Text);
        //            ReportParameter ToDate = new ReportParameter();
        //            ToDate.Name = "ToDate";
        //            ToDate.Values.Add(txtToDate.Text);
        //            ReportParameter[] parameter = new ReportParameter[2];
        //            parameter[1] = FromDate;
        //            parameter[0] = ToDate;
        //            ReportViewer1.LocalReport.SetParameters(parameter);
        //            ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //            ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtSetHeader);
        //            ReportViewer1.LocalReport.DataSources.Clear();
        //            ReportViewer1.LocalReport.DataSources.Add(RDS1);
        //            ReportDataSource RDS2 = new ReportDataSource("DSetData", dtSetData);
        //            ReportViewer1.LocalReport.DataSources.Add(RDS2);
        //            ReportViewer1.ShowPrintButton = true;
        //            this.ReportViewer1.LocalReport.Refresh();
        //            ReportViewer1.Visible = true;
        //            ReportViewer1.ZoomPercent = 100;
        //            LblMessage.Text = String.Empty;
        //        }
        //        else
        //        {
        //            LblMessage.Text = "No Records Exists !!";
        //            ReportViewer1.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message.ToString();
        //    }
        //}

        private void GridDetail()
        {

            string sitecode1;
            try
            {
                sitecode1 = Session["SiteCode"].ToString();
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();

                adp1 = new SqlDataAdapter(" Select A.INVOICE_NO, A.INVOIC_DATE, A.SO_NO,SO_DATE, ('[' +A.CUSTOMER_CODE +']' + ' ' + B.CUSTOMER_NAME) as CUSTOMER , A.INVOICE_VALUE " +
                                           " from ax.ACXSALEINVOICEHEADER  A  INNER JOIN AX.ACXCUSTMASTER B   ON A.CUSTOMER_CODE=B.CUSTOMER_CODE " +
                                           " where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") and A.Invoic_Date>=DateAdd(Day,-1,getdate()) and A.Invoic_Date<=getdate()   ORDER BY a.INVOIC_DATE DESC,a.INVOICE_NO DESC", conn);

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

                //adp1 = new SqlDataAdapter("select a.CUSTOMER_CODE,a.INVOICE_NO,a.LINE_NO,a.PRODUCT_CODE,a.AMOUNT,a.BoxQty,a.PcsQty,a.BoxPcs,a.BOX,a.CRATES,a.LTR,a.QUANTITY," +
                //                        " a.MRP,a.RATE,a.UOM,a.TAX_CODE,a.TAX_AMOUNT,a.DISC_AMOUNT,a.SEC_DISC_AMOUNT,b.product_group,b.product_name" +
                //                         " from ax.ACXSALEINVOICELINE a, ax.InventTable b" +
                //                          " where a.INVOICE_NO = '" + lnk.Text + "' and  a.product_code = b.ItemId and a.SiteID='" + Session["SiteCode"].ToString() + "' ", conn);


                adp1 = new SqlDataAdapter("select a.CUSTOMER_CODE,a.INVOICE_NO,a.LINE_NO,a.PRODUCT_CODE,a.AMOUNT,a.BoxQty,a.PcsQty,a.BoxPcs,a.BOX,a.CRATES,a.LTR,a.QUANTITY," +
                            " a.MRP,a.RATE,a.UOM,a.TAX_CODE,a.TAX_AMOUNT,a.DISC_AMOUNT,a.SEC_DISC_AMOUNT,b.product_group,b.product_name" +
                                " from ax.ACXSALEINVOICELINE a, ax.InventTable b" +
                                " where a.INVOICE_NO = '" + lnk.Text + "' and  a.product_code = b.ItemId and a.SiteID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")  ", conn);
                ds1.Clear();

                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    GridView2.DataSource = ds1.Tables["dtl"];
                    GridView2.DataBind();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            //CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            //bool b = ValidateSearch();
            //if (b == true)
            //{
            //    DateTime endDate = Convert.ToDateTime(this.txtToDate.Text);
            //    endDate.AddDays(1);
            //    DateTime end = endDate.AddDays(1);

            //    string FromDate = txtFromDate.Text;
            //    string ToDate = end.ToString("dd-MM-yyyy");

            //    string query = "Select A.INVOICE_NO, A.INVOIC_DATE, A.SO_NO,SO_DATE, ('[' +A.CUSTOMER_CODE +']' + ' ' + B.CUSTOMER_NAME) as CUSTOMER , A.INVOICE_VALUE " +
            //               " from ax.ACXSALEINVOICEHEADER  A  INNER JOIN AX.ACXCUSTMASTER B   ON A.CUSTOMER_CODE=B.CUSTOMER_CODE " +
            //                " where SITEID= '" + Session["SiteCode"].ToString() + "' and Invoice_Date between '" + FromDate + "' and '" + ToDate + "' ";

            //    if (ddlSearch.Text == "Sales Invoice No")
            //    {
            //        query = query + "and INVOICE_NO like '%" + txtSerch.Text.Trim().ToString() + "%'  ORDER BY a.INVOIC_DATE DESC,a.INVOICE_NO DESC";
            //    }
            //    if (ddlSearch.Text == "Customer")
            //    {
            //        query = query + "and (A.CUSTOMER_CODE like '%" + txtSerch.Text.Trim().ToString() + "%' or B.CUSTOMER_NAME like '%" + txtSerch.Text.Trim().ToString() + "%') " +
            //                        " ORDER BY a.INVOIC_DATE DESC,a.INVOICE_NO DESC";
            //    }


            //    DataTable dtFilter = obj.GetData(query);
            //    if (dtFilter.Rows.Count > 0)
            //    {
            //        GridView1.DataSource = dtFilter;
            //        GridView1.DataBind();
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('No Data Exist !! ');", true);
            //        GridDetail();
            //        txtSearch.Text = string.Empty;
            //    }
            //}

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
                //uppanel.Update();
                //UpdatePanel1.Update();
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The FROM Date Range Parameter !');", true);
                //string message = "alert('Please Provide The FROM Date Range Parameter !');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            if (txtInvoiceNoStart.Text == string.Empty && txtInvoiceNoEnd.Text != string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Provide The From Invoice No Range Parameter !";
                LblMessage.Visible = true;
                //uppanel.Update();
                //UpdatePanel1.Update();
                // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The From Invoice No Range Parameter !');", true);
                //string message = " alert('Please Provide The From Invoice No Range Parameter  !');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            if (txtInvoiceNoStart.Text != string.Empty && txtInvoiceNoEnd.Text == string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Provide The To Invoice No Range Parameter !";
                LblMessage.Visible = true;
                //uppanel.Update();
                //UpdatePanel1.Update();
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide The To Invoice No Range Parameter !');", true);
                //string message = " alert('Please Provide The To Invoice No Range Parameter !');";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            return value;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // GridView1.PageIndex = e.NewPageIndex;
            // GridDetail();
            //  GridView2.Visible = false;


        }

        protected void LnkBtnPrint_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            LinkButton lnk = sender as LinkButton;

            LinkButton LnkBtnInvoice;
            LinkButton LnkBtnPrint;

            int i = gvrow.RowIndex;
            LnkBtnInvoice = (LinkButton)GridView1.Rows[i].FindControl("lnkbtn");

            LnkBtnPrint = (LinkButton)GridView1.Rows[i].FindControl("LnkBtnPrint");

            string invoiceNo = LnkBtnInvoice.Text;

            //LnkBtnPrint.CssClass = "iframe";
            LnkBtnPrint.
            //string invoiceNo =  GridView1.Rows[i].Cells[0].Text.ToString();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('frmReport.aspx?','_newtab');", true);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('frmReport.aspx?" + invoiceNo + "','_newtab');", true);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    String SaleInvoiceNo = string.Empty;
                    String strRetVal2 = "SaleInvoice";
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
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public static byte[] concatAndAddContent(List<byte[]> pdf)
        {
            byte[] all;
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.LETTER);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                foreach (byte[] p in pdf)
                {
                    reader = new PdfReader(p);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        doc.SetPageSize(PageSize.LETTER);
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }

            return all;
        }

        public void murgebytges()
        {
            int size = 0;

            for (int i = 0; i < bytelist.Count; i++)
            {
                if (bytelist.Count == 1)
                {
                    size += bytelist[i].Length + 1;
                }
                else
                {
                    size += bytelist[i].Length;
                }
            }
            byte[] newArray = concatAndAddContent(bytelist);
            try
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Image.pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(newArray);
                Response.End();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string str = ex.Message;
            }

        }

        protected void chkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkboxSelectAll.Checked == true)
            //{
            //    for (int i = 0; i < chklist.Items.Count; i++)
            //    {
            //        chklist.Items[i].Selected = true;
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < chklist.Items.Count; i++)
            //    {
            //        chklist.Items[i].Selected = false;
            //    }
            //}
            //CheckBox ChkBoxHeader = (CheckBox)GridView1.HeaderRow.FindControl("chkboxSelectAll");
            //foreach (GridViewRow row in GridView1.Rows)
            //{
            //    CheckBox ChkBoxRows = (CheckBox)row.FindControl("chklist");
            //    if (ChkBoxHeader.Checked == true)
            //    {
            //        ChkBoxRows.Checked = true;
            //    }
            //    else
            //    {
            //        ChkBoxRows.Checked = false;
            //    }
            //}
        }

        protected void Button2_Click(object sender, EventArgs e)
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
                        multipleInvoice = "" + lnkBtn.Text + "";
                    }
                    else
                    {
                        multipleInvoice += "," + lnkBtn.Text + "";
                    }
                }
            }
            if (multipleInvoice == "")
            {
                LblMessage.Text = "Please select Invoice No.. !";
                return;
            }
            //==================

            string query = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
            DataTable dtSetHeader = new DataTable();
            dtSetHeader = obj.GetData(query);

            //           string queryHeader ="Select SITEID, PRODUCT_PACKSIZE, PRODUCT_NAME, cast(PRODUCT_MRP as decimal(9, 2)) as PRODUCT_MRP, cast(Sum(Box) as decimal(9, 2)) as Box , "+
            //"cast(Sum(PCS) as decimal(9, 2)) as PCS,cast(Sum([TotalBoxPCS]) as decimal(9,2)) as [TotalBoxPCS],Cast(Sum(TotalQtyConv) as Decimal(9, 2)) as TotalQtyConv, "+
            //  " cast(Sum(AMOUNT) as decimal(9, 2)) as Amount , ROW_NUMBER() OVER(ORDER BY SITEID,PRODUCT_SUBCATEGORY DESC) AS No from " +
            //   " (SELECT PROD.PRODUCT_SUBCATEGORY,AIC.SITEID, AIC.TRANSTYPE, CAST(PROD.PRODUCT_PACKSIZE as decimal(9, 2)) as PRODUCT_PACKSIZE ,  AIC.INVOICE_DATE, AIC.INVOICE_NO,  AIC.PRODUCT_NAME,AIC.PRODUCT_MRP, AIC.RATE, " + 
            //    " isnull(BOXQty, '0') as Box,  isnull(PCSQTY, '0') as PCS, isnull(BOXPCS, '0') as [TotalBoxPCS],  BOX as TotalQtyConv, "+
            //      "AIC.LINEAMOUNT,  AIC.AMOUNT" +
            //       " FROM ACX_SALESUMMARY_PARTY_ITEM_WISE AIC "+
            //         " left join ax.Inventtable PROD ON AIC.PRODUCT_CODE = PROD.ITEMID "+
            //            " where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_NO in (" + multipleInvoice + ") ) as a " +
            //    " group by PRODUCT_SUBCATEGORY,PRODUCT_NAME, SITEID, PRODUCT_PACKSIZE, PRODUCT_NAME, PRODUCT_MRP ";

            string queryHeader = "EXEC USP_ITEMWISESALESUMMARYREPORT " + "'" + ucRoleFilters.GetCommaSepartedSiteId() + "','" + multipleInvoice + "'";
            DataTable dtSetData = new DataTable();
            dtSetData = obj.GetData(queryHeader);

            dtSetHeader.Columns.Add("Invoice_No");
            dtSetHeader.Rows[0]["Invoice_No"] = multipleInvoice;
            LoadDataInReportViewer(dtSetHeader, dtSetData);

            // murgebytges();     
        }

        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData)
        {
            try
            {
                ReportViewer1.Visible = true;
                if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
                {


                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ItemWiseSaleDetail.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportParameter[] parameter = new ReportParameter[2];
                    parameter[1] = FromDate;
                    parameter[0] = ToDate;
                    ReportViewer1.LocalReport.SetParameters(parameter);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtSetHeader);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportViewer1.ShowPrintButton = true;


                    #region generate PDF of ReportViewer

                    string savePath = Server.MapPath("Downloads\\SaleSummary.pdf");
                    byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = string.Empty;

                    byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.        //--Logic Given By Amol The Master Asset of .Net--//
                    Response.Buffer = true;
                    // Response.Clear();
                    Response.ContentType = mimeType;
                    string filename1 = "SaleSummary" + "." + extension;
                    //Response.AddHeader("content-disposition:inline;", "filename=" + filename1);
                    Response.BinaryWrite(bytes); // create the file
                    //Response.Flush(); // send it to the client to download
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + filename1);
                    Response.Flush();
                    Response.End();

                    #endregion
                }
                else
                {
                    LblMessage.Text = "No Records Exists !!";
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                LblMessage.Text = ex.Message.ToString();
            }
        }

        //private void ShowReportSaleInvoice_New(DataTable dtHeader, DataTable dtLinetest, string strIdSavePDF)
        //{
        //    try
        //    {
        //        decimal GlobalTaxAmount = 0;

        //        DataTable dtAmountWords = null;
        //        DataTable dtLine = new DataTable();
        //        CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new AmountToWords();
        //        decimal TotalAmount = 0;
        //        string InvoiceNo = dtHeader.Rows[0]["INVOICE_NO"].ToString();
        //        if (dtLinetest.Rows.Count > 0)
        //        {
        //            dtLine.Columns.Add("SRNO"); dtLine.Columns.Add("PRODUCT_CODE"); dtLine.Columns.Add("PRODUCT_NAME"); dtLine.Columns.Add("PRODUCT_PACKSIZE");
        //            dtLine.Columns.Add("BOX"); dtLine.Columns.Add("MRP"); dtLine.Columns.Add("RATE"); dtLine.Columns.Add("LTR");
        //            dtLine.Columns.Add("TAX_CODE"); dtLine.Columns.Add("CRATES"); dtLine.Columns.Add("AMOUNT");
        //            dtLine.Columns.Add("TAX_AMOUNT"); dtLine.Columns.Add("LINEAMOUNT"); dtLine.Columns.Add("DISC_AMOUNT"); dtLine.Columns.Add("SEC_DISC_AMOUNT");
        //            dtLine.Columns.Add("ADDTAX_AMOUNT");
        //            dtLine.Columns.Add("ADDTAX_CODE"); dtLine.Columns.Add("PEValue");
        //            dtLine.Columns.Add("TDValue");
        //            h1 = new HashSet<string>();
        //            for (int i = 0; i < dtLinetest.Rows.Count; i++)
        //            {
        //                if (!h1.Contains(dtLinetest.Rows[i]["TAX_CODE"].ToString()))
        //                {
        //                    h1.Add(dtLinetest.Rows[i]["TAX_CODE"].ToString());
        //                }
        //                dtLine.Rows.Add();
        //                dtLine.Rows[i]["SRNO"] = dtLinetest.Rows[i]["SRNO"].ToString();
        //                dtLine.Rows[i]["PRODUCT_CODE"] = dtLinetest.Rows[i]["PRODUCT_CODE"].ToString();
        //                dtLine.Rows[i]["PRODUCT_NAME"] = dtLinetest.Rows[i]["PRODUCT_NAME"].ToString();
        //                dtLine.Rows[i]["PRODUCT_PACKSIZE"] = dtLinetest.Rows[i]["PRODUCT_PACKSIZE"];
        //                dtLine.Rows[i]["BOX"] = Convert.ToDecimal(dtLinetest.Rows[i]["BOX"]);
        //                dtLine.Rows[i]["MRP"] = Convert.ToDecimal(dtLinetest.Rows[i]["MRP"]);
        //                dtLine.Rows[i]["RATE"] = Convert.ToDecimal(dtLinetest.Rows[i]["RATE"]);
        //                dtLine.Rows[i]["LTR"] = Convert.ToDecimal(dtLinetest.Rows[i]["LTR"]);
        //                dtLine.Rows[i]["TAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_CODE"]);
        //                dtLine.Rows[i]["CRATES"] = Convert.ToDecimal(dtLinetest.Rows[i]["CRATES"]);
        //                dtLine.Rows[i]["AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
        //                dtLine.Rows[i]["TAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

        //                GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);

        //                dtLine.Rows[i]["LINEAMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["LINEAMOUNT"]);
        //                dtLine.Rows[i]["DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["DISC_AMOUNT"]);
        //                dtLine.Rows[i]["ADDTAX_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_AMOUNT"]);
        //                dtLine.Rows[i]["SEC_DISC_AMOUNT"] = Convert.ToDecimal(dtLinetest.Rows[i]["SEC_DISC_AMOUNT"]);
        //                dtLine.Rows[i]["ADDTAX_CODE"] = Convert.ToDecimal(dtLinetest.Rows[i]["ADDTAX_CODE"]);
        //                dtLine.Rows[i]["PEValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["PEValue"]);
        //                dtLine.Rows[i]["TDValue"] = Convert.ToDecimal(dtLinetest.Rows[i]["TDValue"]);
        //                TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
        //            }

        //            //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//

        //            decimal HeaderAmount = TotalAmount;
        //            decimal TotalTaxAmount = GlobalTaxAmount; //dtLine.AsEnumerable().Sum(row => row.Field<decimal>("TAX_AMOUNT"));
        //            //decimal TotalTaxAmount = 12 ;
        //            decimal TotalNetValue = HeaderAmount;     // +TotalTaxAmount;  //Math.Round(HeaderAmount + TotalTaxAmount);

        //            //---Calculating Round Off Value for the Sale Invoice Bill---//
        //            decimal RoundOffValue = 0;
        //            decimal FinalValueForWords = 0;

        //            double decimalpoints = Convert.ToDouble(Math.Abs(TotalNetValue - Math.Floor(TotalNetValue)));
        //            if (decimalpoints > 0.5)
        //            {
        //                RoundOffValue = (decimal)Math.Round(TotalNetValue);
        //                FinalValueForWords = RoundOffValue;   //+ Convert.ToDecimal(decimalpoints);
        //                decimalpoints = 1 - decimalpoints;    // if Rounding Value is greater than 0.50 then plus sign with decimal points.
        //            }
        //            else
        //            {
        //                decimalpoints = 0 - decimalpoints;               // if Rounding Value is less than 0.50 then negative sign with decimal points.
        //                RoundOffValue = (decimal)Math.Floor(TotalNetValue);
        //                FinalValueForWords = RoundOffValue;
        //            }

        //            if (dtLinetest.Rows.Count > 0)
        //            {
        //                h1.Remove("0.00");                                          //----For Finding the different TAX AMOUNT CODE----//
        //                int TaxCodeCount = (h1.Count) * 2;                          //--- Count the TAX_CODE rows----//
        //                int dtlinecount = dtLine.Rows.Count + 6 + TaxCodeCount;     //---6 is for FOOTER ROWS COUNTS [TOTAL]---//
        //                int totalrec = dtlinecount % 41;

        //                if ((dtLinetest.Rows.Count > 41 && dtLinetest.Rows.Count < 49) || (totalrec > 0 && totalrec <= 8))
        //                {
        //                    totalrec = dtlinecount % 32;
        //                    int totalcount = 32 - totalrec;  ///------Total Rows on a single page is 25
        //                    if (totalrec != 0)
        //                    {
        //                        for (int i = 0; i < totalcount; i++)                        //-For adding the empty rows in a report.
        //                        {
        //                            DataRow dr = dtLine.NewRow();

        //                            dr[0] = ""; dr[1] = ""; dr[2] = "";
        //                            dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
        //                            dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
        //                            dr[11] = 0; dr[12] = 0; dr[13] = 0;
        //                            dr[14] = 0; dr[15] = 0; dr[16] = 0;
        //                            dr[17] = 0;
        //                            dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);

        //                        }
        //                    }
        //                    string Words = obj1.words(Convert.ToInt32(FinalValueForWords));

        //                    string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + dtLinetest.Rows[0]["Invoice_No"].ToString() + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

        //                    dtAmountWords = obj.GetData(queryAmountWords);

        //                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
        //                    ReportViewer1.LocalReport.DataSources.Clear();
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
        //                    ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
        //                    ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS3);
        //                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice.rdl");
        //                    ReportViewer1.ShowPrintButton = true;

        //                }
        //                else
        //                {
        //                    totalrec = dtlinecount % 41;
        //                    int totalcount = 41 - totalrec;                           ///------Total Rows on a single page is 25
        //                    if (totalrec != 0)
        //                    {
        //                        for (int i = 0; i <= totalcount; i++)                        //-For adding the empty rows in a report.
        //                        {
        //                            DataRow dr = dtLine.NewRow();

        //                            dr[0] = ""; dr[1] = ""; dr[2] = "";
        //                            dr[3] = 0; dr[4] = 0; dr[5] = 0; dr[6] = 0;
        //                            dr[7] = 0; dr[8] = 0; dr[9] = 0; dr[10] = 0;
        //                            dr[11] = 0; dr[12] = 0; dr[13] = 0;
        //                            dr[14] = 0; dr[15] = 0; dr[16] = 0;
        //                            dr[17] = 0;

        //                            dtLine.Rows.InsertAt(dr, dtLine.Rows.Count + 1);
        //                        }
        //                    }
        //                    string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
        //                    string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.ACXSALEINVOICEHEADER where INVOICE_NO='" + dtLinetest.Rows[0]["Invoice_No"].ToString() + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

        //                    dtAmountWords = obj.GetData(queryAmountWords);

        //                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
        //                    ReportViewer1.LocalReport.DataSources.Clear();
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
        //                    ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
        //                    ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS3);
        //                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleInvoice1.rdl");
        //                    ReportViewer1.ShowPrintButton = true;
        //                }
        //            }
        //        }

        //        ReportViewer1.LocalReport.DisplayName = dtLinetest.Rows[0]["Invoice_No"].ToString();

        //        //ReportViewer1.LocalReport.Refresh();
        //        //UpdatePanel1.Update();

        //        #region generate PDF of ReportViewer

        //        string savePath = Server.MapPath("Downloads\\" + strIdSavePDF + ".pdf");


        //        byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");


        //        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        //        {
        //            stream.Write(Bytes, 0, Bytes.Length);
        //        }


        //        //Warning[] warnings;
        //        //string[] streamIds;
        //        //string mimeType = string.Empty;
        //        //string encoding = string.Empty;
        //        //string extension = string.Empty;

        //        //byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

        //        //bytelist.Add(Bytes);

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                while (f <= sourceFiles.Length)
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
                ErrorSignal.FromCurrentContext().Raise(e);
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            bool b = ValidateSearch();
            GridView1.DataSource = null;
            GridView2.DataSource = null;
            if (b == true)
            {
                DateTime endDate = Convert.ToDateTime(this.txtToDate.Text);
                //endDate.AddDays(1);
                //DateTime end = endDate.AddDays(1);

                string FromDate = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy");
                string ToDate = endDate.ToString("dd-MMM-yyyy");

                //string query = "Select A.INVOICE_NO, A.INVOIC_DATE, A.SO_NO,SO_DATE, ('[' +A.CUSTOMER_CODE +']' + ' ' + B.CUSTOMER_NAME) as CUSTOMER , A.INVOICE_VALUE " +
                //           " from ax.ACXSALEINVOICEHEADER  A  INNER JOIN AX.ACXCUSTMASTER B   ON A.CUSTOMER_CODE=B.CUSTOMER_CODE " +
                //            " where SITEID= '" + Session["SiteCode"].ToString() + "' and Invoic_Date>='" + FromDate + "' and Invoic_Date<='" + ToDate + "'";

                string query = "Select A.INVOICE_NO, A.INVOIC_DATE, A.SO_NO,SO_DATE, ('[' +A.CUSTOMER_CODE +']' + ' ' + B.CUSTOMER_NAME) as CUSTOMER , A.INVOICE_VALUE " +
           " from ax.ACXSALEINVOICEHEADER  A  INNER JOIN AX.ACXCUSTMASTER B   ON A.CUSTOMER_CODE=B.CUSTOMER_CODE " +
            " where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")  and Invoic_Date>='" + FromDate + "' and Invoic_Date<='" + ToDate + "'";

                if (txtInvoiceNoStart.Text != string.Empty && txtInvoiceNoEnd.Text != string.Empty)
                {
                    query = query + "and cast(Right(A.invoice_no,6) as int) between " + txtInvoiceNoStart.Text + " and " + txtInvoiceNoEnd.Text + " ";
                }
                if (rdoSI.Checked == true)
                {
                    query = query + " and A.TranType=1 ";
                }
                else if (rdoSR.Checked == true)
                {
                    query = query + " and A.TranType=2 ";
                }
                string custcode = "";
                foreach (System.Web.UI.WebControls.ListItem litem in lstCustomer.Items)
                {
                    if (litem.Selected)
                    {
                        if (custcode.Length == 0)
                            custcode = "'" + litem.Value.ToString() + "'";
                        else
                            custcode += ",'" + litem.Value.ToString() + "'";
                    }
                }

                string psrcode = "";
                foreach (System.Web.UI.WebControls.ListItem litem2 in lstPSR.Items)
                {
                    if (litem2.Selected)
                    {
                        if (psrcode.Length == 0)
                            psrcode = "'" + litem2.Value.ToString() + "'";
                        else
                            psrcode += ",'" + litem2.Value.ToString() + "'";
                    }
                }
                string beatcode = "";
                foreach (System.Web.UI.WebControls.ListItem litem1 in lstBeat.Items)
                {
                    if (litem1.Selected)
                    {
                        if (beatcode.Length == 0)
                            beatcode = "'" + litem1.Value.ToString() + "'";
                        else
                            beatcode += ",'" + litem1.Value.ToString() + "'";
                    }
                }

                if (lstPSR.SelectedValue != string.Empty)
                {
                    query = query + "and B.psr_code in (" + psrcode + ") ";

                    if (lstBeat.SelectedValue != string.Empty)
                    {
                        query = query + "and B.psr_beat in (" + beatcode + ") ";

                        if (lstCustomer.SelectedValue != string.Empty)
                        {
                            query = query + "and A.CUSTOMER_CODE in (" + custcode + ") ";
                        }
                    }
                }



                if (ddlSearch.Text == "Sales Invoice No")
                {
                    query = query + " and INVOICE_NO like '%" + txtSearch.Text.Trim().ToString() + "%'";
                }
                if (ddlSearch.Text == "Customer")
                {
                    query = query + " and (A.CUSTOMER_CODE like '%" + txtSearch.Text.Trim().ToString() + "%' or B.CUSTOMER_NAME like '%" + txtSearch.Text.Trim().ToString() + "%') ";
                }
                query = query + "  ORDER BY a.INVOIC_DATE DESC,a.INVOICE_NO DESC";

                DataTable dtFilter = obj.GetData(query);
                if (dtFilter.Rows.Count > 0)
                {

                    GridView1.DataSource = dtFilter;
                    GridView1.DataBind();

                    foreach (GridViewRow grv in GridView1.Rows)
                    {
                        CheckBox chkAll = (CheckBox)grv.Cells[0].FindControl("chklist");
                        chkAll.Checked = true;
                    }
                    uppanel.Update();
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

        protected void drpPSR_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBeat();
            uppanel.Update();
          //  FillCustomer();
            //uppanel.Update();
        }

        protected void lstBeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomer();
            uppanel.Update();
        }

        public void FillPSR()
        {
                DataTable dt = new DataTable();
                string query = @"Select PSR_Code +'-'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
                             " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
                             " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
                             " where B.Site_code IN (" + ucRoleFilters.GetCommaSepartedSiteId() + "))";
                dt = new DataTable();
                lstPSR.Items.Clear();
                dt = baseobj.GetData(query);
                //lstPSR.Items.Add("Select...");
                lstPSR.DataSource = dt;
                lstPSR.DataTextField = "PSRName";
                lstPSR.DataValueField = "PSR_Code";
                lstPSR.DataBind();
                //lstPSR.Items.Clear();
                //
                //baseobj.BindToDropDown(lstPSR, query, "PSRName", "PSR_Code");
         }
          
        public void FillBeat()
        {
            string psrcode = "";
            foreach (System.Web.UI.WebControls.ListItem litem in lstPSR.Items)
            {
                if (litem.Selected)
                {
                    if (psrcode.Length == 0)
                        psrcode = "'" + litem.Value.ToString() + "'";
                    else
                        psrcode += ",'" + litem.Value.ToString() + "'";
                }
            }
            if (lstPSR.SelectedValue != string.Empty)
            {
                string strQuery = @"select BeatCode +'-'+BeatName as BeatName,BeatCode from [ax].[ACXPSRBeatMaster] where PSRCode in (" + psrcode + ")";
                lstBeat.Items.Clear();
                // lstBeat.Items.Add("Select...");
                DataTable dt = obj.GetData(strQuery);

                lstBeat.DataSource = dt;
                lstBeat.DataTextField = "BeatName";
                lstBeat.DataValueField = "BeatCode";
                lstBeat.DataBind();
            }
            if (lstPSR.SelectedValue == string.Empty)
            {
                lstBeat.Items.Clear();
                lstCustomer.Items.Clear();
            }
        }


        public void FillCustomer()
        {

            string psrcode = "";
            foreach (System.Web.UI.WebControls.ListItem litem2 in lstPSR.Items)
            {
                if (litem2.Selected)
                {
                    if (psrcode.Length == 0)
                        psrcode = "'" + litem2.Value.ToString() + "'";
                    else
                        psrcode += ",'" + litem2.Value.ToString() + "'";
                }
            }
            string beatcode = "";
            foreach (System.Web.UI.WebControls.ListItem litem1 in lstBeat.Items)
            {
                if (litem1.Selected)
                {
                    if (beatcode.Length == 0)
                        beatcode = "'" + litem1.Value.ToString() + "'";
                    else
                        beatcode += ",'" + litem1.Value.ToString() + "'";
                }
            }
            if (lstBeat.SelectedValue != string.Empty)
            {
            string strQuery = @"Select Customer_Code+'-'+Customer_Name as Name,Customer_Code from ax.ACXCUSTMASTER where Blocked = 0 and PSR_CODE in (" + psrcode + ") and PSR_BEAT in (" + beatcode + ") "
                      + " and SITE_CODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
            lstCustomer.Items.Clear();
            //lstCustomer.Items.Add("Select...");
            //baseobj.BindToDropDown(lstCustomer, strQuery, "Name", "Customer_Code");
            DataTable dt = obj.GetData(strQuery);
            lstCustomer.DataSource = dt;
            lstCustomer.DataTextField = "Name";
            lstCustomer.DataValueField = "Customer_Code";
            lstCustomer.DataBind();
            }
            if (lstBeat.SelectedValue == string.Empty)
            {
                lstCustomer.Items.Clear();
                
            }

        }

        protected void rdoBoth_CheckedChanged(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            uppanel.Update();
            
        }

        protected void chkboxSelectAll_CheckedChanged1(object sender, EventArgs e)
        {
            CheckBox chkboxSelectAll = (CheckBox)GridView1.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
            if (chkboxSelectAll.Checked == true)
            {
                foreach (GridViewRow grv in GridView1.Rows)
                {
                    CheckBox chklist1 = (CheckBox)grv.Cells[0].FindControl("chklist");
                    chklist1.Checked = true;
                }
            }
            else
            {
                foreach (GridViewRow grv in GridView1.Rows)
                {
                    CheckBox chklist1 = (CheckBox)grv.Cells[0].FindControl("chklist");
                    chklist1.Checked = false;
                }
            }

            //CheckBox chk = (sender as CheckBox);
            //GridView gv = chk.NamingContainer.Parent.Parent as GridView;
            //foreach (GridViewRow row in gv.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        (row.FindControl("chklist") as CheckBox).Checked = chk.Checked;
            //    }
            //}
            //upgrid.Update();
        }

        protected void OnChildCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (sender as CheckBox);
            GridView gv = chk.NamingContainer.Parent.Parent as GridView;
            bool isAllChecked = true;
            foreach (GridViewRow row in gv.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (!(row.FindControl("chklist") as CheckBox).Checked)
                    {
                        isAllChecked = false;
                        break;
                    }
                }
            }
            CheckBox chkAll = (gv.HeaderRow.FindControl("chkboxSelectAll") as CheckBox);
            chkAll.Checked = isAllChecked;
        }

        protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Enabled = false;
            if (ddlSearch.SelectedIndex > 0)
            { txtSearch.Enabled = true; txtSearch.Focus(); }
        }
    }
}