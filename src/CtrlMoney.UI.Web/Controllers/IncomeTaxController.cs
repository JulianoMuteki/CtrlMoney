using CtrlMoney.Domain.Entities;
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
            int baseYear = int.Parse(year);
            int lastYear = baseYear - 1;
            var positions = _positionService.GetByBaseYear(int.Parse(year));

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
                         //Base year
                         LastDate = (g.Where(x => x.Year == lastYear).FirstOrDefault() == null ? "--" : g.Where(x => x.Year == lastYear).FirstOrDefault().Year.ToString()),
                         LastTotalValue = g.Where(x => x.Year == lastYear).Sum(x => x.TotalValue).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                         LastQuantity = g.Where(x => x.Year == lastYear).Sum(x => x.Quantity),
                         //Next Year
                         CurrentDate = g.Where(x => x.Year == baseYear).FirstOrDefault() == null ? "--" : g.Where(x => x.Year == baseYear).FirstOrDefault().Year.ToString(),
                         CurrentTotalValue = g.Where(x => x.Year == baseYear).Sum(x => x.TotalValue).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                         CurrentQuantity = g.Where(x => x.Year == baseYear).Sum(x => x.Quantity)
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

            var brokerageHistories = _brokerageHistoryService1.GetByTicketCode(ticketCode, year); // Filtrar ano
            var conversion = _ticketConversionService.GetByTicketInput(ticketCode);

            if (conversion != null)
            {
                string ticketOld = conversion.TicketInput == ticketCode ? conversion.TicketOutput : conversion.TicketInput;

                var brokerageHistoriesConversion = _brokerageHistoryService1.GetByTicketCode(ticketOld, year);
                if (brokerageHistoriesConversion.Count > 0)
                    brokerageHistories = brokerageHistories.Concat(brokerageHistoriesConversion).ToList();
            }

            var movements = _movementService.GetByStartTicketAndYears(ticketCode, year)
                                            .Where(x => x.MovimentType == "Bonificação em Ativos"
                                                        || x.MovimentType == "Leilão de Fração"
                                                        || (x.MovimentType == "Recibo de Subscrição" && x.InputOutput == "Debito")).ToList();
            if (movements.Count > 0)
            {
                foreach (var item in movements)
                {
                    brokerageHistories.Add(new BrokerageHistory()
                    {
                        TicketCode = item.TicketCode,
                        TransactionDate = item.Date,
                        TransactionType = item.MovimentType,
                        TotalPrice = item.TransactionValue,
                        Price = item.UnitPrice,
                        Quantity = item.Quantity
                    });
                }
            }

            var earnings = _earningService.GetByTicketCodeAndBaseYear(ticketCode, year); //TODO: Check filter
            if (movements.Count > 0)
            {
                foreach (var item in movements.Where(x => x.MovimentType == "Leilão de Fração").ToList())
                {
                    earnings.Add(new Earning()
                    {
                        TicketCode = item.TicketCode,
                        PaymentDate = item.Date,
                        EventType = item.MovimentType,
                        NetValue = item.TransactionValue,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    });
                }
            }

            var resumeDataByYear = brokerageHistories.Where(x => x.TransactionType != "Bonificação em Ativos"
                                                      && x.TransactionType != "Leilão de Fração").ToList();
            var brokerageHistoriesTable = brokerageHistories.Where(x => x.TransactionType != "Leilão de Fração").OrderBy(x => x.TransactionDate).ToList();


            var allBrokerageHistoriesByYear = resumeDataByYear.GroupBy(x => x.TransactionDate.Year)
                                                                .Select(g => new ResumeBrokerageHistories
                                                                {
                                                                    Year = g.Key,
                                                                    TransactionsYears = g.GroupBy(y => y.TransactionType)
                                                                                        .Select(t => new TransactionYear
                                                                                        {
                                                                                            TransactionType = t.Key,
                                                                                            UnitPrice = t.Select(z => z.Price).FirstOrDefault().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                                                            TotalValue = t.Sum(z => z.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                                                            Quantity = t.Sum(z => z.Quantity)
                                                                                        }).ToList(),
                                                                    EarningsReport = earnings.Where(e => e.PaymentDate.Year == g.Key)
                                                                                        .GroupBy(e => e.EventType)
                                                                                        .Select(e => new EarningReport
                                                                                        {
                                                                                            EventType = e.Key,
                                                                                            Quantity = e.Sum(s => s.Quantity),
                                                                                            TotalValue = e.Sum(s => s.NetValue).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
                                                                                        }).ToList()
                                                                }).ToList();

            var brokerageHistoryVMs = brokerageHistoriesTable
                                                          .Select(x => new BrokerageHistoryVM()
                                                          {
                                                              TicketCode = x.TicketCode,
                                                              Price = x.Price.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                              Quantity = x.Quantity,
                                                              TotalPrice = x.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                                                              TransactionDate = x.TransactionDate,
                                                              TransactionType = x.TransactionType
                                                          }).ToList();

            string operations = string.Join(", ", brokerageHistoriesTable.Select(operation => string.Format("{0} ({1})", operation.Quantity, operation.Price)));
            string totalYearExercise = brokerageHistoriesTable.Where(x => x.TransactionDate.Year < year).Sum(x => x.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));
            string totalCalendarYear = brokerageHistoriesTable.Where(x => x.TransactionDate.Year <= year).Sum(x => x.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

            var incomeTaxTicket = new IncomeTaxTicket()
            {
                TicketCode = ticketCode,
                Conversion = conversion != null ? $"{conversion.TicketOutput} -> {conversion.TicketInput}" : string.Empty,
                ResumeBrokerageHistories = allBrokerageHistoriesByYear,
                BrokerageHistoryVMs = brokerageHistoryVMs,
                Quantity = brokerageHistoriesTable.Sum(x => x.Quantity),
                TotalValue = brokerageHistoriesTable.Sum(x => x.TotalPrice).ToString(CultureInfo.CreateSpecificCulture("pt-BR")),
                Bookkeeping = string.Join(", ", lastPositions.Where(x => x.Bookkeeping != "ESCRITURADOR NÃO ENCONTRADO").Select(x => x.Bookkeeping).Distinct()),
                Operation= operations,
                TotalCalendarYear = totalCalendarYear,
                TotalYearExercise = totalYearExercise
            };

            return PartialView("_PartialViewStocks", incomeTaxTicket);
        }
    }
}
