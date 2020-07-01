using EdDbLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Text;

namespace EdDbLib {
    public class MajorsController {

        public Connection Connection { get; private set; } = null;


        /// <summary>
        /// refactoring of repeated code 
        /// </summary>
        /// <param name="reader"> must pass in reader you are using </param>
        /// <returns></returns>
        private Major LoadMajorInstance(SqlDataReader reader) {
            var id = Convert.ToInt32(reader["Id"]);
            var major = new Major(id);
            major.Code = reader["Code"].ToString();
            major.Description = reader["Description"].ToString();
            major.MinSAT = Convert.ToInt32(reader["MinSAT"]);
            return major;

            //refactoring of
            ////get the pk first
            //var id = Convert.ToInt32(reader["Id"]);
            ////create instance of major using pk in constructor because is provate
            //var major = new Major(id);
            ////load all the data into each column
            //major.Code = reader["Code"].ToString();
            //major.Description = reader["Description"].ToString();
            //major.MinSAT = Convert.ToInt32(reader["MinSAT"]);
            ////adds instance to collection

        }

        public Major GetByCode(string Code) {
            var sqlStmt = $"SELECT * From Major WHERE Code = @Code;";
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Code", Code);
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



        public bool Insert(Major major) {
            var sqlStmt = "INSERT Major " +
                "(Code, Description, MinSAT)" +
                " VALUES " +
                "(@Code, @Description, @MinSAT);";
            var cmd = CreateCommandFillPerameters(major, sqlStmt);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            switch (rowsAffected) {
                case 0: return false;
                case 1: return true;
                default: throw new Exception($"Insert method affected {rowsAffected} rows");
            }
        }

        private SqlCommand CreateCommandFillPerameters(Major major, string sqlStmt) {
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            sqlCmd.Parameters.AddWithValue("@Code", major.Code);
            sqlCmd.Parameters.AddWithValue("@Description", major.Description);
            sqlCmd.Parameters.AddWithValue("@MinSAT", major.MinSAT);
            return sqlCmd;
        }
    
        public bool Update (Major major) {
            var sqlStmt = "Update Major Set " +
                "Code = @code, " +
                "Description = @Description, " +
                "MinSAT = @MinSAT " +
                "Where Id = @Id;";
            var sqlCmd = CreateCommandFillPerameters(major, sqlStmt);
            sqlCmd.Parameters.AddWithValue("@Id", major.Id);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            switch (rowsAffected) {
                case 0: return false;
                case 1: return true;
                default: throw new Exception($"Update method affected {rowsAffected} rows");
            }
        }


        /// <summary>
        /// deletes a major where the id = PK for major
        /// </summary>
        /// <param name="Id"> PK from table </param>
        /// <returns> true if worked, false if 0 rows affected </returns>
        public bool Delete (int Id) {
            var sqlCmd = new SqlCommand(Major.DeleteMajorByID, Connection.sqlConnection);
            //the parameters for this sql command define Id
            sqlCmd.Parameters.AddWithValue("@Id", Id);
            try {//need to catch a refintegrity exception
                int rowsAffected = sqlCmd.ExecuteNonQuery();

                switch (rowsAffected) {
                    case 0: return false;
                    case 1: return true;
                    default: throw new Exception($"Delete method affected {rowsAffected} rows");
                }
            }catch (SqlException ex) {
                var refIntEx = new Exceptions.ReferentialIntegrityException
                    ("Cannot delete a major that contains a student", ex);
                throw refIntEx;
            }
        }

        public MajorsController(Connection connection) {
            this.Connection = connection;
        }


        public Major GetByPK(int Id) {
            var sqlStmt = $"SELECT * From Major WHERE ID ={Id}";
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
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


        public List<Major> GetAll() {
            //connection has sql connection instance and it is open
            //create sql command passing sql statement and open sql connection
            var sqlCmd = new SqlCommand(Major.SelectAll, Connection.sqlConnection);
            //execute sql statement and return result set
            var reader = sqlCmd.ExecuteReader();
            //create the collection class instance
            var majors = new List<Major>();
            //read moves to the next row and returns true if content
            while (reader.Read()) {
                var major = LoadMajorInstance(reader);
                majors.Add(major);
            }
            //closes reader, returns collection
            reader.Close();
            return majors;
        }

    }
}
