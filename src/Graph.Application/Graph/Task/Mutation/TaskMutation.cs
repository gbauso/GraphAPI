using Graph.Application.Commands.Project;
using Graph.Application.Commands.Task;
using Graph.Application.Graph.Common;
using Graph.Application.Graph.Task.Types.Input;
using Graph.Application.Graph.Types.Input;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Mutation
{
    public class TaskMutation : ObjectGraphType
    {
        public TaskMutation(IServiceProvider serviceProvider)
        {
            Field<MutationResultType>(
                "addTask",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AddTaskInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<AddTaskCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "updateTaskInfo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UpdateDescriptionInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<UpdateTaskInfoCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "changeAssignee",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ChangeAssigneeInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<ChangeAssigneeCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "updateDeadline",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UpdateDeadlineInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<UpdateDeadlineCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "updateTaskStatus",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UpdateTaskStatusInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<UpdateTaskStatusCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });
        }
    }
}
