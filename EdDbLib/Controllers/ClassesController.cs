using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace EdDbLib {
    /// <summary>
    /// Controller allows user to call from and modify class table
    /// </summary>
    class ClassesController : BaseController {


        public ClassesController(Connection connection): base (connection) {
        }

        /// <summary>
        /// Loads class parameters into instance of class
        /// </summary>
        /// <param name="reader"> Reader initalized by method called by user. </param>
        /// <returns> Class instance </returns>
        private Class LoadClassInstance(SqlDataReader reader) {
            var id = Convert.ToInt32(reader["Id"]);
            var ccllaass = new Class(id);
            ccllaass.Code = reader["Code"].ToString();
            ccllaass.Subject = reader["Subject"].ToString();
            ccllaass.Section = Convert.ToInt32(reader["Subject"]);
            ccllaass.InstructorID = null;
            if (!reader["InstructorID"].Equals(DBNull.Value)) {
                ccllaass.InstructorID = Convert.ToInt32(reader["InstructorID"]);
            }
            return ccllaass;            
        }
        
        public IEnumerable<Class> SelectAll() {
            var sqlCmd = new SqlCommand(Class.SelectAll, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var classes = new List<Class>();
            while(reader.Read()){
                var ccllaass = LoadClassInstance(reader);
                classes.Add(ccllaass);
            }
            reader.Close();
            return classes;
        }

        public Class SelectByPK(int Id) {
            var sqlCmd = new SqlCommand(Class.SelectByPK, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Id", Id);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var ccllaass = LoadClassInstance(reader);
            reader.Close();
            return ccllaass;
        }

        public Class SelectByCode(string Code) {
            var sqlCmd = new SqlCommand(Class.SelectByCode, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Code", Code);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var ccllaass = LoadClassInstance(reader);
            reader.Close();
            return ccllaass;
        }

        public bool Delete(int id) {
            var sqlStmt = $"Delete from Class where ID = {id}";
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            try {
                var rowsAffected = sqlCmd.ExecuteNonQuery();
                bool success = rowsAffected switch {
                    0 => false,
                    1 => true,
                    _ => throw new Exception($"Delete method affected {rowsAffected} rows")
                }; 
                return success;
            } catch (SqlException ex) {
                var refIntEx = new Exceptions.ReferentialIntegrityException("Cannot delete a major that contains a student", ex);
                throw refIntEx;
            }
        }

        private int CreateSqlCommand(string sqlStmt) {
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected;
        }

        public bool Insert(Class ccllaass) {
            var instructorId = (ccllaass.InstructorID == null) ? "NULL" :  $"{ccllaass.InstructorID}";
            var sqlStmt = "INSERT Class " +
                "(Code, Subject, Section, InstructorId) VALUES " +
                $"('{ccllaass.Code}', '{ccllaass.Subject}', '{ccllaass.Section}', {instructorId}";
            int rowsAffected = CreateSqlCommand(sqlStmt);
            return CheckRowsAffected(rowsAffected);
        }

        public bool Update (Class ccllaass) {
            var instructorId = (ccllaass.InstructorID == null) ? "NULL" : $"{ccllaass.InstructorID}";
            var sqlStmt = "UPDATE Student SET " +
                $"Code = '{ccllaass.Code}', " +
                $"Subject = '{ccllaass.Subject}', " +
                $"Section = '{ccllaass.Section}', " +
                $"InstructorId = {instructorId}, " +
                $"WHERE Id = {ccllaass.Id};";
            int rowsAffected = CreateSqlCommand(sqlStmt);
            return CheckRowsAffected(rowsAffected);
        }



    }
}
