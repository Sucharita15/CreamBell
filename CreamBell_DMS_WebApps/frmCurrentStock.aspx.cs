using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCurrentStock : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                //btnShow.Attributes.Add("onclick", "javascript: if (Page_ClientValidate() ){" + btnShow.ClientID + ".disabled=true;}" + ClientScript.GetPostBackEventReference(btnShow, ""));
                string sqlstr1 = @"select WAREHOUSENAME,WAREHOUSEVALUE from AX.ACXWAREHOUSES";
                ddlWarehouse.Items.Clear();
                DataTable dt1 = new DataTable();
                dt1 = baseObj.GetData(sqlstr1);
                ddlWarehouse.DataSource = dt1;
                ddlWarehouse.DataTextField = "WAREHOUSENAME";
                ddlWarehouse.DataValueField = "WAREHOUSEVALUE";
                ddlWarehouse.DataBind();
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
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                }

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    tclabel.Width = "90%";
                    Panel1.Visible = true;

                }
                else
                {

                    tclabel.Width = "100%";
                    Panel1.Visible = false;

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
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

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
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
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
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where STATECODE in (" + statesel + ") AND  UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' Order by SiteName";
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
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                            dt = baseObj.GetData(sqlstr1);
                            lstSiteId.DataSource = dt;
                            lstSiteId.DataTextField = "Name";
                            lstSiteId.DataValueField = "Code";
                            lstSiteId.DataBind();

                        }
                        else
                        {
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where SITEID = '" + Session["SiteCode"].ToString() + "' AND UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000'";
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
                    FillBU();
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
                    // chkListGM.Items.Clear();
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
            ddlCountry_SelectedIndexChanged(null, null);
            // chkListASM.DataSource = null;
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



        protected void FillBU()
        {
            string SiteList = "";
            foreach (System.Web.UI.WebControls.ListItem item in lstSiteId.Items)
            {
                if (item.Selected)
                {
                    if (SiteList == "")
                    {
                        SiteList += "'" + item.Value.ToString() + "'";
                    }
                    else
                    {
                        SiteList += ",'" + item.Value.ToString() + "'";
                    }
                }
            }
            ddlBuunit.Items.Clear();
            if (SiteList != "")
            {   
                //string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                string query = "select distinct bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID in (" + SiteList + ")";
                ddlBuunit.Items.Insert(0, "All...");
                baseObj.BindToDropDown(ddlBuunit, query, "bu_desc", "bu_code");
            }
        }
        private void ShowCurrentStock()
        {
            if (lstState.SelectedValue == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Please select atleast one state before show!');", true);
                return;
            }
            else if (lstSiteId.SelectedValue == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Please select atleast one distributor before show!');", true);
                return;
            }
            else if (ddlWarehouse.SelectedValue == string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('Please select atleast one Warehouse before show!');", true);
                return;
            }

            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            DataTable dt = new DataTable();
            List<string> ilist = new List<string>();
            List<string> item = new List<string>();
            string WareHouse;
            WareHouse = "";
            string siteid = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlWarehouse.Items)
            {
                if (litem.Selected)
                {
                    if(WareHouse.Length==0)
                        WareHouse = litem.Value.ToString();
                    else
                        WareHouse += "," + litem.Value.ToString();
                }
            }

            foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
            {
                if (litem.Selected)
                {
                    if (siteid.Length == 0)
                        siteid = litem.Value.ToString();
                    else
                        siteid += "," + litem.Value.ToString();
                }
            }

            string query = "ACX_CurrentStock";
            
            ilist.Add("@Site_Code"); item.Add(siteid.ToString());
            ilist.Add("@TransLocation"); item.Add(WareHouse);//item.Add("59144MW");//
            ilist.Add("@Dataareaid");item.Add(Session["DATAAREAID"].ToString());
            if (ddlBuunit.SelectedIndex >= 1)
            {
                ilist.Add("@BUCODE"); item.Add(ddlBuunit.SelectedValue.ToString());
            }
            else
            {
                ilist.Add("@BUCODE"); item.Add("");
            }
            
            dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                      
            if (dt.Rows.Count > 0)
            {
                //object sumLtr,SumQty,SumQtyLtr;
                //sumLtr = dt.Compute("Sum(LTR)", "");
                //SumQty = dt.Compute("Sum(Qty)", "");
                //SumQtyLtr = dt.Compute("Sum(QTYLtr)", "");
                //dt.Rows.Add("TOTAL", "", "", "0", sumLtr, "0", SumQty, SumQtyLtr, "0", "", "", "");
                gridCurrentStcok.DataSource = dt;
                gridCurrentStcok.DataBind();
                LblMessage.Text = "Total Records : " + dt.Rows.Count.ToString();
                Session["CurrentStock"] = dt;
            }
            else
            {
                gridCurrentStcok.DataSource = dt;
                gridCurrentStcok.DataBind();

                LblMessage.Text = string.Empty;
            }

        }
       
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (gridCurrentStcok.Rows.Count > 0)
            {
                //ExportToExcel();
                ExportToExcelNew();
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
            }
        }

        private void ExportToExcelNew()
        {

            try
            {
                if (gridCurrentStcok.Rows.Count > 0)
                {
                    string sFileName = "CurrentStock" + DateTime.Now + ".xls";

                    sFileName = sFileName.Replace("/", "");
                    // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    EnableViewState = false;

                    System.IO.StringWriter objSW = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                    foreach (GridViewRow row in gridCurrentStcok.Rows)
                    {
                        row.Cells[0].CssClass = "textmode";
                        //row.BackColor = Color.White;
                        //foreach (TableCell cell in row.Cells)
                        //{
                        //    cell.CssClass = "textmode";
                        //}
                    }

                    gridCurrentStcok.RenderControl(objHTW);

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(objSW.ToString());
                    Response.Flush();
                    Response.End();

                    //dg = null;
                }

            }

            catch (Exception ex)
            {
                //LblMessage.Text = ex.Message.ToString();
            }
            finally
            {

            }

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    Response.Charset = "";
            //    string FileName = "CurrentStock" + DateTime.Now + ".xls";
            //    StringWriter strwritter = new StringWriter();
            //    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            //    gridCurrentStcok.GridLines = GridLines.Both;
            //    gridCurrentStcok.HeaderStyle.Font.Bold = true;
            //    gridCurrentStcok.RenderControl(htmltextwrtter);
            //    Response.Write(strwritter.ToString());
            //    Response.End();
        }

        private bool ValidateInput()
        {
            bool value = true;

            if (lstSiteId.SelectedValue == string.Empty)
            {
                value = false;
                string message = "alert('Please Select The SiteID  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            else if(ddlWarehouse.SelectedValue == string.Empty)
            {
                value = false;
                string message = "alert('Please Select Warehouse  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
            }
            return value;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            ShowCurrentStock();
            //gridCurrentStcok.DataSource = null;
            //gridCurrentStcok.DataBind();
            //bool b = ValidateInput();
            //if (b == true)
            //{
            //    
            //}
            
        }

        //protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sqlstr1;
        //    ddldistributor.Items.Clear();
        //    if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
        //    {
        //        sqlstr1 = @"Select Distinct SITEID as Code, NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where STATECODE = '" + ddlstate.SelectedItem.Value + "' AND  UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' ORDER BY NAME";
        //        //sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlstate.SelectedItem.Value + "' ORDER BY NAME";
        //    }
        //    else
        //    {
        //        sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
        //    }
        //    ddldistributor.Items.Add("Select...");
        //    baseObj.BindToDropDown(ddldistributor, sqlstr1, "Name", "Code");
        //    if (ddldistributor.Items.Count == 2)
        //        ddldistributor.SelectedIndex = 1;
        //    ddldistributor_SelectedIndexChanged(sender, e);
        //}

        protected void ddldistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sqlstr1 = @"SELECT i.INVENTLOCATIONID as Code,i.NAME FROM ax.INVENTLOCATION i WHERE i.INVENTSITEID='" + ddldistributor.SelectedValue.ToString() + "';";
            //ddcbWarehouse.Items.Clear();
            //DataTable dt = new DataTable();
            //dt= baseObj.GetData(sqlstr1);
            //ddcbWarehouse.DataSource = dt;
            //ddcbWarehouse.DataTextField = "Name";
            //ddcbWarehouse.DataValueField = "Code";
            //ddcbWarehouse.DataBind();

            FillBU();
        }
    }
}