using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    [DataContract]
    public class AvgWebLoadtimeGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public string Website = null;

        [DataMember(Name = "y")]
        public Nullable<double> Loadtime = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a avg website speed bar graph
        /// </summary>
        /// <param name="website"></param>
        /// <param name="loadtime"></param>
        public AvgWebLoadtimeGraph(string website, double loadtime)
        {
            this.Website = website;
            this.Loadtime = loadtime;
        }
        #endregion
    }
}
