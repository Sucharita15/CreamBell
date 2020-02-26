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
    public partial class frmPurchaseRegisterReport : System.Web.UI.Page
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
                FillState();
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
               // ddlCountry_SelectedIndexChanged(null, null);
            }
            
            
            //string sqlstr11 = "Select Distinct StateCode Code,StateCode Name from [ax].[INVENTSITE] where STATECODE <>'' ";
            //ddlState.Items.Add("Select...");
            //baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            
            
        }
        protected void FillState()
        {
            //DataTable dt = new DataTable();

            //dt = new DataTable();
            //if (Convert.ToString(Session["ISDISTRIBUTOR"]) != "Y")
            //{
            //    ddlState.Items.Clear();
            //    string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
            //   // string sqlstr11 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE ";
            //    ddlState.Items.Add("Select...");
            //    baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");                               
            //}
            //else
            //{
            //    ddlState.Items.Clear();
            //    ddlSiteId.Items.Clear();
            //    string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
            //    baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
            //    baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
            //}
            string sqlstr = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                    ddlState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "--Select--";

                    dtState.Rows.InsertAt(dr, 0);
                    ddlState.DataSource = dtState;
                    ddlState.DataTextField = "STATENAME";
                    ddlState.DataValueField = "STATE";
                    ddlState.DataBind();
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    //ddlState.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                }
            }
            else
            {
                sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
            }
            if (ddlState.Items.Count == 1)
            {
                ddlState.SelectedIndex = 1;
                ddlCountry_SelectedIndexChanged(null, null);
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

        private void ShowReportSummary()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
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


                if (ddlSiteId.SelectedIndex != -1)
                {
                    if (ddlSiteId.SelectedItem.Text != "Select...")
                    {
                        sqlstr += "and CM.SITE_CODE = '" + ddlSiteId.SelectedItem.Value + "'  ";
                    }
                }

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
                    DataSetParameter.Columns.Add("StartDate");
                    DataSetParameter.Columns.Add("EndDate");
                    DataSetParameter.Columns.Add("Site_Code");
                    DataSetParameter.Columns.Add("StateCode");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");
                    DataSetParameter.Rows.Add();
                    DataSetParameter.Rows[0]["StartDate"] = txtFromDate.Text;
                    DataSetParameter.Rows[0]["EndDate"] = txtToDate.Text;

                    if (ddlState.SelectedItem.Text == "Select...")
                    {
                        DataSetParameter.Rows[0]["StateCode"] = "All";
                    }
                    else
                    {
                        DataSetParameter.Rows[0]["StateCode"] = ddlState.SelectedItem.Text;
                    }
                    //if (ddlSiteId.SelectedIndex != -1)
                    //{
                    //    if (ddlSiteId.SelectedItem.Text == "Select...")
                    //    {
                    //        DataSetParameter.Rows[0]["Distributor"] = "All";
                    //    }
                    //    else
                    //    {
                    //        DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Text;
                    //    }
                    if (ddlSiteId.SelectedIndex < 0)
                    {
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            string Site_Code = "";
                            DataSetParameter = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            foreach (DataRow row in DataSetParameter.Rows)
                            {
                                if (Site_Code == "")
                                    Site_Code = Convert.ToString(row["DISTRIBUTOR"]);
                                else
                                    Site_Code += "," + row["DISTRIBUTOR"];
                            }
                        }
                        else
                            DataSetParameter.Rows[0]["Distributor"] = "";
                    }
                    else if (ddlSiteId.SelectedIndex > 0)
                    {
                        DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Value;
                    }
                    else if (ddlSiteId.SelectedItem.Text != "All...")
                    {
                        DataSetParameter.Rows[0]["Distributor"] = ddlSiteId.SelectedItem.Value;
                    }
                    else
                    {
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            string Site_Code = "";
                            DataSetParameter = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            foreach (DataRow row in DataSetParameter.Rows)
                            {
                                if (Site_Code == "")
                                    Site_Code = Convert.ToString(row["DISTRIBUTOR"]);
                                else
                                    Site_Code += "," + row["DISTRIBUTOR"];
                            }
                            DataSetParameter.Rows[0]["Distributor"] = Site_Code;
                        }
                        else
                        {
                            DataSetParameter.Rows[0]["Distributor"] = ""; 
                        }

                    }

                    ReportDataSource RDS1 = new ReportDataSource("DSetPurchaseRegister", dtSetData);
                    ReportViewer1.AsyncRendering = true;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSetHeader", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Add(RDS2);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\PurchaseRegister.rdl");
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

        //protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
        //    ddlSiteId.Items.Clear();
        //    ddlSiteId.Items.Add("Select...");
        //    baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
        //}

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "'";
                ddlSiteId.Items.Add("All...");
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

                    ddlSiteId.DataSource = uniqueCols;
                    ddlSiteId.DataTextField = "DistributorName";
                    ddlSiteId.DataValueField = "Distributor";
                    ddlSiteId.DataBind();
                    //ddlSiteId.Items.Add("All...");
                    ddlSiteId.Items.Insert(0, "All...");
                }
                else
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    //ddlSiteId.Items.Add("All...");

                    baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                }
            }
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            DataTable dt = new DataTable();
            bool b = ValidateInput();
            if (b == true)
            {
                try
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;

                    string query = "ACX_USP_PurchaseRegisterREPORT";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();
                    ilist.Add("@Site_Code");
                    //if (ddlSiteId.SelectedIndex != -1)
                    //{
                    //    if (ddlSiteId.SelectedItem.Text != "All...")
                    //    {
                    //        item.Add(ddlSiteId.SelectedItem.Value);
                    //    }
                    //    else
                    //    {
                    //        item.Add("0");
                    //    }
                    //}
                    //else
                    //{
                    //    item.Add("0");
                    //}

                    if (ddlSiteId.SelectedIndex < 0)
                    {
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            string Site_Code = "";
                            dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (Site_Code == "")
                                    Site_Code = Convert.ToString(row["DISTRIBUTOR"]);
                                else
                                    Site_Code += "," + row["DISTRIBUTOR"];
                            }
                        }
                        else
                            item.Add("");
                    }
                    else if (ddlSiteId.SelectedIndex > 0)
                    {
                        string Site_Code = ddlSiteId.SelectedItem.Value;
                        item.Add(Site_Code);
                    }
                    else if (ddlSiteId.SelectedItem.Text != "All...")
                    {
                        item.Add(ddlSiteId.SelectedItem.Value);
                    }
                    else
                    {
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            string Site_Code = "";
                            dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (Site_Code == "")
                                    Site_Code = Convert.ToString(row["DISTRIBUTOR"]);
                                else
                                    Site_Code += "," + row["DISTRIBUTOR"];
                            }
                            item.Add(Site_Code);
                        }
                        else
                        {
                            item.Add("");
                        }
                        }
                        //state
                        ilist.Add("@StateCode");
                        if (ddlState.SelectedIndex != -1)
                        {
                            if (ddlState.SelectedItem.Text != "--Select--")
                            {
                                item.Add(ddlState.SelectedItem.Value);
                            }
                            else
                            {
                                item.Add("0");
                            }
                        }



                        ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                        ilist.Add("@EndDate"); item.Add(txtToDate.Text);
                        ilist.Add("@UserType"); item.Add(Session["LOGINTYPE"].ToString());
                        ilist.Add("@UserCode"); item.Add(Session["USERID"].ToString());

                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                        if (dt.Rows.Count > 0)
                        {
                            LoadDataInReportViewerDetail(dt);
                        }
                    }
                
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportDataToExcel();       
        }

        protected void ExportDataToExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            DataTable dt = new DataTable();
            bool b = ValidateInput();
            if (b == true)
            {
                try
                {
                    string FromDate = txtFromDate.Text;
                    string ToDate = txtToDate.Text;

                    string query = "ACX_USP_PurchaseRegisterREPORT";
                    List<string> ilist = new List<string>();
                    List<string> item = new List<string>();

                    //if (ddlSiteId.SelectedIndex != -1)
                    //{
                    //    if (ddlSiteId.SelectedItem.Text != "All...")
                    //    {
                    //        item.Add(ddlSiteId.SelectedItem.Value);
                    //    }
                    //    else
                    //    {
                    //        item.Add("0");
                    //    }
                    //}
                    //else
                    //{
                    //    item.Add("0");
                    //}
                    //SITEID
                    ilist.Add("@Site_Code");
                    if (ddlSiteId.SelectedIndex < 0)
                    {
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            string Site_Code = "";
                            dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (Site_Code == "")
                                    Site_Code = Convert.ToString(row["DISTRIBUTOR"]);
                                else
                                    Site_Code += "," + row["DISTRIBUTOR"];
                            }
                        }
                        else
                            item.Add("");
                    }
                    else if (ddlSiteId.SelectedIndex > 0)
                    {
                        item.Add(ddlSiteId.SelectedItem.Value);
                    }
                    else if (ddlSiteId.SelectedItem.Text != "All...")
                    {
                        item.Add(ddlSiteId.SelectedItem.Value);
                    }
                    else
                    {
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            string Site_Code = "";
                            dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (Site_Code == "")
                                    Site_Code = Convert.ToString(row["DISTRIBUTOR"]);
                                else
                                    Site_Code += "," + row["DISTRIBUTOR"];
                            }
                            item.Add(Site_Code);
                        }
                        else
                        {
                            item.Add("");
                        }
                    }
                        //STATE
                        ilist.Add("@StateCode");
                        if (ddlState.SelectedIndex != -1)
                        {
                            if (ddlState.SelectedItem.Text != "--Select--")
                            {
                                item.Add(ddlState.SelectedItem.Value);
                            }
                            else
                            {
                                item.Add("0");
                            }
                        }

                        ilist.Add("@DATAAREAID"); item.Add(Session["DATAAREAID"].ToString());
                        ilist.Add("@StartDate"); item.Add(txtFromDate.Text);
                        ilist.Add("@EndDate"); item.Add(txtToDate.Text);
                        ilist.Add("@UserType"); item.Add(Session["LOGINTYPE"].ToString());
                        ilist.Add("@UserCode"); item.Add(Session["USERID"].ToString());
                        dt = obj.GetData_New(query, CommandType.StoredProcedure, ilist, item);

                        if (dt.Rows.Count > 0)
                        {
                            //=================Create Excel==========

                            string attachment = "attachment; filename=ConsolidatedPurchaseRegister.xls";
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

                            Response.Flush();
                            Response.SuppressContent = true;
                        }
                    }
                
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
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

            // fillSiteAndState(dtHOS);
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

                //fillSiteAndState(dt);
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

                //fillSiteAndState(dt);
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
                //fillSiteAndState(dt);
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
                //fillSiteAndState(dt);

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
                //fillSiteAndState(dt);
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

                //fillSiteAndState(dt);
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

                //fillSiteAndState(dt);
                uppanel.Update();
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            //fillSiteAndState(dt);
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