using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;


namespace CreamBell_DMS_WebApps
{
    public partial class frmSalesOrderPreList : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                string strQuery = " SELECT A.SO_NO,CONVERT(NVARCHAR, A.SO_DATE,106) AS SO_DATE,CONVERT(NVARCHAR, A.DELIVERY_DATE,106) AS DELIVERY_DATE, " +
                                " A.CUSTOMER_CODE+' / '+C.CUSTOMER_NAME AS CUSTOMER_CODE, " +
                                " ROUND(SUM(B.AMOUNT),2) AS AMOUNT FROM [ax].[ACXSALESHEADERPRE] A " +
                                " INNER JOIN [ax].[ACXSALESLINEPRE] B ON A.SO_NO=B.SO_NO AND A.SITEID = B.SITEID " +
                                " left Join ax.ACXCUSTMASTER c on A.CUSTOMER_CODE = C.CUSTOMER_CODE "+//and A.SITEID = C.SITE_CODE " +
                                " WHERE A.SITEID='" + Session["SiteCode"].ToString() + "' " +
                                " GROUP BY A.SO_NO,A.SO_DATE,A.DELIVERY_DATE,A.CUSTOMER_CODE,C.CUSTOMER_NAME ";
                SqlCommand cmd = new SqlCommand(strQuery,conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GvSaleOrderPre.DataSource = ds;
                    GvSaleOrderPre.DataBind();
                }

                conn.Close();
                conn.Dispose();
            }
        }
        protected void lnkSONo_Click(object sender, EventArgs e)
        {
            string SONO = (sender as LinkButton).CommandArgument;
            //             Response.Redirect("frmCustomerPartyMasterNew.aspx?CustID=" + CustID);
            Response.Redirect(String.Format("frmSaleOrder.aspx?SONO="+ SONO));
        }
    }
}