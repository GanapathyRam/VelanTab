<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ICSOrders.aspx.cs" Inherits="VV.Web.Views.ICSOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
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
    </div>
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <section id="loginForm" style="margin-top: 100px; margin-bottom: 40px;">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-6 col-xs-12" style="padding-bottom: 15px;">
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="ProdOrderNo" CssClass="control-label col-md-4 col-xs-4">Production Order No</asp:Label>

                                    <asp:TextBox runat="server" ID="ProdOrderNo" CssClass="form-control" Style="width: 20%;" />
                                    <asp:Button runat="server" OnClick="Submit" Text="Search" Style="margin-right: 145px" CssClass="btn btn-default submit_btn" />
                                    <div>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ProdOrderNo"
                                            CssClass="text-danger col-md-8" Style="float: right" ErrorMessage="Production Order No field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtSalesOrderNo" CssClass="control-label col-md-4 col-xs-4">Sale Order No / Pos</asp:Label>

                                    <asp:TextBox runat="server" ID="txtSalesOrderNo" CssClass="form-control" Style="width: 20%;" Enabled="false" />
                                </div>
                            </div>
                            <div class="col-md-6 col-xs-12" style="padding-bottom: 15px;">
                                <div class="form-group" style="padding-bottom: 15px;">
                                    <asp:Label runat="server" AssociatedControlID="txtCustomerName" CssClass="control-label col-md-4 col-xs-4">Customer Name</asp:Label>

                                    <asp:TextBox runat="server" ID="txtCustomerName" CssClass="form-control" Style="width: 40%;" Enabled="false" />
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="txtFigureNumber" CssClass="control-label col-md-4 col-xs-4">Figure Number</asp:Label>

                                    <asp:TextBox runat="server" ID="txtFigureNumber" CssClass="form-control" Style="width: 40%;" Enabled="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 ">
                            <div class="table-responsive" style="overflow: auto; max-height: 500px;">
                                <asp:GridView ID="GridView2" AutoGenerateColumns="false" runat="server" Width="100%"
                                    CssClass="table table-striped table-bordered table-hover"
                                    EmptyDataText="No Matching Data.">
                                    <RowStyle BackColor="#F7F7DE" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="BodyHeatNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBodyHeatNo" MaxLength="200" runat="server" Text='<%# Eval("BodyHeatNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="BonnetHeatNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBonnetHeatNo" runat="server" Text='<%# Eval("BonnetHeatNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="BodyRTNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBodyRTNo" runat="server" MaxLength="200" Text='<%# Eval("BodyRTNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="BonnetRTNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBonnetRTNo" runat="server" MaxLength="200" Text='<%# Eval("BonnetRTNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="WedgeHeatNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWedgeHeatNo" runat="server" MaxLength="200" Text='<%# Eval("WedgeHeatNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="WedgeRTNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWedgeRTNo" runat="server" MaxLength="200" Text='<%# Eval("WedgeRTNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="PipeHeatNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPipeHeatNo" runat="server" MaxLength="200" Text='<%# Eval("PipeHeatNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="PipeRTNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPipeRTNo" runat="server" MaxLength="200" Text='<%# Eval("PipeRTNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bonnet2HeatNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBonnet2HeatNo" runat="server" MaxLength="200" Text='<%# Eval("Bonnet2HeatNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Bonnet2RTNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtBonnet2RTNo" runat="server" MaxLength="200" Text='<%# Eval("Bonnet2RTNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="ActuatorSerialNo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtActuatorSerialNo" runat="server" MaxLength="200" Text='<%# Eval("ActuatorSerialNo") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="col-md-6" style="padding: 0;">
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-default submit_btn" Style="margin-top: 15px; margin-right: 145px;" OnClick="btnUpdate_Click" />
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
