using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru.Parser
{
    class Worker
    {
        public int TimeSpanInMinutes = 10;
        public void Program()
        {
            bool needWork = true;
            while (needWork)
            {
                Starter.StarterParse starter = new Starter.StarterParse();
                starter.BigParseInfo();

                int timeSpanInMilliseconds = TimeSpanInMinutes * 60 * 1000;

                Thread.Sleep(timeSpanInMilliseconds);
            }
        }
    }
}
