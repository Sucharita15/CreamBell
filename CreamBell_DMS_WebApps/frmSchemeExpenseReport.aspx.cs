using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSchemeExpenseReport : System.Web.UI.Page
    {
        App_Code.Global baseObj = new App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteIdChanged;
            ucRoleFilters.ListStateChanged += UcRoleFilters_ListStateChanged;
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                if (Convert.ToString(Session["LOGINTYPE"]) != "3")
                {
                    //string sqlstr11 = "Select Distinct StateCode Code,StateCode Name from [ax].[INVENTSITE] where STATECODE <>'' ";
                    //ddlState.Items.Add("Select...");
                    //baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                    fillSiteAndState();
                    //FillCustomerGroup();
                    if (ddlSiteId.SelectedIndex == 0)
                    {
                        FillBusinessUnits();
                    }

                    // FillBusinessUnits();
                    // fillSchemeode();
                }
                FillCustomerGroup();
            }
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                ddlState.Visible = false;
                ddlSiteId.Visible = false;
                lblState.Visible = false;
                lblSiteId.Visible = false;
                roleFilterPanel.Visible = true;

                if (ucRoleFilters.SiteList.SelectedIndex == 0)
                {
                    FillBusinessUnitsForLoginType3();
                }

                //phState.Visible = false;
                //ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            }
        }

        private void UcRoleFilters_ListStateChanged(object sender, EventArgs e)
        {
            fillSchemeodeForLoginType3();
        }

        private void UcRoleFilters_ListSiteIdChanged(object sender, EventArgs e)
        {
            if (ddlSiteId.SelectedIndex > 0)
            {
                FillBusinessUnitsForLoginType3();
                fillSchemeodeForLoginType3();

            }
            else { ChkBunt.Items.Clear(); }
        }

        //private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        //{
        //    //FillCustomerGroup();
        //}
        protected void fillSiteAndState()
        {
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                ddlState.Items.Add("All...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
            }
        }
        protected void FillBusinessUnitsForLoginType3()
        {
            DataTable dt = new DataTable();
            string query = "select distinct bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID in ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedSiteId()) + "')";
            dt = new DataTable();

            dt = baseObj.GetData(query);
            ChkBunt.Items.Clear();
            ChkBunt.DataSource = dt;
            ChkBunt.DataTextField = "bu_desc";
            ChkBunt.DataValueField = "bu_code";
            ChkBunt.DataBind();
        }

        private string GetStringValueCommaSeparted(string value)
        {
            return value.Replace(",", "','");
        }

        protected void FillBusinessUnits()
        {
            DataTable dt = new DataTable();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID in ('" + ddlSiteId.SelectedItem.Value.ToString() + "')";
            dt = new DataTable();

            dt = baseObj.GetData(query);
            ChkBunt.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ChkBunt.DataSource = dt;
                ChkBunt.DataTextField = "bu_desc";
                ChkBunt.DataValueField = "bu_code";

                ChkBunt.DataBind();
            }
        }

        protected void fillSchemeodeForLoginType3()
        {
            DataTable dt = new DataTable();
            chkScheme.Items.Clear();
            string sqlstr = string.Empty;
            sqlstr = "Select Distinct Schemecode,[Scheme Description] AS SchemeName From ACXAllSCHEMEVIEW Where ((StartingDate between '" + txtFromDate.Text.ToString() + "' And '" + txtToDate.Text.ToString() + "') or (ENDINGDATE between '" + txtFromDate.Text.ToString() + "' and '" + txtToDate.Text.ToString() + "')) ";
            //((StartingDate<='01-Feb-2017' and ENDINGDATE>='01-Feb-2017') Or (StartingDate<='28-Feb-2017' and ENDINGDATE>='28-Feb-2017'))
            if (ucRoleFilters.GetCommaSepartedStateId().Length > 0 && ucRoleFilters.GetCommaSepartedSiteId().Length == 0)
            {
                sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type] IN ('State','SITE') and ([SalesCode] in ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedStateId()) + "') OR [SalesCode] in (SELECT SITEID FROM AX.INVENTSITE WHERE STATECODE IN ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedStateId()) + "')))))";
            }
            if (ucRoleFilters.GetCommaSepartedSiteId().Length > 0)
            {
                sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type]='State' and [SalesCode] IN ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedStateId()) + "')) OR ([Sales Type]='SITE' AND [SalesCode] IN ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedSiteId()) + "')))";
            }
            else if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type]='State' and [SalesCode] IN ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedStateId()) + "')) OR ([Sales Type]='SITE' AND [SalesCode] IN ('" + GetStringValueCommaSeparted(ucRoleFilters.GetCommaSepartedSiteId()) + "')))";
            }
            dt = new DataTable();
            dt = baseObj.GetData(sqlstr);
            chkScheme.Items.Clear();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            chkScheme.DataSource = dt;
            chkScheme.DataTextField = "SchemeName";
            chkScheme.DataValueField = "Schemecode";
            chkScheme.DataBind();
            Scheme.Update();
            //   }       
        }

        protected void fillSchemeode()
        {
            DataTable dt = new DataTable();
            chkScheme.Items.Clear();
            string sqlstr = string.Empty;
            sqlstr = "Select Distinct Schemecode,[Scheme Description] AS SchemeName From ACXAllSCHEMEVIEW Where ((StartingDate between '" + txtFromDate.Text.ToString() + "' And '" + txtToDate.Text.ToString() + "') or (ENDINGDATE between '" + txtFromDate.Text.ToString() + "' and '" + txtToDate.Text.ToString() + "')) ";
            //((StartingDate<='01-Feb-2017' and ENDINGDATE>='01-Feb-2017') Or (StartingDate<='28-Feb-2017' and ENDINGDATE>='28-Feb-2017'))
            if (ddlState.SelectedIndex > 0 && ddlSiteId.SelectedIndex < 0)
            {
                sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type] IN ('State','SITE') and ([SalesCode] in ('" + ddlState.SelectedValue.ToString() + "') OR [SalesCode] in (SELECT SITEID FROM AX.INVENTSITE WHERE STATECODE='" + ddlState.SelectedValue.ToString() + "'))))";
            }
            if (ddlSiteId.SelectedIndex > 0)
            {
                sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type]='State' and [SalesCode]='" + ddlState.SelectedValue.ToString() + "') OR ([Sales Type]='SITE' AND [SalesCode]='" + ddlSiteId.SelectedValue.ToString() + "'))";
            }
            else if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                sqlstr += " AND ([Sales Type]='ALL' OR ([Sales Type]='State' and [SalesCode]='" + ddlState.SelectedValue.ToString() + "') OR ([Sales Type]='SITE' AND [SalesCode]='" + ddlSiteId.SelectedValue.ToString() + "'))";
            }
            dt = new DataTable();
            dt = baseObj.GetData(sqlstr);
            chkScheme.Items.Clear();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            chkScheme.DataSource = dt;
            chkScheme.DataTextField = "SchemeName";
            chkScheme.DataValueField = "Schemecode";
            chkScheme.DataBind();
            Scheme.Update();
            //   }       
        }
        private bool ValidateInput()
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
        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = ValidateInput();
            if (b)
            {
                //ShowReport();
                ShowReportByFilter();
            }
        }
        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new CreamBell_DMS_WebApps.App_Code.Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "ax.ACX_SCHEMECLAIMREPORT";

                cmd.CommandText = query;
                DateTime now = Convert.ToDateTime(txtToDate.Text);
                now = now.AddMonths(1);
                //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    cmd.Parameters.AddWithValue("@STATECODE", ucRoleFilters.GetCommaSepartedStateId());
                    cmd.Parameters.AddWithValue("@SiteId", ucRoleFilters.GetCommaSepartedSiteId());
                }
                else
                {
                    string State = string.Empty;
                    if (ddlState.SelectedItem.Text == "All...")
                    {
                        cmd.Parameters.AddWithValue("@STATECODE", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@STATECODE", ddlState.SelectedItem.Value);
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "All...")
                        {
                            cmd.Parameters.AddWithValue("@SiteId", "");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                        }
                    }
                }


                // CustGroup
                string CustGroupList = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroupList == "")
                        {
                            CustGroupList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            CustGroupList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (CustGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", "");
                }

                // Scheme Code
                string SchemeList = "";
                foreach (ListItem item in chkScheme.Items)
                {
                    if (item.Selected)
                    {
                        if (SchemeList == "")
                        {
                            SchemeList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            SchemeList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (SchemeList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Scheme", SchemeList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Scheme", "");
                }
                string buunit = "";
                foreach (ListItem items in ChkBunt.Items)
                {
                    if (items.Selected)
                    {
                        if (buunit == "")
                        {
                            buunit += "" + items.Value.ToString() + "";
                        }
                        else
                        {
                            buunit += "," + items.Value.ToString() + "";
                        }
                    }
                }
                if (buunit.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", buunit);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }
                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
                LoadDataInReportViewer(dtDataByfilter);
            }
            catch (Exception ex)
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChkBunt.Items.Clear();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
                ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                //ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            fillSchemeode();
        }
        private void LoadDataInReportViewer(DataTable dtSetData)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FromDate");
                    DataSetParameter.Columns.Add("ToDate");
                    DataSetParameter.Columns.Add("STATECODE");
                    DataSetParameter.Columns.Add("SiteId");
                    DataSetParameter.Columns.Add("CustGroup");
                    DataSetParameter.Columns.Add("Scheme");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;

                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataSetParameter.Rows[0]["STATECODE"] = ucRoleFilters.GetCommaSepartedStateId();
                        DataSetParameter.Rows[0]["SiteId"] = ucRoleFilters.GetCommaSepartedSiteId();
                    }
                    else
                    {
                        if (ddlState.SelectedItem.Text == "All...")
                        {
                            DataSetParameter.Rows[0]["STATECODE"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["STATECODE"] = ddlState.SelectedItem.Text;
                        }
                        if (ddlSiteId.SelectedIndex != -1)
                        {
                            if (ddlSiteId.SelectedItem.Text == "All...")
                            {
                                DataSetParameter.Rows[0]["SiteId"] = "All";
                            }
                            else
                            {
                                DataSetParameter.Rows[0]["SiteId"] = ddlSiteId.SelectedItem.Text;
                            }
                        }
                    }



                    string CustGroupList = "";
                    foreach (ListItem item in chkListCustomerGroup.Items)
                    {
                        if (item.Selected)
                        {
                            if (CustGroupList == "")
                            {
                                CustGroupList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                CustGroupList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    if (CustGroupList.Length > 0)
                    {
                        DataSetParameter.Rows[0]["CustGroup"] = CustGroupList;
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["CustGroup"] = "All";
                    }

                    // Scheme Code
                    string SchemeList = "";
                    foreach (ListItem item in chkScheme.Items)
                    {
                        if (item.Selected)
                        {
                            if (SchemeList == "")
                            {
                                SchemeList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                SchemeList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    if (SchemeList.Length > 0)
                    {
                        DataSetParameter.Rows[0]["Scheme"] = SchemeList;
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["Scheme"] = "All";
                    }
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SchemeClaimReport.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;

                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    this.ReportViewer1.LocalReport.Refresh();

                    ReportViewer1.Visible = true;
                    //ReportViewer1.ZoomPercent = 100;
                    LblMessage.Text = String.Empty;
                }
                else
                {
                    LblMessage.Text = "No Records Exists !!";
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            ShowData_ForExcel();
        }

        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
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
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "ax.ACX_SCHEMECLAIMREPORT";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));
                if (ddlState.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@STATECODE", ddlState.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }

                // site

                if (ddlSiteId.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }

                // CustGroup
                string CustGroupList = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroupList == "")
                        {
                            CustGroupList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            CustGroupList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (CustGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", "");
                }

                // Scheme Code
                string SchemeList = "";
                foreach (ListItem item in chkScheme.Items)
                {
                    if (item.Selected)
                    {
                        if (SchemeList == "")
                        {
                            SchemeList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            SchemeList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (SchemeList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Scheme", SchemeList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Scheme", "");
                }
                string buunites = "";
                foreach (ListItem items in ChkBunt.Items)
                {
                    if (items.Selected)
                    {
                        if (buunites == "")
                        {
                            buunites += "" + items.Value.ToString() + "";
                        }
                        else
                        {
                            buunites += "," + items.Value.ToString() + "";
                        }
                    }
                }
                if (buunites.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", buunites);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }

                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                decimal BASICRATE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASICRATE"));
                decimal INVOICEVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("INVOICE VALUE"));
                //decimal EXPENSEVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("EXPENSE VALUE"));
                decimal BASICVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("BASIC VALUE"));
                decimal LTR = dt.AsEnumerable().Sum(row => row.Field<decimal>("LTR"));
                decimal TOTALQTY = dt.AsEnumerable().Sum(row => row.Field<decimal>("TOTAL QTY"));
                decimal SCHVALUE = dt.AsEnumerable().Sum(row => row.Field<decimal>("ADD SCH VALUE"));

                //////--------------------------------------
                DataGrid dg = new DataGrid();
                dg.DataSource = dt;
                dg.DataBind();
                string sFileName = "SchemeReport" + " - " + System.DateTime.Now.Date + ".xls";

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
                string name = "Scheme Report";

                string DistributoName = ddlSiteId.SelectedItem.Value + " - " + ddlSiteId.SelectedItem.Text;
                Response.Write("<table><tr><td colspan='3' style='width:100px;align-items:center; font:18px;'> <b> " + name + "</b> </td></tr>  <tr><td colspan='3'> <b> Distributor Name : " + DistributoName + "  </td></tr><tr><td><b>From Date:  " + txtFromDate.Text.Replace(",", "") + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text.Replace(",", "") + "</b></td></tr></table>");
                // STYLE THE SHEET AND WRITE DATA TO IT.
                Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                    "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                Response.Write(objSW.ToString());
                // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
                //Response.Write("<table><tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td><b>Total:</b></td><td><b>" + BASICRATE + "</b></td><td><b>" + INVOICEVALUE + " </b></td><td ><b>" + TOTALQTY + "</b></td><td ><b>" + LTR + "</b></td><td ><b>" + BASICVALUE + "</b></td><td ><b>" + SCHVALUE + "</b></td><td ><b>" + EXPENSEVALUE + "</b></td></tr></table>");
                Response.Write("<table><tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td><b>Total:</b></td><td><b>" + BASICRATE + "</b></td><td><b>" + INVOICEVALUE + " </b></td><td ><b>" + TOTALQTY + "</b></td><td ><b>" + LTR + "</b></td><td ><b>" + BASICVALUE + "</b></td><td ><b>" + SCHVALUE + "</b></td></tr></table>");
                Response.End();
                dg = null;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillCustomerGroup()
        {
            DataTable dt = new DataTable();
            string sqlstr = "select Distinct CustGroup_Name as Name,Custgroup_Code as Code from ax.ACXCUSTGROUPMASTER ";
            dt = new DataTable();
            dt = baseObj.GetData(sqlstr);
            chkListCustomerGroup.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chkListCustomerGroup.DataSource = dt;
                chkListCustomerGroup.DataTextField = "NAME";
                chkListCustomerGroup.DataValueField = "Code";
                chkListCustomerGroup.DataBind();
            }

        }

        protected void txtToDate_TextChanged(object sender, EventArgs e)
        {
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    fillSchemeodeForLoginType3();
                }
                else
                {
                    fillSchemeode();
                }
            }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    fillSchemeodeForLoginType3();
                }
                else
                {
                    fillSchemeode();
                }
            }
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //for (int i = 0; i < ddlSiteId.Items.Count; i++)
            //{
            //    if (ddlSiteId.Items[i].Selected == true)
            //    {
            //        FillBusinessUnits();
            //        fillSchemeode();
            //        break;

            //    }
            //    else {
            //        ChkBunt.Items.Clear();
            //    }

            //}

            if (ddlSiteId.SelectedIndex > 0)
            {
                FillBusinessUnits();
                fillSchemeode();

            }
            else { ChkBunt.Items.Clear(); }
        }


    }
}