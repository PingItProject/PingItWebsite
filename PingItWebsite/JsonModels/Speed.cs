using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class Speed
    {
        [JsonProperty("score")]
        public int score { get; set; }
    }
}
