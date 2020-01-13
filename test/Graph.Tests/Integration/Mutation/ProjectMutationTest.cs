using FluentAssertions;
using Graph.Application.Graph.Mutation;
using GraphQL;
using GraphQL.Types;
using Moq;
using Xunit;

namespace Graph.Tests
{
    public class ProjectMutationTest
    {
        private readonly IDocumentExecuter _Executer;
        private readonly ISchema _Schema;

        public ProjectMutationTest(IDocumentExecuter executor, ProjectMutation mutation)
        {
            _Executer = executor;

            var schemaMock = new Mock<Schema>().Object;
            schemaMock.Mutation = mutation;
            _Schema = schemaMock;
        }

        [Fact(DisplayName = "Add a valid project")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectMutation_AddProject_ValidProject()
        {
            var dataJson = "{ description: \"Project\", longDescription: \"Scrum Project\" }";

            var request = "mutation { addProject(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Edit a valid project")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectMutation_EditProject_ValidProject()
        {
            var dataJson = "{ id: \"" + MockHelper.Guids[3] + "\", description: \"Project\", longDescription: \"Kanban Project\" }";

            var request = "mutation { updateProjectInfo(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Add user to a valid project")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectMutation_AddUserProject_ValidProject()
        {
            var dataJson = "{ projectId: \"" + MockHelper.Guids[3] + "\", userId: \"" + MockHelper.Guids[2] + "\" }";

            var request = "mutation { addUserProject(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Remove user to a valid project")]
        [Trait("Integration", "Project")]
        public async void DocumentExecuter_ProjectMutation_RemoveUserProject_ValidProject()
        {
            var dataJson = "{ projectId: \"" + MockHelper.Guids[4] + "\", userId: \"" + MockHelper.Guids[1] + "\" }";

            var request = "mutation { removeUserProject(data: " + dataJson + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

    }
}
