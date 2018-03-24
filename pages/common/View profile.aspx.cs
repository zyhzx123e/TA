using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_TA_View_profile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateFields();
        }        
    }

    private void populateFields()
    {
        string hr_count, probs_count,position_name,selection_date;

        try
        {
            technical_assistant ta = new technical_assistant();

            int id = Int32.Parse(Session["id"].ToString());
            object[] taInfo = new object[13];
            Array.Copy(ta.viewProfile(id), taInfo, 12);


         
            db_connection db_obj = new db_connection();
            db_obj.open();

            int userID = Int32.Parse(Session["id"].ToString());


            /* 
             *SELECT CAST(SUM(FORMAT(TIME_TO_SEC(TIMEDIFF(fypdb.attendance.logout_time, fypdb.attendance.login_time))/3600, 2)) AS DECIMAL(5,2))  AS 'working_hour' FROM 
    attendance inner join user on user.user_id=attendance.user_id WHERE user.user_id=89 AND attendance.logout_date between '2015-12-01 00:00:00' and '2016-06-01 23:59:59'
         
         
             */
            //inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by
            string query = "SELECT CAST(SUM(FORMAT(TIME_TO_SEC(TIMEDIFF(fypdb.attendance.logout_time, fypdb.attendance.login_time))/3600, 2)) AS DECIMAL(5,2)) AS 'working_hour' FROM " +
            " attendance  WHERE attendance.user_id='" + userID + "' ";

            string query1 = " SELECT Count(prob_user.user_id) AS 'Number of solved problems' FROM " +
        " prob_user  WHERE prob_user.user_id='" + userID + "' AND prob_user.action = 's'  ;";

            string query2 = " SELECT fypdb.position.title from fypdb.position inner join fypdb.user on fypdb.user.position_id=fypdb.position.position_id where fypdb.user.user_id='"+userID+"' ";

            string query3 = " SELECT user.selection_date from fypdb.user where fypdb.user.user_id='" + userID + "' ";


            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@userID", userID);

            MySqlCommand cmd1 = new MySqlCommand(query1, db_obj.connection);
            cmd1.Parameters.AddWithValue("@userID", userID);

            MySqlCommand cmd2 = new MySqlCommand(query2, db_obj.connection);
            cmd2.Parameters.AddWithValue("@userID", userID);

            MySqlCommand cmd3 = new MySqlCommand(query3, db_obj.connection);
            cmd3.Parameters.AddWithValue("@userID", userID);



            if ((null != cmd.ExecuteScalar()) || (null != cmd1.ExecuteScalar()) || (null != cmd2.ExecuteScalar()) || (null != cmd3.ExecuteScalar()))
            {
                hr_count = cmd.ExecuteScalar().ToString();


                probs_count = cmd1.ExecuteScalar().ToString();

                position_name = cmd2.ExecuteScalar().ToString();

                selection_date = cmd3.ExecuteScalar().ToString();
            
              hr.Text = hr_count;
            problems.Text = probs_count;
            position_id.Text = position_name;
            selection_date_lb.Text = selection_date;
            }

          

            db_obj.close();

            name.Text = taInfo[0].ToString();
            tp_number.Text = taInfo[1].ToString();
            ta_number.Text = taInfo[2].ToString();
            privateEmail.Text = taInfo[3].ToString();
            contactNumber.Text = taInfo[4].ToString();
            taEmail.Text = taInfo[5].ToString();
            dob.Text = taInfo[6].ToString();
            intake.Text = taInfo[7].ToString();
            gpa.Text = taInfo[8].ToString();
            warningLetters.Text = taInfo[11].ToString();
            achievements.Text = taInfo[10].ToString();
            address.Text = taInfo[9].ToString();
        }
        catch(Exception ex)
        {
            msg.Text = "There was something wrong.";
        }
    }
    protected void save_btn_Click(object sender, EventArgs e)
    {





        technical_assistant TA_obj = new technical_assistant();
        
        object[] TA_newInfo = new object[5];
        TA_newInfo[0]= privateEmail.Text;
        TA_newInfo[1] = address.Text;
        TA_newInfo[2] = intake.Text;
        TA_newInfo[3] = gpa.Text;
        TA_newInfo[4] = contactNumber.Text;
        int userID = Int32.Parse(Session["id"].ToString());

        try
        {
            TA_obj.editProfile(userID, TA_newInfo);
            msg.Text = "Your details has been updated, thank you!";
        } 
        catch(Exception ex)
        {
            msg.Text = "Information could not be updated, error: " + ex.Message;
        }
        
  
    }


   

    protected void working_hr_Click(object sender, EventArgs e)
    {
        string working_hr;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string fromtDate = working_hr_fromDate.Text;
        string toDate = working_hr_toDate.Text;
        int userID = Int32.Parse(Session["id"].ToString());


        /* 
         *SELECT CAST(SUM(FORMAT(TIME_TO_SEC(TIMEDIFF(fypdb.attendance.logout_time, fypdb.attendance.login_time))/3600, 2)) AS DECIMAL(5,2))  AS 'working_hour' FROM 
attendance inner join user on user.user_id=attendance.user_id WHERE user.user_id=89 AND attendance.logout_date between '2015-12-01 00:00:00' and '2016-06-01 23:59:59'
         
         
         */
        //inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by
        string query = "SELECT CAST(SUM(FORMAT(TIME_TO_SEC(TIMEDIFF(fypdb.attendance.logout_time, fypdb.attendance.login_time))/3600, 2)) AS DECIMAL(5,2)) AS 'working_hour' FROM " +
        " attendance inner join user on user.user_id=attendance.user_id WHERE user.user_id='"+userID+"' AND " +
        "attendance.logout_date between '" + fromtDate + " 00:00:00' and '" + toDate + " 23:59:00' ";

        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@userID", userID);
     
        if ((null != cmd.ExecuteScalar()) && (cmd.ExecuteScalar().ToString()!=""))
        {
            working_hr = cmd.ExecuteScalar().ToString()+" hour";
        }
        else
        {
            working_hr = "You did not work in that specific time period! pls work harder!";
        }
        working_hour.Text = working_hr;

        db_obj.close();
    
    }
    protected void problem_count_btn_Click(object sender, EventArgs e)
    {

        string problem_count_str;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string fromDate = problem_countfromDate.Text;
        string toDate = problem_counttoDate.Text;
        int userID = Int32.Parse(Session["id"].ToString());


        /* 
     SELECT Count(prob_user.user_id) AS 'Number of solved problems' FROM " +
        " prob_user  WHERE prob_user.user_id='"+userID+"' AND prob_user.action = 's' AND date between '" + from_date + " 00:00:00' and '" + toDate + " 23:59:00' ;";

         
         */
        //inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by
        string query = " SELECT Count(prob_user.user_id) AS 'Number of solved problems' FROM " +
        " prob_user  WHERE prob_user.user_id='" + userID + "' AND prob_user.action = 's' AND date between '" + fromDate + " 00:00:00' and '" + toDate + " 23:59:00' ;";

        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@userID", userID);

        if ((null != cmd.ExecuteScalar()) && (cmd.ExecuteScalar().ToString() != "0"))
        {
            problem_count_str = cmd.ExecuteScalar().ToString() + " problems were solved by you";
        }
        else
        {
            problem_count_str = "You did not solve any problem in that specific time period! pls contribute more!";
        }
        problem_count.Text = problem_count_str;

        db_obj.close();
    }
}