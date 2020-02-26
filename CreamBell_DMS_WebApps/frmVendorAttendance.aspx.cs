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
    public partial class frmVendorAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                //txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
                CalendarExtender1.StartDate = DateTime.Now.AddDays(-2);
                CalendarExtender1.EndDate = DateTime.Now;
                


            }
        }
        private void ShowVdDetails()
        {

            
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtVdDetails = null;
            string query = string.Empty;
            try
            {

                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "[ax].[ACX_ACXVENDERATTENDANCE]";
                cmd.CommandText = query;
                // DateTime stDate = Convert.ToDateTime(txtMonth.Text);
                cmd.Parameters.AddWithValue("@SITEID", Session["USERID"].ToString());
                cmd.Parameters.AddWithValue("@DATE", Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyyy"));
                dtVdDetails = new DataTable();
                dtVdDetails.Load(cmd.ExecuteReader());

                if (dtVdDetails.Rows.Count > 0)
                {
                    gvDetails.DataSource = dtVdDetails;

                    gvDetails.DataBind();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            ShowVdDetails();
            LblMessage.Text = "";


        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string query = string.Empty;
            try {
                DataSet lds = new DataSet();

                DataTable ldt = new DataTable();
                DataColumn lCol = new DataColumn();

                ldt.Columns.Add("SiteID", typeof(string));
                ldt.Columns.Add("AttDate", typeof(DateTime));
                ldt.Columns.Add("VDCode", typeof(string));
                ldt.Columns.Add("AttStatus", typeof(string));

                ldt.Columns[1].DateTimeMode = DataSetDateTime.Utc;

                lds.Tables.Add(ldt);

                DataRow lRow;

                lRow = ldt.NewRow();


                foreach (GridViewRow grv in gvDetails.Rows)
                {
                    CheckBox Present = (CheckBox)grv.FindControl("chkStatus");

                    if (Present.Enabled == false)
                    {
                        continue;
                    }
                    string VDCode = grv.Cells[2].Text.ToString();
                    //  string VDCode = gvDetails.Rows.Cells[2].ToString();
                    string Status = string.Empty;

                    if (Present.Checked)
                    {
                        Status = "P";
                    }
                    else
                    {
                        Status = "A";
                    }

                    lRow["SiteID"] = Session["USERID"].ToString();
                    lRow["AttDate"] = Convert.ToDateTime(txtFromDate.Text);
                    lRow["VDCode"] = VDCode;
                    lRow["AttStatus"] = Status;

                    lds.Tables[0].Rows.Add(lRow);

                    lRow = ldt.NewRow();

                }

                string xmlData = lds.GetXml();

                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "[ax].[SP_ACX_ACXVENDERATTENDANCESave_New]";
                cmd.CommandText = query;
                // DateTime stDate = Convert.ToDateTime(txtMonth.Text);

                cmd.Parameters.AddWithValue("@XmlData", xmlData);

                cmd.ExecuteReader();
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► Data Has been Updated Successfully...!!! ";
                ShowVdDetails();
            }
            catch (Exception ex )
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtVdDetails = null;
            string query = string.Empty;
            try
            {

                foreach (GridViewRow grv in gvDetails.Rows)
                {
                    CheckBox Present = (CheckBox)grv.FindControl("chkStatus");

                    if (Present.Enabled == false)
                    {
                        continue;
                    }
                    string VDCode = grv.Cells[2].Text.ToString();
                    //  string VDCode = gvDetails.Rows.Cells[2].ToString();
                    string Status = string.Empty;

                    if (Present.Checked)
                    {
                        Status = "P";
                    }
                    else
                    {
                        Status = "A";
                    }

                    conn = new SqlConnection(objGlobal.GetConnectionString());
                    conn.Open();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    query = "[ax].[ACX_ACXVENDERATTENDANCESave]";
                    cmd.CommandText = query;
                    // DateTime stDate = Convert.ToDateTime(txtMonth.Text);

                    cmd.Parameters.AddWithValue("@SITEID", Session["USERID"].ToString());
                    cmd.Parameters.AddWithValue("@DATE", Convert.ToDateTime(txtFromDate.Text));
                    cmd.Parameters.AddWithValue("@VDCODE", VDCode);
                    cmd.Parameters.AddWithValue("@Status", Status);

                    dtVdDetails = new DataTable();
                    dtVdDetails.Load(cmd.ExecuteReader());
                    if (dtVdDetails.Rows.Count > 0)
                    {
                        gvDetails.DataSource = dtVdDetails;

                        gvDetails.DataBind();
                    }
                    else
                    {

                    }
                }
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► Data Has been Updated Successfully...!!! ";
            }
            catch (Exception ex)
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hiddenField = (HiddenField)e.Row.FindControl("HiddenField1");
                if (hiddenField != null)
                {
                    if (hiddenField.Value != "")
                    {
                        CheckBox chk = (CheckBox)e.Row.FindControl("chkStatus");
                        chk.Enabled = false;
                        //chk.Attributes.Add("Enabled", "false");
                        // txt.Attributes.Remove("readonly"); To remove readonly attribute
                    }
                }
            }
        }
    }
}