using CreamBell_DMS_WebApps.App_Code;
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
    public partial class frmOutStandingAgingReport : System.Web.UI.Page
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
                FillState();
            }

            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                phState.Visible = false;
                ucRoleFilters.ListSiteIdChanged += UcRoleFilters_ListSiteChange;
            }
        }

        private void UcRoleFilters_ListSiteChange(object sender, EventArgs e)
        {
            FillCustomerGroup();
        }
        protected void FillState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            DataTable dt = new DataTable();

            dt = new DataTable();
            if (objcheckSitecode != null)
            {
                chkListState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                dt = baseObj.GetData(sqlstr11);
                chkListState.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "NAME";
                    chkListState.DataValueField = "Code";
                    chkListState.DataBind();
                }
            }
            else
            {
                chkListState.Items.Clear();
                chkListSite.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "StateName";
                    chkListState.DataValueField = "StateCode";
                    chkListState.DataBind();

                    chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }
                if (chkListSite.Items.Count > 1)
                {
                    chkListState.Items[0].Selected = true;
                    chkListSite.Items[0].Selected = true;
                }
                FillCustomerGroup();
            }
        }

        protected void FillSite()
        {
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
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")";
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
                // dt = baseObj.GetData(sqlstr1);
                chkListSite.Items.Clear();
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }

            }
            else
            {
                chkListSite.Items.Clear();
            }
        }
        protected void FillCustomerGroup()
        {
            DataTable dt = new DataTable();
            //            string sqlstr = "select distinct A.CUST_GROUP as code,Name=(Select CustGroup_Name from ax.ACXCUSTGROUPMASTER where Custgroup_Code=A.CUST_GROUP ) from ax.acxcustmaster A where A.site_code='" + Session["SiteCode"].ToString() + "'";
            string sqlstr = "select Distinct CustGroup_Name as Name,Custgroup_Code as Code from ax.ACXCUSTGROUPMASTER ";

            dt = new DataTable();
            dt = baseObj.GetData(sqlstr);
            chkListCustomerGroup.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chkListCustomerGroup.DataSource = dt;
                chkListCustomerGroup.DataTextField = "NAME";
                chkListCustomerGroup.DataValueField = "Code";
                chkListCustomerGroup.DataBind();
            }
        }
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }
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
            string strCustomerGroupName = "";
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
                }
            }
            if (strCustomerGroupName.Length > 0)
            {
                DataTable dt = new DataTable();
                string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
                dt = new DataTable();
                dt = baseObj.GetData(sqlstr);
                chkCustomerName.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkCustomerName.DataSource = dt;
                    chkCustomerName.DataTextField = "NAME";
                    chkCustomerName.DataValueField = "Customer_Code";
                    chkCustomerName.DataBind();
                }
            }
            else
            {
                chkCustomerName.Items.Clear();
                chkCustomerName.Items.Add("--Select--");
            }
        }
        private bool Validate()
        {
            bool b = false;

            if (txt30.Text.Trim() == string.Empty && txt60.Text.Trim() == string.Empty && txt90.Text.Trim() == string.Empty && txt120.Text == string.Empty && txtOver120.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide at least one Age Range !";
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;              
            }          
            return b;
        }
        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validate();
                if (b)
                {
                    if (!string.IsNullOrEmpty(txt30.Text.Trim()))
                    {
                        Int64 strAge30 = Convert.ToInt64(txt30.Text.Trim());
                        if (strAge30 > 30)
                        {
                           // ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Please enter valid age range!')", true);
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid age range!');", true);
                            txt30.Focus();
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(txt60.Text.Trim()))
                    {
                        Int64 strAge60 = Convert.ToInt64(txt60.Text.Trim());
                        if (strAge60 > 60)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid age range!');", true);
                            txt60.Focus();
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(txt90.Text.Trim()))
                    {
                        Int64 strAge90 = Convert.ToInt64(txt90.Text.Trim());
                        if (strAge90 > 90)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid age range!');", true);
                            txt120.Focus();
                            return;
                        }
                    }


                    if (!string.IsNullOrEmpty(txt120.Text.Trim()))
                    {
                        Int64 strAge120 = Convert.ToInt64(txt120.Text.Trim());
                        if (strAge120 > 120)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid age range!');", true);
                            txt120.Focus();
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(txtOver120.Text.Trim()))
                    {
                        Int64 strAgeOver120 = Convert.ToInt64(txtOver120.Text.Trim());
                        if (strAgeOver120 < 120)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please enter valid age range!');", true);
                            txtOver120.Focus();
                            return;
                        }
                    }  
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
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            DataTable dtDataByfilter = null;
            try
            {
                #region oldCode
                //conn = new SqlConnection(objGlobal.GetConnectionString());
                //cmd = new SqlCommand();
                //cmd.CommandTimeout = 0;
                //cmd.CommandType = CommandType.StoredProcedure;
                //query = "usp_GetCustomerOutstandingAgingReport";
                //cmd.CommandText = query;

                //if (!string.IsNullOrEmpty(txt30.Text.Trim()))
                //{
                //    cmd.Parameters.AddWithValue("@FirstParm", txt30.Text.Trim());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@FirstParm", 0);
                //}

                //if (!string.IsNullOrEmpty(txt60.Text.Trim()))
                //{
                //    cmd.Parameters.AddWithValue("@SecondParm", txt60.Text.Trim());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SecondParm", 0);
                //}

                //if (!string.IsNullOrEmpty(txt90.Text.Trim()))
                //{
                //    cmd.Parameters.AddWithValue("@Third", txt90.Text.Trim());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Third", 0);
                //}

                //if (!string.IsNullOrEmpty(txt120.Text.Trim()))
                //{
                //    cmd.Parameters.AddWithValue("@Fourth", txt120.Text.Trim());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Fourth", 0);
                //}

                //if (!string.IsNullOrEmpty(txtOver120.Text.Trim()))
                //{
                //    cmd.Parameters.AddWithValue("@FiveParm", txtOver120.Text.Trim());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@FiveParm", 0);
                //}

                //string strStateCode = "";
                //string strSiteCode = "";
                //if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                //    strStateCode = ucRoleFilters.GetCommaSepartedStateId();
                //    strSiteCode = ucRoleFilters.GetCommaSepartedSiteId();
                //} else {
                //    foreach (ListItem item in chkListState.Items)
                //    {
                //        if (item.Selected)
                //        {
                //            if (strStateCode == "")
                //            {
                //                strStateCode += "" + item.Value.ToString() + "";
                //            }
                //            else
                //            {
                //                strStateCode += "," + item.Value.ToString() + "";
                //            }
                //        }
                //    }


                //    foreach (ListItem item in chkListSite.Items)
                //    {
                //        if (item.Selected)
                //        {
                //            if (strSiteCode == "")
                //            {
                //                strSiteCode += "" + item.Value.ToString() + "";
                //            }
                //            else
                //            {
                //                strSiteCode += "," + item.Value.ToString() + "";
                //            }
                //        }
                //    }
                //}

                //if (strStateCode.Length > 0)
                //{
                //    cmd.Parameters.AddWithValue("@STATECODE", strStateCode);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@STATECODE", "");
                //}



                //if (strSiteCode.Length > 0)
                //{
                //    cmd.Parameters.AddWithValue("@SiteCode", strSiteCode);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SiteCode", "");
                //}

                //string strCustomerGroup = "";
                //foreach (ListItem item in chkListCustomerGroup.Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (strCustomerGroup == "")
                //        {
                //            strCustomerGroup += "" + item.Value.ToString() + "";
                //        }
                //        else
                //        {
                //            strCustomerGroup += "," + item.Value.ToString() + "";
                //        }
                //    }
                //}
                //if (strCustomerGroup.Length > 0)
                //{
                //    cmd.Parameters.AddWithValue("@Cust_Group", strCustomerGroup);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Cust_Group", "");
                //}

                //string strCustomerName = "";
                //foreach (ListItem item in chkCustomerName.Items)
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

                //if (strCustomerName.Length > 0)
                //{
                //    cmd.Parameters.AddWithValue("@Customer_Code", strCustomerName);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Customer_Code", "");
                //}
                //dtDataByfilter = new DataTable();
                //cmd.Connection = objGlobal.GetConnection();
                //dtDataByfilter.Load(cmd.ExecuteReader());

                #endregion
                SqlParameter[] sqlParameters = null;
                SqlParameter sqlParameterFirstParm = null;
                SqlParameter sqlParameterSecondParm = null;

                SqlParameter sqlParameterThird = null;
                SqlParameter sqlParameterFourth = null;
                SqlParameter sqlParameterFiveParm = null;
                SqlParameter sqlParameterSTATECODE = null;
                SqlParameter sqlParameterSiteCode = null;
                SqlParameter sqlParameterCust_Group = null;
                SqlParameter sqlParameterCustomer_Code = null;

                if (!string.IsNullOrEmpty(txt30.Text.Trim()))
                {
                    sqlParameterFirstParm = new SqlParameter("@FirstParm", txt30.Text.Trim());
                }
                else
                {
                    sqlParameterFirstParm = new SqlParameter("@FirstParm", 0);
                    sqlParameterFirstParm.Value = 0;

                }

                if (!string.IsNullOrEmpty(txt60.Text.Trim()))
                {
                    sqlParameterSecondParm = new SqlParameter("@SecondParm", txt60.Text.Trim());

                }
                else
                {
                    sqlParameterSecondParm = new SqlParameter("@SecondParm", 0);
                    sqlParameterSecondParm.Value = 0;

                }

                if (!string.IsNullOrEmpty(txt90.Text.Trim()))
                {
                    sqlParameterThird = new SqlParameter("@Third", txt90.Text.Trim());

                }
                else
                {
                    sqlParameterThird = new SqlParameter("@Third", 0);
                    sqlParameterThird.Value = 0;

                }

                if (!string.IsNullOrEmpty(txt120.Text.Trim()))
                {
                    sqlParameterFourth = new SqlParameter("@Fourth", txt120.Text.Trim());

                }
                else
                {
                    sqlParameterFourth = new SqlParameter("@Fourth", 0);
                    sqlParameterFourth.Value = 0;

                }

                if (!string.IsNullOrEmpty(txtOver120.Text.Trim()))
                {
                    sqlParameterFiveParm = new SqlParameter("@FiveParm", txtOver120.Text.Trim());

                }
                else
                {
                    sqlParameterFiveParm = new SqlParameter("@FiveParm",0);
                    sqlParameterFiveParm.Value = 0;

                }

                string strStateCode = "";
                string strSiteCode = "";
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    strStateCode = ucRoleFilters.GetCommaSepartedStateId();
                    strSiteCode = ucRoleFilters.GetCommaSepartedSiteId();
                }
                else
                {
                    foreach (ListItem item in chkListState.Items)
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


                    foreach (ListItem item in chkListSite.Items)
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
                }

                if (strStateCode.Length > 0)
                {
                    sqlParameterSTATECODE = new SqlParameter("@STATECODE", strStateCode);

                }
                else
                {
                    sqlParameterSTATECODE = new SqlParameter("@STATECODE", "");

                }



                if (strSiteCode.Length > 0)
                {
                    sqlParameterSiteCode = new SqlParameter("@SiteCode", strSiteCode);
                }
                else
                {
                    sqlParameterSiteCode = new SqlParameter("@SiteCode", "");

                }

                string strCustomerGroup = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
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
                    sqlParameterCust_Group = new SqlParameter("@Cust_Group", strCustomerGroup);

                }
                else
                {
                    sqlParameterCust_Group = new SqlParameter("@Cust_Group", "");

                }

                string strCustomerName = "";
                foreach (ListItem item in chkCustomerName.Items)
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
                    sqlParameterCustomer_Code = new SqlParameter("@Customer_Code", strCustomerName);

                }
                else
                {
                    sqlParameterCustomer_Code = new SqlParameter("@Customer_Code", "");

                }

                sqlParameters = new SqlParameter[] { sqlParameterFirstParm, sqlParameterSecondParm, sqlParameterThird,sqlParameterFourth,sqlParameterFiveParm,sqlParameterSTATECODE,
                    sqlParameterSiteCode,sqlParameterCust_Group,sqlParameterCustomer_Code};
                dtDataByfilter = CreamBellFramework.GetDataFromStoredProcedure("dbo.usp_GetCustomerOutstandingAgingReport", sqlParameters);

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
                objGlobal.CloseSqlConnection();
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
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3") { } else {
                        foreach (ListItem item in chkListState.Items)
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

                        
                        foreach (ListItem item in chkListSite.Items)
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
                    }
                        
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
                    foreach (ListItem item in chkListCustomerGroup.Items)
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
                    foreach (ListItem item in chkCustomerName.Items)
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
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\CustomerOutstandingAgingReport.rdl");

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