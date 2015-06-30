using System.ServiceModel.Activation;
using System.Web.Routing;
using Ninject;
using Ninject.Extensions.Wcf;
using Ninject.Web.Common;

namespace TancWebApp
{
    public partial class MyApplication : NinjectHttpApplication
        {
        protected override void OnApplicationStarted()
        {
            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.Ignore("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
            RouteTable.Routes.Ignore("");

            RouteTable.Routes.Add(new ServiceRoute("ParishService",  new NinjectWebServiceHostFactory(), typeof(ParishService.ParishService)));

            RouteTable.Routes.Add(new ServiceRoute("PersonService",  new NinjectWebServiceHostFactory(), typeof(PersonService.PersonService)));

            RouteTable.Routes.Add(new ServiceRoute("MarriageService", new NinjectWebServiceHostFactory(), typeof(MarriageService.MarriageService)));

            RouteTable.Routes.Add(new ServiceRoute("SourceTypes", new NinjectWebServiceHostFactory(), typeof(SourceTypeService.SourceTypeService)));

            RouteTable.Routes.Add(new ServiceRoute("Sources", new NinjectWebServiceHostFactory(), typeof(SourceService.SourceService)));

            RouteTable.Routes.Add(new ServiceRoute("", new NinjectWebServiceHostFactory(), typeof(ANDServices.APIServices)));

            base.OnApplicationStarted();
        }


        protected override IKernel CreateKernel()
        {
            return new StandardKernel(new ServiceModule());
        }
        }
    
}