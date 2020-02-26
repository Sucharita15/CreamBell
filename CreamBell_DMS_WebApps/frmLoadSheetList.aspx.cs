using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Microsoft.Reporting.WebForms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmLoadSheetList : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        public static string ParameterName = string.Empty;
        List<byte[]> bytelist = new List<byte[]>();
        HashSet<string> h1 = new HashSet<string>();
        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
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
                    // ContentPlaceHolder myContent = (ContentPlaceHolder)this.Master.FindControl("ContentPage");
                    // myContent.FindControl("fixedHeaderRow").Visible = false; //this is not working 
                    // ContentPlaceHolder myContent1 = (ContentPlaceHolder)this.Master.FindControl("ContentPage");
                    // myContent.FindControl("fixedHeaderRow1").Visible = false; //this is not working 
                GridDetail();
            }
        }
        private void GridDetail()
        {
           
            string sitecode1, DataAreaId;
            try
            {
                sitecode1 = Session["SiteCode"].ToString();

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Load_Sheet_List";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@siteid", sitecode1.ToString());
                try
                {
                    GridView1.EmptyDataText = "No Records Found";
                    GridView1.DataSource = cmd.ExecuteReader() ;
                    GridView1.DataBind(); 
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
                //adp1 = new SqlDataAdapter("Select * from ax.ACXLOADSHEETHEADER where Siteid = '" + sitecode1 + "' "+
                //"ORDER BY LOADSHEET_NO desc", conn);

                //ds2.Clear();
                //adp1.Fill(ds2, "dtl");

                //if (ds2.Tables["dtl"].Rows.Count != 0)
                //{
                //    for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                //    {
                //        GridView1.DataSource = ds2.Tables["dtl"];
                //        GridView1.DataBind();
                //        //foreach (GridViewRow grv in GridView1.Rows)
                //        //{
                //        //    CheckBox chkAll = (CheckBox)grv.Cells[0].FindControl("chklist");
                //        //    chkAll.Checked = true;
                //        //}
                //    }
                //}

                //if (conn.State == ConnectionState.Open)
                //{
                //    conn.Close();
                //    conn.Dispose();
                //}
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            String Indentno = row.Cells[0].Text;
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
          /*  string sitecode1, DataAreaId;
            sitecode1 = Session["SiteCode"].ToString();
            ContentPlaceHolder myContent = (ContentPlaceHolder)this.Master.FindControl("ContentPage");
            myContent.FindControl("fixedHeaderRow").Visible = true; //this is not working 
            ContentPlaceHolder myContent1 = (ContentPlaceHolder)this.Master.FindControl("ContentPage");
            myContent.FindControl("fixedHeaderRow1").Visible = true; //this is not working 

            try
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView3.DataSource = null;
                GridView3.DataBind();

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();
               
                GridViewRow gvrow = (GridViewRow)((CheckBox)sender).NamingContainer;
                int index = gvrow.RowIndex;
                CheckBox chk = sender as CheckBox;

                if(chk.Checked==true)
                {
                    adp1 = new SqlDataAdapter("select c.SO_NO,d.customer_code,d.customer_name,d.address1,a.*,b.product_group," +
                    " b.product_name from ax.ACXLOADSHEETLINE a, ax.ACXProductMaster b,ax.ACXLOADSHEETHEADER c , ax.ACXCUSTMASTER d " +
                   " where a.loadsheet_no = '" + chk.Text + "' and shiteid = '" + sitecode1 + "' and a.product_code = b.product_code and a.loadsheet_no = c.loadsheet_no " +
                   " and a.customer_code = d.customer_code ", conn);
                    
                }
                //adp1.SelectCommand.CommandTimeout = 0;
                ds1.Clear();

                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds1.Tables["dtl"].Rows.Count; i++)
                    {
                        GridView2.DataSource = ds1.Tables["dtl"];
                        GridView2.DataBind();

                        GridView3.DataSource = ds1.Tables["dtl"];
                        GridView3.DataBind();
                    }

                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }*/
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            string sitecode1, DataAreaId;

            try
            {
                sitecode1 = Session["SiteCode"].ToString();
           
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView3.DataSource = null;
                GridView3.DataBind();

                Session["lnk1"] = "";

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;

                Session["lnk1"] = lnk.Text;
               
                //adp1 = new SqlDataAdapter("select c.SO_NO,d.customer_code,d.customer_name,d.address1,a.*,b.product_group," +
                //" b.product_name from ax.ACXLOADSHEETLINE a, ax.inventtable b,ax.ACXLOADSHEETHEADER c , ax.ACXCUSTMASTER d " +
                //" where a.loadsheet_no = '" + lnk.Text + "' and shiteid = '" + sitecode1 + "' and a.product_code = b.ITEMID and a.loadsheet_no = c.loadsheet_no " +
                //" and a.customer_code = d.customer_code ", conn);

                adp1 = new SqlDataAdapter("select c.SO_NO,d.customer_code,d.customer_name,d.address1,a.*,b.product_group,b.product_name " +
               " from ax.ACXLOADSHEETLINE a " +
               "  Inner Join ax.inventtable b on a.product_code = b.ITEMID" +
               " Inner Join ax.ACXLOADSHEETHEADER c on c.siteid=a.shiteid and a.loadsheet_no = c.loadsheet_no " +
               " Inner JOin  ax.ACXCUSTMASTER d  on a.customer_code = d.customer_code " +
               " where a.loadsheet_no = '" + lnk.Text + "' and shiteid = '" + sitecode1 + "' " +
               "  ", conn);
                
                //adp1.SelectCommand.CommandTimeout = 0;
                ds1.Clear();

                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds1.Tables["dtl"].Rows.Count; i++)
                    {
                        GridView2.DataSource = ds1.Tables["dtl"];
                        GridView2.DataBind();

                        GridView3.DataSource = ds1.Tables["dtl"];
                        GridView3.DataBind();
                    }

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
            
            GridView3.DataSource = null;
            GridView3.DataBind();
            string sitecode1, DataAreaId;

            sitecode1 = Session["SiteCode"].ToString();
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            //if (ddlSearch.Text == "SO Number")
            //{
            //    adp1 = new SqlDataAdapter("select c.SO_NO,d.customer_code,d.customer_name,d.address1,a.*," +
            //    " b.product_group,b.product_name from ax.ACXLOADSHEETLINE a, ax.inventtable b,ax.ACXLOADSHEETHEADER c , ax.ACXCUSTMASTER d" +
            //    " where  c.SO_NO = '" + txtSerch.Text + "' and shiteid = '" + sitecode1 + "'"+
            //    " and a.loadsheet_no = '" + Session["lnk1"].ToString() + "' and a.product_code = b.ITEMID and a.loadsheet_no = c.loadsheet_no  " +
            //    " and a.customer_code = d.customer_code", conn);
            //}
            //else 
            //{
            //    adp1 = new SqlDataAdapter("select c.SO_NO,d.customer_code,d.customer_name,d.address1,a.*," +
            //    " b.product_group,b.product_name from ax.ACXLOADSHEETLINE a, ax.inventtable b,ax.ACXLOADSHEETHEADER c , ax.ACXCUSTMASTER d " +
            //    " where  c.Customer_Code like '%" + txtSerch.Text + "%' and shiteid = '" + sitecode1 + "'"+
            //    "and a.loadsheet_no = '" + Session["lnk1"].ToString() + "' and a.product_code = b.ITEMID and a.loadsheet_no = c.loadsheet_no " + 
            //    " and a.customer_code = d.customer_code", conn);               
            //}
            string query = string.Empty;
            if (txtSerch.Text !=string.Empty)
            {
               query = "select LoadSheet_No,LoadSheet_Date,Value from ax.ACXLOADSHEETHEADER where siteid = '" + sitecode1 + "' and LOADSHEET_DATE between '" + txtSerch.Text + "' and  dateadd(dd,1,'" + txtSerch.Text + "')";
            }
            else
            {
                query = "select LoadSheet_No,LoadSheet_Date,Value from ax.ACXLOADSHEETHEADER where siteid = '" + sitecode1 + "' ";
            }
            

            //ds2.Clear();
            //adp1.Fill(ds2, "dtl");
            DataTable dt = new DataTable();
            dt = obj.GetData(query);

            if (dt.Rows.Count>= 0)
            {                
                    GridView1.DataSource =dt;
                    GridView1.DataBind();
                    
                        foreach (GridViewRow grv in GridView1.Rows)
                        {
                            CheckBox chkAll = (CheckBox)grv.Cells[0].FindControl("chklist");
                            if (txtSerch.Text != string.Empty)
                            {
                                chkAll.Checked = true;
                            }
                            else
                            {
                                chkAll.Checked = false;
                            }
                        }                                                           
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Record not found!');", true);
            }


            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridDetail();
            GridView2.Visible = false;
            GridView3.Visible = false;
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // GridView2.PageIndex = e.NewPageIndex;
            
        }
        protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GridView3.PageIndex = e.NewPageIndex;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    String LoadSheetNo = string.Empty;
                    String strRetVal2 = "LoadSheet";
                    LinkButton LnkBtnLSNo;
                    HyperLink HL;
                    HL = new HyperLink();
                    int i = e.Row.RowIndex;

                    string SaleInvoice = e.Row.Cells[0].Text;
                    LnkBtnLSNo = (LinkButton)e.Row.FindControl("lnkbtn");
                    LoadSheetNo = LnkBtnLSNo.Text;

                    HL = (HyperLink)e.Row.FindControl("HPLinkPrint");
        
                    HL.NavigateUrl = "#";
                    HL.Font.Bold = true;
                    HL.Attributes.Add("onclick", "window.open('frmReport.aspx?LaodSheetNo=" + LoadSheetNo + "&Type=" + strRetVal2 + "','_newtab');");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string multipleparameter = string.Empty;
            int count = 0;
            foreach (GridViewRow grv in GridView1.Rows)
            {

                CheckBox chklist1 = (CheckBox)grv.Cells[0].FindControl("chklist");
                LinkButton lnkBtn = (LinkButton)grv.Cells[0].FindControl("lnkbtn");
                if (chklist1.Checked)
                {
                    count += 1;
                    // ShowReportSaleInvoice(string.Empty, chklist1.Text);
                    if (multipleparameter == string.Empty)
                    {
                        multipleparameter = "'" + lnkBtn.Text + "'";
                    }
                    else
                    {
                        multipleparameter += ",'" + lnkBtn.Text + "'";
                    }
                }

            }
            if (count==0)
            {
                return;
            }

            string listLoadsheet = "select * from ax.ACXLOADSHEETHEADER where SITEID = '" + Session["SiteCode"].ToString() + "'  and LOADSHEET_NO in (" + multipleparameter + ") order by LOADSHEET_NO";
            DataTable dtMainHeader1 = new DataTable();
            dtMainHeader1 = obj.GetData(listLoadsheet);

            for (int i = 0; i < dtMainHeader1.Rows.Count; i++)
            {                       
            //==============================                       
                string queryHeader = "select LSH.LOADSHEET_NO,  CONVERT(varchar(15),LSH.LOADSHEET_DATE,105) AS LOADSHEET_DATE , " +
                                    " ( SH.CUSTOMER_CODE +'-' + CUS.CUSTOMER_NAME) as CUSTOMER, SH.SO_NO, CONVERT( varchar(15), SH.SO_DATE,105) AS SO_DATE , " +
                                    " cast(round(SH.SO_VALUE,2) as numeric(36,2)) as SO_VALUE, SH.SITEID, USM.User_Name,USM.State from ax.ACXLOADSHEETHEADER LSH " +
                                    " INNER JOIN ax.ACXSALESHEADER SH ON LSH.LOADSHEET_NO = SH.LoadSheet_No and  LSH.SITEID= SH.SITEID " +
                                    " INNER JOIN AX.ACXCUSTMASTER CUS ON SH.CUSTOMER_CODE = CUS.CUSTOMER_CODE "+// exclude on 4th Apr 2017 and cus.Site_Code = LSH.SITEID" +
                                    " INNER JOIN AX.ACXUSERMASTER USM ON LSH.SITEID= USM.Site_Code " +
                                    " where LSH.SITEID = '" + Session["SiteCode"].ToString() + "'  and LSH.LOADSHEET_NO='" + dtMainHeader1.Rows[i]["LOADSHEET_NO"].ToString() + "' order by LSH.LOADSHEET_NO";

                dtHeader = obj.GetData(queryHeader);
                
                string queryLine = " Select ROW_NUMBER() over (ORDER BY LOADSHEET_NO,LSH.PRODUCT_CODE) AS SRNO,LOADSHEET_NO,( LSH.PRODUCT_CODE + '-'+ PM.PRODUCT_NAME) AS PRODUCT, "                             
                                + " LSH.BOXQTY as BOX,LSH.PCSQTY as PCS,isnull(LSH.BOXPCS,0) as TotalBoxPCS,LSH.BOX as TotalQtyConv,LSH.LTR, LSH.STOCKQTY_BOX,LSH.STOCKQTY_LTR from ax.ACXLOADSHEETLINE LSH " +
                                  " Inner Join ax.inventtable PM on LSH.PRODUCT_CODE = PM.ITEMID " +
                                  " where SHITEID = '" + Session["SiteCode"].ToString() + "' and LOADSHEET_NO='" + dtMainHeader1.Rows[i]["LOADSHEET_NO"].ToString() + "'";

                
                dtLinetest = obj.GetData(queryLine);
                

                ShowReportLoadSheet(dtHeader, dtLinetest);

            }
            murgebytges();     
           
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
            //byte[] newArray = new byte[size];
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
                //Response.Flush();
                // }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string str = ex.Message;
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

        private void ShowReportLoadSheet(DataTable dtHeader, DataTable dtLinetest)
        {
            try
            {
                DataTable dtLine = new DataTable();
                CreamBell_DMS_WebApps.App_Code.AmountToWords obj1 = new AmountToWords();
                DataTable dtAmountWords = null;

                dtLine = dtLinetest;

                string query = "Select VALUE  from ax.ACXLOADSHEETHEADER where LOADSHEET_NO='" + dtLinetest.Rows[0]["LOADSHEET_NO"].ToString() + "' and SITEID='" + Session["SiteCode"].ToString() + "'";
                DataTable dt = obj.GetData(query);


                decimal amount = Math.Round(Convert.ToDecimal(dt.Rows[0]["VALUE"].ToString()));
                string Words = obj1.words(Convert.ToInt32(amount));
                string queryAmountWords = "Select VALUE, '" + Words + "' as AMNTWORDS from ax.ACXLOADSHEETHEADER where LOADSHEET_NO='" + dtLinetest.Rows[0]["LOADSHEET_NO"].ToString() + "' and SITEID='" + Session["SiteCode"].ToString() + "'";

                dtAmountWords = obj.GetData(queryAmountWords);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.AsyncRendering = true;
                ReportDataSource RDS1 = new ReportDataSource("DSetHeader", dtHeader);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(RDS1);
                ReportDataSource RDS2 = new ReportDataSource("DSetLine", dtLine);
                ReportViewer1.LocalReport.DataSources.Add(RDS2);
                ReportDataSource RDS3 = new ReportDataSource("DSetAmountWords", dtAmountWords);
                ReportViewer1.LocalReport.DataSources.Add(RDS3);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\LoadSheet.rdl");
                ReportViewer1.ShowPrintButton = true;

                ReportViewer1.LocalReport.DisplayName = dtLinetest.Rows[0]["LOADSHEET_NO"].ToString();




                #region generate PDF of ReportViewer

                string savePath = Server.MapPath("Downloads\\" + "LoaSheet" + ".pdf");
                byte[] Bytes = ReportViewer1.LocalReport.Render(format: "PDF", deviceInfo: "");

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;



                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                bytelist.Add(bytes);

                #endregion

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}