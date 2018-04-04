using Newtonsoft.Json;
using System.Collections.Generic;

namespace PingItWebsite.JsonModels
{
    public class Geocode
    {
        [JsonProperty("results")]
        public IList<Results> results { get; set; }
    }
}
