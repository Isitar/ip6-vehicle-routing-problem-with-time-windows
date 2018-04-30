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
            //bool[][,] santas

            foreach (var day in days)
            {

            }

            
            //     VisitState[][,] visits = {
            //     int[,] distances = {

            //         int[] visitLength =
            //{
            return null;
        }
    }
}
