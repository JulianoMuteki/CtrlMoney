using CtrlMoney.DataTransfer;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using CtrlMoney.Domain.Interfaces.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CtrlMoney.AppService
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrokerageHistoryService _brokerageHistoryService;
        private readonly ITicketConversionService _ticketConversionService;
        private readonly IHttpClientFactory _clientFactory;

        public ReportService(IUnitOfWork unitOfWork, IBrokerageHistoryService brokerageHistoryService, ITicketConversionService ticketConversionService,
                            IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _brokerageHistoryService = brokerageHistoryService;
            _ticketConversionService = ticketConversionService;
            _ticketConversionService = ticketConversionService;
            _clientFactory = httpClientFactory;
        }

        public IEnumerable<BrokerageHistory> GetBrokeragesHistoriesByCategory(string category)
        {
            var brokeragesHistoriesFiltered = _brokerageHistoryService
                                        .GetAll()
                                        //.Where(x => (category == "all")
                                        //      ? x.Category == "Ações" || x.Category == "Fundos imobiliários"
                                        //      : x.Category == category)
                                        .GroupBy(t => t.TicketCode)
                                        .Select(b => new
                                        {
                                            b.Key,
                                            Quantity = b.Where(x => x.TransactionType == "Compra").Sum(x => x.Quantity) - b.Where(x => x.TransactionType == "Venda").Sum(x => x.Quantity),
                                            histories = b.ToList()
                                        });

            var ticketsConversions = _ticketConversionService.GetAll().ToList();
            var brokeragesHistories = brokeragesHistoriesFiltered.Where(x => x.Quantity > 0 && !ticketsConversions.Any(t => t.TicketOutput == x.Key)).SelectMany(x => x.histories);
            return brokeragesHistories;
        }

        public IList<CompositionStocksDto> GetCompositionTotalStocks(string category)
        {
            //var result = GetBrokeragesHistoriesByCategory(category).GroupBy(x => x.Category).Select(x =>
            //      new CompositionStocksDto()
            //      {
            //          Category = x.Key,                                         
            //          Quantity = x.Sum(s => s.Quantity),                       
            //  }).ToList();

            IEnumerable<BrokerageHistory> brokeragesHistories = GetBrokeragesHistoriesByCategory(category);

            var brokeragesAverages = brokeragesHistories.GroupBy(x => x.TicketCode).Select(x =>
             new
             {
                 x.Key,
                 x.FirstOrDefault().Category,
                 BrokeragesAveragePrice = x.Average(a => a.Price),
                 CurrentPrice = GetAsync(x.Key).Result,
                 Quantity = x.Sum(s => s.Quantity)
             }).ToList();

            var result = brokeragesAverages.GroupBy(x => x.Category).Select(x =>
                   new CompositionStocksDto()
                   {
                       Category = x.Key,
                       TotalValue = x.Sum(s => s.CurrentPrice * s.Quantity),
                   }).ToList();

            return result;
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
