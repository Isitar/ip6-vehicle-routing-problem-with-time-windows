using System;
using System.Collections.Generic;
using System.Linq;
using IRuettae.Core.Algorithm;
using IRuettae.Core.Algorithm.NoTimeSlicing;
using IRuettae.Persistence.Entities;

namespace IRuettae.Preprocessing.Mapping
{
    public class SolverVariableBuilderNew
    {
        public List<Santa> Santas { get; set; }

        public List<Visit> Visits { get; set; }


        /// <summary>
        /// List of days(StartTime, EndTime)
        /// </summary>
        public List<(DateTime, DateTime)> Days { get; set; }

        public SolverVariableBuilderNew(List<Santa> santas = null, List<Visit> visits = null, List<(DateTime, DateTime)> days = null)
        {
            Santas = santas;
            Visits = visits;
            Days = days;
        }


        /// <summary>
        /// Checks, if all variables are set & Builder is ready to build
        /// </summary>
        /// <returns></returns>
        public bool IsReadyToBuild()
        {
            return Days.Count > 0
                   && Santas.Count > 0
                   && Visits.Count > 0;
        }

        //public SolverInputData(bool[,] santas, int[] visitsDuration, VisitState[,] visits, int[,] distances, int[] dayDuration)

        public Core.Algorithm.NoTimeSlicing.SolverInputData Build()
        {
            if (!IsReadyToBuild())
            {
                throw new InvalidOperationException();
            }

        
            bool[,] santasVar = new bool[Days.Count,Santas.Count];
            VisitState[,] visitsVar = new VisitState[Days.Count,Visits.Count];
            int[] visitDuration = Visits.Select(v => (int)Math.Ceiling(v.Duration)).ToArray();

            for (int day = 0; day < Days.Count; day++)
            {
                // set all santas available all days
                for (int santa = 0; santa < Santas.Count; santa++)
                {
                        santasVar[day, santa] = true;                    
                }


                for (int v = 0; v < Visits.Count; v++)
                {
                        visitsVar[day,v] = VisitState.Default;
                    // TODO: Set other states
                }


            }


            int[,] distances = new int[Visits.Count, Visits.Count];
            for (int v = 0; v < Visits.Count; v++)
            {
                for (int d = 0; d < Visits.Count; d++)
                {
                    distances[v, d] = Visits[v].FromWays.First(w => w.To.Equals(Visits[d])).Duration;
                }
            }

            int[] dayDuration = Days.Select((startEnd) => (int) Math.Ceiling((startEnd.Item2 - startEnd.Item1).TotalSeconds)).ToArray();

            return new Core.Algorithm.NoTimeSlicing.SolverInputData(santasVar, visitDuration, visitsVar, distances,
                dayDuration);
        }
    }
}
