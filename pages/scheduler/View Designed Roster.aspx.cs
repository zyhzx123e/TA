using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;

public partial class pages_scheduler_View_Designed_Roster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            generateRoster();
        }
    }

    protected void generateRoster()
    {
        StringBuilder html = new StringBuilder();

        foreach (string day in commonMethods.getRosterDays())
        {
            //table and first row (with day name)
            int numberOfLocationsForDay = commonMethods.getRosterLocations(day).Count;
            html.Append("<div class='table-responsive'>");
            html.Append("<table class='table'>");
            html.Append("<tr><th class='tableHeader' colspan='" + numberOfLocationsForDay + 1 + "'>");
            html.Append(day);
            html.Append("</th></tr>");

            //second row for location
            html.Append("<tr>");
            html.Append("<td>");
            foreach (int location in commonMethods.getRosterLocations(day))
            {
                int numberOfLocation = commonMethods.getRosterLocations(day).Count;
                int numberOfColumsForEachTable = 12 / numberOfLocation;
                //html.Append("<table class='table-bordered col-md-" + numberOfColumsForEachTable + "'>");
                html.Append("<table class='table-bordered forceInline'>");
                html.Append("<tr>");
                html.Append("<th class='tableHeader' colspan='10'>");
                html.Append(commonMethods.getLocationName(location));
                //html.Append(location.ToString());
                html.Append("</th>");
                html.Append("</tr>");

                //third row for shifts header
                html.Append("<tr>");
                html.Append("<th class='tableHeader'>");
                html.Append("Shift");
                html.Append("</th>");

                //third row create a header for each duty in that location at that day
                foreach (KeyValuePair<int, int> duty in commonMethods.getRosterDuties(day, location))
                {
                    for (int i = 1; i <= duty.Value;i++ )
                    {
                        html.Append("<th class='tableHeader'>");
                        html.Append(commonMethods.getDutyName(duty.Key));
                        html.Append("</th>");
                    }
                }

                html.Append("</td>");
                html.Append("</tr>");

                //to create a row for each shift at a location on a specific day
                foreach (int shift in commonMethods.getRosterShifts(day, location))
                {
                    html.Append("<tr>");
                    html.Append("<td>");
                    html.Append(commonMethods.getShiftTiming(shift, day, location));
                    html.Append("</td>");

                    //create cells under each duty header
                    foreach (KeyValuePair<int, int> duty in commonMethods.getRosterDuties(day, location))
                    {
                        for (int i = 1; i <= duty.Value; i++)
                        {
                            html.Append("<td ");
                            if (commonMethods.getRosterDutiesCells(day, location, shift).ContainsKey(duty.Key))
                            {
                                if (commonMethods.getRosterDutiesCells(day, location, shift)[duty.Key] >= i)
                                {
                                    html.Append("class='greenCell'>");
                                }
                                else
                                {
                                    html.Append("class='blackCell'>");
                                }      
                            }
                            else
                            {
                                html.Append("class='blackCell'>");
                            }
                            html.Append("</td>");
                        }
                    }

                    html.Append("</tr>");
                }

                html.Append("</table>");
            }
            html.Append("</td>");
            html.Append("</tr>");


            html.Append("</table><hr />");
            html.Append("</div>");
        }

        PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
    }

    protected void exportRoster_Click(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=Print.xls");
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Report Data</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");
            Response.Write("hellooooooooo");
            Response.Write("</head>");
        }
        catch (Exception ex)
        {

        }
    }
}