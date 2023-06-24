using CtrlMoney.CrossCutting;
using CtrlMoney.CrossCutting.Enums;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.UI.Web.Extensions;
using CtrlMoney.UI.Web.Models;
using Microsoft.AspNetCore.Mvc;
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
            int yearNum = int.Parse(year);
            int yearExercise = yearNum;
            int calendarYear = yearNum - 1;
            int lastYear = yearNum - 2;

            var brokerageHistories = _brokerageHistoryService.GetByCategory(category).ToList();
            var conversions = _ticketConversionService.GetAll().ToList();

            var ticketsResume = CreateTicketsResume(brokerageHistories, conversions, yearExercise);

            var incomesTaxes = ticketsResume
                     .Select(g => new
                     {
                         Id = g.TicketCode,
                         g.TicketCode,
                         Tickets = string.IsNullOrEmpty(g.OldTicketCode)
                                         ? g.TicketCode
                                         : $"{g.TicketCode}, {g.OldTicketCode}",
                         g.Quantity,
                         TotalValue = g.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
                     }).ToList();

            return Json(new
            {
                aaData = incomesTaxes.ToList(),
                success = true
            });
        }

        private IList<TicketResume> CreateTicketsResume(ICollection<BrokerageHistory> brokerageHistories, ICollection<TicketConversion> ticketConversions, int baseYear)
        {
            IList<TicketResume> ticketsResumes = new List<TicketResume>();

            var ticketsCode = brokerageHistories.Select(x => x.TicketCode).Distinct();
            
            foreach (var ticketCode in ticketsCode)
            {
                if (!ticketsResumes.Any(x => x.TicketCode == ticketCode))
                {
                    string oldTicketCode = string.Empty;
                    string currentTickent = ticketCode;

                    IList<BrokerageHistory> brokerageHistoriesByTicket = brokerageHistories.Where(x => x.TicketCode == ticketCode).ToList();

                    if (ticketConversions.Any(k => k.TicketOutput == ticketCode))
                    {
                        var oldTicket = ticketConversions.Where(x => x.TicketOutput == ticketCode).FirstOrDefault();
                        oldTicketCode = oldTicket.TicketOutput;
                        currentTickent = oldTicket.TicketInput;

                        brokerageHistoriesByTicket = brokerageHistories.Where(x => x.TicketCode == oldTicket.TicketInput
                                                                                   || x.TicketCode == oldTicket.TicketOutput).ToList();
                    }

                    ticketsResumes.Add(new TicketResume()
                    {
                        TicketCode = currentTickent,
                        OldTicketCode = oldTicketCode,
                        Quantity = brokerageHistoriesByTicket.Where(x => x.TransactionType == "Compra").Sum(x => x.Quantity) - brokerageHistoriesByTicket.Where(x => x.TransactionType == "Venda").Sum(x => x.Quantity),
                        TotalPrice = brokerageHistoriesByTicket.Where(x => x.TransactionType == "Compra").Sum(x => x.TotalPrice) - brokerageHistoriesByTicket.Where(x => x.TransactionType == "Venda").Sum(x => x.TotalPrice)
                    });
                }
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
            var yearsWithData = yearsWithDataBrokerageHistories.Concat(yearsWithDataEarnings).OrderBy(x => x).Distinct();

            if (conversion != null)
            {
                string ticketOld = conversion.TicketInput == ticketCode ? conversion.TicketOutput : conversion.TicketInput;

                var brokerageHistoriesConversion = _brokerageHistoryService.GetByTicketCode(ticketOld, calendarYear);
                if (brokerageHistoriesConversion.Count > 0)
                    brokerageHistories = brokerageHistories.Concat(brokerageHistoriesConversion).ToList();
            }

            var movementsTransactions = _movementService.GetByStartTicketAndYears(ticketCode, calendarYear)
                                            .Where(x => (x.MovimentType == MovementTypeList.MovimentsTypes[MovimentType.BONIFICACAO_EM_ATIVOS]
                                                         || x.MovimentType == MovementTypeList.MovimentsTypes[MovimentType.RECIBO_DE_SUBSCRICAO])
                                                        && x.InputOutput == "Credito").ToList();

            if (movementsTransactions.Count > 0)
            {
                foreach (var item in movementsTransactions)
                {
                    brokerageHistories.Add(new BrokerageHistory()
                    {
                        TicketCode = item.TicketCode,
                        TransactionDate = item.Date,
                        StockBroker = item.StockBroker,
                        TransactionType = item.MovimentType,
                        TotalPrice = item.TransactionValue,
                        Price = item.UnitPrice,
                        Quantity = item.Quantity
                    });
                }
            }

            var movementsEarnings = _movementService.GetByStartTicketAndYears(ticketCode, calendarYear)
                                            .Where(x => (x.MovimentType == MovementTypeList.MovimentsTypes[MovimentType.LEILAO_DE_FRACAO]
                                                        || x.MovimentType == MovementTypeList.MovimentsTypes[MovimentType.AMORTIZACAO])
                                                        && x.InputOutput == "Credito").ToList();
            if (movementsEarnings.Count > 0)
            {
                foreach (var item in movementsEarnings)
                {
                    earnings.Add(new Earning()
                    {
                        TicketCode = item.TicketCode,
                        PaymentDate = item.Date,
                        EventType = item.MovimentType,
                        TotalNetAmount = item.TransactionValue,
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
                                                        TransactionsYears = brokerageHistoriesTableInput.Where(b => b.TransactionDate.Year == g.Key)
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
                                                                                    TotalValue = e.Sum(s => s.TotalNetAmount).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
                                                                                }).ToList()
                                                    }).ToList();

            #region Movements data

            var historyMovements = brokerageHistoriesTableInput.Select(x => new BrokerageHistoryVM()
            {
                TicketCode = x.TicketCode,
                Price = x.Price.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Quantity = x.Quantity,
                TotalPrice = x.TransactionType != MovementTypeList.MovimentsTypes[MovimentType.BONIFICACAO_EM_ATIVOS] ? x.TotalPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")) : "--",
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType
            }).ToList();

            decimal totalLastYearInputBuy = brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" &&
                                                                                     x.TransactionDate.Year <= lastYear).Sum(x => x.TotalPrice);

            decimal totalLastYearInputSell = brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda" &&
                                                                                     x.TransactionDate.Year <= lastYear).Sum(x => x.TotalPrice);
            string operationResultLastYear = string.Empty;
            decimal totalLastYearInput = totalLastYearInputBuy;
            if (totalLastYearInputSell > 0)
            {
                operationResultLastYear = totalLastYearInputSell > totalLastYearInputBuy ? "Lucro" : "Prejuízo";
                totalLastYearInput = totalLastYearInputSell - totalLastYearInputBuy;
            }

            decimal totalCalendarYearInputBuy = brokerageHistoriesTableInput.Where(x => x.TransactionType == "Compra" &&
                                                                                     x.TransactionDate.Year <= calendarYear).Sum(x => x.TotalPrice);

            decimal totalcalendarYearInputSell = brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda" &&
                                                                                     x.TransactionDate.Year <= calendarYear).Sum(x => x.TotalPrice);
            string operationResultCalendarYear = string.Empty;
            decimal totalCalendarYearInput = totalCalendarYearInputBuy;
            if (totalcalendarYearInputSell > 0)
            {
                operationResultCalendarYear = totalcalendarYearInputSell > totalCalendarYearInputBuy ? "Lucro" : "Prejuízo";
                totalCalendarYearInput = totalcalendarYearInputSell - totalCalendarYearInputBuy;
            }

            DataOperation dataOperationInput = new()
            {
                BrokeragesHistories = historyMovements,
                Quantity = brokerageHistoriesTableInput.Where(x => (x.TransactionType == "Compra" || x.TransactionType == MovementTypeList.MovimentsTypes[MovimentType.BONIFICACAO_EM_ATIVOS]))
                                                       .Sum(x => x.Quantity)
                          - brokerageHistoriesTableInput.Where(x => x.TransactionType == "Venda")
                                                        .Sum(x => x.Quantity),
                TotalValue = totalCalendarYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                Operation = string.Join(", ", brokerageHistoriesTableInput.Where(x => (x.TransactionType == "Compra" || x.TransactionType == MovementTypeList.MovimentsTypes[MovimentType.BONIFICACAO_EM_ATIVOS]) && x.TransactionDate.Year == calendarYear)
                                                                          .Select(operation => string.Format("{0} (R${1})", operation.Quantity, operation.Price))),
                WeightedAverage = CalcularMediaPonderada(brokerageHistories).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                TotalLastYear = totalLastYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                TotalCalendarYear = totalCalendarYearInput.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"))
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

        private decimal CalcularMediaPonderada(IList<BrokerageHistory> brokerageHistories)
        {
            decimal totalValuePurchases = 0;
            decimal totalQuantityPurchases = 0;

            foreach (var brokerageHistory in brokerageHistories)
            {
                totalValuePurchases += brokerageHistory.Price * brokerageHistory.Quantity; //valor * quantidade
                totalQuantityPurchases += brokerageHistory.Quantity;
            }

            decimal totalWeightedAverage = (totalValuePurchases / totalQuantityPurchases) * totalQuantityPurchases;

            return totalValuePurchases / totalQuantityPurchases;
        }
    }
}
