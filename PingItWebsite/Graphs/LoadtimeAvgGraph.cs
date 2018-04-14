using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    [DataContract]
    public class LoadtimeAvgGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public Nullable<int> Key = null;

        [DataMember(Name = "y")]
        public Nullable<double> Loadtime = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a loadtime bar graph
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loadtime"></param>
        public LoadtimeAvgGraph(int key, double loadtime)
        {
            this.Key = key;
            this.Loadtime = loadtime;
        }
        #endregion
    }
}
