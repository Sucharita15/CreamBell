using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps.UserControl
{
    public partial class ucDropDown : System.Web.UI.UserControl
    {
        public ListBox GetListBox
        {
            get
            {
                return lstControl;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}