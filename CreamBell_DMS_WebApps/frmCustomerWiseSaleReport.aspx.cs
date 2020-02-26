using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using ClosedXML.Excel;
using System.IO;
using Elmah;
using CreamBell_DMS_WebApps.App_Code;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCustomerWiseSaleReport : System.Web.UI.Page
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
                baseObj.FillSaleHierarchy();
                fillHOS();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //DataView DtSaleHierarchy = (DataTable)HttpContext.Current.Session["SaleHierarchy"];
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    if (dt.Rows.Count > 0)
                    {
                        var dr_row = dt.AsEnumerable();
                        var test = (from r in dr_row
                                    select r.Field<string>("SALEPOSITION")).First<string>();
                        //string dr1 = dt.Select("SALEPOSITION").ToString();
                        if (test == "VP")
                        {
                            chkListHOS.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "GM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "DGM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "RM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "ZM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkListRM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            CheckBox4.Enabled = false;
                            CheckBox4.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "ASM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkListRM.Enabled = false;
                            chkListZM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            CheckBox4.Enabled = false;
                            CheckBox4.Checked = true;
                            CheckBox5.Enabled = false;
                            CheckBox5.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "EXECUTIVE")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkListRM.Enabled = false;
                            chkListZM.Enabled = false;
                            chkListEXECUTIVE.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            CheckBox4.Enabled = false;
                            CheckBox4.Checked = true;
                            CheckBox5.Enabled = false;
                            CheckBox5.Checked = true;
                            CheckBox6.Enabled = false;
                            CheckBox6.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                }

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //tclabel.Width = "90%";
                    Panel1.Visible = true;
                }
                else
                {
                    //tclabel.Width = "100%";
                    Panel1.Visible = false;
                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "0" && Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    bind();
                }
            }
        }

        protected void bind()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new DataTable();

                //DataTable dtState = dt.DefaultView.ToTable(true, "STATECODE", "STATENAME");
                lstSiteId.Items.Clear();
                //  DataRow dr = dtState.NewRow();

                // sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                string sqlstr = "Select distinct I.SITEID, I.SITEID+'-'+I.NAME Name from ax.INVENTSITE I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where  I.STATECODE <>'' AND I.SITEID ='" + Convert.ToString(Session["SiteCode"]) + "'";
                dt = baseObj.GetData(sqlstr);
                lstSiteId.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstSiteId.DataSource = dt;
                    lstSiteId.DataTextField = "NAME";
                    lstSiteId.DataValueField = "SITEID";
                    lstSiteId.DataBind();
                }
                if (lstSiteId.Items.Count == 1)
                {
                    //CheckBox8.Visible = false;
                    lstSiteId.Items[0].Selected = true;
                    // lstDIS_SelectedIndexChanged(null, null);
                }
                FillCustomerName();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void Fillstate()
        {
            try
            {
                string sqlstr = "";
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataTable dt = new DataTable();
                        dt = new DataTable();

                        DataTable dtState = dt.DefaultView.ToTable(true, "STATECODE", "STATENAME");
                        lstState.Items.Clear();
                        //  DataRow dr = dtState.NewRow();

                        // sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                        sqlstr = "Select distinct I.SITEID, I.SITEID+'-'+LS.NAME Name from ax.INVENTSITE I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where  I.STATECODE <>'' AND I.SITEID ='" + Convert.ToString(Session["SiteCode"]) + "'";
                        dt = baseObj.GetData(sqlstr);
                        lstState.Items.Add("All...");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lstState.DataSource = dt;
                            lstState.DataTextField = "SITEID";
                            lstState.DataValueField = "NAME";
                            lstState.DataBind();
                        }
                        if (lstState.Items.Count == 1)
                        {
                            //CheckBox8.Visible = false;
                            lstState.Items[0].Selected = true;
                            // lstDIS_SelectedIndexChanged(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillSite()
        {
            try
            {
                string StateList = "";
                foreach (ListItem item in lstSiteId.Items)
                {
                    if (item.Selected)
                    {
                        if (StateList == "")
                        {
                            StateList += "'" + item.Value.ToString() + "'";
                        }
                        else
                        {
                            StateList += ",'" + item.Value.ToString() + "'";
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
                        sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                    }
                    else
                    {
                        sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    }

                    dt = new DataTable();
                    lstSiteId.Items.Clear();
                    dt = baseObj.GetData(sqlstr1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "SiteName";
                        lstSiteId.DataValueField = "SiteId";
                        lstSiteId.DataBind();
                    }
                    if (lstSiteId.Items.Count == 1)
                    {
                        lstSiteId.Items[0].Selected = true;
                    }
                    if (lstSiteId.Items.Count == 0)
                    {
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    lstSiteId.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void fillSiteAndState(DataTable dt)
        {
            try
            {
                string sqlstr = "";
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                        dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                        lstState.Items.Clear();
                        DataRow dr = dtState.NewRow();

                        lstState.DataSource = dtState;
                        lstState.DataTextField = "STATENAMES";
                        lstState.DataValueField = "STATE";
                        lstState.DataBind();
                    }
                    else
                    {
                        sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "' order by Name";
                        DataTable dt1 = baseObj.GetData(sqlstr);
                        lstState.DataSource = dt1;
                        lstState.DataTextField = "Name";
                        lstState.DataValueField = "Code";
                        lstState.DataBind();
                    }
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' order by Name ";
                    lstState.Items.Add("Select...");
                    // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                    DataTable dt1 = baseObj.GetData(sqlstr);
                    lstState.DataSource = dt1;
                    lstState.DataTextField = "Name";
                    lstState.DataValueField = "Code";
                    lstState.DataBind();
                }
                if (lstState.Items.Count == 1)
                {
                    foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                    {
                        litem.Selected = true;
                    }
                    lstSiteId_SelectedIndexChanged(null, null);
                }
                // fillbu();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
            if (lstState.SelectedValue == string.Empty)
            {
                b = false;
                string message = "alert('Please Select The State  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }
            if (lstSiteId.SelectedValue == string.Empty)
            {
                b = false;
                string message = "alert('Please Select The SiteID  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }
            if (lstcustomers.SelectedValue == string.Empty)
            {
                b = false;
                string message = "alert('Please Select The Customers  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }

            return b;
        }
        //private void FillCustomerName()
        //{
        //    string strCustomerGroupName = "";
        //    string strSubDistCustGroup = "";
        //    string LstSiteList = App_Code.Global.GetSelectCustGroup(ref lstSiteId, true);
        //    #region Get All selected CustomerGroup
        //    foreach (ListItem item in lstcustomers.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            if (strCustomerGroupName == "")
        //            {
        //                strCustomerGroupName += "'" + item.Value.ToString() + "'";
        //            }
        //            else
        //            {
        //                strCustomerGroupName += ",'" + item.Value.ToString() + "'";
        //            }
        //        }
        //    }
        //    #endregion

        //    #region get all selected site
        //    string SiteList = "";
        //    foreach (ListItem site in lstSiteId.Items)
        //    {
        //        if (site.Selected)
        //        {
        //            if (SiteList == "")
        //            {
        //                SiteList += "'" + site.Value.ToString() + "'";
        //            }
        //            else
        //            {
        //                SiteList += ",'" + site.Value.ToString() + "'";
        //            }
        //        }
        //    }
        //    #endregion

        //    if (SiteList.Length > 0)
        //    {
        //        DataTable dt = new DataTable();
        //        //string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
        //        string sqlstr = "Select distinct Customer_Code+'-'+Customer_Name as Name,Customer_Code from VW_CUSTOMERMASTER_HISTORY where Customer_Code in (" + strCustomerGroupName + ") and Site_Code in (" + SiteList + ") Order By Name";
        //        //if (strSubDistCustGroup != "")
        //        //{
        //        //    sqlstr += "Union Select distinct CU.Customer_Code+'-'+CU.Customer_Name as Name,CU.Customer_Code From ax.ACX_SDLINKING SD Left Join ax.ACXCUSTMASTER CU on CU.Customer_Code=SD.SubDistributor where SD.Other_site in (" + SiteList + ") And CU.CUST_GROUP='CG0004' Order By Name";
        //        //}
        //        //else
        //        //{
        //        //    sqlstr += " Order By Name";
        //        //}
        //        dt = new DataTable();
        //        dt = baseObj.GetData(sqlstr);
        //        lstcustomers.Items.Clear();
        //        //for (int i = 0; i < dt.Rows.Count; i++)
        //        //{
        //        lstcustomers.DataSource = dt;
        //        lstcustomers.DataTextField = "NAME";
        //        lstcustomers.DataValueField = "Customer_Code";
        //        lstcustomers.DataBind();

        //        //lstcustomers.Items.Clear();
        //        //lstcustomers.Items.Add("--Select--");
        //        //baseObj.BindToDropDown(chkCustomerName, sqlstr, "NAME", "Customer_Code");

        //        //}
        //        if (dt.Rows.Count == 0)
        //        {
        //            lstcustomers.Items.Clear();
        //            lstcustomers.Items.Add("--Select--");
        //        }
        //    }
        //    else
        //    {
        //        lstcustomers.Items.Clear();
        //        lstcustomers.Items.Add("--Select--");
        //    }
        //}
        protected void FillCustomerName()
        {
            try
            {
                string LstSiteList = App_Code.Global.GetSelectCustGroup(ref lstSiteId, true);
                string strCondition = "";
                lstcustomers.Items.Clear();
                string sqlstr1;
                if (LstSiteList != "")
                {
                    strCondition = " AND SITE_CODE in (" + LstSiteList + ")";
                    sqlstr1 = "Select DISTINCT Customer_Code + '-' + Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 " + strCondition;
                }
                else
                {
                    sqlstr1 = "Select DISTINCT Customer_Code + '-' + Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 and customer_code=''";
                }
                lstcustomers.Items.Add("All...");
                DataTable dt = baseObj.GetData(sqlstr1);
                lstcustomers.DataSource = dt;
                lstcustomers.DataTextField = "Name";
                lstcustomers.DataValueField = "Customer_Code";
                lstcustomers.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        //private void ShowReportSummary()
        //{
        //    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
        //    // string BuList = App_Code.Global.GetSelectBUList(ref lstbu, false);
        //    DataTable dtSetData = null;
        //    string siteid = "";
        //    int count = 0;
        //    string FromDate = txtFromDate.Text;
        //    string ToDate = txtToDate.Text;
        //    foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
        //    {
        //        if (litem.Selected)
        //        {
        //            count += 1;
        //            if (siteid.Length == 0)
        //                siteid = "" + litem.Value.ToString() + "";
        //            else
        //                siteid += "," + litem.Value.ToString() + "";
        //            dtSetData = new DataTable();
        //        }
        //    }
        //    if (count > 5)
        //    {
        //        // string message = "alert('Click On Export to Excel Only.If more than 5 Distributor Selected!');";
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Click On Export To Excel Only.If More Than 5 Distributor Selected!');", true);
        //        return;
        //    }
        //    try
        //    {
        //        dtSetData = new DataTable();

        //        string query = "ACX_SALEREGISTER_DATEWISENEW";
        //        List<string> ilist = new List<string>();
        //        List<string> item = new List<string>();
        //        string StateCode, bu;
        //        siteid = "";

        //        foreach (System.Web.UI.WebControls.ListItem litem1 in lstSiteId.Items)
        //        {
        //            if (litem1.Selected)
        //            {
        //                count += 1;
        //                if (siteid.Length == 0)
        //                    siteid = "" + litem1.Value.ToString() + "";
        //                else
        //                    siteid += "," + litem1.Value.ToString() + "";
        //                dtSetData = new DataTable();
        //            }
        //        }
        //        ilist.Add("@SITEID"); item.Add(siteid);

        //        StateCode = "";
        //        foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
        //        {
        //            if (litem.Selected)
        //            {
        //                count += 1;
        //                if (StateCode.Length == 0)
        //                    StateCode = "" + litem.Value.ToString() + "";
        //                else
        //                    StateCode += "," + litem.Value.ToString() + "";
        //                dtSetData = new DataTable();
        //            }
        //        }
        //        bu = "";
        //        foreach (System.Web.UI.WebControls.ListItem litem2 in lstbu.Items)
        //        {
        //            if (litem2.Selected)
        //            {
        //                count += 1;
        //                if (bu.Length == 0)
        //                    bu = "" + litem2.Value.ToString() + "";
        //                else
        //                    bu += "," + litem2.Value.ToString() + "";
        //                dtSetData = new DataTable();
        //            }
        //        }
        //        ilist.Add("@STATE"); item.Add(StateCode);
        //        ilist.Add("@FROMDATE"); item.Add(txtFromDate.Text);
        //        ilist.Add("@TODATE"); item.Add(txtToDate.Text);
        //        ilist.Add("@BUCODE"); item.Add(bu);
        //        if (bu.Length != 0)
        //        {
        //            item.Add(lstbu.SelectedItem.Value.ToString());
        //        }
        //        else
        //        {
        //            item.Add("");
        //        }
        //        dtSetData = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
        //        //dtSetData = obj.GetData(sqlstr);
        //        LoadDataInReportViewerDetail(dtSetData);
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message.ToString();
        //    }
        //}

        //private void LoadDataInReportViewerDetail(DataTable dtSetData)
        //{
        //    try
        //    {
        //        if (dtSetData.Rows.Count > 0)
        //        {
        //            DataTable DataSetParameter = new DataTable();
        //            DataSetParameter.Columns.Add("FromDate");
        //            DataSetParameter.Columns.Add("ToDate");
        //            DataSetParameter.Columns.Add("StateCode");
        //            DataSetParameter.Columns.Add("Distributor");
        //            DataSetParameter.Columns.Add("BU");
        //            DataSetParameter.Rows.Add();
        //            DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
        //            DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
        //            if (lstState.SelectedItem.Text == "Select...")
        //            {
        //                DataSetParameter.Rows[0]["StateCode"] = "All";
        //            }
        //            else
        //            {
        //                DataSetParameter.Rows[0]["StateCode"] = lstState.SelectedItem.Text;
        //            }
        //            if (lstSiteId.SelectedIndex != -1)
        //            {
        //                if (lstSiteId.SelectedItem.Text == "Select...")
        //                {
        //                    DataSetParameter.Rows[0]["Distributor"] = "All";
        //                }
        //                else
        //                {
        //                    DataSetParameter.Rows[0]["Distributor"] = lstSiteId.SelectedItem.Text;
        //                }
        //            }
        //            if (lstbu.SelectedIndex == 0)
        //            {
        //                if (lstbu.SelectedValue == string.Empty)
        //                {
        //                    //validate();
        //                }
        //                else
        //                {
        //                    DataSetParameter.Rows[0]["BU"] = lstbu.SelectedItem.Text;
        //                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
        //                    ReportViewer1.LocalReport.DataSources.Clear();
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
        //                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
        //                    ReportViewer1.LocalReport.DataSources.Add(RDS2);

        //                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SaleRegisterDateWiseReport.rdl");
        //                    ReportViewer1.LocalReport.Refresh();
        //                    ReportViewer1.Visible = true;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            LblMessage.Text = "No Records Exists !!";
        //            ReportViewer1.Visible = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message.ToString();
        //    }
        //}

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string statesel = "";
                foreach (System.Web.UI.WebControls.ListItem litem1 in lstState.Items)
                {
                    if (litem1.Selected)
                    {
                        if (statesel.Length == 0)
                            statesel = "'" + litem1.Value.ToString() + "'";
                        else
                            statesel += ",'" + litem1.Value.ToString() + "'";
                    }
                }
                if (lstState.SelectedValue == string.Empty)
                {
                    lstSiteId.Items.Clear();
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

                    DataView view = new DataView(dt);
                    DataTable distinctValues = view.ToTable(true, "Distributor", "DistributorName");
                    lstSiteId.DataSource = distinctValues;
                    string AllSitesFromHierarchy = "";
                    foreach (DataRow row in distinctValues.Rows)
                    {
                        if (AllSitesFromHierarchy == "")
                        {
                            AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                        }
                        else
                        {
                            AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                        }
                    }
                    if (AllSitesFromHierarchy != "")
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                        dt = baseObj.GetData(sqlstr1);
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "Name";
                        lstSiteId.DataValueField = "Code";
                        lstSiteId.DataBind();
                    }
                }
                else
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode != null)
                    {
                        lstSiteId.Items.Clear();
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where STATECODE in (" + statesel + ") Order by SiteName";
                        DataTable dt = baseObj.GetData(sqlstr1);
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "Name";
                        lstSiteId.DataValueField = "Code";
                        lstSiteId.DataBind();
                    }
                    else
                    {
                        lstSiteId.Items.Clear();
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            dt.DefaultView.RowFilter = "STATE in (" + statesel + ")";
                            DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                            string AllSitesFromHierarchy = "";
                            foreach (DataRow row in uniqueCols.Rows)
                            {
                                if (AllSitesFromHierarchy == "")
                                {
                                    AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                                }
                                else
                                {
                                    AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                                }
                            }
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                            dt = baseObj.GetData(sqlstr1);
                            lstSiteId.DataSource = dt;
                            lstSiteId.DataTextField = "Name";
                            lstSiteId.DataValueField = "Code";
                            lstSiteId.DataBind();

                        }
                        else
                        {
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where SITEID = '" + Session["SiteCode"].ToString() + "'";
                            DataTable dt = baseObj.GetData(sqlstr1);
                            lstSiteId.DataSource = dt;
                            lstSiteId.DataTextField = "Name";
                            lstSiteId.DataValueField = "Code";
                            lstSiteId.DataBind();
                        }
                    }
                }
                if (lstSiteId.Items.Count == 1)
                {
                    foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
                    {
                        litem.Selected = true;
                    }
                }
                Session["SalesData"] = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void fillHOS()
        {
            try
            {
                chkListHOS.Items.Clear();
                DataTable dtHOS = (DataTable)Session["SaleHierarchy"];
                DataTable uniqueCols = dtHOS.DefaultView.ToTable(true, "HOSNAME", "HOSCODE");
                chkListHOS.DataSource = uniqueCols;
                chkListHOS.DataTextField = "HOSNAME";
                chkListHOS.DataValueField = "HOSCODE";
                chkListHOS.DataBind();
                if (uniqueCols.Rows.Count == 1)
                {
                    chkListHOS.Items[0].Selected = true;
                    lstHOS_SelectedIndexChanged(null, null);
                }
                fillSiteAndState(dtHOS);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstHOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkListVP.Items.Clear();
                chkListGM.Items.Clear();
                chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
                if (CheckSelect(ref chkListHOS))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    //chkListVP.Items.Clear();
                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "VPNAME", "VPCODE");
                    chkListVP.DataSource = uniqueCols2;
                    chkListVP.DataTextField = "VPNAME";
                    chkListVP.DataValueField = "VPCODE";
                    chkListVP.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListVP.Items[0].Selected = true;
                        lstVP_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        chkListVP.Items[0].Selected = false;
                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                    //chkListGM.Items.Clear();
                }
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstVP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkListGM.Items.Clear();
                chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();

                if (CheckSelect(ref chkListVP))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "GMNAME", "GMCODE");

                    chkListGM.DataSource = uniqueCols2;
                    chkListGM.DataTextField = "GMNAME";
                    chkListGM.DataValueField = "GMCODE";
                    chkListGM.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListGM.Items[0].Selected = true;
                        lstGM_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        chkListGM.Items[0].Selected = false;
                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                }
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
                if (CheckSelect(ref chkListGM))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "DGMNAME", "DGMCODE");

                    chkListDGM.DataSource = uniqueCols2;
                    chkListDGM.DataTextField = "DGMNAME";
                    chkListDGM.DataValueField = "DGMCODE";
                    chkListDGM.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListDGM.Items[0].Selected = true;
                        lstDGM_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        chkListDGM.Items[0].Selected = false;
                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                }
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstDGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
                if (CheckSelect(ref chkListDGM))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "RMNAME", "RMCODE");
                    chkListRM.DataSource = uniqueCols2;
                    chkListRM.DataTextField = "RMNAME";
                    chkListRM.DataValueField = "RMCODE";
                    chkListRM.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListRM.Items[0].Selected = true;
                        lstRM_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        chkListRM.Items[0].Selected = false;
                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                }
                ddlCountry_SelectedIndexChanged(null, null);
                //upsale.Update()
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public Boolean CheckSelect(ref CheckBoxList ChkList)
        {
            foreach (System.Web.UI.WebControls.ListItem litem in ChkList.Items)
            {
                if (litem.Selected)
                {
                    return true;
                }
            }
            return false;
        }

        protected void lstRM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
                if (CheckSelect(ref chkListRM))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "ZMNAME", "ZMCODE");
                    chkListZM.DataSource = uniqueCols2;
                    chkListZM.DataTextField = "ZMNAME";
                    chkListZM.DataValueField = "ZMCODE";
                    chkListZM.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListZM.Items[0].Selected = true;
                        lstZM_SelectedIndexChanged(null, null);
                    }
                    else
                    {

                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                }
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstZM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
                if (CheckSelect(ref chkListZM))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    chkListASM.Items.Clear();
                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "ASMNAME", "ASMCODE");
                    chkListASM.DataSource = uniqueCols2;
                    chkListASM.DataTextField = "ASMNAME";
                    chkListASM.DataValueField = "ASMCODE";
                    chkListASM.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListASM.Items[0].Selected = true;
                        lstASM_SelectedIndexChanged(null, null);
                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                }
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstASM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
                if (CheckSelect(ref chkListASM))
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    chkListEXECUTIVE.Items.Clear();
                    DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "EXECUTIVENAME", "EXECUTIVECODE");
                    chkListEXECUTIVE.DataSource = uniqueCols2;
                    chkListEXECUTIVE.DataTextField = "EXECUTIVENAME";
                    chkListEXECUTIVE.DataValueField = "EXECUTIVECODE";
                    chkListEXECUTIVE.DataBind();
                    if (uniqueCols2.Rows.Count == 1)
                    {
                        chkListEXECUTIVE.Items[0].Selected = true;
                    }
                    fillSiteAndState(dt);
                    uppanel.Update();
                }
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                fillSiteAndState(dt);
                uppanel.Update();
                ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(chkAll, chkListHOS);
                lstHOS_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(chkAll, chkListHOS);
                chkListVP.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox1, chkListVP);
                lstVP_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox1, chkListVP);
                // chkListVP.Items.Clear();
                chkListGM.Items.Clear();
                //chkListRM.Items.Clear();
                //chkListZM.Items.Clear();
                //chkListASM.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox2, chkListGM);
                lstGM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox2, chkListGM);
                // chkListGM.Items.Clear();
                chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox3, chkListDGM);
                lstDGM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox3, chkListDGM);
                //chkListGM.Items.Clear();
                //chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox4, chkListRM);
                lstRM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox4, chkListRM);
                //chkListGM.Items.Clear();
                // chkListDGM.Items.Clear();
                //chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox5, chkListZM);
                //chkListASM.Items.Clear();
                lstZM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox5, chkListZM);
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox6, chkListASM);
                //chkListASM.Items.Clear();
                lstASM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox6, chkListASM);
                //chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox7, chkListEXECUTIVE);
            ddlCountry_SelectedIndexChanged(null, null);
            //chkListASM.DataSource = null;
        }

        protected void CheckAll_CheckedChanged(CheckBox CheckAll, CheckBoxList ChkList)
        {
            if (CheckAll.Checked == true)
            {
                for (int i = 0; i < ChkList.Items.Count; i++)
                {
                    ChkList.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < ChkList.Items.Count; i++)
                {
                    ChkList.Items[i].Selected = false;
                }
            }
        }

        //protected void BtnShowReport_Click(object sender, EventArgs e)
        //{
        //    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
        //    DataTable dt = new DataTable();
        //    validate();
        //    bool b = ValidateInput();
        //    if (b == true)
        //    {
        //        try
        //        {
        //            string FromDate = txtFromDate.Text;
        //            string ToDate = txtToDate.Text;

        //            ShowReportSummary();
        //        }
        //        catch (Exception ex)
        //        {
        //            LblMessage.Text = ex.Message.ToString();
        //        }
        //    }
        //}

        protected void lstSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerName();
        }

        protected void validate()
        {
            if (lstState.SelectedValue == string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Select State!');", true);
                return;
            }
            else if (lstSiteId.SelectedValue == string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Select Site ID!');", true);
                return;
            }
        }

        protected void btnExport2Excel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
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

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["dtCustomer"];

                string attachment = "attachment; filename=DateWiseSalesReport.xls";
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

        private void ExportToExcel()
        {            
            try
            {
                //if (string.IsNullOrEmpty(rdbListExcelFileFormat.SelectedValue))
                //{
                //    rdbListExcelFileFormat.SelectedValue = "XLSB";
                //}
                if (!ucRadioButtonList.GetXLS.Checked &&
                   !ucRadioButtonList.GetXLSX.Checked &&
                   !ucRadioButtonList.GetXLSB.Checked)
                {
                    ucRadioButtonList.GetXLSB.Checked = true;
                }
                bool b = ValidateInput();
                if (b == true)
                {
                    //GridView gvvc = new GridView();
                    //CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                    //SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                    if (lstState.SelectedValue == string.Empty)
                    {
                        string message = "alert('Please Select The State  !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    }
                    
                    //int count = 0, count1 = 0, count2 = 0;                   
                    string StateCode, Customer;
                    string siteid;
                    siteid = StateCode = "";
                    foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                    {
                        if (litem.Selected)
                        {
                            //count1 += 1;
                            if (StateCode.Length == 0)
                                StateCode = "" + litem.Value.ToString() + "";
                            else
                                StateCode += "," + litem.Value.ToString() + "";
                            //dtSetData = new DataTable();
                        }
                    }

                    foreach (System.Web.UI.WebControls.ListItem litem1 in lstSiteId.Items)
                    {
                        if (litem1.Selected)
                        {
                            //count += 1;
                            if (siteid.Length == 0)
                                siteid = "" + litem1.Value.ToString() + "";
                            else
                                siteid += "," + litem1.Value.ToString() + "";
                            //dtSetData = new DataTable();
                        }
                    }                    
                   
                    Customer = "";
                    foreach (System.Web.UI.WebControls.ListItem litem2 in lstcustomers.Items)
                    {
                        if (litem2.Selected)
                        {
                            //count2 += 1;
                            if (Customer.Length == 0)
                                Customer = "" + litem2.Value.ToString() + "";
                            else
                                Customer += "," + litem2.Value.ToString() + "";
                            //dtSetData = new DataTable();
                        }
                    }
                    
                    //string query = "USP_CustomerWiseSaleReport '" + txtFromDate.Text + "','" + txtToDate.Text + "','" + StateCode.ToString() + "','" + siteid.ToString() + "','" + Customer + "'";
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter fromDateParam = new SqlParameter("@FROMDATE", SqlDbType.SmallDateTime);
                    fromDateParam.Value = txtFromDate.Text;
                    sqlParameters.Add(fromDateParam);

                    SqlParameter toDateParam = new SqlParameter("@TODATE", SqlDbType.SmallDateTime);
                    toDateParam.Value = txtToDate.Text;
                    sqlParameters.Add(toDateParam);

                    SqlParameter stateCodeParam = new SqlParameter("@STATE", SqlDbType.NVarChar, 5000);
                    stateCodeParam.Value = StateCode;
                    sqlParameters.Add(stateCodeParam);

                    SqlParameter siteIdParam = new SqlParameter("@SITEID", SqlDbType.NVarChar, 5000);
                    siteIdParam.Value = siteid.ToString();
                    sqlParameters.Add(siteIdParam);
                    SqlParameter customerParam = new SqlParameter("@CUSTOMER", SqlDbType.NVarChar, 50);
                    customerParam.Value = Customer;
                    sqlParameters.Add(customerParam);

                    DataTable dtCustomer = CreamBellFramework.GetDataFromStoredProcedure("USP_CustomerWiseSaleReport", sqlParameters.ToArray());
                    if (dtCustomer.Rows.Count > 0)
                    {
                        ExcelExport excelExport = new ExcelExport();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        //if ("XLSX".CompareTo(rdbListExcelFileFormat.SelectedValue) == 0)
                        if (ucRadioButtonList.GetXLSX.Checked)
                        {
                            excelExport.ExportXLSX(Response, dtCustomer, "CustomerWiseSaleRegister", "CustomerWiseSaleRegister");
                        }

                        //if ("XLS".CompareTo(rdbListExcelFileFormat.SelectedValue) == 0)
                        if (ucRadioButtonList.GetXLS.Checked)
                        {
                            excelExport.ExportXLS(Response, dtCustomer, "CustomerWiseSaleRegister", "CustomerWiseSaleRegister");
                        }

                        //if ("XLSB".CompareTo(rdbListExcelFileFormat.SelectedValue) == 0)
                        if (ucRadioButtonList.GetXLSB.Checked)
                        {
                            string tempXlsbFilePath = string.Empty;
                            foreach (var file in Directory.GetFiles(Server.MapPath("xlfiles")))
                            {
                                try
                                {
                                    File.Delete(file);
                                }
                                catch { }
                            }
                            var tempFile = DateTime.Now.Ticks.ToString();
                            tempXlsbFilePath = Server.MapPath("xlfiles/tempXlsxFile_" + tempFile + ".xlsb");
                            excelExport.ExportXLSB(Response, dtCustomer, "CustomerWiseSaleRegister", "CustomerWiseSaleRegister", tempXlsbFilePath);
                        }

                        Response.Flush();
                        Response.End();
                    }



                    //using (SqlCommand cmd = new SqlCommand(query))
                    //{
                    //    using (SqlDataAdapter sda = new SqlDataAdapter())
                    //    {
                    //        cmd.Connection = obj.GetConnection();
                    //        sda.SelectCommand = cmd;
                    //        using (DataTable dt = new DataTable())
                    //        {
                    //        sda.Fill(dt);
                    //        using (XLWorkbook wb = new XLWorkbook())
                    //        {
                    //            wb.Worksheets.Add(dt, "CustomerWiseSaleRegister");

                    //            Response.Clear();
                    //            Response.Buffer = true;
                    //            Response.Charset = "";
                    //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //            Response.AddHeader("content-disposition", "attachment;filename=CustomerWiseSaleRegister.xlsx");
                    //            using (MemoryStream MyMemoryStream = new MemoryStream())
                    //            {
                    //                wb.SaveAs(MyMemoryStream);
                    //                MyMemoryStream.WriteTo(Response.OutputStream);
                    //                Response.Flush();
                    //                Response.End();
                    //            }
                    //        }
                    //      }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString() != "Thread was being aborted.")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "');", true);
                }

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