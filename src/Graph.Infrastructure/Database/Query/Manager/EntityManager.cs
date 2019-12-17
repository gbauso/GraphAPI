using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Graph.CrossCutting.Extensions.GraphQL;
using Graph.CrossCutting.Interfaces;

namespace Graph.Infrastructure.Database.Query.Manager
{
    public abstract class EntityManager<T> : IEntityManager<T> where T: IQueryModel
    {
        protected readonly IManager<T> _Manager;

        public EntityManager(IManager<T> manager)
        {
            _Manager = manager;
        }

        public Task<IEnumerable<T>> Get(string[] fields, IDictionary<string, GraphFilter> filters, string order, int skip, int take)
        {
            return _Manager.Get(fields, filters, order, skip, take);
        }

        public Task<T> GetById(Guid id, string[] fields)
        {
            return _Manager.GetById(id, fields);
        }

        public Task<bool> Index(T entry)
        {
            return _Manager.Index(entry);
        }

        public Task<bool> Remove(Guid entryId)
        {
            return _Manager.Remove(entryId);
        }
    }
}
