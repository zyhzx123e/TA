using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_TA_Change_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void change_password_btn_Click(object sender, EventArgs e)
    {
        user userObject = new user();
        int user_id = Int32.Parse(Session["id"].ToString());
        if (old_pass.Text != verifyPassword(user_id))
        {
            msg.Text = "Your password is incorrect, please provide the correct password in order to be able to change your password.";
        }else
        {
            try
            {
                if(new_pass.Text == verify_new_pass.Text)
                {
                    userObject.changePassword(user_id, new_pass.Text);
                    emptyForm();
                    msg.Text = "Your password has been changed.";
                }else
                {
                    msg.Text = "Your new passwords do not match!, please retype again.";
                }
            }
            catch(Exception ex)
            {
                msg.Text = "There was something wrong, error: " + ex.Message;
            }
        }
    }

    protected string verifyPassword(int id)
    {
        string old_password = "";
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT password FROM user WHERE user_id=@userID";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        cmd.Parameters.AddWithValue("@userID", id);

        MySqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            old_password = reader["password"].ToString();
        }

        db_obj.close();
        return old_password;
    }

    protected void emptyForm()
    {
        old_pass.Text = "";
        new_pass.Text = "";
        verify_new_pass.Text = "";
    }
}