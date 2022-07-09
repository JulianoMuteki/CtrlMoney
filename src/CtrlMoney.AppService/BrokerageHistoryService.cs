using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class BrokerageHistoryService : IBrokerageHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrokerageHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BrokerageHistory Add(BrokerageHistory entity)
        {
            throw new NotImplementedException();
        }

        public Task<BrokerageHistory> AddAsync(BrokerageHistory entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<BrokerageHistory> entity)
        {
            try
            {
                var result = _unitOfWork.Repository<BrokerageHistory>().AddRange(entity);
                _unitOfWork.CommitSync();
                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<Bank>("Unexpected error add range", nameof(this.AddRange), ex);
            }

            return 0;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<BrokerageHistory> GetAll()
        {
            try
            {
                var brokerageHistories = _unitOfWork.Repository<BrokerageHistory>().GetAll();
               
                return brokerageHistories;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<Bank>("Unexpected error add range", nameof(this.AddRange), ex);
            }
        }

        public Task<ICollection<BrokerageHistory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public BrokerageHistory GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BrokerageHistory> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public BrokerageHistory Update(BrokerageHistory updated)
        {
            throw new NotImplementedException();
        }

        public Task<BrokerageHistory> UpdateAsync(BrokerageHistory updated)
        {
            throw new NotImplementedException();
        }
    }
}
