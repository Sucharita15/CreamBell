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
    public partial class frmNonBuyerReport : System.Web.UI.Page
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
                fillCategory();
                baseObj.FillSaleHierarchy();
                fillHOS();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    // DataView DtSaleHierarchy = (DataTable)HttpContext.Current.Session["SaleHierarchy"];
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
                            //  chkAll_CheckedChanged(null, null);

                        }
                        else if (test == "GM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            //    chkAll_CheckedChanged(null, null);
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
                            // chkAll_CheckedChanged(null, null);

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
                            // chkAll_CheckedChanged(null, null);

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

                            // chkAll_CheckedChanged(null, null);

                        }
                    }
                }

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    tclink.Width = "10%";
                    tclabel.Width = "90%";
                    Panel1.Visible = true;
                    LinkButton2.Visible = true;
                }
                else
                {
                    tclink.Width = "0%";
                    tclabel.Width = "100%";
                    Panel1.Visible = false;
                    LinkButton2.Visible = false;

                }
		        fillCustomeGroup();
            }
            //ddlCountry_SelectedIndexChanged(null, null);
        }
        private bool ValidateInput()
        {
            bool b;
            string siteid = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlSiteId.Items)
            {
                if (litem.Selected)
                {
                    if (siteid.Length == 0)
                        siteid = "" + litem.Value.ToString() + "";
                    else
                        siteid += "," + litem.Value.ToString() + "";
                }
            }
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide From Date and To Date";
            }
            else if (siteid == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Select Site";
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
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    ShowReportByFilter();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                //conn = new SqlConnection(objGlobal.GetConnectionString());
                //conn.Open();
                cmd = new SqlCommand();
                //cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "dbo.ACX_USP_NON_BUYER";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@STARTDATE", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@ENDDATE", Convert.ToDateTime(txtToDate.Text));
                cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));

                // state
                if (ddlState.SelectedIndex > 0 || ddlState.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@StateCode", ddlState.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StateCode", "");
                }
                // site
                string siteid = "";
                foreach (System.Web.UI.WebControls.ListItem litem in ddlSiteId.Items)
                {
                    if (litem.Selected)
                    {
                        if (siteid.Length == 0)
                            siteid = "" + litem.Value.ToString() + "";
                        else
                            siteid += "," + litem.Value.ToString() + "";
                    }
                }

                cmd.Parameters.AddWithValue("@SITEID", siteid);

                //if (ddlSiteId.SelectedIndex < 0)
                //{
                //    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                //    {
                //        string siteid = "";
                //        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                //        string AllSitesFromHierarchy = "";
                //        foreach (DataRow row in dt.Rows)
                //        {
                //            if (AllSitesFromHierarchy == "")
                //            {
                //                AllSitesFromHierarchy += row["Distributor"].ToString();
                //            }
                //            else
                //            {
                //                AllSitesFromHierarchy += "," + row["Distributor"].ToString();
                //            }
                //        }
                //        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                //        dt = baseObj.GetData(sqlstr1);
                //        foreach (DataRow row in dt.Rows)
                //        {
                //            if (siteid == "")
                //                siteid = Convert.ToString(row["Code"]);
                //            else
                //                siteid += "," + row["Code"];
                //        }
                //    }
                //    else
                //        cmd.Parameters.AddWithValue("@SiteId", "");
                //}

                //else if (ddlSiteId.SelectedIndex > 0)
                //{
                //    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                //}
                //else if (ddlSiteId.SelectedItem.Text != "All...")
                //{
                //    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                //}
                //else
                //{
                //    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                //    {
                //        string siteid = "";
                //        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                //        DataView view = new DataView(dt);
                //        if (ddlState.SelectedItem.Text != "All...")
                //        {
                //            view.RowFilter = "STATE='" + ddlState.SelectedItem.Value.Trim() + "'";
                //        }
                //        DataTable uniqueCols = view.ToTable(true, "Distributor", "DistributorName");
                //        string AllSitesFromHierarchy = "";
                //        foreach (DataRow row in uniqueCols.Rows)
                //        {
                //            if (AllSitesFromHierarchy == "")
                //            {
                //                AllSitesFromHierarchy += row["Distributor"].ToString();
                //            }
                //            else
                //            {
                //                AllSitesFromHierarchy += "," + row["Distributor"].ToString();
                //            }
                //        }
                //        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                //        dt = baseObj.GetData(sqlstr1);
                //        foreach (DataRow row in dt.Rows)
                //        {
                //            if (siteid == "")
                //                siteid = Convert.ToString(row["Code"]);
                //            else
                //                siteid += "," + row["Code"];
                //        }
                //        cmd.Parameters.AddWithValue("@SiteId", siteid);                         
                //    }
                //    else
                //        cmd.Parameters.AddWithValue("@SiteId", "");

                //}

                if (drpCategory.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@Category", drpCategory.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Category", "");
                }
                if (drpSubCategory.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@SubCategory", drpSubCategory.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SubCategory", "");
                }
                if (drpProduct.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@Product", drpProduct.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Product", "");
                }
                if (drpCustGroup.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@CustGroup", drpCustGroup.SelectedItem.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CustGroup", "");
                }
                if (DDLBusinessUnit.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", DDLBusinessUnit.SelectedItem.Value.ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }
                dtDataByfilter = new DataTable();
                cmd.Connection = objGlobal.GetConnection();
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
                objGlobal.CloseSqlConnection();
                //if (conn != null)
                //{
                //    if (conn.State.ToString() == "Open") { conn.Close(); }
                //}
            }
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
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");

                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    if (ddlState.SelectedItem.Text == "All...")
                    {
                        DataSetParameter.Rows[0]["StateCode"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "All...")
                        {
                            DataSetParameter.Rows[0]["Distributor"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Text;
                        }
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\NonBuyerReport.rdl");
                    // ReportParameter FromDate = new ReportParameter();
                    //FromDate.Name = "FromDate";
                    //FromDate.Values.Add(txtFromDate.Text);
                    //ReportParameter ToDate = new ReportParameter();
                    //ToDate.Name = "ToDate";
                    //ToDate.Values.Add(txtToDate.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet1", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    this.ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;
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

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlState.SelectedItem == null)
            {
            }
            else
            {
                if (ddlState.SelectedItem.Text == "All...")
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

                    //DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");

                    DataView view = new DataView(dt);
                    DataTable distinctValues = view.ToTable(true, "Distributor", "DistributorName");
                    //distinctValues.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
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
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                    dt = baseObj.GetData(sqlstr1);
                    ddlSiteId.DataSource = dt;
                    ddlSiteId.DataTextField = "Name";
                    ddlSiteId.DataValueField = "Code";
                    ddlSiteId.DataBind();
                    //ddlSiteId.Items.Add("All...");
                    //ddlSiteId.Items.Insert(0, "All...");
                }

                else
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode != null)
                    {
                        ddlSiteId.Items.Clear();
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where STATECODE = '" + ddlState.SelectedItem.Value + "' AND  UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000'";
                        //ddlSiteId.Items.Insert(0, "All...");
                        DataTable dt = baseObj.GetData(sqlstr1);
                        ddlSiteId.DataSource = dt;
                        ddlSiteId.DataTextField = "Name";
                        ddlSiteId.DataValueField = "Code";
                        ddlSiteId.DataBind();
                        //baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                    }
                    else
                    {
                        ddlSiteId.Items.Clear();
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            // DataTable dt = (DataTable)Session["SaleHierarchy"];
                            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            if(ddlState.SelectedItem.Value.Trim() != "All...")
                                dt.DefaultView.RowFilter = "STATE='" + ddlState.SelectedItem.Value.Trim() + "'";

                            DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                            //uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
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
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name as Name,Name  as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where STATECODE='"+ ddlState.SelectedItem.Value.Trim() + "' AND UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                            dt = baseObj.GetData(sqlstr1);
                            ddlSiteId.DataSource = dt;
                            ddlSiteId.DataTextField = "Name";
                            ddlSiteId.DataValueField = "Code";
                            ddlSiteId.DataBind();
                            //ddlSiteId.Items.Add("All...");
                            //ddlSiteId.Items.Insert(0, "All...");
                        }
                        else
                        {
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where SITEID = '" + Session["SiteCode"].ToString() + "' AND UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000'";
                            //ddlSiteId.Items.Add("All...");

                            //baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                            DataTable dt = baseObj.GetData(sqlstr1);
                            ddlSiteId.DataSource = dt;
                            ddlSiteId.DataTextField = "Name";
                            ddlSiteId.DataValueField = "Code";
                            ddlSiteId.DataBind();
                            ddlSiteId_SelectedIndexChanged(null, null);
                        }
                    }
                }
            }
        }
        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
        //        ddlSiteId.Items.Add("All...");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlSiteId.Items.Clear();
        //        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
        //        {
        //            DataTable dt = (DataTable)Session["SaleHierarchy"];
        //            DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "distributor Name");
        //            ddlSiteId.DataSource = uniqueCols;
        //            ddlSiteId.DataTextField = "distributor name";
        //            ddlSiteId.DataValueField = "distributor";
        //            ddlSiteId.DataBind();

        //        }
        //        else
        //        {
        //            string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
        //            //ddlSiteId.Items.Add("All...");
        //            baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //        }
        //    }
        //}
        protected void fillSiteAndState(DataTable dt)
        {
            string sqlstr = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATEWNAME");
                   // dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                    ddlState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                    dr[0] = "All...";
                    dr[1] = "All...";
                    dtState.Rows.InsertAt(dr, 0);
                    ddlState.DataSource = dtState;
                    ddlState.DataTextField = "STATEWNAME";
                    ddlState.DataValueField = "STATE";
                    ddlState.DataBind();
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name  Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    //ddlState.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                }
            }
            else
            {
                sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
            }
            if (ddlState.Items.Count == 1 || ddlState.SelectedItem.Text == "All...")
            {
                ddlState.SelectedIndex = 1;
                ddlCountry_SelectedIndexChanged(null, null);
            }
        }
        protected void fillCategory()
        {
            drpCategory.Items.Clear();
            string sqlstr1 = @"select distinct(Product_Group)  from ax.inventtable  order by Product_Group";
            drpCategory.Items.Add("All...");
            baseObj.BindToDropDown(drpCategory, sqlstr1, "Product_Group", "Product_Group");
        }
        protected void fillSubCategory()
        {
            if (drpCategory.SelectedIndex > 0)
            {
                drpSubCategory.Items.Clear();
                string sqlstr1 = @"select distinct(Product_Subcategory) from ax.inventtable where Product_Group='" + drpCategory.SelectedItem.Text + "' order by Product_Subcategory";
                drpSubCategory.Items.Add("All...");
                baseObj.BindToDropDown(drpSubCategory, sqlstr1, "Product_Subcategory", "Product_Subcategory");
            }

        }
        protected void fillProduct()
        {
            if (drpSubCategory.SelectedIndex > 0)
            {
                drpProduct.Items.Clear();
                string sqlstr1 = @"select distinct(ItemId),ItemId+'-'+Product_Name as ItemName from ax.inventtable
                                 where Product_Group='" + drpCategory.SelectedItem.Value + "' and Product_Subcategory='" + drpSubCategory.SelectedItem.Value + "' order by ItemId";
                drpProduct.Items.Add("All...");
                baseObj.BindToDropDown(drpProduct, sqlstr1, "ItemName", "ItemId");
            }
        }
        protected void fillCustomeGroup()
        {
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                //DataTable dt = (DataTable)Session["SaleHierarchy"];
                // DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM);
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                DataTable uniqueCols = dt.DefaultView.ToTable(true, "CUSTGROUP", "CUSTGROUPDESC");

                drpCustGroup.DataSource = uniqueCols;
                drpCustGroup.DataTextField = "CUSTGROUPDESC";
                drpCustGroup.DataValueField = "CUSTGROUP";
                drpCustGroup.DataBind();
                drpCustGroup.Items.Insert(0, "All...");

            }
            else
            {
                drpCustGroup.Items.Clear();
                string sqlstr1 = @"Select distinct(CustGroup_Code),CustGroup_Name from [ax].[ACXCUSTGROUPMASTER] Order BY CustGroup_Code";
                drpCustGroup.Items.Add("All...");
                baseObj.BindToDropDown(drpCustGroup, sqlstr1, "CustGroup_Name", "CustGroup_Code");
            }
        }
        protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillSubCategory();
        }
        protected void drpSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillProduct();
        }
        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void drpCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string siteid = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlSiteId.Items)
            {
                if (litem.Selected)
                {
                    if (siteid.Length == 0)
                        siteid = "'" + litem.Value.ToString() + "'";
                    else
                        siteid += ",'" + litem.Value.ToString() + "'";
                }
            }
            if (siteid != string.Empty)
            {
                string query = "select DISTINCT bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID IN (" + siteid + ")";
                DDLBusinessUnit.Items.Clear();
                DDLBusinessUnit.Items.Add("All...");
                baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
            }
            else
            {
                DDLBusinessUnit.Items.Clear();
                DDLBusinessUnit.Items.Add("All...");
            }
        }
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                Panel1.Visible = false;
                LinkButton2.Text = "Show sales person filter";
                // Panel1.Height = 0;
            }

            else if (Panel1.Visible == false)
            {
                Panel1.Visible = true;
                LinkButton2.Text = "hide sales person filter";
            }
        }
        protected void fillHOS()
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

            }
            lstHOS_SelectedIndexChanged(null, null);
            fillSiteAndState(dtHOS);
        }

        protected void lstHOS_SelectedIndexChanged(object sender, EventArgs e)
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
                // chkListGM.Items.Clear();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }


        protected void lstVP_SelectedIndexChanged(object sender, EventArgs e)
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


        protected void lstGM_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void lstDGM_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void lstZM_SelectedIndexChanged(object sender, EventArgs e)
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
        protected void lstASM_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            fillSiteAndState(dt);
            uppanel.Update();
            ddlCountry_SelectedIndexChanged(null, null);

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
                // chkListDGM.Items.Clear();
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
                //     chkListASM.Items.Clear();
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
                //     chkListASM.Items.Clear();
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
            // chkListASM.DataSource = null;
            ddlCountry_SelectedIndexChanged(null, null);
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
    }
}