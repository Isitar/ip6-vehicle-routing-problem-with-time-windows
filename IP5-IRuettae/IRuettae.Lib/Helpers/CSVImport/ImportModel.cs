using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Lib.Helpers.CSVImport
{
    public class ImportModel
    {
        public ImportModel()
        {
        }

        public ImportModel(string iD, string street, int pLZ, int childrean, List<Period> desired, List<Period> unavailable)
        {
            ID = iD;
            Street = street;
            PLZ = pLZ;
            Childrean = childrean;
            Desired = desired;
            Unavailable = unavailable;
        }

        public string ID { get; set; }
        public string Street { get; set; }
        public int PLZ { get; set; }
        public int Childrean { get; set; }
        public List<Period> Desired { get; set; }
        public List<Period> Unavailable { get; set; }

        /// <summary>
        /// Merges the consumable into this model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="consumable"></param>
        public void Merge(ImportModel consumable)
        {
            Desired.AddRange(consumable.Desired);
            Unavailable.AddRange(consumable.Unavailable);
        }

        public void DeleteEmptyPeriods()
        {
            Desired.RemoveAll(x => !Period.IsValid(x));
            Unavailable.RemoveAll(x => !Period.IsValid(x));
        }

        public override bool Equals(object obj)
        {
            var model = obj as ImportModel;
            return model != null &&
                   ID == model.ID &&
                   Street == model.Street &&
                   PLZ == model.PLZ &&
                   Childrean == model.Childrean &&
                   Desired.SequenceEqual(model.Desired) &&
                   Unavailable.SequenceEqual(model.Unavailable);
        }
    }
}
