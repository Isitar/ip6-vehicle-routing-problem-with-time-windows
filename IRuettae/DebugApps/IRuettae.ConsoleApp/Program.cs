using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.ConsoleApp.Programs;

namespace IRuettae.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //CreateManualSolution.Run(args);
            //Console.WriteLine("Program has finished, press enter to close it.");
            new DataSet8Helper().FillRemainingWays();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
