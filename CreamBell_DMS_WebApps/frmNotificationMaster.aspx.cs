using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps
{
    public partial class frmNotificationMaster : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlConnection conn = null;
        SqlCommand cmd = null;
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
                ShowNotifications();
            }
        }
        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlState_SelectedIndexChanged(null, null);
        }

        protected void fillState()
        {
            ddlState.Items.Clear();
            string sqlstr = "";
            sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
            ddlState.Items.Add("ALL");
            baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
            ddlState_SelectedIndexChanged(null, null);
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string usertype = string.Empty;
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                usertype = ddlUserType.SelectedItem.Value.ToString();
                DataTable dtDataByfilter = null;
                string query = string.Empty;
                try
                {
                    ddlUsers.Items.Clear();
                    conn = new SqlConnection(baseObj.GetConnectionString());
                    conn.Open();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;
                    query = "USP_FILLUSERSINDROPDOWN";

                    cmd.CommandText = query;
                    if(ddlState.SelectedItem.Text == "ALL")
                    {
                        cmd.Parameters.AddWithValue("@STATECODE", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@STATECODE", ddlState.SelectedItem.Value);
                    }
                    cmd.Parameters.AddWithValue("@USERTYPE", usertype);
                    dtDataByfilter = new DataTable();
                    dtDataByfilter.Load(cmd.ExecuteReader());
                    if (dtDataByfilter.Rows.Count > 0)
                    {
                        ddlUsers.DataSource = dtDataByfilter;
                        ddlUsers.DataTextField = "Name";
                        ddlUsers.DataValueField = "Code";
                        ddlUsers.DataBind();
                    }
                    ddlUsers.Items.Insert(0, "ALL");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private bool ValidateInput()
        {
            bool b;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                lblMessage.Text = "Please Provide FromDate and ToDate";
                lblMessage.Visible = true;
            }
            else if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                b = false;
                lblMessage.Text = "'From Date' cannot be greater than 'To Date'";
                lblMessage.Visible = true;
            }
            else if (ddlDisplayOn.SelectedItem.Text == "--Select--")
            {
                b = false;
                lblMessage.Text = "Please Select Display On";
                lblMessage.Visible = true;
            }
            else if (txtMessage.Text == "")
            {
                b = false;
                lblMessage.Text = "Message Cannot be Empty";
                lblMessage.Visible = true;
            }
            else
            {
                b = true;
                lblMessage.Text = string.Empty;
                lblMessage.Visible = false;
            }
            return b;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    SaveData();
                    uppanel.Update();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
            }
        }

        private void SaveData()
        {
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(baseObj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "USP_INSERTNOTIFICATION";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@FROMDATE", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@TODATE", Convert.ToDateTime(txtToDate.Text));
                cmd.Parameters.AddWithValue("@STATE", ddlState.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@USERTYPE", ddlUserType.SelectedItem.Text);
                cmd.Parameters.AddWithValue("@USER", ddlUsers.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@MESSAGE", txtMessage.Text.ToString());
                cmd.Parameters.AddWithValue("@DISPLAYON", ddlDisplayOn.SelectedItem.Value);
                cmd.ExecuteNonQuery();
                ResetData();
                ShowNotifications();                
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
            }
        }
        
        private void ResetData()
        {
            ddlDisplayOn.SelectedIndex = 0;
            txtMessage.Text = string.Empty;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            fillState();
            ddlUserType.SelectedIndex = 0;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        protected void lnkbtnstatus_Click(object sender, EventArgs e)
        {
            string query = string.Empty;
            conn = baseObj.GetConnection();
            cmd = new SqlCommand();
            cmd.Connection = conn;

            LinkButton btn = sender as LinkButton;
            GridViewRow row = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            string recid = ((HiddenField)row.FindControl("hndrecid")).Value.ToString();
            try
            {
                if (btn.Text == "ACTIVE")
                {
                    query = "update AX.ACXNOTIFICATIONMASTER set ISACTIVE=0 WHERE RECID='" + recid + "'";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    ShowNotifications();
                }
                else
                {
                    query = "update AX.ACXNOTIFICATIONMASTER set ISACTIVE=1 WHERE RECID='" + recid + "'";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    ShowNotifications();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
            }
        }
        public void ShowNotifications()
        {
            string query = string.Empty;
            DataTable data = new DataTable();
            try
            {
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                conn = new SqlConnection(baseObj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "USP_FETCHALLNOTIFICATIONS";
                cmd.CommandText = query;
                data.Load(cmd.ExecuteReader());
                gvDetails.DataSource = data;
                gvDetails.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "► " + ex.Message.ToString();
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
            }
        }
    }
}