using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Graphs
{
    [DataContract]
    public class SpeedAvgGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public Nullable<int> Key = null;

        [DataMember(Name = "y")]
        public Nullable<double> Speed = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a speed bar graph
        /// </summary>
        /// <param name="key"></param>
        /// <param name="speed"></param>
        public SpeedAvgGraph(int key, double speed)
        {
            this.Key = key;
            this.Speed = speed;
        }
        #endregion
    }
}
