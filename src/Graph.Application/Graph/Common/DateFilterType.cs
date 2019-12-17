using Graph.CrossCutting.Extensions;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Common
{
    public class DateFilterType : InputObjectGraphType<Filter>
    {
        public DateFilterType()
        {
            Field(f => f.Operation);
            Field(f => f.Value);
        }
    }
}
