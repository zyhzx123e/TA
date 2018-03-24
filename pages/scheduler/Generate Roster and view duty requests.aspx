<%@ Page Title="View Duty Requests" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Generate Roster and view duty requests.aspx.cs" Inherits="pages_scheduler_Generate_Roster_and_view_duty_requests" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Generate Roster</h2>
    <hr />
    <div class="row">
        <div>
            <h2>The upcoming week date is: <asp:Label ID="upcomingWeek" runat="server"></asp:Label></h2>
        </div>
        <div class="col-md-12">
            <div class="table-responsive upDownMargin">
                <asp:PlaceHolder ID="dutyRequestsPlaceHolder" runat="server" />
            </div>
        </div>

        <div class="col-md-12">
            <h3>Select roster generation criteria</h3>
            <div class="form-group">
                <asp:RadioButtonList ID="generationCretira" CssClass="radio-inline" runat="server">
                    <asp:ListItem Value="shuffle">Shuffle</asp:ListItem>
                    <asp:ListItem Value="seniority">Seniority</asp:ListItem>
                    <asp:ListItem Value="fcfs">First come first serve</asp:ListItem>
                    <asp:ListItem Value="top_recipe">TOP3 Special Recipe for Helpdesk</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ControlToValidate="generationCretira" ErrorMessage="Gerenation criteria must be chosen" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:Button ID="generateRoster" CssClass="btn btn-default" Text="Generate Roster" runat="server" OnClick="generateRoster_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
                <asp:LinkButton ID="roster_link" Visible="false" CausesValidation="false" runat="server" ></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 