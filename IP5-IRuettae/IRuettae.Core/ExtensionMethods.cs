using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Core
{
    internal static class ExtensionMethods
    {
        internal static int CeilDivision(this int top, int bottom)
        {
            return (top + bottom - 1) / bottom;
        }
    }
}
