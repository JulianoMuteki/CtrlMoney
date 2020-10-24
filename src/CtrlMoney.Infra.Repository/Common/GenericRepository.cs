using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Interfaces.Base;
using CtrlMoney.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CtrlMoney.Infra.Repository.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CtrlMoneyContext _context;

        public GenericRepository(CtrlMoneyContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query()
        {
            try
            {
                return _context.Set<T>().AsQueryable();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching all not available clients", nameof(this.Query), ex);
            }
        }

        public ICollection<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching get", nameof(this.GetAll), ex);
            }
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching get", nameof(this.GetAllAsync), ex);
            }
        }

        public T GetById(Guid id)
        {
            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching get", nameof(this.GetById), ex);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                return await _context.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching get", nameof(this.GetByIdAsync), ex);
            }
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            try
            {
                return _context.Set<T>().SingleOrDefault(match);
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching find", nameof(this.Find), ex);
            }
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return await _context.Set<T>().SingleOrDefaultAsync(match);
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching find", nameof(this.FindAsync), ex);
            }
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> match)
        {
            try
            {
                return _context.Set<T>().Where(match).ToList();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching find", nameof(this.FindAll), ex);
            }
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return await _context.Set<T>().Where(match).ToListAsync();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching find", nameof(this.FindAllAsync), ex);
            }
        }

        public T Add(T entity)
        {
            try
            {
                if (_context.Entry<T>(entity).State == EntityState.Added)
                {
                    _context.Set<T>().Add(entity);
                }
                if (_context.Entry<T>(entity).State == EntityState.Detached)
                {
                    _context.Entry(entity).State = EntityState.Added;                   
                }
                else if (_context.Entry<T>(entity).State == EntityState.Modified)
                {
                    _context.Set<T>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching add", nameof(this.Add), ex);
            }
        }

        public int AddRange(ICollection<T> entity)
        {
            try
            {
                _context.Set<T>().AddRange(entity);
                //  var result = _context.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching add", nameof(this.AddRange), ex);
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);

                return entity;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching add", nameof(this.AddAsync), ex);
            }
        }

        public T Update(T updated)
        {
            try
            {
                if (updated == null)
                {
                    return null;
                }

                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
                //   _context.SaveChanges();

                return updated;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching update", nameof(this.Update), ex);
            }
        }

        public ICollection<T> UpdateRange(ICollection<T> updateds)
        {
            try
            {
                if (updateds == null || updateds.Count == 0)
                {
                    return null;
                }

                _context.Set<T>().UpdateRange(updateds);

                return updateds;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching update", nameof(this.UpdateRange), ex);
            }
        }

        public async Task<T> UpdateAsync(T updated)
        {
            try
            {
                if (updated == null)
                {
                    return null;
                }

                _context.Set<T>().Attach(updated);
                _context.Entry(updated).State = EntityState.Modified;
                //  await _unitOfWork.Commit();

                return updated;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching update", nameof(this.UpdateAsync), ex);
            }
        }

        public void Delete(T t)
        {
            try
            {
                _context.Set<T>().Remove(t);
                //  _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching delete", nameof(this.Delete), ex);
            }
        }


        public int Count()
        {
            try
            {
                return _context.Set<T>().Count();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching count", nameof(this.Count), ex);
            }
        }

        public async Task<int> CountAsync()
        {
            try
            {
                return await _context.Set<T>().CountAsync();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching count", nameof(this.CountAsync), ex);
            }
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? page = null,
            int? pageSize = null)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (includeProperties != null)
                {
                    foreach (
                        var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                if (page != null && pageSize != null)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching filter", nameof(this.Filter), ex);
            }

        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return _context.Set<T>().Where(predicate);
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching find", nameof(this.FindBy), ex);
            }
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var exist = _context.Set<T>().Where(predicate);
                return exist.Any();
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching exists", nameof(this.Exist), ex);
            }

        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task<int> DeleteAsync(T t)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(ICollection<T> deleted)
        {
            try
            {
                _context.Set<T>().RemoveRange(deleted);
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching delete range", nameof(this.DeleteRange), ex);
            }
        }

        public T AddUpdate(T entity)
        {
            try
            {
                if (_context.Entry<T>(entity).State == EntityState.Added)
                {
                    _context.Set<T>().Add(entity);
                }
                else if (_context.Entry<T>(entity).State == EntityState.Modified)
                {
                    _context.Set<T>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Modified;
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<T>("Unexpected error fetching AddUpdate", nameof(this.AddUpdate), ex);
            }
        }
    }
}
