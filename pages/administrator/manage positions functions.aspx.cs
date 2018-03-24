using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

public partial class pages_administrator_manage_positions_functions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void functionsManagementGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)  // Check if row is data row, not header, footer etc.
        {
            int functionGivenValue = Convert.ToInt32(e.Row.Cells[2].Text);
            Button assignBtn = (Button)e.Row.FindControl("assign_function");
            Button removeBtn = (Button)e.Row.FindControl("remove_function");

            if (functionGivenValue == 1)
            {
                assignBtn.Visible = false;
                e.Row.Cells[2].Text = "YES";
            }
            else
            {
                removeBtn.Visible = false;
                e.Row.Cells[2].Text = "NO";
            }
        }
    }

    protected void assign_function_Command(object sender, CommandEventArgs e)
    {
        string[] commandKeys = new string[2];
        commandKeys = e.CommandArgument.ToString().Split(';');

        try
        {
            administrator admin = new administrator();
            admin.updateFunctionAccess(commandKeys[0], commandKeys[1], 1);
            Response.Redirect(Request.RawUrl);
        }
        catch(Exception ex)
        {
            msg.Text = "There was a problem, error: " + ex.Message;
        }
    }


    protected void remove_function_Command(object sender, CommandEventArgs e)
    {
        string[] commandKeys = new string[2];
        commandKeys = e.CommandArgument.ToString().Split(';');

        try
        {
            administrator admin = new administrator();
            admin.updateFunctionAccess(commandKeys[0], commandKeys[1], 0);
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            msg.Text = "There was a problem, error: " + ex.Message;
        }
    }
    protected void functionsManagementGrid_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}