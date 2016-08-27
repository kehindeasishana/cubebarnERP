using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Core.Data;
using Data;
using Logging;
using Newtonsoft.Json.Serialization;
using Services.Financial;
using Services.Inventory;
using Services.Purchasing;
using Services.Sales;
using Services.Security;
using Services.TaxSystem;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IContainer container;
        private ContainerBuilder builder;

        protected void Application_Start()
        {
            var updateSchemaApp = new DbMigrator(new Migrations.Configuration());
            updateSchemaApp.Update();


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            JsonMediaTypeFormatter formatter = GlobalConfiguration.Configuration.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            DependecyResolver();
        }

        private void DependecyResolver()
        {
            builder = new ContainerBuilder();

            //controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //dbcontext
            builder.Register<IDbContext>(c => new ApplicationContext()).InstancePerLifetimeScope();

            //generic repositories
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //services
            builder.RegisterType<FinancialService>().As<IFinancialService>().InstancePerLifetimeScope();
            builder.RegisterType<InventoryService>().As<IInventoryService>().InstancePerLifetimeScope();
            builder.RegisterType<SalesService>().As<ISalesService>().InstancePerLifetimeScope();
            builder.RegisterType<PurchasingService>().As<IPurchasingService>().InstancePerLifetimeScope();
            //builder.RegisterType<AdministrationService>().As<IAdministrationService>().InstancePerLifetimeScope();
            builder.RegisterType<SecurityService>().As<ISecurityService>().InstancePerLifetimeScope();
            builder.RegisterType<TaxService>().As<ITaxService>().InstancePerLifetimeScope();
            builder.RegisterType<NLoggingService>().As<ILoggingService>().InstancePerLifetimeScope();

            container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
