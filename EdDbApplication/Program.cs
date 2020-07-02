using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using EdDbLib;

namespace EdDbApplication {
    class Program {
        static void Main() {
            TestStudentController();
        }


        static void TestMajorsController() {
            var connection = new Connection("localhost", "sqlexpress", "EdDb");
            //passes to connection constructor
            var majorsCtrl = new MajorsController(connection);
            //opens connection

            var majorUBW = majorsCtrl.GetByCode("UBW");


            var major = majorsCtrl.GetByPK(5);
            major.Description = "English";
            var success = majorsCtrl.Update(major);

            var newMajor = new Major() {
                Code = "UBW",
                Description = "Underwater Basket Weaving",
                MinSAT = 1000,
            };

            var itWorked = majorsCtrl.Insert(newMajor);

        }




        static void TestStudentController() {
            var connection = new Connection("localhost", "sqlexpress", "EdDb");
            //user calls shorter constructor. uses connection class
            var studentsCtrl = new StudentsController(connection);
            //opens connection for controller class
           
            
            
            var newStudent = new Student() {
                Firstname = "Fred", 
                //passes info into instance of student
                Lastname = "Flintstone",
                StateCode = "SA",
                SAT = 1000,
                GPA = 2.5m,
                MajorId = null
            };
            var itWorked = studentsCtrl.Insert(newStudent, "UBW");
            //inserts student into EdDb

            //var fred = new Student(61) {
            //    Firstname = "Fredrick",
            //    Lastname = "Flintstone",
            //    StateCode = "SA",
            //    SAT = 1000,
            //    GPA = 2.8m,
            //    MajorId = null
            //};
            // var itWorked = studentsCtrl.Update(fred); //commits update

            // var itWorked = studentsCtrl.Delete(61);

            //var student = studentsCtrl.GetByPK(10);
            //var noStudent = studentsCtrl.GetByPK(-1);
            //var students = studentsCtrl.GetAll();
            //connection.Close();
        }

        static void Test1() {

            var connectionString = @"server=localhost\sqlexpress;database=EdDb;trusted_connection=true;";
            //connection string
            var sqlConnection = new SqlConnection(connectionString);
            //instance of sql, passing connection string
            sqlConnection.Open();
            //call open on that constructor
            if (sqlConnection.State != ConnectionState.Open) {
                throw new Exception("Connection did not open");
                //in case connection fails
            }

            var sql = "Select * from Student;";
            //sql select statement
            var sqlCmd = new SqlCommand(sql, sqlConnection);
            //into sql command that needs a statement and connection on which to operate
            var reader = sqlCmd.ExecuteReader();
            //starts reader

            var students = new List<Student>(100);
            //initalizes new list of up to 100 students

            while (reader.Read()) {
                var id = Convert.ToInt32(reader["Id"]);
                //looks for id, passes into variable
                var student = new Student(id);
                // empty instance of one student passing ID from database
                student.Firstname = reader["FirstName"].ToString(); 
                //sets results from read to values of properties for each student instance during while loop
                student.Lastname = reader["LastName"].ToString();
                student.StateCode = reader["StateCode"].ToString();
                student.SAT = Convert.ToInt32(reader["SAT"]);
                student.GPA = Convert.ToDecimal(reader["GPA"]);
                student.MajorId = null;
                if (!reader["MajorId"].Equals(DBNull.Value)) {
                    //or reader majorid != dbnull...
                    student.MajorId = Convert.ToInt32(reader["MajorId"]);
                }
                students.Add(student);
                //add information from this student to list of students
            }
            reader.Close();
            //stops the reader
            sqlConnection.Close();
            //closes the cocnnection
        }
    }
}