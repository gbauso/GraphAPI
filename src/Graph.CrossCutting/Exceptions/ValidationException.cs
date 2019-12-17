using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.CrossCutting.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {

        }

        public ValidationException(string message) : base(message)
        {

        }

    }
}
