using System;

namespace IRuettae.Core.ILP.Algorithm
{
    public enum VisitState
    {
        Default, // no preference, should be available
        Desired,
        Unavailable,
    }

    internal static class Extensions
    {

        public static bool IsAvailable(this VisitState s)
        {
            switch (s)
            {
                case VisitState.Default:
                    return true;
                case VisitState.Desired:
                    return true;
                case VisitState.Unavailable:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, "unexpected");
            }
        }
    }
}
