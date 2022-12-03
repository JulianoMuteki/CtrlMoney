using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
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
        public JsonResult GetAjaxHandlerReport()
        {
            try
            {

                var brokeragesHistories = _brokerageHistoryService.GetAll().Where(x => x.Category == "Ações" || x.Category == "Fundos imobiliários").ToList();
                var earnings = _earningService.GetAll().ToList();


                var brokeragesAverage = brokeragesHistories.GroupBy(x => x.TicketCode).Select(x =>
                new
                {
                    x.Key,
                    x.FirstOrDefault().Category,
                    BrokeragesAveragePrice = x.Average(a => a.Price),
                    Quantity = x.Sum(s => s.Quantity)
                });

                var earningsAverage = earnings.GroupBy(x => x.TicketCode).Select(x =>
                new
                {
                    x.Key,
                    x.FirstOrDefault().Category,
                    AllAveragePrice = x.Average(a => a.Price),
                    DyAverageList = x.Where(d => d.EventType == "Dividendo").Select(p => p.Price).ToList(),
                }).ToList();

                var test = brokeragesAverage.Select(x => new
                {
                    TicketCode = x.Key,
                    x.Category,
                    AllEarningAvaregePrice = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.AllAveragePrice).FirstOrDefault(),
                    DyEarningAvaregePrice = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.DyAverageList.Count > 0 ? s.DyAverageList.Average() : 0).FirstOrDefault(),
                    x.BrokeragesAveragePrice,
                    x.Quantity,
                    Profit = 0
                }).ToList();

                NumberFormatInfo nfi = new CultureInfo("pt-BR", false).NumberFormat;
                nfi.PercentDecimalDigits = 2;

                var report = test.Select(x => new
                {
                    x.TicketCode,
                    x.Category,
                    YoCTotal = decimal.Round((x.AllEarningAvaregePrice / x.BrokeragesAveragePrice), 6).ToString("P", nfi),
                    YoCDyTotal = decimal.Round((x.DyEarningAvaregePrice / x.BrokeragesAveragePrice), 6).ToString("P", nfi),
                    BrokeragesAveragePrice = x.BrokeragesAveragePrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    AllEarningAvaregePrice = x.AllEarningAvaregePrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    DyEarningAvaregePrice = x.DyEarningAvaregePrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    x.Quantity,
                    x.Profit
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
