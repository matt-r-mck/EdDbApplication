using EdDbLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Text;

namespace EdDbLib {
    /// <summary>
    /// Controller allows user to call from and modify student table.
    /// </summary>
    public class MajorsController : BaseController {

        /// <summary>
        /// Establishes a connection for this controller using base controller
        /// </summary>
        /// <param name="connection"></param>
        public MajorsController(Connection connection) : base (connection) {
        }

        /// <summary>
        /// Loads parameters for Major table into major instance
        /// </summary>
        /// <param name="reader"> Reader initalized by method called by user. </param>
        /// <returns> A major. </returns>
        private Major LoadMajorInstance(SqlDataReader reader) {
            var id = Convert.ToInt32(reader[Major.ID]);
            var major = new Major(id);
            major.Code = reader[Major.CODE].ToString();
            major.Description = reader[Major.DESCRIPTION].ToString();
            major.MinSAT = Convert.ToInt32(reader[Major.MINSAT]);
            return major;
        }

        /// <summary>
        /// Allows user to select a Major by code.
        /// </summary>
        /// <param name="Code"> User must pass code of Major they are looking for. </param>
        /// <returns> null if doesn't exist. Major if does. </returns>
        public Major SelectByCode(string Code) {
            var sqlCmd = new SqlCommand(Major.SelectByCode, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue(Major.CODE, Code);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var major = LoadMajorInstance(reader);
            reader.Close();
            return major;
        }

        /// <summary>
        /// Allows user to call one major by its PK.
        /// </summary>
        /// <param name="Id"> ID of major user is looking for. </param>
        /// <returns> NULL if no ID | major if exists. </returns>
        public Major SeleceByPK(int Id) {
            var sqlCmd = new SqlCommand(Major.SelectByPK, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            var major = LoadMajorInstance(reader);
            reader.Close();
            return major;
        }

        /// <summary>
        /// Allows user to get record of all majors in table.
        /// </summary>
        /// <returns> List of all majors in table. </returns>
        public IEnumerable <Major> SelectAll() {
            var sqlCmd = new SqlCommand(Major.SelectAll, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            var majors = new List<Major>();
            while (reader.Read()) {
                var major = LoadMajorInstance(reader);
                majors.Add(major);
            }
            reader.Close();
            return majors;
        }


        /// <summary>
        /// Allows user to insert a new major into the table. 
        /// </summary>
        /// <param name="major"> User must pass name of new major. </param>
        /// <returns >false if no rows affected, Except if many records, true if worked.</returns>
        public bool Insert(Major major) {
            var sqlStmt = Major.INSERT;
            var sqlCmd = CreateCommandFillPerameters(major, sqlStmt);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }
    
        /// <summary>
        /// Allows user to update contents of any major row.
        /// </summary>
        /// <param name="major"> User must call major by name. </param>
        /// <returns> false if doesn't exist, Except if many records, true if worked. </returns>
        public bool Update (Major major) {
            var sqlStmt = Major.UPDATE;
            var sqlCmd = CreateCommandFillPerameters(major, sqlStmt);
            sqlCmd.Parameters.AddWithValue(Major.ID, major.Id);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
            }
        
    

        /// <summary>
        /// Refactored method to insert or update a major record.
        /// </summary>
        /// <param name="major"> User must pass major to be modified. </param>
        /// <param name="sqlStmt"> User must pass sql command i.e. insert, update. </param>
        /// <returns> Uniform modification command. </returns>
        private SqlCommand CreateCommandFillPerameters(Major major, string sqlStmt) {
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue(Major.CODE, major.Code);
            sqlCmd.Parameters.AddWithValue(Major.DESCRIPTION, major.Description);
            sqlCmd.Parameters.AddWithValue(Major.MINSAT, major.MinSAT);
            return sqlCmd;
        }


        /// <summary>
        /// deletes a major where the id = PK for major
        /// </summary>
        /// <param name="Id"> PK from table </param>
        /// <returns> true if worked, false if 0 rows affected </returns>
        public bool DeleteByPK (int Id) {
            var sqlCmd = new SqlCommand(Major.DeleteMajorByID, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue(Major.ID, Id);
            try {
                int rowsAffected = sqlCmd.ExecuteNonQuery();
                return CheckRowsAffected(rowsAffected);
            } catch (SqlException ex) {
                var refIntEx = new Exceptions.ReferentialIntegrityException
                    ("Cannot delete a major that contains a student", ex);
                throw refIntEx;
            }
        }



    }
}
