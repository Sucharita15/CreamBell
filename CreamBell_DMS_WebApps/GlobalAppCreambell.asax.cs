using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CreamBell_DMS_WebApps
{
    public class GlobalAppCreambell : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started  
            if (Session["USERID"] != null)
            {
                //Redirect to Welcome Page if Session is not null  
                Response.Redirect("Home.aspx");

            }
            else
            {
                //Redirect to Login Page if Session is null & Expires 
                
                Response.Redirect("Login.aspx");

            }  
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}