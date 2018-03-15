using Newtonsoft.Json;

namespace PingItWebsite.JsonModels
{
    public class LoadingExperience
    {
        [JsonProperty("overall_category")]
        public string category { get; set; }
    }
}
