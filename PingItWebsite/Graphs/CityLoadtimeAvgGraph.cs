using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    [DataContract]
    public class CityLoadtimeAvgGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public string Location = null;

        [DataMember(Name = "y")]
        public Nullable<double> Loadtime = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a avg website with cities loadtime bar graph
        /// </summary>
        /// <param name="location"></param>
        /// <param name="loadtime"></param>
        public CityLoadtimeAvgGraph(string location, double loadtime)
        {
            this.Location = location;
            this.Loadtime = loadtime;
        }
        #endregion
    }
}
