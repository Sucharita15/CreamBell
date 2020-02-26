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
    public partial class frmRunningSchemeDetail : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlConnection conn = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SiteCode"] != null)
            {
                if (!Page.IsPostBack)
                {
                  //  Bind_Grid();
                    fillSiteAndState();         
                }
            }
        }
        protected void fillSiteAndState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY LS.Name ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();              
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'  ORDER BY LS.Name";
                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");              
            }
        }
        public void Bind_Grid()
        {
            try
            {
//                string sqlstr = @"Select Distinct SCHEMECODE,[SCHEME DESCRIPTION] as Name,Case when [SCHEME TYPE]=0 then 'Quantity' when [SCHEME TYPE] = 1 then 'Value' end as [SCHEME TYPE],STARTINGDATE,ENDINGDATE from [dbo].[ACXSCHEMEVIEW]
//                             Where SALESCODE in ( '" + Session["SiteCode"].ToString() + "','" + Session["SITELOCATION"].ToString() + "','') ";
                string sqlstr = @"Select Distinct SCHEMECODE,[SCHEME DESCRIPTION] as Name,Case when [SCHEME TYPE]=0 then 'Quantity' when [SCHEME TYPE] = 1 then 'Value' when [SCHEME TYPE] = 2 then 'Percent' when [SCHEME TYPE] = 3 then 'Valueoff'  end as [SCHEME TYPE],STARTINGDATE,ENDINGDATE from [dbo].[ACXSCHEMEVIEW]
                             Where SALESCODE in ( '" + ddlSiteId.SelectedItem.Value + "','" + ddlState.SelectedItem.Value + "','') ";

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
                            where SCHEMECODE = '" + lnkbtn.Text + "' and  Salescode like (Case when [Sales Type] = 'State' then '" + ddlState.SelectedItem.Value + "' else '%' end)";

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
								+ " where ASFS.SCHEMEITEMGROUP = '"+h.Value+"' " ;
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
                        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + ddlSiteId.SelectedItem.Value + "','" + ddlState.SelectedItem.Value + "','')  "
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
                        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + ddlSiteId.SelectedItem.Value + "','" + ddlState.SelectedItem.Value + "','')  "
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
                        + "	(MINIMUMQUANTITY ='" + lnkbtn.Text + "' or MINIMUMQUANTITYPCS ='" + lnkbtn.Text + "' or MINIMUMVALUE ='" + lnkbtn.Text + "')  AND SALESCODE in ( '" + ddlSiteId.SelectedItem.Value + "','" + ddlState.SelectedItem.Value + "','')  "
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

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' Order By NAME";               
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order By NAME";       
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
        }

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
            if (ddlState.Text == string.Empty || ddlState.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide State.";
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

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
        }
       
    }
}