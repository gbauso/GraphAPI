using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Common
{
    public class EnumFilterType: InputObjectGraphType<Filter> 
    {
        public EnumFilterType()
        {
            Field(f => f.Operation);
            Field<TaskStatusEnumType>().Name("value");
        }
    }
}
