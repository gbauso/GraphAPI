using System;
using System.Collections.Generic;
using System.Text;
using Graph.Application.Commands.Task;
using Graph.Application.Graph.Common;
using GraphQL.Types;

namespace Graph.Application.Graph.Types.Input
{
    public class UpdateTaskStatusInput : InputObjectGraphType<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusInput()
        {
            Field<StringGraphType>().Name("id");
            Field<TaskStatusEnumType>().Name("status");
        }
    }
}
