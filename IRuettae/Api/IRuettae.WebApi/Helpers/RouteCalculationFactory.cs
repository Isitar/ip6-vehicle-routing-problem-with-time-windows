using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Manual;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using IRuettae.WebApi.Models;

namespace IRuettae.WebApi.Helpers
{
    public static class RouteCalculationFactory
    {
        public static RouteCalculation[] CreateRouteCalculation(AlgorithmStarter starter)
        {
            switch (starter.Algorithm)
            {
                case AlgorithmType.Hybrid:
                    return new[]
                    {
                        CreateRouteCalculationInternal(starter, AlgorithmType.GoogleRouting, 0.4 * starter.TimeLimitMinutes),
                        CreateRouteCalculationInternal(starter, AlgorithmType.GeneticAlgorithm, 0.6 * starter.TimeLimitMinutes),
                    };
                default:
                    return new[] { CreateRouteCalculationInternal(starter) };
            }
        }

        private static RouteCalculation CreateRouteCalculationInternal(AlgorithmStarter starter)
        {
            return CreateRouteCalculationInternal(starter, starter.Algorithm, starter.TimeLimitMinutes);
        }

        private static RouteCalculation CreateRouteCalculationInternal(AlgorithmStarter starter, AlgorithmType type, double timeLimitMinutes)
        {
            const long minutesToMiliseconds = 60 * 1000;
            return new RouteCalculation
            {
                Days = starter.Days,
                MaxNumberOfAdditionalSantas = starter.MaxNumberOfAdditionalSantas,
                SantaJson = "",
                VisitsJson = "",
                TimeLimitMiliseconds = (long)(timeLimitMinutes * minutesToMiliseconds),
                StarterVisitId = starter.StarterId,
                State = RouteCalculationState.Creating,
                TimePerChildMinutes = starter.TimePerChild,
                TimePerChildOffsetMinutes = starter.Beta0,
                Year = starter.Year,
                Algorithm = type,
            };
        }
    }
}