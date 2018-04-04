using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class Results
    {
        [JsonProperty("geometry")]
        public Geometry geometry { get; set; }
    }
}
