using Graph.Application.Commands;
using Graph.Application.Commands.User;
using Graph.Application.Graph.Common;
using Graph.Application.Graph.Types.Input;
using Graph.Application.Graph.User.Types;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Graph.Application.Graph.Mutation
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(IServiceProvider serviceProvider)
        {
            Name = "UserMutation";
            Field<MutationResultType>(
                "addUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AddUserInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<AddUserCommand>("data");
                    
                    using(var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });

            Field<MutationResultType>(
                "editUserInfo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<EditUserInput>> { Name = "data" }
                ),
                resolve: context =>
                {
                    var command = context.GetArgument<UpdateUserInfoCommand>("data");

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        return mediator.Send(command).Result;
                    }
                });
        }

    }
}
