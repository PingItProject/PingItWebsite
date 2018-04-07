using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class Counties
    {
        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public Counties()
        {

        }
        #endregion

        /// <summary>
        /// Get state code 
        /// </summary>
        /// <param name="city"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public string GetState(string city, Database database)
        {
            string result = "";
            try
            {
                string query = "SELECT * FROM PingIt.counties WHERE city = '"+ city + "' ORDER BY population DESC LIMIT 1";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString("state_id");
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Counties): Cannot get state from counties.");
            }
            return result;
        }

        /// <summary>
        /// Get the census fips code from the database
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public int GetCensusCode(string city, string state, Database database)
        {
            int result = -1;
            try
            {
                string query = "SELECT county_fips FROM PingIt.counties WHERE city = '" + city + "' AND " +
                    "state_id = '" + state + "' ORDER BY population DESC LIMIT 1";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetInt32("county_fips");
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Counties): Cannot get fips code from counties.");
            }
            return result;
        }
    }

}
