using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Model = Graph.Infrastructure.Database.Query.UserSchema;

namespace Graph.Application.Graph.User.Types.Query
{
    public class UserProjectType : ObjectGraphType<Model.UserProject>
    {
        public UserProjectType()
        {
            Field(i => i.Id);
            Field(i => i.Description);
            Field(i => i.LongDescription);
        }
    }
}
