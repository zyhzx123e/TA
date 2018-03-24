<%@ Page Title="Assign Duties to Shifts" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="Assign duties to shifts.aspx.cs" Inherits="pages_scheduler_Assign_duties_to_shifts" %>

<asp:Content ID="Content2" ContentPlaceHolderID="page_head" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="pageContent" Runat="Server">
    <h2>Assign duties to shifts</h2>
    <hr />
        <div class="col-md-12">
            <h3>Current duties in the roster:</h3>
            <div class="form-group form-inline">
                <div class="table-responsive upDownMargin">
                    <asp:GridView ID="location_shift_duty_grid" CssClass="table" runat="server" AutoGenerateColumns="False" DataKeyNames="location_id,shift_id,duty_id,day" DataSourceID="locationShiftDutyDS" AllowPaging="True" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="location_id" HeaderText="location_id" ReadOnly="True" SortExpression="location_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                            <asp:BoundField DataField="shift_id" HeaderText="shift_id" ReadOnly="True" SortExpression="shift_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                            <asp:BoundField DataField="duty_id" HeaderText="duty_id" ReadOnly="True" SortExpression="duty_id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"  />
                            <asp:BoundField DataField="day" HeaderText="Day" ReadOnly="True" SortExpression="day" />
                            <asp:BoundField DataField="LNAME" ReadOnly="true" HeaderText="Location" SortExpression="LNAME" />
                            <asp:BoundField DataField="SNAME" ReadOnly="true" HeaderText="Shift" SortExpression="SNAME" />
                            <asp:BoundField DataField="DTITLE" ReadOnly="true" HeaderText="Duty" SortExpression="DTITLE" />
                            <asp:BoundField DataField="active" HeaderText="Active" Visible="false" SortExpression="required_number" />
                            <asp:CommandField ShowEditButton="False" />
                            <asp:CommandField ShowDeleteButton="True" />
                        </Columns>

                    </asp:GridView>
                    <asp:SqlDataSource ID="locationShiftDutyDS" runat="server" ConnectionString="<%$ ConnectionStrings:fypdbConnectionString %>" ProviderName="<%$ ConnectionStrings:fypdbConnectionString.ProviderName %>" SelectCommand="SELECT duty_shift_location.location_id,  duty_shift_location.shift_id, duty_shift_location.duty_id, duty_shift_location.day, location.name AS LNAME, shift.name AS SNAME, duty.title AS DTITLE, duty_shift_location.active FROM shift INNER JOIN duty_shift_location ON shift.shift_id = duty_shift_location.shift_id INNER JOIN location ON duty_shift_location.location_id = location.location_id INNER JOIN duty ON duty_shift_location.duty_id = duty.duty_id"
                            DeleteCommand="DELETE FROM duty_shifT_location WHERE location_id=? AND shift_id =? AND duty_id=? AND day=?"
                            UpdateCommand="UPDATE duty_shift_location SET active=? WHERE  location_id=? AND shift_id =? AND duty_id=? AND day=? "></asp:SqlDataSource>
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <h3>Add new duty to the roster:</h3>
            <div class="form-group form-inline">
                <asp:Label runat="server" AssociatedControlID="locationDropList" Text="Location: "></asp:Label>
                <asp:DropDownList ID="locationDropList" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="locationDropList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label runat="server" AssociatedControlID="dayList" Text="Day: "></asp:Label>
                <asp:DropDownList ID="dayList" CssClass="form-control" AutoPostBack="true" runat="server" Enabled="false" OnSelectedIndexChanged="dayList_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label runat="server" AssociatedControlID="shiftNameDropList" Text="Shift: "></asp:Label>
                <asp:DropDownList ID="shiftNameDropList" CssClass="form-control" runat="server" Enabled="false">
                </asp:DropDownList>
                <asp:Label runat="server" AssociatedControlID="DutyDropList" Text="Duty: "></asp:Label>
                <asp:DropDownList ID="DutyDropList" CssClass="form-control" runat="server">
                </asp:DropDownList>
            </div>
            <div class="form-group form-inline">
                        
            </div>
            <div class="form-group">
                <asp:Button Text="Assign duty to shift" ID="assignDutyToShift_btn" CssClass="btn btn-success" runat="server" OnClick="assignDutyToShift_btn_Click" />
                <asp:Label ID="msg" runat="server"></asp:Label>
            </div>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
</asp:Content> 