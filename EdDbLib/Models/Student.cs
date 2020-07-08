using System;
using System.Globalization;

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

        /// <summary>
        /// Constants support students controller, limit number of strings user must pass.
        /// </summary>
        public const string ID = "Id";
        public const string FIRSTNAME = "Firstname";
        public const string LASTNAME = "Lastname";
        public const string ISTENURED = "IsTenured";
        public const string YEARSEXPERIENCE = "YearsExperience";
        public const string STATECODE = "StateCode";
        public const string sat = "SAT";
        public const string gpa = "GPA";
        public const string MAJORID = "MajorID";
        public const string DELETE = "DELETE From Student where ID = @Id;";
        public const string INSERT = "INSERT Instructor" +
                                        "(Firstname, Lastname, StateCode, SAT, GPA, MajorId) VALUES + " +
                                        "(@Firstname, @Lastname, @StateCode, @SAT, @GPA, @MajorId);";
        public const string UPDATE = "UPDATE Student Set " +
                                    "Firstname = @Firstname' " +
                                    "Lastname = @Lastname, " +
                                    "StateCode = @StateCode, " +
                                    "SAT = @SAT, " +
                                    "GPA = @GPA, " +
                                    "MajorId = @MajorId" +
                                    "Where Id = @Id;";


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
