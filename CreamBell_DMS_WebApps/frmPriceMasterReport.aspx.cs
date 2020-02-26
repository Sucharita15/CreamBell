using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Microsoft.Reporting.WebForms;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPriceMasterReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
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
                    //  ShowReportSummary();
                    // bind();
                }

            }

            //if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            //{
            //    phState.Visible = false;
            //    ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            //}
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
        //private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        //{
        //    loadPSR();
        //}

        //protected void fillSiteAndState()
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        lstState.Items.Clear();
        //        string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
        //        lstState.Items.Add("All...");
        //        baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlState.Items.Clear();
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
        //        baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
        //        ddlSiteId_SelectedIndexChanged(null, null);
        //    }
        //}

        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' ORDER BY NAME";
        //        ddlSiteId.Items.Add("All...");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";               
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //    }
        //}

        //protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    loadPSR();
        //}

        //void loadPSR()
        //{
        //    //string strSite = ddlSiteId.SelectedValue.ToString();
        //    string strSite = "";


        //    foreach (System.Web.UI.WebControls.ListItem litem1 in lstSiteId.Items)
        //    {
        //        if (litem1.Selected)
        //        {

        //            if (strSite.Length == 0)
        //                strSite = "" + litem1.Value.ToString() + "";
        //            else
        //                strSite += "," + litem1.Value.ToString() + "";
        //        }
        //    }


        //    string strQuery = "";
        //    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
        //    {
        //        //strSite = ucRoleFilters.GetCommaSepartedSiteId();
        //        strQuery = @"Select PSR_Code +'-'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
        //                   " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
        //                   " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
        //                   " where B.Site_code  IN (" + strSite + ")";
        //    }
        //    else
        //    {
        //        strQuery = @"Select PSR_Code +'-'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
        //                   " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
        //                   " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
        //                   " where B.Site_code IN (" + strSite + "))";
        //    }

        //    ddlPSR.Items.Clear();
        //    ddlPSR.Items.Add("Select...");
        //    baseObj.BindToDropDown(ddlPSR, strQuery, "PSRName", "PSR_Code");
        //}
        protected void ddlPSR_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //protected void BtnShowReport_Click(object sender, EventArgs e)
        //{
        //    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
        //    DataTable dt = new DataTable();
        //    bool b = Validate();
        //    if (b == true)
        //    {
        //        try
        //        {

        //            ShowReportSummary();
        //        }
        //        catch (Exception ex)
        //        {
        //            LblMessage.Text = ex.Message.ToString();
        //            ErrorSignal.FromCurrentContext().Raise(ex);
        //        }
        //    }
        //}

        //private void ShowReportSummary()
        //{
        //    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
        //    DataTable dtSetData = null;
        //    DataTable dtSetData1 = null;


        //    try
        //    {
        //        dtSetData = new DataTable();
        //        dtSetData1 = new DataTable();
        //        string query = "ACX_USP_PSRDSR";
        //        List<string> ilist = new List<string>();
        //        List<string> item = new List<string>();
        //        string PSRCode, SiteCode;
        //        SiteCode = "";
        //        foreach (System.Web.UI.WebControls.ListItem litem1 in lstSiteId.Items)
        //        {
        //            if (litem1.Selected)
        //            {

        //                if (SiteCode.Length == 0)
        //                    SiteCode = "" + litem1.Value.ToString() + "";
        //                else
        //                    SiteCode += "," + litem1.Value.ToString() + "";
        //            }
        //        }
        //        //if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
        //        //    SiteCode = ucRoleFilters.GetCommaSepartedSiteId();
        //        //} else {
        //        //    if (ddlSiteId.SelectedIndex >= 0)
        //        //    {
        //        //        if (ddlSiteId.SelectedItem.Text != "All...")
        //        //        {
        //        //            SiteCode = ddlSiteId.SelectedItem.Value;
        //        //        }
        //        //    }
        //        //}

        //        ilist.Add("@SITECODE"); item.Add(SiteCode);

        //        PSRCode = "";

        //        if (ddlPSR.SelectedIndex >= 0)
        //        {
        //            if (ddlPSR.SelectedItem.Text != "All...")
        //            {
        //                PSRCode = ddlPSR.SelectedItem.Value;
        //            }
        //        }
        //        ilist.Add("@PSRCODE"); item.Add(PSRCode);

        //        DateTime now = new DateTime();
        //        now = Convert.ToDateTime(txtFromDate.Text);
        //        var startDate = new DateTime(now.Year, now.Month, 1);
        //        string FromDate = startDate.ToString();
        //        ilist.Add("@DATE"); item.Add(Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));

        //        dtSetData = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
        //        dtSetData1 = obj.GetData_New("ACX_USP_PSRDSR_HEADER", CommandType.StoredProcedure, ilist, item);

        //        LoadDataInReportViewerDetail(dtSetData, dtSetData1);
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message.ToString();
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //    }
        //}

        private void LoadDataInReportViewerDetail(DataTable dtSetData, DataTable dtSetData2)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PSR_DSR.rdl");
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", dtSetData2);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);

                    ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;


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

        private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        {
            fillPriceGroup();
        }

        //protected void fillSiteAndState()
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlState.Items.Clear();
        //        string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY LS.Name ";
        //        ddlState.Items.Add("Select...");
        //        baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlState.Items.Clear();
        //        ddlSiteId.Items.Clear();
        //        ddlPriceGroup.Items.Clear();
        //        string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")   ORDER BY LS.Name";
        //        baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");


        //        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
        //        {
        //            sqlstr1 = @"select distinct A.Pricegroup ,B.Name as PriceGroupName from ax.acxcustmaster A left join [ax].[PRICEDISCGROUP] B on B.GroupId=A.Pricegroup  where site_code IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")  Order by PricegroupName";
        //        }
        //        else {
        //            sqlstr1 = @"select distinct A.Pricegroup ,B.Name as PriceGroupName from ax.acxcustmaster A left join [ax].[PRICEDISCGROUP] B on B.GroupId=A.Pricegroup  where site_code= '" + ddlSiteId.SelectedItem.Value + "' Order by PricegroupName";
        //        }
        //        baseObj.BindToDropDown(ddlPriceGroup, sqlstr1, "PriceGroupName", "Pricegroup");
        //    }
        //}

        protected void fillPriceGroup()
        {
            string DistributorList = "";
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
            ddlPriceGroupNew.Items.Clear();
            // string sqlstr1 = @"select distinct A.Pricegroup ,B.Name as PriceGroupName from ax.acxcustmaster A left join [ax].[PRICEDISCGROUP] B on B.GroupId=A.Pricegroup where site_code= '" + ddlSiteId.SelectedItem.Value + "' Order by PricegroupName";
            string sqlstr1 = "";
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                sqlstr1 = "EXEC ACX_GETALLPRICEGROUP '" + DistributorList + "'";
            }
            else
            {
                sqlstr1 = "EXEC ACX_GETALLPRICEGROUP '" + DistributorList + "'";
            }

            ddlPriceGroupNew.Items.Add("All...");
            baseObj.BindToDropDownp(ddlPriceGroupNew, sqlstr1, "PriceGroupName", "Pricegroup");
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validate();
                if (b)
                {
                    ShowReportByFilter();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool Validate()
        {
            string DistributorList = "";
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
            bool b;
            b = true;
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                if (DistributorList.Length == 0)
                {
                    b = false;
                    LblMessage.Text = "Please Provide Distru=ibutor.";
                }

                if (DistributorList.Length == 0)
                {
                    b = false;
                    LblMessage.Text = "Please Provide State.";
                }
            }
            else
            {
                if (lstState.Text == string.Empty || lstState.Text == "Select...")
                {
                    b = false;
                    LblMessage.Text = "Please Provide State.";
                }
                if (lstSiteId.Text == string.Empty || lstSiteId.Text == "Select...")
                {
                    b = false;
                    LblMessage.Text = "Please Provide Distru=ibutor.";
                }
            }

            if (ddlPriceGroupNew.Text == string.Empty || ddlPriceGroupNew.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide Price Group.";
            }

            return b;
        }

        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
                string DistributorList = "";
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
                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "[dbo].[ACX_GETProductPriceMaster]";

                cmd.CommandText = query;
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    cmd.Parameters.AddWithValue("@State", DistributorList);
                    cmd.Parameters.AddWithValue("@SiteId", DistributorList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@State", lstState.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@SiteId", lstSiteId.SelectedItem.Value);
                }

                cmd.Parameters.AddWithValue("@PriceGroup", ddlPriceGroupNew.SelectedItem.Value);

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
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PriceMasterReport.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
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

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validate();
                if (b == true)
                {
                    ShowData_ForExcel();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void ShowData_ForExcel()
        {
            string DistributorList = "";
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
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;

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
            try
            {
                string query1 = "Select NAME from ax.inventsite where SITEID IN (" + DistributorList + ") ";
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

                query = "[dbo].[ACX_GETProductPriceMaster]";

                cmd.CommandText = query;

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    cmd.Parameters.AddWithValue("@SiteId",DistributorList);
                    cmd.Parameters.AddWithValue("@State", DistributorList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@State", lstState.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@SiteId", lstSiteId.SelectedItem.Value);
                }
                cmd.Parameters.AddWithValue("@PriceGroup", ddlPriceGroupNew.SelectedItem.Value);

                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
                DataTable dt = new DataTable();
                dt = dtDataByfilter;
                dt.Columns.Remove("Short");
                //=================Create Excel==========

                string attachment = "attachment; filename=PriceMasterReport.xls";
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
        }


        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillPriceGroup();
        }

        protected void ddlPriceGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
    }
}