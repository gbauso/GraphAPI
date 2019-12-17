using Graph.Infrastructure.Database.Repository;
using Graph.Infrastructure.Database.Command.Interfaces;
using System.Linq;
using Graph.Infrastructure.Database.Command.Model;
using Thread = System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Graph.CrossCutting.Extensions;

namespace Graph.Infrastructure.Database.Command.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(GraphContext context) : base(context)
        {
        }

        public async Thread.Task AddUser(UserProject userProject)
        {
            await _Context.UserProjects.AddAsync(userProject);
        }

        public Thread.Task RemoveUser(UserProject userProject)
        {
            ClearChangeTrack<UserProject>();
            
            _Context.UserProjects.Remove(userProject);

            return Thread.Task.CompletedTask;
        }
    }
}
