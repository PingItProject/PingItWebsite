using MySql.Data.MySqlClient;
using PingItWebsite.Controllers;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class Broadband
    {
        public string blockcode { get; set; }

        public string provider { get; set; }

        public string state { get; set; }

        public string city { get; set; }
        
        public double speed { get; set; }

        public Dictionary<double, int> speedDict { get; set; }

        #region Stored Procedures
        /// <summary>
        /// Get the user's provider data
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="domain"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Broadband> GetUserProviderData(string city, string state, string domain, Database database)
        {
            database.CheckConnection();
            List<Broadband> data = new List<Broadband>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserProviderData", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@user", HomeController._username);
                command.Parameters.AddWithValue("@ucity", city);
                command.Parameters.AddWithValue("@ustate", state);
                command.Parameters.AddWithValue("@domain", domain);

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Broadband b = new Broadband
                    {
                        provider = "*" + reader.GetString("provider"),
                        speed = reader.GetDouble("speed")
                    };
                    data.Add(b);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetUserTests.");
            }
            return data;
        }
        #endregion
    }
}
