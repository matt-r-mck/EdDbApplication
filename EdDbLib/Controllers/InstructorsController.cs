using EdDbLib.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Text;

namespace EdDbLib {
    public class InstructorsController : BaseController {

        public InstructorsController(Connection connection) : base(connection) {
        }


        private int CreateSqlCommand(string sqlStatement) {
            var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected;
        }

        private static Instructor ExecuteReader(SqlDataReader reader{
            var id = Convert.ToInt32(reader[Instructor.ID]);
            var instructor = new Instructor(id);
            instructor.Firstname = reader[Instructor.FIRSTNAME].ToString();
            instructor.Lastname = reader[Instructor.LASTNAME].ToString();
            instructor.YearsExperience = (int)reader[Instructor.YEARSEXPERIENCE];
            instructor.IsTenured = (bool)reader[Instructor.ISTENURED];
            return instructor;
        }

        public IEnumerable<Instructor> SelectAll() {
            SqlCommand cmd = new SqlCommand(Instructor.SelectAll, Connection.sqlConnection);
            var reader = cmd.ExecuteReader();
            var instructors = new List<Instructor>();
            while (reader.Read()) {
                ExecuteReader(reader);
            }
            reader.Close();
            return instructors;
        }

        public Instructor SelectByPK(int id) {
            SqlCommand cmd = new SqlCommand(Instructor.SelectByPK, Connection.sqlConnection);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            reader.Read();
            Instructor instructor = ExecuteReader(reader);
            reader.Close();
            return instructor;

        }

        public bool Delete(int id) {
            var sqlStmt = Instructor.DELETE;
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Insert(Instructor instructor) {
            var sqlStmt = Instructor.Insert;
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }

        public bool Update(Instructor instructor) {
            var sqlStmt = Instructor.UPDATE;
            var sqlCmd = new SqlCommand(sqlStmt, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }
    }
}
