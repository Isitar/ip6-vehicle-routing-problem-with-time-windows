using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRuettae.Persistence.Entities;

namespace IRuettae.WebApi.Models
{
    public class SantaDTO
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<VisitDTO> Breaks { get; set; }

        public static explicit operator SantaDTO(Santa s)
        {
            return new SantaDTO()
            {
                Id = s.Id,
                Name = s.Name,
                Breaks = s.Breaks.Select(b => (VisitDTO)b).ToList()
            };
        }
    }
}