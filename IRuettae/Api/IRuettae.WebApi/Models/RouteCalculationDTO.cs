using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Core.Models;
using IRuettae.Persistence.Entities;
using Newtonsoft.Json;

namespace IRuettae.WebApi.Models
{
    public class RouteCalculationDTO
    {
        public virtual long Id { get; set; }

        // starter Properties
        public virtual DateTime StartTime { get; set; }
        public virtual int Year { get; set; }
        public virtual List<(DateTime, DateTime)> Days { get; set; }
        public virtual int MaxNumberOfAdditionalSantas { get; set; }
        public virtual int TimePerChildMinutes { get; set; }
        public virtual int TimePerChildOffsetMinutes { get; set; }
        public virtual int StarterVisitId { get; set; }
        public virtual long TimeLimitMiliseconds { get; set; }


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
        public virtual int NumberOfMissingBreaks { get; set; }
        public virtual int NumberOfAdditionalSantas { get; set; }
        public virtual int AdditionalSantaWorkTime { get; set; }
        public virtual int VisitTimeInUnavailable { get; set; }
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
            var dto = new RouteCalculationDTO
            {
                Id = rc.Id,
                StartTime = rc.StartTime,
                Year = rc.Year,
                Days = rc.Days,
                MaxNumberOfAdditionalSantas = rc.MaxNumberOfAdditionalSantas,
                TimePerChildMinutes = rc.TimePerChildMinutes,
                TimePerChildOffsetMinutes = rc.TimePerChildOffsetMinutes,
                StarterVisitId = rc.StarterVisitId,
                TimeLimitMiliseconds = rc.TimeLimitMiliseconds,
                NumberOfSantas = rc.NumberOfSantas,
                NumberOfVisits = rc.NumberOfVisits,
                Algorithm = rc.Algorithm,
                State = rc.State,
                Progress = rc.Progress,
                StateText = rc.StateText,
                EndTime = rc.EndTime,
            };

            if (rc.Result != null)
            {
                var routeCalculationResult = JsonConvert.DeserializeObject<RouteCalculationResult>(rc.Result);
                var or = routeCalculationResult.OptimizationResult;
                if (or != null)
                {
                    // metrics
                    dto.Cost = or.Cost();
                    dto.NumberOfNotVisitedFamilies = or.NumberOfNotVisitedFamilies();
                    dto.NumberOfMissingBreaks = or.NumberOfMissingBreaks();
                    dto.NumberOfAdditionalSantas = or.NumberOfAdditionalSantas();
                    dto.AdditionalSantaWorkTime = or.AdditionalSantaWorkTime();
                    dto.VisitTimeInUnavailable = or.VisitTimeInUnavailable();
                    dto.VisitTimeInDesired = or.VisitTimeInDesired();
                    dto.SantaWorkTime = or.SantaWorkTime();
                    dto.LongestDay = or.LongestDay();
                    dto.NumberOfNeededSantas = or.NumberOfNeededSantas();
                    dto.NumberOfRoutes = or.NumberOfRoutes();
                    dto.TotalWayTime = or.TotalWayTime();
                    dto.TotalVisitTime = or.TotalVisitTime();
                    dto.AverageWayTimePerRoute = or.AverageWayTimePerRoute();
                    dto.LatestVisit = or.Routes.SelectMany(r => r.Waypoints
                        .Where(wp => wp.VisitId != Constants.VisitIdHome))
                        .Select(wp => routeCalculationResult.ConvertTime(wp.StartTime))
                        .Append(DateTime.MinValue)
                        .OrderBy(t => t - t.TimeOfDay).Last();
                    dto.AverageDurationPerRoute = or.AverageDurationPerRoute();
                }
            }
            return dto;
        }
    }
}