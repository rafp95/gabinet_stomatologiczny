using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GabinetStomatologiczny.DAL;
using System.Data.Entity;
using GabinetStomatologiczny.Models;
using GabinetStomatologiczny.DAO.Models;
using AutoMapper;
using AutoMapper.Configuration;
using System.Reflection;
using WebGrease.Css.Extensions;

namespace GabinetStomatologiczny
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*
            AutoMapper.Mapper.Initialize( cfg => {
                cfg.AddProfile<AutoMapperWizyta>();
                cfg.AddProfile<AutoMapperPacjents>();
                cfg.AddProfile<AutoMapperAdmin>();
                cfg.AddProfile<AutoMapperLekarz>();
            });
            */

            AutoMapperConfiguration.Configure();

            Mapper.AssertConfigurationIsValid();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new GabinetInitializer());
        }

       
    }
}
