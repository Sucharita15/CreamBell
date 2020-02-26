using ClosedXML.Excel;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExcelLib = Microsoft.Office.Interop.Excel;
using EasyXLS;
using System.Configuration;

namespace CreamBell_DMS_WebApps
{
    public partial class frmSaleOrderDetails : System.Web.UI.Page
    {
        bool USE_EASYXLS = false;
        string CURRENT_WORKING_DIRECTRY_FOR_XL = "";

        Stopwatch stopwatch = new Stopwatch();
        static int logCounter = 0;
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlConnection conn = null;
        SqlCommand cmd;
        const string PAGE_NAME = "frmSaleOrderDetails";
        protected void Page_Load(object sender, EventArgs e)
        {
            USE_EASYXLS = Convert.ToBoolean(ConfigurationManager.AppSettings["EasyXLS"].ToString());
            CURRENT_WORKING_DIRECTRY_FOR_XL = ConfigurationManager.AppSettings["CURRENT_WORKING_DIRECTRY_FOR_XL"].ToString();

            stopwatch.Start();
            //var message = $"SaleOrderDetail- PageLoad -Start: {stopwatch.Elapsed}";
            //CreamBellFramework.LogMessage("Start", PAGE_NAME, "Page_Load", stopwatch.Elapsed, logCounter++);

            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                CalendarExtender1.StartDate = Convert.ToDateTime("01-July-2017");
                txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
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
                    Panel1.Visible = true;
                }
                //if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                //{
                //    //tclabel.Width = "90%";

                //}
                else
                {
                    //tclabel.Width = "100%";
                    Panel1.Visible = false;
                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "0" && Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    ShowCustomerMaster();
                }
            }

            //var message1 = $"SaleOrderDetail- PageLoad -Completed: {stopwatch.Elapsed}";
            //CreamBellFramework.LogMessage("Completed", PAGE_NAME, "Page_Load", stopwatch.Elapsed, logCounter++);
        }

        //protected void Page_UnLoad(object sender, EventArgs e)
        //{
        //    //var message1 = $"SaleOrderDetail- PageUnLoad -Completed: {stopwatch.Elapsed}";
        //    CreamBellFramework.LogMessage("Completed", PAGE_NAME, "Page_UnLoad", stopwatch.Elapsed, logCounter++);
        //}

        private void ShowCustomerMaster()
        {
            stopwatch.Restart();
            CreamBellFramework.LogMessage("Started", PAGE_NAME, "ShowCustomerMaster", stopwatch.Elapsed, logCounter++);
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

            //if (lstSiteId.SelectedValue == string.Empty)
            //{
            //    gridViewCustomers.DataSource = null;
            //    gridViewCustomers.DataBind();
            //    ViewState["dtCustomer"] = null;
            //    string message = "alert('Please Select The SiteID !');";
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            //}
            string siteid, StateCode;

            int count = 0;
            string FromDate = txtFromDate.Text;
            string ToDate = txtToDate.Text;
            siteid = StateCode = "";
            foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
            {
                if (litem.Selected)
                {
                    count += 1;
                    if (StateCode.Length == 0)
                        StateCode = "" + litem.Value.ToString() + "";
                    else
                        StateCode += "," + litem.Value.ToString() + "";
                }
            }
            foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
            {
                if (litem.Selected)
                {
                    count += 1;
                    if (siteid.Length == 0)
                        siteid = "" + litem.Value.ToString() + "";
                    else
                        siteid += "," + litem.Value.ToString() + "";
                }
            }
            //if (count > 5)
            //{
            //    // string message = "alert('Click On Export to Excel Only.If more than 5 Distributor Selected!');";
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Click On Export To Excel Only.If More Than 5 Distributor Selected!');", true);
            //    return;
            //}

            string query = "EXEC ACX_USP_OpenSaleOrder '" + StateCode + "','" + siteid.ToString() + "','" + Session["DATAAREAID"].ToString() + "','" + FromDate + "','" + ToDate + "','" + Convert.ToString(Session["USERID"]) + "','" + Convert.ToString(Session["LOGINTYPE"]) + "'";
            //cmd = new SqlCommand();
            //cmd.Connection = conn;
            //cmd.CommandTimeout = 500;
            //cmd.CommandType = CommandType.StoredProcedure;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            SqlParameter stateCodeParam = new SqlParameter("@StateCode", SqlDbType.NVarChar, 5000);
            stateCodeParam.Value = StateCode;
            sqlParameters.Add(stateCodeParam);
            SqlParameter siteCodeParam = new SqlParameter("@Site_Code", SqlDbType.NVarChar, 5000);
            siteCodeParam.Value = siteid.ToString();
            sqlParameters.Add(siteCodeParam);
            SqlParameter dataAreaIdParam = new SqlParameter("@DATAAREAID", SqlDbType.NVarChar, 4);
            dataAreaIdParam.Value = Session["DATAAREAID"].ToString();
            sqlParameters.Add(dataAreaIdParam);
            SqlParameter startDateParam = new SqlParameter("@StartDate", SqlDbType.Date);
            startDateParam.Value = FromDate;
            sqlParameters.Add(startDateParam);
            SqlParameter endDateParam = new SqlParameter("@EndDate", SqlDbType.Date);
            endDateParam.Value = ToDate;
            sqlParameters.Add(endDateParam);
            SqlParameter userCodeParam = new SqlParameter("@UserCode", SqlDbType.NVarChar, 50);
            userCodeParam.Value = Session["USERID"].ToString();
            sqlParameters.Add(userCodeParam);
            SqlParameter userTypeParam = new SqlParameter("@UserType", SqlDbType.NVarChar, 50);
            userTypeParam.Value = Session["LOGINTYPE"].ToString();
            sqlParameters.Add(userTypeParam);
            CreamBellFramework.LogMessage("DB Call Started", PAGE_NAME, "ShowCustomerMaster", stopwatch.Elapsed, logCounter++);
            DataTable dtCustomer = CreamBellFramework.GetDataFromStoredProcedure("ACX_USP_OpenSaleOrder", sqlParameters.ToArray());
            CreamBellFramework.LogMessage($"DB Call Completed Records- {dtCustomer.Rows.Count}", PAGE_NAME, "ShowCustomerMaster", stopwatch.Elapsed, logCounter++);

            //DataTable dtCustomer = obj.GetData(query);
            if (dtCustomer.Rows.Count > 0)
            {
                gridViewCustomers.DataSource = dtCustomer;
                //gridViewCustomers.Columns[0].Visible = false;           //Column Index 1 for Customer Code 
                gridViewCustomers.DataBind();
                ViewState["dtCustomer"] = dtCustomer;
                gridViewCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            else
            {
                gridViewCustomers.Columns[1].Visible = false;       //Column Index 1 for Customer Code 
                gridViewCustomers.DataBind();
            }

            CreamBellFramework.LogMessage("GridBinding Completed", PAGE_NAME, "ShowCustomerMaster", stopwatch.Elapsed, logCounter++);
        }

        private bool ValidateInput()
        {
            bool value = true;

            if (lstState.SelectedValue == string.Empty)
            {
                value = false;
                string message = "alert('Please Select The SiteID  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }
            return value;
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                gridViewCustomers.DataSource = null;
                gridViewCustomers.DataBind();
                bool b = ValidateInput();
                if (b == true)
                {
                    ShowCustomerMaster();
                }
                uppanel.Update();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //protected void btnSearchCustomer_Click(object sender, EventArgs e)
        //{
        //    if (txtSearch.Text == string.Empty)
        //    {
        //        this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please provide the Searching Keyword !');", true);
        //        txtSearch.Focus();
        //        gridViewCustomers.DataSource = ViewState["dtCustomer"];
        //        gridViewCustomers.DataBind();
        //    }
        //    else
        //    {
        //        ShowCustomerByFilter();
        //    }
        //}

        protected void fillSiteAndState(DataTable dt)
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
                    //ddlState.Items.Add("Select...");
                    // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
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
                ddlCountry_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
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
                lstHOS_SelectedIndexChanged(null, null);
            }
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
                //chkListGM.Items.Clear();
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

        protected void gridViewCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 0)
                    e.Row.Style.Add("height", "50px");
            }
        }

        protected void gridViewCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewCustomers.PageIndex = e.NewPageIndex;
            //ShowCustomerMaster();
        }

        private void PrepareForExport(Control ctrl)
        {
            //iterate through all the grid controls
            foreach (Control childControl in ctrl.Controls)
            {
                //if the control type is link button, remove it
                //from the collection
                if (childControl.GetType() == typeof(LinkButton))
                {
                    ctrl.Controls.Remove(childControl);
                }
                //if the child control is not empty, repeat the process
                // for all its controls
                else if (childControl.HasControls())
                {
                    PrepareForExport(childControl);
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        private void ExportToExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition",
                                  "attachment;filename=SaleOrderDetailsReport.xls");
            Response.Charset = String.Empty;
            Response.ContentType = "application/ms-excel";
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            gridViewCustomers.RenderControl(HtmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        protected void btnExport2Excel_Click(object sender, EventArgs e)
        {
            ExportToExcelNew();
        }


        private void ExportToExcelNew()
        {
            stopwatch.Restart();
            CreamBellFramework.LogMessage("ExportToExcelNew -Started", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);

            //string tempXlsxFilePath = string.Empty;
            string tempXlsbFilePath = string.Empty;
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

                    if (lstState.SelectedValue == string.Empty)
                    {
                        string message = "alert('Please Select The SiteID  !');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                    }
                    string siteid, StateCode;
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;
                    siteid = StateCode = "";
                    foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                    {
                        if (litem.Selected)
                        {
                            if (StateCode.Length == 0)
                                StateCode = "" + litem.Value.ToString() + "";
                            else
                                StateCode += "," + litem.Value.ToString() + "";
                        }
                    }
                    foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
                    {
                        if (litem.Selected)
                        {
                            if (siteid.Length == 0)
                                siteid = "" + litem.Value.ToString() + "";
                            else
                                siteid += "," + litem.Value.ToString() + "";
                        }
                    }

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter stateCodeParam = new SqlParameter("@StateCode", SqlDbType.NVarChar, 5000);
                    stateCodeParam.Value = StateCode;
                    sqlParameters.Add(stateCodeParam);
                    SqlParameter siteCodeParam = new SqlParameter("@Site_Code", SqlDbType.NVarChar, 5000);
                    siteCodeParam.Value = siteid.ToString();
                    sqlParameters.Add(siteCodeParam);
                    SqlParameter dataAreaIdParam = new SqlParameter("@DATAAREAID", SqlDbType.NVarChar, 4);
                    dataAreaIdParam.Value = Session["DATAAREAID"].ToString();
                    sqlParameters.Add(dataAreaIdParam);
                    SqlParameter startDateParam = new SqlParameter("@StartDate", SqlDbType.Date);
                    startDateParam.Value = FromDate;
                    sqlParameters.Add(startDateParam);
                    SqlParameter endDateParam = new SqlParameter("@EndDate", SqlDbType.Date);
                    endDateParam.Value = ToDate;
                    sqlParameters.Add(endDateParam);
                    SqlParameter userCodeParam = new SqlParameter("@UserCode", SqlDbType.NVarChar, 50);
                    userCodeParam.Value = Session["USERID"].ToString();
                    sqlParameters.Add(userCodeParam);
                    SqlParameter userTypeParam = new SqlParameter("@UserType", SqlDbType.NVarChar, 50);
                    userTypeParam.Value = Session["LOGINTYPE"].ToString();
                    sqlParameters.Add(userTypeParam);

                    CreamBellFramework.LogMessage("DB Call Started", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                    //DataTable dt = CreamBellFramework.GetDataFromStoredProcedure("ACX_USP_OpenSaleOrder", sqlParameters.ToArray());
                    DataSet dataSet = CreamBellFramework.GetDataSetFromStoredProcedure("ACX_USP_OpenSaleOrder", sqlParameters.ToArray());
                    DataTable dt = dataSet.Tables[0];
                    CreamBellFramework.LogMessage($"DB Call Completed Records- {dt.Rows.Count}", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                    //sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        ExcelExport excelExport = new ExcelExport();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";

                        if (ucRadioButtonList.GetXLS.Checked)
                        {
                            CreamBellFramework.LogMessage("XSL Call started", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                            if (USE_EASYXLS)
                            {
                                Response.AppendHeader("content-disposition", "attachment; filename=SaleOrderDetailsReport.xls");
                                Response.ContentType = "application/octetstream";
                                Response.Clear();
                                EasyXLS.ExcelDocument workbook = new EasyXLS.ExcelDocument();
                                workbook.easy_WriteXLSFile_FromDataSet(
                                         Response.OutputStream,
                                         dataSet,
                                         new EasyXLS.ExcelAutoFormat(EasyXLS.Constants.Styles.AUTOFORMAT_NONE),
                                         "SaleOrderDetailsReport");
                                workbook.Dispose();
                            }
                            else
                            {
                                excelExport.ExportXLS(Response, dt, "Customers", "SaleOrderDetailsReport");
                            }
                            CreamBellFramework.LogMessage("XSL Call Completed", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                        }

                        //foreach (var file in Directory.GetFiles(Server.MapPath("xlfiles")))
                        //{
                        //    try
                        //    {
                        //        File.Delete(file);
                        //    }
                        //    catch { }
                        //}

                        if (ucRadioButtonList.GetXLSX.Checked)
                        {
                            CreamBellFramework.LogMessage("XSLX Call started", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                            if (USE_EASYXLS)
                            {
                                Response.AppendHeader("content-disposition", "attachment; filename=SaleOrderDetailsReport.xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.Clear();
                                EasyXLS.ExcelDocument workbook = new EasyXLS.ExcelDocument();
                                //tempXlsbFilePath = Server.MapPath("xlfiles/SaleOrderDetailsReport.xlsx");
                                Environment.CurrentDirectory = CURRENT_WORKING_DIRECTRY_FOR_XL;
                                workbook.easy_WriteXLSXFile_FromDataSet(
                                         Response.OutputStream,
                                         dataSet,
                                         new EasyXLS.ExcelAutoFormat(EasyXLS.Constants.Styles.AUTOFORMAT_NONE),
                                         "SaleOrderDetailsReport");
                                workbook.Dispose();
                                //Response.TransmitFile(tempXlsbFilePath);
                            }
                            else
                            {
                                excelExport.ExportXLSX(Response, dt, "Customers", "SaleOrderDetailsReport");
                            }
                            CreamBellFramework.LogMessage("XSLX Call Completed", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                        }

                        if (ucRadioButtonList.GetXLSB.Checked)
                        {
                            CreamBellFramework.LogMessage("XSLB Call started", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);

                            if (USE_EASYXLS)
                            {
                                Response.AppendHeader("content-disposition", "attachment; filename=SaleOrderDetailsReport.xlsb");
                                Response.ContentType = "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
                                Response.Clear();
                                EasyXLS.ExcelDocument workbook = new EasyXLS.ExcelDocument();
                                Environment.CurrentDirectory = CURRENT_WORKING_DIRECTRY_FOR_XL;
                                Environment.CurrentDirectory = @"E:\Vijay\CreamBell_Latest\CreamBell\easyxl";
                                workbook.easy_WriteXLSBFile_FromDataSet(
                                        Response.OutputStream,
                                         dataSet,
                                         new EasyXLS.ExcelAutoFormat(EasyXLS.Constants.Styles.AUTOFORMAT_NONE),
                                         "SaleOrderDetailsReport");
                                workbook.Dispose();
                            }
                            else
                            {
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
                                excelExport.ExportXLSB(Response, dt, "Customers", "SaleOrderDetailsReport", tempXlsbFilePath);
                            }
                            CreamBellFramework.LogMessage("XSLB Call Completed", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                        }

                        Response.Flush();
                        Response.End();
                        CreamBellFramework.LogMessage("Response.End() Completed", PAGE_NAME, "ExportToExcelNew", stopwatch.Elapsed, logCounter++);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        //private void ConvertToXLSBFile(string filePath, string tempXlsbFilePath)
        //{
        //    ExcelLib.Application excelApplication = new ExcelLib.Application();
        //    ExcelLib.Workbook workbook = excelApplication.Workbooks.Open(filePath, ExcelLib.XlUpdateLinks.xlUpdateLinksNever, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    workbook.SaveAs(tempXlsbFilePath, ExcelLib.XlFileFormat.xlExcel12, Type.Missing, Type.Missing, Type.Missing, Type.Missing, ExcelLib.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    workbook.Close(false, Type.Missing, Type.Missing);
        //    excelApplication.Quit();
        //}


    }
}