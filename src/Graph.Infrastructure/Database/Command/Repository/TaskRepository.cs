using Graph.Infrastructure.Database.Repository;
using Graph.Infrastructure.Database.Command.Interfaces;
using System;
using Graph.Infrastructure.Database.Command.Model;
using Microsoft.EntityFrameworkCore;
using Thread = System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Command.Repository
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(GraphContext context) : base(context)
        {
        }

    }
}
