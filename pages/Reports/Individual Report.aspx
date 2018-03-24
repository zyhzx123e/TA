<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Individual Report.aspx.cs" Inherits="pages_Reports_Individual_Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Technical Assistant Individual Report</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:DropDownList CssClass="form-control" AutoPostBack="false" ID="userDropList" runat="server">
                </asp:DropDownList>
            </div>
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="yearList" Text="Year" runat="server"></asp:Label>
                <asp:DropDownList CssClass="form-control"  AutoPostBack="true" ID="yearList" runat="server" OnSelectedIndexChanged="yearList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label AssociatedControlID="MonthsList"  Text="Month" runat="server"></asp:Label>
                <asp:DropDownList CssClass="form-control" Enabled="false" AutoPostBack="false" ID="MonthsList" runat="server">
                </asp:DropDownList>
                <asp:Button CssClass="btn btn-default" ID="exportToPdf" Text="Export to pdf" runat="server" OnClick="exportToPdf_Click" />
            </div>
            <div class="table table-responsive">
                <asp:GridView ID="reprotGrid" runat="server">

                </asp:GridView>
            </div>
            <div class="form-group">
                <asp:Button ID="generateReportBtn" CssClass="btn btn-info" Text="Generate Report" runat="server" OnClick="generateReportBtn_Click" />
            </div>
            <asp:Label ID="msg" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

