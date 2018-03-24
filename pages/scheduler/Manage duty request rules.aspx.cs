using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_scheduler_Manage_duty_request_rules : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            getCurrentDetails();
        }
    }

    protected void getCurrentDetails()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM duty_request_rule WHERE rule_id=1";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            sday.SelectedValue = reader["request_sday"].ToString();
            eday.SelectedValue = reader["request_eday"].ToString();
            stime.Text = reader["request_stime"].ToString();
            etime.Text = reader["request_etime"].ToString();
            signinWindow.Text = reader["duty_signInWindow"].ToString();
            signOutWindow.Text = reader["duty_signOutWindow"].ToString();
        }

        db_obj.close();
    }
    protected void updateDetails_Click(object sender, EventArgs e)
    {
        try
        {
            scheduler schedulerObj = new scheduler();

            string newSday = sday.SelectedValue;
            string newEday = eday.SelectedValue;
            string newStime = stime.Text;
            string newEtime = etime.Text;
            string newInWindow = signinWindow.Text;
            string newOutWindow = signOutWindow.Text;
            if(sday.SelectedIndex <= eday.SelectedIndex)
            {
                schedulerObj.modifyDutyRequestRules(newSday, newEday, newStime, newEtime, newInWindow, newOutWindow);

                Response.Redirect(Request.RawUrl);
                msg.Text = "Details has been updated, thank you.";
            }
            else
            {
                msg.Text = "Start day must be before the end day!";
            } 
        }
        catch(Exception ex)
        {
            msg.Text = "There was something wrong, error + " + ex.Message;
        }
        
    }
}