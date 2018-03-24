<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="View profile.aspx.cs" Inherits="pages_TA_View_profile" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
          <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
          <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
   <script type="text/javascript" src="js/jquery-1.12.3.js"></script>
    <h2>My Profile</h2>
    <hr />
    <div class="row">
        <div class="col-lg-12">


             <!--
                Total hour that for current TA for specific period of time
                 -->

             <div class="form-group">
                <em><b>Your total hours have worked</b></em> from     
               <span>  <asp:TextBox ID="working_hr_fromDate" placeholder="From" CssClass="datepicker form-control" runat="server" Width="120px"></asp:TextBox>
                       to<asp:TextBox ID="working_hr_toDate" placeholder="To" CssClass="datepicker form-control" runat="server" Width="120px"></asp:TextBox> &nbsp; :  <asp:label ID="working_hour" runat="server"></asp:label>
       <asp:Button ID="Button1" CssClass="btn btn-info btn-block" Text="Display hour" runat="server"  Width="120px" OnClick="working_hr_Click" />
          </span>       
            </div> 


            <!--
               ******************** 
                 -->

            <!--
                Total problem  that have solved  for current TA within specific period of time
                 -->

             <div class="form-group">
                <em><b>Your total problems that have been solved</b></em> from     
               <span>  <asp:TextBox ID="problem_countfromDate" placeholder="From" CssClass="datepicker form-control" runat="server" Width="120px"></asp:TextBox>
                       to<asp:TextBox ID="problem_counttoDate" placeholder="To" CssClass="datepicker form-control" runat="server" Width="120px"></asp:TextBox> &nbsp; :  <asp:label ID="problem_count" runat="server"></asp:label>
       <asp:Button ID="problem_count_btn" CssClass="btn btn-block" Text="Display problem count" runat="server"  Width="170px" OnClick="problem_count_btn_Click" />
          </span>       
            </div> 


            <!--
                
                 -->






            <div class="form-group">
                Full Name:
                <asp:label ID="name" runat="server">test</asp:label>
            </div>

            <div class="form-group">
                Position:
                <asp:label ID="position_id" runat="server"></asp:label>
            </div>


              <div class="form-group">
                <b>You have solved totally </b>    <asp:label ID="problems" runat="server"></asp:label> problems till now        
            </div> 

            <div class="form-group">
                <b>You have worked totally </b>   <asp:label ID="hr" runat="server"></asp:label> hour till now      
            </div> 

            <div class="form-group form-inline">
                TP Number:
                <asp:label ID="tp_number" runat="server">TP0312312</asp:label>
            </div>
            <div class="form-group form-inline">
                TA Number:
                <asp:label ID="ta_number"  runat="server">TA321312</asp:label>
            </div>
            <div class="form-group form-inline">
                <asp:Label runat="server"  AssociatedControlID="privateEmail">Private email</asp:Label>
                <asp:TextBox ID="privateEmail" TextMode="Email" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group form-inline">
                <asp:Label runat="server"  AssociatedControlID="contactNumber">Contact Number</asp:Label>
                <asp:TextBox ID="contactNumber" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                TA Email:
                <asp:label ID="taEmail" runat="server"></asp:label>
            </div>
            <div class="form-group">
                Date of birth:
                <asp:label ID="dob" runat="server"></asp:label>
            </div>
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="intake" runat="server">Intake Code</asp:Label>
                <asp:TextBox ID="intake" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="gpa"  runat="server">GPA</asp:Label>
                <asp:TextBox ID="gpa" TextMode="Number" step="any" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                Date of selected as a TA :
                <asp:label ID="selection_date_lb" runat="server"></asp:label>
            </div> 

            <div class="form-group">
                Number of warning letter
                <asp:label ID="warningLetters" runat="server"></asp:label>
            </div> 
            <div class="form-group">
                Achievements
                <asp:label ID="achievements" runat="server"></asp:label>
            </div> 
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="address" runat="server">Address</asp:Label>
                <asp:TextBox TextMode="MultiLine" Rows="4" ID="address" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
     
            <br /><hr />
            <div class="form-group">
                <asp:Button Text="Save" ID="save_btn" CssClass="btn btn-default" runat="server" OnClick="save_btn_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
    <script>
        /*
        $(document).ready(function () {
            $('#rounding_dashboard').fadeIn(6000);
            $("#pcReportSearchDiv").hide();
            $("#locationVenueReport").hide();
            $("#problemTroubleshootReport").hide();
            $("#topPerfomersReport").hide();
            $("#solved_problemsReport").hide();

        });
        */

        function hideDiv(name) {
            $("#" + name).toggle(function () { });

            //$("#" + name + "_span").attr('class', 'glyphicon glyphicon-chevron-down');

        }




        $(function () {
            $(".datepicker").datepicker({ dateFormat: 'yy-mm-dd' });
        });



    </script>
</asp:Content>

 