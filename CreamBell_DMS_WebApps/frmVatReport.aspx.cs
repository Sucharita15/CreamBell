using CreamBell_DMS_WebApps.App_Code;
using Elmah;
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
    public partial class frmVatReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        public string strSiteName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                FillState();
                //chkCustomerName.Items.Clear();
                // chkCustomerName.Items.Add("--Select--");
            }
        }
        protected void FillState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            DataTable dt = new DataTable();

            dt = new DataTable();
            if (objcheckSitecode != null)
            {
                chkListState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                dt = baseObj.GetData(sqlstr11);
                chkListState.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "NAME";
                    chkListState.DataValueField = "Code";
                    chkListState.DataBind();
                }
            }
            else
            {
                chkListState.Items.Clear();
                chkListSite.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "StateName";
                    chkListState.DataValueField = "StateCode";
                    chkListState.DataBind();

                    chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }
                chkListState.Items[0].Selected = true;
                chkListSite.Items[0].Selected = true;
                // FillCustomerGroup();
            }
        }
        protected void FillSite()
        {
            string StateList = "";
            strSiteName = "";
            foreach (ListItem item in chkListState.Items)
            {
                if (item.Selected)
                {
                    if (StateList == "")
                    {
                        StateList += "'" + item.Value.ToString() + "'";
                        strSiteName += "'" + item.Text.ToString() + "'";
                    }
                    else
                    {
                        StateList += ",'" + item.Value.ToString() + "'";
                        strSiteName += "'" + item.Text.ToString() + "'";
                    }
                }
            }
            if (StateList.Length > 0)
            {
                DataTable dt = new DataTable();
                string sqlstr1 = string.Empty;
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                }
                else
                {
                    sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                }

                dt = new DataTable();
                // dt = baseObj.GetData(sqlstr1);
                chkListSite.Items.Clear();
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }

            }
            else
            {
                chkListSite.Items.Clear();
            }
        }
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }
        private bool Validate()
        {
            bool b;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide From Date and To Date";
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }


            return b;
        }
        private DataTable GetData(string usp_SaleTaxReport, string strReportName, CreamBell_DMS_WebApps.App_Code.Global obj, ref DataTable dtSetHeader)
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
            query = usp_SaleTaxReport;
            cmd.CommandText = query;
            DateTime now = Convert.ToDateTime(txtToDate.Text);
            now = now.AddMonths(1);

            cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
            cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));
            string SiteList = "";
            foreach (ListItem item in chkListSite.Items)
            {
                if (item.Selected)
                {
                    if (SiteList == "")
                    {
                        SiteList += "" + item.Value.ToString() + "";
                    }
                    else
                    {
                        SiteList += "," + item.Value.ToString() + "";
                    }
                }
            }
            if (SiteList.Length > 0)
            {
                cmd.Parameters.AddWithValue("@SITEID", SiteList);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SITEID", "");
            }

            string strStateCode = "";
            foreach (ListItem item in chkListState.Items)
            {
                if (item.Selected)
                {
                    if (strStateCode == "")
                    {
                        strStateCode += "" + item.Value.ToString() + "";
                    }
                    else
                    {
                        strStateCode += "," + item.Value.ToString() + "";
                    }
                }
            }
            if (strStateCode.Length > 0)
            {
                cmd.Parameters.AddWithValue("@STATECODE", strStateCode);
            }
            else
            {
                cmd.Parameters.AddWithValue("@STATECODE", "");
            }
            string strRetailerType = "";

            foreach (ListItem item in chkCustomerType.Items)
            {
                if (item.Selected)
                {
                    if (strRetailerType == "")
                    {

                        strRetailerType += "" + item.Value.ToString() + "";
                    }
                    else
                    {
                        strRetailerType += "," + item.Value.ToString() + "";
                    }
                }
            }
            if (ddlReportType.SelectedValue.ToString() == "3")
            {
                if (strRetailerType.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@RetailerType", strRetailerType);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RetailerType", "1,2");
                }
            }

            dtDataByfilter = new DataTable();
            dtDataByfilter.Load(cmd.ExecuteReader());
            DataTable dt = new DataTable();
            dt = dtDataByfilter;
            return dt;
        }
        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text=="")
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select the from date!');", true);
                    return;
                }
                if (txtFromDate.Text == "")
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select the to date!');", true);
                    return;
                }
                if (Convert.ToDateTime(txtFromDate.Text)>(Convert.ToDateTime(txtToDate.Text)))
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please set from date before to date!');", true);
                    return;
                }
                bool b = Validate();
                if (b)
                {
                    string StateList = "";
                    strSiteName = "";
                    foreach (ListItem item in chkListSite.Items)
                    {
                        if (item.Selected)
                        {
                            if (StateList == "")
                            {
                                StateList += "'" + item.Value.ToString() + "'";
                                strSiteName += "'" + item.Text.ToString() + "'";
                            }
                            else
                            {
                                StateList += ",'" + item.Value.ToString() + "'";
                                strSiteName += "'" + item.Text.ToString() + "'";
                            }
                        }
                    }

                    if (ddlReportType.SelectedValue.ToString() != "0")
                    {
                        if (ddlReportType.SelectedValue == "1")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_SALETAXREPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "2")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_SALETAXSUMMARYREPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "3")
                        {
                            ShowData_ForExcel("ACX_USP_CUSTOMERWISE_VAT_REPORT_SALES", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "4")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_PURCHASETAXREPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "5")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_PURCHASETAX_SUMMARY_REPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "6")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_PRODUCTWISEINPUTOUTPUT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "7")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_OUTPUTVATREPORT", ddlReportType.SelectedItem.Text.ToString());
                        }


                        if (ddlReportType.SelectedValue == "8")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_PURCHASEREGISTER_REPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "9")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_SALESREGISTER_REPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "10")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_ANNEXURE_AB_SALES_REPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "11")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_ANNEXURE_AB_PURCHASE_REPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                        if (ddlReportType.SelectedValue == "12")
                        {
                            ShowData_ForExcel("ACX_USP_VAT_INPUTVATREPORT", ddlReportType.SelectedItem.Text.ToString());
                        }
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select Report Type!');", true);
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        #region|SHOW DATA IN GRIDVIEW|
        protected void BtnShowReport0_Click1(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text == "")
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select the from date!');", true);
                    return;
                }
                if (txtFromDate.Text == "")
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select the to date!');", true);
                    return;
                }
                if (Convert.ToDateTime(txtFromDate.Text) > (Convert.ToDateTime(txtToDate.Text)))
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please set from date before to date!');", true);
                    return;
                }
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string FilterQuery = string.Empty;
                DataTable dtSetHeader = null;
                DataTable dt = new DataTable();
                if (ddlReportType.SelectedValue.ToString() != "0")
                {
                    if (ddlReportType.SelectedValue == "1")
                    {
                        dt = GetData("ACX_USP_VAT_SALETAXREPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "2")
                    {
                        dt = GetData("ACX_USP_VAT_SALETAXSUMMARYREPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "3")
                    {

                        dt = GetData("ACX_USP_CUSTOMERWISE_VAT_REPORT_SALES", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "4")
                    {
                        dt = GetData("ACX_USP_VAT_PURCHASETAXREPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "5")
                    {
                        dt = GetData("ACX_USP_VAT_PURCHASETAX_SUMMARY_REPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "6")
                    {
                        dt = GetData("ACX_USP_VAT_PRODUCTWISEINPUTOUTPUT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "8")
                    {
                        dt = GetData("ACX_USP_VAT_PURCHASEREGISTER_REPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "9")
                    {
                        dt = GetData("ACX_USP_VAT_SALESREGISTER_REPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "10")
                    {
                        dt = GetData("ACX_USP_VAT_ANNEXURE_AB_SALES_REPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "11")
                    {
                        dt = GetData("ACX_USP_VAT_ANNEXURE_AB_PURCHASE_REPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                    if (ddlReportType.SelectedValue == "12")
                    {
                        dt = GetData("ACX_USP_VAT_INPUTVATREPORT", ddlReportType.SelectedItem.Text.ToString(), obj, ref dtSetHeader);
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please select Report Type!');", true);
                    return;
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvDetail.DataSource = dt;
                    gvDetail.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion
        #region|SHOW DATA IN EXCEL|
        private void ShowData_ForExcel(string usp_SaleTaxReport, string strReportName)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            try
            {
                DataTable dt = GetData(usp_SaleTaxReport, strReportName, obj, ref dtSetHeader);
                DataGrid dg = new DataGrid();
                dg.DataSource = dt;
                dg.DataBind();
                // THE EXCEL FILE.
                string sFileName = strReportName + " - " + System.DateTime.Now.Date + ".xls";
                sFileName = sFileName.Replace("/", "");
                // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                Response.ContentType = "application/vnd.ms-excel";
                EnableViewState = false;

                System.IO.StringWriter objSW = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
                dg.RenderControl(objHTW);

                Response.Write("<table><tr><td colspan='5' style='width:100px;align-items:center; font:18px;'> <b> " + ddlReportType.SelectedItem.Text.ToString() + "</b> </td></tr>  <tr><td colspan='3'> <b> Distributor Name : " + strSiteName + " </td></tr><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
                // STYLE THE SHEET AND WRITE DATA TO IT.
                Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                    "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                Response.Write(objSW.ToString());
                // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
                // Response.Write("<table><tr><td><b>Total: </b></td><td></td><td><b>" +"N2" + "</b></td></tr></table>");
                Response.End();
                dg = null;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion

        #region|REPORT TYPE|
        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportType.SelectedValue.ToString() == "3")
            {
                chkCustomerType.Visible = true;
            }
            else
            {
                chkCustomerType.Visible = false;
            }
            gvDetail.DataSource = null;
            gvDetail.DataBind();
        }
        #endregion
    }
}