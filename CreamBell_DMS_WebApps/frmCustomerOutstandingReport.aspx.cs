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
    public partial class frmCustomerOutstandingReport : System.Web.UI.Page
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
                txtToDate.Attributes.Add("readonly", "readonly");
                //txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
                //CalendarExtender1.StartDate = DateTime.Now.AddDays(-2);
                //CalendarExtender1.EndDate = DateTime.Now;
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
                    //  ShowReportSummary();
                    // bind();
                }


            }
            //if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
            //    phState.Visible = false;
            //    ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            //}

        }
        //private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        //{
        //    FillCustomerGroup();
        //}

        //protected void FillState()
        //{
        //    try
        //    {
        //        string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
        //        object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //        DataTable dt = new DataTable();

        //        dt = new DataTable();
        //        if (objcheckSitecode != null)
        //        {
        //            chkListState.Items.Clear();
        //            string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
        //            dt = baseObj.GetData(sqlstr11);
        //            chkListState.Items.Add("All...");
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                chkListState.DataSource = dt;
        //                chkListState.DataTextField = "NAME";
        //                chkListState.DataValueField = "Code";
        //                chkListState.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            chkListState.Items.Clear();
        //            chkListSite.Items.Clear();
        //            string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
        //            dt = baseObj.GetData(sqlstr1);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                chkListState.DataSource = dt;
        //                chkListState.DataTextField = "StateName";
        //                chkListState.DataValueField = "StateCode";
        //                chkListState.DataBind();

        //                chkListSite.DataSource = dt;
        //                chkListSite.DataTextField = "SiteName";
        //                chkListSite.DataValueField = "SiteId";
        //                chkListSite.DataBind();
        //            }
        //            chkListState.Items[0].Selected = true;
        //            chkListSite.Items[0].Selected = true;
        //            FillCustomerGroup();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //    }
        //}

        //protected void FillSite()
        //{
        //    try
        //    {
        //        string StateList = "";
        //        foreach (ListItem item in chkListState.Items)
        //        {
        //            if (item.Selected)
        //            {
        //                if (StateList == "")
        //                {
        //                    StateList += "'" + item.Value.ToString() + "'";
        //                }
        //                else
        //                {
        //                    StateList += ",'" + item.Value.ToString() + "'";
        //                }
        //            }
        //        }
        //        if (StateList.Length > 0)
        //        {
        //            DataTable dt = new DataTable();
        //            string sqlstr1 = string.Empty;
        //            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //            if (objcheckSitecode != null)
        //            {
        //                sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by SiteName";
        //            }
        //            else
        //            {
        //                sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where SITEID  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
        //            }

        //            dt = new DataTable();
        //            // dt = baseObj.GetData(sqlstr1);
        //            chkListSite.Items.Clear();
        //            dt = baseObj.GetData(sqlstr1);
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                chkListSite.DataSource = dt;
        //                chkListSite.DataTextField = "SiteName";
        //                chkListSite.DataValueField = "SiteId";
        //                chkListSite.DataBind();
        //            }

        //        }
        //        else
        //        {
        //            chkListSite.Items.Clear();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //    }
        //}

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

            }
            // fillbu();
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


        protected void FillCustomerGroup()
        {
            try
            {
                DataTable dt = new DataTable();
                //            string sqlstr = "select distinct A.CUST_GROUP as code,Name=(Select CustGroup_Name from ax.ACXCUSTGROUPMASTER where Custgroup_Code=A.CUST_GROUP ) from ax.acxcustmaster A where A.site_code='" + Session["SiteCode"].ToString() + "'";
                string sqlstr = "select Distinct CustGroup_Name as Name,Custgroup_Code as Code from ax.ACXCUSTGROUPMASTER ";

                dt = new DataTable();
                dt = baseObj.GetData(sqlstr);
                chkListCustomerGroupNew.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListCustomerGroupNew.DataSource = dt;
                    chkListCustomerGroupNew.DataTextField = "NAME";
                    chkListCustomerGroupNew.DataValueField = "Code";
                    chkListCustomerGroupNew.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillSite();
        //}
        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerGroup();
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
                string DistributorList = "";
                foreach (ListItem item in chkListCustomerGroupNew.Items)
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

                foreach (System.Web.UI.WebControls.ListItem litem1 in lstSiteId.Items)
                {
                    if (litem1.Selected)
                    {
                       
                        if (DistributorList.Length == 0)
                            DistributorList = "" + litem1.Value.ToString() + "";
                        else
                            DistributorList += "," + litem1.Value.ToString() + "";
                    }
                }
                if (strCustomerGroupName.Length > 0)
                {
                    DataTable dt = new DataTable();

                    //string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
                    string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code IN (" + DistributorList + ") ";
                    if (strSubDistCustGroup != "")
                    {
                        sqlstr += "Union Select distinct CU.Customer_Code+'-'+CU.Customer_Name as Name,CU.Customer_Code From ax.ACX_SDLINKING SD Left Join ax.ACXCUSTMASTER CU on CU.Customer_Code=SD.SubDistributor where SD.Other_site IN (" + DistributorList + ") And CU.CUST_GROUP='CG0004' Order By Name";
                    }
                    else
                    {
                        sqlstr += " Order By Name";
                    }
                    dt = new DataTable();
                    dt = baseObj.GetData(sqlstr);
                    ddlCustomerNameNew.Items.Clear();
                    //chkCustomerName.DataSource = dt;
                    //chkCustomerName.DataTextField = "NAME";
                    //chkCustomerName.DataValueField = "Customer_Code";
                    //chkCustomerName.DataBind();
                    ddlCustomerNameNew.Items.Clear();
                    ddlCustomerNameNew.Items.Add("--Select--");
                    baseObj.BindToDropDownp(ddlCustomerNameNew, sqlstr, "NAME", "Customer_Code");
                    if (dt.Rows.Count == 0)
                    {
                        ddlCustomerNameNew.Items.Clear();
                        ddlCustomerNameNew.Items.Add("--Select--");
                    }
                }
                else
                {
                    ddlCustomerNameNew.Items.Clear();
                    ddlCustomerNameNew.Items.Add("--Select--");
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
                if(rdoCORI.Checked==true)
                {
                    ShowReportByFilter1();
                }
                else if(rdoCORC.Checked==true)
                {
                    ShowReportByFilter();
                }
            }
            catch (Exception ex)
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
                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "usp_GetCustomerOutstandingDetail";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@Todate", txtToDate.Text);

                string strStateCode = "";
                string strSiteCode = "";

                //if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                //    strStateCode = ucRoleFilters.GetCommaSepartedStateId();
                //    strSiteCode = ucRoleFilters.GetCommaSepartedSiteId();
                //} else
                //{
                    foreach (ListItem item in lstState.Items)
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

                    foreach (ListItem item in lstSiteId.Items)
                    {
                        if (item.Selected)
                        {
                            if (strSiteCode == "")
                            {
                                strSiteCode += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                strSiteCode += "," + item.Value.ToString() + "";
                            }
                        }
                    }

                //}
                   
                if (strStateCode.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", strStateCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }

              
                
               
                if (strSiteCode.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteCode", strSiteCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteCode", "");
                }

                string strCustomerGroup = "";
                foreach (ListItem item in chkListCustomerGroupNew.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerGroup == "")
                        {
                            strCustomerGroup += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            strCustomerGroup += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (strCustomerGroup.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Cust_Group", strCustomerGroup);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Cust_Group", "");
                }

                string strCustomerName = "";
                if (ddlCustomerNameNew.SelectedIndex>0)
                {
                    strCustomerName = ddlCustomerNameNew.SelectedValue.ToString();
                }

                //foreach (ListItem item in ddlCustomerName.Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (strCustomerName == "")
                //        {
                //            strCustomerName = item.Value.ToString();
                //        }
                //        else
                //        {
                //            strCustomerName += "," + item.Value.ToString() + "";
                //        }
                //    }
                //}

                if (strCustomerName.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Customer_Code", strCustomerName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Customer_Code", "");
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

        private void ShowReportByFilter1()
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
                query = "usp_GetInvoicewiseOutstandingDetail";
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@Todate", txtToDate.Text);

                string strStateCode = "";
                string strSiteCode = "";
                //if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                //{
                //    strStateCode = ucRoleFilters.GetCommaSepartedStateId();
                //    strSiteCode = ucRoleFilters.GetCommaSepartedSiteId();
                //}
                //else {
                    foreach (ListItem item in lstState.Items)
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

                    foreach (ListItem item in lstSiteId.Items)
                    {
                        if (item.Selected)
                        {
                            if (strSiteCode == "")
                            {
                                strSiteCode += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                strSiteCode += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                //}
                    
                if (strStateCode.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", strStateCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }


                
                
                if (strSiteCode.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteCode", strSiteCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteCode", "");
                }

                string strCustomerGroup = "";
                foreach (ListItem item in chkListCustomerGroupNew.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerGroup == "")
                        {
                            strCustomerGroup += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            strCustomerGroup += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (strCustomerGroup.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Cust_Group", strCustomerGroup);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Cust_Group", "");
                }

                string strCustomerName = "";
                foreach (ListItem item in ddlCustomerNameNew.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerName == "")
                        {
                            strCustomerName = item.Value.ToString();
                        }
                        else
                        {
                            strCustomerName += "," + item.Value.ToString() + "";
                        }
                    }
                }

                if (strCustomerName.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Customer_Code", strCustomerName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Customer_Code", "");
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

        private void LoadDataInReportViewer(DataTable dtSetData)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("STATECODE");
                    DataSetParameter.Columns.Add("SiteCode");
                    DataSetParameter.Columns.Add("Cust_Group");
                    DataSetParameter.Columns.Add("Customer_Code");           
                    DataSetParameter.Rows.Add();


                    string strStateCode = "";
                    string DistributorList = "";

                    //if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                    //    strStateCode = ucRoleFilters.GetCommaSepartedStateId();
                    //    DistributorList = ucRoleFilters.GetCommaSepartedSiteId();
                    //} else {
                        foreach (ListItem item in lstState.Items)
                        {
                            if (item.Selected)
                            {
                                if (strStateCode == "")
                                {
                                    strStateCode += "" + item.Text.ToString() + "";
                                }
                                else
                                {
                                    strStateCode += "," + item.Text.ToString() + "";
                                }
                            }
                        }

                        foreach (ListItem item in lstSiteId.Items)
                        {
                            if (item.Selected)
                            {
                                if (DistributorList == "")
                                {
                                    DistributorList += "" + item.Text.ToString() + "";
                                }
                                else
                                {
                                    DistributorList += "," + item.Text.ToString() + "";
                                }
                            }
                        }
                   // }
                       
                    if (strStateCode == string.Empty)
                    {
                        strStateCode = "ALL";
                    }
                    DataSetParameter.Rows[0]["STATECODE"] = strStateCode;
                    
                    if (DistributorList == string.Empty)
                    {
                        DistributorList = "ALL";
                    }
                    DataSetParameter.Rows[0]["SiteCode"] = DistributorList;


                    string strCustomerGroup = "";
                    foreach (ListItem item in chkListCustomerGroupNew.Items)
                    {
                        if (item.Selected)
                        {
                            if (strCustomerGroup == "")
                            {
                                strCustomerGroup += "" + item.Text.ToString() + "";
                            }
                            else
                            {
                                strCustomerGroup += "," + item.Text.ToString() + "";
                            }
                        }

                    }
                    if (strCustomerGroup == string.Empty)
                    {
                        strCustomerGroup = "ALL";
                    }
                    DataSetParameter.Rows[0]["Cust_Group"] = strCustomerGroup;

                    string strCustomerName = "";
                    foreach (ListItem item in ddlCustomerNameNew.Items)
                    {
                        if (item.Selected)
                        {
                            if (strCustomerName == "")
                            {
                                strCustomerName += "" + item.Text.ToString() + "";
                            }
                            else
                            {
                                strCustomerName += "," + item.Text.ToString() + "";
                            }
                        }

                    }
                    if (strCustomerName == string.Empty)
                    {
                        strCustomerName = "ALL";
                    }
                    DataSetParameter.Rows[0]["Customer_Code"] = strCustomerName;
                    if(rdoCORC.Checked==true)
                    {
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\CustomerOutstanding.rdl");
                    }
                    else if (rdoCORI.Checked == true)
                    {
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\InvoiceOutstanding.rdl");
                    }

                    ReportParameter FromDate = new ReportParameter();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
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
    }
}