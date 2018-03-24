<%@ Page Title="Manage Roster Specifications" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="manage roster specifications.aspx.cs" Inherits="pages_scheduler_manage_roster_specifications" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <div class="col-md-11 col-centered">
        <h2>Location</h2>
        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <asp:ListBox ID="locations" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="locations_SelectedIndexChanged">
                    </asp:ListBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="delete_location" CssClass="btn btn-danger" Text="Delete" Enabled="false" runat="server" OnClick="delete_location_Click" />
                    <asp:Button ID="edit_location" CssClass="btn btn-default" Text="Edit" Enabled="false" runat="server" OnClick="edit_location_Click" />
                </div>
            </div>
            <div class="col-md-8">
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="location_name" Text="Name" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:TextBox ID="location_name" CssClass="form-control"  runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="location_address" Text="Address" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:TextBox ID="location_address" CssClass="form-control"  TextMode="MultiLine"  runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline form-inline">
                    <asp:Label AssociatedControlID="save_location_btn" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:Button ID="save_location_btn" CssClass="btn btn-default" Text="Save" runat="server" OnClick="save_location_btn_Click" />
                    <asp:Button ID="cancel_location_btn" CssClass="btn btn-default" Text="Cancel" Visible="false" runat="server" OnClick="cancel_location_btn_Click" />
                    <asp:Label ID="location_msg" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <!-- location's form ends -->
    <hr />
    <!-- shift's form starts -->
    <div class="col-md-11 col-centered">
        <h2>Shifts</h2>
        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <asp:ListBox ID="shifts" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="shifts_SelectedIndexChanged">
                    </asp:ListBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="delete_shift" CssClass="btn btn-danger" Text="Delete" Enabled="false" runat="server" OnClick="delete_shift_Click" />
                    <asp:Button ID="edit_shift" CssClass="btn btn-default" Text="Edit" Enabled="false" runat="server" OnClick="edit_shift_Click" />
                </div>
            </div>
            <div class="col-md-8">
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="shift_name" Text="Name" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:TextBox ID="shift_name" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="save_shift_btn" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:Button ID="save_shift_btn" CssClass="btn btn-default" Text="Add" runat="server" OnClick="save_shift_btn_Click" />
                    <asp:Button ID="cancel_shift_btn" CssClass="btn btn-default" Text="Cancel" Visible="false" runat="server" OnClick="cancel_shift_btn_Click" />
                    <asp:Label ID="shift_msg" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <!-- shift's form ends -->
    <hr />
    <!-- duty's form starts -->
    <div class="col-md-11 col-centered">
        <h2>Duties</h2>
        <hr />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <asp:ListBox ID="duties" AutoPostBack="true" CssClass="form-control" runat="server" OnSelectedIndexChanged="duties_SelectedIndexChanged">
                    </asp:ListBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="delete_duty" CssClass="btn btn-danger" Enabled="false" Text="Delete" runat="server" OnClick="delete_duty_Click" />
                    <asp:Button ID="edit_duty" CssClass="btn btn-default" Enabled="false" Text="Edit" runat="server" OnClick="edit_duty_Click" />
                </div>
            </div>
            <div class="col-md-8">
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="duty_title" Text="Title" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:TextBox ID="duty_title" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="duty_desc" Text="Description" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:TextBox ID="duty_desc" CssClass="form-control" TextMode="MultiLine" rows="4" runat="server"></asp:TextBox>
                </div>
                <div class="form-group form-inline">
                    <asp:Label AssociatedControlID="save_duty_btn" runat="server" CssClass="col-md-2"></asp:Label>
                    <asp:Button ID="save_duty_btn" CssClass="btn btn-default" Text="Add" runat="server" OnClick="save_duty_btn_Click" />
                    <asp:Button ID="cancel_duty_btn" CssClass="btn btn-default" Text="Cancel" Visible="false" runat="server" OnClick="cancel_duty_btn_Click" />
                    <asp:Label ID="duty_msg" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <!-- location's form ends -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 