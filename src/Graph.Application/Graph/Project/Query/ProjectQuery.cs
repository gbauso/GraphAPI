using Graph.Application.Graph.Common;
using Graph.Application.Graph.Project.Types.Input;
using Graph.Application.Graph.Project.Types.Query;
using Graph.CrossCutting.Extensions.GraphQL;
using Graph.Infrastructure.Database.Query.Manager;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Model = Graph.Infrastructure.Database.Query.ProjectSchema;

namespace Graph.Application.Graph.Query
{
    public class ProjectQuery : ObjectGraphType
    {
        public ProjectQuery(IEntityManager<Model.Project> query)
        {
            Func<ResolveFieldContext, string, object> projectById = (context, id) => query.GetById(Guid.Parse(id), context.SubFields.ParseSubFields());

            Func<ResolveFieldContext, object> projects = context => query.Get
                                                                       (
                                                                           context.SubFields.ParseSubFields(),
                                                                           context.GetArgument<IDictionary<string, object>>("filter").ParseArgumentFilter(),
                                                                           context.GetArgument<string>("order"),
                                                                           context.GetArgument<Pagination>("pagination").Skip,
                                                                           context.GetArgument<Pagination>("pagination").Take
                                                                       )
                                                                       .Result;

            FieldDelegate<ProjectType>(
                "project",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the project" }
                ),
                resolve: projectById
            );

            FieldDelegate<ListGraphType<ProjectType>>(
                "projects",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PaginationType>> { Name = "pagination" },
                    new QueryArgument<StringGraphType> { Name = "order" },
                    new QueryArgument<ProjectFilterType> { Name = "filter" }
                ),
                resolve: projects
            );
        }
    }
}
