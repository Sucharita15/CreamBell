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
    public partial class frmExpenseCalculation : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        SqlTransaction transaction;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (!Page.IsPostBack)
            {
                Session["DistributorExpense"] = null;
                ddlBUnit();
            }

        }

        private DataTable GetData(DateTime fromdate)
        {
            DataTable dt = null;
            try
            {
                SqlConnection con = baseObj.GetConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.StoredProcedure;
                if (drptargetType.SelectedValue.ToString() == "1")
                { cmd.CommandText = "ACX_TARGET_DISTRIBUTOREXPENSE"; }
                if (drptargetType.SelectedValue.ToString() == "2")
                { cmd.CommandText = "ACX_TARGET_ANNUALEXPENSE"; }
                if (drptargetType.SelectedValue.ToString() == "3")
                { cmd.CommandText = "ACX_GETPSRSTIPEND"; }
                if (drptargetType.SelectedValue.ToString() == "4")
                { cmd.CommandText = "ACX_TARGET_VRSEXPENSE"; }
                if (drptargetType.SelectedValue.ToString() == "5")
                { cmd.CommandText = "ACX_TARGET_VRSEXPENSE1"; }
                if (drptargetType.SelectedValue.ToString() == "6")
                //{ cmd.CommandText = "ACX_TARGET_VRSCLAIMSETUP"; }
                { cmd.CommandText = "ACX_TARGET_VRSCLAIMSETUP_MONTHLY"; }
                if (drptargetType.SelectedValue.ToString() == "7")
                { cmd.CommandText = "ACX_TARGET_VDEXPENSE2"; }
                if (drptargetType.SelectedValue.ToString() == "8")
                { cmd.CommandText = "ACX_TARGET_VDEXPENSE3"; }
                if (drptargetType.SelectedValue.ToString() == "9")
                { cmd.CommandText = "ACX_TARGET_VDEXPENSE4"; }
                if (drptargetType.SelectedValue.ToString() == "10")
                { cmd.CommandText = "ACX_TARGET_VDEXPENSE5"; }
                cmd.Parameters.Add("@EXPENSEMONTH", SqlDbType.SmallDateTime).Value = fromdate.ToString("yyyy-MM-01");
                cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar).Value = Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@BUCode", SqlDbType.NVarChar).Value = ddlBusinessUnit.SelectedItem.Value;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                dt = new DataTable();
                ad.Fill(dt);
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
            return dt;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (ddlBusinessUnit.SelectedIndex == 0)
                {
                    lblError.Text = "Select Business Unit!!!";
                }
                else
                {
                    try
                    {

                        DataTable dt = new DataTable();
                        dt = GetData(Convert.ToDateTime(txtFromDate.Text));
                        Session["DistributorExpense"] = dt;
                        if (dt.Rows.Count > 0)
                            btnsave.Visible = true;
                        else
                            btnsave.Visible = false;

                        if (drptargetType.SelectedValue.ToString() == "3")
                        {
                            grvDetail.Visible = false;
                            gvDetailsPSRStipend.Visible = true;
                            gvDetailsPSRStipend.DataSource = dt;
                            gvDetailsPSRStipend.DataBind();
                        }
                        else
                        {
                            gvDetailsPSRStipend.Visible = false;
                            grvDetail.DataSource = dt;
                            grvDetail.DataBind();
                            grvDetail.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text != "")
                {
                    for (int i = 0; gvDetailsPSRStipend.Rows.Count > i; i++)
                    {
                        int totaldays = 0;
                        DateTime fromdate = Convert.ToDateTime(txtFromDate.Text);
                        totaldays = System.DateTime.DaysInMonth(Convert.ToInt16(fromdate.ToString("yyyy")), Convert.ToInt16(fromdate.ToString("MM")));
                        decimal CONTRIBUTIONSTIPENED = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[3].Text);
                        decimal TA = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[4].Text);
                        decimal DA = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[5].Text);
                        decimal MOBILE = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[6].Text);
                        TextBox txtStipendDays = (TextBox)gvDetailsPSRStipend.Rows[i].FindControl("txtStipendDays");
                        decimal StipendDays = Convert.ToDecimal(txtStipendDays.Text);
                        TextBox txtTA_DA_Days = (TextBox)gvDetailsPSRStipend.Rows[i].FindControl("txtTADADays");
                        decimal TA_DA_Days = Convert.ToDecimal(txtTA_DA_Days.Text);

                        decimal Actual = ((CONTRIBUTIONSTIPENED / totaldays) * StipendDays) + ((TA + DA) * TA_DA_Days) + MOBILE;                       
                        gvDetailsPSRStipend.Rows[i].Cells[10].Text = Convert.ToString(Actual.ToString("0.00"));

                    }
                }     
                
                UPPSRStipend.Update();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + ex.Message.ToString().Replace("'","''") + "');", true);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally
            {
            }
        }

        protected void VRSClaimSetup_Save(int i, DataTable dtExpense, ref SqlCommand cmd, string NUMSEQ, string Doc_NO)
        {
            try { 
            DataTable dt;
            string DISTRIBUTOR_CODE = dtExpense.Rows[i]["DISTRIBUTOR CODE"].ToString();
            string OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
            string OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
            string FROM_DATE = "", TO_DATE = "";
            string TARGET = "0";
            string ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
            string TARGET_INCENTIVE = dtExpense.Rows[i]["FIXED AMT"].ToString();
            string ACTUAL_INCENTIVE = dtExpense.Rows[i]["EXPENSE AMOUNT"].ToString();
            string CLAIM_CATEGORY = dtExpense.Rows[i]["CATEGORY"].ToString();
            string CLAIM_SUBCATEGORY = dtExpense.Rows[i]["EXPENSECODE"].ToString();
            string CLAIM_TYPE = "1";
            string STATUS = "0";
            string CLAIM_MONTH = "";

            string EXP_TYPE = "0";
            string INCENTIVE_ON = "0";
            string CALCULATION_ON = "0";

            string VALUE_PER = dtExpense.Rows[i]["AMOUNT%"].ToString(); ;
            string FIXED_AMT = dtExpense.Rows[i]["FIXED AMT"].ToString(); ;
            string FIXED_PER_DAY = dtExpense.Rows[i]["FIXED PER DAY"].ToString(); ;
            string PRESENT_DAYS = dtExpense.Rows[i]["PRESENT DAYS"].ToString(); ;
            string BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();

            CLAIM_MONTH = FROM_DATE = Convert.ToDateTime("01 " + txtFromDate.Text).ToString("yyyy-MM-dd");
            TO_DATE = Convert.ToDateTime(FROM_DATE).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            if (drptargetType.SelectedValue.ToString() == "6" && OBJECT_CODE.Trim().Length>0)
            {
                string sqlstr1 = @"SELECT * FROM ax.ACX_VRSClaimMaster where SITE_CODE='" + DISTRIBUTOR_CODE + "' and FROM_DATE='" + FROM_DATE + "' ";
                sqlstr1 += "and TO_DATE='" + TO_DATE + "' and VRS_CODE='" + OBJECT_CODE + "'  and VD_SUBCODE='" + OBJECT_SUBCODE + "' and CLAIM_CATEGORY='" + CLAIM_CATEGORY + "' and CLAIM_SUBCATEGORY='" + CLAIM_SUBCATEGORY + "' and DATAAREAID='" + Session["DATAAREAID"] + "' AND  CLAIM_TYPE ='" + CLAIM_TYPE + "'";

          
                dt = baseObj.GetData(sqlstr1);
                if (dt.Rows.Count == 0)
                {
                    if (Convert.ToDouble(ACTUAL_INCENTIVE) > 0)
                    {
                        cmd.CommandText = "ACXINSERTCLAIMMASTER_MONTHLY";
                        cmd.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar).Value = Session["DATAAREAID"];
                        cmd.Parameters.Add("@DOCUMENT_CODE", SqlDbType.NVarChar).Value = Doc_NO;
                        cmd.Parameters.Add("@SITE_CODE", SqlDbType.NVarChar).Value = DISTRIBUTOR_CODE;

                        cmd.Parameters.Add("@VRS_CODE", SqlDbType.NVarChar).Value = OBJECT_CODE; // VRS Code
                        cmd.Parameters.Add("@VD_SUBCODE", SqlDbType.NVarChar).Value = OBJECT_SUBCODE; // VD Code

                        cmd.Parameters.Add("@FROM_DATE", SqlDbType.SmallDateTime).Value = FROM_DATE;
                        cmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = TO_DATE;

                        cmd.Parameters.Add("@EXP_TYPE", SqlDbType.Int).Value = EXP_TYPE;

                        // INCENTIVE_ON : 0 For Amount, 1 For Qty
                        cmd.Parameters.Add("@INCENTIVE_ON", SqlDbType.Int).Value = INCENTIVE_ON;
                        
                        // Calculation_On : 0 For Amount, 1 For Percentage
                        if (Convert.ToDecimal(VALUE_PER) > 0)
                        {
                            CALCULATION_ON = "1";
                        }
                        
                        cmd.Parameters.Add("@CALCULATION_ON", SqlDbType.Int).Value = CALCULATION_ON;

                        cmd.Parameters.Add("@TARGET", SqlDbType.Decimal).Value = TARGET;
                        cmd.Parameters.Add("@ACHIVEMENT", SqlDbType.Decimal).Value = ACHIVEMENT;

                        cmd.Parameters.Add("@VALUE_PER", SqlDbType.Decimal).Value = VALUE_PER;
                        cmd.Parameters.Add("@FIXED_AMT", SqlDbType.Decimal).Value = FIXED_AMT;
                        cmd.Parameters.Add("@FIXED_PER_DAY", SqlDbType.Decimal).Value = FIXED_PER_DAY;
                        cmd.Parameters.Add("@PRESENT_DAYS", SqlDbType.Decimal).Value = PRESENT_DAYS;

                        cmd.Parameters.Add("@ACTUAL_INCENTIVE", SqlDbType.Decimal).Value = ACTUAL_INCENTIVE;
                        cmd.Parameters.Add("@CLAIM_CATEGORY", SqlDbType.NVarChar).Value = CLAIM_CATEGORY;
                        cmd.Parameters.Add("@CLAIM_SUBCATEGORY", SqlDbType.NVarChar).Value = CLAIM_SUBCATEGORY;
                        cmd.Parameters.Add("@CLAIM_TYPE", SqlDbType.NVarChar).Value = CLAIM_TYPE;
                        cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = STATUS;
                        cmd.Parameters.Add("@NumSeq", SqlDbType.BigInt).Value = NUMSEQ;
                        cmd.Parameters.Add("CLAIM_MONTH", SqlDbType.NVarChar).Value = CLAIM_MONTH;
                        cmd.Parameters.Add("@BUCode", SqlDbType.NVarChar).Value = BUCODE;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (drptargetType.SelectedValue.ToString() == "3")
                { SavePSRStipend(); }
                else
                {
                    DataTable dtExpense = new DataTable();
                    if (Session["DistributorExpense"] == null)
                    { dtExpense = GetData(Convert.ToDateTime(txtFromDate.Text)); }
                    else
                    { dtExpense = (DataTable)Session["DistributorExpense"]; }
                    SqlConnection con = baseObj.GetConnection();
                    SqlTransaction tran = null;
                    SqlCommand cmd = new SqlCommand();
                    SqlCommand cmd1 = new SqlCommand();
                    try
                    {
                        DataTable dt = baseObj.GetNumSequenceNew(11, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                        string Doc_NO = string.Empty;
                        string NUMSEQ = string.Empty;
                        if (dt != null)
                        {
                            Doc_NO = dt.Rows[0][0].ToString();
                            NUMSEQ = dt.Rows[0][1].ToString();
                        }
                        else
                        { return; }

                        //Generate Document Number Seq for VRS Claim Setup
                        dt = baseObj.GetNumSequenceNew(17, Session["SiteCode"].ToString(), Session["DATAAREAID"].ToString());  //st.Substring(st.Length - 6) + System.DateTime.Now.ToString("yyMMddhhmmss");
                        string Doc_NO_Claim = string.Empty;
                        string NUMSEQ_Claim = string.Empty;
                        if (dt != null)
                        {
                            Doc_NO_Claim = dt.Rows[0][0].ToString();
                            NUMSEQ_Claim = dt.Rows[0][1].ToString();
                        }

                        

                        if (con.State.ToString() == "Closed") { con.Open(); }
                        tran = con.BeginTransaction();
                        cmd.Transaction = tran;

                        Boolean FoundAlready, FoundSuccess;
                        FoundAlready = false;
                        FoundSuccess = false;
                        dtExpense.Columns.Add("STATUS", typeof(string));
                        //foreach (GridViewRow grv in grvDetail.Rows)
                        for (int i = 0; i < dtExpense.Rows.Count; i++)
                        {
                            string OBJECT_CODE = dtExpense.Rows[i]["DISTRIBUTOR CODE"].ToString();
                            string OBJECT_SUBCODE = "";
                            string TARGET_CODE = "";
                            string TARGET_DESCRIPTION = "";// dtExpense.Rows[i]["EXPENSE TYPE"].ToString();
                            string FROM_DATE = "", TO_DATE = "";
                            string TARGET = "0";
                            string ACHIVEMENT = "0";
                            string TARGET_INCENTIVE = "";
                            string ACTUAL_INCENTIVE = dtExpense.Rows[i]["EXPENSE AMOUNT"].ToString();
                            string CLAIM_CATEGORY = dtExpense.Rows[i]["CATEGORY"].ToString();
                            string CLAIM_SUBCATEGORY = dtExpense.Rows[i]["EXPENSECODE"].ToString();
                            string CLAIM_TYPE = "6";
                            string STATUS = "0";
                            string MODIFIEDDATETIME = string.Empty;
                            string CREATEDDATETIME = string.Empty;
                            string TARGET_GROUP = "";
                            string CALCULATION_PATTERN = "";
                            string CLAIM_MONTH = "";
                            string OBJECT= "1";
                            string BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();

                            //string VALUE_PER = "0";// dtExpense.Rows[i]["AMOUNT%"].ToString(); ;
                            //string FIXED_AMT = "0"; //dtExpense.Rows[i]["FIXED AMT"].ToString(); ;
                            //string FIXED_PER_DAY = "0"; //dtExpense.Rows[i]["FIXED PER DAY"].ToString(); ;
                            string PRESENT_DAYS = "0"; //dtExpense.Rows[i]["PRESENT DAYS"].ToString(); ;
                            string VDCLAIM_TYPE = "1";
                                                        

                            CLAIM_MONTH = FROM_DATE = Convert.ToDateTime("01 " + txtFromDate.Text).ToString("yyyy-MM-dd");
                            TO_DATE = Convert.ToDateTime(FROM_DATE).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

                            if (drptargetType.SelectedValue.ToString() == "1")
                            {
                                TARGET_INCENTIVE = dtExpense.Rows[i]["CALCULATE ON"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["EXPENSE ON"].ToString();
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "2")
                            {
                                OBJECT_SUBCODE = dtExpense.Rows[i]["CUSTGROUP"].ToString();
                                FROM_DATE = Convert.ToDateTime(dtExpense.Rows[i]["FROMDATE"]).ToString("yyyy-MM-dd");
                                TO_DATE = Convert.ToDateTime(dtExpense.Rows[i]["TODATE"]).ToString("yyyy-MM-dd");
                                TARGET = dtExpense.Rows[i]["TARGET"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                CLAIM_MONTH = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-01");
                                TARGET_INCENTIVE = dtExpense.Rows[i]["INCENTIVE VALUE"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "7";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "4")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();                             
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();                                
                                TARGET_INCENTIVE = dtExpense.Rows[i]["INCENTIVE VALUE"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "9";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "5")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
                                OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
                                FROM_DATE = Convert.ToDateTime(dtExpense.Rows[i]["FROMDATE"]).ToString("yyyy-MM-dd");
                                TO_DATE = Convert.ToDateTime(dtExpense.Rows[i]["TODATE"]).ToString("yyyy-MM-dd");
                                TARGET = dtExpense.Rows[i]["TARGET"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                TARGET_INCENTIVE = dtExpense.Rows[i]["FIXVALUE"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "10";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "6")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
                                OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                TARGET_INCENTIVE = dtExpense.Rows[i]["FIXED AMT"].ToString();
                                CLAIM_TYPE = "11";
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "7")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
                                OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
                                FROM_DATE = Convert.ToDateTime(dtExpense.Rows[i]["FROMDATE"]).ToString("yyyy-MM-dd");
                                TO_DATE = Convert.ToDateTime(dtExpense.Rows[i]["TODATE"]).ToString("yyyy-MM-dd");
                                //TARGET = dtExpense.Rows[i]["TARGET"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                TARGET_INCENTIVE = dtExpense.Rows[i]["FIXVALUE"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "12";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                VDCLAIM_TYPE = "2";
                                PRESENT_DAYS = dtExpense.Rows[i]["PRESENT DAYS"].ToString();
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "8")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
                                OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
                                FROM_DATE = Convert.ToDateTime(dtExpense.Rows[i]["FROMDATE"]).ToString("yyyy-MM-dd");
                                TO_DATE = Convert.ToDateTime(dtExpense.Rows[i]["TODATE"]).ToString("yyyy-MM-dd");
                                TARGET = dtExpense.Rows[i]["TARGET"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                TARGET_INCENTIVE = dtExpense.Rows[i]["FIXVALUE"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "13";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                VDCLAIM_TYPE = "3";
                                PRESENT_DAYS = dtExpense.Rows[i]["PRESENT DAYS"].ToString();
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "9")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
                                OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
                                FROM_DATE = Convert.ToDateTime(dtExpense.Rows[i]["FROMDATE"]).ToString("yyyy-MM-dd");
                                TO_DATE = Convert.ToDateTime(dtExpense.Rows[i]["TODATE"]).ToString("yyyy-MM-dd");
                                TARGET = dtExpense.Rows[i]["TARGET"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                TARGET_INCENTIVE = dtExpense.Rows[i]["FIXVALUE"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "14";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                VDCLAIM_TYPE = "4";
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }
                            if (drptargetType.SelectedValue.ToString() == "10")
                            {
                                OBJECT = "3";
                                OBJECT_CODE = dtExpense.Rows[i]["VRS CODE"].ToString();
                                OBJECT_SUBCODE = dtExpense.Rows[i]["VENDORCODE"].ToString();
                                FROM_DATE = Convert.ToDateTime(dtExpense.Rows[i]["FROMDATE"]).ToString("yyyy-MM-dd");
                                TO_DATE = Convert.ToDateTime(dtExpense.Rows[i]["TODATE"]).ToString("yyyy-MM-dd");
                                TARGET = dtExpense.Rows[i]["TARGET"].ToString();
                                ACHIVEMENT = dtExpense.Rows[i]["ACHIVEMENT"].ToString();
                                TARGET_INCENTIVE = dtExpense.Rows[i]["VALUE%"].ToString();
                                CALCULATION_PATTERN = dtExpense.Rows[i]["CALCULATION ON"].ToString();
                                CLAIM_TYPE = "15";
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                TARGET_GROUP = dtExpense.Rows[i]["INCENTIVE ON"].ToString();
                                VDCLAIM_TYPE = "5";
                                BUCODE = dtExpense.Rows[i]["BUCODE"].ToString();
                            }

                            string sqlstr = @"SELECT * FROM AX.ACXCLAIMMASTER where SITE_CODE='" + Session["SiteCode"].ToString() + "' and FROM_DATE='" + FROM_DATE + "' ";
                            sqlstr += "and TO_DATE='" + TO_DATE + "' and OBJECT_CODE='" + OBJECT_CODE + "' AND OBJECT='" + OBJECT + "' and OBJECT_SUBCODE='" + OBJECT_SUBCODE + "' and TARGET_CODE='" + TARGET_CODE + "' ";
                            sqlstr += "and TARGET=" + TARGET + " and CLAIM_CATEGORY='" + CLAIM_CATEGORY + "' and CLAIM_SUBCATEGORY='" + CLAIM_SUBCATEGORY + "' and DATAAREAID='" + Session["DATAAREAID"] + "' and TARGET_GROUP='" + TARGET_GROUP + "'";
                            dt = baseObj.GetData(sqlstr);

                            if (dt.Rows.Count == 0)
                            {
                                if (Convert.ToDouble(ACTUAL_INCENTIVE) > 0)
                                {
                                    cmd.Connection = con;
                                    cmd.Parameters.Clear();
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    if (drptargetType.SelectedValue.ToString() == "6" && OBJECT_CODE.Trim().Length>0)
                                    {
                                        //NEW METHOD VRS CLAIM SETUP[VD LINKED WITH VRS]
                                        CLAIM_TYPE = "1";
                                        //if (Doc_NO_Claim == string.Empty || Doc_NO_Claim.Trim().Length == 0)
                                        //{
                                        //    return;
                                        //}
                                        //if (NUMSEQ_Claim == string.Empty || NUMSEQ_Claim.Trim().Length == 0)
                                        //{
                                        //    dtClaimNumSeq = baseObj.FillDataTable("SELECT ISNULL(MAX(NumSeq),0)+1 FROM AX.ACX_VRSClaimMaster WHERE CLAIM_TYPE = '" + CLAIM_TYPE +"' AND DATAAREAID ='" + Session["DATAAREAID"].ToString() + "'");
                                        //    if (dtClaimNumSeq != null)
                                        //    {
                                        //        NUMSEQ_Claim = dtClaimNumSeq.Rows[0][0].ToString();
                                        //    }
                                        //}
                                        //if (NUMSEQ_Claim == string.Empty || NUMSEQ_Claim.Trim().Length == 0)
                                        //{ return; }
                                        VRSClaimSetup_Save(i, dtExpense, ref cmd, NUMSEQ_Claim, Doc_NO_Claim);
                                    }
                                    else
                                    {
                                        cmd.CommandText = "ACXINSERTCLAIMMASTER";
                                        cmd.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar).Value = Session["DATAAREAID"];
                                        cmd.Parameters.Add("@DOCUMENT_CODE", SqlDbType.NVarChar).Value = Doc_NO;
                                        cmd.Parameters.Add("@SITE_CODE", SqlDbType.NVarChar).Value = Session["SiteCode"].ToString();

                                        cmd.Parameters.Add("@OBJECT", SqlDbType.NVarChar).Value = OBJECT;
                                        cmd.Parameters.Add("@OBJECT_CODE", SqlDbType.NVarChar).Value = OBJECT_CODE;
                                        cmd.Parameters.Add("@OBJECT_SUBCODE", SqlDbType.NVarChar).Value = OBJECT_SUBCODE;
                                        cmd.Parameters.Add("@TARGET_CODE", SqlDbType.NVarChar).Value = TARGET_CODE;
                                        cmd.Parameters.Add("@TARGET_DESCRIPTION", SqlDbType.NVarChar).Value = TARGET_DESCRIPTION;

                                        cmd.Parameters.Add("@FROM_DATE", SqlDbType.SmallDateTime).Value = FROM_DATE;
                                        cmd.Parameters.Add("@TO_DATE", SqlDbType.SmallDateTime).Value = TO_DATE;
                                        cmd.Parameters.Add("@TARGET", SqlDbType.Decimal).Value = TARGET;
                                        cmd.Parameters.Add("@ACHIVEMENT", SqlDbType.Decimal).Value = ACHIVEMENT;
                                        cmd.Parameters.Add("@TARGET_INCENTIVE", SqlDbType.Decimal).Value = TARGET_INCENTIVE;
                                        cmd.Parameters.Add("@ACTUAL_INCENTIVE", SqlDbType.Decimal).Value = ACTUAL_INCENTIVE;
                                        cmd.Parameters.Add("@CLAIM_CATEGORY", SqlDbType.NVarChar).Value = CLAIM_CATEGORY;
                                        cmd.Parameters.Add("@CLAIM_SUBCATEGORY", SqlDbType.NVarChar).Value = CLAIM_SUBCATEGORY;
                                        cmd.Parameters.Add("@CLAIM_TYPE", SqlDbType.NVarChar).Value = CLAIM_TYPE;
                                        cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = STATUS;
                                        cmd.Parameters.Add("@NumSeq", SqlDbType.BigInt).Value = NUMSEQ;
                                        cmd.Parameters.Add("@TARGET_GROUP", SqlDbType.NVarChar).Value = TARGET_GROUP;
                                        cmd.Parameters.Add("@CALCULATION_PATTERN", SqlDbType.NVarChar).Value = CALCULATION_PATTERN;
                                        cmd.Parameters.Add("CLAIM_MONTH", SqlDbType.NVarChar).Value = CLAIM_MONTH;
                                        cmd.Parameters.Add("@BU_CODE", SqlDbType.NVarChar).Value = BUCODE;

                                        cmd.ExecuteNonQuery();
                                    }
                                    if (drptargetType.SelectedValue.ToString() == "1")
                                    {
                                        DataRow[] dr = dtExpense.Select("[EXPENSECODE]='" + CLAIM_SUBCATEGORY + "'");
                                        if (dr.Length > 0)
                                        {
                                            for (int j = 0; j < dr.Length; j++)
                                            { dr[j]["STATUS"] = "Success"; }
                                        }
                                    }
                                    //grvDetail.Rows[i].Cells[22].Text = "Success";
                                    FoundSuccess = true;

                                    #region  SAVE ON VRS CLAIM TABLE


                                    //===================New===============


                                    if ((drptargetType.SelectedValue.ToString() == "7" || drptargetType.SelectedValue.ToString() == "8" || drptargetType.SelectedValue.ToString() == "9" || drptargetType.SelectedValue.ToString() == "10") && OBJECT_CODE.Trim().Length > 0)
                                    {

                                        //if (Doc_NO_Claim == string.Empty || Doc_NO_Claim.Trim().Length == 0)
                                        //{
                                        //    return;
                                        //}
                                        //if (NUMSEQ_Claim == string.Empty || NUMSEQ_Claim.Trim().Length == 0)
                                        //{
                                        //    dtClaimNumSeq = baseObj.FillDataTable("SELECT ISNULL(MAX(NumSeq),0)+1 FROM AX.ACX_VRSClaimMaster WHERE CLAIM_TYPE = '" + CLAIM_TYPE + "' AND DATAAREAID ='" + Session["DATAAREAID"].ToString() + "'");
                                        //    if (dtClaimNumSeq != null)
                                        //    {
                                        //        NUMSEQ_Claim = dtClaimNumSeq.Rows[0][0].ToString();
                                        //    }
                                        //}
                                        //if (NUMSEQ_Claim == string.Empty || NUMSEQ_Claim.Trim().Length == 0)
                                        //{ return; }

                                        string EXP_TYPE = dtExpense.Rows[i]["TARGET TYPE"].ToString();
                                        string VALUE_PER = "0";
                                        string FIXED_AMT = dtExpense.Rows[i]["FIXVALUE"].ToString();
                                        string FIXED_PER_DAY = "0";


                                        if (EXP_TYPE == "SALE")
                                        { EXP_TYPE = "1"; }
                                        else if (EXP_TYPE == "PURCHASE") { EXP_TYPE = "2"; }
                                        else { EXP_TYPE = "0"; }

                                        if (TARGET_GROUP == "VALUE")
                                        { TARGET_GROUP = "0"; }
                                        else if (TARGET_GROUP == "LTR") { TARGET_GROUP = "1"; }

                                        if (CALCULATION_PATTERN == "FIX")
                                        { CALCULATION_PATTERN = "0"; }
                                        else { CALCULATION_PATTERN = "1"; }

                                        cmd1 = new SqlCommand("ACXINSERTCLAIMMASTER_MONTHLY");
                                        cmd1.Connection = con;
                                        cmd1.Parameters.Clear();
                                        cmd1.Transaction = tran;
                                        cmd1.CommandTimeout = 3600;
                                        cmd1.CommandType = CommandType.StoredProcedure;

                                        string sqlstr1 = @"SELECT * FROM ax.ACX_VRSClaimMaster where SITE_CODE='" + Session["SiteCode"].ToString() + "' and FROM_DATE='" + FROM_DATE + "' ";
                                        sqlstr1 += "and TO_DATE='" + TO_DATE + "' and VRS_CODE='" + OBJECT_CODE + "'  and VD_SUBCODE='" + OBJECT_SUBCODE + "' and CLAIM_CATEGORY='" + CLAIM_CATEGORY + "' and CLAIM_SUBCATEGORY='" + CLAIM_SUBCATEGORY + "' and DATAAREAID='" + Session["DATAAREAID"] + "' ";


                                        dt = baseObj.GetData(sqlstr1);
                                        if (dt.Rows.Count == 0)
                                        {
                                            if (Convert.ToDouble(ACTUAL_INCENTIVE) > 0)
                                            {

                                                cmd1.Parameters.Add("@DATAAREAID", SqlDbType.NVarChar).Value = Session["DATAAREAID"];
                                                cmd1.Parameters.Add("@DOCUMENT_CODE", SqlDbType.NVarChar).Value = Doc_NO_Claim;
                                                cmd1.Parameters.Add("@SITE_CODE", SqlDbType.NVarChar).Value = Session["SiteCode"].ToString();

                                                cmd1.Parameters.Add("@VRS_CODE", SqlDbType.NVarChar).Value = OBJECT_CODE;
                                                cmd1.Parameters.Add("@VD_SUBCODE", SqlDbType.NVarChar).Value = OBJECT_SUBCODE;

                                                cmd1.Parameters.Add("@FROM_DATE", SqlDbType.DateTime).Value = FROM_DATE;
                                                cmd1.Parameters.Add("@TO_DATE", SqlDbType.DateTime).Value = TO_DATE;

                                                cmd1.Parameters.Add("@EXP_TYPE", SqlDbType.Int).Value = EXP_TYPE;
                                                cmd1.Parameters.Add("@INCENTIVE_ON", SqlDbType.Int).Value = TARGET_GROUP;
                                                cmd1.Parameters.Add("@CALCULATION_ON", SqlDbType.Int).Value = CALCULATION_PATTERN;

                                                cmd1.Parameters.Add("@TARGET", SqlDbType.Decimal).Value = TARGET;
                                                cmd1.Parameters.Add("@ACHIVEMENT", SqlDbType.Decimal).Value = ACHIVEMENT;

                                                cmd1.Parameters.Add("@VALUE_PER", SqlDbType.Decimal).Value = VALUE_PER;
                                                cmd1.Parameters.Add("@FIXED_AMT", SqlDbType.Decimal).Value = FIXED_AMT;
                                                cmd1.Parameters.Add("@FIXED_PER_DAY", SqlDbType.Decimal).Value = FIXED_PER_DAY;
                                                cmd1.Parameters.Add("@PRESENT_DAYS", SqlDbType.Int).Value = PRESENT_DAYS;

                                                cmd1.Parameters.Add("@ACTUAL_INCENTIVE", SqlDbType.Decimal).Value = ACTUAL_INCENTIVE;
                                                cmd1.Parameters.Add("@CLAIM_CATEGORY", SqlDbType.NVarChar).Value = CLAIM_CATEGORY;
                                                cmd1.Parameters.Add("@CLAIM_SUBCATEGORY", SqlDbType.NVarChar).Value = CLAIM_SUBCATEGORY;
                                                cmd1.Parameters.Add("@CLAIM_TYPE", SqlDbType.NVarChar).Value = VDCLAIM_TYPE;
                                                cmd1.Parameters.Add("@STATUS", SqlDbType.Int).Value = STATUS;
                                                cmd1.Parameters.Add("@NumSeq", SqlDbType.BigInt).Value = NUMSEQ_Claim;
                                                cmd1.Parameters.Add("CLAIM_MONTH", SqlDbType.SmallDateTime).Value = CLAIM_MONTH;
                                                cmd1.Parameters.Add("@BU_CODE", SqlDbType.NVarChar).Value = BUCODE;

                                                cmd1.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            } //IF END
                            else
                            {
                                FoundAlready = true;
                                if (drptargetType.SelectedValue.ToString() == "1")
                                {
                                    DataRow[] dr = dtExpense.Select("[EXPENSECODE]='" + CLAIM_SUBCATEGORY + "'");
                                    if (dr.Length > 0)
                                    {
                                        for (int j = 0; j < dr.Length; j++)
                                        { dr[j]["STATUS"] = "Already"; }
                                    }
                                }
                            }

                        } //FOR END
                        tran.Commit();
                        if (FoundAlready == true && FoundSuccess == true)
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Some data already saved. Except that saved successfully.');", true);
                            grvDetail.DataSource = dtExpense;
                            grvDetail.DataBind();
                            return;
                        }
                        else if (FoundAlready)
                        { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data already saved.');", true); }
                        else
                        { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data saved successfully.');", true); }
                        grvDetail.DataSource = null;
                        grvDetail.Visible = false;
                        // grvDetail.Columns[22].Visible = true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        lblError.Text = ex.Message;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    finally { if (con.State.ToString() == "Open") { con.Close(); } }
                }
                
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            finally { }
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (grvDetail.Rows.Count > 0 || gvDetailsPSRStipend.Rows.Count > 0)
            {
                ExportToExcelNew();
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
            }
        }

        private void ExportToExcelNew()
        {
            try
            {
                DataTable dtExpense = new DataTable();
                if (Session["DistributorExpense"] == null)
                { dtExpense = GetData(Convert.ToDateTime(txtFromDate.Text)); }
                else
                { dtExpense = (DataTable)Session["DistributorExpense"]; }

                App_Code.Global.ExportDataTable(dtExpense, "Expense.xls", "EXCEL", "DISTRIBUTOR EXPENSE");
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void drptargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetForm();

            lbldate.Text = ""; 
            if (drptargetType.SelectedValue.ToString() == "2")
            {
                lbldate.Text = "Annual Expense Date";
            }
            if (drptargetType.SelectedValue.ToString() == "3")
            {
                lbldate.Text = "PSR Stipend Expense";
            }
            else
            {
                lbldate.Text = "Expense Month";
            }
           
        }

        protected void txtStipendDays_TextChanged(object sender, EventArgs e)
        {
            if (txtFromDate.Text!="")
            {
                GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;
                TextBox txtNoOfDays = (TextBox)sender;

                #region  Find Total No Of Days
                int noofdays = 0;
                DateTime fromdate = Convert.ToDateTime(txtFromDate.Text);
                noofdays = System.DateTime.DaysInMonth(Convert.ToInt16(fromdate.ToString("yyyy")), Convert.ToInt16(fromdate.ToString("MM")));
                if (Convert.ToInt16(txtNoOfDays.Text) > noofdays)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Please Enter Correct No Of Days.');", true);
                    txtNoOfDays.Text = "0";
                }
                #endregion 
               
                CalculationStipned(row.RowIndex);
               
            }           
        }

        protected void txtTADADays_TextChanged(object sender, EventArgs e)
        {
            if (txtFromDate.Text != "")
            {
                GridViewRow row = (GridViewRow)((TextBox)sender).NamingContainer;
                TextBox txtNoOfDays = (TextBox)sender;

                #region  Find Total No Of Days
                int noofdays = 0;
                DateTime fromdate = Convert.ToDateTime(txtFromDate.Text);
                noofdays = System.DateTime.DaysInMonth(Convert.ToInt16(fromdate.ToString("yyyy")), Convert.ToInt16(fromdate.ToString("MM")));
                if (Convert.ToInt16(txtNoOfDays.Text) > noofdays)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Please Enter Correct No Of Days.');", true);
                    txtNoOfDays.Text = "0";
                }
                #endregion

                CalculationStipned(row.RowIndex);               
            }   
        }

        protected void CalculationStipned(int i)
        {
            #region Calculation
            int noofdays = 0;
            DateTime fromdate = Convert.ToDateTime(txtFromDate.Text);
            noofdays = System.DateTime.DaysInMonth(Convert.ToInt16(fromdate.ToString("yyyy")), Convert.ToInt16(fromdate.ToString("MM")));

            decimal CONTRIBUTIONSTIPENED = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[3].Text);
            decimal TA = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[5].Text);
            decimal DA = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[6].Text);
            decimal MOBILE = Convert.ToDecimal(gvDetailsPSRStipend.Rows[i].Cells[7].Text);
            TextBox txtStipendDays = (TextBox)gvDetailsPSRStipend.Rows[i].FindControl("txtStipendDays");
            decimal StipendDays = Convert.ToDecimal(txtStipendDays.Text);
            TextBox txtTA_DA_Days = (TextBox)gvDetailsPSRStipend.Rows[i].FindControl("txtTADADays");
            decimal TA_DA_Days = Convert.ToDecimal(txtTA_DA_Days.Text);

            decimal Actual = ((CONTRIBUTIONSTIPENED / noofdays) * StipendDays) + ((TA + DA) * TA_DA_Days) + MOBILE;
            gvDetailsPSRStipend.Rows[i].Cells[11].Text = Convert.ToString(Actual.ToString("0.00"));
            #endregion
        }

        protected void SavePSRStipend()
        {
            SqlConnection conn = null;
            SqlCommand cmd;
            
            try
            {   
                conn = baseObj.GetConnection();
                string strCode = string.Empty;
                cmd = new SqlCommand();
                transaction = conn.BeginTransaction();
                cmd.Connection = conn;
                cmd.Transaction = transaction;
                cmd.CommandTimeout = 3600;

                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_Insert_PSRSTIPEND";

                Boolean FoundAlready, FoundSuccess;
                FoundAlready = false;
                FoundSuccess = false;
                int count = 0;
                foreach (GridViewRow grv in gvDetailsPSRStipend.Rows)
                {
                    cmd.Parameters.Clear();
                
                    string PSRCODE = grv.Cells[0].Text;
                    string SITEID = Session["SITECODE"].ToString();
                    string EMPLOYEENO = grv.Cells[2].Text;
                    decimal CONTRIBUTION = Convert.ToDecimal(grv.Cells[3].Text);
                    decimal TA = Convert.ToDecimal(grv.Cells[5].Text);
                    decimal DA= Convert.ToDecimal(grv.Cells[6].Text);
                    decimal MOBILE = Convert.ToDecimal(grv.Cells[7].Text);
                    decimal TOTALSTIPEND = 0;// Convert.ToDecimal(grv.Cells[7].Text);
                    decimal ACTUALSTIPEND = 0;
                    if (grv.Cells[11].Text!="")
                    {
                        ACTUALSTIPEND = Convert.ToDecimal(grv.Cells[11].Text);
                    }                   
                    TextBox txtSTIPENDWORKIGDAYS = (TextBox)grv.FindControl("txtStipendDays");
                    TextBox txtTADAWORKIGDAYS = (TextBox)grv.FindControl("txtTADADays");

                    decimal STIPENDWORKIGDAYS = Convert.ToDecimal(txtSTIPENDWORKIGDAYS.Text);
                    decimal TADAWORKIGDAYS = Convert.ToDecimal(txtTADAWORKIGDAYS.Text);
                    string stipendmnth = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
                    string sqlstr = @"SELECT * FROM [ax].[ACXPSRSTIPEND] where siteid='" + SITEID + "' and psrcode ='" + PSRCODE + "' and stipendmonth='" + stipendmnth + "' ";
                    string BUCODE = grv.Cells[4].Text;
                    
                    DataTable dt = baseObj.GetData(sqlstr);
                    if (dt.Rows.Count == 0)
                    {
                        if (ACTUALSTIPEND > 0)
                        {
                            count = count + 1;
                            cmd.Parameters.AddWithValue("@SITEID", SITEID);
                            cmd.Parameters.AddWithValue("@PSRCODE", PSRCODE);
                            cmd.Parameters.AddWithValue("@EMPLOYEENO", EMPLOYEENO);
                            cmd.Parameters.AddWithValue("@MONTHSTIPEND", Convert.ToDateTime(txtFromDate.Text));
                            cmd.Parameters.AddWithValue("@DATAAREAID", Session["DATAAREAID"].ToString());
                            cmd.Parameters.AddWithValue("@CONTRIBUTION", CONTRIBUTION);
                            cmd.Parameters.AddWithValue("@TA", TA);
                            cmd.Parameters.AddWithValue("@DA", DA);
                            cmd.Parameters.AddWithValue("@MOBILE", MOBILE);
                            cmd.Parameters.AddWithValue("@TOTALSTIPEND", TOTALSTIPEND);
                            cmd.Parameters.AddWithValue("@ACTUALSTIPEND", ACTUALSTIPEND);
                            cmd.Parameters.AddWithValue("@STIPENDWORKIGDAYS", STIPENDWORKIGDAYS);
                            cmd.Parameters.AddWithValue("@TADAWORKIGDAYS", TADAWORKIGDAYS);
                            cmd.Parameters.AddWithValue("@BUCode", BUCODE);

                            cmd.ExecuteNonQuery();

                            FoundSuccess = true;
                        }
                    }
                    else
                    {
                        FoundAlready = true;
                    }
                    
                  }

                    transaction.Commit();
                    if (FoundAlready == true && FoundSuccess == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Some data already saved. Except that saved successfully.');", true);                    
                    }
                    else if (FoundAlready)
                    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data already saved.');", true); }
                    else if (count==0)
                    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data not saved.');", true); }
                    else 
                    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data saved successfully.');", true); }
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data saved successfully.');", true);
                    gvDetailsPSRStipend.DataSource = null;
                    gvDetailsPSRStipend.Visible = false;
                    btnsave.Visible = false;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string message = "alert('Error:" + ex.Message + " !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally { if (conn.State.ToString() == "Open") { conn.Close(); } }
        }

        private void ResetForm()
        {
            gvDetailsPSRStipend.DataSource = null;
            gvDetailsPSRStipend.Visible = false;
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void ddlBUnit()
        {
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            ddlBusinessUnit.Items.Add("...Select...");
            baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
        }
    }
}