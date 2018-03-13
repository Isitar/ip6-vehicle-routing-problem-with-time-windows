using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class Period : BaseEntity
    {
        virtual public long VisitId { get; set; }
        virtual public DateTime? Start { get; set; }
        virtual public DateTime? End { get; set; }
    }
}
