
namespace CtrlMoney.Domain.Interfaces.Base
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        T RepositoryCustom<T>() where T : class;
        void CommitSync();
        void Rollback();
        void SetTrackAll();
    }
}
