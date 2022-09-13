using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CtrlMoney.UI.Web.Controllers
{
    public class IncomeTaxController : Controller
    {
        private readonly IBrokerageHistoryService _brokerageHistoryService1;
        private readonly IPositionService _positionService;
        private readonly IEarningService _earningService;
        private readonly IMovimentService _movementService;
        private readonly ITicketConversionService _ticketConversionService;
        public IncomeTaxController(IBrokerageHistoryService brokerageHistoryService,
                                   IPositionService positionService,
                                   IEarningService earningService,
                                   IMovimentService movementService,
                                   ITicketConversionService ticketConversionService)
        {
            _brokerageHistoryService1 = brokerageHistoryService;
            _positionService = positionService;
            _earningService = earningService;
            _movementService = movementService;
            _ticketConversionService = ticketConversionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerIncomesTaxes(string year)
        {
            var positions = _positionService.GetByBaseYear(int.Parse(year));// Filtrar ano corrente e anterior

            var incomesTaxesYear = positions.GroupBy(x => new { x.TicketCode, x.PositionDate.Year })
                           .Select(g => new
                           {
                               TicketCode = g.Key.TicketCode,
                               Year = g.Key.Year,
                               TotalValue = g.Where(z => z.PositionDate.Year == g.Key.Year).First().ValueUpdated,
                               Quantity = g.Where(z => z.PositionDate.Year == g.Key.Year).First().Quantity
                           }
                           ).ToList();

            string[] finalTickes = new string[] { "12", "13" };

            var incomesTaxes = incomesTaxesYear.GroupBy(x => x.TicketCode.Substring(0, 4))
                     .Select(g => new
                     {
                         Id = g.Key,
                         TicketCode = g.Select(x => x.TicketCode).Where(ticket => !finalTickes.Any(code => ticket.EndsWith(code))).First(),
                         Tickets = g.Select(x => x.TicketCode),
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
        public ActionResult Details(string ticketCode, int year)
        {
            var lastPositions = _positionService.GetByTicketCodeAndYears(ticketCode, year).Result;

            var brokerageHistories = _brokerageHistoryService1.GetByTicketCode(ticketCode); // Filtral ticket com data
            var conversion = _ticketConversionService.GetByTicketInput(ticketCode);

            if (conversion != null)
            {
                var brokerageHistoriesConversion = _brokerageHistoryService1.GetByTicketCode(conversion.TicketOutput);
                if (brokerageHistoriesConversion.Count > 0)
                    brokerageHistories = brokerageHistories.Concat(brokerageHistoriesConversion).ToList();
            }

            var resumeBrokerageHistoriesByYear = brokerageHistories.GroupBy(x => x.TransactionDate.Year)
                                                                .Select(g => new ResumeBrokerageHistories
                                                                {
                                                                    Year = g.Key,
                                                                    TotalValue = g.Sum(x => x.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                                    Quantity = g.Sum(x => x.Quantity)
                                                                }).ToList();
            IList<BrokerageHistoryVM> brokerageHistoriesVMs = brokerageHistories.Select(x => new BrokerageHistoryVM()
            {
                TicketCode = x.TicketCode,
                Price = x.Price.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Quantity = x.Quantity,
                TotalPrice = x.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType
            }).ToList();


            var movements = _movementService.GetByStartTicketAndYears(ticketCode, year)
                                            .Where(x => x.MovimentType == "Bonificação em Ativos"
                                                        || (x.MovimentType == "Recibo de Subscrição" && x.InputOutput == "Debito")).ToList();
            if (movements.Count > 0)
            {
                foreach (var item in movements)
                {
                    brokerageHistoriesVMs.Add(new BrokerageHistoryVM()
                    {
                        TicketCode = item.TicketCode,
                        Price = item.UnitPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                        Quantity = item.Quantity,
                        TotalPrice = item.TransactionValue.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                        TransactionDate = item.Date,
                        TransactionType = item.MovimentType
                    });
                }
            }

            var incomeTaxTicket = new IncomeTaxTicket()
            {
                TicketCode = ticketCode,
                ResumeBrokerageHistories = resumeBrokerageHistoriesByYear,
                BrokerageHistoryVMs = brokerageHistoriesVMs.OrderBy(x => x.TransactionDate).ToList(),
                Quantity = brokerageHistoriesVMs.Sum(x => x.Quantity),
                TotalValue = brokerageHistories.Sum(x => x.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Bookkeeping = string.Join(", ", lastPositions.Where(x => x.Bookkeeping != "ESCRITURADOR NÃO ENCONTRADO").Select(x=>x.Bookkeeping).Distinct())
            };

            return PartialView("_PartialViewStocks", incomeTaxTicket);
        }
    }
}
