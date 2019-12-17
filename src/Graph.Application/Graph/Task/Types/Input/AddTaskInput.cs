using Graph.Application.Commands.Project;
using Graph.Application.Commands.Task;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Types.Input
{
    public class AddTaskInput : InputObjectGraphType<AddTaskCommand>
    {
        public AddTaskInput()
        {
            Field(i => i.Description);
            Field(i => i.LongDescription);
            Field(i => i.DeadLine);
            Field<StringGraphType>("projectId");
            Field<StringGraphType>("reporterId");
            Field<StringGraphType>("assigneeId");

        }
    }
}
