<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="FindPatrolNumber.aspx.cs" Inherits="VV.Web.Views.FindPatrolNumber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/Custom.css" rel="stylesheet" />

    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>

    <!-- Bootstrap -->
    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <script lang="javascript" type="text/javascript">
        function Check(parentChk) {

            var elements = document.getElementsByTagName("INPUT");
            for (i = 0; i < elements.length; i++) {
                if (parentChk.checked == true) {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = true;
                    }

                }
                else {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = false;
                    }
                }
            }
        }

        function IsCheckBox(chk) {
            return (chk.type == 'checkbox');
        }

        $(function () {
            $('[id*=txtPatrolDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "tr"
            });
        });

        //$(function () {
        //    $("[id*=btnShowPopup]").click(function () {
        //        ShowPopup();
        //        return false;
        //    });
        //});
        //function ShowPopup() {
        //    $("#dialog").dialog({
        //        title: "GridView",
        //        width: 450,
        //        buttons: {
        //            Ok: function () {
        //                $(this).dialog('close');
        //            }
        //        },
        //        modal: true
        //    });
        //}

    </script>

    <div class="container-fluid" style="text-align: left; margin-top: 20px;">
        <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/ICSOrders.aspx">ICS Orders</a>
        <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/NonICSOrders.aspx">Non ICS Orders</a>
        <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/BulkHeatNoUpdate.aspx">Bulk Update</a>
        <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/PatrolInspection.aspx">Patrol Inspection</a>
        <%--<a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/FindPatrolNumber.aspx">Find Patrol No</a>--%>
        <span class="container-fluid" style="float: right; margin-right: 10px;">
            <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Account/Login.aspx">Logout</a>
        </span>
    </div>

</asp:Content>


