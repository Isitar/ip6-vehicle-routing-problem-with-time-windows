using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Helpers;
using IRuettae.WebApi.Infrastructure;
using IRuettae.WebApi.Persistence;

namespace IRuettae.WebApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            CleanupRouteCalculations();
            RouteCalculator.StartWorker();
        }

        /// <summary>
        /// Sets runnign route calculations 
        /// </summary>
        private static void CleanupRouteCalculations()
        {
            using (var dbSession = SessionFactory.Instance.OpenSession())
            {
                dbSession.Query<RouteCalculation>()
                    .Where(rc => new[] { RouteCalculationState.Creating, RouteCalculationState.Ready, RouteCalculationState.Running }.Contains(rc.State))
                    .ToList()
                    .ForEach(rc =>
                    {
                        rc.State = RouteCalculationState.Cancelled;
                        dbSession.Update(rc);
                    });
                dbSession.Flush();
            }
        }
    }
}
