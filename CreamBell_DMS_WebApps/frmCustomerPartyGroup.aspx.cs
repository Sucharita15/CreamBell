using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCustomerPartyGroup : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!Page.IsPostBack)
            {
                GridDetail();
            }
        }

        private void GridDetail()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                adp1 = new SqlDataAdapter("SELECT CUSTGROUP_CODE,CUSTGROUP_NAME, CASE BLOCKED  WHEN 0 THEN 'Yes'   ELSE 'No' END as blocked FROM ax.ACXCUSTGROUPMASTER where BLOCKED = '0' order by CUSTGROUP_CODE asc ", conn);
                ds2.Clear();
                adp1.Fill(ds2, "dtl");

                if (ds2.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                    {
                        GridView1.DataSource = ds2.Tables["dtl"];
                        GridView1.DataBind();
                    }
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (txtCustomergroupcode.Text != "" && txtCustomergroupname.Text != "")
                {
                    adp2 = new SqlDataAdapter("ACX_ACXCUSTGROUPMASTER '" + txtCustomergroupcode.Text + "','" + txtCustomergroupname.Text + "','" + DropActive.Text + "'", conn);
                    adp2.Fill(ds1, "prod");
                }

                GridDetail();

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridDetail();            
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;
                adp1 = new SqlDataAdapter("SELECT * FROM ax.ACXCUSTGROUPMASTER where where BLOCKED = 1  and CustGroup_Code = '" + lnk.Text + "' order by CUSTGROUP_CODE asc", conn);
                //adp1.SelectCommand.CommandTimeout = 0;
                ds1.Clear();

                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds1.Tables["dtl"].Rows.Count; i++)
                    {
                        txtCustomergroupcode.Text  = string.Copy(ds1.Tables["dtl"].Rows[i][0].ToString());
                        txtCustomergroupname.Text   = string.Copy(ds1.Tables["dtl"].Rows[i][5].ToString());
                        DropActive.Text  = string.Copy(ds1.Tables["dtl"].Rows[i][6].ToString());                       
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GridView1.DataSource = null;
                GridView1.DataBind();

                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (ddlSearch.Text == "Customer Group Code")
                {
                    adp1 = new SqlDataAdapter("SELECT CUSTGROUP_CODE,CUSTGROUP_NAME, CASE BLOCKED  WHEN 0 THEN 'Yes'   ELSE 'No' END as blocked FROM ax.ACXCUSTGROUPMASTER where  BLOCKED = 0 and custgroup_code like '%" + txtSerch.Text + "%' order by CUSTGROUP_CODE asc", conn);
                }
                else
                {
                    adp1 = new SqlDataAdapter("SELECT CUSTGROUP_CODE,CUSTGROUP_NAME, CASE BLOCKED  WHEN 0 THEN 'Yes'   ELSE 'No' END as blocked FROM ax.ACXCUSTGROUPMASTER where  BLOCKED = 0 and custgroup_name like '%" + txtSerch.Text + "%' order by CUSTGROUP_CODE asc", conn);
                }
                ds2.Clear();
                adp1.Fill(ds2, "dtl");

                if (ds2.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                    {
                        GridView1.DataSource = ds2.Tables["dtl"];
                        GridView1.DataBind();
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Record not found!');", true);
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}