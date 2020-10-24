using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using CtrlMoney.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class GrandChildTreeAppService : IGrandChildTreeAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GrandChildTreeAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GrandChildTree Add(GrandChildTree entity)
        {
            try
            {
                var grandChildTree = _unitOfWork.Repository<GrandChildTree>().Add(entity);
                _unitOfWork.CommitSync();
                return grandChildTree;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<GrandChildTreeAppService>("Unexpected error fetching Add", nameof(this.Add), ex);
            }
        }

        public GrandChildTree GetById(Guid id)
        {
            try
            {
                return _unitOfWork.Repository<GrandChildTree>().GetById(id);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<GrandChildTreeAppService>("Unexpected error fetching get", nameof(this.GetById), ex);
            }
        }

        public Task<GrandChildTree> AddAsync(GrandChildTree entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<GrandChildTree> entity)
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

        public ICollection<GrandChildTree> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<GrandChildTree>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GrandChildTree> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public GrandChildTree Update(GrandChildTree updated)
        {
            throw new NotImplementedException();
        }

        public Task<GrandChildTree> UpdateAsync(GrandChildTree updated)
        {
            throw new NotImplementedException();
        }
    }
}
