using System;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class RoutingModelCreator
    {
        private readonly RoutingData data;

        // dimensions
        private const String DimensionTime = "time";

        public RoutingModelCreator(RoutingData data)
        {
            this.data = data;
        }

        /// <summary>
        /// requires data.SantaIds
        /// requires data.Visits
        /// requires data.HomeIndex
        /// requires data.HomeIndexAdditional
        /// requires data.Unavailable
        /// requires data.Start
        /// requires data.End
        /// creates data.RoutingModel
        /// </summary>
        public void Create()
        {
            var model =
                new RoutingModel(data.Visits.Length, data.SantaIds.Length,
                                 data.SantaStartIndex, data.SantaEndIndex);

            // setting up dimensions
            var maxTime = GetMaxTime();
            var timeEvaluator = new TimeEvaluator(data, 1);
            model.AddDimension(timeEvaluator, maxTime, maxTime, false, DimensionTime);

            // setting up santas (=vehicles)
            var costCallback = new TimeEvaluator(data, data.Cost.CostWorkPerHour);
            var costCallbackAdditional = new TimeEvaluator(data, data.Cost.CostWorkPerHour + data.Cost.CostAdditionalSantaPerHour);
            for (int i = 0; i < data.NumberOfSantas; i++)
            {
                if (data.Input.IsAdditionalSanta(data.SantaIds[i]))
                {
                    model.SetVehicleCost(i, costCallbackAdditional);
                }
                else
                {
                    model.SetVehicleCost(i, costCallback);
                }

                // limit time per santa
                var day = i / (data.NumberOfSantas / data.Input.Days.Length);
                model.CumulVar(model.End(i), "time").SetRange(GetDayStart(day), GetDayEnd(day));
            }

            /*
            NodeEvaluator2[] cost_callbacks = new NodeEvaluator2[number_of_vehicles];
            for (int vehicle = 0; vehicle < number_of_vehicles; ++vehicle)
            {
                int cost_coefficient = vehicle_cost_coefficients_[vehicle];
                NodeEvaluator2 manhattan_cost_callback =
                    new Manhattan(locations_, cost_coefficient);
                cost_callbacks[vehicle] = manhattan_cost_callback;
                model.SetVehicleCost(vehicle, manhattan_cost_callback);
                model.CumulVar(model.End(vehicle), "time").SetMax(
                    vehicle_end_time_[vehicle]);
            }*/

            data.RoutingModel = model;
        }

        /// <summary>
        /// Returns the duration of the longest possible day.
        /// </summary>
        /// <returns></returns>
        public int GetMaxTime()
        {
            var longestDay = int.MinValue;
            foreach (var (from, to) in data.Input.Days)
            {
                longestDay = Math.Max(longestDay, to - from);
            }

            return longestDay + 2 * data.Input.MaxWayDuration();
        }

        /// <summary>
        /// Returns the earliest time for the santas to start.
        /// </summary>
        /// <returns></returns>
        private int GetDayStart(int day)
        {
            return data.Input.Days[day].from - data.Input.MaxWayDuration();
        }

        /// <summary>
        /// Returns the lastest time for the santas to return home.
        /// </summary>
        /// <returns></returns>
        private int GetDayEnd(int day)
        {
            return data.Input.Days[day].to + data.Input.MaxWayDuration();
        }
    }
}