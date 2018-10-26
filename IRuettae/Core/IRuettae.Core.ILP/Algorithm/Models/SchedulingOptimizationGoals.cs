using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.ILP.Algorithm.Models
{
    public enum SchedulingOptimizationGoals
    {
        Default, // minimises time,
        OldDefault, // minimises time, early and maximises desired
        MinTimeOnly, // only minimises time
        TryDesiredOnly, // only minimises try desired
    }
}
