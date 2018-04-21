using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    public enum VisitState
    {
        Default, // no preference, should be available
        NotAvailable,
        Prefered,
    }
}
