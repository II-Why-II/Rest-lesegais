using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru.Parser.MakeQuery.WebClient
{
    class CreateWebClientWithHeaders : System.Net.WebClient
    {
        public CreateWebClientWithHeaders()
        {
            //AddingHeaders(); 
        }

        private void AddingHeaders()
        {
            //this.Proxy = proxy;
            this.Headers.Add("Accept", "application/json");
            this.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            this.Headers.Add("Content-Type", "application/json");
            this.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            this.Headers.Add("Host", "www.lesegais.ru");
            this.Headers.Add("Origin", "chrome-extension://flnheeellpciglgpaodhkhmapeljopja");
            this.Headers.Add("sec-ch-ua-mobile", "?0");
            this.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            this.Headers.Add("Sec-Fetch-Dest", "empty");
            this.Headers.Add("Sec-Fetch-Mode", "cors");
            this.Headers.Add("Sec-Setch-Site", "same-origin");
            this.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }

    }
}
