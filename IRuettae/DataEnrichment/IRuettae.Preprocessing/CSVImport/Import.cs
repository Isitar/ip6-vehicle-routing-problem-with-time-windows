using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IRuettae.Preprocessing.CSVImport
{
    public class Import
    {
        /// <summary>
        /// Number cols each line should have
        /// </summary>
        private const int NumberCols = 8;

        /// <summary>
        /// Starts the Import
        /// The Result is in the property with the same name
        /// </summary>
        /// <param name="path">Path to the CSV-file which should be imported</param>
        /// <returns>Number records imported</returns>
        public static IEnumerable<ImportModel> StartImportFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var Result = new List<ImportModel>();

            return StartImport(File.ReadAllText(path));
        }

        /// <summary>
        /// Starts the Import
        /// The Result is in the property with the same name
        /// </summary>
        /// <param name="path">Path to the CSV-file which should be imported</param>
        /// <returns>Number records imported</returns>
        public static IEnumerable<ImportModel> StartImport(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var Result = new List<ImportModel>();

            var csvData = Regex.Split(content, "\r\n|\r|\n");

            foreach (string row in csvData.Skip(1).Where(s => !string.IsNullOrEmpty(s)))
            {
                var cells = row.Split(';');
                var model = FromCells(cells);
                model.DeleteEmptyPeriods();
                if (string.IsNullOrEmpty(model.Id) && Result.Count > 0)
                {
                    // Additional periods (desired/unvailable)
                    Result.Last().Merge(model);
                }
                else
                {
                    // New record
                    Result.Add(model);
                }
            }

            // Remove empty records
            Result.RemoveAll(x => string.IsNullOrEmpty(x.Id));

            return Result;
        }

        private static ImportModel FromCells(string[] cells)
        {
            if (cells != null && cells.Length == NumberCols)
            {
                return new ImportModel
                {
                    Id = cells[0],
                    Street = cells[1],
                    Zip = TryParseInt(cells[2]),
                    NumberOfChildren = TryParseInt(cells[3]),
                    Desired = new List<Period> { TryParsePeriod(cells[4], cells[5]) },
                    Unavailable = new List<Period> { TryParsePeriod(cells[6], cells[7]) }
                };
            }

            return new ImportModel();
        }

        private static int TryParseInt(string s)
        {
            int.TryParse(s, out var temp);
            return temp;
        }

        private static Period TryParsePeriod(string s1, string s2)
        {
            var culture = CultureInfo.CreateSpecificCulture("DE-ch");
            const DateTimeStyles styles = DateTimeStyles.None;

            DateTime.TryParse(s1, culture, styles, out var from);
            DateTime.TryParse(s2, culture, styles, out var to);

            return new Period(from, to);
        }
    }
}
