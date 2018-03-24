using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_common_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void login_btn_Click(object sender, EventArgs e)
    {
        try
        {
            user newUser = new user();
            string input_tp = tpNumber.Text;
            string input_pass = password.Text;
            if(string.IsNullOrEmpty(input_tp) || string.IsNullOrEmpty(input_pass))
            {
                msg.Text = "TP and Password cannot be empty";
            }
            else
            {
                if (newUser.login(input_tp, input_pass) == true)
                {
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    msg.Text = "Wrong TP number or password! Please try again.";
                }
            }
            
        }
        catch (Exception ex)
        {
            msg.Text = "The login process was unsuccessful, for more information please contact the administrator.";
        }
        
    }
}