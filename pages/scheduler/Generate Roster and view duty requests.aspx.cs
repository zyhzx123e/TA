using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

public partial class pages_scheduler_Generate_Roster_and_view_duty_requests : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string week = commonMethods.getNextMondayDate();
        if (!IsPostBack)
        {
            upcomingWeek.Text = week;
            if (commonMethods.checkTime() == true)
            {
                generateRoster.Enabled = false;
                msg.Text = "Request time is not over yet!";
            }
            else
            {
                if (commonMethods.checkRosterVersion(week) == 0)
                {
                    msg.Text = " ";
                    generateRoster.Visible = true;
                    roster_link.Visible = false;
                }
                else if (commonMethods.checkRosterVersion(week) == 1)
                {
                    Session["generation_choice"] = null;

                    int userPositionID = int.Parse(Session["Position"].ToString());

                    if (commonMethods.isScheduler(userPositionID))
                    {
                        roster_link.PostBackUrl = "../scheduler/Generated Roster.aspx";
                    }
                    else
                    {
                        roster_link.PostBackUrl = "../TA/View Final Roster.aspx";
                    }
                    generateRoster.Visible = false;
                    roster_link.Visible = true;
                    roster_link.Text = "Click here to view it";
                    msg.Text = "Roster is at Draft stage";
                }
                else
                {
                    Session["generation_choice"] = null;
                    generateRoster.Visible = false;
                    msg.Text = "The final roster is already generated for this week!";

                    roster_link.PostBackUrl = "../TA/View Final Roster.aspx";
                    roster_link.Visible = true;
                    roster_link.Text = "Click here to view it";
                }
            }
        }
        
    }

    //method to get the duty requests for a specific week
    protected DataTable getRequests(string weekDate)
    {
        DataTable dt = new DataTable();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT user_id, shift_id, location_id, requestForDay, request_time, DATE_FORMAT(request_date, '%Y-%m-%d'), DATE_FORMAT(request_for_date, '%Y-%m-%d'), request_remark FROM duty_request WHERE request_status=0 AND request_type=0 AND week_start_date=@weekDate;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@weekDate", weekDate);
        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        adapter.Fill(dt);

        db_obj.close();
        return dt;
    }



    //to return location name instead of its ID in the roster
    protected string getLocationName(int locationID)
    {
        string locationName;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT name FROM location WHERE location_id=@lid";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@lid", locationID);

        locationName = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return locationName;
    }

    protected string getShiftName(int shiftID)
    {
        string shiftName;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT name FROM shift WHERE shift_id=@lid";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@lid", shiftID);

        shiftName = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return shiftName;
    }

    protected string getUserName(int userID)
    {
        string name;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT user_name FROM user WHERE user_id=@lid";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@lid", userID);

        name = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return name;
    }
    protected void generateRoster_Click(object sender, EventArgs e)
    {
        if(generationCretira.SelectedIndex>-1)
        {
            Session["generation_choice"] = generationCretira.SelectedValue;
            Response.Redirect("Generated Roster.aspx");
        }
        else
        {
            msg.Text = "Yo, chose criteria to generate the roster!";
        }
        
    }

    

    

}