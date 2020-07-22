using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CtrlMoney.Domain.Interfaces.Base
{
    public interface IGenericRepository<T> : IDisposable where T : class //: IDisposable where TEntity : class
    {
        IQueryable<T> Query();

        ICollection<T> GetAll();

        Task<ICollection<T>> GetAllAsync();

        T GetById(Guid id);

        Task<T> GetByIdAsync(Guid id);

        T Find(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        T Add(T entity);

        Task<T> AddAsync(T entity);

        int AddRange(ICollection<T> entity);

        T Update(T updated);

        ICollection<T> UpdateRange(ICollection<T> updateds);

        Task<T> UpdateAsync(T updated);

        void Delete(T t);

        void DeleteRange(ICollection<T> deleted);

        Task<int> DeleteAsync(T t);

        int Count();

        Task<int> CountAsync();

        IEnumerable<T> Filter(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? page = null,
            int? pageSize = null);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        bool Exist(Expression<Func<T, bool>> predicate);

        T AddUpdate(T entity);
    }
}
