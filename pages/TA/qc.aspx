<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="qc.aspx.cs" Inherits="pages_TA_qc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>QC</h2>
    <hr />
    <div class="row">
        <div class="col-lg-7">
            <asp:Button ID="add_new_problem" CssClass="btn btn-default" Text="Add a problem" runat="server" OnClick="add_new_problem_Click" />
            <asp:Label ID="msg" runat="server"></asp:Label>
        </div>
        <div class="col-lg-5">
            <div class="form-group form-inline">
                <asp:DropDownList ID="location_list" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="location_list_SelectedIndexChanged"></asp:DropDownList>
                <asp:Button ID="load_problems" Text="Load" CssClass="btn btn-info" runat="server" OnClick="load_problems_Click" />
                <asp:Button ID="reset_location" Text="Reset location" CssClass="btn btn-sm btn-default" runat="server" OnClick="reset_location_Click"/>
            </div>  
        </div>
    </div>
        
    <div class="form-group table-responsive">
       
        <div id="srollableTableDiv" style="height:800px;">

 <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
            <asp:PlaceHolder ID = "roundingProblems_ph" runat="server" />
        <asp:Timer ID="Timer1" runat="server" Interval="588" OnTick="table"></asp:Timer>
     
            </ContentTemplate>
</asp:UpdatePanel>
            
              </div>        
              

    </div>

     <!-- add new problem pop up form starts here -->
      
        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="add_new_problem_panel" TargetControlID="add_new_problem" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
        <asp:Panel ID="add_new_problem_panel" runat="server" CssClass="popForm" style ='display:none' align="center" >      
            <h2>Add a problem</h2>
            <hr />
            <div class="form-group form-inline">
                <asp:Label Text="Venue/Lab" runat="server"></asp:Label>
                <asp:TextBox ID="venueTxt" CssClass="form-control" runat="server"></asp:TextBox>
            </div>

            <div class="form-group form-inline">
                <asp:Label Text="Location" runat="server"></asp:Label>
                <asp:DropDownList ID="newProblemLocation" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group form-inline">
                <asp:Label Text="Shift" runat="server"></asp:Label>
                <asp:DropDownList ID="shiftsList" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>

            <div class="form-group form-inline">
                <asp:Label Text="Description" runat="server"></asp:Label>
                <asp:TextBox TextMode="MultiLine" ID="newProblemDescription" CssClass="form-control" Columns="30" Rows="5" runat="server"></asp:TextBox>
            </div>

            <div class="form-group form-inline">
                <asp:Button ID="submit_prob_btn" CssClass="btn btn-success" Text="Submit" runat="server" OnClick="submit_prob_btn_Click" />
                <asp:Button ID="btnClose" runat="server" CssClass="btn btn-danger" Text="Cancel" />
            </div>
        </asp:Panel>

        <!-- pop up form ends here -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content>

