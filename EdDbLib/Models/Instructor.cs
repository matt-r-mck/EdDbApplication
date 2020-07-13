using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EdDbLib {

    /// <summary>
    /// Models an instructor in our university.
    /// </summary>
    public class Instructor {

        public const string ID = "Id";
        public const string LASTNAME = "Lastname";
        public const string FIRSTNAME = "Firstname";
        public const string YEARSEXPERIENCE = "YearsExperience";
        public const string ISTENURED = "IsTenured";
        public const string SelectAll = "Select * From Student";
        public const string SelectByPK = "Select * from Instructor WHERE Id = {Id}";
        public const string DELETE = "DELETE * from Instructor wher Id = @Id";
        public const string UPDATE = "UPDATE Instructor SET " +
                                        "Firstname = @Firstname, " +
                                        "Lastname = @Lastname, " +
                                        "YearsExperience = @YearsExperince, " +
                                        "IsTenured = @IsTenured, " +
                                        "WHERE Id = @Id;";         
        public const string Insert = "Insert Instructor (Firstname, Lastname, YearsExperience, IsTenured) VALUES " +
                                        "(@Firstname, @Lastname, @YearsExperience, @IsTenured)";

        public int Id { get; private set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int YearsExperience { get; set; }
        public bool IsTenured { get; set; } = false;

        public Instructor(int id) {
            Id = id;
        }

        public Instructor() : this(0) { }


    }
}
