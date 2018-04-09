using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class Santa : BaseEntity
    {
        public virtual string Name { get; set; }
        public virtual IList<Visit> Breaks { get; set; }

        public Santa()
        {
            Breaks = new List<Visit>();
        }
    }
}
