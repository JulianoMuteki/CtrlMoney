using CtrlMoney.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface IPositionService : IApplicationServiceBase<Position>
    {
        Task<ICollection<Position>> GetByTicketCodeAndYears(string startTicket, int baseYear);
        ICollection<Position> GetByBaseYear(int year);
    }
}
