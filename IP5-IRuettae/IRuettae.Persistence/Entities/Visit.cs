using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class Visit : BaseEntity
    {
        virtual public int Year { get; set; }
        public string Street { get; set; }
        public int Zip { get; set; }
        virtual public int NumberOfChildrean { get; set; }
        virtual public IList<Period> Desired { get; set; }
        virtual public IList<Period> Unavailable { get; set; }
    }
}
