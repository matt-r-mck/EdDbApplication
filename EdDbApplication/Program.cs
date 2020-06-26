using System;
using System.Data;
using System.Data.SqlClient;
using EdDbLib;

namespace EdDbApplication {
    class Program {
        static void Main(string[] args) {

            var connectionString = @"server=localhost\sqlexpress;database=EdDb;trusted_connection=true;";
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            if (sqlConnection.State != ConnectionState.Open) {
                throw new Exception("Connection did not open");
            }

            var sql = "Select * from Student;";
            var sqlCmd = new SqlCommand(sql, sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            while (reader.Read()) {
                var firstName = reader["FirstName"].ToString();
                var lastName = reader["LastName"].ToString();
                Console.WriteLine($"{firstName} {lastName}");
            }
            reader.Close();
            sqlConnection.Close();
        }
    }
}