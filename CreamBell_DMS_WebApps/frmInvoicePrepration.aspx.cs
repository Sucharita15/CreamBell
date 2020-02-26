using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmInvoicePrepration : System.Web.UI.Page
    {
        static DropDownList ddl1 = null;
        static string siteid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            ddl1 = ddlSerch;  
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if(!IsPostBack)
            {
                if (Session["SiteCode"] != null)
                {
                    siteid = Session["SiteCode"].ToString();
                    BindGridview();
                  
                }
            }            
        }
        protected void BindGridview()
        {
            try
            {
                string query;
                query = "EXEC [dbo].[ACX_GETPENDINGSO] '" + Session["SiteCode"].ToString() + "'";
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    dt = SortDataView(dt);
                    gvHeader.DataSource = dt;
                    gvHeader.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    gvHeader.DataSource = dt;
                    gvHeader.DataBind();
                    int columncount = gvHeader.Rows[0].Cells.Count;
                    gvHeader.Rows[0].Cells.Clear();
                    gvHeader.Rows[0].Cells.Add(new TableCell());
                    gvHeader.Rows[0].Cells[0].ColumnSpan = columncount;
                    gvHeader.Rows[0].Cells[0].Text = "No Records Found";
                }
                Session["SaleOrderSearch"] = dt;
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void SONo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                CheckBox lnk = sender as CheckBox;               
                DataTable dt = new DataTable();
                string query = "Select B.SO_NO,CONVERT(VARCHAR(11),B.SO_Date,106) as SO_Date," +
                                "Customer_Group=(select C.CustGroup_Name from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=D.Cust_Group)," +
                                "Concat(D.Customer_Code,'-',D.Customer_Name) as customer," +
                                "E.Product_Group, E.ItemId+'-'+E.Product_Name as Product," +
                                "cast(A.BOX as decimal(10,2)) as BOX,cast(A.Crates as decimal(10,2)) as Crates,cast(A.Ltr as decimal(10,2)) as Ltr " +
                                " from ax.ACXLOADSHEETLINE A" +
                                " left join ax.ACXLOADSHEETHEADER B on A.LoadSheet_No=B.LoadSheet_No" +
                                " left join ax.Acxcustmaster D on D.Customer_Code=B.Customer_Code" +
                               // " left Join ax.ACXProductMaster E on A.Product_COde=E.Product_Code" +
                                " left Join ax.InventTable E on A.Product_COde=E.ItemId" +
                                " Where B.SO_NO='" + lnk.Text + "' and A.SHITEID='" + Session["SiteCode"].ToString() + "'" +
                                " Order By A.Line_No";

                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvLineDetails.DataSource = dt;
                    gvLineDetails.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    gvLineDetails.DataSource = dt;
                    gvLineDetails.DataBind();
                    int columncount = gvHeader.Rows[0].Cells.Count;
                    gvLineDetails.Rows[0].Cells.Clear();
                    gvLineDetails.Rows[0].Cells.Add(new TableCell());
                    gvLineDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                }

            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        public void BindLineItem()
        {
          try

          {

           DataTable dt = new DataTable();
          
           string query = "select A.SO_NO,CONVERT(VARCHAR(11),B.[SO_DATE],106) as SO_Date," +
                        "Customer_Group=(select C.CustGroup_Name from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=D.Cust_Group),Concat (D.Customer_Code,'-',D.Customer_Name) as customer," +
                        "Concat(D.Customer_Code,'-',D.Customer_Name) as customer," +
                        "E.Product_Group, E.ItemId+'-'+E.Product_Name as Product,cast(A.BOX as decimal(10,2)) as BOX,cast(A.Crates as decimal(10,2)) as Crates,cast(A.Ltr as decimal(10,2)) as Ltr" +
                        " from ax.ACXSALESLINE A left join [ax].[ACXSALESHEADER] B on A.SO_NO=B.SO_NO left join ax.Acxcustmaster D on D.Customer_Code=B.Customer_Code left Join ax.InventTable E on A.Product_COde=E.ItemId" +
                        " Where B.SO_NO='" + Session["SO_NO"].ToString() + "' and A.SITEID='" + Session["SiteCode"].ToString() + "'" +
                        " Order By A.Line_No";




                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvLineDetails.DataSource = dt;
                    gvLineDetails.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    gvLineDetails.DataSource = dt;
                    gvLineDetails.DataBind();
                    int columncount = gvLineDetails.Rows[0].Cells.Count;
                    gvLineDetails.Rows[0].Cells.Clear();
                    gvLineDetails.Rows[0].Cells.Add(new TableCell());
                    gvLineDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                }

            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;
                Session["So_No"] = lnk.Text;
                DataTable dt = new DataTable();
             

                string query = "select A.SO_NO,CONVERT(VARCHAR(11),B.[SO_DATE],106) as SO_Date," +
                             "Customer_Group=(select C.CustGroup_Name from ax.ACXCUSTGROUPMASTER C where C.CustGroup_Code=D.Cust_Group),Concat (D.Customer_Code,'-',D.Customer_Name) as customer," +
                             "Concat(D.Customer_Code,'-',D.Customer_Name) as customer," +
                             "E.Product_Group, E.ItemId+'-'+E.Product_Name as Product,cast(A.BOX as decimal(10,2)) as BOX,cast(A.Crates as decimal(10,2)) as Crates,cast(A.Ltr as decimal(10,2)) as Ltr" +
                             " from ax.ACXSALESLINE A left join [ax].[ACXSALESHEADER] B on A.SO_NO=B.SO_NO left join ax.Acxcustmaster D on D.Customer_Code=B.Customer_Code left Join ax.InventTable E on A.Product_COde=E.ItemId" +
                             " Where B.SO_NO='" + lnk.Text + "' and A.SITEID='" + Session["SiteCode"].ToString() + "'" +
                             " Order By A.Line_No";


                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gvLineDetails.DataSource = dt;
                    gvLineDetails.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    gvLineDetails.DataSource = dt;
                    gvLineDetails.DataBind();
                    int columncount = gvLineDetails.Rows[0].Cells.Count;
                    gvLineDetails.Rows[0].Cells.Clear();
                    gvLineDetails.Rows[0].Cells.Add(new TableCell());
                    gvLineDetails.Rows[0].Cells[0].ColumnSpan = columncount;
                }

            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "" || ddlSerch.SelectedItem.Text != "")
            {
                gvHeader.DataSource = null;
                gvHeader.DataBind();
                gvLineDetails.DataSource = null;
                gvLineDetails.DataBind();

                if (ddlSerch.SelectedItem.Text == "All")
                {
                    BindGridview();
                }
                else
                {
                    string search = "";
                    search = "%" + txtSearch.Text + "%";

                    if (Session["SaleOrderSearch"] == null)
                    {
                        string query = "EXEC [dbo].[ACX_GETPENDINGSO] '" + Session["SiteCode"].ToString() + "'";
                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                        DataTable dt = new DataTable();
                        dt = obj.GetData(query);
                        Session["SaleOrderSearch"] = dt;
                        dt = null;
                    }
                    DataTable dtSearch = (DataTable)Session["SaleOrderSearch"];
                    DataRow[] drSearch;
                    if (ddlSerch.Text == "Customer Name")
                    {
                        drSearch = dtSearch.Select("Customer_Name like '" + search + "'");
                    }
                    else if (ddlSerch.SelectedItem.Text == "Date")
                    {
                        drSearch = dtSearch.Select("SO_Date='" + Convert.ToDateTime(txtSearch.Text).ToString("dd MMM yyyy") + "'");
                    }
                    else if (ddlSerch.SelectedItem.Text == "Load Sheet No")
                    {
                        drSearch = dtSearch.Select("LoadSheet_No like '" + search + "'");
                    }
                    else if (ddlSerch.SelectedItem.Text == "Customer Group")
                    {
                        drSearch = dtSearch.Select("Customer_Group like '" + search + "'");
                    }
                    else
                    {
                        drSearch = dtSearch.Select(ddlSerch.SelectedItem.Value + " like '" + search + "'");
                    }

                    if (drSearch.Count() > 0)
                    {
                        DataTable dt = drSearch.CopyToDataTable();
                        dt = SortDataView(dt);
                        gvHeader.DataSource = dt;
                        gvHeader.DataBind();
                        CheckBox chkbox = (CheckBox)gvHeader.HeaderRow.Cells[0].FindControl("CheckAll");
                        chkbox.Checked = true;
                        chkbox.Visible = true;
                        foreach (GridViewRow grv in gvHeader.Rows)
                        {
                            CheckBox chkAll = (CheckBox)grv.Cells[0].FindControl("chkSONO");
                            chkAll.Checked = true;
                        }
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
                        return;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Enter the text in serchbox..')", true);
            }

        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetProductDescription(string prefixText)
        {
                List<string> customers = new List<string>();
          
                CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
                
                SqlConnection conn = obj.GetConnection();
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //  CreamBell_DMS_WebApps.frmInvoicePrepration f1 = new CreamBell_DMS_WebApps.frmInvoicePrepration();

                    if (ddl1.SelectedItem.Text == "SO No")
                    {
                        cmd.CommandText = "select Top 20 SO_No as Code from [ax].[ACXSALESHEADER] where " +
                         "replace(replace(SO_No, char(9), ''), char(13) + char(10), '') Like @Code+'%' and SiteId = '" + siteid + "'";
                    }
                    if (ddl1.SelectedItem.Text == "Load Sheet No")
                    {
                        cmd.CommandText = "Select distinct Top 20 LOADSHEET_NO  as Code from ax.ACXSALEINVOICEHEADER where " +
                                          "  replace(replace(LOADSHEET_NO, char(9), ''), char(13) + char(10), '') Like @Code+'%' and SiteId = '" + siteid + "'";
                    }
                    if (ddl1.SelectedItem.Text == "Customer Name")
                    {
                        cmd.CommandText = "select Top 20 CUSTOMER_CODE +'_'+ CUSTOMER_NAME as Code from ax.Acxcustmaster where " +
                            "replace(replace(CUSTOMER_NAME, char(9), ''), char(13) + char(10), '') Like @Code +'%' " +
                           "  or replace(replace(CUSTOMER_CODE, char(9), ''), char(13) + char(10), '') Like @Code+'%'  and Site_Code ='" + siteid + "'";
                    }


                    cmd.Parameters.AddWithValue("@Code", prefixText);
                    cmd.Connection = conn;
                    if (conn.State.ToString() == "Closed") { conn.Open(); }

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(sdr["Code"].ToString());
                        }
                    }
                    conn.Close();
                    return customers;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                string str = ex.Message; return customers;
            }
        }

        protected void btnInvPreparation_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                string SO_NO = string.Empty;
                string custname = string.Empty;
                foreach (GridViewRow rw in gvHeader.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSONO");
                    Label lblCustName = (Label)rw.FindControl("Customer_Name");


                    if (chkBx != null && chkBx.Checked)
                    {
                        if (custname == lblCustName.Text || custname == string.Empty)
                        {
                            count += 1;
                            int row = rw.RowIndex;

                            custname = lblCustName.Text;
                            LinkButton lblSOno = (LinkButton)gvHeader.Rows[row].FindControl("lnkbtn");
                            Label lblinv = (Label)gvHeader.Rows[row].FindControl("Invoice_No");
                            if (lblSOno.Text != "")
                            {
                                if (lblinv.Text == "")
                                {
                                    SO_NO += "" + lblSOno.Text + ",";
                                }
                            }


                        }
                        else
                        {
                            chkBx.Checked = false;
                            //lblMessge.Text = "Please select same customer..";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerts", "alert('Please select same customer..');", true);
                            return;
                        }
                    }

                    lblMessge.Text = "";

                }
                if (count > 10)
                {
                  //  ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Please Select Only 10 Sales Order..')", true);

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerts", "alert('Please Select Only 10 Sales Order..');", true);

                    lblMessge.Text = "Please Select Only 10 Sales Order..";
                    return;
                }
                SO_NO = SO_NO.Remove(SO_NO.Length - 1);
                Session["SO_NOList"] = SO_NO;
                Response.Redirect("frmInvoiceGeneration.aspx");
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
          
        }
        

        protected void gvLineDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLineDetails.PageIndex = e.NewPageIndex;

            // LinkButton lnk = (LinkButton)gvLineDetails.Rows[e.].FindControl("lnkbtn");
            BindLineItem();
        }

        protected void gvLineDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void chkSONO_CheckedChanged(object sender, EventArgs e)
        {
           
                
        }

        protected void chkSONO_CheckedChanged1(object sender, EventArgs e)
        {
            //CheckBox chk = (CheckBox)sender;
            //GridViewRow gv = (GridViewRow)chk.NamingContainer;
            //int rownumber = gv.RowIndex;

            //if (chk.Checked)
            //{
            //  for(int i=0;gvdeDetails.)
            //}

        }

        protected void ddlSerch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl1 = ddlSerch;
            txtSearch.Text = "";
        }

        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkbox = (CheckBox)sender;
                if (chkbox.Checked)
                {
                    foreach (GridViewRow grv in gvHeader.Rows)
                    {
                        CheckBox chkboxTest = (CheckBox)grv.Cells[0].FindControl("chkSONO");
                        chkboxTest.Checked = true;
                    }
                }
                else
                {
                    foreach (GridViewRow grv in gvHeader.Rows)
                    {
                        CheckBox chkboxTest = (CheckBox)grv.Cells[0].FindControl("chkSONO");
                        chkboxTest.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
         
        }

        protected void gvHeader_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[9].Text == "MOBILE")
                {
                    e.Row.BackColor = System.Drawing.Color.LightCyan;
                }
            }
        }

        protected void btnSaleOrder_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("~/frmSaleOrder.aspx");
        }

        protected void btnBulkInv_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
            SqlConnection con = obj.GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 3600;
            try
            {
                int count = 0;
                string SO_NO = string.Empty;
                string custname = string.Empty;
                foreach (GridViewRow rw in gvHeader.Rows)
                {
                    CheckBox chkBx = (CheckBox)rw.FindControl("chkSONO");
                    Label lblCustName = (Label)rw.FindControl("Customer_Name");


                    if (chkBx != null && chkBx.Checked)
                    {

                        count += 1;
                        int row = rw.RowIndex;

                        custname = lblCustName.Text;
                        LinkButton lblSOno = (LinkButton)gvHeader.Rows[row].FindControl("lnkbtn");
                        Label lblinv = (Label)gvHeader.Rows[row].FindControl("Invoice_No");
                        if (lblSOno.Text != "")
                        {
                            if (lblinv.Text == "")
                            {
                                SO_NO += "" + lblSOno.Text + ",";
                            }
                        }

                    }

                    lblMessge.Text = "";

                }
                if (count > 8)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerts", "alert('Maximum 8 sale order selected in single bulk invoice generation');", true);
                    return;
                }
                SO_NO = SO_NO.Remove(SO_NO.Length - 1);
                cmd.Connection = con;
                cmd.CommandText = "EXEC AX.ACX_SOTOINVOICECREATION_BULK '" + Session["TransLocation"] + "','" + Session["SiteCode"] + "','" + SO_NO + "'";
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                gridviewRecords.DataSource = dt;
                gridviewRecords.Visible = true;
                gridviewRecords.DataBind();
                ModalPopupExtender1.Show();
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "javascript:MyMessage(" + dt.Rows.Count + "," + dt.Select("Remarks='Success'").Count() + ");", true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "javascript:MyMessage("+dt.Rows.Count+","+dt.Select("Remarks='Success'").Count() +");", true);
                //Response.Write("<script>alert('test');</script>");
                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", "alert('test');", true);


            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        protected void gvHeader_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlsort_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindGridview();
            btnSearch_Click(null, null);
        }

        private DataTable SortDataView(DataTable dt)
        {
            if (ddlsort.SelectedIndex >= 0)
            {
                string srtType = "";
                srtType = rdasc.Checked == true ? " ASC" : " DESC";
                DataView dv = dt.DefaultView;
                dv.Sort = ddlsort.SelectedValue.ToString() + srtType;
                
                dt = dv.ToTable();
            }
            return dt;
        }

        protected void rddesc_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }
    }
}