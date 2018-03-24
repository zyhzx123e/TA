<%@ Page Title="View Duty Cancellation Requests" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Approve and Reject duty cancelation.aspx.cs" Inherits="pages_HR_Approve_and_Reject_duty_cancelation" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Duty cancellation requests</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <h3>Below are the pending cancellation requests for the week of: <asp:Label ID="nextMondayDate" runat="server"></asp:Label></h3>
            <asp:PlaceHolder ID = "dutiesContainerPlaceHolder" runat="server" />
            <asp:Label ID="msg" runat="server"></asp:Label>
            </div>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 