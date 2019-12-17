using Graph.Infrastructure.Database.Query.ProjectSchema;
using Graph.Infrastructure.Database.Query.TaskSchema;

namespace Graph.Application.MessageHandler
{
    public class TaskProjectMessage
    {
        public Project Project { get; set; }
        public Task Task { get; set; }
    }
}
