using System;
using System.Collections.Generic;
using System.Text;
using Graph.Application.Commands.Task;
using GraphQL.Types;

namespace Graph.Application.Graph.Types.Input
{
    public class UpdateDeadlineInput : InputObjectGraphType<UpdateDeadlineCommand>
    {
        public UpdateDeadlineInput()
        {
            Field<StringGraphType>().Name("id");
            Field(i => i.Deadline);
        }
    }
}
