using System.Data.Entity;
using BLL.Interfaces.Services;
using BLL.Services;
using DAL.Concrete;
using DAL.Interface.Repository;
using ORM;

[assembly: WebActivator.PreApplicationStartMethod(typeof(PhotoalbumMvcPL.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(PhotoalbumMvcPL.App_Start.NinjectWebCommon), "Stop")]

namespace PhotoalbumMvcPL.App_Start
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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<DbContext>().To<EntityModel>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope(); 

            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();  
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();

            kernel.Bind<IRoleRepository>().To<RoleRepository>().InRequestScope();
            kernel.Bind<IRoleService>().To<RoleService>().InRequestScope(); 

            kernel.Bind<IPhotoeRepository>().To<PhotoeRepository>().InRequestScope();
            kernel.Bind<IPhotoeService>().To<PhotoeService>().InRequestScope();

            kernel.Bind<ICommentRepository>().To<CommentRepository>().InRequestScope();
            kernel.Bind<ICommentService>().To<CommentService>().InRequestScope();

            kernel.Bind<IAlbumRepository>().To<AlbumRepository>().InRequestScope();
            kernel.Bind<IAlbumService>().To<AlbumService>().InRequestScope();

        }        
    }
}
