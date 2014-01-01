using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;


namespace ThackrayNetServices
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Web.Routing.RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
            System.Web.Routing.RouteTable.Routes.Ignore("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
            System.Web.Routing.RouteTable.Routes.Ignore("");

            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("APIServices",
                new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ANDServices.APIServices)));

            System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("PersonService",
               new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(PersonService.PersonService)));

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