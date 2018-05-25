using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using IRuettae.Core.Algorithm;
using IRuettae.Persistence.Entities;
using IRuettae.Preprocessing.Mapping;
using IRuettae.WebApi.Persistence;
using NHibernate;

namespace IRuettae.WebApi.Helpers
{
    public class Pipeline
    {
        private RouteCalculation routeCalculation;
        private readonly ISession dbSession;
        private BackgroundWorker bgWorker;


        public Pipeline(RouteCalculation routeCalculation, List<Visit> visits, List<Santa> santas)
        {
            this.routeCalculation = routeCalculation;
            this.dbSession = SessionFactory.Instance.OpenSession();

            SetupBgWorker(visits, santas);

        }

        private void SetupBgWorker(List<Visit> visits, List<Santa> santas)
        {
            bgWorker = new BackgroundWorker();
            routeCalculation = dbSession.Merge(routeCalculation);

            bgWorker.DoWork += (sender, args) =>
            {
                visits.ForEach(v => v.Duration = 60 * (v.NumberOfChildren * routeCalculation.TimePerChild +
                                       routeCalculation.TimePerChildOffset));

                // set starterId to front
                visits.Sort((a, b) =>
                {
                    if (a.Id == routeCalculation.StarterVisitId)
                    {
                        return -1;
                    }

                    if (b.Id == routeCalculation.StarterVisitId)
                    {
                        return 1;
                    }

                    return a.Id.CompareTo(b.Id);
                });


                var solverVariableBuilder = new SolverVariableBuilderClustering
                {
                    Visits = visits,
                    Santas = santas,
                    Days = routeCalculation.Days
                };

                var solverInputDataClustering = solverVariableBuilder.Build();
                var eventTextWriter = new EventTextWriter();
                var lastUpdate = DateTime.Now;


                eventTextWriter.CharWritten += (o, c) =>
                {
                    routeCalculation.StateText.Append(c);
                    if (DateTime.Now - lastUpdate > TimeSpan.FromMinutes(1))
                    {
                        lastUpdate = DateTime.Now;
                        dbSession.Update(routeCalculation);
                    }
                };
                Console.SetOut(eventTextWriter);

                routeCalculation.StartTime = DateTime.Now;

                routeCalculation.State = RouteCalculationState.RunningPhase1;
                dbSession.Update(routeCalculation);
                var phase1Result = Starter.Optimise(solverInputDataClustering, TargetBuilderType.Default, routeCalculation.ClustringMipGap);
                var clusteredRoutes = phase1Result.Waypoints
                    .Cast<List<Waypoint>>()
                    .Select(wp => wp.Aggregate("",
                        (carry, n) => carry + Environment.NewLine + solverInputDataClustering.VisitNames[n.visit]));

                routeCalculation.ClusteringResult = "";
                var sb = new StringBuilder();
                var ctr = 0;
                foreach (var route in clusteredRoutes)
                {
                    sb.AppendLine(ctr.ToString());
                    sb.AppendLine(route);
                }

                routeCalculation.ClusteringResult = sb.ToString();
                dbSession.Update(routeCalculation);
                routeCalculation.State = RouteCalculationState.RunningPhase2;
                dbSession.Update(routeCalculation);
                
            };
        }

        public void StartWorker()
        {
            bgWorker.RunWorkerAsync();
        }
    }
}