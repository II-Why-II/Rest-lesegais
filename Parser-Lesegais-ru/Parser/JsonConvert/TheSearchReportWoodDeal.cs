using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TheSearchReportWoodDeal
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
        public Content[] Content { get; set; }
        public string Typename { get; set; }
    }

    public partial class Content
    {
        public string SellerName { get; set; }
        public string SellerInn { get; set; }
        public string BuyerName { get; set; }
        public string BuyerInn { get; set; }
        public double WoodVolumeBuyer { get; set; }
        public double WoodVolumeSeller { get; set; }
        public DateTimeOffset DealDate { get; set; }
        public string DealNumber { get; set; }
        public Typename Typename { get; set; }
    }

    public enum Typename { ReportWoodDeal };
    public partial class Welcome
    {
        public static Welcome FromJson(string json) => JsonConvert.DeserializeObject<Welcome>(json, TheSearchReportWoodDeal.Converter.Settings);
    }
    public static class Serialize
    {
        public static string ToJson(this Welcome self) => JsonConvert.SerializeObject(self, TheSearchReportWoodDeal.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                TypenameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
    internal class TypenameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Typename) || t == typeof(Typename?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "ReportWoodDeal")
            {
                return Typename.ReportWoodDeal;
            }
            throw new Exception("Cannot unmarshal type Typename");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Typename)untypedValue;
            if (value == Typename.ReportWoodDeal)
            {
                serializer.Serialize(writer, "ReportWoodDeal");
                return;
            }
            throw new Exception("Cannot marshal type Typename");
        }

        public static readonly TypenameConverter Singleton = new TypenameConverter();
    }
}
