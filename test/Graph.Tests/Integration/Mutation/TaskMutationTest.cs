using FluentAssertions;
using Graph.Application.Graph.Mutation;
using GraphQL;
using GraphQL.Types;
using Moq;
using System;
using Xunit;

namespace Graph.Tests
{
    public class TaskMutationTest
    {
        private readonly IDocumentExecuter _Executer;
        private readonly ISchema _Schema;

        public TaskMutationTest(IDocumentExecuter executor, TaskMutation mutation)
        {
            _Executer = executor;

            var schemaMock = new Mock<Schema>().Object;
            schemaMock.Mutation = mutation;
            _Schema = schemaMock;
        }

        [Fact(DisplayName = "Add a valid task")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskMutation_AddTask_ValidTask()
        {
            var dataJson = "{ assigneeId: \"" + MockHelper.Guids[2] + "\", " + 
                             "reporterId: \"" + MockHelper.Guids[1] + "\", " +
                             "projectId : \"" + MockHelper.Guids[4] + "\"," +
                             "description: \"Task\"," +
                             "deadLine: \""+ DateTime.Now.AddDays(5).Date.ToString("yyyy-MM-dd") +"\", " +
                             "longDescription: \"Fix bug #42343\" }";

            var request = "mutation { addTask(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Edit a valid task")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskMutation_EditTask_ValidTask()
        {
            var dataJson = "{ id: \"" + MockHelper.Guids[6] + "\", description: \"Task\", longDescription: \"Fix bug #432163\" }";

            var request = "mutation { updateTaskInfo(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Change assignee of a valid task")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskMutation_ChangeAssignee_ValidTask()
        {
            var dataJson = "{ id: \"" + MockHelper.Guids[6] + "\", newAssigneeId: \"" + MockHelper.Guids[2] + "\" }";

            var request = "mutation { changeAssignee(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Update status of a valid task")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskMutation_UpdateStatus_ValidTask()
        {
            var dataJson = "{ id: \"" + MockHelper.Guids[6] + "\", status: DONE }";

            var request = "mutation { updateTaskStatus(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Update deadline of a valid task")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskMutation_UpdateDeadline_ValidTask()
        {
            var dataJson = "{ id: \"" + MockHelper.Guids[6] + "\", deadline: \"" + DateTime.Now.AddDays(5).Date.ToString("yyyy-MM-dd") + "\"}";

            var request = "mutation { updateDeadline(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

    }
}
