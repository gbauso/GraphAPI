using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Task
{
    public class ChangeAssigneeCommand : CommandBase<bool>
    {
        public Guid Id { get; set; }
        public Guid NewAssigneeId { get; set; }
    }
}
