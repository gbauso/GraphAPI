using FluentAssertions;
using Graph.Application.Graph.Mutation;
using GraphQL;
using GraphQL.Types;
using Moq;
using Xunit;

namespace Graph.Tests
{
    public class UserMutationTest
    {
        private readonly IDocumentExecuter _Executer;
        private readonly ISchema _Schema;

        public UserMutationTest(IDocumentExecuter executor, UserMutation mutation)
        {
            _Executer = executor;

            var schemaMock = new Mock<Schema>().Object;
            schemaMock.Mutation = mutation;
            _Schema = schemaMock;
        }

        [Fact(DisplayName = "Add a valid User")]
        [Trait("Integration", "User")]
        public async void DocumentExecuter_UserMutation_AddUser_ValidUser()
        {
            var dataJson = "{ name: \"user\", email: \"user@email.com\" }";

            var request = "mutation { addUser(data: " + dataJson.ToLower() + " ) { result } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        //[Fact(DisplayName = "Edit a valid User")]
        //[Trait("Integration", "User")]
        //public async void DocumentExecuter_UserMutation_EditUser_ValidUser()
        //{
        //    var dataJson = "{ id: \"" + MockHelper.Guids[0] + "\", name: \"user\", email: \"user@email.com\" }"; 

        //    var request = "mutation { editUserInfo(data: " + dataJson.ToLower() + " ) { result } }";

        //    var result = await _Executer.ExecuteAsync(_ =>
        //    {
        //        _.Schema = _Schema;
        //        _.Query = request;
        //    });

        //    result.Errors.Should().BeNullOrEmpty();
        //}
    }
}
