using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Task
{
    public class RemoveTaskCommand : CommandBase<bool>
    {
        public Guid Id { get; set; }
    }
}
