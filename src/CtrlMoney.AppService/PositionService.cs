﻿using CtrlMoney.CrossCutting;
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
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Position Add(Position entity)
        {
            throw new NotImplementedException();
        }

        public Task<Position> AddAsync(Position entity)
        {
            throw new NotImplementedException();
        }

        public int AddRange(ICollection<Position> entity)
        {
            try
            {
                var result = _unitOfWork.Repository<Position>().AddRange(entity);
                _unitOfWork.CommitSync();
                return result;
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<PositionService>("Unexpected error add range", nameof(this.AddRange), ex);
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

        public ICollection<Position> GetAll()
        {
            try
            {
               return _unitOfWork.Repository<Position>().GetAll();
            }
            catch (CustomException exc)
            {
                throw exc;
            }
            catch (Exception ex)
            {
                throw CustomException.Create<PositionService>("Unexpected error add range", nameof(this.GetAll), ex);
            }
        }

        public Task<ICollection<Position>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Position GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Position Update(Position updated)
        {
            throw new NotImplementedException();
        }

        public Task<Position> UpdateAsync(Position updated)
        {
            throw new NotImplementedException();
        }
    }
}
