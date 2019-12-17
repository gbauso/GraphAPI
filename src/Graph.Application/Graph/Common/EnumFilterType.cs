using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Common
{
    public class EnumFilterType<E> : InputObjectGraphType<Filter> where E : EnumerationGraphType
    {
        public EnumFilterType()
        {
            Field(f => f.Operation);
            Field<E>().Name("value");
        }
    }
}
