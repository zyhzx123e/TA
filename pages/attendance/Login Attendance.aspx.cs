using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_attendance_Login_Attendance : System.Web.UI.Page
{
    int locationID = 1;
    string currentWeek = commonMethods.getCurrentMondayDate();
    string today = DateTime.Now.DayOfWeek.ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        user newUser = new user();
        string input_tp = tpNumber.Text;
        string input_pass = password.Text;

        if (newUser.login(input_tp, input_pass) == true)
        {
            int userID = int.Parse(Session["id"].ToString());
            if(verifyTA_has_Duty(userID, locationID, currentWeek, today))
            {
                if(checkLogginStatus(userID, locationID, currentWeek, today))
                {
                    Session["attendance_case"] = 1;
                    Response.Redirect("attendance page.aspx");
                }
                else
                {
                    Session["attendance_case"] = 2;
                    Response.Redirect("attendance page.aspx");
                }
            }
            else
            {
                Session["attendance_case"] =0;
                Response.Redirect("attendance page.aspx");
            }
        }
        else
        {
            msg.Text = "Wrong TP number or password! Please try again.";
        }
    }


    //method to check if TA has duty today at the location from the attendance table
    protected bool verifyTA_has_Duty(int userID, int locationID, string week, string day)
    {
        bool result = false;
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "SELECT IF((SELECT DISTINCT shift_id FROM attendance WHERE user_id=@user AND week_start_date=@week AND location_id=@location AND day=@day LIMIT 1), 1, 0);";

        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);

        string queryResult = (cmd.ExecuteScalar()).ToString();
        if (queryResult == "1") result = true;
        else result = false;

        db_obj.close();
        return result;
    }

    //a method to check if TA is logged in to shift or not from attendance table
    protected bool checkLogginStatus(int userID, int locationID, string week, string day)
    {
        bool result = false;
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "SELECT IF((SELECT DISTINCT shift_id FROM attendance WHERE user_id=@user AND week_start_date=@week AND location_id=@location AND day=@day AND logIn_status=1 LIMIT 1), 1, 0)";

        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);

        string queryResult = (cmd.ExecuteScalar()).ToString();
        if (queryResult == "1") result = true;
        else result = false;

        db_obj.close();
        return result;
    }

}