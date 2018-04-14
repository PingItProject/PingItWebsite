using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    [DataContract]
    public class DomainLoadtimeAvgGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public string Domain = null;

        [DataMember(Name = "y")]
        public Nullable<double> Loadtime = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create an avg domain speed bar graph
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="loadtime"></param>
        public DomainLoadtimeAvgGraph(string domain, double loadtime)
        {
            this.Domain = domain;
            this.Loadtime = loadtime;
        }
        #endregion
    }
}
