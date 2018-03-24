using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class Master_pages_main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["id"] != null)
        {
            userMenu_btn.Text = Session["username"].ToString();
            populateFunctionsList(Int32.Parse(Session["position"].ToString()));
        }
        else
        {
            Response.Redirect("~/pages/common/Sign out.aspx");
        }

        if ((Session["chosenRoundingLocation"] != null) & Page.Title != "Rounding")
        {
            Session["chosenRoundingLocation"] = null;
        }

        Response.Write("Number of Applications : " + Application["TotalApplications"]);

        Response.Write(" || ");


        Response.Write("Number of Users Online : " + Application["TotalUserSessions"]);
   
    }

    //popluate functions left menu list box
    protected void populateFunctionsList(int position)
    {
        db_connection db_obj = new db_connection();
        db_obj.open();


        string query = "SELECT  function_position.*, function.title, function.url FROM function INNER JOIN function_position ON function.function_id = function_position.function_id WHERE function_position.given = 1 AND function_position.position_id = @userPosition ORDER BY title ;";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@userPosition", position);
        MySqlDataReader reader = cmd.ExecuteReader();

        userFucntionsList.DataSource = reader;
        userFucntionsList.DataTextField = "title";
        userFucntionsList.DataValueField = "url".ToString();
        userFucntionsList.DataBind();
        reader.Close();
        db_obj.close();
    }
}
