using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    [DataContract]
    public class BroadbandSpeedGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public String Provider = null;

        [DataMember(Name = "y")]
        public Nullable<double> Speed = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a broadband speed bar graph
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="speed"></param>
        public BroadbandSpeedGraph(string provider, double speed)
        {
            this.Provider = provider;
            this.Speed = speed;
        }
        #endregion
    }
}
