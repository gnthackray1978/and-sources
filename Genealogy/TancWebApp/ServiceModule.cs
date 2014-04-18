using System;
using ANDServices;
using Ninject;
using Ninject.Extensions.Wcf;
using Ninject.Parameters;
using Ninject.Web.Common;
using TDBCore.BLL;
using TDBCore.Types.security;

namespace TancWebApp
{
    public class ServiceModule : WcfModule
    {
        public override void Load()
        {
            // Binding services InRequestScope allows a single instance to be shared for all requests to the Ninject kernel for
            // instances of that type in a given WCF REST Request.  Other options include:
            //   InTransientScope() - If you dont' want instances to be shared for a given WCF REST Request
            //   InSingletonScope() - If you want instances shared between all WCF REST Requests.  You of course need to handle thread safety in this case
            // You probably don't want to use InThreadScope() since IIS can re-use the same thread for multiple requests
            
            this.Bind<IMarriagesDal>().To<MarriagesDal>().InRequestScope();
            this.Bind<IPersonDal>().To<PersonDal>().InRequestScope();
            this.Bind<IMarriageWitnessesDal>().To<MarriageWitnessesDal>().InRequestScope();
            this.Bind<IParishsDal>().To<ParishsDal>().InRequestScope();
            this.Bind<IRelationsDal>().To<RelationsDal>().InRequestScope();
            this.Bind<ISourceDal>().To<SourceDal>().InRequestScope();
            this.Bind<ISourceMappingParishsDal>().To<SourceMappingParishsDal>().InRequestScope();
            this.Bind<ISourceMappingsDal>().To<SourceMappingsDal>().InRequestScope();
            this.Bind<ISourceTypesDal>().To<SourceTypesDal>().InRequestScope();
            this.Bind<IUserDal>().To<UserDal>().InRequestScope();             
            this.Bind<ISecurity>()  .To<Security>().InRequestScope() .WithConstructorArgument("userName", WebHelper.GetUser()).WithConstructorArgument("isSecurityEnabled", true); ;



        }
    }
}