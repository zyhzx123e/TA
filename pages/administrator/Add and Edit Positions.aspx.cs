using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class pages_administrator_Add_and_Edit_Positions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            populateListBox();
        }  
    }
    protected void save_btn_Click(object sender, EventArgs e)
    {
        administrator admin = new administrator();
        string position_title = title.Text;
        int position_rate = int.Parse(pay_rate.Text);
        int position_quota = int.Parse(quota.Text);

        //to add values if no editing is taking place.
        if(cancel_btn.Visible==false)
        {
            try
            {
                admin.addNewPosition(position_title, position_rate, position_quota);
                emptyForm();
                msg.Text = "Position has been added.";
                emptyForm();
            }
            catch (Exception ex)
            {
                msg.Text = "Position could not be added, error: " + ex.Message;
            }
        }
        else //to save the edited values.
        {
            try
            {
                int positionID = int.Parse(position_list.SelectedValue);
                admin.editPosition(positionID, position_title, position_rate, position_quota);
                msg.Text = "Position has been edited.";
                clearPageAfterEdit();
            }
            catch(Exception ex)
            {
                msg.Text = "Position could not be modified, error: " + ex.Message;
            }
            
        }
    }

    //retrieve values of the selected item to edit and populate the form
    protected void edit_btn_Click(object sender, EventArgs e)
    {
        int selected_position_id = Int32.Parse(position_list.SelectedValue);
        try
        {
            db_connection db_obj = new db_connection();
            db_obj.open();
            string query = "SELECT * FROM position WHERE position_id =@id ;";
            MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
            cmd.Parameters.AddWithValue("@id", selected_position_id);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            title.Text = reader["title"].ToString();
            pay_rate.Text = reader["pay_rate"].ToString();
            quota.Text = reader["monthly_quota_hours"].ToString();

            reader.Close();
            db_obj.close();
            save_btn.Text = "Save Changes";
            cancel_btn.Visible = true;
        }
        catch (Exception ex)
        {
            msg.Text = "Something went wrong, error: " + ex.Message;
        }
    }

    protected void cancel_btn_Click(object sender, EventArgs e)
    {
        clearPageAfterEdit();
        msg.Text = "";
    }

    //populate positions list box with values from DB
    protected void populateListBox()
    {
        db_connection db_obj = new db_connection();
        db_obj.open();

        string query = "SELECT * FROM position";
        MySqlCommand cmd = new MySqlCommand(query, db_obj.connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        position_list.DataSource = reader;
        position_list.DataTextField = "title";
        position_list.DataValueField = "position_id";
        position_list.DataBind();
        position_list.Items.RemoveAt(0);
        reader.Close();
        db_obj.close();
    }

    //clear values from the form
    protected void emptyForm()
    {
        title.Text = "";
        pay_rate.Text = "";
        quota.Text = "";
    }

    //enable edit & cancel button once user selects an item from the list
    protected void position_list_SelectedIndexChanged(object sender, EventArgs e)
    {
           edit_btn.Enabled = true;
    }
 
    protected void clearPageAfterEdit()
    {
        emptyForm();
        position_list.ClearSelection();
        save_btn.Text = "ADD";
        edit_btn.Enabled = false;
        cancel_btn.Visible = false;
    }
    
   
    
}