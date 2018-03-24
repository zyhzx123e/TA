using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Web;
using System.Text;

public partial class pages_Reports_Location_Report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateLocation();
            populateYearsList();
        }
        
    }

    protected void populateLocation()
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
        locationsList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECT LOCATION", "0"));
        db_obj.close();
    }

    //populate the years drop down
    protected void populateYearsList()
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT DISTINCT YEAR(week_start_date) AS 'Year' FROM attendance;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

        MySqlDataReader reader = cmd.ExecuteReader();

        yearList.DataSource = reader;
        yearList.DataTextField = "Year";
        yearList.DataValueField = "Year";
        yearList.DataBind();
        yearList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("SELECT", "N/A"));
        db_obj.close();
    }

    //populate the months drop down
    protected void populateMonthsList(string year)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "SELECT DISTINCT MONTHNAME(week_start_date) AS 'Month' FROM attendance WHERE DATE_FORMAT(attendance.week_start_date, '%Y') = @year";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@year", year);
        MySqlDataReader reader = cmd.ExecuteReader();

        MonthsList.DataSource = reader;
        MonthsList.DataTextField = "Month";
        MonthsList.DataValueField = "Month";
        MonthsList.DataBind();
        db_obj.close();
    }


    protected void yearList_SelectedIndexChanged(object sender, EventArgs e)
    {
        populateMonthsList(yearList.SelectedValue);
        MonthsList.Enabled = true;
    }

    protected void generateReportBtn_Click(object sender, EventArgs e)
    {
        try
        {
            int locationID = int.Parse(locationsList.SelectedValue);
            string month = MonthsList.SelectedValue;
            string year = yearList.SelectedValue;

            if (locationsList.SelectedIndex != 0 && yearList.SelectedIndex != 0)
            {
                populateReportTable(locationID, month, year);
            }
            else
            {
                msg.Text = "Please specify all the required details.";
            }
        }
        catch (Exception ex)
        {
            msg.Text = "There was something wrong." + ex.Message;
        }
    }

    //populate table from DB
    protected void populateReportTable(int locationID, string month, string year)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "SELECT user.user_name AS 'Technical Assistant', concat(login_date, ', ', attendance.login_time) AS 'Check In', " +
            "concat(attendance.logout_date, ', ', attendance.logout_time) AS 'Check Out', " +
            " FORMAT(TIME_TO_SEC(TIMEDIFF(attendance.logout_time, attendance.login_time)) / 3600, 2) AS 'Workig Hours'," +
            "position.pay_rate AS 'Rate', FORMAT(position.pay_rate * TIME_TO_SEC(TIMEDIFF(attendance.logout_time, attendance.login_time)) / 3600, 2) AS 'Total'," +
            " location.name AS 'Location', attendance.system_remark FROM attendance JOIN user ON attendance.user_id = user.user_id JOIN location ON attendance.location_id = " +
            " location.location_id JOIN position ON position.position_id = user.position_id WHERE attendance.location_id=@location  AND DATE_FORMAT(attendance.week_start_date, '%M-%Y') = " +
            " '" + month + "-" + year + "' UNION ALL SELECT 'TOTAL AMOUNT', null, null, null , null, CAST(SUM(FORMAT(position.pay_rate * TIME_TO_SEC(TIMEDIFF(attendance.logout_time, " +
            " attendance.login_time))/3600, 2)) AS DECIMAL(5,2)), null, null FROM attendance JOIN user ON attendance.user_id = user.user_id JOIN position ON " +
            " user.position_id = position.position_id WHERE attendance.location_id=@location AND  DATE_FORMAT(attendance.week_start_date, '%M-%Y') = '" + month + "-" + year + "';";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        reportGrid.DataSource = dt;
        reportGrid.CssClass = "table";
        reportGrid.DataBind();
    }

    protected void exportBtn_Click(object sender, EventArgs e)
    {
        try
        {
            int locationID = int.Parse(locationsList.SelectedValue);
            string month = MonthsList.SelectedValue;
            string year = yearList.SelectedValue;

            if (locationsList.SelectedIndex != 0 && yearList.SelectedIndex != 0)
            {
                System.Web.UI.WebControls.GridView reportGrid = new System.Web.UI.WebControls.GridView();
                reportGrid.DataSource = pdfReportData(locationID, month, year);
                reportGrid.DataBind();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=DataTable.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                reportGrid.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();
                Response.Write(pdfDoc);
                Response.End();
            }
            else
            {
                msg.Text = "Please specify all the required report's details.";
            }
        }
        catch (Exception ex)
        {
            msg.Text = "There was something wrong." + ex.Message;
        }
    }

   
    protected DataTable pdfReportData(int locationID, string month, string year)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();
        string query = "SELECT user.user_name AS 'Technical Assistant', concat(login_date, ', ', attendance.login_time) AS 'Check In', " +
            "concat(attendance.logout_date, ', ', attendance.logout_time) AS 'Check Out', " +
            " FORMAT(TIME_TO_SEC(TIMEDIFF(attendance.logout_time, attendance.login_time)) / 3600, 2) AS 'Workig Hours'," +
            "position.pay_rate AS 'Rate', FORMAT(position.pay_rate * TIME_TO_SEC(TIMEDIFF(attendance.logout_time, attendance.login_time)) / 3600, 2) AS 'Total'," +
            " location.name AS 'Location', attendance.system_remark FROM attendance JOIN user ON attendance.user_id = user.user_id JOIN location ON attendance.location_id = " +
            " location.location_id JOIN position ON position.position_id = user.position_id WHERE attendance.location_id=@location  AND DATE_FORMAT(attendance.week_start_date, '%M-%Y') = " +
            " '" + month + "-" + year + "' UNION ALL SELECT 'TOTAL AMOUNT', null, null, null , null, CAST(SUM(FORMAT(position.pay_rate * TIME_TO_SEC(TIMEDIFF(attendance.logout_time, " +
            " attendance.login_time))/3600, 2)) AS DECIMAL(5,2)), null, null FROM attendance JOIN user ON attendance.user_id = user.user_id JOIN position ON " +
            " user.position_id = position.position_id WHERE attendance.location_id=@location AND  DATE_FORMAT(attendance.week_start_date, '%M-%Y') = '" + month + "-" + year + "';";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@location", locationID);
        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }
    protected void locationsList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}