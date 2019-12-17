using Graph.Application.Graph.Common;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.User.Types.Input
{
    public class UserFilterType : InputObjectGraphType
    {
        public UserFilterType()
        {
            Field<FilterType>("name");
            Field<FilterType>("email");
        }
    }
}
