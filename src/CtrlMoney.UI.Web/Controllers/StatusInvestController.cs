using CtrlMoney.AppService;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Extensions;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
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
            var positions = _brokerageHistoryService.GetByCategory(category);                          

           var conversions = _ticketConversionService.GetAll();

            if (conversions.Count > 0)
            {
                foreach (var conversion in conversions)
                {
                    var ticket = positions.Where(x => x.TicketCode == conversion.TicketInput || x.TicketCode == conversion.TicketOutput).FirstOrDefault();
                    if (ticket != null)
                    {
                        string ticketOld = conversion.TicketInput == ticket.TicketCode ? conversion.TicketOutput : conversion.TicketInput;

                        var brokerageHistoriesConversion = _brokerageHistoryService.GetByTicketCode(ticketOld, baseYear);
                        if (brokerageHistoriesConversion.Count > 0)
                            positions = positions.Concat(brokerageHistoriesConversion).ToList();
                    }
                }
            }

            var incomesTaxesYear = positions.GroupBy(x => new { x.TicketCode, x.TransactionDate.Year })
               .Select(g => new
               {
                   TicketCode = g.Key.TicketCode,
                   Year = g.Key.Year,
                   TotalValue = g.Where(z => z.TransactionDate.Year == g.Key.Year).Sum(s => s.TotalPrice),
                   Quantity = g.Where(z => z.TransactionDate.Year == g.Key.Year).Sum(s => s.Quantity)
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
            var brokerageHistories = _brokerageHistoryService.GetByTicketCode(ticketCode, year).OrderBy(x => x.TransactionDate).ToList(); // Filtrar ano

            var earnings = _earningService.GetByTicketCodeAndBaseYear(ticketCode, year); //TODO: Check filter
            var conversion = _ticketConversionService.GetByTicketInput(ticketCode);

            if (conversion != null)
            {
                string ticketOld = conversion.TicketInput == ticketCode ? conversion.TicketOutput : conversion.TicketInput;

                var brokerageHistoriesConversion = _brokerageHistoryService.GetByTicketCode(ticketOld, year);
                if (brokerageHistoriesConversion.Count > 0)
                    brokerageHistories = brokerageHistories.Concat(brokerageHistoriesConversion).ToList();
            }
            var movements = _movementService.GetByStartTicketAndYears(ticketCode, year)
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

            var resumeDataByYear = brokerageHistories;

            var resumeTransactionsAndEarningsByYear = resumeDataByYear.GroupBy(x => x.TransactionDate.Year)
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
                                                                                TotalValue = e.Sum(s => s.TotalPrice).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
                                                                            }).ToList()
                                                    }).ToList();

            #region Movements data
            var brokerageHistoriesTableInput = brokerageHistories; //.OrderBy(x => x.TransactionDate).ToList();

            var historyMovements = brokerageHistoriesTableInput.Select(x => new BrokerageHistoryVM()
            {
                TicketCode = x.TicketCode,
                Price = x.Price.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Quantity = x.Quantity,
                TotalPrice = x.TransactionType != "Bonificação" ? x.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")) : "--",
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType
            }).ToList();

            decimal totalCalendarYearInput = (brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" &&
                                                                                     x.TransactionDate.Year < year).Sum(x => x.TotalPrice)
                                              -
                                             brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda" &&
                                                                                     x.TransactionDate.Year < year).Sum(x => x.TotalPrice)
                                             );


            decimal totalYearExerciseInput = (brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" &&
                                                                                     x.TransactionDate.Year <= year).Sum(x => x.TotalPrice)
                                               -
                                               brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda" &&
                                                                                     x.TransactionDate.Year <= year).Sum(x => x.TotalPrice)
                                               );

            DataOperation dataOperationInput = new()
            {
                BrokeragesHistories = historyMovements,
                Quantity = brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra")
                                                       .Sum(x => x.Quantity)
                          - brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda")
                                                        .Sum(x => x.Quantity),
                TotalValue = totalYearExerciseInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Operation = string.Join(", ", brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" && x.TransactionDate.Year == year)
                                                                          .Select(operation => string.Format("{0} (R${1})", operation.Quantity, operation.Price))),
                TotalCalendarYear = totalCalendarYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                TotalYearExercise = totalYearExerciseInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
            };
            #endregion

            var incomeTaxTicket = new IncomeTaxTicket()
            {
                TicketCode = ticketCode,
                Conversion = conversion != null ? $"{conversion.TicketOutput} -> {conversion.TicketInput}" : string.Empty,
                ResumeBrokerageHistories = resumeTransactionsAndEarningsByYear,
                Bookkeeping = "ESCRITURADOR NÃO ENCONTRADO", //string.Join(", ", lastPositions.Where(x => x.Bookkeeping != "ESCRITURADOR NÃO ENCONTRADO").Select(x => x.Bookkeeping).Distinct()),
                DataOperationInput = dataOperationInput,
                YearExercise = year,
                CalendarYear = year - 1
            };

            return PartialView("_PartialViewStocks", incomeTaxTicket);
        }
    }
}
