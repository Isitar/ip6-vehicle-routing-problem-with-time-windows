using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Lib.Helpers.CSVImport;

namespace IRuettae.ConsoleApp
{
    class ImportCSV
    {
        internal static void Run(string[] args)
        {
            // Change path here
            Import import = new Import(@"C:\Users\Janik\Desktop\Vorlage Datenerfassung.csv");
            import.Start();
        }
    }
}
