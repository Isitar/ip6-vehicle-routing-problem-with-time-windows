using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Preprocessing.Helpers.CSVImport;

namespace IRuettae.ConsoleApp
{
    class ImportCSV
    {
        internal static void Run(string[] args)
        {
            // Change path here
            var result = Import.StartImport(@"C:\Users\Janik\Desktop\Vorlage Datenerfassung.csv");
        }
    }
}
