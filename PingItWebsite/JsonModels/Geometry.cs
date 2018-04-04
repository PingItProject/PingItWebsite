using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class Geometry
    {
        [JsonProperty("location")]
        public Location location { get; set; }
    }
}
