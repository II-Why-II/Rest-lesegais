using GraphQL;
using GraphQL.Client.Http;
using System;
using System.Threading.Tasks;

namespace Parser_Lesegais_ru.Parser.MakeQuery.GraphQl
{
    class SendRequestViaGraphQL
    {
        public TheSearchReportWoodDeal.Content[] GetContent(int sizeOfPage, int sizeOfQuery) => MakeRequestAndGetContentOrNull(sizeOfPage, sizeOfQuery).Result;

        private GraphQLRequest GraphQuery(int numberOfPage, int sizeOfRequest)
        {
            GraphQLRequest querySearchReportWoodDeal = new GraphQLRequest
            {
                Query = @"query SearchReportWoodDeal($size: Int!, $number: Int!) {
                   searchReportWoodDeal(filter: null, pageable: {number: $number, size: $size}, orders: null) {
                     content {
                       sellerName
                       sellerInn
                       buyerName
                       buyerInn
                       woodVolumeBuyer
                       woodVolumeSeller
                       dealDate
                       dealNumber
                       __typename
                     }
                     __typename  }
                 }
                 ",
                Variables = new { size = sizeOfRequest, number = numberOfPage },
                OperationName = "SearchReportWoodDeal"
            };
            return querySearchReportWoodDeal;
        }
        private async Task<TheSearchReportWoodDeal.Content[]> MakeRequestAndGetContentOrNull(int sizeOfPage, int sizeOfQuery)
        {
            try
            {
                GraphQLHttpClient graphQLClient = new GraphQLHttpClient("https://www.lesegais.ru/open-area/graphql", new GraphQL.Client.Serializer.SystemTextJson.SystemTextJsonSerializer()); // new NewtonsoftJsonSerializer());

                //System.Uri uri = new System.Uri("https://www.lesegais.ru");
                //graphQLClient.HttpClient.BaseAddress = uri;
                graphQLClient.HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                //graphQLClient.HttpClient.DefaultRequestHeaders.Add("Host", "www.lesegais.ru:443");
                //graphQLClient.HttpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                graphQLClient.HttpClient.DefaultRequestHeaders.UserAgent.Clear();
                graphQLClient.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36");
                graphQLClient.HttpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                graphQLClient.HttpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

                _ = 1;
                var graphQLResponse = await graphQLClient.SendQueryAsync<TheSearchReportWoodDeal.Welcome>(GraphQuery(sizeOfPage, sizeOfQuery));

                _ = 1;
                var data = graphQLResponse.Data;
                var contents = graphQLResponse.Data.Data.SearchReportWoodDeal.Content;

                _ = 1;
                return contents;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex); 
            }
            return null;
        }
    }
}
