<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        System.Web.Routing.RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
        System.Web.Routing.RouteTable.Routes.Ignore("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
        System.Web.Routing.RouteTable.Routes.Ignore("");

        
        
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("ParishService",
   new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ParishService.ParishService)));
        
        
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("PersonService",
   new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(PersonService.PersonService)));


        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("MarriageService",
   new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(MarriageService.MarriageService)));


        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("SourceTypes",
new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(SourceTypeService.SourceTypeService)));


        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("Sources",
new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(SourceService.SourceService)));
        
        System.Web.Routing.RouteTable.Routes.Add(new System.ServiceModel.Activation.ServiceRoute("", 
            new System.ServiceModel.Activation.WebServiceHostFactory(), typeof(ANDServices.APIServices)));


        
        
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
