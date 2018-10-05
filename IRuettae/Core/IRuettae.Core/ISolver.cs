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
        OptimisationResult Solve(int timelimit, IProgress<int> progress);
    }
}
