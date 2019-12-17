using Graph.CrossCutting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Task
{
    public class UpdateTaskStatusCommand : CommandBase<bool>
    {
        public Guid Id { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
