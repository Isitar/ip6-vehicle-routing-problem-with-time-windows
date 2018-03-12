using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Preprocessing.Helpers.CSVImport
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class ImportModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public ImportModel()
        {
        }

        public ImportModel(string iD, string street, int zip, int numberOfChildren, List<Period> desired, List<Period> unavailable)
        {
            ID = iD;
            Street = street;
            Zip = zip;
            NumberOfChildren = numberOfChildren;
            Desired = desired;
            Unavailable = unavailable;
        }

        public string ID { get; set; }
        public string Street { get; set; }
        public int Zip { get; set; }
        public int NumberOfChildren { get; set; }
        public List<Period> Desired { get; set; }
        public List<Period> Unavailable { get; set; }

        /// <summary>
        /// Merges the consumable into this model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="consumable"></param>
        public void Merge(ImportModel consumable)
        {
            if (Desired == null)
            {
                Desired = consumable.Desired;
            }
            if (Unavailable == null)
            {
                Unavailable = consumable.Unavailable;
            }

            if (consumable.Desired != null)
            {
                Desired.AddRange(consumable.Desired);
            }
            if (consumable.Unavailable != null)
            {
                Unavailable.AddRange(consumable.Unavailable);
            }
        }

        public void DeleteEmptyPeriods()
        {
            Desired?.RemoveAll(x => !Period.IsValid(x));
            Unavailable?.RemoveAll(x => !Period.IsValid(x));
        }

        public override bool Equals(object obj)
        {
            return obj is ImportModel model &&
                   ID == model.ID &&
                   Street == model.Street &&
                   Zip == model.Zip &&
                   NumberOfChildren == model.NumberOfChildren &&
                   Desired.SequenceEqual(model.Desired) &&
                   Unavailable.SequenceEqual(model.Unavailable);
        }
    }
}
