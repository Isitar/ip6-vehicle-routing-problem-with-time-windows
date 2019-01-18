using System;
using Google.OrTools.ConstraintSolver;
using IRuettae.Core.Google.Routing.Models;

namespace IRuettae.Core.Google.Routing.Algorithm
{
    public class RoutingModelCreator
    {
        private readonly RoutingData data;

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
            model.AddDimension(timeEvaluator, maxTime, maxTime, false, "time");


            // setting up santas (=vehicles)
            /*var costCallback = new TimeEvaluator(data);
            var costCallbackAdditional = new TimeEvaluator(data); // Todo
            for (int i = 0; i < data.NumberOfSantas; i++)
            {
                var day = i / (data.NumberOfSantas / data.Input.Days.Length);
                if (data.Input.IsAdditionalSanta(data.SantaIds[i]))
                {
                    model.SetVehicleCost(i, costCallbackAdditional);
                }
                else
                {
                    model.SetVehicleCost(i, costCallback);
                }
            }


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
    }
}