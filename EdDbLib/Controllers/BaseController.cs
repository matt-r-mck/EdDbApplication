using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EdDbLib {
    public class BaseController {

        public Connection Connection { get; protected set; } = null;

        public BaseController(Connection connection) {
            Connection = connection;
        }

        /// <summary>
        /// user may use any of the three following method to surround method calls w/ transaction
        /// </summary>
        public void BeginTransaction() {
            var sqlCmd = new SqlCommand("BEGIN TRANSACTION", Connection.sqlConnection);
            var result = sqlCmd.ExecuteNonQuery();
        }

        public void CommitTransaction() {
            var sqlCmd = new SqlCommand("COMMIT TRANSACTION", Connection.sqlConnection);
            var result = sqlCmd.ExecuteNonQuery();
        }

        public void RollbackTransaction() {
            var sqlCmd = new SqlCommand("ROLLBACK TRANSACTION", Connection.sqlConnection);
            var result = sqlCmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Used in Controllers to check if method affected > 1 row
        /// </summary>
        /// <param name="rowsAffected"> Number of rows passed method affected. </param>
        /// <returns> false if failed, true if worked, Exception if else. </returns>
        protected bool CheckRowsAffected(int rowsAffected) {
                return rowsAffected switch
                {
                    0 => false,
                    1 => true,
                    _ => throw new Exception($"Method affected {rowsAffected} rows");
            };
            }

        }
    }
}
