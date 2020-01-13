using FluentAssertions;
using Graph.Application.Graph.Query;
using Graph.Infrastructure.Database.Query.UserSchema;
using GraphQL;
using GraphQL.Types;
using Moq;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace Graph.Tests
{
    public class UserQueryTest
    {
        private readonly IDocumentExecuter _Executer;
        private readonly ISchema _Schema;

        public UserQueryTest(IDocumentExecuter executor, UserQuery query)
        {
            _Executer = executor;

            var schemaMock = new Mock<Schema>().Object;
            schemaMock.Query = query;
            _Schema = schemaMock;
        }

        [Fact(DisplayName = "Get valid users")]
        [Trait("Integration", "User")]
        public async void DocumentExecuter_UserQuery_GetUsers()
        {
            var request = "query { users(pagination: { skip: 0, take: 10 }) { id, name } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact(DisplayName = "Get a valid user")]
        [Trait("Integration", "User")]
        public async void DocumentExecuter_UserQuery_GetUser()
        {
            var request = "query { user(id: \""+ MockHelper.Guids[1] +"\") { id, name } }";

            var result = await _Executer.ExecuteAsync(_ =>
            {
                _.Schema = _Schema;
                _.Query = request;
            });

            result.Errors.Should().BeNullOrEmpty();
        }
    }
}
