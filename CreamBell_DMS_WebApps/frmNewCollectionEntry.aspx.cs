using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class frmNewCollectionEntry : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        public DataTable dt;
        public DataTable newdt;
        public DataTable Completedata;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;
        string strCustomerGroupName = "";
        string block = "";

        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string query = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindGridview();
                fillCustomerGroup();
                string FromDate = txtCollectionDate.Text;
                BindGridViewInstrument();                
                //rdActCust_CheckedChanged(null,null);
                txtCollectionDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();
                newdt = null;
                Completedata = null;
                Session["AllCusData"] = null;
                //CalendarExtender1.StartDate = DateTime.Now;
            }
        }

        public void fillCustomerGroup()
        {
            try
            {
                query = "SELECT CUSTGROUP_CODE+'-'+CUSTGROUP_NAME AS Name,CUSTGROUP_CODE FROM AX.ACXCUSTGROUPMASTER WHERE DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' AND  Blocked = 0";
                drpCustomerGroup.Items.Clear();
                ListItem lst = new ListItem("All...", "");
                drpCustomerGroup.Items.Insert(0, lst);
                baseObj.BindToDropDown(drpCustomerGroup, query, "Name", "CUSTGROUP_CODE");
                drpCustomerName.Items.Add("All...");
                drpCustomerGroup_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                LblMessage.Text = "'Please Try Again!! !";
                LblMessage.Visible = true;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void drpCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string block = "";
                if (rdActCust.Checked == true)
                {
                    block = "0";
                }

                else if (rdAllCust.Checked == true)
                {
                    block = "0,1";
                }

                if (rdActCust.Checked == true && drpCustomerGroup.SelectedIndex != 0)
                {
                    query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "' AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' AND BLOCKED ='" + block + "' union " +
                         "SELECT C.[CUSTOMER_CODE], C.[CUSTOMER_CODE] + ' - ' + ISNULL(C.[CUSTOMER_NAME], '') AS Customer FROM AX.ACXCUSTMASTER C JOIN AX.ACX_SDLINKING SD ON SD.SUBDISTRIBUTOR = C.CUSTOMER_CODE AND SD.CUSTGROUP='" + drpCustomerGroup.SelectedValue + "' WHERE SD.OTHER_SITE='" + Session["SiteCode"].ToString() + "'";
                    drpCustomerName.Items.Clear();
                    drpCustomerName.Items.Add("All...");
                    baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
                    //BindGridview();
                    //query = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
                }

                if (rdActCust.Checked == true && drpCustomerGroup.SelectedIndex == 0)
                {
                    query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "' AND BLOCKED ='" + block + "' UNION " +
                        "SELECT C.[CUSTOMER_CODE], C.[CUSTOMER_CODE] + ' - ' + ISNULL(C.[CUSTOMER_NAME], '') AS Customer FROM AX.ACXCUSTMASTER C JOIN AX.ACX_SDLINKING SD ON SD.SUBDISTRIBUTOR = C.CUSTOMER_CODE WHERE SD.OTHER_SITE='" + Session["SiteCode"].ToString() + "'";
                    drpCustomerName.Items.Clear();
                    drpCustomerName.Items.Add("All...");
                    baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
                    //BindGridview();
                    //query = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
                }
                if (rdAllCust.Checked == true && drpCustomerGroup.SelectedIndex != 0)
                {
                    //block = "1";
                    query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "' AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' AND BLOCKED IN (" + block + ")  UNION " +
                         "SELECT C.[CUSTOMER_CODE], C.[CUSTOMER_CODE] + ' - ' + ISNULL(C.[CUSTOMER_NAME], '') AS Customer FROM AX.ACXCUSTMASTER C JOIN AX.ACX_SDLINKING SD ON SD.SUBDISTRIBUTOR = C.CUSTOMER_CODE AND SD.CUSTGROUP='" + drpCustomerGroup.SelectedValue + "' WHERE SD.OTHER_SITE='" + Session["SiteCode"].ToString() + "'";
                    drpCustomerName.Items.Clear();
                    drpCustomerName.Items.Add("All...");
                    baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
                }
                if (rdAllCust.Checked == true && drpCustomerGroup.SelectedIndex == 0)
                {
                    //block = "1";
                    query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "'  AND BLOCKED IN (" + block + ") UNION " +
                         "SELECT C.[CUSTOMER_CODE], C.[CUSTOMER_CODE] + ' - ' + ISNULL(C.[CUSTOMER_NAME], '') AS Customer FROM AX.ACXCUSTMASTER C JOIN AX.ACX_SDLINKING SD ON SD.SUBDISTRIBUTOR = C.CUSTOMER_CODE WHERE SD.OTHER_SITE='" + Session["SiteCode"].ToString() + "'";
                    drpCustomerName.Items.Clear();
                    drpCustomerName.Items.Add("All...");
                    baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = "'Please Try Again!! !";
                LblMessage.Visible = true;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        //protected void drpCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string block = "";
        //    if (rdActCust.Checked == true)
        //    {
        //        block = "0";                
        //    }

        //    else if (rdAllCust.Checked == true)
        //    {
        //        block = "0,1";
        //    }

        //    if (rdActCust.Checked == true && drpCustomerGroup.SelectedIndex != 0)
        //    {
        //        query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "' AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' AND BLOCKED ='" + block + "' ";
        //        drpCustomerName.Items.Clear();
        //        drpCustomerName.Items.Add("All...");
        //        baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
        //        //BindGridview();
        //        //query = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
        //    }

        //    if (rdActCust.Checked == true && drpCustomerGroup.SelectedIndex == 0)
        //    {
        //        query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "'  AND BLOCKED ='" + block + "' ";
        //        drpCustomerName.Items.Clear();
        //        drpCustomerName.Items.Add("All...");
        //        baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
        //        //BindGridview();
        //        //query = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
        //    }
        //    if(rdAllCust.Checked == true && drpCustomerGroup.SelectedIndex != 0)
        //    {
        //        //block = "1";
        //        query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "' AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' AND BLOCKED IN (" + block + ")";
        //        drpCustomerName.Items.Clear();
        //        drpCustomerName.Items.Add("All...");
        //        baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
        //    }
        //    if (rdAllCust.Checked == true && drpCustomerGroup.SelectedIndex == 0)
        //    {
        //        //block = "1";
        //        query = "SELECT CUSTOMER_CODE,CUSTOMER_CODE + ' - ' + CUSTOMER_NAME AS CUSTOMER  FROM VW_CUSTOMERMASTER_HISTORY WHERE SITE_CODE ='" + Session["SiteCode"].ToString() + "' AND BLOCKED IN (" + block + ")";
        //        drpCustomerName.Items.Clear();
        //        drpCustomerName.Items.Add("All...");
        //        baseObj.BindToDropDown(drpCustomerName, query, "CUSTOMER", "CUSTOMER_CODE");
        //    }
        //}
        protected void BindGridview()
        {
            try
            {
                string condition = string.Empty;
                string FromDate = txtCollectionDate.Text;
                string status = "";

                string block = "";
                if (rdActCust.Checked == true)
                {
                    block = "0";
                }

                else if (rdAllCust.Checked == true)
                {
                    block = "1";
                }
                if (rdPenInv.Checked == true)
                {
                    status = "0";
                }
                else if (rdWitInv.Checked == true)
                {
                    status = "1";
                }

                if (drpCustomerName.SelectedIndex != 0)
                {
                    query = "EXEC ACX_GETCOLLECTIONENTRYREPORT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "','" + status + "','" + block + "'";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }
                else if (drpCustomerGroup.SelectedIndex != 0 && drpCustomerName.SelectedIndex != 0)
                {
                    query = "EXEC ACX_GETCOLLECTIONENTRYREPORT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "','" + status + "','" + block + "'";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }
                else
                {
                    query = "EXEC ACX_GETCOLLECTIONENTRYREPORT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','','" + status + "',''";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }
                if (dt.Rows.Count > 0)
                {
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('No Data Exist !! ');", true);
                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = "'Please Try Again!! !";
                LblMessage.Visible = true;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        //protected void BindGridview()
        //{
        //    try
        //    {                
        //        string condition = string.Empty;
        //        string FromDate = txtCollectionDate.Text;
        //        string status = "";

        //        string block = "";
        //        if (rdActCust.Checked == true)
        //        {
        //            block = "0";
        //        }

        //        else if (rdAllCust.Checked == true)
        //        {
        //            block = "1";
        //        }
        //        if (rdPenInv.Checked == true)
        //        {
        //            status = "0";
        //        }
        //        else if (rdWitInv.Checked == true)
        //        {
        //            status = "1";
        //        }

        //        if (drpCustomerGroup.SelectedIndex != 0 && drpCustomerName.SelectedIndex != 0)
        //        {                    
        //            query = "EXEC ACX_GETCOLLECTIONENTRYREPORT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "','" + status + "','" + block + "'";
        //            dt = new DataTable();
        //            dt = baseObj.GetData(query);                    
        //        }
        //        else
        //        {                    
        //            query = "EXEC ACX_GETCOLLECTIONENTRYREPORT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','','" + status+ "','" + block + "'";
        //            dt = new DataTable();
        //            dt = baseObj.GetData(query);
        //        }
        //        if (dt.Rows.Count > 0)
        //        {
        //            gvDetails.DataSource = dt;
        //            gvDetails.DataBind();
        //        }
        //        else
        //        {
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('No Data Exist !! ');", true);
        //            gvDetails.DataSource = dt;
        //            gvDetails.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = "'Please Try Again!! !";
        //        LblMessage.Visible = true;                
        //    }
        //}

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            this.BindGridview();            
            chkPenAmt.Checked = false;                       
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string FromDate = txtCollectionDate.Text;
                //============ Fill Data to dropdown of Gridview=================== 
              
                DropDownList ddList = (DropDownList)e.Row.FindControl("drpInstrument");
                DataTable dt = new DataTable();
                List<string> ilist = new List<string>();
                List<string> litem = new List<string>();
                string query;

                //==========Instrument
                if (newdt == null)
                {
                    query = "SELECT [Instrument_Code],[Instrument_Description] FROM AX.ACXINSTRUMENTMASTER WHERE DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' AND  Blocked = 0 ORDER BY [Instrument_Description]";

                    newdt = baseObj.GetData_New(query, CommandType.Text, ilist, litem);
                }
               
                ddList.DataSource = newdt;
                ddList.DataTextField = "Instrument_Description";
                ddList.DataValueField = "Instrument_Code";
                ddList.DataBind();
                ddList.Items.Insert(0, new ListItem("--Select--", "0"));


                //==========Ref Doc Number
                Label RefDocNo = (Label)e.Row.FindControl("lblRefDocNo");
                Label RefDate = (Label)e.Row.FindControl("lblRefDocDate");
                Label PendingAmount = (Label)e.Row.FindControl("lblPendingAmount");
                HiddenField customercode = (HiddenField)e.Row.FindControl("HiddenField2");
                DropDownList ddRefNo = (DropDownList)e.Row.FindControl("drpRefDocument");
                dt = new DataTable();                              
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {                
                bool b = Validation();

                if (b == true)
                {
                    SaveDetails();
                    BindGridViewInstrument();                   
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool Validation()
        {
            bool b = true;
            try
            {
                LblMessage.Text = string.Empty;
                LblMessage.Visible = false;
                //if (drpCustomerGroup.Text == "All..." && drpCustomerName.Text == "All...")
                //{
                //    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer Group !');", true);
                //    LblMessage.Text = "Select Customer Group ";
                //    LblMessage.Visible = true;
                //    uppanel.Update();
                //    drpCustomerGroup.Focus();
                //    b = false;
                //    return b;
                //}
                int cnt = 0;
                foreach (GridViewRow gv in gvDetails.Rows)
                {                   
                    TextBox Amount = (TextBox)gv.Cells[8].FindControl("txtAmount");
                    DropDownList InstrumentCode = (DropDownList)gv.Cells[3].FindControl("drpInstrument");                    
                    if (Amount.Text == "")
                    {
                        cnt++;                     
                    }                                                        
                }                
                if (gvDetails.Rows.Count == cnt)
                {
                    b = false;
                    LblMessage.Text = "Collection amount cannot be left blank !";
                    LblMessage.Visible = true;

                    return b;
                }                
                if (txtCollectionDate.Text == string.Empty)
                {
                    b = false;
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Collection Date cannot be left blank !');", true);
                    LblMessage.Text = "Collection Date cannot be left blank !";
                    LblMessage.Visible = true;
                    uppanel.Update();
                    return b;
                }
                //==========For Grivew Detail===============
                dt = new DataTable();
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    DropDownList InstrumentCode = (DropDownList)gv.Cells[3].FindControl("drpInstrument");
                    TextBox InstrumentNo = (TextBox)gv.Cells[4].FindControl("txtInstrument");
                    Label RefDocument_No = (Label)gv.Cells[5].FindControl("lblRefDocNo");
                    Label RefDocument_Date = (Label)gv.Cells[6].FindControl("lblRefDocDate");
                    Label PendingAmount = (Label)gv.Cells[7].FindControl("lblPendingAmount");
                    TextBox Amount = (TextBox)gv.Cells[8].FindControl("txtAmount");
                    decimal amount;
                    if (Amount.Text == string.Empty)
                    {
                        amount = 0;
                    }
                    else
                    {
                        amount = Convert.ToDecimal(Amount.Text);
                    }

                    if (amount > 0)
                    {
                        if (InstrumentCode.SelectedIndex == 0)
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Instrument !');", true);
                            LblMessage.Text = "Please Select Instrument !";
                            LblMessage.Visible = true;
                            uppanel.Update();
                            InstrumentCode.Focus();
                            b = false;
                            return b;
                        }

                        else
                        {
                            LblMessage.Visible = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return b;
        }

        public void SaveDetails()
        {
            try
            {
                dt = new DataTable();

                //============Generate Doc Number=============
                query = "SELECT ISNULL(MAX(CAST(RIGHT([Document_No],6) AS INT)),0)+1 as [Document_No] FROM [ax].[ACXCOLLECTIONENTRY] where [SiteCode]='" + Session["SiteCode"].ToString() + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "'";
                dt = baseObj.GetData(query);
                int vc = Convert.ToInt32(dt.Rows[0]["Document_No"].ToString());

                //string DocumentNo = obj.GetNumSequence(8, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());

                DataTable dtNumSeq = obj.GetNumSequenceNew(8, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
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
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd1 = new SqlCommand("ACX_CollectionEntry");
                cmd1.Connection = conn;
                cmd1.Transaction = transaction;
                cmd1.CommandTimeout = 0;
                cmd1.CommandType = CommandType.StoredProcedure;

                //======Save CollectionEntry===================
                //cmd.CommandText = string.Empty;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd = new SqlCommand("ACX_CollectionEntry");

                int i = 0;
                foreach (GridViewRow gv in gvDetails.Rows)
                {
                    i = i + 1;

                    HiddenField Customercode = (HiddenField)gv.Cells[0].FindControl("HiddenField2");
                    DropDownList InstrumentCode = (DropDownList)gv.Cells[3].FindControl("drpInstrument");
                    TextBox InstrumentNo = (TextBox)gv.Cells[4].FindControl("txtInstrument");
                    Label RefDocument_No = (Label)gv.Cells[5].FindControl("lblRefDocNo");
                    Label RefDocument_Date = (Label)gv.Cells[6].FindControl("lblRefDocDate");
                    Label PendingAmount = (Label)gv.Cells[7].FindControl("lblPendingAmount");
                    TextBox Amount = (TextBox)gv.Cells[8].FindControl("txtAmount");
                    //TextBox Remark = (TextBox)gv.Cells[9].FindControl("txtRemark");
                    TextBox Remark = gv.FindControl("txtRemark") as TextBox;

                    decimal amount, pendingamount;
                    if (Amount.Text == string.Empty)
                    {
                        amount = 0;
                    }
                    else
                    {
                        amount = Convert.ToDecimal(Amount.Text);
                    }
                    if (PendingAmount.Text == string.Empty)
                    {
                        pendingamount = 0;
                    }
                    else
                    {
                        pendingamount = Convert.ToDecimal(PendingAmount.Text);
                    }

                    if (amount != 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                        cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd.Parameters.AddWithValue("@Customer_Code", Customercode.Value);
                        cmd.Parameters.AddWithValue("@Document_No", DocumentNo);
                        cmd.Parameters.AddWithValue("@Instrument_Code", InstrumentCode.SelectedItem.Value);
                        cmd.Parameters.AddWithValue("@Instrument_No", InstrumentNo.Text);
                        cmd.Parameters.AddWithValue("@Ref_DocumentNo", RefDocument_No.Text);
                        cmd.Parameters.AddWithValue("@Ref_DocumentDate", RefDocument_Date.Text);
                        cmd.Parameters.AddWithValue("@Collection_Amount", amount);
                        cmd.Parameters.AddWithValue("@Collection_Date", txtCollectionDate.Text);
                        cmd.Parameters.AddWithValue("@Status", "INSERT");
                        cmd.Parameters.AddWithValue("@NUMSEQ", NUMSEQ);
                        cmd.Parameters.AddWithValue("@Remark", Remark.Text.Trim());

                        cmd.ExecuteNonQuery();

                        //===============Update Customer Ledger/Transaction Table===============
                        cmd1.Parameters.Clear();
                        cmd1.Parameters.AddWithValue("@SiteCode", Session["SiteCode"].ToString());
                        cmd1.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                        cmd1.Parameters.AddWithValue("@Customer_Code", Customercode.Value);
                        cmd1.Parameters.AddWithValue("@Document_No", RefDocument_No.Text);
                        cmd1.Parameters.AddWithValue("@RemainingAmount", pendingamount - amount);
                        cmd1.Parameters.AddWithValue("@Status", "UPDATE");
                        cmd1.Parameters.AddWithValue("@Remark", Remark.Text.Trim());

                        cmd1.ExecuteNonQuery();
                        Amount.Text = "";
                        InstrumentCode.SelectedIndex = 0;

                        //obj.UpdateLastNumSequence(8, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString(), conn, transaction);

                        //============Commit All Data================                                               
                    }
                    else
                    {
                    }
                }
                transaction.Commit();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Collection Entry Document No : " + DocumentNo + " Generated Successfully.!');", true);
                BindGridview();
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

        protected void BindGridViewInstrument()
        {
            try
            {
                string FromDate = txtCollectionDate.Text;
                string condition = string.Empty;

                if (drpCustomerGroup.SelectedIndex != 0 && drpCustomerName.SelectedIndex != 0)
                {
                    query = "EXEC USP_GETCOLLECTIONINSTRUMRNT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedValue + "','" + FromDate + "'";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }
                //else if(drpCustomerGroup.SelectedIndex == 0 && drpCustomerName.SelectedIndex == 0)
                //{
                //    query = "EXEC USP_GETCOLLECTIONINSTRUMRNT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','','" + FromDate + "'";
                //    dt = new DataTable();
                //    dt = baseObj.GetData(query);
                //}
                else
                {
                    query = "EXEC USP_GETCOLLECTIONINSTRUMRNT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','','" + string.Format("{0:dd/MMM/yyyy }", DateTime.Today) + "'";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }

                //query = "EXEC USP_GETCOLLECTIONINSTRUMRNT '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "','" + FromDate + "' ";
                //query = "EXEC USP_GETCOLLECTIONDATA '" + Session["SiteCode"].ToString() + "','" + txtCollectionDate.Text + "'";

                dt = new DataTable();

                dt = baseObj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    uppanel.Update();                   
                    //GridViewFooterCalculate(dt);
                }
                else
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }           
        }                   

        protected void BindGridViewCollectionData()
        {
            try
            {
                string FromDate = txtCollectionDate.Text;
                string condition = string.Empty;

                if (drpCustomerGroup.SelectedIndex != 0 && drpCustomerName.SelectedIndex != 0)
                {
                    query = "EXEC [dbo].[ACX_GETCOLLECTIONENTRYNEWREPORT] '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "','" + FromDate + "'";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }
                else
                {
                    query = "EXEC [dbo].[ACX_GETCOLLECTIONENTRYNEWREPORT] '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','','" + FromDate + "'";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }

                //query = "EXEC [dbo].[ACX_GETCOLLECTIONENTRYNEWREPORT] '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "','" + FromDate + "' ";
                //query = "EXEC USP_GETCOLLECTIONDATA '" + Session["SiteCode"].ToString() + "','" + txtCollectionDate.Text + "'";

                dt = new DataTable();

                dt = baseObj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    gridCollectionData.DataSource = dt;
                    gridCollectionData.DataBind();
                    GridViewFooterCalculate(dt);
                }
                else
                {
                    gridCollectionData.DataSource = dt;
                    gridCollectionData.DataBind();
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }            
       }

        private void GridViewFooterCalculate(DataTable dt)
        {
            //decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));          //For Total[Sum] Value Show in Footer--//
            decimal CollectionAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("Collection_Amount"));

            //===============11-5-2016=====
            gridCollectionData.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            gridCollectionData.FooterRow.Cells[5].ForeColor = System.Drawing.Color.MidnightBlue;
            gridCollectionData.FooterRow.Cells[5].Text = "TOTAL";
            gridCollectionData.FooterRow.Cells[5].Font.Bold = true;

            gridCollectionData.FooterRow.Visible = true;
            gridCollectionData.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            gridCollectionData.FooterRow.Cells[6].ForeColor = System.Drawing.Color.MidnightBlue;
            gridCollectionData.FooterRow.Cells[6].Text = CollectionAmt.ToString("N2");
            gridCollectionData.FooterRow.Cells[6].Font.Bold = true;
        }

        protected void rdActCust_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if(rdo.ID == "rdActCust")
            {
                fillCustomerGroup();
                drpCustomerName.Items.Clear();
                drpCustomerGroup_SelectedIndexChanged(null, null);
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gridCollectionData.DataSource = null;
                gridCollectionData.DataBind();                              
            }
            else 
            {
                fillCustomerGroup();
                drpCustomerGroup_SelectedIndexChanged(null, null);
                //drpCustomerName.Items.Add("All...");                           
            }
            if(rdo.ID == "rdAllCust")
            {
                fillCustomerGroup();                
                drpCustomerName.Items.Clear();
                drpCustomerName.Items.Add("All...");
                drpCustomerGroup_SelectedIndexChanged(null, null);
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gridCollectionData.DataSource = null;
                gridCollectionData.DataBind();                
            }
            else
            {
                fillCustomerGroup();
                drpCustomerGroup_SelectedIndexChanged(null, null);
                //drpCustomerName.Items.Add("All...");                
            }
        }

        protected void rdPenInv_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
            if(rdo.ID == "rdPenInv")
            {
                fillCustomerGroup();
                drpCustomerName.Items.Clear();
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gridCollectionData.DataSource = null;
                gridCollectionData.DataBind();             
            }
            else
            {
                fillCustomerGroup();
                //drpCustomerName.Items.Add("All...");
                drpCustomerGroup_SelectedIndexChanged(null, null);
            }
            if(rdo.ID == "rdWitInv")
            {
                drpCustomerGroup_SelectedIndexChanged(null, null);
                //drpCustomerName.Items.Clear();
                //drpCustomerName.Items.Add("All...");
                gvDetails.DataSource = null;
                gvDetails.DataBind();
                gridCollectionData.DataSource = null;
                gridCollectionData.DataBind();          
            }
            else
            {
                fillCustomerGroup();
                drpCustomerGroup_SelectedIndexChanged(null, null);
            }
        }

        protected void chkPenAmt_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkBx = (CheckBox)sender;
                if (chkBx.Checked)
                {                    
                    foreach (GridViewRow gv in gvDetails.Rows)
                    {                        
                        DropDownList InstrumentCode = (DropDownList)gv.Cells[3].FindControl("drpInstrument");
                        Label PendingAmount = (Label)gv.Cells[7].FindControl("lblPendingAmount");
                        TextBox Amount = (TextBox)gv.Cells[8].FindControl("txtAmount");
                        Amount.Text = PendingAmount.Text.ToString();
                        InstrumentCode.SelectedIndex = 1;                                                                                                                   
                    }
                }                
                else
                {
                    foreach (GridViewRow gv in gvDetails.Rows)
                    {
                        DropDownList InstrumentCode = (DropDownList)gv.Cells[3].FindControl("drpInstrument");
                        TextBox Amount = (TextBox)gv.Cells[8].FindControl("txtAmount");
                        Amount.Text = "";
                        InstrumentCode.SelectedIndex = 0;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        
        protected void tnShow_Click(object sender, EventArgs e)
        {
            BindGridview();
            BindGridViewInstrument();
            BindGridViewCollectionData();
            uppanel.Update();
            chkPenAmt.Checked = false;
        }
    }
}