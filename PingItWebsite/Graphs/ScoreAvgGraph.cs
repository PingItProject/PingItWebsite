using System;
using System.Runtime.Serialization;

namespace PingItWebsite.Graphs
{
    [DataContract]
    public class ScoreAvgGraph
    {
        #region Data Contract Variables
        [DataMember(Name = "label")]
        public Nullable<int> Key = null;

        [DataMember(Name = "y")]
        public Nullable<double> Score = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor used to create a score bar graph
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loadtime"></param>
        public ScoreAvgGraph(int key, int score)
        {
            this.Key = key;
            this.Score = score;
        }
        #endregion

    }
}
