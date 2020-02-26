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
    public partial class frmTargetDetailReport : System.Web.UI.Page
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
                string sqlstr = @"select  distinct TC.Name,TC.Category as Code from  [ax].[ACXTARGETCATEGORY] TC  ";

                ddlClaimCat.Items.Add("Select...");
                baseObj.BindToDropDown(ddlClaimCat, sqlstr, "Name", "Code");

               // txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
               // txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);
            }

        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = Validate();
            if (b)
            {
                ShowReport();
                uppanel.Update();
            }
        }

        private bool Validate()
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
            return b;
        }

        private void ShowReport()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtSetData = null;
            try
            {
                string query = "Select Top 1 NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                object obj1 = obj.GetScalarValue(query);

                string sqlstr = @"Select Case when Claim_Type = '0' then 'Sale' else 'Purchase' end as ClaimType,
                                Document_Code as ClaimDocCode,DOCUMENT_DATE as ClaimDocDate,from_Date,To_Date,
                                Case when OBJECT =0 then 'PSR' when OBJECT =1 then 'SITE'  when OBJECT =2 then 'Customer Group' end as Object,
                                Object_Code, 
                                Object_Name1 = (Select Case when OBJECT = 0 then 
                                (select Top 1 PSR_Name from [ax].[ACXPSRMaster] P where PSR_Code = Object_Code) 
                                 when OBJECT = 1 then 
                                (select Top 1 Name from [ax].[INVENTSITE] where SiteID = Object_Code)
                                when OBJECT = 2 then 
                                (Select Top 1 CUSTGROUP_NAME from [ax].[ACXCUSTGROUPMASTER] where CustGroup_Code= Object_Code) 
                                end  ),
                                Object_SubCode,Target_GROUP,
                                Object_SubName = (Select Case when OBJECT = 2 then
                                (select Top 1 CUSTOMER_NAME from [ax].[ACXCUSTMASTER] where Customer_Code = Object_SubCode) else ''  end),
                                Target_Code,Target_Description,CM.Target,ACHIVEMENT,ACTUAL_INCENTIVE,TARGET_CODE,CAlCULATIONON=(Select distinct CAlCULATIONON from [ax].[ACXTARGETLINE] where th.TargetCode=TargetCode),Target_INCENTIVE,
                                TC.Name as Cat,TS.Name as Subcat 
                                from [ax].[ACXCLAIMMASTER] CM   
                                 left join [ax].[ACXTARGETCATEGORY] TC on CM.Claim_Category = TC.CATEGORY  
                                left join [ax].[ACXTARGETSUBCATEGORY] TS on CM.CLAIM_SUBCATEGORY = TS.Subcategory 
                                left Join [ax].[ACXtargetheader] th on CM.Target_Code = th.targetcode 
                                
                                where " +
                                " from_Date >='" + Convert.ToDateTime(txtFromDate.Text) + "' and CM.TO_DATE <= '" + Convert.ToDateTime(txtToDate.Text) + "' and CM.SITE_CODE = '" + Session["SiteCode"].ToString() + "' ";

                //left join [ax].[ACXTARGETLINE] tl on th.TargetCode = tl.TargetCode and th.DataAreaId=tl.DataAreaId

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

                dtSetData = new DataTable();
                dtSetData = obj.GetData(sqlstr);

                LoadDataInReportViewer(dtSetData);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
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
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;

                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSetParameter", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\TargetDetailReport.rdl");
                    ReportViewer1.LocalReport.Refresh();
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
    }
}