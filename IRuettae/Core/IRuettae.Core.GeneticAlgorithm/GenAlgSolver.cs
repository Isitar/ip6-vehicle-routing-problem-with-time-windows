using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Models;
using Route = IRuettae.Core.Models.Route;
using Waypoint = IRuettae.Core.Models.Waypoint;

namespace IRuettae.Core.GeneticAlgorithm
{
    public class GenAlgSolver : ISolver
    {
        private readonly OptimizationInput input;
        private readonly GenAlgStarterData starterData;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="starterData"></param>
        public GenAlgSolver(OptimizationInput input, GenAlgStarterData starterData)
        {
            this.input = input;
            this.starterData = starterData;
        }

        public OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress)
        {
            throw new NotImplementedException();
        }
    }
}
