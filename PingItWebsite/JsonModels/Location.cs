using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class Location
    {
        [JsonProperty("lat")]
        public double lat { get; set; }

        [JsonProperty("lng")]
        public double lng { get; set; }
    }
}
