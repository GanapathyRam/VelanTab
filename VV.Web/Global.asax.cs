using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace VV.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Exception objErr = Server.GetLastError().GetBaseException();
            //string RootURL = "Error Caught in Application_Error event\n" + "Error in: " + Request.Url.ToString();
            //string errorMsg = "\nError Message:" + objErr.Message.ToString();
            //string errorStackTrace = "\nStack Trace:" + objErr.StackTrace.ToString();
            //Session["RootURl"] = RootURL;
            //Session["errorMsg"] = errorMsg;
            //Session["errorStackTrace"] = errorStackTrace;
            //Server.ClearError();
            //Logger.Write(this.GetType().ToString() + " : Application_Error() " + " : " + DateTime.Now + " : " + errorMsg.ToString(), "General", 0);
            //Response.Redirect("ErrorPage.aspx");
        }
    }
}