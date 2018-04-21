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
            CreateVisits();
            CreateWays();
            CreateSantas();
        }

        private void CreateVisits()
        {
            solverData.Variables.Visits = new GLS.Variable[solverData.NumberOfDays][][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                solverData.Variables.Visits[i] = new GLS.Variable[solverData.NumberOfSantas][,];

                var rows = solverData.NumberOfVisits;
                var cols = solverData.SlicesPerDay[i];
                for (int j = 0; j < solverData.NumberOfSantas; j++)
                {
                    solverData.Variables.Visits[i][j] = solverData.Solver.MakeBoolVarMatrix(rows, cols);
                }
            }
        }

        private void CreateWays()
        {
            solverData.Variables.UsesWay = new GLS.Variable[solverData.NumberOfSantas][,];
            for (int i = 0; i < solverData.NumberOfSantas; i++)
            {
                solverData.Variables.UsesWay[i] = solverData.Solver.MakeBoolVarMatrix(solverData.NumberOfVisits, solverData.NumberOfVisits);
            }
        }

        private void CreateSantas()
        {
            solverData.Variables.Santas = new GLS.Variable[solverData.NumberOfDays][,];

            for (int i = 0; i < solverData.NumberOfDays; i++)
            {
                solverData.Variables.Visits[i] = new GLS.Variable[solverData.NumberOfSantas][,];

                var day = solverData.Input.Visits[i];
                var rows = solverData.NumberOfSantas;
                var cols = solverData.SlicesPerDay[i];
                solverData.Variables.Santas[i] = solverData.Solver.MakeBoolVarMatrix(rows, cols);
            }
        }
    }
}
