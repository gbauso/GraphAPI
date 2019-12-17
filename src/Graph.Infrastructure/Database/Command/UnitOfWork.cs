using Graph.Infrastructure.Database.Command.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Thread = System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Command
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GraphContext _Context;

        public UnitOfWork(GraphContext context)
        {
            _Context = context;
        }
        public async Thread.Task Commit()
        {
            using (var transaction = _Context.Database.BeginTransaction())
            {
                try
                {
                    await _Context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
