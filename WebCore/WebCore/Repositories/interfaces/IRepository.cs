using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore.Repositories.interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetId(int id);
        Task<int> Create(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(int id);
    }
}
