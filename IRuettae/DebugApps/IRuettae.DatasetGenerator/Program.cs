using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.DatasetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine(new DatasetGenerator(4000, 4000, 10, 2, 1,new []{2,0},new []{2,2}).Generate("Dataset 1"));
            Console.ReadLine();
        }
    }
}
