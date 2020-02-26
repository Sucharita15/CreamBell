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
    public partial class frmPSRBeatMaster : System.Web.UI.Page
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
                GetPSRBeat();
            }
        }

        private void GetPSRBeat()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();


               

                string query =  " Select A.PSRCode,C.PSR_Name as PSRName,A.BeatCode,A.BeatName,d.Discription as BeatDayDesc " +
                                " from AX.ACXPSRBEATMASTER A " +
                                " inner JOIN AX.ACXPSRSITELinkingMaster B ON A.PSRCode = B.PSRCode and A.Site_code = B.Site_Code  " +
                                " inner Join ax.[ACXPSRMaster] c on A.PSRCode = C.PSR_Code " +
                                " left Join ax.ACXWeekMaster d on d.DayCode=A.BeatDay " +
                                " where B.Site_Code='" + Session["SiteCode"].ToString() + "'" +
                                " order By C.PSR_Name,A.BeatName ";
                              

                DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }
            }
            catch (Exception ex){ ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            GetPSRBeat();
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != string.Empty)
            {
                SearchdataByFilter(DDLSearchFilter.Text.ToString(), txtSearch.Text);
            }
            else
            {
                GetPSRBeat();
            }
        }

        private void SearchdataByFilter(string searchFilter, string searchText)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string query = " Select A.PSRCode,C.PSR_Name as PSRName,A.BeatCode,A.BeatName,d.Discription as BeatDayDesc  from AX.ACXPSRBEATMASTER A  inner JOIN AX.ACXPSRSITELinkingMaster B" +
                           " ON A.PSRCode = B.PSRCode and A.Site_code = B.Site_Code   inner Join ax.[ACXPSRMaster] c on A.PSRCode = C.PSR_Code  left Join ax.ACXWeekMaster d on d.DayCode = A.BeatDay where A.Site_Code='" + Session["SiteCode"].ToString() + "'";

            if (DDLSearchFilter.Text == "PSR Name")
            {
                query += " and A.PSRCode=(select D.PSR_Code from [ax].[ACXPSRMaster] D where D.PSR_Code=A.PSRCode and D.PSR_Name like '%" + txtSearch.Text + "%')";
            }
            if (DDLSearchFilter.Text =="Beat Name")
            {
                query += " and A.BeatName like '%" + txtSearch.Text + "%'";
            }
            query += " order By PSRName,A.BeatName";

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

        protected void gvDetails_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            GetPSRBeat();
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

                string query = " Select A.PSRCode,C.PSR_Name as PSRName,A.BeatCode,A.BeatName,d.Discription as BeatDayDesc  from AX.ACXPSRBEATMASTER A  inner JOIN AX.ACXPSRSITELinkingMaster B" +
                          " ON A.PSRCode = B.PSRCode and A.Site_code = B.Site_Code   inner Join ax.[ACXPSRMaster] c on A.PSRCode = C.PSR_Code  left Join ax.ACXWeekMaster d on d.DayCode = A.BeatDay where A.Site_Code='" + Session["SiteCode"].ToString() + "'";

                if (DDLSearchFilter.Text == "PSR Name")
                {
                    query += " and A.PSRCode=(select D.PSR_Code from [ax].[ACXPSRMaster] D where D.PSR_Code=A.PSRCode and D.PSR_Name like '%" + txtSearch.Text + "%')";
                }
                if (DDLSearchFilter.Text == "Beat Name")
                {
                    query += " and A.BeatName like '%" + txtSearch.Text + "%'";
                }
                query += " order By PSRName,A.BeatName";

                DataTable dt = obj.GetData(query);

                //dt = new DataTable();
                //dt.Load(cmd.ExecuteReader());

                //=================Create Excel==========

                string attachment = "attachment; filename=PSRBeatMaster.xls";
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
            catch(Exception ex)
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