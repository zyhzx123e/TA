using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_Reports_reports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int userPosition = int.Parse(Session["Position"].ToString());
        if (!commonMethods.isScheduler(userPosition))
        {
            Response.Redirect("Individual Report.aspx");
        }
    }
}