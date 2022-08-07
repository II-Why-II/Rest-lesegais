using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Worker worker = new Parser.Worker();
            worker.Program();
        }
    }
}
