using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm
{
    enum TargetType
    {
        ShortestRoute, // the routes should be as short as possible
        MinSantas, // the number of santas needed overall should be minimised
        MinSantaShifts, // the number of santas needed each day should be minimised
    }
}
