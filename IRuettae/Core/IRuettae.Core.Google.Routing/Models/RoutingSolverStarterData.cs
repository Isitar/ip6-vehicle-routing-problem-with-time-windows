using System;
using System.Collections.Generic;
using System.Text;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class RoutingSolverStarterData
    {
        public int MaxNumberOfSantas { get; set; }

        public static RoutingSolverStarterData GetDefault(OptimizationInput input)
        {
            return new RoutingSolverStarterData
            {
                MaxNumberOfSantas = input.NumberOfSantas(),
            };
        }
    }
}
