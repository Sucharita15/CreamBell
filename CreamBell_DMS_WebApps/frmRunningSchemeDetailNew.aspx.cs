using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmRunningSchemeDetailNew : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlConnection conn = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SiteCode"] != null)
            //if (Session["USERID"] == null)
            {
                if (!IsPostBack)
                {
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
                        Panel1.Visible = true;
                        Panel3.Visible = true;
                    }
                    else
                    {
                        Panel3.Visible = false;
                    }
                    if (Convert.ToString(Session["LOGINTYPE"]) == "0" && Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                    {
                        //   ShowCustomerMaster();
                        Bind_Grid();
                    }
                }
            }
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
        //protected void fillSiteAndState()
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlState.Items.Clear();
        //        string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY Name ";
        //        ddlState.Items.Add("Select...");
        //        baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlState.Items.Clear();
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId= '" + Session["SiteCode"].ToString() + "'  ORDER BY LS.Name";
        //        baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
        //    }
        //}
        public void Bind_Grid()
        {
            try
            {
                string statesel1 = "";
                string sitesel = "";
                foreach (System.Web.UI.WebControls.ListItem litem1 in lstState.Items)
                {
                    if (litem1.Selected)
                    {
                        if (statesel1.Length == 0)
                            statesel1 = "'" + litem1.Value.ToString() + "'";
                        else
                            statesel1 += ",'" + litem1.Value.ToString() + "'";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem1 in lstSiteId.Items)
                {
                    if (litem1.Selected)
                    {
                        if (sitesel.Length == 0)
                            sitesel = "'" + litem1.Value.ToString() + "'";
                        else
                            sitesel += ",'" + litem1.Value.ToString() + "'";
                    }
                }
                string sqlstr = @"Select Distinct SCHEMECODE,[SCHEME DESCRIPTION] as Name,Case when [SCHEME TYPE]=0 then 'Quantity' when [SCHEME TYPE] = 1 then 'Value' when [SCHEME TYPE] = 2 then 'Percent' when [SCHEME TYPE] = 3 then 'Valueoff'  end as [SCHEME TYPE],STARTINGDATE,ENDINGDATE from [dbo].[ACXSCHEMEVIEW]
                             Where SALESCODE in ( " + sitesel + "," + statesel1 + ",'') ";

                DataTable dt = baseObj.GetData(sqlstr);
                gvSchemeDetail.DataSource = dt;
                gvSchemeDetail.DataBind();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null) { if (conn.State.ToString() == "Closed") { conn.Open(); } }
            }

        }

        protected void lnkbtnSchemeDetail_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnkbtn = sender as LinkButton;

                //                string sqlstr = @"Select Distinct 
                //                            case when [Scheme Type]=0 then (Case when MINIMUMQUANTITY = 0 then cast(MINIMUMQUANTITYPCS as decimal(9,2))  else cast(MINIMUMQUANTITY as decimal(9,2)) end) 
                //                            else cast(MINIMUMVALUE as decimal(9,2))
                //							end  as slabdetail,[TYPE],CODE + '-' +
                //                            case when [TYPE] = 'Group' then (Select TOP 1 ACG.CUSTGROUP_NAME from [ax].[ACXCUSTMASTER] ACM
                //                            Inner join 
                //                            [ax].[ACXCUSTGROUPMASTER] ACG on ACG.CUSTGROUP_CODE = ACM.CUST_GROUP 
                //                            where ACG.CUSTGROUP_CODE = CODE )
                //                            when [TYPE] = 'Table' then (Select TOP 1 ACM.CUSTOMER_NAME from [ax].[ACXCUSTMASTER] ACM 
                //                            where ACM.CUSTOMER_CODE = CODE ) 
                //                            end as Name,case when [Scheme Type]=0 then cast(MINIMUMQUANTITY as decimal(9,2)) else 0.00
                //							end as [Buying Quantity Box],
                //                            Case when [Scheme Type]=0 then cast(MINIMUMQUANTITYPCS as decimal(9,2)) else 0.00
                //							end as [Buying Quantity PCS]
                //                            ,[Scheme Item group],SCHEMECODE,[Scheme Item Type],[Sales Type]
                //                            from [dbo].[ACXSCHEMEVIEW] 
                //                            where SCHEMECODE = '" + lnkbtn.Text + "' and  Salescode like (Case when [Sales Type] = 'State' then '" + Session["SITELOCATION"].ToString() + "' else '%' end)";

                string sqlstr = @"Select Distinct 
                            case when [Scheme Type]=0 then (Case when MINIMUMQUANTITY = 0 then cast(MINIMUMQUANTITYPCS as decimal(9,2))   else cast(MINIMUMQUANTITY as decimal(9,2)) end) 
                            WHEN [Scheme Type]=2  THEN (CASE WHEN MINIMUMQUANTITY> 0 THEN cast(MINIMUMQUANTITY as decimal(9,2)) WHEN  MINIMUMQUANTITYPCS>0 THEN cast(MINIMUMQUANTITYPCS as decimal(9,2)) ELSE  cast(MINIMUMVALUE as decimal(9,2)) END)
                            when [Scheme Type]=3 then (Case when MINIMUMQUANTITY = 0 then cast(MINIMUMQUANTITYPCS as decimal(9,2))   else cast(MINIMUMQUANTITY as decimal(9,2)) end) 
                            else cast(MINIMUMVALUE as decimal(9,2))
							end  as slabdetail,[TYPE],CODE + '-' +
                            case when [TYPE] = 'Group' then (Select TOP 1 ACG.CUSTGROUP_NAME from [ax].[ACXCUSTMASTER] ACM
                            Inner join 
                            [ax].[ACXCUSTGROUPMASTER] ACG on ACG.CUSTGROUP_CODE = ACM.CUST_GROUP 
                            where ACG.CUSTGROUP_CODE = CODE )
                            when [TYPE] = 'Table' then (Select TOP 1 ACM.CUSTOMER_NAME from [ax].[ACXCUSTMASTER] ACM 
                            where ACM.CUSTOMER_CODE = CODE ) 
                            end as Name,case when [Scheme Type]=0 then cast(MINIMUMQUANTITY as decimal(9,2)) else 0.00
							end as [Buying Quantity Box],
                            Case when [Scheme Type]=0 then cast(MINIMUMQUANTITYPCS as decimal(9,2)) else 0.00
							end as [Buying Quantity PCS],
                            [Scheme Item group] ,SCHEMECODE,[Scheme Item Type],[Sales Type], [SchemeLineNo]
                            from [dbo].[ACXSCHEMEVIEW] 
                            where SCHEMECODE = '" + lnkbtn.Text + "' and  Salescode like (Case when [Sales Type] = 'State' then '" + lstState.SelectedItem.Value + "' else '%' end)";

                DataTable dt = baseObj.GetData(sqlstr);
                gridViewSlabDetail.DataSource = dt;
                gridViewSlabDetail.DataBind();

                gridViewSchemeItemGroup.DataSource = null;
                gridViewSchemeItemGroup.DataBind();
                gridViewFreeItemGroup.DataSource = null;
                gridViewFreeItemGroup.DataBind();
                gridView1.DataSource = null;
                gridView1.DataBind();
                gridView2.DataSource = null;
                gridView2.DataBind();

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null) { if (conn.State.ToString() == "Closed") { conn.Open(); } }
            }

        }

        protected void lnkbtngridViewSlabDetail_Click(object sender, EventArgs e)
        {
            try
            {
                gridViewSchemeItemGroup.DataSource = null;
                gridViewSchemeItemGroup.DataBind();
                gridViewFreeItemGroup.DataSource = null;
                gridViewFreeItemGroup.DataBind();

                GridViewRow row = (GridViewRow)(((LinkButton)sender)).NamingContainer;

                LinkButton lnkbtn = sender as LinkButton;
                HiddenField h = (HiddenField)gridViewSlabDetail.Rows[row.RowIndex].FindControl("hiddenSchemeItemGroup");
                HiddenField sln = (HiddenField)gridViewSlabDetail.Rows[row.RowIndex].FindControl("HiddenFieldSchemeLineNo");

                HiddenField SchemeCode = (HiddenField)gridViewSlabDetail.Rows[row.RowIndex].FindControl("HiddenFieldSchemeCode");

                Label lblSchemeItemType = (Label)gridViewSlabDetail.Rows[row.RowIndex].FindControl("lblSchemeItemType");

                string sqlstr = string.Empty;
                if (lblSchemeItemType.Text == "Group")
                {
                    sqlstr = @"Select Distinct AFGT.[GROUP] as [PRODUCT GROUP],p.PRODUCT_SUBCATEGORY as ProductSubCat,AFGT.ITEMID ,AFGT.ITEMNAME  from 
                                  [ax].[ACXFREEITEMGROUPTABLE] AFGT INNER JOIN 
                                  ax.INVENTTABLE P ON AFGT.ITEMID = P.ITEMID where AFGT.[GROUP] = '" + h.Value + "' ";
                }

                if (lblSchemeItemType.Text == "Item")
                {
                    sqlstr = " Select Distinct Product_Group as [PRODUCT GROUP] ,PRODUCT_SUBCATEGORY as ProductSubCat,ASFS.SCHEMEITEMGROUP as ITEMID,Product_Name as ITEMNAME  "
                                 + " from ax.ACXSALESFREESUPPLY ASFS  "
                                 + " Inner Join ax.INVENTTABLE P ON ASFS.SCHEMEITEMGROUP = P.ITEMID "
                                 + " where ASFS.SCHEMEITEMGROUP = '" + h.Value + "' ";
                }

                if (lblSchemeItemType.Text == "All")
                {
                    sqlstr = " select Distinct Product_Group as [PRODUCT GROUP] ,PRODUCT_SUBCATEGORY as ProductSubCat,ITEMID,Product_Name as ITEMNAME from  ax.INVENTTABLE ";
                }

                DataTable dt = baseObj.GetData(sqlstr);
                gridViewSchemeItemGroup.DataSource = dt;
                gridViewSchemeItemGroup.DataBind();



                //sqlstr = @"Select Distinct p.[PRODUCT_GROUP],p.PRODUCT_SUBCATEGORY as ProductSubCat,p.ITEMID ,p.PRODUCT_NAME ,Cast(FREEQTY as decimal(9,2)) FREEQTY, Cast(FREEQTYPCS as decimal(9,2)) as FREEQTYPCS,SV.SETNO "
                //        + " from ax.INVENTTABLE p  "
                //        + "	left join [dbo].[ACXSCHEMEVIEW] SV on p.ITEMID = SV.[Free Item Code] and "
                //        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + Session["SiteCode"].ToString() + "','" + Session["SITELOCATION"].ToString() + "','')  "
                //        + " and SCHEMECODE = '" + SchemeCode.Value + "'"
                //        + " Where p.ITEMID in (Select [Free Item Code] from [dbo].[ACXSCHEMEVIEW]  "
                //        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  "
                //        //+ " AND SALESCODE in ( '" + Session["SiteCode"].ToString() + "','" + Session["SITELOCATION"].ToString() + "','')  "
                //        + " and SCHEMECODE = '" + SchemeCode.Value+ "') ";

                sqlstr = @"Select Distinct p.[PRODUCT_GROUP],p.PRODUCT_SUBCATEGORY as ProductSubCat,p.ITEMID ,p.PRODUCT_NAME ,Cast(FREEQTY as decimal(9,2)) FREEQTY, Cast(FREEQTYPCS as decimal(9,2)) as FREEQTYPCS,SV.SETNO,ROUND(PERCENTSCHEME,2) PERCENTSCHEME,cast(SCHEMEVALUEOFF as decimal(10,2)) SCHEMEVALUEOFF"
                        + " from ax.INVENTTABLE p  "
                        + "	RIGHT join [dbo].[ACXSCHEMEVIEW] SV on SV.[Free Item Code] = P.ITEMID and [SchemeLineNo]= '" + sln.Value + "' and "
                        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + lstSiteId.SelectedItem.Value + "','" + lstState.SelectedItem.Value + "','')  "
                        + " and SCHEMECODE = '" + SchemeCode.Value + "'"
                        + " Where  p.ITEMID in (Select [Free Item Code] from [dbo].[ACXSCHEMEVIEW]  "
                        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  "
                        + " and SCHEMECODE = '" + SchemeCode.Value + "') "
                        + " OR ((MINIMUMQUANTITY + MINIMUMQUANTITYPCS + MINIMUMVALUE)  = (Select SUM(MINIMUMQUANTITY + MINIMUMQUANTITYPCS + MINIMUMVALUE) from[dbo].[ACXSCHEMEVIEW]"
                        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  and SCHEMECODE = '" + SchemeCode.Value + "'  and  [SchemeLineNo]= '" + sln.Value + "') and SCHEMECODE = '" + SchemeCode.Value + "'  and  [SchemeLineNo]= '" + sln.Value + "')";

                dt = baseObj.GetData(sqlstr);
                gridViewFreeItemGroup.DataSource = dt;
                gridViewFreeItemGroup.DataBind();

                sqlstr = @"Select SV.MINIMUMPURCHASEITEMTYPE,SV.MINIMUMPURCHASEITEMGROUP,SV.MINIMUMPURCHASEITEMDESCRIPTION,Cast(SV.MINIMUMPURCHASEBOX as decimal(10,2)) AS MINIMUMPURCHASEBOX,Cast(SV.MINIMUMPURCHASEPCS as decimal(10,2)) AS MINIMUMPURCHASEPCS"
                        + " from ax.INVENTTABLE p  "
                        + "	RIGHT join [dbo].[ACXSCHEMEVIEW] SV on SV.[Free Item Code] = P.ITEMID and [SchemeLineNo]= '" + sln.Value + "' and "
                        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + lstSiteId.SelectedItem.Value + "','" + lstState.SelectedItem.Value + "','')  "
                        + " and SCHEMECODE = '" + SchemeCode.Value + "'"
                        + " Where  p.ITEMID in (Select [Free Item Code] from [dbo].[ACXSCHEMEVIEW]  "
                        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  "
                        + " and SCHEMECODE = '" + SchemeCode.Value + "') "
                        + " OR ((MINIMUMQUANTITY + MINIMUMQUANTITYPCS + MINIMUMVALUE)  = (Select SUM(MINIMUMQUANTITY + MINIMUMQUANTITYPCS + MINIMUMVALUE) from[dbo].[ACXSCHEMEVIEW]"
                        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  and SCHEMECODE = '" + SchemeCode.Value + "'  and  [SchemeLineNo]= '" + sln.Value + "') and SCHEMECODE = '" + SchemeCode.Value + "'  and  [SchemeLineNo]= '" + sln.Value + "')";

                dt = baseObj.GetData(sqlstr);
                gridView1.DataSource = dt;
                gridView1.DataBind();

                sqlstr = @"Select SV.ADDITIONDISCOUNTITEMTYPE,SV.ADDITIONDISCOUNTITEMGROUP,SV.ADDITIONDISCOUNTITEMGROUPDESC,Cast(SV.ADDITIONDISCOUNTPERCENT as decimal(10,2)) AS ADDITIONDISCOUNTPERCENT,Cast(SV.ADDITIONDISCOUNTVALUEOFF as decimal(10,2)) AS ADDITIONDISCOUNTVALUEOFF"
                        + " from ax.INVENTTABLE p  "
                        + "	RIGHT join [dbo].[ACXSCHEMEVIEW] SV on SV.[Free Item Code] = P.ITEMID and [SchemeLineNo]= '" + sln.Value + "' and "
                        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + lstSiteId.SelectedItem.Value + "','" + lstState.SelectedItem.Value + "','')  "
                        + " and SCHEMECODE = '" + SchemeCode.Value + "'"
                        + " Where  p.ITEMID in (Select [Free Item Code] from [dbo].[ACXSCHEMEVIEW]  "
                        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  "
                        + " and SCHEMECODE = '" + SchemeCode.Value + "') "
                        + " OR ((MINIMUMQUANTITY + MINIMUMQUANTITYPCS + MINIMUMVALUE)  = (Select SUM(MINIMUMQUANTITY + MINIMUMQUANTITYPCS + MINIMUMVALUE) from[dbo].[ACXSCHEMEVIEW]"
                        + " where (MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  and SCHEMECODE = '" + SchemeCode.Value + "'  and  [SchemeLineNo]= '" + sln.Value + "') and SCHEMECODE = '" + SchemeCode.Value + "'  and  [SchemeLineNo]= '" + sln.Value + "')";

                dt = baseObj.GetData(sqlstr);
                gridView2.DataSource = dt;
                gridView2.DataBind();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null) { if (conn.State.ToString() == "Closed") { conn.Open(); } }
            }


        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
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
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        lstSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where STATECODE = '" + lstState.SelectedItem.Value + "' Order By NAME";
        //        baseObj.BindToDropDown(lstSiteId, sqlstr1, "Name", "Code");
        //    }
        //    else
        //    {
        //        lstSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order By NAME";
        //        baseObj.BindToDropDown(lstSiteId, sqlstr1, "Name", "Code");
        //    }
        //}

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    Bind_Grid();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateInput()
        {
            bool b;
            b = true;
            if (lstState.Text == string.Empty || lstState.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide State.";
            }
            else if (lstSiteId.Text == string.Empty || lstSiteId.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide Site ID.";
            }
            else
            {
                LblMessage.Visible = false;
            }

            return b;
        }

        protected void ClearAll()
        {
            gvSchemeDetail.DataSource = null;
            gvSchemeDetail.DataBind();

            gridViewSlabDetail.DataSource = null;
            gridViewSlabDetail.DataBind();


            gridViewSchemeItemGroup.DataSource = null;
            gridViewSchemeItemGroup.DataBind();

            gridViewFreeItemGroup.DataSource = null;
            gridViewFreeItemGroup.DataBind();
        }

        protected void lstSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
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
    }
}