using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CreamBell_DMS_WebApps.App_Code;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                ShowCustomerMaster();
                ShowCustomerMaster1();
                Showmessage();
                LoadDashBoardValues();
                graphvalue();
            }
        }
        public void graphvalue()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            string query1 = string.Empty;
            try
            {
                conn = new SqlConnection(obj.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.Text;
                query1 = "Exec [ax].[ACX_DashboarSaleGraph] '" + Session["SiteCode"].ToString() + "'";
                cmd.CommandText = query1;
                //string query1 = "select trantype,siteid,left(DateName(month , DateAdd( month , month(invoic_date) , 0 ) ),3),Sum(invoice_value)/100000 from ax.acxsaleinvoiceheader where siteid='000059144'  and Year(invoic_date)=year(getdate()) and invoice_no not like 'SR%' group by trantype,siteid,month(invoic_date) order by month(invoic_date)";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                    Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 6.5F, System.Drawing.FontStyle.Bold);
                    Chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 6.5F, System.Drawing.FontStyle.Bold);
                    Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
                    Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
                    Chart1.DataSource = dt;

                }

            }
            catch (Exception ex)
            {
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

        public void LoadDashBoardValues()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            try
            {
                string query = "Select CONVERT(VARCHAR(19), LastLoginDatetime, 121) AS LoginDate from ax.ACXUSERMASTER where User_Code='" + Session["USERID"].ToString() + "'";



                string query1 = "EXEC ACX_GETDashBoardDetails '" + Session["SiteCode"].ToString() + "'";
                DataTable dt = new DataTable();
                dt = obj.GetData(query1);
                if (dt.Rows.Count > 0)
                {
                    LblPurchaseIndent.Text = dt.Rows[0]["Indent"].ToString();
                    LblPurchaseInvoice.Text = dt.Rows[0]["PurchReceipt"].ToString();
                    LblPurchaseReturn.Text = dt.Rows[0]["PurchReturn"].ToString();
                    LblSaleInvoice.Text = dt.Rows[0]["Invoice"].ToString();
                    LblSaleOrder.Text = dt.Rows[0]["LS"].ToString();
                    LblTotProduct.Text = dt.Rows[0]["TotalProduct"].ToString();
                    LblDistributorGroup.Text = dt.Rows[0]["DistGroup"].ToString();
                    LblTotDistributor.Text = dt.Rows[0]["TotalDist"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void Showmessage()
        {
            Label1.Text = "Call received by 5 PM will be treated on same day and data will refelect on next day.Futher calls received after 5PM will be treated on next subsequent working day.and the data will reflect redirect on subsequent working Day.";
        }
        private void ShowCustomerMaster()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string query = "select DATAAREAID, MASTER, APPROVER1,APPROVER2, EMAIL, CONTACTNO from AX.ACX_AUTHMATRIX order by seqno";

                DataTable dtCustomer = obj.GetData(query);
                if (dtCustomer.Rows.Count > 0)
                {
                    gridViewCustomers.DataSource = dtCustomer;
                    //gridViewCustomers.Columns[0].Visible = false;           //Column Index 1 for Customer Code 
                    gridViewCustomers.DataBind();

                    //ViewState["dtCustomer"] = dtCustomer;
                }
                else
                {
                    //gridViewCustomers.Columns[1].Visible = false;       //Column Index 1 for Customer Code 
                    //gridViewCustomers.DataBind();

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void ShowCustomerMaster1()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                string query = "select ESCALATION, CONTACTPERSON, EMAIL, CONTACTNO from AX.ACX_ESCALATION order by seqno";
                DataTable dtCustomer = obj.GetData(query);
                if (dtCustomer.Rows.Count > 0)
                {
                    gridViewCustomers1.DataSource = dtCustomer;
                    //gridViewCustomers.Columns[0].Visible = false;           //Column Index 1 for Customer Code 
                    gridViewCustomers1.DataBind();

                    //ViewState["dtCustomer"] = dtCustomer;
                }
                else
                {
                    //gridViewCustomers.Columns[1].Visible = false;       //Column Index 1 for Customer Code 
                    //gridViewCustomers.DataBind();

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}