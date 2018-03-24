<%@ Page Title="Duty Roster" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="View Final Roster.aspx.cs" Inherits="pages_scheduler_Generated_Roster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Duty Roster for the week of: <asp:Label ID="nextMondayDate" runat="server"></asp:Label> - Version: <asp:Label ID="versionLbl" runat="server"></asp:Label></h2>
    <asp:DropDownList ID="weekOptionsList" runat="server" OnSelectedIndexChanged="weekOptionsList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <div class="row">
            <div class="col-md-12 table table-responsive">
                <asp:PlaceHolder ID = "rosterHolder" runat="server" />
                <div class="form-group">
                    <asp:Label ID="msg" CssClass="warningLbl" runat="server"></asp:Label>
                </div>
                <hr />

                <asp:Panel ID="dutyAfterDraft" Visible="false" runat="server">
                    <div class="form-group form-inline">
                    <h3>Fill an empty duty slot</h3>
                           
                    <asp:Label AssociatedControlID="taNamesList" runat="server" >TA:</asp:Label>
                    <asp:DropDownList CssClass="form-control" ID="taNamesList" runat="server"></asp:DropDownList>
                    <asp:Label AssociatedControlID="locationsList" runat="server" >Location:</asp:Label>
                    <asp:DropDownList  CssClass="form-control" ID="locationsList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="locationsList_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label AssociatedControlID="daysList" runat="server" >Day:</asp:Label>
                    <asp:DropDownList CssClass="form-control" Enabled="false" ID="daysList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="daysList_SelectedIndexChanged"></asp:DropDownList>     
                    <asp:Label AssociatedControlID="shiftsList" runat="server" >Shift:</asp:Label>
                    <asp:DropDownList  CssClass="form-control" Enabled="false" ID="shiftsList" runat="server"></asp:DropDownList>
                    </div>
                    <div class="form-grou">
                        <asp:Button ID="addDutyToTA" Text="Fill Roster" runat="server" CssClass="btn btn-info" OnClick="addDutyToTA_Click" />
                    </div>
                </asp:Panel>
                        
            </div>      
        </div>
    <hr />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

