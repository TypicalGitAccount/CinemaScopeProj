using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using CinemaScopeWeb.App_Start;
using CinemaScopeWeb.ScheduledTasks;


namespace CinemaScopeWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile<MappingProfile>();
                }
            );

            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();                           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SchedulerService.StartAsync().GetAwaiter().GetResult();
        }
    }
}
