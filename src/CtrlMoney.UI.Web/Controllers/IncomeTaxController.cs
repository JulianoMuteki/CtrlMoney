using CtrlMoney.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CtrlMoney.UI.Web.Controllers
{
    public class IncomeTaxController : Controller
    {
        private readonly IBrokerageHistoryService _brokerageHistoryService1;
        private readonly IPositionService _positionService;
        private readonly IEarningService _earningService;
        public IncomeTaxController(IBrokerageHistoryService brokerageHistoryService, IPositionService positionService, IEarningService earningService)
        {
            _brokerageHistoryService1 = brokerageHistoryService;
            _positionService = positionService;
            _earningService = earningService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerIncomesTaxes()
        {
            var positions = _positionService.GetAll();// Filtrar ano corrente e anterior

            var incomesTaxes = positions.GroupBy(x =>x.TicketCode)
                                    .Select(g => new
                                  {
                                        Id = g.Key,
                                        TicketCode = g.Key,
                                        LastDate = g.OrderBy(y => y.PositionDate).First().PositionDate.ToString("dd/MM/yyyy"),
                                        LastTotalValue = g.OrderBy(y => y.PositionDate).First().ValueUpdated,
                                        CurrentDate = g.OrderBy(y => y.PositionDate).Skip(1).First().PositionDate.ToString("dd/MM/yyyy"),
                                        CurrentTotalValue = g.OrderBy(y => y.PositionDate).Skip(1).First().ValueUpdated
                                    });

            //var process = incomesTaxes.Select(g => new
            //{
            //    Id = g.TicketCode,
            //    TicketCode = g.TicketCode,
            //    LastDate = g.ListData.OrderBy(y => y.PositionDate).First().PositionDate,
            //    LastTotalValue = g.ListData.OrderBy(y => y.PositionDate).First().ValueUpdated,
            //    CurrentDate = g.ListData.OrderByDescending(y => y.PositionDate).First().PositionDate,
            //    CurrentTotalValue = g.ListData.OrderByDescending(y => y.PositionDate).First().ValueUpdated
            //});

            return Json(new
            {
                aaData = incomesTaxes,
                success = true
            });
        }

    }
}
