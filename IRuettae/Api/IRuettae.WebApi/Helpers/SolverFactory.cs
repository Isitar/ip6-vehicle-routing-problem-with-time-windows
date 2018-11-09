using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core;
using IRuettae.Core.ILP;
using IRuettae.Core.ILP.Algorithm.Models;
using IRuettae.Core.Models;

namespace IRuettae.WebApi.Helpers
{
    public static class SolverFactory
    {
        public static ISolver GetSolver(OptimizationInput input, IStarterData starterData)
        {
            switch (starterData)
            {
                case ILPStarterData ilp:
                    return new ILPSolver(input, ilp);
            }
            return null;
        }
    }
}