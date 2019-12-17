using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.CrossCutting.Exceptions
{
    public class QueryArgumentException : Exception
    {
        public QueryArgumentException()
        {
                
        }

        public QueryArgumentException(string message) : base(message)
        {

        }
    }
}
