using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Saplin.Controls;
using System.Web.Script.Serialization;
using ClosedXML.Excel;
using System.IO;
using Elmah;
using CreamBell_DMS_WebApps.App_Code;

namespace CreamBell_DMS_WebApps
{
    public partial class frmVRSSpecialDiscountReport : System.Web.UI.Page
    {
        CreamBell_DMS_WebApps.App_Code.Global baseObj = new CreamBell_DMS_WebApps.App_Code.Global();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null)
            {
                //Amol
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                // txtFromDate.Text = string.Format("{ 0:dd/MMM/yyyy}",DateTime.Today);
                //  txtToDate.Text = string.Format("{ 0:dd/MMM/yyyy}", DateTime.Today);
                baseObj.FillSaleHierarchy();
                fillHOS();
                //FillBusiness();
                fillCustomeGroup();
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    // DataView DtSaleHierarchy = (DataTable)HttpContext.Current.Session["SaleHierarchy"];
                    DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                    if (dt.Rows.Count > 0)
                    {
                        var dr_row = dt.AsEnumerable();
                        var test = (from r in dr_row
                                    select r.Field<string>("SALEPOSITION")).First<string>();



                        //string dr1 = dt.Select("SALEPOSITION").ToString();
                        if (test == "VP")
                        {

                            chkListHOS.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            //  chkAll_CheckedChanged(null, null);

                        }
                        else if (test == "GM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            //    chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "DGM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "RM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            //chkAll_CheckedChanged(null, null);
                        }
                        else if (test == "ZM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkListRM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            CheckBox4.Enabled = false;
                            CheckBox4.Checked = true;
                            // chkAll_CheckedChanged(null, null);

                        }
                        else if (test == "ASM")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkListRM.Enabled = false;
                            chkListZM.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            CheckBox4.Enabled = false;
                            CheckBox4.Checked = true;
                            CheckBox5.Enabled = false;
                            CheckBox5.Checked = true;
                            // chkAll_CheckedChanged(null, null);

                        }
                        else if (test == "EXECUTIVE")
                        {
                            chkListHOS.Enabled = false;
                            chkListVP.Enabled = false;
                            chkListGM.Enabled = false;
                            chkListDGM.Enabled = false;
                            chkListRM.Enabled = false;
                            chkListZM.Enabled = false;
                            chkListEXECUTIVE.Enabled = false;
                            chkAll.Enabled = false;
                            chkAll.Checked = true;
                            CheckBox1.Enabled = false;
                            CheckBox1.Checked = true;
                            CheckBox2.Enabled = false;
                            CheckBox2.Checked = true;
                            CheckBox3.Enabled = false;
                            CheckBox3.Checked = true;
                            CheckBox4.Enabled = false;
                            CheckBox4.Checked = true;
                            CheckBox5.Enabled = false;
                            CheckBox5.Checked = true;
                            CheckBox6.Enabled = false;
                            CheckBox6.Checked = true;

                            // chkAll_CheckedChanged(null, null);

                        }
                        lstSiteId_SelectedIndexChanged(null, null);
                        // fillCustomeGroup();
                    }
                }

                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    //tclabel.Width = "90%";
                    Panel1.Visible = true;

                }
                else
                {

                    //tclabel.Width = "100%";
                    Panel1.Visible = false;

                }
                if (Convert.ToString(Session["LOGINTYPE"]) == "0" && Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
                {
                    //  ShowCustomerMaster();
                }
            }
        }

        protected void fillCustomeGroup()
        {
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                DataView view = new DataView(dt);
                DataTable distinctValues = view.ToTable(true, "CUSTGROUP", "CUSTGROUPDESC");
                string CustList = "";
                foreach (DataRow row in distinctValues.Rows)
                {
                    if (CustList == "")
                    {
                        CustList += "'" + row["CUSTGROUP"].ToString() + "'";
                    }
                    else
                    {
                        CustList += ",'" + row["CUSTGROUP"].ToString() + "'";
                    }
                }
                string sqlstr1 = @"Select distinct CustGroup_Code + '-' + CustGroup_Name  as CustGroup_Name,CustGroup_Code from VW_ReportsCustGroup WHERE MENUCODE='M160' AND CustGroup_Code IN (" + CustList + ") Order BY CustGroup_Code";
                DataTable dt1 = baseObj.GetData(sqlstr1);
                lstCustGroup.DataSource = dt1;
                lstCustGroup.DataTextField = "CustGroup_Name";
                lstCustGroup.DataValueField = "CustGroup_Code";
                lstCustGroup.DataBind();
            }
            else
            {
                lstCustGroup.Items.Clear();
                string sqlstr1 = @"Select distinct CustGroup_Code + '-' + CustGroup_Name as CustGroup_Name,CustGroup_Code from VW_ReportsCustGroup WHERE MENUCODE='M160' Order BY CustGroup_Code";
                DataTable dt1 = baseObj.GetData(sqlstr1);
                lstCustGroup.DataSource = dt1;
                lstCustGroup.DataTextField = "CustGroup_Name";
                lstCustGroup.DataValueField = "CustGroup_Code";
                lstCustGroup.DataBind();
                //lstCust.Items.Insert(0, "All...");
            }
        }
        protected void FillCustomer()
        {
            string LstCustGroup = App_Code.Global.GetSelectCustGroup(ref lstCustGroup, true);
            string LstSiteList = App_Code.Global.GetSelectCustGroup(ref lstSiteId, true);
            string strCondition = "";
            string strCondition1 = "";
            lstCust.Items.Clear();
            string sqlstr1;
            if (LstSiteList != "")
            {
                strCondition = " AND SITE_CODE in (" + LstSiteList + ")";
                if (LstCustGroup != "")
                {
                    strCondition += " AND CUST_GROUP in (" + LstCustGroup + ")";
                }
                else
                {
                    strCondition += " AND CUST_GROUP in (select CustGroup_Code from VW_ReportsCustGroup WHERE MENUCODE='M160')";

                }
                strCondition1 = " AND SD.OTHER_SITE in (" + LstSiteList + ")";
                if (LstCustGroup != "")
                {
                    strCondition1 += " AND CUSTGROUP in (" + LstCustGroup + ")";
                }
                else
                {
                    strCondition1 += " AND CUST_GROUP in (select CustGroup_Code from VW_ReportsCustGroup WHERE MENUCODE='M160')";

                }
                //string sqlstr1 = "Select Customer_Code + '-' + Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 " + strCondition;
                //sqlstr1 = "Select DISTINCT Customer_Code + '-' + Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 " + strCondition;
                sqlstr1 = "Select DISTINCT Customer_Code + '-' + Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 " + strCondition + " UNION " +
                          "SELECT DISTINCT Customer_Code + '-' + Customer_Name as Name,Customer_Code FROM AX.ACXCUSTMASTER C JOIN AX.ACX_SDLINKING SD ON SD.SUBDISTRIBUTOR = C.CUSTOMER_CODE WHERE SD.Blocked = 0 " + strCondition1;
            }
            else
            {
                sqlstr1 = "Select DISTINCT Customer_Code + '-' + Customer_Name as Name,Customer_Code  from ax.ACXCUSTMASTER where Blocked = 0 and customer_code=''";
            }
            lstCust.Items.Add("All...");
            DataTable dt = baseObj.GetData(sqlstr1);
            lstCust.DataSource = dt;
            lstCust.DataTextField = "Name";
            lstCust.DataValueField = "Customer_Code";
            lstCust.DataBind();
            // }
        }
        private bool ValidateInput()
        {
            bool value = true;

            if (lstSiteId.SelectedValue == string.Empty)
            {
                value = false;
                string message = "alert('Please Select The SiteID  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }

            if (lstCustGroup.SelectedValue == string.Empty)
            {
                value = false;
                string message = "alert('Please Select The Customer Group  !');";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", message, true);

            }
            return value;
        }

        protected void BtnShowReport_Click(object sender, EventArgs e)
        {
            try
            {
                //bool b = ValidateInput();
                //if (b == true)
                //{
                BindGridViewCollectionData();
                //}
                uppanel.Update();
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void BindGridViewCollectionData()
        {
            string FromDate = txtFromDate.Text;
            string ToDate = txtToDate.Text;
            string condition = string.Empty;
            DataSet ds = new DataSet();

            string Stateid = App_Code.Global.GetSelectState(ref lstState, false);
            string siteid = App_Code.Global.GetSelectState(ref lstSiteId, false);
            string CustGroup = App_Code.Global.GetSelectState(ref lstCustGroup, false);
            string BuList = App_Code.Global.GetSelectBUList(ref lstbu, false);
            string CustList = App_Code.Global.GetSelectCustMaster(ref lstCust, false);
            string strQuery = "EXEC USP_GETVRSSPDISCOUNTREPORT '" + Stateid + "','" + siteid + "','" + BuList + "','" + CustGroup + "','" + CustList + "','" + Session["USERID"].ToString() + "','" + Session["LOGINTYPE"].ToString() + "','" + FromDate + "','" + ToDate + "' ";
            ds = baseObj.GetDsData(strQuery);
            if (ds.Tables.Count > 0)
            {
                gvDetails.DataSource = ds.Tables[0];
                gvDetails.DataBind();
                grdSummary.DataSource = ds.Tables[1];
                grdSummary.DataBind();
            }
        }


        protected void fillSiteAndState(DataTable dt)
        {
            string sqlstr = "";
            if (Convert.ToString(Session["ISDISTRIBUTOR"]) == "Y")
            {
                if (Convert.ToString(Session["LOGINTYPE"]) == "3")
                {
                    DataTable dtState = dt.DefaultView.ToTable(true, "STATE", "STATENAME");
                    dtState.Columns.Add("STATENAMES", typeof(string), "STATE + ' - ' + STATENAME");
                    lstState.Items.Clear();
                    DataRow dr = dtState.NewRow();

                    lstState.DataSource = dtState;
                    lstState.DataTextField = "STATENAMES";
                    lstState.DataValueField = "STATE";
                    lstState.DataBind();
                }
                else
                {
                    sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' AND I.SITEID='" + Convert.ToString(Session["SiteCode"]) + "' order by Name";
                    //ddlState.Items.Add("Select...");
                    // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                    DataTable dt1 = baseObj.GetData(sqlstr);
                    lstState.DataSource = dt1;
                    lstState.DataTextField = "Name";
                    lstState.DataValueField = "Code";
                    lstState.DataBind();
                }
            }
            else
            {
                sqlstr = "Select Distinct I.StateCode Code,I.StateCode + ' - ' + LS.Name AS Name from [ax].[INVENTSITE] I left join [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = I.STATECODE where I.STATECODE <>'' order by Name ";
                lstState.Items.Add("Select...");
                // baseObj.BindToDropDown(ddlState, sqlstr, "Name", "Code");
                DataTable dt1 = baseObj.GetData(sqlstr);
                lstState.DataSource = dt1;
                lstState.DataTextField = "Name";
                lstState.DataValueField = "Code";
                lstState.DataBind();
            }
            if (lstState.Items.Count == 1)
            {
                foreach (System.Web.UI.WebControls.ListItem litem in lstState.Items)
                {
                    litem.Selected = true;
                }
                lstState_SelectedIndexChanged(null, null);
            }
        }



        protected void fillHOS()
        {

            chkListHOS.Items.Clear();
            DataTable dtHOS = (DataTable)Session["SaleHierarchy"];
            DataTable uniqueCols = dtHOS.DefaultView.ToTable(true, "HOSNAME", "HOSCODE");
            chkListHOS.DataSource = uniqueCols;
            chkListHOS.DataTextField = "HOSNAME";
            chkListHOS.DataValueField = "HOSCODE";
            chkListHOS.DataBind();
            if (uniqueCols.Rows.Count == 1)
            {
                chkListHOS.Items[0].Selected = true;
                lstHOS_SelectedIndexChanged(null, null);
            }

            fillSiteAndState(dtHOS);
        }

        protected void lstHOS_SelectedIndexChanged(object sender, EventArgs e)
        {

            chkListVP.Items.Clear();
            chkListGM.Items.Clear();
            chkListDGM.Items.Clear();
            chkListRM.Items.Clear();
            chkListZM.Items.Clear();
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListHOS))
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                //chkListVP.Items.Clear();
                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "VPNAME", "VPCODE");
                chkListVP.DataSource = uniqueCols2;
                chkListVP.DataTextField = "VPNAME";
                chkListVP.DataValueField = "VPCODE";
                chkListVP.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListVP.Items[0].Selected = true;
                    lstVP_SelectedIndexChanged(null, null);
                }
                else
                {
                    chkListVP.Items[0].Selected = false;

                }

                fillSiteAndState(dt);
                uppanel.Update();
                // chkListGM.Items.Clear();
            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void lstVP_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListGM.Items.Clear();
            chkListDGM.Items.Clear();
            chkListRM.Items.Clear();
            chkListZM.Items.Clear();
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();

            if (CheckSelect(ref chkListVP))
            {

                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "GMNAME", "GMCODE");

                chkListGM.DataSource = uniqueCols2;
                chkListGM.DataTextField = "GMNAME";
                chkListGM.DataValueField = "GMCODE";
                chkListGM.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListGM.Items[0].Selected = true;
                    lstGM_SelectedIndexChanged(null, null);
                }
                else
                {
                    chkListGM.Items[0].Selected = false;
                }

                fillSiteAndState(dt);
                uppanel.Update();
            }
            lstState_SelectedIndexChanged(null, null);
        }


        protected void lstGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListDGM.Items.Clear();
            chkListRM.Items.Clear();
            chkListZM.Items.Clear();
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListGM))
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "DGMNAME", "DGMCODE");

                chkListDGM.DataSource = uniqueCols2;
                chkListDGM.DataTextField = "DGMNAME";
                chkListDGM.DataValueField = "DGMCODE";
                chkListDGM.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListDGM.Items[0].Selected = true;
                    lstDGM_SelectedIndexChanged(null, null);
                }
                else
                {
                    chkListDGM.Items[0].Selected = false;
                }
                fillSiteAndState(dt);
                uppanel.Update();
            }
            lstState_SelectedIndexChanged(null, null);
        }


        protected void lstDGM_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListRM.Items.Clear();
            chkListZM.Items.Clear();
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListDGM))
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "RMNAME", "RMCODE");
                chkListRM.DataSource = uniqueCols2;
                chkListRM.DataTextField = "RMNAME";
                chkListRM.DataValueField = "RMCODE";
                chkListRM.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListRM.Items[0].Selected = true;
                    lstRM_SelectedIndexChanged(null, null);

                }
                else
                {
                    chkListRM.Items[0].Selected = false;
                }
                fillSiteAndState(dt);

                uppanel.Update();
            }
            lstState_SelectedIndexChanged(null, null);
            //upsale.Update()
        }

        public Boolean CheckSelect(ref CheckBoxList ChkList)
        {
            foreach (System.Web.UI.WebControls.ListItem litem in ChkList.Items)
            {
                if (litem.Selected)
                {
                    return true;
                }
            }
            return false;
        }

        protected void lstRM_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListZM.Items.Clear();
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListRM))
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "ZMNAME", "ZMCODE");
                chkListZM.DataSource = uniqueCols2;
                chkListZM.DataTextField = "ZMNAME";
                chkListZM.DataValueField = "ZMCODE";
                chkListZM.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListZM.Items[0].Selected = true;
                    lstZM_SelectedIndexChanged(null, null);
                }
                else
                {

                }
                fillSiteAndState(dt);
                uppanel.Update();


            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void lstZM_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListZM))
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                chkListASM.Items.Clear();
                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "ASMNAME", "ASMCODE");
                chkListASM.DataSource = uniqueCols2;
                chkListASM.DataTextField = "ASMNAME";
                chkListASM.DataValueField = "ASMCODE";
                chkListASM.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListASM.Items[0].Selected = true;
                    lstASM_SelectedIndexChanged(null, null);

                }

                fillSiteAndState(dt);
                uppanel.Update();
            }
            lstState_SelectedIndexChanged(null, null);
        }
        protected void lstASM_SelectedIndexChanged(object sender, EventArgs e)
        {
            //chkListASM.Items.Clear();
            chkListEXECUTIVE.Items.Clear();
            if (CheckSelect(ref chkListASM))
            {
                DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                chkListEXECUTIVE.Items.Clear();
                DataTable uniqueCols2 = dt.DefaultView.ToTable(true, "EXECUTIVENAME", "EXECUTIVECODE");
                chkListEXECUTIVE.DataSource = uniqueCols2;
                chkListEXECUTIVE.DataTextField = "EXECUTIVENAME";
                chkListEXECUTIVE.DataValueField = "EXECUTIVECODE";
                chkListEXECUTIVE.DataBind();
                if (uniqueCols2.Rows.Count == 1)
                {
                    chkListEXECUTIVE.Items[0].Selected = true;
                }

                fillSiteAndState(dt);
                uppanel.Update();
            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void lstEXECUTIVE_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);

            fillSiteAndState(dt);
            uppanel.Update();
            lstState_SelectedIndexChanged(null, null);

        }
        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(chkAll, chkListHOS);
                lstHOS_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(chkAll, chkListHOS);
                chkListVP.Items.Clear();

            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox1, chkListVP);
                lstVP_SelectedIndexChanged(null, null);
            }
            else
            {

                CheckAll_CheckedChanged(CheckBox1, chkListVP);
                // chkListVP.Items.Clear();
                chkListGM.Items.Clear();
                //chkListRM.Items.Clear();
                //chkListZM.Items.Clear();
                //chkListASM.Items.Clear();

            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox2, chkListGM);
                lstGM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox2, chkListGM);
                // chkListGM.Items.Clear();
                chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();

            }

            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox3, chkListDGM);
                lstDGM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox3, chkListDGM);
                //chkListGM.Items.Clear();
                // chkListDGM.Items.Clear();
                chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();

            }

            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox4, chkListRM);
                lstRM_SelectedIndexChanged(null, null);
            }
            else
            {
                CheckAll_CheckedChanged(CheckBox4, chkListRM);
                //chkListGM.Items.Clear();
                // chkListDGM.Items.Clear();
                //chkListRM.Items.Clear();
                chkListZM.Items.Clear();
                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox5, chkListZM);
                //     chkListASM.Items.Clear();
                lstZM_SelectedIndexChanged(null, null);

            }
            else
            {
                CheckAll_CheckedChanged(CheckBox5, chkListZM);

                chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
            {
                CheckAll_CheckedChanged(CheckBox6, chkListASM);
                //     chkListASM.Items.Clear();
                lstASM_SelectedIndexChanged(null, null);

            }
            else
            {
                CheckAll_CheckedChanged(CheckBox6, chkListASM);

                //chkListASM.Items.Clear();
                chkListEXECUTIVE.Items.Clear();
            }
            lstState_SelectedIndexChanged(null, null);
        }

        protected void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            CheckAll_CheckedChanged(CheckBox7, chkListEXECUTIVE);
            lstState_SelectedIndexChanged(null, null);
            // chkListASM.DataSource = null;
        }
        protected void CheckAll_CheckedChanged(CheckBox CheckAll, CheckBoxList ChkList)
        {
            if (CheckAll.Checked == true)
            {
                for (int i = 0; i < ChkList.Items.Count; i++)
                {
                    ChkList.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < ChkList.Items.Count; i++)
                {
                    ChkList.Items[i].Selected = false;
                }
            }
        }


        protected void btnExport2Excel_Click(object sender, EventArgs e)
        {
            ExportToExcelNew();
        }
        private void ExportToExcelNew()
        {
            //CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();
            //SqlConnection conn = null;

            string FromDate = txtFromDate.Text;
            string ToDate = txtToDate.Text;

            try
            {
                //conn = new SqlConnection(obj.GetConnectionString());
                //conn.Open();
                //bool b = ValidateInput();
                //if (b == true)
                {
                    //GridView gvvc = new GridView();
                    //CreamBell_DMS_WebApps.App_Code.Global obj = new CreamBell_DMS_WebApps.App_Code.Global();

                    string Stateid = App_Code.Global.GetSelectState(ref lstState, false);

                    string siteid = App_Code.Global.GetSelectState(ref lstSiteId, false);

                    string CustGroup = App_Code.Global.GetSelectState(ref lstCustGroup, false);

                    string BuList = App_Code.Global.GetSelectBUList(ref lstbu, false);


                    string CustList = App_Code.Global.GetSelectCustMaster(ref lstCust, false);
                    string query = "EXEC USP_GETVRSSPDISCOUNTREPORT '"
                        + Stateid + "','"
                        + siteid + "','"
                        + BuList + "','"
                        + CustGroup + "','"
                        + CustList + "','"
                        + Session["USERID"].ToString() + "','"
                        + Session["LOGINTYPE"].ToString() + "','"
                        + FromDate + "','"
                        + ToDate + "' ";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter stateIdParam = new SqlParameter("@STATEID", SqlDbType.NVarChar, 5000);
                    stateIdParam.Value = Stateid;
                    sqlParameters.Add(stateIdParam);

                    SqlParameter siteIdParam = new SqlParameter("@SITEID", SqlDbType.NVarChar, 5000);
                    siteIdParam.Value = siteid;
                    sqlParameters.Add(siteIdParam);

                    SqlParameter buParam = new SqlParameter("@BU", SqlDbType.NVarChar, 5000);
                    buParam.Value = BuList;
                    sqlParameters.Add(buParam);

                    SqlParameter custgroupParam = new SqlParameter("@CUSTGROUP", SqlDbType.NVarChar, 5000);
                    custgroupParam.Value = CustGroup;
                    sqlParameters.Add(custgroupParam);

                    SqlParameter customerParam = new SqlParameter("@CUSTOMER", SqlDbType.NVarChar, 5000);
                    customerParam.Value = CustList;
                    sqlParameters.Add(customerParam);

                    SqlParameter userCodeParam = new SqlParameter("@USERCODE", SqlDbType.NVarChar, 20);
                    userCodeParam.Value = Session["USERID"].ToString();
                    sqlParameters.Add(userCodeParam);

                    SqlParameter userTypeParam = new SqlParameter("@USERTYPE", SqlDbType.Int);
                    userTypeParam.Value = Session["LOGINTYPE"].ToString();
                    sqlParameters.Add(userTypeParam);

                    SqlParameter fromDateParam = new SqlParameter("@FROMDATE", SqlDbType.DateTime);
                    fromDateParam.Value = FromDate;
                    sqlParameters.Add(fromDateParam);
                    SqlParameter toDateParam = new SqlParameter("@TODATE", SqlDbType.DateTime);
                    toDateParam.Value = ToDate;
                    sqlParameters.Add(toDateParam);
                    DataSet ds = CreamBellFramework.GetDataSetFromStoredProcedure("USP_GETVRSSPDISCOUNTREPORT", sqlParameters.ToArray());

                    // string query = "EXEC USP_GETVRSSPDISCOUNTREPORT '" + lstState.SelectedValue + "','" + Session["SiteCode"].ToString() + "','" + lstbu.SelectedValue + "','" + lstCustGroup.SelectedValue + "','" + lstCust.SelectedItem.Value + "','" + Session["USERID"].ToString() + "','" + Session["LOGINTYPE"].ToString() + "','" + FromDate + "','" + ToDate + "' ";                    
                    //using (SqlCommand cmd = new SqlCommand(query))
                    //{
                    //    cmd.CommandTimeout = 600;
                    //    using (SqlDataAdapter sda = new SqlDataAdapter())
                    //    {
                    //        cmd.Connection = obj.GetConnection();
                    //        sda.SelectCommand = cmd;
                    //        using (DataSet ds = new DataSet())
                    //        {
                    //sda.Fill(ds);
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[0], "Detail");
                        wb.Worksheets.Add(ds.Tables[1], "Summary");
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=VRSSpecialDiscountReport.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                    //        }
                    //    }
                    //}


                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                if (ex.Message.ToString() != "Thread was being aborted.")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Validation", "alert('" + ex.Message.ToString().Replace("'", "") + "');", true);
                }
                //LblMessage.Text = ex.Message.ToString();
            }
            //finally
            //{
            //    if (conn != null)
            //    {
            //        if (conn.State == ConnectionState.Open)
            //        {
            //            conn.Close();
            //            conn.Dispose();
            //        }
            //    }
            //}
        }
        protected void lstCustGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCustomer();
            //FillBusiness();
        }
        //protected void FillBusiness()
        //{
        //    if (Convert.ToString(Session["LOGINTYPE"]) == "3")
        //    {
        //        string sqlstr1 = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + lstSiteId.SelectedValue.ToString() + "'";
        //        DataTable dt = baseObj.GetData(sqlstr1);
        //        lstbu.DataSource = dt;
        //        lstbu.DataTextField = "bu_desc";
        //        lstbu.DataValueField = "bu_code";
        //        lstbu.DataBind();
        //    }
        //    else
        //    {
        //        lstbu.Items.Clear();
        //        string sqlstr1 = "select bm.bu_code,bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID = '" + lstSiteId.SelectedValue.ToString() + "'";
        //        lstbu.Items.Clear();
        //        lstbu.Items.Add("All...");
        //        DataTable dt = baseObj.GetData(sqlstr1);
        //        lstbu.DataSource = dt;
        //        lstbu.DataTextField = "bu_desc";
        //        lstbu.DataValueField = "bu_code";
        //        lstbu.DataBind();
        //    }
        //}
        protected void lstSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SiteList = App_Code.Global.GetSelectSite(ref lstSiteId, true);
            if (lstSiteId.SelectedIndex != -1)
            {
                string query = "SELECT distinct bm.BU_CODE,bm.BU_CODE + '-' + bu_desc as bu_desc from ax.acxsitebumapping sbp join ax.ACXBUMASTER bm on bm.bu_code = sbp.BU_CODE where SITEID in (" + SiteList + ")";
                lstbu.Items.Clear();
                lstbu.Items.Add("All...");
                DataTable dt1 = baseObj.GetData(query);
                lstbu.DataSource = dt1;
                lstbu.DataTextField = "bu_desc";
                lstbu.DataValueField = "BU_CODE";
                lstbu.DataBind();
            }
            else
            {
                lstbu.Items.Clear();
                lstbu.Items.Add("All...");
            }

            FillCustomer();
        }

        protected void lstState_SelectedIndexChanged(object sender, EventArgs e)
        {

            string StateList = App_Code.Global.GetSelectState(ref lstState, true);

            string strSqlQuery = "", strCondition = "";

            lstSiteId.Items.Clear();
            strSqlQuery = @"Select Distinct SITEID as Code,SITEID + ' - ' + NAME AS NAME,Name as SiteName from [ax].[INVENTSITE] IV JOIN AX.ACXUSERMASTER UM on IV.SITEID=UM.SITE_CODE ";
            if (Convert.ToString(Session["LOGINTYPE"]) == "3")
            {
                lstSiteId.Items.Clear();
                DataTable dt1 = App_Code.Global.HierarchyDataTable(ref chkListHOS, ref chkListVP, ref chkListGM, ref chkListDGM, ref chkListRM, ref chkListZM, ref chkListASM, ref chkListEXECUTIVE);
                DataView view = new DataView(dt1);
                DataTable distinctValues = view.ToTable(true, "Distributor", "DistributorName");
                string AllSitesFromHierarchy = "";
                foreach (DataRow row in distinctValues.Rows)
                {
                    if (AllSitesFromHierarchy == "")
                    {
                        AllSitesFromHierarchy += "'" + row["Distributor"].ToString() + "'";
                    }
                    else
                    {
                        AllSitesFromHierarchy += ",'" + row["Distributor"].ToString() + "'";
                    }
                }
                strCondition = " WHERE SITEID IN (" + AllSitesFromHierarchy + ")";
            }
            else
            {
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode != null)
                {

                }
                else
                {
                    strCondition = " WHERE SITEID='" + Session["SiteCode"].ToString() + "'";
                }
            }
            if (StateList != "")
                strCondition += (strCondition == "" ? " WHERE STATECODE in (" + StateList + ")" : " AND STATECODE in (" + StateList + ")");

            strCondition += " Order by SiteName";
            strSqlQuery += StateList = strCondition;

            DataTable dt = baseObj.GetData(strSqlQuery);
            lstSiteId.DataSource = dt;
            lstSiteId.DataTextField = "Name";
            lstSiteId.DataValueField = "Code";
            lstSiteId.DataBind();
            if (lstSiteId.Items.Count == 1)
            {
                foreach (System.Web.UI.WebControls.ListItem litem in lstSiteId.Items)
                {
                    litem.Selected = true;
                }
                lstSiteId_SelectedIndexChanged(sender, e);
            }
        }
    }
}
