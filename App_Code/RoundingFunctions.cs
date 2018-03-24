using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient; 

/// <summary>
/// Summary description for RoundingFunctions
/// </summary>
public class RoundingFunctions
{
    public RoundingFunctions()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<problem> getProblems(int locationID, char type)
    {
        List<problem> problemsList = new List<problem>();

        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM problem WHERE location_id=@location AND type=@type AND status = 'p'";
        MySqlCommand command = new MySqlCommand(query, db_obj.connection);
        command.Parameters.AddWithValue("@type", type);
        command.Parameters.AddWithValue("@location", locationID);

        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            problem prob = new problem();
            prob.id = int.Parse(reader["problem_id"].ToString());
            prob.venue = reader["venue"].ToString();
            prob.pc = reader["pc"].ToString();
            prob.description = reader["description"].ToString();
            prob.location_id = int.Parse(reader["location_id"].ToString());
            prob.shift = reader["shift"].ToString();
            prob.date = reader["date"].ToString();
            prob.status = char.Parse(reader["status"].ToString());
            prob.type = char.Parse(reader["type"].ToString());
            prob.added_by = int.Parse(reader["added_by"].ToString());

            problemsList.Add(prob);
        }
        db_obj.close();

        return problemsList;
    }

    public static void addNewProblem(problem prob)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "INSERT INTO problem (venue, pc, description, location_id, shift, date, status, type, added_by) VALUES (@venue, @pc, @description, @location_id, @shift, @date, @status, @type, @added_by)";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@venue", prob.venue);
        cmd.Parameters.AddWithValue("@pc", prob.pc);
        cmd.Parameters.AddWithValue("@description", prob.description);
        cmd.Parameters.AddWithValue("@location_id", prob.location_id);
        cmd.Parameters.AddWithValue("@shift", prob.shift);
        cmd.Parameters.AddWithValue("@date", prob.date);
        cmd.Parameters.AddWithValue("@status", prob.status);
        cmd.Parameters.AddWithValue("@type", prob.type);
        cmd.Parameters.AddWithValue("@added_by", prob.added_by);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    public static problem getProblem(int problemID)
    {
        problem prob = new problem();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM problem WHERE problem_id=@problem_id";
        MySqlCommand command = new MySqlCommand(query, db_obj.connection);
        command.Parameters.AddWithValue("@problem_id", problemID);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            prob.id = int.Parse(reader["problem_id"].ToString());
            prob.venue = reader["venue"].ToString();
            prob.pc = reader["pc"].ToString();
            prob.description = reader["description"].ToString();
            prob.location_id = int.Parse(reader["location_id"].ToString());
            prob.shift = reader["shift"].ToString();
            prob.date = reader["date"].ToString();
            prob.status = char.Parse(reader["status"].ToString());
            prob.type = char.Parse(reader["type"].ToString());
            prob.added_by=int.Parse(reader["added_by"].ToString());
        }

        db_obj.close();

        return prob;
    }

    public static string getTypeName(char c)
    {
        string typeName;
        typeName = c == 'r' ? "Rounding" : "QC";

        return typeName;
    }

    public static void addTroubleshoot(int problemID, int userID, string Date, char action, string content)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "INSERT INTO prob_user (prob_id, user_id, date, action, content) VALUES (@prob, @user, @date, @action, @content)";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@prob", problemID);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@date", Date);
        cmd.Parameters.AddWithValue("@action", action);
        cmd.Parameters.AddWithValue("@content", content);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    public static List<string> getTroublshootingSteps(int problemID)
    {
        List<string> troubleshootingList = new List<string>();

        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT user_id, date, content FROM prob_user WHERE prob_id=@problem_id";
        MySqlCommand command = new MySqlCommand(query, db_obj.connection);
        command.Parameters.AddWithValue("@problem_id", problemID);
        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            string troubleshoot = reader["content"].ToString() + "," + reader["date"].ToString() + "," + reader["user_id"];
            troubleshootingList.Add(troubleshoot);
        }

        db_obj.close();

        return troubleshootingList;
    } 

    public static void solveProblem(int problemID)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "UPDATE problem SET status='s' WHERE problem_id=@problemID";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@problemID", problemID);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }
    
}