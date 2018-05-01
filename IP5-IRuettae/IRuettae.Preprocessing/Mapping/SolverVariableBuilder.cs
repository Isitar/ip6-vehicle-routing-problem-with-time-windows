using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Preprocessing.Mapping
{
    public class SolverVariableBuilder
    {
        private List<Santa> santas = null;
        private List<Visit> visits = null;
        private List<Way> ways = null;
        private const int timeslotLength = 5 * 60; //5 minutes

        /// <summary>
        /// List of days(StartTime, EndTime)
        /// </summary>
        private List<(DateTime, DateTime)> days = null;

        public SolverVariableBuilder(List<Santa> santas = null, List<Visit> visits = null, List<Way> ways = null, List<(DateTime, DateTime)> days = null)
        {
            this.santas = santas;
            this.visits = visits;
            this.ways = ways;
            this.days = days;
        }

        /// <summary>
        /// Converts seconds to timeslice unit.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private int SecondsToTimeslice(double seconds) => Convert.ToInt32(Math.Ceiling(seconds / timeslotLength));


        public SolverInputData Build()
        {
            bool[][,] santasVar = new bool[days.Count][,];
            VisitState[][,] visitsVar = new VisitState[days.Count][,];


            for (int day = 0; day < days.Count; day++)
            {
                (var starttime, var endtime) = days[day];
                var numberOfTimeslots = Convert.ToInt32(Math.Ceiling((endtime - starttime).TotalSeconds / timeslotLength));
                santasVar[day] = new bool[santas.Count, numberOfTimeslots];
                // set all santas available
                for (int i = 0; i < santas.Count; i++)
                {
                    for (int j = 0; j < numberOfTimeslots; j++)
                    {
                        santasVar[day][i, j] = true;
                    }
                }

                // visits
                visitsVar[day] = new VisitState[visits.Count, numberOfTimeslots];

                for (int v = 0; v < visits.Count; v++)
                {
                    // set all to default
                    for (int j = 0; j < numberOfTimeslots; j++)
                    {
                        visitsVar[day][v, j] = VisitState.Default;
                    }
                    // TODO: Set other states
                }
            }


            int[,] distances = new int[visits.Count, visits.Count];
            for (int v = 0; v < visits.Count; v++)
            {
                for (int d = 0; d < visits.Count; d++)
                {
                    distances[v, d] = SecondsToTimeslice(visits[v].FromWays.First(w => w.To.Equals(visits[d])).Duration);
                }
            }

            int[] visitLength = visits.Select(v => SecondsToTimeslice(v.Duration)).ToArray();
            return new SolverInputData(santasVar, visitLength, visitsVar, timeslotLength / 60, distances);
        }
    }
}
