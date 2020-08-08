using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using CtrlMoney.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class BankAppService : IBankAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ICollection<Bank> GetAllBanks()
        {
            try
            {
                return _unitOfWork.RepositoryCustom<IBankRepository>().GetAllBanks();

            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<BankAppService>("Unexpected error fetching get all banks", nameof(this.GetAllBanks), ex);
            }
        }

        public Bank Add(Bank entity)
        {
            throw new NotImplementedException();
        }

        public Task<Bank> AddAsync(Bank entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Bank> GetAll()
        {
            try
            {
                var banks = _unitOfWork.Repository<Bank>().GetAll();
                return banks;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<Bank>("Unexpected error fetching my banks", nameof(this.GetAll), ex);
            }
        }

        public Task<ICollection<Bank>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Bank GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Bank> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Bank Update(Bank updated)
        {
            throw new NotImplementedException();
        }

        public Task<Bank> UpdateAsync(Bank updated)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<Bank> entity)
        {
            try
            {

                //if (!client.ComponentValidator.IsValid)
                //{
                //    entity.SetNotifications(client.ComponentValidator.GetNotifications());
                //    return entity;
                //}
                var result = _unitOfWork.Repository<Bank>().AddRange(entity);
                _unitOfWork.CommitSync();

                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<BankAppService>("Unexpected error fetching all Addrange bank", nameof(this.AddRange), ex);
            }
        }
    }
}
