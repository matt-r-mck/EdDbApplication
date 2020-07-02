using System;
using System.Collections.Generic;
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

        public int Id { get; private set; } = 0;
        
        private string _code = string.Empty;
        public string Code {
            get { return _code; }
            set {
                if (value.Length > 4) {
                    throw new Exception("Code length must be <= 4 characters");
                } else _code = value;
            }
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
        public int? InstructorID { get; set; }


        public Class() : this (0) {}

        public Class(int id) {
            this.Id = id;
        }

    }
}
