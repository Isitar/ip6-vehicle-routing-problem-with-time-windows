using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class RouteCalculationLog : BaseEntity
    {
        public virtual string Log { get; set; }
        public virtual DateTime CreationDate { get; set; }

        public RouteCalculationLog()
        {
            CreationDate = DateTime.Now;
        }
    }
}
