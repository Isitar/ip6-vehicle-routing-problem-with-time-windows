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



        public SolverInputData Build()
        {
            var timeslotLength = 5 * 60 * 1000; //5 minutes

            bool[][,] santasVar = new bool[days.Count][,];
            VisitState[][,] visitsVar = new VisitState[days.Count][,];


            for (int day = 0; day < days.Count; day++)
            {
                (var starttime, var endtime) = days[day];
                var numberOfTimeslots = Convert.ToInt32(Math.Ceiling((endtime - starttime).TotalMinutes / timeslotLength));
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
                    distances[v, d] = visits[v].FromWays.First(w => w.To.Equals(visits[d]))?.Duration ?? int.MaxValue;
                }
            }

            int[] visitLength = visits.Select(v => Convert.ToInt32(Math.Ceiling(v.Duration / (60 * timeslotLength)))).ToArray();
            return null;
        }
    }
}
