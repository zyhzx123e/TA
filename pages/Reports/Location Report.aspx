<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Location Report.aspx.cs" Inherits="pages_Reports_Location_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Locations Report</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:DropDownList CssClass="form-control" AutoPostBack="false" ID="locationsList" runat="server" OnSelectedIndexChanged="locationsList_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="yearList" Text="Year" runat="server"></asp:Label>
                <asp:DropDownList CssClass="form-control"  AutoPostBack="true" ID="yearList" runat="server" OnSelectedIndexChanged="yearList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label AssociatedControlID="MonthsList"  Text="Month" runat="server"></asp:Label>
                <asp:DropDownList CssClass="form-control" Enabled="false" AutoPostBack="false" ID="MonthsList" runat="server">
                </asp:DropDownList>
                <asp:Button runat="server" ID="exportBtn" CssClass="btn btn-default" Text="Export to pdf" OnClick="exportBtn_Click" />
            </div>
        </div>
        <div class="table table-responsive">
            <asp:GridView ID="reportGrid" runat="server">

            </asp:GridView>
        </div>
        <div class="form-group">
            <asp:Button ID="generateReportBtn" Text="Generate Report" CssClass="btn btn-info" runat="server" OnClick="generateReportBtn_Click" />
            <asp:label ID="msg" runat="server"></asp:label>
        </div>
      </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

