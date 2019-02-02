using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm.TimeWindow
{
    public interface ITimeWindowStrategy
    {
        void AddConstraints(RoutingData data, RoutingModel model, IntVar cumulTime, RoutingDimension timeDim, int visit);
    }
}
