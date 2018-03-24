<%@ Page Title="Find TA" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Find TA.aspx.cs" Inherits="pages_HR_Find_TA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Find a TA</h2>
    <hr />
    <div class="form-group form-inline">
        <asp:Label AssociatedControlID="tpToFind" Text="Search by TP number" runat="server" CssClass="col-md-3"></asp:Label>
        <asp:TextBox ID="tpToFind" CssClass="form-control" runat="server"></asp:TextBox>
        <asp:Button ID="searchByNameBtn" CssClass="btn btn-info" Text="Search" runat="server" OnClick="searchByNameBtn_Click"/>
        <asp:RequiredFieldValidator ControlToValidate="tpToFind" ErrorMessage="You did not specify a valid TP number." CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
    </div>
    <div class="form-group form-inline">
        <asp:Label id="or" Text="OR" runat="server" CssClass="col-md-3"></asp:Label><br />
    </div>
    <div class="form-group form-inline">
        <asp:Label AssociatedControlID="namesDropList" Text="Select Name:" runat="server" CssClass="col-md-3"></asp:Label>
        <asp:DropDownList ID="namesDropList" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="namesDropList_SelectedIndexChanged"></asp:DropDownList>
    </div>
    <hr />
    <h3>TA Details:</h3>
    <div class="form-group form-inline">
        <asp:BulletedList ID="taDetailsList" BulletStyle="Square" runat="server">
        </asp:BulletedList>
        <asp:Label ID="msg" runat="server"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

