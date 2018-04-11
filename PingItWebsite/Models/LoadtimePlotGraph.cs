using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    [DataContract]
    public class LoadtimePlotGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "x")]
        public Nullable<long> Date = null;

        [DataMember(Name = "y")]
        public Nullable<int> Loadtime = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a speed over time graph
        /// </summary>
        /// <param name="date"></param>
        /// <param name="loadtime"></param>
        public LoadtimePlotGraph(long date, int loadtime)
        {
            this.Date = date;
            this.Loadtime = loadtime;
        }
        #endregion
    }
}

