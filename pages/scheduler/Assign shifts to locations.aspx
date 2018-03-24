<%@ Page Title="Assign Shifts to Locations" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Assign shifts to locations.aspx.cs" Inherits="pages_scheduler_Assign_shifts_to_locations" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Assign Shifts to Locations</h2>
    <hr />
    <div class="col-md-12">
        <h3>Add new shift to the roster:</h3>
            <hr />
        <div class="form-group form-inline">
            <asp:Label AssociatedControlID="locationDropList" Text="Location: " runat="server"></asp:Label>
            <asp:DropDownList ID="locationDropList" CssClass="form-control" runat="server">
            </asp:DropDownList>
            <asp:Label AssociatedControlID="shiftNameDropList" Text="Shift: " runat="server"></asp:Label>
            <asp:DropDownList ID="shiftNameDropList" CssClass="form-control" runat="server">
            </asp:DropDownList> 
        </div>
        <div class="form-group form-inline">
            <asp:DropDownList ID="dayDropList" CssClass="form-control" runat="server">
                <asp:ListItem Value="N/A">SELECT DAY</asp:ListItem>
                <asp:ListItem Value="Monday">Monday</asp:ListItem>
                <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                <asp:ListItem Value="Friday">Friday</asp:ListItem>
                <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group form-inline">
            <asp:TextBox ID="startTime" CssClass="form-control" placeholder="Start time.." runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ControlToValidate="startTime" ErrorMessage="*" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ControlToValidate="startTime" CssClass="warningLbl" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$" ErrorMessage="use hh:mm:ss format" runat="server" ></asp:RegularExpressionValidator>
            <asp:TextBox ID="endTime" CssClass="form-control" placeholder="End time.." runat="server"></asp:TextBox>   
            <asp:RequiredFieldValidator ControlToValidate="endTime" ErrorMessage="*" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ControlToValidate="endTime" CssClass="warningLbl" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$" ErrorMessage="use hh:mm:ss format" runat="server" ></asp:RegularExpressionValidator>
        </div>
        <div class="form-group">
            <asp:Button Text="Assign shift to location" ID="assignShitToLocation_btn" CssClass="btn btn-success" runat="server" OnClick="assignShitToLocation_btn_Click" />
            <asp:Label ID="msg" runat="server"></asp:Label><br /> 
        </div>
        <hr />
            <h3>Modify a shift in the roster:</h3>
        <hr />
        <div class="form-group form-inline">
            <asp:Label AssociatedControlID="currentLocationsList" runat="server">Location</asp:Label>
            <asp:DropDownList ID="currentLocationsList" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="currentLocationsList_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group form-inline">
            <asp:Label AssociatedControlID="currentDaysList" runat="server">Day</asp:Label>
            <asp:DropDownList ID="currentDaysList" AutoPostBack="true" Enabled="false" CssClass="form-control" runat="server" OnSelectedIndexChanged="currentDaysList_SelectedIndexChanged">
                <asp:ListItem Value="N/A">SELECT DAY</asp:ListItem>
                <asp:ListItem Value="Monday">Monday</asp:ListItem>
                <asp:ListItem Value="Tuesday">Tuesday</asp:ListItem>
                <asp:ListItem Value="Wednesday">Wednesday</asp:ListItem>
                <asp:ListItem Value="Thursday">Thursday</asp:ListItem>
                <asp:ListItem Value="Friday">Friday</asp:ListItem>
                <asp:ListItem Value="Saturday">Saturday</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group form-inline">
            <asp:Label AssociatedControlID="currentShiftsDaysList" runat="server">Shift</asp:Label>
            <asp:DropDownList ID="currentShiftsDaysList" AutoPostBack="true" Enabled="false" CssClass="form-control" runat="server" OnSelectedIndexChanged="currentShiftsDaysList_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group form-inline">
            <asp:Label AssociatedControlID="currentStartTime" runat="server">Start Time</asp:Label>
            <asp:TextBox ID="currentStartTime" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group form-inline">
            <asp:Label AssociatedControlID="currentEndTime" runat="server">End Time</asp:Label>
            <asp:TextBox ID="currentEndTime" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group form-inline">
            <asp:Label CssClass="alert-danger warningLbl" Text="NOTE: REMOVING A SHIFT FROM A LOCATION WILL DELETE ALL ITS ASSOCIATED DUTIES AT THAT LOCATION" runat="server"></asp:Label>
        </div>
        <div class="form-group form-inline">
            <asp:Button CssClass="btn btn-info" CausesValidation="false" ID="updateCurrentShiftBtn" Text="Update Shift Details" Enabled="false" runat="server" OnClick="updateCurrentShiftBtn_Click" />
            <asp:Button CssClass="btn btn-danger" CausesValidation="false" ID="decativateCurrentShiftBtn" Text="Remove Shift" Enabled="false" runat="server" OnClick="decativateCurrentShiftBtn_Click" />
            <asp:Label ID="msg1" CssClass="warningLbl" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 