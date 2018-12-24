using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core.GeneticAlgorithm.Algorithm.Helpers
{
    /// <summary>
    /// Class to improve performance of cost calculation
    /// </summary>
    class SimplifiedOptimizationResult : OptimizationResult
    {
        public SimplifiedOptimizationResult(OptimizationResult result)
        {
            ResultState = result.ResultState;
            Routes = result.Routes;
            OptimizationInput = result.OptimizationInput;
            TimeElapsed = result.TimeElapsed;
        }

        /// <summary>
        /// Genetic Algorithm Solutions always visit every family
        /// </summary>
        /// <returns></returns>
        public override int NumberOfNotVisitedFamilies()
        {
            return 0;
        }

        /// <summary>
        /// Genetic Algorithm Solutions never miss a break
        /// </summary>
        /// <returns></returns>
        public override int NumberOfMissingBreaks()
        {
            return 0;
        }
    }
}
