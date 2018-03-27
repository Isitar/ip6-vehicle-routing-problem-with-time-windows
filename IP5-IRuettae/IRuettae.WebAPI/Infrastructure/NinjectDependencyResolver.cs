using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.WebApi.Properties;
using Ninject;

namespace IRuettae.WebApi.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
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
#if DEBUG
            kernel.Bind<IRouteCalculator>().To<DebugRouteCalculator>().InThreadScope();
#else
            kernel.Bind<IRouteCalculator>().To<GoogleRouteCalculator>().WithConstructorArgument(Settings.Default.GoogleAPIKey);
#endif
        }
    }
}