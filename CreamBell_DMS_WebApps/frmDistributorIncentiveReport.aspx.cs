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
    public partial class frmDistributorIncentiveReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (!Page.IsPostBack)
            {
                Session["TargetIncentive"] = null;
                try{
                string sqlstr = @"select  distinct TC.Name,TC.Category as Code from [ax].[ACXTARGETCATEGORY] TC ";
                ddlClaimCategory.Items.Add("Select...");
                baseObj.BindToDropDown(ddlClaimCategory, sqlstr, "Name", "Code");
                    ddlBUnit();
                }
                catch(Exception ex)
                {
                    lblError.Text = ex.Message;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        private DataTable GetData(DateTime fromdate,DateTime todate,string targettype,string targetcategory,string targetsubcategory,string objecttype,string objectcode)
        {
            DataTable dt=null;
            try
            {
                SqlConnection con = baseObj.GetConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = string.Empty;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ACX_Target_Sale";
                cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar).Value = Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@FromDate", SqlDbType.SmallDateTime).Value = fromdate.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@ToDate", SqlDbType.SmallDateTime).Value = todate.ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@TargetType", SqlDbType.Int).Value = targettype;//Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@TargetCategory", SqlDbType.NVarChar).Value = targetcategory;// Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@TargetSubCategory", SqlDbType.NVarChar).Value = targetsubcategory;// Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@ObjectType", SqlDbType.NVarChar).Value = objecttype;// Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@ObjectCode", SqlDbType.NVarChar).Value = objectcode;// Convert.ToString(Session["SiteCode"]);
                cmd.Parameters.Add("@BUCODE", SqlDbType.NVarChar).Value = ddlBusinessUnit.SelectedItem.Value;// Convert.ToString(Session["SiteCode"]);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                return dt;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                if (ddlBusinessUnit.SelectedIndex == 0)
                {
                    lblError.Text = "Select BUCode!!";
                }
                else
                {
                    try
                    {
                        SqlConnection con = baseObj.GetConnection();
                        SqlCommand cmd = new SqlCommand();
                        grvDetail.DataSource = null;
                        grvDetail.DataBind();
                        btnSubmit.Visible = false;
                        Session["TargetIncentive"] = null;
                        cmd.Connection = con;
                        cmd.CommandText = string.Empty;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        //cmd.CommandText = "Acx_TargetTable";
                        string CategoryName, SubCategory, ObjectType, ObjectCode;
                        CategoryName = SubCategory = ObjectCode = "";
                        if (drptargetType.SelectedValue.ToString() == "0")
                        { ObjectType = "0,1,2"; }
                        else
                        {
                            ObjectType = "2";
                        }

                        if (ddlClaimCategory.SelectedValue.ToString() != "Select..." && ddlClaimCategory.SelectedValue.ToString().Length > 0)
                        { CategoryName = ddlClaimCategory.SelectedValue.ToString(); }
                        if (ddlClaimSubCategory.SelectedValue.ToString() != "Select..." && ddlClaimSubCategory.SelectedValue.ToString().Length > 0)
                        { SubCategory = ddlClaimSubCategory.SelectedValue.ToString(); }
                        if (ddlobject.SelectedValue.ToString() != "-1" && ddlobject.SelectedValue.ToString().Length > 0)
                        { ObjectType = ddlobject.SelectedValue.ToString(); }
                        if (ddlobjectname.SelectedValue.ToString() != "Select..." && ddlobjectname.SelectedValue.ToString().Length > 0)
                        { ObjectCode = ddlobjectname.SelectedValue.ToString(); }
                        if (drptargetType.SelectedValue.ToString() == "0")
                        { cmd.CommandText = "ACX_Target_Sale"; }
                        else if (drptargetType.SelectedValue.ToString() == "3")
                        {
                            //cmd.CommandText = "ACX_Target_Handling";
                            cmd.CommandText = "ACX_TARGET_HANDLING_MONTHLY";
                        }
                        else if (drptargetType.SelectedValue.ToString() == "2")
                        {
                            cmd.CommandText = "ACX_TARGET_FREIGHT_MONTHLY";
                            //cmd.CommandText = "ACX_Target_Freight";
                        }
                        else if (drptargetType.SelectedValue.ToString() == "4")
                        {
                            cmd.CommandText = "ACX_Target_Electricity";
                        }

                        cmd.Parameters.Add("@SiteId", SqlDbType.NVarChar).Value = Convert.ToString(Session["SiteCode"]);
                        if (drptargetType.SelectedValue.ToString() == "2" || drptargetType.SelectedValue.ToString() == "3")
                        {
                            cmd.Parameters.Add("@EXPMONTH", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
                            cmd.Parameters.Add("@BUCODE", SqlDbType.NVarChar).Value = ddlBusinessUnit.SelectedItem.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd");
                            cmd.Parameters.Add("@ToDate", SqlDbType.SmallDateTime).Value = Convert.ToDateTime(txttoDate.Text).ToString("yyyy-MM-dd");
                            cmd.Parameters.Add("@TargetType", SqlDbType.Int).Value = drptargetType.SelectedValue.ToString();//Convert.ToString(Session["SiteCode"]);
                            cmd.Parameters.Add("@TargetCategory", SqlDbType.NVarChar).Value = CategoryName;// Convert.ToString(Session["SiteCode"]);
                            cmd.Parameters.Add("@TargetSubCategory", SqlDbType.NVarChar).Value = SubCategory;// Convert.ToString(Session["SiteCode"]);
                            cmd.Parameters.Add("@ObjectType", SqlDbType.NVarChar).Value = ObjectType;// Convert.ToString(Session["SiteCode"]);
                            cmd.Parameters.Add("@ObjectCode", SqlDbType.NVarChar).Value = ObjectCode;// Convert.ToString(Session["SiteCode"]);
                            cmd.Parameters.Add("@BUCODE", SqlDbType.NVarChar).Value = ddlBusinessUnit.SelectedItem.Value;// Convert.ToString(Session["SiteCode"]);
                        }

                        SqlDataAdapter ad = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        ad.Fill(dt);
                        Session["TargetIncentive"] = dt;
                        //cmd.Parameters.Clear();
                        //cmd.Parameters.AddWithValue("@SITE_CODE", Session["SiteCode"].ToString());

                        if (dt.Rows.Count > 0)
                        {
                            //GrvTarget.DataSource = dt;
                            //GrvTarget.DataBind();
                            grvDetail.Columns[0].Visible = true;
                            grvDetail.Columns[3].Visible = true;
                            grvDetail.Columns[7].Visible = true;
                            grvDetail.Columns[9].Visible = true;
                            grvDetail.Columns[14].Visible = true;
                            grvDetail.Columns[16].Visible = true;
                            grvDetail.Columns[22].Visible = true;
                            grvDetail.Columns[23].Visible = true;
                            grvDetail.Columns[24].Visible = true;
                            grvDetail.Visible = true;
                            grvDetail.DataSource = null;
                            grvDetail.DataBind();
                            grvDetail.DataSource = dt;
                            grvDetail.DataBind();
                            grvDetail.Columns[0].Visible = false;
                            grvDetail.Columns[3].Visible = false;
                            grvDetail.Columns[7].Visible = false;
                            grvDetail.Columns[9].Visible = false;
                            grvDetail.Columns[14].Visible = false;
                            grvDetail.Columns[16].Visible = false;
                            grvDetail.Columns[22].Visible = false;
                            grvDetail.Columns[23].Visible = false;
                            grvDetail.Columns[24].Visible = false;
                        }
                        else
                        {
                            //GrvTarget.DataSource = null;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Target", "alert('Data not found.')", true);
                            grvDetail.DataSource = null;
                            grvDetail.Visible = false;
                        }
                        btnSubmit.Visible = true;
                        btnsave.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    finally { btnSubmit.Visible = true; }
                }
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (drptargetType.SelectedValue.ToString() == "0")
                {
                    foreach (GridViewRow grv in grvDetail.Rows)
                    {
                        if (grv.Cells[23].Text.ToString().ToUpper() == "0")   // THIS BLOCK IS USED TO GIVE INCETIVE ALL THE ACHIVED TARGET
                        {
                            if (grv.Cells[11].Text.ToString().ToUpper() == "FIX")
                            {
                                if (Convert.ToDouble(grv.Cells[18].Text.ToString()) <= Convert.ToDouble(grv.Cells[20].Text.ToString()))
                                {
                                    grv.Cells[21].Text = Convert.ToDecimal(grv.Cells[19].Text).ToString("0.00");
                                }
                                else
                                { grv.Cells[21].Text = "0"; }
                            }
                            else if (grv.Cells[11].Text.ToString().ToUpper() == "MULTIPLE")
                            {
                                if (Convert.ToDouble(grv.Cells[18].Text.ToString()) <= Convert.ToDouble(grv.Cells[20].Text.ToString()))
                                {
                                    grv.Cells[21].Text = Convert.ToDecimal(Convert.ToDouble(grv.Cells[19].Text.ToString()) * Convert.ToDouble(grv.Cells[20].Text.ToString())).ToString("0.00");
                                }
                                else
                                { grv.Cells[21].Text = "0"; }
                            }
                            else if (grv.Cells[11].Text.ToString().ToUpper() == "PERCENTAGE")
                            {
                                if (Convert.ToDouble(grv.Cells[18].Text.ToString()) <= Convert.ToDouble(grv.Cells[20].Text.ToString()))
                                {
                                    grv.Cells[21].Text = Convert.ToDecimal((Convert.ToDouble(grv.Cells[19].Text.ToString())/100) * Convert.ToDouble(grv.Cells[20].Text.ToString())).ToString("0.00");
                                }
                                else
                                { grv.Cells[21].Text = "0"; }
                            }
                        }
                        if (grv.Cells[23].Text.ToString().ToUpper() == "1")         // THIS BLOCK IS USED TO CALCULATE THE HIGHEST TARGET ACHIVED
                        {
                            #region multiple calucalation
                            if (Convert.ToDouble(grv.Cells[18].Text.ToString()) <= Convert.ToDouble(grv.Cells[20].Text.ToString()))
                            {
                                DataTable dt = new DataTable();
                                if (Session["TargetIncentive"] != null)
                                { dt = (DataTable)Session["TargetIncentive"]; }
                                else
                                {
                                    string objecttype;
                                    if (grv.Cells[6].Text.ToString() == "Site")
                                        objecttype = "1";
                                    else if (grv.Cells[6].Text.ToString() == "PSR")
                                        objecttype = "0";
                                    else
                                        objecttype = "2";
                                    dt = GetData(Convert.ToDateTime(grv.Cells[4].Text.ToString()), Convert.ToDateTime(grv.Cells[5].Text.ToString()), drptargetType.SelectedValue.ToString(), grv.Cells[14].Text.ToString(), grv.Cells[16].Text.ToString(), objecttype, grv.Cells[7].Text.ToString());
                                }
                                string achiveamt;
                                achiveamt = Convert.ToDecimal(Convert.ToDouble(grv.Cells[18].Text)).ToString("0.00");
                                DataRow[] dr = dt.Select("siteid='" + grv.Cells[0].Text.ToString() + "' and TARGETCODE='" + grv.Cells[1].Text.ToString() + "' and Object='" + grv.Cells[6].Text.ToString() + "' and objectcode='" + grv.Cells[7].Text.ToString().Replace("&nbsp;", "") + "' and targetsubobject='" + grv.Cells[9].Text.ToString().Replace("&nbsp;", "") + "' and productgroup='" + grv.Cells[13].Text.ToString().Replace("&nbsp;", "") + "' and targetcategory='" + grv.Cells[14].Text.ToString().Replace("&nbsp;", "") + "' and targetsubcategory='" + grv.Cells[16].Text.ToString().Replace("&nbsp;", "") + "' and From_Date=#" + Convert.ToDateTime(grv.Cells[4].Text).ToString("yyyy-MM-dd") + "# and To_Date=#" + Convert.ToDateTime(grv.Cells[5].Text).ToString("yyyy-MM-dd") + "# and target>" + Convert.ToDouble(grv.Cells[18].Text.ToString()));
                                Boolean FoundGreater;
                                FoundGreater = false;
                                if (dr.Count() > 0)
                                {
                                    for (int i = 0; i < dr.Count(); i++)
                                    {
                                        if (Convert.ToDouble(dr[i][20].ToString()) < Convert.ToDouble(grv.Cells[20].Text.ToString()))
                                        {
                                            FoundGreater = true;
                                            break;
                                        }
                                    }
                                }
                                if (FoundGreater)
                                    grv.Cells[21].Text = "0";
                                else
                                {
                                    if (grv.Cells[11].Text.ToString().ToUpper() == "FIX")
                                    { grv.Cells[21].Text = grv.Cells[19].Text.ToString(); }
                                    else if (grv.Cells[11].Text.ToString().ToUpper() == "MULTIPLE")
                                    { grv.Cells[21].Text = Convert.ToDecimal(Convert.ToDouble(grv.Cells[19].Text.ToString()) * Convert.ToDouble(grv.Cells[20].Text.ToString())).ToString("0.00"); }
                                    else if (grv.Cells[11].Text.ToString().ToUpper() == "PERCENTAGE")
                                    { grv.Cells[21].Text = Convert.ToDecimal((Convert.ToDouble(grv.Cells[19].Text.ToString()) / 100) * Convert.ToDouble(grv.Cells[20].Text.ToString())).ToString("0.00"); }
                                }
                            }
                            else
                            { grv.Cells[21].Text = "0"; }
                            #endregion
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow grv in grvDetail.Rows)
                    {
                        grv.Cells[21].Text = grv.Cells[24].Text;
                    }
                }

                btnsave.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }                        
        }

        //protected void btnCalculate_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (drptargetType.SelectedValue.ToString() == "0")
        //        {
        //            foreach (GridViewRow grv in grvDetail.Rows)
        //            {
        //                if (grv.Cells[24].Text.ToString().ToUpper() == "0")   // THIS BLOCK IS USED TO GIVE INCETIVE ALL THE ACHIVED TARGET
        //                {
        //                    if (grv.Cells[12].Text.ToString().ToUpper() == "FIX")
        //                    {
        //                        if (Convert.ToDouble(grv.Cells[19].Text.ToString()) <= Convert.ToDouble(grv.Cells[21].Text.ToString()))
        //                        {
        //                            grv.Cells[22].Text = Convert.ToDecimal(grv.Cells[20].Text).ToString("0.00");
        //                        }
        //                        else
        //                        { grv.Cells[22].Text = "0"; }
        //                    }
        //                    if (grv.Cells[12].Text.ToString().ToUpper() == "MULTIPLE")
        //                    {
        //                        if (Convert.ToDouble(grv.Cells[19].Text.ToString()) <= Convert.ToDouble(grv.Cells[21].Text.ToString()))
        //                        {
        //                            grv.Cells[22].Text = Convert.ToDecimal(Convert.ToDouble(grv.Cells[20].Text.ToString()) * Convert.ToDouble(grv.Cells[21].Text.ToString())).ToString("0.00");
        //                        }
        //                        else
        //                        { grv.Cells[22].Text = "0"; }
        //                    }
        //                }
        //                if (grv.Cells[24].Text.ToString().ToUpper() == "1")         // THIS BLOCK IS USED TO CALCULATE THE HIGHEST TARGET ACHIVED
        //                {
        //                    #region multiple calucalation
        //                    if (Convert.ToDouble(grv.Cells[19].Text.ToString()) <= Convert.ToDouble(grv.Cells[21].Text.ToString()))
        //                    {
        //                        DataTable dt = new DataTable();
        //                        if (Session["TargetIncentive"] != null)
        //                        { dt = (DataTable)Session["TargetIncentive"]; }
        //                        else
        //                        {
        //                            string objecttype;
        //                            if (grv.Cells[6].Text.ToString() == "Site")
        //                                objecttype = "1";
        //                            else if (grv.Cells[6].Text.ToString() == "PSR")
        //                                objecttype = "0";
        //                            else
        //                                objecttype = "2";
        //                            dt = GetData(Convert.ToDateTime(grv.Cells[4].Text.ToString()), Convert.ToDateTime(grv.Cells[5].Text.ToString()), drptargetType.SelectedValue.ToString(), grv.Cells[15].Text.ToString(), grv.Cells[17].Text.ToString(), objecttype, grv.Cells[7].Text.ToString());
        //                        }
        //                        string achiveamt;
        //                        achiveamt = Convert.ToDecimal(Convert.ToDouble(grv.Cells[19].Text)).ToString("0.00");
        //                        DataRow[] dr = dt.Select("siteid='" + grv.Cells[0].Text.ToString() + "' and TARGETCODE='" + grv.Cells[1].Text.ToString() + "' and Object='" + grv.Cells[6].Text.ToString() + "' and objectcode='" + grv.Cells[7].Text.ToString().Replace("&nbsp;", "") + "' and targetsubobject='" + grv.Cells[10].Text.ToString().Replace("&nbsp;", "") + "' and productgroup='" + grv.Cells[14].Text.ToString().Replace("&nbsp;", "") + "' and targetcategory='" + grv.Cells[15].Text.ToString().Replace("&nbsp;", "") + "' and targetsubcategory='" + grv.Cells[17].Text.ToString().Replace("&nbsp;", "") + "' and From_Date=#" + Convert.ToDateTime(grv.Cells[4].Text).ToString("yyyy-MM-dd") + "# and To_Date=#" + Convert.ToDateTime(grv.Cells[5].Text).ToString("yyyy-MM-dd") + "# and target>" + Convert.ToDouble(grv.Cells[19].Text.ToString()));
        //                        Boolean FoundGreater;
        //                        FoundGreater = false;
        //                        if (dr.Count() > 0)
        //                        {
        //                            for (int i = 0; i < dr.Count(); i++)
        //                            {
        //                                if (Convert.ToDouble(dr[i][21].ToString()) < Convert.ToDouble(grv.Cells[21].Text.ToString()))
        //                                {
        //                                    FoundGreater = true;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                        if (FoundGreater)
        //                            grv.Cells[22].Text = "0";
        //                        else
        //                        {
        //                            if (grv.Cells[12].Text.ToString().ToUpper() == "FIX")
        //                            { grv.Cells[22].Text = grv.Cells[20].Text.ToString(); }
        //                            if (grv.Cells[12].Text.ToString().ToUpper() == "MULTIPLE")
        //                            { grv.Cells[22].Text = Convert.ToDecimal(Convert.ToDouble(grv.Cells[20].Text.ToString()) * Convert.ToDouble(grv.Cells[21].Text.ToString())).ToString("0.00"); }
        //                        }
        //                    }
        //                    else
        //                    { grv.Cells[22].Text = "0"; }
        //                    #endregion
        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (GridViewRow grv in grvDetail.Rows)
        //            {
        //                grv.Cells[22].Text = grv.Cells[25].Text;
        //            }
        //        }

        //        btnsave.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        lblError.Text = ex.Message;
        //    }
        //}

        protected void ddlClaimCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            if (ddlClaimCategory.SelectedIndex > 0)
            {
//                string sqlstr1 = @"select distinct TS.Name,TT.TargetSUBCATEGORY as Code from  [ax].[ACXTARGETTABLE] TT
//                                    left Join [ax].[ACXTARGETSUBCATEGORY] TS on   TS.DataAreaid =TT.DataAreaId
//                                    and TS.Category =TS.Category  and TT.TargetSUBCATEGORY = TS.SubCategory WHERE TS.CATEGORY='" + ddlClaimCategory.SelectedValue.ToString() + "'";

                string sqlstr1 = @"select distinct TS.Name,TS.SUBCATEGORY AS CODE from  [ax].[ACXTARGETSUBCATEGORY] TS where TS.CATEGORY='" + ddlClaimCategory.SelectedValue.ToString() + "'";

               
                ddlClaimSubCategory.Items.Clear();
                ddlClaimSubCategory.Items.Add("Select...");
                baseObj.BindToDropDown(ddlClaimSubCategory, sqlstr1, "Name", "Code");
            }
            else
            { ddlClaimSubCategory.Items.Clear(); }
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void ddlobject_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            if (ddlobject.SelectedIndex > 0)
            {
                string sqlstr1,strsubquery ;
                strsubquery = "";
                if (ddlobject.SelectedValue.ToString() == "0")
                {
                    sqlstr1 = @"select distinct OBJECTCODE,psr_name NAME from ax.ACXTARGETLINE JOIN AX.ACXPSRMaster ON PSR_Code=OBJECTCODE where TARGETOBJECT=0 ";
                }
                else if (ddlobject.SelectedValue.ToString() == "1")
                {
                    if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                        strsubquery = " and AX.INVENTSITE.SITEID='" + Convert.ToString(Session["SiteCode"]) +"'";
                    sqlstr1 = @"select distinct OBJECTCODE,NAME from ax.ACXTARGETLINE JOIN AX.INVENTSITE ON AX.INVENTSITE.SITEID=OBJECTCODE where TARGETOBJECT=1" + strsubquery;
                }
                else
                { sqlstr1 = @"select distinct TARGETSUBOBJECT OBJECTCODE,CUSTGROUP_NAME NAME from ax.ACXTARGETLINE JOIN AX.ACXCUSTGROUPMASTER ON CUSTGROUP_CODE=TARGETSUBOBJECT where TARGETOBJECT=2"; }
                
                ddlobjectname.Items.Clear();
                ddlobjectname.Items.Add("Select...");
                baseObj.BindToDropDown(ddlobjectname, sqlstr1, "Name", "OBJECTCODE");
                if (ddlobjectname.Items.Count == 2)
                    ddlobjectname.SelectedIndex = 1;
            }
            else
            { ddlobjectname.Items.Clear(); }
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = baseObj.GetConnection();
                SqlTransaction tran = null;
                SqlCommand cmd = new SqlCommand();
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
                    {
                        return;
                    }

                    if (con.State.ToString() == "Closed") { con.Open(); }
                    tran = con.BeginTransaction();
                    cmd.Transaction = tran;

                    Boolean FoundAlready, FoundSuccess;
                    FoundAlready = false;
                    FoundSuccess = false;
                    //foreach (GridViewRow grv in grvDetail.Rows)
                    for (int i = 0; i < grvDetail.Rows.Count; i++ )
                    {
                        string OBJECT = string.Empty;
                        if (grvDetail.Rows[i].Cells[6].Text.ToString() == "Site")
                            OBJECT = "1";
                        else if (grvDetail.Rows[i].Cells[6].Text.ToString() == "PSR")
                            OBJECT = "0";
                        else
                            OBJECT = "2";
                        string OBJECT_CODE = grvDetail.Rows[i].Cells[7].Text.ToString().Replace("&nbsp;", "");
                        string OBJECT_SUBCODE = grvDetail.Rows[i].Cells[9].Text.ToString().Replace("&nbsp;", "");
                        string TARGET_CODE = grvDetail.Rows[i].Cells[1].Text.ToString().Replace("&nbsp;", "");
                        string TARGET_DESCRIPTION = grvDetail.Rows[i].Cells[2].Text.ToString().Replace("&nbsp;", "");
                        string FROM_DATE = Convert.ToDateTime(grvDetail.Rows[i].Cells[4].Text).ToString("yyyy-MM-dd");
                        string TO_DATE = Convert.ToDateTime(grvDetail.Rows[i].Cells[5].Text).ToString("yyyy-MM-dd");
                        string TARGET = Convert.ToDouble(grvDetail.Rows[i].Cells[18].Text.ToString().Replace("&nbsp;", "")).ToString();
                        string ACHIVEMENT = Convert.ToDouble(grvDetail.Rows[i].Cells[20].Text.ToString().Replace("&nbsp;", "")).ToString();
                        string TARGET_INCENTIVE = Convert.ToDouble(grvDetail.Rows[i].Cells[19].Text.ToString().Replace("&nbsp;", "")).ToString();
                        string ACTUAL_INCENTIVE = Convert.ToDouble(grvDetail.Rows[i].Cells[21].Text.ToString().Replace("&nbsp;", "")).ToString();
                        string CLAIM_CATEGORY = grvDetail.Rows[i].Cells[14].Text.ToString().Replace("&nbsp;", "");
                        string CLAIM_SUBCATEGORY = grvDetail.Rows[i].Cells[16].Text.ToString().Replace("&nbsp;", "");
                        string CLAIM_TYPE = drptargetType.SelectedValue.ToString();
                        //string BUCODE = grvDetail.Rows[i].Cells[8].Text.ToString().Replace("&nbsp;", "");
                        string BUCODE = ddlBusinessUnit.SelectedItem.Value;
                        string STATUS = "0";
                        string MODIFIEDDATETIME = string.Empty;
                        string CREATEDDATETIME = string.Empty;
                        string TARGET_GROUP = grvDetail.Rows[i].Cells[14].Text.ToString().Replace("&nbsp;", ""); ;
                        string CALCULATION_PATTERN = grvDetail.Rows[i].Cells[12].Text.ToString().Replace("&nbsp;", ""); 
                        string sqlstr = @"SELECT * FROM AX.ACXCLAIMMASTER where SITE_CODE='" + Session["SiteCode"].ToString() + "' and FROM_DATE='" + FROM_DATE + "' ";
                        sqlstr += "and TO_DATE='" + TO_DATE + "' and OBJECT_CODE='" + OBJECT_CODE + "' AND OBJECT='" + OBJECT + "' and OBJECT_SUBCODE='" + OBJECT_SUBCODE + "' and TARGET_CODE='" + TARGET_CODE + "' ";
                        sqlstr += "and TARGET=" + TARGET + " and CLAIM_CATEGORY='" + CLAIM_CATEGORY + "' and CLAIM_SUBCATEGORY='" + CLAIM_SUBCATEGORY + "' and DATAAREAID='" + Session["DATAAREAID"] + "' and TARGET_GROUP='" + TARGET_GROUP + "' and BU_CODE = '" + BUCODE + "'";
                        dt = baseObj.GetData(sqlstr);
                        if (dt.Rows.Count == 0)
                        {
                            #region this for creating a single row of fix incentive
                            //for (int j = i + 1; j < grvDetail.Rows.Count; j++)
                            //{
                            //    string NEXT_OBJECT = string.Empty;
                            //    if (grvDetail.Rows[j].Cells[6].Text.ToString() == "Site")
                            //        NEXT_OBJECT = "1";
                            //    else if (grvDetail.Rows[j].Cells[6].Text.ToString() == "PSR")
                            //        NEXT_OBJECT = "0";
                            //    else
                            //        NEXT_OBJECT = "2";
                            //    string NEXT_OBJECT_CODE = grvDetail.Rows[j].Cells[7].Text.ToString().Replace("&nbsp;", "");
                            //    string NEXT_OBJECT_SUBCODE = grvDetail.Rows[j].Cells[9].Text.ToString().Replace("&nbsp;", "");
                            //    string NEXT_TARGET_CODE = grvDetail.Rows[j].Cells[1].Text.ToString().Replace("&nbsp;", "");
                            //    string NEXT_FROM_DATE = Convert.ToDateTime(grvDetail.Rows[j].Cells[4].Text).ToString("yyyy-MM-dd");
                            //    string NEXT_TO_DATE = Convert.ToDateTime(grvDetail.Rows[j].Cells[5].Text).ToString("yyyy-MM-dd");
                            //    string NEXT_ACTUAL_INCENTIVE = Convert.ToDouble(grvDetail.Rows[j].Cells[21].Text.ToString().Replace("&nbsp;", "")).ToString();
                            //    string NEXT_CLAIM_CATEGORY = grvDetail.Rows[j].Cells[14].Text.ToString().Replace("&nbsp;", "");
                            //    string NEXT_CLAIM_SUBCATEGORY = grvDetail.Rows[j].Cells[16].Text.ToString().Replace("&nbsp;", "");
                            //    string NEXT_TARGET_GROUP = grvDetail.Rows[j].Cells[13].Text.ToString().Replace("&nbsp;", ""); ;

                            //    if (FROM_DATE == NEXT_FROM_DATE && TO_DATE == NEXT_TO_DATE && OBJECT_CODE == NEXT_OBJECT_CODE && OBJECT == NEXT_OBJECT && TARGET_CODE == NEXT_TARGET_CODE && CLAIM_CATEGORY == NEXT_CLAIM_CATEGORY && CLAIM_SUBCATEGORY == NEXT_CLAIM_SUBCATEGORY && TARGET_GROUP == NEXT_TARGET_GROUP && OBJECT_SUBCODE == NEXT_OBJECT_SUBCODE)
                            //    {
                            //        ACTUAL_INCENTIVE += NEXT_ACTUAL_INCENTIVE;
                            //    }
                            //    else
                            //    { break; }
                            //}
                            #endregion
                            if (Convert.ToDouble(ACTUAL_INCENTIVE) > 0)
                            {
                                cmd.Connection = con;
                                cmd.Parameters.Clear();
                                cmd.CommandType = CommandType.StoredProcedure;
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
                                cmd.Parameters.Add("@BU_CODE", SqlDbType.NVarChar).Value = BUCODE;
                                cmd.Parameters.Add("CLAIM_MONTH", SqlDbType.NVarChar).Value = null;
                                cmd.ExecuteNonQuery();

                                grvDetail.Rows[i].Cells[23].Text = "Success";
                                FoundSuccess = true;
                            }

                        }
                        else
                        {
                            FoundAlready = true;
                            grvDetail.Rows[i].Cells[23].Text = "Already"; 
                        }
                    }
                    tran.Commit();
                    if (FoundAlready == true && FoundSuccess == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Some data already saved. Except that saved successfully.');", true);
                    }
                    else if (FoundAlready)
                    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data already saved.');", true); }
                    else
                    { ScriptManager.RegisterStartupScript(this, typeof(Page), "Information", "alert('Data saved successfully.');", true); }
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
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void drptargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grvDetail.DataSource = null;
                grvDetail.Visible = false;
                if (drptargetType.SelectedValue.ToString() == "0")
                {
                    ddlobject.Enabled = true;
                    ddlobject.SelectedValue = "-1";
                    btnsave.Visible = false;
                }
                else
                {
                    trCategory.Visible = true;
                    if (drptargetType.SelectedValue.ToString() == "2")
                    {
                        trCategory.Visible = false;
                    }
                    if (drptargetType.SelectedValue.ToString() == "3")
                    {
                        trCategory.Visible = false;
                    }
                    ddlobject.Enabled = false;
                    ddlobject.SelectedValue = "2";
                    btnsave.Visible = true;
                }
            }
            catch (Exception ex) { ErrorSignal.FromCurrentContext().Raise(ex); }
        }

        protected void txtFromDate_TextChanged(object sender, EventArgs e)
        {
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
        }

        protected void txttoDate_TextChanged(object sender, EventArgs e)
        {
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
        }

        protected void ddlClaimSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
        }

        protected void ddlobjectname_SelectedIndexChanged(object sender, EventArgs e)
        {
            grvDetail.DataSource = null;
            grvDetail.Visible = false;
        }

        protected void imgBtnExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (grvDetail.Rows.Count > 0)
            {
                //ExportToExcel();
                ExportToExcelNew();
            }
            else
            {
                this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Cannot Export Data due to No Records available. !');", true);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        private void ExportToExcelNew()
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                string FileName = "SaleAutoClaim" + DateTime.Now + ".xls";
                System.IO.StringWriter strwritter = new System.IO.StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                grvDetail.GridLines = GridLines.Both;
                grvDetail.HeaderStyle.Font.Bold = true;
                string Header = "";
                Header = "";
                if (ddlClaimCategory.SelectedIndex > 0)
                    Header += "<td> Claim Category : </td><td>" + ddlClaimCategory.SelectedItem.Text.ToString() + "</td>";
                if (ddlClaimSubCategory.SelectedIndex > 0)
                    Header += "<td> Claim Sub Category : </td><td>" + ddlClaimSubCategory.SelectedItem.Text.ToString() + "</td>";
                if (ddlobject.SelectedIndex > 0)
                    Header += "<td> Object : </td><td>" + ddlobject.SelectedItem.Text.ToString() + "</td>";
                grvDetail.RenderControl(htmltextwrtter);
                {
                    Response.Write("<table> <tr><td>  Auto Sale Claim Calucation </td></tr> <tr><td><b>From Date:  " + txtFromDate.Text + "</b></td><td></td> <td><b>To Date: " + txttoDate.Text + "</b></td></tr><tr><b>" + Header + "</b><tr></table>");
                }
                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void ddlBUnit()
        {
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            ddlBusinessUnit.Items.Add("...Select...");
            baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");
        }
    }
}