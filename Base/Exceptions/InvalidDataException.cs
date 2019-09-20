using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Exceptions
{
    public class InvalidDataException : Exception
    {
        public InvalidDataException(string msg) : base(msg)
        {

        }

        public override string StackTrace => "This exception basically is thrown when there is any invalid data according to the business logic of the system";
    }
}
