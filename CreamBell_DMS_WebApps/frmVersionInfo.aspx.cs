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
    public partial class frmVersionInfo : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataTable dt = new DataTable();

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                GridDetail();
            }
        }

        private void ResetControls()
        {
            txtVersion.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtDownloadLink.Text = string.Empty;
            txtVersion.Focus();
        }

        private void GridDetail()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            adp1 = new SqlDataAdapter("SELECT VersionCode,VersionName,IsConfirm,UrlAddress,Description FROM ACX_VW_GetVersionInfo ORDER BY VersionName", conn);
            dt.Clear();
            adp1.Fill(dt);

            gvVersionInfo.DataSource = dt;
            gvVersionInfo.DataBind();

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
            this.ResetControls();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string message = string.Empty;

            if (txtVersion.Text.ToString().Trim().Length == 0)
            {
                message = " alert('Version Code Type is required!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtVersion.Focus();
                return;
            }

            if (txtDescription.Text.ToString().Trim().Length == 0)
            {
                message = " alert('Description is required!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtDescription.Focus();
                return;
            } 

            if (txtDownloadLink.Text.ToString().Trim().Length == 0)
            {
                message = " alert('Download Link is required!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                txtDownloadLink.Focus();
                return;
            }

            using (SqlCommand cmd =new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_USP_VERSIONINFO_INSERT";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@VersionName", txtVersion.Text);
                cmd.Parameters.AddWithValue("@UrlAddress", txtDownloadLink.Text);
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                cmd.Parameters.Add("@RetMsg", SqlDbType.NVarChar,500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                message = " alert('" + cmd.Parameters["@RetMsg"].Value.ToString() + "')";
                if (message.IndexOf("SUCCESS") >= 0)
                {
                    GridDetail();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return;
            }
        }

        protected void gvVersionInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVersionInfo.PageIndex = e.NewPageIndex;
            GridDetail();            
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string message = string.Empty;
            string confirmCode = string.Empty;
            string versionName = string.Empty;
            string versionCode = string.Empty;

            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            HiddenField hf = (HiddenField)gvrow.FindControl("hfVersionCode");
            LinkButton lnk = (LinkButton)gvrow.FindControl("lnkbtn");
            confirmCode = lnk.Text;
            versionName = gvVersionInfo.Rows[gvrow.RowIndex].Cells[0].Text;
            versionCode = hf.Value.ToString();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_USP_VERSIONINFO_UPDATE";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@VersionCode", versionCode);
                cmd.Parameters.AddWithValue("@VersionName", versionName);
                cmd.Parameters.AddWithValue("@IsConfirm", (lnk.Text.ToUpper() == "YES" ? 0 : 1));
                cmd.Parameters.Add("@RetMsg", SqlDbType.NVarChar,500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                message = " alert('" + cmd.Parameters["@RetMsg"].Value.ToString() + "')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                if (message.IndexOf("SUCCESS") >= 0)
                {
                    GridDetail();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
                //GridView1.DataSource = null;
                //GridView1.DataBind();
   
                //CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                //conn = obj.GetConnection();

                //if (ddlSearch.Text == "Customer Group Code")
                //{
                //    adp1 = new SqlDataAdapter("SELECT CUSTGROUP_CODE,CUSTGROUP_NAME, CASE BLOCKED  WHEN 0 THEN 'Yes'   ELSE 'No' END as blocked FROM ax.ACXCUSTGROUPMASTER where  BLOCKED = 0 and custgroup_code like '%" + txtSerch.Text + "%' order by CUSTGROUP_CODE asc", conn);
                //}
                //else
                //{
                //    adp1 = new SqlDataAdapter("SELECT CUSTGROUP_CODE,CUSTGROUP_NAME, CASE BLOCKED  WHEN 0 THEN 'Yes'   ELSE 'No' END as blocked FROM ax.ACXCUSTGROUPMASTER where  BLOCKED = 0 and custgroup_name like '%" + txtSerch.Text + "%' order by CUSTGROUP_CODE asc", conn);
                //}
                //ds2.Clear();
                //adp1.Fill(ds2, "dtl");

                //if (ds2.Tables["dtl"].Rows.Count != 0)
                //{
                //    for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                //    {
                //        GridView1.DataSource = ds2.Tables["dtl"];
                //        GridView1.DataBind();
                //    }
                //}
                //else
                //{
                //    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Record not found!');", true);
                //}

                //if (conn.State == ConnectionState.Open)
                //{
                //    conn.Close();
                //    conn.Dispose();
                //}
        }

        protected void chkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;

        }
    }
}