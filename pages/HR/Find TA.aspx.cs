using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_HR_Find_TA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateTA_namesList();
        }
    }



    protected void populateTA_namesList()
    {
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();

            string query = "SELECT user_name AS user_name, user_id FROM user;";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            namesDropList.DataSource = reader;
            namesDropList.DataTextField = "user_name";
            namesDropList.DataValueField = "user_id";
            namesDropList.DataBind();
            namesDropList.Items.Insert(0, new ListItem("Choose a name....", "N/A"));
            reader.Close();
            db_obj.close();
        }
        catch (Exception ex)
        {
            msg.Text = "Name list could not be retrieved, error : " + ex.Message;
        }
    }

    protected void populateTAdetails(string[] taDetails)
    {
        foreach (string str in taDetails)
        {
            ListItem newItem = new ListItem(str);
            taDetailsList.Items.Add(newItem);
            msg.Text = " ";
        }
    }
    protected void namesDropList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(namesDropList.SelectedIndex != 0)
        {
            tpToFind.Text = "";
            taDetailsList.Items.Clear();
            human_resource newHR = new human_resource();
            populateTAdetails(newHR.viewTAdetails(int.Parse(namesDropList.SelectedValue)));
        }
        
    }

    protected void searchByNameBtn_Click(object sender, EventArgs e)
    {
        int userID;
        taDetailsList.Items.Clear();
        namesDropList.SelectedIndex = 0;
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT user_id FROM user WHERE user_tp = @tp";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@tp", tpToFind.Text);

            userID = int.Parse(cmd.ExecuteScalar().ToString());

            db_obj.close();

            human_resource newHR = new human_resource();
            populateTAdetails(newHR.viewTAdetails(userID)); 
        }
        catch(Exception ex)
        {
            msg.Text = "User Could not be found!";
        }
    }
}