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
    public partial class frmTargetSummaryReport : System.Web.UI.Page
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
                string sqlstr = @"select  distinct TC.Name,TC.Category as Code from  [ax].[ACXTARGETCATEGORY] TC ";

                ddlClaimCat.Items.Add("Select...");
                baseObj.BindToDropDown(ddlClaimCat, sqlstr, "Name", "Code");

              //  txtFromDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today.AddDays(-1));
               // txtToDate.Text = string.Format("{0:dd-MMM-yyyy }", DateTime.Today);
            }
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = Validate();
            if (b)
            {
                ShowReport();
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


                string sqlstr = @"Select SITE_CODE,'"+obj1.ToString()+"' as Name,from_Date,To_Date,TC.Name as Cat,TS.Name as Subcat,Target_Description,Actual_Incentive " +
                                "   from [ax].[ACXCLAIMMASTER] CM " +
                                "   left join [ax].[ACXTARGETCATEGORY] TC on CM.Claim_Category = TC.CATEGORY " +
                                "   left join [ax].[ACXTARGETSUBCATEGORY] TS on CM.CLAIM_SUBCATEGORY = TS.Subcategory where CM.from_Date  >=" +
                                   " '" + Convert.ToDateTime(txtFromDate.Text) + "' and CM.TO_DATE <= '" + Convert.ToDateTime(txtToDate.Text) + "' and CM.SITE_CODE = '" + Session["SiteCode"].ToString() + "' ";
                                 //  "  ";

                if (ddlClaimCat.SelectedItem.Text != "Select..." && ddlClaimCat.SelectedItem.Text != " ")
                {
                    sqlstr += " and CM.Claim_Category = '"+ddlClaimCat.SelectedItem.Value+"' ";
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
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\TargetReport.rdl");
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