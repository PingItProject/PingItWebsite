using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class Website
    {
        public string website { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public double connection { get; set; }
        public double firstByte { get; set; }
        public double total { get; set; }

        public string location { get; set; }

        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public Website()
        {

        }
        #endregion

        #region Stored Procedures
        /// <summary>
        /// Get the public website info
        /// </summary>
        /// <param name="website"></param>
        /// <param name="ordering"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Website> GetDomainLoadtimes(string website, bool ordering, Database database)
        {
            database.CheckConnection();
            List<Website> info = new List<Website>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetDomainLoadtimes", database.Connection);
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@uwebsite", website);

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
                    Website w = new Website
                    {
                        website = website,
                        city = reader.GetString("city"),
                        country = reader.GetString("country"),
                        connection = reader.GetDouble("connection"),
                        firstByte = reader.GetDouble("first_byte"),
                        total = reader.GetDouble("total")
                    };
                    info.Add(w);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetPublicWebsiteInfo.");
            }
            return info;
        }

        /// <summary>
        /// Get the public website info
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Website> GetAvgLoadTime(Database database)
        {
            database.CheckConnection();
            List<Website> info = new List<Website>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetAvgWebLoadtime", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Website w = new Website
                    {
                        website = reader.GetString("website"),
                        total = reader.GetDouble("total")
                    };
                    info.Add(w);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetAvgWebLoadtime.");
            }
            return info;
        }

        /// <summary>
        /// Get the public website info
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Website> GetAvgLoadTimeCities(Database database)
        {
            database.CheckConnection();
            List<Website> info = new List<Website>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetWebLoadtimeCities", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Website w = new Website
                    {
                        location = reader.GetString("city") + ", " + reader.GetString("country"),
                        total = reader.GetDouble("total")
                    };
                    info.Add(w);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetWebLoadtimeCities.");
            }
            return info;
        }
        #endregion
    }
}
