using MySql.Data.MySqlClient;
using PingItWebsite.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace PingItWebsite.Models
{
    public class UserTestAvgs
    {
        #region Variables
        public int key { get; set; }
        public string url { get; set; }
        public string provider { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public double speed { get; set; }
        public double loadtime { get; set; }
        public int score { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public UserTestAvgs() {

        }
        #endregion

        #region Stored Procedures
        /// <summary>
        /// Get user averages
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<UserTestAvgs> GetWebsiteTestCounts(Database database)
        {
            database.CheckConnection();
            List<UserTestAvgs> tests = new List<UserTestAvgs>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserTestAvgs", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@user", HomeController._username);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserTestAvgs wtc = new UserTestAvgs
                    {
                        key = reader.GetInt32("id"),
                        url = reader.GetString("url"),
                        provider = reader.GetString("provider"),
                        state = reader.GetString("state"),
                        city = reader.GetString("city"),
                        speed = reader.GetDouble("speed"),
                        loadtime = reader.GetDouble("loadtime"),
                        score = reader.GetInt32("score")
                    };
                    tests.Add(wtc);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetUserTestAvgs.");
            }
            return tests;
        }
        #endregion

    }
}
