using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core.Algorithm.GoogleORTools.TargetFunctionBuilders
{
    enum TargetType
    {
        MinTime, // the overall time should be minimised
        MinSantas, // the number of santas needed overall should be minimised
        MinSantaShifts, // the number of santas needed each day should be minimised
    }
}
