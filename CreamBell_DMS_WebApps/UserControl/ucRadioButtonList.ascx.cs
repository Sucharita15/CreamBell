using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreamBell_DMS_WebApps.UserControl
{
    public partial class ucRadioButtonList : System.Web.UI.UserControl
    {
        public RadioButton GetXLSB
        {
            get
            {
                return rdbXLSB;
            }
        }
        public RadioButton GetXLSX
        {
            get
            {
                return rdbXLSX;
            }
        }
        public RadioButton GetXLS
        {
            get
            {
                return rdbXLS;
            }
        }

    }
}