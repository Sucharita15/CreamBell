using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CreamBell_DMS_WebApps.App_Code;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmUseNouseReport  : System.Web.UI.Page
    {


        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {

                fillState();

                ddlstate.Focus();
                gvDetails.Visible = true;
                CalendarExtender1.EndDate = DateTime.Now.Date;
                CalendarExtender2.EndDate = DateTime.Now.Date;

            }
        }

        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string query = string.Empty;
            bool endRequest = false;

            try
            {
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;

                if (rdWorking.Checked)
                {
                    query = "GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "','" + rdWorking.Text + "'";
                }
                else if (rdNotWorking.Checked)
                {
                    query = "GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "','" + rdNotWorking.Text + "'";
                }
                else
                {
                    query = "GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "'";
                }

                cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
                //cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                //=================Create Excel==========

                //var workbook = new ExcelFile();

                //// Add new worksheet to Excel file.
                //var worksheet = workbook.Worksheets.Add("New worksheet");

                //// Set the value of the cell "A1".
                //worksheet.Cells["A1"].Value = "Hello world!";

                //// Save Excel file.
                //workbook.Save("Workbook.xls");
                string attachment = "attachment; filename=Usenousereport.xls";
                Response.ClearContent();
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", attachment);
                
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

        protected void ImDnldTemp_Click(object sender, ImageClickEventArgs e)
        {
            if (txtFromDate.Text == "")
            {
                string message = "alert('From Date cannot be empty');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }
            if (txtToDate.Text == "")
            {
                string message = "alert('To Date cannot be empty');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }

            int result = DateTime.Compare(Convert.ToDateTime(txtFromDate.Text), DateTime.Now.Date);
            if (result != 0 && result > 0)
            {
                string message = "alert('From Date cannot be greater than current date');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }

            int result1 = DateTime.Compare(Convert.ToDateTime(txtToDate.Text), DateTime.Now.Date);
            if (result1 != 0 && result1 > 0)
            {
                string message = "alert('To Date cannot be greater than current date');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }

            int result2 = DateTime.Compare(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
            if (result2 != 0 && result2 > 0)
            {
                string message = "alert('From Date cannot be greater than To Date');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }
            ShowData_ForExcel();
        }

        public void fillState()
        {
            string strQuery;
            strQuery = "Select STATEID+'-'+NAME as Name,STATEID from ax.LOGISTICSADDRESSSTATE where stateid not in ('All','IN') and isnull(stateid,'')<>'' ";
            ddlstate.Items.Clear();
            ddlstate.Items.Add("Select...");
            baseObj.BindToDropDown(ddlstate, strQuery, "Name", "STATEID");
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Text == "")
            {
                string message = "alert('From Date cannot be empty');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }
            if (txtToDate.Text == "")
            {
                string message = "alert('To Date cannot be empty');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }
            
            int result = DateTime.Compare(Convert.ToDateTime(txtFromDate.Text), DateTime.Now.Date);
            if (result != 0 && result > 0)
            {
                string message = "alert('From Date cannot be greater than current date');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }

            int result1 = DateTime.Compare(Convert.ToDateTime(txtToDate.Text), DateTime.Now.Date);
            if (result1 != 0 && result1 > 0)
            {
                string message = "alert('To Date cannot be greater than current date');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }

            int result2 = DateTime.Compare(Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
            if (result2 != 0 && result2 > 0)
            {
                string message = "alert('From Date cannot be greater than To Date');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", message, true);
                return;
            }

            SqlConnection con= baseObj.GetConnection();
            string query = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.CommandTimeout = 0;
            if (rdWorking.Checked)
            {
                query = "GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "','" + rdWorking.Text + "'";
            }
            else if (rdNotWorking.Checked)
            {
                query = "GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "','" + rdNotWorking.Text + "'";
            }
            else
            {
                query = "GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "'";
            }

            cmd.CommandText = query;
            //cmd.Parameters.AddWithValue("@SITEID", Session["SiteCode"].ToString());
            //cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
            DataTable dt = new DataTable();
            dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            gvDetails.DataSource = dt;
            gvDetails.DataBind();
            gvDetails.Visible = true;

            //cmd.CommandText = "exec GetDataForUseNonuseReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + ddlstate.SelectedValue + "'";
            //DataTable dt = new DataTable();
            // dt.Load(cmd.ExecuteReader());
            //if (rdWorking.Checked == true)
            //{
            //    gvDetails.DataSource = dt.Select("[status]='Working'").CopyToDataTable();
            //}
            //else if (rdNotWorking.Checked == true)
            //{
            //    gvDetails.DataSource = dt.Select("[status]='Not Working'").CopyToDataTable();
            //}
            //else
            //{
            //    gvDetails.DataSource = dt;
            //}

            //gvDetails.DataBind();
            //gvDetails.Visible = true;


        }
    }
}