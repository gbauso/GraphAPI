using GraphQL.Types;
using Model = Graph.Infrastructure.Database.Query.TaskSchema;

namespace Graph.Application.Graph.Project.Types.Query
{
    public class TaskProjectType : ObjectGraphType<Model.TaskProject>
    {
        public TaskProjectType()
        {
            Field(i => i.Id);
            Field(i => i.Description);
        }
    }
}
