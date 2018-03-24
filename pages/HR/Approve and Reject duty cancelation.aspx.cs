using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_HR_Approve_and_Reject_duty_cancelation : System.Web.UI.Page
{

    //Event that happens before page load , used to handle dynamic created events
    protected override void OnInit(EventArgs e)
    {
            base.OnInit(e);
            nextMondayDate.Text = commonMethods.getCurrentMondayDate();
            populateDutiesList(nextMondayDate.Text);
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void populateDutiesList(string weekDate)
    {
        try
        {
            //Table duty
            Table cancelRequestTable = new Table();
            cancelRequestTable.CssClass = "table";

            TableHeaderRow thr = new TableHeaderRow();
            TableHeaderCell thc_ta_name = new TableHeaderCell();
            thc_ta_name.Text = "TA name";
            TableHeaderCell thc_location = new TableHeaderCell();
            thc_location.Text = "Location";
            TableHeaderCell thc_shift = new TableHeaderCell();
            thc_shift.Text = "Shift";
            TableHeaderCell thc_day = new TableHeaderCell();
            thc_day.Text = "Day";
            TableHeaderCell thc_remark= new TableHeaderCell();
            thc_remark.Text = "Remark";

            thr.Cells.Add(thc_ta_name);
            thr.Cells.Add(thc_location);
            thr.Cells.Add(thc_shift);
            thr.Cells.Add(thc_day);
            thr.Cells.Add(thc_remark);

            cancelRequestTable.Rows.Add(thr);

            foreach (string duty in getDutiesToCancelRequests(weekDate))
            {
                bool disable_btn = false;
                TableRow newRow = new TableRow();
                TableCell TA_nameCell = new TableCell();
                TableCell locationCell = new TableCell();
                TableCell shiftCell = new TableCell();
                TableCell dayCell = new TableCell();
                TableCell requestStatusCell = new TableCell();
                TableCell buttonsCell = new TableCell();

                string[] dutyInfo = duty.Split(',');
                TA_nameCell.Text = commonMethods.getTAName(int.Parse(dutyInfo[0]));
                locationCell.Text = commonMethods.getLocationName(int.Parse(dutyInfo[1]));
                shiftCell.Text = commonMethods.getShiftName(int.Parse(dutyInfo[2]));
                dayCell.Text = dutyInfo[3];


                switch (dutyInfo[4])
                {
                    case "1":
                        { requestStatusCell.Text = dutyInfo[5] + "PENDING - Draft"; break; }
                    case "2":
                        { requestStatusCell.Text = dutyInfo[5] + "PENDING - Final"; break; }
                    case "3":
                        { requestStatusCell.Text = dutyInfo[5] + "Rejected"; disable_btn = true; break; }
                    case "4":
                        { requestStatusCell.Text = dutyInfo[5] + "Approved"; disable_btn = true; break; }
                    default:
                        { requestStatusCell.Text = "no"; break; }
                }

                Button approveBtn = new Button();
                approveBtn.ID = duty+",approve";
                approveBtn.Text = "Apprve";
                approveBtn.CssClass = "btn btn-success";
                buttonsCell.Controls.Add(approveBtn);
                //adding event to the dynamic button
                approveBtn.Click += new EventHandler(this.approveBtn_Click);

                Button RejectBtn = new Button();
                RejectBtn.ID = duty+",reject";;
                RejectBtn.Text = "Reject";
                RejectBtn.CssClass = "btn btn-danger";
                buttonsCell.Controls.Add(RejectBtn);

                //adding event to the dynamic button
                RejectBtn.Click += new EventHandler(this.RejectBtn_Click);

                newRow.Cells.Add(TA_nameCell);
                newRow.Cells.Add(locationCell);
                newRow.Cells.Add(shiftCell);
                newRow.Cells.Add(dayCell);
                newRow.Cells.Add(requestStatusCell);
                newRow.Cells.Add(buttonsCell);
                if (disable_btn == true)
                {
                    approveBtn.Enabled = false;
                    RejectBtn.Enabled = false;
                }

                cancelRequestTable.Rows.Add(newRow);
            }

            dutiesContainerPlaceHolder.Controls.Add(cancelRequestTable);
        }
        catch (Exception ex)
        {
            msg.Text = ex.Message;
        }
    }

    protected List<string> getDutiesToCancelRequests(string weekStartDate)
    {
        List<string> duties = new List<string>();
        db_connection db_object = new db_connection();
        db_object.open();

        string query = "SELECT user_id, location_id, shift_id, requestForDay, request_status, request_type, cancel_remark FROM duty_request WHERE request_type=1 AND week_start_date=@week";
        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@week", weekStartDate);

        MySqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string newCancelRequest = reader["user_id"] + "," + reader["location_id"].ToString() + "," + reader["shift_id"].ToString() + "," + reader["requestForDay"] + "," + reader["request_status"].ToString()+ "," + reader["cancel_remark"].ToString();
            duties.Add(newCancelRequest);
        }

        db_object.close();
        return duties;

    }


    protected void approveBtn_Click(object sender, EventArgs e)
    {
        Button cancelBtn = sender as Button;
        string[] dutyDetails = cancelBtn.ID.Split(',');

        db_connection db_object = new db_connection();
        db_object.open();

        string query = "UPDATE duty_request SET request_status=4 WHERE user_id=@userID AND location_id=@location AND week_start_date=@week AND shift_id=@shift AND requestForDay=@day;";

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@userID", dutyDetails[0]);
        cmd.Parameters.AddWithValue("@location", dutyDetails[1]);
        cmd.Parameters.AddWithValue("@shift", dutyDetails[2]);
        cmd.Parameters.AddWithValue("@week", nextMondayDate.Text);
        cmd.Parameters.AddWithValue("@day", dutyDetails[3]);

        cmd.ExecuteNonQuery();
        db_object.close();


        db_connection db_object1 = new db_connection();
        db_object1.open();

        string query1 = "UPDATE attendance SET system_remark='canceled' WHERE user_id=@userID AND location_id=@location AND week_start_date=@week AND shift_id=@shift AND day=@day;";

        MySqlCommand cmd1 = new MySqlCommand(query1, db_object1.connection);
        cmd1.Parameters.AddWithValue("@userID", dutyDetails[0]);
        cmd1.Parameters.AddWithValue("@location", dutyDetails[1]);
        cmd1.Parameters.AddWithValue("@shift", dutyDetails[2]);
        cmd1.Parameters.AddWithValue("@week", nextMondayDate.Text);
        cmd1.Parameters.AddWithValue("@day", dutyDetails[3]);

        cmd1.ExecuteNonQuery();
        Response.Redirect(Request.RawUrl);
        db_object1.close();
    }

    protected void RejectBtn_Click(object sender, EventArgs e)
    {
        Button cancelBtn = sender as Button;
        string[] dutyDetails = cancelBtn.ID.Split(',');

        db_connection db_object = new db_connection();
        db_object.open();

        string query = "UPDATE duty_request SET request_status=3 WHERE user_id=@userID AND location_id=@location AND week_start_date=@week AND shift_id=@shift AND requestForDay=@day;";

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@userID", dutyDetails[0]);
        cmd.Parameters.AddWithValue("@location", dutyDetails[1]);
        cmd.Parameters.AddWithValue("@shift", dutyDetails[2]);
        cmd.Parameters.AddWithValue("@week", nextMondayDate.Text);
        cmd.Parameters.AddWithValue("@day", dutyDetails[3]);

        cmd.ExecuteNonQuery();
        Response.Redirect(Request.RawUrl);
        db_object.close();
    }

    
}