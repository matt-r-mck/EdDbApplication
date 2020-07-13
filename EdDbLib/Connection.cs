using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace EdDbLib {

    /// <summary>
    /// Stores all connectin info.
    /// </summary>
    public class Connection {

        public const string LOCALHOST = "localhost";
        public const string SQLEXPRESS = "sqlexpress";
        public const string EDDB = "EdDb";

        //Makes the SQL connection and passed connection string a property of this class.
        public SqlConnection sqlConnection { get; private set; } = null;
        private string connectionString { get; set; } = null;

        /// <summary>
        /// Connection string constructor.
        /// </summary>
        /// <param name="server"> Server to connect to. </param>
        /// <param name="instance"> Current intance of server. </param>
        /// <param name="database"> Database to be accessed. </param>
         //constructor to establish connection
        public Connection(string server, string instance, string database) {
            //sets string equal to user input
            connectionString = $"server={server}\\{instance};" + $"database={database};" + $"trusted_connection=true;"; 
            //passes user input into conneciton 
            sqlConnection = new SqlConnection(connectionString); 
            sqlConnection.Open();
            if (sqlConnection.State != System.Data.ConnectionState.Open) { 
                //checks that user input works
                throw new Exception("Connection Failed: Check entered parameters");
            }
        }
        public void Close() {
            if(sqlConnection != null){
                sqlConnection.Close();
                sqlConnection = null;
            }
            }
    }
    }

