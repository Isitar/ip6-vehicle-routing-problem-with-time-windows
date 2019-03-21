using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.LocalSolver.Models;
using IRuettae.Core.Manual;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Models;

namespace IRuettae.WebApi.Helpers
{
    public static class SolverConfigFactory
    {
        public static ISolverConfig CreateSolverConfig(RouteCalculation routeCalculation, OptimizationInput input)
        {
            switch (routeCalculation.Algorithm)
            {
                case AlgorithmType.ILP:
                    return
                        new ILPConfig
                        {
                            TimeSliceDuration = Properties.Settings.Default.TimeSliceDurationSeconds,
                            ClusteringMIPGap = Properties.Settings.Default.MIPGapClustering,
                            ClusteringTimeLimitMiliseconds = (long)(0.7 * routeCalculation.TimeLimitMiliseconds),
                            SchedulingMIPGap = Properties.Settings.Default.MIPGapScheduling,
                            SchedulingTimeLimitMiliseconds = (long)(0.3 * routeCalculation.TimeLimitMiliseconds),
                        };
                case AlgorithmType.LocalSolver:
                    return
                        new LocalSolverConfig
                        {
                            VrpTimeLimitFactor = 0.1,
                            VrptwTimeLimitFactor = 0.8,
                            MaxNumberOfAdditionalSantas = routeCalculation.MaxNumberOfAdditionalSantas,
                        };
                case AlgorithmType.GeneticAlgorithm:
                    return
                        new ParallelGenAlgConfig(new GenAlgConfig(input, routeCalculation.MaxNumberOfAdditionalSantas), Properties.Settings.Default.NumberOfGARuns);
                case AlgorithmType.GoogleRouting:
                    return new GoogleRoutingConfig(routeCalculation.MaxNumberOfAdditionalSantas, SolvingMode.All);

                case AlgorithmType.Hybrid:
                    throw new ArgumentException("Algorithm type Hybrid not allowed", nameof(routeCalculation.Algorithm));

                default:
                    throw new ArgumentOutOfRangeException(nameof(routeCalculation), routeCalculation, "Unknown AlgorithmType. Cannot create a StartData.");
            }
        }
    }
}