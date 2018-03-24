using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_scheduler_Assign_duties_to_shifts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateLocationsList();
            populateDutyList();
        }
    }


    protected void populateLocationsList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        locationDropList.DataSource = reader;
        locationDropList.DataTextField = "name";
        locationDropList.DataValueField = "location_id";
        locationDropList.DataBind();
        locationDropList.Items.Insert(0, new ListItem("SELECT LOCATION", "N/A"));
        reader.Close();
        db_obj.close();
    }

    protected void populateDayList(int selectedLocationID)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT DISTINCT day FROM shift_location WHERE location_id = @locationID";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@locationID", selectedLocationID);
        MySqlDataReader reader = cmd.ExecuteReader();

        dayList.DataSource = reader;
        dayList.DataTextField = "day";
        dayList.DataValueField = "day";
        dayList.DataBind();
        dayList.Items.Insert(0, new ListItem("SELECT DAY", "N/A"));
        reader.Close();
        db_obj.close();
    }

    protected void populateDutyList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM duty";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        DutyDropList.DataSource = reader;
        DutyDropList.DataTextField = "title";
        DutyDropList.DataValueField = "duty_id";
        DutyDropList.DataBind();

        reader.Close();
        db_obj.close();
    }
    protected void populateShiftsList(int selectedLocationID, string day)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT shift_location.shift_id, shift.name FROM shift_location INNER JOIN Shift ON shift_location.shift_id = shift.shift_id WHERE shift_location.location_id = @locationID AND shift_location.day=@day";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@locationID", selectedLocationID);
        cmd.Parameters.AddWithValue("@day", day);
        MySqlDataReader reader = cmd.ExecuteReader();

        shiftNameDropList.DataSource = reader;
        shiftNameDropList.DataTextField = "name";
        shiftNameDropList.DataValueField = "shift_id";
        shiftNameDropList.DataBind();
        shiftNameDropList.Items.Insert(0, new ListItem("SELECT SHIFT", "N/A"));
        reader.Close();
        db_obj.close();
    }
    protected void locationDropList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(locationDropList.SelectedIndex!=0)
        {
            int selectedLocationID = int.Parse(locationDropList.SelectedValue.ToString());
            populateDayList(selectedLocationID);
            dayList.Enabled = true;
        }
        else
        {
            dayList.Enabled = false;
            shiftNameDropList.SelectedIndex = 0;
            dayList.SelectedIndex = 0;
            shiftNameDropList.Enabled = false;
        }
        
    }

    protected void dayList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (locationDropList.SelectedIndex != 0)
        {
            int selectedLocationID = int.Parse(locationDropList.SelectedValue.ToString());
            string selectedDay = dayList.SelectedValue;
            populateShiftsList(selectedLocationID, selectedDay);
            shiftNameDropList.Enabled = true;
        }
        else
        {
            shiftNameDropList.Enabled = false;
        }
        
    }

    protected void assignDutyToShift_btn_Click(object sender, EventArgs e)
    {
        if ((dayList.SelectedIndex != 0 && locationDropList.SelectedIndex != 0) && shiftNameDropList.SelectedIndex != 0)
        {
            string[] details = new string[5];
            details[0] = DutyDropList.SelectedValue;
            details[1] = shiftNameDropList.SelectedValue;
            details[2] = locationDropList.SelectedValue;
            details[3] = dayList.SelectedValue;
            details[4] = "1";

            try
            {
                scheduler schedulerObject = new scheduler();
                schedulerObject.assignDutyToShift(details);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                msg.Text = "The duty was not assigned, as this details exist for another duty in the system.";
            }
        }
        else
        {
            msg.Text = "Duty is not assigned because not all required details are specified";
        }
        
    }
    
}