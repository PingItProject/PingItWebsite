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
    public class UserPlotKey
    {
        public int key { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string provider { get; set; }

        #region Stored Procedures
        /// <summary>
        /// Get user plot keys
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<UserPlotKey> GetPlotKeys(Database database)
        {
            database.CheckConnection();
            List<UserPlotKey> data = new List<UserPlotKey>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetUserPlotKeys", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@user", HomeController._username);

                //Run stored procedure to get the event dates in asc order of the current month
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserPlotKey upk = new UserPlotKey
                    {
                        key = reader.GetInt32("rank"),
                        state = reader.GetString("state"),
                        city = reader.GetString("city"),
                        provider = reader.GetString("provider")
                    };
                    data.Add(upk);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetUserPlotKeys.");
            }
            return data;
        }
        #endregion
    }
}
