using Graph.Infrastructure.Database.Repository;
using Graph.Infrastructure.Database.Repository.Interfaces;
using Graph.Infrastructure.Database.Command;
using Graph.Infrastructure.Database.Command.Model;

namespace Graph.Infrastructure.Database.Command.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
    }
}
