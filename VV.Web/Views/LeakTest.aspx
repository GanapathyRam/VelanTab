<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="LeakTest.aspx.cs" Inherits="VV.Web.Views.LeakTest" %>

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

        function findPatrolNumber() {
            //window.location.href = "FindPatrolNumber.aspx";
            window.open("FindPatrolNumber.aspx", '_blank');
        }

        function IsCheckBox(chk) {
            return (chk.type == 'checkbox');
        }

        $(function () {
            $('[id*=txtPatrolDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "mm/dd/yyyy",
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
        <span class="container-fluid" style="float: right; margin-right: 10px;">
            <a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Account/Login.aspx">Logout</a>
        </span>
    </div>

    <asp:Panel ID="Message" Style="text-align: right;" runat="server" Visible="true">
        <asp:Label ID="lblMessage" Visible="false" runat="server" />
        <asp:Label ID="Label1" runat="server" CssClass="required" Text="(*) This fields are required" />
    </asp:Panel>

    <div class="container-fluid" style="border-bottom: 1px solid; padding-bottom: 10px; border-bottom-color: silver;">
        <div class="row" style="margin: 20px">
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblSubLocation" runat="server" Text="Type" CssClass="text-right form-required res-pad col-lg-4 col-md-5 col-sm-4 col-xs-5" required="required"></asp:Label>
                <asp:DropDownList ID="ddlType" DataTextField="SubLocationName" DataValueField="SubLocationCode" runat="server" CssClass="input-box col-md-6 col-sm-6 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----" ControlToValidate="ddlType" runat="server" />
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblProdOrderNo" runat="server" Text="Prod Order No" CssClass="text-right res-pad form-required col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtProdOrderNo" runat="server" OnTextChanged="txtProdOrderNo_TextChanged" AutoPostBack="true" CssClass="form-control col-md-8 col-sm-8 col-xs-7"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvLine1"
                    ControlToValidate="txtProdOrderNo" runat="server"
                    Display="Dynamic"
                    CssClass="field-validation-error"
                    Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" />
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblDate" runat="server" Text="Date" CssClass="text-right form-required col-md-4 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtPatrolDate" runat="server" placeholder="mm/dd/yyyy" CssClass="form-control col-md-8 col-sm-8 col-xs-7"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                    ControlToValidate="txtPatrolDate" runat="server"
                    Display="Dynamic"
                    CssClass="field-validation-error"
                    Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" />
            </div>

            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblLeakTestNo" runat="server" Text="Leak Test No" CssClass="text-right col-lg-4 col-md-5 form-required col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtLeakTestNo" runat="server" OnTextChanged="txtLeakTestNo_TextChanged" ToolTip="Please enter saved patrol number or select location and sub location to generate new one!." AutoPostBack="true" CssClass="form-control col-md-7 col-sm-8 col-xs-7"></asp:TextBox>
                <%--<asp:LinkButton ID="LinkButton1" Visible="true" Style="margin-left: 5px; text-decoration: none; font-style: italic; color: #0000EE;" OnClientClick="findPatrolNumber()" target="_blank" runat="server">Find Patrol Number?</asp:LinkButton>--%>
                <%--<asp:Button ID="btnEdit" OnClick="btnEdit_Click" PostBackUrl="~/Views/FindPatrolNumber.aspx" runat="server" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Edit" />--%>
            </div>
            <div class="sec-grid gen-button col-lg-3 col-md-3 col-sm-6 col-xs-6">
                <asp:Button ID="btnGenerate" runat="server" Style="background-color: #c1c1c1;" OnClick="btnGenerate_Click" CssClass="text-right btn btn-default" Text="Generate" />
                <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Delete" />
                <%--   <asp:Button ID="btnReport" OnClick="btnReport_Click" runat="server" Visible="true" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Report" />--%>
            </div>

            <div class="container-fluid" style="border-bottom: 1px solid; border-bottom-color: silver; padding-bottom: 100px;">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive col-lg-9 col-md-9 col-sm-12 col-xs-12" style="height: 200px; width: 80%; overflow: auto;">
                            <asp:GridView ID="GridViewPopUp" runat="server" Style="width: 300px;" AutoGenerateColumns="false"
                                OnPageIndexChanging="GridViewPopUp_PageIndexChanging" CssClass="table table-striped table-bordered table-hover"
                                EmptyDataText="No Serial No(s) to display."
                                AllowPaging="false">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Select" HeaderStyle-Width="30px" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                                        <HeaderTemplate>
                                            <input type="checkbox" id="chkAll" name="chkAll" onclick="Check(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" EnableViewState="false" runat="server" onclick="EnableTextBox(this)" />
                                        </ItemTemplate>
                                        <ItemStyle Wrap="false" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Serial No" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCheckListSerial" Text='<%# Bind("SerialNo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="loc-grid-button">
                                <asp:Button ID="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-default" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnPopDelete" Text="Delete" Visible="false" runat="server" CssClass="btn btn-default" OnClick="btnPopDelete_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>

    <div class="container-fluid" style="border-bottom: 1px solid; border-bottom-color: silver; padding-bottom: 10px;">
        <div class="row" style="margin: 20px">
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblItemNumber" runat="server" Text="Item Number" CssClass="text-right col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtItemNumber" runat="server" Style="width: 160px;" CssClass="form-control col-md-7 col-sm-8 col-xs-7"></asp:TextBox>
            </div>
            <div class="sec-grid col-lg-8 col-md-8 col-sm-8 col-xs-9">
                <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="text-right margin-text col-md-2 col-sm-3 col-xs-3"></asp:Label>
                <asp:TextBox ID="txtDescription" runat="server" Style="max-width: 400px; margin-left: -2px;" CssClass="form-control customer-box col-md-7 col-sm-7 col-xs-7"></asp:TextBox>
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-8 col-xs-9">
                <asp:Label ID="lblCustomer" runat="server" Text="Customer" CssClass="text-right margin-text col-lg-4 col-md-5 col-sm-3 col-xs-3"></asp:Label>
                <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control cus-md customer-box col-md-8 col-sm-7 col-xs-7"></asp:TextBox>
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblSaleOrder" runat="server" Text="Sale Order" CssClass="text-right col-md-4 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtSaleOrder" runat="server" CssClass="form-control col-md-5 col-sm-8 col-xs-7"></asp:TextBox>
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblProdOrdAndBalQty" runat="server" Visible="false" Text="Prod Ord & Bal Qty" CssClass="text-right col-md-4 col-sm-3 col-xs-3"></asp:Label>
                <asp:TextBox ID="txtProdOrdAndBalQty" runat="server" Visible="false" CssClass="form-control col-md-5 col-sm-8 col-xs-7"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="container-fluid" style="border-bottom: 1px solid; border-bottom-color: silver; padding-bottom: 10px;">
        <div class="row" style="margin: 20px">
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="Label2" runat="server" Text="Qty Tested" CssClass="text-right col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtQtyTested" runat="server" Style="width: 160px;" CssClass="form-control col-md-7 col-sm-8 col-xs-7"></asp:TextBox>
            </div>
            <div class="sec-grid col-lg-8 col-md-8 col-sm-8 col-xs-9">
                <asp:Label ID="lblOperator" runat="server" Text="Operator" CssClass="text-right form-required margin-text col-md-2 col-sm-3 col-xs-3"></asp:Label>
                <asp:DropDownList ID="ddlOperator" DataTextField="OperatorName" DataValueField="OperatorCode" runat="server" CssClass="input-box col-md-6 col-sm-6 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----" ControlToValidate="ddlOperator" runat="server" />

            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-8 col-xs-9">
                <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="text-right margin-text col-lg-4 col-md-5 col-sm-3 col-xs-3"></asp:Label>
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control cus-md customer-box col-md-8 col-sm-7 col-xs-7"></asp:TextBox>
            </div>
        </div>
    </div>

    <%-- third block end--%>


    <div class="container-fluid">
        <div class="table-responsive leak-grid col-lg-10 col-md-10 col-sm-12">
            <div style="overflow:auto;">
                <asp:GridView ID="gridLeakTest" OnRowCommand="gridLeakTest_RowCommand" AllowPaging="false"
                    OnRowEditing="gridLeakTest_RowEditing" OnRowUpdating="gridLeakTest_RowUpdating"
                    OnRowCancelingEdit="gridLeakTest_RowCancelingEdit" OnPageIndexChanged="gridLeakTest_PageIndexChanged"
                    OnRowDataBound="gridLeakTest_RowDataBound" runat="server" Width="100%" Style="overflow: auto;"
                    CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                    EmptyDataText="There are no data records to display." OnSelectedIndexChanging="gridLeakTest_SelectedIndexChanging">
                    <Columns>

                        <asp:TemplateField HeaderText="SL No" HeaderStyle-Width="50px" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="true" HeaderText="Serial No" HeaderStyle-Width="200px" ItemStyle-VerticalAlign="Middle" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNo" runat="server" Style="width:100px;" Text='<%# Eval("SerialNo") %>' />
                                <%--<asp:TextBox ID="txtObservation" runat="server" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("SerialNo") %>'> </asp:TextBox>--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Shell Leak" HeaderStyle-Width="80px" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <ItemStyle Wrap="true" />
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtShellLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("ShellLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="High Pressure SeatLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtHighPressureSeatLeak" Style="width:80px;" runat="server" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("HighPressureSeatLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Low Pressure SeatLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtLowPressureSeatLeak" Style="width:80px;" runat="server" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("LowPressureSeatLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Back SeatLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtBackSeatLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("BackSeatLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Weld Leak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtWeldLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("WeldLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Joint Leak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtJointLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("JointLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trap ShellLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtTrapShellLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("TrapShellLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trap WeldLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtTrapWeldLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("TrapWeldLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trap JointLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtTrapJointLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("TrapJointLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Trap ShutLeak" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                            ItemStyle-CssClass="visible-xs visible-md visible-lg">
                            <ItemTemplate>
                                <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                                <asp:TextBox ID="txtTrapShutLeak" runat="server" Style="width:80px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("TrapShutLeak") %>'> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                <div class="leak-grid-updatebutton">
                    <asp:Button ID="btnUpdate" Visible="false" runat="server" CssClass="btn btn-default" Text="Update" OnClick="btnUpdate_Click" />
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
