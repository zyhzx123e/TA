using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_administrator_Work_with_CSV_data : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void import_users_Click(object sender, EventArgs e)
    {
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * INTO OUTFILE 'C:/Users/Shaher/OneDrive/FYP/Payroll/Project/fypSys/csv/users.csv' FIELDS TERMINATED BY ',' FROM user";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

            cmd.ExecuteNonQuery();

            db_obj.close();
        }
        catch(MySqlException ex)
        {
            msg.Text = "This file already exist, please backup and delete the current csv file to be able to export another one" + ex.Message; 
        }
        
    }
    protected void export_locations_Click(object sender, EventArgs e)
    {
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * INTO OUTFILE 'C:/Users/Shaher/OneDrive/FYP/Payroll/Project/fypSys/csv/locations.csv' FIELDS TERMINATED BY ',' FROM location";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

            cmd.ExecuteNonQuery();

            db_obj.close();
        }
        catch (MySqlException ex)
        {
            msg.Text = "This file already exist, please backup and delete the current csv file to be able to export another one" + ex.Message;
        }
    }
    protected void export_duty_requests_Click(object sender, EventArgs e)
    {
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * INTO OUTFILE 'C:/Users/Shaher/OneDrive/FYP/Payroll/Project/fypSys/csv/duty_requests.csv' FIELDS TERMINATED BY ',' FROM duty_request";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

            cmd.ExecuteNonQuery();

            db_obj.close();
        }
        catch (MySqlException ex)
        {
            msg.Text = "This file already exist, please backup and delete the current csv file to be able to export another one" + ex.Message;
        }
    }
}