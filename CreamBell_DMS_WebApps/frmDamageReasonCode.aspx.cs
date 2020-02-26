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
    public partial class frmDamageReasonCode : System.Web.UI.Page
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

                adp1 = new SqlDataAdapter("SELECT * FROM ax.ACXDAMAGEREASON order by DamageReason_Code asc", conn);
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            String Indentno = row.Cells[0].Text;
        }
        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;
                adp1 = new SqlDataAdapter("SELECT * FROM ax.ACXDAMAGEREASON where DamageReason_Code = '" + lnk.Text + "'", conn);
                //adp1.SelectCommand.CommandTimeout = 0;
                ds1.Clear();

                adp1.Fill(ds1, "dtl");

                if (ds1.Tables["dtl"].Rows.Count != 0)
                {
                    for (int i = 0; i < ds1.Tables["dtl"].Rows.Count; i++)
                    {
                        txtDamageCode.Text  = string.Copy(ds1.Tables["dtl"].Rows[i][0].ToString());
                        txtDescription.Text  = string.Copy(ds1.Tables["dtl"].Rows[i][5].ToString());                       
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (txtDamageCode.Text != "" && txtDescription.Text != "")
                {
                    adp2 = new SqlDataAdapter("ACX_ACXDAMAGEREASONMASTER '" + txtDamageCode.Text + "','" + txtDescription.Text + "'", conn);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();

                if (ddlSearch.Text == "Damage Reason Code")
                {
                    adp1 = new SqlDataAdapter("SELECT * FROM ax.ACXDAMAGEREASON where DamageReason_Code like '%" + txtSerch.Text + "%'", conn);
                }
                else
                {
                    adp1 = new SqlDataAdapter("SELECT * FROM ax.ACXDAMAGEREASON where DamageReason_Name like '%" + txtSerch.Text + "%'", conn);
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
    }
}