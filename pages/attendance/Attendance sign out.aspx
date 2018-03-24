<%@ Page Title="Sign Out" Language="C#" MasterPageFile="~/Master pages/basic.master" AutoEventWireup="true" CodeFile="Attendance sign out.aspx.cs" Inherits="pages_common_Sign_out" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userMenuContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    <div class="row">
        <div class="col-md-12">
            <h3>You hve been logged out of the atnednace system</h3>
            <hr />
            <p>In order to login again please click <a href="Login Attendance.aspx">here</a>.</p>
            <p>Thank you.</p>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageScripts" Runat="Server">
</asp:Content> 