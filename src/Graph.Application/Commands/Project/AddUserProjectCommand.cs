using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Project
{
    public class AddUserProjectCommand : CommandBase<bool>
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}
