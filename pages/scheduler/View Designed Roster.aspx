<%@ Page Title="View Roster" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="View Designed Roster.aspx.cs" Inherits="pages_scheduler_View_Designed_Roster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>View Roster Template</h2>
        <div class="row">
            <div class="col-md-12 table-responsive">
                <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
                <asp:Button ID="exportRoster" Text="Export to excel" runat="server" OnClick="exportRoster_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
            </div>      
        </div>
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

