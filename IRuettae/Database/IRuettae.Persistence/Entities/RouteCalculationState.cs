using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public enum RouteCalculationState
    {
        Creating,
        Ready,
        Running,
        Cancelled,
        Finished
    }
}
