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
        OptimizationResult Solve(long timelimitMiliseconds, EventHandler<ProgressReport> progress, EventHandler<string> consoleProgress);
    }
}
