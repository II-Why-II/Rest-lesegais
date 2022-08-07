using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TheRearchReportWoodDealCounter
{
    public partial class Welcome
    {
        public Data Data { get; set; }
    }

    public partial class Data
    {
        public SearchReportWoodDeal SearchReportWoodDeal { get; set; }
    }

    public partial class SearchReportWoodDeal
    {
        public int Total { get; set; }
        public long Number { get; set; }
        public long Size { get; set; }
        public double OverallBuyerVolume { get; set; }
        public double OverallSellerVolume { get; set; }
        public string Typename { get; set; }
    }

    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, TheRearchReportWoodDealCounter.Converter.Settings);
    }
    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, TheRearchReportWoodDealCounter.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
