using CtrlMoney.Domain.Interfaces.Application;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;

namespace CtrlMoney.UI.Web.Controllers
{
    public class MyMoneyController : Controller
    {
        private readonly IBrokerageHistoryService _brokerageHistoryService;
        private readonly ITicketConversionService _ticketConversionService;
        private readonly IXLWorkbookService _xLWorkbookService;
        private readonly IEarningService _earningService;
        private readonly IMovimentService _movementService;
        public MyMoneyController(IXLWorkbookService xLWorkbookService,
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

        [HttpGet]
        public JsonResult GetAjaxHandlerReport(string category, string period)
        {
            try
            {
                var brokeragesHistoriesFiltered = _brokerageHistoryService
                                            .GetAll()
                                            .Where(x => (category == "all")
                                                  ? x.Category == "Ações" || x.Category == "Fundos imobiliários"
                                                  : x.Category == category)
                                            .GroupBy(t => t.TicketCode)
                                            .Select(b => new
                                            {
                                                b.Key,
                                                Quantity = b.Where(x => x.TransactionType == "Compra").Sum(x => x.Quantity) - b.Where(x => x.TransactionType == "Venda").Sum(x => x.Quantity),
                                                histories = b.ToList()
                                            });

                var brokeragesHistories = brokeragesHistoriesFiltered.Where(x => x.Quantity > 0).SelectMany(x => x.histories);

                var earningsAll = _earningService.GetAll();

                var earnings = earningsAll.Where(x => (category == "all")
                                                  ? x.Category == "Ações" || x.Category == "Fundos imobiliários"
                                                  : x.Category == category).ToList();

                DateTime dateTimePeriod = brokeragesHistories.OrderByDescending(x => x.TransactionDate).Select(x=>x.TransactionDate).FirstOrDefault();

                if (period != "all")
                {
                    int periodValue = 0;
                    if (period == "thisYear")
                    {
                        dateTimePeriod = new DateTime(DateTime.Now.Year, 01, 01);
                    }
                    else
                    {
                        periodValue = int.Parse(period);
                        dateTimePeriod = DateTime.Now.AddMonths(-periodValue);
                    }

                    brokeragesHistories = brokeragesHistories.Where(x => x.TransactionDate >= dateTimePeriod).ToList();
                    earnings = earnings.Where(e => e.PaymentDate >= dateTimePeriod).ToList();
                }

                var brokeragesAverage = brokeragesHistories.GroupBy(x => x.TicketCode).Select(x =>
                new
                {
                    x.Key,
                    x.FirstOrDefault().Category,
                    BrokeragesAveragePrice = x.Average(a => a.Price),
                    Quantity = x.Sum(s => s.Quantity)
                }).ToList();

                var earningsAverage = earnings.GroupBy(x => x.TicketCode).Select(x =>
                new
                {
                    x.Key,
                    x.FirstOrDefault().Category,
                    SumEarningTotal = x.Where(d => d.EventType == "Rendimento" || d.EventType == "Dividendo" || d.EventType == "JCP").Sum(p => p.Price),
                    SumDividendTotal = x.Where(d => d.EventType == "Rendimento" || d.EventType == "Dividendo").Sum(p => p.TotalNetAmount),
                    JCPTotal = x.Where(d => d.EventType == "JCP").Sum(p => p.TotalNetAmount),
                    Total = x.Where(d => d.EventType == "Rendimento" || d.EventType == "Dividendo" || d.EventType == "JCP").Sum(p => p.TotalNetAmount)
                }).ToList();

                NumberFormatInfo nfi = new CultureInfo("pt-BR", false).NumberFormat;
                nfi.PercentDecimalDigits = 2;

                var report = brokeragesAverage.Select(x => new
                {
                    TicketCode = x.Key,
                    x.Category,
                    DateInitSet = dateTimePeriod.ToString("dd-MM-yyyy"),
                    Dividend = decimal.Round((earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumEarningTotal).FirstOrDefault() / x.BrokeragesAveragePrice), 6).ToString("P", nfi),
                    YieldOnCost = decimal.Round((earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumEarningTotal).FirstOrDefault() / x.BrokeragesAveragePrice), 6).ToString("P", nfi),
                    BrokeragesAveragePrice = x.BrokeragesAveragePrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    Total = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.Total).FirstOrDefault().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    DividendTotal = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumDividendTotal).FirstOrDefault().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    JCPTotal = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.JCPTotal).FirstOrDefault().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    QuantityStocks = x.Quantity,
                    Profit = 0
                }).ToList();

                return Json(new
                {
                    aaData = report,
                    success = true
                });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
