using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace EdDbLib {
    public class StudentsController {

        public Connection Connection { get; private set; } = null;


        public bool Delete(int Id) {
            var sqlStatement = $"DELETE From Student where ID = {Id}";
            var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            if (rowsAffected != 1) {
                throw new Exception($"Delete method affected {rowsAffected} row(s).");
            }
            return true;
        }




        public bool Update(Student student) {
        var majorId = (student.MajorId == null) ? "NULL" : $"{student.MajorId}";
            var sqlStatement = "UPDATE Student Set " +
                $"Firstname = '{student.Firstname}'," +
                $"Lastname = '{student.Lastname}'," +
                $"StateCode = '{student.StateCode}'," +
                $"SAT = {student.SAT}," +
                $"GPA = {student.GPA}," +
                $"MajorId = {majorId}" +
                $"Where Id = {student.Id};";
        var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
        var rowsAffected = sqlCmd.ExecuteNonQuery();
        if (rowsAffected != 1) {
            throw new Exception($"Insert method affected {rowsAffected} row(s).");
        }
        return true;
    
}

        public bool Insert(Student student, string MajorCode) {
            var majorCtrl = new MajorsController(this.Connection);
            var major = majorCtrl.GetByCode(MajorCode);
            //if major is null, set to null. otherwise set to Id.
            student.MajorId = major?.Id;
            //passes this method into insert method
            return Insert(student);
        }

            public bool Insert(Student student) {
            var majorId = "NULL";
            if(student.MajorId != null) {
                majorId = $"{student.MajorId}";
            }
            var sqlStatement = "INSERT Student" +
                "(Firstname, Lastname, StateCode, SAT, GPA, MajorId)" +
                "VALUES" +
                $"('{student.Firstname}', '{student.Lastname}', '{student.StateCode}', {student.SAT}, {student.GPA}, {majorId});";
            var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
            var rowsAffected = sqlCmd.ExecuteNonQuery();
            if(rowsAffected != 1) {
                throw new Exception($"Insert method affected {rowsAffected} row.");
            }
            return true;
        }

        public Student GetByPK(int Id) {//looks for one student by pk
            var sqlStatement = $"Select * From Student where ID = {Id}";
            var sqlCmd = new SqlCommand(sqlStatement, Connection.sqlConnection);
            var reader = sqlCmd.ExecuteReader();
            if (!reader.HasRows) {
                reader.Close();
                return null;
            }//found a student
            reader.Read();
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
            reader.Close();
            return student;


        }

        public List<Student> GetAll() {//creates list of students
            var sqlStatement = "Select * from Student;"; //what sql will select
            var sqlCommand = new SqlCommand(sqlStatement, Connection.sqlConnection);//command to make selection on conneciton using our connection class
            var reader = sqlCommand.ExecuteReader();//initalizes reader that reads what the command asked for
            var students = new List<Student>(); //creates a list of students from select
            while (reader.Read()) {//reads each line
                var id = Convert.ToInt32(reader["Id"]); //reads id and converts to int
                var student = new Student(id); //passes id into new student instance
                student.Firstname = reader["FirstName"].ToString(); //read column, pass value into each student
                student.Lastname = reader["LastName"].ToString();
                student.StateCode = reader["StateCode"].ToString();
                student.SAT = Convert.ToInt32(reader["SAT"]);
                student.GPA = Convert.ToDecimal(reader["GPA"]);
                student.MajorId = null;
                if (!reader["MajorId"].Equals(DBNull.Value)) { //reads or sets major id
                    student.MajorId = Convert.ToInt32(reader["MajorId"]);
                }
                students.Add(student);  //passes each student into list of students
            }
            reader.Close();
            return students;
        }

        public StudentsController(Connection connection) {
            this.Connection = connection;
        }

    }
}
