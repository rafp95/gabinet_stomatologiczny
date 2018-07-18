using AutoMapper;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microsoft.Extensions.DependencyInjection;
using GabinetStomatologiczny.DAO.Models;
using GabinetStomatologiczny.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System;
using System.Collections.Generic;
using GabinetStomatologiczny.Services;


[assembly: OwinStartupAttribute(typeof(GabinetStomatologiczny.Startup))]
namespace GabinetStomatologiczny
{
    public partial class Startup
    {
        // tedt
        public void Configuration(IAppBuilder app)
        {
            //We will use Dependency Injection for all controllers and other classes, so we'll need a service collection

           var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            //configure all of the services required for DI

           ConfigureServices(services);

            //Configure authentication

           ConfigureAuth(app);

            //Create a new resolver from our own default implementation
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());

            //Set the application resolver to our default resolver.This comes from "System.Web.Mvc"
            //Other services may be added elsewhere through time
            DependencyResolver.SetResolver(resolver);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //====================================================
            // Create the DB context for the IDENTITY database
            //====================================================
            // Add a database context - this can be instantiated with no parameters
            services.AddTransient(typeof(GabinetContext));

            //====================================================
            // ApplicationUserManager
            //====================================================
            // instantiation requires the following instance of the Identity database
            //services.AddTransient(typeof(IUserStore<ApplicationUser>), p => new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // with the above defined, we can add the user manager class as a type
            //services.AddTransient(typeof(ApplicationUserManager));

            //====================================================
            // ApplicationSignInManager
            //====================================================
            // instantiation requires two parameters, [ApplicationUserManager] (defined above) and [IAuthenticationManager]
            //services.AddTransient(typeof(Microsoft.Owin.Security.IAuthenticationManager), p => new OwinContext().Authentication);
            ///services.AddTransient(typeof(ApplicationSignInManager));

            //====================================================
            // ApplicationRoleManager
            //====================================================
            // Maps the rolemanager of identity role to the concrete role manager type
            //ervices.AddTransient<RoleManager<IdentityRole>, ApplicationRoleManager>();

            // Maps the role store role to the implemented type
            //services.AddTransient<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>();
            //services.AddTransient(typeof(ApplicationRoleManager));

            //====================================================
            // Add all controllers as services
            //====================================================
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
            .Where(t => typeof(IController).IsAssignableFrom(t)
            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
    
            services.AddTransient<IPacjentsService, PacjentsService>();
            services.AddTransient<ILekarzService, LekarzsService>();
            services.AddTransient<ISendEmailService, SendEmailService>();
            services.AddTransient<ISortPacjentService, SortPacjentService>();
        }
    }

    /// <summary>
    /// Provides the default dependency resolver for the application - based on IDependencyResolver, which hhas just two methods
    /// </summary>
    public class DefaultDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider _serviceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return this._serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._serviceProvider.GetServices(serviceType);
        }

        public void AddService()
        {

        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services, IEnumerable<Type> serviceTypes)
        {
            foreach (var type in serviceTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}
