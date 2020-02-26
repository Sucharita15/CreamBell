using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps
{
    public partial class frmCustomerPartyMasterNew : System.Web.UI.Page
    {
        static string strid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (Request.QueryString["CustID"] != null && this.txtCustomerName.Text.ToString() == string.Empty)
            {
                string custid= Request.QueryString["CustID"].ToString();
                strid = Request.QueryString["ID"].ToString();
                ShowData(custid);
            }
            
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (strid == "CartOperator")
            {
                Response.Redirect("~/frmCartOperatorMaster.aspx");
            }
            if (strid == "CustParty")
            {
                Response.Redirect("~/frmCustomerPartyMaster.aspx");
            }
        }

        private void ShowData(string CustID)
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

                string query = " Select *, B.CUSTGROUP_NAME, C.PSR_Name,D.BeatName,CG.Description As ChannelGroupName,CT.SubSegmentDescription As ChannelTypeName " +
                               " FROM  [ax].[ACXCUSTMASTER] A " +
                               " LEFT OUTER JOIN [AX].[ACXSMMBUSRELSEGMENTGROUP] CG ON CG.SegmentID= A.CHANNELGROUP " +
                               " LEFT OUTER JOIN [AX].[ACXSMMBUSRELSUBSEGMENTGROUP] CT ON CT.SubSegmentID= A.CHANNELTYPE " +
                               " LEFT OUTER JOIN ax.ACXCUSTGROUPMASTER B ON A.CUST_GROUP=B.CUSTGROUP_CODE " +
                               " LEFT OUTER JOIN [ax].[ACXPSRMaster] C ON A.PSR_CODE=C.PSR_Code LEFT OUTER JOIN " +
                               " [ax].[ACXPSRBeatMaster] D ON A.PSR_BEAT = D.BeatCode and A.SITE_CODE=D.Site_code where Customer_Code ='" + CustID + "' and A.BLOCKED=0";

                System.Data.DataTable dt = obj.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    LiteralCustID.Text = "[" + Request.QueryString["CustID"].ToString() + "] ";
                    txtCustomerName.Text = dt.Rows[0]["Customer_Name"].ToString();
                    txtCustomerName.Font.Bold = true;
                    txtContactPerson.Text = dt.Rows[0]["Contact_Name"].ToString();
                    txtAddress1.Text = dt.Rows[0]["Address1"].ToString();
                    txtAddress2.Text = dt.Rows[0]["Address2"].ToString();
                    txtCity.Text = dt.Rows[0]["City"].ToString();
                    txtZipCode.Text = dt.Rows[0]["ZipCode"].ToString();
                    txtArea.Text = dt.Rows[0]["Area"].ToString();
                    txtDistrict.Text = dt.Rows[0]["District"].ToString();
                    txtState.Text = dt.Rows[0]["State"].ToString();
                    txtMobileNo.Text = dt.Rows[0]["Mobile_No"].ToString();
                    txtMobileNo.Font.Bold = true;
                    txtPhoneNo.Text = dt.Rows[0]["Phone_No"].ToString();
                    txtEmailID.Text = dt.Rows[0]["EmailId"].ToString();
                    txtPaymentTerm.Text = dt.Rows[0]["Payment_Term"].ToString();
                    txtPaymentMode.Text = dt.Rows[0]["Payment_Mode"].ToString();
                    txtPAN.Text = dt.Rows[0]["Pan"].ToString();
                    txtTINVAT.Text = dt.Rows[0]["VAT"].ToString();
                    txtCST.Text = dt.Rows[0]["CST"].ToString();
                    txtTAN.Text = dt.Rows[0]["TAN"].ToString();
                    txtRegDate.Text = dt.Rows[0]["Register_Date"].ToString();
                    txtClosingDate.Text = dt.Rows[0]["Closing_Date"].ToString();
                    txtCustGroup.Text = "[" + dt.Rows[0]["CUST_GROUP"].ToString() + "]" + " " + dt.Rows[0]["CUSTGROUP_NAME"].ToString();
                    txtCustGroup.Font.Bold = true;
                    txtDistance.Text = string.Empty;
                    txtPSRName.Text = "[" + dt.Rows[0]["PSR_Code"].ToString() + "]" + " " + dt.Rows[0]["PSR_Name"].ToString();
                    txtPSRName.Font.Bold = true;
                    txtPSRBeatName.Text = "[" + dt.Rows[0]["PSR_Beat"].ToString() + "]" + " " + dt.Rows[0]["BeatName"].ToString(); //+"/" + dt.Rows[0]["PSR_Day"].ToString();
                    txtPSRBeatName.Font.Bold = true;
                    txtDeepFreeer.Text = dt.Rows[0]["Deep_Frizer"].ToString();
                    txtDeepFreeer.Font.Bold = true;
                    txtDeepFreeer.ForeColor = System.Drawing.Color.DarkGreen;
                    //txtChannelType.Text = dt.Rows[0]["Channel_Type"].ToString();
                    txtChannelGroup.Text = dt.Rows[0]["ChannelGroupName"].ToString();
                    txtChannelType.Text = dt.Rows[0]["ChannelTypeName"].ToString();
                    txtMonday.Text = dt.Rows[0]["Monday"].ToString();
                    txtTuesday.Text = dt.Rows[0]["Tuesday"].ToString();
                    txtWednesday.Text = dt.Rows[0]["Wednesday"].ToString();
                    txtThursday.Text = dt.Rows[0]["Thursday"].ToString();
                    txtFriday.Text = dt.Rows[0]["Friday"].ToString();
                    txtSaturday.Text = dt.Rows[0]["Saturday"].ToString();
                    txtSunday.Text = dt.Rows[0]["Sunday"].ToString();
                    txtVisitFrequency.Text = dt.Rows[0]["VISITFREQUENCY"].ToString();
                    txtRepeatWeek.Text = dt.Rows[0]["REPEATWEEK"].ToString();
                    txtSequenceno.Text = dt.Rows[0]["SEQUENCENO"].ToString();
                    if (Convert.ToInt32(dt.Rows[0]["Key_Customer"].ToString()) == 0)
                    {
                        txtKeyCustomer.Text = "No";
                        txtKeyCustomer.Font.Bold = true;
                        txtKeyCustomer.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        txtKeyCustomer.Text = "Yes";
                        txtKeyCustomer.Font.Bold = true;
                        txtKeyCustomer.ForeColor = System.Drawing.Color.Green;
                    }
                    //txtKeyCustomer.Text = dt.Rows[0]["Key_Customer"].ToString();
                    if (Convert.ToInt32(dt.Rows[0]["Blocked"].ToString()) == 0)
                    {
                        txtActiveInactive.Text = "Yes";
                        txtActiveInactive.Font.Bold = true;
                        txtActiveInactive.ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    else
                    {
                        txtActiveInactive.Text = "No";
                        txtActiveInactive.ForeColor = System.Drawing.Color.Red;
                    }
                    string APPLICABLESCHEMEDISCOUNT;
                    if (dt.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString() == "0")
                    { APPLICABLESCHEMEDISCOUNT = "None"; }
                    else if (dt.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString() == "1")
                    { APPLICABLESCHEMEDISCOUNT = "Scheme"; }
                    else if (dt.Rows[0]["APPLICABLESCHEMEDISCOUNT"].ToString() == "2")
                    { APPLICABLESCHEMEDISCOUNT = "Discount"; }
                    else
                    { APPLICABLESCHEMEDISCOUNT = "Both"; }
                    txtOutletType.Text = dt.Rows[0]["Outlet_Type"].ToString();
                    txtSchemeDisc.Text = APPLICABLESCHEMEDISCOUNT; //dt.Rows[0]["Scheme_Discount"].ToString();
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('No Data Exists !');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }        
    }
}