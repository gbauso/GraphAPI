using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Project
{
    public class RemoveUserProjectCommand : CommandBase<bool>
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
