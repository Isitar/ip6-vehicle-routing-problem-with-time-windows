using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.ILP.Algorithm;
using IRuettae.Core.ILP.Algorithm.Scheduling;
using IRuettae.Core.Models;


namespace IRuettae.Preprocessing.Mapping
{
    public class SchedulingSolverVariableBuilder
    {
        public List<Santa> Santas { get; set; }

        public List<Visit> Visits { get; set; }

        private readonly int timeslotLength;
        private readonly OptimizationInput input;
        private readonly int[] presolved;

        private const int TwentyFourHours = 24 * 60 * 60;

        /// <summary>
        /// List of days(StartTime, EndTime)
        /// </summary>
        public List<(int Start, int End)> Days { get; set; }

        public SchedulingSolverVariableBuilder(int timeslotLength, OptimizationInput input, int[] presolved)
        {
            Santas = input.Santas.ToList();
            Visits = input.Visits.ToList();
            Days = input.Days.ToList();
            this.timeslotLength = timeslotLength;
            this.input = input;
            this.presolved = presolved;
        }

        /// <summary>
        /// Converts seconds to timeslice unit.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private int SecondsToTimeslice(double seconds) => Convert.ToInt32(Math.Ceiling(seconds / timeslotLength));

        /// <summary>
        /// Checks, if all variables are set & Builder is ready to build
        /// </summary>
        /// <returns></returns>
        public bool IsReadyToBuild()
        {
            return Days.Count > 0
                   && Santas.Count > 0
                   && Visits.Count > 0
                   && timeslotLength != 0;
        }

        public SolverInputData Build()
        {
            if (!IsReadyToBuild())
            {
                throw new InvalidOperationException();
            }

            bool[][,] santasVar = new bool[Days.Count][,];
            VisitState[][,] visitsVar = new VisitState[Days.Count][,];


            for (int d = 0; d < Days.Count; d++)
            {
                var (Start, End) = Days[d];
                var numberOfTimeslots = Convert.ToInt32((End - Start) / timeslotLength);
                santasVar[d] = new bool[Santas.Count, numberOfTimeslots];
                // set all santas available
                for (int i = 0; i < Santas.Count; i++)
                {
                    for (int j = 0; j < numberOfTimeslots; j++)
                    {
                        santasVar[d][i, j] = true;
                    }
                }

                // visits
                visitsVar[d] = new VisitState[Visits.Count + 1, numberOfTimeslots];

                for (int j = 0; j < numberOfTimeslots; j++)
                {
                    visitsVar[d][0, j] = VisitState.Default;
                }

                for (int v = 1; v <= Visits.Count; v++)
                {
                    var visit = Visits[v-1];

                    // set all to default
                    for (int j = 0; j < numberOfTimeslots; j++)
                    {
                        visitsVar[d][v, j] = VisitState.Default;
                    }

                    bool isCurrentDay((int Start, int End) p) => p.Start / TwentyFourHours == Start / TwentyFourHours;
                    (int startSlice, int endSlice) toTimeslice((int Start, int End) p)
                    {
                        return (
                            SecondsToTimeslice(p.Start - Start),
                            SecondsToTimeslice(p.End - Start)
                        );
                    }

                    // set desired
                    if (visit.Desired != null)
                    {
                        foreach (var desired in visit.Desired.Where(isCurrentDay))
                        {
                            (var startSlice, var endSlice) = toTimeslice(desired);
                            for (int t = startSlice; t < endSlice && t < numberOfTimeslots; t++)
                            {
                                visitsVar[d][v, t] = VisitState.Desired;
                            }
                        }
                    }

                    // set unavailable
                    if (visit.Unavailable != null)
                    {
                        foreach (var unavailable in visit.Unavailable.Where(isCurrentDay))
                        {
                            (var startSlice, var endSlice) = toTimeslice(unavailable);
                            for (int t = startSlice; t < endSlice && t < numberOfTimeslots; t++)
                            {
                                visitsVar[d][v, t] = VisitState.Unavailable;
                            }
                        }
                    }
                }
            }


            int[,] distances = new int[Visits.Count + 1, Visits.Count + 1];
            for (int i = 1; i < distances.GetLength(0); i++)
            {
                for (int j = 1; j < distances.GetLength(1); j++)
                {
                    distances[i, j] = SecondsToTimeslice(input.RouteCosts[i - 1, j - 1]);
                }
            }

            for (int i = 1; i < distances.GetLength(0); i++)
            {
                distances[i, 0] = SecondsToTimeslice(input.Visits[i - 1].WayCostToHome);
                distances[0, i] = SecondsToTimeslice(input.Visits[i - 1].WayCostFromHome);
            }

            distances[0, 0] = 0;

            int[] visitLength = Visits.Select(v => SecondsToTimeslice(v.Duration)).Prepend(0).ToArray();
            return new SolverInputData(santasVar, visitLength, visitsVar, distances, Visits.Select(v => v.Id).ToArray(), Santas.Select(s => s.Id).ToArray(), presolved, Days.Select(d => d.Start).ToArray());
        }
    }
}

