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
    public partial class frmPartyWiseSaleSummaryDisc : System.Web.UI.Page
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
                FillState();
                FillCustomerGroup();
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

        protected void FillCustomerGroup()
        {
            DataTable dt = new DataTable();
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
        protected void FillState()
        {
            DataTable dt = new DataTable();

            dt = new DataTable();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            {
                chkListState1.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                dt = baseObj.GetData(sqlstr11);
                chkListState1.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState1.DataSource = dt;
                    chkListState1.DataTextField = "NAME";
                    chkListState1.DataValueField = "Code";
                    chkListState1.DataBind();
                }
            }
            else
            {
                chkListState1.Items.Clear();
                chkListSite1.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState1.DataSource = dt;
                    chkListState1.DataTextField = "StateName";
                    chkListState1.DataValueField = "StateCode";
                    chkListState1.DataBind();

                    chkListSite1.DataSource = dt;
                    chkListSite1.DataTextField = "SiteName";
                    chkListSite1.DataValueField = "SiteId";
                    chkListSite1.DataBind();
                }
                if (dt.Rows.Count > 0)
                {
                    chkListState1.Items[0].Selected = true;
                    chkListSite1.Items[0].Selected = true;
                }
            }


                
        }
        protected void FillSite()
        {
            string StateList = "";
            foreach (ListItem item in chkListState1.Items)
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
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (Convert.ToString(Session["ISDISTRIBUTOR"]).ToUpper() != "Y")
                {
                    sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by SiteName";
                }
                else
                {
                    sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where SITEID  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                }

                dt = new DataTable();
               // dt = baseObj.GetData(sqlstr1);
                chkListSite1.Items.Clear();
                dt = baseObj.GetData(sqlstr1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {                  
                    chkListSite1.DataSource = dt;
                    chkListSite1.DataTextField = "SiteName";
                    chkListSite1.DataValueField = "SiteId";
                    chkListSite1.DataBind();
                }

            }
            else
            {
                chkListSite1.Items.Clear();
            }
        }
        private bool ValidateInput()
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
        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            bool b = ValidateInput();
            if (b)
            {
                ShowReport();
            }
        }
        private void ShowReport()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            DataTable dtSetData = null;
            DataTable dtTotalInvoiceNo = null;
            try
            {
                string query1 = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query1);
            
                SqlConnection conn = null;
                SqlCommand cmd = null;
                DataTable dtDataByfilter = null;
                string query = string.Empty;

                //conn = new SqlConnection(obj.GetConnectionString());
                //conn.Open();
                cmd = new SqlCommand();
                //cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
               // string qr = "";
                query = "[ax].[ACX_PartyWiseSaleSummaryDiscount]";

                cmd.CommandText = query;
                DateTime now = Convert.ToDateTime(txtToDate.Text);
                now = now.AddMonths(1);
                //DateTime lastDay = new DateTime(now.Year, now.Month, 1);
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"));
                string SiteList = "";
                string StateList = "";
                if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                    SiteList = ucRoleFilters.GetCommaSepartedSiteId();
                    StateList = ucRoleFilters.GetCommaSepartedStateId();

                } else {
                    foreach (ListItem item in chkListSite1.Items)
                    {
                        if (item.Selected)
                        {
                            if (SiteList == "")
                            {
                                SiteList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                SiteList += "," + item.Value.ToString() + "";
                            }
                        }
                    }

                    
                    foreach (ListItem item in chkListState1.Items)
                    {
                        if (item.Selected)
                        {
                            if (StateList == "")
                            {
                                StateList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                StateList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                }
                    
                if (SiteList.Length > 0)
                {
                     cmd.Parameters.AddWithValue("@SiteId", SiteList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }

                
                if (StateList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", StateList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }
             
                string CustGroupList = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroupList == "")
                        {
                            CustGroupList += "" + item.Value.ToString() + "";                         
                        }
                        else
                        {
                            CustGroupList += "," + item.Value.ToString() + "";                           
                        }
                    }
                }                
                if (CustGroupList.Length>0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", "");
                }
                cmd.Connection = obj.GetConnection();
                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
               


                string queryTotInv = " Select Count(Distinct INVOICE_NO) as InvoiceNo FROM ACX_SALESUMMARY_PARTY_ITEM_WISE SP " +
                                     " Inner Join [ax].[ACXCUSTMASTER] C on C.Customer_Code = SP.CUSTOMER_CODE  and C.APPLICABLESCHEMEDISCOUNT = '2'  " +
                                     " where SITEID  IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ")  and INVOICE_DATE >=" +
                                     " '" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "' group by SP.CUSTOMER_NAME";

                dtSetData = new DataTable();
                #region product group

                string ProductGroupList = "";
                foreach (ListItem item in chkProductGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (ProductGroupList == "")
                        {
                            ProductGroupList += "'" + item.Value.ToString() + "'";
                        }
                        else
                        {
                            ProductGroupList += ",'" + item.Value.ToString() + "'";
                        }
                    }
                }
                if (ProductGroupList.Length > 0)
                {
                    dtSetData = dtDataByfilter.Select("PRODUCT_GROUP IN (" + ProductGroupList + ")").CopyToDataTable();
                }
                else
                {
                    dtSetData = dtDataByfilter;
                }
                
                #endregion
                dtTotalInvoiceNo = obj.GetData(queryTotInv);


                LoadDataInReportViewer(dtSetHeader, dtSetData, dtTotalInvoiceNo);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                obj.CloseSqlConnection();
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";

            //ddlSiteId.Items.Add("Select...");
            //baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            FillSite();

        }
        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData, DataTable dtTotalInvoiceNo)
        {
            try
            {
                if (dtSetHeader.Rows.Count > 0 && dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FromDate");
                    DataSetParameter.Columns.Add("ToDate");

                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("Customet_Group");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;

                    string strState = string.Empty;
                    string strDist = string.Empty;
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3") {
                        strState = ucRoleFilters.GetCommaSepartedStateId();
                        strDist = ucRoleFilters.GetCommaSepartedSiteId();
                    } else {
                        foreach (ListItem item in chkListState1.Items)
                        {
                            if (item.Selected)
                            {
                                strState += item.Text + ",";
                            }
                        }

                        
                        foreach (ListItem item in chkListSite1.Items)
                        {
                            if (item.Selected)
                            {
                                strDist += item.Text + ",";
                            }
                        }
                    }
                        
                    if (strState != String.Empty)
                    {
                        strState = strState.Remove(strState.Length - 1);
                    }

                    

                    if (strDist != String.Empty)
                    {
                        strDist = strDist.Remove(strDist.Length - 1);
                    }

                    string strCustGroup = string.Empty;
                    foreach (ListItem item in chkListCustomerGroup.Items)
                    {
                        if (item.Selected)
                        {
                            strCustGroup += item.Text + ",";
                        }
                    }

                    if (strCustGroup != String.Empty)
                    {
                        strCustGroup = strCustGroup.Remove(strCustGroup.Length - 1);
                    }

                    DataSetParameter.Rows[0]["StateCode"] = strState;
                    DataSetParameter.Rows[0]["Distributor"] = strDist;
                    DataSetParameter.Rows[0]["Customet_Group"] = strCustGroup;

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PartyWiseSaleSummaryDiscount.rdl");
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    ReportDataSource RDS3 = new ReportDataSource("TotalDistinctSumGroupbyInvoiceNo", dtTotalInvoiceNo);
                    ReportViewer1.LocalReport.DataSources.Add(RDS3);
                    ReportViewer1.ShowPrintButton = true;
                    this.ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;
                    ReportViewer1.ZoomPercent = 100;
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
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();       
        }
        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            ShowData_ForExcel();
        }
        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            try
            {
                string query1 = "Select NAME from ax.inventsite where SITEID IN (" + ucRoleFilters.GetCommaSepartedSiteId() + ") ";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query1);

                SqlConnection conn = null;
                SqlCommand cmd = null;
                DataTable dtDataByfilter = null;
                string query = string.Empty;

                //conn = new SqlConnection(obj.GetConnectionString());
                //conn.Open();
                cmd = new SqlCommand();
                // cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "ACX_PartyWiseSaleSummaryDiscount_ExcelOutput";

                cmd.CommandText = query;
                DateTime now = Convert.ToDateTime(txtToDate.Text);
                now = now.AddMonths(1);

                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"));
                string SiteList = "";
                string StateList = "";
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    SiteList = ucRoleFilters.GetCommaSepartedSiteId();
                    StateList = ucRoleFilters.GetCommaSepartedStateId();
                }
                else
                {
                    foreach (ListItem item in chkListSite1.Items)
                    {
                        if (item.Selected)
                        {
                            if (SiteList == "")
                            {
                                SiteList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                SiteList += "," + item.Value.ToString() + "";
                            }
                        }
                    }


                    foreach (ListItem item in chkListState1.Items)
                    {
                        if (item.Selected)
                        {
                            if (StateList == "")
                            {
                                StateList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                StateList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                }

                if (SiteList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", SiteList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }


                if (StateList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", StateList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }

                string CustGroupList = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroupList == "")
                        {
                            CustGroupList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            CustGroupList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (CustGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", "");
                }

                dtDataByfilter = new DataTable();
                cmd.Connection = obj.GetConnection();
                dtDataByfilter.Load(cmd.ExecuteReader());
                DataTable dt = new DataTable();
                #region product group

                string ProductGroupList = "";
                foreach (ListItem item in chkProductGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (ProductGroupList == "")
                        {
                            ProductGroupList += "'" + item.Value.ToString() + "'";
                        }
                        else
                        {
                            ProductGroupList += ",'" + item.Value.ToString() + "'";
                        }
                    }
                }
                if (ProductGroupList.Length > 0)
                {
                    dt = dtDataByfilter.Select("PRODUCT_GROUP IN (" + ProductGroupList + ")").CopyToDataTable();
                }
                else
                {
                    dt = dtDataByfilter;
                }

                #endregion
                //dt = dtDataByfilter;

                //=================Create Excel==========

                string attachment = "attachment; filename=PartyWiseSaleSummaryDiscount.xls";
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
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally {
                obj.CloseSqlConnection();
            }
        }
        
    }
}