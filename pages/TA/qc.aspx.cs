using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_TA_qc : System.Web.UI.Page
{
    //Event that happens before page load , used to handle dynamic created events
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
		
	try
        {
            int user_id = int.Parse(Session["id"].ToString());
        }catch(Exception ex){
            Response.Redirect("../common/Sign out.aspx");
        }
		
        if (!IsPostBack)
        {
            populateCurrentLocationsList();
            popluateShiftsList();
        }
        else
        {
            try
            {
                string test = Session["chosenQC_location"].ToString();
                populate_qc_problems(test);
            }
            catch (Exception ex)
            {

            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack & (location_list.SelectedIndex != -1 & location_list.SelectedIndex != 0))
        {
            location_list.Enabled = false;
        }

        if (load_problems.Enabled == true)
        {
            reset_location.Enabled = false;
        }
    }

    protected void populateCurrentLocationsList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM location";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        location_list.DataSource = reader;
        location_list.DataTextField = "name";
        location_list.DataValueField = "location_id";
        location_list.DataBind();
        location_list.Items.Insert(0, new ListItem("SELECT LOCATION", "N/A"));
        reader.Close();

        MySqlCommand cmd1 = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader1 = cmd1.ExecuteReader();

        newProblemLocation.DataSource = reader1;
        newProblemLocation.DataTextField = "name";
        newProblemLocation.DataValueField = "location_id";
        newProblemLocation.DataBind();
        newProblemLocation.Items.Insert(0, new ListItem("SELECT LOCATION", "N/A"));
        reader1.Close();
        db_obj.close();
    }

    protected void popluateShiftsList()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM shift";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        shiftsList.DataSource = reader;
        shiftsList.DataTextField = "name";
        shiftsList.DataValueField = "name";
        shiftsList.DataBind();
        shiftsList.Items.Insert(0, new ListItem("SELECT SHIFT", "N/A"));

        reader.Close();
        db_obj.close();
    }

    protected void location_list_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selected_location = location_list.SelectedValue.ToString();
        Session["chosenQC_location"] = selected_location;
        msg.Text = "";
    }

    protected void populate_qc_problems(string location_id)
    {
        Table roundingTable = new Table();
        roundingTable.CssClass = "table table-bordered table-hover table-striped table-responsive";

        TableHeaderRow thr = new TableHeaderRow();
        TableHeaderCell thc_venue = new TableHeaderCell();
        thc_venue.Text = "Venue";
        TableHeaderCell thc_desc = new TableHeaderCell();
        thc_desc.Text = "Description";
        TableHeaderCell thc_shift = new TableHeaderCell();
        thc_shift.Text = "Shift";
        TableHeaderCell thc_date = new TableHeaderCell();
        thc_date.Text = "Date";
        TableHeaderCell thc_troubleshoot = new TableHeaderCell();
        thc_troubleshoot.Text = "Troubleshoot";

        thr.Cells.Add(thc_venue);
        thr.Cells.Add(thc_desc);
        thr.Cells.Add(thc_shift);
        thr.Cells.Add(thc_date);
        thr.Cells.Add(thc_troubleshoot);

        roundingTable.Rows.Add(thr);

        int locationID = int.Parse(location_id);

        foreach (problem prob in RoundingFunctions.getProblems(locationID, 'q'))
        {
            TableRow newRow = new TableRow();
            TableCell venueCell = new TableCell();
            TableCell descCell = new TableCell();
            TableCell shiftCell = new TableCell();
            TableCell dateCell = new TableCell();
            TableCell troubleshootBtnCell = new TableCell();

            venueCell.Text = prob.venue;
            descCell.Text = prob.description;
            shiftCell.Text = prob.shift;
            dateCell.Text = prob.date;

            Button troubleshootBtn = new Button();
            troubleshootBtn.ID = prob.id.ToString();
            troubleshootBtn.Text = "Troubleshoot";
            troubleshootBtn.CssClass = "btn-sm btn-warning";
            troubleshootBtnCell.Controls.Add(troubleshootBtn);

            troubleshootBtn.Click += new EventHandler(this.troubleshootBtn_Click);
            newRow.Cells.Add(venueCell);
            newRow.Cells.Add(descCell);
            newRow.Cells.Add(shiftCell);
            newRow.Cells.Add(dateCell);
            newRow.Cells.Add(troubleshootBtnCell);

            roundingTable.Rows.Add(newRow);
        }

        roundingProblems_ph.Controls.Add(roundingTable);
    }

    protected void troubleshootBtn_Click(object sender, EventArgs e)
    {
        Button troubleshootBtn = sender as Button;
        Session["problem_id"] = troubleshootBtn.ID;
     //   Session["chosenQC_location"] = null;
        Response.Redirect("troubleshoot_problem.aspx");
    }

    protected void table(object sender, EventArgs e)
    {
        if (Session["chosenQC_location"] != null)
        {
            string test = Session["chosenQC_location"].ToString();
            populate_qc_problems(test);
        }
    }


    protected void add_new_problem_Click(object sender, EventArgs e)
    {

    }

    protected void submit_prob_btn_Click(object sender, EventArgs e)
    {
        //Response.Redirect("http://www.google.com");

        if (validateAddingNewProblem() == false)
        {
            msg.Text = "The problem was not added, please fill up all details";
        }
        else
        {
            problem prob = new problem();
            prob.venue = venueTxt.Text;
            prob.location_id = int.Parse(newProblemLocation.SelectedValue);
            prob.description = newProblemDescription.Text;
            prob.shift = shiftsList.SelectedValue;
            prob.date = DateTime.Now.ToString("yyy-MM-dd");
            prob.type = 'q';
            prob.status = 'p';
            prob.added_by=int.Parse(Session["id"].ToString());

            RoundingFunctions.addNewProblem(prob);

            msg.Text = "The problem has been added, thank you";
			emptyPopUpFrom();
        }

        
    }


    protected void load_problems_Click(object sender, EventArgs e)
    {
        if (location_list.SelectedIndex == 0)
        {
            msg.Text = "Please select a location before clicking Load button.";
        }
        else
        {
            load_problems.Enabled = false;
            reset_location.Enabled = true;
        }
    }


    protected void reset_location_Click(object sender, EventArgs e)
    {
        location_list.Enabled = true;
      //  Session["chosenQC_location"] = null;
        location_list.SelectedIndex = 0;
        load_problems.Enabled = true;
        reset_location.Enabled = false;
        msg.Text = "";
    }

    protected bool validateAddingNewProblem()
    {
        bool result = true;

        List<string> values = new List<string>();

        values.Add(venueTxt.Text);
        values.Add(newProblemDescription.Text);

        foreach (string s in values)
        {
            if (string.IsNullOrEmpty(s)) return false;
        }

        if (newProblemLocation.SelectedIndex == 0) return false;

        if (shiftsList.SelectedIndex == 0) return false;

        return result;
    }
	
	protected void emptyPopUpFrom()
    {
        venueTxt.Text = "";
        newProblemLocation.SelectedIndex =0 ;
        newProblemDescription.Text = "";
        shiftsList.SelectedIndex = 0;
    }
}