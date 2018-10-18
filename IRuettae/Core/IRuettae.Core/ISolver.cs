﻿using System;
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
        OptimizationResult Solve(int timelimit, IProgress<ProgressReport> progress, IProgress<string> consoleProgress);
    }
}
