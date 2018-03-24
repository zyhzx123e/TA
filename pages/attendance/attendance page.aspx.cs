using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_attendance_attendance_page : System.Web.UI.Page
{
    /*
       attendance_case is a variable passed from the attendance login page to indicate the following:
       if value = 0 : TA do not have any shift at the location today.
       if value = 1 : TA is already logged in to a shift at this location today.
       if value = 2 : TA has shift(s) but he has not signed in yet.
    */

    string today = DateTime.Now.DayOfWeek.ToString();
    string week = commonMethods.getCurrentMondayDate();
    int locationID = 1;
    string now = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss");

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            handle_user_case();
            output_time.Text = "Current Date & Time : " + DateTime.Now.ToString();
        }
    }

    protected void handle_user_case()
    {
        try
        {
            int userID = int.Parse(Session["id"].ToString());
            
            username.Text = Session["username"].ToString();
            int userCase = int.Parse(Session["attendance_case"].ToString());
            switch (userCase)
            {
                case 0:
                    {
                        output.Text = "You do not have any shift at this location today!";
                        break;
                    }
                case 1:
                    {
                        output.Text = "You are already logged in to a shift!";
                        signOutFromDuty.Visible = true;
                        break;
                    }
                case 2:
                    {
                        output.Text = "Below are your shifts for today:";
                        foreach (string s in get_today_duties(userID, locationID, week, today))
                        {
                            shiftsList.Items.Add(s);
                        }
                        dutySignInBtn.Visible = true;
                        
                        break;
                    }
                default:
                    break;
            }

        }
        catch (Exception ex)
        {
            msg.Text = "There was something wrong, error: " + ex.Message;
        }
    }

    //a method to retreive a list of all the TA duties for a specific day at a specific location
    protected List<string> get_today_duties(int userID, int locationID, string week, string day)
    {
        bool enableBtn = false;
        List<string> shifts_list = new List<string>();
        db_connection db_obj = new db_connection();
        db_obj.open();
        //string query = "SELECT attendance.shift_id, shift_location.start_time, shift_location.end_time FROM attendance INNER JOIN shift_location ON attendance.shift_id = shift_location.shift_id AND shift_location.day=@day AND shift_location.location_id=@location WHERE user_id=@user AND week_start_date=@week AND day=@day;";
        string query = "SELECT attendance.shift_id, shift_location.start_time, shift_location.end_time FROM attendance INNER JOIN shift_location ON attendance.shift_id = shift_location.shift_id AND shift_location.day = @day AND shift_location.location_id = @location WHERE user_id =@user AND week_start_date =@week AND attendance.day =@day";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);

        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            string shift_name = commonMethods.getShiftName(int.Parse(reader["shift_id"].ToString()));

            string sTime = reader["start_time"].ToString();
            TimeSpan timeDifference = TimeSpan.Parse(sTime).Subtract(TimeSpan.Parse(now));
            int timeDifferenceInMinutes = timeDifference.Hours * 60 + timeDifference.Minutes;
            if(timeDifferenceInMinutes>0)
            {
                string shift = shift_name + " Start Time: " + reader["start_time"].ToString() + " End Time: " + reader["end_time"] + " (" + timeDifferenceInMinutes + " minutes left)";
                shifts_list.Add(shift);
                if (timeDifferenceInMinutes < getSignInWindow()) enableBtn = true;
            }
            else
            {
                string shift = shift_name + " Start Time: " + reader["start_time"].ToString() + " End Time: " + reader["end_time"];
                shifts_list.Add(shift);
            }

        }

        if(enableBtn==true)
        {
            //dutySignInBtn.Enabled = true;
        }
        else
        {
           // dutySignInBtn.Enabled = false;
           // msg.Text = "You can only sign in " + getSignInWindow().ToString() + " minutes before your duty, for more info please contact the scheduler."; 
        }

        db_obj.close();
        return shifts_list;
    }

    // a method to verify if TA signin in on time or late
    protected bool checkSignInTimeWithShiftTime()
    {
        bool result = false;


        return result;
    }


    //this method is used to handle user log action
    protected void handleLogginIn()
    {
        int userID = int.Parse(Session["id"].ToString());
        List<string> shifts = new List<string>();
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT attendance.shift_id, shift_location.start_time, shift_location.end_time, attendance.logIn_status FROM attendance INNER JOIN shift_location ON attendance.shift_id = shift_location.shift_id AND shift_location.day=@day AND shift_location.location_id=@location WHERE user_id=@user AND week_start_date=@week;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", today);

        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string shift = reader["shift_id"] + "," + reader["start_time"] + "," + reader["end_time"] + "," + reader["logIn_status"];
            shifts.Add(shift);  
        }

        try
        {
            foreach (string s in shifts)
            {
                string[] shift_info = s.Split(',');
                int shiftID = int.Parse(shift_info[0].ToString());
                string sTime = shift_info[1];
                string eTime = shift_info[2];
                string status = shift_info[3];
                string todayDate = DateTime.Now.ToString("yyy-MM-dd");

                if ((TimeSpan.Parse(now) > TimeSpan.Parse(eTime)) && (status == "6"))
                {
                    captureAbsent(userID, week, shiftID, locationID, today);
                }
                else if((TimeSpan.Parse(now) > TimeSpan.Parse(sTime) && TimeSpan.Parse(now) < TimeSpan.Parse(eTime)) && (status == "6"))
                {
                    captureSignIn(userID, week, shiftID, locationID, today, now, todayDate);
                }
                else if ((TimeSpan.Parse(now) < TimeSpan.Parse(sTime)) && (status == "6"))
                {
                    captureSignIn(userID, week, shiftID, locationID, today, sTime, todayDate);
                } 
                else if (TimeSpan.Parse(now) > TimeSpan.Parse(eTime) && (status == "0"))
                {
                    captureAbsent(userID, week, shiftID, locationID, today);
                }
                else if (TimeSpan.Parse(now) > TimeSpan.Parse(sTime) && TimeSpan.Parse(now) < TimeSpan.Parse(eTime))
                {
                    captureSignIn(userID, week, shiftID, locationID, today, now, todayDate);
                }
                else if (TimeSpan.Parse(now) < TimeSpan.Parse(sTime))
                {
                    captureSignIn(userID, week, shiftID, locationID, today, sTime, todayDate);
                }
            }
        }catch(Exception e)
        {
            msg.Text = e.Message;
        }
        
       
        db_obj.close();
    }

    //this method is to handle logout from the systme action
    protected void handleLogOut()
    {
        int userID = int.Parse(Session["id"].ToString());
        List<string> shifts = new List<string>();
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT attendance.shift_id, shift_location.start_time, shift_location.end_time FROM attendance INNER JOIN shift_location ON attendance.shift_id = shift_location.shift_id AND shift_location.day=@day AND shift_location.location_id=@location WHERE user_id=@user AND week_start_date=@week AND logIn_status=1;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", today);

        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string shift = reader["shift_id"] + "," + reader["start_time"] + "," + reader["end_time"];
            shifts.Add(shift);
        }

        foreach (string s in shifts)
        {
            string[] shift_info = s.Split(',');
            int shiftID = int.Parse(shift_info[0].ToString());
            string sTime = shift_info[1];
            string eTime = shift_info[2];
            string todayDate = DateTime.Now.ToString("yyy-MM-dd");

            if (TimeSpan.Parse(now) > TimeSpan.Parse(eTime))
            {
                TimeSpan timeDifference = TimeSpan.Parse(now).Subtract(TimeSpan.Parse(eTime));
                int timeDifferenceInMinutes = timeDifference.Hours * 60 + timeDifference.Minutes;
                if (timeDifferenceInMinutes > getSignOutWindow())
                {
                    signUserOut(userID, week, shiftID, locationID, today, eTime, todayDate, "Sing Out Late", 3);
                }
                else
                {
                    signUserOut(userID, week, shiftID, locationID, today, eTime, todayDate, "Normal", 3);
                }
                
            }
            else if ((TimeSpan.Parse(now) < TimeSpan.Parse(eTime)) && (TimeSpan.Parse(now) < TimeSpan.Parse(sTime)))
            {
                signUserOut(userID, week, shiftID, locationID, today, now, todayDate, "Not signed, signed out before duty starts", 6);
            }
            else if (TimeSpan.Parse(now) < TimeSpan.Parse(eTime))
            {
                signUserOut(userID, week, shiftID, locationID, today, now, todayDate, "Early sign out", 5);
            }
        }

        db_obj.close();
    }


    protected void captureSignIn(int userID, string week, int shift, int location, string day ,string loginTime, string loginDate)
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "UPDATE attendance SET login_date=@date, logout_time=@login, logout_date=@date, logIn_status=1, system_remark='Did not sign out' WHERE user_id=@user AND location_id=@location AND shift_id=@shift AND week_start_date=@week AND day=@day;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@login", loginTime);
        cmd.Parameters.AddWithValue("@date", loginDate);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@shift", shift);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    protected void captureAbsent(int userID, string week, int shift, int location, string day)
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "UPDATE attendance SET status=3, system_remark='Absent for duty' WHERE user_id=@user AND location_id=@location AND shift_id=@shift AND week_start_date=@week AND day=@day;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@shift", shift);
        cmd.Parameters.AddWithValue("@day", day);
        

        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    protected void signUserOut(int userID, string week, int shift, int location, string day, string logoutTime, string logoutDate, string remark, int loginStatus)
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "UPDATE attendance SET logout_time=@logout, logout_date=@date, logIn_status=@loginStatus, system_remark=@remark WHERE user_id=@user AND location_id=@location AND shift_id=@shift AND week_start_date=@week AND day=@day;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@logout", logoutTime);
        cmd.Parameters.AddWithValue("@date", logoutDate);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@shift", shift);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@remark", remark);
        cmd.Parameters.AddWithValue("@loginStatus", loginStatus);
        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    protected void dutySignInBtn_Click(object sender, EventArgs e)
    {
        //handleLogginIn();
    }

    protected void signOutFromDuty_Click(object sender, EventArgs e)
    {
        handleLogOut();
        msg.Text = "You have signed out, thank you.";
        signOutFromDuty.Visible = false;       
    }

    protected void dutySignInBtn_Click1(object sender, EventArgs e)
    {
        handleLogginIn();

        //msg.Text = "You have been signed in, thank you.";
        dutySignInBtn.Visible = false;
    }

    //this method to get the number of minutes window for TAs to sign in
    protected int getSignInWindow()
    {
        int minutes_num = 0;
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT duty_signInWindow FROM duty_request_rule WHERE rule_id=1;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

        minutes_num = int.Parse(cmd.ExecuteScalar().ToString());

        db_obj.close();

        return minutes_num;
    }

    //this method to get the number of minutes window for TAs to sign out
    protected int getSignOutWindow()
    {
        int minutes_num = 0;
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT duty_signOutWindow FROM duty_request_rule WHERE rule_id=1;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

        minutes_num = int.Parse(cmd.ExecuteScalar().ToString());

        db_obj.close();

        return minutes_num;
    }
}