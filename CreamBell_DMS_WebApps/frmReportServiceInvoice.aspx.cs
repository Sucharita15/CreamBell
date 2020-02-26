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
    public partial class frmReportServiceInvoice : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();
        public static string ParameterName = string.Empty;
        List<byte[]> bytelist = new List<byte[]>();
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
                CalendarExtender1.StartDate = Convert.ToDateTime("01-July-2017");
                txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                GridDetail();

                lblMessage.Text = "";
                string sqlstr11;
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                    sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+' -'+LS.Name  Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                else
                    sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+' -'+LS.Name  Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                ddlState.Items.Add("Select...");
                baseobj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                if (ddlState.Items.Count == 2)
                {
                    ddlState.SelectedIndex = 1;
                    ddlState_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ NAME as NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'order by NAME";
                ddlSiteId.Items.Clear();
                ddlSiteId.Items.Add("Select...");
                baseobj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ NAME as NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                ddlSiteId.Items.Clear();
                ddlSiteId.Items.Add("Select...");
                baseobj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");

            }
            if (ddlSiteId.Items.Count == 2)
            {
                ddlSiteId.SelectedIndex = 1;
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
                string Siteid = ((HiddenField)gvrow.FindControl("hndSiteid")).Value.ToString();
                adp1 = new SqlDataAdapter("EXEC USP_SERVICEINVOICELINEDETAIL '" + lnk.Text + "','" + Siteid.ToString() + "'", conn);
                ds1.Clear();
                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    GridView2.DataSource = ds1;
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    String ServiceInvoiceNo = string.Empty;
                    String strRetVal2 = "ServiceInvoice";
                    LinkButton LnkBtnInvoice;
                    HyperLink HL;
                    HL = new HyperLink();
                    int i = e.Row.RowIndex;
                
                    string SaleInvoice = e.Row.Cells[0].Text;
                    LnkBtnInvoice = (LinkButton)e.Row.FindControl("lnkbtn");
                    ServiceInvoiceNo = LnkBtnInvoice.Text;
                
                    HL = (HyperLink)e.Row.FindControl("HPLinkPrint");
                    string Siteid = ((HiddenField)e.Row.FindControl("hndSiteid")).Value.ToString();
                
                    //HL.CssClass = "iframe";
                    HL.NavigateUrl = "#";
                    //HL.NavigateUrl = "frmReport.aspx?SaleInvoiceNo=" + SaleInvoiceNo + "&Type=" + strRetVal2 + "";
                    HL.Font.Bold = true;
                    HL.Attributes.Add("onclick", "window.open('frmReport.aspx?ServiceInvoiceNo=" + ServiceInvoiceNo + "&Type=" + strRetVal2 + "&Site=" + Siteid + "','_newtab');");
                    //HL.Attributes.Add("onclick", "setFrame();");
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
                lblMessage.Text = "Please Provide The Date Range Parameter !";
                lblMessage.Visible = true;

            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text == string.Empty)
            {
                value = false;
                lblMessage.Text = "Please Provide The TO Date Range Parameter !";
                lblMessage.Visible = true;

            }
            if (txtFromDate.Text == string.Empty && txtToDate.Text != string.Empty)
            {
                value = false;
                lblMessage.Text = "Please Provide The FROM Date Range Parameter !";
                lblMessage.Visible = true;

            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            return value;
        }

        private void GridDetail()
        {
            string sitecode1;
            try
            {
                sitecode1 = Session["SiteCode"].ToString();
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                conn = obj.GetConnection();
                string FromDate = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy");
                string ToDate = Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyyy");
                string query = "EXEC USP_SERVICEINVOICEDETAIL '" + sitecode1 + "','" + FromDate + "','" + ToDate + "'";
                adp1 = new SqlDataAdapter(query, conn);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();

            if (txtFromDate.Text.Trim().Length > 0)
            {
                if (!baseObj.IsDate(txtFromDate.Text))
                {
                    string message = "alert('Invalid From Date !!!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    txtFromDate.Focus();
                    return;
                }
            }
            if (txtToDate.Text.Trim().Length > 0)
            {
                if (!baseObj.IsDate(txtToDate.Text))
                {
                    string message = "alert('Invalid To Date !!!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
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

                string FromDate = Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy");
                string ToDate = endDate.ToString("dd-MMM-yyyy");
                if (ddlState.SelectedItem.Text == "Select...")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please fill the State !! ');", true);
                    return;
                }
                else
                {
                    if (ddlSiteId.SelectedItem.Text == "Select...")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Please fill the Distributor !! ');", true);
                        return;
                    }
                    else
                    {
                        string query = "EXEC USP_SERVICEINVOICEDETAIL '" + ddlSiteId.SelectedValue + "','" + FromDate + "','" + ToDate + "'";
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
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('No Data Exist !! ');", true);
                            GridView1.DataSource = null;
                            GridView1.DataBind();
                        }
                    }
                }
            }
        }

        protected void btnMultiPrint_Click(object sender, EventArgs e)
        {
            string multipleInvoice = string.Empty;
            string MulqueryHeader = "USP_ServiceInvoiceHeaderReport";
            string MulqueryLine = "USP_ServiceInvoiceLineReport";
            string MulqueryGST = "USP_SERVICEINVOICETAXTRAN";
            List<string> ilist = new List<string>();
            List<string> litem = new List<string>();
            DataTable dtMulHeader, dtMulLine, dtMulGst;
            DataTable dtGSTCAL = null;
            dtMulHeader = new DataTable();
            dtMulLine = new DataTable();
            dtMulGst = new DataTable();
            foreach (GridViewRow grv in GridView1.Rows)
            {
                CheckBox chklist1 = (CheckBox)grv.Cells[0].FindControl("chklist");
                LinkButton lnkBtn = (LinkButton)grv.Cells[0].FindControl("lnkbtn");
                string Siteid = ((HiddenField)grv.FindControl("hndSiteid")).Value.ToString();
                if (chklist1.Checked)
                {
                    ilist = new List<string>();
                    litem = new List<string>();
                    ilist.Add("@Invoice_No"); litem.Add(lnkBtn.Text);
                    ilist.Add("@SiteID"); litem.Add(Siteid.ToString());
                    dtHeader = obj.GetData_New(MulqueryHeader, CommandType.StoredProcedure, ilist, litem);

                    dtMulHeader.Merge(dtHeader);
                    dtLinetest = obj.GetData_New(MulqueryLine, CommandType.StoredProcedure, ilist, litem);
                    dtMulLine.Merge(dtLinetest);
                    dtGSTCAL = obj.GetData_New(MulqueryGST, CommandType.StoredProcedure, ilist, litem);
                    dtMulGst.Merge(dtGSTCAL);
                }
            }
            string[] strSavePDFID = new string[dtMulHeader.Rows.Count + 1];

            //for merging a pdf into single one,I want extra pdf and for this i have called SaveInvoicePDf funtion
            //for single invoice PDF i have created 2 pdf and murge into one
            SaveInvoicePDf(dtMulHeader, dtMulLine, dtMulGst, 0, 0, strSavePDFID);
            //SaveInvoicePDf(dtMainHeader1, dtLine1, 0, 0, strSavePDFID);
            //

            for (int i = 0; i < dtMulHeader.Rows.Count; i++)
            {
                SaveInvoicePDf(dtMulHeader, dtMulLine, dtMulGst, i, i + 1, strSavePDFID);
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

        public void SaveInvoicePDf(DataTable dtMainHeader1, DataTable dtLine1, DataTable dtGst, int i, int j, string[] strSavePDFID)
        {
            var Hrows = (from row in dtMainHeader1.AsEnumerable()
                         where row.Field<string>("INVOICENO").Trim() == dtMainHeader1.Rows[i]["INVOICENO"].ToString()
                         select row);
            var Header = Hrows.CopyToDataTable();
            dtHeader = Header;

            var rows = (from row in dtLine1.AsEnumerable()
                        where row.Field<string>("INVOICENO").Trim() == dtMainHeader1.Rows[i]["INVOICENO"].ToString()
                        orderby row.Field<string>("SortFilter").Trim()
                        select row);
            var lines = rows.CopyToDataTable();
            dtLinetest = lines;
            //orderby row.Field<string>("SortFilter").Trim(), row.Field<string>("HSNCODE").Trim()
            DataTable Gstdt;
            if (dtGst.Select("INVOICENO='" + dtMainHeader1.Rows[i]["INVOICENO"].ToString() + "'").Length > 0)
            {
                var GSTrows = (from row in dtGst.AsEnumerable()
                               where row.Field<string>("INVOICENO").Trim() == dtMainHeader1.Rows[i]["INVOICENO"].ToString()
                               select row);
                var Gst = GSTrows.CopyToDataTable();
                Gstdt = Gst;
            }
            else
            {
                Gstdt = dtGst.Clone();
            }

            //DataRow[] rows = dtLinetest.Select("Invoice_No = + dtHeader.Rows[i]["INVOICE_NO"].ToString()");
            string strId = dtMainHeader1.Rows[0]["SITEID"].ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + i.ToString();
            strSavePDFID[j] = strId;
            ShowReportSaleInvoice_New(dtHeader, dtLinetest, Gstdt, strId);
        }

        private void ShowReportSaleInvoice_New(DataTable dtHeader, DataTable dtLinetest, DataTable dtGst, string strIdSavePDF)
        {
            try
            {
                decimal GlobalTaxAmount = 0;

                DataTable dtAmountWords = null;
                DataTable dtLine = new DataTable();
                CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new App_Code.AmountToWords();
                decimal TotalAmount = 0;
                string InvoiceNo = dtHeader.Rows[0]["INVOICENO"].ToString();
                if (dtLinetest.Rows.Count > 0)
                {


                    //decimal HeaderAmount = Convert.ToDecimal(dtHeader.Rows[0]["INVOICE_VALUE"].ToString());//
                    for (int i = 0; i < dtLinetest.Rows.Count; i++)
                    {
                        TotalAmount += Convert.ToDecimal(dtLinetest.Rows[i]["AMOUNT"]);
                        GlobalTaxAmount += Convert.ToDecimal(dtLinetest.Rows[i]["TAX_AMOUNT"]);
                    }

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
                        string Words = obj1.words(Convert.ToInt32(FinalValueForWords));
                        string queryAmountWords = "Select INVOICE_VALUE , '" + Words + "' as AMNTWORDS, " + decimalpoints + " as RoundOffValue from ax.AcxServiceSaleInvheader where INVOICENO='"  + dtHeader.Rows[0]["INVOICENO"].ToString() + "' and SITEID='" + dtHeader.Rows[0]["SITEID"].ToString() + "'";

                        dtAmountWords = obj.GetData(queryAmountWords);
                        ReportViewer1.AsyncRendering = true;
                        ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(RDS1);
                        ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLinetest);
                        ReportViewer1.LocalReport.DataSources.Add(RDS2);
                        ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                        ReportViewer1.LocalReport.DataSources.Add(RDS3);
                        ReportDataSource RDS4 = new ReportDataSource("DSetGST", dtGst);
                        ReportViewer1.LocalReport.DataSources.Add(RDS4);
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ServiceInvoiceGST.rdl");
                        ReportViewer1.ShowPrintButton = true;
                        #region COMMENT
                        
                        #endregion
                    }
                }

                ReportViewer1.LocalReport.DisplayName = dtLinetest.Rows[0]["INVOICENO"].ToString();

                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + strIdSavePDF + ".pdf");


                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");


                using (FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    stream.Write(Bytes, 0, Bytes.Length);
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
                ErrorSignal.FromCurrentContext().Raise(e);
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }

        }
        
    }
}