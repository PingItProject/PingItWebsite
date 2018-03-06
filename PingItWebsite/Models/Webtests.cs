using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PingItWebsite.Models
{
    public class Webtests
    {
        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public Webtests()
        {

        }

        /// <summary>
        /// Add webtest to database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <param name="loadtime"></param>
        /// <param name="pagesize"></param>
        /// <param name="requests"></param>
        /// <param name="location"></param>
        /// <param name="platform"></param>
        /// <param name="guid"></param>
        /// <param name="database"></param>
        public Webtests(string username, DateTime timestamp, string url, TimeSpan loadtime, double pagesize, int requests, 
            string location, string platform, Guid guid, Database database)
        {
            database.CheckConnection();
            try
            {
                string formatDate = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                string insert = "INSERT INTO webtests VALUES ('" + username + "','" + formatDate + "','" + url + "','" + loadtime + 
                    "'," + pagesize + "," + requests + ", NULL ,'" + platform + "','" + guid + "');";

                MySqlCommand command = new MySqlCommand(insert, database.Connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Webtests): Cannot enter a new webtest in database");
            }
        }
        #endregion
    }
}
