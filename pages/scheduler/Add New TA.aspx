<%@ Page Title="Add a New TA" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Add New TA.aspx.cs" Inherits="pages_scheduler_Add_New_TA" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
    <!-- CSS for datepicker JQuery item -->
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Add New TA</h2>
    <hr />
    <div class="row">
        <div class="col-lg-12">
            <div class="form-group form-inline">
                <asp:TextBox ID="name" CssClass="form-control" placeholder="Full name" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="name" CssClass="warningLbl" ErrorMessage="*Name is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox ID="tp_number" MaxLength="8" CssClass="form-control" placeholder="TP number" runat="server"></asp:TextBox>  
                <asp:TextBox ID="ta_number" MaxLength="8" CssClass="form-control" placeholder="TA number" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="ta_number" CssClass="warningLbl" ErrorMessage="*Required fields" runat="server"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ControlToValidate="ta_number" CssClass="warningLbl" ErrorMessage="*Required fields" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox ID="privateEmail" CssClass="form-control" TextMode="Email" placeholder="Private email" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="privateEmail" CssClass="warningLbl" ErrorMessage="*Private email is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox ID="taEmail" CssClass="form-control" TextMode="Email" placeholder="TA email" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="taEmail" CssClass="warningLbl" ErrorMessage="*TA email is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox ID="contact_number" CssClass="form-control" TextMode="Number" placeholder="Contact number" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="contact_number" CssClass="warningLbl" ErrorMessage="*Contact Number is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox ID="dob" CssClass="datepicker form-control" placeholder="Date of birth" runat="server"></asp:TextBox>
                <asp:DropDownList CssClass="form-control" ID="nationality" runat="server">
                            
                </asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="dob" CssClass="warningLbl" ErrorMessage="*Required fields" runat="server"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ControlToValidate="nationality" CssClass="warningLbl" ErrorMessage="*Required fields" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:RadioButtonList ID="gender" runat="server" CssClass="radio radio-inline">
                    <asp:ListItem Value='m'>Male</asp:ListItem>
                    <asp:ListItem Value='f'>Female</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ControlToValidate="gender" CssClass="warningLbl" ErrorMessage="*Gender is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox ID="intake" CssClass="form-control" placeholder="Intake Code" runat="server"></asp:TextBox>
                <asp:TextBox ID="gpa" step="any" CssClass="form-control" TextMode="Number" placeholder="TA GPA" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="intake" CssClass="warningLbl" ErrorMessage="*Required fields" runat="server"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ControlToValidate="gpa" CssClass="warningLbl" ErrorMessage="*Required fields" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group form-inline">
                <asp:TextBox CssClass="datepicker form-control" ID="selection_date" placeholder="Selection date" runat="server"></asp:TextBox>
                <asp:TextBox ID="warningLetter" CssClass="form-control" TextMode="Number" placeholder="warning letter number" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="selection_date" CssClass="warningLbl" ErrorMessage="*Selection date is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:TextBox TextMode="MultiLine" Rows="4" ID="address" CssClass="form-control" placeholder="Address" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="address" CssClass="warningLbl" ErrorMessage="*Address is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:TextBox TextMode="MultiLine" Rows="4" ID="achievements" CssClass="form-control" placeholder="Achievements" runat="server"></asp:TextBox>
            </div>
            <div class="form-group form-inline">
                <asp:Button Text="Add" ID="add_btn" CssClass="btn btn-default" runat="server" OnClick="add_btn_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
    <!-- calling datepicker -->
    <script>
        $(function () {
            $(".datepicker").datepicker({ dateFormat : 'yy-mm-dd'});
        });
    </script>
</asp:Content> 