using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_scheduler_manage_roster_specifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateLocationList();
            populateShiftList();
            populateDutyList();
        }
    }

    /* Save details wether they are new details or modification for existing record */
    protected void save_location_btn_Click(object sender, EventArgs e)
    {
        string name = location_name.Text.Trim();
        string address = location_address.Text.Trim();

        scheduler scheduler_obj = new scheduler();

        //to add values if no editing is taking place.
        if(cancel_location_btn.Visible==false)
        {
            try
            {
                if(string.IsNullOrEmpty(name)||string.IsNullOrEmpty(address))
                {
                    location_msg.Text = "Please fill in the full details.";
                }
                else
                {
                    scheduler_obj.addLocation(name, address);
                    Response.Redirect(Request.RawUrl);
                    
                }
                
            }
            catch (Exception ex)
            {
                location_msg.Text = "Location could not be added, error: " + ex.Message;
            }
        }
        else  //to save values if editing is taking place.
        {
            try
            {
                int locationID = Int32.Parse(locations.SelectedValue);
                scheduler_obj.modifyLocation(locationID, name, address);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                location_msg.Text = "Location could not be modified, error: " + ex.Message;
            }
        }
        
    }
    protected void save_shift_btn_Click(object sender, EventArgs e)
    {
        string name = shift_name.Text.Trim();

        scheduler scheduler_obj = new scheduler();
        
        if(cancel_shift_btn.Visible==false)
        {
            try
            {
                if(string.IsNullOrEmpty(name))
                {
                    shift_msg.Text = "Please specify the shift name.";
                }
                else
                {
                    scheduler_obj.addShift(name);
                    Response.Redirect(Request.RawUrl);
                }
                
            }
            catch (Exception ex)
            {
                shift_msg.Text = "Shift could not be added, error: " + ex.Message;
            }
        }else
        {
            try
            {
                int shiftID = int.Parse(shifts.SelectedValue);
                scheduler_obj.modifyShift(shiftID, name);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                shift_msg.Text = "Shift could not be modified, error: " + ex.Message;
            }
        }

       
    }
    protected void save_duty_btn_Click(object sender, EventArgs e)
    {
        string title = duty_title.Text.Trim();
        string description = duty_desc.Text.Trim();

        scheduler scheduler_obj = new scheduler();
        
        if(cancel_duty_btn.Visible==false)
        {
            try
            {
                if(string.IsNullOrEmpty(title)||string.IsNullOrEmpty(description))
                {
                    duty_msg.Text = "Please specify the duty's full details.";
                }
                else
                {
                    scheduler_obj.addDuty(title, description);
                    Response.Redirect(Request.RawUrl);
                }          
            }
            catch (Exception ex)
            {
                duty_msg.Text = "Duty could not be added, error: " + ex.Message;
            }
        }
        else
        {
            try
            {
                int dutyID = Int32.Parse(duties.SelectedValue);
                scheduler_obj.modifyDuty(dutyID, title, description);
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                duty_msg.Text = "Duty could not be modified, error: " + ex.Message;
            }
        }
        
    }

    /*** enable edit & delete buttons once user select an item from the lists ***/

    protected void locations_SelectedIndexChanged(object sender, EventArgs e)
    {
        edit_location.Enabled = true;
        delete_location.Enabled = true;
    }
    protected void shifts_SelectedIndexChanged(object sender, EventArgs e)
    {
        edit_shift.Enabled = true;
        delete_shift.Enabled = true;
    }
    protected void duties_SelectedIndexChanged(object sender, EventArgs e)
    {
        edit_duty.Enabled = true;
        delete_duty.Enabled = true;
    }

    /*** Edit button action for all the lists : retrieve values of the required item to be edited ***/
    protected void edit_location_Click(object sender, EventArgs e)
    {
        int selected_location_id = Int32.Parse(locations.SelectedValue);
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * FROM location WHERE location_id =@id ;";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@id", selected_location_id);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            location_name.Text = reader["name"].ToString();
            location_address.Text = reader["address"].ToString();

            reader.Close();
            db_obj.close();
            save_location_btn.Text = "Save Changes";
            cancel_location_btn.Visible = true;
        }
        catch (Exception ex)
        {
            location_msg.Text = "Something went wrong, error: " + ex.Message;
        }
    }
    protected void edit_shift_Click(object sender, EventArgs e)
    {
        int selected_shift_id = Int32.Parse(shifts.SelectedValue);
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * FROM shift WHERE shift_id =@id ;";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@id", selected_shift_id);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            shift_name.Text = reader["name"].ToString();

            reader.Close();
            db_obj.close();
            save_shift_btn.Text = "Save Changes";
            cancel_shift_btn.Visible = true;
        }
        catch (Exception ex)
        {
            shift_msg.Text = "Something went wrong, error: " + ex.Message;
        }
    }
    protected void edit_duty_Click(object sender, EventArgs e)
    {
        int selected_duty_id = Int32.Parse(duties.SelectedValue);
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * FROM duty WHERE duty_id =@id ;";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@id", selected_duty_id);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            duty_title.Text = reader["title"].ToString();
            duty_desc.Text = reader["description"].ToString();

            reader.Close();
            db_obj.close();
            save_duty_btn.Text = "Save Changes";
            cancel_duty_btn.Visible = true;
        }
        catch (Exception ex)
        {
            duty_msg.Text = "Something went wrong, error: " + ex.Message;
        }
    }

    //popluate location list box
    protected void populateLocationList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        locations.DataSource = reader;
        locations.DataTextField = "name";
        locations.DataValueField = "location_id";
        locations.DataBind();

        reader.Close();
        db_obj.close();
    }

    //popluate Shifts list box
    protected void populateShiftList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM shift";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        shifts.DataSource = reader;
        shifts.DataTextField = "name";
        shifts.DataValueField = "shift_id";
        shifts.DataBind();

        reader.Close();
        db_obj.close();
    }

    //popluate Duties list box
    protected void populateDutyList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM duty";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        duties.DataSource = reader;
        duties.DataTextField = "title";
        duties.DataValueField = "duty_id";
        duties.DataBind();

        reader.Close();
        db_obj.close();
    }

    /** Cancel button actions **/
    protected void cancel_location_btn_Click(object sender, EventArgs e)
    {
        clearLocationForm();
    }
    protected void cancel_shift_btn_Click(object sender, EventArgs e)
    {
        clearShiftForm();
    }
    protected void cancel_duty_btn_Click(object sender, EventArgs e)
    {
        clearDutyForm();
    }

    /** Clear forms if user cancel editing opration **/
    protected void clearLocationForm()
    {
        location_name.Text = null;
        location_address.Text = null;
        locations.ClearSelection();
        save_location_btn.Text = "ADD";
        edit_location.Enabled = false;
        delete_location.Enabled = false;
        cancel_location_btn.Visible = false;
    }
    protected void clearShiftForm()
    {
        shift_name.Text = null;
        shifts.ClearSelection();
        save_shift_btn.Text = "ADD";
        edit_shift.Enabled = false;
        delete_shift.Enabled = false;
        cancel_shift_btn.Visible = false;
    }
    protected void clearDutyForm()
    {
        duty_title.Text = null;
        duty_desc.Text = null;
        duties.ClearSelection();
        save_duty_btn.Text = "ADD";
        edit_duty.Enabled = false;
        delete_duty.Enabled = false;
        cancel_duty_btn.Visible = false;
    }


    /* DELETE actions */
    protected void delete_location_Click(object sender, EventArgs e)
    {
        int locationID = Int32.Parse(locations.SelectedValue);
        scheduler scheduler_obj = new scheduler();
        try
        {
            scheduler_obj.deleteLocation(locationID);
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        { 
            location_msg.Text = "Location could not be deleted, please delete all the associated shifts and duties with this location before deleting it";
        }
    }
    protected void delete_shift_Click(object sender, EventArgs e)
    {
        int shiftID = Int32.Parse(shifts.SelectedValue);
        scheduler scheduler_obj = new scheduler();
        try
        {
            scheduler_obj.deleteShift(shiftID);
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            shift_msg.Text = "Shift could not be deleted, please delete all the associated duities this shift before deleting it ";
        }
    }
    protected void delete_duty_Click(object sender, EventArgs e)
    {
        int dutyID = Int32.Parse(duties.SelectedValue);
        scheduler scheduler_obj = new scheduler();
        try
        {
            scheduler_obj.deleteDuty(dutyID);
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            duty_msg.Text = "Duty could not be deleted, please delete the associated recoreds before deleting it." ;
        }
    }
}