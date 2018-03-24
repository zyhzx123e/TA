using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for user
/// </summary>
public class user
{
    public string user_name { get; set; }
    public string user_tp { get; set; }
    public string user_ta_code { get; set; }
    public string private_email { get; set; }
    public string ta_email { get; set; }
    public string password { get; set; }
    public DateTime dob { get; set; }
    public char gender { get; set; }
    public string nationality { get; set; }
    public string address { get; set; }
    public string intake_code { get; set; }
    public double gpa { get; set; }
    public string achievement { get; set; }
    public DateTime selection_date { get; set; }
    public int warning_count { get; set; }
    public DateTime resing_date { get; set; }

	public user()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public Boolean login(string tp, string password)
    {
        bool i = false;
        db_connection db = new db_connection();
        db.open();


        string query = "SELECT user_id, user_name, user_tp, password, active, position_id FROM user";
        MySqlCommand cmd = new MySqlCommand(query, db.connection);

        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader["user_tp"].ToString() == tp && reader["password"].ToString() == password)
            {
                if(int.Parse(reader["active"].ToString())==1)
                {
                    System.Web.HttpContext.Current.Session["id"] = reader["user_id"];
                    System.Web.HttpContext.Current.Session["username"] = reader["user_name"];
                    System.Web.HttpContext.Current.Session["tp"] = reader["user_tp"];
                    System.Web.HttpContext.Current.Session["position"] = reader["position_id"];
                    i = true;
                    db.close();
                    break;
                }else{
                    return i;
                } 
            }
        }
        return i;
      }

    public void changePassword(int id, string newPassword)
    {
        db_connection db = new db_connection();
        db.open();

        string query = "UPDATE user SET password=@newPassword WHERE user_id=@userID;";
        MySqlCommand cmd = new MySqlCommand(query, db.connection);
        cmd.Parameters.AddWithValue("@newPassword", newPassword);
        cmd.Parameters.AddWithValue("@userID", id);

        cmd.ExecuteNonQuery();

        db.close();
    }
}