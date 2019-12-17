using Graph.Infrastructure.Database.Query.TaskSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Infrastructure.Database.Query.Manager
{
    public class TaskManager : EntityManager<Task>, IEntityManager<Task>
    {
        public TaskManager(IManager<Task> manager) : base(manager)
        {
        }
    }
}
