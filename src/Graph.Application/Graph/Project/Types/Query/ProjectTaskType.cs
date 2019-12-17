using GraphQL.Types;
using Model = Graph.Infrastructure.Database.Query.ProjectSchema;

namespace Graph.Application.Graph.Project.Types.Query
{
    public class ProjectTaskType : ObjectGraphType<Model.ProjectTask>
    {
        public ProjectTaskType()
        {
            Field(i => i.Id);
            Field(i => i.Description);
            Field(i => i.Status);
            Field(i => i.Responsible);
        }
    }
}
