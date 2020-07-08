using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EdDbLib {
    /// <summary>
    /// This class models a Class in our university
    /// Contains all parameters from Class toable.
    /// Allows User to create instance of a class.
    /// </summary>
    public class Class {


        public const string SelectAll = "Select * from Class;";
        public const string SelectByPK = "Select * from Class WHERE ID = @Id;";
        public const string SelectByCode = "Select * from Class Where Code = @Code;";
        public const string DELETE = "DELETE from Class WHERE ID = @Id;";
        public const string INSERT = "INSERT Class (Code, Subject, Section, InstructorId) VALUES " +
                                        "(@Code, @Subject, @Section, @InstructorId);";
        public const string UPDATE = "UPDATE Student SET " +
                                        "Code = @Code, " +
                                        "Subject = @Subject, " +
                                        "Section = @Section, " +
                                        "InstructorId = @instructorId, " +
                                        "WHERE Id = @Id;";

        public const string ID = "Id";
        public const string CODE = "Code";
        public const string SUBJECT = "Subject";
        public const string SECTION = "Section";
        public const string INSTRUCTORID = "InstructorID";


        public int Id { get; private set; } = 0;
        
        private string _code = string.Empty;
        public string Code {
            get { return _code; }
            set { _code = value; }
            
        }

        private string _subject = string.Empty;
        public string Subject {
            get { return _subject; }
            set {
                if (value.Length > 30) {
                    throw new Exception("Subject length must be <= 30 characters");
                } else _subject = value;
            }
        }

        public int Section { get; set; }
        public int? InstructorId { get; set; }


        public Class() : this (0) {}

        public Class(int id) {
            this.Id = id;
        }

    }
}
