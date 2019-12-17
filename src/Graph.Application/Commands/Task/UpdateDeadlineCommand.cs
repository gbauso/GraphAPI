using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Task
{
    public class UpdateDeadlineCommand : CommandBase<bool>
    {
        public Guid Id { get; set; }
        public DateTime Deadline { get; set; }
    }
}
