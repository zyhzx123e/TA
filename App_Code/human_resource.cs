using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
/// <summary>
/// Summary description for human_resource
/// </summary>
public class human_resource : technical_assistant
{
	public human_resource()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string[] viewTAdetails(int id)
    {
        string[] ta_details = new string[16];
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM user WHERE user_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@id", id);
        MySqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            ta_details[0] = "Name: "+ reader["user_name"].ToString();
            ta_details[1] = "TP number: " + reader["user_tp"].ToString();
            ta_details[2] = "TA code: " + reader["user_ta_code"].ToString();
            ta_details[3] = "Private Email: " + reader["private_email"].ToString();
            ta_details[4] = "TA Emai;: " + reader["ta_email"].ToString();
            ta_details[5] = "Date of birth: " + String.Format("{0:yyyy-MM-dd}", reader["dob"]);
            ta_details[6] = reader["gender"].ToString();
            ta_details[7] = "Nationality: " + reader["nationality"].ToString();
            ta_details[8] = "Address: " + reader["address"].ToString();
            ta_details[9] = "Intake code: " + reader["intake_code"].ToString();
            ta_details[10] = "GPA: " + reader["gpa"].ToString();
            ta_details[11] = "Achievements: " + reader["achievements"].ToString();
            ta_details[12] = "Became TA on: " + String.Format("{0:yyyy-MM-dd}", reader["selection_date"]).ToString();
            ta_details[13] = "Warning Letters Count: " + reader["warning_count"].ToString();
            ta_details[14] = "Contact Number: " + reader["contact_number"].ToString();
            ta_details[15] = reader["position_id"].ToString();
        }


        if (ta_details[6] == "m")  ta_details[6] = "Gender: Male";
        else ta_details[6] = "Gender: Female";

        switch(ta_details[15])
        {
            case "1":
                    ta_details[15] = "Position: Admin"; break;
            case "2":
                    ta_details[15] = "Position: TA"; break;
            case "3":
                    ta_details[15] = "Position: TOP3"; break;
            default :
                    ta_details[15] = "Position: HR"; break;
        }

        db_obj.close();
        return ta_details;
    }


}