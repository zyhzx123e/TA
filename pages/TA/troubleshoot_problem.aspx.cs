using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_TA_troubleshoot_problem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Session["problem_id"].ToString()))
        {
            int problemID = int.Parse(Session["problem_id"].ToString());
            populateProblemDetails(RoundingFunctions.getProblem(problemID));
            populate_troublshoot_table(problemID);
        }
    }


    protected void populateProblemDetails(problem prob)
    {


        string ta_name = commonMethods.getTAName(prob.added_by);


        id_lbl.Text = prob.id.ToString();
        pc_lbl.Text = prob.pc;
        location_lbl.Text = commonMethods.getLocationName(prob.location_id);
        venue_lbl.Text = prob.venue;
        desc_lbl.Text = prob.description;
        added_lbl.Text = DateTime.Parse(prob.date).ToString("yyy-MM-dd") + " " + prob.shift;
      
        //by whom
        by_whom_lbl.Text = ta_name;
        type_lbl.Text = RoundingFunctions.getTypeName(prob.type);
    }

    protected void submutNewTroubleshootBtn_Click(object sender, EventArgs e)
    {
        int problemID = int.Parse(Session["problem_id"].ToString());
        int userID = int.Parse(Session["id"].ToString());
        string date = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
        char action = solvedProblem.Checked ? 's' : 't';
        string content = newTroubelshootStep.Text;
        if(content.Contains(",")){
            msg.Text = "Sorry, comma (,) is invalid character. You may use - or . to separate your sentences.";
            return;
        }
        
        RoundingFunctions.addTroubleshoot(problemID, userID, date, action, content);

        if (action == 's')
        {
            RoundingFunctions.solveProblem(problemID);
            submutNewTroubleshootBtn.Enabled = false;
            msg.Text = "Impressive! thanks for solving the problem.";
            return;
        }

        Response.Redirect(Request.RawUrl);
    }


    protected void populate_troublshoot_table(int problem_id)
    {
        Table troublshootTable = new Table();
        troublshootTable.CssClass = "table table-bordered table-striped";

        TableHeaderRow thr = new TableHeaderRow();
        TableHeaderCell thc_step = new TableHeaderCell();
        thc_step.Text = "Step";
        TableHeaderCell thc_date = new TableHeaderCell();
        thc_date.Text = "On";
        TableHeaderCell thc_by = new TableHeaderCell();
        thc_by.Text = "Person";

        thr.Cells.Add(thc_step);
        thr.Cells.Add(thc_date);
        thr.Cells.Add(thc_by);

        troublshootTable.Rows.Add(thr);

        foreach (string troubleshoot in RoundingFunctions.getTroublshootingSteps(problem_id))
        {
            TableRow newRow = new TableRow();
            TableCell stepCell = new TableCell();
            TableCell onDayCell = new TableCell();
            TableCell byWhomCell = new TableCell();

            string[] troubleshootInfo = troubleshoot.Split(',');

            stepCell.Text = troubleshootInfo[0];
            onDayCell.Text = troubleshootInfo[1];
            byWhomCell.Text = getUserName(int.Parse(troubleshootInfo[2]));
            
            newRow.Cells.Add(stepCell);
            newRow.Cells.Add(onDayCell);
            newRow.Cells.Add(byWhomCell);

            troublshootTable.Rows.Add(newRow);
        }

        troubleshoot_ph.Controls.Add(troublshootTable);
    }

    public static string getUserName(int userID)
    {
        string userName;
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT user_name FROM user WHERE user_id=@userID";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@userID", userID);

        userName = cmd.ExecuteScalar().ToString();

        db_obj.close();
        return userName;
    }

    protected void backBtn_Click(object sender, EventArgs e)
    {


       //history.back();
        if (Session["chosenQC_location"] != null)
        {
            Response.Redirect("qc.aspx");
        }
        else
        {
            Response.Redirect("rounding.aspx");
        }
    }
}