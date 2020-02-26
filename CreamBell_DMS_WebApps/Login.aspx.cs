using CreamBell_DMS_WebApps.App_Code;
using Elmah;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace CreamBell_DMS_WebApps
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection conn = null;
        SqlCommand cmd = null;
        SqlDataReader RD;

        string DataAreaID = System.Configuration.ConfigurationManager.AppSettings["DataAreaId"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                txtUserName.Focus();
                Session[SessionKeys.MENU] = null;
                BtnLogin.Attributes.Add("onclick", "return ValidateLogin();");
                Session.Clear();

            }

        }
        //        protected void BtnLogin_Click(object sender, EventArgs e)
        //        {
        //            //Response.Redirect("frmStockLocationTransfer.aspx");

        //            try
        //            {
        //                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
        //                conn = obj.GetConnection();
        //                string query = "Select User_Code,User_Name,User_Password,State,User_Type,Site_Code from [ax].[ACXUSERMASTER] A where User_Code=@User_Code and  User_Password = @User_Password and A.Site_Code=(select SITEID from AX.INVENTSITE where SITEID =A.Site_Code)";
        //                cmd = new SqlCommand();
        //                cmd.Connection = conn;
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.Text;

        //                cmd.CommandText = query;
        //                cmd.Parameters.Add(new SqlParameter("@User_Code", txtUserName.Text.Trim()));
        //                cmd.Parameters.Add(new SqlParameter("@User_Password", txtPassword.Text.Trim()));

        //                RD = cmd.ExecuteReader();
        //                if (RD.HasRows == false)
        //                {
        //                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Login Failed !');", true);
        //                    txtPassword.Text = string.Empty;
        //                    txtUserName.Focus();
        //                }
        //                else
        //                {

        //                    RD.Read();
        //                    Session["USERID"] = RD["User_Code"].ToString();
        //                    Session["USERNAME"] = RD["User_Name"].ToString();
        //                    Session["SITELOCATION"] = RD["State"].ToString();
        //                    Session["LOGINTYPE"] = RD["User_Type"].ToString();
        //                    Session["SiteCode"] = RD["Site_Code"].ToString();
        //                    Session["DATAAREAID"] = DataAreaID;
        //                    RD.Close();
        //                    UpdateLoginTime();
        //                    try
        //                    {
        //                        Session["ISDISTRIBUTOR"] = "N";
        //                        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
        //                        string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session["SiteCode"].ToString() + "'";
        //                        object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
        //                        if (objcheckSitecode == null)
        //                        {
        //                            Session["ISDISTRIBUTOR"] = "Y";
        //                        }
        //                    }
        //                    catch
        //                    { }
        //                    //Response.Redirect("frmCustomerPartyMaster.aspx");


        ////                    Response.Redirect("Home.aspx");

        //                    Server.Transfer("Home.aspx",false);

        //                    //Response.Redirect("frmPurchaseInvoiceReceipt.aspx");
        //                    //Response.Redirect("frmPurchaseReturn.aspx");
        //                    //Response.Redirect("ReportSalesInvoice.aspx");
        //                }
        //                if (RD.IsClosed == false)
        //                {
        //                    RD.Close();
        //                }

        //                if (conn.State == ConnectionState.Open)
        //                {
        //                    conn.Close();
        //                    conn.Dispose();
        //                }
        //            }
        //            catch (System.Data.SqlClient.SqlException sqlex)
        //            {
        //                if (sqlex.Message.ToString().IndexOf("The server was not found or was not accessible.") > 0)
        //                { ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Connection not established with server. Please check the network connection.');", true); return; }
        //                else
        //                {
        //                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + sqlex.Message.ToString().Replace("'","''") + "');", true); return;
        //                }

        //            }

        //            catch (Exception ex)
        //            {
        //                if (ex.Message.ToString().IndexOf("The timeout period elapsed prior to obtaining a connection from the pool.") > 0)
        //                { ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Connection not established with server. Please check the network connection.');", true); return; }
        //                else
        //                {
        //                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + ex.Message.ToString().Replace("'", "''") + "');", true); return;
        //                }
        //            }

        //        }       
        private void UpdateLoginTime()
        {
            string dt = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
            CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
            string updateQuery = "Update [ax].[ACXUSERMASTER] Set LastLoginDatetime ='" + dt + "' where User_Code ='" + Session[SessionKeys.USERID].ToString() + "'";

            obj.ExecuteCommand(updateQuery);
        }

        protected void BtnLogin_Click1(object sender, ImageClickEventArgs e)
        {
            //Response.Redirect("frmStockLocationTransfer.aspx");

            try
            {
                CreamBell_DMS_WebApps.App_Code.Global obj = new Global();
                conn = obj.GetConnection();
                string query = "Acx_getUserCredentials";
                //string query = "ACX_GetLoginDetails";
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = query;
                cmd.Parameters.Add(new SqlParameter("@User_Code", txtUserName.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@User_Password", txtPassword.Text.Trim()));

                RD = cmd.ExecuteReader();
                if (RD.HasRows == false)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Alert", " alert('Login Failed !');", true);
                    txtPassword.Text = string.Empty;
                    txtUserName.Focus();
                }
                else
                {
                    RD.Read();



                    Session[SessionKeys.USERID] = RD["User_Code"].ToString();
                    Session[SessionKeys.USERNAME] = RD["User_Name"].ToString();
                    Session[SessionKeys.NAME] = RD["NAME"].ToString();
                    Session[SessionKeys.SITEADDRESS] = RD["SITEADDRESS"].ToString();

                    Session[SessionKeys.SITELOCATION] = RD["State"].ToString();
                    Session[SessionKeys.STATECODE] = RD["STATECODE"].ToString();
                    Session[SessionKeys.SCHSTATE] = RD["SCHSTATE"].ToString();
                    Session[SessionKeys.UNIONTERRITORY] = RD["UNIONTERRITORY"].ToString();

                    Session[SessionKeys.SITEGSTINNO] = RD["GSTINNO"].ToString();
                    Session[SessionKeys.SITEGSTINREGDATE] = RD["GSTREGISTRATIONDATE"].ToString();
                    Session[SessionKeys.SITECOMPOSITIONSCHEME] = RD["COMPOSITIONSCHEME"];

                    Session[SessionKeys.LOGINTYPE] = RD["User_Type"].ToString();
                    Session[SessionKeys.SITECODE] = RD["Site_Code"].ToString();
                    Session[SessionKeys.DATAAREAID] = DataAreaID;
                    RD.Close();
                    Global.strSessionID = HttpContext.Current.Session.SessionID;

                    if (Convert.ToString(Session[SessionKeys.LOGINTYPE]) == "0")
                    {
                        string query1 = "select MainWarehouse from ax.inventsite where siteid='" + Session[SessionKeys.SITECODE].ToString() + "'";

                        string MainWarehouse = obj.GetScalarValue(query1);
                        if (MainWarehouse.Trim().Length > 0)
                        {
                            Session[SessionKeys.TRANSLOCATION] = MainWarehouse;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Main Warehouse setup not defined.');", true); return;
                        }
                    }
                    //UpdateLoginTime();
                    try
                    {
                        Session[SessionKeys.ISDISTRIBUTOR] = "N";
                        CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
                        string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session[SessionKeys.SITECODE].ToString() + "'";
                        object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                        if (objcheckSitecode == null)
                        {
                            Session[SessionKeys.ISDISTRIBUTOR] = "Y";
                        }
                        // Pcs Billing Applicable
                        Session["ApplicableOnState"] = "N";
                        sqlstr = "EXEC [dbo].[ACX_USP_PCSBillingApplicable] '" + Session[SessionKeys.SCHSTATE].ToString() + "',''";
                        objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                        if (objcheckSitecode == null)
                        {
                            Session[SessionKeys.APPLICABLEONSTATE] = "N";// objcheckSitecode.ToString();
                        }
                        else
                        {
                            Session[SessionKeys.APPLICABLEONSTATE] = "Y";
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                    //Response.Redirect("frmCustomerPartyMaster.aspx");


                    //                    Response.Redirect("Home.aspx");

                    // Server.Transfer("Home.aspx", false);
                    Response.Redirect("frmDashboard.aspx", false);
                    // HttpContext.Current.RewritePath("frmDashboard.aspx");

                    //Response.Redirect("~/frmDashboard.aspx", false);
                    // HttpContext.Current.RewritePath("frmDashboard.aspx");

                    //Response.Redirect("frmPurchaseInvoiceReceipt.aspx");
                    //Response.Redirect("frmPurchaseReturn.aspx");
                    //Response.Redirect("ReportSalesInvoice.aspx");
                }
                if (RD.IsClosed == false)
                {
                    RD.Close();
                }

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                ErrorSignal.FromCurrentContext().Raise(sqlex);
                if (sqlex.Message.ToString().IndexOf("The server was not found or was not accessible.") > 0)
                { ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Connection not established with server. Please check the network connection.');", true); return; }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + sqlex.Message.ToString().Replace("'", "''") + "');", true); return;
                }

            }

            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
                if (ex.Message.ToString().IndexOf("The timeout period elapsed prior to obtaining a connection from the pool.") > 0)
                { ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('Connection not established with server. Please check the network connection.');", true); return; }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Error", "alert('" + ex.Message.ToString().Replace("'", "''") + "');", true); return;
                }
            }
        }
    }
}