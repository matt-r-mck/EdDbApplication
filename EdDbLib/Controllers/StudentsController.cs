using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace EdDbLib {
    /// <summary>
    /// Controller allows user to call from and edit Students table.
    /// </summary>
    public class StudentsController : BaseController {

        public StudentsController(Connection connection) : base(connection) {
        }


        private SqlCommand CreateAndFillParameters(Student student, string sqlStatement) {
            var sqlCommand = new SqlCommand(sqlStatement, Connection.sqlConnection);
            sqlCommand.Parameters.AddWithValue(Student.FIRSTNAME, student.Firstname);
            sqlCommand.Parameters.AddWithValue(Student.LASTNAME, student.Lastname);
            sqlCommand.Parameters.AddWithValue(Student.sat, student.SAT);
            sqlCommand.Parameters.AddWithValue(Student.gpa, student.GPA);
            sqlCommand.Parameters.AddWithValue(Student.STATECODE, student.StateCode);
            sqlCommand.Parameters.AddWithValue(Student.MAJORID, student.MajorId);
            return sqlCommand;
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
            var sqlStatement = Student.DELETE;
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
            var majorId = (student.MajorId == null) ? "NULL" : Student.MAJORID;
            var sqlStatement = Student.UPDATE;
            var sqlCommand = CreateAndFillParameters(student, sqlStatement);
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
            var sqlStatement = Student.INSERT;
            var sqlCommand = CreateAndFillParameters(student, sqlStatement);
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
            var id = Convert.ToInt32(reader[Student.ID]);
            var student = new Student(id);
            student.Firstname = reader[Student.FIRSTNAME].ToString();
            student.Lastname = reader[Student.LASTNAME].ToString();
            student.StateCode = reader[Student.STATECODE].ToString();
            student.SAT = Convert.ToInt32(reader[Student.sat]);
            student.GPA = Convert.ToDecimal(reader[Student.gpa]);
            student.MajorId = null;
            if (!reader[Student.MAJORID].Equals(DBNull.Value)) {
                student.MajorId = Convert.ToInt32(reader[Student.MAJORID]);
                
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
            var sqlCommand = new SqlCommand(Student.SelectAll, Connection.sqlConnection);
            var reader = sqlCommand.ExecuteReader();
            var students = new List<Student>();
                while (reader.Read()) {
                    var student = ReadEntireStudent(reader);
                    students.Add(student);
                }
                reader.Close();
                return students;
        }

        public IEnumerable<Student> GetByLastName(string startstWith) {
            var students = SelectAll();
            var result = from s in students
                         where s.Lastname.StartsWith(startstWith)
                         orderby s.Lastname
                         select s;
            return result;
        }


        public struct StudentsPerState {
            public string StateCode { get; set; }
            public int Count { get; set; }
        }


        public IEnumerable<StudentsPerState> GetStudentsPerState() {

            var studentsPerState = from s in SelectAll()
                                   group s by s.StateCode into sc
                                   select new StudentsPerState {
                                       StateCode = sc.Key, Count = sc.Count()
                                   };
            return studentsPerState;
        }

        public struct StudentWithMajor {
            public int Id { get; set; }
            public string Fullname { get; set; }
            public string Major { get; set; }
        }

        public IEnumerable<StudentWithMajor> GetStudentWithMajor() {
            var majCtrl = new MajorsController(Connection);
            var studentWithMajor = from s in SelectAll()
                                   join m in majCtrl.SelectAll()
                                   on s.MajorId equals m.Id into sm
                                   from s2 in sm.DefaultIfEmpty()
                                   select new StudentWithMajor {
                                       Id = s.Id, Fullname = $"{s.Firstname} {s.Lastname}", Major = s2?.Description ?? "Undeclared"
                                   };
            return studentWithMajor;

        }
    }
}
