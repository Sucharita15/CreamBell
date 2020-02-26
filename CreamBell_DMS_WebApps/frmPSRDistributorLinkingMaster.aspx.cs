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
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPSRDistributorLinkingMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                GetPSRDistributorLinking();
            }

        }
        private void GetPSRDistributorLinking()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();


                string query = " Select A.Site_Code,Site_Name=(select B.Name from [ax].[INVENTSITE] B where B.SITEID=A.Site_Code)," +
                               " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode)," +
                               " CONVERT(VARCHAR(11),A.[FromDate],106) as [FromDate],CONVERT(VARCHAR(11),A.[TODate],106) as [TODate]" +
                               " from [ax].[ACXPSRSITELinkingMaster] A where A.Site_Code='" + Session["SiteCode"].ToString() + "' and A.Blocked=0 Order By PSRName,A.[FromDate] Desc";

                DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Provide the " + DDLSearchFilter.Text.ToString() + "  !');", true);
                GetPSRDistributorLinking();
            }
        }

        protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            GetPSRDistributorLinking();
           
        }
        private void SearchdataByFilter(string searchFilter, string searchText)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

           
            string query= " Select A.Site_Code,Site_Name=(select B.Name from [ax].[INVENTSITE] B where B.SITEID=A.Site_Code)," +
                               " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode)," +
                               " CONVERT(VARCHAR(11),A.[FromDate],106) as [FromDate],CONVERT(VARCHAR(11),A.[TODate],106) as [TODate]" +
                               " from [ax].[ACXPSRSITELinkingMaster] A where A.Site_Code='" + Session["SiteCode"].ToString() + "' and A.Blocked=0 ";

            if (DDLSearchFilter.Text == "PSR Name")
            {
                query += " and A.PSRCode=(select D.PSR_Code from [ax].[ACXPSRMaster] D where D.PSR_Code=A.PSRCode and D.PSR_Name like '%"+ txtSearch.Text +"%')";
            }
            query+="Order By PSRName,A.[FromDate] Desc";
           
            DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                gvDetails.DataSource = dt;
                gvDetails.DataBind();
                
            }
            else
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Found !');", true);
               
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            BtnSearch.Focus();
        }
        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            ShowData_ForExcel();
        }

        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string FilterQuery = string.Empty;
            SqlConnection conn = null;
            SqlCommand cmd = null;
        
            try
            {
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

         
                string query = " Select A.Site_Code,Site_Name=(select B.Name from [ax].[INVENTSITE] B where B.SITEID=A.Site_Code)," +
                               " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode)," +
                               " CONVERT(VARCHAR(11),A.[FromDate],106) as [FromDate],CONVERT(VARCHAR(11),A.[TODate],106) as [TODate]" +
                               " from [ax].[ACXPSRSITELinkingMaster] A where A.Site_Code='" + Session["SiteCode"].ToString() + "' and A.Blocked=0 ";

                if (DDLSearchFilter.Text == "PSR Name")
                {
                    query += " and A.PSRCode=(select D.PSR_Code from [ax].[ACXPSRMaster] D where D.PSR_Code=A.PSRCode and D.PSR_Name like '%" + txtSearch.Text + "%')";
                }
                query += "Order By PSRName,A.[FromDate] Desc";

                DataTable dt = obj.GetData(query);

                //dt = new DataTable();
                //dt.Load(cmd.ExecuteReader());

                //=================Create Excel==========

                string attachment = "attachment; filename=PSRDistributorLinkingMaster.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

    }
}