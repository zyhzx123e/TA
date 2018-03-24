using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Web.Services;

public partial class pages_TA_Request_duty : System.Web.UI.Page
{
    List<int> selected_shifts = new List<int>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            populateDaysList();
            nextMondayDate.Text = commonMethods.getNextMondayDate();

            if(Session["message"] != null)
            {
                msg.Text = Session["message"].ToString();
                Session["message"] = null;
            }

            if (commonMethods.checkTime() == false)
            {
                loadShifts.Enabled = false;
                weekDaysList.Enabled = false;
                msg.Text = "Duty request is not available at the moment.";
            }
        }
    }

    //populate the content of locations + shifts into a table

    protected void populateRquestOptions(string selectedDay)
    {
        try
        {
            string selected_day = selectedDay;
            foreach (int location in getLocations(selected_day))
            {
                Table locationTable = new Table();
                locationTable.ID = location.ToString();
                locationTable.CssClass = "table-bordered table-hover table-striped col-md-3 forceInline";

                TableHeaderRow tbl_Header = new TableHeaderRow();
                TableHeaderCell tbl_cell_header = new TableHeaderCell();
                tbl_cell_header.Text = commonMethods.getLocationName(location);
                tbl_cell_header.ColumnSpan = 3;
                tbl_cell_header.CssClass = "tableHeader";
                tbl_Header.Cells.Add(tbl_cell_header);
                locationTable.Rows.Add(tbl_Header);


                foreach (int shift in getShifts(selected_day, location))
                {
                    TableRow shiftRow = new TableRow();
                    TableCell shiftCell_name = new TableCell();
                    TableCell shiftCell_timing = new TableCell();
                    TableCell shiftCell_chkBox = new TableCell();
                    CheckBox chkBox = new CheckBox();
                    chkBox.ID = shift.ToString() + "," + location.ToString() + "," + selected_day;

                    shiftCell_name.Text = getShiftDetails(shift, selected_day, location).Item1;
                    shiftCell_name.CssClass = "tableHeader";
                    shiftCell_timing.Text = getShiftDetails(shift, selected_day, location).Item2;
                    shiftCell_chkBox.Controls.Add(chkBox);
                    shiftCell_chkBox.CssClass = "tableHeader";

                    shiftRow.Cells.Add(shiftCell_name);
                    shiftRow.Cells.Add(shiftCell_timing);
                    shiftRow.Cells.Add(shiftCell_chkBox);
                    locationTable.Rows.Add(shiftRow);

                    int userID = Int32.Parse(Session["id"].ToString());
                    string weekDate = commonMethods.getNextMondayDate();
                    string weekDateMySQLFormat = weekDate.Substring(4, 4) + "-" + weekDate.Substring(2, 2) + "-" + weekDate.Substring(0, 2);

                    if (disableRequestedShift(userID, weekDateMySQLFormat).Contains(chkBox.ID))
                    {
                        chkBox.Enabled = false;
                    }
                }
                requestOptionsPlaceHolder.Controls.Add(locationTable);
            }
        }
        catch(Exception ex)
        {
            msg.Text = "There is something wrong!, error: " + ex.Message;
        }
    }

    //view states to capture the user selection before posting the page back which will remove all those selections
    protected override object SaveViewState()
    {
        var viewState = new object[2];
        //Saving the checkboxes values to the View State
        viewState[0] = weekDaysList.SelectedValue;
        viewState[1] = base.SaveViewState();
        return viewState;
    }
    protected override void LoadViewState(object savedState)
    {
        //Getting the dropdown list value from view state.
        if (savedState is object[] && ((object[])savedState).Length == 2)
        {
            var viewState = (object[])savedState;
            var selectedDay = viewState[0];
            populateRquestOptions(selectedDay.ToString());
            base.LoadViewState(viewState[1]);
        }
        else
        {
            base.LoadViewState(savedState);
        }
    }

    //get the list of days which have shifts & duties
    protected void populateDaysList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT DISTINCT day FROM duty_shift_location;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();
        
        weekDaysList.DataSource = reader;
        weekDaysList.DataTextField = "day";
        weekDaysList.DataValueField = "day";
        weekDaysList.DataBind();
        weekDaysList.Items.Insert(0, new ListItem("SELECT Day", "N/A"));

        reader.Close();
        db_obj.close();
    }

    //get the list of locations that have duty for a specific day
    protected List<int> getLocations(string day)
    {
        List<int> locationsList = new List<int>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT DISTINCT location_id FROM duty_shift_location WHERE day=@day;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@day", day);
        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            locationsList.Add(Int32.Parse(reader["location_id"].ToString()));
        }

        reader.Close();
        db_obj.close();
        return locationsList;
    }

    //get the list of shifts for a specific location on a specific day
    protected List<int> getShifts(string day, int locationID)
    {
        List<int> shiftsList = new List<int>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT DISTINCT shift_id FROM duty_shift_location WHERE day=@day AND location_id=@sid;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@sid", locationID);
        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            shiftsList.Add(Int32.Parse(reader["shift_id"].ToString()));
        }

        reader.Close();
        db_obj.close();
        return shiftsList;
    }

    //once user select the day > populate the requests options (locations + shifts)
    protected void weekDaysList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (weekDaysList.SelectedIndex != 0)
        {
            requestOptionsPlaceHolder.Visible = false;
            submitRequestBtn.Enabled = false;
            loadShifts.Enabled = true;
        }else
        {
            loadShifts.Enabled = false;
        }
        
    }

    //to return shift Timing and name at the location on a specific day instead of its ID in the roster
    protected Tuple<string, string> getShiftDetails(int shift_ID, string day, int locationID)
    {
        string name ="", timing="";
        //Dictionary<string, string> shiftDetails = new Dictionary<string, string>();
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT shift.name, CONCAT(TIME_FORMAT(start_time, '%H:%i'),'-', TIME_FORMAT(end_time, '%H:%i')) AS TIMING FROM shift_location INNER JOIN shift ON shift_location.shift_id = shift.shift_id WHERE shift_location.shift_id=@sid AND day=@day AND location_id=@lid";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@sid", shift_ID);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@lid", locationID);

        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
             name = reader["name"].ToString();
             timing = reader["TIMING"].ToString();
        }

        db_obj.close();
        return Tuple.Create(name, timing);
    }

    protected void submitRequestBtn_Click(object sender, EventArgs e)
    {
        List<string> requestedShifts = new List<string>();

        foreach (Table table in requestOptionsPlaceHolder.Controls)
        {
            foreach (TableRow row in table.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    foreach (CheckBox checkBox in cell.Controls)
                    {
                        if (checkBox.Checked)
                        {
                            try
                            {
                                string[] dutyDetails = checkBox.ID.Split(',');
                                int shift = int.Parse(dutyDetails[0].ToString());
                                int location = int.Parse(dutyDetails[1].ToString());
                                string day = dutyDetails[2];
                                queryDutyRequest(shift, location, day);
                            }
                            catch (MySql.Data.MySqlClient.MySqlException)
                            {
                                msg.Text = "You have already requested for one of those shift!";
                                return;
                            }
                            catch (Exception ex)
                            {
                                msg.Text = "There was something wrong, please contact the administrator," + " the error is: "+ex.Message;
                            }
                        }
                    }
                }
            }
        }
        
        Session["message"] = "Your request for " + weekDaysList.SelectedValue + " has been taken, thank you";
        Response.Redirect(Request.RawUrl);
    }

    //query duty request into database
    protected void queryDutyRequest(int shift_id, int location_id, string day)
    {
        int userID = int.Parse(Session["id"].ToString());

        object[] requestDetails = new object[11];
        requestDetails[0] = commonMethods.getNextMondayDate();
        requestDetails[1] = shift_id;
        requestDetails[2] = location_id;
        requestDetails[3] = day;
        requestDetails[4] = string.Format("HH:mm:ss", DateTime.Now);
        requestDetails[5] = DateTime.Now.ToString("yyy-MM-dd");
        requestDetails[6] = getDayDate(weekDaysList.SelectedValue);
        requestDetails[7] = 0;   //0 means request status is pending
        requestDetails[8] = 0;   //0 means request type is duty request
        requestDetails[9] = remark.Text;
        requestDetails[10] = 0;

        technical_assistant TA_obj = new technical_assistant();
        TA_obj.requestDuty(userID, requestDetails);
    }

    //a method to get the date of the selected request day
    protected string getDayDate(string day)
    {
        DateTime today = DateTime.Today;
        // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
        int dayDate = 1;
        switch(day)
        {
            case "Monday":
                dayDate = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                break;
            case "Tuesday":
                dayDate = ((int)DayOfWeek.Tuesday - (int)today.DayOfWeek + 7) % 7;
                break;
            case "Wednesday":
                dayDate = ((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7;
                break;
            case "Thursday":
                dayDate = ((int)DayOfWeek.Thursday - (int)today.DayOfWeek + 7) % 7;
                break;
            case "Friday":
                dayDate = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
                break;
            case "Saturday":
                dayDate = (((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 14) % 7)+7;
                break;
            default:
                msg.Text = "There is something wrong with getting the next day";
                break;
        }
   
        DateTime requiredDate = today.AddDays(dayDate);
        return requiredDate.ToString("yyy-MM-dd");
    }
    protected void loadShifts_Click(object sender, EventArgs e)
    {
        requestOptionsPlaceHolder.Visible = true;
        submitRequestBtn.Enabled = true;
    }

    //to disable the shifts which have already been requested by the user
    protected List<string> disableRequestedShift(int userID, string weekDate)
    {
        List<string> requestedShifts = new List<string>(); 

        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT shift_id, location_id, requestForDay FROM duty_request WHERE user_id=@id AND week_start_date=@weekDate";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@id", userID);
        cmd.Parameters.AddWithValue("@weekDate", weekDate);

        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            string requested_shift = reader["shift_id"].ToString() + "," + reader["location_id"].ToString() + "," + reader["requestForDay"].ToString();
            requestedShifts.Add(requested_shift);
        }

        db_obj.close();
        return requestedShifts;
    }


}


