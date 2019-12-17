using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Infrastructure.Database.Repository.Interfaces
{
    public interface IRepository<T> : IDisposable 
    {
        Task Add(T obj);
        Task<T> GetById(Guid id);
        Task<IQueryable<T>> GetAll();
        Task Update(T obj);
        Task Remove(T obj);
    }
}
