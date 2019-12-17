using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Commands.Project
{
    public class AddProjectCommand : CommandBase<bool>
    {
        public string Description { get; set; }
        public string LongDescription { get; set; }
    }
}
