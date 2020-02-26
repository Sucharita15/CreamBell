using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmRetailerStatusreport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlConnection conn = null;
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                // fillPSR();
                fillstatus();
                txtFromDate.Text = string.Format("{0:dd-MMM-yyyy}", DateTime.Today.AddDays(-1));
                txtToDate.Text = string.Format("{0:dd-MMM-yyyy}", DateTime.Today);

                // fillCategory();
                //fillCustomeGroup();
                baseObj.FillSaleHierarchy();
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

                            // chkAll_CheckedChanged(null, null);

                        }
                    }
                }

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    tclabel.Width = "90%";
                    Panel1.Visible = true;

                }
                else
                {

                    tclabel.Width = "100%";
                    Panel1.Visible = false;

                }
                //ddlCountry_SelectedIndexChanged(null, null);
            }
            //fillVP();
        }
        protected void fillPSR()
        {
            string siteid = "";
            foreach (System.Web.UI.WebControls.ListItem litem in ddlSiteId.Items)
            {
                if (litem.Selected)
                {
                    if (siteid.Length == 0)
                        siteid = "'" + litem.Value.ToString() + "'";
                    else
                        siteid += ",'" + litem.Value.ToString() + "'";
                }
            }
            if (ddlSiteId.SelectedValue != string.Empty)
            {
                DataTable dt = new DataTable();
                string query = @"Select PSR_Code +'-'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
                             " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
                             " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
                             " where B.Site_code in (" + siteid + "))";
                dt = new DataTable();
                drpPSR.Items.Clear();
                dt = baseObj.GetData(query);
                //drpPSR.Items.Add("Select...");
                drpPSR.DataSource = dt;
                drpPSR.DataTextField = "PSRName";
                drpPSR.DataValueField = "PSR_Code";
                drpPSR.DataBind();
                //drpPSR.Items.Clear();
                //
                //baseobj.BindToDropDown(drpPSR, query, "PSRName", "PSR_Code");


            }
        }

        private bool ValidateInput()
        {
            bool value = false;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Provide From Date and To Date";    
            }
            if (txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
            {
                value = true;
            }
            
            if (ddlSiteId.SelectedValue == string.Empty)
            {
                value = false;
                LblMessage.Text = "Please Select The SiteID  !";
                LblMessage.Visible = true;

            }
            

            return value;
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            GridView2.DataSource = null;
            LblMessage.Text = "";
            try
            {
                bool b = ValidateInput();
                GridView2.DataSource = null;
                if (b==true)
                {
                    ReportShow();
     //               ShowReportByFilter();
                    uppanel.Update();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void ReportShow()
        {
            string frmDate = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
            string endDate = Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd");

           
            string psrcode = "";
            var psrlist = drpPSR.Items.Cast<ListItem>().Where(it => it.Selected == true).Select(it => it.Value);
            if (psrlist.Count() > 0) {
                psrcode = string.Join(",", psrlist);
            }
            
            //foreach (System.Web.UI.WebControls.ListItem litem in drpPSR.Items)
            //{
            //    if (litem.Selected)
            //    {
            //        if (psrcode.Length == 0)
            //            psrcode = "" + litem.Value.ToString() + "";
            //        else
            //            psrcode += "," + litem.Value.ToString() + "";
            //    }
            //}

            string beatcode = "";
            var beatcodelist = drpBEAT.Items.Cast<ListItem>().Where(it => it.Selected == true).Select(it => it.Value);
            
            if (beatcodelist.Count() > 0) {
                beatcode = string.Join(",", beatcodelist);
            }
            //foreach (System.Web.UI.WebControls.ListItem litem in drpBEAT.Items)
            //{
            //    if (litem.Selected)
            //    {
            //        if (beatcode.Length == 0)
            //            beatcode = "" + litem.Value.ToString() + "";
            //        else
            //            beatcode += "," + litem.Value.ToString() + "";
            //    }
            //}

            string siteid = "";
            var siteidlist = ddlSiteId.Items.Cast<ListItem>().Where(it => it.Selected == true).Select(it => it.Value);
            if (siteidlist.Count() > 0) {
                siteid = string.Join(",", siteidlist);
            }
            //foreach (System.Web.UI.WebControls.ListItem litem in ddlSiteId.Items)
            //{
            //    if (litem.Selected)
            //    {
            //        if (siteid.Length == 0)
            //            siteid = "" + litem.Value.ToString() + "";
            //        else
            //            siteid += "," + litem.Value.ToString() + "";
            //    }
            //}

            
            string rtlstate = "";
            var rtlstatelist = drpStatus.Items.Cast<ListItem>().Where(it => it.Selected == true).Select(it => it.Value);
            if (rtlstatelist.Count() > 0) {
                rtlstate= string.Join(",", rtlstatelist);
            }
            //foreach (System.Web.UI.WebControls.ListItem litem in drpStatus.Items)
            //{
            //    if (litem.Selected)
            //    {
            //        if (rtlstate.Length == 0)
            //            rtlstate = "" + litem.Value.ToString() + "";
            //        else
            //            rtlstate += "," + litem.Value.ToString() + "";
            //    }
            //}


            //string query = " SELECT SiteCode[Distributor Code],D.Name[Distributor Name],E.ROLEDESCRIPTION[Distributor Type],h.PSR_Code[PSR Code],h.PSR_Name[PSR Name],g.BeatCode[Beat Code],g.BeatName[Beat Name],c.CUSTOMER_CODE[Customer Code],replace(C.CUSTOMER_NAME, '''', '') [CUSTOMER NAME],CONVERT(NVARCHAR(11), a.StatusDate, 106)[Status Date],B.ShopStatus_Name[Shop Status]" +
            //                " FROM AX.ACXShopStatusEntry A" +
            //               " LEFT JOIN AX.ACXShopStatus B ON A.StatusCode = B.ShopStatus_Code" +
            //              "  LEFT JOIN AX.ACXCUSTMASTER C ON A.CustomerCode = C.CUSTOMER_CODE" +
            //               " LEFT JOIN AX.InventSite D ON A.SiteCode = D.SiteID" +
            //               " left join ax.acxusermaster E on A.SiteCode=E.Site_Code"+
            //               " left join ax.acxpsrbeatmaster g on PSR_CODE =g.PSRCode"+
            //               " left join ax.acxpsrmaster h on g.PSRCode =h.PSR_Code"+
            //               " and C.PSR_BEAT = g.BeatCode"+
            //               " WHERE A.StatusDate >= '" + frmDate + "' AND A.StatusDate <= '" + endDate + "' ";


            conn = baseObj.GetConnection();
            cmd = new SqlCommand("[dbo].[USP_StatusReport]");
            cmd.Connection = conn;
            cmd.CommandTimeout = 0;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@FDate", frmDate);
            cmd.Parameters.AddWithValue("@TDate",endDate);
            cmd.Parameters.AddWithValue("@SiteID", siteid);
            cmd.Parameters.AddWithValue("@psrcode", psrcode);
            cmd.Parameters.AddWithValue("@beatcode", beatcode);
            cmd.Parameters.AddWithValue("@statusshp", rtlstate);

            DataTable dtFilter = new DataTable();
            dtFilter.Load(cmd.ExecuteReader());
          
            if (dtFilter.Rows.Count > 0)
            {
                ViewState["temp"] = dtFilter;
                GridView2.DataSource = dtFilter;
                GridView2.DataBind();
                GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;

                foreach (GridViewRow grv in GridView2.Rows)
                {
                    CheckBox chkAll = (CheckBox)grv.Cells[0].FindControl("chklist");
                    chkAll.Checked = true;
                }
                uppanel.Update();
            }
             else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('No Data Exist !! ');", true);
                GridView2.DataSource = null;
                GridView2.DataBind();
               
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
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["ToDate"] = txtToDate.Text;
                    if (ddlState.SelectedItem.Text == "All...")
                    {
                        DataSetParameter.Rows[0]["StateCode"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;
                    }
                    if (ddlSiteId.SelectedIndex != -1)
                    {
                        if (ddlSiteId.SelectedItem.Text == "All...")
                        {
                            DataSetParameter.Rows[0]["Distributor"] = "All";
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Text;
                        }
                    }
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\DiscountPartyWise.rdl");
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DSetData", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet1", DataSetParameter);
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

        protected void fillSiteAndState(DataTable dt)

        {
            string sqlstr="";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                    ddlState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                   
                    ddlState.DataSource = dtState;
                    ddlState.DataTextField = "STATENAME";
                    ddlState.DataValueField = "STATE";
                    ddlState.DataBind();
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    //ddlState.Items.Add("Select...");
                    // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                    DataTable dt1 = baseObj.GetData(sqlstr);
                    ddlState.DataSource = dt1;
                    ddlState.DataTextField = "Name";
                    ddlState.DataValueField = "Code";
                    ddlState.DataBind();
                }
            }
            else
            {
                sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                ddlState.Items.Add("Select...");
               // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                DataTable dt1 = baseObj.GetData(sqlstr);
                ddlState.DataSource = dt1;
                ddlState.DataTextField = "Name";
                ddlState.DataValueField = "Code";
                ddlState.DataBind();
            }
            //if (ddlState.Items.Count == 1 || ddlState.SelectedItem.Text == "All...")
          //  {
            //    ddlState.SelectedIndex = 0;
            //    ddlCountry_SelectedIndexChanged(null, null);
            //}
        }

       
     
        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillCustomerGroup();
        }

        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //    if (objcheckSitecode != null)
        //    {
        //        ddlSiteId.Items.Clear();
        //        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
        //        ddlSiteId.Items.Add("All...");
        //        ddlSiteId.Items.Insert(0, "All...");
        //        baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //    }
        //    else
        //    {
        //        ddlSiteId.Items.Clear();
        //        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
        //        {
        //            DataTable dt = (DataTable)Session["SaleHierarchy"];
        //            DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "distributor Name");

        //            ddlSiteId.DataSource = uniqueCols;
        //            ddlSiteId.DataTextField = "distributor name";
        //            ddlSiteId.DataValueField = "distributor";
        //            ddlSiteId.DataBind();
        //            //ddlSiteId.Items.Add("All...");
        //            ddlSiteId.Items.Insert(0, "All...");
        //        }
        //        else
        //        {
        //            string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
        //            ddlSiteId.Items.Add("All...");

        //            baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //        }
        //    }                
             

        //}

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string statesel = "";
            foreach (System.Web.UI.WebControls.ListItem litem1 in ddlState.Items)
            {
                if (litem1.Selected)
                {
                    if (statesel.Length == 0)
                        statesel = "'" + litem1.Value.ToString() + "'";
                    else
                        statesel += ",'" + litem1.Value.ToString() + "'";
                }
            }
            if (ddlState.SelectedValue == string.Empty)
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

                //DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");

                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "Distributor", "DistributorName");
                ddlSiteId.DataSource = distinctValues;
                ddlSiteId.DataTextField = "DistributorName";
                ddlSiteId.DataValueField = "distributor";
                ddlSiteId.DataBind();
                //ddlSiteId.Items.Add("All...");
               

            }

            else
            {
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    ddlSiteId.Items.Clear();
                    string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE in (" + statesel + ")";
                    //ddlSiteId.Items.Add("All...");
                   // ddlSiteId.Items.Insert(0, "All...");
                    //baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                    DataTable dt = baseObj.GetData(sqlstr1);
                    ddlSiteId.DataSource = dt;
                    ddlSiteId.DataTextField = "Name";
                    ddlSiteId.DataValueField = "Code";
                    ddlSiteId.DataBind();
                }
                else
                {
                    ddlSiteId.Items.Clear();
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        //DataTable dt = (DataTable)Session["SaleHierarchy"];
                        // DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM);
                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                        dt.DefaultView.RowFilter = "STATE in (" + statesel + ")";
                        DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");

                        ddlSiteId.DataSource = uniqueCols;
                        ddlSiteId.DataTextField = "DistributorName";
                        ddlSiteId.DataValueField = "distributor";
                        ddlSiteId.DataBind();
                        //ddlSiteId.Items.Add("All...");
                       
                    }
                    else
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                        //ddlSiteId.Items.Add("All...");

                        //baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                        DataTable dt = baseObj.GetData(sqlstr1);
                        ddlSiteId.DataSource = dt;
                        ddlSiteId.DataTextField = "Name";
                        ddlSiteId.DataValueField = "Code";
                        ddlSiteId.DataBind();
                    }
                }
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
            ddlCountry_SelectedIndexChanged(null, null);
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

        protected void drpPSR_SelectedIndexChanged1(object sender, EventArgs e)
        {
            FillBeat();

        }

        protected void FillBeat()
        {
            string psrcode = "";
            foreach (System.Web.UI.WebControls.ListItem litem in drpPSR.Items)
            {
                if (litem.Selected)
                {
                    if (psrcode.Length == 0)
                        psrcode = "'" + litem.Value.ToString() + "'";
                    else
                        psrcode += ",'" + litem.Value.ToString() + "'";
                }
            }
            if (drpPSR.SelectedValue != string.Empty)
            {
                string strQuery = @"select BeatCode +'-'+BeatName as BeatName,BeatCode from [ax].[ACXPSRBeatMaster] where PSRCode in (" + psrcode + ")";
                drpBEAT.Items.Clear();
                // drpBeat.Items.Add("Select...");
                DataTable dt = baseObj.GetData(strQuery);

                drpBEAT.DataSource = dt;
                drpBEAT.DataTextField = "BeatName";
                drpBEAT.DataValueField = "BeatCode";
                drpBEAT.DataBind();
            }
            if (drpPSR.SelectedValue == string.Empty)
            {
                drpBEAT.Items.Clear();
              //  drpCustomer.Items.Clear();
            }
        }

        protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstatus();
        }

        protected void fillstatus()
        {
            
                string strQuery = @"select ShopStatus_Name as ShopStatus,ShopStatus_Code from [ax].[ACXShopStatus]";
                drpStatus.Items.Clear();
                // drpBeat.Items.Add("Select...");
                DataTable dt = baseObj.GetData(strQuery);

                drpStatus.DataSource = dt;
                drpStatus.DataTextField = "ShopStatus";
                drpStatus.DataValueField = "ShopStatus_Code";
                drpStatus.DataBind();
            
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView2.DataSource = null;
            GridView2.DataBind();
            fillPSR();

        }

        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            BtnShowReport_Click(null, null);
            try
            {
                DataTable dt = (DataTable)ViewState["temp"];
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                if (gv.Rows.Count > 0)
                {
                    string sFileName = "RetailerStatusReport.xls";

                    sFileName = sFileName.Replace("/", "");
                    // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                    Response.ContentType = "application/vnd.ms-excel";
                    EnableViewState = false;

                    System.IO.StringWriter objSW = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);


                    gv.RenderControl(objHTW);



                    Response.Write(objSW.ToString());

                    Response.End();

                }

            }

            catch (Exception ex)
            {
                //LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
    }
}
