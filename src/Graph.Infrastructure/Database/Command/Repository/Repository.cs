using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using Graph.Infrastructure.Database.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Graph.Infrastructure.Database.Command;
using Graph.CrossCutting.Extensions;
using Graph.CrossCutting.Interfaces;

namespace Graph.Infrastructure.Database.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected GraphContext _Context;

        public Repository(GraphContext context)
        {
            _Context = context;
        }

        public virtual async Task Add(T obj)
        {
            await _Context.Set<T>().AddAsync(obj);
        }

        public virtual Task<IQueryable<T>> GetAll()
        {
            return Task.FromResult(_Context.Set<T>().AsQueryable());
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await _Context.Set<T>().FindAsync(id);
        }

        public virtual Task Remove(T obj)
        {
            ClearChangeTrack<T>();

            return Task.FromResult(_Context.Set<T>().Remove(obj));
        }

        public virtual Task Update(T obj)
        {
            ClearChangeTrack<T>();

            _Context.Set<T>().Update(obj);

            return Task.CompletedTask;
        }

        public async void Dispose()
        {
            await _Context.DisposeAsync();
        }

        protected void ClearChangeTrack<E>() where E : class
        {
            var entry = _Context.ChangeTracker.Entries<E>().FirstOrDefault();
            if (!entry.IsNull()) entry.State = EntityState.Detached;
        }

    }
}
