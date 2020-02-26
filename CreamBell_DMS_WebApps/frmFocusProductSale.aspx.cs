﻿using System;
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
    public partial class frmFocusProductSale : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        DataTable dt = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
               
                baseObj.FillSaleHierarchy();
                fillHOS();
                // FillCustomerGroup();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {        
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
                ProductGroup();
                chkListState_SelectedIndexChanged(null, null);
            }
            uppanel.Update();

        }

        public void ProductGroup()
        {
			string strProductGroup = "Select Distinct a.Product_Group as Code,a.Product_Group as Name from ax.InventTable a where a.Product_Group <>'' order by a.Product_Group";
                    ChkItemCat.Items.Clear();
                    dt = baseObj.GetData(strProductGroup);
                    ChkItemCat.Items.Add("All...");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ChkItemCat.DataSource = dt;
                        ChkItemCat.DataTextField = "NAME";
                        ChkItemCat.DataValueField = "Code";
                        ChkItemCat.DataBind();
                    }
            //if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            //{
            //    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            //    {
            //
            //        string strProductGroup = "Select Distinct a.Product_Group as Code,a.Product_Group as Name from ax.InventTable a where a.Product_Group <>'' order by a.Product_Group";
            //        ChkItemCat.Items.Clear();
            //        dt = baseObj.GetData(strProductGroup);
            //        ChkItemCat.Items.Add("All...");
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            ChkItemCat.DataSource = dt;
            //            ChkItemCat.DataTextField = "NAME";
            //            ChkItemCat.DataValueField = "Code";
            //            ChkItemCat.DataBind();
            //        }
            //    }
            //}
        }

        protected void FillState(DataTable dt)
        {
            string sqlstr = "";
            //dt=new DataTable();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    // DataTable dt = new DataTable();

                    //string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";

                    //object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    //    

                    //     dt = new DataTable();
                    //DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                    //if (objcheckSitecode != null)
                    //{

                    //chkListState.Items.Clear();
                    //chkListSite.Items.Clear();
                    //ChkListPSR.Items.Clear();

                    //string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                    //dt = baseObj.GetData(sqlstr11);
                    //chkListState.Items.Add("All...");
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    chkListState.DataSource = dt;
                    //    chkListState.DataTextField = "NAME";
                    //    chkListState.DataValueField = "Code";
                    //    chkListState.DataBind();
                    //}
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATEWNAME");
                    //dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");

                    chkListState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "--Select--";

                    dtState.Rows.InsertAt(dr, 0);
                    chkListState.DataSource = dtState;
                    chkListState.DataTextField = "STATEWNAME";
                    chkListState.DataValueField = "STATE";
                    chkListState.DataBind();
                }
                else
                {
                    DataTable dt2 = new DataTable();
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    //lstSTATE.Items.Add("Select...");
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = baseObj.GetConnection();
                    cmd1.CommandText = sqlstr;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                    sda.Fill(dt2);
                    //dt2.Load(cmd1.ExecuteReader());
                    chkListState.DataSource = dt2;
                    chkListState.DataTextField = "Name";
                    chkListState.DataValueField = "Code";
                    chkListState.DataBind();
                }
               
            }
            else
            {
                //chkListState.Items.Clear();
                //chkListSite.Items.Clear();
                //ChkListPSR.Items.Clear();
                //string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                //dt = baseObj.GetData(sqlstr1);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    chkListState.DataSource = dt;
                //    chkListState.DataTextField = "StateName";
                //    chkListState.DataValueField = "StateCode";
                //    chkListState.DataBind();
                //
                //    chkListSite.DataSource = dt;
                //    chkListSite.DataTextField = "SiteName";
                //    chkListSite.DataValueField = "SiteId";
                //    chkListSite.DataBind();
                //}
                //chkListState.Items[0].Selected = true;
                //chkListSite.Items[0].Selected = true;
                sqlstr = "Select Distinct I.StateCode StateCode,I.StateCode+'-'+LS.Name as StateName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                chkListState.Items.Add("Select...");
                dt = baseObj.GetData(sqlstr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListState.DataSource = dt;
                    chkListState.DataTextField = "StateName";
                    chkListState.DataValueField = "StateCode";
                    chkListState.DataBind();
                }
                DataTable dt2 = new DataTable();
                string sqlstr2 = "Select Distinct I.SiteId,I.SiteId+'-'+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                chkListSite.Items.Add("Select...");
                dt2 = baseObj.GetData(sqlstr2);
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    chkListSite.DataSource = dt2;
                    chkListSite.DataTextField = "SiteName";
                    chkListSite.DataValueField = "SiteId";
                    chkListSite.DataBind();
                }
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
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                }
                else
                {
                    sqlstr1 = @"Select Distinct SITEID ,SITEID+'-'+NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
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
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                // string qr = "";
                query = "ax.ACX_FocusProductSaleReport";
                cmd.CommandText = query;
                //add the parameter..
               
                DateTime Todate = Convert.ToDateTime(txtFromDate.Text);
                var firstDayOfMonth = new DateTime(Todate.Year, Todate.Month, 1);
                DateTime FromDate = firstDayOfMonth;


                cmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(txtToDate.Text));
                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
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
                if (SiteList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", SiteList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }
                
                string StateList = "";
                foreach (ListItem item in chkListState.Items)
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
                if (StateList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", StateList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }

                string PSRList = "";
                foreach (ListItem item in ChkListPSR.Items)
                {
                    if (item.Selected)
                    {
                        if (PSRList == "")
                        {
                            PSRList += "" + item.Text.ToString() + "";
                        }
                        else
                        {
                            PSRList += "," + item.Text.ToString() + "";
                        }
                    }
                }
                if (PSRList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@PSR_Code", PSRList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PSR_Code", "");
                }

                string ProductList = "";
                foreach (ListItem item in ChkItem.Items)
                {
                    if (item.Selected)
                    {
                        if (ProductList == "")
                        {
                            ProductList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            ProductList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (ProductList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@PRODUCT_CODE", ProductList);
                    
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PRODUCT_CODE", "");
                    
                }

                string ProductGroupList = "";
                foreach (ListItem item in ChkItemCat.Items)
                {
                    if (item.Selected)
                    {
                        if (ProductGroupList == "")
                        {
                            ProductGroupList += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            ProductGroupList += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (ProductGroupList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@PRODUCT_GROUP", ProductGroupList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PRODUCT_GROUP", "");
                }
                string ProductSubCat = "";
                foreach (ListItem item in ChkItemSub.Items)
                {
                    if (item.Selected)
                    {
                        if (ProductSubCat == "")
                        {
                            ProductSubCat += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            ProductSubCat += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (ProductSubCat.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@PRODUCT_SUBCATEGORY", ProductSubCat);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PRODUCT_SUBCATEGORY", "");
                }
               
                //add siteid param....
              
                dtDataByfilter = new DataTable();
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
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                chkListSite.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + chkListState.SelectedItem.Value + "'";
                chkListSite.Items.Add("All...");
                chkListSite.Items.Insert(0, "All...");
               // baseObj.BindToDropDown(chkListSite, sqlstr1, "Name", "Code");
            }
            else
            {
                chkListSite.Items.Clear();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //DataTable dt = (DataTable)Session["SaleHierarchy"];
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                    uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                    chkListSite.DataSource = uniqueCols;
                    chkListSite.DataTextField = "Name";
                    chkListSite.DataValueField = "distributor";
                    chkListSite.DataBind();
                    //ddlSiteId.Items.Add("All...");
                    chkListSite.Items.Insert(0, "All...");
                }
                else
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    chkListSite.Items.Add("All...");
                    chkListSite.DataTextField = "DistributorName";
                    chkListSite.DataValueField = "distributor";
                    chkListSite.DataBind();
                 //   baseObj.BindToDropDown(chkListSite, sqlstr1, "Name", "Code");
                }
            }
        }

        private void LoadDataInReportViewer(string Siteid, DataTable dtSetData)
        {
            try
            {
                if (dtSetData.Rows.Count > 0)
                {
                    int count = dtSetData.Rows.Count;

                    DataTable DataSetParameter = new DataTable();
                    DataSetParameter.Columns.Add("FromDate");
                    DataSetParameter.Columns.Add("Distributor");
                    DataSetParameter.Columns.Add("UserType");
                    DataSetParameter.Columns.Add("UserCode");
                
                    DataSetParameter.Rows.Add();
                    //DataSetParameter.Rows[0]["FromDate"] = txtFromDate.Text;
                   // DataSetParameter.Rows[0]["Distributor"] =

                    dtSetData.Columns.Add("GroupValue");     //ProductCodeWise 0 and ProductSubCatWise
                    dtSetData.Columns.Add("MonthName");     //Month Name
                    if(rdoItemWise.Checked)
                    {
                    dtSetData.Rows[0]["GroupValue"] = "0" ;
                    }
                    else
                    {
                        dtSetData.Rows[0]["GroupValue"] = "1";
                    }
                    string str = Convert.ToDateTime(txtFromDate.Text).ToString("MMM");

                    dtSetData.Rows[0]["MonthName"] = str;

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\FocusProductSale.rdl");
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportViewer1.AsyncRendering = true;
                    ReportDataSource RDS1 = new ReportDataSource("DataSetParameter", DataSetParameter);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet1", dtSetData);
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
            int selectedCount = chkListState.Items.Cast<ListItem>().Count(li => li.Selected);
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                DataTable dt = new DataTable();
                dt = new DataTable();

                chkListSite.Items.Clear();
                if (selectedCount >= 1)
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name  from [ax].[INVENTSITE] where STATECODE = '" + chkListState.SelectedItem.Value + "'";
                    dt = baseObj.GetData(sqlstr1);
                    //chkListState.Items.Add("All...");
                    for (int i = 0; i < dt.Rows.Count; i++)

                        chkListSite.DataSource = dt;
                    chkListSite.DataTextField = "NAME";
                    chkListSite.DataValueField = "Code";
                    chkListSite.DataBind();
                }
            }
            else
            {
                chkListSite.Items.Clear();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //DataTable dt = (DataTable)Session["SaleHierarchy"];
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                    uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");

                    chkListSite.DataSource = uniqueCols;
                    chkListSite.DataTextField = "Name";
                    chkListSite.DataValueField = "Distributor";
                    chkListSite.DataBind();
                    //ddlSiteId.Items.Add("All...");
                    // chkListSite.Items.Insert(0, "All...");
                }
                else
                {
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    DataTable dt = new DataTable();
                    dt = new DataTable();

                    dt = baseObj.GetData(sqlstr1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkListSite.DataSource = dt;
                        chkListSite.DataTextField = "NAME";
                        chkListSite.DataValueField = "CODE";
                        chkListSite.DataBind();
                    }
                }
            }
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

                query = "[ax].[ACX_FocusProductSaleReport]";

                cmd.CommandText = query;
                //DateTime now = Convert.ToDateTime(txtToDate.Text);
                //now = now.AddMonths(1);

                cmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(txtFromDate.Text));
                cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));
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

        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SiteList = "";
            foreach (ListItem item in chkListSite.Items)
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
                dt = new DataTable();
                string sqlstr1 = string.Empty;
                string sqlstr = @"Select psr.PSRCode as Code,p.PSR_Name as Name from [ax].[ACXPSRSITELinkingMaster] psr 
                                  Inner join ax.acxpsrmaster p on p.PSR_Code=psr.PSRCode  
                                  where psr.PSRCode<>'' and psr.Site_Code  in (" + SiteList + ") ";
                dt = baseObj.GetData(sqlstr);
                ChkListPSR.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ChkListPSR.DataSource = dt;
                    ChkListPSR.DataTextField = "Code";
                    ChkListPSR.DataValueField = "Name";
                    ChkListPSR.DataBind();
                }
            }
            else
            {
                ChkListPSR.Items.Clear();
            }

          

        }

        public void ProductSubCategory()
        {
            string ItemCat = "";
            foreach (ListItem item in ChkItemCat.Items)
            {
                if (item.Selected)
                {
                    if (ItemCat == "")
                    {
                        ItemCat += "'" + item.Value.ToString() + "'";
                    }
                    else
                    {
                        ItemCat += ",'" + item.Value.ToString() + "'";
                    }
                }
            }
            if (ItemCat.Length > 0)
            {
                string strQuery = @"Select distinct P.PRODUCT_SUBCATEGORY as Name,P.PRODUCT_SUBCATEGORY as Code from ax.InventTable P "
                            + "where P.PRODUCT_GROUP in (" + ItemCat + ") ";

                ChkItemSub.Items.Clear();
                ChkItem.Items.Clear();
                dt = baseObj.GetData(strQuery);
                ChkItemSub.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ChkItemSub.DataSource = dt;
                    ChkItemSub.DataTextField = "NAME";
                    ChkItemSub.DataValueField = "Code";
                    ChkItemSub.DataBind();
                }
            }
            else
            {
                ChkItemSub.Items.Clear();
                ChkItem.Items.Clear();
            }
        }

        public void Product()
        {
            string Item = "";
            foreach (ListItem item in ChkItemSub.Items)
            {
                if (item.Selected)
                {
                    if (Item == "")
                    {
                        Item += "'" + item.Value.ToString() + "'";
                    }
                    else
                    {
                        Item += ",'" + item.Value.ToString() + "'";
                    }
                }
            }
            if (Item.Length > 0)
            {
                string strQuery = @"Select Distinct P.ITEMID +'-'+ P.Product_Name as Name,P.ITEMID as Code  from ax.InventTable P where P.PRODUCT_SUBCATEGORY in (" + Item + " ) ";
                ChkItem.Items.Clear();
                dt = baseObj.GetData(strQuery);
                ChkItem.Items.Add("All...");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ChkItem.DataSource = dt;
                    ChkItem.DataTextField = "NAME";
                    ChkItem.DataValueField = "Code";
                    ChkItem.DataBind();
                }
            }
            else
            {
                ChkItem.Items.Clear();
            }
        }

        protected void ChkItemCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductSubCategory();
        }

        protected void ChkItemSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            Product();
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
            DataTable dt = (DataTable)Session["SaleHierarchy"];
            DataTable uniqueCols = dt.DefaultView.ToTable(true, "HOSNAME", "HOSCODE");
            chkListHOS.DataSource = uniqueCols;
            chkListHOS.DataTextField = "HOSNAME";
            chkListHOS.DataValueField = "HOSCODE";
            chkListHOS.DataBind();
            if (uniqueCols.Rows.Count == 1)
            {
                chkListHOS.Items[0].Selected = true;
                lstHOS_SelectedIndexChanged(null, null);
            }
            FillState(dt);
            FillSite();
            //fillSiteAndState(dtHOS);
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

               // fillSiteAndState(dt);
                 FillState(dt);
                 FillSite();
                uppanel.Update();
                // chkListGM.Items.Clear();
            }
            chkListState_SelectedIndexChanged(null, null);
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

              //  fillSiteAndState(dt);
                FillState(dt);
                FillSite();
                
                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
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
              //  fillSiteAndState(dt);
                FillState(dt);
                FillSite();
                
                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
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
              //  fillSiteAndState(dt);
                FillState(dt);
                FillSite();
                
                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
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
                FillState(dt);
                FillSite();
                
                uppanel.Update();


            }
            chkListState_SelectedIndexChanged(null, null);
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
                FillSite();

                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
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
                FillSite();

                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);


            FillState(dt);
            FillSite();

            uppanel.Update();
            chkListState_SelectedIndexChanged(null, null);

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
            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
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

            chkListState_SelectedIndexChanged(null, null);
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

            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
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
            chkListState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox7, chkListEXECUTIVE);
            chkListState_SelectedIndexChanged(null, null);
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


        protected void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox9, chkListSite);
                //     chkListASM.Items.Clear();
                chkListSite_SelectedIndexChanged(null, null);

            }
            else
            {
                CheckAll_CheckedChanged(CheckBox7, chkListSite);

                //chkListASM.Items.Clear();

            }
        }

        protected void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox8, ChkListPSR);
                //     chkListASM.Items.Clear();
                //chkListSite_SelectedIndexChanged(null, null);

            }
            else
            {
                CheckAll_CheckedChanged(CheckBox8, ChkListPSR);

                //chkListASM.Items.Clear();

            }
        }

    }
}