using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_TA_cancel_duty : System.Web.UI.Page
{
    //Event that happens before page load , used to handle dynamic created events
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        nextMondayDate.Text = commonMethods.getCurrentMondayDate();
        string weekDate = commonMethods.getCurrentMondayDate();
        int user_id = int.Parse(Session["id"].ToString());
        populateDuties(weekDate, user_id);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void populateDuties(string weekDate, int userID)
    {
        try
        {
            //Table duty
            Table dutiesTable = new Table();
            dutiesTable.CssClass = "table";

            TableHeaderRow thr = new TableHeaderRow();
            TableHeaderCell thc_location = new TableHeaderCell();
            thc_location.Text = "Location";
            TableHeaderCell thc_shift = new TableHeaderCell();
            thc_shift.Text= "Shift";
            TableHeaderCell thc_day = new TableHeaderCell();
            thc_day.Text = "Day";
            TableHeaderCell thc_status = new TableHeaderCell();
            thc_status.Text = "Status";
            ////-  TableHeaderCell thc_type = new TableHeaderCell();
            ////-  thc_status.Text = "Type";
            ////- TableHeaderCell thc_cancel_btn = new TableHeaderCell();
            ////- thc_status.Text = "Cancel";

            thr.Cells.Add(thc_location);
            thr.Cells.Add(thc_shift);
            thr.Cells.Add(thc_day);
            thr.Cells.Add(thc_status);
            ////-  thr.Cells.Add(thc_type);
            ////- thr.Cells.Add(thc_cancel_btn);

            dutiesTable.Rows.Add(thr);

            foreach (string duty in getDutiesToCancel(weekDate, userID))
            {
                bool disable_btn = false;
                TableRow newRow = new TableRow();
                TableCell locationCell = new TableCell();
                TableCell shiftCell = new TableCell();
                TableCell dayCell = new TableCell();
                TableCell requestStatusCell = new TableCell();
                ////-    TableCell requestTypeCell = new TableCell();

                TableCell cancelBtnCell = new TableCell();

                string[] dutyInfo = duty.Split(',');

                locationCell.Text = commonMethods.getLocationName(int.Parse(dutyInfo[0]));
                shiftCell.Text = commonMethods.getShiftName(int.Parse(dutyInfo[1]));
                dayCell.Text = dutyInfo[2];

                if(dutyInfo[4] == "0")
                {
                    switch (dutyInfo[3])
                    {
                        case "1":
                            { requestStatusCell.Text = "Draft"; break; }
                        case "2":
                            { requestStatusCell.Text = "Final"; break; }
                        case "3":
                            { requestStatusCell.Text = "Cancel Request - Rejected"; disable_btn = true; break; }
                        case "4":
                            { requestStatusCell.Text = "Cancel Request - Approved"; disable_btn = true; break; }
                        default:
                            { requestStatusCell.Text = "no"; break; }
                    }
                }
                else
                {
                    switch (dutyInfo[3])
                    {
                        case "1":
                            { requestStatusCell.Text = "Cancel Requested - Awaiting Approval"; disable_btn = true; break; }
                        case "2":
                            { requestStatusCell.Text = "Cancel Requested - Awaiting Approval"; disable_btn = true; break; }
                        case "3":
                            { requestStatusCell.Text = "Cancel Request - Rejected"; disable_btn = true;  break; }
                        case "4":
                            { requestStatusCell.Text = "Cancel Request - Approved"; disable_btn = true;  break; }
                        default:
                            { requestStatusCell.Text = "this shift have already applied cancellation, waiting for approve"; break; }
                    }
                }

                Button cancelBtn = new Button();
                cancelBtn.ID = duty;
                cancelBtn.Text = "Cancel";
                cancelBtn.CssClass = "btn btn-danger";
                cancelBtnCell.Controls.Add(cancelBtn);
                
                //adding event to the dynamic button
                cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
                //cancelBtn.Click += (s, e) => { Response.Redirect("Request Duty.aspx"); };
                newRow.Cells.Add(locationCell);
                newRow.Cells.Add(shiftCell);
                newRow.Cells.Add(dayCell);
                newRow.Cells.Add(requestStatusCell);
               ////- newRow.Cells.Add(requestTypeCell);
                newRow.Cells.Add(cancelBtnCell);
                if(disable_btn==true)
                {
                    cancelBtn.Enabled = false;
                }

                dutiesTable.Rows.Add(newRow);
            }

            dutiesContainerPlaceHolder.Controls.Add(dutiesTable);
        }
        catch(Exception ex)
        {
            msg.Text = ex.Message;
        }
    }

    protected void cancelBtn_Click(object sender, EventArgs e)
    {
        Button cancelBtn = sender as Button;
        //Response.Redirect("Request Duty.aspx");
        string[] dutyDetails = cancelBtn.ID.Split(',');

        db_connection db_object = new db_connection();
        db_object.open();

        string query = "UPDATE duty_request SET request_type=1, cancel_remark=@remark WHERE user_id=@userID AND location_id=@location AND week_start_date=@week AND shift_id=@shift AND requestForDay=@day;";

        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@userID", Session["id"].ToString());
        cmd.Parameters.AddWithValue("@location", dutyDetails[0]);
        cmd.Parameters.AddWithValue("@shift", dutyDetails[1]);
        cmd.Parameters.AddWithValue("@week", nextMondayDate.Text);
        cmd.Parameters.AddWithValue("@day", dutyDetails[2]);
        cmd.Parameters.AddWithValue("@remark", cancelRemark.Text);

        cmd.ExecuteNonQuery();
        Response.Redirect(Request.RawUrl);
        db_object.close();
        msg.Text = "Your cancellation will be considered soon.";
    }

    protected List<string> getDutiesToCancel(string weekStartDate, int userID)
    {
        List<string> duties = new List<string>();
        db_connection db_object = new db_connection();
        db_object.open();

        string query = "SELECT location_id, shift_id, requestForDay, request_status, request_type FROM duty_request WHERE request_status in (0,1,2,3,4) AND week_start_date=@week AND user_id=@user";
        MySqlCommand cmd = new MySqlCommand(query, db_object.connection);
        cmd.Parameters.AddWithValue("@week", weekStartDate);
        cmd.Parameters.AddWithValue("@user", userID);

        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            string newDuty = reader["location_id"].ToString()+ ","+ reader["shift_id"].ToString() + "," + reader["requestForDay"]+","+reader["request_status"].ToString()+","+reader["request_type"];
            duties.Add(newDuty);
        }

        db_object.close();
        return duties;
        
    }
}