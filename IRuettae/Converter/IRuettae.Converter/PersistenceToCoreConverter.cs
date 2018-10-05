using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;
using Visit = IRuettae.Persistence.Entities.Visit;

namespace IRuettae.Converter
{
    public class PersistenceToCoreConverter
    {
        /// <summary>
        /// Converts the input params to an OptimisationInput
        /// </summary>
        /// <param name="visits">All visits for the problem</param>
        /// <param name="santas">All santas for the problem</param>
        /// <param name="breaks">All breaks for the problem</param>
        /// <returns>An optimisation input that can be used to solve the problem</returns>
        public OptimisationInput Convert(List<Visit> visits, List<Santa> santas, List<Visit> breaks)
        {
            throw new NotImplementedException();
        }
    }
}
