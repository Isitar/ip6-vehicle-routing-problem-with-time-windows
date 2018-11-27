using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Models
{
    /// <summary>
    /// Struct containing the optimization input data
    /// </summary>
    public struct OptimizationInput
    {
        /// <summary>
        /// Visit input data
        /// </summary>
        public Visit[] Visits { get; set; }

        /// <summary>
        /// Santa input data
        /// </summary>
        public Santa[] Santas { get; set; }

        /// <summary>
        /// Days input data
        /// </summary>
        public (int from, int to)[] Days { get; set; }

        /// <summary>
        /// Route costs from x to y in seconds
        /// x -> y is on position [x,y]
        /// </summary>
        public int[,] RouteCosts { get; set; }

        public int NumberOfSantas() => Santas.Length;
        public int NumberOfVisits() => Visits.Length;
        public int SumVisitDuration() => Visits.Sum(v => v.Duration);
        public int MinWayDuration() => Math.Min(RouteCosts.Cast<int>().Min(rc => rc), Visits.Select(v => Math.Min(v.WayCostFromHome, v.WayCostToHome)).Min());
        public int MaxWayDuration() => Math.Max(RouteCosts.Cast<int>().Max(rc => rc), Visits.Select(v => Math.Max(v.WayCostFromHome, v.WayCostToHome)).Max());
    }
}
