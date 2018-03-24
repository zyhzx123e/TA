<%@ Page Title="Request Duty" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Request duty.aspx.cs" Inherits="pages_TA_Request_duty" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Request for the week of <asp:Label ID="nextMondayDate" runat="server"></asp:Label></h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div class="form-group form-inline">
                <asp:DropDownList CssClass="form-control" ID="weekDaysList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="weekDaysList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Button CssClass="btn btn-info" ID="loadShifts" Text="Load Shifts" Enabled="false" runat="server" OnClick="loadShifts_Click" />
            </div>
            <div class="form-group table-responsive">
                <asp:PlaceHolder ID = "requestOptionsPlaceHolder" runat="server" />
            </div>
            <div class="form-group">
                <asp:TextBox ID="remark" CssClass="form-control" TextMode="MultiLine" placeholder="Any remarks.." runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button CssClass="btn btn-default" ID="submitRequestBtn" runat="server" Enabled="false" Text="Request" OnClick="submitRequestBtn_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 