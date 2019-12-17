using Graph.Application.Commands;
using Graph.Application.Commands.Project;
using Graph.Application.Graph.Common;
using Graph.Application.Graph.Project.Types.Input;
using Graph.Application.Graph.User.Types;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Mutation
{
    public class ProjectMutation : ObjectGraphType
    {
        public ProjectMutation(IServiceProvider serviceProvider)
        {
            Name = "projectMutation";
            Field<MutationResultType>(
                "addProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AddProjectInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<AddProjectCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "addUserProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserProjectInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<AddUserProjectCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "removeUserProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserProjectInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<RemoveUserProjectCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "updateProjectInfo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UpdateDescriptionInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<UpdateProjectInfoCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });
        }
    }
}
