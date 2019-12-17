using Thread = System.Threading.Tasks;
using Graph.Infrastructure.Database.Repository.Interfaces;
using System;
using Graph.Infrastructure.Database.Command.Model;

namespace Graph.Infrastructure.Database.Command.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Thread.Task AddUser(UserProject userProject);
        Thread.Task RemoveUser(UserProject userProject);
    }
}
