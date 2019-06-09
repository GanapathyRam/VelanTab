using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Libraries.Entity;
using System.IO;
using System.Web.UI.WebControls;
using VV.Web.Models;

namespace VV.Web.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Session.Abandon(); //- See more at: http://codeverge.com/asp.net.state-management/kill-session-and-clear-local-browser/309617#sthash.L1F3U5J6.dpuf
            Control c = this.Master.FindControl("Menus");// "masterDiv"= the Id of the div.
            c.Visible = false;//to set the div to be hidden.

            //Response.Redirect("Login.aspx");
        }

        protected void LogIn(object sender, EventArgs e)
        {
            //if (IsValid)
            //{
            //    // Validate the user password
            //    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

            //    // This doen't count login failures towards account lockout
            //    // To enable password failures to trigger lockout, change to shouldLockout: true
            //    var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

            //    switch (result)
            //    {
            //        case SignInStatus.Success:
            //            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            //            break;
            //        case SignInStatus.LockedOut:
            //            Response.Redirect("/Account/Lockout");
            //            break;
            //        case SignInStatus.RequiresVerification:
            //            Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}", 
            //                                            Request.QueryString["ReturnUrl"],
            //                                            RememberMe.Checked),
            //                              true);
            //            break;
            //        case SignInStatus.Failure:
            //        default:
            //            FailureText.Text = "Invalid login attempt";
            //            ErrorMessage.Visible = true;
            //            break;
            //    }
            //}

            try
            {
                DBUtil _dbObj = new DBUtil();

                if (_dbObj.IsValidUser(UserName.Text.Trim(), Password.Text.Trim()))
                {
                    var userDto = UsersDTO.Instance;

                    if (userDto.UserName == null)
                    {
                        userDto.UserName = UserName.Text.Trim().ToString();
                    }

                    DataSet ds_Menu = _dbObj.GetScreenAccessInfo(UserName.Text.Trim());

                    Session["ds_AccessPages"] = ds_Menu;

                    GenerateDictinoaryForYearAlphabetMapping();

                    DataSet ds_temp = _dbObj.GetItemGroupDataMapping();
                    Session["ItemGroupDataMapping"] = ds_temp;

                    Response.Redirect("~/Views/ICSOrders.aspx", false);
                }
                else
                {
                    FailureText.Visible = true;
                    FailureText.Text = "Invalid Credentials";
                    Password.Text = "";
                    UserName.Text = "";
                    Response.Redirect("/Account/Login.aspx");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Exception from login screen");
            }
        }

        private void LogError(Exception ex, string section)
        {

            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += "Exception from SQLConnectionOpen" + "-" + section;
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = Server.MapPath("~/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        protected void GenerateDictinoaryForYearAlphabetMapping()
        {
            Dictionary<int, string> dictionary_Year_Alphabet = new Dictionary<int, string>();
            dictionary_Year_Alphabet.Add(2015, "D");
            dictionary_Year_Alphabet.Add(2016, "E");
            dictionary_Year_Alphabet.Add(2017, "F");
            dictionary_Year_Alphabet.Add(2018, "G");
            dictionary_Year_Alphabet.Add(2019, "H");
            dictionary_Year_Alphabet.Add(2020, "I");
            dictionary_Year_Alphabet.Add(2021, "J");
            dictionary_Year_Alphabet.Add(2022, "K");
            dictionary_Year_Alphabet.Add(2023, "L");
            dictionary_Year_Alphabet.Add(2024, "M");

            Session["YearAlphabetDictionary"] = dictionary_Year_Alphabet;
        }
    }
}