using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

public class scheduler : technical_assistant
{
	public scheduler()
	{
		
	}

    //add new technical assistant function
    public void addNewTA(object[] TA_info)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "INSERT INTO user (user_name, user_tp, user_ta_code, private_email, ta_email, contact_number, password, dob, gender, nationality, address, intake_code, gpa, achievements, selection_date, warning_count, active, position_id) " +
                        "VALUES (@name, @tp, @taCode, @pemail, @temail, @contactNumber, @password, @dob, @gender, @nationality, @address, @intake, @gpa, @achievement, @sdate, @warnings, 1, @positionID) ";
        
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@name", TA_info[0]);
        cmd.Parameters.AddWithValue("@tp", TA_info[1]);
        cmd.Parameters.AddWithValue("@taCode", TA_info[2]);
        cmd.Parameters.AddWithValue("@pemail", TA_info[3]);
        cmd.Parameters.AddWithValue("@temail", TA_info[4]);
        cmd.Parameters.AddWithValue("@contactNumber", TA_info[5]);
        cmd.Parameters.AddWithValue("@password", TA_info[6]);
        cmd.Parameters.AddWithValue("@dob", TA_info[7]);
        cmd.Parameters.AddWithValue("@gender", TA_info[8]);
        cmd.Parameters.AddWithValue("@nationality", TA_info[9]);
        cmd.Parameters.AddWithValue("@address", TA_info[10]);
        cmd.Parameters.AddWithValue("@intake", TA_info[11]);
        cmd.Parameters.AddWithValue("@gpa",TA_info[12]);
        cmd.Parameters.AddWithValue("@achievement", TA_info[13]);
        cmd.Parameters.AddWithValue("@sdate", TA_info[14]);
        cmd.Parameters.AddWithValue("@warnings", TA_info[15]);
        cmd.Parameters.AddWithValue("@positionID", TA_info[16]);

        cmd.ExecuteNonQuery();
        db_obj.close();      
    }

    //add new location fucntion
    public void addLocation(string locationName, string locationAddress)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "INSERT INTO location (name, address) VALUES (@name, @address);";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@name", locationName);
        cmd.Parameters.AddWithValue("@address", locationAddress);
        cmd.ExecuteNonQuery();
        db_obj.close();
    }

    //add new shift function
    public void addShift(string shiftName)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "INSERT INTO shift (name) VALUES (@name);";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@name", shiftName);
        cmd.ExecuteNonQuery();
        db_obj.close();
    }

    //add new duty function
    public void addDuty(string dutyTitle, string dutyDescription)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "INSERT INTO duty (title, description) VALUES (@title, @description);";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@title", dutyTitle);
        cmd.Parameters.AddWithValue("@description", dutyDescription);
        cmd.ExecuteNonQuery();
        db_obj.close();  
    }

    //edit location function
    public void modifyLocation(int locationID, string locationName, string locationAddress)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "UPDATE location set name=@newName, address=@newAddress WHERE location_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@newName", locationName);
        cmd.Parameters.AddWithValue("@newAddress", locationAddress);
        cmd.Parameters.AddWithValue("@id", locationID);

        cmd.ExecuteNonQuery();
        dbObj.close();
    }

    //edit shift function
    public void modifyShift(int shiftID, string shiftName)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "UPDATE shfit set name=@newName WHERE shift_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@newName", shiftName);
        cmd.Parameters.AddWithValue("@id", shiftID);

        cmd.ExecuteNonQuery();
        dbObj.close();
    }

    //edit duty function
    public void modifyDuty(int dutyID, string dutyTitle, string dutyDescription)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "UPDATE duty set title=@newTitle, description=@newDescription WHERE duty_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@newTitle", dutyTitle);
        cmd.Parameters.AddWithValue("@newDescription", dutyDescription);
        cmd.Parameters.AddWithValue("@id", dutyID);

        cmd.ExecuteNonQuery();
        dbObj.close();
    }
    
    //delete location fucntion
    public void deleteLocation(int locationID)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();
        string query = "DELETE FROM location WHERE location_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@id", locationID);
        cmd.ExecuteNonQuery();
        dbObj.close();
    }

    //delete shift fucntion
    public void deleteShift(int shiftID)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();
        string query = "DELETE FROM shift WHERE shift_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@id", shiftID);
        cmd.ExecuteNonQuery();
        dbObj.close();
    }

    //delete duty fucntion
    public void deleteDuty(int dutyID)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();
        string query = "UPDATE duty_shift_location set active='0' WHERE duty_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@id", dutyID);
        cmd.ExecuteNonQuery();
        dbObj.close();
    }

    //asign a shift to location function
    public void assignShiftToLocation(string [] details)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "INSERT INTO shift_location (shift_id, location_id, start_time, end_time, day) VALUES (@s_id, @l_id, @stime, @etime, @day);";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@s_id", details[0]);
        cmd.Parameters.AddWithValue("@l_id", details[1]);
        cmd.Parameters.AddWithValue("@stime", details[2]);
        cmd.Parameters.AddWithValue("@etime", details[3]);
        cmd.Parameters.AddWithValue("@day", details[4]);
        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    //assign duty to shift
    public void assignDutyToShift(string[] details)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "INSERT INTO duty_shift_location (duty_id, shift_id, location_id, day, active) VALUES (@d_id, @s_id, @l_id, @day, @number);";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@d_id", details[0]);
        cmd.Parameters.AddWithValue("@s_id", details[1]);
        cmd.Parameters.AddWithValue("@l_id", details[2]);
        cmd.Parameters.AddWithValue("@day", details[3]);
        cmd.Parameters.AddWithValue("@number", details[4]);
        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    public void modifyDutyRequestRules(string sday, string eday, string stime, string etime, string inWindw, string outWindow)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "UPDATE duty_request_rule SET request_sday=@sday, request_eday=@eday, request_stime=@stime, request_etime=@etime, duty_signInWindow=@inWindw, duty_signOutWindow=@outWindow  WHERE rule_id=1;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@sday", sday);
        cmd.Parameters.AddWithValue("@eday", eday);
        cmd.Parameters.AddWithValue("@stime", stime);
        cmd.Parameters.AddWithValue("@etime", etime);
        cmd.Parameters.AddWithValue("@inWindw", inWindw);
        cmd.Parameters.AddWithValue("@outWindow", outWindow);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }
}