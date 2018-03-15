using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Persistence.Entities
{
    public class Visit : BaseEntity
    {
        /// <summary>
        /// ExternalReference to connect this visit with other data saved in another system
        /// (eg. names of the childrean, notes, favorite meal etc.)
        /// </summary>
        public virtual string ExternalReference { get; set; }
        public virtual int Year { get; set; }
        public virtual string Street { get; set; }
        public virtual int Zip { get; set; }
        public virtual int NumberOfChildrean { get; set; }
        public virtual IList<Period> Desired { get; set; }
        public virtual IList<Period> Unavailable { get; set; }

        public Visit()
        {
            Desired = new List<Period>();
            Unavailable = new List<Period>();
        }

    }
}
