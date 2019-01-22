using System;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    internal static class InternalSolver
    {
        // dimensions
        private const String DimensionTime = "time";

        /// <summary>
        /// requires data.SantaIds
        /// requires data.Visits
        /// requires data.HomeIndex
        /// requires data.HomeIndexAdditional
        /// requires data.Unavailable
        /// requires data.Start
        /// requires data.End
        /// </summary>
        public static (RoutingModel, Assignment) Solve(RoutingData data, long timeLimitMilliseconds)
        {
            if (false
                || data.SantaIds == null
                || data.Visits == null
                || data.Unavailable == null
                || data.SantaStartIndex == null
                || data.SantaEndIndex == null
                )
            {
                throw new ArgumentNullException();
            }

            var model =
                new RoutingModel(data.Visits.Length, data.SantaIds.Length,
                                 data.SantaStartIndex, data.SantaEndIndex);

            // setting up dimensions
            var maxTime = GetMaxTime(data);
            var timeCallback = new TimeEvaluator(data, 1);
            model.AddDimension(timeCallback, maxTime, maxTime, false, DimensionTime);

            // setting up santas (=vehicles)
            var costCallbacks = new NodeEvaluator2[data.NumberOfSantas];
            for (int i = 0; i < data.NumberOfSantas; i++)
            {
                // must be a new instance per santa
                NodeEvaluator2 costCallback;
                if (data.Input.IsAdditionalSanta(data.SantaIds[i]))
                {
                    costCallback = new TimeEvaluator(data, data.Cost.CostWorkPerHour + data.Cost.CostAdditionalSantaPerHour);
                }
                else
                {
                    costCallback = new TimeEvaluator(data, data.Cost.CostWorkPerHour);
                }
                costCallbacks[i] = costCallback;
                model.SetVehicleCost(i, costCallback);

                // limit time per santa
                var day = i / (data.NumberOfSantas / data.Input.Days.Length);
                var start = GetDayStart(data, day);
                var end = GetDayEnd(data, day);
                model.CumulVar(model.End(i), DimensionTime).SetRange(start, end);
                model.CumulVar(model.Start(i), DimensionTime).SetRange(start, end);
            }

            // setting up visits (=orders)
            for (int i = 0; i < data.NumberOfVisits; ++i)
            {
                model.CumulVar(i, DimensionTime).SetRange(data.OverallStart, data.OverallEnd);
                model.AddDisjunction(new int[] { i }, data.Cost.CostNotVisitedVisit);
            }

            // Solving
            RoutingSearchParameters search_parameters =
                RoutingModel.DefaultSearchParameters();
            search_parameters.FirstSolutionStrategy =
                FirstSolutionStrategy.Types.Value.Automatic; // maybe try AllUnperformed or PathCheapestArc
            search_parameters.TimeLimitMs = timeLimitMilliseconds;

            Console.WriteLine("Search");
            Assignment solution = model.SolveWithParameters(search_parameters);

            // protect callbacks from the GC
            GC.KeepAlive(timeCallback);
            foreach (var costCallback in costCallbacks)
            {
                GC.KeepAlive(costCallback);
            }

            // Todo maybe work with GetFixedCostOfVehicle


            return (model, solution);
        }

        /// <summary>
        /// Returns the total lenght of the time dimension.
        /// </summary>
        /// <returns></returns>
        public static int GetMaxTime(RoutingData data)
        {
            return data.OverallEnd - data.OverallStart + 2 * data.Input.MaxWayDuration();
        }

        /// <summary>
        /// Returns the earliest time for the santas to start.
        /// </summary>
        /// <returns></returns>
        public static int GetDayStart(RoutingData data, int day)
        {
            return data.Input.Days[day].from - data.Input.MaxWayDuration();
        }

        /// <summary>
        /// Returns the lastest time for the santas to return home.
        /// </summary>
        /// <returns></returns>
        public static int GetDayEnd(RoutingData data, int day)
        {
            return data.Input.Days[day].to + data.Input.MaxWayDuration();
        }
    }
}