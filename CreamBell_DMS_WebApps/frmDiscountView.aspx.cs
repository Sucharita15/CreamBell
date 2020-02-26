using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;
using CreamBell_DMS_WebApps.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;

namespace CreamBell_DMS_WebApps
{
    public partial class frmDiscountView : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                fillSiteAndState();
            }
        }

        protected void fillSiteAndState()
        {
            try
            {
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = obj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    ddlState.Items.Clear();
                    string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' ORDER BY LS.Name ";
                    ddlState.Items.Add("Select...");
                    obj.BindToDropDown(ddlState, sqlstr11, "Name", "Code");
                }
                else
                {
                    ddlState.Items.Clear();
                    ddlSiteId.Items.Clear();

                    string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'  ORDER BY LS.Name";
                    obj.BindToDropDown(ddlState, sqlstr1, "StateName", "StateCode");
                    obj.BindToDropDown(ddlSiteId, sqlstr1, "SiteName", "SiteId");

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
                string sqlstr = "Select ITEMID,[GROUP],[ITEMNAME],[GROUPNAME] from [ax].[ACXFREEITEMGROUPTABLE] where [Group] = '"+lnkbtn.Text+"'" ;
                DataTable dtCustomer = obj.GetData(sqlstr);
                gridView1.DataSource = dtCustomer;
                gridView1.DataBind();
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        private bool Validate()
        {
            bool b;
            b = true;
            if (ddlState.Text == string.Empty || ddlState.Text == "Select...")
            {
                b = false;
                LblMessage.Text = "Please Provide State.";
            }

            return b;
        }

        private void ShowData_ForExcel()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
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

                query = "ACX_GetDiscountView";

                cmd.CommandText = query;
                //cmd.Parameters.AddWithValue("@siteid", Session["SiteCode"].ToString());
                cmd.Parameters.AddWithValue("@siteid", ddlSiteId.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@SiteLocation", Session["SITELOCATION"].ToString());
                cmd.Parameters.AddWithValue("@SiteLocation", ddlState.SelectedItem.Value);

                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                //=================Create Excel==========

                string attachment = "attachment; filename=DiscountView.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                
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
                        Response.Write(tab+ dr[i].ToString());                
                        tab = "\t";
                    }
                    Response.Write("\n");
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

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gridViewCustomers.DataSource = null;
                gridViewCustomers.DataBind();

                gridView1.DataSource = null;
                gridView1.DataBind();

                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = obj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {
                    ddlSiteId.Items.Clear();
                    string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE where UM.DEACTIVATIONDATE='1900-01-01 00:00:00.000' and STATECODE = '" + ddlState.SelectedItem.Value + "' Order By NAME";
                    //  string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where STATECODE = '" + ddlState.SelectedItem.Value + "' Order By NAME";
                    // ddlSiteId.Items.Add("All...");
                    obj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                }
                else
                {
                    ddlSiteId.Items.Clear();
                    string sqlstr1 = @"Select Distinct SITEID as Code,NAME from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "' Order By NAME";
                    //ddlSiteId.Items.Add("All...");
                    obj.BindToDropDown(ddlSiteId, sqlstr1, "Name", "Code");
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                bool b = Validate();
                if(b)
                { 
                string sqlstr = "Select A.STARTINGDATE,A.ENDINGDATE,A.[Sales Type],A.[SalesCode],A.[SALESDESCRIPTION],A.[Type] as CustomerType,A.[CUSTOMERCODE] "
                               + " ,A.[Discount Type] as [Scheme Item Type],A.SCHEMEITEMGROUP,A.SCHEMEITEMGROUPNAME,A.CalculationBase,A.[Calculation Type],A.Value "
                               + " ,Isnull(B.Customer_Name,'') as Customer_Name "
                               + " from Acx_DiscountView A "
                               + " Left Join [ax].[ACXCUSTMASTER] B on A.[CUSTOMERCODE]=B.[CUSTOMER_CODE] and A.Salescode=B.Site_code "
                                //+ " where Salescode in ('" + Session["SiteCode"].ToString() + "','" + Session["SITELOCATION"].ToString() + "','')";
                                + " where Salescode in ('" + ddlSiteId.SelectedItem.Value + "','" + ddlState.SelectedItem.Value + "','')";
                    DataTable dtCustomer = obj.GetData(sqlstr);
                gridViewCustomers.DataSource = dtCustomer;
                gridViewCustomers.DataBind();
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ShowData_ForExcel();
        }

        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridViewCustomers.DataSource = null;
            gridViewCustomers.DataBind();

            gridView1.DataSource = null;
            gridView1.DataBind();
        }
    }
}