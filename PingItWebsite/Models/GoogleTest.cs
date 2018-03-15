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
        public List<GoogleTest> GetGoogleTests(Guid guid, Database database)
        {
            database.CheckConnection();
            List<GoogleTest> tests = new List<GoogleTest>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetGoogleTestResults", database.Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@guid", guid);

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
                        webspeed = (decimal)reader.GetDouble("webspeed")
                    };
                    tests.Add(gt);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetGoogleTestResults.");
            }
            _complete = true;
            return tests;
        }
        #endregion
    }
}
