using System;

namespace EdDbLib {
    /// <summary>
    /// This class models a student in our university.
    /// Contains all parameters from student table.
    /// Allows user to create instance of student w/ or w/o ID.
    /// </summary>
    public class Student {

        //doesnt allow users to change ID, sets ID to 0 by default
        public int Id { get; private set; } 
        public string Firstname {  get; set; }
        public string Lastname { get; set; }
        public string StateCode { get; set; }
        public int SAT { get; set; }
        public decimal GPA { get; set; }
        public int? MajorId { get; set; }
        
        public const string SelectByPK = "Select * From Student where ID = {Id}";
        public const string SelectAll = "Select * from Student;";

        //needs a constructor that calls for ID so you can find row by ID
        public Student(int id) { 
            Id = id;
        }

        //sets id to zero if user doesnt pass value
        public Student() : this(0) {
        }

    }



}
