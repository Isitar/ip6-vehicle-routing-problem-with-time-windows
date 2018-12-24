using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Core
{
    /// <summary>
    /// Interface for all solvers.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Solves the optimization problem and returns an optimization result as output
        /// </summary>
        /// <param name="timeLimitMilliseconds">the timelimit in ms</param>
        /// <param name="progress">Eventhandler for progress reports</param>
        /// <param name="consoleProgress">Eventhandler for console progress reports</param>
        /// <returns>An OptimizationResult with the route</returns>
        OptimizationResult Solve(long timeLimitMilliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress);
    }
}
