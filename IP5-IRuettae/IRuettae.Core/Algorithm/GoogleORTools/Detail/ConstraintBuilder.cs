using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.OrTools.LinearSolver;
using GLS = Google.OrTools.LinearSolver;

namespace IRuettae.Core.Algorithm.GoogleORTools.Detail
{
    class ConstraintBuilder
    {
        private SolverData solverData;

        public ConstraintBuilder(SolverData solverData)
        {
            this.solverData = solverData;
        }

        public void CreateConstraints()
        {
            CreateVisitAvailableConstraint();
            CreateVisitOverallLengthConstraint();
            CreateSantaVisitsConstraint();
            CreateOnlyOneSantaPerVisitConstraint();
            CreateSantaAvailableConstraint();
            CreateSantaOnlyOnePlaceConstraint();
            CreateSantaNeedTimeToFirstVisitConstraint();
            CreateSantaNeedsTimeToGetHomeConstraint();
            //CreateSantaNeedTimeBetweenVisitsConstraint();
        }

        /// <summary>
        /// Santas need time to go to the first Visit
        /// </summary>
        private void CreateSantaNeedTimeToFirstVisitConstraint()
        {
            const int startingPoint = 0;
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                var distance = solverData.Input.Distances[startingPoint, visit];
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        for (int timeslice = 0; timeslice < Math.Min(distance, solverData.SlicesPerDay[day]); timeslice++)
                        {
                            // Z1 == 0
                            var expr = solverData.Variables.Visits[day][santa][visit, timeslice];
                            solverData.Solver.Add(expr == 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Santas need to go back home from their last Visit
        /// </summary>
        private void CreateSantaNeedsTimeToGetHomeConstraint()
        {
            const int homePoint = 0;
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                var distance = solverData.Input.Distances[visit, homePoint];
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        var lastTimeslice = solverData.SlicesPerDay[day] - 1;
                        for (int timeslice = 0; timeslice < Math.Min(distance, lastTimeslice); timeslice++)
                        {
                            // Z1 == 0
                            var expr = solverData.Variables.Visits[day][santa][visit, lastTimeslice - timeslice];
                            solverData.Solver.Add(expr == 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Santas are not able to beam and therefore,
        /// it needs a certain time to get from one visit to another
        /// </summary>
        private void CreateSantaNeedTimeBetweenVisitsConstraint()
        {

            // Warning, potential performance problem
        }

        /// <summary>
        /// Visit is only available, when the inputs says so
        /// </summary>
        private void CreateVisitAvailableConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        var available = new LinearExpr() + Convert.ToInt32(solverData.Input.Visits[day][visit, timeslice].IsAvailable());
                        // availble >= Z1 + Z2 + ...
                        var sum = new LinearExpr();
                        for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                        {
                            sum += solverData.Variables.Visits[day][santa][visit, timeslice];
                        }
                        solverData.Solver.Add(available >= sum);
                    }
                }
            }
        }

        /// <summary>
        /// Santa is only available, when the inputs says so
        /// </summary>
        private void CreateSantaAvailableConstraint()
        {
            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        // Z2 == (int)availble
                        var expr = solverData.Variables.Santas[day][santa, timeslice] == Convert.ToInt32(solverData.Input.Santas[day][santa, timeslice]);
                        solverData.Solver.Add(expr);
                    }
                }
            }
        }

        /// <summary>
        /// Santa can only be at once play at a time, and only if he's available
        /// </summary>
        private void CreateSantaOnlyOnePlaceConstraint()
        {
            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        // Z3 >= Z1 + Z2 + ...
                        var sum = new LinearExpr();
                        for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                        {
                            sum += solverData.Variables.Visits[day][santa][visit, timeslice];
                        }
                        solverData.Solver.Add(solverData.Variables.Santas[day][santa, timeslice] >= sum);
                    }
                }
            }
        }

        /// <summary>
        /// Each Visit is visited by exactly one Santa
        /// </summary>
        private void CreateOnlyOneSantaPerVisitConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                // 1 = Z1 + Z2 + ...
                var sum = new LinearExpr();
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    sum += solverData.Variables.SantaVisits[santa, visit];
                }
                solverData.Solver.Add(1 == sum);
            }
        }

        /// <summary>
        /// Each Visit is visited by a Santa, if Santa is there at at least one visit-timeslice
        /// </summary>
        private void CreateSantaVisitsConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    var santaVisits = solverData.Variables.SantaVisits[santa, visit];
                    // Z3 <= Z1 + Z2 + ...
                    var sumOfSlices = new LinearExpr();
                    for (int day = 0; day < solverData.NumberOfDays; day++)
                    {
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            var slice = solverData.Variables.Visits[day][santa][visit, timeslice];
                            sumOfSlices += slice;

                            // Z3 >= Z1
                            solverData.Solver.Add(santaVisits >= slice);
                        }
                    }
                    solverData.Solver.Add(santaVisits <= sumOfSlices);
                }
            }
        }

        /// <summary>
        /// Each visit must be overall visited the right number of timeslices
        /// </summary>
        private void CreateVisitOverallLengthConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                // X1 = Z1 + Z2 + ...
                var sum = new LinearExpr();
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            sum += solverData.Variables.Visits[day][santa][visit, timeslice];
                        }
                    }
                }
                solverData.Solver.Add(solverData.Input.VisitsDuration[visit] + new LinearExpr() == sum);
            }
        }
    }
}
