using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPendingPurchaseReciept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["SiteCode"] != null)
                {
                    BindGridview("");
                }
            }
        }
        protected void BindGridview(string param)
        {
            try
            {
                string query;

                query = @"Select A.INVOICE_NO,A.INVOIC_DATE,A.INVOICE_VALUE,A.SITEID as DistCode,DistName = (select Name from [ax].[INVENTSITE] where SITEID =  A.SITEID)
                        from [ax].[ACXSALEINVOICEHEADER] A left Join  [ax].[ACXPURCHINVRECIEPTHEADER] B on B.Site_Code=A.Customer_Code 
                        and B.Supplier_Code=A.SiteID and B.Sale_InvoiceNo=A.Invoice_NO inner join [ax].[ACXUSERMASTER] C on C.Site_Code = A.CUSTOMER_CODE
                        Where  B.Sale_InvoiceNo is null and A.Trantype<>2 and  A.INVOIC_DATE >= C.PurchaseStartDate  and 
                        (SELECT ISNULL(SUM(BOX),0) FROM [ax].[ACXSALEINVOICELINE] SL WHERE SL.SITEID=A.SITEID AND SL.INVOICE_NO=A.INVOICE_NO)
	                    >(SELECT ISNULL(SUM(BOX),0) FROM [ax].[ACXSALEINVOICELINE] SL JOIN AX.ACXSALEINVOICEHEADER SH ON SH.SITEID=SL.SITEID 
                        AND SH.INVOICE_NO=SL.INVOICE_NO AND SL.SITEID=A.SITEID AND SH.SO_NO=A.INVOICE_NO AND SL.TRANTYPE=2)and A.Customer_Code='" + Session["SiteCode"].ToString() + param + "' Order by A.SITEID,A.INVOICE_NO ";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvHeader.DataSource = dt;
                    gvHeader.DataBind();
                }
                else
                {
                    gvHeader.DataSource = null;
                    gvHeader.DataBind();
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
                LinkButton lnkbtn = (LinkButton)sender as LinkButton;
                GridViewRow row = (GridViewRow)lnkbtn.Parent.Parent;
                string lblDistCode = row.Cells[3].Text;
                Response.Redirect("frmSDPurchaseInvoiceReceipt.aspx?ID=" + lnkbtn.Text + "&Dist="+ lblDistCode, false);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        protected void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                if (DDLSearchType.SelectedIndex == 0)
                {
                    string param = "and A.INVOICE_NO = '" + txtSearch.Text + "'";
                    BindGridview(param);
                }
                if (DDLSearchType.SelectedIndex == 1)
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(txtSearch.Text);
                        string param = "and A.INVOIC_DATE = '" + dt + "'";
                        BindGridview(param);
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Date is not valid..";
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }

                }
            }
            else
            {
                BindGridview("");
            }
        }

    }
}