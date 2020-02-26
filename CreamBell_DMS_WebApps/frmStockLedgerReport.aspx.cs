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
    public partial class frmStockLedgerReport : System.Web.UI.Page
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
                fillBU();
                fillSiteAndState();
                fillProduct();
            }
        }

        protected void fillSiteAndState()
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlState.Items.Clear();
                string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                ddlState.Items.Add("Select...");
                baseObj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
            }
            else
            {
                ddlState.Items.Clear();
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                baseObj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");
            }
            ddlState_SelectedIndexChanged(null, null);
        }

        protected void fillProduct()
        {
            string strQuery = string.Empty;
            if (ddlBusinessUnit.SelectedItem.Text == "All...")
            {
                drpProduct.Items.Clear();
                // DDLMaterialCode.Items.Add("Select...");
                //if (DDLProductGroup.Text == "Select..." && DDLProdSubCategory.Text == "Select..." || DDLProdSubCategory.Text == "")
                //{
                    strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0 and BU_CODE in (select bm.bu_code from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "')";
                    //if (rdStock.Checked == true)
                    //{
                    //    strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                    //}
                    strQuery += "  order by Product_Name";
                    drpProduct.Items.Clear();
                    drpProduct.Items.Add("Select...");
                    //DataTable dt = baseObj.GetData(strQuery);

                    baseObj.BindToDropDown(drpProduct, strQuery, "Product_Name", "Product_Code");
                    drpProduct.Focus();
                //}
            }
            else
            {
                drpProduct.Items.Clear();
                // DDLMaterialCode.Items.Add("Select...");
                //if (DDLProductGroup.Text == "Select..." && DDLProdSubCategory.Text == "Select..." || DDLProdSubCategory.Text == "")
                //{
                    strQuery = "SELECT DISTINCT(ITEMID) as Product_Code,concat([ITEMID],' - ',PRODUCT_NAME) as Product_Name from ax.INVENTTABLE inv WHERE  INV.block=0"
                        + " and BU_CODE in('" + ddlBusinessUnit.SelectedItem.Value.ToString() + "')";
                    //if (rdStock.Checked == true)
                    //{
                    //    strQuery += " AND inv.ITEMID IN (SELECT DISTINCT ProductCode FROM AX.ACXINVENTTRANS TT WHERE SITECODE='" + Session["SiteCode"].ToString() + "' AND TransLocation IN (SELECT i.MAINWAREHOUSE FROM ax.INVENTSITE i WHERE i.SITEID=TT.SiteCode) GROUP BY ProductCode HAVING SUM(TransQty)>0)";
                    //}
                    strQuery += " order by Product_Name";
                    drpProduct.Items.Clear();
                    drpProduct.Items.Add("Select...");
                    //DataTable dt = baseObj.GetData(strQuery);

                    baseObj.BindToDropDown(drpProduct, strQuery, "Product_Name", "Product_Code");
                    drpProduct.Focus();
                //}
            }
            //string BU;
            //if (ddlBusinessUnit.SelectedIndex == 0)
            //{
            //    BU = "";
            //}
            //else
            //{
            //    BU = ddlBusinessUnit.SelectedValue.ToString();
            //}
            //    drpProduct.Items.Clear();
            //    string sqlstr1 = @"select distinct(ItemId),ItemId+'-'+Product_Name as ItemName from ax.inventtable where BU_CODE like CASE WHEN LEN('" + BU + "')>0 THEN '" + BU + "' ELSE '%' END order by ItemId";
            //    drpProduct.Items.Add("All...");
            //    baseObj.BindToDropDown(drpProduct, sqlstr1, "ItemName", "ItemId");            
        }

        protected void fillBU()
        {
            ddlBusinessUnit.Items.Clear();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            ddlBusinessUnit.Items.Add("All...");
            baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
        
        }

        protected void ddlWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
            object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
            if (objcheckSitecode != null)
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' Order By NAME";
                ddlSiteId.Items.Add("Select...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            else
            {
                ddlSiteId.Items.Clear();
                string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order By NAME";
                //ddlSiteId.Items.Add("All...");
                baseObj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
            }
            ddlWarehouse.Items.Clear();
            fillWareHouse();
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillWareHouse();
        }

        protected void fillWareHouse()
        {
            ddlWarehouse.Items.Clear();
            string sqlstr1 ="";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            { sqlstr1 = @"select INVENTLOCATIONID,NAME from Ax.inventlocation where inventsiteid='" + ddlSiteId.SelectedItem.Value + "'"; }
            else
            {
                if (ddlSiteId.SelectedIndex > 0)
                {
                    sqlstr1 = @"select INVENTLOCATIONID,NAME from Ax.inventlocation where inventsiteid='" + ddlSiteId.SelectedItem.Value + "'";
                }
            }
            if (sqlstr1.Length > 0)
            {
                baseObj.BindToDropDown(ddlWarehouse, sqlstr1, "Name", "INVENTLOCATIONID");
            }
        }
       
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b)
                {
                    ShowReportByFilter();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool ValidateInput()
        {
            bool b;
            b = true;
            if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
            {
                b = false;
                LblMessage.Text = "Please Provide From Date and To Date";
            }
            //if (drpProduct.Text == string.Empty || drpProduct.Text == "Select...")
            //{
            //    b = false;
            //    LblMessage.Text = "Please Provide Item";
            //}
            if (ddlState.Text == string.Empty || ddlState.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide State";
            }
            if (ddlSiteId.Text == string.Empty || ddlSiteId.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide Site";
            }
            if (ddlWarehouse.Text == string.Empty || ddlWarehouse.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide Warehouse.";
            }
            //else
            //{
            //    b = true;
            //    LblMessage.Text = string.Empty;
            //}
            return b;
        }

        private void ShowReportByFilter()
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new CreamBell_DMS_WebApps.App_Code.Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter1 = null;
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "ACX_STOCKLEDGER";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);
                cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
               
                // site
               if (ddlSiteId.SelectedIndex > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else if (ddlSiteId.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                //bucode
                if (ddlBusinessUnit.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", ddlBusinessUnit.SelectedItem.Value);
                }
                if (ddlBusinessUnit.SelectedIndex == 0 || ddlBusinessUnit.SelectedItem.Text == "All...")
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }
                //ItemId               
                if (drpProduct.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@ItemId", drpProduct.SelectedItem.Value);
                }
                if (drpProduct.SelectedIndex == 0 || drpProduct.Text== "All...")
                {
                    cmd.Parameters.AddWithValue("@ItemId", "");
                }
                //WareLocation               
                if (ddlWarehouse.SelectedIndex >= 0)
                {
                    cmd.Parameters.AddWithValue("@TransLocation", ddlWarehouse.SelectedItem.Value);
                }
                if (rdoDetail.Checked==true)
                {
                    cmd.Parameters.AddWithValue("@Type", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", 1);

                }
               
                dtDataByfilter1 = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                //dtDataByfilter1.Load(cmd.ExecuteReader());
                LoadDataInReportViewer(dt);
            }
            catch (Exception ex)
            {
                this.LblMessage.Visible = true;
                this.LblMessage.Text = "► " + ex.Message.ToString();
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State.ToString() == "Open") { conn.Close(); }
                }
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
                    DataSetParameter.Columns.Add("Item");
                    DataSetParameter.Columns.Add("WareHouse");
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
                    //ItemId               
                    if (drpProduct.SelectedIndex >= 1)
                    {
                        DataSetParameter.Rows[0]["Item"] = drpProduct.SelectedItem.Text;
                    }
                    //WareLocation               
                    if (ddlWarehouse.SelectedIndex >=0)
                    {
                        DataSetParameter.Rows[0]["WareHouse"] = ddlWarehouse.SelectedItem.Text;
                    }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Reports\\StockLedgerReport.rdl");
                    ReportViewer1.AsyncRendering = true;
                    ReportParameter FromDate = new ReportParameter();
                    FromDate.Name = "FromDate";
                    FromDate.Values.Add(txtFromDate.Text);
                    ReportParameter ToDate = new ReportParameter();
                    ToDate.Name = "ToDate";
                    ToDate.Values.Add(txtToDate.Text);
                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    ReportDataSource RDS1 = new ReportDataSource("DataSet1", dtSetData);
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(RDS1);
                    ReportDataSource RDS2 = new ReportDataSource("DataSet2", DataSetParameter);
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

        protected void btnExporttoExcel_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = ValidateInput();
                if (b == true)
                {
                    ShowData_ForExcel();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void ShowData_ForExcel()
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
                string query = string.Empty;

                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;

                query = "[dbo].[ACX_STOCKLEDGER]";

                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@FromDate", txtFromDate.Text);
                cmd.Parameters.AddWithValue("@ToDate", txtToDate.Text);
                //cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@ItemId", drpProduct.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@TransLocation", ddlWarehouse.SelectedItem.Value);
                //ItemId               
                if (drpProduct.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@ItemId", drpProduct.SelectedItem.Value);
                }
                if (drpProduct.SelectedIndex == 0 || drpProduct.Text == "All...")
                {
                    cmd.Parameters.AddWithValue("@ItemId", "");
                }
                //bucode
                if (ddlBusinessUnit.SelectedIndex >= 1)
                {
                    cmd.Parameters.AddWithValue("@BUCODE", ddlBusinessUnit.SelectedItem.Value);
                }
                if (ddlBusinessUnit.SelectedIndex == 0 || drpProduct.Text == "All...")
                {
                    cmd.Parameters.AddWithValue("@BUCODE", "");
                }
                // site
                if (ddlSiteId.SelectedIndex > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
                else if (ddlSiteId.SelectedItem.Text != "All...")
                {
                    cmd.Parameters.AddWithValue("@SiteId", ddlSiteId.SelectedItem.Value);
                }
              
                //WareLocation               
                if (ddlWarehouse.SelectedIndex >= 0)
                {
                    cmd.Parameters.AddWithValue("@TransLocation", ddlWarehouse.SelectedItem.Value);
                }
                if (rdoDetail.Checked == true)
                {
                    cmd.Parameters.AddWithValue("@Type", 0);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", 1);

                }
                

                //dtDataByfilter1 = new DataTable();

                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                //dtDataByfilter = new DataTable();
                //dtDataByfilter.Load(cmd.ExecuteReader());
                //DataTable dt = new DataTable();
                //dt = dtDataByfilter;

                //==================New Code=============
                DataGrid dg = new DataGrid();
                dg.DataSource = dt;
                dg.DataBind();
                // THE EXCEL FILE
                string sFileName = "StockLedgerReport" + " - " + System.DateTime.Now.Date + ".xls";
                
                sFileName = sFileName.Replace("/", "");
                // SEND OUTPUT TO THE CLIENT MACHINE USING "RESPONSE OBJECT".
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + sFileName);
                Response.ContentType = "application/vnd.ms-excel";
                EnableViewState = false;

                System.IO.StringWriter objSW = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter objHTW = new System.Web.UI.HtmlTextWriter(objSW);

                dg.HeaderStyle.Font.Bold = true;     // SET EXCEL HEADERS AS BOLD.
                dg.RenderControl(objHTW);
                string name = "Stock Ledger Report";
                if (rdoDetail.Checked == true)
                {
                    name =name+ " - Details";
                }
                else
                {
                    name = name + " - Summary";
                }

                string DistributoName = ddlSiteId.SelectedItem.Value + " - " + ddlSiteId.SelectedItem.Text;
                Response.Write("<table><tr><td colspan='5' style='width:100px;align-items:center; font:18px;'> <b> " + name + "</b> </td></tr>  <tr><td colspan='3'> <b> Distributor Name : " + DistributoName + "  </td></tr><tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txtToDate.Text + "</b></td></tr></table>");
                // STYLE THE SHEET AND WRITE DATA TO IT.
                Response.Write("<style> TABLE { border:dotted 1px #999; } " +
                    "TD { border:dotted 1px #D5D5D5; text-align:center } </style>");
                Response.Write(objSW.ToString());
                // ADD A ROW AT THE END OF THE SHEET SHOWING A RUNNING TOTAL OF PRICE.
                // Response.Write("<table><tr><td><b>Total: </b></td><td></td><td><b>" +"N2" + "</b></td></tr></table>");
                Response.End();
                dg = null;
                //=================Create Excel==========

                //string attachment = "attachment; filename=StockLedgerReport.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.ms-excel";
                //string tab = "";
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    Response.Write(tab + dc.ColumnName);
                //    tab = "\t";
                //}
                //Response.Write("\n");
                //int i;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    tab = "";
                //    for (i = 0; i < dt.Columns.Count; i++)
                //    {
                //        Response.Write(tab + dr[i].ToString());
                //        tab = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void rdoSummary_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rdoDetail_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillProduct();
        }
        
    }
}
