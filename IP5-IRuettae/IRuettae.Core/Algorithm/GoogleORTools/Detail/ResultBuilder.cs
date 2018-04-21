using System;
using System.Diagnostics;
using System.Linq;
using IRuettae.Core.Algorithm.GoogleORTools.Detail;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class ResultBuilder
    {
        private readonly SolverData solverData;

        public ResultBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public Route CreateResult()
        {
            Debug.WriteLine($"{solverData.Solver.Objective().Value()} is the value of the target function.");

            //var route = new Route();
            //{
            //    int? nextLocation = 0;
            //    do
            //    {
            //        route.Waypoints.Add(nextLocation.Value);
            //        nextLocation = GetNextLocation(nextLocation.Value);
            //    } while (nextLocation.HasValue && nextLocation.Value != route.Waypoints.First());
            //}

            return null;
        }

        private int? GetNextLocation(int from)
        {
            //for (int col = 0; col < variables.NumberLocations; col++)
            //{
            //    if (variables.UsesWay[from, col].SolutionValue() == 1.0)
            //    {
            //        return col;
            //    }
            //}
            return null;
        }
    }
}