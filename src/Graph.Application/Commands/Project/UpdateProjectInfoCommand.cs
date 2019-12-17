using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Project
{
    public class UpdateProjectInfoCommand : CommandBase<bool>
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
    }
}
