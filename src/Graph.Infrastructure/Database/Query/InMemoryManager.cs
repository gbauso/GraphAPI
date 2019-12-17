using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Graph.CrossCutting.Extensions.GraphQL;
using Graph.CrossCutting.Interfaces;

namespace Graph.Infrastructure.Database.Query
{
    public class InMemoryManager<T> : IManager<T> where T : IQueryModel
    {
        protected readonly ICollection<T> MockDatabase;

        public InMemoryManager()
        {
            MockDatabase = new List<T>();
        }

        public Task<IEnumerable<T>> Get(string[] fields, IDictionary<string, GraphFilter> filters, string order, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(Guid id, string[] fields)
        {
            var entry = MockDatabase.FirstOrDefault(i => i.Id == id.ToString());

            var result = (T) Activator.CreateInstance(typeof(T));

            foreach (var field in fields)
                result.GetType()
                            .GetField(field)
                            .SetValue(result, 
                                      entry.GetType().GetField(field).GetValue(entry)
                                      );


            return Task.FromResult(result);
        }

        public Task<bool> Index(T entry)
        {
            MockDatabase.Add(entry);

            return Task.FromResult(true);
        }

        public Task<bool> Remove(Guid entryId)
        {
            var entry = MockDatabase.FirstOrDefault(i => i.Id == entryId.ToString());

            if (entry != null) MockDatabase.Remove(entry);

            return Task.FromResult(true);
        }
    }
}
