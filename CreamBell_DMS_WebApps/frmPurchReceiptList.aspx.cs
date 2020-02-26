using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchReceiptList : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                //ContentPlaceHolder myContent = (ContentPlaceHolder)this.Master.FindControl("ContentPage");
                //myContent.FindControl("fixedHeaderRow").Visible = false; //this is not working 
                GridDetail();
            }
        }
        private void GridDetail()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //string query = "Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, " +
            //               " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR " +
            //               " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH " +
            //               " INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO and PIL.SITEID=PIH.SITE_CODE and PIL.DATAAREAID=PIH.DATAAREAID " +
            //               " WHERE PIH.SITE_CODE='" + Session["SiteCode"].ToString() + "' AND PIH.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' " +
            //               " GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, " +
            //               " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO  " +
            //               " ORDER BY PIH.DOCUMENT_DATE DESC,PIH.DOCUMENT_NO DESC";

            string query = "select distinct DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,MATERIAL_VALUE,SALE_INVOICENO,SSI.ODNNO GSTINNO,SALE_INVOICEDATE,REFERENCENUMBER,Box,Crates,LTR from (Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO," +
                            " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR "+ 
                            " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH  INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO"+
                            " and PIL.SITEID=PIH.SITE_CODE and PIL.DATAAREAID=PIH.DATAAREAID  WHERE PIH.SITE_CODE='" + Session["SiteCode"].ToString() + "' AND PIH.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' " +
                            " GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, "+ 
                            " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO)"+
                            " BIS"+
                            " left join ax.ACX_STAGINGSALESINVOICE SSI"+
                            " on BIS.SALE_INVOICENO=SSI.invoice_no"+
                            " ORDER BY DOCUMENT_DATE DESC,DOCUMENT_NO DESC";
            DataTable dtPostedPurchInvoiceList = obj.GetData(query);
            if (dtPostedPurchInvoiceList.Rows.Count > 0)
            {
                GridHeaderPurchList.DataSource = dtPostedPurchInvoiceList;
                GridHeaderPurchList.DataBind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' No Records Exist !! ');", true);
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
                    
                    //adp1 = new SqlDataAdapter( " Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO," +
                    //                           " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR " +
                    //                           " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO " +
                    //                           " WHERE PIH.SITE_CODE='"+Session["SiteCode"].ToString()+"' AND PIH.DATAAREAID='"+ Session["DATAAREAID"].ToString() +"' and " +
                    //                           " PIH.DOCUMENT_NO='"+txtSerch.Text+"' GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, " +
                    //                           " PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO " +
                    //                           " ORDER BY PIH.DOCUMENT_DATE DESC,PIH.DOCUMENT_NO DESC ",conn);

                    adp1 = new SqlDataAdapter( "select distinct DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,MATERIAL_VALUE,SALE_INVOICENO,SSI.GSTINNO,SALE_INVOICEDATE,REFERENCENUMBER,Box,Crates,LTR from (Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO,"+
                            " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR "+ 
                            " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH  INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO"+
                            " and PIL.SITEID=PIH.SITE_CODE and PIL.DATAAREAID=PIH.DATAAREAID  WHERE PIH.SITE_CODE='" + Session["SiteCode"].ToString() + "' AND PIH.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and " +
                            " PIH.DOCUMENT_NO='"+txtSerch.Text+"' GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, "+ 
                            " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO)"+
                            " BIS"+
                            " left join ax.ACX_STAGINGSALESINVOICE SSI"+
                            " on BIS.SALE_INVOICENO=SSI.invoice_no"+
                            " ORDER BY DOCUMENT_DATE DESC,DOCUMENT_NO DESC",conn);

                }
                else if (ddlSearch.Text == "Invoice No")
                {


                    //adp1 = new SqlDataAdapter(" Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO," +
                    //                           " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR " +
                    //                           " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO " +
                    //                           " WHERE PIH.SITE_CODE='" + Session["SiteCode"].ToString() + "' AND PIH.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and " +
                    //                           " PIH.SALE_INVOICENO='" + txtSerch.Text + "' GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, " +
                    //                           " PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO " +
                    //                           " ORDER BY PIH.DOCUMENT_DATE DESC,PIH.DOCUMENT_NO DESC ", conn);


                    adp1 = new SqlDataAdapter("select distinct DOCUMENT_NO,DOCUMENT_DATE,PURCH_INDENTNO,PURCH_INDENTDATE,MATERIAL_VALUE,SALE_INVOICENO,SSI.GSTINNO,SALE_INVOICEDATE,REFERENCENUMBER,Box,Crates,LTR from (Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO," +
                            " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR " +
                            " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH  INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO" +
                            " and PIL.SITEID=PIH.SITE_CODE and PIL.DATAAREAID=PIH.DATAAREAID  WHERE PIH.SITE_CODE='" + Session["SiteCode"].ToString() + "' AND PIH.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and " +
                            " PIH.SALE_INVOICENO='" + txtSerch.Text + "' GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, " +
                            " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO)" +
                            " BIS" +
                            " left join ax.ACX_STAGINGSALESINVOICE SSI" +
                            " on BIS.SALE_INVOICENO=SSI.invoice_no" +
                            " ORDER BY DOCUMENT_DATE DESC,DOCUMENT_NO DESC", conn);

                }
                //else if (ddlSearch.Text == "Receipt Date")
                //{
                //    adp1 = new SqlDataAdapter(" Select PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO," +
                //                               " PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO AS REFERENCENUMBER, SUM(PIL.BOX) AS BOX,SUM(PIL.CRATES) AS CRATES, SUM(PIL.LTR) AS LTR " +
                //                               " from [ax].[ACXPURCHINVRECIEPTHEADER] PIH INNER JOIN [ax].[ACXPURCHINVRECIEPTLINE] PIL ON PIH.DOCUMENT_NO=PIL.PURCH_RECIEPTNO " +
                //                               " WHERE PIH.SITE_CODE='" + Session["SiteCode"].ToString() + "' AND PIH.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and " +
                //                               " PIH.SALE_INVOICENO='" + txtSerch.Text + "' GROUP BY PIH.DOCUMENT_NO, PIH.DOCUMENT_DATE, " +
                //                               " PIH.PURCH_INDENTNO, PIH.PURCH_INDENTDATE, PIH.MATERIAL_VALUE,PIH.SALE_INVOICENO, PIH.SALE_INVOICEDATE,PIH.PREDOCUMENT_NO " +
                //                               " ORDER BY PIH.DOCUMENT_DATE DESC,PIH.DOCUMENT_NO DESC ", conn);

                //    adp1 = new SqlDataAdapter("SELECT a.document_no,a.DOCUMENT_DATE,a.PURCH_INDENTNO,a.purch_indentdate,a.so_no," +
                //                       " a.purch_recieptno,a.sale_invoiceno,sum(b.box) as box,sum(b.crates)as crates ,sum(b.ltr) as ltr " +
                //                       " ,a.SALE_INVOICEDATE  FROM ax.ACXPURCHINVRECIEPTHEADER a, ax.ACXPURCHINVRECIEPTLINE b ,ax.InventTable c " +
                //                       " where a.PURCH_INDENTNO = b.PURCH_RECIEPTNO   and b.PRODUCT_CODE = c.ItemId " +
                //                       " and convert(varchar(10),a.DOCUMENT_DATE,103) = '" + Convert.ToDateTime(txtSerch.Text).ToString("dd/MM/yyyy") + "'" +
                //                      "  group by a.document_no,a.DOCUMENT_DATE,a.SALE_INVOICEDATE, a.PURCH_INDENTNO,a.purch_indentdate,a.so_no, " +
                //                       " a.purch_recieptno,a.sale_invoiceno order by a.DOCUMENT_DATE asc", conn);

                    
                //}

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
                    GridDetail();
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GridViewRow row = //GridView1.SelectedRow;
            //String Indentno = row.Cells[0].Text;
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                //conn = obj.GetConnection();

                //ContentPlaceHolder myContent = (ContentPlaceHolder)this.Master.FindControl("ContentPage");
                //myContent.FindControl("fixedHeaderRow").Visible = true; //this is not working 

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;

                string query = "Select A.PURCH_RECIEPTNO,A.BOX,A.CRATES,A.LTR, B.PRODUCT_GROUP, A.PRODUCT_CODE +'/'+ B.PRODUCT_NAME AS PRODUCTDESC, " +
                               " B.PRODUCT_MRP,A.RATE,A.BASICVALUE,A.TRDDISCPERC,A.TRDDISCVALUE,A.PRICE_EQUALVALUE,A.VAT_INC_PERC," +
                               " A.VAT_INC_PERCVALUE,A.GROSSRATE,A.AMOUNT from ax.ACXPURCHINVRECIEPTLINE A , AX.INVENTTABLE B " +
                               " where A.PURCH_RECIEPTNO = '"+lnk.Text+"' and A.PRODUCT_CODE = B.ITEMID  and A.SITEID='" + Session["SiteCode"].ToString() + "' AND A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "'";

                DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert(' No Line Items Exists For This Invoice Number !! ');", true);
                }

            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
                     
        }

        

    }
}