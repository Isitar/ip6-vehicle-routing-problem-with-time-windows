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
        public virtual string City { get; set; }
        public virtual int NumberOfChildren { get; set; }
        public virtual IList<Period> Desired { get; set; }
        public virtual IList<Period> Unavailable { get; set; }
        public virtual IList<Way> FromWays { get; set; }
        public virtual IList<Way> ToWays { get; set; }
        public virtual int DeltaWayDistance { get; set; }
        public virtual int DeltaWayDuration { get; set; }
        public virtual VisitTypes VisitType { get; set; }

        public virtual Santa Santa { get; set; }
        public virtual double Duration { get; set; }

        public Visit()
        {
            Desired = new List<Period>();
            Unavailable = new List<Period>();
            VisitType = VisitTypes.Visit;
        }

    }
}
