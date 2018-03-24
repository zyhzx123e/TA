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
        if (!IsPostBack)
        {
            populateLocationList();
            populateTAsList();
        }
        string rosterGenerationCriteria = "";
        mondayDate = commonMethods.getNextMondayDate();
        nextMondayDate.Text = mondayDate;
        if (Session["generation_choice"] != null)
        {
            rosterGenerationCriteria = Session["generation_choice"].ToString();
            versionLbl.Text = "Before Draft (regenration is possible)";
            generateRoster(rosterGenerationCriteria);
        }
        else
        {
            if (commonMethods.checkRosterVersion(mondayDate) == 2)
            {
                versionLbl.Text = "Final";
                generateRoster("final");
                draftBtn.Enabled = false;
                dutyAfterDraft.Visible = true;
            }
            else if (commonMethods.checkRosterVersion(mondayDate) == 1)
            {
                generateRoster("draft");
                draftBtn.Visible = false;
                versionLbl.Text = "Draft";
                finalBtn.Enabled = true;
                dutyAfterDraft.Visible = true;
            }
            else
            {
                msg.Text = "You have not chosen generation criteria!";
            }
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
                    locationTable.CssClass = "table-bordered forceInline";
                    TableHeaderRow locationRow = new TableHeaderRow();
                    TableHeaderCell locationCell = new TableHeaderCell();
                    locationCell.ColumnSpan = 10;
                    locationCell.Text = commonMethods.getLocationName(location);
                    locationRow.Cells.Add(locationCell);
                    locationTable.Rows.Add(locationRow);

                    //third row for shifts header
                    TableHeaderRow shiftHeaderRow = new TableHeaderRow();
                    TableHeaderCell shiftHeaderCell = new TableHeaderCell();
                    shiftHeaderCell.Text = "Shift";

                    shiftHeaderRow.Cells.Add(shiftHeaderCell);

                    //third row create a header for each duty in that location at that day
                    foreach (KeyValuePair<int, int> duty in commonMethods.getRosterDuties(day, location))
                    {
                        for (int i = 1; i <= duty.Value; i++)
                        {
                            TableHeaderCell dutyCellHedare = new TableHeaderCell();
                            dutyCellHedare.Text = commonMethods.getDutyName(duty.Key);
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
                        shiftRow.Cells.Add(shift_timing_cell);

                        List<int> final_TA_list = new List<int>();
                        int requiredNumber = commonMethods.getRosterDutiesCells(day, location, shift).Count;

                        switch(generationCriteria)
                        {
                            case "shuffle":
                                {
                                    final_TA_list = shuffle(shift, location, day, mondayDate, requiredNumber);
                                    break;
                                }
                            case "fcfs":
                                {
                                    final_TA_list = firtComeFirstServe(shift, location, day, mondayDate, requiredNumber);
                                    break;
                                }
                            case "seniority":
                            {
                                final_TA_list = seniority(shift, location, day, mondayDate, requiredNumber);
                                break;
                            }
                            case "top_recipe":
                                {
                                    final_TA_list = top_recipe(shift, location, day, mondayDate, requiredNumber);
                                    break;
                                }
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
                                    final_TA_list = shuffle(shift, location, day, mondayDate, requiredNumber);
                                    break;
                                }
                        }

                        //create cells under each duty header
                        foreach (KeyValuePair<int, int> duty in commonMethods.getRosterDuties(day, location))
                        {
                            k++;
                            //int requiredNumber = commonMethods.getRosterDutiesCells(day, location, shift).Count;
                            for (int i = 1; i <= duty.Value; i++)
                            {
                                TableCell dutyCell = new TableCell();
                                if (commonMethods.getRosterDutiesCells(day, location, shift).ContainsKey(duty.Key))
                                {
                                    int numberOfPeopleRequired = commonMethods.getRosterDutiesCells(day, location, shift).Count;
                                    if (commonMethods.getRosterDutiesCells(day, location, shift)[duty.Key] >= i)
                                    {
                                        dutyCell.BackColor = System.Drawing.Color.Green;
                                        dutyCell.ForeColor = System.Drawing.Color.White;
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
            draftBtn.Enabled = true;
        }
        catch(Exception ex)
        {
            msg.Text = "There was something wrong, error= " + ex.Message;
        }
    }

    //Select TAs based on TOP 3 recipe
    protected List<int> top_recipe(int shift, int location, string day, string weekDate, int requiredNumber)
    {
        List<int> TAs = new List<int>();
        //SELECT duty_request.user_id, user.position_id FROM duty_request JOIN user ON duty_request.user_id = user.user_id  WHERE shift_id = 7 AND location_id = 1 AND requestForDay = 'Monday' AND week_start_date = '2016-03-28' AND request_status = 0 AND request_type = 0 ORDER BY (user.position_id = 3) DESC, request_date, request_time ASC LIMIT 4
        string query = "SELECT duty_request.user_id, user.position_id FROM duty_request JOIN user ON duty_request.user_id = user.user_id  WHERE shift_id=@shift AND location_id=@location AND requestForDay=@day AND week_start_date=@week AND request_status=0 AND request_type=0 ORDER BY (user.position_id = 3) DESC, request_date, request_time ASC LIMIT " + requiredNumber + ";";
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

    //Select TAs on a first come first serve basis
    protected List<int> firtComeFirstServe(int shift, int location, string day, string weekDate, int requiredNumber)
    {
        List<int> TAs = new List<int>();
        string query = "SELECT user_id FROM duty_request WHERE shift_id=@shift AND location_id=@location AND requestForDay=@day AND week_start_date=@week AND request_status=0 AND request_type=0 ORDER BY request_date, request_time ASC LIMIT " + requiredNumber + ";";
        db_connection db_object = new db_connection();
        db_object.open();

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@shift", shift);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@day", day);
        cmd.Parameters.AddWithValue("@week", weekDate);

        MySqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            int userID = Int32.Parse(reader["user_id"].ToString());
            TAs.Add(userID);
        }

        db_object.close();
        return TAs;
    }

    //Select TAs based on Seniority
    protected List<int> seniority(int shift, int location, string day, string weekDate, int requiredNumber)
    {
        List<int> TAs = new List<int>();
        string query = "SELECT duty_request.user_id, user.user_id, user.selection_date FROM duty_request INNER JOIN user ON duty_request.user_id = user.user_id WHERE shift_id=@shift AND location_id=@location AND requestForDay=@day AND week_start_date=@week AND request_status=0 AND request_type=0 ORDER BY user.selection_date, rand() ASC LIMIT " + requiredNumber + ";";
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

    //Select TAs on a shuffle basis
    protected List<int> shuffle(int shift, int location, string day, string weekDate, int requiredNumber)
    {
        List<int> TAs = new List<int>();
        string query = "SELECT user_id FROM duty_request WHERE shift_id=@shift AND location_id=@location AND requestForDay=@day AND week_start_date=@week AND request_status=0 AND request_type=0 ORDER BY RAND() LIMIT " + requiredNumber + ";";
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
            //TAs.Add(getTAName(userID));
        }

        db_object.close();
        return TAs;
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

    //a method to set the request status in the database from pending, draft or final...
    protected void setRequestStatus(int userID, string weekDate, int shiftID, int locationID, string day, int status, int orderNumber)
    {
        string query = "UPDATE duty_request SET request_status=@status, request_order=@orderNumber WHERE user_id=@user AND week_start_date=@week AND shift_id=@shift AND location_id=@location AND requestForDay=@day;";
        db_connection db_object = new db_connection();
        db_object.open();

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@status", status);
        cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", weekDate);
        cmd.Parameters.AddWithValue("@shift", shiftID);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);

        cmd.ExecuteNonQuery();

        db_object.close();
    }

    //to create attendance record
    protected void createAttendanceRecord(int userID, string weekDate, int shiftID, int locationID, string day)
    {
        string query = "INSERT INTO attendance (user_id, week_start_date, shift_id, location_id, day, status, logIn_status) VALUES (@user, @week, @shift, @location, @day, 0, 0);";
        db_connection db_object = new db_connection();
        db_object.open();

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@user", userID);
        cmd.Parameters.AddWithValue("@week", weekDate);
        cmd.Parameters.AddWithValue("@shift", shiftID);
        cmd.Parameters.AddWithValue("@location", locationID);
        cmd.Parameters.AddWithValue("@day", day);

        cmd.ExecuteNonQuery();

        db_object.close();
    }


    //add attendance record for each final duty
    protected void insertAttendanceRecords(List<string> TAList)
    {
        int i = 1;
        foreach (string s in TAList)
        {
            List<object> pks = new List<object>();
            string[] request_PKs = s.Split(',');
            foreach (string pk in request_PKs)
            {
                pks.Add(pk);
            }
            createAttendanceRecord(int.Parse(pks[0].ToString()), pks[1].ToString(), int.Parse(pks[2].ToString()), int.Parse(pks[3].ToString()), pks[4].ToString());
            i++;
        }

    }

    // a method to transfer the duty request status from pending to draft of final
    protected void trasferStatus(int status, List<string> TAList)
    {
        int i = 1;
        foreach (string s in TAList)
        {
            List<object> pks = new List<object>();
            string[] request_PKs = s.Split(',');
            foreach(string pk in request_PKs)
            {
                pks.Add(pk);
            }
            setRequestStatus(int.Parse(pks[0].ToString()), pks[1].ToString(), int.Parse(pks[2].ToString()), int.Parse(pks[3].ToString()), pks[4].ToString(), status, i);
            i++;
        }

    }


    protected void draftBtn_Click(object sender, EventArgs e)
    {
        List<string> ChosenTAs = (List<string>)Session["chosenTAsList"];
        trasferStatus(1, ChosenTAs);
        Response.Redirect("../common/Home.aspx");
    }

    protected void finalBtn_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> ChosenTAs = (List<string>)Session["chosenTAsList"];
            trasferStatus(2, ChosenTAs);
            insertAttendanceRecords(ChosenTAs);
            msg.Text = "The final roster for this have been generated!";
       
            Response.Redirect("../common/Home.aspx");
        }catch(Exception ex)
        {
            msg.Text = "could not generate the final roster, error: " + ex.Message;
       
        }
        
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

        int userPosition = int.Parse(Session["position"].ToString());

        if (commonMethods.isScheduler(userPosition))
        {
        }
        else
        {
            taNamesList.SelectedValue = Session["id"].ToString();
            taNamesList.Enabled = false;
            finalBtn.Visible = false;
            draftBtn.Visible = false;
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
        if (locationsList.SelectedIndex != 0 && daysList.SelectedIndex != 0)
        {
            if (shiftsList.SelectedIndex != 0)
            {
                if (commonMethods.isFull(shift_id, location_id, week, day) != true)
                {
                    int order = getTheLatestOrder(week) + 1;
                    int status = commonMethods.checkRosterVersion(week);
                    commonMethods.takeEmptyDuty(user_id, week, shift_id, location_id, day, status, order);

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
        if (locationsList.SelectedIndex != 0)
        {
            PopulateDayList(int.Parse(locationsList.SelectedValue.ToString()));
            daysList.Enabled = true;
        }
    }

    protected void daysList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (daysList.SelectedIndex != 0)
        {
            populateShiftsList(int.Parse(locationsList.SelectedValue.ToString()), daysList.SelectedValue);
            shiftsList.Enabled = true;
        }
    }
}