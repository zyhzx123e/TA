<%@ Page Title="Manage Positions' Fucntions" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="manage positions functions.aspx.cs" Inherits="pages_administrator_manage_positions_functions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Manage positions' functions</h2>
    <hr />
    <div class="row row-centered">
        <asp:Label ID="msg" runat="server"></asp:Label>
        <div class="col-md-11 col-centered">
            <asp:GridView ID="functionsManagementGrid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="functionsManagemenrtDataSource" CssClass="table table-responsive" OnRowDataBound="functionsManagementGrid_RowDataBound" OnSelectedIndexChanged="functionsManagementGrid_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="FTITLE" HeaderText="Function" SortExpression="FTITLE" />
                    <asp:BoundField DataField="PTITLE" HeaderText="Position" SortExpression="PTITLE"  />
                    <asp:BoundField DataField="given" HeaderText="given" SortExpression="given" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="assign_function" Text="Assign" CommandArgument='<%# Eval("FTITLE") + ";" + Eval("PTITLE") %>' OnCommand="assign_function_Command"  CssClass="btn btn-success" runat="server" />
                            <asp:Button ID="remove_function" Text="Remove" CommandArgument='<%# Eval("FTITLE") + ";" + Eval("PTITLE")   %>' OnCommand="remove_function_Command" CssClass="btn btn-danger" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="functionsManagemenrtDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:fypdbConnectionString %>" ProviderName="<%$ ConnectionStrings:fypdbConnectionString.ProviderName %>" SelectCommand="SELECT function.title AS FTITLE, position.title AS PTITLE, function_position.given FROM function INNER JOIN function_position ON function.function_id = function_position.function_id INNER JOIN position ON function_position.position_id = position.position_id"></asp:SqlDataSource>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 