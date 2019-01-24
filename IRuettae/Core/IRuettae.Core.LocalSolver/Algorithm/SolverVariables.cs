using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using localsolver;

namespace IRuettae.Core.LocalSolver.Algorithm
{
    internal class SolverVariables
    {
        public LSExpression[] SantaUsed { get; }
        public LSExpression[] VisitSequences { get; }
        public LSExpression[] SantaWalkingTime { get; }
        public LSExpression[] SantaRouteTime { get; }
        public LSExpression[] SantaVisitDurations { get; }
        public LSExpression[] SantaDesiredDuration { get; }
        public LSExpression[] SantaUnavailableDuration { get; }
        public LSExpression[] SantaVisitStartingTimes { get; }

        public LSExpression[] SantaOvertime { get; }
        public LSExpression[] SantaWaitBeforeStart { get; }
        public LSExpression[][] SantaWaitBetweenVisit { get; }
        public LSExpression[] SantaWaitBetweenVisitArray { get; }


        public LSExpression VisitDurationArray { get;  }
        public LSExpression DistanceToHomeArray { get;  }
        public LSExpression DistanceFromHomeArray { get;  }
        public LSExpression DistanceArray { get;  }

        public LSExpression VisitUnavailableCountArray { get; set; }
        public LSExpression VisitUnavailableArray { get; set; }

        public LSExpression VisitDesiredCountArray { get; set; }
        public LSExpression VisitDesiredArray { get; set; }


        public LSModel Model { get; }

        public List<Visit> Visits { get; }

        public int NumberOfRoutes { get; }

        public OptimizationInput OptimizationInput { get; }

        public int NumberOfSantas { get; }
        public int NumberOfFakeSantas { get; }

        public SolverVariables(LSModel model, int numberOfRoutes, List<Visit> visits, int[,] routeCosts, OptimizationInput optimizationInput, int numberOfFakeSantas)
        {
            Model = model;
            Visits = visits;
            NumberOfRoutes = numberOfRoutes;
            OptimizationInput = optimizationInput;
            NumberOfFakeSantas = numberOfFakeSantas;
            NumberOfSantas = optimizationInput.Santas.Length;

            SantaUsed = new LSExpression[numberOfRoutes];
            VisitSequences = new LSExpression[numberOfRoutes + 1];
            SantaWalkingTime = new LSExpression[numberOfRoutes];
            SantaRouteTime = new LSExpression[numberOfRoutes];
            SantaVisitDurations = new LSExpression[numberOfRoutes];
            SantaDesiredDuration = new LSExpression[numberOfRoutes];
            SantaUnavailableDuration = new LSExpression[numberOfRoutes];
            SantaVisitStartingTimes = new LSExpression[numberOfRoutes];

            SantaOvertime = new LSExpression[numberOfRoutes];
            SantaWaitBeforeStart = new LSExpression[numberOfRoutes];
            SantaWaitBetweenVisit = new LSExpression[numberOfRoutes][];
            SantaWaitBetweenVisitArray = new LSExpression[numberOfRoutes];

            int numberOfVisits = Visits.Count;
            var longestDay = OptimizationInput.Days.Max(d => d.to - d.from);
            for (var k = 0; k < numberOfRoutes; k++)
            {
                VisitSequences[k] = Model.List(numberOfVisits);
                SantaOvertime[k] = Model.Int(0, int.MaxValue);
                SantaWaitBeforeStart[k] = Model.Int(0, longestDay);

                SantaWaitBetweenVisit[k] = new LSExpression[numberOfVisits];
                for (var i = 0; i < SantaWaitBetweenVisit[k].Length; i++)
                {
                    SantaWaitBetweenVisit[k][i] = Model.Int(0, longestDay);
                }

                SantaWaitBetweenVisitArray[k] = Model.Array(SantaWaitBetweenVisit[k]);
            }

            // overflow for unused santa breaks
            VisitSequences[numberOfRoutes] = model.List(numberOfVisits);

            DistanceArray = model.Array(RouteCostJagged(Visits, routeCosts));
            DistanceFromHomeArray = model.Array(Visits.Select(v => v.WayCostFromHome).ToArray());
            DistanceToHomeArray = model.Array(Visits.Select(v => v.WayCostToHome).ToArray());
            VisitDurationArray = model.Array(Visits.Select(v => v.Duration).ToArray());

            // desired
            var visitsOnlyDesired = visits
                .Select(v =>
                    // fake arr
                    v.Desired.Length == 0
                        ? new[] { new[] { -1, -1 } }
                        : v.Desired.Select(d => new[] { d.from, d.to }).ToArray()
                )
                .ToArray();
            VisitDesiredArray = model.Array(visitsOnlyDesired);
            VisitDesiredCountArray = model.Array(visits.Select(v => v.Desired.Length).ToArray());

            // unavailable
            var visitsOnlyUnavailable = visits
                .Select(v =>
                    // fake arr
                    v.Unavailable.Length == 0
                        ? new[] { new[] { -1, -1 } }
                        : v.Unavailable.Select(d => new[] { d.from, d.to }).ToArray()
                )
                .ToArray();
            VisitUnavailableArray = model.Array(visitsOnlyUnavailable);
            VisitUnavailableCountArray = model.Array(visits.Select(v => v.Unavailable.Length).ToArray());
        }

       

        /// <summary>
        /// Takes the input route costs and the visits from params to create a jagged array containing all route costs
        /// </summary>
        /// <param name="visits">The input visits including duplicated breaks</param>
        /// <param name="routeCosts">the input route costs</param>
        /// <returns>The route costs as a jagged array</returns>
        private int[][] RouteCostJagged(IReadOnlyList<Visit> visits, int[,] routeCosts)
        {
            var routeCostJagged = new int[visits.Count][];
            for (var i = 0; i < routeCostJagged.Length; i++)
            {
                routeCostJagged[i] = new int[visits.Count];
                for (var j = 0; j < routeCostJagged[i].Length; j++)
                {
                    routeCostJagged[i][j] = routeCosts[visits[i].Id, visits[j].Id];
                }
            }

            return routeCostJagged;
        }
    }
}
