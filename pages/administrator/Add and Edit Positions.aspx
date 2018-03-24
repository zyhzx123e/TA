<%@ Page Title="Add & Edit Positions" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Add and Edit Positions.aspx.cs" Inherits="pages_administrator_Add_and_Edit_Positions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content> 

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
        <h2>Positions</h2>
        <hr />
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <asp:ListBox ID="position_list" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="position_list_SelectedIndexChanged">
                    </asp:ListBox>
                </div>
                <div class="form-group">
                    <asp:Button CssClass="btn btn-default" id="edit_btn" Enabled="false" runat="server" text="Edit" CausesValidation="false" OnClick="edit_btn_Click"/>
                </div>
            </div>
            <div class="col-md-7">
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="title" Text="Title" runat="server"></asp:Label>
                    <asp:TextBox CssClass="form-control" ID="title" placeholder="ex: Technical Assistant" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="title" CssClass="warningLbl" ErrorMessage="*Title is required" runat="server"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ControlToValidate="title" CssClass="warningLbl" ErrorMessage="Title is not valid!" ValidationExpression="[a-zA-Z]{2,}[0-9]{0,16}" runat="server"></asp:RegularExpressionValidator>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="pay_rate" Text="Pay rate" runat="server"></asp:Label>
                    <asp:TextBox CssClass="form-control" TextMode="Number" ID="pay_rate" placeholder="ex: 5" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="pay_rate" CssClass="warningLbl" ErrorMessage="*Pay rate is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="quota" Text="Quota Hours" runat="server"></asp:Label>
                    <asp:TextBox CssClass="form-control" TextMode="Number" ID="quota" placeholder="Ex: 11" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ControlToValidate="quota" CssClass="warningLbl" ErrorMessage="*Qutoa is required" runat="server"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group form-inline">
                    <asp:Button CssClass="btn btn-default" id="save_btn" runat="server" text="ADD" OnClick="save_btn_Click"/>
                    <asp:Button CssClass="btn btn-default" id="cancel_btn" Visible="false" CausesValidation="false" runat="server" text="Cancel" OnClick="cancel_btn_Click" />
                    <asp:Label ID="msg" runat="server"></asp:Label>
                </div>
            </div>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 