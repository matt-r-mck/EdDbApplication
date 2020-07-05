using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace EdDbLib {
    /// <summary>
    /// 
    /// </summary>
    public class Connection {
        
        //install package to talk to sql
        public SqlConnection sqlConnection { get; private set; } = null;

        //Makes user connection strings properties of Connection class and creates connection w/ SQL?_________________
        private string connectionString { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="instance"></param>
        /// <param name="database"></param>
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

