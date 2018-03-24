using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;

using MySql.Data.MySqlClient;

public class administrator : user
{
	public administrator()
	{
		
	}

    //add new position function
    public void addNewPosition(string title, int payRate , int quotaHours)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "INSERT INTO position (title, pay_rate, monthly_quota_hours) VALUES (@title, @payRate, @monthlyQuota);";
        
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@title", title);
        cmd.Parameters.AddWithValue("@payRate", payRate);
        cmd.Parameters.AddWithValue("@monthlyQuota", quotaHours);

        cmd.ExecuteNonQuery();
        dbObj.close();

        addFunctionsForPosition(title);
    }

    //assign the position to default function
    protected void addFunctionsForPosition(string positionTitle)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query1 = "SELECT position_id FROM position WHERE title = @title";
        MySqlCommand cmd1 = new MySqlCommand(query1, dbObj.connection);
        cmd1.Parameters.AddWithValue("@title", positionTitle);
        string positionID = cmd1.ExecuteScalar().ToString();

        string query2 = "SELECT function_id FROM function";
        MySqlCommand cmd2 = new MySqlCommand(query2, dbObj.connection);
        MySqlDataReader reader = cmd2.ExecuteReader();

        List<string> functionsList = new List<string>();

        while(reader.Read())
        {
            functionsList.Add(reader["function_id"].ToString());
        }

        reader.Close();

        foreach(string function in functionsList)
        {
            string query3 = "INSERT INTO function_position (function_id, position_id, given) VALUES (@functionID, @positionID, 0)";
            MySqlCommand cmd3 = new MySqlCommand(query3, dbObj.connection);
            cmd3.Parameters.AddWithValue("@functionID", function);
            cmd3.Parameters.AddWithValue("@positionID", positionID);
            cmd3.ExecuteNonQuery();
        }
        

        dbObj.close();
    }

    //edit position function
    public void editPosition(int id, string newTitle, int newPayRate, int newQutoa)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "UPDATE position set title=@newTitle, pay_rate=@newPayRate, monthly_quota_hours=@newQuota WHERE position_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@newTitle", newTitle);
        cmd.Parameters.AddWithValue("@newPayRate", newPayRate);
        cmd.Parameters.AddWithValue("@newQuota", newQutoa);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();

        dbObj.close();
    }

    public void deletePosition(int id)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "DELETE FROM position WHERE position_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();

        dbObj.close();
    }

    public void deleteUser(int id)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "UPDATE user SET active=0 WHERE user_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
        dbObj.close();
    }

    public void editUser(int id, object[] newInfo)
    {
        db_connection dbObj = new db_connection();
        dbObj.open();

        string query = "UPDATE user set position_id=@positionID, warning_count=@wanringCount, password=@newPass, achievements=@achievement, resigning_date=@rdate "
        + "WHERE user_id=@id;";
        MySqlCommand cmd = new MySqlCommand(query, dbObj.connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@positionID", newInfo[0]);
        cmd.Parameters.AddWithValue("@wanringCount", newInfo[1]);
        cmd.Parameters.AddWithValue("@newPass", newInfo[2]);
        cmd.Parameters.AddWithValue("@achievement", newInfo[3]);
        cmd.Parameters.AddWithValue("@rdate", newInfo[4]);

        cmd.ExecuteNonQuery();

        dbObj.close();
    }

    //Function to get function & position IDs based on grid view row and update the joint table access value 
    public void updateFunctionAccess(string functionTitle, string positionTitle, int accessValue)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        //get function ID from db
        string getFunctionIDquery = " SELECT function_id FROM function WHERE title = @function_title;";
        MySqlCommand cmd1 = new MySqlCommand(getFunctionIDquery, db_obj.connection);
        cmd1.Parameters.AddWithValue("@function_title", functionTitle);
        int functionID = (int)cmd1.ExecuteScalar();

        //get position ID from db
        string getPositionIDquery = " SELECT position_id FROM position WHERE title = @position_title;";
        MySqlCommand cmd2 = new MySqlCommand(getPositionIDquery, db_obj.connection);
        cmd2.Parameters.AddWithValue("@position_title", positionTitle);
        int positionID = (int)cmd2.ExecuteScalar();

        //update function_position table
        string giveAccessToFunctionQuery = "UPDATE function_position SET given=@access WHERE function_id =@FID AND position_id=@PID;";
        MySqlCommand cmd3 = new MySqlCommand(giveAccessToFunctionQuery, db_obj.connection);
        cmd3.Parameters.AddWithValue("@FID", functionID);
        cmd3.Parameters.AddWithValue("@PID", positionID);
        cmd3.Parameters.AddWithValue("@access", accessValue);
        cmd3.ExecuteNonQuery();

        db_obj.close();
    }
}