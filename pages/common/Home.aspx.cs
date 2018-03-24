using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_TA_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            username_lbl.Text = Session["username"].ToString();
            weekDate.Text = commonMethods.getCurrentMondayDate();
            populateProblemsCount('r');
            populateProblemsCount('q');
        }catch (Exception ex){
            Response.Redirect("~/pages/common/Sign out.aspx");
        }
       }

    protected int getProblemsCount(int locationID, char type)
    {
        int counter;

        counter = RoundingFunctions.getProblems(locationID, type).Count;

        return counter;
    }


    protected void populateProblemsCount(char type)
    {
        foreach (int location_id in commonMethods.getRosterLocations("Monday"))
        {
            int problems_count = getProblemsCount(location_id, type);
            string location_name = commonMethods.getLocationName(location_id);

            string buttonColor = problems_count > 0 ? "danger" : "success";

            Literal litROUNDING = new Literal();
            Literal litQC = new Literal();
            litROUNDING.Text = "<a href='/Pages/TA/rounding.aspx'><button  class='btn btn-" + buttonColor + "' type='button'> " + location_name + " - Problems <span class='badge'>" + problems_count + "</span></button></a>  ";

            litQC.Text = "<a href='/Pages/TA/qc.aspx'><button  class='btn btn-" + buttonColor + "' type='button'> " + location_name + " - Problems <span class='badge'>" + problems_count + "</span></button></a>  ";


            if (type=='r')
            {
                problemCounts_ph.Controls.Add(litROUNDING);
            }
            else
            {
                qc_ph.Controls.Add(litQC);
            }
        }

    }
    protected void roundingLinkBtn_Click(object sender, EventArgs e)
    {

    }
}