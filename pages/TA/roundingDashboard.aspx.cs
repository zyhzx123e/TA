using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;

using System.Web.UI.WebControls.WebParts;
using MySql.Data.MySqlClient;

using System.Data;
using System.Data.SqlClient;

using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Configuration;

public partial class pages_TA_roundingDashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        populateLocationLists();


       // GetPC(pcReport_pc.Text);


    }


    protected void fill_report(string query, List<string> columns)
    {
        int myCoutner = 0;
        Table reportTable = new Table();
        reportTable.CssClass = "table table-bordered table-hover table-striped table-responsive";

        TableHeaderRow thr = new TableHeaderRow();
        foreach (string column in columns)
        {
            TableHeaderCell newColumn = new TableHeaderCell();
            newColumn.Text = column;
            thr.Cells.Add(newColumn);
        }

        reportTable.Rows.Add(thr);

        db_connection db_obj = new db_connection();
        db_obj.open();
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            myCoutner++;
            TableRow newRow = new TableRow();
            foreach (string s in columns)
            {
                TableCell newCell = new TableCell();
                newCell.Text = reader[s].ToString();
                newRow.Cells.Add(newCell);
            }
            reportTable.Rows.Add(newRow);
        }

        if (myCoutner ==0)
        {
            reportMsg.Text = "There was no result.";
            showSearchPanel();
            return;
        }

        search_result_ph.Controls.Add(reportTable);
    }



    public static string[] GetPC(string prefix)
    {
        List<string> PCS = new List<string>();
        db_connection db_obj1 = new db_connection();

        using (db_obj1.connection)
        {
            string sql = "select fypdb.problem.pc from fypdb.problem where fypdb.problem.pc like '" + prefix + "%'";
            // conn.ConnectionString = ConfigurationManager.ConnectionStrings[db_obj1.getConn()].ConnectionString;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                sql = "select fypdb.problem.pc from fypdb.problem where fypdb.problem.pc like '"+prefix +"%'";
                cmd.CommandText = sql;
                
                cmd.Connection = db_obj1.connection;
                db_obj1.open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        PCS.Add(string.Format("{0}-{1}", sdr["pc"]));
                    }
                }
                db_obj1.close();
            }
        }
        return PCS.ToArray();
    }
   




                 //               public string[] GetPC(string prefix)
                 //               {
                //
                 //                  db_connection db_obj1 = new db_connection();
                //
                //                    //**************************
                //                     int count = 10;
                //                     db_obj1.open();
                //                     string sql = "select fypdb.problem.pc from fypdb.problem where fypdb.problem.pc like @prefix ";
                //             MySqlDataAdapter da = new MySqlDataAdapter(sql,db_obj1.connection);
                //
                //             da.SelectCommand.Parameters.Add("@prefix", MySqlDbType.VarChar, 50).Value = prefix  +'%'; 
                //
                //             DataTable dt = new DataTable(); 
                //             da.Fill(dt); 
                //             string[] items = new string[dt.Rows.Count];
                //             int i = 0; 
                //             foreach (DataRow dr in dt.Rows) 
                //             { 
                //              items.SetValue(dr["pc"].ToString(),i); 
                //              i++;
                //             } db_obj1.close();
                //             return items;

                //                }


    protected void showReport()
    {
        reportDiv.Visible = true;
        searchPanelDiv.Visible = false;
        searchBtn.Text = "Do Another Search";
        searchBtn.Visible = true;
    }

    protected void showSearchPanel()
    {
        searchPanelDiv.Visible = true;
        reportDiv.Visible = false;
        searchBtn.Visible = false;
    }

    protected void searchBtn_Click(object sender, EventArgs e)
    {
        if (searchBtn.Text.Equals("Search"))
        {
            showReport();
        }
        else
        {
            showSearchPanel();
            reportMsg.Text = "";
        }
    }

    protected void populateLocationLists()
    {
        if (pcReport_locationList.Items.Count == 0)
        {
            foreach (int location_id in commonMethods.getRosterLocations("Monday"))
            {
                pcReport_locationList.Items.Add(new ListItem(commonMethods.getLocationName(location_id), location_id.ToString()));
                locationReport_locationList.Items.Add(new ListItem(commonMethods.getLocationName(location_id), location_id.ToString()));
                solvedProblemsDropList.Items.Add(new ListItem(commonMethods.getLocationName(location_id), location_id.ToString()));
            }
        }
    }

    protected void pcReport_btn_Click(object sender, EventArgs e)
    {
        string locationValue = pcReport_locationList.SelectedValue;
        string labValue = pcReport_lab.Text;
        string pcValue = pcReport_pc.Text;
        string fromtDate = pcReport_fromDate.Text;
        string toDate = pcReport_toDate.Text;
        //inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by
        string query = "SELECT problem_id AS 'Problem ID', description, status, shift, fypdb.problem.date, fypdb.user.user_name AS 'Added by' FROM " +
        " problem inner join user on user.user_id=problem.added_by WHERE user.user_id = problem.added_by AND " +
        "pc='" + pcValue + "' AND venue= '" + labValue + "' AND location_id = '" + locationValue + "' AND "+
        "problem.date between '" + fromtDate + " 00:00:00' and '" + toDate + " 23:59:00' order by date ASC";
//The problem is the table [prob_user] DID NOT join with table [problem], so here it use prob_user.date, MySql would not execute
        //so have to get inner join to prob_user or just use date.
        List<string> columnsList = new List<string>();
        columnsList.Add("Problem ID");
        columnsList.Add("description");
        columnsList.Add("status");
        columnsList.Add("shift");
        columnsList.Add("date");
        columnsList.Add("Added by");

        fill_report(query, columnsList);
        showReport();
    }

    protected void locationRpeort_btn_Click(object sender, EventArgs e)
    {
        string query = "";
        List<string> columnsList = new List<string>();

        string locationValue = locationReport_locationList.SelectedValue;
        string venue = locationReport_venue.Text;
        string fromtDate = locationReport_fromDate.Text;
        string toDate = locationReport_toDate.Text;
        string type = locationReport_problemType.SelectedValue;

        

        switch (type)
        {
            case "r":
                {
                    
                    columnsList.Add("Problem ID");
                    columnsList.Add("pc");
                    columnsList.Add("description");
                    columnsList.Add("status");
                    columnsList.Add("shift");
                    columnsList.Add("date");
                    columnsList.Add("Added by");

                    if (!string.IsNullOrEmpty(venue))
                    {
                        query = "SELECT problem_id AS 'Problem ID', pc, description, status, shift, date, fypdb.user.user_name AS 'Added by' FROM " +
                        " problem inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by WHERE fypdb.user.user_id = fypdb.problem.added_by AND venue='" + venue + "' AND location_id = '" + locationValue + "' AND date between '" + fromtDate + " 00:00:00' and '" + toDate + " 23:59:00' AND type='r' order by date ASC";
                    }
                    else
                    {
                        query = "SELECT problem_id AS 'Problem ID', pc, description, venue, status, shift, date, added_by fypdb.user.user_name AS 'Added by' FROM " +
                        " problem inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by WHERE fypdb.user.user_id = fypdb.problem.added_by AND location_id = '" + locationValue + "' AND date between '" + fromtDate + " 00:00:00' and '" + toDate + " 23:59:00' AND type='r' order by date ASC";
                        columnsList.Add("venue");
                    }
                }

                break;
             case "q":
                {
                    columnsList.Add("Problem ID");
                    columnsList.Add("description");
                    columnsList.Add("status");
                    columnsList.Add("shift");
                    columnsList.Add("date");
                    columnsList.Add("Added by");

                    if (!string.IsNullOrEmpty(venue))
                    {
                        query = "SELECT problem_id AS 'Problem ID', description, status, shift, date, fypdb.user.user_name AS 'Added by' FROM " +
                        " problem inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by WHERE fypdb.user.user_id = fypdb.problem.added_by AND venue='" + venue + "' AND location_id = '" + locationValue + "' AND date between '" + fromtDate + " 00:00:00' and '" + toDate + " 23:59:00' AND type='q' order by date ASC";
                       
                    }
                    else
                    {
                        query = "SELECT problem_id AS 'Problem ID', description, status, shift, date, fypdb.user.user_name AS 'Added by',venue FROM " +
                        " problem inner join fypdb.user on fypdb.user.user_id=fypdb.problem.added_by WHERE fypdb.user.user_id = fypdb.problem.added_by AND location_id = '" + locationValue + "' AND date between '" + fromtDate + " 00:00:00' and '" + toDate + " 23:59:00' AND type='q' order by date ASC";
                         columnsList.Add("venue");
                    }
                }
                break;
            default: break;
        }


        fill_report(query, columnsList);
        showReport();
    }

    protected void problemTroubleshootReport_btn_Click(object sender, EventArgs e)
    {
        string problmeID = problemReport_problemID.Text;//pu.user_id

        string query = "SELECT p.location_id, u.user_name AS 'Added by', u1.user_name AS 'Troublshoot by', pu.date AS 'On', pu.content 'Step taken' FROM problem p JOIN prob_user pu ON p.problem_id = pu.prob_id join user u on u.user_id=p.added_by join user u1 on u1.user_id=pu.user_id WHERE u.user_id = p.added_by AND p.problem_id='" + problmeID + "' ORDER BY pu.date DESC";

        List<string> columnsList = new List<string>();
        columnsList.Add("location_id");
        columnsList.Add("Added by");
        columnsList.Add("Troublshoot by");
        columnsList.Add("On");
        columnsList.Add("Step taken");

        fill_report(query, columnsList);
        showReport();
    }

    protected void performersReport_Btn_Click(object sender, EventArgs e)
    {
        string from_date = performers_FromDate.Text;
        string toDate = performers_toDate.Text;

        string query = "SELECT Count(prob_user.user_id) AS 'Number of solved problems', user.user_name FROM " +
        " prob_user JOIN user ON prob_user.user_id = user.user_id WHERE prob_user.action = 's' AND date between '" + from_date + " 00:00:00' and '" + toDate + " 23:59:00' GROUP BY user.user_name ORDER by Count(prob_user.user_id) DESC;";

        List<string> columnsList = new List<string>();
        columnsList.Add("Number of solved problems");
        columnsList.Add("user_name");

        fill_report(query, columnsList);
        showReport();
    }

    protected void solvedProblemReport_Click(object sender, EventArgs e)
    {
        string from_date = solvedReport_fromDate.Text;
        string toDate = solvedReport_toDate.Text;
        string location = solvedProblemsDropList.Text;
        string type = solvedProblemsType.SelectedValue;

        string query = "SELECT problem.problem_id, u.user_name AS 'Added by', problem.venue, problem.pc, problem.description, problem.date AS 'Added On', u1.user_name AS 'Sovled by', prob_user.date AS 'Solved on', prob_user.content 'Step taken' FROM " +
        " problem inner join user u on u.user_id=problem.added_by  JOIN prob_user ON problem.problem_id = prob_user.prob_id join user u1 on u1.user_id=prob_user.user_id WHERE u1.user_id=prob_user.user_id AND u.user_id = problem.added_by AND prob_user.action = 's' AND problem.location_id=" + location + " AND problem.type='" + type + "' AND prob_user.date between '" + from_date + " 00:00:00' and '" + toDate + " 23:59:00' ORDER BY prob_user.date DESC;";

        List<string> columnsList = new List<string>();
        columnsList.Add("problem_id");
        columnsList.Add("Added by");
        columnsList.Add("venue");
        columnsList.Add("pc");
        columnsList.Add("description");
        columnsList.Add("Added On");
        columnsList.Add("Sovled by");
        columnsList.Add("Solved On");
        columnsList.Add("Step taken");

        fill_report(query, columnsList);
        showReport();
    }
    protected void pcReport_locationList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void pcReport_pc_TextChanged(object sender, EventArgs e)
    {

    }
}