using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Common
{
    public class FilterType : InputObjectGraphType<Filter>
    {
        public FilterType()
        {
            Field(f => f.Operation);
            Field(f => f.Value);
        }
    }
}
