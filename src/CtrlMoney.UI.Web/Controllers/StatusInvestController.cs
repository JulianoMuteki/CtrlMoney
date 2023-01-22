using CtrlMoney.AppService;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Extensions;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CtrlMoney.UI.Web.Controllers
{
    public class StatusInvestController : Controller
    {
        private readonly IBrokerageHistoryService _brokerageHistoryService;
        private readonly ITicketConversionService _ticketConversionService;
        private readonly IXLWorkbookService _xLWorkbookService;
        private readonly IEarningService _earningService;
        private readonly IMovimentService _movementService;
        public StatusInvestController(IXLWorkbookService xLWorkbookService,
                                      IBrokerageHistoryService brokerageHistoryService,
                                      IEarningService earningService,
                                      ITicketConversionService ticketConversionService,
                                      IMovimentService movementService)
        {
            _ticketConversionService = ticketConversionService;
            _brokerageHistoryService = brokerageHistoryService;
            _xLWorkbookService = xLWorkbookService;
            _earningService = earningService;
            _movementService = movementService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BrokeragesHistories()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerBrokeragesHistories()
        {
            var brokeragesHistories = _brokerageHistoryService.GetAll();

            return Json(new
            {
                aaData = brokeragesHistories,
                success = true
            });
        }

        public IActionResult UploadTransaction()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadTransaction(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {
                var fullfileName = this.CreateFile(model);
                var brokerageHistories = _xLWorkbookService.ImportSITransactionsSheet(fullfileName);
                _brokerageHistoryService.AddRange(brokerageHistories);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return RedirectToAction("BrokeragesHistories");
        }

        public IActionResult EarningIndex()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerEarnings()
        {
            var earnings = _earningService.GetAll();

            return Json(new
            {
                aaData = earnings,
                success = true
            });
        }

        public IActionResult UploadEarning()
        {
            SingleFileModel model = new SingleFileModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadEarning(SingleFileModel model)
        {
            if (ModelState.IsValid)
            {
                var fullfileName = this.CreateFile(model);
                var earnings = _xLWorkbookService.ImportSIEarningsSheet(fullfileName);
                _earningService.AddRange(earnings);
                System.IO.File.Delete(fullfileName);

                model.IsSuccess = true;
                model.Message = "File upload successfully";
                model.IsResponse = true;
            }
            return RedirectToAction("EarningIndex");
        }

        public IActionResult IncomeTaxReport()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAjaxHandlerIncomesTaxes(string year, string category)
        {
            int baseYear = int.Parse(year);
            int lastYear = baseYear - 1;
            var brokeragesHistory = _brokerageHistoryService.GetByCategory(category);

            var conversions = _ticketConversionService.GetAll();

            var oldTickets = new Dictionary<string, string>();

            if (conversions.Count > 0)
            {
                foreach (var conversion in conversions)
                {
                    var ticket = brokeragesHistory.Where(x => x.TicketCode == conversion.TicketInput || x.TicketCode == conversion.TicketOutput).FirstOrDefault();
                    if (ticket != null)
                    {
                        string ticketOld = conversion.TicketInput == ticket.TicketCode ? conversion.TicketOutput : conversion.TicketInput;
                        oldTickets.Add(ticket.TicketCode, ticketOld);
                    }
                }
            }

            var ticketsResume = CreateTicketsResume(brokeragesHistory, oldTickets, baseYear);

            var incomesTaxes = ticketsResume.GroupBy(x => x.TicketCode)
                     .Select(g => new
                     {
                         Id = g.Key,
                         TicketCode = g.Key,
                         Tickets = string.IsNullOrEmpty(g.Select(t => t.OldTicketCode).First()) 
                                         ? g.Key 
                                         : $"{g.Key}, {g.Select(t=>t.OldTicketCode).First()}",
                         Quantity = g.Sum(x => x.Quantity),
                         TotalValue = g.Sum(s => s.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
                     }).ToList();

            return Json(new
            {
                aaData = incomesTaxes,
                success = true
            });
        }

        private IList<TicketResume> CreateTicketsResume(ICollection<BrokerageHistory> brokerageHistories, Dictionary<string, string> oldTickets, int baseYear)
        {
            IList<TicketResume> ticketsResumes = new List<TicketResume>();

            var ticketsCode = brokerageHistories.Select(x => x.TicketCode).Distinct();
           
            foreach (var ticketCode in ticketsCode)
            {                
                string oldTicket = string.Empty;

                IList<BrokerageHistory> brokerageHistoriesByTicket = brokerageHistories.Where(x => x.TicketCode == ticketCode).ToList();
                if(oldTickets.Any(k=>k.Key == ticketCode))
                {
                    oldTicket = oldTickets.Where(x => x.Key == ticketCode).Select(t => t.Value).FirstOrDefault();

                    var brokerageHistoriesConversion = _brokerageHistoryService.GetByTicketCode(oldTicket, baseYear);
                    if (brokerageHistoriesConversion.Count > 0)
                        brokerageHistoriesByTicket = brokerageHistoriesByTicket.Concat(brokerageHistoriesConversion).ToList();
                }

                ticketsResumes.Add(new TicketResume()
                {
                    TicketCode = ticketCode,
                    OldTicketCode = oldTicket,
                    Quantity = brokerageHistoriesByTicket.Sum(s => s.Quantity),
                    TotalPrice = brokerageHistoriesByTicket.Sum(s => s.TotalPrice)
                });
            }

            return ticketsResumes;
        }

        [HttpGet]
        public ActionResult Details(string ticketCode, int year)
        {
            int yearExercise = year;
            int calendarYear = year - 1;
            int lastYear = year - 2;

            var brokerageHistories = _brokerageHistoryService.GetByTicketCode(ticketCode, calendarYear).OrderBy(x => x.TransactionDate).ToList();
            var earnings = _earningService.GetByTicketCodeAndBaseYear(ticketCode, calendarYear); //TODO: Check filter
            var conversion = _ticketConversionService.GetByTicketInput(ticketCode);

            var yearsWithDataBrokerageHistories = brokerageHistories.Select(x => x.TransactionDate.Year).Distinct();
            var yearsWithDataEarnings = earnings.Select(x => x.PaymentDate.Year).Distinct();
            var yearsWithData = yearsWithDataBrokerageHistories.Concat(yearsWithDataEarnings).OrderBy(x=>x).Distinct();

            if (conversion != null)
            {
                string ticketOld = conversion.TicketInput == ticketCode ? conversion.TicketOutput : conversion.TicketInput;

                var brokerageHistoriesConversion = _brokerageHistoryService.GetByTicketCode(ticketOld, calendarYear);
                if (brokerageHistoriesConversion.Count > 0)
                    brokerageHistories = brokerageHistories.Concat(brokerageHistoriesConversion).ToList();
            }
            var movements = _movementService.GetByStartTicketAndYears(ticketCode, calendarYear)
                                            .Where(x => x.MovimentType == "Leilão de Fração" && x.InputOutput == "Credito").ToList();
            if (movements.Count > 0)
            {
                foreach (var item in movements)
                {
                    earnings.Add(new Earning()
                    {
                        TicketCode = item.TicketCode,
                        PaymentDate = item.Date,
                        EventType = item.MovimentType,
                        TotalPrice = item.TransactionValue,
                        Price = item.UnitPrice,
                        Quantity = item.Quantity
                    });
                }
            }

            var brokerageHistoriesTableInput = brokerageHistories.OrderBy(x => x.TransactionDate).ToList();

            //brokerageHistoriesTableInput
            var resumeTransactionsAndEarningsByYear = yearsWithData.GroupBy(x => x)
                                                    .Select(g => new ResumeBrokerageHistories
                                                    {
                                                        Year = g.Key,
                                                        TransactionsYears = brokerageHistoriesTableInput.Where(b=>b.TransactionDate.Year == g.Key)
                                                                                                        .GroupBy(y => y.TransactionType)
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
                                                                                    TotalValue = e.Sum(s => s.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
                                                                                }).ToList()
                                                    }).ToList();

            #region Movements data

            var historyMovements = brokerageHistoriesTableInput.Select(x => new BrokerageHistoryVM()
            {
                TicketCode = x.TicketCode,
                Price = x.Price.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Quantity = x.Quantity,
                TotalPrice = x.TransactionType != "Bonificação" ? x.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")) : "--",
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType
            }).ToList();

            decimal totalLastYearInput = (brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" &&
                                                                                     x.TransactionDate.Year <= lastYear).Sum(x => x.TotalPrice)
                                              -
                                             brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda" &&
                                                                                     x.TransactionDate.Year <= lastYear).Sum(x => x.TotalPrice)
                                             );


            decimal totalcalendarYearInput = (brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" &&
                                                                                     x.TransactionDate.Year <= calendarYear).Sum(x => x.TotalPrice)
                                               -
                                               brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda" &&
                                                                                     x.TransactionDate.Year <= calendarYear).Sum(x => x.TotalPrice)
                                               );

            DataOperation dataOperationInput = new()
            {
                BrokeragesHistories = historyMovements,
                Quantity = brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra")
                                                       .Sum(x => x.Quantity)
                          - brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda")
                                                        .Sum(x => x.Quantity),
                TotalValue = totalcalendarYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Operation = string.Join(", ", brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" && x.TransactionDate.Year == calendarYear)
                                                                          .Select(operation => string.Format("{0} (R${1})", operation.Quantity, operation.Price))),
                TotalLastYear = totalLastYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                TotalCalendarYear = totalcalendarYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
            };
            #endregion

            var incomeTaxTicket = new IncomeTaxTicket()
            {
                TicketCode = ticketCode,
                Conversion = conversion != null ? $"{conversion.TicketOutput} -> {conversion.TicketInput}" : string.Empty,
                ResumeBrokerageHistories = resumeTransactionsAndEarningsByYear,
                Bookkeeping = "ESCRITURADOR NÃO ENCONTRADO", //string.Join(", ", lastPositions.Where(x => x.Bookkeeping != "ESCRITURADOR NÃO ENCONTRADO").Select(x => x.Bookkeeping).Distinct()),
                DataOperationInput = dataOperationInput,
                ExerciseYear = yearExercise,
                CalendarYear = calendarYear,
                LastYear = lastYear
            };

            return PartialView("_PartialViewStocks", incomeTaxTicket);
        }
    }
}
