using System;
using System.Collections.Generic;
using Graph.Infrastructure.Database.Query.ProjectSchema;
using Microsoft.Extensions.Options;

namespace Graph.Infrastructure.Database.Query.Manager
{
    public class ProjectManager : EntityManager<Project>
    {
        public ProjectManager(IManager<Project> manager) : base(manager)
        {
        }
    }
}
