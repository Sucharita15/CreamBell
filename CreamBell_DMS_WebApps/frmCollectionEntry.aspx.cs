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
    public partial class frmCollectionEntry : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();
        public DataTable dt;
        public DataTable newdt;
        public DataTable Completedata;
        SqlConnection conn = null;
        SqlCommand cmd, cmd1;
        SqlTransaction transaction;

        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string query = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillCustomerGroup();
                txtCollectionDate.Text = string.Format("{0:dd/MMM/yyyy }", DateTime.Today);// DateTime.Today.ToString();
                newdt = null;
                Completedata = null;
                Session["AllCusData"] = null;
                // CalendarExtender1.StartDate = DateTime.Now;
            }
        }

        public void fillCustomerGroup()
        {
            try
            {
                query = "Select CUSTGROUP_CODE+'-'+CUSTGROUP_NAME as Name,CUSTGROUP_CODE from ax.ACXCUSTGROUPMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0";
                drpCustomerGroup.Items.Clear();
                ListItem lst = new ListItem("All...", "");
                drpCustomerGroup.Items.Insert(0, lst);
                baseObj.BindToDropDown(drpCustomerGroup, query, "Name", "CUSTGROUP_CODE");
                drpCustomerGroup_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void drpCustomerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //BindGridview();
                //query = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + drpCustomerGroup.SelectedValue + "' and Site_Code='" + Session["SiteCode"].ToString() + "' and Dataareaid='" + Session["DATAAREAID"].ToString() + "' order by Name";
                query = "EXEC Acx_GetCustomerDetailsWithSDLinking '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','' ";
                drpCustomerName.Items.Clear();
                drpCustomerName.Items.Add("--Select--");
                baseObj.BindToDropDown(drpCustomerName, query, "Customer", "Customer_Code");
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void BindGridview()
        {

            try
            {
                string condition = string.Empty;
                if (drpCustomerGroup.SelectedItem.Value == "--Select--")
                {
                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer Group');", true);
                    LblMessage.Text = "Select Customer Group !";
                    LblMessage.Visible = true;
                    uppanel.Update();
                    return;
                }
                //if (drpCustomerName.SelectedItem.Value != "--Select--")
                //{
                //    condition = " And A.[CUSTOMER_CODE]='" + drpCustomerName.SelectedItem.Value + "'";
                //}
                //query = "select A.[CUSTOMER_CODE],A.[CUSTOMER_CODE]+'-'+A.[CUSTOMER_NAME] as Customer" +
                //    " ,A.[ADDRESS1]+','+A.[ADDRESS2]+','+A.[CITY]+','+A.[AREA]+','+A.[DISTRICT]+','+A.[STATE]+','+A.[ZIPCODE] as Address " +
                //    " from  [ax].[ACXCUSTMASTER] A " +
                //    " where A.[SITE_CODE]='" + Session["SiteCode"].ToString() + "' and A.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' and A.Blocked=0 and A.[CUST_GROUP]='" + drpCustomerGroup.SelectedItem.Value + "' " + condition +
                //    " order by Customer";

                if (drpCustomerName.SelectedItem.Value != "--Select--")
                {
                    query = "EXEC Acx_GetCustomerDetailsWithSDLinking '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "' ";
                    dt = new DataTable();
                    dt = baseObj.GetData(query);
                }
                else
                {
                    query = "EXEC Acx_GetCustomerDetailsWithSDLinking '" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','' ";
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
                    gvDetails.DataSource = null;
                    gvDetails.DataBind();
                }

            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            gvDetails.PageIndex = e.NewPageIndex;
            this.BindGridview();
        }

        protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //============ Fill Data to dropdown of Gridview===================
                    DropDownList ddList = (DropDownList)e.Row.FindControl("drpInstrument");
                    DataTable dt = new DataTable();
                    List<string> ilist = new List<string>();
                    List<string> litem = new List<string>();
                    string query;

                    //==========Instrument
                    if (newdt == null)
                    {

                        query = "Select [Instrument_Code],[Instrument_Description] from ax.ACXINSTRUMENTMASTER where DATAAREAID ='" + Session["DATAAREAID"].ToString() + "' and  Blocked = 0 order by [Instrument_Description]";

                        newdt = baseObj.GetData_New(query, CommandType.Text, ilist, litem);
                    }

                    ddList.DataSource = newdt;
                    ddList.DataTextField = "Instrument_Description";
                    ddList.DataValueField = "Instrument_Code";
                    ddList.DataBind();
                    ddList.Items.Insert(0, new ListItem("--Select--", "0"));

                    //==========Ref Doc Number
                    HiddenField customercode = (HiddenField)e.Row.FindControl("HiddenField2");
                    DropDownList ddRefNo = (DropDownList)e.Row.FindControl("drpRefDocument");
                    dt = new DataTable();
                    //query = "select distinct ax.ACXCUSTTRANS.[Document_No] from ax.ACXCUSTTRANS INNER JOIN  ax.ACXCUSTTRANS ACXCUSTTRANS_1 ON ax.ACXCUSTTRANS.Document_No <> ACXCUSTTRANS_1.RefNo_DocumentNo " +
                    //        " where ax.ACXCUSTTRANS.RemainingAmount<>0 and  ax.ACXCUSTTRANS.DocumentType<>2 and ax.ACXCUSTTRANS.[Customer_Code]='" + customercode.Value + "' and ax.ACXCUSTTRANS.[SiteCode]='" + Session["SiteCode"].ToString() + "' and ax.ACXCUSTTRANS.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' " +
                    //        " order by ax.ACXCUSTTRANS.[Document_No] desc ";

                    //query = " select ax.ACXCUSTTRANS.[Document_No] from ax.ACXCUSTTRANS where ax.ACXCUSTTRANS.RemainingAmount>0 And AMOUNT>=REMAININGAMOUNT  " +
                    //       // " ax.ACXCUSTTRANS.RefNo_DocumentNo is null and ax.ACXCUSTTRANS.[Document_No] not in (select ax.ACXCUSTTRANS.RefNo_DocumentNo from ax.ACXCUSTTRANS where ax.ACXCUSTTRANS.RefNo_DocumentNo is not null) " +
                    //        " and ax.ACXCUSTTRANS.[Customer_Code]='" + customercode.Value + "' and ax.ACXCUSTTRANS.[SiteCode]='" + Session["SiteCode"].ToString() + "' and ax.ACXCUSTTRANS.[DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' " +
                    //        "  and ax.ACXCUSTTRANS.[Document_No] not in (select SO_NO from ax.ACXSALEINVOICEheader where trantype=2 and ax.ACXCUSTTRANS.[Document_No]=ax.ACXSALEINVOICEheader.SO_NO and  ax.ACXCUSTTRANS.[SiteCode]=ax.ACXSALEINVOICEheader.SiteID)"+
                    //        " order by ax.ACXCUSTTRANS.[Document_No] desc ";
                    //flag = 0;

                    if (drpCustomerName.SelectedItem.Value != "--Select--")
                    {
                        query = "EXEC ACX_GETCOLLECTIONENTRYREPORT'" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + drpCustomerName.SelectedItem.Value + "' ";

                        dt = baseObj.GetData_New(query, CommandType.Text, ilist, litem);

                        ddRefNo.DataSource = dt;
                        ddRefNo.DataTextField = "Document_No";
                        ddRefNo.DataValueField = "Document_No";
                        ddRefNo.DataBind();
                        ddRefNo.Items.Insert(0, new ListItem("--Select--", "0"));
                        Session["AllCusData"] = null;
                    }
                    else
                    {
                        if (Session["AllCusData"] == null)
                        {
                            query = "EXEC ACX_GETCOLLECTIONENTRYREPORT'" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "',''";
                            Completedata = baseObj.GetData_New(query, CommandType.Text, ilist, litem);
                            Session["AllCusData"] = Completedata;
                        }
                        else
                        {
                            Completedata = (DataTable)Session["AllCusData"];
                        }
                        DataTable _newDataTable = new DataTable();
                        var rows = Completedata.AsEnumerable().Where
                           (row => row.Field<string>("CUSTOMER_CODE").ToLower() == customercode.Value.ToLower());

                        if (rows.Any())
                        {
                            _newDataTable = rows.CopyToDataTable<DataRow>();
                        }
                        ddRefNo.DataSource = _newDataTable;
                        ddRefNo.DataTextField = "Document_No";
                        ddRefNo.DataValueField = "Document_No";
                        ddRefNo.DataBind();
                        ddRefNo.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                    //else
                    //{
                    //    query = "EXEC ACX_GETCOLLECTIONENTRYREPORT'" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + customercode.Value + "' ";

                    //    dt = baseObj.GetData_New(query, CommandType.Text, ilist, litem);

                    //    ddRefNo.DataSource = dt;
                    //    ddRefNo.DataTextField = "Document_No";
                    //    ddRefNo.DataValueField = "Document_No";
                    //    ddRefNo.DataBind();
                    //    ddRefNo.Items.Insert(0, new ListItem("--Select--", "0"));
                    //}

                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void drpRefDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                DropDownList RefDoc = (DropDownList)sender;
                GridViewRow row = (GridViewRow)RefDoc.Parent.Parent;
                HiddenField customercode = (HiddenField)row.Cells[0].FindControl("HiddenField2");
                Label RefDate = (Label)row.Cells[0].FindControl("lblRefDocDate");
                Label PendingAmount = (Label)row.Cells[0].FindControl("lblPendingAmount");
                string strDocumentNo = RefDoc.SelectedItem.Text.ToString();
                if (strDocumentNo != "--Select--")
                {
                    //query = "select [Document_No],CONVERT(nvarchar(20), [Document_Date],106) as [Document_Date], coalesce(cast([RemainingAmount] as decimal(10,2)),0) as [RemainingAmount]  from ax.ACXCUSTTRANS" +
                    //     " where  Document_No='" + strDocumentNo + "' and [Customer_Code]='" + customercode.Value + "' and [SiteCode]='" + Session["SiteCode"].ToString() + "' and [DATAAREAID]='" + Session["DATAAREAID"].ToString() + "' ";
                    query = "EXEC [ACX_REPORT]'" + Session["SiteCode"].ToString() + "','" + drpCustomerGroup.SelectedValue + "','" + customercode.Value + "' ,'" + strDocumentNo + "'";
                    dt = baseObj.GetData(query);
                    if (dt.Rows.Count > 0)
                    {
                        RefDate.Text = dt.Rows[0]["Document_Date"].ToString();
                        PendingAmount.Text = dt.Rows[0]["RemainingAmount"].ToString();
                    }
                }
                else
                {
                    RefDate.Text = string.Empty;
                    PendingAmount.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            TextBox txtAmt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtAmt.Parent.Parent;

            int nRow = row.RowIndex;

            if (nRow < gvDetails.Rows.Count)
            {
                // gvDetails.Rows[nRow].Focus = false;
                gvDetails.Rows[++nRow].Cells[0].FindControl("txtAmt").Focus();
            }
            //TextBox txtAmtNext = (TextBox)row.Cells[0].FindControl("txtAmt");
            //txtAmtNext.Focus();

        }

        protected void txtAmount_TextChanged1(object sender, EventArgs e)
        {
            TextBox txtAmt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtAmt.Parent.Parent;

            int nRow = row.RowIndex;

            if (nRow + 1 < gvDetails.Rows.Count)
            {
                gvDetails.Rows[++nRow].Cells[0].FindControl("txtAmount").Focus();

            }
            else
            {
                //btnSave_Click(sender, e);
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
                //if (drpCustomerGroup.Text == "--Select--" || drpCustomerGroup.Text == "")
                //{
                //    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Select Customer Group !');", true);
                //    LblMessage.Text = "Select Customer Group ";
                //    LblMessage.Visible = true;
                //    uppanel.Update();
                //    drpCustomerGroup.Focus();
                //    b = false;
                //    return b;
                //}

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
                    DropDownList RefDocument_No = (DropDownList)gv.Cells[5].FindControl("drpRefDocument");
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
                            LblMessage.Text = "Select Instrument !";
                            LblMessage.Visible = true;
                            uppanel.Update();
                            InstrumentCode.Focus();
                            b = false;
                            return b;
                        }
                        ////if (InstrumentNo.Text == string.Empty)
                        ////{
                        ////    // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Insert Instrument No !');", true);
                        ////    LblMessage.Text = "'Please Insert Instrument No ";
                        ////    LblMessage.Visible = true;
                        ////    uppanel.Update();
                        ////    InstrumentNo.Focus();
                        ////    b = false;
                        ////    return b;
                        ////}
                        if (RefDocument_No.Text == "--Select--")
                        {
                            // this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Please Select RefDocument_No !');", true);
                            LblMessage.Text = "'Please Select RefDocument_No !";
                            LblMessage.Visible = true;
                            uppanel.Update();
                            RefDocument_No.Focus();
                            b = false;
                            return b;
                        }
                    }
                }
            }
            catch (Exception ex)
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
                    DropDownList RefDocument_No = (DropDownList)gv.Cells[5].FindControl("drpRefDocument");
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
                        cmd.Parameters.AddWithValue("@Ref_DocumentNo", RefDocument_No.SelectedItem.Text);
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
                        cmd1.Parameters.AddWithValue("@Document_No", RefDocument_No.SelectedItem.Text);
                        cmd1.Parameters.AddWithValue("@RemainingAmount", pendingamount - amount);
                        cmd1.Parameters.AddWithValue("@Status", "UPDATE");
                        cmd1.Parameters.AddWithValue("@Remark", Remark.Text.Trim());

                        cmd1.ExecuteNonQuery();

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
            catch (Exception ex)
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

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                string condition = string.Empty;

                query = "EXEC USP_GETCOLLECTIONDATA '" + Session["SiteCode"].ToString() + "','" + txtCollectionDate.Text + "'";

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
                    gridCollectionData.DataSource = null;
                    gridCollectionData.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void GridViewFooterCalculate(DataTable dt)
        {
            try
            {
                //decimal total = dt.AsEnumerable().Sum(row => row.Field<decimal>("Value"));          //For Total[Sum] Value Show in Footer--//
                decimal CollectionAmt = dt.AsEnumerable().Sum(row => row.Field<decimal>("Collection Amount"));

                //===============11-5-2016=====
                gridCollectionData.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                gridCollectionData.FooterRow.Cells[7].ForeColor = System.Drawing.Color.MidnightBlue;
                gridCollectionData.FooterRow.Cells[7].Text = "TOTAL";
                gridCollectionData.FooterRow.Cells[7].Font.Bold = true;

                gridCollectionData.FooterRow.Visible = true;
                gridCollectionData.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                gridCollectionData.FooterRow.Cells[8].ForeColor = System.Drawing.Color.MidnightBlue;
                gridCollectionData.FooterRow.Cells[8].Text = CollectionAmt.ToString("N2");
                gridCollectionData.FooterRow.Cells[8].Font.Bold = true;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void tnShow_Click(object sender, EventArgs e)
        {
            BindGridview();
            uppanel.Update();
        }
    }
}