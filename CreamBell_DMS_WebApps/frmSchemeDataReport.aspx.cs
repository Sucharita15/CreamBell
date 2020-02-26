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
    public partial class frmSchemeDataReport : System.Web.UI.Page
    {
        string strQuery = string.Empty;
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        protected void Page_Load(object sender, EventArgs e)
        {
           if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (Session["ISDISTRIBUTOR"] == null || Session["ISDISTRIBUTOR"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                FillSchemeType();
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
            //if (ddlSiteId.SelectedIndex == -1)
            //{
            //    b = false;
            //    LblMessage.Text = "Please Select State...";
            //}

            return b;
        }

         private void ShowReportSummary()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dtSetData = null;

            try
            {
                string query = "Select Top 1 NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                object obj1 = obj.GetScalarValue(query);


                string sqlstr = @"Select I.Siteid,I.Name,I.StateCode,from_Date,To_Date,TC.Name as Cat,TS.Name as Subcat,Target_Description,Actual_Incentive,
                                    Case when Claim_Type = '0' then 'Sale' else 'Purchase' end as ClaimType 
                                    from [ax].[ACXCLAIMMASTER] CM 
                                    left join 
                                    [ax].[ACXTARGETCATEGORY] TC on CM.Claim_Category = TC.CATEGORY
                                    left join 
                                    [ax].[ACXTARGETSUBCATEGORY] TS on CM.CLAIM_SUBCATEGORY = TS.Subcategory
                                    left join 
                                    [ax].[INVENTSITE] I on CM.SITE_CODE = I.Siteid
                                    where CM.from_Date  >= " +
                                   " '" + Convert.ToDateTime(txtFromDate.Text) + "' and CM.TO_DATE <= '" + Convert.ToDateTime(txtToDate.Text) + "'  ";
                //  "  ";
        //        if (ddlSiteId.SelectedIndex != -1)
        //        {
        //            if (ddlSiteId.SelectedItem.Text != "Select...")
        //            {
        //                sqlstr += "and CM.SITE_CODE = '" + ddlSiteId.SelectedItem.Value + "'  ";
        //            }
        //        }

                dtSetData = new DataTable();
                dtSetData = obj.GetData(sqlstr);

                LoadDataInReportViewerDetail(dtSetData);
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
                    DataSetParameter.Columns.Add("SchemeCode");
                    DataSetParameter.Columns.Add("Scheme");
                    
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    if (ddlSiteIdNew.SelectedIndex != -1)
                    {
                        if (ddlSiteIdNew.SelectedItem.Text == "Select...")
                        {
                            DataSetParameter.Rows[0]["SchemeCode"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["SchemeCode"] = ddlSiteIdNew.SelectedItem.Text;
                        }
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["SchemeCode"] = "All";
                    }

                    if (ddlSchemeNew.SelectedItem.Text == "Select...")
                    {
                        DataSetParameter.Rows[0]["SchemeCode"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["SchemeCode"] = ddlSchemeNew.SelectedItem.Text;
                    }
                  

                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\SCHEMEDATAREPORT.rdl");
                    //ReportViewer1.LocalReport.Refresh();
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

       
         protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            DataTable dt = new DataTable();
            bool b = Validate();
            if (b == true)
            {
                try
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;

                    string query = "ACX_USP_SchemeDataReport";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    
                    ilist.Add("@FromDate"); item.Add(txtFromDate.Text);
                    ilist.Add("@ToDate"); item.Add(txtToDate.Text);
                    ilist.Add("@SchemeCode");

                    if (ddlSiteIdNew.SelectedIndex != -1)
                    {
                        if (ddlSiteIdNew.SelectedItem.Text != "Select...")
                        {
                            item.Add(ddlSiteIdNew.SelectedItem.Value);
                        }
                        else
                        {
                            item.Add("0");
                        }
                    }
                    else
                    {
                        item.Add("0");
                    }

		            if (ddlSchemeNew.SelectedIndex == 0)
                    {
                        ilist.Add("@SalesType"); item.Add("0");
                    }
                    else
                    {
                        ilist.Add("@SalesType"); item.Add(ddlSchemeNew.SelectedItem.Text);
                    }
                    ilist.Add("@UserCode"); item.Add(ucRoleFilters.GetCommaSepartedSiteId());
                    //  ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                    // ilist.Add("@SiteId"); item.Add(Session["USERID"].ToString());

                    dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                    //if (dt.Rows.Count > 0)
                    //{
                        LoadDataInReportViewerDetail(dt);
                    //}
                    
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        public void FillSchemeType()
        {
            //if ((Session["ISDISTRIBUTOR"].ToString() == "Y"))
            // {
            //    ddlScheme.Visible = false;
            //     LblScheme.Visible = false;
            //     ddlScheme_SelectedIndexChanged(null, null);
            // }
            // else
            // {
            ddlSchemeNew.Items.Insert(0, new ListItem("All", "0"));
            ddlSchemeNew.Items.Insert(1, new ListItem("State", "1"));
            ddlSchemeNew.Items.Insert(2, new ListItem("Site", "2"));
            ddlSchemeNew.SelectedIndex = 2;
            ddlScheme_SelectedIndexChanged(null, null);
            //}
        }
         protected void ddlScheme_SelectedIndexChanged(object sender, EventArgs e) 
         {
            string strSubQuery;
            strSubQuery = "";
            if ((Session["ISDISTRIBUTOR"].ToString() == "Y"))
            {
                if (ddlSchemeNew.SelectedIndex==1)
                {
                    strSubQuery = "AND SALESCODE='" + Convert.ToString(Session["SCHSTATE"]) + "'";
                }
                if (ddlSchemeNew.SelectedIndex == 2)
                {
                    strSubQuery = "AND SALESCODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
                }
                if (ddlSchemeNew.SelectedIndex == 0)
                {
                    strSubQuery = "AND (SALESCODE IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") or SALESCODE='" + Convert.ToString(Session["SCHSTATE"]) + "' or SALESCODE='') ";
                }
            }
            string strQuery = "";
            if (ddlSchemeNew.SelectedItem.Text!="All")
            {
                strQuery = "Select Distinct SCHEMECODE,[Scheme Description] from [dbo].[ACXSCHEMEVIEW] where SCHEMECODE <>'' and [Sales Type] ='" + ddlSchemeNew.SelectedItem.Text + "'" + strSubQuery;
            }
            else
            {
                strQuery = "Select Distinct SCHEMECODE,[Scheme Description] from [dbo].[ACXSCHEMEVIEW] where SCHEMECODE <>'' " + strSubQuery;
            }
            
            ddlSiteIdNew.Items.Clear();
            ddlSiteIdNew.Items.Add("Select...");

            baseObj.BindToDropDownp(ddlSiteIdNew, strQuery, "Scheme Description", "SCHEMECODE");
            ReportViewer1.Visible = false;
        }
           
   }
       
}