using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Lib.Helpers.CSVImport
{
    public class Period
    {
        public Period(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public DateTime From { get; }
        public DateTime To { get; }

        public static bool IsValid(Period p)
        {
            return p != null && p.From != null && p.To != null
                && p.From != DateTime.MinValue && p.To != DateTime.MinValue;
        }

        public override bool Equals(object obj)
        {
            var period = obj as Period;
            return period != null &&
                   From == period.From &&
                   To == period.To;
        }

        /// <summary>
        /// Auto-generated
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = -1781160927;
            hashCode = hashCode * -1521134295 + From.GetHashCode();
            hashCode = hashCode * -1521134295 + To.GetHashCode();
            return hashCode;
        }
    }
}
