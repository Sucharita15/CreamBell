using CreamBell_DMS_WebApps.App_Code;
using Elmah;
using System;
using System.Data;

namespace CreamBell_DMS_WebApps
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["USERID"] != null || Session["USERID"].ToString() != string.Empty)
            {
                if (!IsPostBack)
                {
                    LoadDashBoardValues();
                }
                 
            }
        }

        public void LoadDashBoardValues()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            try
            {
                string query = "Select CONVERT(VARCHAR(19), LastLoginDatetime, 121) AS LoginDate from ax.ACXUSERMASTER where User_Code='"+ Session["USERID"].ToString()+"'";

                LblUserName.Text = "Welcome : " + Session["USERNAME"].ToString();
                LblLastLoginTime.Text = "Last Login at : " + obj.GetScalarValue(query);
                LblSite.Text = "Site Location : " + Session["SITELOCATION"].ToString();

                //string query1 = "Select  count(*) from [ax].[ACXPURCHINDENTHEADER] as PURCHASEINDENT where SITEID='" + Session["SiteCode"].ToString()+ "' ";
                //string query2 = "Select  count(*) from [ax].[ACXPURCHINVRECIEPTHEADER] as PURCHASEINVOICE where SITE_CODE='" + Session["SiteCode"].ToString() + "' ";
                //string query3 = "Select count(*) from [ax].[ACXPURCHRETURNHEADER] as PURCHASERETURN where SITE_CODE='" + Session["SiteCode"].ToString() + "' ";
                //string query4 = "Select  count(*) from [ax].[ACXSALEINVOICEHEADER] as SALEINVOICE where SITEID='" + Session["SiteCode"].ToString() + "' ";                
                //string query5 = "Select Count(SO_NO) from ax.[ACXSALESHEADER] where LoadSheet_Status=0 and SITEID='" + Session["SiteCode"].ToString() + "'";
                //string query6 = "Select count(*) from ax.inventtable as TOTALPRODUCT ";
                //string query7 = "Select count(*) from ax.ACXCUSTGROUPMASTER as DISTRIBUTORGROUP ";
                //string query8 = " Select count(*) from ax.ACXCUSTMASTER as TOTALDISTRIBUTOR where SITE_CODE='" + Session["SiteCode"].ToString() + "' ";
                //LblPurchaseIndent.Text = obj.GetScalarValue(query1);
                //LblPurchaseInvoice.Text = obj.GetScalarValue(query2);
                //LblPurchaseReturn.Text = obj.GetScalarValue(query3);
                //LblSaleInvoice.Text = obj.GetScalarValue(query4);
                //LblSaleOrder.Text = obj.GetScalarValue(query5);
                //LblTotProduct.Text = obj.GetScalarValue(query6);
                //LblDistributorGroup.Text = obj.GetScalarValue(query7);
                //LblTotDistributor.Text = obj.GetScalarValue(query8);

                string query1 = "EXEC ACX_GETDashBoardDetails '" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count>0)
                {
                    LblPurchaseIndent.Text = dt.Rows[0]["Indent"].ToString();
                    LblPurchaseInvoice.Text = dt.Rows[0]["PurchReceipt"].ToString();
                    LblPurchaseReturn.Text = dt.Rows[0]["PurchReturn"].ToString();
                    LblSaleInvoice.Text = dt.Rows[0]["SO"].ToString();
                    LblSaleOrder.Text = dt.Rows[0]["LS"].ToString();
                    LblTotProduct.Text = dt.Rows[0]["TotalProduct"].ToString();
                    LblDistributorGroup.Text = dt.Rows[0]["DistGroup"].ToString();
                    LblTotDistributor.Text = dt.Rows[0]["TotalDist"].ToString();
                }               
            }
            catch(Exception ex)
            {
                //logger.Error("Error in Home : {0}", ex);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }                        
        }
    }
}