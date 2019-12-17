using Graph.Application.Graph.Common;
using Graph.Application.Graph.Project.Types.Input;
using Graph.Application.Graph.Project.Types.Query;
using Graph.Application.Graph.Task.Types.Query;
using Graph.CrossCutting.Extensions.GraphQL;
using Graph.Infrastructure.Database.Query.Manager;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Model = Graph.Infrastructure.Database.Query.TaskSchema;

namespace Graph.Application.Graph.Query
{
    public class TaskQuery : ObjectGraphType
    {
        public TaskQuery(IEntityManager<Model.Task> query)
        {
            Func<ResolveFieldContext, string, object> taskById = (context, id) => query.GetById(Guid.Parse(id), context.SubFields.ParseSubFields());

            Func<ResolveFieldContext, object> tasks = context => query.Get
                                                                       (
                                                                           context.SubFields.ParseSubFields(),
                                                                           context.GetArgument<IDictionary<string, object>>("filter").ParseArgumentFilter(),
                                                                           context.GetArgument<string>("order"),
                                                                           context.GetArgument<Pagination>("pagination").Skip,
                                                                           context.GetArgument<Pagination>("pagination").Take
                                                                       )
                                                                       .Result;

            FieldDelegate<TaskType>(
                "task",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the task" }
                ),
                resolve: taskById
            );

            FieldDelegate<ListGraphType<TaskType>>(
                "tasks",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PaginationType>> { Name = "pagination" },
                    new QueryArgument<StringGraphType> { Name = "order" },
                    new QueryArgument<TaskFilterType> { Name = "filter" }
                ),
                resolve: tasks
            );
        }
    }
}
