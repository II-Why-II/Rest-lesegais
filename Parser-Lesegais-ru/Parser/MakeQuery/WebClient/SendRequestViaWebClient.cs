using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru.Parser.MakeQuery.WebClient
{
    class SendRequestViaWebClient
    {
        public TheSearchReportWoodDeal.Content[] getWoodDealContentsOrNull(int page, int sizeOneRequest)
        {
            try
            {
                string responseString = GetStringByMakeResponce(page, sizeOneRequest);
                if (responseString != null)
                {
                    var data = TheSearchReportWoodDeal.Welcome.FromJson(responseString);

                    return data.Data.SearchReportWoodDeal.Content;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private string GetStringByMakeResponce(int numberOfPage, int sizeOfRequest)
        {
            var postDataAboutWoodDeal = "{\"query\":\"\\n{\\n    searchReportWoodDeal(filter: null, pageable: {" + $"number: {numberOfPage}, size: {sizeOfRequest}" + "}, orders: null)\\n      {\\n        content \\n        {\\n          sellerName\\n          sellerInn\\n          buyerName\\n          buyerInn\\n          woodVolumeBuyer\\n          woodVolumeSeller\\n          dealDate\\n          dealNumber\\n          __typename\\n        }\\n        __typename\\n      }\\n}\",\"variables\":{},\"operationName\":null}";
            return SendRequestAndReturnResult("https://www.lesegais.ru/open-area/graphql", postDataAboutWoodDeal);
        }

        private string SendRequestAndReturnResult(string url, string postData)
        {
            using (CreateWebClientWithHeaders webClient = new CreateWebClientWithHeaders())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AlwaysGoodCertificate);


                webClient.Headers.Add("Accept", "application/json"); //"*/*";
                webClient.Headers.Add("ContentType", "application/json"); //"application/x-www-form-urlencoded"; //
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
                webClient.Headers.Add("Origin", "chrome-extension://flnheeellpciglgpaodhkhmapeljopja");
                webClient.Headers.Add("Sec-Fetch-Site", "none"); //"same-origin"
                webClient.Headers.Add("Sec-Fetch-Mode", "cors");
                webClient.Headers.Add("Sec-Fetch-Dest", "empty");
                webClient.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
                webClient.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, Deflate";
                

                var responce = webClient.UploadString(url, "POST", postData);
                var result = responceDecoder(responce);

                _ = 1;
                webClient.Dispose();

                return result;
            }
        }
        private static string responceDecoder(string text)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(text);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            string result = win1251.GetString(win1251Bytes);
            return result;
        }
        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        private static bool AlwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
}
