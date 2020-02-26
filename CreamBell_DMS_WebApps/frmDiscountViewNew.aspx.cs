using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using ClosedXML.Excel;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmDiscountViewNew : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
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
                baseObj.FillSaleHierarchy_Active();
                fillHOS();
                RunningSite();
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
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //tclabel.Width = "90%";
                    Panel1.Visible = true;
                }
                else
                {
                    //tclabel.Width = "100%";
                    Panel1.Visible = false;
                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "0" && Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                }
            }
        }

        protected void fillSiteAndState(DataTable dt)
        {
            try
            {
                string sqlstr = "";
                if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                        dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                        lstState.Items.Clear();
                        DataRow dr = dtState.NewRow();

                        lstState.DataSource = dtState;
                        lstState.DataTextField = "STATENAMES";
                        lstState.DataValueField = "STATE";
                        lstState.DataBind();
                    }
                    else
                    {
                        sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "' order by Name";
                        //ddlState.Items.Add("Select...");
                        // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                        DataTable dt1 = baseObj.GetData(sqlstr);
                        lstState.DataSource = dt1;
                        lstState.DataTextField = "Name";
                        lstState.DataValueField = "Code";
                        lstState.DataBind();
                    }
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' order by Name ";
                    lstState.Items.Add("Select...");
                    // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                    DataTable dt1 = baseObj.GetData(sqlstr);
                    lstState.DataSource = dt1;
                    lstState.DataTextField = "Name";
                    lstState.DataValueField = "Code";
                    lstState.DataBind();
                }
                if (lstState.Items.Count == 1)
                {
                    foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                    {
                        litem.Selected = true;
                    }
                    ddlCountry_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                //GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer ;
                LinkButton lnkbtn = (LinkButton)sender;
                string sqlstr = "Select ITEMID,[GROUP],[ITEMNAME],[GROUPNAME] from [ax].[ACXFREEITEMGROUPTABLE] where [Group] = '" + lnkbtn.Text + "'";
                DataTable dtCustomer = obj.GetData(sqlstr);
                gridView1.DataSource = dtCustomer;
                gridView1.DataBind();
                // gridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        private DataTable GridToDataTable()
        {
            DataTable _discountView = new DataTable();
            _discountView.Columns.Add("STARTINGDATE", typeof(string));
            _discountView.Columns.Add("ENDINGDATE", typeof(string));
            _discountView.Columns.Add("Sales Type", typeof(string));
            _discountView.Columns.Add("SalesCode", typeof(string));
            _discountView.Columns.Add("SALESDESCRIPTION", typeof(string));
            _discountView.Columns.Add("CustomerType", typeof(string));
            _discountView.Columns.Add("CUSTOMERCODE", typeof(string));
            _discountView.Columns.Add("Customer_Name", typeof(string));
            _discountView.Columns.Add("Customer Status", typeof(string));
            _discountView.Columns.Add("Scheme Item Type", typeof(string));
            _discountView.Columns.Add("SCHEMEITEMGROUP", typeof(string));
            _discountView.Columns.Add("SCHEMEITEMGROUPNAME", typeof(string));
            _discountView.Columns.Add("CalculationBase", typeof(string));
            _discountView.Columns.Add("Calculation Type", typeof(string));
            _discountView.Columns.Add("Value", typeof(string));
            _discountView.Columns.Add("Discount Status", typeof(string));
            
            foreach (GridViewRow row in gridViewCustomers.Rows)
            {    DataRow dr = _discountView.NewRow();
                Label _STARTINGDATE = (Label)row.Cells[0].FindControl("Label1");
                string _startdate = _STARTINGDATE.Text;
                Label _ENDINGDATE = (Label)row.Cells[0].FindControl("Label2");
                LinkButton _SCHEMEITEMGROUP = (LinkButton)row.Cells[10].FindControl("lnkbtn");
                string _endingdate = _ENDINGDATE.Text;
                
                int getRowIndex = row.RowIndex;
                GridViewRow gvRow = gridViewCustomers.Rows[getRowIndex];
                for (int j = 0; j < gridViewCustomers.Columns.Count; j++)
                    {
                    if (j == 0) dr[0] = _startdate;
                    else if (j == 1) dr[1] = _endingdate;
                    else if (j == 10) dr[10] = _SCHEMEITEMGROUP.Text;
                    else
                    {
                        dr[j] = row.Cells[j].Text.Replace("&#39;", "").Replace("&nbsp;", " ").Replace("&amp;", "");
                    }                    
                    }             
                _discountView.Rows.Add(dr);
            }
            return _discountView;
        }

        private bool ValidateInput()
        {
            bool b;
            b = true;
            if (lstState.Text == string.Empty || lstState.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide State.";
            }
            return b;
        }
        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                //if (lstState.SelectedValue == string.Empty)
                //{
                //    string message = "alert('Please Select The SiteID  !');";
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                //}
                string siteid = "";
                foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
                {
                    if (litem.Selected)
                    {
                        if (siteid.Length == 0)
                            siteid = "" + litem.Value.ToString() + "";
                        else
                            siteid += "," + litem.Value.ToString() + "";
                    }
                }
                string SiteLocation = "";
                foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                {
                    if (litem.Selected)
                    {
                        if (SiteLocation.Length == 0)
                            SiteLocation = "" + litem.Value.ToString() + "";
                        else
                            SiteLocation += "," + litem.Value.ToString() + "";
                    }
                }
                string block = "";
                if (rdRunningC.Checked == true)
                {
                    block = "0";
                }
                else if (rdBlockC.Checked == true)
                {
                    block = "1";
                }
                else if (rdBothC.Checked == true)
                {
                    block = "2";
                }
                query = "ACX_GetDiscountViewNew";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@siteid", siteid);
                cmd.Parameters.AddWithValue("@SiteLocation", SiteLocation);
                cmd.Parameters.AddWithValue("@BLOCK", block);
                cmd.CommandTimeout = 3600;
                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                DataTable dt1 = new DataTable();
                //=================Create Excel==========
                if (Session["avc"] != null)
                {
                    dt1 = (DataTable)Session["avc"];
                    if (dt1.Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt1, "DiscountView");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=DiscountView.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                        //string attachment = "attachment; filename=DiscountView.xls";
                        //Response.ClearContent();
                        //Response.AddHeader("content-disposition", attachment);
                        //Response.ContentType = "application/vnd.ms-excel";
                        string tab = "";

                        StringWriter sw = new StringWriter();
                        System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                        foreach (DataColumn dc in dt1.Columns)
                        {
                            Response.Write(tab + dc.ColumnName);
                            tab = "\t";
                        }

                        Response.Write("\n");
                        int i;
                        foreach (DataRow dr in dt1.Rows)
                        {
                            tab = "";
                            for (i = 0; i < dt1.Columns.Count; i++)
                            {
                                Response.Write(tab + dr[i].ToString());
                                tab = "\t";
                            }
                            Response.Write("\n");
                        }
                    }
                }
                else if (txtSearch.Text != string.Empty)
                {

                    DataTable _discountView = GridToDataTable();
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(_discountView, "DiscountView");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=DiscountView.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }                   
                    string tab = "";

                    StringWriter sw = new StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                    foreach (DataColumn dc in _discountView.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }

                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in _discountView.Rows)
                    {
                        tab = "";
                        for (i = 0; i < _discountView.Columns.Count; i++)
                        {
                            //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                            //Response.Write(style);
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                }
                else
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "DiscountView");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=DiscountView.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }                   
                    string tab = "";
                    gridViewCustomers.DataSource = null;
                    StringWriter sw = new StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

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
                            //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                            //Response.Write(style);
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                }
                Response.End();
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                Session["avc"] = null;
                //if (lstSiteId.SelectedValue == string.Empty)
                //{
                //    gridViewCustomers.DataSource = null;
                //    gridViewCustomers.DataBind();

                //    ViewState["dtCustomer"] = null;
                //    string message = "alert('Please Select The SiteID !');";
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                //}
                string siteid = "";
                string SiteLocation = "";
                int count = 0;
                foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
                {
                    if (litem.Selected)
                    {
                        count += 1;
                        if (siteid.Length == 0)
                            siteid = "" + litem.Value.ToString() + "";
                        else
                            siteid += "," + litem.Value.ToString() + "";
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                {
                    if (litem.Selected)
                    {
                        if (SiteLocation.Length == 0)
                            SiteLocation = "" + litem.Value.ToString() + "";
                        else
                            SiteLocation += "," + litem.Value.ToString() + "";
                    }
                }

                if (count > 5)
                {
                    // string message = "alert('Click On Export to Excel Only.If more than 5 Distributor Selected!');";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Click On Export To Excel Only.If More Than 5 Distributor Selected!');", true);
                    txtSearch.Text = "";
                    return;
                }
                string block = "";
                if (rdRunningC.Checked == true)
                {
                    block = "0";
                }
                else if (rdBlockC.Checked == true)
                {
                    block = "1";
                }
                else if (rdBothC.Checked == true)
                {
                    block = "2";
                }
                txtSearch.Text = "";
                string query = "EXEC ACX_GetDiscountViewNew '" + siteid.ToString() + "','" + SiteLocation + "','" + block + "'";
                DataTable dtCustomer = obj.GetData(query);
                Session.Add("GridData", dtCustomer);
                if (dtCustomer.Rows.Count > 0)
                {
                    gridViewCustomers.DataSource = dtCustomer;
                    //gridViewCustomers.Columns[0].Visible = false;           //Column Index 1 for Customer Code 
                    gridViewCustomers.DataBind();
                    // ViewState["dtCustomer"] = dtCustomer;
                    gridViewCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    gridViewCustomers.Columns[1].Visible = false;       //Column Index 1 for Customer Code 
                    gridViewCustomers.DataBind();
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ShowData_ForExcel();
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gridViewCustomers.DataSource = null;
                gridViewCustomers.DataBind();
                gridView1.DataSource = null;
                gridView1.DataBind();
                string statesel = "";
                foreach (System.Web.UI.WebControls.ListItem litem1 in lstState.Items)
                {
                    if (litem1.Selected)
                    {
                        if (statesel.Length == 0)
                            statesel = "'" + litem1.Value.ToString() + "'";
                        else
                            statesel += ",'" + litem1.Value.ToString() + "'";
                    }
                }
                if (lstState.SelectedValue == string.Empty)
                {
                    lstSiteId.Items.Clear();
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    DataView view = new DataView(dt);
                    DataTable distinctValues = view.ToTable(true, "Distributor", "DistributorName");
                    lstSiteId.DataSource = distinctValues;
                    string AllSitesFromHierarchy = "";
                    foreach (DataRow row in distinctValues.Rows)
                    {
                        if (AllSitesFromHierarchy == "")
                        {
                            AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                        }
                        else
                        {
                            AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                        }
                    }
                    if (AllSitesFromHierarchy != "")
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                        dt = baseObj.GetData(sqlstr1);
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "Name";
                        lstSiteId.DataValueField = "Code";
                        lstSiteId.DataBind();
                    }
                }
                else
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode != null)
                    {
                        lstSiteId.Items.Clear();
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where STATECODE in (" + statesel + ") Order by SiteName";
                        DataTable dt = baseObj.GetData(sqlstr1);
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "Name";
                        lstSiteId.DataValueField = "Code";
                        lstSiteId.DataBind();
                    }
                    else
                    {
                        lstSiteId.Items.Clear();
                        if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                        {
                            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                            dt.DefaultView.RowFilter = "STATE in (" + statesel + ")";
                            DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                            string AllSitesFromHierarchy = "";
                            foreach (DataRow row in uniqueCols.Rows)
                            {
                                if (AllSitesFromHierarchy == "")
                                {
                                    AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                                }
                                else
                                {
                                    AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                                }
                            }
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                            dt = baseObj.GetData(sqlstr1);
                            lstSiteId.DataSource = dt;
                            lstSiteId.DataTextField = "Name";
                            lstSiteId.DataValueField = "Code";
                            lstSiteId.DataBind();
                        }
                        else
                        {
                            string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where SITEID = '" + Session["SiteCode"].ToString() + "'";
                            DataTable dt = baseObj.GetData(sqlstr1);
                            lstSiteId.DataSource = dt;
                            lstSiteId.DataTextField = "Name";
                            lstSiteId.DataValueField = "Code";
                            lstSiteId.DataBind();
                        }
                    }
                }
                if (lstSiteId.Items.Count == 1)
                {
                    foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
                    {
                        litem.Selected = true;
                    }
                }
                Session["SalesData"] = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                RunningSite();
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void btnSearchCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewCustomers.Rows.Count == 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please provide the Searching Keyword !');", true);
                    txtSearch.Focus();
                    gridViewCustomers.DataSource = ViewState["dtCustomer"];
                    gridViewCustomers.DataBind();
                }
                else if (txtSearch.Text == string.Empty)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please provide the Searching Keyword !');", true);
                    txtSearch.Focus();
                    gridViewCustomers.DataSource = ViewState["dtCustomer"];
                    gridViewCustomers.DataBind();
                }
                else
                {
                    //ShowCustomerByFilter();
                    Showcustomer();
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        private void Showcustomer()
        {
            try
            {
                DataTable _gridData = Session["GridData"] as DataTable;
                DataTable _newGridData = new DataTable();
                if (DDLSearchType.SelectedValue == "Customer Name")
                {
                    string _search = txtSearch.Text.Trim().ToString();
                    int count = _gridData.Select("Customer_Name like '%" + _search + "%'").Count();
                    if (count > 0)
                    {
                        _newGridData = _gridData.Select("Customer_Name like '%" + _search + "%'").CopyToDataTable();
                        gridViewCustomers.DataSource = _newGridData;
                        gridViewCustomers.DataBind();
                        gridViewCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    else
                    {
                        string message = "alert('Customer Name not found');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        gridViewCustomers.DataSource = null;
                        gridViewCustomers.DataBind();
                        gridViewCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
                if (DDLSearchType.SelectedValue == "Customer Code")
                {
                    string _search = txtSearch.Text.Trim().ToString();
                    int count = _gridData.Select("CUSTOMERCODE like '%" + _search + "%'").Count();
                    if (count > 0)
                    {
                        _newGridData = _gridData.Select("CUSTOMERCODE like '%" + _search + "%'").CopyToDataTable();
                        gridViewCustomers.DataSource = _newGridData;
                        gridViewCustomers.DataBind();
                        gridViewCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    else
                    {
                        string message = "alert('Customer Code not found');";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                        gridViewCustomers.DataSource = null;
                        gridViewCustomers.DataBind();
                        gridViewCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void fillHOS()
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstHOS_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstVP_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstDGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
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
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstZM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstASM_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
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
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
            fillSiteAndState(dt);
            uppanel.Update();
            ddlCountry_SelectedIndexChanged(null, null);
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
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
                //chkListASM.Items.Clear();
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

        protected void RunningSite()
        {
            try
            {
                string statesel = "";
                foreach (System.Web.UI.WebControls.ListItem litem1 in lstState.Items)
                {
                    if (litem1.Selected)
                    {
                        if (statesel.Length == 0)
                            statesel = "'" + litem1.Value.ToString() + "'";
                        else
                            statesel += ",'" + litem1.Value.ToString() + "'";
                    }
                }
                if (lstState.SelectedValue == string.Empty)
                {
                    lstSiteId.Items.Clear();
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

                    DataView view = new DataView(dt);
                    DataTable distinctValues = view.ToTable(true, "Distributor", "DistributorName");
                    lstSiteId.DataSource = distinctValues;
                    string AllSitesFromHierarchy = "";
                    foreach (DataRow row in distinctValues.Rows)
                    {
                        if (AllSitesFromHierarchy == "")
                        {
                            AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                        }
                        else
                        {
                            AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                        }
                    }
                    if (AllSitesFromHierarchy != "")
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV LEFT JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                        dt = baseObj.GetData(sqlstr1);
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "Name";
                        lstSiteId.DataValueField = "Code";
                        lstSiteId.DataBind();
                    }
                }
                else
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode != null)
                    {
                        lstSiteId.Items.Clear();
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where UM.DEACTIVATIONDATE='1900-01-01 00:00:00.000' AND STATECODE in (" + statesel + ") Order by SiteName";
                        DataTable dt = baseObj.GetData(sqlstr1);
                        lstSiteId.DataSource = dt;
                        lstSiteId.DataTextField = "Name";
                        lstSiteId.DataValueField = "Code";
                        lstSiteId.DataBind();
                    }
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void rdRunningC_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            gridViewCustomers.DataSource = null;
            gridViewCustomers.DataBind();
            gridView1.DataSource = null;
            gridView1.DataBind();
            txtSearch.Text = "";
        }
    }
}