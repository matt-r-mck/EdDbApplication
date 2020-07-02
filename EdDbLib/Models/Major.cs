using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace EdDbLib {
    /// <summary>
    /// This class models a major in our university.
    /// Contains all parameters from major table.
    /// Allows user to create instance of major w/ or w/o ID.
    /// </summary>
    public class Major {

        /// <summary>
        /// Parameters for common SQL selection statements.
        /// </summary>
        public const string SelectAll = "SELECT * From Major;";
        public const string SelectByPK = "SELECT * From Major WHERE ID ={Id};";
        public const string DeleteMajorByID = "DELETE from Major where ID = @Id;";
        public const string SelectByCode = "SELECT * From Major WHERE Code = @Code;";

        //ID can be called, set to 0 to prevent conflicts with the table.
        public int Id { get; private set; } = 0;

        //Major code parameter | Prevents user from passing > 4 characters.
        private string _code = string.Empty; 
        public string Code {
            get => _code;
            set {
                CheckLength(_code, 4, value);
            }
        }
   

        private void CheckLength(string field, int maxLength, string value) {
            if (value.Length > 4) {
                throw new Exception("Value length must match table.");
            } else field = value;
        }

        //Major description parameter | Prevents user from passing > 50 characters.
        private string _description = string.Empty;
        public string Description {
            get => _description;
            set {
                CheckLength(_description, 4, value);
            }
        }

        //Major Minimum SAT parameter | Prevents user from passing invalid SAT score.
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
        
        //Default constructor to create instance of major.
        public Major() {}
        
        /// <summary>
        /// Allows user to create instance of major with an id.
        /// </summary>
        /// <param name="id"> ID of major passes from user's command. </param>
        public Major(int id) {
            this.Id = id;
        }
    }
}
