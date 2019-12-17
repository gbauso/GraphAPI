using Graph.Infrastructure.Database.Repository;
using Graph.Infrastructure.Database.Repository.Interfaces;
using Graph.Infrastructure.Database.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Graph.Infrastructure.Database.Command.Model;
using Microsoft.EntityFrameworkCore;
using Thread = System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Command.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GraphContext context) : base(context)
        {
        }

    }
}
