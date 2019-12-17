using System;
using System.Collections.Generic;
using System.Text;
using Graph.Application.Commands.Task;
using GraphQL.Types;

namespace Graph.Application.Graph.Task.Types.Input
{
    public class ChangeAssigneeInput : InputObjectGraphType<ChangeAssigneeCommand>
    {
        public ChangeAssigneeInput()
        {
            Field<StringGraphType>().Name("id");
            Field<StringGraphType>().Name("newAssigneeId");
        }
    }
}
