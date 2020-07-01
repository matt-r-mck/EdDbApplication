using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EdDbLib {
    public class Major {

        public const string SelectAll = "SELECT * From Major;";
        public const string SelectByPK = "SELECT * From Major WHERE ID ={Id}";
        public const string DeleteMajorByID = "DELETE from Major where ID = @Id;";

        public int Id { get; private set; } = 0;

        private string _code = string.Empty; //stops user from imputting more than 4 characters to code
        public string Code {
            get{return _code;}
            set {
                if(value.Length > 4) {
                    throw new Exception("Code length must be <= 4 characters");
                }
                else _code = value;
            }
        }

        private string _description = string.Empty;
        public string Description {
            get { return _description;}
            set {
                if (value.Length > 50) {
                    throw new Exception("Description length must be <= 50 characters");
                } else _description = value;
            }
        }

        public int _minSAT = 400;
        public int MinSAT {
            get => _minSAT;
            set {
                if (value < 400 || value > 1600) {
                    throw new Exception("MinSAT must between btw 400 and 1600.");
                    }
                _minSAT = value;
            }
        }
        
        public Major(int id) {
            this.Id = id;
        }

        public Major() {
        }

    }
}
