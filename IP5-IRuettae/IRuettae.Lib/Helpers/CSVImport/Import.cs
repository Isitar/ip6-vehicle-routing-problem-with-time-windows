using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.Lib.Helpers.CSVImport
{
    public class Import
    {
        /// <summary>
        /// Number cols each line should have
        /// </summary>
        private const int NumberCols = 8;

        /// <summary>
        /// Path to the CSV-File which should be imported
        /// </summary>
        /// <param name="path"></param>
        public Import(string path)
        {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public string Path { get; set; }
        public List<ImportModel> Result { get; } = new List<ImportModel>();

        /// <summary>
        /// Starts the Import
        /// The Result is in the property with the same name
        /// </summary>
        /// <returns>Number records imported</returns>
        public int Start()
        {
            var csvData = File.ReadAllLines(Path);
            foreach (string row in csvData.Skip(1).Where(s => !string.IsNullOrEmpty(s)))
            {
                var cells = row.Split(';');
                var model = FromCells(cells);
                if (string.IsNullOrEmpty(cells[0]) && Result.Count > 0)
                {
                    // Additional periods (desired/unvailable)
                    Result.Last().Merge(model);
                }
                else
                {
                    // New record
                    Result.Add(model);
                }

                Result.Last().DeleteEmptyPeriods();
            }

            // Remove empty records
            Result.RemoveAll(x => string.IsNullOrEmpty(x.ID));

            return Result.Count;
        }

        public static ImportModel FromCells(string[] cells)
        {
            ImportModel model = new ImportModel();

            if (cells != null || cells.Length == NumberCols)
            {
                model.ID = cells[0];
                model.Street = cells[1];

                model.PLZ = TryParseInt(cells[2]);
                model.Childrean = TryParseInt(cells[3]);
                model.Desired = new List<Period> { TryParsePeriod(cells[4], cells[5]) };
                model.Unavailable = new List<Period> { TryParsePeriod(cells[6], cells[7]) };
            }

            return model;
        }

        public static int TryParseInt(string s)
        {
            int temp;
            int.TryParse(s, out temp);
            return temp;
        }

        public static Period TryParsePeriod(string s1, string s2)
        {
            // TODO what if one time is empty? set default
            DateTime from;
            DateTime.TryParse(s1, out from);
            DateTime to;
            DateTime.TryParse(s2, out to);
            return new Period(from, to);
        }
    }
}
