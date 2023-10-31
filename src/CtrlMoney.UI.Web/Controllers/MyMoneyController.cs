using CtrlMoney.Domain.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CtrlMoney.UI.Web.Controllers
{
    public class MyMoneyController : Controller
    {
        private readonly IBrokerageHistoryService _brokerageHistoryService;
        private readonly ITicketConversionService _ticketConversionService;
        private readonly IXLWorkbookService _xLWorkbookService;
        private readonly IEarningService _earningService;
        private readonly IMovimentService _movementService;
        private readonly IHttpClientFactory _clientFactory;
        private const string _jcp = "Juros Sobre Capital Próprio";
        public MyMoneyController(IXLWorkbookService xLWorkbookService,
                                      IBrokerageHistoryService brokerageHistoryService,
                                      IEarningService earningService,
                                      ITicketConversionService ticketConversionService,
                                      IMovimentService movementService,
                                      IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
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

        private async Task<bool> GetStockInfo(string tickerCode, string url)
        {
            var client = _clientFactory.CreateClient("YFinance");
            var response = await client.GetAsync($"/{url}?ticker=TAEE11.SA");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                dynamic stuff = JsonConvert.DeserializeObject(content.ToString());

                if (true)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<decimal> GetAsync(string tickerCode)
        {
            decimal currentPrice = 1;
            try
            {
                var httpClient = _clientFactory.CreateClient("YFinance");
                using HttpResponseMessage response = await httpClient.GetAsync($"info?ticker={tickerCode}.SA");

                response.EnsureSuccessStatusCode()
                    .WriteRequestToConsole();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic tickerInfo = JObject.Parse(jsonResponse);

                currentPrice = Convert.ToDecimal(tickerInfo.bid, CultureInfo.CreateSpecificCulture("pt-BR"));
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException tex)
            {
                Console.WriteLine($"{tickerCode} Timed out: {ex.Message}, {tex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{tickerCode} Timed out: {ex.Message}");
            }

            return currentPrice;
        }

        [HttpGet]
        public async Task<JsonResult> GetAjaxHandlerReport(string category, string period)
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

                var ticketsConversions = _ticketConversionService.GetAll().ToList();
                var brokeragesHistories = brokeragesHistoriesFiltered.Where(x => x.Quantity > 0 && !ticketsConversions.Any(t=>t.TicketOutput == x.Key) ).SelectMany(x => x.histories);

                var brokeragesAverage = brokeragesHistories.GroupBy(x => x.TicketCode).Select(x =>
                 new
                 {
                     x.Key,
                     x.FirstOrDefault().Category,
                     BrokeragesAveragePrice = x.Average(a => a.Price),
                     CurrentPrice = GetAsync(x.Key).Result,
                     Quantity = x.Sum(s => s.Quantity)
                 }).ToList();


                var earningsAll = _earningService.GetAll();

                var earnings = earningsAll.Where(x => (category == "all")
                                                  ? x.Category == "Ações" || x.Category == "Fundos imobiliários"
                                                  : x.Category == category).ToList();

                DateTime dateTimePeriod = brokeragesHistories.OrderByDescending(x => x.TransactionDate).Select(x => x.TransactionDate).FirstOrDefault();

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

                var earningsAverage = earnings.GroupBy(x => x.TicketCode).Select(x =>
                new
                {
                    x.Key,
                    x.FirstOrDefault().Category,
                    SumPriceByTicket = x.Where(d => d.EventType == "Rendimento" || d.EventType == "Dividendo" || d.EventType == _jcp).Sum(p => p.Price),
                    SumEarningTotal = x.Where(d => d.EventType == "Rendimento" || d.EventType == "Dividendo" || d.EventType == _jcp).Sum(p => p.TotalPrice),
                    JCPTotal = x.Where(d => d.EventType == _jcp).Sum(p => p.TotalNetAmount),
                    Total = x.Where(d => d.EventType == "Rendimento" || d.EventType == "Dividendo" || d.EventType == _jcp).Sum(p => p.TotalNetAmount)
                }).ToList();

                NumberFormatInfo nfi = new CultureInfo("pt-BR", false).NumberFormat;
                nfi.PercentDecimalDigits = 2;

                var report = brokeragesAverage.Select(x => new
                {
                    TicketCode = x.Key,
                    x.Category,
                    DateInitSet = dateTimePeriod.ToString("dd-MM-yyyy"),
                    Dividend = (earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumPriceByTicket).FirstOrDefault() / x.CurrentPrice).ToString("P", nfi),
                    YieldOnCost = (earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumPriceByTicket).FirstOrDefault() / x.BrokeragesAveragePrice).ToString("P", nfi),
                    BrokeragesAveragePrice = x.BrokeragesAveragePrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    TotalPriceByTicket = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumPriceByTicket).FirstOrDefault().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    EarningTotal = earningsAverage.Where(e => e.Key == x.Key).Select(s => s.SumEarningTotal).FirstOrDefault().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
                    CurrentPrice = x.CurrentPrice.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")),
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    static class HttpResponseMessageExtensions
    {
        internal static void WriteRequestToConsole(this HttpResponseMessage response)
        {
            if (response is null)
            {
                return;
            }

            var request = response.RequestMessage;
            Console.Write($"{request?.Method} ");
            Console.Write($"{request?.RequestUri} ");
            Console.WriteLine($"HTTP/{request?.Version}");
        }
    }
}
