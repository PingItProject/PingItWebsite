using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PingItWebsite.Models
{
    [DataContract]
    public class LoadTimeAvg
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public Nullable<int> Key = null;

        [DataMember(Name = "y")]
        public Nullable<double> Loadtime = null;

        //[DataMember(Name = "score")]
        //public Nullable<int> Score = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a loadtime bar graph
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loadtime"></param>
        public LoadTimeAvg(int key, double loadtime)
        {
            this.Key = key;
            this.Loadtime = loadtime;
        }
        #endregion
    }
}
