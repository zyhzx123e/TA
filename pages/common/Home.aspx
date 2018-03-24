<%@ Page Title="Home" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="pages_TA_Home" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
    <!--

        *****************timer

    -->



    <!---->
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
   
     <h2>Welcome <asp:Label ID="username_lbl" runat="server"></asp:Label> -
        This is the week of: <asp:Label runat="server" ID="weekDate"></asp:Label>

    </h2>


    <hr />
    

    <div class="row">
        <div class="col-md-5 systemPanel">
            <div class="panel panel-info">
                <div class="panel-heading"><h3>Rounding</h3></div>
                <div class="panel-body">
                     <img src="../../images/rounding.png" />
                     <hr />
                     <asp:PlaceHolder ID = "problemCounts_ph" runat="server" />
                     <hr />
                    <asp:LinkButton ID="roundingLinkBtn" CssClass="btn btn-lg btn-primary btn-block" PostBackUrl="~/pages/TA/rounding.aspx" Text="UPDATE" runat="server" OnClick="roundingLinkBtn_Click" />
                </div>
            </div>
        </div>
        <div class="col-md-5 systemPanel">
            <div class="panel panel-info">
                <div class="panel-heading"><h3>QC</h3></div>
                <div class="panel-body">
                <img src="../../images/qc.png" />
                     <hr />
                     <asp:PlaceHolder ID ="qc_ph" runat="server" />
                     <hr />
                     <asp:LinkButton ID="qcLinkBtn" CssClass="btn btn-lg btn-primary btn-block" PostBackUrl="~/pages/TA/qc.aspx" Text="UPDATE" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 