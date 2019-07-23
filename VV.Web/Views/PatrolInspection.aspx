<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="PatrolInspection.aspx.cs" Inherits="VV.Web.Views.PatrolInspection" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>

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

        function findPatrolNumber()
        {
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
        <%--<a class="btn btn-info btn-md" style="color: #fff; padding: 5px; margin-left: 10px; background-color: #0068a6; border-color: #0068a6; margin-bottom: 15px; box-sizing: border-box;" runat="server" href="~/Views/FindPatrolNumber.aspx">Find Patrol No</a>--%>
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
                <asp:Label ID="lblLocation" runat="server" Text="Location" CssClass="text-right col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:DropDownList ID="ddlLocation" DataTextField="LocationName" DataValueField="LocationCode" runat="server" CssClass="input-box col-md-6 col-sm-6 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----"
                    ControlToValidate="ddlLocation" runat="server" />
            </div>
            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblSubLocation" runat="server" Text="Sub Location" CssClass="text-right res-pad col-lg-4 col-md-5 col-sm-4 col-xs-5" required="required"></asp:Label>
                <asp:DropDownList ID="ddlSubLocation" DataTextField="SubLocationName" DataValueField="SubLocationCode" runat="server" CssClass="input-box col-md-6 col-sm-6 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----" ControlToValidate="ddlSubLocation" runat="server" />
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
                <asp:Label ID="lblProdOrderNo" runat="server" Text="Prod Order No" CssClass="text-right res-pad form-required col-lg-4 col-md-5 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtProdOrderNo" runat="server" AutoPostBack="true" OnTextChanged="txtProdOrderNo_TextChanged" CssClass="form-control col-md-8 col-sm-8 col-xs-7"></asp:TextBox>

                <asp:RequiredFieldValidator ID="rfvLine1"
                    ControlToValidate="txtProdOrderNo" runat="server"
                    Display="Dynamic"
                    CssClass="field-validation-error"
                    Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" />
            </div>

            <div class="sec-grid col-lg-4 col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblPatrolNumber" runat="server" Text="Patrol No" CssClass="text-right col-lg-4 col-md-5 form-required col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtPatrolNumber" runat="server" ToolTip="Please enter saved patrol number or select location and sub location to generate new one!." OnTextChanged="txtPatrolNumber_TextChanged" AutoPostBack="true" CssClass="form-control col-md-7 col-sm-8 col-xs-7"></asp:TextBox>
                <asp:LinkButton ID="LinkButton1" Visible="true" style="margin-left:5px; text-decoration:none; font-style:italic; color:#0000EE;" OnClientClick="findPatrolNumber()" target="_blank" runat="server">Find Patrol Number?</asp:LinkButton>
                <%--<asp:Button ID="btnEdit" OnClick="btnEdit_Click" PostBackUrl="~/Views/FindPatrolNumber.aspx" runat="server" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Edit" />--%>
            </div>
           <%-- <div>
                <asp:Label ID="lblFindPatrolNumber" runat="server" Text="?"></asp:Label>
            </div>--%>
            <div class="sec-grid gen-button col-lg-3 col-md-3 col-sm-6 col-xs-6">
                <asp:Button ID="btnGenerate" OnClick="btnGenerate_Click" runat="server" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Generate" />
                <asp:Button ID="btnDelete" OnClick="btnDelete_Click" runat="server" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Delete" />
                <asp:Button ID="btnReport" OnClick="btnReport_Click" runat="server" Visible="true" Style="background-color: #c1c1c1;" CssClass="text-right btn btn-default" Text="Report" />
            </div>
            <%-- <asp:Button ID="btnShowPopup" runat="server" Text="Show Popup" />
            <div id="dialog" style="display: none">
            </div>--%>

            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="table-responsive col-lg-9 col-md-9 col-sm-12 col-xs-12">
                        <asp:GridView ID="GridViewPopUp" runat="server" Style="width: 300px;" AutoGenerateColumns="false"
                            OnPageIndexChanging="GridViewPopUp_PageIndexChanging" CssClass="table table-striped table-bordered table-hover"
                            EmptyDataText="No Serial No(s) to display."
                            AllowPaging="true">
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
    <%-- first block end--%>

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
        </div>
    </div>
    <%-- second block end--%>


    <div class="container-fluid" style="border-bottom: 1px solid; border-bottom-color: silver; padding-bottom: 10px;">
        <div class="row" style="margin: 20px">
            <div class="sec-grid col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblPatrolQty" runat="server" Text="Patrol Qty" CssClass="text-right col-md-4 col-sm-4 col-xs-5"></asp:Label>
                <asp:TextBox ID="txtPatrolQty" AutoPostBack="true" OnTextChanged="txtPatrolQty_TextChanged" runat="server" CssClass="form-control col-md-4 col-sm-8 col-xs-7"></asp:TextBox>

                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                    ControlToValidate="txtPatrolQty" runat="server"
                    Display="Dynamic"
                    CssClass="field-validation-error"
                    Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" />--%>
            </div>
            <div class="sec-grid col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblOperator" runat="server" Text="Operator" CssClass="text-right col-md-4 col-sm-4 col-xs-5"></asp:Label>
                <asp:DropDownList ID="ddlOperator" DataTextField="OperatorName" DataValueField="OperatorCode" runat="server" CssClass="input-box col-md-7 col-sm-8 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----"
                    ControlToValidate="ddlOperator" runat="server" />
            </div>
            <div class="sec-grid col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblShift" runat="server" Text="Shift" CssClass="text-right col-md-4 col-sm-4 col-xs-5"></asp:Label>
                <asp:DropDownList ID="ddlShift" DataTextField="ShiftName" DataValueField="ShiftCode" runat="server" CssClass="input-box col-md-7 col-sm-8 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----"
                    ControlToValidate="ddlShift" runat="server" />
            </div>
            <div class="sec-grid col-md-4 col-sm-6 col-xs-6">
                <asp:Label ID="lblEmployee" runat="server" Text="Insp By" CssClass="text-right col-md-4 col-sm-4 col-xs-5"></asp:Label>
                <asp:DropDownList ID="ddlInspBy" DataTextField="EmployeeName" DataValueField="EmployeeCode" runat="server" CssClass="input-box col-md-7 col-sm-8 col-xs-7"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Text="*" ForeColor="Red" Font-Bold="true" Style="margin-left: 10px;" InitialValue="----Please Select----"
                    ControlToValidate="ddlInspBy" runat="server" />
            </div>
            <div class="sec-grid col-md-8 col-sm-8 col-xs-8">
                <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="text-right col-md-2 col-sm-3 col-xs-4"></asp:Label>
                <asp:TextBox ID="txtRemarks" runat="server" Style="max-width: 600px;" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8"></asp:TextBox>
            </div>
        </div>
    </div>
    <%-- third block end--%>


    <div class="container-fluid">

        <%--  <div class="col-lg-12 col-md-9 col-sm-12">--%>
        <%--   <asp:UpdatePanel ID="EmployeePanel" UpdateMode="Conditional" runat="server">
                <ContentTemplate>--%>
        <div class="table-responsive col-lg-9 col-md-9 col-sm-12">
            <asp:GridView ID="gridPatrolInspection" OnRowCommand="gridPatrolInspection_RowCommand" AllowPaging="false"
                OnRowEditing="gridPatrolInspection_RowEditing" OnRowUpdating="gridPatrolInspection_RowUpdating"
                OnRowCancelingEdit="gridPatrolInspection_RowCancelingEdit" OnPageIndexChanged="gridPatrolInspection_PageIndexChanged"
                OnRowDataBound="gridPatrolInspection_RowDataBound" runat="server" Width="100%"
                CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False"
                EmptyDataText="There are no data records to display." OnSelectedIndexChanging="gridPatrolInspection_SelectedIndexChanging">
                <Columns>
                    <%--<asp:TemplateField HeaderText="Select" HeaderStyle-Width="30px" HeaderStyle-CssClass="visible-xs visible-md visible-lg"
                        ItemStyle-CssClass="visible-xs visible-md visible-lg">
                        <HeaderTemplate>
                            <input type="checkbox" id="chkAll" name="chkAll" onclick="Check(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" EnableViewState="false" runat="server" onclick="EnableTextBox(this)" />
                        </ItemTemplate>
                        <ItemStyle Wrap="false" />
                    </asp:TemplateField>--%>

                    <asp:TemplateField HeaderText="SL No" HeaderStyle-Width="50px" HeaderStyle-CssClass="visible-xs visible-sm visible-md visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField Visible="false" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                        <ItemTemplate>
                            <asp:Label ID="lblCheckListSerial" Text='<%# Bind("CheckListSerial") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Check List" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                        <ItemTemplate>
                            <asp:Label ID="lblCheckListDescription" Text='<%# Bind("CheckListDescription") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Meets" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg" ItemStyle-CssClass="visible-xs visible-sm visible-md visible-lg">
                        <ItemTemplate>
                            <%-- <asp:DropDownList ID="ddlMeets" runat="server">
                            </asp:DropDownList>--%>
                            <%--<asp:Label ID="lblMeets" runat="server" Text='<%# Eval("MeetName")%>'></asp:Label>--%>
                            <%--SelectedValue='<%# Bind("Meets")%>'--%>
                            <asp:Label ID="lblMeets" runat="server" Text='<%# Eval("MeetName")%>' Visible="false"></asp:Label>
                            <asp:DropDownList ID="ddlMeets" CssClass="input-box col-md-6 col-sm-6 col-xs-8" Style="padding-right: 0; width: 130px;" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <%-- <EditItemTemplate>
                            <asp:DropDownList ID="ddlMeets" CssClass="input-box col-md-6 col-sm-6 col-xs-8" Style="padding-right: 0;" runat="server">
                            </asp:DropDownList>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Observation" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                        ItemStyle-CssClass="visible-xs visible-md visible-lg" HeaderStyle-Width="100px">
                        <%--<ItemTemplate>
                                        <asp:TextBox ID="txtObservation" Text='<%# Bind("MeetsComments") %>' runat="server"></asp:TextBox>
                                    </ItemTemplate>--%>
                        <ItemTemplate>
                            <%--<asp:Label ID="lblObservation" runat="server" Text='<%# Eval("MeetsComments") %>' />--%>
                            <asp:TextBox ID="txtObservation" runat="server" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("MeetsComments") %>'> </asp:TextBox>
                        </ItemTemplate>
                        <%-- <EditItemTemplate>
                            <asp:TextBox ID="txtObservation" runat="server" CssClass="form-control margin-left col-md-9 col-sm-8 col-xs-8" Text='<%# Bind("MeetsComments") %>'> </asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Image" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                        ItemStyle-CssClass="visible-xs visible-md visible-lg">
                        <ItemTemplate>
                            <asp:Image ID="Image1" runat="server"
                                ImageUrl='<%# !string.IsNullOrEmpty(Eval("PatrolImage").ToString()) ? "~/FileUpload/" + Eval("PatrolImage") : "~/FileUpload/NoImageFound_1.jpg" %>'
                                Height="100px" Width="100px" />
                        </ItemTemplate>
                        <%-- <EditItemTemplate>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </EditItemTemplate>--%>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="visible-xs visible-md visible-sm visible-lg"
                        ItemStyle-CssClass="visible-xs visible-md visible-lg">
                        <ItemTemplate>
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--ImageUrl="~/Image/NoImageFound_1.jpg"--%>
                            <%--<asp:Image ID="img_user" runat="server"
                                    Height="80px" Width="100px" /><br />--%>
                            <%--<asp:FileUpload ID="FileUpload1" runat="server" />--%>
                            <%--<asp:Button ID="btnUpload" runat="server" Text="Upload Photo" OnClick="btnUpload_Click1" />--%>
                            <%--<asp:FileUpload ID="FileUpload1" runat="server" />--%>
                            <%--<input type="file" accept="image/*" capture="camera" />--%>
                            <%--<asp:Button ID="btnUpload" runat="server" Text="Upload" />--%>

                            <%--   <asp:Button ID="btnAsyncUpload" runat="server"
                                            Text="Async_Upload" OnClick="Async_Upload_File" />

                                        <asp:Button ID="btnUpload" runat="server" Text="Upload"
                                            OnClick="Upload_File" />--%>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderStyle-CssClass="visible-xs visible-md visible-lg"
                        ItemStyle-CssClass="visible-xs visible-md visible-lg">
                        <ItemTemplate>
                            <asp:LinkButton ID="LkB1" runat="server" CommandName="Edit">Edit</asp:LinkButton>
                            <asp:LinkButton ID="LkB11" Visible="false" runat="server" CommandName="">Delete</asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="LkB2" runat="server" CommandName="Update">Update</asp:LinkButton>
                            <asp:LinkButton ID="LkB3" runat="server" CommandName="Cancel">Cancel</asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
            <div class="loc-grid-updatebutton">
                <asp:Button ID="btnUpdate" Visible="false" runat="server" CssClass="btn btn-default" Text="Update" OnClick="btnUpdate_Click" />
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <div>
                <div>
                    <%-- <asp:FileUpload ID="fileupload" runat="server" /><a href="../Content/">../Content/</a>
                        <br />
                        <asp:Button ID="upload" runat="server" Font-Bold="true" Text="Upload" OnClick="upload_Click" />
                        <br />
                        <br />--%>
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional">
                            <ContentTemplate>
                                <asp:Image ID="imgViewFile" Height="80px" Width="100px" runat="server" />
                                <br />
                                <asp:FileUpload ID="FileUpload1" runat="server" /><br />
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" /><br />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnUpload" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
        <%--     </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnUpload"
                        EventName="PageIndexChanging" />
                </Triggers>
            </asp:UpdatePanel>--%>
    </div>
    <%--</div>--%>
    <%--</div>--%>
</asp:Content>
