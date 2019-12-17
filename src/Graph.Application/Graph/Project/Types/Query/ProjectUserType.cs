using GraphQL.Types;
using Model = Graph.Infrastructure.Database.Query.ProjectSchema;

namespace Graph.Application.Graph.Project.Types.Query
{
    public class ProjectUserType : ObjectGraphType<Model.ProjectUser>
    {
        public ProjectUserType()
        {
            Field(i => i.Id);
            Field(i => i.Name);
        }
    }
}
