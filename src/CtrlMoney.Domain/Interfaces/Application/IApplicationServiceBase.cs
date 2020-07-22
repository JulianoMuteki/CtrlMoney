using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IApplicationServiceBase<T> where T : class
    {
        ICollection<T> GetAll();

        Task<ICollection<T>> GetAllAsync();

        T GetById(Guid id);

        Task<T> GetByIdAsync(Guid id);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        T Update(T updated);

        Task<T> UpdateAsync(T updated);

        void Delete(Guid id);

        Task<Guid> DeleteAsync(Guid id);
    }
}
