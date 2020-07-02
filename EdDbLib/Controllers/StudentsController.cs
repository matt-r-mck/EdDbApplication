using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EdDbLib {
    /// <summary>
    /// Controller allows user to call from and edit Students table.
    /// </summary>
    public class StudentsController : BaseController {

        public StudentsController(Connection connection) : base (connection) {
        }

        /// <summary>
        /// Refactored method: creates SQL connection, passes statement, executes, returns rowsAffected.
        /// </summary>
        /// <param name="sqlStatement">SQL statement from user's method </param>
        /// <returns> rowsAffected </returns>
        private int CreateSqlCommand(string sqlStatement) {
            var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return rowsAffected;
        }


        /// <summary>
        /// Method allows user to delete specific student by ID
        /// </summary>
        /// <param name="Id">Student ID from table</param>
        /// <returns>Exception if no student, true if successful.</returns>
        public bool Delete(int Id) {
            var sqlStatement = $"DELETE From Student where ID = {Id}";
            //Creates a new SQL command on this connection and passes the SQL statement
            var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            return CheckRowsAffected(rowsAffected);
        }



        /// <summary>
        /// Method allows user to update any column info for any student.
        /// </summary>
        /// <param name="student">Must call student by name from table to modify</param>
        /// <returns>Except if afffects > 1 row, true if successful.</returns>
        public bool Update(Student student) {
            var majorId = (student.MajorId == null) ? "NULL" : $"{student.MajorId}";
            var sqlStatement = "UPDATE Student Set " +
                $"Firstname = '{student.Firstname}', " +
                $"Lastname = '{student.Lastname}', " +
                $"StateCode = '{student.StateCode}', " +
                $"SAT = {student.SAT}, " +
                $"GPA = {student.GPA}, " +
                $"MajorId = {majorId}" +
                $"Where Id = {student.Id};";
            //Refactored method here creates SQL connection, passes statement, executes, returns rowsAffected.
            int rowsAffected = CreateSqlCommand(sqlStatement);
            return CheckRowsAffected(rowsAffected);

        }

        /// <summary>
        /// Allows user to insert a student without a declared major.
        /// Also supports insert method to insert student with declared major.
        /// </summary>
        /// <param name="student">Name of student to be inserted. </param>
        /// <returns>Except if affects > 1 row, true if successful.</returns>
        public bool Insert(Student student) {
            var majorId = "NULL";
            if(student.MajorId != null) {
                majorId = $"{student.MajorId}";
            }
            var sqlStatement = "INSERT Student" +
                "(Firstname, Lastname, StateCode, SAT, GPA, MajorId)" +
                "VALUES" +
                $"('{student.Firstname}', '{student.Lastname}', '{student.StateCode}', {student.SAT}, {student.GPA}, {majorId});";
            int rowsAffected = CreateSqlCommand(sqlStatement);
            return CheckRowsAffected(rowsAffected);
        }

        /// <summary>
        /// Allows user to insert a student with declared major.
        /// Passes student and major into origonal insert method.
        /// </summary>
        /// <param name="student">Name of student to be added</param>
        /// <param name="MajorCode">Student's declared major</param>
        /// <returns></returns>
        public bool Insert(Student student, string MajorCode) {
            MajorsController majorCtrl = new MajorsController(Connection);
            var major = majorCtrl.SelectByCode(MajorCode);
            //if major is null, set to null. otherwise set to Id.
            student.MajorId = major?.Id;
            //passes this student instance into insert method
            return Insert(student);
        }

        /// <summary>
        /// Refactored method supports all call functions to student table.
        /// Reads entire student table and returns entire record.
        /// User may also call one student.
        /// </summary>
        /// <param name="reader">SQL command from user's statement</param>
        /// <returns>Student records called by user.</returns>
        private static Student ReadEntireStudent(SqlDataReader reader) {
            var id = Convert.ToInt32(reader["Id"]);
            var student = new Student(id);
            student.Firstname = reader["FirstName"].ToString();
            student.Lastname = reader["LastName"].ToString();
            student.StateCode = reader["StateCode"].ToString();
            student.SAT = Convert.ToInt32(reader["SAT"]);
            student.GPA = Convert.ToDecimal(reader["GPA"]);
            student.MajorId = null;
            if (!reader["MajorId"].Equals(DBNull.Value)) {
                student.MajorId = Convert.ToInt32(reader["MajorId"]);
            }

            return student;
        }

        /// <summary>
        /// Allows user to call one student from the table by their PK
        /// </summary>
        /// <param name="Id">ID of student to be called</param>
        /// <returns>NULL if no record, Student if exists.</returns>
        public Student SelectByPK(int Id) {
            var sqlCmd = new SqlCommand(Student.SelectByPK, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            //Can't find a student
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }
            //Found a student
            reader.Read();
            Student student = ReadEntireStudent(reader);
            reader.Close();
            return student;


        }

        /// <summary>
        /// Allows user to call all students from table.
        /// </summary>
        /// <returns>List of all students from student table</returns>
        public IEnumerable<Student> SelectAll() {
            SqlCommand sqlCommand = new SqlCommand(Student.SelectAll, Connection.sqlConnection);
            var reader = sqlCommand.ExecuteReader();
            var students = new List<Student>(); 
            while (reader.Read()) {
                ReadEntireStudent(reader);
            }
            reader.Close();
            return students;
        }



    }
}
