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
    public partial class frmTargetReport : System.Web.UI.Page
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
                string sqlstr = @"Select Distinct TC.Name,TC.Category as Code from  [ax].[ACXTARGETCATEGORY] TC ";
                ddlClaimCat.Items.Add("Select...");
                baseObj.BindToDropDown(ddlClaimCat, sqlstr, "Name", "Code");
                string sqlstr11 ;
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                    sqlstr11 = "Select Distinct StateCode Code,StateCode+'-'+lg.NAME Name from [ax].[INVENTSITE] sa inner join ax.LOGISTICSADDRESSSTATE lg on lg.STATEID = sa.STATECODE where STATECODE<>'' AND SITEID='" + Convert.ToString(Session["SiteCode"]) + "' ";
                else
                    sqlstr11 = "Select Distinct StateCode Code,StateCode+'-'+lg.NAME Name from [ax].[INVENTSITE] sa inner join ax.LOGISTICSADDRESSSTATE lg on lg.STATEID = sa.STATECODE where STATECODE<>''";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                if (ddlState.Items.Count == 2)
                {
                    ddlState.SelectedIndex = 1;
                    ddlCountry_SelectedIndexChanged(sender, e);
                }
                //ddlBusinessUnit.Items.Add("Select...");
                //ddlSiteId.Items.Add("Select...");

            }
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            //bool b = ValidateInput();
            if (ValidateInput())
            {
                if (rdoSummary.Checked)
                {
                    LblMessage.Text = "";
                    ShowReportSummary();
                    MultiView1.ActiveViewIndex = 0;
                }
                if (rdoDetail.Checked)
                {
                    ShowReportDetail();
                    MultiView1.ActiveViewIndex = 1;
                }
            }
            else
            {
                ReportViewer1.Visible = false;
                ReportViewer2.Visible = false;
            }
        }

        private bool ValidateInput()
        {
            bool b=true;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide From Date and To Date";
                return b;
            }
            if (ddlSiteId.SelectedItem==null || ddlSiteId.SelectedItem.Value == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Select Site";
                return b;
            }
            if (ddlBusinessUnit.SelectedItem==null || ddlBusinessUnit.SelectedItem.Value == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Select BUUNIT";
                return b;
            }
            //if (ddlClaimCat.SelectedItem.Value == "Select...")
            //{
            //    b = false;
            //    LblMessage.Text = "Please Select Claim Category";
            //    return b;
            //}
            //if (ddlClaimSubCat.SelectedItem.Value == "Select...")
            //{
            //    b = false;
            //    LblMessage.Text = "Please Select Claim SubCategory";
            //    return b;
            //}
            else
            {
                LblMessage.Text = "";
            }
            //if (ddlSiteId.SelectedIndex == -1)
            //{
            //    b = false;
            //    LblMessage.Text = "Please Select State...";
            //}

            return b;
        }

        private void ShowReportSummary()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            DataTable dtSetData = null;

            try
            {
                string query = "Select Top 1 NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                object obj1 = obj.GetScalarValue(query);


                string sqlstr = @"Select I.Siteid,I.Name,I.StateCode,from_Date,To_Date,TC.Name as Cat,TS.Name as Subcat,Target_Description,Actual_Incentive,CM.BU_CODE,
                                    Case when Claim_Type = '0' then 'Sale' else 'Purchase' end as ClaimType 
                                    from [ax].[ACXCLAIMMASTER] CM 
                                    left join 
                                    [ax].[ACXTARGETCATEGORY] TC on CM.Claim_Category = TC.CATEGORY
                                    left join 
                                    [ax].[ACXTARGETSUBCATEGORY] TS on CM.CLAIM_SUBCATEGORY = TS.Subcategory
                                    left join 
                                    [ax].[INVENTSITE] I on CM.SITE_CODE = I.Siteid
                                    where CM.from_Date  >= " +
                                   " '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and CM.TO_DATE <= '" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "'";
         
                if (ddlClaimType.SelectedItem.Text != "Select...")
                {
                    if (ddlClaimType.SelectedItem.Text == "Sale")
                    {
                        sqlstr += "and CM.Claim_Type = '0'  ";
                    }
                    if (ddlClaimType.SelectedItem.Text == "Purchase")
                    {
                        sqlstr += "and CM.Claim_Type = '1'  ";
                    }
                    
                }

                if (ddlSiteId.SelectedIndex != -1)
                {
                    if (ddlSiteId.SelectedItem.Text != "Select...")
                    {
                        sqlstr += "and CM.SITE_CODE = '" + ddlSiteId.SelectedItem.Value + "'  ";
                    }
                }

                if (ddlClaimCat.SelectedItem.Text != "Select..." && ddlClaimCat.SelectedItem.Text != " ")
                {
                    sqlstr += " and CM.Claim_Category = '" + ddlClaimCat.SelectedItem.Value + "' ";
                }

                if (ddlClaimSubCat.SelectedIndex != -1)
                {
                    if (ddlClaimSubCat.SelectedItem.Text != "Select..." && ddlClaimSubCat.SelectedItem.Text != " ")
                    {
                        sqlstr += " and CM.CLAIM_SubCategory = '" + ddlClaimSubCat.SelectedItem.Value + "' ";
                    }
                }
                if (ddlBusinessUnit.SelectedIndex != -1)
                {
                    if (ddlBusinessUnit.SelectedItem.Text != "Select..." && ddlBusinessUnit.SelectedItem.Text != " ")
                    {
                        sqlstr += " and CM.BU_CODE = '" + ddlBusinessUnit.SelectedItem.Value + "' ";
                    }
                }
                dtSetData = new DataTable();
                dtSetData = obj.GetData(sqlstr);

                LoadDataInReportViewerSummary(dtSetData);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadDataInReportViewerSummary(DataTable dtSetData)
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
                    DataSetParameter.Columns.Add("BUCode");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;

                    if (ddlState.SelectedItem.Text == "Select...")
                    {
                        DataSetParameter.Rows[0]["StateCode"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "Select...")
                        {
                            DataSetParameter.Rows[0]["Distributor"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Text;
                        }
                    }
                    DataSetParameter.Rows[0]["BUCode"] = ddlBusinessUnit.SelectedItem.Value;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSetParameter", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ClaimReport.rdl");
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

        protected void ddlClaimCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr1 = @"Select Distinct  TS.Name as Name,TS.Subcategory as Code from [ax].[ACXTARGETSUBCATEGORY] TS
									 where CATEGORY  ='" + ddlClaimCat.SelectedItem.Value + "'";

            ddlClaimSubCat.Items.Clear();
            ddlClaimSubCat.Items.Add("Select...");
            baseObj.BindToDropDown(ddlClaimSubCat, sqlstr1, "Name", "Code");
        }

        private void ShowReportDetail()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            DataTable dt = new DataTable();
            try
            {
                //string query = "Select Top 1 NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                //object obj1 = obj.GetScalarValue(query);

                string FromDate = txtFromDate.Text;
                string ToDate = txtToDate.Text;
                string Claim_Type = string.Empty;
                string SITEID = string.Empty;
                string Claim_Category = string.Empty;
                string CLAIM_SubCategory = string.Empty;
                string BUCODE = string.Empty;

                string query = "[dbo].[ACX_USP_EXPENSEDETAIL_TI_Version]";
                //string query = "ACX_USP_EXPENSEDETAIL";
                List<string> ilist = new List<string>();
                List<string> item = new List<string>();
                ilist.Add("@FromDate"); item.Add(txtFromDate.Text);
                ilist.Add("@ToDate"); item.Add(txtToDate.Text);
                //string sqlstr = "exec Acx_usp_ExpenseDetail '"+ Convert.ToDateTime(txtFromDate.Text) + "','" + Convert.ToDateTime(txtToDate.Text) + "' ";



                //string sqlstr = @"Select I.StateCode as State,I.Siteid as DistributorCode				
                //                ,I.Name DistributorName			
                //                ,Case when Claim_Type = '0' then 'Sale' else 'Purchase' end as ClaimType,
                //                Document_Code as ClaimDocNo,DOCUMENT_DATE as ClaimDocDate,from_Date,To_Date,
                //                Case when OBJECT =0 then 'PSR' when OBJECT =1 then 'SITE'  when OBJECT =2 then 'Customer Group' end as Object,
                //                Object_Code, 
                //                Object_Name1 = (Select Case when OBJECT = 0 then 
                //                (select Top 1 PSR_Name from [ax].[ACXPSRMaster] P where PSR_Code = Object_Code) 
                //                 when OBJECT = 1 then 
                //                (select Top 1 Name from [ax].[INVENTSITE] where SiteID = Object_Code)
                //                when OBJECT = 2 then 
                //                (Select Top 1 CUSTOMER_NAME from [ax].[ACXCUSTMASTER] where Customer_Code= Object_Code) 
                //                end ),
                //                Object_SubCode,Target_GROUP,
                //                Object_SubName = (Select Case when OBJECT = 2 then
                //                (select Top 1 CUSTGROUP_NAME from [ax].[ACXCUSTGROUPMASTER] where CustGroup_Code = Object_SubCode) else ''  end),
                //                Target_Code,Target_Description,CM.Target,ACHIVEMENT,ACTUAL_INCENTIVE,TARGET_CODE,tl.CAlCULATIONON,Target_INCENTIVE,
                //                TC.Name as Cat,TS.Name as Subcat,CM.CALCULATION_PATTERN from [ax].[ACXCLAIMMASTER] CM   
                //                 left join [ax].[ACXTARGETCATEGORY] TC on CM.Claim_Category = TC.CATEGORY  
                //                left join [ax].[ACXTARGETSUBCATEGORY] TS on CM.CLAIM_SUBCATEGORY = TS.Subcategory 
                //                left Join [ax].[ACXtargetheader] th on CM.Target_Code = th.targetcode
                //                left join [ax].[ACXTARGETLINE] tl on th.TargetCode = tl.TargetCode and th.DataAreaId=tl.DataAreaId
                //                AND CM.OBJECT=TL.TARGETOBJECT AND CM.OBJECT_SUBCODE=TL.TARGETSUBOBJECT AND CM.OBJECT_CODE=TL.OBJECTCODE 
                //                AND CM.FROM_DATE=TL.STARTDATE AND CM.TO_DATE=TL.ENDDATE AND CM.TARGET=TL.TARGET
                //                left join [ax].[INVENTSITE] I on CM.SITE_CODE = I.Siteid where" +
                //                " CM.from_Date >='" + Convert.ToDateTime(txtFromDate.Text) + "' and CM.TO_DATE <= '" + Convert.ToDateTime(txtToDate.Text) + "' ";
                ilist.Add("@Claim_Type");
                if (ddlClaimType.SelectedItem.Text != "Select...")
                {
                    //item.Add(ddlClaimType.SelectedItem.Value);

                    if (ddlClaimType.SelectedItem.Text == "Sale")
                    {
                        Claim_Type = "0";
                        item.Add(Claim_Type);
                    }
                  else if (ddlClaimType.SelectedItem.Text == "Purchase")
                    {
                        Claim_Type = "1";
                        item.Add(Claim_Type);
                    }

                }
                else
                {
                    item.Add("");
                }
                ilist.Add("@SITEID");
                if (ddlSiteId.SelectedIndex != -1)
                {
                    if (ddlSiteId.SelectedItem.Text != "Select...")
                    {
                        // item.Add(ddlSiteId.SelectedItem.Value);
                        SITEID = ddlSiteId.SelectedItem.Value;
                      item.Add(SITEID);
                    }
                    else
                    {
                        item.Add("");
                    }

                }
                else
                {
                    item.Add("");
                }
                ilist.Add("@Claim_Category");
                if (ddlClaimCat.SelectedItem.Text != "Select..." && ddlClaimCat.SelectedItem.Text != "")
                {
                    Claim_Category = ddlClaimCat.SelectedItem.Value;
                  item.Add(Claim_Category);
                    // item.Add(ddlClaimCat.SelectedItem.Value);
                }
                else
                {
                    item.Add("");
                }

                ilist.Add("@CLAIM_SubCategory");
                if (ddlClaimSubCat.SelectedIndex != -1)
                {
                    if (ddlClaimSubCat.SelectedItem.Text != "Select..." && ddlClaimSubCat.SelectedItem.Text != "")
                    {
                        CLAIM_SubCategory = ddlClaimSubCat.SelectedItem.Value;
                          item.Add(CLAIM_SubCategory);
                        //  item.Add(ddlClaimSubCat.SelectedItem.Value);
                    }
                    else
                    {
                        item.Add("");
                    }
                    
                }
                else
                {
                    item.Add("");
                }

                ilist.Add("@BUCode");
                if (ddlBusinessUnit.SelectedIndex != -1)
                {
                    if (ddlBusinessUnit.SelectedItem.Text != "Select..." && ddlBusinessUnit.SelectedItem.Text != "")
                    {
                        BUCODE = ddlBusinessUnit.SelectedItem.Value;
                        item.Add(BUCODE);
                    }
                    else
                    {
                        item.Add("");
                    }
                }
                else
                {
                    item.Add("");
                }

                dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);
                if (dt.Rows.Count > 0)
                {
                    LblMessage.Text = "";
                    LoadDataInReportViewerDetail(dt);
                    ReportViewer1.Visible = true;
                }else
                {
                    LblMessage.Text = "No data available for selected criteria, please try again.";
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LoadDataInReportViewerDetail(DataTable dtSetData)
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
                    DataSetParameter.Columns.Add("BUCode");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    if (ddlState.SelectedItem.Text == "Select...")
                    {
                        DataSetParameter.Rows[0]["StateCode"] = "All";      
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;    
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "Select...")
                        {
                            DataSetParameter.Rows[0]["Distributor"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Text;
                        }
                    }
                    DataSetParameter.Rows[0]["BUCode"] = ddlBusinessUnit.SelectedItem.Value;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer2.LocalReport.DataSources.Clear();
                    ReportViewer2.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSetParameter", DataSetParameter);
                    ReportViewer2.LocalReport.DataSources.Add(RDS2);
                    ReportViewer2.LocalReport.ReportPath = Server.MapPath("Reports\\ClaimDetailReport.rdl");
                    ReportViewer2.LocalReport.Refresh();
                    ReportViewer2.Visible = true;
                }
                else
                {
                    LblMessage.Text = "No Records Exists !!";
                    ReportViewer2.Visible = false;
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
            ddlSiteId.Items.Clear();
            string sqlstr1;
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' AND SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
            }
            else
            {
                sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
            }
            ddlSiteId.Items.Clear();
            ddlSiteId.Items.Add("Select...");
            baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            if (ddlSiteId.Items.Count == 2)
            {
                ddlSiteId.SelectedIndex = 1;
                ddlSiteId_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlBUnit()
        {
            ddlBusinessUnit.Items.Clear();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID ='" + ddlSiteId.SelectedValue.ToString()+"'";
            ddlBusinessUnit.Items.Add("Select...");
            baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBUnit();
        }
    }
}