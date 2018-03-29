using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class PageStats
    {
        [JsonProperty("numberResources")]
        public int numResources { get; set; }

        [JsonProperty("numberHosts")]
        public int numHosts { get; set; }

        [JsonProperty("totalRequestBytes")]
        public long bytes { get; set; }

        [JsonProperty("numberStaticResources")]
        public int numStaticResources { get; set; }

        [JsonProperty("htmlResponseBytes")]
        public long htmlBytes { get; set; }

        [JsonProperty("overTheWireResponseBytes")]
        public long wireBytes { get; set; }

        [JsonProperty("cssResponseBytes")]
        public long cssBytes { get; set; }

        [JsonProperty("imageResponseBytes")]
        public long imageBytes { get; set; }

        [JsonProperty("javascriptResponseBytes")]
        public long javascriptBytes { get; set; }

        [JsonProperty("otherResponseBytes")]
        public long otherBytes { get; set; }

    }
}
