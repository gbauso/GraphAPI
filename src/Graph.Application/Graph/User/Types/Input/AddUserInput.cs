using Graph.Application.Commands;
using Graph.Application.Commands.User;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Types.Input
{
    public class AddUserInput : InputObjectGraphType<AddUserCommand>
    {
        public AddUserInput()
        {
            Name = "UserInput";
            Field(f => f.Name);
            Field(f => f.Email);
        }
    }
}
