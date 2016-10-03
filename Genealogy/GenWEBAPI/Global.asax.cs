using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ModelBindingContext = System.Web.Http.ModelBinding.ModelBindingContext;

namespace GenWEBAPI
{ 


    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
