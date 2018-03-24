<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="troubleshoot_problem.aspx.cs" Inherits="pages_TA_troubleshoot_problem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2><a href="javascript:history.go(-1)" onclick="history.go(-1);return false;"> <asp:Button ID="backBtn" CssClass="btn btn-sm btn-info" Text="<< Back" runat="server" /> </a>Problem Troubleshooting </h2>
    <hr />
    <div class="row">
        <div class="col-md-8" id="troublshootsStepsDiv">
            <div class="form-group table-responsive">
                <asp:PlaceHolder ID = "troubleshoot_ph" runat="server" />
            </div>
        </div>
        <div class="col-md-4" id="problemDetailsDiv">
            <h3>Problem details:</h3>
            ID: <asp:Label ID="id_lbl" runat="server"></asp:Label><br />
            Venue: <asp:Label ID="venue_lbl" runat="server"></asp:Label><br />
            PC: <asp:Label ID="pc_lbl" runat="server"></asp:Label><br />
            Description: <asp:Label ID="desc_lbl" runat="server"></asp:Label><br />
            Location: <asp:Label ID="location_lbl" runat="server"></asp:Label><br />
            Added on: <asp:Label ID="added_lbl" runat="server"></asp:Label><br />
            Added by:<asp:Label ID="by_whom_lbl" runat="server"></asp:Label><br />
       
            Type: <asp:Label ID="type_lbl" runat="server"></asp:Label><br />
        </div>
    </div>
    <hr />
    <asp:Label ID="msg" runat="server"></asp:Label>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <div class="form-group form-inline">
                <asp:TextBox ID="newTroubelshootStep" CssClass="form-control" Rows="4" Columns="80" placeholder="New Troubleshoot Step . . ." TextMode="MultiLine" runat="server"></asp:TextBox>
                <asp:CheckBox CssClass="form-control checkbox-inline list-group-item-success" ID="solvedProblem" Text="Problem is solved by this step" runat="server" />
            </div>
            <div class="form-group form-inline">
                <asp:Button ID="submutNewTroubleshootBtn" Text="Add Troubleshoot" CssClass="btn btn-primary" runat="server" OnClick="submutNewTroubleshootBtn_Click" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

