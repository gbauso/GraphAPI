using System;
using System.Collections.Generic;
using System.Text;
using Thread = System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Command.Interfaces
{
    public interface IUnitOfWork
    {
        Thread.Task Commit();
    }
}
