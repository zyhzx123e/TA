<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="reports.aspx.cs" Inherits="pages_Reports_reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Reports</h2>
    <hr />
    <div class="row">
    <div class="col-md-12">
        <asp:Label runat="server" Text="Individual Reports"></asp:Label>
        <asp:HyperLink runat="server" NavigateUrl="~/pages/Reports/Individual Report.aspx" Text="click here"></asp:HyperLink>
        <hr />
        <asp:Label runat="server" Text="Locations Reports"></asp:Label>
        <asp:HyperLink ID="locationReportLink" runat="server" NavigateUrl="~/pages/Reports/Location Report.aspx" Text="click here"></asp:HyperLink>
        <asp:Label ID="msg" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

