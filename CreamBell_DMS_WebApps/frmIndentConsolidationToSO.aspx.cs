using System;
using System.Collections;
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
    public partial class frmIndentConsolidationToSO : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        //ACXINDENTCONSOLIDATED
        SqlConnection conn = null;
        SqlDataAdapter adp2, adp1;
        DataSet ds2 = new DataSet();
        DataSet ds1 = new DataSet();
        decimal BoxQty = 0;
        decimal Crates = 0;
        decimal ltr = 0;
        int prodCnt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //lblMsg.Visible = false;
                if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
                {
                    Response.Redirect("Login.aspx");
                    return;
                }

                if (!Page.IsPostBack)
                {
                    this.FillIndentList();
                    this.FillDistributor();
                    this.FillOrderType();
                    //CalendarExtender1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                //lblMsg.Text = "Error :" + ex.Message;
                //lblMsg.Visible = true;
            }


        }

        protected void FillIndentList()
        {
            string strSQL = string.Format("EXEC Ax.ACX_IndentToSO '{0}','{1}'", Session["USERID"].ToString(), Session["DATAAREAID"].ToString());
            gvDetails.DataSource = null;
            using (DataTable dt = baseObj.GetData(strSQL))
            {
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                }
            }
            gvDetails.DataBind();
        }

        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)                           //sale Header Grid View for Filling the Sale Line
        {
            string strIndentNO = string.Empty;
            string strDistributorCode = string.Empty;
            if (validateIndentConsolidation())
            {
                CheckBox chk = (CheckBox)sender;
                chk.Checked = false;
                string message = " alert('Multiple Distributors Selection Not Allowed!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                return;
            }
            foreach (GridViewRow row in gvDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                    if (chkRow.Checked)
                    {
                        strIndentNO += chkRow.Text + ",";
                        strDistributorCode += row.Cells[2].Text + ","; 
                    }
                }
            }

            if (strIndentNO != string.Empty)
            {
                strIndentNO = strIndentNO.Remove(strIndentNO.Length - 1);

                string strSQL = string.Format("EXEC Ax.ACX_IndentToSODetailDis '{0}','{1}','{2}','{3}'", strIndentNO, Session["DATAAREAID"].ToString(), Session["USERID"].ToString(), strDistributorCode);

                GridView2.DataSource = null;
                using (DataTable dt = baseObj.GetData(strSQL))
                {
                    GridView2.DataSource = dt;
                }
                GridView2.DataBind();
                Session["IndentNo"] = strIndentNO;
            }
            else
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                Session["IndentNo"] = string.Empty;
            }
        }

        protected bool validateIndentConsolidation()
        {
            bool retVal =false;

            try
            {
                string strDistributoCode = string.Empty;
                foreach (GridViewRow row in gvDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                        if (chkRow.Checked)
                        {
                            if (strDistributoCode == string.Empty)
                            {
                                strDistributoCode = row.Cells[2].Text;
                            }
                            else
                            {
                                if (strDistributoCode.IndexOf(row.Cells[2].Text) < 0)
                                {


                                    retVal = true;
                                    return retVal;
                                }
                            }
                        }
                    }
                }
                return retVal;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return retVal;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string      sitecode            = Session["SiteCode"].ToString();
            string      dataAreaId          = Session["DATAAREAID"].ToString();
            string      indent_No           = string.Empty;
            string      distributor_Code    = string.Empty;
            string      salesOfficeCode     = string.Empty;
            int         line_No             = 1;
            string      product_Code        = string.Empty;
            decimal     crateQty            = 0;
            decimal     boxQty = 0;
            decimal     PcsQty = 0;
            int recid;
            bool flag;
            int retVal;
            string strIndentNO = string.Empty;
            string strDistributorCode = string.Empty;
            string message = string.Empty;

            //Validate Inputs
            if (drpOrderType.SelectedItem.Value.ToString().Trim() == "-Select-" || drpOrderType.SelectedItem.Value.ToString().Trim().Length == 0)
            {
                message = " alert('Order Type is required!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                drpOrderType.Focus();
                return;
            }
            if (DrpPlant.SelectedItem.Value.ToString().Trim() == "-Select-" || DrpPlant.SelectedItem.Value.ToString().Trim().Length == 0)
            {
                message = " alert('Plant Code is required!');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                DrpPlant.Focus();
                return;
            }
            //if (validateIndentConsolidation()) { return; }

            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            conn = obj.GetConnection();

            adp1 = new SqlDataAdapter("SELECT Top (1) recid FROM ax.ACXINDENTCONSOLIDATED order by recid desc ", conn);
            ds2.Clear();
            adp1.Fill(ds2, "dtl");

            if (ds2.Tables["dtl"].Rows.Count != 0)
            {
                recid = Convert.ToInt16(ds2.Tables["dtl"].Rows[0][0].ToString()) + 1;
            }
            else
            {
                recid = 1;
            }

            SqlTransaction transaction;

            transaction = conn.BeginTransaction();

            try
            {
                SqlCommand comm = new SqlCommand();
                SqlCommand comm1 = new SqlCommand();
                SqlCommand comm2 = new SqlCommand();
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataTable dt = new DataTable();

                DataTable dtNumSeq = obj.GetNumSequenceNew(16, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  
                string strIndentConsolidateNo = string.Empty;
                string NUMSEQ = string.Empty;

                if (dtNumSeq != null)
                {
                    strIndentConsolidateNo = dtNumSeq.Rows[0][0].ToString();
                    NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", "alert('Number Sequence Not Defined for Indent Consolidation!');", true);
                    return;
                }

                foreach (GridViewRow row in gvDetails.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                        if (chkRow.Checked)
                        {
                            strIndentNO += chkRow.Text + ",";
                            strDistributorCode += row.Cells[2].Text + ",";
                        }
                    }
                }

                comm = new SqlCommand("ax.ACX_IndentConsolidationInsertUsp", conn, transaction);
                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("@DATAAREAID", dataAreaId);
                comm.Parameters.AddWithValue("@RECID", recid);
                comm.Parameters.AddWithValue("@CONINDENT_NO", strIndentConsolidateNo);
                comm.Parameters.AddWithValue("@SITEID", sitecode);
                comm.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                comm.Parameters.AddWithValue("@USERCODE", Session["USERID"].ToString());
                comm.Parameters.AddWithValue("@InventNOs", strIndentNO);
                comm.Parameters.AddWithValue("@DistributorCode", strDistributorCode);
                comm.Parameters.AddWithValue("@ORDERTYPECODE", drpOrderType.SelectedItem.Value.ToString());
                comm.Parameters.AddWithValue("@PLANTCODE", DrpPlant.SelectedItem.Value.ToString());

                retVal = comm.ExecuteNonQuery();

                transaction.Commit();
                flag = true;

                if (DrpDistributor.SelectedItem.Value.ToString() != "" && DrpDistributor.SelectedItem.Value.ToString() != "-Select-")
                {
                    FillIndentListFilter();
                }
                else
                {
                    FillIndentList();
                }

                GridView2.DataSource = null;
                GridView2.DataBind();
                Session["IndentNo"] = string.Empty;
                if (flag == true)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Indent Consolidation N0:" + strIndentConsolidateNo + " generated Suucessfull!');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                transaction.Rollback();
                conn.Close();
            }
        }

        protected bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            FillIndentListFilter();
        }

        protected void FillIndentListFilter()
        {
            string strSQL = string.Format("EXEC Ax.ACX_IndentToSOWithFilter '{0}','{1}','{2}','{3}','{4}'", Session["USERID"].ToString(), Session["DATAAREAID"].ToString(), txtFromDate.Text, txtToDate.Text, DrpDistributor.SelectedItem.Value.ToString());
            gvDetails.DataSource = null;
            using (DataTable dt = baseObj.GetData(strSQL))
            {
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                }
            }
            gvDetails.DataBind();

            GridView2.DataSource = null;
            GridView2.DataBind();
        }

        protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        } 
               
        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Crates += Convert.ToDecimal(e.Row.Cells[5].Text);
                BoxQty += Convert.ToDecimal(e.Row.Cells[6].Text);
                ltr += Convert.ToDecimal(e.Row.Cells[7].Text);
                prodCnt += 1;
               
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = "TOTAL  (Product # " + prodCnt.ToString() + ")";
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].ForeColor = System.Drawing.Color.MidnightBlue;
                e.Row.Cells[3].Font.Bold = true;

                e.Row.Cells[5].Text = "" + Crates;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
                e.Row.Cells[5].Font.Bold = true;

                e.Row.Cells[6].Text ="" + BoxQty;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
                e.Row.Cells[6].Font.Bold = true;

                e.Row.Cells[7].Text = "" + ltr;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
                e.Row.Cells[7].Font.Bold = true;
            }
        }

        protected void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            string strIndentNO = string.Empty;
            string strDistributorCode = string.Empty;

            if (validateIndentConsolidation()) {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Multiple Distributors Selection Not Allowed!');", true);
                return;
            }

            foreach (GridViewRow row in gvDetails.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkStatus") as CheckBox);
                    if (chkRow.Checked)
                    {
                        strIndentNO += chkRow.Text + ",";
                        strDistributorCode += row.Cells[2].Text + ",";
                    }
                }
            }

            if (strIndentNO != string.Empty)
            {
                strIndentNO = strIndentNO.Remove(strIndentNO.Length - 1);

                string strSQL = string.Format("EXEC Ax.ACX_IndentToSODetailDis '{0}','{1}','{2}','{3}'", strIndentNO, Session["DATAAREAID"].ToString(), Session["USERID"].ToString(), strDistributorCode);

                GridView2.DataSource = null;
                using (DataTable dt = baseObj.GetData(strSQL))
                {
                    GridView2.DataSource = dt;
                }

                GridView2.DataBind();
                Session["IndentNo"] = strIndentNO;
            }
            else
            {
                GridView2.DataSource = null;
                GridView2.DataBind();
                Session["IndentNo"] = string.Empty;
            }
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if (e.Row.Cells[12].Text == "MOBILE")
                //{
                //    e.Row.BackColor = System.Drawing.Color.LightCyan;
                //}              
            }
        }

        protected void drpSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (drpSearch.SelectedItem.Text == "Indent Date")
            //{
            //    CalendarExtender1.Enabled = true;
            //    txtSerch.Text = "";
            //}
            //else
            //{
            //    CalendarExtender1.Enabled = false;
            //}
        }

        protected void DrpDistributor_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillIndentListFilter();
        }

        private void FillDistributor()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = "SELECT DistributorID, DistributorName FROM [ax].[ACX_IndentToSODistributor] WHERE USERCODE='" + Session["USERID"].ToString() + "' and DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' ORDER BY DistributorName";
            
            DrpDistributor.Items.Clear();
            DrpDistributor.Items.Add("-Select-");
            obj.BindToDropDown(DrpDistributor, strQuery, "DistributorName", "DistributorID");
        }

        private void FillOrderType()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = string.Empty;
            DataTable dt = new DataTable();
            //if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            //{
            strQuery = "SELECT  B.USERCODE,A.ORDERTYPE, (A.ORDERTYPE + ' - ' + A.ORDERDESCRIPTION) AS ORDERTYPEDESCRIPTION FROM ax.ACX_ORDERPLANTMAPPING A "
                        + "Left JOIN AX.Acx_PACUserCreation B ON A.ORDERTYPE = B.ORDERTYPE ";
                        //+ " INNER JOIN AX.ACXUSERMASTER C ON C.USER_CODE = B.USERCODE";
                          //+ " WHERE C.user_code = '" + Session["USERID"].ToString() + "'";
            //}
           //else
           // {
           //     strQuery = "SELECT ORDERTYPE, (ORDERTYPE + ' - ' + ORDERDESCRIPTION) AS ORDERTYPEDESCRIPTION FROM AX.ACX_ORDERPLANTMAPPING ORDER BY ORDERTYPE";

           // }
            dt = obj.GetData(strQuery);
            drpOrderType.Items.Clear();
            DataTable dtunique = new DataTable();
            dtunique = dt.DefaultView.ToTable(true, "ORDERTYPEDESCRIPTION", "ORDERTYPE");
            drpOrderType.DataSource = dtunique;
            drpOrderType.DataTextField = "ORDERTYPEDESCRIPTION";
            drpOrderType.DataValueField = "ORDERTYPE";
            drpOrderType.DataBind();
            DataRow[] dr = dt.Select("USERCODE = '" + Session["USERID"].ToString() + "'");
            if (dr.Length > 0)
            {
                drpOrderType.SelectedValue =  dr[0]["ORDERTYPE"].ToString();
            }
            
            //drpOrderType.Items.Add("-Select-");
            //obj.BindToDropDown(drpOrderType, strQuery, "ORDERTYPEDESCRIPTION", "ORDERTYPE");
            
            FillPlantByOrderType();
            
        }
        private void FillPlantByOrderType()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string strQuery = string.Empty;
            DataTable dt = new DataTable();
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                strQuery = "SELECT P.USERCODE,O.PLANTCODE,(O.PLANTCODE+'-'+O.PLANTDESCRIPTION) AS PLANTDESCRIPTION FROM AX.ACX_ORDERPLANTMAPPING O "
                              + " LEFT JOIN AX.Acx_PACUserCreation P ON O.PLANTCODE=P.PLANTCODE "
                             // + " INNER JOIN AX.ACXUSERMASTER A ON A.USER_CODE=P.USERCODE"
                //+ " WHERE A.User_Code='" + Session["USERID"].ToString() + "'";
                            + " Where O.ORDERTYPE='" + drpOrderType.SelectedItem.Value.ToString() + "'";
            }
            else
            {
                   strQuery = "EXEC ACX_ORDERTYPE_PLANT '" + drpOrderType.SelectedItem.Value.ToString() +"'";

            }

            dt = obj.GetData(strQuery);
            DrpPlant.Items.Clear();
            DataTable dtunique = new DataTable();
            dtunique = dt.DefaultView.ToTable(true, "PLANTDESCRIPTION", "PLANTCODE");
            DrpPlant.DataSource = dtunique;
            DrpPlant.DataTextField = "PLANTDESCRIPTION";
            DrpPlant.DataValueField = "PLANTCODE";
            DrpPlant.DataBind();
            DataRow[] dr = dt.Select("USERCODE = '" + Session["USERID"].ToString() + "'");
            if (dr.Length > 0)
            {
                DrpPlant.SelectedValue = dr[0]["PLANTCODE"].ToString();
            }

            //DrpPlant.Items.Clear();
            //DrpPlant.Items.Add("-Select-");
           // obj.BindToDropDown(DrpPlant, strQuery, "PLANTDESCRIPTION", "PLANTCODE");
            DrpPlant.Focus();
        }
        protected void drpOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillPlantByOrderType();
        }
    }
}