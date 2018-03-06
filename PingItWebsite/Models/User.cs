using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PingItWebsite.Models
{
    public class User
    {
        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public User()
        {

        }

        /// <summary>
        /// Add user to the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <param name="db"></param>
        public User(string username, string fname, string lname, string email, string password, string type, Database database)
        {
            database.CheckConnection();
            try
            {
                string insert = "INSERT INTO users VALUES ('" + username + "','" + fname + "','" + lname + "','" + email + "','" + password + "','" + type + "');";
                MySqlCommand command = new MySqlCommand(insert, database.Connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Users): Cannot enter a new user in database");
            }
        }
        #endregion

        #region Queries
        /// <summary>
        /// Get the value from the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public string GetValue(string username, string col, Database database)
        {
            string result = null;
            database.CheckConnection();
            try
            {
                string query = "SELECT " + col + " FROM users WHERE username = '" + username + "';";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = reader.GetString(col);
                }
                reader.Close();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error (Users): Cannot get user's " + col + ".");
            }
            return result;
        }
        #endregion
    }
}
