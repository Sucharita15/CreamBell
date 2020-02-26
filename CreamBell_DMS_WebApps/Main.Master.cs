using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CreamBell_DMS_WebApps.App_Code;
using Elmah;

namespace CreamBell_DMS_WebApps
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager1.AsyncPostBackTimeout = 7200;

            if (Session[SessionKeys.USERID] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (Session[SessionKeys.USERID].ToString() != null)
            {
                if (!IsPostBack)
                {
                    //Session["Menu"] = null;
                    LblHeader.Text = Session[SessionKeys.USERNAME].ToString();
                    LoadSiteIDDetails();
                    HideMenuFromDataBase();
                }
                // HideMenuLink();                
            }

        }

        private void LoadSiteIDDetails()
        {
            CreamBell_DMS_WebApps.App_Code.Global obj = new App_Code.Global();

            string query = "Select NAME , ACXADDRESS1 + ACXADDRESS2 + ',' + ACXCITY AS SITEADDRESS from ax.inventsite where SITEID='" + Session["SiteCode"] + "'";
            System.Data.DataTable dt = obj.GetData(query);
            if (dt.Rows.Count > 0)
            {
                lblAddress.Text = dt.Rows[0]["SITEADDRESS"].ToString();
            }
        }

        //protected void BtnLogOut_Click(object sender, EventArgs e)
        //{
        //    Session.Clear();
        //    Session.Abandon();
        //    Session.RemoveAll();
        //    Response.Redirect("Login.aspx");
        //}

        //protected void ImgBtnCalc_Click(object sender, ImageClickEventArgs e)
        //{
        //    //Process p = new Process();
        //    //p.StartInfo.UseShellExecute = false;
        //    //p.StartInfo.RedirectStandardOutput = true;
        //    //p.StartInfo.FileName = "calc.exe";
        //    //p.Start();
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void BtnHome_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Home.aspx");
        }

        //protected void BtnLogout_Click1(object sender, EventArgs e)
        //{
        //    Session.Clear();
        //    Session.Abandon();
        //    Session.RemoveAll();
        //    Response.Redirect("Login.aspx");
        //}

        protected void Customer_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            //foreach (TreeNode treenode in Customer.Nodes)
            //{
            //    // If node is expanded

            //    if (treenode != e.Node && treenode.Expanded == true)
            //    {
            //        // Collapse all other nodes
            //        treenode.ExpandAll();
            //        //if (e.Node.Expanded == true)
            //        //{
            //        //    e.Node.Expanded = true;
            //        //}
            //        //else
            //        //{
            //        //    e.Node.Expanded = false;
            //        //}

            //    }
            //    if (treenode != e.Node && treenode.Expanded == false )
            //    {
            //        treenode.CollapseAll();
            //    }

            //}
        }

        protected void Customer_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {
            //foreach (TreeNode treenode in Customer.Nodes)
            //{
            //    // If node is expanded

            //    if (treenode.Text.ToString()=="Master")
            //    {
            //        // Collapse all other nodes
            //        //treenode.Collapse();
            //        treenode.Expand();
            //    }

            //}
        }

        private void RemoveNodechildRecurrently(TreeNodeCollection trc, string text)
        {
            foreach (TreeNode tr in trc)
            {
                if (tr.Text.Trim() == text)
                {
                    //this.Customer.Nodes.Remove(tr);
                    //TreeNode parentNode = tr.Parent;
                    trc.Remove(tr);
                    break;
                }
            }
        }

        private void RemoveNodemain(TreeNodeCollection childNodeCollection, string text)
        {
            foreach (TreeNode childNode in childNodeCollection)
            {
                if (childNode.Text.Trim() == text)
                {
                    childNodeCollection.Remove(childNode);
                    break;
                }
            }
        }
        private void HideMenuLink()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
                string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session[SessionKeys.SITECODE].ToString() + "'";
                object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                if (objcheckSitecode == null)
                {

                    // lnkvatCalculation.Visible = false;
                    StockOpening.Visible = false;
                    //linkDiscountedOutletSale.Visible = false;
                    linkConsolidatedSaleRegister.Visible = false;
                    linkConsolidatedPurchaseRegister.Visible = false;
                    //linkConsolidatedClaimReport.Visible = false;
                    //linkClaimScheme.Visible = false;
                    linkSyncData.Visible = false;
                    linkVersionReleaseInfo.Visible = false;
                    linkVersionInfo.Visible = false;
                    linkVersionReleaseRegister.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void RemoveNodeRecurrently(TreeNodeCollection childNodeCollection, string text)
        {
            CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
            foreach (TreeNode childNode in childNodeCollection)
            {

                if (childNode.Text.Trim().ToUpper() == "STOCK")
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session[SessionKeys.SITECODE].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode == null)
                    {
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Opening Stock Upload");
                    }
                }
                if (childNode.Text.Trim().ToUpper() == "REPORTS")
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session[SessionKeys.SITECODE].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode == null)
                    {
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Claim Scheme");
                    }
                }
                if (childNode.Text.Trim().ToUpper() == "Expense Report")
                {
                    string sqlstr = "select * from ax.ACXSITEMENU where SITE_CODE ='" + Session[SessionKeys.SITECODE].ToString() + "'";
                    object objcheckSitecode = baseObj.GetScalarValue(sqlstr);
                    if (objcheckSitecode == null)
                    {
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Consolidated Claim Report");
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Consolidated Purchase Register");
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Consolidated Sale Register");
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Discounted Outlet Report");
                        RemoveNodechildRecurrently(childNode.ChildNodes, "Monthly Expense Summary");
                    }
                }
            }
        }
        private void HideMenuFromDataBase_Old()
        {
            try
            {
                CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
                string sqlstr = @"select upper(A.menu_Code) as menu_Code,Upper(A.Header) as Header,Upper(A.Menu) as Menu,A.Tech_Name,case when B.ROLE_CODE is null then 0 else 1 end as Active 
                                from [ax].[ACXMENU_MASTER] A Left Join [ax].[ACXMENUROLE_MASTER] B On B.MENU_CODE=A.Menu_Code and B.ROLE_CODE='R01' 
                                where B.ROLE_CODE is null";
                System.Data.DataTable dt = baseObj.GetData(sqlstr);
                if (dt != null)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        HtmlGenericControl listName = new HtmlGenericControl("li");
                        string linkname = dt.Rows[i]["Tech_Name"].ToString();
                        var ctrl = this.Master.FindControl(linkname);
                        if (ctrl != null)
                        {
                            ctrl.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void HideMenuFromDataBase()
        {
            try
            {
                // Session.Clear();
                //string role = Session["USERID"].ToString();
                if (Session[SessionKeys.MENU] == null)
                {
                    CreamBell_DMS_WebApps.App_Code.Global baseObj = new Global();
                    string sqlstr = @"select upper(A.menuCode) as menuCode,Upper(A.Header) as Header,Upper(A.Menu) as Menu,A.TechName,case when B.ROLECODE is null then 0 else 1 end as Active 
                                  from [ax].[ACXMENUMASTER] A Left Join [ax].[ACXMENUROLEMASTER] B On B.MENUCODE=A.MenuCode and B.ROLECODE=(SELECT ROLECODE FROM [ax].[acxusermaster] WHERE USER_CODE='" + Session["USERID"].ToString() + "') ORDER BY A.menuCode";
                    System.Data.DataTable dt = baseObj.GetData(sqlstr);
                    Session[SessionKeys.MENU] = dt;
                    BindMenu(dt);
                    dt = null;
                }
                else
                {
                    BindMenu((DataTable)Session[SessionKeys.MENU]);
                }
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void BindMenu(System.Data.DataTable dt)
        {

            if (dt != null)
            {
                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        HtmlGenericControl listName = new HtmlGenericControl("li");
                        string linkname = dt.Rows[i]["TechName"].ToString();
                        string Status = dt.Rows[i]["Active"].ToString();
                        if (linkname == "linkSaleRegister1")
                        {
                            string s = "";
                        }
                        if (this.Page.Master.FindControl(linkname) == null)
                        {
                        }
                        else
                        {
                            Control ctrl = this.Page.Master.FindControl(linkname);

                            if (Status == "1")
                            {
                                ctrl.Visible = true;
                            }
                            else
                            {
                                //  linkImageUpload.Visible = true;
                                ctrl.Visible = false;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }
    }
}