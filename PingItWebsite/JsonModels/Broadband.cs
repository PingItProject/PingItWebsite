using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class Broadband
    {
        [JsonProperty("blockcode")]
        public string blockcode { get; set; }

        [JsonProperty("providername")]
        public string provider { get; set; }

        [JsonProperty("stateabbr")]
        public string state { get; set; }

        [JsonProperty("maxaddown")]
        public double speed { get; set; }
    }
}