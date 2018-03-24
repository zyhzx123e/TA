using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections;
using System.Text;

public partial class pages_scheduler_Add_New_TA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            //load the countries to the nationality drop down menu.
            nationality.DataSource = CountryList();
            nationality.DataTextField = "Key";
            nationality.DataValueField = "Key";
            nationality.DataBind();
        }  
    }
    protected void add_btn_Click(object sender, EventArgs e)
    {
        scheduler schedulerObj = new scheduler();
        object[] TA_information = new object[17];

        string TA_name = name.Text;
        string tp = tp_number.Text;
        string ta_code = ta_number.Text;
        string pemail = privateEmail.Text;
        string TA_Email = taEmail.Text;
        string TA_contactNumber = contact_number.Text;
        string password = "password";
        string TA_dob = dob.Text;
        char TA_gender = char.Parse(gender.SelectedValue.ToString());
        string TA_nationality = nationality.SelectedValue.ToString();
        string TA_address = address.Text;
        string TA_intake = intake.Text;
        double TA_gpa = double.Parse(gpa.Text);
        string TA_achievements = achievements.Text;
        string sdate = selection_date.Text;
        int warning = int.Parse(warningLetter.Text);
        int positionID = 2;

        TA_information[0] = TA_name;
        TA_information[1] = tp;
        TA_information[2] = ta_code;
        TA_information[3] = pemail;
        TA_information[4] = TA_Email;
        TA_information[5] = TA_contactNumber;
        TA_information[6] = password;
        TA_information[7] = TA_dob;
        TA_information[8] = TA_gender;
        TA_information[9] = TA_nationality;
        TA_information[10] = TA_address;
        TA_information[11] = TA_intake;
        TA_information[12] = TA_gpa;
        TA_information[13] = TA_achievements;
        TA_information[14] = sdate;
        TA_information[15] = warning;
        TA_information[16] = positionID;

        try
        {
            schedulerObj.addNewTA(TA_information);
            msg.Text = "TA has been added successfully.";
            emptyForm();
        }
        catch (Exception ex)
        {
            msg.Text = "TA was not added, error: " + ex.Message;
        }
    }

    /* to get all countries name */
    public SortedList CountryList()
    {
        SortedList slCountry = new SortedList();
        string Key = "";
        string Value = "";

        foreach (CultureInfo info in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            RegionInfo info2 = new RegionInfo(info.LCID);
            if (!slCountry.Contains(info2.EnglishName))
            {
                Value = info2.TwoLetterISORegionName;
                Key = info2.EnglishName;
                slCountry.Add(Key, Value);
            }
        }
        return slCountry;
    }

    //to clear the form values
    public void emptyForm()
    {
        name.Text = null;
        tp_number.Text = null;
        ta_number.Text = null;
        privateEmail.Text = null;
        taEmail.Text = null;
        dob.Text = null;
        gender.SelectedIndex= 0;
        nationality.SelectedIndex = 0;
        address.Text = null;
        intake.Text = null;
        gpa.Text = null;
        achievements.Text = null;
        selection_date.Text = null;
        warningLetter.Text = null;
    }
}