using Graph.Application.Commands;
using Graph.Application.Commands.Project;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Project.Types.Input
{
    public class UserProjectInput : InputObjectGraphType
    {
        public UserProjectInput()
        {
            Field<IdGraphType>().Name("projectId");
            Field<IdGraphType>().Name("userId");
        }
    }
}
