using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps
{
    public partial class frmPowerAppDisplay : System.Web.UI.Page
    {
      protected void Page_Load(object sender, EventArgs e)
        {
              if (!IsPostBack)
               {
                  iframe();   
               }
        }
      protected void iframe()
       {
        LiteralControl literal = new LiteralControl();        
        literal.Text=  "<iframe width='100%' height='500' src='https://app.powerbi.com/view?r=eyJrIjoiYzA1MjgxMzktNDZkNC00MGI3LWFkNzMtYWI0MzlhYWMzOTAzIiwidCI6ImM0MGU3OTkwLWZmOTktNGE5My05NGU5LWYyZTRkNjY5NTNlMiIsImMiOjEwfQ%3D%3D' frameborder='0' allowFullScreen='true'></iframe>";
        div1.Controls.Add(literal);
       }

    }
}