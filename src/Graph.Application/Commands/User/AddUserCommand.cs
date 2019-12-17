using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.User
{
    public class AddUserCommand : CommandBase<bool>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
