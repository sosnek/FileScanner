using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileScan;

namespace FileScan
{
    public partial class ScanResults
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("authentihash")]
        public string Authentihash { get; set; }

        [JsonProperty("creation_date")]
        public long CreationDate { get; set; }

        [JsonProperty("first_submission_date")]
        public long FirstSubmissionDate { get; set; }

        [JsonProperty("last_analysis_date")]
        public long LastAnalysisDate { get; set; }

        [JsonProperty("last_analysis_results")]
        public Dictionary<string, LastAnalysisResult> LastAnalysisResults { get; set; }

        [JsonProperty("last_analysis_stats")]
        public LastAnalysisStats LastAnalysisStats { get; set; }

        [JsonProperty("last_modification_date")]
        public long LastModificationDate { get; set; }

        [JsonProperty("last_submission_date")]
        public long LastSubmissionDate { get; set; }

        [JsonProperty("magic")]
        public string Magic { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("meaningful_name")]
        public string MeaningfulName { get; set; }

        [JsonProperty("names")]
        public string[] Names { get; set; }

        [JsonProperty("reputation")]
        public long Reputation { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("ssdeep")]
        public string Ssdeep { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("times_submitted")]
        public long TimesSubmitted { get; set; }

        [JsonProperty("total_votes")]
        public TotalVotes TotalVotes { get; set; }

        [JsonProperty("trid")]
        public Trid[] Trid { get; set; }

        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }

        [JsonProperty("type_tag")]
        public string TypeTag { get; set; }

        [JsonProperty("unique_sources")]
        public long UniqueSources { get; set; }

        [JsonProperty("vhash")]
        public string Vhash { get; set; }
    }

    public enum Category { Malicious, TypeUnsupported, Undetected, Timeout, Failure };

    public partial class LastAnalysisResult
    {
        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("engine_name")]
        public string EngineName { get; set; }

        [JsonProperty("engine_update")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long EngineUpdate { get; set; }

        [JsonProperty("engine_version")]
        public string EngineVersion { get; set; }

        [JsonProperty("method")]
        public Method Method { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
    }

    public partial class LastAnalysisStats
    {
        [JsonProperty("confirmed-timeout")]
        public long ConfirmedTimeout { get; set; }

        [JsonProperty("failure")]
        public long Failure { get; set; }

        [JsonProperty("harmless")]
        public long Harmless { get; set; }

        [JsonProperty("malicious")]
        public long Malicious { get; set; }

        [JsonProperty("suspicious")]
        public long Suspicious { get; set; }

        [JsonProperty("timeout")]
        public long Timeout { get; set; }

        [JsonProperty("type-unsupported")]
        public long TypeUnsupported { get; set; }

        [JsonProperty("undetected")]
        public long Undetected { get; set; }
    }


    public partial class ImportList
    {
        [JsonProperty("imported_functions")]
        public string[] ImportedFunctions { get; set; }

        [JsonProperty("library_name")]
        public string LibraryName { get; set; }
    }

    public partial class ResourceDetail
    {
        [JsonProperty("chi2")]
        public double Chi2 { get; set; }

        [JsonProperty("entropy")]
        public double Entropy { get; set; }

        [JsonProperty("filetype")]
        public string Filetype { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class ResourceLangs
    {
        [JsonProperty("ENGLISH US")]
        public long EnglishUs { get; set; }
    }

    public partial class ResourceTypes
    {
        [JsonProperty("RT_DIALOG")]
        public long RtDialog { get; set; }

        [JsonProperty("RT_MANIFEST")]
        public long RtManifest { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("chi2")]
        public double Chi2 { get; set; }

        [JsonProperty("entropy")]
        public double Entropy { get; set; }

        [JsonProperty("flags")]
        public string Flags { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("raw_size")]
        public long RawSize { get; set; }

        [JsonProperty("virtual_address")]
        public long VirtualAddress { get; set; }

        [JsonProperty("virtual_size")]
        public long VirtualSize { get; set; }
    }

    public partial class TotalVotes
    {
        [JsonProperty("harmless")]
        public long Harmless { get; set; }

        [JsonProperty("malicious")]
        public long Malicious { get; set; }
    }

    public partial class Trid
    {
        [JsonProperty("file_type")]
        public string FileType { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }
    }



    public enum Method { Blacklist };

    public partial class ScanResults
    {
        public static ScanResults FromJson(string json) => JsonConvert.DeserializeObject<ScanResults>(json, FileScan.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CategoryConverter.Singleton,
                MethodConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Category?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "malicious":
                    return Category.Malicious;
                case "type-unsupported":
                    return Category.TypeUnsupported;
                case "undetected":
                    return Category.Undetected;
                case "timeout":
                    return Category.Timeout;
                case "failure":
                    return Category.Failure;
            }
            throw new Exception("Cannot unmarshal type Category");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Category)untypedValue;
            switch (value)
            {
                case Category.Malicious:
                    serializer.Serialize(writer, "malicious");
                    return;
                case Category.TypeUnsupported:
                    serializer.Serialize(writer, "type-unsupported");
                    return;
                case Category.Undetected:
                    serializer.Serialize(writer, "undetected");
                    return;
                case Category.Timeout:
                    serializer.Serialize(writer, "timeout");
                    return;
                case Category.Failure:
                    serializer.Serialize(writer, "failure");
                    return;
            }
            throw new Exception("Cannot marshal type Category");
        }

        public static readonly CategoryConverter Singleton = new CategoryConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class MethodConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Method) || t == typeof(Method?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "blacklist")
            {
                return Method.Blacklist;
            }
            throw new Exception("Cannot unmarshal type Method");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Method)untypedValue;
            if (value == Method.Blacklist)
            {
                serializer.Serialize(writer, "blacklist");
                return;
            }
            throw new Exception("Cannot marshal type Method");
        }

        public static readonly MethodConverter Singleton = new MethodConverter();
    }
}
