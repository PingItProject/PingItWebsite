using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class Database
    {
        #region Variables
        private Database db;
        private MySqlConnection connection;
        #endregion

        #region Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        public Database()
        {
           
        }
        #endregion

        #region Getters/Setters
        public MySqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }
        #endregion

        #region Connection Methods
        /// <summary>
        /// Initialize database
        /// </summary>
        /// <returns></returns>
        public MySqlConnection Initialize()
        {
            string connectionString = "[INSERT CREDENTIALS]";
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error: Cannot connect to database.");
            }
            return connection;
        }
        
        /// <summary>
        /// Closes the connection
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error: Cannot close the database.");
                return false;
            }
        }

        /// <summary>
        /// Checks whether there is a database connection
        /// </summary>
        public void CheckConnection()
        {
            if (connection == null)
            {
                db = new Database();
                connection = db.Initialize();
            }
            else if (connection.State != ConnectionState.Open)
            {
                if (db == null)
                {
                    db = new Database();
                }
                connection = db.Initialize();
            }
        }
        #endregion
    }
}
