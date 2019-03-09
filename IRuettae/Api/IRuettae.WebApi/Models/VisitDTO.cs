using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Persistence.Entities;

namespace IRuettae.WebApi.Models
{
    /// <summary>
    /// Used by REST-Api for get requests
    /// </summary>
    public class VisitDTO 
    {
        public virtual long Id { get; set; }
        public virtual string ExternalReference { get; set; }
        public virtual int Year { get; set; }
        public virtual string Street { get; set; }
        public virtual string OriginalStreet { get; set; }
        public virtual int Zip { get; set; }
        public virtual string City { get; set; }
        public virtual int NumberOfChildren { get; set; }
        public virtual IList<Period> Desired { get; set; }
        public virtual IList<Period> Unavailable { get; set; }
        public virtual double Lat { get; set; }
        public virtual double Long { get; set; }
        public virtual VisitType VisitType { get; set; }

        public virtual double Duration { get; set; }

        public static explicit operator VisitDTO(Visit v)
        {
            return new VisitDTO
            {
                Id = v.Id,
                Desired = v.Desired.ToList(),
                Street = v.Street,
                OriginalStreet = v.OriginalStreet,
                Zip = v.Zip,
                City = v.City,
                ExternalReference = v.ExternalReference,
                Year = v.Year,
                NumberOfChildren = v.NumberOfChildren,
                Unavailable = v.Unavailable.ToList(),
                Duration = v.Duration,
                VisitType = v.VisitType,
                Lat = v.Lat,
                Long = v.Long,
            };
        }
    }
}