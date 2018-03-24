<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="manage user.aspx.cs" Inherits="pages_administrator_manage_user_position" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
     <!-- CSS for datepicker JQuery item -->
     <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
        <h2>Manage Users</h2>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <asp:DropDownList CssClass="form-control" AutoPostBack="true" ID="userDropList" runat="server" OnSelectedIndexChanged="userDropList_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
                
            <div class="col-md-12">
                <h3>User details:</h3>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="usernameText" CssClass="col-md-3" Text="Full Name" runat="server"></asp:Label>
                    <asp:TextBox ID="usernameText" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                </div>
                    
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="userPositionDropList" CssClass="col-md-3" Text="Position" runat="server"></asp:Label>
                    <asp:DropDownList ID="userPositionDropList" CssClass="form-control" runat="server">
                        <asp:ListItem>position</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="warningLettterCount" CssClass="col-md-3" Text="Warning letter number" runat="server"></asp:Label>
                    <asp:TextBox ID="warningLettterCount" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="warningLettterCount" CssClass="warningLbl" runat="server" ErrorMessage="*Warning letter count is required."></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="userPassword" CssClass="col-md-3" Text="Passowrd" runat="server"></asp:Label>
                    <asp:TextBox ID="userPassword" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="userPassword" ErrorMessage="Password cannot be empty!" CssClass="warningLbl" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="achievements" CssClass="col-md-3" Text="Achievements" runat="server"></asp:Label>
                    <asp:TextBox ID="achievements" CssClass="form-control"  TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="resignDate" CssClass="col-md-3" Text="Resignation date" runat="server"></asp:Label>
                    <asp:TextBox CssClass="form-control datepicker" ID="resignDate" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <div class="container">
                        <asp:Button Text="Deactivate User" ID="delete" CssClass="btn btn-danger" runat="server" Enabled="false" OnClick="delete_Click" />
                        <asp:Button Text="Save Changes" ID="save" CssClass="btn btn-default" Enabled="false" runat="server" OnClick="save_Click" />
                        <asp:Label ID="msg" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
    <!-- calling datepicker -->
    <script>
        $(function () {
            $(".datepicker").datepicker({ dateFormat: 'yy-mm-dd' });
        });
    </script>
</asp:Content> 