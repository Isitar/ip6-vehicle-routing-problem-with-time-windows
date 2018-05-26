using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;

namespace IRuettae.Preprocessing.Mapping
{
    public class SchedulingSolverVariableBuilder
    {
        public List<Santa> Santas { get; set; }

        public List<Visit> Visits { get; set; }

        private readonly int timeslotLength;


        /// <summary>
        /// List of days(StartTime, EndTime)
        /// </summary>
        public List<(DateTime, DateTime)> Days { get; set; }

        public SchedulingSolverVariableBuilder(int timeslotLength, List<Santa> santas = null, List<Visit> visits = null, List<(DateTime, DateTime)> days = null)
        {
            Santas = santas;
            Visits = visits;
            Days = days;
            this.timeslotLength = timeslotLength;
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


            for (int day = 0; day < Days.Count; day++)
            {
                (var starttime, var endtime) = Days[day];
                var numberOfTimeslots = Convert.ToInt32(Math.Ceiling((endtime - starttime).TotalSeconds / timeslotLength));
                santasVar[day] = new bool[Santas.Count, numberOfTimeslots];
                // set all santas available
                for (int i = 0; i < Santas.Count; i++)
                {
                    for (int j = 0; j < numberOfTimeslots; j++)
                    {
                        santasVar[day][i, j] = true;
                    }
                }

                // visits
                visitsVar[day] = new VisitState[Visits.Count, numberOfTimeslots];

                for (int v = 0; v < Visits.Count; v++)
                {
                    // set all to default
                    for (int j = 0; j < numberOfTimeslots; j++)
                    {
                        visitsVar[day][v, j] = VisitState.Default;
                    }
                    // TODO: Set other states
                }
            }


            int[,] distances = new int[Visits.Count, Visits.Count];
            for (int v = 0; v < Visits.Count; v++)
            {
                for (int d = 0; d < Visits.Count; d++)
                {
                    distances[v, d] = SecondsToTimeslice(Visits[v].FromWays.First(w => w.To.Equals(Visits[d])).Duration);
                }
            }

            int[] visitLength = Visits.Select(v => SecondsToTimeslice(v.Duration)).ToArray();
            return new SolverInputData(santasVar, visitLength, visitsVar, distances, Visits.Select(v => v.Id).ToArray());
        }
    }
}
