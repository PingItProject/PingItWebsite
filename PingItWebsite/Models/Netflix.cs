using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PingItWebsite.Models
{
    public class Netflix
    {
        public string website { get; set; }
        public string isp { get; set; }
        public string type { get; set; }
        public double speed { get; set; }
        public string date { get; set; }

        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public Netflix()
        {

        }
        #endregion

        #region Stored Procedures
        /// <summary>
        /// Get the Netflix website info
        /// </summary>
        /// <param name="ordering"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<Netflix> GetNetflixInfo(bool ordering, Database database)
        {
            database.CheckConnection();
            List<Netflix> info = new List<Netflix>();
            try
            {
                MySqlCommand command = new MySqlCommand("GetNetflixInfo", database.Connection);
                command.CommandType = CommandType.StoredProcedure;

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
                    Netflix n = new Netflix
                    {
                        website = "netflix",
                        isp = reader.GetString("isp"),
                        type = reader.GetString("type"),
                        speed = reader.GetDouble("speed"),
                        date = reader.GetString("date")

                    };
                    info.Add(n);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Stored Procedure: Cannot perform GetNetflixInfo.");
            }
            return info;
        }
        #endregion
    }
}
