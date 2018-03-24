using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_scheduler_Generated_Roster : System.Web.UI.Page
{
    List<string> chosenRequests = new List<string>();
    string mondayDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        dispalyCurrentWeekRoster();
		if (!IsPostBack)
        {
            populateWeekList();
            populateLocationList();
            populateTAsList();	
        }

    }

    protected void generateRoster(string generationCriteria)
    {
        try
        {
            foreach (string day in commonMethods.getRosterDays())
            {
                int numberOfLocationsForDay = commonMethods.getRosterLocations(day).Count;
                Table dayTable = new Table();
                dayTable.CssClass = "table";

                TableHeaderRow tbl_Header = new TableHeaderRow();

                TableHeaderCell tbl_cell_header = new TableHeaderCell();
                tbl_cell_header.CssClass = "colspan:'" + numberOfLocationsForDay + "';";
                tbl_cell_header.Text = day;

                tbl_Header.Cells.Add(tbl_cell_header);
                dayTable.Rows.Add(tbl_Header);

                //second row for location
                TableRow newRow = new TableRow();
                TableCell newCell = new TableCell();

                foreach (int location in commonMethods.getRosterLocations(day))
                {
                    
                    int numberOfLocation = commonMethods.getRosterLocations(day).Count;
                    Table locationTable = new Table();
                    locationTable.CssClass = "table-bordered forceInline table-hover table-responsive";
                    TableHeaderRow locationRow = new TableHeaderRow();
                    TableHeaderCell locationCell = new TableHeaderCell();
                    locationCell.ColumnSpan = 10;
                    locationCell.Text = commonMethods.getLocationName(location);
                    locationCell.BackColor = System.Drawing.Color.LightGray;
                    locationRow.Cells.Add(locationCell);
                    locationTable.Rows.Add(locationRow);

                    //third row for shifts header
                    TableHeaderRow shiftHeaderRow = new TableHeaderRow();
                    TableHeaderCell shiftHeaderCell = new TableHeaderCell();
                    shiftHeaderCell.Text = "Shift";
                    shiftHeaderCell.BackColor = System.Drawing.Color.LightGray;
                    shiftHeaderRow.Cells.Add(shiftHeaderCell);

                    //third row create a header for each duty in that location at that day
                    foreach (KeyValuePair<int, int> duty in commonMethods.getRosterDuties(day, location))
                    {
                        for (int i = 1; i <= duty.Value; i++)
                        {
                            TableHeaderCell dutyCellHedare = new TableHeaderCell();
                            dutyCellHedare.Text = commonMethods.getDutyName(duty.Key);
                            dutyCellHedare.BackColor = System.Drawing.Color.LightGray;
                            shiftHeaderRow.Cells.Add(dutyCellHedare);

                        }
                    }
                    locationTable.Rows.Add(shiftHeaderRow);

                    //to create a row for each shift at a location on a specific day
                    foreach (int shift in commonMethods.getRosterShifts(day, location))
                    {
                        int k = 0;
                        TableRow shiftRow = new TableRow();
                        TableCell shift_timing_cell = new TableCell();
                        shift_timing_cell.Text = commonMethods.getShiftTiming(shift, day, location);
                        shift_timing_cell.BackColor = System.Drawing.Color.LightGray;
                        shiftRow.Cells.Add(shift_timing_cell);

                        List<int> final_TA_list = new List<int>();
                        int requiredNumber = commonMethods.getRosterDutiesCells(day, location, shift).Count;

                        switch(generationCriteria)
                        {
                            case "draft":
                            {
                                final_TA_list = getDraft(shift, location, day, mondayDate, requiredNumber);
                                break;
                            }
                            case "final":
                            {
                                final_TA_list = getFinal(shift, location, day, mondayDate, requiredNumber);
                                break;
                            }    
                            default:
                                {
                                    break;
                                }
                        }

                        //create cells under each duty header
                        foreach (KeyValuePair<int, int> duty in commonMethods.getRosterDuties(day, location))
                        {
                            k++;
                            for (int i = 1; i <= duty.Value; i++)
                            {
                                TableCell dutyCell = new TableCell();
                                if (commonMethods.getRosterDutiesCells(day, location, shift).ContainsKey(duty.Key))
                                {
                                    int numberOfPeopleRequired = commonMethods.getRosterDutiesCells(day, location, shift).Count;
                                    if (commonMethods.getRosterDutiesCells(day, location, shift)[duty.Key] >= i)
                                    {
                                      
                                        dutyCell.BackColor = System.Drawing.Color.White;
                                        dutyCell.ForeColor = System.Drawing.Color.Black;
                                        if (k <= final_TA_list.Count)
                                        {
                                            int taID = final_TA_list[k - 1];
                                            dutyCell.Text = commonMethods.getTAName(taID);

                                            string fullRequestRecord = taID.ToString() + "," + mondayDate + "," + shift.ToString() + "," + location + "," + day;
                                            chosenRequests.Add(fullRequestRecord);
                                        }
                                        else
                                        {
                                            dutyCell.Text = "no one";
                                        }
                                    }
                                    else
                                    {
                                        dutyCell.BackColor = System.Drawing.Color.Black;
                                    }
                                }
                                else
                                {
                                    dutyCell.BackColor = System.Drawing.Color.Black;
                                }
                                shiftRow.Cells.Add(dutyCell);
                            }
                        }
                        locationTable.Rows.Add(shiftRow);
                    }


                    newCell.Controls.Add(locationTable);
                    newRow.Cells.Add(newCell);
                }

                dayTable.Rows.Add(newRow);

                rosterHolder.Controls.Add(dayTable);
            }

            Session["chosenTAsList"] = chosenRequests;
        }
        catch(Exception ex)
        {
            msg.Text = "There was something wrong, error= " + ex.Message;
        }
    }


    protected List<int> getDraft(int shift, int location, string day, string weekDate, int requiredNumber)
    {
        List<int> TAs = new List<int>();
        string query = "SELECT user_id FROM duty_request WHERE shift_id=@shift AND location_id=@location AND requestForDay=@day AND week_start_date=@week AND request_status in (1,3) ORDER BY request_order LIMIT " + requiredNumber + ";";
        db_connection db_object = new db_connection();
        db_object.open();

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@shift", shift);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@week", weekDate);

        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int userID = Int32.Parse(reader["user_id"].ToString());
            TAs.Add(userID);
        }

        db_object.close();
        return TAs;
    }

    protected List<int> getFinal(int shift, int location, string day, string weekDate, int requiredNumber)
    {
        List<int> TAs = new List<int>();
        string query = "SELECT user_id FROM duty_request WHERE shift_id=@shift AND location_id=@location AND requestForDay=@day AND week_start_date=@week AND request_status in (2,3) ORDER BY request_order LIMIT " + requiredNumber + ";";
        db_connection db_object = new db_connection();
        db_object.open();

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@shift", shift);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@week", weekDate);

        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int userID = Int32.Parse(reader["user_id"].ToString());
            TAs.Add(userID);
        }

        db_object.close();
        return TAs;
    }



    //method to help in getting the latest order in order to continue ordering when duties are added in draft and final.
    protected int getTheLatestOrder(string week)
    {
        int orderValue;
        string query = "SELECT DISTINCT request_order FROM duty_request WHERE week_start_date=@weekDate ORDER BY request_order DESC LIMIT 1;";
        db_connection db_object = new db_connection();
        db_object.open();
        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@weekDate", week);
        orderValue = int.Parse(cmd.ExecuteScalar().ToString());

        db_object.close();
        return orderValue;
    }

    protected void populateTAsList()
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT user_id, user_name FROM user";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

        MySqlDataReader reader = cmd.ExecuteReader();

        taNamesList.DataSource = reader;
        taNamesList.DataTextField = "user_name";
        taNamesList.DataValueField = "user_id";
        taNamesList.DataBind();
        taNamesList.Items.RemoveAt(0);

        db_obj.close();

        int userPositionID = int.Parse(Session["position"].ToString());

        if (commonMethods.isScheduler(userPositionID))
        {
        }
        else
        {
            taNamesList.SelectedValue = Session["id"].ToString();
            taNamesList.Enabled = false;
        }
        
    }

    protected void populateShiftsList(int locationID, string day)
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT DISTINCT shift_location.shift_id, shift.name FROM shift_location INNER JOIN shift ON shift_location.shift_id = shift.shift_id WHERE location_id=@location AND day=@day;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);
        MySqlDataReader reader = cmd.ExecuteReader();

        shiftsList.DataSource = reader;
        shiftsList.DataTextField = "name";
        shiftsList.DataValueField = "shift_id";
        shiftsList.DataBind();
        shiftsList.Items.Insert(0, new ListItem("SELECT", "N/A"));
        db_obj.close();
    }

    protected void PopulateDayList(int location_id)
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT DISTINCT day FROM shift_location WHERE location_id=@location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", location_id);
        MySqlDataReader reader = cmd.ExecuteReader();

        daysList.DataSource = reader;
        daysList.DataTextField = "day";
        daysList.DataValueField = "day";
        daysList.DataBind();
        daysList.Items.Insert(0, new ListItem("SELECT", "N/A"));

        db_obj.close();
    }

    protected void populateLocationList()
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT location_id, name FROM location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

        MySqlDataReader reader = cmd.ExecuteReader();

        locationsList.DataSource = reader;
        locationsList.DataTextField = "name";
        locationsList.DataValueField = "location_id";
        locationsList.DataBind();
        locationsList.Items.Insert(0, new ListItem("SELECT", "N/A"));
        db_obj.close();
    }

    protected void addDutyToTA_Click(object sender, EventArgs e)
    {
        int user_id = int.Parse(taNamesList.SelectedValue);
        int shift_id = int.Parse(shiftsList.SelectedValue);
        int location_id = int.Parse(locationsList.SelectedValue);
        string week = commonMethods.getCurrentMondayDate();
        string day = daysList.SelectedValue;
        if (locationsList.SelectedIndex!=0 && daysList.SelectedIndex!=0)
        {
            if(shiftsList.SelectedIndex != 0)
            {
                if(commonMethods.isFull(shift_id, location_id, week, day) != true)
                {
                    int order = getTheLatestOrder(week) + 1;
                    int status = commonMethods.checkRosterVersion(week);
                    commonMethods.takeEmptyDuty(user_id, week, shift_id, location_id, day, status, order);

                    if(commonMethods.checkRosterVersion(mondayDate) == 2)
                    {
                        insertIntoAttendance();
                    }

                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    msg.Text = "This shift is full, you may try another.";
                }
            }
            else
            {
                msg.Text = "Please specify all the shift details from the drop down menus.";
            }
        }
        else
        {
            msg.Text = "Please specify all the shift details from the drop down menus.";
        }
        
    }

    protected void locationsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(locationsList.SelectedIndex!=0)
        {
            PopulateDayList(int.Parse(locationsList.SelectedValue.ToString()));
            daysList.Enabled = true;
        }
    }

    protected void daysList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(daysList.SelectedIndex!=0)
        {
            populateShiftsList(int.Parse(locationsList.SelectedValue.ToString()), daysList.SelectedValue);
            shiftsList.Enabled = true;
        }
    }


    protected void insertIntoAttendance()
    {
        int user = int.Parse(taNamesList.SelectedValue.ToString());
        string week = nextMondayDate.Text;
        int shiftID = int.Parse(shiftsList.SelectedValue.ToString());
        int locationID = int.Parse(locationsList.SelectedValue.ToString());
        string day = daysList.SelectedValue;


        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "INSERT INTO attendance (user_id, week_start_date, shift_id, location_id, day, status, logIn_status) VALUES (@user, @week, @shift, @location, @day, 0, 0);";

        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@user", user);
        cmd.Parameters.AddWithValue("@week", week);
        cmd.Parameters.AddWithValue("@shift", shiftID);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);

        cmd.ExecuteNonQuery();

        db_obj.close();
    }

    protected void populateWeekList()
    {
        string current_week = commonMethods.getCurrentMondayDate();
        string next_week = commonMethods.getNextMondayDate();
        weekOptionsList.Items.Insert(0, new ListItem("SELECT Week", "N/A"));
        weekOptionsList.Items.Insert(1, new ListItem(current_week, current_week));
        weekOptionsList.Items.Insert(2, new ListItem(next_week, next_week));
    }

    protected void weekOptionsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selected_week = weekOptionsList.SelectedIndex;

        switch (selected_week)
        {
            case 2:
                    mondayDate = commonMethods.getNextMondayDate();
                    break;
            default:
                    mondayDate = commonMethods.getCurrentMondayDate();
                    break;
        }

        nextMondayDate.Text = mondayDate;

        if (commonMethods.checkRosterVersion(mondayDate) == 2)
        {
            versionLbl.Text = "Final";
            generateRoster("final");
            dutyAfterDraft.Visible = true;
        }
        else if (commonMethods.checkRosterVersion(mondayDate) == 1)
        {
            generateRoster("draft");
            versionLbl.Text = "Draft";
            dutyAfterDraft.Visible = true;
        }
        else
        {
            msg.Text = "The Roster is not available at the moment!";
        }

    }
	
	protected void dispalyCurrentWeekRoster(){
		mondayDate = commonMethods.getCurrentMondayDate();
		nextMondayDate.Text = mondayDate;

		if (commonMethods.checkRosterVersion(mondayDate) == 2)
		{
			versionLbl.Text = "Final";
			generateRoster("final");
			dutyAfterDraft.Visible = true;
		}
		else if (commonMethods.checkRosterVersion(mondayDate) == 1)
		{
			generateRoster("draft");
			versionLbl.Text = "Draft";
			dutyAfterDraft.Visible = true;
		}
		else
		{
			msg.Text = "The Roster is not available at the moment!";
		}
	}
}