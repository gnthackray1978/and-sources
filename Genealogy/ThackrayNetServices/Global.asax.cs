using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace ThackrayNetServices
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            System.Web.Routing.RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
            System.Web.Routing.RouteTable.Routes.Ignore("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
            System.Web.Routing.RouteTable.Routes.Ignore("");



       

            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("",
                new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ANDServices.APIServices)));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}