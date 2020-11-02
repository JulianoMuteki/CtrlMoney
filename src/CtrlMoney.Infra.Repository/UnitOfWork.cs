using CtrlMoney.Infra.Repository.Repositories;
using CtrlMoney.Infra.Context;
using CtrlMoney.Infra.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CtrlMoney.Domain.Interfaces.Repository;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Base;

namespace CtrlMoney.Infra.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CtrlMoneyContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public Dictionary<Type, object> Repositories
        {
            get { return _repositories; }
            set { Repositories = value; }
        }

        public UnitOfWork(CtrlMoneyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IGenericRepository<T>;
            }

            IGenericRepository<T> repo = new GenericRepository<T>(_dbContext);
            Repositories.Add(typeof(T), repo);
            return repo;
        }


        private readonly Dictionary<Type, object> _repositoriesCustom = new Dictionary<Type, object>();

        public Dictionary<Type, object> RepositoriesCustom
        {
            get { return _repositoriesCustom; }
            set { RepositoriesCustom = value; }
        }

        public T RepositoryCustom<T>() where T : class
        {
            if (!RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                CreateReository<T>();
            }

            var obj = RepositoriesCustom[typeof(T)] as T;

            return obj;
        }

        private void CreateReository<T>() where T : class
        {
            if (typeof(IBankRepository).Equals((typeof(T))) && !RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                IBankRepository repository = new BankRepository(_dbContext);
                RepositoriesCustom.Add(typeof(T), repository);
            }
            else if  (typeof(IParentTreeRepository).Equals((typeof(T))) && !RepositoriesCustom.Keys.Contains(typeof(T)))
            {
                IParentTreeRepository repository = new ParentTreeRepository(_dbContext);
                RepositoriesCustom.Add(typeof(T), repository);
            }
        }

        //public async Task<int> Commit()
        //{
        //    return await _dbContext.SaveChangesAsync();
        //}

        public void CommitSync()
        {
             _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        public void SetTrackAll()
        {
            _dbContext.SetTrackAll();
        }

        public void EnableLazyLoading()
        {
            _dbContext.EnableLazyLoading();
        }
    }
}
