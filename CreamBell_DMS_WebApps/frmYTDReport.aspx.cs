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
    public partial class frmYTDReport : System.Web.UI.Page
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
                baseObj.FillSaleHierarchy();
                fillHOS();
                //FillHOS(); 
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    // DataView DtSaleHierarchy = (DataTable)HttpContext.Current.Session["SaleHierarchy"];
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    if (dt.Rows.Count>0)
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
                            CheckBox9.Enabled = false;
                            CheckBox9.Checked = true;

                            // chkAll_CheckedChanged(null, null);
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
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }       
        protected void FillState(DataTable dt)
        {
            //string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            //object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            
           // DataTable dt = new DataTable();
            //dt = new DataTable();
            //if (objcheckSitecode != null)
            if (Session["ISDISTRIBUTOR"].ToString() == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATEWNAME");
                    //dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                    chkListState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                     dr[0] = "--Select--";
                     dr[1] = "--Select--";
                    chkListState.DataSource = dtState;
                    chkListState.DataTextField = "STATEWNAME";
                    chkListState.DataValueField = "STATE";
                    chkListState.DataBind();
                }
                else
                {
                    string sqlstr = "";
                    DataTable dt2 = new DataTable();
                    //sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    //drpSTATE.Items.Add("Select...");
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = baseObj.GetConnection();
                    cmd1.CommandText = sqlstr;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                    sda.Fill(dt2);
                   
                    chkListState.DataSource = dt2;
                    chkListState.DataTextField = "NAME";
                    chkListState.DataValueField = "Code";
                    chkListState.DataBind();
          
                }
            }
            else
            {
                chkListState.Items.Clear();
                chkListSite.Items.Clear();
                DataTable dt1 = new DataTable();
               string sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                //string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                chkListState.Items.Add("Select...");
                //only name and code have to be insertd in a new datatable according to this sqlstr
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = baseObj.GetConnection();
                cmd1.CommandText = sqlstr;
                SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                sda.Fill(dt1);
                chkListState.DataSource = dt1;
                chkListState.DataTextField = "name";
                chkListState.DataValueField = "code";
                chkListState.DataBind();

            }
            //else
            //{
            //    chkListState.Items.Clear();
            //    chkListSite.Items.Clear();
            //    string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName,I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
            //    dt = baseObj.GetData(sqlstr1);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        chkListState.DataSource = dt;
            //        chkListState.DataTextField = "StateName";
            //        chkListState.DataValueField = "StateCode";
            //        chkListState.DataBind();

            //        chkListSite.DataSource = dt;
            //        chkListSite.DataTextField = "SiteName";
            //        chkListSite.DataValueField = "SiteId";
            //        chkListSite.DataBind();
            //    }
            //  //  chkListState.Items[0].Selected = true;
            //  //  chkListSite.Items[0].Selected = true;
            //}
            if (chkListState.Items.Count == 1)
            {
                chkListState.Items[0].Selected = true;
                ddlCountry_SelectedIndexChanged(null, null);
            }
        }
        //protected void FillSite()
        //{
        //    string StateList = "";
        //    foreach (ListItem item in chkListState.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            if (StateList == "")
        //            {
        //                StateList += "'" + item.Value.ToString() + "'";
        //            }
        //            else
        //            {
        //                StateList += ",'" + item.Value.ToString() + "'";
        //            }
        //        }
        //    }
        //    if (StateList.Length > 0)
        //    {
        //        DataTable dt = new DataTable();
        //        string sqlstr1 = string.Empty;
        //        string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //        object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //        if (objcheckSitecode != null)
        //        {
        //            sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
        //        }
        //        else
        //        {
        //            sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
        //        }

        //        dt = new DataTable();                
        //        chkListSite.Items.Clear();
        //        dt = baseObj.GetData(sqlstr1);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            chkListSite.DataSource = dt;
        //            chkListSite.DataTextField = "SiteName";
        //            chkListSite.DataValueField = "SiteId";
        //            chkListSite.DataBind();
        //        }
        //        if (chkListSite.Items.Count == 1)
        //        {
        //            chkListSite.Items[0].Selected = true;     
        //        }
        //    }
        //    else
        //    {
        //        chkListSite.Items.Clear();
        //    }
        //}

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
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    chkListSite.Items.Clear();
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+Name Name from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") Order by Name";
                    chkListSite.Items.Add("All...");
                    DataTable dt1 = new DataTable();
                    dt1 = baseObj.GetData(sqlstr1);
                    chkListSite.DataSource = dt1;
                    chkListSite.DataTextField = "Name";
                    chkListSite.DataValueField = "Code";
                    chkListSite.DataBind();
                }
                else
                {
                    chkListSite.Items.Clear();
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                        DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");

                        uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                        chkListSite.DataSource = uniqueCols;
                        chkListSite.DataTextField = "Name";
                        chkListSite.DataValueField = "distributor";
                        chkListSite.DataBind();

                    }
                    else
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                        //drpDIS.Items.Add("All...");
                        DataTable dt1 = new DataTable();
                        dt1 = baseObj.GetData(sqlstr1);
                        chkListSite.DataSource = dt1;
                        chkListSite.DataTextField = "Name";
                        chkListSite.DataValueField = "Code";
                        chkListSite.DataBind();
                    }
                    if (chkListSite.Items.Count == 1)
                    {
                        CheckBox8.Visible = false;
                        chkListSite.Items[0].Selected = true;
                       // lstDIS_SelectedIndexChanged(null, null);
                    }
                }
            }
        }
        private bool ValidateInput()
        {
            bool b;
            //if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            //{
            //    b = false;
            //    LblMessage.Text = "Please Provide From Date and To Date";
            //}
            //else
            //{
            //    b = true;
            //    LblMessage.Text = string.Empty;
            //}

            ////if (ddlSiteId.SelectedIndex == -1)
            //{
                b = true;
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
            try
            {
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                SqlCommand cmd = new SqlCommand();
                DataTable dtDataByfilter = null;
                string query = string.Empty;
                // conn.Open();
                // cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                // string qr = "";
                query = "ACX_USP_MONTHCLOSINGSALEREPORT";
                cmd.CommandText = query;
                //add the parameter..
                cmd.Parameters.AddWithValue("@AsOnDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));

                string SiteList = "";
                foreach (ListItem item in chkListSite.Items)
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
                //add siteid param....
                if (SiteList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Distributor", SiteList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Distributor", "");
                }

                dtDataByfilter = new DataTable();
                cmd.Connection = obj.GetConnection();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                // dtDataByfilter.Load(cmd.ExecuteNonQuery);
                LoadDataInReportViewer(SiteList, dtDataByfilter);

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
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }
        private void LoadDataInReportViewer(string Siteid, DataTable dtSetData) 
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    int count = dtSetData.Rows.Count;
                    for (int i = 0; i < count; i += 2)
                    {
                        string DistName = dtSetData.Rows[i]["PSRCode"].ToString();
                        string SiteID = dtSetData.Rows[i]["SiteID"].ToString();
                        string PSRNAME = dtSetData.Rows[i]["PSRNAME"].ToString();
                        string SiteName = dtSetData.Rows[i]["SiteName"].ToString();

                        decimal DayLtrYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["DayLtr"]);
                        decimal DayLtrCur = Convert.ToDecimal(dtSetData.Rows[i+1]["DayLtr"]);
                        decimal MonthLtrYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["MonthLtr"]);
                        decimal MonthLtrCur = Convert.ToDecimal(dtSetData.Rows[i+1]["MonthLtr"]);
                        decimal YearLtrYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["YearLtr"]);
                        decimal YearLtrCur = Convert.ToDecimal(dtSetData.Rows[i+1]["YearLtr"]);

                        decimal DayValueYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["DayValue"]);
                        decimal DayValueCur = Convert.ToDecimal(dtSetData.Rows[i+1]["DayValue"]);
                        decimal MonthValueYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["MonthValue"]);
                        decimal MonthValueCur = Convert.ToDecimal(dtSetData.Rows[i+1]["MonthValue"]);
                        decimal YearValueYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["YearValue"]);
                        decimal YearValueCur = Convert.ToDecimal(dtSetData.Rows[i+1]["YearValue"]);

                        decimal DayRelizationYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["DayRelization"]);
                        decimal DayRelizationCur = Convert.ToDecimal(dtSetData.Rows[i+1]["DayRelization"]);
                        decimal MonthRelizationYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["MonthRelization"]);
                        decimal MonthRelizationCur = Convert.ToDecimal(dtSetData.Rows[i+1]["MonthRelization"]);
                        decimal YearRelizationYearAgo = Convert.ToDecimal(dtSetData.Rows[i]["YearRelization"]);
                        decimal YearRelizationCur = Convert.ToDecimal(dtSetData.Rows[i + 1]["YearRelization"]);

                        decimal GrowthDay              = 0 ;
                        decimal GrowthMonth            = 0 ;
                        decimal GrowthYear             = 0 ;
                        decimal GrowthDayValue         = 0 ;
                        decimal GrowthMonthVAlue       = 0 ;
                        decimal GrowthYearValue        = 0 ;
                        decimal GrowthDayRelization    = 0 ;
                        decimal GrowthMonthRelization  = 0 ;
                        decimal GrowthYearRelization   = 0 ;

                        if(DayLtrYearAgo != 0)
                        {
                               GrowthDay = (DayLtrCur / DayLtrYearAgo )*100 ;
                        }
                        if(MonthLtrYearAgo != 0)
                        {
                              GrowthMonth = (MonthLtrCur / MonthLtrYearAgo ) * 100 ;
                        }
                        if(YearLtrYearAgo != 0)
                        {
                              GrowthYear = (YearLtrCur / YearLtrYearAgo) * 100 ;
                        }
                        if (DayValueYearAgo != 0)
                        {
                            GrowthDayValue = (DayValueCur / DayValueYearAgo) * 100;
                        }
                        if (MonthValueYearAgo != 0)
                        {
                            GrowthMonthVAlue = (MonthValueCur / MonthValueYearAgo) * 100;
                        }
                        if (YearValueYearAgo != 0)
                        {
                            GrowthYearValue = (YearValueCur / YearValueYearAgo) * 100;
                        }

                        if (DayRelizationYearAgo != 0)
                        {
                            GrowthDayRelization = (DayRelizationCur - DayRelizationYearAgo ) ;
                        }
                        if (MonthRelizationYearAgo != 0)
                        {
                            GrowthMonthRelization = (MonthRelizationCur - MonthRelizationYearAgo )  ;
                        }
                        if (YearRelizationYearAgo != 0)
                        {
                            GrowthYearRelization = (YearRelizationCur - YearRelizationYearAgo);
                        }

                        dtSetData.Rows.Add(dtSetData.Rows.Count);

                        dtSetData.Rows[dtSetData.Rows.Count-1]["PSRCode"] = DistName;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["SiteID"] = SiteID;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["PSRNAME"] = PSRNAME;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["SiteName"] = SiteName;

                        dtSetData.Rows[dtSetData.Rows.Count-1]["DayLtr"] = GrowthDay;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["MonthLtr"] = GrowthMonth;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["YearLtr"] = GrowthYear;

                        dtSetData.Rows[dtSetData.Rows.Count-1]["DayValue"] = GrowthDayValue;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["MonthValue"] = GrowthMonthVAlue;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["YearValue"] = GrowthYearValue;

                        dtSetData.Rows[dtSetData.Rows.Count-1]["DayRelization"] = GrowthDayRelization;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["MonthRelization"] = GrowthMonthRelization;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["YearRelization"] = GrowthYearRelization;
                        dtSetData.Rows[dtSetData.Rows.Count-1]["Year"] = 3000;  //as Growth



                    }



                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FromDate");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["Distributor"] = 

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\YTDReport.rdl");
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DSetHeader", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
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
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "[ax].[ACX_PartyWiseSaleSummaryDiscount]";

                cmd.CommandText = query;
                //DateTime now = Convert.ToDateTime(txtToDate.Text);
                //now = now.AddMonths(1);

                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
               // cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));
                string SiteList = "";
                foreach (ListItem item in chkListSite.Items)
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
                //foreach (ListItem item in chkListCustomerGroup.Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (CustGroupList == "")
                //        {
                //            CustGroupList += "" + item.Value.ToString() + "";
                //        }
                //        else
                //        {
                //            CustGroupList += "," + item.Value.ToString() + "";
                //        }
                //    }
                //}
                if (CustGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", CustGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CUSTGROUP", "");
                }

                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
                DataTable dt = new DataTable();
                dt = dtDataByfilter;

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

            FillState(dtHOS);
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

                FillState(dt);
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

                FillState(dt);
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
                FillState(dt);
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
                FillState(dt);

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
                FillState(dt);
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

                FillState(dt);

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

                FillState(dt);

                uppanel.Update();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            FillState(dt);

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

        protected void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox9, chkListState);
            FillSite();
            // chkListASM.DataSource = null;
        }

        protected void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox8, chkListSite);
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