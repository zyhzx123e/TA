    using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for commonMethods
/// </summary>
public class commonMethods
{
	public commonMethods()
	{
	    

	}

    //to return location name instead of its ID in the roster
    public static string getLocationName(int locationID)
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

    //to return duty name instead of its ID in the roster
    public static string getDutyName(int dutyID)
    {
        string dutyName;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT title FROM duty WHERE duty_id=@id";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@id", dutyID);

        dutyName = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return dutyName;
    }

    //to return shift name instead of its ID in the roster
    public static string getShiftName(int id)
    {
        string shiftID;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT name FROM shift WHERE shift_id=@id";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@id", id);

        shiftID = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return shiftID;
    }

    //to return shift Timing at the location on a specific day instead of its ID in the roster
    public static string getShiftTiming(int shift_ID, string day, int locationID)
    {
        string shiftTiming;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT CONCAT(TIME_FORMAT(start_time, '%H:%i'),'-', TIME_FORMAT(end_time, '%H:%i')) AS TIMING FROM shift_location WHERE shift_id=@sid AND day=@day AND location_id=@lid";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@sid", shift_ID);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@lid", locationID);

        shiftTiming = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return shiftTiming;
    }

    //Based on DB specification create a cell for each required manpower
    public static Dictionary<int, int> getRosterDutiesCells(string day, int locationID, int shiftID)
    {
        Dictionary<int, int> dutiesList = new Dictionary<int, int>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string dutiesQuery = "SELECT distinct duty_id, active FROM duty_shift_location WHERE day=@day AND location_id=@lid AND shift_id=@sid;";
        MySqlCommand dutyCommand = new MySqlCommand(dutiesQuery, db_obj.connection);
        dutyCommand.Parameters.AddWithValue("@day", day);
        dutyCommand.Parameters.AddWithValue("@lid", locationID);
        dutyCommand.Parameters.AddWithValue("@sid", shiftID);
        MySqlDataReader dutyReader = dutyCommand.ExecuteReader();
        while (dutyReader.Read())
        {
            dutiesList.Add(Int32.Parse(dutyReader["duty_id"].ToString()), Int32.Parse(dutyReader["active"].ToString()));
        }
        db_obj.close();
        return dutiesList;
    }

    //to retrieve the list of duties required in a location on a specfic day
    public static List<KeyValuePair<int, int>> getRosterDuties(string day, int locationID)
    {
        List<KeyValuePair<int, int>> dutyInfo = new List<KeyValuePair<int, int>>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string dutiesQuery = "SELECT DISTINCT duty_id, active FROM duty_shift_location WHERE day=@day AND location_id=@lid GROUP BY duty_id;";
        MySqlCommand dutyCommand = new MySqlCommand(dutiesQuery, db_obj.connection);
        dutyCommand.Parameters.AddWithValue("@day", day);
        dutyCommand.Parameters.AddWithValue("@lid", locationID);
        MySqlDataReader dutyReader = dutyCommand.ExecuteReader();
        while (dutyReader.Read())
        {
            dutyInfo.Add(new KeyValuePair<int, int>(Int32.Parse(dutyReader["duty_id"].ToString()), Int32.Parse(dutyReader["active"].ToString())));
        }

        db_obj.close();
        return dutyInfo;
    }

    //Retrive all shifts which have duties from DB
    public static List<int> getRosterShifts(string day, int locationID)
    {
        List<int> shiftsList = new List<int>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string shiftsQuery = "SELECT DISTINCT shift_id FROM duty_shift_location WHERE day=@day AND location_id=@locationID";
        MySqlCommand shiftCommand = new MySqlCommand(shiftsQuery, db_obj.connection);
        shiftCommand.Parameters.AddWithValue("@day", day);
        shiftCommand.Parameters.AddWithValue("@locationID", locationID);
        MySqlDataReader shiftReader = shiftCommand.ExecuteReader();
        while (shiftReader.Read())
        {
            shiftsList.Add(Int32.Parse(shiftReader["shift_id"].ToString()));
        }

        db_obj.close();
        return shiftsList;
    }

    //Retrive all locations which have duties from DB
    public static List<int> getRosterLocations(string day)
    {
        List<int> locationsList = new List<int>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string locationQuery = "SELECT DISTINCT location_id FROM duty_shift_location WHERE day=@day;";
        MySqlCommand locationCommand = new MySqlCommand(locationQuery, db_obj.connection);
        locationCommand.Parameters.AddWithValue("@day", day);
        MySqlDataReader locationReader = locationCommand.ExecuteReader();
        while (locationReader.Read())
        {
            locationsList.Add(Int32.Parse(locationReader["location_id"].ToString()));
        }

        db_obj.close();
        return locationsList;
    }

    //Retrive all days which have duties from DB
    public static List<string> getRosterDays()
    {
        List<string> daysList = new List<string>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string daysQuery = "SELECT DISTINCT day FROM duty_shift_location ORDER BY FIELD(day, 'MONDAY', 'TUESDAY', 'WEDNESDAY', 'THURSDAY', 'FRIDAY', 'SATURDAY', 'SUNDAY');";
        MySqlCommand daysCommand = new MySqlCommand(daysQuery, db_obj.connection);
        MySqlDataReader daysReader = daysCommand.ExecuteReader();
        while (daysReader.Read())
        {
            daysList.Add(daysReader["day"].ToString());
        }

        db_obj.close();
        return daysList;
    }


    //method to disable duty requests if it is out of the time range
    public static bool checkTime()
    {
        bool result = false;
        //int d = (int)System.DateTime.Now.DayOfWeek;
        string today = DateTime.Today.DayOfWeek.ToString();
        string now = DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss");
        string sday = "", eday = "", stime = "", etime = " ";



        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM duty_request_rule WHERE rule_id=1;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            sday = reader["request_sday"].ToString();
            eday = reader["request_eday"].ToString();
            stime = reader["request_stime"].ToString();
            etime = reader["request_etime"].ToString();
        }

        if (getDayIntValue(today) == getDayIntValue(sday))
        {
            if (TimeSpan.Parse(stime) < TimeSpan.Parse(now))
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else if (getDayIntValue(today) > getDayIntValue(sday) && getDayIntValue(today) < getDayIntValue(eday))
        {
            result = true;
        }
        else if (getDayIntValue(today) > getDayIntValue(sday) && getDayIntValue(today) == getDayIntValue(eday))
        {
            if (TimeSpan.Parse(now) < TimeSpan.Parse(etime))
            {
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }


        db_obj.close();
        return result;
    }

    public static int getDayIntValue(string day)
    {
        int value = 0;
        switch (day)
        {
            case "Monday":
                { value = (int)DayOfWeek.Monday; break; }          
            case "Tuesday":
                { value = (int)DayOfWeek.Tuesday; break; }
            case "Wednesday":
                { value = (int)DayOfWeek.Wednesday; break; }
            case "Thursday":
                { value = (int)DayOfWeek.Thursday; break; }
            case "Friday":
                { value = (int)DayOfWeek.Friday; break; }
            case "Saturday":
                { value = (int)DayOfWeek.Saturday; break; } 
            case "Sunday":
                { value = (int)DayOfWeek.Sunday; break; }
            default:
                { value = 0; break; }
        }

        return value;

    }

    //to check if the roster is in pending (value =0), draft (value =1) or final(value =2) version
    public static int checkRosterVersion(string weekDate)
    {
        int rosterStatus = 0;
        db_connection db_object = new db_connection();
        db_object.open();
        string query = "SELECT DISTINCT request_status FROM duty_request WHERE week_start_date=@week;";
        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@week", weekDate);

        MySqlDataReader reader = cmd.ExecuteReader();

        List<int> statuses = new List<int>();
        while (reader.Read())
        {
            statuses.Add(int.Parse(reader["request_status"].ToString()));
        }

        if (statuses.Contains(2))
        {
            rosterStatus = 2;
        }
        else if (statuses.Contains(1))
        {
            rosterStatus = 1;
        }
        else
        {
            rosterStatus = 0;
        }

        db_object.close();
        return rosterStatus;
    }

    //method to get the date of Monday in the next week.
    public static string getNextMondayDate()
    {
        DateTime today = DateTime.Today;
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysUntilTuesday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
        DateTime nextMonday = today.AddDays(daysUntilTuesday);

        return nextMonday.ToString("yyyy-MM-dd");
    }

    public static string getTAName(int id)
    {
        string TA_name;
        string query = "SELECT user_name FROM user WHERE user_id=@id";
        db_connection db_object = new db_connection();
        db_object.open();

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@id", id);

        TA_name = cmd.ExecuteScalar().ToString();

        db_object.close();
        return TA_name;
    }


    //a method to get the date of the selected request day
    public static string getDayDate(string day)
    {
        DateTime today = DateTime.Today;
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int dayDate = 1;
        switch (day)
        {
            case "Monday":
                dayDate = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7; break;            
            case "Tuesday":
                dayDate = ((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7; break;              
            case "Wednesday":
                dayDate = ((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7; break;               
            case "Thursday":
                dayDate = ((int)DayOfWeek.Thursday - (int)today.DayOfWeek + 7) % 7; break;               
            case "Friday":
                dayDate = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7; break; 
            case "Saturday":
                dayDate = (((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 14) % 7) + 7; break;
            default: break;     
        }

        DateTime requiredDate = today.AddDays(dayDate);
        return requiredDate.ToString("yyy-MM-dd");
    }

    public static void takeEmptyDuty(int userID, string week, int shiftID, int locationID, string day, int status, int orderNumber)
    {
        technical_assistant newTA = new technical_assistant();
        object[] requestDetails = new object[11];
        requestDetails[0] = week;
        requestDetails[1] = shiftID;
        requestDetails[2] = locationID;
        requestDetails[3] = day;
        requestDetails[4] = string.Format("HH:mm:ss", DateTime.Now);
        requestDetails[5] = DateTime.Now.ToString("yyy-MM-dd");
        requestDetails[6] = getDayDate(day);
        requestDetails[7] = status;   
        requestDetails[8] = 0;   //0 means request type is duty request
        requestDetails[9] = "Added after draft is out";
        requestDetails[10] = orderNumber;

        newTA.requestDuty(userID, requestDetails);
    }

    public static string getCurrentMondayDate()
    {
        DateTime today = DateTime.Today;
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int daysUntilTuesday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek) % 7;
        DateTime thisMonday = today.AddDays(daysUntilTuesday);

        return thisMonday.ToString("yyy-MM-dd");
    }

    //a method to test if a shift is full.
    public static bool isFull(int shift_id, int location_id, string week, string day)
    {
        bool result = false;

        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT COUNT(user_id) FROM duty_request WHERE shift_id=@shift AND location_id=@location AND week_start_date=@week AND requestForDay =@day AND request_status IN (1,2);";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", location_id);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@shift", shift_id);

        int takenSlots_count = int.Parse(cmd.ExecuteScalar().ToString());

        int requiredNumber = commonMethods.getRosterDutiesCells(day, location_id, shift_id).Count;

        if (takenSlots_count < requiredNumber)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    public static bool isScheduler(int positionID)
    {
        string result;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT given FROM function_position WHERE function_id=8 AND position_id=@positionID";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@positionID", positionID);

        result = cmd.ExecuteScalar().ToString();

        db_obj.close();

        if (int.Parse(result) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}