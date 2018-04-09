using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    /// <summary>
    /// Represents the different visit types for visit
    /// </summary>
    public enum VisitTypes
    {
        // !!! ATTENTION !!!
        // If you rename these entities, you need to update the db since enum values are saved as string.

        Visit,
        Break,
    }
}
