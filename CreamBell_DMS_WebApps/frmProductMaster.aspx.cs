using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.IO;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmProductMaster : System.Web.UI.Page
    {
        SqlConnection conn = null;
        //SqlDataAdapter adp3,adp2, adp1;
        SqlDataAdapter adp1;
        DataSet ds2 = new DataSet();
        //DataSet ds1 = new DataSet();
        
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

            if (Session[SessionKeys.PRODUCT_MASTER] != null)
            {
                GridView1.DataSource = (DataTable)Session[SessionKeys.PRODUCT_MASTER];
                GridView1.DataBind();
            }
            else
            {
                Global obj = new Global();
                conn = obj.GetConnection();
                //string query = "Select distinct A.ITEMID, A.PRODUCT_GROUP,PRODUCT_SUBCATEGORY, A.PRODUCT_NAME , A.PRODUCT_PACKSIZE ,A.PRODUCT_MRP from ax.inventtable A ORDER BY A.ITEMID asc ";
                string query = "EXEC ACX_GETPRODUCTMASTER";
                //adp1 = new SqlDataAdapter("SELECT * FROM ax.ACXPRODUCTMASTER order by product_code asc", conn);
                adp1 = new SqlDataAdapter(query, conn);
                ds2.Clear();
                adp1.Fill(ds2, "dtl");

                if (ds2.Tables["dtl"].Rows.Count != 0)
                {
                    GridView1.DataSource = ds2.Tables["dtl"];
                    GridView1.DataBind();
                    Session[SessionKeys.PRODUCT_MASTER] = ds2.Tables["dtl"];
                    //for (int i = 0; i < ds2.Tables["dtl"].Rows.Count; i++)
                    //{
                    //    GridView1.DataSource = ds2.Tables["dtl"];
                    //    GridView1.DataBind();
                    //}
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }


        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    string productcode, productcode1;
        //    int productcodeint;
            
        //    CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
        //    conn = obj.GetConnection();

        //    adp1 = new SqlDataAdapter("SELECT Top (1) Product_Code FROM ax.ACXPRODUCTMASTER order by Product_Code desc ", conn);
        //    ds2.Clear();
        //    adp1.Fill(ds2, "dtl");

        //    if (ds2.Tables["dtl"].Rows.Count != 0)
        //    {
        //      productcode1 = string.Copy(ds2.Tables["dtl"].Rows[0][0].ToString());
        //      productcodeint = Convert.ToInt16( (productcode1).ToString()) + 1;
        //      productcode = Convert.ToString(productcodeint); 

        //    }
        //    else
        //    {
        //        productcode = "1";
        //    }

        //    if (txtProductName.Text != "" && txtMaterialGroup.Text != "" && txtMaterialNickName.Text != "" && txtMaterialMRP.Text != "" && txtMaterialPackSize.Text != "" && txtMaterialCratePackSize.Text != "" && txtUOM.Text != "" && txtLTR.Text != "" && txtGrossWt.Text != "" && txtNetWt.Text != "" && txtBarcodeNumber.Text != "" && DefaultWarehouse.Text != "" && txtproductNature.Text != "" && txtMaterialSubCatoery.Text != "" && txtFlavor.Text != "")
        //    {
        //        adp2 = new SqlDataAdapter("ACX_ProductMaster '" + productcode + "','" + txtProductName.Text + "','" + txtMaterialGroup.Text + "','" + txtMaterialNickName.Text + "','" + txtMaterialMRP.Text + "','" + txtMaterialPackSize.Text + "','" + txtMaterialCratePackSize.Text + "','" + txtUOM.Text + "','" + txtLTR.Text + "','" + txtGrossWt.Text + "','" + txtNetWt.Text + "','" + txtBarcodeNumber.Text + "','" + DefaultWarehouse.Text + "','" + txtproductNature.Text + "','" + txtMaterialSubCatoery.Text + "','" + txtFlavor.Text + "'", conn);
        //        adp2.Fill(ds1, "prod");
        //    }
          
        //    GridDetail();

        //    if (conn.State == ConnectionState.Open)
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
      
        //}

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
               //CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                //conn = obj.GetConnection();

                //GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;
                //Response.Redirect("frmDetailProductMaster.aspx?Data=" + Server.UrlEncode(lnk.Text));
                Response.Redirect("frmDetailProductMaster.aspx?Data=" + Server.UrlEncode(lnk.Text));
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridView1.SelectedRow;
            String Indentno = row.Cells[0].Text;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GridDetail();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GridView1.PageIndex = e.NewPageIndex;
            //GridDetail();
        }

        protected void btnExport2Excel_Click(object sender, EventArgs e)
        {
            //PrepareForExport(GridView1);
           // ExportToExcel();
            ShowData_ForExcel();
        }

        private void PrepareForExport(Control ctrl)
        {
            //iterate through all the grid controls
            foreach (Control childControl in ctrl.Controls)
            {
                //if the control type is link button, remove it
                //from the collection
                if (childControl.GetType() == typeof(LinkButton))
                {
                    ctrl.Controls.Remove(childControl);
                }
                //if the child control is not empty, repeat the process
                // for all its controls
                else if (childControl.HasControls())
                {
                    PrepareForExport(childControl);
                }
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        private void ExportToExcel()
        {
            Response.Clear();
            Response.AddHeader("content-disposition",
                                  "attachment;filename=ProductMaster.xls");
            Response.Charset = String.Empty;
            Response.ContentType = "application/ms-excel";
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(stringWriter);
            GridView1.RenderControl(HtmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }

        private void ShowData_ForExcel()
        {
            //CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            //string FilterQuery = string.Empty;
            //SqlConnection conn = null;
            //SqlCommand cmd = null;            
            //string query = string.Empty;
            try
            {
                //conn = new SqlConnection(obj.GetConnectionString());
                //conn.Open();
                //cmd = new SqlCommand();
                //cmd.Connection = conn;
                //cmd.CommandTimeout = 100;
                //cmd.CommandType = CommandType.StoredProcedure;

                //query = "ACX_GETPRODUCTMASTER";

                //cmd.CommandText = query;
                DataTable dt = new DataTable();
                //dt = new DataTable();
                dt = (DataTable) Session[SessionKeys.PRODUCT_MASTER];
                //dt.Load(cmd.ExecuteReader());
                
                //=================Create Excel==========

                string attachment = "attachment; filename=ProductMaster.xls";
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
            catch(Exception ex)
            {
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
        
    }
}