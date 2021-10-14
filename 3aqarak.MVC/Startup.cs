using _3aqarak.BLL.DI;
using _3aqarak.DAL.DI;
using _3aqarak.MVC.DI;
using _3aqarak.MVC.Helpers;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Caching;
using System.Web.Mvc;


[assembly: OwinStartup(typeof(_3aqarak.MVC.Startup))]

namespace _3aqarak.MVC
{
   
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {         

            var container = UseAutofac();   
            //set mvc dependency resolver to autofac resolver
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
            GlobalHost.DependencyResolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
            app.MapSignalR();
            //set signalr hub dependency resolver to autofac resolver
            // Get your HubConfiguration. In OWIN, you'll create one
            // rather than using GlobalHost.
            //var config = new HubConfiguration();
            //config.Resolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
            //app.UseAutofacMiddleware(container);
            //app.MapSignalR("/signalr", config);


            //sql cache dependency

            var tables = new string[] { "tbl_PostbonedCalls","tbl_ExpectedContracts", "tbl_PreviewDetails", "tbl_PreviewHeaders", "tbl_RentAgreementHeaders", "tbl_SaleAgreementHeaders", "tbl_FellowupCall" };
            var CS = ConfigurationManager.ConnectionStrings["RealEstateDB"].ToString();
            SqlCacheDependencyAdmin.EnableNotifications(CS);
            SqlCacheDependencyAdmin.EnableTableForNotifications(CS, tables);

        }


        private static IContainer UseAutofac()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //register abtsraction classes
            builder.RegisterModule<AutofacWebTypesModule>();

            //enable property injection for action filters
            builder.RegisterFilterProvider();

            // MVC - OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());            // STANDARD SIGNALR SETUP:           
        

            //enable property injection for signalr hubs
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // Install localization service
            builder.RegisterModule(new AutofacBLLContainer());
            builder.RegisterModule(new AutofacMVCContainer());
            builder.RegisterModule(new AutoFacDALContainer("RealEstateDB"));

            return builder.Build();

        }
    }
}
