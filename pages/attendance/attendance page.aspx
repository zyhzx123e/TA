<%@ Page Title="Attendance System" Language="C#" MasterPageFile="~/Master pages/basic.master" AutoEventWireup="true" CodeFile="attendance page.aspx.cs" Inherits="pages_attendance_attendance_page" %>

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
                       <asp:Label ID="output_time" runat="server"></asp:Label>
                       <hr />
                       <p>Hello <asp:Label ID="username" runat="server"></asp:Label></p>
                       <p><asp:Label ID="output" runat="server"></asp:Label></p>
                       <asp:BulletedList ID="shiftsList" runat="server"></asp:BulletedList>
                       <asp:button ID="dutySignInBtn" Text="Sign in for duty" CssClass="btn btn-success" Visible="false" runat="server" OnClick="dutySignInBtn_Click1" />
                       <asp:button ID="signOutFromDuty" Text="Sign out from duty" CssClass="btn btn-danger" Visible="false" runat="server" OnClick="signOutFromDuty_Click" />
                       <asp:LinkButton ID="systemLogOutLink" Text="Logout of the attendance system" PostBackUrl="~/pages/attendance/Attendance sign out.aspx" runat="server"></asp:LinkButton>
                       <br />
                       <asp:Label ID="msg" runat="server"></asp:Label>
                   </div>
               </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="pageScripts" Runat="Server">
</asp:Content>

