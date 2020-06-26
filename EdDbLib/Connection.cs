using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace EdDbLib {
    public class Connection {

        public SqlConnection sqlConnection { get; private set; } = null;//install package to talk to sql
        private string connectionString { get; set; } = null;

        public Connection(string server, string instance, string database) { //constructor to establish connection
            connectionString = $"server={server}\\{instance};" + $"database={database};" + $"trusted_connection=true;"; //sets string equal to user input
            sqlConnection = new SqlConnection(connectionString); //passes user input into conneciton 
            sqlConnection.Open();
            if (sqlConnection.State != System.Data.ConnectionState.Open) { //checks that user input works
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

