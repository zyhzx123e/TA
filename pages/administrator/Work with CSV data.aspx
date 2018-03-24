   <%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Work with CSV data.aspx.cs" Inherits="pages_administrator_Work_with_CSV_data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Work with .CSV files</h2>
    <hr />
    <div class="form-group">
        <asp:Button Text="Export users to CSV format" CssClass="btn btn-info" id="import_users" runat="server" OnClick="import_users_Click"/>
    </div>
    <div class="form-group">
        <asp:Button Text="Export locations to CSV format" CssClass="btn btn-info" id="export_locations" runat="server" OnClick="export_locations_Click"/>
    </div>
    <div class="form-group">
        <asp:Button Text="Export duty requests to CSV format" CssClass="btn btn-info" id="export_duty_requests" runat="server" OnClick="export_duty_requests_Click" />
    </div>
    <div class="form-group">
        <asp:Label ID="msg" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

