using Graph.CrossCutting.Extensions.GraphQL;
using Graph.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Query
{
    public interface IManager<T> where T: IQueryModel
    {
        Task<bool> Index(T entry);
        Task<bool> Remove(Guid entryId);
        Task<T> GetById(Guid id, string[] fields);
        Task<IEnumerable<T>> Get(string[] fields, IDictionary<string, GraphFilter> filters, string order, int skip, int take);
    }
}
