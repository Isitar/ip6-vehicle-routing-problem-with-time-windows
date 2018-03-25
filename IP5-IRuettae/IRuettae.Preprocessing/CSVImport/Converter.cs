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
        public static IEnumerable<Visit> ToDatabase(IEnumerable<ImportModel> models, int? year = null)
        {
            return models.Select(x => ImportModelToVisit(x, year));
        }

        private static Visit ImportModelToVisit(ImportModel model, int? year)
        {
            DateTime? toNullable(DateTime x) => (DateTime.MinValue == x) ? (DateTime?)null : x;
            Persistence.Entities.Period p2p(Period x)
            {
                return new Persistence.Entities.Period() { Start = toNullable(x.From), End = toNullable(x.To) };
            }
            int tryGetYear(String id)
            {
                const int lengthYear = 4;
                int y = year ?? DateTime.Now.Year;
                if (id.Length >= lengthYear)
                {
                    Int32.TryParse(id.Substring(0, 4), out y);
                }
                return y;
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
    }


}
