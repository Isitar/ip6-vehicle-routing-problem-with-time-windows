using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
            //TODO: organize methods properly

            //variables
            CreateVisitsConstraint();
            CreateSantaDayVisitsConstraint();
            CreateSantaVisitsConstraint();

            // real constraints
            CreateVisitAvailableConstraint();
            CreateVisitOverallLengthConstraint();

            CreateOnlyOneSantaPerVisitConstraint();
            CreateSantaAvailableConstraint();
            CreateSantaOnlyOnePlaceConstraint();
            CreateSantaNeedTimeToFirstVisitConstraint();
            CreateSantaNeedsTimeToGetHomeConstraint();

            CreateSantaNeedTimeBetweenVisitsConstraint();

            CreateSingleVisitConstraint();
            CreateUsesSantaConstraint();
            CreateNumberOfSantasNeededConstraint();
            CreateNumberOfSantasNeededOverallConstraint();
        }

        /// <summary>
        /// The number of santas needed overall must be bigger or equal the santas needed on every day
        /// </summary>
        private void CreateNumberOfSantasNeededOverallConstraint()
        {
            var count = solverData.Variables.NumberOfSantasNeededOverall;
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                solverData.Solver.Add(count >= solverData.Variables.NumberOfSantasNeeded[day]);
            }
        }

        /// <summary>
        /// The number of santas needed on a day is the sum of the individual santas which are in use
        /// </summary>
        private void CreateNumberOfSantasNeededConstraint()
        {
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                var sum = new LinearExpr();
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    sum += solverData.Variables.UsesSanta[day, santa];
                }
                solverData.Solver.Add(solverData.Variables.NumberOfSantasNeeded[day] == sum);
            }
        }

        /// <summary>
        /// Santa is beeing used if he visits anybody on that day
        /// </summary>
        private void CreateUsesSantaConstraint()
        {
            for (int day = 0; day < solverData.NumberOfDays; day++)
            {
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    var isUsed = solverData.Variables.UsesSanta[day, santa];
                    var sum = new LinearExpr();
                    for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                    {
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            var current = solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                            sum += current;
                            // Z >= Z1
                            solverData.Solver.Add(isUsed >= current);
                        }
                    }
                    // Z <= Z1 + Z2 + ...
                    solverData.Solver.Add(isUsed <= sum);
                }
            }
        }

        /// <summary>
        /// Visit is beeing visited if it is visited by a santa
        /// </summary>
        private void CreateVisitsConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        // Z = Z1 + Z2 + ...
                        var expr = new LinearExpr();
                        for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                        {
                            expr += solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                        }
                        solverData.Solver.Add(solverData.Variables.Visits[day][visit, timeslice] == expr);
                    }
                }
            }
        }

        /// <summary>
        /// A visit has to be in one piece
        /// </summary>
        private void CreateSingleVisitConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                // Z = Z1 + Z2 + ...
                // one possible start must be true
                var sum = new LinearExpr();

                var duration = solverData.Input.VisitsDuration[visit];
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int startTimeslice = 0; startTimeslice + duration < solverData.SlicesPerDay[day]; startTimeslice++)
                    {
                        // Z + (duration - 1) >= Z1 + Z2 + ...
                        var sumStart = new LinearExpr();
                        var start = solverData.Variables.VisitStart[day][visit][startTimeslice];
                        for (int timeslice = 0; timeslice < duration; timeslice++)
                        {
                            int currentTimeslice = startTimeslice + timeslice;
                            // Z <= Z1
                            solverData.Solver.Add(start <= solverData.Variables.Visits[day][visit, currentTimeslice]);
                            sumStart += solverData.Variables.Visits[day][visit, currentTimeslice];
                        }
                        solverData.Solver.Add(start + (duration - 1) >= sumStart);
                        sum += start;
                    }
                }
                solverData.Solver.Add(1 == sum);
            }
        }

        /// <summary>
        /// Santas need time to go to the first Visit
        /// </summary>
        private void CreateSantaNeedTimeToFirstVisitConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                var distance = solverData.Input.Distances[solverData.StartEndPoint, visit];
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    // Z1 + Z2 + ... == 0
                    var sum = new LinearExpr();
                    for (int timeslice = 0; timeslice < Math.Min(distance, solverData.SlicesPerDay[day]); timeslice++)
                    {
                        sum += solverData.Variables.Visits[day][visit, timeslice];
                    }
                    solverData.Solver.Add(sum == 0);
                }
            }
        }

        /// <summary>
        /// Santas need to go back home from their last Visit
        /// </summary>
        private void CreateSantaNeedsTimeToGetHomeConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                var distance = solverData.Input.Distances[visit, solverData.StartEndPoint];
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    var start = Math.Max(0, solverData.SlicesPerDay[day] - distance);
                    // Z1 + Z2 + ... == 0
                    var sum = new LinearExpr();
                    for (int timeslice = start; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                    {
                        sum += solverData.Variables.Visits[day][visit, timeslice];
                    }
                    solverData.Solver.Add(sum == 0);
                }
            }
        }

        /// <summary>
        /// Santas are not able to beam and therefore,
        /// it needs a certain time to get from one visit to another
        /// except if distance is 0
        /// </summary>
        private void CreateSantaNeedTimeBetweenVisitsConstraint()
        {
#if DEBUG
            var constraintCounter = 0;
#endif

            for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
            {
                for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                {
                    for (int destination = 1; destination < solverData.NumberOfVisits; destination++)
                    {
                        var distance = solverData.Input.Distances[visit, destination];
                        // don't add unnecessary constraints
                        if (distance <= 0) continue;

                        for (int day = 0; day < solverData.NumberOfDays; day++)
                        {
                            // since we're in bidirectional space, you can not visit B if a -> b > #timeslices
                            // but you can visit B if b->a < #timeslices
                            // --> add constraint to dissalow future use of B

                            int slicesPerDay = solverData.SlicesPerDay[day];
                            if (distance >= slicesPerDay)
                            {
                                for (int timeslice = 0; timeslice < slicesPerDay; timeslice++)
                                {
                                    var A = solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                                    var B = new LinearExpr();

                                    for (int timeSliceB = timeslice; timeSliceB < slicesPerDay; timeSliceB++)
                                    {
                                        B += solverData.Variables.VisitsPerSanta[day][santa][destination, timeSliceB];
                                    }

                                    int numberOfBs = slicesPerDay - timeslice;

                                    solverData.Solver.Add(numberOfBs * A <= numberOfBs - B);
                                }
                            }
                            // default case
                            else
                            {
                                for (int timeslice = 0; timeslice < slicesPerDay - distance; timeslice++)
                                {
                                    var A = solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                                    var B = new LinearExpr();
                                    int numberOfBs = 0;

                                    // 1 because same timeslot is handled by another constraint
                                    for (int distCounter = 1; distCounter <= distance; distCounter++)
                                    {
                                        B += solverData.Variables.VisitsPerSanta[day][santa][destination, timeslice + distCounter];
                                        numberOfBs++;
                                    }

                                    // A <= 1 - B would be easy but B can be greater than 0 and A has to be >= 0
                                    // so we multiply A by numberOfBs, possible values are 0 (if A == 0) or numberOfBs (if A == 1)
                                    // if B == 0, A can be 1 (numberOfBs <= numberOfBs), else A has to be 0 (numberOfBs - (at least 1)) is smaller than numberOfBs
                                    solverData.Solver.Add(numberOfBs * A <= numberOfBs - B);

#if DEBUG
                                    constraintCounter++;
#endif
                                }
                            }
                        }
                    }

                }
            }
#if DEBUG
            Debug.WriteLine($"CreateSantaNeedTimeBetweenVisitsConstraint - added {constraintCounter} constraints");
#endif
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
                            sum += solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
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
                        // Z == (int)availble
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
                        // Z >= Z1 + Z2 + ...
                        var sum = new LinearExpr();
                        for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
                        {
                            sum += solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
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
                    // Z <= Z1 + Z2 + ...
                    var sumOfSlices = new LinearExpr();
                    for (int day = 0; day < solverData.NumberOfDays; day++)
                    {
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            var slice = solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                            sumOfSlices += slice;

                            // Z >= Z1
                            solverData.Solver.Add(santaVisits >= slice);
                        }
                    }
                    solverData.Solver.Add(santaVisits <= sumOfSlices);
                }
            }
        }

        /// <summary>
        /// Variable creation
        /// </summary>
        private void CreateSantaDayVisitsConstraint()
        {
            for (int visit = 1; visit < solverData.NumberOfVisits; visit++)
            {
                for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                {
                    for (int day = 0; day < solverData.NumberOfDays; day++)
                    {
                        var santaDayVisits = solverData.Variables.SantaDayVisit[day][santa, visit];
                        var sumOfSlices = new LinearExpr();
                        // Z <= Z1 + Z2 + ...
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            var slice = solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                            sumOfSlices += slice;

                            // Z >= Z1
                            solverData.Solver.Add(santaDayVisits >= slice);
                        }

                        solverData.Solver.Add(santaDayVisits <= sumOfSlices);
                    }
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
                // X = Z1 + Z2 + ...
                var sum = new LinearExpr();
                for (int day = 0; day < solverData.NumberOfDays; day++)
                {
                    for (int santa = 0; santa < solverData.NumberOfSantas; santa++)
                    {
                        for (int timeslice = 0; timeslice < solverData.SlicesPerDay[day]; timeslice++)
                        {
                            sum += solverData.Variables.VisitsPerSanta[day][santa][visit, timeslice];
                        }
                    }
                }
                solverData.Solver.Add(solverData.Input.VisitsDuration[visit] + new LinearExpr() == sum);
            }
        }
    }
}
