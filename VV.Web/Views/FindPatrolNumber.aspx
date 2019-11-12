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
        <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/LeakTest.aspx">Leak Test</a>

        <%--<a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/FindPatrolNumber.aspx">Find Patrol No</a>--%>
        <span class="container-fluid" style="float: right; margin-right: 10px;">
            <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Account/Login.aspx">Logout</a>
        </span>
    </div>

    <div class="container-fluid">
        <div class="row" style="margin: 20px">
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblProdOrderNo" runat="server" Text="Prod Order No" CssClass="text-right res-pad form-required col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtProdOrderNo" runat="server" CssClass="form-control col-lg-8 col-md-8 col-sm-8 col-xs-7"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvLine1"
                    ControlToValidate="txtProdOrderNo" runat="server"
                    Display="Dynamic"
                    CssClass="field-validation-error"
                    Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" />
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblLocation" runat="server" Text="Location" CssClass="text-right col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:DropDownList ID="ddlLocation" DataTextField="LocationName" DataValueField="LocationCode" runat="server" CssClass="input-box col-lg-6 col-md-6 col-sm-6 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----"
                    ControlToValidate="ddlLocation" runat="server" />
            </div>
             <div class="sec-grid find-submit-btn col-lg-2 col-md-2 col-sm-12 col-xs-12">
                <asp:Button ID="btnSubmit" OnClick="btnSubmit_Click" runat="server" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Submit" />
              </div>
            <%--<div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblSubLocation" runat="server" Text="Sub Location" CssClass="text-right res-pad col-lg-4 col-md-5 col-sm-4 col-xs-5" required="required"></asp:Label>
                <asp:DropDownList ID="ddlSubLocation" DataTextField="SubLocationName" DataValueField="SubLocationCode" runat="server" CssClass="input-box col-md-6 col-sm-6 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----" ControlToValidate="ddlSubLocation" runat="server" />
            </div>--%>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="table-responsive col-lg-9 col-md-9 col-sm-12 col-xs-12">
                        <asp:GridView ID="GridViewFindPatrol" runat="server" Style="width: 300px;" AutoGenerateColumns="false"
                            OnPageIndexChanging="GridViewFindPatrol_PageIndexChanging" CssClass="table table-striped table-bordered table-hover"
                            EmptyDataText="No Patrol Number's to display."
                            AllowPaging="true">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Patrol Number" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatrolNumber" Text='<%# Bind("PatrolNumber") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Patrol Date" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatrolDate" Text='<%# Bind("PatrolDate", "{0:dd/MM/yyyy}") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Qty" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatrolQty" Text='<%# Bind("PatrolQty") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
    </div>

</asp:Content>


