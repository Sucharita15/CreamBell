
using Elmah;
using Microsoft.Reporting.WebForms;
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
    public partial class frmBulkBuyerReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        string DistributorList = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)

            {
                
                chkCustomerName.Items.Clear();
                chkCustomerName.Items.Add("All...");
                //Fillstate();
                baseObj.FillSaleHierarchy();
                fillCustomeGroup();
                fillHOS();

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {

                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    if (dt.Rows.Count>0)
                    { 
                    var dr_row = dt.AsEnumerable();
                    var test = (from r in dr_row
                                select r.Field<string>("SALEPOSITION")).First<string>();
                                                                     
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
                        }
                    }

                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    tclink.Width = "10%";
                    tclabel.Width = "90%";

                    Panel1.Visible = true;
                    LinkButton1.Visible = true;
                }
                else
                {
                    tclink.Width = "0%";
                    tclabel.Width = "100%";

                    Panel1.Visible = false;
                    LinkButton1.Visible = false;
                    LinkButton1.Enabled = false;

                }
                //chkListState_SelectedIndexChanged(null, null);
            }
        }

        protected void Fillstate()
        {
            string sqlstr = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dt = new DataTable();
                    dt = new DataTable();
                    
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATECODE", "STATENAME");
                    chkListState.Items.Clear();
                  //  DataRow dr = dtState.NewRow();

                    // sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    sqlstr = "Select distinct I.SITEID, LS.NAME from ax.INVENTSITE I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where  I.STATECODE <>'' AND I.SITEID ='" + Convert.ToString(Session["SiteCode"]) + "'";
                    dt = baseObj.GetData(sqlstr);
                    chkListState.Items.Add("All...");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkListState.DataSource = dt;
                        chkListState.DataTextField = "SITEID";
                        chkListState.DataValueField = "NAME";
                        chkListState.DataBind();
                    }
                    if (chkListSite.Items.Count == 1)
                    {
                        CheckBox8.Visible = false;
                        chkListSite.Items[0].Selected = true;
                        // lstDIS_SelectedIndexChanged(null, null);
                    }
                }
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
                        chkListState.Items.Clear();
                        DataRow dr = dtState.NewRow();
                        //dr[0] = "All...";
                        //dr[1] = "All...";

                        //dtState.Rows.InsertAt(dr, 0);
                        chkListState.DataSource = dtState;
                        chkListState.DataTextField = "STATENAMES";
                        chkListState.DataValueField = "STATE";
                        chkListState.DataBind();
                    }
                    else
                    {
                        sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name  Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                        DataTable dt2 = new DataTable();
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.Connection = baseObj.GetConnection();
                        cmd1.CommandText = sqlstr;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                        sda.Fill(dt2);
                        //chkListState.Items.Add("All...");
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            chkListState.DataSource = dt2;
                            chkListState.DataTextField = "NAME";
                            chkListState.DataValueField = "Code";
                            chkListState.DataBind();
                        }

                    }
                }
                else
                {

                    sqlstr = "Select Distinct I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                    chkListState.Items.Add("Select...");
                    dt = baseObj.GetData(sqlstr);
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "StateName";
                    chkListState.DataValueField = "StateCode";
                    chkListState.DataBind();
                    DataTable dt2 = new DataTable();
                    string sqlstr2 = "Select Distinct I.SiteId,I.SiteId +'-'+ I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                    chkListSite.Items.Add("Select...");
                    dt2 = baseObj.GetData(sqlstr2);
                    chkListSite.DataSource = dt2;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();

                }

                if (chkListState.Items.Count == 2)
                {
                    //chkListState.SelectedIndex = 1;
                    // ddlCountry_SelectedIndexChanged(null, null);
                }


                if (chkListState.Items.Count == 1)
                {
                    chkListState.Items[0].Selected = true;
                    ddlCountry_SelectedIndexChanged(null, null);
                    //chkListState_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void fillCustomeGroup()
        {
            try {
                chkListCustomerGroup.Items.Clear();
                DataTable dt = new DataTable();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //DataTable dt = (DataTable)Session["SaleHierarchy"];
                    // DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM);
                    dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols = dt.DefaultView.ToTable(true, "CUSTGROUP", "CUSTGROUPDESC");
                    DataView dv = new DataView(uniqueCols);
                    //dv.RowFilter = "CUSTGROUP in('CG0002','CG0003')"; // query example = "CUSTGROUP in('CG0002','CG0003')"
                    chkListCustomerGroup.DataSource = dv;
                    chkListCustomerGroup.DataTextField = "CUSTGROUPDESC";
                    chkListCustomerGroup.DataValueField = "CUSTGROUP";
                    chkListCustomerGroup.DataBind();
                    //chkListCustomerGroup.Items.Insert(0, "All...");
                }
                else
                {
                    chkListCustomerGroup.Items.Clear();
                    string sqlstr1 = @"Select distinct(CustGroup_Code),CustGroup_Name from [ax].[ACXCUSTGROUPMASTER] Order BY CustGroup_Code";
                    dt = baseObj.GetData(sqlstr1);
                    chkListCustomerGroup.Items.Clear();
                    chkListCustomerGroup.DataSource = dt;
                    chkListCustomerGroup.DataTextField = "CustGroup_Name";
                    chkListCustomerGroup.DataValueField = "CustGroup_Code";
                    chkListCustomerGroup.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    DataTable dt = new DataTable();
                    dt = new DataTable();

                    chkListSite.Items.Clear();
                    if (chkListState.SelectedItem == null)
                    {
                    }
                    else
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +'-'+ NAME as NAME from [ax].[INVENTSITE] where STATECODE = '" + chkListState.SelectedItem.Value + "'";
                        dt = baseObj.GetData(sqlstr1);
                        //chkListState.Items.Add("All...");

                        chkListSite.DataSource = dt;
                        chkListSite.DataTextField = "NAME";
                        chkListSite.DataValueField = "Code";
                        chkListSite.DataBind();
                    }
                }
                else
                {
                    chkListSite.Items.Clear();
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        //DataTable dt = (DataTable)Session["SaleHierarchy"];
                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                        DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                        uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                        chkListSite.DataSource = uniqueCols;
                        chkListSite.DataTextField = "Name";
                        chkListSite.DataValueField = "Distributor";
                        chkListSite.DataBind();
                        //ddlSiteId.Items.Add("All...");
                        // chkListSite.Items.Insert(0, "All...");
                    }
                    else
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +'-'+ NAME as NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                        DataTable dt = new DataTable();
                        dt = new DataTable();

                        dt = baseObj.GetData(sqlstr1);
                        chkListSite.DataSource = dt;
                        chkListSite.DataTextField = "NAME";
                        chkListSite.DataValueField = "CODE";
                        chkListSite.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSite.Items.Count < 2)
            {
                chkListSite.Items[0].Selected = true;
            }
            fillCustomeGroup();
        }

        protected void chkListCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerName();
        }

        private void FillCustomerName()
        {
            try
            {
                string strCustomerGroupName = "";
                string strSubDistCustGroup = "";
                #region Get All selected CustomerGroup
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerGroupName == "")
                        {
                            strCustomerGroupName += "'" + item.Value.ToString() + "'";
                        }
                        else
                        {
                            strCustomerGroupName += ",'" + item.Value.ToString() + "'";
                        }
                        if (item.Value == "CG0004")
                        {
                            strSubDistCustGroup = "'CG0004'";
                        }
                        else
                        {
                            strSubDistCustGroup = "";
                        }
                    }
                }
                #endregion

                #region get all selected site
                string SiteList = "";
                foreach (ListItem site in chkListSite.Items)
                {
                    if (site.Selected)
                    {
                        if (SiteList == "")
                        {
                            SiteList += "'" + site.Value.ToString() + "'";
                        }
                        else
                        {
                            SiteList += ",'" + site.Value.ToString() + "'";
                        }
                    }
                }
                #endregion

                if (strCustomerGroupName.Length > 0 && SiteList.Length > 0)
                {
                    DataTable dt = new DataTable();
                    //string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
                    string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code in (" + SiteList + ") ";
                    if (strSubDistCustGroup != "")
                    {
                        sqlstr += "Union Select distinct CU.Customer_Code+'-'+CU.Customer_Name as Name,CU.Customer_Code From ax.ACX_SDLINKING SD Left Join ax.ACXCUSTMASTER CU on CU.Customer_Code=SD.SubDistributor where SD.Other_site in (" + SiteList + ") And CU.CUST_GROUP='CG0004' Order By Name";
                    }
                    else
                    {
                        sqlstr += " Order By Name";
                    }
                    dt = new DataTable();
                    dt = baseObj.GetData(sqlstr);
                    chkCustomerName.Items.Clear();
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //chkCustomerName.DataSource = dt;
                    //chkCustomerName.DataTextField = "NAME";
                    //chkCustomerName.DataValueField = "Customer_Code";
                    //chkCustomerName.DataBind();

                    chkCustomerName.Items.Clear();
                    chkCustomerName.Items.Add("All...");
                    baseObj.BindToDropDown(chkCustomerName, sqlstr, "NAME", "Customer_Code");
                    //}
                    if (dt.Rows.Count == 0)
                    {
                        chkCustomerName.Items.Clear();
                        chkCustomerName.Items.Add("All...");
                    }
                }
                else
                {
                    chkCustomerName.Items.Clear();
                    chkCustomerName.Items.Add("All...");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateInput()
        {
            string StateList = string.Empty;
            foreach (ListItem item in chkListState.Items)
            {
                if (item.Selected)
                {
                    if (StateList == "")
                    {
                        StateList += "" + item.Value.ToString() + "";
                    }
                    else
                    {
                        StateList += "," + item.Value.ToString() + "";
                    }
                }
            }
            foreach (ListItem item in chkListSite.Items)
            {
                if (item.Selected)
                {
                    if (DistributorList == "")
                    {
                        DistributorList += "" + item.Value.ToString() + "";
                    }
                    else
                    {
                        DistributorList += "," + item.Value.ToString() + "";
                    }
                }
            }
            bool b;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide From Date and To Date";
            }
            else if(StateList.Length == 0 || StateList == "")
            {
                b = false;
                LblMessage.Text = "Please Select Atlest One State";
            }
            else if (DistributorList.Length == 0 || DistributorList == "")
            {
                b = false;
                LblMessage.Text = "Please Select Atlest One Site";
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }


            return b;
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }

        protected void FillSite()
        {
            try {
                string StateList = "";
                foreach (ListItem item in chkListState.Items)
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
                        sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                    }
                    else
                    {
                        sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    }

                    dt = new DataTable();
                    chkListSite.Items.Clear();
                    dt = baseObj.GetData(sqlstr1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkListSite.DataSource = dt;
                        chkListSite.DataTextField = "SiteName";
                        chkListSite.DataValueField = "SiteId";
                        chkListSite.DataBind();
                    }
                    if (chkListSite.Items.Count == 1)
                    {
                        chkListSite.Items[0].Selected = true;
                    }
                    if (chkListSite.Items.Count == 0)
                    {
                        chkListState_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    chkListSite.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    ExportToExcelByFilter();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ExportToExcelByFilter()
        {
            try {
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
                    query = "USP_BULKBUYERREPORT";
                    cmd.CommandText = query;
                    //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyy"));
                    cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyy"));
                    cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                    cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));

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
                    if (DistributorList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@SiteCode", DistributorList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SiteCode", "");
                    }

                    if (CustGroupList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@Cust_Group", CustGroupList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Cust_Group", "");
                    }

                    if (chkCustomerName.SelectedItem.Text == "All...")
                    {
                        cmd.Parameters.AddWithValue("@Customer_Code", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Customer_Code", chkCustomerName.SelectedValue.ToString());
                    }

                    dtDataByfilter = new DataTable();
                    dtDataByfilter.Load(cmd.ExecuteReader());
                    if (dtDataByfilter.Rows.Count > 0)
                    {
                        GridView gvvc = new GridView();
                        gvvc.DataSource = dtDataByfilter;
                        gvvc.DataBind();


                        if (gvvc.Rows.Count > 0)
                        {
                            LblMessage.Visible = false;
                            LblMessage.Text = "";
                            string sFileName = "BulkBuyerReport.xls";

                            sFileName = sFileName.Replace("/", "");
                            Response.ClearContent();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                            Response.ContentType = "application/vnd.ms-excel";
                            EnableViewState = false;

                            System.IO.StringWriter objSW = new System.IO.StringWriter();
                            System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                            foreach (GridViewRow row in gvvc.Rows)
                            {
                                //row.BackColor = Color.White;
                                foreach (TableCell cell in row.Cells)
                                {
                                    cell.CssClass = "textmode";
                                }
                            }

                            gvvc.RenderControl(objHTW);

                            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                            Response.Write(style);
                            Response.Output.Write(objSW.ToString());
                            Response.Flush();
                            Response.End();
                        }
                    }
                    else
                    {
                        LblMessage.Visible = true;
                        LblMessage.Text = "► " + "No Record Exists...";
                    }
                }
                catch (Exception ex)
                {
                    LblMessage.Visible = true;
                    LblMessage.Text = "► " + ex.Message.ToString();
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
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                Panel1.Visible = false;
                LinkButton1.Text = "Show sales person filter";

            }

            else if (Panel1.Visible == false)
            {
                Panel1.Visible = true;
                LinkButton1.Text = "hide sales person filter";
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
                // FillSite();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstHOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
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
                    //FillSite();
                    uppanel.Update();
                    // chkListGM.Items.Clear();
                }
                chkListState_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstVP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
               // FillSite();
                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
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
                    //  FillSite();
                    uppanel.Update();
                }
                chkListState_SelectedIndexChanged(null, null);
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
                    // FillSite();
                    uppanel.Update();
                }
                chkListState_SelectedIndexChanged(null, null);
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
                    // FillSite();
                    uppanel.Update();
                }
                chkListState_SelectedIndexChanged(null, null);
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
                chkListState_SelectedIndexChanged(null, null);
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
                chkListState_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            fillSiteAndState(dt);

            uppanel.Update();
            chkListState_SelectedIndexChanged(null, null);

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
            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
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

            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);

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
            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox7, chkListEXECUTIVE);
            chkListState_SelectedIndexChanged(null, null);
            // chkListASM.DataSource = null;
        }

        protected void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox8, chkListSite);
            // chkListASM.DataSource = null;
            if (chkListSite.Items.Count < 2)
            {
                CheckBox8.Checked = true;
                chkListSite.Items[0].Selected = true;
            }
            chkListSite_SelectedIndexChanged(null, null);
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    ShowDataInGrid();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ShowDataInGrid()
        {
            try
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
                    query = "USP_BULKBUYERREPORT";
                    cmd.CommandText = query;
                    //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                    cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text).ToString("dd-MMM-yyy"));
                    cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text).ToString("dd-MMM-yyy"));
                    cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                    cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));

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
                    if (DistributorList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@SiteCode", DistributorList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SiteCode", "");
                    }

                    if (CustGroupList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@Cust_Group", CustGroupList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Cust_Group", "");
                    }

                    if (chkCustomerName.SelectedItem.Text == "All...")
                    {
                        cmd.Parameters.AddWithValue("@Customer_Code", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Customer_Code", chkCustomerName.SelectedValue.ToString());
                    }

                    dtDataByfilter = new DataTable();
                    dtDataByfilter.Load(cmd.ExecuteReader());
                    if (dtDataByfilter.Rows.Count > 0)
                    {
                        GvBulkBuyer.DataSource = dtDataByfilter;
                        GvBulkBuyer.DataBind();
                        LblMessage.Visible = false;
                        LblMessage.Text = "";
                    }
                    else
                    {
                        GvBulkBuyer.DataSource = null;
                        GvBulkBuyer.DataBind();
                        LblMessage.Visible = true;
                        LblMessage.Text = "► " + "No Record Exists...";
                    }
                }
                catch (Exception ex)
                {
                    LblMessage.Visible = true;
                    LblMessage.Text = "► " + ex.Message.ToString();
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
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}  