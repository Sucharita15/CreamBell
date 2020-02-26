
using Elmah;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCustomerOutStandingWriteOff : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        public DataTable dt;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string query = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                FillState();
            }
        }
        protected void FillState()
        {
            try
            {
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                DataTable dt = new DataTable();

                dt = new DataTable();
                if (objcheckSitecode != null)
                {
                    chkListState.Items.Clear();
                    string sqlstr11 = "Select Distinct I.StateCode Code,LS.Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>''  ";
                    dt = baseObj.GetData(sqlstr11);
                    chkListState.Items.Add("All...");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkListState.DataSource = dt;
                        chkListState.DataTextField = "NAME";
                        chkListState.DataValueField = "Code";
                        chkListState.DataBind();
                    }
                }
                else
                {
                    chkListState.Items.Clear();
                    chkListSite.Items.Clear();
                    string sqlstr1 = @"Select I.StateCode StateCode,LS.Name as StateName,I.SiteId,I.Name as SiteName from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.SiteId = '" + Session["SiteCode"].ToString() + "'";
                    dt = baseObj.GetData(sqlstr1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkListState.DataSource = dt;
                        chkListState.DataTextField = "StateName";
                        chkListState.DataValueField = "StateCode";
                        chkListState.DataBind();

                        chkListSite.DataSource = dt;
                        chkListSite.DataTextField = "SiteName";
                        chkListSite.DataValueField = "SiteId";
                        chkListSite.DataBind();
                    }
                    chkListState.Items[0].Selected = true;
                    chkListSite.Items[0].Selected = true;
                    FillCustomerGroup();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void FillSite()
        {
            try
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
                        sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where STATECODE in (" + StateList + ") order by NAME";
                    }
                    else
                    {
                        sqlstr1 = @"Select Distinct SITEID ,NAME as SiteName from [ax].[INVENTSITE] where SITEID = '" + Session["SiteCode"].ToString() + "'";
                    }

                    dt = new DataTable();
                    // dt = baseObj.GetData(sqlstr1);
                    chkListSite.Items.Clear();
                    dt = baseObj.GetData(sqlstr1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkListSite.DataSource = dt;
                        chkListSite.DataTextField = "SiteName";
                        chkListSite.DataValueField = "SiteId";
                        chkListSite.DataBind();
                    }

                }
                else
                {
                    chkListSite.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void FillCustomerGroup()
        {
            try
            {
                DataTable dt = new DataTable();
                //            string sqlstr = "select distinct A.CUST_GROUP as code,Name=(Select CustGroup_Name from ax.ACXCUSTGROUPMASTER where Custgroup_Code=A.CUST_GROUP ) from ax.acxcustmaster A where A.site_code='" + Session["SiteCode"].ToString() + "'";
                string sqlstr = "select Distinct CustGroup_Name as Name,Custgroup_Code as Code from ax.ACXCUSTGROUPMASTER ";

                dt = new DataTable();
                dt = baseObj.GetData(sqlstr);
                chkListCustomerGroup.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    chkListCustomerGroup.DataSource = dt;
                    chkListCustomerGroup.DataTextField = "NAME";
                    chkListCustomerGroup.DataValueField = "Code";
                    chkListCustomerGroup.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void chkListState_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSite();
        }
        protected void chkListSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerGroup();
        }
        protected void chkListCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomerName();
        }
        private void FillCustomerName()
        {
            try
            {
                string strCustomerGroupName = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerGroupName == "")
                        {
                            strCustomerGroupName += "'" + item.Value.ToString() + "'";
                        }
                        else
                        {
                            strCustomerGroupName += ",'" + item.Value.ToString() + "'";
                        }
                    }
                }
                if (strCustomerGroupName.Length > 0)
                {
                    DataTable dt = new DataTable();
                    string sqlstr = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from VW_CUSTOMERMASTER_HISTORY(NOLOCK) where Blocked = 0 AND CUST_GROUP in (" + strCustomerGroupName + ") and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "'"
                    + " UNION "
                    + "SELECT SUBDISTRIBUTOR + '-' + NAME AS NAME , SUBDISTRIBUTOR AS Customer_Code from ax.ACX_SDLINKING where Blocked = 0 AND CUSTGROUP in (" + strCustomerGroupName + ") and OTHER_SITE = '" + Session["SiteCode"].ToString() + "' and Dataareaid = '" + Session["DATAAREAID"].ToString() + "' ORDER BY NAME";

                    dt = new DataTable();
                    dt = baseObj.GetData(sqlstr);
                    chkCustomerName.Items.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        chkCustomerName.DataSource = dt;
                        chkCustomerName.DataTextField = "NAME";
                        chkCustomerName.DataValueField = "Customer_Code";
                        chkCustomerName.DataBind();
                    }
                }
                else
                {
                    chkCustomerName.Items.Clear();
                    chkCustomerName.Items.Add("--Select--");
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void BtnShowReport0_Click(object sender, EventArgs e)
        {
            CreamBell_DMS_WebApps.App_Code.Global objGlobal = new CreamBell_DMS_WebApps.App_Code.Global();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataTable dtDataByfilter = null;
            string query = string.Empty;
            try
            {
                conn = new SqlConnection(objGlobal.GetConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                query = "usp_GetCustomerOutstandingForWirteoff";
                cmd.CommandText = query;
                string strStateCode = "";
                foreach (ListItem item in chkListState.Items)
                {
                    if (item.Selected)
                    {
                        if (strStateCode == "")
                        {
                            strStateCode += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            strStateCode += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (strStateCode.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@STATECODE", strStateCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@STATECODE", "");
                }


                string strSiteCode = "";
                foreach (ListItem item in chkListSite.Items)
                {
                    if (item.Selected)
                    {
                        if (strSiteCode == "")
                        {
                            strSiteCode += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            strSiteCode += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (strSiteCode.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@SiteCode", strSiteCode);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SiteCode", "");
                }

                string strCustomerGroup = "";
                foreach (ListItem item in chkListCustomerGroup.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerGroup == "")
                        {
                            strCustomerGroup += "" + item.Value.ToString() + "";
                        }
                        else
                        {
                            strCustomerGroup += "," + item.Value.ToString() + "";
                        }
                    }
                }
                if (strCustomerGroup.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Cust_Group", strCustomerGroup);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Cust_Group", "");
                }

                string strCustomerName = "";
                foreach (ListItem item in chkCustomerName.Items)
                {
                    if (item.Selected)
                    {
                        if (strCustomerName == "")
                        {
                            strCustomerName = item.Value.ToString();
                        }
                        else
                        {
                            strCustomerName += "," + item.Value.ToString() + "";
                        }
                    }
                }

                if (strCustomerName.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@Customer_Code", strCustomerName);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Customer_Code", "");
                }
                dtDataByfilter = new DataTable();
                dtDataByfilter.Load(cmd.ExecuteReader());
                lblShowTotalOutStaning.Text = string.Empty;
                if (dtDataByfilter != null && dtDataByfilter.Rows.Count > 0)
                {
                    object sumObject;
                    sumObject = dtDataByfilter.Compute("Sum(RemainingAmount)", "");
                    lblShowTotalOutStaning.Text = Convert.ToString(sumObject);
                    gvDetails.DataSource = dtDataByfilter;
                    gvDetails.DataBind();
                    uppanel.Update();
                }
                else
                {
                    gvDetails.DataSource = dtDataByfilter;
                    gvDetails.DataBind();
                }

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

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
                e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
            }
        }

        #region|SAVE WRITE OFF|
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDetails.Rows.Count > 0)
                {
                    Button btn = sender as Button;
                    GridViewRow row = btn.NamingContainer as GridViewRow;
                    // string pk = gvDetails.DataKeys[row.RowIndex].Values[0].ToString();
                    // string strInvoiceNo = string.Empty;                  

                    foreach (GridViewRow gv in gvDetails.Rows)
                    {
                        DataTable dt = new DataTable();
                        //============Generate Doc Number=============
                        query = "SELECT ISNULL(MAX(CAST(RIGHT([Document_No],6) AS INT)),0)+1 as [Document_No] FROM [ax].[ACXCOLLECTIONENTRY] where [SiteCode]='" + Session["SiteCode"].ToString() + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'";
                        dt = baseObj.GetData(query);
                        int vc = Convert.ToInt32(dt.Rows[0]["Document_No"].ToString());
                        DataTable dtNumSeq = baseObj.GetNumSequenceNew(8, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                        string NUMSEQ = string.Empty;
                        string DocumentNo = string.Empty;
                        if (dtNumSeq != null)
                        {
                            DocumentNo = dtNumSeq.Rows[0][0].ToString();
                            NUMSEQ = dtNumSeq.Rows[0][1].ToString();
                        }
                        else
                        {
                            return;
                        }
                        conn = baseObj.GetConnection();
                        cmd = new SqlCommand("ACX_CollectionEntry");
                        transaction = conn.BeginTransaction();
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandTimeout = 3600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd1 = new SqlCommand("ACX_CollectionEntry");
                        cmd1.Connection = conn;
                        cmd1.Transaction = transaction;
                        cmd1.CommandTimeout = 3600;
                        cmd1.CommandType = CommandType.StoredProcedure;
                        decimal amount = 0;
                        CheckBox chkbox = (CheckBox)gv.FindControl("chkInvoice");
                        HiddenField strInvoice_No = (HiddenField)gv.FindControl("hdInvoice_No");
                        if (chkbox.Checked == true)
                        {
                            try
                            {
                                Label lblRemainingAmount = (Label)gv.FindControl("lblRemainingAmount");
                                Label lblCUSTOMER_CODE = (Label)gv.FindControl("lblCUSTOMER_CODE");
                                Label lblDocument_Date = (Label)gv.FindControl("lblDocument_Date");
                                amount = Convert.ToDecimal(lblRemainingAmount.Text.Trim());
                                if (amount > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                                    cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                                    cmd.Parameters.AddWithValue("@Customer_Code", lblCUSTOMER_CODE.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Document_No", DocumentNo);
                                    cmd.Parameters.AddWithValue("@Instrument_Code", "CASH");
                                    cmd.Parameters.AddWithValue("@Instrument_No", "");
                                    cmd.Parameters.AddWithValue("@Ref_DocumentNo", strInvoice_No.Value);
                                    cmd.Parameters.AddWithValue("@Ref_DocumentDate", Convert.ToDateTime(lblDocument_Date.Text));
                                    cmd.Parameters.AddWithValue("@Collection_Amount", amount);
                                    cmd.Parameters.AddWithValue("@Collection_Date", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@Status", "INSERT");
                                    cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                                    cmd.Parameters.AddWithValue("@Remark", "WRITE OFF");

                                    cmd.ExecuteNonQuery();

                                    //===============Update Customer Ledger/Transaction Table===============
                                    cmd1.Parameters.Clear();
                                    cmd1.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                                    cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                                    cmd1.Parameters.AddWithValue("@Customer_Code", lblCUSTOMER_CODE.Text.Trim());
                                    cmd1.Parameters.AddWithValue("@Document_No", strInvoice_No.Value);
                                    cmd1.Parameters.AddWithValue("@RemainingAmount", 0);
                                    cmd1.Parameters.AddWithValue("@Status", "UPDATE");
                                    cmd1.Parameters.AddWithValue("@Remark", "WRITE OFF");

                                    cmd1.ExecuteNonQuery();

                                    //obj.UpdateLastNumSequence(8, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);

                                    //============Commit All Data================
                                    transaction.Commit();
                                }
                                else
                                {

                                }
                                // 
                            }
                            catch(Exception ex)
                            {
                                transaction.Rollback();
                                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Try Again!!');", true);
                                LblMessage.Text = "'Please Try Again!! !";
                                LblMessage.Visible = true;
                                uppanel.Update();
                                ErrorSignal.FromCurrentContext().Raise(ex);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }
                    BtnShowReport0_Click(null, null);
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Generated Successfully.!');", true);

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #endregion


    }
}