using System;
using System.Linq;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class ResultBuilder
    {
        private readonly VariableBuilder variables;

        public ResultBuilder(VariableBuilder variables)
        {
            this.variables = variables;
        }

        public Route CreateResult()
        {
            Route route = new Route();

            int? nextLocation = 0;
            do
            {
                route.Waypoints.Add(nextLocation.Value);
                nextLocation = GetNextLocation(nextLocation.Value);
            } while (nextLocation.HasValue && nextLocation.Value != route.Waypoints.First());

            return route;
        }

        private int? GetNextLocation(int from)
        {
            for (int col = 0; col < variables.NumberLocations; col++)
            {
                if (variables.UsesWay[from, col].SolutionValue() == 1.0)
                {
                    return col;
                }
            }
            return null;
        }
    }
}