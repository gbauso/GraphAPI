using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Model = Graph.Infrastructure.Database.Query.ProjectSchema;

namespace Graph.Application.Graph.Project.Types.Query
{
    public class ProjectType : ObjectGraphType<Model.Project>
    {
        public ProjectType()
        {
            Field(i => i.Id);
            Field(i => i.Description);
            Field<StringGraphType>().Name("longDescription").Resolve(ctx => ctx.Source.LongDescription ?? string.Empty);
            Field(i => i.FinishedCount);
            Field(i => i.UnfinishedCount);

            Field<ListGraphType<ProjectUserType>>().Name("participants").Resolve((ctx) => ctx.Source.Participants);
            Field<ListGraphType<ProjectTaskType>>().Name("tasks").Resolve((ctx) => ctx.Source.Tasks);
        }
    }
}
