using GraphQL.Types;
using Model = Graph.Infrastructure.Database.Query.TaskSchema;

namespace Graph.Application.Graph.Project.Types.Query
{
    public class TaskUserType : ObjectGraphType<Model.TaskUser>
    {
        public TaskUserType()
        {
            Field(i => i.Id);
            Field(i => i.Name);
        }
    }
}
