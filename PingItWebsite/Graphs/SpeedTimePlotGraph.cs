using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Graphs
{
    [DataContract]
    public class SpeedTimePlotGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "x")]
        public Nullable<long> Date = null;

        [DataMember(Name = "y")]
        public Nullable<double> Speed = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a speed over time graph
        /// </summary>
        /// <param name="date"></param>
        /// <param name="speed"></param>
        public SpeedTimePlotGraph(long date, double speed)
        {
            this.Date = date;
            this.Speed = speed;
        }
        #endregion
    }
}
