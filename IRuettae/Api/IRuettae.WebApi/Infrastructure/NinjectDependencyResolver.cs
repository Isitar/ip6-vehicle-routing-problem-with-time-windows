using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRuettae.GeoCalculations.Geocoding;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.WebApi.Properties;
using Ninject;

namespace IRuettae.WebApi.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;
        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            var settings = Settings.Default;
#if DEBUG
            kernel.Bind<IRouteCalculator>().To<DebugRouteCalculator>().InThreadScope();
            kernel.Bind<IGeocoder>().To<DebugGeocoder>().InThreadScope();
#else

            kernel.Bind<IGeocoder>().To<GoogleGeocoder>()
                .WithConstructorArgument("apiKey", settings.GoogleAPIKey)
                .WithConstructorArgument("region", settings.GoogleSearchRegion);
            
            kernel.Bind<IRouteCalculator>().To<GoogleRouteCalculator>()
                .WithConstructorArgument("apiKey", settings.GoogleAPIKey)
                .WithConstructorArgument("region", Settings.Default.GoogleSearchRegion);
#endif
        }
    }
}