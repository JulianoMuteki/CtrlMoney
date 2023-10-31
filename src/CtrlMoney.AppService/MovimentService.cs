using CtrlMoney.CrossCutting;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class MovimentService: IMovimentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MovimentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Moviment Add(Moviment entity)
        {
            throw new NotImplementedException();
        }

        public Task<Moviment> AddAsync(Moviment entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<Moviment> entity)
        {
            try
            {
                var result = _unitOfWork.Repository<Moviment>().AddRange(entity);
                _unitOfWork.CommitSync();
                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<MovimentService>("Unexpected error add range", nameof(this.AddRange), ex);
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

        public void DeleteByRangeDate(DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                var movements = _unitOfWork.Repository<Moviment>().FindBy(x=>x.Date >= dtStart && x.Date <= dtEnd).ToList();
                _unitOfWork.Repository<Moviment>().DeleteRange(movements);
                _unitOfWork.CommitSync();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<MovimentService>("Unexpected error Update", nameof(this.DeleteByRangeDate), ex);
            }
        }

        public ICollection<Moviment> GetAll()
        {
            try
            {
                return _unitOfWork.Repository<Moviment>().GetAll();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<MovimentService>("Unexpected error get all", nameof(this.GetAll), ex);
            }
        }

        public Task<ICollection<Moviment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Moviment GetById(Guid id)
        {
            try
            {
                return _unitOfWork.Repository<Moviment>().GetById(id);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<MovimentService>("Unexpected error GetById", nameof(this.GetById), ex);
            }
        }

        public Task<Moviment> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Moviment> GetByStartTicketAndYears(string ticketCode, int year)
        {
            try
            {
                return _unitOfWork.Repository<Moviment>().FindAll(x => x.TicketCode.Contains(ticketCode.Substring(0, 4)) && x.Date.Year <= year);
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<MovimentService>("Unexpected error GetByStartTicketAndYears", nameof(this.GetByStartTicketAndYears), ex);
            }
        }

        public Moviment Update(Moviment updated)
        {
            try
            {               
                var movement = _unitOfWork.Repository<Moviment>().Update(updated);
                
                _unitOfWork.CommitSync();
                return movement;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<MovimentService>("Unexpected error Update", nameof(this.Update), ex);
            }
        }

        public Task<Moviment> UpdateAsync(Moviment updated)
        {
            throw new NotImplementedException();
        }
    }
}
