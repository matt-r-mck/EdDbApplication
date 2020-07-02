using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib {
    class Instructor {

        public int Id { get; private set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int YearsExperience { get; set; }
        public bool IsTenured { get; set; }

        public Instructor(int id) {
            Id = id;
        }

        public Instructor() : this(0) { }


    }
}
