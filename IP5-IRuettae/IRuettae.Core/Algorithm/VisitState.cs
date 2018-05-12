using System;

namespace IRuettae.Core.Algorithm
{
    public enum VisitState
    {
        Default, // no preference, should be available
        NotAvailable,
    }

    internal static class Extensions
    {

        public static bool IsAvailable(this VisitState s)
        {
            switch (s)
            {
                case VisitState.Default:
                    return true;
                case VisitState.NotAvailable:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }
    }
}
