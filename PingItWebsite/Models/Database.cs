using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics;

namespace PingItWebsite.Models
{
    public class Database
    {
        private Database _db;
        private MySqlConnection _connection;

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
            get { return _connection; }
            set { _connection = value; }
        }
        #endregion

        #region Connection Methods
        /// <summary>
        /// Initialize database
        /// </summary>
        /// <returns></returns>
        public MySqlConnection Initialize()
        {
            string connectionString = "Server=35.226.2.191; Database=PingIt; Uid=root; Password=sherrybrighton; SslMode=None; charset=utf8";
            _connection = new MySqlConnection(connectionString);
            try
            {
                _connection.Open();
            }
            catch (MySqlException)
            {
                Debug.WriteLine("Database Error: Cannot connect to database.");
            }
            return _connection;
        }
        

        /// <summary>
        /// Closes the connection
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            try
            {
                _connection.Close();
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
            if (_connection == null)
            {
                _db = new Database();
                _connection = _db.Initialize();
            }
            else if (_connection.State != ConnectionState.Open)
            {
                if (_db == null)
                {
                    _db = new Database();
                }
                _connection = _db.Initialize();
            }
        }
        #endregion
    }
}
