using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities.FinancialClassification;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using CtrlMoney.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class ParentTreeAppService : IParentTreeAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParentTreeAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ParentTree Add(ParentTree entity)
        {
            try
            {
                var parentTree = _unitOfWork.Repository<ParentTree>().Add(entity);
                _unitOfWork.CommitSync();
                return parentTree;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ParentTreeAppService>("Unexpected error fetching Add", nameof(this.Add), ex);
            }
        }

        public ParentTree GetById(Guid id)
        {
            try
            {
                return _unitOfWork.Repository<ParentTree>().GetById(id);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ParentTreeAppService>("Unexpected error fetching get", nameof(this.GetById), ex);
            }
        }

        public Task<ParentTree> AddAsync(ParentTree entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<ParentTree> entity)
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

        public ICollection<ParentTree> GetAll()
        {
            try
            {
                return _unitOfWork.Repository<ParentTree>().GetAll();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ParentTreeAppService>("Unexpected error fetching get", nameof(this.GetAll), ex);
            }
        }

        public Task<ICollection<ParentTree>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ParentTree> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ParentTree Update(ParentTree updated)
        {
            throw new NotImplementedException();
        }

        public Task<ParentTree> UpdateAsync(ParentTree updated)
        {
            throw new NotImplementedException();
        }

        public ICollection<ParentTree> GetAll_WithChildrem()
        {
            try
            {
                return _unitOfWork.RepositoryCustom<IParentTreeRepository>().GetAll_WithChildrem();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<ParentTreeAppService>("Unexpected error fetching get", nameof(this.GetAll_WithChildrem), ex);
            }
        }
    }
}
