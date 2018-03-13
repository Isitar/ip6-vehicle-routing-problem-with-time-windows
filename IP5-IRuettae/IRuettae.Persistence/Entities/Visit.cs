using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class Visit : BaseEntity
    {
        public virtual int Year { get; set; }
        public virtual string Street { get; set; }
        public virtual int Zip { get; set; }
        public virtual int NumberOfChildrean { get; set; }
        public virtual IList<Period> Desired { get; set; }
        public virtual IList<Period> Unavailable { get; set; }
    }
}
