using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class VariableBuilder
    {
        private SolverData solverData;

        public VariableBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public void CreateVariables()
        {
            CreateVisitsPerSanta();
            CreateVisits();
            CreateSantaVisits();
            CreateSantas();
            CreateUsesSanta();
        }

        private void CreateVisits()
        {
            solverData.Variables.Visits = new GLS.Variable[solverData.NumberOfDays][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                var rows = solverData.NumberOfVisits;
                var cols = solverData.SlicesPerDay[i];
                solverData.Variables.Visits[i] = solverData.Solver.MakeBoolVarMatrix(rows, cols);
            }
        }

        private void CreateSantaVisits()
        {
            solverData.Variables.SantaVisits = solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfSantas, solverData.NumberOfVisits);
        }

        private void CreateVisitsPerSanta()
        {
            solverData.Variables.VisitsPerSanta = new GLS.Variable[solverData.NumberOfDays][][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                solverData.Variables.VisitsPerSanta[i] = new GLS.Variable[solverData.NumberOfSantas][,];

                var rows = solverData.NumberOfVisits;
                var cols = solverData.SlicesPerDay[i];
                for (int j = 0; j < solverData.NumberOfSantas; j++)
                {
                    solverData.Variables.VisitsPerSanta[i][j] = solverData.Solver.MakeBoolVarMatrix(rows, cols);
                }
            }
        }

        private void CreateSantas()
        {
            solverData.Variables.Santas = new GLS.Variable[solverData.NumberOfDays][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                var rows = solverData.NumberOfSantas;
                var cols = solverData.SlicesPerDay[i];
                solverData.Variables.Santas[i] = solverData.Solver.MakeBoolVarMatrix(rows, cols);
            }
        }

        private void CreateUsesSanta()
        {
            solverData.Variables.UsesSanta = solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfDays, solverData.NumberOfSantas);
        }
    }
}
