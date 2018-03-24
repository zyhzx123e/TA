<%@ Page Title="Attendance Login" Language="C#" MasterPageFile="~/Master pages/basic.master" AutoEventWireup="true" CodeFile="Login Attendance.aspx.cs" Inherits="pages_attendance_Login_Attendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userMenuContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    <div class="container">
        <div class="row row-centered">
            <div class="col-md-6 col-centered" id="loginForm">
               <div class="row">
                   <div class="col-md-12">
                       <h2>Attendance Portal</h2>
                       <hr />
                       <div class="form-group">
                            <asp:TextBox ID="tpNumber" CssClass="form-control" placeholder="TP number" runat="server"></asp:TextBox>
                       </div>
                       <div class="form-group">
                            <asp:TextBox ID="password" TextMode="Password" CssClass="form-control" placeholder="Password" runat="server"></asp:TextBox>
                       </div>
                       <div class="form-group">
                            <asp:Button CssClass="col-md-4 btn btn-default" ID="attendanceLogin" Text="Login" runat="server" OnClick="Unnamed1_Click" />
                            <asp:Label runat="server" id="msg"></asp:Label>
                       </div>
                   </div>
               </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageScripts" Runat="Server">
</asp:Content> 