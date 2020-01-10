using System;
using System.Collections.Generic;
using Graph.Infrastructure.Database.Query.UserSchema;
using Microsoft.Extensions.Options;

namespace Graph.Infrastructure.Database.Query.Manager
{
    public class UserManager : EntityManager<User>
    {
        public UserManager(IManager<User> manager) : base(manager)
        {
        }
    }
}
