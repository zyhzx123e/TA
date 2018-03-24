using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
public partial class pages_administrator_manage_user_position : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateUserList();
            populateListBox();
        }
    }

    //populate the drop down menu with usernames
    protected void populateUserList()
    {
        db_connection db_obj = new db_connection();

        db_obj.open();
        string query = "SELECT user_id, user_name FROM user WHERE active=1";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);

        MySqlDataReader reader = cmd.ExecuteReader();

        userDropList.DataSource = reader;
        userDropList.DataTextField = "user_name";
        userDropList.DataValueField = "user_id";
        userDropList.DataBind();
        userDropList.Items.RemoveAt(0);
        userDropList.Items.Insert(0, new ListItem("SELECT", "N/A"));
        db_obj.close();
    }

    //load user details once the username is selected from the dropdown menu
    protected void userDropList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (userDropList.SelectedIndex != 0)
        {
            int user_id = int.Parse(userDropList.SelectedValue);
            Session["selectedUser"] = user_id;
            try
            {
                db_connection db_obj = new db_connection();
                db_obj.open();
                string query = "SELECT user_name, position_id, warning_count, password, resigning_date, achievements FROM user WHERE user_id=@id ;";
                MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
                cmd.Parameters.AddWithValue("@id", user_id);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                usernameText.Text = reader["user_name"].ToString();
                userPositionDropList.SelectedValue = reader["position_id"].ToString();
                warningLettterCount.Text = reader["warning_count"].ToString();
                userPassword.Text = reader["password"].ToString();
                achievements.Text = reader["achievements"].ToString();
                resignDate.Text = reader["resigning_date"].ToString();

                reader.Close();
                db_obj.close();

                delete.Enabled = true;
                save.Enabled = true;
            }
            catch (Exception ex)
            {
                msg.Text = "There was something wrong while retrieveing the user details. Error: " + ex.Message;
            }
        }
        
    }

    //populate positions list box with values from DB
    protected void populateListBox()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM position";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        userPositionDropList.DataSource = reader;
        userPositionDropList.DataTextField = "title";
        userPositionDropList.DataValueField = "position_id";
        userPositionDropList.DataBind();

        reader.Close();
        db_obj.close();
    }

    //delete user button action
    protected void delete_Click(object sender, EventArgs e)
    {
        administrator admin = new administrator();
        int user_id = int.Parse(userDropList.SelectedValue);
        try
        {
            if(user_id==1)
            {
                msg.Text = "Administrator account cannot be deleted!";
            }
            else
            {
                admin.deleteUser(user_id);
                Response.Redirect(Request.RawUrl);
            }
        } 
        catch(Exception ex)
        {
            msg.Text = "User could not be deleted, error: " + ex.Message;
        }
    }

    //update user details button action
    protected void save_Click(object sender, EventArgs e)
    {
        administrator admin = new administrator();
        int user_id = int.Parse(Session["selectedUser"].ToString());

        try
        {
            object[] updated_info = new object[6];
            updated_info[0] = userPositionDropList.SelectedValue;
            updated_info[1] = int.Parse(warningLettterCount.Text);
            updated_info[2] = userPassword.Text;
            updated_info[3] = achievements.Text;
            if (string.IsNullOrWhiteSpace(resignDate.Text))
            {
                updated_info[4] = null;
            }
            else
            {
                if(validateDateFromat(resignDate.Text) ==true)
                {
                    updated_info[4] = resignDate.Text;
                }
                else
                {
                    msg.Text = "Resgin date is not valid, please use the following format: yyyy-MM-dd";
                    return;
                }
            }

            admin.editUser(user_id, updated_info);
            Response.Redirect(Request.RawUrl);
            
        }
        catch(Exception ex)
        {
            msg.Text = "User details cannot be updated, error: "+ ex.Message;
        }
    }

    protected bool validateDateFromat(string date)
    {
        DateTime dt;
        if (DateTime.TryParse(date, out dt))
        {
            return true;
        }

        return false;
    }
}