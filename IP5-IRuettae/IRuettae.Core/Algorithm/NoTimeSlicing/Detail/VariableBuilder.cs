using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.NoTimeSlicing.Detail
{
    internal class VariableBuilder
    {
        private SolverData solverData;


        public VariableBuilder(SolverData solverData)
        {
            this.solverData = solverData;

        }

        public void CreateVariables()
        {
            CreateSantaVisit();
            CreateSantaRouteAndVisitTime();
            CreateSantaUsesWay();
        }

        private void CreateSantaUsesWay()
        {
            solverData.Variables.SantaUsesWay = new GLS.Variable[solverData.NumberOfSantas][,];
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                solverData.Variables.SantaUsesWay[santa] =
                    solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfVisits, solverData.NumberOfVisits, $"Santa_{santa}_usesWay");
            }
        }


        private void CreateSantaRouteAndVisitTime()
        {
            solverData.Variables.SantaRouteCost = new GLS.Variable[solverData.NumberOfSantas];
            solverData.Variables.SantaVisitTime = new GLS.Variable[solverData.NumberOfSantas];
            var numberOfSantas = solverData.SolverInputData.Santas.GetLength(1);
            foreach (var day in Enumerable.Range(0, solverData.SolverInputData.DayDuration.Length))
            {
                foreach (var santa in Enumerable.Range(0, numberOfSantas))
                {
                    var newIndex = day * numberOfSantas + santa;
                    solverData.Variables.SantaRouteCost[newIndex] = solverData.Solver.MakeNumVar(0,
                        double.MaxValue, $"Day_{day}_Santa_{santa}_routecost");
                    solverData.Variables.SantaVisitTime[newIndex] = solverData.Solver.MakeNumVar(0,
                        solverData.SolverInputData.DayDuration[day], $"Day_{day}_Santa_{santa}_visittime");
                }
            }
        }

        public void CreateSantaVisit()
        {
            var rows = solverData.SolverInputData.Santas.Length;
            var cols = solverData.SolverInputData.Visits.GetLength(1);
            solverData.Variables.SantaVisit = solverData.Solver.MakeBoolVarMatrix(rows, cols, "SantaVisit");
        }

    }
}
