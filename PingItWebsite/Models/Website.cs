using MySql.Data.MySqlClient;
using PingItWebsite.Controllers;
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
        /// Get the average domain loadtimes
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Website> GetAvgDomainLoadtime(string domain, Database database)
        {
            database.CheckConnection();
            List<Website> info = new List<Website>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetAvgDomainLoadtime", database.Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@domain", domain);

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
                Debug.WriteLine("Stored Procedure: Cannot perform GetAvgDomainLoadtime.");
            }
            return info;
        }

        /// <summary>
        /// Get the user average domain loadtimes
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public Website GetUserAvgDomainLoadtime(string domain, Database database)
        {
            database.CheckConnection();
            Website w = null;
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserAvgDomainLoadtime", database.Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@user", HomeController._username);
                command.Parameters.AddWithValue("@domain", domain);

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    w = new Website
                    {
                        total = reader.GetDouble("loadtime")
                    };
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetUserAvgDomainLoadtime.");
            }
            return w;
        }

        /// <summary>
        /// Get avg loadtime per city
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Website> GetAvgCityLoadtime(string domain, Database database)
        {
            database.CheckConnection();
            List<Website> info = new List<Website>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetAvgCityLoadtime", database.Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@domain", domain);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Website w = new Website
                    {
                        location = reader.GetString("city"),
                        total = reader.GetDouble("total")
                    };
                    info.Add(w);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetAvgCityLoadtime.");
            }
            return info;
        }

        /// <summary>
        /// Get avg loadtime per city
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Website> GetDomainLoadtimeByLocation(string domain, Database database)
        {
            database.CheckConnection();
            List<Website> info = new List<Website>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetDomainLoadtimeByLocation", database.Connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@user", HomeController._username);
                command.Parameters.AddWithValue("@domain", domain);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Website w = new Website
                    {
                        location = "*" + reader.GetString("city"),
                        total = reader.GetDouble("loadtime")
                    };
                    info.Add(w);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetDomainLoadtimeByLocation.");
            }
            return info;
        }
        #endregion
    }
}
