using System.Linq;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.Clustering.Detail
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

            solverData.Variables.SantaWayFlow = new GLS.Variable[solverData.NumberOfSantas][,];
            solverData.Variables.SantaWayHasFlow = new GLS.Variable[solverData.NumberOfSantas][,];
            foreach (var santa in Enumerable.Range(0, solverData.NumberOfSantas))
            {
                solverData.Variables.SantaUsesWay[santa] =
                    solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfVisits, solverData.NumberOfVisits, $"Santa_{santa}_usesWay");

                solverData.Variables.SantaWayFlow[santa] =
                    solverData.Solver.MakeNumVarMatrix(solverData.NumberOfVisits, solverData.NumberOfVisits, 0, solverData.NumberOfVisits, $"Santa_{santa}_wayFlow");
                solverData.Variables.SantaWayHasFlow[santa] =
                    solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfVisits, solverData.NumberOfVisits, $"Santa_{santa}_wayHasFlow");
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
            solverData.Variables.SantaVisitBonus = solverData.Solver.MakeBoolVarMatrix(rows, cols, "SantaVisitBonus");
            //solverData.Variables.SantaVisitFlow = solverData.Solver.MakeNumVarMatrix(rows, cols, 0, solverData.NumberOfVisits, "SantaVisitFlow");
        }

    }
}

