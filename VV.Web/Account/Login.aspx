<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VV.Web.Account.Login" Async="true" %>

   

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <div class="container-fluid" style="margin-top:-40px; margin-left:0px; height:50px;">
         <a class="navbar-brand" style="padding:10px;"><img id="IMG1" alt="Company Logo" src="../Image/logo_velan.png" style="cursor: pointer; width: 167px; height: 39px;" /></a>
     </div>
    <%--<h2><%: Title %>.</h2>--%>
    <link href="../Content/Site.css" rel="stylesheet" />
    <div class="row login_container" style="width:550px; padding:30px; border-radius:10px; margin-top:100px; border:1px solid #ccc;">
        <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                   
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <%-- <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Sign In:</asp:Label>
                    </div>--%>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-md-4 control-label">Username</asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" Autofocus="true" />
                            <div>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                    CssClass="text-danger" ErrorMessage="The username field is required." />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-4 control-label">Password</asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                            <div>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                            </div>
                        </div>
                    </div>
                    <%-- <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>--%>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-6" style="text-align:center;">
                            <asp:Button runat="server" OnClick="LogIn" Text="Login" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
              
            </section>
        </div>

        <%--<div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>--%>
    </div>
</asp:Content>
