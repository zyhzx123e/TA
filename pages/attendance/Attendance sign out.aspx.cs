using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_common_Sign_out : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        HttpContext.Current.User = null;
        System.Web.Security.FormsAuthentication.SignOut();
    }
}