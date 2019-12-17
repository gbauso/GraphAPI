using AutoMapper;
using Graph.Domain.Service.CommandHandler;
using Graph.Domain.Service.Mappings;
using Graph.Infrastructure.Database.Command;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Graph.Infrastructure.Database;
using Microsoft.Extensions.Options;
using Moq;
using Graph.Infrastructure.Database.Command.Interfaces;
using Graph.Infrastructure.ServiceBus;
using Graph.Infrastructure.Database.Command.Repository;
using Xunit;
using Graph.Application.Commands.User;
using MediatR;
using Graph.Infrastructure.Database.Command.Model;
using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Graph.Infrastructure.Database.Repository.Interfaces;
using Graph.CrossCutting.Interfaces;
using Graph.Tests.Comparers;
using Graph.CrossCutting.Exceptions;

namespace Graph.Tests
{
    public class UserCommandHandlerTest
    {
        private readonly Guid GatesId;
        private readonly Guid MarkId;
        private readonly Guid WindowsProjectId;
        private readonly Guid FacebookId;
        private readonly Guid FacebookEmployeeId;

        public UserCommandHandlerTest()
        {
            GatesId = Guid.NewGuid();
            MarkId = Guid.NewGuid();
            WindowsProjectId = Guid.NewGuid();
            FacebookId = Guid.NewGuid();
            FacebookEmployeeId = Guid.NewGuid();
        }

        [Theory(DisplayName = "Add valid users")]
        [Trait("Command", "User")]
        [InlineData("Steve Jobs", "steve@jobs.com")]
        [InlineData("Elon Musk", "elon@musk.com")]
        [InlineData("Warren Buffett", "warren@buffett.com")]
        public void UserCommandHandler_Handle_AddUserCommand_AddValidUsers(string name, string email)
        {
            IRequestHandler<AddUserCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserCommand() { Email = email, Name = name };

            var handlerCall = handler.Handle(commandData, default);

            handlerCall.Result.Should().BeTrue();
        }

        [Theory(DisplayName = "Add invalid users")]
        [Trait("Command", "User")]
        [InlineData("Steve Jobs", "steve_jobs.com")]
        [InlineData("Elon Musk", "elon@musk")]
        [InlineData("", "warren@buffett.com")]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData(null, "")]
        public void UserCommandHandler_Handle_AddUserCommand_AddInvalidUsers(string name, string email)
        {
            IRequestHandler<AddUserCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserCommand() { Email = email, Name = name };

            Action handle = () => handler.Handle(commandData, default).Wait();

            handle.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "Add an existing User")]
        [Trait("Command", "User")]
        public void UserCommandHandler_Handle_AddUserCommand_AddExistingUser()
        {
            IRequestHandler<AddUserCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserCommand() { Email = "bill@gates.com", Name = "Bill Gates" };

            Action action = () => handler.Handle(commandData, default).Wait();

            action.Should().Throw<Exception>();
        }

        [Fact(DisplayName = "Edit an existing User, with a valid email")]
        [Trait("Command", "User")]
        public void UserCommandHandler_Handle_UpdateUserInfoCommand_EditExistingUser_ValidEmail()
        {
            IRequestHandler<UpdateUserInfoCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new UpdateUserInfoCommand() { Id = GatesId, Email = "gates@microsoft.com", Name = "Bill Gates" };

            var result = handler.Handle(commandData, default).Result;

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Edit an existing User, with an invalid email")]
        [Trait("Command", "User")]
        public void UserCommandHandler_Handle_UpdateUserInfoCommand_EditExistingUser_InvalidEmail()
        {
            IRequestHandler<UpdateUserInfoCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new UpdateUserInfoCommand() { Id = MarkId, Email = "bill@gates.com", Name = "Mark Zuckerberg" };

            Action action = () => handler.Handle(commandData, default).Wait();

            action.Should().Throw<Exception>();
        }



        private UserCommandHandler GetCommandHandlerInstance()
        {
            var mapper = MockHelper.GetMapper();
            var serviceBus = MockHelper.GetServiceBus();
            var unitOfWork = MockHelper.GetUnitOfWork();
            var userRepository = MockHelper.GetUserRepository(new UserComparer(), new[] { GatesId, MarkId, FacebookEmployeeId, WindowsProjectId, FacebookId });

            return new UserCommandHandler(unitOfWork, serviceBus, userRepository, mapper);
        }

    }


    

}


