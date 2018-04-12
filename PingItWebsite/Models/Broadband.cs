using System.Collections.Generic;

namespace PingItWebsite.Models
{
    public class Broadband
    {
        public string blockcode { get; set; }

        public string provider { get; set; }

        public string state { get; set; }

        public string city { get; set; }
        
        public double speed { get; set; }

        public Dictionary<double, int> speedDict { get; set; }

    }
}
