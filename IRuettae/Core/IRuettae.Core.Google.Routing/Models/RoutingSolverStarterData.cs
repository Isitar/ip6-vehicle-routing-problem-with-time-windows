using System;
using System.Collections.Generic;
using System.Text;
using IRuettae.Core.Models;

namespace IRuettae.Core.Google.Routing.Models
{
    public class RoutingSolverStarterData
    {
        public int MaxNumberOfSantas { get; private set; }
        public SolvingMode Mode { get; private set; }

        /// <summary>
        /// user should use GetDefault
        /// </summary>
        private RoutingSolverStarterData()
        {

        }

        public static RoutingSolverStarterData GetDefault(OptimizationInput input)
        {
            return new RoutingSolverStarterData
            {
                MaxNumberOfSantas = input.NumberOfSantas(),
                Mode = input.NumberOfVisits() <= 50 ? SolvingMode.Default : SolvingMode.Fast,
            };
        }
    }
}
