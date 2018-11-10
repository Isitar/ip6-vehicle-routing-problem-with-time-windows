using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRuettae.ConsoleApp
{
    public static class ConsoleExt
    {
        public static void WriteLine(string s, ConsoleColor c)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.WriteLine(s);
            Console.ForegroundColor = oldColor;
        }
    }
}
