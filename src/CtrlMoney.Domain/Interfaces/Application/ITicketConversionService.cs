using CtrlMoney.Domain.Entities;

namespace CtrlMoney.Domain.Interfaces.Application
{
    public interface ITicketConversionService : IApplicationServiceBase<TicketConversion>
    {
        TicketConversion GetByTicketInput(string ticketCode);
    }
}
