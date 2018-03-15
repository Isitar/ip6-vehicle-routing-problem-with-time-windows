using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class Period : BaseEntity
    {
        public virtual DateTime? Start { get; set; }
        public virtual DateTime? End { get; set; }
    }
}
