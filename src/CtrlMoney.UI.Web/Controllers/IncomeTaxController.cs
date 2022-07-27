using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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
        public JsonResult GetAjaxHandlerIncomesTaxes(string year)
        {
            var positions = _positionService.GetAll();// Filtrar ano corrente e anterior

            var incomesTaxesYear = positions.GroupBy(x => new { x.TicketCode, x.PositionDate.Year })
                           .Select(g => new
                           {
                               TicketCode = g.Key.TicketCode,
                               Year = g.Key.Year,
                               TotalValue = g.Where(z => z.PositionDate.Year == g.Key.Year).First().ValueUpdated,
                               Quantity = g.Where(z => z.PositionDate.Year == g.Key.Year).First().Quantity
                           }
                           ).ToList();

            var incomesTaxes = incomesTaxesYear.GroupBy(x => x.TicketCode)
                     .Select(g => new
                     {
                         Id = g.Key,
                         TicketCode = g.Key,

                         LastDate = g.Where(x => x.Year == g.OrderBy(y => y.Year).First().Year).FirstOrDefault().Year,
                         LastTotalValue = g.Where(x => x.Year == g.OrderBy(y => y.Year).First().Year).Sum(x => x.TotalValue).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                         LastQuantity = g.Where(x => x.Year == g.OrderBy(y => y.Year).First().Year).Sum(x => x.Quantity),

                         CurrentDate = g.Where(x => x.Year == g.OrderByDescending(y => y.Year).First().Year).FirstOrDefault().Year,
                         CurrentTotalValue = g.Where(x => x.Year == g.OrderByDescending(y => y.Year).First().Year).Sum(x => x.TotalValue).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                         CurrentQuantity = g.Where(x => x.Year == g.OrderByDescending(y => y.Year).First().Year).Sum(x => x.Quantity)
                     }).ToList();

            return Json(new
            {
                aaData = incomesTaxes,
                success = true
            });
        }

        [HttpGet]
        public ActionResult Details(string ticketCode)
        {
            var lastPosition = _positionService.GetLatestYearByTicketCode(ticketCode);

            var brokerageHistories = _brokerageHistoryService1.GetByTicketCode(ticketCode); // Filtral ticket com data
            var resumeBrokerageHistories = brokerageHistories.GroupBy(x => x.TransactionDate.Year)
                                                                .Select(g => new ResumeBrokerageHistories
                                                                {
                                                                    Year = g.Key,
                                                                    TotalValue = g.Sum(x => x.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                                    Quantity = g.Sum(x => x.Quantity)
                                                                }).ToList();

            var incomeTaxTicket = new IncomeTaxTicket()
            {
                TicketCode = ticketCode,
                ResumeBrokerageHistories = resumeBrokerageHistories,
                BrokerageHistoryVMs = brokerageHistories.Select(x => new BrokerageHistoryVM()
                                                            {
                                                                Price = x.Price.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                                Quantity = x.Quantity,
                                                                TotalPrice = x.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                                TransactionDate = x.TransactionDate,
                                                                TransactionType = x.TransactionType
                                                            })
                                                        .OrderBy(x => x.TransactionDate).ToList(),
                Quantity = brokerageHistories.Sum(x => x.Quantity),
                TotalValue = brokerageHistories.Sum(x => x.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Bookkeeping = lastPosition.Bookkeeping
            };
            return PartialView("_PartialViewStocks", incomeTaxTicket);
        }
    }
}
