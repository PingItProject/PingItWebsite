using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class RuleGroups
    {
        [JsonProperty("SPEED")]
        public Speed speed { get; set; }
    }
}
