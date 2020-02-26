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
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmVersionReleaseInfo : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataTable dt = new DataTable();
        CreamBell_DMS_WebApps.App_Code.Global baseobj = new Global();
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
                FillDdlList();
            }
        }

        private void FillDdlList()
        {
            string query = string.Empty;
            DataTable dt = new DataTable();

            query = @"SELECT VersionName ,VersionName +'-'+ Description VersionInfo FROM ACX_VW_GetVersionInfo Where IsConfirm='Yes' Order By VersionName";
            ddlVersionNew.Items.Clear();
            ddlVersionNew.Items.Add("Select...");
            baseobj.BindToDropDownp(ddlVersionNew, query, "VersionInfo", "VersionName");

            ddlVersionCodeNew.Items.Clear();
            query = @"SELECT 'Un-Assigned' AS VersionName UNION SELECT VersionName FROM ACX_VW_GetVersionInfo Order By VersionName";
            dt = baseobj.GetData(query);
            ddlVersionCodeNew.DataSource = dt;
            ddlVersionCodeNew.DataTextField = "VersionName";
            ddlVersionCodeNew.DataValueField = "VersionName";
            ddlVersionCodeNew.DataBind();
            //baseobj.BindToDropDown(ddlVersionCode, query, "VersionName", "VersionName");

            query = @"SELECT STATEID,NAME FROM AX.LOGISTICSADDRESSSTATE ORDER BY NAME";
            //baseobj.BindToDropDown(ddlState, query, "Name", "STATEID");
            ddlStateNew.Items.Clear();
            dt = baseobj.GetData(query);
            ddlStateNew.DataSource = dt;
            ddlStateNew.DataTextField = "Name";
            ddlStateNew.DataValueField = "STATEID";
            ddlStateNew.DataBind();


            query = @"SELECT 'VRS' AS NAME UNION SELECT 'PSR' AS NAME ORDER BY NAME";
            //baseobj.BindToDropDown(ddlUserType, query, "Name", "ID");
            ddlUserTypeNew.Items.Clear();
            dt = baseobj.GetData(query);
            ddlUserTypeNew.DataSource = dt;
            ddlUserTypeNew.DataTextField = "Name";
            ddlUserTypeNew.DataValueField = "Name";
            ddlUserTypeNew.DataBind();

            query = @"SELECT 'YES' AS NAME UNION SELECT 'NO' AS NAME ORDER BY NAME";
            ddlIsBlockNew.Items.Clear();
            dt = baseobj.GetData(query);
            ddlIsBlockNew.DataSource = dt;
            ddlIsBlockNew.DataTextField = "Name";
            ddlIsBlockNew.DataValueField = "Name";
            ddlIsBlockNew.DataBind();
            //baseobj.BindToDropDown(ddlIsBlock, query, "Name", "ID");
        }

        private void GridDetail()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string strSQL = string.Empty;

            strSQL = @"SELECT TOP 100 RECID,STATECODE,STATENAME,DISTRIBUTORCODE,DISTRIBUTORNAME,USERTYPE,USERCODE,USERNAME,VERSIONNAME,DESCRIPTION,";
            strSQL += "ISBLOCK FROM AX.ACX_VW_VERSIONRELEASE ORDER BY STATECODE,DISTRIBUTORCODE,USERCODE";

            adp1 = new SqlDataAdapter(strSQL, conn);
            dt.Clear();
            adp1.Fill(dt);

            gvVersionInfo.DataSource = dt;
            gvVersionInfo.DataBind();

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
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
            string Recid = string.Empty;
            string UserCode = string.Empty;
            string StateCode = string.Empty;

            GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            HiddenField hf = (HiddenField)gvrow.FindControl("hfRecId");
            LinkButton lnk = (LinkButton)gvrow.FindControl("lnkbtn");
            StateCode = gvVersionInfo.Rows[gvrow.RowIndex].Cells[1].Text;
            UserCode = gvVersionInfo.Rows[gvrow.RowIndex].Cells[6].Text;
            Recid = hf.Value.ToString();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_USP_VERSIONRELEASE_UPDATE";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RECID", Recid);
                cmd.Parameters.AddWithValue("@USERCODE", UserCode);
                cmd.Parameters.AddWithValue("@STATECODE", StateCode);
                cmd.Parameters.AddWithValue("@ISBLOCK", lnk.Text.ToUpper());
                cmd.Parameters.Add("@RetMsg", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
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

            string sqlCond = string.Empty;
            string strValues = string.Empty;
            SqlDataAdapter dap = null;
            DataTable dt = null;

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            gvVersionInfo.DataSource = null;
            gvVersionInfo.DataBind();

            sqlCond = "EXEC ACX_USP_GETVERSIONRELEASE ";
            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlStateNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues;

            if (txtDistributor.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'"+ txtDistributor.Text + "',";
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlUserTypeNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues;

            if (txtUserCode.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtUserCode.Text + "',";
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlVersionCodeNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues.Replace("Un-Assigned","");

            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlIsBlockNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "'"; }
            else { sqlCond = sqlCond + "'All'"; }
            sqlCond = sqlCond + strValues;

            dap = new SqlDataAdapter(sqlCond, conn);
            dt = new DataTable("dtTemp");
            dap.Fill(dt);
            gvVersionInfo.DataSource = null;
            gvVersionInfo.DataSource = dt;
            gvVersionInfo.DataBind();

            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }
        protected void chkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
        }
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e) //sale Header Grid View for Filling the Sale Line
        {
            GridViewRow gvrow = (GridViewRow)(((CheckBox)sender)).NamingContainer;
            CheckBox chkStatus = (CheckBox)gvrow.FindControl("chkStatus");
            LinkButton lnk = (LinkButton)gvrow.FindControl("lnkbtn");

            if (lnk.Text.ToUpper() == "YES" && chkStatus.Checked == true)
            {
                chkStatus.Checked = false;
                string message = " alert('Access Denied !!! User is Blocked!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return;

            }
        }
        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvVersionInfo.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                    LinkButton lnk = (LinkButton)row.Cells[0].FindControl("lnkbtn");
                    if (lnk.Text.ToUpper() == "YES" && chkRow.Checked)
                    {
                        chkRow.Checked = false;
                        string message = " alert('Access Denied !!! User is Blocked!');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        return;
                    }
                }
            }
        }
        protected void btnRelease_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();
            string message = string.Empty;
            CheckBox chkRow;
            LinkButton lnk;
            HiddenField hfRecid;
            string stateCode = string.Empty;
            string UserCode = string.Empty;
            string UserType = string.Empty;
            string DistributorCode = string.Empty;
            bool flag = false;

            try
            {
                if (ddlVersionNew.SelectedItem.Value.Trim().Length == 0 || ddlVersionNew.SelectedItem.Text == "Select...")
                {
                    message = " alert('Version Code is required!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    ddlVersionNew.Focus();
                    return;
                }
                foreach (GridViewRow row in gvVersionInfo.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                        lnk = (LinkButton)row.Cells[0].FindControl("lnkbtn");
                        hfRecid = (HiddenField)row.Cells[0].FindControl("hfRecId");
                        stateCode = gvVersionInfo.Rows[row.RowIndex].Cells[1].Text.ToString();
                        UserCode = gvVersionInfo.Rows[row.RowIndex].Cells[6].Text.ToString();
                        UserType = gvVersionInfo.Rows[row.RowIndex].Cells[5].Text.ToString();
                        DistributorCode = gvVersionInfo.Rows[row.RowIndex].Cells[3].Text.ToString();

                        if ((lnk.Text.ToUpper() == "NO" || lnk.Text.Trim().Length == 0) && chkRow.Checked)
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ACX_USP_VERSIONRELEASE_INSERT";
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@RECID", hfRecid.Value);
                                cmd.Parameters.AddWithValue("@VERSIONNAME", ddlVersionNew.SelectedItem.Value);
                                cmd.Parameters.AddWithValue("@USERTYPE", UserType);
                                cmd.Parameters.AddWithValue("@USERCODE", UserCode);
                                cmd.Parameters.AddWithValue("@DISTRIBUTORCODE", DistributorCode);
                                cmd.Parameters.AddWithValue("@STATECODE", stateCode);

                                cmd.Parameters.Add("@RetMsg", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();
                                message = " alert('" + cmd.Parameters["@RetMsg"].Value.ToString() + "');";
                                if (message.IndexOf("SUCCESS") < 0)
                                {
                                    throw new System.Exception();
                                }
                            }
                        }
                    }
                }
                flag = true;
                if (message.IndexOf("SUCCESS") >= 0)
                {
                    GridDetail();
                    ResetControls();
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return;
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                if (!flag)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    return;
                }
            }
        }
        private void ResetControls()
        {
            txtDistributor.Text = "";
            txtUserCode.Text = "";
            ddlStateNew.SelectedIndex = -1;
            ddlIsBlockNew.SelectedIndex = -1;
            ddlUserTypeNew.SelectedIndex = -1;
            ddlVersionCodeNew.SelectedIndex = -1;
        }
        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlVersionCode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlIsBlock_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            ShowData_ForExcel();
        }

        private void ShowData_ForExcel()
        {
            string sqlCond = string.Empty;
            string strValues = string.Empty;
            SqlDataAdapter dap = null;
            DataTable dt = null;

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            gvVersionInfo.DataSource = null;
            gvVersionInfo.DataBind();

            sqlCond = "EXEC ACX_USP_GETVERSIONRELEASE ";
            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlStateNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }
            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues;

            if (txtDistributor.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtDistributor.Text + "',";
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlUserTypeNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues;

            if (txtUserCode.Text.Trim().Length > 0)
            {
                sqlCond = sqlCond + "'" + txtUserCode.Text + "',";
            }
            else
            {
                sqlCond = sqlCond + "'',";
            }

            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlVersionCodeNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "',"; }
            else { sqlCond = sqlCond + "'All',"; }
            sqlCond = sqlCond + strValues.Replace("Un-Assigned", "");

            strValues = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlIsBlockNew.Items)
            {
                if (litem.Selected)
                {
                    strValues += "''" + litem.Value.ToString() + "'',";
                }
            }

            if (strValues.Trim().Length > 0) { strValues = "'" + strValues.Substring(0, strValues.Length - 1) + "'"; }
            else { sqlCond = sqlCond + "'All'"; }
            sqlCond = sqlCond + strValues;

            dap = new SqlDataAdapter(sqlCond, conn);
            string query = sqlCond;
            dt = obj.GetData(query);
            //dt = new DataTable("dtTemp");
            string attachment = "attachment; filename=VersionReleaseInfo.xls";
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
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

    }
}