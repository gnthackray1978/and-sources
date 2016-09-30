using GenOnline;
using TDBCore.BLL;
using TDBCore.Interfaces;
using TDBCore.Types.security;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GenWEBAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(GenWEBAPI.App_Start.NinjectWebCommon), "Stop")]

namespace GenWEBAPI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IMarriagesDal>().To<MarriagesDal>().InRequestScope();
            kernel.Bind<IPersonDal>().To<PersonDal>().InRequestScope();
            kernel.Bind<IMarriageWitnessesDal>().To<MarriageWitnessesDal>().InRequestScope();
            kernel.Bind<IParishsDal>().To<ParishsDal>().InRequestScope();
            kernel.Bind<IRelationsDal>().To<RelationsDal>().InRequestScope();
            kernel.Bind<ISourceDal>().To<SourceDal>().InRequestScope();
            kernel.Bind<ISourceMappingParishsDal>().To<SourceMappingParishsDal>().InRequestScope();
            kernel.Bind<ISourceMappingsDal>().To<SourceMappingsDal>().InRequestScope();
            kernel.Bind<ISourceTypesDal>().To<SourceTypesDal>().InRequestScope();
            kernel.Bind<IUserDal>().To<UserDal>().InRequestScope();
            kernel.Bind<ILogDal>().To<LogDal>().InRequestScope();
            kernel.Bind<IBatchDal>().To<BatchDal>().InRequestScope();
            kernel.Bind<ISecurity>().To<Security>().InRequestScope().WithConstructorArgument("user", new WebUser()).WithConstructorArgument("isSecurityEnabled", false);
        }        
    }
}
