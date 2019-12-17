using FluentAssertions;
using Graph.Application.Commands.Task;
using Graph.Domain.Service.CommandHandler;
using Graph.Tests.Comparers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Graph.CrossCutting.Exceptions;

namespace Graph.Tests
{
    public class TaskCommandHandlerTest
    {
        private readonly Guid GatesId;
        private readonly Guid MarkId;
        private readonly Guid WindowsProjectId;
        private readonly Guid FacebookId;
        private readonly Guid FacebookTaskId;
        private readonly Guid WindowsTaskId;
        private readonly Guid FacebookEmployeeId;

        public TaskCommandHandlerTest()
        {
            GatesId = Guid.NewGuid();
            MarkId = Guid.NewGuid();
            WindowsProjectId = Guid.NewGuid();
            FacebookId = Guid.NewGuid();
            FacebookTaskId = Guid.NewGuid();
            WindowsTaskId = Guid.NewGuid();
            FacebookEmployeeId = Guid.NewGuid();
        }

        [Theory(DisplayName = "Add valid tasks")]
        [Trait("Command", "Task")]
        [InlineData("Fix bug #2232", "Adjust CSS Position", 3, true)]
        [InlineData("Fix bug #2241", null, 5, true)]
        [InlineData("Fix bug #2152", "", 1, false)]
        [InlineData("Fix bug #2985", "Check flags on Back-End", 2, false)]
        public void TaskCommandHandler_Handle_AddTaskCommand_AddValidTasks(string description, string longDescription, int deadlineDaysAfter, bool facebookProject)
        {
            IRequestHandler<AddTaskCommand, bool> handler = GetCommandHandlerInstance();
            var commmand = new AddTaskCommand() 
            { 
                Description = description,
                LongDescription = longDescription,
                DeadLine = DateTime.UtcNow.AddDays(deadlineDaysAfter),
                ProjectId = facebookProject ? FacebookId : WindowsProjectId,
                ReporterId = facebookProject ? MarkId : GatesId,
                AssigneeId = facebookProject ? MarkId : GatesId
            };

            var handle = handler.Handle(commmand, default).Result;

            handle.Should().BeTrue();
        }

        [Theory(DisplayName = "Add invalid tasks, must trown an exception")]
        [Trait("Command", "Task")]
        [InlineData(null, "Adjust CSS Position", 3, true, true)]
        [InlineData("Fix bug #2241", null, -5, true, true)]
        [InlineData("Fix bug #2152", "", 1, true, false)]
        [InlineData("", "Check flags on Back-End", 2, false, true)]
        public void TaskCommandHandler_Handle_AddTaskCommand_AddInvalidValidTasks(string description, string longDescription, int deadlineDaysAfter, bool facebookProject, bool partOfProject)
        {
            IRequestHandler<AddTaskCommand, bool> handler = GetCommandHandlerInstance();
            var commmand = new AddTaskCommand()
            {
                Description = description,
                LongDescription = longDescription,
                DeadLine = DateTime.UtcNow.AddDays(deadlineDaysAfter),
                ProjectId = facebookProject ? FacebookId : WindowsProjectId,
                ReporterId = facebookProject ? MarkId : GatesId,
                AssigneeId = facebookProject && partOfProject? MarkId : GatesId
            };

            Action handle = () => handler.Handle(commmand, default).Wait();

            handle.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "Change assignee that is part of the project")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_ChangeAssigneeCommand_AddValidAssignee()
        {
            IRequestHandler<ChangeAssigneeCommand, bool> handler = GetCommandHandlerInstance();
            var command = new ChangeAssigneeCommand()
            {
                Id = FacebookTaskId,
                NewAssigneeId = FacebookEmployeeId
            };

            var result = handler.Handle(command, default).Result;

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Change assignee that is not part of the project, must throw an exception")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_ChangeAssigneeCommand_AddInvalidAssignee_MustThrow()
        {
            IRequestHandler<ChangeAssigneeCommand, bool> handler = GetCommandHandlerInstance();
            var command = new ChangeAssigneeCommand()
            {
                Id = FacebookTaskId,
                NewAssigneeId = GatesId
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "Choose a valid status for a valid task")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_UpdateTaskStatusCommand_AddValidStatus()
        {
            IRequestHandler<UpdateTaskStatusCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateTaskStatusCommand()
            {
                Id = FacebookTaskId,
                Status = CrossCutting.TaskStatusEnum.DONE
            };

            var result = handler.Handle(command, default).Result;

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Choose a valid status for an invalid task")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_UpdateTaskStatusCommand_AddValidStatusInvalidTask_ThrowException()
        {
            IRequestHandler<UpdateTaskStatusCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateTaskStatusCommand()
            {
                Id = Guid.NewGuid(),
                Status = CrossCutting.TaskStatusEnum.DONE
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ElementNotFoundException>();
        }

        [Fact(DisplayName = "Choose an invalid status for a valid task")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_UpdateTaskStatusCommand_AddInvalidStatusValidTask_ThrowException()
        {
            IRequestHandler<UpdateTaskStatusCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateTaskStatusCommand()
            {
                Id = WindowsTaskId,
                Status = (CrossCutting.TaskStatusEnum)7
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ValidationException>();
        }

        [Theory(DisplayName = "Change deadline in future")]
        [Trait("Command", "Task")]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(88)]
        public void TaskCommandHandler_Handle_UpdateDeadlineCommand_AddValidDeadline(int daysInAdvance)
        {
            IRequestHandler<UpdateDeadlineCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateDeadlineCommand()
            {
                Id = WindowsTaskId,
                Deadline = DateTime.UtcNow.AddDays(daysInAdvance)
            };

            var result = handler.Handle(command, default).Result;

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "Change deadline in past, must throw")]
        [Trait("Command", "Task")]
        [InlineData(-1)]
        [InlineData(-4)]
        [InlineData(-3)]
        [InlineData(-88)]
        public void TaskCommandHandler_Handle_UpdateDeadlineCommand_AddInvalidDeadline_MustThrowException(int daysInAdvance)
        {
            IRequestHandler<UpdateDeadlineCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateDeadlineCommand()
            {
                Id = WindowsTaskId,
                Deadline = DateTime.UtcNow.AddDays(daysInAdvance)
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ValidationException>();
        }

        [Theory(DisplayName = "Change deadline on invalid task, must throw")]
        [Trait("Command", "Task")]
        [InlineData(-1)]
        [InlineData(-4)]
        [InlineData(-3)]
        [InlineData(-88)]
        public void TaskCommandHandler_Handle_UpdateDeadlineCommand_InvalidTask_MustThrowException(int daysInAdvance)
        {
            IRequestHandler<UpdateDeadlineCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateDeadlineCommand()
            {
                Id = Guid.NewGuid(),
                Deadline = DateTime.UtcNow.AddDays(daysInAdvance)
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ElementNotFoundException>();
        }


        [Theory(DisplayName = "Valid Update info of a valid task")]
        [Trait("Command", "Task")]
        [InlineData(true, "Add new Feature #3434", "")]
        [InlineData(true, "Add new Feature #3435", "Implement it accordingly documentation")]
        [InlineData(false, "Add new Feature #3436", "Implement it accordingly documentation")]
        [InlineData(false, "Add new Feature #3437", null)]
        public void TaskCommandHandler_Handle_UpdateTaskInfoCommand_ValidUpdateInfo(bool facebook, string description, string longDescription)
        {
            IRequestHandler<UpdateTaskInfoCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateTaskInfoCommand()
            {
                Id = facebook ? FacebookTaskId : WindowsTaskId,
                Description = description,
                LongDescription = longDescription
            };

            var result = handler.Handle(command, default).Result;

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "Invalid Update info of a valid task")]
        [Trait("Command", "Task")]
        [InlineData(true, "", "")]
        [InlineData(true, null, "Implement it accordingly documentation")]
        [InlineData(false, null, null)]
        [InlineData(false, "", null)]
        public void TaskCommandHandler_Handle_UpdateTaskInfoCommand_InvalidUpdateInfo_ThrowException(bool facebook, string description, string longDescription)
        {
            IRequestHandler<UpdateTaskInfoCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateTaskInfoCommand()
            {
                Id = facebook ? FacebookTaskId : WindowsTaskId,
                Description = description,
                LongDescription = longDescription
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "Valid Update info of an invalid task")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_UpdateTaskInfoCommand_InvalidTask_ThrowException()
        {
            IRequestHandler<UpdateTaskInfoCommand, bool> handler = GetCommandHandlerInstance();
            var command = new UpdateTaskInfoCommand()
            {
                Id = Guid.NewGuid(),
                Description = "Add new Feature #3435",
                LongDescription = "Implement it accordingly documentation"
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ElementNotFoundException>();
        }

        [Fact(DisplayName = "Remove an invalid task, must throw")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_RemoveTaskCommand_InvalidTask_ThrowException()
        {
            IRequestHandler<RemoveTaskCommand, bool> handler = GetCommandHandlerInstance();
            var command = new RemoveTaskCommand()
            {
                Id = Guid.NewGuid()
            };

            Action result = () => handler.Handle(command, default).Wait();

            result.Should().Throw<ElementNotFoundException>();
        }

        [Fact(DisplayName = "Remove a valid task")]
        [Trait("Command", "Task")]
        public void TaskCommandHandler_Handle_RemoveTaskCommand_ValidTask()
        {
            IRequestHandler<RemoveTaskCommand, bool> handler = GetCommandHandlerInstance();
            var command = new RemoveTaskCommand()
            {
                Id = FacebookTaskId
            };

            var result = handler.Handle(command, default).Result;

            result.Should().BeTrue();
        }




        private TaskCommandHandler GetCommandHandlerInstance()
        {
            var mapper = MockHelper.GetMapper();
            var serviceBus = MockHelper.GetServiceBus();
            var unitOfWork = MockHelper.GetUnitOfWork();
            var userRepository = MockHelper.GetUserRepository(new UserComparer(), new[] { GatesId, MarkId, FacebookEmployeeId, WindowsProjectId, FacebookId, FacebookEmployeeId });
            var projectRepository = MockHelper.GetProjectRepository(new ProjectComparer(), new[] { GatesId, MarkId, FacebookEmployeeId, WindowsProjectId, FacebookId, FacebookEmployeeId, WindowsTaskId, FacebookTaskId });
            var taskRepository = MockHelper.GetTaskRepository(new TaskComparer(), new[] { GatesId, MarkId, FacebookEmployeeId, WindowsProjectId, FacebookId, WindowsTaskId, FacebookTaskId, FacebookEmployeeId });

            return new TaskCommandHandler(unitOfWork, serviceBus, taskRepository, userRepository, projectRepository, mapper);
        }


    }
}
