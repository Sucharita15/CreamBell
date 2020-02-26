
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
    public partial class frmDailySaleTrackingReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                //this.ReportViewer1.LocalReport.Refresh();
                return;
            }

            if (!IsPostBack)
            {
                //this.ReportViewer1.LocalReport.Refresh();
                baseObj.FillSaleHierarchy();
                //FillSite();
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
                            
                        }
                    }
                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    tclink.Width = "10%";
                    tclabel.Width = "90%";

                    Panel1.Visible = true;
                    LinkButton1.Visible = true;
                }
                else
                {
                    tclink.Width = "0%";
                    tclabel.Width = "100%";

                    Panel1.Visible = false;
                    LinkButton1.Visible = false;
                    LinkButton1.Enabled = false;

                }
                //uppanel.Update();
                lstSTATE_SelectedIndexChanged(null, null);
            }
            
        }
        //protected void FillCustomerGroup()
        //{
        //    DataTable dt = new DataTable();
        //    string sqlstr = "select Distinct CustGroup_Name as Name,Custgroup_Code as Code from ax.ACXCUSTGROUPMASTER ";
        //    dt = new DataTable();
        //    dt = baseObj.GetData(sqlstr);
        //    drpPSR.Items.Clear();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        drpPSR.DataSource = dt;
        //        drpPSR.DataTextField = "NAME";
        //        drpPSR.DataValueField = "Code";
        //        drpPSR.DataBind();
        //    }
        //}
      
        protected void fillSiteAndState(DataTable dt)
        {
            try
            {
                string sqlstr = "";
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATEWNAME");
                        // dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                        lstSTATE.Items.Clear();
                        DataRow dr = dtState.NewRow();
                        dr[0] = "--Select--";
                        dr[1] = "--Select--";


                        lstSTATE.DataSource = dtState;
                        lstSTATE.DataTextField = "STATEWNAME";
                        lstSTATE.DataValueField = "STATE";
                        lstSTATE.DataBind();
                    }
                    else
                    {
                        DataTable dt2 = new DataTable();
                        sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name  Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                        //lstSTATE.Items.Add("Select...");
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.Connection = baseObj.GetConnection();
                        cmd1.CommandText = sqlstr;
                        SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                        sda.Fill(dt2);
                        //dt2.Load(cmd1.ExecuteReader());
                        lstSTATE.DataSource = dt2;
                        lstSTATE.DataTextField = "Name";
                        lstSTATE.DataValueField = "Code";
                        lstSTATE.DataBind();
                    }
                }
                else
                {
                    DataTable dt1 = new DataTable();
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                    lstSTATE.Items.Add("Select...");
                    //only name and code have to be insertd in a new datatable according to this sqlstr
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = baseObj.GetConnection();
                    cmd1.CommandText = sqlstr;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                    sda.Fill(dt1);
                    lstSTATE.DataSource = dt1;
                    lstSTATE.DataTextField = "name";
                    lstSTATE.DataValueField = "code";
                    lstSTATE.DataBind();





                }
                if (lstSTATE.Items.Count == 1)
                {
                    CheckBox7.Visible = false;
                    lstSTATE.Items[0].Selected = true;
                    //lstSTATE(null, null);
                    lstSTATE_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillSite()
        {
            try
            {
                string StateList = "";
                foreach (ListItem item in lstSTATE.Items)
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
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode != null)
                    {
                        lstDIS.Items.Clear();
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+Name Name from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") Order by Name";
                        lstDIS.Items.Add("All...");
                        DataTable dt1 = new DataTable();
                        dt1 = baseObj.GetData(sqlstr1);
                        lstDIS.DataSource = dt1;
                        lstDIS.DataTextField = "Name";
                        lstDIS.DataValueField = "Code";
                        lstDIS.DataBind();


                    }
                    else
                    {
                        lstDIS.Items.Clear();
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                            uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                            lstDIS.DataSource = uniqueCols;
                            lstDIS.DataTextField = "Name";
                            lstDIS.DataValueField = "distributor";
                            lstDIS.DataBind();

                        }
                        else
                        {
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                            //drpDIS.Items.Add("All...");
                            DataTable dt1 = new DataTable();
                            dt1 = baseObj.GetData(sqlstr1);
                            lstDIS.DataSource = dt1;
                            lstDIS.DataTextField = "Name";
                            lstDIS.DataValueField = "Code";
                            lstDIS.DataBind();
                        }
                        if (lstDIS.Items.Count == 1)
                        {
                            CheckBox8.Visible = false;
                            lstDIS.Items[0].Selected = true;
                            lstDIS_SelectedIndexChanged(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
            DataTable dtTotalInvoiceNo = null;
            try
            {
                DateTime Todate = Convert.ToDateTime(txtFromDate.Text);
               // DateTime FromDate = DateTime.Today.AddDays(1 - Todate.Day);
               // DateTime date = DateTime.Now;
                var firstDayOfMonth = new DateTime(Todate.Year, Todate.Month, 1);
                DateTime FromDate = firstDayOfMonth;

                string query1 = "Select NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
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
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "[ax].[ACX_DailySaleTrackingReport]";
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(FromDate));
                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(Todate));
                cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));
                string SiteList = "";
                foreach (ListItem item in lstDIS.Items)
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
                if (SiteList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", SiteList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }
                string CustGroupList = "";
                foreach (ListItem item in lstPSRNAME.Items)
                {
                    if (item.Selected)
                    {
                        if (CustGroupList == "")
                        {
                            CustGroupList += "" + item.Text.ToString() + "";
                        }
                        else
                        {
                            CustGroupList += "," + item.Text.ToString() + "";
                        }
                    }
                }
                if (CustGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@PSR_Code", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PSR_Code", "");
                }

                string ListState = "";
                foreach (ListItem item in lstSTATE.Items)
                {
                    if (item.Selected)
                    {
                        if (ListState == "")
                        {
                            ListState += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            ListState += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (ListState.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", ListState);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }

                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());

                //string queryTotInv = " Select Count(Distinct INVOICE_NO) as InvoiceNo FROM ACX_SALESUMMARY_PARTY_ITEM_WISE SP " +
                //                     " Inner Join [ax].[ACXCUSTMASTER] C on C.Customer_Code = SP.CUSTOMER_CODE  and C.APPLICABLESCHEMEDISCOUNT = '2'  " +
                //                     " where SITEID = '" + Session["SiteCode"].ToString() + "' and INVOICE_DATE >=" +
                //                     " '" + Convert.ToDateTime(txtFromDate.Text) + "' and  INVOICE_DATE <='" + Convert.ToDateTime(txtToDate.Text) + "' group by SP.CUSTOMER_NAME";

                //dtSetData = new DataTable();
                //dtSetData = dtDataByfilter;
                dtTotalInvoiceNo = null;
                LoadDataInReportViewer(dtSetHeader, dtDataByfilter, dtTotalInvoiceNo);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateInput()
        {
            bool b;
            if (string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                b = false;
                LblMessage.Text = "Please Provide From Date!";
            }
            else
            {
                b = true;
                LblMessage.Text = string.Empty;
            }
            return b;
        }
        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            //ShowData_ForExcel();
        }

        protected void lstDIS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string SiteList = "";
                foreach (ListItem item in lstDIS.Items)
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
                if (SiteList.Length > 0)
                {

                    if (SiteList.Length > 0)
                    {
                        DataTable dt = new DataTable();
                        string sqlstr1 = string.Empty;
                        string sqlstr = @"Select psr.PSRCode as Code,p.PSR_Name as Name from [ax].[ACXPSRSITELinkingMaster] psr 
                                  Inner join ax.acxpsrmaster p on p.PSR_Code=psr.PSRCode  
                                  where psr.PSRCode<>'' and psr.Site_Code  in (" + SiteList + ") ";
                        dt = baseObj.GetData(sqlstr);
                        lstPSRNAME.Items.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lstPSRNAME.DataSource = dt;
                            lstPSRNAME.DataTextField = "Code";
                            lstPSRNAME.DataValueField = "Name";
                            lstPSRNAME.DataBind();
                        }
                    }
                    else
                    {
                        lstPSRNAME.Items.Clear();
                    }
                    //DataTable dt = new DataTable();
                    //string sqlstr1 = string.Empty;
                    //string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                    //object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    //if (objcheckSitecode != null)
                    //{
                    //    sqlstr1 = @"SELECT Distinct ax.ACXPSRSITELinkingMaster.Site_Code, ax.ACXPSRMaster.PSR_Name, ax.ACXPSRMaster.PSR_Code FROM ax.ACXPSRSITELinkingMaster INNER JOIN  ax.ACXPSRMaster ON ax.ACXPSRSITELinkingMaster.PSRCode = ax.ACXPSRMaster.PSR_Code where Site_Code in (" + SiteList + ") order by ax.ACXPSRMaster.PSR_Name";
                    //}
                    //else
                    //{
                    //    //sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    //    sqlstr1 = @"SELECT Distinct ax.ACXPSRSITELinkingMaster.Site_Code, ax.ACXPSRMaster.PSR_Name, ax.ACXPSRMaster.PSR_Code FROM ax.ACXPSRSITELinkingMaster INNER JOIN  ax.ACXPSRMaster ON ax.ACXPSRSITELinkingMaster.PSRCode = ax.ACXPSRMaster.PSR_Code where ax.ACXPSRSITELinkingMaster.Site_Code = '" + Session["SiteCode"].ToString() + "'";
                    //}
                    //
                    //DataTable dt1 = new DataTable();
                    //dt1 = baseObj.GetData(sqlstr1);
                    //lstPSRNAME.Items.Clear();
                    //
                    //for (int i = 0; i < dt1.Rows.Count; i++)
                    //{
                    //    lstPSRNAME.DataSource = dt1;
                    //    lstPSRNAME.DataTextField = "PSR_Name";
                    //    lstPSRNAME.DataValueField = "PSR_Code";
                    //    lstPSRNAME.DataBind();
                    //}
                    //if (lstPSRNAME.Items.Count == 1)
                    //{
                    //    CheckBox9.Visible = false;
                    //    lstPSRNAME.Items[0].Selected = true;
                    //}
                }
                //else
                //{
                //    lstPSRNAME.Items.Clear();
                //}
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
     
        private void LoadDataInReportViewer(DataTable dtSetHeader, DataTable dtSetData, DataTable dtTotalInvoiceNo)
        {
            try
            {
                LblMessage.Text = String.Empty;
                if (dtSetHeader.Rows.Count >= 0 && dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FromDate");
                    DataSetParameter.Columns.Add("ToDate");
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");

                    //DataSetParameter.Columns.Add("Distributor");
                    // DataSetParameter.Columns.Add("Customet_Group");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = "";

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\DailySaleTracking.rdl");
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSetParameter", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    // ReportDataSource RDS3 = new ReportDataSource("TotalDistinctSumGroupbyInvoiceNo", dtTotalInvoiceNo);
                    //ReportViewer1.LocalReport.DataSources.Add(RDS3);
                    ReportViewer1.ShowPrintButton = true;
                    this.ReportViewer1.LocalReport.Refresh();
                    //ReportViewer1.LocalReport.Refresh();
                    //ReportViewer1.ShowPrintButton = true;
                    ReportViewer1.Visible = true;
                    ReportViewer1.ZoomPercent = 100;
                    return;

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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (Panel1.Visible == true)
            {
                Panel1.Visible = false;
                LinkButton1.Text = "Show sales person filter";

            }

            else if (Panel1.Visible == false)
            {
                Panel1.Visible = true;
                LinkButton1.Text = "hide sales person filter";
            }
        }

        protected void lstSTATE_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }
        
        //protected void lstPSRNAME_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillPSR();
        //}

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
                LblMessage.Text = ex.Message.ToString();
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
                lstSTATE_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
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
                lstSTATE_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
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
                lstSTATE_SelectedIndexChanged(null, null);
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
                lstSTATE_SelectedIndexChanged(null, null);
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
                lstSTATE_SelectedIndexChanged(null, null);
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
                lstSTATE_SelectedIndexChanged(null, null);
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
                lstSTATE_SelectedIndexChanged(null, null);
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
                lstSTATE_SelectedIndexChanged(null, null);
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
            lstSTATE_SelectedIndexChanged(null, null);
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
            lstSTATE_SelectedIndexChanged(null, null);
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

            lstSTATE_SelectedIndexChanged(null, null);
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

            lstSTATE_SelectedIndexChanged(null, null);
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
            lstSTATE_SelectedIndexChanged(null, null);
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
            lstSTATE_SelectedIndexChanged(null, null);
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
            lstSTATE_SelectedIndexChanged(null, null);
        }

        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox7, chkListEXECUTIVE);
            lstSTATE_SelectedIndexChanged(null, null);
            // chkListASM.DataSource = null;
        }
        protected void CheckBox10_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox10, lstSTATE);
            // chkListASM.DataSource = null;
        }

        protected void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            //CheckAll_CheckedChanged(CheckBox8, lstDIS);
            // chkListASM.DataSource = null;
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox8, lstDIS);
                //     chkListASM.Items.Clear();
                lstDIS_SelectedIndexChanged(null, null);

            }
            else
            {
                CheckAll_CheckedChanged(CheckBox8, lstDIS);

                //chkListASM.Items.Clear();

            }
        }

        protected void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            //CheckAll_CheckedChanged(CheckBox9, lstPSRNAME);
            //FillPSR();
            // chkListASM.DataSource = null;
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox9, lstPSRNAME);
                //     chkListASM.Items.Clear();
                //chkListSite_SelectedIndexChanged(null, null);

            }
            else
            {
                CheckAll_CheckedChanged(CheckBox9, lstPSRNAME);

                //chkListASM.Items.Clear();

            }
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