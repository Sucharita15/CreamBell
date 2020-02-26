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
    public partial class frmReachReport : System.Web.UI.Page
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
                baseObj.FillSaleHierarchy();
                fillHOS();
               // fillSiteAndState();
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
                            CheckBox6.Enabled = false;
                            CheckBox6.Checked = true;

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

                uppanel.Update();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }
        //protected void FillSiteAndState()
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlState.Items.Clear();
        //        ddlState.Items.Add("All...");
        //        string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";              
        //        baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlState.Items.Clear();
        //        chkListSite.Items.Clear();
        //        string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
        //        baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
        //        DataTable dt = new DataTable();
        //        dt = baseObj.GetData(sqlstr1);
                
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            chkListSite.DataSource = dt;
        //            chkListSite.DataTextField = "SiteName";
        //            chkListSite.DataValueField = "SiteId";
        //            chkListSite.DataBind();
        //        }
        //        //chkListSite.Items[0].Selected = true;
        //    }
        //}        

        protected void fillSiteAndState(DataTable dt)
        {
            string sqlstr = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATEWNAME");
                    //dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");

                    ddlState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "--Select--";

                    dtState.Rows.InsertAt(dr, 0);
                    ddlState.DataSource = dtState;
                    ddlState.DataTextField = "STATEWNAME";
                    ddlState.DataValueField = "STATE";
                    ddlState.DataBind();
                }
                else
                {   
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");   
                }
            }
            else
            {
                sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
            }
            if (ddlState.Items.Count == 1)
            {
                ddlState.SelectedIndex = 1;
                ddlCountry_SelectedIndexChanged(null, null);
            }    
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validate();
                if (b == true)
                {
                    ShowReportByFilter();
                    uppanel.Update();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }            
        }
//        protected void FillSite()
//        {
//                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
////                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
//                DataTable dt = new DataTable();
//                dt = baseObj.GetData(sqlstr1);
//                chkListSite.Items.Clear();
                
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {
//                    chkListSite.DataSource = dt;
//                    chkListSite.DataTextField = "Name";
//                    chkListSite.DataValueField = "Code";
//                    chkListSite.DataBind();
//                }         
//        }
        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillSite();
        //    uppanel.Update();
        //}
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
               // ddlSiteId.Items.Add("All...");
                ddlSiteId.Items.Insert(0, "All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //DataTable dt = (DataTable)Session["SaleHierarchy"];
                    // DataTable dt = HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM);
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                    uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                    ddlSiteId.DataSource = uniqueCols;
                    ddlSiteId.DataTextField = "Name";
                    ddlSiteId.DataValueField = "distributor";
                    ddlSiteId.DataBind();
                    //ddlSiteId.Items.Add("All...");
                    ddlSiteId.Items.Insert(0, "All...");
                }
                else
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    //ddlSiteId.Items.Add("All...");

                    baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                }
            }
        }
        private bool Validate()
        {
            bool b;
            if (txtCurrentDate.Text == string.Empty )
            {
                b = false;             
            }
            else
            {
                b = true;                
            }      
            if (ddlState.SelectedItem.Text=="All...")
            {
                b = false;
            }
            return b;
        }
        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(objGlobal.GetConnectionString());
               // conn.Open();
                cmd = new SqlCommand();
                //cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "[dbo].[ACX_USP_REACHREPORT]";

                DateTime stDate = Convert.ToDateTime(txtCurrentDate.Text);
                DateTime firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
               
                cmd.CommandText = query;
             
              
                //site
                string SiteList = "";
                //foreach (ListItem item in ddlSiteId.Items)
                //{
                //    if (item.Selected)
                //    {
                //        if (SiteList == "")
                //        {
                //            SiteList += "" + item.Value.ToString() + "";
                //        }
                //        else
                //        {
                //            SiteList += "," + item.Value.ToString() + "";
                //        }
                //    }
                //}
                //ddlSiteId.SelectedIndex
                //if (ddlSiteId.SelectedIndex > 0)
                //{
                //    cmd.Parameters.AddWithValue("@SITEID", ddlSiteId.SelectedValue.ToString());
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@SITEID", "");
                //}
                if (ddlSiteId.SelectedIndex < 0)
                {
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        string siteid = "";
                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (siteid == "")
                                siteid = Convert.ToString(row["DISTRIBUTOR"]);
                            else
                                siteid += "," + row["DISTRIBUTOR"];
                        }
                    }
                    else
                        cmd.Parameters.AddWithValue("@SiteId", "");

                }
                else if (ddlSiteId.SelectedIndex > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else if (ddlSiteId.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else
                {
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        string siteid = "";
                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                        foreach (DataRow row in dt.Rows)
                        {
                            if (siteid == "")
                                siteid = Convert.ToString(row["DISTRIBUTOR"]);
                            else
                                siteid += "," + row["DISTRIBUTOR"];
                        }


                        cmd.Parameters.AddWithValue("@SiteId", siteid);
                    }
                    else
                        cmd.Parameters.AddWithValue("@SiteId", "");

                }
                cmd.Parameters.AddWithValue("@MONTHYEAR", Convert.ToDateTime(firstDayOfMonth));
                cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));
             
                dtDataByfilter = new DataTable();
                cmd.Connection = objGlobal.GetConnection();
                dtDataByfilter.Load(cmd.ExecuteReader());
                LoadDataInReportViewer(dtDataByfilter);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                objGlobal.CloseSqlConnection();
                //if (conn != null)
                //{
                //    if (conn.State.ToString() == "Open") { conn.Close(); }
                //}
            }
        }
        private void LoadDataInReportViewer(DataTable dtSetData)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("MonthDate");                    
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");
                    //cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                    //cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));
              
                    DataSetParameter.Rows.Add();

                    DataSetParameter.Rows[0]["MonthDate"] = txtCurrentDate.Text;                                        
                    DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;
                    //site
                    string SiteList = "";
                    foreach (ListItem item in ddlSiteId.Items)
                    {
                        if (item.Selected)
                        {
                            if (SiteList == "")
                            {
                                SiteList += "" + item.Text.ToString() + "";
                            }
                            else
                            {
                                SiteList += "," + item.Text.ToString() + "";
                            }
                        }
                    }
                    if (SiteList.Length > 0)
                    {
                        DataSetParameter.Rows[0]["Distributor"] = SiteList;
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["Distributor"] = "All";
                    }        
                   
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\ReachReport.rdl");
                    // ReportParameter MonthDate = new ReportParameter();
                    //   MonthDate.Name = "CurrentDate";
                    //  MonthDate.Values.Add(txtCurrentDate.Text);
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet1", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);
                    this.ReportViewer1.LocalReport.Refresh();
                    ReportViewer1.Visible = true;
                  
                }
                else
                {                   
                    ReportViewer1.Visible = false;
                }
            }
            catch (Exception ex)
            {
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
            // chkListASM.DataSource = null;
            ddlCountry_SelectedIndexChanged(null, null);
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