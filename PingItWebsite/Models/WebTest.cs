using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class WebTest
    {
        #region WebTest properties
        //main variables
        public DateTime date { get; set; }
        public string url { get; set; }
        public TimeSpan loadtime { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string browser { get; set; }
        public string provider { get; set; }

        //google test variable
        public GoogleTest googleTest { get; set; }

        //unique id
        public Guid guid { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public WebTest()
        {

        }
        #endregion


        #region Commands
        /// <summary>
        /// Add webtest to database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <param name="loadtime"></param>
        /// <param name="pagesize"></param>
        /// <param name="requests"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="browser"></param>
        /// <param name="guid"></param>
        /// <param name="database"></param>
        public void CreateWebTest(string username, DateTime timestamp, string url, TimeSpan loadtime, int requests, 
            string city, string state, string platform, string provider, int batch, Guid guid, Database database)
        {
            database.CheckConnection();
            try
            {
                string formatDate = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                string insert = "INSERT INTO webtests VALUES ('" + username + "','" + formatDate + "','" + url + "','" + loadtime +
                    "'," + requests + ",'" + city + "','" + state + "','" + platform + "','" + provider + "'," + batch + ",'" + guid + "');";

                MySqlCommand command = new MySqlCommand(insert, database.Connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Webtests): Cannot enter a new webtest in database");
            }
        }
        #endregion

        #region Queries
        /// <summary>
        /// Gets the latest batch number by checking if the user tested before and then calling helper fx
        /// </summary>
        /// <param name="username"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public int GetBatch(string username, Database database)
        {
            int result = 0;
            int count = 0;
            database.CheckConnection();
            try
            {
                string query = "SELECT Count(*) FROM webtests WHERE username = '" + username + "';";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Webtests): Cannot get webtest count from users.");
            }
            if (count != 0)
            {
                result = GetBatchHelper(username, database);
            }
            return result;
        }

        /// <summary>
        /// Get the latest batch number
        /// </summary>
        /// <param name="username"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public int GetBatchHelper(string username, Database database)
        {
            int result = 0;
            try
            {
                string query = "SELECT batch FROM webtests WHERE username = '" + username + "' ORDER BY batch DESC LIMIT 1;";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetInt32("batch");
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Users): Cannot get user row from users.");
            }
            return result;
        }

      
        /// <summary>
        /// Get guid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="batch"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public Guid GetGuid(string username, int batch, Database database)
        {
            Guid result;
            try
            {
                string query = "SELECT guid FROM webtests WHERE username = '" + username + "' AND batch = " + batch + ";";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetGuid("guid");
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Users): Cannot get user row from users.");
            }
            return result;
        }
        #endregion

        #region StoredProcedures
        /// <summary>
        /// Get user specific web tests
        /// </summary>
        /// <param name="username"></param>
        /// <param name="batch"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<WebTest> GetUserTests(string username, int batch, Database database)
        {
            database.CheckConnection();
            List<WebTest> tests = new List<WebTest>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserTests", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@user", username);
                command.Parameters.AddWithValue("@num", batch);

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    WebTest wt = new WebTest
                    {
                        date = reader.GetDateTime("tstamp"),
                        url = reader.GetString("url"),
                        loadtime = reader.GetTimeSpan("loadtime"),
                        city = reader.GetString("city"),
                        state = reader.GetString("state"),
                        browser = reader.GetString("browser"),
                        provider = reader.GetString("provider"),
                        guid = reader.GetGuid("guid")
                    };
                    tests.Add(wt);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetUserTests.");
            }
            return tests;
        }

        /// <summary>
        /// Get crowdsourcing tests
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="browser"></param>
        /// <param name="website"></param>
        /// <param name="ordering"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<WebTest> GetTests(string city, string state, string browser, string website, bool ordering, Database database)
        {
            database.CheckConnection();
            List<WebTest> tests = new List<WebTest>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetTests", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                if (String.IsNullOrEmpty(city))
                {
                    city = "null";
                } 
                if (String.IsNullOrEmpty(state))
                {
                    state = "null";
                }
                if (String.IsNullOrEmpty(browser))
                {
                    browser = "null";
                }
                if (String.IsNullOrEmpty(website))
                {
                    website = "null";
                }

                command.Parameters.AddWithValue("@ucity", city);
                command.Parameters.AddWithValue("@ustate", state);
                command.Parameters.AddWithValue("@ubrowser", browser);
                command.Parameters.AddWithValue("@uwebsite", website);

                
                if (ordering)
                {
                    command.Parameters.AddWithValue("@ordering", true);
                } else
                {
                    command.Parameters.AddWithValue("@ordering", false);
                }
                
                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    WebTest wt = new WebTest
                    {
                        date = reader.GetDateTime("tstamp"),
                        url = reader.GetString("url"),
                        loadtime = reader.GetTimeSpan("loadtime"),
                        city = reader.GetString("city"),
                        state = reader.GetString("state"),
                        browser = reader.GetString("browser"),
                        provider = reader.GetString("provider"),
                        guid = reader.GetGuid("guid")
                    };
                    tests.Add(wt);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetTests.");
            }
            return tests;
        }
        #endregion
    }
}
