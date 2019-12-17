using Graph.Infrastructure.Database.Query.ProjectSchema;
using Graph.Infrastructure.Database.Query.UserSchema;

namespace Graph.Application.MessageHandler
{
    public class ProjectUserMessage
    {
        public Project Project { get; set; }
        public User User { get; set; }
    }
}
