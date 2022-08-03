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

        public ICollection<Moviment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Moviment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Moviment GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Moviment> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Moviment Update(Moviment updated)
        {
            throw new NotImplementedException();
        }

        public Task<Moviment> UpdateAsync(Moviment updated)
        {
            throw new NotImplementedException();
        }
    }
}
