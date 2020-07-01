using System;
using System.Collections.Generic;
using System.Text;

namespace EdDbLib.Exceptions {
        //when you create your own exception, you must inherit from exception
    public class ReferentialIntegrityException : Exception {

        public ReferentialIntegrityException() : base() {} //base class constructor
            
        //passes message into base constructor
        public ReferentialIntegrityException(string Message) : base(Message) { } 
           
        //constgructor takes a method and a sql exception
        public ReferentialIntegrityException(string Message, Exception innerException)
            :base (Message, innerException) { }
    }
}
