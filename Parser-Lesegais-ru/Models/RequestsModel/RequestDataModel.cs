using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru.Models.RequestsModel
{
    class StartPageRequestDataModel
    {
        public int PageSize { get; set; }
        public int NimberOfPage { get; set; }
        public string Filter { get; set; }
    }
    class RequestDataModel
    {
        public int PageSize { get; set; }
        public int NimberOfPage { get; set; }
        public string Filter { get; set; }
        public string Orders { get; set; }
    }
}
