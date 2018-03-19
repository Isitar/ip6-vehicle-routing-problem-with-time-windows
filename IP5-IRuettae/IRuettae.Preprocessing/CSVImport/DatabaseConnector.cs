using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;

namespace IRuettae.Preprocessing.CSVImport
{
    class DatabaseConnector
    {
        public static void SaveToDatabase(IEnumerable<ImportModel> models, int year)
        {
            var visits = models.Select(x => ImportModelToVisit(x, year));

            // TODO MEYERJ insert here
        }

        private static Visit ImportModelToVisit(ImportModel model, int year)
        {
            Func<DateTime, DateTime?> toNullable = x => (DateTime.MinValue == x) ? (DateTime?)null : x;
            Func<Period, Persistence.Entities.Period> p2p = x =>
            {
                return new Persistence.Entities.Period() { Start = toNullable(x.From), End = toNullable(x.To) };
            };

            return new Visit()
            {
                ExternalReference = model.Id,
                Year = year,
                Street = model.Street,
                Zip = model.Zip,
                NumberOfChildrean = model.NumberOfChildren,
                Desired = model.Desired.Select(p2p).ToList(),
                Unavailable = model.Unavailable.Select(p2p).ToList(),
            };
        }

        public static void GenerateWays(Visit fromVisit)
        {
            var calculator = new GoogleRouteCalculator();
            Func<Visit, string> getAdress = x => x.Zip + " " + x.Street;
            var fromAdress = getAdress(fromVisit);

            // TODO MEYERJ query here
            var visits = new List<Visit>();
            visits.ForEach(x =>
            {
                var resFrom = calculator.CalculateWalkingDistance(fromAdress, getAdress(x));
                fromVisit.FromWays.Add(new Way() { From = fromVisit, To = x, Distance = resFrom.distance, Duration = resFrom.duration });
                fromVisit.ToWays.Add(FromApiResult(fromVisit, x, resFrom));
                var resTo = calculator.CalculateWalkingDistance(getAdress(x), fromAdress);
                fromVisit.ToWays.Add(FromApiResult(x, fromVisit, resTo));
            });
        }

        private static Way FromApiResult(Visit from, Visit to, (double distance, double duration) res)
        {
            return new Way() { From = from, To = to, Distance = res.distance, Duration = res.duration };
        }
    }


}
