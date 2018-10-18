using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Persistence.Entities
{
    public class RouteCalculationResult
    {
        public OptimizationResult OptimizationResult { get; set; }
        public Dictionary<int, long> VisitMap { get; set; }
        public Dictionary<int, long> SantaMap { get; set; }
        public DateTime ZeroTime { get; set; }
        public DateTime ConvertTime(int time)
        {
            return ZeroTime.AddSeconds(time);
        }
    }
}
