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
    public partial class frmPSRCustomerMaster : System.Web.UI.Page
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
                GetPSRCustomer();
            }
        }
        private void GetPSRCustomer()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                string query = " Select A.CustomerCode,A.CustomerName ," +
                              " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode )," +
                               " A.BeatCode,B.BeatName,B.BeatDay,BeatDayDesc=(Select F.Discription from ax.ACXWeekMaster F where F.DayCode=B.BeatDay)  " +
                               " from [ax].[ACXPSRCUSTLinkingMaster] A   " +
                               " Left Join [ax].[ACXPSRBeatMaster] B on A.BeatCode=B.BeatCode and A.PSRCode=b.PSRCode AND A.SITE_CODE = B.SITE_CODE " +
                               " Left Join [ax].[ACXCUSTMASTER] C on A.CustomerCode=C.Customer_Code  AND A.SITE_CODE = C.SITE_CODE " +
                               " where A.Site_Code='" + Session["SiteCode"].ToString() + "' and C.Blocked = 0 order by PSRName,BeatName,A.CustomerName";

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
           
        }

        protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            GetPSRCustomer();
        }
        private void SearchdataByFilter(string searchFilter, string searchText)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            //string query = " Select A.CustomerCode,A.CustomerName ," +
            //                   " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode )," +
            //                    " A.BeatCode,BeatName=(select B.BeatName from [ax].[ACXPSRBeatMaster] B where A.BeatCode=B.BeatCode) " +
            //                    " from [ax].[ACXPSRCUSTLinkingMaster] A " +
            //                    " where A.Site_Code='" + Session["SiteCode"].ToString() + "' ";

            string query = " Select A.CustomerCode,A.CustomerName ," +
                             " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode )," +
                              " A.BeatCode,B.BeatName,B.BeatDay,BeatDayDesc=(Select F.Discription from ax.ACXWeekMaster F where F.DayCode=B.BeatDay)  " +
                              " from [ax].[ACXPSRCUSTLinkingMaster] A   " +
                              " Left Join [ax].[ACXPSRBeatMaster] B on A.BeatCode=B.BeatCode and A.PSRCode=b.PSRCode AND A.SITE_CODE = B.SITE_CODE " +
                              " Left Join [ax].[ACXCUSTMASTER] C on A.CustomerCode=C.Customer_Code  AND A.SITE_CODE = C.SITE_CODE " +
                              " where A.Site_Code='" + Session["SiteCode"].ToString() + "' and C.Blocked=0 ";

            if (DDLSearchFilter.Text == "Retailer Name")
            {
                query += " and A.CustomerName like '%" + txtSearch.Text + "%'";
            }
            if (DDLSearchFilter.Text == "PSR Name")
            {
                query += " and A.PSRCode=(select D.PSR_Code from [ax].[ACXPSRMaster] D where D.PSR_Code=A.PSRCode and D.PSR_Name like '%" + txtSearch.Text + "%')";
            }
            if (DDLSearchFilter.Text == "Beat Name")
            {
                query += " and B.BeatName like '%" + txtSearch.Text + "%' ";//" and A.BeatCode=(select E.BeatCode from [ax].[ACXPSRBeatMaster] E where A.BeatCode=E.BeatCode and E.BeatName like '%" + txtSearch.Text + "%')";
            }
            query += " order by PSRName,BeatName,A.CustomerName";

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

        protected void BtnSearch_Click1(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                SearchdataByFilter(DDLSearchFilter.Text.ToString(), txtSearch.Text);
            }
            else
            {
                GetPSRCustomer();
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
            string query = string.Empty;

            try
            {
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.Text;

                query = " Select A.CustomerCode,A.CustomerName ," +
                             " A.PSRCode,PSRName=(select C.PSR_Name from [ax].[ACXPSRMaster] C where C.PSR_Code=A.PSRCode )," +
                              " A.BeatCode,B.BeatName,B.BeatDay,BeatDayDesc=(Select F.Discription from ax.ACXWeekMaster F where F.DayCode=B.BeatDay)  " +
                              " from [ax].[ACXPSRCUSTLinkingMaster] A   " +
                              " Left Join [ax].[ACXPSRBeatMaster] B on A.BeatCode=B.BeatCode and A.PSRCode=b.PSRCode AND A.SITE_CODE = B.SITE_CODE " +
                              " Left Join [ax].[ACXCUSTMASTER] C on A.CustomerCode=C.Customer_Code  AND A.SITE_CODE = C.SITE_CODE " +
                              " where A.Site_Code='" + Session["SiteCode"].ToString() + "' and C.Blocked=0 ";

                if (DDLSearchFilter.Text == "Retailer Name")
                {
                    query += " and A.CustomerName like '%" + txtSearch.Text + "%'";
                }
                if (DDLSearchFilter.Text == "PSR Name")
                {
                    query += " and A.PSRCode=(select D.PSR_Code from [ax].[ACXPSRMaster] D where D.PSR_Code=A.PSRCode and D.PSR_Name like '%" + txtSearch.Text + "%')";
                }
                if (DDLSearchFilter.Text == "Beat Name")
                {
                    query += " and B.BeatName like '%" + txtSearch.Text + "%' ";//" and A.BeatCode=(select E.BeatCode from [ax].[ACXPSRBeatMaster] E where A.BeatCode=E.BeatCode and E.BeatName like '%" + txtSearch.Text + "%')";
                }
                query += " order by PSRName,BeatName,A.CustomerName";

                DataTable dt = obj.GetData(query);
               
                //dt = new DataTable();
                //dt.Load(cmd.ExecuteReader());

                //=================Create Excel==========

                string attachment = "attachment; filename=PSRCustomerMaster.xls";
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