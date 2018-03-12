using System;

namespace IRuettae.Preprocessing.CSVImport
{
    public struct Period
    {
        public Period(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public readonly DateTime From;
        public readonly DateTime To;

        public static bool IsValid(Period p)
        {
            return p.From < p.To && (p.From != DateTime.MinValue || p.To != DateTime.MinValue);
        }
    }
}
