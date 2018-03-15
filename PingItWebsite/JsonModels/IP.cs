using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class IP
    {
        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("countryCode")]
        public string countryCode { get; set; }

        [JsonProperty("region")]
        public string region { get; set; }

        [JsonProperty("regionName")]
        public string regionName { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("zip")]
        public int zip { get; set; }

        [JsonProperty("lat")]
        public double latitude { get; set; }

        [JsonProperty("lon")]
        public double longitude { get; set; }

        [JsonProperty("isp")]
        public string isp { get; set; }
    }
}
