using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmUserLoginDetails : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                fillSiteAndState();
            }
        }

        protected void fillSiteAndState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY LS.Name ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();                
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'  ORDER BY LS.Name";
                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");               
            }
        }
        protected void rdoDistributor_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' Order By NAME";
                ddlSiteId.Items.Add("All");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order By NAME";                
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validate();
                if (b == true)
                {
                    //();

                    ExportToExcelNew();
                    //ExportDataTable(DataTable dtExport, string FileName, string ExportType, string HeaderText)
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool Validate()
        {
            bool b;
            b = true;
            if (ddlState.Text == string.Empty || ddlState.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide State.";
            }
          
            return b;
        }

        private void ExportToExcelNew()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;

            conn = new SqlConnection(obj.GetConnectionString());
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandTimeout = 3600;
            cmd.CommandType = CommandType.StoredProcedure;

            query = "[dbo].[ACX_GETUSERDETAILS]";

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@State", ddlState.SelectedItem.Value);
            if (ddlSiteId.SelectedItem.Value == "All")
            {
                cmd.Parameters.AddWithValue("@SITECODE", "");
            }
            else
            {
                cmd.Parameters.AddWithValue("@SITECODE", ddlSiteId.SelectedItem.Value);
            }
            if (rdoDistributor.Checked == true)
            {
                cmd.Parameters.AddWithValue("@USERTYPE", "DISTRIBUTOR");
            }
            else
            {
                cmd.Parameters.AddWithValue("@USERTYPE", "PSRVRS");
            }

            dtDataByfilter = new DataTable();
            dtDataByfilter.Load(cmd.ExecuteReader());
            DataTable dt = new DataTable();
            dt = dtDataByfilter;
               
            
            Global.ExportDataTable(dt, "UserLoginDetails.xls", "EXCEL", "UserLoginDetails");

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.Charset = "";
            //string FileName = "UserLoginDetails" + DateTime.Now + ".xls";
            //StringWriter strwritter = new StringWriter();
            //HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            //gridLoginDetails.GridLines = GridLines.Both;
            //gridLoginDetails.HeaderStyle.Font.Bold = true;
            //gridLoginDetails.RenderControl(htmltextwrtter);
            //Response.Write(strwritter.ToString());
            //Response.End();
        }
        protected void ShowData_ForExcel()
        {
            
            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            try
            {
                string query1 = "Select NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query1);

                SqlConnection conn = null;
                SqlCommand cmd = null;
                DataTable dtDataByfilter = null;
                string query = string.Empty;

                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "[dbo].[ACX_GETUSERDETAILS]";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@State", ddlState.SelectedItem.Value);
                if (ddlSiteId.SelectedItem.Value=="All")
                {
                    cmd.Parameters.AddWithValue("@SITECODE", "");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SITECODE", ddlSiteId.SelectedItem.Value);
                }
                if (rdoDistributor.Checked==true)
                {
                    cmd.Parameters.AddWithValue("@USERTYPE", "DISTRIBUTOR");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@USERTYPE", "PSRVRS");
                }
                                
                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
                DataTable dt = new DataTable();
                dt = dtDataByfilter;
                
                //=================Create Excel==========

                string attachment = "attachment; filename=UserLoginDetails.xls";
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
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);

                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        //string style = @" .text { mso-number-format:\@; }  ";
                        //Response.Write(style);
                        //Response.ContentType = "application/text";
                       
                        Response.Write(tab + dr[i].ToString());
                        Response.Write( dr[i].ToString());
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
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
          
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;

            conn = new SqlConnection(obj.GetConnectionString());
            conn.Open();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandTimeout = 3600;
            cmd.CommandType = CommandType.StoredProcedure;

            query = "[dbo].[ACX_GETUSERDETAILS]";

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@State", ddlState.SelectedItem.Value);
            if (ddlSiteId.SelectedItem.Value == "All")
            {
                cmd.Parameters.AddWithValue("@SITECODE", "");
            }
            else
            {
                cmd.Parameters.AddWithValue("@SITECODE", ddlSiteId.SelectedItem.Value);
            }
            if (rdoDistributor.Checked == true)
            {
                cmd.Parameters.AddWithValue("@USERTYPE", "DISTRIBUTOR");
            }
            else
            {
                cmd.Parameters.AddWithValue("@USERTYPE", "PSRVRS");
            }

            dtDataByfilter = new DataTable();
            dtDataByfilter.Load(cmd.ExecuteReader());
            DataTable dt = new DataTable();
            dt = dtDataByfilter;

            if (dt.Rows.Count > 0)
            {

                gridLoginDetails.DataSource = dt;
                gridLoginDetails.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
               
            }
            else
            {
                gridLoginDetails.DataSource = dt;
                gridLoginDetails.DataBind();

                LblMessage.Text = string.Empty;
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
    }
}