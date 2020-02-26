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
    public partial class frmManualClaim : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (!Page.IsPostBack)
            {
                try
                {
                   // string sqlstr = @"select  distinct TC.Name,TC.Category as Code from  [ax].[ACXTARGETTABLE] TT Inner Join [ax].[ACXTARGETCATEGORY] TC  on TT.TARGETCATEGORY = TC.Category and TT.DataAreaId = TC.DataAreaid ";
                    string sqlstr = @"select  distinct TC.Name,TC.Category as Code from  [ax].[ACXTARGETCATEGORY] TC ";
                  //  string sqlstr1 = @"Select Distinct TS.Name,TS.Subcategory from [ax].[ACXTARGETSUBCATEGORY] TS ";
                    ddlClaimCategory.Items.Add("Select...");
                   // ddlClaimSubCategory.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlClaimCategory, sqlstr, "Name", "Code");
                    //baseObj.BindToDropDown(ddlClaimSubCategory, sqlstr1, "Name", "Code");

                    string sqlstr2 = @"Select Distinct PM.PSR_Code as Code,PM.PSR_Code +'-'+ PM.PSR_Name as Name from [ax].[ACXPSRCUSTLinkingMaster] PLM
                        inner join [ax].[ACXPSRMaster] PM on PLM.PSRCode = PM.PSR_Code
                        where Site_Code = '" + Session["SiteCode"].ToString() + "'";

                    ddlObjectCode.Items.Clear();
                    ddlObjectCode.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlObjectCode, sqlstr2, "Name", "Code");
                    string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
                    ddlBusinessUnit.Items.Add("Select...");
                    baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");


                    creataDataTabel();
                    ddlObjectType_SelectedIndexChanged(ddlObjectType, new EventArgs());



                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }
        public void creataDataTabel()
        {
            DataTable dtLine = new DataTable();
            dtLine.Columns.Add("DateFrom");
            dtLine.Columns.Add("DateTo");
            dtLine.Columns.Add("ClaimCat");
            dtLine.Columns.Add("CatName");
            dtLine.Columns.Add("ClaimSub");
            dtLine.Columns.Add("SubCatName");
            dtLine.Columns.Add("Object");
            dtLine.Columns.Add("SubObject");
            dtLine.Columns.Add("ObjectCode");
            dtLine.Columns.Add("BUName");
            dtLine.Columns.Add("Description");
            dtLine.Columns.Add("Incentive");
            dtLine.Columns.Add("BUCode");
            ViewState["dtLine"] = dtLine;

        }

        public bool validateData()
        {
            if (txtFromDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select from Date... !');", true);
                return false;
            }
            if (txttoDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select To Date... !');", true);
                return false;
            }
           
            if (ddlClaimCategory.SelectedItem.Text == "Select...")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Claim Category... !');", true);
                return false;
            }
            if (ddlClaimSubCategory.SelectedItem.Text == "Select...")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Claim Sub_Category... !');", true);
                return false;
            }
            if (ddlObjectCode.SelectedItem.Text == "Select...")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Object Cade... !');", true);
                return false;
            }

            if (ddlBusinessUnit.SelectedItem.Text == "Select...")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Select Business Unit... !');", true);
                return false;
            }

            if (txtDescription.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Description Should not Empty... !');", true);
                return false;
            }
            if (txtIncentive.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Incentive Should not Empty... !');", true);
                return false;
            }
            return true;
        }

        public bool checkifDataAlreadyExist()
        {
            try { 
            string ClaimType = "0";
            if (ddlObjectType.SelectedItem.Text == "PSR") { ClaimType = "0"; } if (ddlObjectType.SelectedItem.Text == "SITE") { ClaimType = "1"; } if (ddlObjectType.SelectedItem.Text == "CUSTOMERGROUP") { ClaimType = "2"; }

            string sqstr = @"Select * from [ax].[ACXCLAIMMASTER] where FROM_DATE ='"+ txtFromDate.Text +"' and TO_DATE = '"+txttoDate.Text+"' " +
                            " and SITE_CODE = '"+Session["SiteCode"].ToString()+"' and CLAIM_CATEGORY= '"+ddlClaimCategory.SelectedItem.Value+"' " +
                            " and CLAIM_TYPE = '" + ClaimType + "'  and OBJECT_CODE = '"+ddlObjectCode.SelectedItem.Value +"' and BU_CODE= '"+ddlBusinessUnit.SelectedItem.Value +"'";

            if (ddlClaimSubCategory.SelectedIndex > 0)
            {
                sqstr += " and CLAIM_SUBCATEGORY = '" + ddlClaimSubCategory.SelectedItem.Value + "' "; 
            }

            object obj = baseObj.GetScalarValue(sqstr);
            if (obj != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Data already exist for combination.. !');", true);
                return false;
            }
            else
            {
                return true ;
            }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
           
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            bool check = true;
            try
            {
                check = validateData();
                if (check)
                {
                    try
                    {
                        Convert.ToDateTime(txtFromDate.Text);
                    }
                    catch(Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('From Date is not in Valid format !');", true);
                        check = false;
                    }
                    try
                    {
                        Convert.ToDateTime(txttoDate.Text);
                    }
                    catch( Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('To Date is not in Valid format !');", true);
                        check = false;
                    }
                    check = checkifDataAlreadyExist();
                    if (check)
                    {

                        if (grvDetail.Rows.Count > 0)
                        {
                            string subobject = " " ;
                            if(ddlSubObejctType.SelectedIndex == -1)
                            {
                                subobject = " " ;
                            }
                            else
                            {
                                subobject = ddlSubObejctType.SelectedItem.Value ;
                            }

                            foreach(GridViewRow grv1 in grvDetail.Rows)
                            {
                                if (grv1.Cells[0].Text == txtFromDate.Text && grv1.Cells[1].Text == txttoDate.Text && grv1.Cells[2].Text == ddlClaimCategory.SelectedItem.Value && grv1.Cells[4].Text == ddlClaimSubCategory.SelectedItem.Value && grv1.Cells[6].Text == ddlObjectType.SelectedItem.Text && grv1.Cells[7].Text == subobject && grv1.Cells[8].Text == ddlObjectCode.SelectedItem.Value)
                                {
                                    check = false;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", " alert('Line already exist.. !');", true);
                                }
                            }
                        }
                        if (check)
                        {

                            if (grvDetail.Rows.Count <= 0)
                            {
                                if (ViewState["dtLine"] != null)
                                {
                                    DataTable dtline = (DataTable)ViewState["dtLine"];
                                    dtline.Rows.Add();
                                    dtline.Rows[0]["DateFrom"] = txtFromDate.Text;
                                    dtline.Rows[0]["DateTo"] = txttoDate.Text;
                                    dtline.Rows[0]["ClaimCat"] = ddlClaimCategory.SelectedItem.Value;
                                    dtline.Rows[0]["CatName"] = ddlClaimCategory.SelectedItem.Text;
                                    dtline.Rows[0]["ClaimSub"] = ddlClaimSubCategory.SelectedItem.Value;
                                    dtline.Rows[0]["SubCatName"] = ddlClaimSubCategory.SelectedItem.Text;
                                    dtline.Rows[0]["Object"] = ddlObjectType.SelectedItem.Value;
                                    string ddlSubObejctType1 = string.Empty;
                                    if (ddlSubObejctType.SelectedIndex == -1)
                                    {
                                        ddlSubObejctType1 = " ";
                                    }
                                    else
                                    {
                                        ddlSubObejctType1 = ddlSubObejctType.SelectedItem.Value;
                                    }
                                    dtline.Rows[0]["SubObject"] = ddlSubObejctType1;
                                    dtline.Rows[0]["ObjectCode"] = ddlObjectCode.SelectedItem.Value;
                                    dtline.Rows[0]["BUName"] = ddlBusinessUnit.SelectedItem.Text;
                                    dtline.Rows[0]["Description"] = txtDescription.Text;
                                    dtline.Rows[0]["Incentive"] = txtIncentive.Text;
                                    dtline.Rows[0]["BUCode"] = ddlBusinessUnit.SelectedItem.Value;
                                    ViewState["dtLine"] = dtline;
                                }
                            }
                            else
                            {
                                DataTable dtline = (DataTable)ViewState["dtLine"];
                                int Count = grvDetail.Rows.Count;
                                dtline.Rows.Add();
                                dtline.Rows[Count]["DateFrom"] = txtFromDate.Text;
                                dtline.Rows[Count]["DateTo"] = txttoDate.Text;
                                dtline.Rows[Count]["ClaimCat"] = ddlClaimCategory.SelectedItem.Value;
                                dtline.Rows[Count]["CatName"] = ddlClaimCategory.SelectedItem.Text;
                                dtline.Rows[Count]["ClaimSub"] = ddlClaimSubCategory.SelectedItem.Value;
                                dtline.Rows[Count]["SubCatName"] = ddlClaimSubCategory.SelectedItem.Text;
                                dtline.Rows[Count]["Object"] = ddlObjectType.SelectedItem.Value;
                                string ddlSubObejctType1 = string.Empty;
                                if (ddlSubObejctType.SelectedIndex == -1)
                                {
                                    ddlSubObejctType1 = " ";
                                }
                                else
                                {
                                    ddlSubObejctType1 = ddlSubObejctType.SelectedItem.Value;
                                }

                                dtline.Rows[Count]["SubObject"] = ddlSubObejctType1;
                                dtline.Rows[Count]["ObjectCode"] = ddlObjectCode.SelectedItem.Value;
                                dtline.Rows[Count]["BUName"] = ddlBusinessUnit.SelectedItem.Text;
                                dtline.Rows[Count]["Description"] = txtDescription.Text;
                                dtline.Rows[Count]["Incentive"] = txtIncentive.Text;
                                dtline.Rows[Count]["BUCode"] = ddlBusinessUnit.SelectedItem.Value;
                                ViewState["dtLine"] = dtline;
                            }

                            DataTable data = (DataTable)ViewState["dtLine"];
                            if (data.Rows.Count > 0)
                            {
                                grvDetail.DataSource = data;
                                grvDetail.DataBind();
                                grvDetail.Visible = true;

                            }
                            else
                            {
                                grvDetail.DataSource = null;
                            }
                            updatepnlGrid.Update();
                            Cleardata();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                lblError.Text = ex.Message;
            }
        
        }

        protected void ddlClaimCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sqlstr1 = @"Select Distinct  TS.Name as Name,TS.Subcategory as Code from [ax].[ACXTARGETSUBCATEGORY] TS
									 where CATEGORY  ='" + ddlClaimCategory.SelectedItem.Value + "'";

            ddlClaimSubCategory.Items.Clear();
            ddlClaimSubCategory.Items.Add("Select...");
            baseObj.BindToDropDown(ddlClaimSubCategory, sqlstr1, "Name", "Code");
        }

        protected void ddlObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlObjectType.SelectedItem.Text == "PSR")
            {
                string strQuery = @"Select PSR_Code +'_'+ PSR_Name as PSRName,PSR_Code from [ax].[ACXPSRMaster] where PSR_Code  " +
                         " in (select A.PSRCode from [ax].[ACXPSRBeatMaster] A  " +
                         " left Join [ax].[ACXPSRSITELinkingMaster] B on A.PSRCode = B.PSRCode " +
                         " where B.Site_code ='" + Session["SiteCode"].ToString() + "')";
              
                ddlSubObejctType.Items.Clear();
                ddlObjectCode.Items.Clear();
                ddlObjectCode.Items.Add("Select...");
                baseObj.BindToDropDown(ddlObjectCode, strQuery, "PSRName", "PSR_Code");
              
            }
            if (ddlObjectType.SelectedItem.Text == "SITE")
            {
                ddlSubObejctType.Items.Clear();
                ddlObjectCode.Items.Clear();
                ddlObjectCode.Items.Add("Select...");
                ddlObjectCode.Items.Add(Session["SiteCode"].ToString());
                ddlObjectCode.SelectedIndex = 1; 
                
            }
            if (ddlObjectType.SelectedItem.Text == "CUSTOMERGROUP")
            {
                string sqlstr = @"select Distinct CUSTGROUP_CODE as Code,CUSTGROUP_NAME as Name from [ax].[ACXCUSTGROUPMASTER]";
                ddlSubObejctType.Items.Clear();
                ddlSubObejctType.Items.Add("Select...");
                baseObj.BindToDropDown(ddlSubObejctType, sqlstr, "Name", "Code");
            }
        }

        protected void ddlSubObejctType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strQuery = "Select Customer_Code+'-'+Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 AND CUST_GROUP='" + ddlSubObejctType.SelectedValue + "' AND SITE_CODE='" + Session["SiteCode"].ToString() + "'"; //--AND SITE_CODE='657546'";
            ddlObjectCode.Items.Clear();
            ddlObjectCode.Items.Add("Select...");
            baseObj.BindToDropDown(ddlObjectCode, strQuery, "Name", "Customer_Code");
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            SqlConnection con = baseObj.GetConnection();
            SqlCommand cmd = new SqlCommand();
            SqlTransaction tran = null;
            try
            {
                if (grvDetail.Rows.Count <= 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Line Exist in GridView...!');", true);
                    return;
                }

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
                cmd.Connection = con;
                cmd.Transaction = tran;
               
                foreach (GridViewRow grv in grvDetail.Rows)
                {
                    string OBJECT_CODE = grv.Cells[8].Text;
                    string OBJECT_SUBCODE = grv.Cells[7].Text; 
                    string TARGET_CODE = "";
                    string OBJECT = "";
                    string TARGET_DESCRIPTION = grv.Cells[10].Text;
                    string FROM_DATE = grv.Cells[0].Text;
                    string TO_DATE = grv.Cells[1].Text;
                    string TARGET = "0";
                    string ACHIVEMENT = "0";
                    string TARGET_INCENTIVE = "0";
                    string ACTUAL_INCENTIVE = grv.Cells[11].Text;
                    string CLAIM_CATEGORY = grv.Cells[2].Text;
                    string CLAIM_SUBCATEGORY = grv.Cells[4].Text;
                    string ClaimType = "0" ;
                    HiddenField hdnBUCode = (HiddenField)grv.FindControl("BUCode");
                    string BU_CODE = hdnBUCode.Value.ToString();
                    if (grv.Cells[6].Text == "PSR") { ClaimType = "0"; } if (grv.Cells[6].Text == "SITE") { ClaimType = "1"; } if (grv.Cells[6].Text == "CUSTOMERGROUP") { ClaimType = "2"; }

                    string CLAIM_TYPE = ClaimType;
                    string STATUS = "0";
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
                    cmd.Parameters.Add("@TARGET_GROUP", SqlDbType.NVarChar).Value = "";
                    cmd.Parameters.Add("@CALCULATION_PATTERN", SqlDbType.NVarChar).Value = "";
                    cmd.Parameters.Add("CLAIM_MONTH", SqlDbType.NVarChar).Value = FROM_DATE;
                    cmd.Parameters.Add("@BU_CODE", SqlDbType.NVarChar).Value = BU_CODE;

                    cmd.ExecuteNonQuery();
//                    string sqlstr = @"INSERT INTO [ax].[ACXCLAIMMASTER] ([DATAAREAID],[RECID]
//                                   ,[DOCUMENT_CODE],[DOCUMENT_DATE],[SITE_CODE],[OBJECT],[OBJECT_CODE],[OBJECT_SUBCODE],[TARGET_CODE]
//                                   ,[TARGET_DESCRIPTION],[FROM_DATE],[TO_DATE],[TARGET],[ACHIVEMENT],[TARGET_INCENTIVE],[ACTUAL_INCENTIVE],[CLAIM_CATEGORY]
//                                   ,[CLAIM_SUBCATEGORY],[CLAIM_TYPE],[STATUS],[NumSeq],[Target_Group],[Calculation_Pattern])
//                                    VALUES ( '" + Session["DATAAREAID"] + "','" + Recid + "','" + Doc_NO + "','" + System.DateTime.Now + "','" + Session["SiteCode"].ToString() + "','" + CLAIM_TYPE + "','" + OBJECT_CODE + "', " +
//                        " '" + OBJECT_SUBCODE + "', '" + TARGET_CODE + "','" + TARGET_DESCRIPTION + "','" + FROM_DATE + "','" + TO_DATE + "','" + TARGET + "','" + ACHIVEMENT + "', " +
//                        " '" + TARGET_INCENTIVE + "','" + ACTUAL_INCENTIVE + "','" + CLAIM_CATEGORY + "','" + CLAIM_SUBCATEGORY + "','0','" + STATUS + "','" + NUMSEQ + "','','')";

//                    cmd.Connection = con;
//                    cmd.CommandText = sqlstr;
//                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
                //string message = "alert('Data Saved Successfully : " + Doc_NO + "')";
                //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Data Saved Successfully : " + Doc_NO + "');", true);
                Cleardata();
                grvDetail.DataSource = null;
                grvDetail.DataBind();
                grvDetail.Visible = false;
                creataDataTabel();
                updatepnlGrid.Update();
                string message = "alert('Data Saved Successfully : " + Doc_NO + "')";
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);



            }
            catch (Exception ex)
            {
                tran.Rollback();
                lblError.Text = ex.Message;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            finally { if (con.State.ToString() == "Open") { con.Close(); }
            }
        }

        public void Cleardata()
        {
           
            txtIncentive.Text = "";
            txtDescription.Text = "";
            ddlBusinessUnit.Items.Clear();
            string query = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + Convert.ToString(Session["SiteCode"]) + "'";
            ddlBusinessUnit.Items.Add("Select...");
            baseObj.BindToDropDown(ddlBusinessUnit, query, "bu_desc", "bu_code");

            uplanel.Update();
        }
    }
}