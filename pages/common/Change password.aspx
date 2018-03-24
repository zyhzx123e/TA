<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Change password.aspx.cs" Inherits="pages_TA_Change_password" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Change password</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:TextBox ID="old_pass" CssClass="form-control" TextMode="Password" placeholder="Old password" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="old_pass" CssClass="warningLbl" ErrorMessage="*Old password is required" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:TextBox ID="new_pass" CssClass="form-control" TextMode="Password" placeholder="New password" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="new_pass" CssClass="warningLbl" ErrorMessage="*Password cannot be empty!" runat="server"></asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:TextBox ID="verify_new_pass" CssClass="form-control" TextMode="Password" placeholder="Retype new password" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="change_password_btn" CssClass="btn btn-default" Text="Save" runat="server" OnClick="change_password_btn_Click" />
                <asp:Label runat="server" ID="msg" CssClass="warningLbl" ></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 