using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
/// <summary>
/// Summary description for technical_assistant
/// </summary>
public class technical_assistant : user
{
	public technical_assistant()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public object[] viewProfile(int TA_id)
    {
        object[] TA_profile_info = new object[12];

        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT user_name, user_tp, user_ta_code, private_email, ta_email, contact_number, dob, intake_code, gpa, address, achievements, warning_count FROM user WHERE user_id = @userID; ";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@userID", TA_id);
        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            TA_profile_info[0] = reader["user_name"];
            TA_profile_info[1] = reader["user_tp"];
            TA_profile_info[2] = reader["user_ta_code"];
            TA_profile_info[3] = reader["private_email"];
            TA_profile_info[4] = reader["contact_number"];
            TA_profile_info[5] = reader["ta_email"];
            TA_profile_info[6] = reader["dob"];
            TA_profile_info[7] = reader["intake_code"];
            TA_profile_info[8] = reader["gpa"];
            TA_profile_info[9] = reader["address"];
            TA_profile_info[10] = reader["achievements"];
            TA_profile_info[11] = reader["warning_count"];
        }
        
        db_obj.close();

        return TA_profile_info;
    }

    public void editProfile(int id, object[] newInfo)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "UPDATE user SET private_email = @newPrivateEmail, address=@newAddress, intake_code=@newIntake, gpa=@newGPA, contact_number=@newNumber WHERE user_id = @userID;";

        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@newPrivateEmail", newInfo[0]);
        cmd.Parameters.AddWithValue("@newAddress",newInfo[1] );
        cmd.Parameters.AddWithValue("@newIntake",newInfo[2]);
        cmd.Parameters.AddWithValue("@newGPA",newInfo[3]);
        cmd.Parameters.AddWithValue("@newNumber",newInfo[4]);
        cmd.Parameters.AddWithValue("@userID", id);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    public void requestDuty(int id, object[] requestDetails)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string weekDate = requestDetails[0].ToString();

        string query = "INSERT INTO duty_request (user_id, week_start_date, shift_id, location_id, requestForDay, request_time, request_date, request_for_date, request_status, request_type, request_remark, request_order) VALUES (@id, @week, @shift, @location, @forDay, CURTIME(), @date, @forDate, @status, @type, @remark, @order)";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@week", weekDate);
        cmd.Parameters.AddWithValue("@shift", requestDetails[1]);
        cmd.Parameters.AddWithValue("@location", requestDetails[2]);
        cmd.Parameters.AddWithValue("@forDay", requestDetails[3]);
        //cmd.Parameters.AddWithValue("@time", requestDetails[4]);
        cmd.Parameters.AddWithValue("@date", requestDetails[5]);
        cmd.Parameters.AddWithValue("@forDate", requestDetails[6]);
        cmd.Parameters.AddWithValue("@status", requestDetails[7]);
        cmd.Parameters.AddWithValue("@type", requestDetails[8]);
        cmd.Parameters.AddWithValue("@remark", requestDetails[9]);
        cmd.Parameters.AddWithValue("@order", requestDetails[10]);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }

}