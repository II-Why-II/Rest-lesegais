using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Parser_Lesegais_ru.Parser.MakeQuery.HttpRequests
{
    class SendRequestWithHttpWebRequest
    {
        public TheSearchReportWoodDeal.Content[] getWoodDealContentsOrNull(int page, int sizeOneRequest)
        {
            try
            {
                string responseString = GetWoodDealStringsOrNull(page, sizeOneRequest);
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

        public int? GetMaxPageOrNull(int sizeOneRequest)
        {
            try
            {
                var woodDealCounterString = GetWoodDealCounterStringOrNull(sizeOneRequest);
                if (woodDealCounterString != null)
                {
                    var counterData = TheRearchReportWoodDealCounter.Welcome.FromJson(woodDealCounterString);

                    var counterOfPages = counterData.Data.SearchReportWoodDeal.Total / sizeOneRequest;

                    var result = Convert.ToInt32(counterOfPages.ToString().Split(',').First());

                    var r = counterData.Data.SearchReportWoodDeal.Total % sizeOneRequest;

                    _ = 1;
                    if (r != 0)
                        return result + 1;
                    else
                        return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }

        private string GetWoodDealStringsOrNull(int numberOfPage, int size)
        {
            var postDataAboutWoodDeal = "{\"query\":\"\\n{\\n    searchReportWoodDeal(filter: null, pageable: {" + $"number: {numberOfPage}, size: {size}" + "}, orders: null)\\n      {\\n        content \\n        {\\n          sellerName\\n          sellerInn\\n          buyerName\\n          buyerInn\\n          woodVolumeBuyer\\n          woodVolumeSeller\\n          dealDate\\n          dealNumber\\n          __typename\\n        }\\n        __typename\\n      }\\n}\",\"variables\":{},\"operationName\":null}";
            return getResponseStringOrNull(postDataAboutWoodDeal);
        }
        private string GetWoodDealCounterStringOrNull(int size)
        {
            var postDataAboutWoodDealCounter = "{\"query\":\"query SearchReportWoodDealCount($size: Int!, $number: Int!, $filter: Filter, $orders: [Order!]) {\\n  searchReportWoodDeal(filter: $filter, pageable: {number: $number, size: $size}, orders: $orders) {\\n    total\\n    number\\n    size\\n    overallBuyerVolume\\n    overallSellerVolume\\n    __typename\\n  }\\n}\\n\",\"variables\":{\"size\":" + $"{size}" + ",\"number\":0,\"filter\":null},\"operationName\":\"SearchReportWoodDealCount\"}";
            return getResponseStringOrNull(postDataAboutWoodDealCounter);
        }

        private string getResponseStringOrNull(string postData)
        {
            try
            {
                var req = (HttpWebRequest)HttpWebRequest.Create("https://www.lesegais.ru/open-area/graphql");
                var data = Encoding.ASCII.GetBytes(postData);


                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AlwaysGoodCertificate);


                req.Method = "POST";
                req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                req.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.Default);
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
                req.Accept = "text/html, application/xhtml+xml, */*";
                req.Headers.Add("Accept-Encoding: gzip, deflate");
                req.Headers.Add("Accept-Language: ru-RU");
                req.Headers.Add("DNT: 1");
                req.SetRawHeader("content-type", "application/json");
                req.ContentLength = data.Length;

                //WebProxy pr = new WebProxy("127.0.0.1:8888");
                //req.Proxy = pr;

                using (var stream = req.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)req.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                response.Close();

                return responseString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        private static bool AlwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

    }

    public static class HttpWebRequestExtensions
    {
        static string[] RestrictedHeaders = new string[] {
            "Accept",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Date",
            "Expect",
            "Host",
            "If-Modified-Since",
            "Keep-Alive",
            "Proxy-Connection",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "User-Agent"
        };

        static Dictionary<string, PropertyInfo> HeaderProperties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

        static HttpWebRequestExtensions()
        {
            Type type = typeof(HttpWebRequest);
            foreach (string header in RestrictedHeaders)
            {
                string propertyName = header.Replace("-", "");
                PropertyInfo headerProperty = type.GetProperty(propertyName);
                HeaderProperties[header] = headerProperty;
            }
        }

        public static void SetRawHeader(this HttpWebRequest request, string name, string value)
        {
            if (HeaderProperties.ContainsKey(name))
            {
                PropertyInfo property = HeaderProperties[name];
                if (property.PropertyType == typeof(DateTime))
                    property.SetValue(request, DateTime.Parse(value), null);
                else if (property.PropertyType == typeof(bool))
                    property.SetValue(request, Boolean.Parse(value), null);
                else if (property.PropertyType == typeof(long))
                    property.SetValue(request, Int64.Parse(value), null);
                else
                    property.SetValue(request, value, null);
            }
            else
            {
                request.Headers[name] = value;
            }
        }
    }
}