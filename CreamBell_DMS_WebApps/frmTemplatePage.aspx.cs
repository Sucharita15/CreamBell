using CreamBell_DMS_WebApps.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmTemplatePage : System.Web.UI.Page
    {

        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                //FillState();
                FillTemplate();
                baseObj.FillSaleHierarchy();
                fillHOS();

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {

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
                //chkListState_SelectedIndexChanged(null, null);
            }
        }
        protected void FillTemplate()
        {
            try
            {
                DataTable dt = new DataTable();
                string sqlstr11 = "select * from [ax].[ACX_TemplateInfo] ";
                dt = baseObj.GetData(sqlstr11);
                    ddlTemplate.DataSource = dt;
                    ddlTemplate.DataTextField = "TemplateName";
                    ddlTemplate.DataValueField = "TemplateCode";
                    ddlTemplate.DataBind();
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void fillSiteAndState(DataTable dt)
        {
            string sqlstr = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                    chkListState.Items.Clear();
                    DataRow dr = dtState.NewRow();
                    //dr[0] = "--Select--";
                    //dr[1] = "--Select--";
                    dtState.Columns.Add("Name", typeof(string), "STATE + ' - ' + STATENAME");
                    DataView dv = dtState.DefaultView;
                    dv.Sort = "STATENAME ASC";
                    //dtState.Rows.InsertAt(dr, 0);
                    chkListState.DataSource = dv;
                    chkListState.DataTextField = "NAME";
                    chkListState.DataValueField = "STATE";
                    chkListState.DataBind();
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' +LS.Name as Name,LS.Name as StateName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "' ORDER BY StateName";
                    DataTable dt2 = new DataTable();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = baseObj.GetConnection();
                    cmd1.CommandText = sqlstr;
                    SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                    sda.Fill(dt2);
                    //chkListState.Items.Add("All...");
               
                    chkListState.DataSource = dt2;
                    chkListState.DataTextField = "NAME";
                    chkListState.DataValueField = "Code";
                    chkListState.DataBind();
  

                }
            }
            else
            {

                sqlstr = "Select Distinct I.StateCode StateCode,I.StateCode + ' - ' + LS.Name as StateName,LS.Name as Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY Name";
                chkListState.Items.Add("Select...");
                dt = baseObj.GetData(sqlstr);
                chkListState.DataSource = dt;
                chkListState.DataTextField = "StateName";
                chkListState.DataValueField = "StateCode";
                chkListState.DataBind();
                

            }
            

            if (chkListState.Items.Count == 1)
            {
                chkListState.Items[0].Selected = true;
                chkListState_SelectedIndexChanged(null, null);
            }
        }

        protected void FillState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            DataTable dt = new DataTable();
            dt = new DataTable();
            if (objcheckSitecode != null)
            {

                chkListState.Items.Clear();
                chkListSite1.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,I.StateCode +' - '+LS.Name as Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                dt = baseObj.GetData(sqlstr11);
                chkListState.Items.Add("All...");                
                chkListState.DataSource = dt;
                chkListState.DataTextField = "Name";
                chkListState.DataValueField = "Code";
                chkListState.DataBind();

            }
            else
            {
                chkListState.Items.Clear();
                chkListSite1.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,I.StateCode+' - '+LS.Name as StateName,I.SiteId,I.SiteId+' - '+I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                dt = baseObj.GetData(sqlstr1);
                chkListState.DataSource = dt;
                chkListState.DataTextField = "StateName";
                chkListState.DataValueField = "StateCode";
                chkListState.DataBind();
                chkListSite1.DataSource = dt;
                chkListSite1.DataTextField = "SiteName";
                chkListSite1.DataValueField = "SiteId";
                chkListSite1.DataBind();
                if (chkListSite1.Items.Count > 0)
                {
                    chkListState.Items[0].Selected = true;
                    chkListSite1.Items[0].Selected = true;
                }
            }
        }
        //protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    FillSite();
        //}
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
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
                    DataTable dt = new DataTable();
                    dt = new DataTable();

                    chkListSite1.Items.Clear();
                    string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where STATECODE in (" + StateList + ") AND UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000'  Order by SiteName ";
                    dt = baseObj.GetData(sqlstr1);
                    //chkListState.Items.Add("All...");

                    chkListSite1.DataSource = dt;
                    chkListSite1.DataTextField = "NAME";
                    chkListSite1.DataValueField = "Code";
                    chkListSite1.DataBind();
                }
                else
                {
                    chkListSite1.Items.Clear();
                    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                    {
                        //DataTable dt = (DataTable)Session["SaleHierarchy"];
                        DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
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
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID = UM.SITE_CODE where STATECODE in (" + StateList + ") AND UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' AND SITEID IN (" + AllSitesFromHierarchy + ") Order by SiteName ";
                        dt = baseObj.GetData(sqlstr1);
                        //uniqueCols.Columns.Add("DName", typeof(string), "Distributor + ' - ' + DistributorName");
                        //DataView dv = uniqueCols.DefaultView;
                        //dv.Sort = "DistributorName ASC";
                        chkListSite1.DataSource = dt;
                        chkListSite1.DataTextField = "NAME";
                        chkListSite1.DataValueField = "Code";
                        chkListSite1.DataBind();
                        //ddlSiteId.Items.Add("All...");
                        // chkListSite.Items.Insert(0, "All...");
                    }
                    else
                    {
                        string sqlstr1 = @"Select Distinct SITEID as Code,SITEID +' - '+ Name AS Name,Name as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order by SiteName";
                        DataTable dt = new DataTable();
                        dt = new DataTable();

                        dt = baseObj.GetData(sqlstr1);

                        chkListSite1.DataSource = dt;
                        chkListSite1.DataTextField = "NAME";
                        chkListSite1.DataValueField = "CODE";
                        chkListSite1.DataBind();
                    }
                }
                if (chkListState.Items.Count == 1)
                {
                    CheckBox9.Checked = true;
                    chkListState.Items[0].Selected = true;
                }
                if (chkListSite1.Items.Count == 1)
                {
                    CheckBox8.Checked = true;
                    chkListSite1.Items[0].Selected = true;
                }
            }
            else
            {
                if (chkListState.Items.Count == 1)
                {
                    CheckBox9.Checked = true;
                    chkListState.Items[0].Selected = true;
                }
                else
                {
                    CheckBox8.Checked = false;
                    chkListSite1.Items.Clear();
                    //chkListSite1.DataSource = null;
                    //chkListSite1.DataTextField = null;
                    //chkListSite1.DataValueField = null;
                    //chkListSite1.DataBind();
                }
            }
        }
        private bool ValidateLineItemAdd()
        {
            bool b = false;

            string SiteList = "";
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
            if (StateList.Length == 0)
            {
                 ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Atleast One State !');", true);
                 chkListState.Focus();
                 b = false;
                 return b;
            }
            else if (SiteList.Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Atleast One Site !');", true);
                chkListSite1.Focus();
                b = false;
                return b;
            }
            else
            {
                b = true;
                return b;
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
                    sqlstr1 = @"Select Distinct IV.SITEID ,IV.NAME as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where IV.STATECODE in (" + StateList + ") and UM.DEACTIVATIONDATE = '1900-01-01 00:00:00.000' order by IV.NAME";
                }
                else
                {
                    sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
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
        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                bool valid = ValidateLineItemAdd();
                if (valid == true)
                {
                    if (ddlTemplate.SelectedItem.Text == "Sale and Purchase Target Template")
                    {
                        ExcelDownload();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void ExcelDownload()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                SqlConnection conn = new SqlConnection(obj.GetConnectionString());
                SqlCommand cmd = new SqlCommand();
                DataSet dtDataByfilter = new DataSet();
                string query = string.Empty;
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "ax.ACX_SalePurchaseTemplate";
                cmd.CommandText = query;
                //------------------------
                string SiteList = "";
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
                if (SiteList.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", SiteList);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteId", "");
                }
                //-----------------------------------
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
                //------------------
                dtDataByfilter = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dtDataByfilter);
                ExcelCreation(dtDataByfilter);

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void ExcelCreation(DataSet dtDataByfilter)
        {
            try
            {
                dtDataByfilter.Tables[0].TableName = "Target";
                // dtDataByfilter.Tables[1].TableName = "Incentive Plan";
                // dtDataByfilter.Tables[2].TableName = "Report";                 
                string myServerPath = Server.MapPath("~/ExcelTemplate/SalePurchase-Template.xlsx");

                //var excel = new Microsoft.Office.Interop.Excel.Application();
                //var workbook = excel.Workbooks.Add(true);

                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                //excel.Visible = true;
                //excel.Worksheets.Add(dtDataByfilter.Tables[0], "Target1");
                Microsoft.Office.Interop.Excel.Workbook wbExiting = excel.Workbooks.Open(myServerPath);
                Microsoft.Office.Interop.Excel.Worksheet sh = wbExiting.Sheets[1];
                if (dtDataByfilter != null && dtDataByfilter.Tables[0].Rows.Count>0)
                {
                    for (int i = 0; i < dtDataByfilter.Tables[0].Rows.Count; i++)
                    {
                        sh.Cells[i+2, 1] = dtDataByfilter.Tables[0].Rows[i][0];
                        sh.Cells[i + 2, 2] = dtDataByfilter.Tables[0].Rows[i][1];
                        sh.Cells[i + 2, 3] = dtDataByfilter.Tables[0].Rows[i][2];
                        sh.Cells[i + 2, 4] = dtDataByfilter.Tables[0].Rows[i][3];
                        sh.Cells[i + 2, 5] = dtDataByfilter.Tables[0].Rows[i][4];
                        sh.Cells[i + 2, 6] = dtDataByfilter.Tables[0].Rows[i][5];
                        sh.Cells[i + 2, 7] = dtDataByfilter.Tables[0].Rows[i][6];
                        sh.Cells[i + 2, 8] = dtDataByfilter.Tables[0].Rows[i][7];
                        sh.Cells[i + 2, 9] = dtDataByfilter.Tables[0].Rows[i][8];
                       
                    }                    
                }


                string strUniqueFileName = string.Empty ;
                strUniqueFileName = Session["USERID"].ToString();

                string myServerPath1 = Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx");
                FileInfo file = new FileInfo(myServerPath1);
                if (file.Exists)
                {
                    file.Delete();
                }

                wbExiting.SaveAs(Server.MapPath("~/ExcelTemplate/" + strUniqueFileName + ".xlsx"), Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing);
                excel.DisplayAlerts = false;
                wbExiting.Close(false, Type.Missing, Type.Missing);
                excel.Quit();

                FileInfo file1 = new FileInfo(myServerPath1);

                if (file1.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=SalePurchase.xlsx");
                    Response.AddHeader("Content-Type", "application/Excel");
                    Response.ContentType = "application/vnd.xls";
                    Response.AddHeader("Content-Length", file1.Length.ToString());
                    Response.WriteFile(file1.FullName);
                   // file.Delete();    
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
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
            // FillSite();
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
                //FillSite();
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

                fillSiteAndState(dt);
                // FillSite();
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
                fillSiteAndState(dt);
                //  FillSite();
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
                fillSiteAndState(dt);
                // FillSite();
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
                fillSiteAndState(dt);
                // FillSite();
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

                fillSiteAndState(dt);

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

                fillSiteAndState(dt);

                uppanel.Update();
            }
            chkListState_SelectedIndexChanged(null, null);
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            fillSiteAndState(dt);

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
        protected void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox8, chkListSite1);
            if (chkListSite1.Items.Count == 1)
            {
                CheckBox8.Checked = true;
                chkListSite1.Items[0].Selected = true;
            }
            // chkListASM.DataSource = null;
        }
        //protected void CheckBox8_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckAll_CheckedChanged(CheckBox8, chkListSite);
        //    // chkListASM.DataSource = null;
        //    if (chkListSite.Items.Count < 2)
        //    {
        //        CheckBox8.Checked = true;
        //        chkListSite.Items[0].Selected = true;
        //    }
        //}

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

        protected void chkListSite1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkListSite1.Items.Count < 2)
            {
                CheckBox8.Checked = true;
                chkListSite1.Items[0].Selected = true;
            }
        }

        protected void CheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox9, chkListState);
            if (chkListState.Items.Count == 1)
            {
                CheckBox9.Checked = true;
                chkListState.Items[0].Selected = true;
            }
            chkListState_SelectedIndexChanged(null, null);
        }
    }
}