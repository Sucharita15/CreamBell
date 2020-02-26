using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.Drawing;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchUnPostList : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
               ShowPrePurchaseInvoiceList();
            }
        }

        private void ShowPrePurchaseInvoiceList()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //string query = " Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE, " +
            //               " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS] " + 
            //               " from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE SITE_CODE='"+ Session["SiteCode"].ToString() +"' AND DATAAREAID='"+Session["DATAAREAID"].ToString()+"' " +
            //               " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC";

             string query = "select DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,SALE_INVOICENO,SSI.ODNNO GSTINNo,SALE_INVOICEDATE,MATERIAL_VALUE,[STATUS] from (Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE,"+  
                             " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS]  from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE "+
                             " SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "') PIR" +
                             " left join ax.ACX_STAGINGSALESINVOICE SSI"+
                             " on PIR.SALE_INVOICENO = SSI.INVOICE_NO" +
                             " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC";

            DataTable dtPurchInvoiceList = obj.GetData(query);
            if (dtPurchInvoiceList.Rows.Count > 0)
            {
                GridHeaderPurchList.DataSource = dtPurchInvoiceList;
                GridHeaderPurchList.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' No Records Exist !! ');", true);
            }
        }

        protected void GridHeaderPurchList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell cell = e.Row.Cells[7];
                string Status = cell.Text;
                if (Status == "UnPosted")
                {
                    cell.ForeColor = Color.DarkOrange;

                }
                if (Status == "Posted")
                {
                    cell.ForeColor = Color.Green;
                }
                
            }
        }

        protected void LnkBtnDocumnetNo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gridRow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton Lnk = sender as LinkButton;
                Response.Redirect("frmPurchaseInvoiceReceipt.aspx?PreNo=" + Server.UrlEncode(Lnk.Text));

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btn2_Click(object sender, EventArgs e)       // For Search Button //
        {

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            try
            {
                if (ddlSearch.Text == "Receipt No")
                {

                

                    //adp1 = new SqlDataAdapter( "Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE, " +
                    //      " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS] " +
                    //      " from [ax].[ACXPURCHINVRECIEPTHEADERPRE] " + 
                    //      " WHERE SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' " +
                    //      " and DOCUMENT_NO='" + txtSerch.Text + "' " +
                    //      " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC ", conn);

                    string test = "select DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,SALE_INVOICENO,SSI.GSTINNo,SALE_INVOICEDATE,MATERIAL_VALUE,[STATUS] from (Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE," +
                             " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS]  from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE " +
                             " SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and DOCUMENT_NO='" + txtSerch.Text + "') PIR" +
                             " left join ax.ACX_STAGINGSALESINVOICE SSI" +
                             " on PIR.SALE_INVOICENO = SSI.INVOICE_NO" +
                             " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC";

                    adp1 = new SqlDataAdapter("select DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,SALE_INVOICENO,SSI.GSTINNo,SALE_INVOICEDATE,MATERIAL_VALUE,[STATUS] from (Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE," +
                             " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS]  from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE " +
                             " SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and DOCUMENT_NO='" + txtSerch.Text + "') PIR" +
                             " left join ax.ACX_STAGINGSALESINVOICE SSI" +
                             " on PIR.SALE_INVOICENO = SSI.INVOICE_NO" +
                             " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC", conn);

                }
                else if (ddlSearch.Text == "Invoice No")
                {
                    //adp1 = new SqlDataAdapter("Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE, " +
                    //    " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS] " +
                    //    " from [ax].[ACXPURCHINVRECIEPTHEADERPRE] " +
                    //    " WHERE SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' " +
                    //    " and SALE_INVOICENO ='" + txtSerch.Text + "' " +
                    //    " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC ", conn);

                    adp1 = new SqlDataAdapter("select DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,SALE_INVOICENO,SSI.GSTINNo,SALE_INVOICEDATE,MATERIAL_VALUE,[STATUS] from (Select DISTINCT DOCUMENT_NO, DOCUMENT_DATE, PURCH_INDENTNO, PURCH_INDENTDATE, SALE_INVOICENO, SALE_INVOICEDATE,MATERIAL_VALUE," +
                             " CASE STATUS WHEN 0 THEN 'UnPosted' WHEN 1 THEN 'Posted' END AS [STATUS]  from [ax].[ACXPURCHINVRECIEPTHEADERPRE] WHERE " +
                             " SITE_CODE='" + Session["SiteCode"].ToString() + "' AND DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and SALE_INVOICENO ='" + txtSerch.Text + "') PIR" +
                             " left join ax.ACX_STAGINGSALESINVOICE SSI" +
                             " on PIR.SALE_INVOICENO = SSI.INVOICE_NO" +
                             " ORDER BY DOCUMENT_NO DESC, DOCUMENT_DATE DESC", conn);
                }
               
                ds2.Clear();
                adp1.Fill(ds2, "dtl");

                if (ds2.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                    {
                        GridHeaderPurchList.DataSource = ds2.Tables["dtl"];
                        GridHeaderPurchList.DataBind();
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' No Data Found !! ');", true);
                    ShowPrePurchaseInvoiceList();
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
    }
}