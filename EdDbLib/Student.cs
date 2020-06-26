using System;

namespace EdDbLib {
    public class Student {
        public int Id { get; private set; } //doesnt allow users to change ID, sets ID to 0 by default
        public string Firstname {  get; set; }
        public string Lastname { get; set; }
        public string StateCode { get; set; }
        public int SAT { get; set; }
        public decimal GPA { get; set; }
        public int? MajorId { get; set; }

        public Student(int id) { //needs a constructor that calls for ID
            this.Id = id;
        }

        public Student() : this(0) {
        }

    }



}
