using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.GeoCalculations.RouteCalculation;
using IRuettae.Persistence.Entities;

namespace IRuettae.Preprocessing.CSVImport
{
    public class Converter
    {
        public static IEnumerable<Visit> ToDatabase(IEnumerable<ImportModel> models)
        {
            return models.Select(x => ImportModelToVisit(x));
        }

        private static Visit ImportModelToVisit(ImportModel model)
        {
            DateTime? toNullable(DateTime x) => (DateTime.MinValue == x) ? (DateTime?)null : x;
            Persistence.Entities.Period p2p(Period x)
            {
                return new Persistence.Entities.Period() { Start = toNullable(x.From), End = toNullable(x.To) };
            }
            int tryGetYear(String id)
            {
                const int lengthYear = 4;
                int year = DateTime.Now.Year;
                if (id.Length >= lengthYear)
                {
                    Int32.TryParse(id.Substring(0, 4), out year);
                }
                return year;
            }

            return new Visit()
            {
                ExternalReference = model.Id,
                Year = tryGetYear(model.Id),
                Street = model.Street,
                Zip = model.Zip,
                NumberOfChildren = model.NumberOfChildren,
                Desired = model.Desired.Select(p2p).ToList(),
                Unavailable = model.Unavailable.Select(p2p).ToList(),
            };
        }

        // TODO MEYERJ
        /*
        public static void GenerateWays(Visit fromVisit)
        {
            var calculator = new GoogleRouteCalculator("");
            Func<Visit, string> getAdress = x => x.Zip + " " + x.Street;
            var fromAdress = getAdress(fromVisit);

            // TODO MEYERJ query here
            var visits = new List<Visit>();
            visits.ForEach(x =>
            {
                var resFrom = calculator.CalculateWalkingDistance(fromAdress, getAdress(x));
                fromVisit.FromWays.Add(new Way() { From = fromVisit, To = x, Distance = Convert.ToInt32(resFrom.distance), Duration = Convert.ToInt32(resFrom.duration) });
                fromVisit.ToWays.Add(FromApiResult(fromVisit, x, resFrom));
                var resTo = calculator.CalculateWalkingDistance(getAdress(x), fromAdress);
                fromVisit.ToWays.Add(FromApiResult(x, fromVisit, resTo));
            });
        }

        private static Way FromApiResult(Visit from, Visit to, (double distance, double duration) res)
        {
            return new Way() { From = from, To = to, Distance = Convert.ToInt32(res.distance), Duration = Convert.ToInt32(res.duration) };
        }
        */
    }


}
