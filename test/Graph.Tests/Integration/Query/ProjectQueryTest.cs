using FluentAssertions;
using Graph.Application.Graph.Query;
using Graph.Infrastructure.Database.Query.ProjectSchema;
using GraphQL;
using GraphQL.Types;
using Moq;
using System.Collections.Generic;
using Xunit;
using AutoMapper;
using System.Linq;

namespace Graph.Tests
{
    public class ProjectQueryTest
    {
        private readonly IDocumentExecuter _Executer;
        private readonly ISchema _Schema;

        public ProjectQueryTest(IDocumentExecuter executor, ProjectQuery query)
        {
            _Executer = executor;

            var schemaMock = new Mock<Schema>().Object;
            schemaMock.Query = query;
            _Schema = schemaMock;
        }

        [Fact(DisplayName = "Invalid Project Query ( without pagination )")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectQuery_InvalidQuery_MissingPagination()
        {
            var request = "query { projects() { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Invalid Project Query ( without projection )")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectQuery_InvalidQuery_MissingProjection()
        {
            var request = "query { projects(pagination: { skip: 0, take: 10 }) { } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Get valid projects")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectQuery_GetProjects()
        {
            var request = "query { projects(pagination: { skip: 0, take: 10 }) { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Get valid projects with filters")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectQuery_GetProjects_WithFilters()
        {
            var request = "query { projects(pagination: { skip: 0, take: 10 }, filter: { description: {operation: \"c\", value: \"a\" } }) { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Get a valid project")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectQuery_GetProject()
        {
            var request = "query { project(id: \""+ MockHelper.Guids[3] + "\") { id, description } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }
    }
}
