using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class GoogleTest
    {
        public static bool _complete = false;

        public int score { get; set; }
        public string category { get; set; }
        public int resources { get; set; }
        public int hosts { get; set; }
        public long bytes { get; set; }
        public long htmlBytes { get; set; }
        public long cssBytes { get; set; }
        public long imageBytes { get; set; }
        public decimal webspeed { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string browser { get; set; }

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public GoogleTest()
        {

        }
        #endregion

        #region Commands
        /// <summary>
        /// Create a google test
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="score"></param>
        /// <param name="category"></param>
        /// <param name="resources"></param>
        /// <param name="hosts"></param>
        /// <param name="bytes"></param>
        /// <param name="htmlBytes"></param>
        /// <param name="cssBytes"></param>
        /// <param name="imageBytes"></param>
        /// <param name="webSpeed"></param>
        /// <param name="database"></param>
        public void CreateGoogleTest(Guid guid, int score, string category, int resources, int hosts, long bytes,
            long htmlBytes, long cssBytes, long imageBytes, decimal webSpeed, Database database)
        {
            database.CheckConnection();
            try
            {
                string insert = "INSERT INTO googletests VALUES ('" + guid + "'," + score + ",'" + category + "'," + resources + "," + hosts +
                    "," + bytes + "," + htmlBytes + "," + cssBytes + "," + imageBytes + "," + webSpeed + ");";

                Debug.WriteLine("I'm in hereeeeeeeeee");
                MySqlCommand command = new MySqlCommand(insert, database.Connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (GoogleTests): Cannot enter a new google test in database");
            }
        }
        #endregion

        #region StoredProcedures
        /// <summary>
        /// Returns a matching Google Test
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public GoogleTest GetUserGoogleTest(Guid guid, Database database)
        {
            database.CheckConnection();
            GoogleTest gt = null;
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserGoogleTestResults", database.Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@guid", guid);

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    gt = new GoogleTest
                    {
                        score = reader.GetInt32("score"),
                        category = reader.GetString("category"),
                        resources = reader.GetInt32("resources"),
                        hosts = reader.GetInt32("hosts"),
                        bytes = reader.GetInt64("bytes"),
                        htmlBytes = reader.GetInt64("html_bytes"),
                        cssBytes = reader.GetInt64("css_bytes"),
                        imageBytes = reader.GetInt64("image_bytes"),
                        webspeed = (decimal)reader.GetDouble("webspeed")
                    };
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetGoogleTestResults.");
            }
            _complete = true;
            return gt;
        }

        /// <summary>
        /// Get google tests
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="browser"></param>
        /// <param name="ordering"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<GoogleTest> GetGoogleTests(string city, string state, string browser, bool ordering, Database database)
        {
            database.CheckConnection();
            List<GoogleTest> tests = new List<GoogleTest>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetGoogleTestResults", database.Connection);
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

                command.Parameters.AddWithValue("@c", city);
                command.Parameters.AddWithValue("@s", state);
                command.Parameters.AddWithValue("@browser", browser);

                if (ordering)
                {
                    command.Parameters.AddWithValue("@ordering", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@ordering", false);
                }

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    GoogleTest gt = new GoogleTest
                    {
                        score = reader.GetInt32("score"),
                        category = reader.GetString("category"),
                        resources = reader.GetInt32("resources"),
                        hosts = reader.GetInt32("hosts"),
                        bytes = reader.GetInt64("bytes"),
                        htmlBytes = reader.GetInt64("html_bytes"),
                        cssBytes = reader.GetInt64("css_bytes"),
                        imageBytes = reader.GetInt64("image_bytes"),
                        webspeed = (decimal)reader.GetDouble("webspeed"),
                        city = reader.GetString("city"),
                        state = reader.GetString("state"),
                        browser = reader.GetString("platform"),
                    };
                    tests.Add(gt);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetTestResults.");
            }
            return tests;
        }
        #endregion
    }
}
