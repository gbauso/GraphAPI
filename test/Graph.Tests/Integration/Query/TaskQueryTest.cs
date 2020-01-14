using FluentAssertions;
using Graph.Application.Graph.Query;
using Graph.Infrastructure.Database.Query.TaskSchema;
using GraphQL;
using GraphQL.Types;
using Moq;
using System.Collections.Generic;
using Xunit;
using AutoMapper;
using System.Linq;

namespace Graph.Tests
{
    public class TaskQueryTest
    {
        private readonly IDocumentExecuter _Executer;
        private readonly ISchema _Schema;

        public TaskQueryTest(IDocumentExecuter executor, TaskQuery query)
        {
            _Executer = executor;

            var schemaMock = new Mock<Schema>().Object;
            schemaMock.Query = query;
            _Schema = schemaMock;
        }

        [Fact(DisplayName = "Invalid Task Query ( without pagination )")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskQuery_InvalidQuery_MissingPagination()
        {
            var request = "query { tasks() { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Invalid Task Query ( without taskion )")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskQuery_InvalidQuery_MissingTaskion()
        {
            var request = "query { tasks(pagination: { skip: 0, take: 10 }) { } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Get valid tasks")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskQuery_GetTasks()
        {
            var request = "query { tasks(pagination: { skip: 0, take: 10 }) { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Get valid tasks with filters")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskQuery_GetTasks_WithFilters()
        {
            var request = "query { tasks(pagination: { skip: 0, take: 10 }, filter: { description: {operation: \"c\", value: \"a\" } }) { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Get a valid task")]
        [Trait("Integration", "Task")]
        public async void DocumentExecuter_TaskQuery_GetTask()
        {
            var request = "query { task(id: \""+ MockHelper.Guids[5] + "\") { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }
    }
}
