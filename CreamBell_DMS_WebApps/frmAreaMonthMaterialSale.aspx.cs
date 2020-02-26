using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Reporting.WebForms;

namespace CreamBell_DMS_WebApps
{
    public partial class frmAreaMonthMaterialSale : System.Web.UI.Page
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
                try
                {
                    FillCategory();
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
                            if (test == "VP")
                            {
                                chkListHOS.Enabled = false;
                                chkAll.Enabled = false;
                                chkAll.Checked = true;
                            }
                            else if (test == "GM")
                            {
                                chkListHOS.Enabled = false;
                                chkListVP.Enabled = false;
                                chkAll.Enabled = false;
                                chkAll.Checked = true;
                                CheckBox1.Enabled = false;
                                CheckBox1.Checked = true;
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
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            drpSTATE_SelectedIndexChanged(null, null);
        }

        protected void FillState(DataTable dt)
        {
            string sqlstr = "";
            try{
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATEWNAME");
                  //  dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                    drpSTATE.Items.Clear();
                    DataRow dr = dtState.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "--Select--";

                    dtState.Rows.InsertAt(dr, 0);
                    drpSTATE.DataSource = dtState;
                    drpSTATE.DataTextField = "STATEWNAME";
                    drpSTATE.DataValueField = "STATE";
                    drpSTATE.DataBind();
                }
                else
                {
                    DataTable dt2 = new DataTable();
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode+'-'+LS.Name Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "'";
                    //drpSTATE.Items.Add("Select...");
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = baseObj.GetConnection();
                    cmd1.CommandText = sqlstr;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                    sda.Fill(dt2);
                    //dt2.Load(cmd1.ExecuteReader());
                    drpSTATE.DataSource = dt2;
                    drpSTATE.DataTextField = "name";
                    drpSTATE.DataValueField = "code";
                    drpSTATE.DataBind();
                }
            }
            else
            {
                DataTable dt1 = new DataTable();
                sqlstr = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ";
                drpSTATE.Items.Add("Select...");
                //only name and code have to be insertd in a new datatable according to this sqlstr
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection= baseObj.GetConnection();
                cmd1.CommandText = sqlstr;
                SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                sda.Fill(dt1);
                drpSTATE.DataSource = dt1;
                drpSTATE.DataTextField = "name";
                drpSTATE.DataValueField = "code";
                drpSTATE.DataBind();
            }
            if (drpSTATE.Items.Count == 1)
            {
                drpSTATE.Items[0].Selected = true;
                drpSTATE_SelectedIndexChanged(null, null);
               // ddlCountry_SelectedIndexChanged(null, null);
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillSite()
        {
            string StateList = "";
            foreach (ListItem item in drpSTATE.Items)
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
                try{
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {   
                    ddlSiteId.Items.Clear();
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+Name Name from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") Order by Name";
                   // ddlSiteId.Items.Add("All...");
                    DataTable dt1 = new DataTable();
                    dt1 = baseObj.GetData(sqlstr1);
                    ddlSiteId.DataSource = dt1;
                    ddlSiteId.DataTextField = "Name";
                    ddlSiteId.DataValueField = "Code";
                    ddlSiteId.Items.Insert(0, "All...");
                    ddlSiteId.DataBind();
                }
                else
                {
                    ddlSiteId.Items.Clear();
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {

                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                        DataTable uniqueCols = dt.DefaultView.ToTable(true, "Distributor", "DistributorName");
                        uniqueCols.Columns.Add("Name", typeof(string), "Distributor + ' - ' + DistributorName");
                        ddlSiteId.DataSource = uniqueCols;
                        ddlSiteId.DataTextField = "Name";
                        ddlSiteId.DataValueField = "Distributor";
                        ddlSiteId.DataBind();
                        ddlSiteId.Items.Insert(0, "All...");
                    }
                    else
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID+'-'+NAME Name from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                        //ddlSiteId.Items.Add("All...");
                        DataTable dt1 = new DataTable();
                        dt1 = baseObj.GetData(sqlstr1);
                        ddlSiteId.DataSource = dt1;
                        ddlSiteId.DataTextField = "name";
                        ddlSiteId.DataValueField = "code";
                        //ddlSiteId.Items.Insert(0, "All...");
                        ddlSiteId.DataBind();
                        ddlSiteId_SelectedIndexChanged(null, null);
                    }
                }
                }
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }
        
        protected void FillCategory()
        {
            try{                       
            DataTable dt = new DataTable();
            string sqlstr11 = "select distinct(Product_Group)  from ax.inventtable  order by Product_Group";
            dt = new DataTable();
            dt = baseObj.GetData(sqlstr11);

            drpCAT.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drpCAT.DataSource = dt;
                drpCAT.DataTextField = "Product_Group";
                drpCAT.DataValueField = "Product_Group";
                drpCAT.DataBind();
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }        

        protected void FillSubCategory()
        {
            string CategoryList = "";
            try{
            foreach (ListItem item in drpCAT.Items)
            {
                if (item.Selected)
                {
                    if (CategoryList == "")
                    {
                        CategoryList += "" + item.Value.ToString() + "";
                    }
                    else
                    {
                        CategoryList += "," + item.Value.ToString() + "";
                    }
                }
            }
            if (CategoryList.Length > 0)
            {
                DataTable dt = new DataTable();
                string sqlstr1 = string.Empty;

                sqlstr1 = @"select distinct(Product_Subcategory) from ax.inventtable where Product_Group in ('" + drpCAT.SelectedItem.Text + "') order by Product_Subcategory";
              
                dt = new DataTable();
                dt = baseObj.GetData(sqlstr1);
                drpSUBCAT.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drpSUBCAT.DataSource = dt;
                    drpSUBCAT.DataTextField = "Product_Subcategory";
                    drpSUBCAT.DataValueField = "Product_Subcategory";
                    drpSUBCAT.DataBind();
                }
            }       
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            //try
            //{
                bool b = ValidateInput();
                if (b==true)
                {
                    CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                    string FilterQuery = string.Empty;
                    DataTable dtSetHeader = null;
                    //DataTable dtDataByfilter = null;
                    SqlConnection conn = null;
                    SqlCommand cmd = null;
                try
                {
                    string query1 = "Select NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                    dtSetHeader = new DataTable();
                    dtSetHeader = obj.GetData(query1);

                    string query = string.Empty;
                    conn = new SqlConnection(obj.GetConnectionString());
                    conn.Open();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 3600;
                    cmd.CommandType = CommandType.StoredProcedure;

                    query = "ACX_USP_AREAMONTHMATERIALSALE";
                    cmd.CommandText = query;

                    DateTime stDate = Convert.ToDateTime(txtCurrentDate.Text);
                    DateTime firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
                    cmd.Parameters.AddWithValue("@CURRENTSTDATE", firstDayOfMonth);

                    stDate = Convert.ToDateTime(txtBaseDate.Text);
                    firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
                    cmd.Parameters.AddWithValue("@BASESTDATE", firstDayOfMonth);
                    cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                    cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));
                    //state
                    string StateList = "";
                    foreach (ListItem item in drpSTATE.Items)
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
                        cmd.Parameters.AddWithValue("@StateId", StateList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@StateId", "");
                    }

                    // site
                    if (ddlSiteId.SelectedIndex < 0)
                    {
                        cmd.Parameters.AddWithValue("@SITEID", "");
                    }
                    else if (ddlSiteId.SelectedIndex > 0)
                    {
                        cmd.Parameters.AddWithValue("@SITEID", ddlSiteId.SelectedItem.Value);
                    }
                    else if (ddlSiteId.SelectedItem.Text != "All...")
                    {
                        cmd.Parameters.AddWithValue("@SITEID", ddlSiteId.SelectedItem.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SITEID", "");
                    }

                    //category
                    string CategoryList = "";
                    foreach (ListItem item in drpCAT.Items)
                    {
                        if (item.Selected)
                        {
                            if (CategoryList == "")
                            {
                                CategoryList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                CategoryList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    if (CategoryList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTGROUP", CategoryList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTGROUP", "");
                    }

                    //Subcategory
                    string SubCategoryList = "";
                    foreach (ListItem item in drpSUBCAT.Items)
                    {
                        if (item.Selected)
                        {
                            if (SubCategoryList == "")
                            {
                                SubCategoryList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                SubCategoryList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    if (SubCategoryList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTSUBCATEGORY", SubCategoryList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTSUBCATEGORY", "");
                    }
                    if (DDLBusinessUnit.SelectedIndex >= 1)
                    {
                        cmd.Parameters.AddWithValue("@BUCODE", DDLBusinessUnit.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BUCODE", "");
                    }
                    DataTable dtDataByfilter = new DataTable();
                    dtDataByfilter = new DataTable();
                    dtDataByfilter.Load(cmd.ExecuteReader());

                    //=================Create Excel==========

                    //DataTable dt = new DataTable();
                    //dt = dtDataByfilter;
                    string attachment = "attachment; filename=AreaMonthMaterialSale.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dtDataByfilter.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dtDataByfilter.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dtDataByfilter.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message.ToString();
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
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
                    //ShowData_ForExcel();
                }
            //}
            //catch (Exception ex)
            //{
            //    LblMessage.Text = ex.Message.ToString();
            //}
            
        }
        protected void drpCAT_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSubCategory();
        }
        protected void drpSUBCAT_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void drpSTATE_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }
        private bool ValidateInput()
        {
            bool b;
            if (txtCurrentDate.Text == string.Empty || txtBaseDate.Text == string.Empty)
            {
                b = false;
                //LblMessage.Text = "Please Provide Current Date and Base Date";
            }
            else
            {
                b = true;
                //LblMessage.Text = string.Empty;
            }
            if (txtCurrentDate.Text == txtBaseDate.Text)
            {
                b = false;
            }
            string StateList = "";
            foreach (ListItem item in drpSTATE.Items)
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
                b = true;
            }
            else
            {
                b = false;
            }
            return b;
        }
        protected void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            string FilterQuery = string.Empty;
            DataTable dtSetHeader = null;
            //DataTable dtDataByfilter = null;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                string query1 = "Select NAME from ax.inventsite where SITEID='" + Session["SiteCode"].ToString() + "'";
                dtSetHeader = new DataTable();
                dtSetHeader = obj.GetData(query1);

                string query = string.Empty;
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "ACX_USP_AREAMONTHMATERIALSALE";
                cmd.CommandText = query;

                DateTime stDate = Convert.ToDateTime(txtCurrentDate.Text);
                DateTime firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
                cmd.Parameters.AddWithValue("@CURRENTSTDATE", firstDayOfMonth);

                stDate = Convert.ToDateTime(txtBaseDate.Text);
                firstDayOfMonth = new DateTime(stDate.Year, stDate.Month, 1);
                cmd.Parameters.AddWithValue("@BASESTDATE", firstDayOfMonth);
                cmd.Parameters.AddWithValue("@UserType", Convert.ToString(Session["LOGINTYPE"]));
                cmd.Parameters.AddWithValue("@UserCode", Convert.ToString(Session["USERID"]));
                //state
                string StateList = "";
                foreach (ListItem item in drpSTATE.Items)
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
                    cmd.Parameters.AddWithValue("@StateId", StateList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@StateId", "");
                }

                // site
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
                    cmd.Parameters.AddWithValue("@SITEID", ddlSiteId.SelectedItem.Value);
                }
                else if (ddlSiteId.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@SITEID", ddlSiteId.SelectedItem.Value);
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
                    {
                        cmd.Parameters.AddWithValue("@SITEID", "");
                    }

                    //category
                    string CategoryList = "";
                    foreach (ListItem item in drpCAT.Items)
                    {
                        if (item.Selected)
                        {
                            if (CategoryList == "")
                            {
                                CategoryList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                CategoryList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    if (CategoryList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTGROUP", CategoryList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTGROUP", "");
                    }

                    //Subcategory
                    string SubCategoryList = "";
                    foreach (ListItem item in drpSUBCAT.Items)
                    {
                        if (item.Selected)
                        {
                            if (SubCategoryList == "")
                            {
                                SubCategoryList += "" + item.Value.ToString() + "";
                            }
                            else
                            {
                                SubCategoryList += "," + item.Value.ToString() + "";
                            }
                        }
                    }
                    if (SubCategoryList.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTSUBCATEGORY", SubCategoryList);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PRODUCTSUBCATEGORY", "");
                    }
                    DataTable dtDataByfilter = new DataTable();
                    dtDataByfilter = new DataTable();
                    dtDataByfilter.Load(cmd.ExecuteReader());

                    //=================Create Excel==========

                    //DataTable dt = new DataTable();
                    //dt = dtDataByfilter;
                    string attachment = "attachment; filename=Test.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", attachment);
                    Response.ContentType = "application/vnd.ms-excel";
                    string tab = "";
                    foreach (DataColumn dc in dtDataByfilter.Columns)
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }
                    Response.Write("\n");
                    int i;
                    foreach (DataRow dr in dtDataByfilter.Rows)
                    {
                        tab = "";
                        for (i = 0; i < dtDataByfilter.Columns.Count; i++)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }
                        Response.Write("\n");
                    }
                    Response.End();

                    // HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
                try{
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
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            drpSTATE_SelectedIndexChanged(null, null);
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
                try
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
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            drpSTATE_SelectedIndexChanged(null, null);
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
                try{
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
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            drpSTATE_SelectedIndexChanged(null, null);
        }

        protected void lstDGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListRM.Items.Clear();
            chkListZM.Items.Clear();
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListDGM))
            {
                try{
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
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            drpSTATE_SelectedIndexChanged(null, null);
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
                try{
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
                catch(Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            drpSTATE_SelectedIndexChanged(null, null);
        }

        protected void lstZM_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            try{
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
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            drpSTATE_SelectedIndexChanged(null, null);
        }

        protected void lstASM_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            try{
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
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            drpSTATE_SelectedIndexChanged(null, null);
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            FillState(dt);
            uppanel.Update();
            drpSTATE_SelectedIndexChanged(null, null);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try{
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
            drpSTATE_SelectedIndexChanged(null, null);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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
            drpSTATE_SelectedIndexChanged(null, null);
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

            }

            drpSTATE_SelectedIndexChanged(null, null);
        }

        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            try{
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
            }

            drpSTATE_SelectedIndexChanged(null, null);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            try{
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
            }
            drpSTATE_SelectedIndexChanged(null, null);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            try{
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
            }
            drpSTATE_SelectedIndexChanged(null, null);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            try{
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
            drpSTATE_SelectedIndexChanged(null, null);
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox7, chkListEXECUTIVE);
            drpSTATE_SelectedIndexChanged(null, null);
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

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try{
            if (ddlSiteId.SelectedItem.Text != "All...")
            {
                string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + ddlSiteId.SelectedValue.ToString() + "'";
                DDLBusinessUnit.Items.Clear();
                DDLBusinessUnit.Items.Add("All...");
                baseObj.BindToDropDown(DDLBusinessUnit, query, "bu_desc", "bu_code");
            }
            else
            {
                DDLBusinessUnit.Items.Clear();
                DDLBusinessUnit.Items.Add("All...");
            }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}