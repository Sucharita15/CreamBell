using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using AjaxControlToolkit;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPurchaseReturnList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null )
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadPurchaseReturnHeaderList();
            }
        }

        private void LoadPurchaseReturnHeaderList()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //string query= " Select A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO, SUM(B.AMOUNT) as TOTAMOUNT " +
            //              " from [ax].[ACXPURCHRETURNHEADER] A INNER JOIN [ax].[ACXPURCHRETURNLINE] B ON A.PURCH_RETURNNO=B.PURCH_RETURNNO GROUP BY " +
            //              " A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO ";

            string query = "Select A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO, SUM(B.AMOUNT) as TOTAMOUNT " +
                           "from [ax].[ACXPURCHRETURNHEADER] A INNER JOIN [ax].[ACXPURCHRETURNLINE] B ON A.PURCH_RETURNNO=B.PURCH_RETURNNO  AND A.SITE_CODE=B.SITEID " +
                           "Where A.SITE_CODE='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' " +
                           " GROUP BY A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO Order By A.PURCH_RETURNDATE desc";

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                GridHeaderDetail.DataSource = dt;
                GridHeaderDetail.DataBind();
                //GridHeaderDetail.Columns[5].Visible = false;
            }
        }

        protected void LnkReturnNo_Click(object sender, EventArgs e)
        {
            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            LinkButton lnk = sender as LinkButton;

            ShowReturnLineItem(lnk.Text);
        }

        private void ShowReturnLineItem(string PurcReturnNo)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string query = " Select distinct A.PURCH_RETURNNO,B.PRODUCT_GROUP, A.PRODUCT_CODE,B.PRODUCT_NAME, B.UOM, A.CRATES,A.BOX,A.LTR " +
                           " from [ax].[ACXPURCHRETURNLINE] A INNER JOIN ax.inventtable B ON A.PRODUCT_CODE= B.ITEMID " +
                           " where A.PURCH_RETURNNO='" + PurcReturnNo + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' and A.SITEID='" + Session["SiteCode"].ToString() + "'";

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gridLineDetails.Visible = true;
                gridLineDetails.DataSource = dt;
                gridLineDetails.DataBind();
               

                decimal Box = dt.AsEnumerable().Sum(row => row.Field<decimal>("BOX"));                   //For Total[Sum] Box Show in Footer--//
                gridLineDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                gridLineDetails.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
                gridLineDetails.FooterRow.Cells[6].Text = "Tot:" + Box.ToString("N2");
                gridLineDetails.FooterRow.Cells[6].Font.Bold = true;

                decimal Crate = dt.AsEnumerable().Sum(row => row.Field<decimal>("CRATES"));          //For Total[Sum] Show in Footer--//
                gridLineDetails.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                gridLineDetails.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
                gridLineDetails.FooterRow.Cells[5].Text = "Tot:" + Crate.ToString("N2");
                gridLineDetails.FooterRow.Cells[5].Font.Bold = true;

                decimal Litre = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTR"));          //For Total[Sum] Litre Show in Footer--//
                gridLineDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                gridLineDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
                gridLineDetails.FooterRow.Cells[7].Text = "Tot:" + Litre.ToString("N2");
                gridLineDetails.FooterRow.Cells[7].Font.Bold = true;
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Line Item Exists !');", true);
                gridLineDetails.DataSource = null;
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                SearchdataByFilter(DDLSearchFilter.Text.ToString(), txtSearch.Text);
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide the " + DDLSearchFilter.Text.ToString() + "  !');", true);
            }
        }

        //GetPurchaseOrder(Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy"), Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyyy"));
        private void SearchdataByFilter(string searchFilter, string searchText)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string query = " Select A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO, SUM(B.AMOUNT) as TOTAMOUNT " +
                          " from [ax].[ACXPURCHRETURNHEADER] A INNER JOIN [ax].[ACXPURCHRETURNLINE] B ON A.PURCH_RETURNNO=B.PURCH_RETURNNO ";

            if (DDLSearchFilter.Text == "Receipt Number")
            {
                query = query + " where A.PURCH_RECIEPTNO ='" + searchText + "' and A.SITE_CODE='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' GROUP BY A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO";
            }
            if (DDLSearchFilter.Text == "Receipt Date")
            {
                query = query + " where A.PURCH_RECIEPTDATE ='" + searchText + "' and A.SITE_CODE='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' GROUP BY A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO";
            }
            if (DDLSearchFilter.Text == "Return Number")
            {
                query = query + " where A.PURCH_RETURNNO ='" + searchText + "' and A.SITE_CODE='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' GROUP BY A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO";
            }
            if (DDLSearchFilter.Text == "Return Date")
            {
                query = query + " where A.PURCH_RETURNDATE ='" + searchText + "' and A.SITE_CODE='" + Session["SiteCode"].ToString() + "' and A.DATAAREAID='" + Session["DATAAREAID"].ToString() + "' GROUP BY A.PURCH_RETURNNO,A.PURCH_RETURNDATE, A.PURCH_RECIEPTNO, A.PURCH_RECIEPTDATE, A.SALE_INVOICENO";
            }

            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                GridHeaderDetail.DataSource = dt;
                GridHeaderDetail.DataBind();
                gridLineDetails.Visible = true;
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Found !');", true);
                LoadPurchaseReturnHeaderList();
                gridLineDetails.Visible = false;
            }
        }

        protected void GridHeaderDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridHeaderDetail.PageIndex = e.NewPageIndex;
            LoadPurchaseReturnHeaderList();
            gridLineDetails.Visible = false;

        }

        
    }
}