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
    public class ChildTreeAppService : IChildTreeAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChildTreeAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ChildTree Add(ChildTree entity)
        {
            try
            {
                var childTree = _unitOfWork.Repository<ChildTree>().Add(entity);
                _unitOfWork.CommitSync();
                return childTree;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ChildTreeAppService>("Unexpected error fetching Add", nameof(this.Add), ex);
            }
        }

        public ChildTree GetById(Guid id)
        {
            try
            {
                return _unitOfWork.Repository<ChildTree>().GetById(id);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ChildTreeAppService>("Unexpected error fetching get", nameof(this.GetById), ex);
            }
        }

        public Task<ChildTree> AddAsync(ChildTree entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<ChildTree> entity)
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

        public ICollection<ChildTree> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ChildTree>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ChildTree> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ChildTree Update(ChildTree updated)
        {
            throw new NotImplementedException();
        }

        public Task<ChildTree> UpdateAsync(ChildTree updated)
        {
            throw new NotImplementedException();
        }
    }
}
