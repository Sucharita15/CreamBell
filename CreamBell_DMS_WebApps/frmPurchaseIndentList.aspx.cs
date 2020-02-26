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
    public partial class frmPurchaseIndentList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (Session["SiteCode"] != null)
            {                
                BindGridview();
            }
        }

        protected void BindGridview()
        {
            try
            {
                string query;
                query = "Select A.Indent_NO,CONVERT(VARCHAR(11),A.Indent_Date,106) as Indent_Date ,coalesce(CONVERT(VARCHAR(11),Required_Date,106),'') as Required_Date," +
                       // "SALEOFFICE_CODE=(select B.SALEOFFICE_CODE from ax.ACXSITEMASTER B where B.SiteId=A.SiteId )," +
                       "SALEOFFICE_CODE=(select B.ACXPLANTCODE from [ax].[INVENTSITE] B where B.SiteId=A.SiteId )," +
                        //"SALEOFFICE_NAME=(select B.SALEOFFICE_NAME from ax.ACXSITEMASTER B where B.SiteId=A.SiteId ),"+
                        "SALEOFFICE_NAME=(select B.ACXPLANTNAME from [ax].[INVENTSITE] B where B.SiteId=A.SiteId )," +
                        "Box=(Select cast(sum(C.BOX) as decimal(10,2)) BOX   from ax.ACXPURCHINDENTLINE C where C.Indent_No=A.Indent_No and A.SiteId=C.SiteId )," +
                        "Crates=(Select cast(sum(C.Crates) as decimal(10,2)) Crates from ax.ACXPURCHINDENTLINE C where C.Indent_No=A.Indent_No and A.SiteId=C.SiteId )," +
                        "Ltr=(Select cast(sum(C.Ltr) as decimal(10,2)) Ltr  from ax.ACXPURCHINDENTLINE C where C.Indent_No=A.Indent_No and A.SiteId=C.SiteId )," +
                        "A.So_No, A.Invoice_No ,"+
                        " case A.STATUS when  1 then 'Confirm' when 0  then 'Pending' end as Confirm "+
                        "from ax.ACXPURCHINDENTHEADER A where A.indent_No!='' and [Siteid]='" + Session["SiteCode"].ToString() + "' order by A.Indent_Date desc";

                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
                DataTable dt = new DataTable();               
                dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
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

            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        public void BindNewLine()
        {
            try
            {
                
                DataTable dt = new DataTable();
                string query;
                query = "select A.Indent_No, A.Line_No,A.Product_Group,A.Product_Code," +
                        "Product_Name=(Select B.Product_Name from ax.AcxProductMaster B where B.Product_Code=A.Product_Code and " +
                        "B.Product_GRoup=A.Product_Group)," +
                        "cast(A.Box as decimal(10,2)) Box ,cast(A.Crates as decimal(10,2)) Crates ,cast(A.Ltr as decimal(10,2)) Ltr " +
                        "from ax.ACXPurchIndentLine A Where A.Indent_No='" + Session["IndentNo"].ToString() + "' and A.SITEID='" + Session["SiteCode"].ToString() + "' order by A.Line_no";

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
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {

            }
        }
        protected void Status_Click(object sender, EventArgs e)
        {
            // GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
            //GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            //string abc = (clickedRow.FindControl("lnkbtnDel") as LinkButton).Text;
            //LinkButton lnk = (LinkButton)gvrow.FindControl("Status");
            LinkButton lnk = (LinkButton)sender;
            Session["TEMP"] = lnk.CommandArgument.ToString();
            Response.Redirect("frmPurchaseIndent.aspx");

        }
        protected void lnkbtnso_Click(object sender, EventArgs e)
        {

        }

        protected void lnkbtninv_Click(object sender, EventArgs e)
        {

        }

      

        protected void lnkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;
                LinkButton lnk = sender as LinkButton;
                Session["Lndentno"] = lnk.Text;
                DataTable dt = new DataTable();
                string query;
                //query = "select A.Indent_No, A.Line_No,A.Product_Group,A.Product_Code," +
                //        "Product_Name=(Select B.Product_Name from ax.AcxProductMaster B where B.Product_Code=A.Product_Code and " +
                //        "B.Product_GRoup=A.Product_Group)," +
                //        "cast(A.Box as decimal(10,2)) Box ,cast(A.Crates as decimal(10,2)) Crates ,cast(A.Ltr as decimal(10,2)) Ltr " +
                //        "from ax.ACXPurchIndentLine A Where A.Indent_No='" + lnk.Text + "' and A.SITEID='" + Session["SiteCode"].ToString() + "' order by A.Line_no";

                query = "select A.Indent_No, A.Line_No,A.Product_Group,A.Product_Code," +
                        "Product_Name=(Select B.Product_Name from ax.INVENTTABLE B where B.Itemid=A.Product_Code and " +
                        "B.Product_GRoup=A.Product_Group)," +
                        "cast(A.Box as decimal(10,2)) Box ,cast(A.Crates as decimal(10,2)) Crates ,cast(A.Ltr as decimal(10,2)) Ltr " +
                        "from ax.ACXPurchIndentLine A Where A.Indent_No='" + lnk.Text + "' and A.SITEID='" + Session["SiteCode"].ToString() + "' order by A.Line_no";
                                                
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
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
               
            }
        }

        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "" || ddlSerch.SelectedItem.Text == "All")
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
                    try
                    {
                        string query = "Select A.Indent_NO,CONVERT(VARCHAR(11),A.Indent_Date,106) as Indent_Date ,coalesce(CONVERT(VARCHAR(11),Required_Date,106),'') as Required_Date," +
                        //"SALEOFFICE_CODE=(select B.SALEOFFICE_CODE from ax.ACXSITEMASTER B where B.SiteId=A.SiteId )," +
                        "SALEOFFICE_CODE=(select B.ACXPLANTCODE from [ax].[INVENTSITE] B where B.SiteId=A.SiteId )," +
                        //"SALEOFFICE_NAME=(select B.SALEOFFICE_NAME from ax.ACXSITEMASTER B where B.SiteId=A.SiteId )," +
                        "SALEOFFICE_NAME=(select B.ACXPLANTNAME from [ax].[INVENTSITE] B where B.SiteId=A.SiteId )," +
                        "Box=(Select cast(sum(C.BOX) as decimal(10,2)) BOX   from ax.ACXPURCHINDENTLINE C where C.Indent_No=A.Indent_No and A.SiteId=C.SiteId )," +
                        "Crates=(Select cast(sum(C.Crates) as decimal(10,2)) Crates from ax.ACXPURCHINDENTLINE C where C.Indent_No=A.Indent_No and A.SiteId=C.SiteId )," +
                        "Ltr=(Select cast(sum(C.Ltr) as decimal(10,2)) Ltr  from ax.ACXPURCHINDENTLINE C where C.Indent_No=A.Indent_No and A.SiteId=C.SiteId )," +                       
                        " A.So_No, A.Invoice_No ," +
                        " case A.STATUS when  1 then 'Confirm' when 0  then 'Pending' end as Confirm " +                       
                        "from ax.ACXPURCHINDENTHEADER A where A.indent_No!='' and [Siteid]='" + Session["SiteCode"].ToString() + "' and A." + ddlSerch.SelectedItem.Value + " like '" + search + "'  order by A.Indent_No";                           
                                                
                        DataTable dt = new DataTable();
                        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                        dt = obj.GetData(query);                                   
                        if (dt.Rows.Count > 0)
                        {
                            gvHeader.DataSource = dt;
                            gvHeader.DataBind();
                            
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
                            
                        }
                        //===============================================                       
                        dt.Clear();
                        search = txtSearch.Text;
                       // query = "select A.Indent_No, A.Line_No,A.Product_Group,A.Product_Code," +
                       //"Product_Name=(Select B.Product_Name from ax.AcxProductMaster B where B.Product_Code=A.Product_Code and " +
                       //"B.Product_GRoup=A.Product_Group)," +
                       //"cast(A.Box as decimal(10,2)) Box ,cast(A.Crates as decimal(10,2)) Crates ,cast(A.Ltr as decimal(10,2)) Ltr " +
                       //"from ax.ACXPurchIndentLine A Where A." + ddlSerch.SelectedItem.Value + "='" + search + "' and A.SITEID='" + Session["SiteCode"].ToString() + "' order by A.Line_no";

                        query = "select A.Indent_No, A.Line_No,A.Product_Group,A.Product_Code," +
                      "Product_Name=(Select B.Product_Name from ax.INVENTTABLE B where B.Itemid=A.Product_Code and " +
                      "B.Product_GRoup=A.Product_Group)," +
                      "cast(A.Box as decimal(10,2)) Box ,cast(A.Crates as decimal(10,2)) Crates ,cast(A.Ltr as decimal(10,2)) Ltr " +
                      "from ax.ACXPurchIndentLine A Where A." + ddlSerch.SelectedItem.Value + "='" + search + "' and A.SITEID='" + Session["SiteCode"].ToString() + "' order by A.Line_no";


                        dt = obj.GetData(query);   
                        if (dt.Rows.Count > 0)
                        {
                            gvLineDetails.DataSource = dt;
                            gvLineDetails.DataBind();
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Record Not Found..')", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    finally
                    {
                       
                    }

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alerts", "javascript:alert('Enter the text in serchbox..')", true);
            }

        }


        //protected void gvLineDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    gvLineDetails.PageIndex = e.NewPageIndex;
        //    BindNewLine();
        //}

        protected void gvHeader_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                String Status = string.Empty;
                LinkButton LblStatus;
                LblStatus = (LinkButton)e.Row.FindControl("Status");
                //Session["lblstatus"] = LblStatus.Text;  
                //int i = e.Row.RowIndex;
                if (LblStatus.Text == "Confirm")
                {
                    LblStatus.ForeColor=System.Drawing.Color.Green;
                    LblStatus.Enabled = false;
                }
                else if (LblStatus.Text == "Pending")
                {
                    LblStatus.ForeColor = System.Drawing.Color.DarkOrange;
                }
                
            }
        }


    }
}