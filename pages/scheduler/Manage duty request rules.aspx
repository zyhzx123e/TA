<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Manage duty request rules.aspx.cs" Inherits="pages_scheduler_Manage_duty_request_rules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Duty Request's Rules</h2>
    <hr />
    <div class="row">
        <div class="col-lg-12">
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="sday" runat="server">Start Day</asp:Label>
                <asp:DropDownList ID="sday" CssClass="form-control" runat="server">
                    <asp:ListItem Value="Monday">Monday</asp:ListItem>
                    <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                    <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                    <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                    <asp:ListItem Value="Friday">Friday</asp:ListItem>
                    <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="sday" ErrorMessage="*Required" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="eday" runat="server">End Day</asp:Label>
                <asp:DropDownList ID="eday" CssClass="form-control" runat="server">
                    <asp:ListItem Value="Monday">Monday</asp:ListItem>
                    <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                    <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                    <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                    <asp:ListItem Value="Friday">Friday</asp:ListItem>
                    <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ControlToValidate="eday" ErrorMessage="*Required" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="stime" runat="server">Start Time</asp:Label>
                <asp:TextBox ID="stime" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="sTime" ErrorMessage="*Required" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ControlToValidate="sTime" CssClass="warningLbl" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$" ErrorMessage="Wrong time format, please use hh:mm:ss format" runat="server" ></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="etime" runat="server">End Time</asp:Label>
                <asp:TextBox ID="etime" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="etime" ErrorMessage="*Required" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ControlToValidate="etime" CssClass="warningLbl" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$" ErrorMessage="Wrong time format, please use hh:mm:ss format" runat="server" ></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="signinWindow" runat="server">Sign In Window</asp:Label>
                <asp:TextBox ID="signinWindow" TextMode="Number" MaxLength="2" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="signinWindow" ErrorMessage="*" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
                <asp:Label AssociatedControlID="signOutWindow" runat="server">Sign Out Window</asp:Label>
                <asp:TextBox ID="signOutWindow" TextMode="Number" MaxLength="2" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="signOutWindow" ErrorMessage="*" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
                *In Minutes
            </div>
        </div>
        <div class="col-lg-12">
            
        </div>
        <div class="col-lg-12">
            <div class="form-group">
                <asp:button ID="updateDetails" Text="Update Details" CssClass="btn btn-info" runat="server" OnClick="updateDetails_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

