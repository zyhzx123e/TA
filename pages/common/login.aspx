<%@ Page Title="Login" Language="C#" MasterPageFile="~/Master pages/basic.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="pages_common_login" %>

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
                       <h2>Login</h2>
                       <hr />
                            <img src="../../images/login.PNG" class="frontImage" />
                       <hr />
                       <div class="form-group">
                            <asp:TextBox ID="tpNumber" CssClass="form-control" placeholder="TP number" runat="server"></asp:TextBox>
                       </div>
                       <div class="form-group">
                            <asp:TextBox ID="password" TextMode="Password" CssClass="form-control" placeholder="Password" runat="server"></asp:TextBox>
                       </div>
                       <div class="form-group">
                            <asp:Button ID="login_btn" CssClass="col-md-4 btn btn-default" Text="Login" height="40px" Width="150px" runat="server" OnClick="login_btn_Click" />
                            <asp:Label ID="msg" runat="server" CssClass="warningLbl alert-danger"></asp:Label>
                       </div><br />
                   </div>
               </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageScripts" Runat="Server">
</asp:Content> 