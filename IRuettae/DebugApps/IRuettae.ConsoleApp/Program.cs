using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Test.Run(args);
            TestILPClustering.Test();
            Console.WriteLine("Program has finished, press enter to close it.");
            Console.ReadLine();
        }
    }
}
