using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CreamBell_DMS_WebApps.App_Code;
using System.Web.UI.HtmlControls;

namespace CreamBell_DMS_WebApps
{
    public partial class frmUserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["USERID"] == null || Session["USERID"].ToString() == string.Empty)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SesionLoginID.Value = Session["USERNAME"].ToString();
                SessionUserPopUp();
                ShowUserProfileData();

            }
        }

        private void ShowUserProfileData()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();

            string query = "   SELECT A.User_Code,A.User_Name, A.State,B.SiteID,B.Name as Site_Name,B.ACXMOBILE as MobileNo,B.ACXAddress1+','+B.ACXAddress2 +','+B.ACXCITY +','+B.ACXZIPCODE "
                           + " AS [Address],ISNULL(b.ACXTELEPHONE,'') ACXTELEPHONE,ISNULL(B.ACXVAT,'') ACXVAT,ISNULL(B.ACXPAN,'') ACXPAN ,LS.NAME AS STATENAME,"
                           + " GSTINNO,CASE WHEN YEAR(GSTREGISTRATIONDATE)=1900 THEN NULL ELSE GSTREGISTRATIONDATE END AS GSTREGISTRATIONDATE,CASE WHEN COMPOSITIONSCHEME='0' THEN '' ELSE COMPOSITIONSCHEME END AS COMPOSITION "
                           + " FROM [ax].[ACXUSERMASTER] A " 
                           + " LEFT JOIN [ax].[INVENTSITE] B on A.Site_Code = B.SiteID "
                           + " LEFT JOIN [ax].[LOGISTICSADDRESSSTATE] LS on LS.STATEID = B.STATECODE "
                           + " WHERE User_Code = '" + Session["USERID"].ToString() + "'";


            DataTable dtProfileDetails = obj.GetData(query);
            if (dtProfileDetails.Rows.Count > 0)
            {
                txtUserName.Text = dtProfileDetails.Rows[0]["User_Name"].ToString();
                txtUserID.Text = dtProfileDetails.Rows[0]["User_Code"].ToString();
                txtUserState.Text = dtProfileDetails.Rows[0]["STATENAME"].ToString();
                txtSiteName.Text = dtProfileDetails.Rows[0]["Site_Name"].ToString();
                txtMobileNo.Text = dtProfileDetails.Rows[0]["MobileNo"].ToString();
                txtAddress.Text = dtProfileDetails.Rows[0]["Address"].ToString();
                txtVat.Text = dtProfileDetails.Rows[0]["ACXVAT"].ToString();
                txtPAN.Text = dtProfileDetails.Rows[0]["ACXPAN"].ToString();
                txtPhone.Text = dtProfileDetails.Rows[0]["ACXTELEPHONE"].ToString();
                txtGSTNo.Text = dtProfileDetails.Rows[0]["GSTINNO"].ToString();
                txtRegnDate.Text = dtProfileDetails.Rows[0]["GSTREGISTRATIONDATE"].ToString();
                txtComposition.Text = (dtProfileDetails.Rows[0]["COMPOSITION"].ToString() == "0" ? "No" : "Yes");
            }
        }

        public void SessionUserPopUp()
        {
            //HtmlGenericControl divControl = new HtmlGenericControl("div");
            //divControl.Attributes.Add("id", "logindiv");
            //divControl.InnerHtml = "Welcome," + " " + SesionLoginID.Value;
            ////string cssStyle = "background-color:Orange;margin-top:43%;margin-left:80%;height:60px;width:230px;text-align:center;font-size:large;";
            ////cssStyle += "font-family:Verdana;font-weight:bold;color:MidnightBlue;font-style:italic;text-align:center;opacity:0.9";
            //string cssStyle = "-webkit-border-radius: 99px 0px 99px 11px;-moz-border-radius: 99px 0px 99px 11px;border-radius: 99px 0px 99px 11px; "
            //                 + " background-color:Orange;-webkit-box-shadow: #B3B3B3 20px 20px 20px;-moz-box-shadow: #B3B3B3 20px 20px 20px; box-shadow: #B3B3B3 20px 20px 20px; "
            //                 + "font-family:Verdana;font-weight:bold;color:MidnightBlue;font-style:italic;text-align:center;opacity:0.9;"
            //                 + "margin-left:80%;height:60px;width:230px;text-align:center;font-size:large;";
            //divControl.Attributes.Add("Style", cssStyle);
            ////divControl.Attributes.Add("class", "w3-deep-orange");
            //this.Controls.Add(divControl);
           
        }
    }
}