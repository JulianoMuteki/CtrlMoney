using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class TicketConversionService: ITicketConversionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketConversionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TicketConversion Add(TicketConversion entity)
        {
            try
            {
                var result = _unitOfWork.Repository<TicketConversion>().Add(entity);
                _unitOfWork.CommitSync();
                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<TicketConversionService>("Unexpected error add", nameof(this.Add), ex);
            }
        }

        public Task<TicketConversion> AddAsync(TicketConversion entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<TicketConversion> entity)
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

        public ICollection<TicketConversion> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TicketConversion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public TicketConversion GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TicketConversion> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public TicketConversion GetByTicketInput(string ticketCode)
        {
            try
            {
                var result = _unitOfWork.Repository<TicketConversion>().Find(x => x.TicketInput == ticketCode || x.TicketOutput == ticketCode);                
                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<TicketConversionService>("Unexpected error GetByTicketInput", nameof(this.GetByTicketInput), ex);
            }
        }

        public TicketConversion Update(TicketConversion updated)
        {
            throw new NotImplementedException();
        }

        public Task<TicketConversion> UpdateAsync(TicketConversion updated)
        {
            throw new NotImplementedException();
        }
    }
}
