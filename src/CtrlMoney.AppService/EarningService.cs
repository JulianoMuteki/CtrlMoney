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
    public class EarningService: IEarningService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EarningService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Earning Add(Earning entity)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> AddAsync(Earning entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<Earning> entity)
        {
            try
            {
                var result = _unitOfWork.Repository<Earning>().AddRange(entity);
                _unitOfWork.CommitSync();
                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<EarningService>("Unexpected error add range", nameof(this.AddRange), ex);
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
                var earnings = _unitOfWork.Repository<Earning>().FindBy(x => x.PaymentDate >= dtStart && x.PaymentDate <= dtEnd).ToList();
                _unitOfWork.Repository<Earning>().DeleteRange(earnings);
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

        public ICollection<Earning> GetAll()
        {
            try
            {
                return _unitOfWork.Repository<Earning>().GetAll();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<EarningService>("Unexpected error add range", nameof(this.GetAll), ex);
            }
        }

        public Task<ICollection<Earning>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Earning GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Earning> GetByTicketCodeAndBaseYear(string ticketCode, int year)
        {
            try
            {
                return _unitOfWork.Repository<Earning>().FindAll(x => x.TicketCode.StartsWith(ticketCode) && x.PaymentDate != DateTime.MinValue).Where(x=>x.PaymentDate.Year <= year).ToList();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<EarningService>("Unexpected error add GetByTicketCodeAndBaseYear", nameof(this.GetByTicketCodeAndBaseYear), ex);
            }
        }

        public Earning Update(Earning updated)
        {
            throw new NotImplementedException();
        }

        public Task<Earning> UpdateAsync(Earning updated)
        {
            throw new NotImplementedException();
        }
    }
}
