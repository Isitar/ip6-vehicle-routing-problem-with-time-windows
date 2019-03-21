using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core;
using IRuettae.Core.GeneticAlgorithm;
using IRuettae.Core.GeneticAlgorithm.Algorithm.Models;
using IRuettae.Core.Google.Routing;
using IRuettae.Core.Google.Routing.Models;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.LocalSolver;
using IRuettae.Core.LocalSolver.Models;
using IRuettae.Core.Manual;
using IRuettae.Core.Models;

namespace IRuettae.WebApi.Helpers
{
    public static class SolverFactory
    {
        public static ISolver CreateSolver(OptimizationInput input, ISolverConfig solverConfig)
        {
            switch (solverConfig)
            {
                case ILPConfig ilp:
                    return new ILPSolver(input, ilp);
                case ManualConfig manual:
                    return new ManualSolver(input, manual);
                case LocalSolverConfig ls:
                    return new Solver(input, ls);
                case ParallelGenAlgConfig pga:
                    return new ParallelGenAlgSolver(input, pga);
                case GoogleRoutingConfig gr:
                    return new GoogleRoutingSolver(input, gr);
            }
            return null;
        }
    }
}