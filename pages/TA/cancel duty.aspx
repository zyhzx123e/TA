<%@ Page Title="Cancel Duty" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="cancel duty.aspx.cs" Inherits="pages_TA_cancel_duty" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Cancel Duty</h2>
    <hr />
    <div class="row">
        <div class="col-md-12">
            <h3>Below are your requested duties for the week of <asp:Label ID="nextMondayDate" runat="server"></asp:Label></h3>
            <div class="form-group form-inline">
                <asp:Label AssociatedControlID="cancelRemark" CssClass="col-md-4"  Text="Please specify reason first" runat="server"></asp:Label>
                <asp:TextBox ID="cancelRemark" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="cancelRemark" CssClass="warningLbl" ErrorMessage="Reason cannot be empty." runat="server"></asp:RequiredFieldValidator>
            </div>
                    
            <div class="form-group table-responsive">
                <asp:PlaceHolder ID = "dutiesContainerPlaceHolder" runat="server" />
            </div>

            <asp:Label ID="msg" runat="server"></asp:Label>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 