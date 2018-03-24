using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Collections;

public partial class pages_scheduler_Assign_shifts_to_locations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateLocationsList();
            populateCurrentLocationsList();
            popluateShiftsList();
        }
    }

    protected void popluateShiftsList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM shift";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        shiftNameDropList.DataSource = reader;
        shiftNameDropList.DataTextField = "name";
        shiftNameDropList.DataValueField = "shift_id";
        shiftNameDropList.DataBind();
        shiftNameDropList.Items.Insert(0, new ListItem("SELECT SHIFT", "N/A"));

        reader.Close();
        db_obj.close();
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

    protected void populateCurrentLocationsList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        currentLocationsList.DataSource = reader;
        currentLocationsList.DataTextField = "name";
        currentLocationsList.DataValueField = "location_id";
        currentLocationsList.DataBind();
        currentLocationsList.Items.Insert(0, new ListItem("SELECT LOCATION", "N/A"));

        reader.Close();
        db_obj.close();
    }

    protected void populateCurrentDaysList(int locationID)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT DISTINCT day FROM shift_location WHERE location_id=@location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        MySqlDataReader reader = cmd.ExecuteReader();

        currentDaysList.DataSource = reader;
        currentDaysList.DataTextField = "day";
        currentDaysList.DataValueField = "day";
        currentDaysList.DataBind();
        currentDaysList.Items.Insert(0, new ListItem("SELECT DAY", "N/A"));

        reader.Close();
        db_obj.close();
    }

    protected void populateCurrentShiftsList(int locationID, string day)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT DISTINCT shift_location.shift_id, shift.name FROM shift_location INNER JOIN shift ON shift_location.shift_id = shift.shift_id WHERE location_id=@location AND day=@day";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);
        MySqlDataReader reader = cmd.ExecuteReader();

        currentShiftsDaysList.DataSource = reader;
        currentShiftsDaysList.DataTextField = "name";
        currentShiftsDaysList.DataValueField = "shift_id";
        currentShiftsDaysList.DataBind();
        currentShiftsDaysList.Items.Insert(0, new ListItem("SELECT SHIFT", "N/A"));

        reader.Close();
        db_obj.close();
    }

    protected void assignShitToLocation_btn_Click(object sender, EventArgs e)
    {
        if((locationDropList.SelectedIndex !=0 && shiftNameDropList.SelectedIndex!=0) && dayDropList.SelectedIndex!=0)
        {
            if(isStartTimeBeforeEndTime(startTime.Text, endTime.Text)==true)
            {
                if(isThereTimeInterference(int.Parse(locationDropList.SelectedValue), dayDropList.SelectedValue, startTime.Text) != true)
                {
                    string[] details = new string[5];
                    details[0] = shiftNameDropList.SelectedValue;
                    details[1] = locationDropList.SelectedValue;
                    details[2] = startTime.Text;
                    details[3] = endTime.Text;
                    details[4] = dayDropList.SelectedValue;

                    try
                    {
                        scheduler schedulerObject = new scheduler();
                        schedulerObject.assignShiftToLocation(details);
                        Response.Redirect(Request.RawUrl);
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        msg.Text = "This shift is already added to the chosen location, please either modify or remove it before performing this action!."+ex.Message;
                    }
                    catch (Exception ex1)
                    {
                        msg.Text = "There was something wrong, error: " + ex1.Message;
                    }
                }
                else
                {
                    msg.Text = "The shift cannot be added because the timing interfers with another shift at the same location.";
                }  
            }
            else
            {
                msg.Text = "Start time must be before end time!";
            }
        }
        else
        {
            msg.Text = "Shift was not assigned, please specify all the required details!.";
        }
    }

    protected bool isStartTimeBeforeEndTime(string startTime, string endTime)
    {
        bool result = false;
        if(TimeSpan.Parse(startTime) < TimeSpan.Parse(endTime))
        {
            result = true;
        }
        return result;
    }

    protected bool isThereTimeInterference(int locationID, string day, string timeToCheck)
    {
        bool result = false;

        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT end_time FROM shift_location WHERE location_id=@location AND day =@day";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);
        MySqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            if(TimeSpan.Parse(timeToCheck) < TimeSpan.Parse(reader["end_time"].ToString()))
            {
                return result = true;
            }
        }

        reader.Close();
        db_obj.close();

        return result;
    }

    protected void currentLocationsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(currentLocationsList.SelectedIndex!=0)
        {
            populateCurrentDaysList(int.Parse(currentLocationsList.SelectedValue));
            currentDaysList.Enabled = true;
            //currentShiftsDaysList.SelectedIndex = 0;
            currentShiftsDaysList.Enabled = false;
            currentStartTime.Text = "";
            currentStartTime.Enabled = false;
            currentEndTime.Text = "";
            currentEndTime.Enabled = false;
        }
        else
        {
            currentDaysList.Enabled = false;
            currentShiftsDaysList.Enabled = false;
            currentStartTime.Text = "";
            currentStartTime.Enabled = false;
            currentEndTime.Text = "";
            currentEndTime.Enabled = false;
        }
    }

    protected void currentDaysList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (currentLocationsList.SelectedIndex != 0 && currentDaysList.SelectedIndex!=0)
        {
            populateCurrentShiftsList(int.Parse(currentLocationsList.SelectedValue), currentDaysList.SelectedValue);
            currentShiftsDaysList.Enabled = true;
            currentStartTime.Text = "";
            currentStartTime.Enabled = false;
            currentEndTime.Text = "";
            currentEndTime.Enabled = false;
        }
        else
        {
            currentShiftsDaysList.Enabled = false;
            currentStartTime.Text = "";
            currentStartTime.Enabled = false;
            currentEndTime.Text = "";
            currentEndTime.Enabled = false;
        }
    }

    protected void getShiftDetails(int locationID, string day, int shiftID)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT start_time, end_time FROM shift_location WHERE location_id=@location AND day =@day AND shift_id=@shift";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@shift", shiftID);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            currentStartTime.Text = reader["start_time"].ToString();
            currentEndTime.Text = reader["end_time"].ToString();
        }

        reader.Close();
        db_obj.close();
    }

    protected void currentShiftsDaysList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((currentLocationsList.SelectedIndex != 0 && currentDaysList.SelectedIndex != 0) && currentShiftsDaysList.SelectedIndex!=0)
        {
            msg1.Text = "";
            getShiftDetails(int.Parse(currentLocationsList.SelectedValue), currentDaysList.SelectedValue, int.Parse(currentShiftsDaysList.SelectedValue));
            currentStartTime.Enabled = true;
            currentEndTime.Enabled = true;
            updateCurrentShiftBtn.Enabled = true;
            decativateCurrentShiftBtn.Enabled = true;
        }
        else
        {
            currentStartTime.Text = "";
            currentStartTime.Enabled = false;
            currentEndTime.Text = "";
            currentEndTime.Enabled = false;
        }
    }

    protected void updateCurrentShiftBtn_Click(object sender, EventArgs e)
    {
        try
        {
            if((currentLocationsList.SelectedIndex!=0 && currentShiftsDaysList.SelectedIndex!=0) && currentDaysList.SelectedIndex!=0)
            {
                if(isStartTimeBeforeEndTime(currentStartTime.Text, currentEndTime.Text) == true)
                {
                    if(isThereTimeInterference(int.Parse(currentLocationsList.SelectedValue), currentDaysList.SelectedValue, currentStartTime.Text) != true)
                    {
                        int locationID = int.Parse(currentLocationsList.SelectedValue);
                        int shiftID = int.Parse(currentShiftsDaysList.SelectedValue);
                        string day = currentDaysList.SelectedValue;
                        string newStart = currentStartTime.Text;
                        string newEnd = currentEndTime.Text;
                        db_connection db_obj = new db_connection();
                        db_obj.open();

                        string query = "UPDATE shift_location SET start_time=@newStart, end_time=@newEnd WHERE location_id=@location AND day =@day AND shift_id=@shift";
                        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
                        cmd.Parameters.AddWithValue("@location", locationID);
                        cmd.Parameters.AddWithValue("@day", day);
                        cmd.Parameters.AddWithValue("@shift", shiftID);
                        cmd.Parameters.AddWithValue("@newStart", newStart);
                        cmd.Parameters.AddWithValue("@newEnd", newEnd);
                        cmd.ExecuteNonQuery();

                        db_obj.close();
                    }
                    else
                    {
                        msg1.Text = "Shift cannot be updated, there is time intereference with another shift";
                    } 
                }
                else
                {
                    msg1.Text = "Start time must be before end time";
                }
            }
            else
            {
                msg1.Text = "Please specify all the required details";
            }
            
        }catch(Exception ex)
        {
            msg1.Text = "Shift could not be updated, time input is not valid";
        }
        
    }

    protected void decativateCurrentShiftBtn_Click(object sender, EventArgs e)
    {
        try
        {
            int locationID = int.Parse(currentLocationsList.SelectedValue);
            int shiftID = int.Parse(currentShiftsDaysList.SelectedValue);
            string day = currentDaysList.SelectedValue;

            deleteShiftRelatedDuties(locationID, day, shiftID);

            db_connection db_obj = new db_connection();
            db_obj.open();

            string query = "DELETE FROM shift_location WHERE location_id=@location AND day=@day AND shift_id=@shift";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@location", locationID);
            cmd.Parameters.AddWithValue("@day", day);
            cmd.Parameters.AddWithValue("@shift", shiftID);
            cmd.ExecuteNonQuery();

            db_obj.close();

            msg1.Text = "DONE!";
            Response.Redirect(Request.RawUrl);
        }
        catch(Exception ex)
        {
            msg1.Text = "There was something wrong!" + ex.Message;
        }
    }

    protected void deleteShiftRelatedDuties(int locationID, string day, int shiftID)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "DELETE FROM duty_shift_location WHERE location_id=@location AND day=@day AND shift_id=@shift";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@shift", shiftID);
        cmd.ExecuteNonQuery();

        db_obj.close();
    }
}