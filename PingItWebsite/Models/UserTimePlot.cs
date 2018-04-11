using MySql.Data.MySqlClient;
using PingItWebsite.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PingItWebsite.Models
{
    public class UserTimePlot
    {
        #region Variables
        public int rank { get; set; }
        public DateTime date { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string provider { get; set; }
        public double speed { get; set; }
        public TimeSpan loadtime { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public UserTimePlot()
        {

        }
        #endregion

        #region Stored Procedures
        /// <summary>
        /// Get user test data organized chronologically
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<UserTimePlot> GetUserTestChron(Database database)
        {
            database.CheckConnection();
            List<UserTimePlot> tests = new List<UserTimePlot>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserTestChron", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@user", HomeController._username);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserTimePlot utp = new UserTimePlot
                    {
                        rank = reader.GetInt32("rank"),
                        date = reader.GetDateTime("tstamp"),
                        city = reader.GetString("city"),
                        state = reader.GetString("state"),
                        provider = reader.GetString("provider"),
                        speed = reader.GetDouble("speed"),
                        loadtime = reader.GetTimeSpan("loadtime")
                    };
                    tests.Add(utp);
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
