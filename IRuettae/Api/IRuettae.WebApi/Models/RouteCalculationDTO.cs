using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Persistence.Entities;
using Newtonsoft.Json;

namespace IRuettae.WebApi.Models
{
    public class RouteCalculationDTO
    {
        // starter Properties
        public virtual DateTime StartTime { get; set; }
        public virtual int Year { get; set; }
        public virtual List<(DateTime, DateTime)> Days { get; set; }
        public virtual int TimePerChild { get; set; }
        public virtual int TimePerChildOffset { get; set; }
        public virtual int StarterVisitId { get; set; }


        // info for history purpuse
        public virtual int NumberOfSantas { get; set; }
        public virtual int NumberOfVisits { get; set; }

        // Algorithm specific data
        public virtual AlgorithmType Algorithm { get; set; }

        // Running & Result
        public virtual RouteCalculationState State { get; set; }
        public virtual double Progress { get; set; }
        public virtual IList<RouteCalculationLog> StateText { get; set; }
        public virtual DateTime EndTime { get; set; }

        // Metrics
        public virtual int Cost { get; set; }
        public virtual int NumberOfNotVisitedFamilies { get; set; }
        public virtual int NumberOfAdditionalSantas { get; set; }
        public virtual int AdditionalSantaWorkTime { get; set; }
        public virtual int VisitTimeInUnavailabe { get; set; }
        public virtual int VisitTimeInDesired { get; set; }
        public virtual int SantaWorkTime { get; set; }
        public virtual int LongestDay { get; set; }
        public virtual int NumberOfNeededSantas { get; set; }
        public virtual int NumberOfRoutes { get; set; }
        public virtual int TotalWayTime { get; set; }
        public virtual int TotalVisitTime { get; set; }
        public virtual int AverageWayTimePerRoute { get; set; }
        public virtual DateTime LatestVisit { get; set; }
        public virtual int AverageDurationPerRoute { get; set; }

        public static explicit operator RouteCalculationDTO(RouteCalculation rc)
        {
            var routeCalculationResult = JsonConvert.DeserializeObject<RouteCalculationResult>(rc.Result);
            var or = routeCalculationResult.OptimizationResult;


            var dto = new RouteCalculationDTO
            {
                StartTime = rc.StartTime,
                Year = rc.Year,
                Days = rc.Days,
                TimePerChild = rc.TimePerChild,
                TimePerChildOffset = rc.TimePerChildOffset,
                StarterVisitId = rc.StarterVisitId,
                NumberOfSantas = rc.NumberOfSantas,
                NumberOfVisits = rc.NumberOfVisits,
                Algorithm = rc.Algorithm,
                State = rc.State,
                Progress = rc.Progress,
                StateText = rc.StateText,
                EndTime = rc.EndTime,
            };

            dto.Cost = or.Cost();
            dto.NumberOfNotVisitedFamilies = or.NumberOfNotVisitedFamilies();
            dto.NumberOfAdditionalSantas = or.NumberOfAdditionalSantas();
            dto.AdditionalSantaWorkTime = or.AdditionalSantaWorkTime();
            dto.VisitTimeInUnavailabe = or.VisitTimeInUnavailabe();
            dto.VisitTimeInDesired = or.VisitTimeInDesired();
            dto.SantaWorkTime = or.SantaWorkTime();
            dto.LongestDay = or.LongestDay();
            dto.NumberOfNeededSantas = or.NumberOfNeededSantas();
            dto.NumberOfRoutes = or.NumberOfRoutes();
            dto.TotalWayTime = or.TotalWayTime();
            dto.TotalVisitTime = or.TotalVisitTime();
            dto.AverageWayTimePerRoute = or.AverageWayTimePerRoute();
            dto.LatestVisit = or.Routes.SelectMany(r => r.Waypoints.Where(wp => wp.VisitId != -1)).
                Select(wp => routeCalculationResult.ConvertTime(wp.StartTime)).
                Max();
            return dto;
        }
    }
}