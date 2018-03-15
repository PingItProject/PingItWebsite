using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class PageSpeed
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("responseCode")]
        public int responseCode { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("ruleGroups")]
        public RuleGroups ruleGroups { get; set; }

        [JsonProperty("loadingExperience")]
        public LoadingExperience experience { get; set; }

        [JsonProperty("pageStats")]
        public PageStats stats { get; set; }
    }
}
