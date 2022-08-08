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
            try
            {
                Parser.Worker worker = new Parser.Worker();
                worker.Program();
            }
            catch(Exception ex)
            {
                Console.WriteLine("error in main: " + ex.Message);
                throw;
            }
        }
    }
}
