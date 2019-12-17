using Graph.Application.Commands;
using Graph.Application.Commands.Project;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Project.Types.Input
{
    public class AddProjectInput : InputObjectGraphType<AddProjectCommand>
    {
        public AddProjectInput()
        {
            Field<NonNullGraphType<StringGraphType>>().Name("description");
            Field<StringGraphType>().Name("longDescription");
        }
    }
}
