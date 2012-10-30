<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
   //System.Web.Routing.RouteTable.Routes.RouteExistingFiles 
        
        //  System.Web.Routing.RouteTable.Routes.Ignore("{handler}.ashx");
      //  System.Web.Routing.RouteTable.Routes.Ignore("{*allashx}", new { allcss = @".*\.ashx(/.*)?" });
      //  System.Web.Routing.RouteTable.Routes.Ignore("{resource}.ashx/{*pathInfo}");      




        System.Web.Routing.RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
        System.Web.Routing.RouteTable.Routes.Ignore("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
        System.Web.Routing.RouteTable.Routes.Ignore("");



        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("", new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ANDServices.APIServices)));

      //  System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("", new TancRestAPI.MyServiceHostFactory(), typeof(TancRestAPI.TancAPIService)));
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occursnth

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

 

</script>
