using Graph.Infrastructure.Database.Command.Interfaces;
using Graph.Infrastructure.Database.Command.Model;
using Moq;
using System;
using System.Collections.Generic;
using Graph.Tests.Comparers;
using System.Linq;
using System.Text;
using Thread = System.Threading.Tasks;
using Graph.Domain.Service.CommandHandler;
using MediatR;
using Graph.Application.Commands.Project;
using FluentAssertions;
using Xunit;
using Graph.CrossCutting.Exceptions;

namespace Graph.Tests
{
    public class ProjectCommandHandlerTest
    {
        private readonly Guid GatesId;
        private readonly Guid MarkId;
        private readonly Guid WindowsProjectId;
        private readonly Guid FacebookId;
        private readonly Guid FacebookEmployeeId;
        private readonly Guid FacebookTaskId;
        private readonly Guid WindowsTaskId;

        public ProjectCommandHandlerTest()
        {
            GatesId = Guid.NewGuid();
            MarkId = Guid.NewGuid();
            WindowsProjectId = Guid.NewGuid();
            FacebookId = Guid.NewGuid();
            FacebookEmployeeId = Guid.NewGuid();
            FacebookTaskId = Guid.NewGuid();
            WindowsTaskId = Guid.NewGuid();

        }

        [Theory(DisplayName = "Add valid projects")]
        [Trait("Command", "Project")]
        [InlineData("Windows 95", "1995 windows version")]
        [InlineData("Windows 98", "")]
        [InlineData("Windows ME", "2000 windows version")]
        [InlineData("Windows XP", null)]
        public void ProjectCommandHandler_Handle_AddProjectCommand_AddValidProjects(string description, string longDescription)
        {
            IRequestHandler<AddProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commmand = new AddProjectCommand() { Description = description, LongDescription = longDescription };

            var handle = handler.Handle(commmand, default).Result;

            handle.Should().BeTrue();
        }

        [Theory(DisplayName = "Add invalid projects, must trown an exception")]
        [Trait("Command", "Project")]
        [InlineData("", "1995 windows version")]
        [InlineData(null, "")]
        [InlineData(null, "2000 windows version")]
        [InlineData(null, null)]
        public void ProjectCommandHandler_Handle_AddProjectCommand_AddNewProject(string description, string longDescription)
        {
            IRequestHandler<AddProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commmand = new AddProjectCommand() { Description = description, LongDescription = longDescription };

            Action handle = () => handler.Handle(commmand, default).Wait();

            handle.Should().Throw<ValidationException>();
        }

        [Theory(DisplayName = "Edit an existing project, with valid inputs")]
        [Trait("Command", "Project")]
        [InlineData(true,"Facebook social network", "Social Network")]
        [InlineData(true, "Facebook whatsapp", "")]
        [InlineData(false, "Windows OS", null)]
        [InlineData(false, "Windows 95", "1995 edition")]
        public void ProjectCommandHandler_Handle_UpdateProjectInfoCommand_EditProject_ValidInput(bool isFacebookProject, string description, string longDescription)
        {
            IRequestHandler<UpdateProjectInfoCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new UpdateProjectInfoCommand() { Id = isFacebookProject ? FacebookId : WindowsProjectId, Description = description, LongDescription = longDescription };

            var result = handler.Handle(commandData, default).Result;

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "Edit an existing project, with invalid inputs")]
        [Trait("Command", "Project")]
        [InlineData(true, "", "Social Network")]
        [InlineData(true, null, "")]
        [InlineData(false, null, null)]
        [InlineData(false, null, "1995 edition")]
        public void ProjectCommandHandler_Handle_UpdateProjectInfoCommand_EditProject_InvalidInput(bool isFacebookProject, string description, string longDescription)
        {
            IRequestHandler<UpdateProjectInfoCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new UpdateProjectInfoCommand() { Id = isFacebookProject ? FacebookId : WindowsProjectId, Description = description, LongDescription = longDescription };

            Action handle = () => handler.Handle(commandData, default).Wait();

            handle.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "Add an user to a project")]
        [Trait("Command", "Project")]
        public void ProjectCommandHandler_Handle_AddUserProjectCommand_AddValidUserValidProject()
        {
            IRequestHandler<AddUserProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserProjectCommand() { ProjectId = FacebookId, UserId = GatesId };

            var handle = handler.Handle(commandData, default).Result;

            handle.Should().BeTrue();
        }

        [Fact(DisplayName = "Add an user to a project that him is already part of, must throw an exception")]
        [Trait("Command", "Project")]
        public void ProjectCommandHandler_Handle_AddUserProjectCommand_AddInvalidUserRepeatedProject()
        {
            IRequestHandler<AddUserProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserProjectCommand() { ProjectId = FacebookId, UserId = MarkId };

            Action handle = () => handler.Handle(commandData, default).Wait();

            handle.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "Add a valid user to an inexistent project, must throw an exception")]
        [Trait("Command", "Project")]
        public void ProjectCommandHandler_Handle_AddUserProjectCommand_AddValidUserInvalidProject()
        {
            IRequestHandler<AddUserProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserProjectCommand() { ProjectId = Guid.NewGuid(), UserId = MarkId };

            Action handle = () => handler.Handle(commandData, default).Wait();

            handle.Should().Throw<ElementNotFoundException>();
        }

        [Fact(DisplayName = "Add an invalid user to a valid project, must throw an exception")]
        [Trait("Command", "Project")]
        public void ProjectCommandHandler_Handle_AddUserProjectCommand_AddInvalidUserValidProject()
        {
            IRequestHandler<AddUserProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new AddUserProjectCommand() { ProjectId = FacebookId, UserId = Guid.NewGuid() };

            Action handle = () => handler.Handle(commandData, default).Wait();

            handle.Should().Throw<ElementNotFoundException>();
        }

        [Fact(DisplayName = "Remove an user of a project")]
        [Trait("Command", "Project")]
        public void ProjectCommandHandler_Handle_RemoveUserProjectCommand_ValidUserValidProject()
        {
            IRequestHandler<RemoveUserProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new RemoveUserProjectCommand() { ProjectId = FacebookId, UserId = MarkId };

            var handle = handler.Handle(commandData, default).Result;

            handle.Should().BeTrue();
        }

        [Fact(DisplayName = "Remove an user of a project that him is not part of, must thrown an exception")]
        [Trait("Command", "Project")]
        public void ProjectCommandHandler_Handle_RemoveUserProjectCommand_ValidUserNotInProject()
        {
            IRequestHandler<RemoveUserProjectCommand, bool> handler = GetCommandHandlerInstance();
            var commandData = new RemoveUserProjectCommand() { ProjectId = FacebookId, UserId = GatesId };

            Action handle = () => handler.Handle(commandData, default).Wait();

            handle.Should().Throw<ValidationException>();
        }

        private ProjectCommandHandler GetCommandHandlerInstance()
        {
            var mapper = MockHelper.GetMapper();
            var serviceBus = MockHelper.GetServiceBus();
            var unitOfWork = MockHelper.GetUnitOfWork();
            var userRepository = MockHelper.GetUserRepository(new UserComparer(), new[] { GatesId, MarkId, FacebookEmployeeId, WindowsProjectId, FacebookId });
            var projectRepository = MockHelper.GetProjectRepository(new ProjectComparer(), new[] { GatesId, MarkId, FacebookEmployeeId, WindowsProjectId, FacebookId, WindowsTaskId, FacebookTaskId });

            return new ProjectCommandHandler(unitOfWork, serviceBus, projectRepository, userRepository, mapper);
        }

        
    }

    
}
