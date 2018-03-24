<%@ Page Title="" Language="C#" MasterPageFile="~/Master pages/main.master" AutoEventWireup="true" CodeFile="roundingDashboard.aspx.cs" Inherits="pages_TA_roundingDashboard" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="page_head" Runat="Server">
    <!-- CSS for datepicker JQuery item -->
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <!--init_fade_in-->
<script type="text/javascript" src="js/jquery-1.12.3.js"></script>
    <script type="text/javascript" src="js/init_fade_in.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="pageContent" Runat="Server">
    <script type="text/javascript" src="js/jquery-1.12.3.js"></script>
    <script type="text/javascript" src="js/init_fade_in.js"></script>
    <div id="rounding_dashboard"> <h2 >Rounding Dashboard</h2></div>
    <hr />
    <div class="row" id="searchPanelDiv" runat="server">
        <div class="col-md-12">
            <div class="panel panel-default">
              <div id="pc_specific" class="panel-heading">
                <h3  class="panel-title">PC specific report <button class="btn btn-default" onclick="hideDiv('pcReportSearchDiv'); return false;"><span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span></button></h3>
              </div>
              <div class="panel-body" id="pcReportSearchDiv">

                 <div class="col-md-3">
                    <h4>1 > Choose location </h4><hr />
                    <asp:DropDownList CssClass="form-control" ID="pcReport_locationList" runat="server" OnSelectedIndexChanged="pcReport_locationList_SelectedIndexChanged"></asp:DropDownList>
                    <asp:TextBox ID="pcReport_lab" placeholder="LAB Name - ex: LAB03-03" CssClass="form-control" runat="server"></asp:TextBox>
                 </div>

                  <div class="col-md-3">
                     <h4>2 > Fill up PC details</h4><hr />
                   
                          
                      <script type="text/javascript">
                          $(function () {
                              $("[id$=pcReport_pc]").autocomplete({
                                  source: function (request, response) {
                                      $.ajax({
                                          url: "~/roundingDashboard.aspx/GetPC",
                                          data: "{ 'prefix': '" + $("[id$=pcReport_pc]").val() + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    async: true,
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('-')[0],
                                val: item.split('-')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("[id$=pcs]").val(i.item.val);
            },
            minLength: 1
        });
    });
</script>

 <!--                    
<script type="text/javascript">
    $(document).ready(function () {
        $("#pcReport_pc").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "roundingDashboard.aspx/GetPC",
                    data: "{'prefix':'" + $("#pcReport_pc").val() + "'}",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        if(data.d.length>0){
                            response($map(data.d, function (item) {
                                return {
                                    label: item.split('')[0],
                                    val: item.split('')[1]
                                }
                            }));

                        } else {
                            response([{label:'No Records Found',val:-1}]);
                            $('#pcReport_pc').val('');
                        }
                    },
                    error: function (pcs) {
                        alert("Error");
                    }
                });
            },
            select: function (event, ui) {
                if (ui.item.val == -1) {
                    return false;
                } else {
                    $("[id$=pcs]").val(ui.item.val);
                }


            },
            minLength: 1
        });
    });
</script> 

                      -->

                      
                      <!--auto complete by using js ajax with mysql   jquery-ui-->
                     <script type="text/javascript" src="js/jquery-1.12.3.js"></script>
                            <script type="text/javascript" src="js/jquery-ui.js"></script>
                   <script type="text/css" src="js/jquery-ui.structure.css"></script>
                   <script type="text/css" src="js/jquery-ui.theme.css"></script>
                         
                          <cc1:AutoCompleteExtender ID="pcReport_pc_AutoCompleteExtender" MinimumPrefixLength="1" runat="server" BehaviorID="pcReport_pc_AutoCompleteExtender"  ServiceMethod="GetPC" ServicePath="WebService.asmx" TargetControlID="pcReport_pc" FirstRowSelected="True"> </cc1:AutoCompleteExtender>
                      <asp:TextBox ID="pcReport_pc" placeholder="PC number - ex: PC02"  CssClass="form-control" runat="server" OnTextChanged="pcReport_pc_TextChanged"></asp:TextBox>
                   <asp:HiddenField ID="pcs" runat="server" /> <!--   placeholder="PC number - ex: PC02" -->
                     
                   
                   
                 </div>
                  <div class="col-md-3">
                      <h4>3 > Specify Timeline</h4><hr />
                      <asp:TextBox ID="pcReport_fromDate" placeholder="From" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                      <asp:TextBox ID="pcReport_toDate" placeholder="To" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                 </div>
                 <div class="col-md-3">
                     <h4>4> Go for it!</h4><hr />
                     <asp:Button ID="pcReport_btn" CssClass="btn btn-lg btn-info btn-block" Text="Search" runat="server" OnClick="pcReport_btn_Click" />
                 </div>
              </div>
            </div>

            <!-- Solved problems report -->
            <div class="panel panel-default">
              <div id="solved_problem" class="panel-heading">
                <h3  class="panel-title">Solved Problems Report <button class="btn btn-default toggleBtn" onclick="hideDiv('solved_problemsReport'); return false;"><span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span></button></h3>
              </div>
              <div class="panel-body" id="solved_problemsReport">
                  <div class="col-md-4">   
                      <h4>1 > Specify Timeline</h4><hr />
                      <asp:TextBox ID="solvedReport_fromDate" placeholder="From" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                      <asp:TextBox ID="solvedReport_toDate" placeholder="To" CssClass="datepicker form-control" runat="server"></asp:TextBox>  
                  </div>
                  <div class="col-md-4">
                      <h4>2 > Chose Location / Type</h4><hr />
                      <asp:DropDownList CssClass="form-control" ID="solvedProblemsDropList" runat="server"></asp:DropDownList>
                      <asp:DropDownList CssClass="form-control" ID="solvedProblemsType" runat="server">
                          <asp:ListItem Value="r">Rounding</asp:ListItem>
                          <asp:ListItem Value="q">QC</asp:ListItem>
                      </asp:DropDownList>
                 </div>
                  <div class="col-md-4">
                      <h4>2 > Go for it!</h4><hr />
                      <asp:Button ID="solvedProblemReport" CssClass="btn btn-info btn-block" Text="Search" runat="server" OnClick="solvedProblemReport_Click" />
                 </div>
                  <div class="col-md-4">
                 </div>
              </div>
            </div>

            <!-- location based -->
            <div class="panel panel-default">
              <div id="venue_location" class="panel-heading">
                <h3  class="panel-title">Location / Venue specific report <button class="btn btn-default" onclick="hideDiv('locationVenueReport'); return false;"><span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span></button></h3>
              </div>
              <div class="panel-body" id="locationVenueReport">
                 <div class="col-md-4">
                    <h4>1 > Choose location </h4><hr />
                    <asp:DropDownList CssClass="form-control" ID="locationReport_locationList" runat="server"></asp:DropDownList>
                    <asp:TextBox ID="locationReport_venue" placeholder="LAB03-03 or L2-1" CssClass="form-control" runat="server"></asp:TextBox>
                 </div>
                  <div class="col-md-4">
                      <h4>2 > Specify Timeline</h4><hr />
                      <asp:TextBox ID="locationReport_fromDate" placeholder="From" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                      <asp:TextBox ID="locationReport_toDate" placeholder="To" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                 </div>
                 <div class="col-md-4">
                     <h4>3 > Go for it!</h4><hr />
                     <asp:DropDownList CssClass="form-control" ID="locationReport_problemType" runat="server">
                         <asp:ListItem Value="r">Rounding</asp:ListItem>
                         <asp:ListItem Value="q">QC</asp:ListItem>
                     </asp:DropDownList>
                     <asp:Button ID="locationRpeort_btn" CssClass="btn btn-info btn-block" Text="Search" runat="server" OnClick="locationRpeort_btn_Click" />
                 </div>
              </div>
            </div>

            <!-- problem troubleshoot -->
            <div class="panel panel-default">
              <div id="troubleshoot" class="panel-heading">
                <h3  class="panel-title">Problem's troubleshoot search <button class="btn btn-default" onclick="hideDiv('problemTroubleshootReport'); return false;"><span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span></button></h3>
              </div>
              <div class="panel-body" id="problemTroubleshootReport">
                  <div class="col-md-4">
                      <h4>1 > Insert Problem ID</h4><hr />
                      <asp:TextBox ID="problemReport_problemID" placeholder="Problem ID" CssClass="form-control" runat="server"></asp:TextBox>
                 </div>
                 <div class="col-md-4">
                     <h4>2 > Go for it!</h4><hr />
                     <asp:Button ID="problemTroubleshootReport_btn" CssClass="btn btn-info btn-block" Text="Search" runat="server" OnClick="problemTroubleshootReport_btn_Click" />
                 </div>
                  <div class="col-md-4">
                 </div>
              </div>
            </div>

            <!-- top performers reports -->
            <div class="panel panel-default">
              <div id="top_performers" class="panel-heading">
                <h3  class="panel-title">Top Performers <button class="btn btn-default" onclick="hideDiv('topPerfomersReport'); return false;"><span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span></button></h3>
              </div>
              <div class="panel-body" id="topPerfomersReport">
                  <div class="col-md-4">   
                      <h4>1 > Specify Timeline</h4><hr />
                      <asp:TextBox ID="performers_FromDate" placeholder="From" CssClass="datepicker form-control" runat="server"></asp:TextBox>
                      <asp:TextBox ID="performers_toDate" placeholder="To" CssClass="datepicker form-control" runat="server"></asp:TextBox>  
                  </div>
                  <div class="col-md-4">
                      <h4>2 > Go for it!</h4><hr />
                      <asp:Button ID="performersReport_Btn" CssClass="btn btn-info btn-block" Text="Search" runat="server" OnClick="performersReport_Btn_Click" />
                 </div>
                  <div class="col-md-4">
                 </div>
              </div>
            </div>

        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <asp:Button ID="searchBtn" Visible="false" CssClass="btn btn-lg btn-primary btn-block" Text="Search" runat="server" OnClick="searchBtn_Click" />
        </div>
    </div>

    <div class="col-md-12" id="reportMsgDiv">
        <h3><asp:Label ID="reportMsg" runat="server"></asp:Label></h3> 
    </div>

    <div class="row" >
        <div class="col-md-12">
            <div class="form-group table-responsive" id="reportDiv" runat="server">
                <asp:PlaceHolder ID = "search_result_ph" runat="server" />
            </div>
        </div>
    </div>

    </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server">
    <script>
        $(document).ready(function () {
            $('#rounding_dashboard').fadeIn(6000);
            $("#pcReportSearchDiv").hide();
            $("#locationVenueReport").hide();
            $("#problemTroubleshootReport").hide();
            $("#topPerfomersReport").hide();
            $("#solved_problemsReport").hide();
            
        });

        function hideDiv(name) {
            $("#" + name).toggle(function(){});

            //$("#" + name + "_span").attr('class', 'glyphicon glyphicon-chevron-down');

        }


        

        $(function () {
            $(".datepicker").datepicker({ dateFormat: 'yy-mm-dd' });
        });

      
     
    </script>
</asp:Content>

