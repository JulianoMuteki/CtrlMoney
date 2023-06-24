using ClosedXML.Excel;
using CtrlMoney.CrossCutting.Enums;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CtrlMoney.WorkSheet.Service
{
    public class XLWorkbookService : IXLWorkbookService
    {
        private readonly IHttpClientFactory _clientFactory;

        private const string _transactionSheetTabName = "Negociação";
        private const string _earningSheetTabName = "Proventos";
        private const string _movimentSheetTabName = "Movimentação";
        private const string _earningB3SheetTabName = "Proventos Recebidos";
        private const string _transactionB3SheetTabName = "Negociação";
        private readonly string _categoryStock = "Ações";
        private readonly string _categoryREITs = "Fundos imobiliários";
        private readonly string _categoryETF = "ETF";
        private readonly string _categoryInvalid = "Invalid";
        
        private readonly IList<string> _movementsTypesRequired = new List<string>() { "Atualização", "Fração em Ativos", "Bonificação em Ativos", "Leilão de Fração", "Amortização", "Recibo de Subscrição" };

        private readonly IDictionary<string, string> _categories = new Dictionary<string, string>();

        public XLWorkbookService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

            _categories.Add(_categoryStock, "acoes");
            _categories.Add(_categoryREITs, "fundos-imobiliarios");
            _categories.Add(_categoryETF, "etfs");
        }
        private async Task<string> CheckTickerCategoryType(string tickerCode)
        {
            foreach (var item in _categories)
            {                
                if (await GetStockType(tickerCode, item.Value))
                {
                    return item.Key;
                }
            }
            return _categoryInvalid;
        }

        private async Task<bool> GetStockType(string tickerCode, string url)
        {
            var client = _clientFactory.CreateClient("Tickers");
            var response = await client.GetAsync($"/{url}/{tickerCode.ToLower()}");

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {                 
                string padrao = $@"COTA&#xC7;&#xC3;O [A-Za-z]{{2}} {tickerCode.ToUpper()}";                
                Regex regex = new Regex(padrao);

                // Read the response content as a string
                var content = await response.Content.ReadAsStringAsync();
                if (content.TrimStart().StartsWith("<!DOCTYPE html>") && content.Contains("<meta name=\"publisher\" content=\"statusinvest.com.br\">")
                    && content.Contains("class=\"d-flex justify-between align-items-center mb-sm-2  flex-wrap flex-xs-nowrap\"") 
                    && regex.IsMatch(content)
                    && !content.Contains("<span class=\"d-block mb-1 fw-900\">OPS. . .</span>Não encontramos o que você está procurando"))
                {
                    return true;
                }
            }

            return false;
        }

        public IList<Earning> ImportEarningsB3Sheet(string fullfileName, IDictionary<string, string> keyTickersCategories)
        {
            var xls = new XLWorkbook(fullfileName);
            var sheet = xls.Worksheets.First(w => w.Name == _earningB3SheetTabName);

            var totalRows = sheet.Rows().Count();
            IList<Earning> earnings = new List<Earning>(totalRows);

            for (int l = 2; l <= totalRows; l++)
            {
                var product = sheet.Cell($"A{l}").CellToString();
                if (!string.IsNullOrEmpty(product))
                {
                    var ticketCode = product.Split('-')[0].Trim().TrimEnd('F');
                    DateTime paymentDate = DateTime.Parse(sheet.Cell($"B{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                    var eventType = sheet.Cell($"C{l}").CellToString();
                    var stockBroker = sheet.Cell($"D{l}").CellToString();
                    int quantity = int.Parse(sheet.Cell($"E{l}").CellToString().Split(',')[0]);
                    decimal unitPrice = decimal.Parse(sheet.Cell($"F{l}").CellToString().Replace('-', '0'));
                    decimal netValue = decimal.Parse(sheet.Cell($"G{l}").CellToString());
                    string category = "";

                    if (!keyTickersCategories.ContainsKey(ticketCode))
                    {
                        category = CheckTickerCategoryType(ticketCode).Result;
                        keyTickersCategories.Add(ticketCode, category);
                    }
                    else
                    {
                        category = keyTickersCategories[ticketCode];
                    }

                    earnings.Add(new Earning(ticketCode, paymentDate, eventType, stockBroker, quantity, unitPrice, netValue, category));
                }
            }

            return earnings;
        }

        public IList<Position> ImportPositionsSheet(string fullfileName, DateTime positionDate)
        {
            var xls = new XLWorkbook(fullfileName);
            List<Position> positions = new List<Position>();
            positions.AddRange(ImportSheetTabForPositions(xls, positionDate, "Acoes", EInvestmentType.STOCK));
            positions.AddRange(ImportSheetTabForPositions(xls, positionDate, "Fundo de Investimento", EInvestmentType.INVESTMENT_FUNDS));

            return positions;
        }

        private IList<Position> ImportSheetTabForPositions(XLWorkbook xls, DateTime positionDate, string tabName, EInvestmentType investmentType)
        {
            var sheet = xls.Worksheets.First(w => w.Name == tabName);
            var totalRowsCount = sheet.Rows().Count();
            IList<Position> positions = new List<Position>();

            for (int l = 2; l <= totalRowsCount; l++)
            {
                var isProductValid = sheet.Cell($"A{l}").CellToString();
                if (!string.IsNullOrEmpty(isProductValid))
                {
                    var stockBroker = sheet.Cell($"B{l}").CellToString();
                    var tickerCode = sheet.Cell($"D{l}").CellToString();
                    var isinCode = sheet.Cell($"E{l}").CellToString();
                    var type = sheet.Cell($"F{l}").CellToString();
                    var bookkeeping = sheet.Cell($"G{l}").CellToString();
                    int quantity = int.Parse(sheet.Cell($"H{l}").CellToString().Replace('-', '0').Split(',')[0]);
                    int quantityAvailable = int.Parse(sheet.Cell($"I{l}").CellToString().Replace('-', '0'));
                    var strQuantityUnavailable = sheet.Cell($"J{l}").CellToString();
                    var quantityUnavailable = string.IsNullOrEmpty(strQuantityUnavailable) || strQuantityUnavailable == "-" ? 0 : int.Parse(strQuantityUnavailable);
                    var reason = sheet.Cell($"K{l}").CellToString();
                    decimal closingPrice = decimal.Parse(sheet.Cell($"L{l}").CellToString());
                    decimal valueUpdated = decimal.Parse(sheet.Cell($"M{l}").CellToString());

                    positions.Add(new Position(positionDate, investmentType, stockBroker, tickerCode, isinCode, type, bookkeeping, quantity,
                                                   quantityAvailable, quantityUnavailable, reason, closingPrice, valueUpdated));
                }
            }

            return positions;
        }

        public IList<BrokerageHistory> ImportTransactionsB3Sheet(string fullfileName, IDictionary<string, string> keyTickersCategories)
        {
            var xls = new XLWorkbook(fullfileName);
            var planilha = xls.Worksheets.First(w => w.Name == _transactionB3SheetTabName);

            var totalLinhas = planilha.Rows().Count();
            IList<BrokerageHistory> brokerageHistories = new List<BrokerageHistory>(totalLinhas);

            // primeira linha é o cabecalho
            for (int l = 2; l <= totalLinhas; l++)
            {
                DateTime transactionDate = DateTime.Parse(planilha.Cell($"A{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                var movementType = planilha.Cell($"B{l}").CellToString();
                var market = planilha.Cell($"C{l}").CellToString();
                var expire = planilha.Cell($"D{l}").CellToString();
                var stockBroker = planilha.Cell($"E{l}").CellToString();
                var ticketCode = planilha.Cell($"F{l}").CellToString().TrimEnd('F');

                int quantity = int.Parse(planilha.Cell($"G{l}").CellToString().Split(',')[0]);
                decimal price = decimal.Parse(planilha.Cell($"H{l}").CellToString());
                decimal totalPrice = decimal.Parse(planilha.Cell($"I{l}").CellToString());
                string category = "";

                if (!keyTickersCategories.ContainsKey(ticketCode))
                {
                    category = CheckTickerCategoryType(ticketCode).Result;
                    keyTickersCategories.Add(ticketCode, category);
                }
                else
                {
                    category = keyTickersCategories[ticketCode];
                }

                DateTime eExpireDate = string.IsNullOrEmpty(expire) || expire == "-" ? DateTime.MinValue : DateTime.Parse(expire, CultureInfo.CreateSpecificCulture("pt-BR"));
                BrokerageHistory brokerageHistory = new(totalPrice, price, quantity, ticketCode, stockBroker, eExpireDate, transactionDate, movementType, market, category);
                brokerageHistories.Add(brokerageHistory);
            }

            return brokerageHistories;
        }

        /// <summary>
        /// Import StatusInvest Sheet Transactions
        /// </summary>
        /// <param name="fullfileName"></param>
        /// <returns></returns>
        public IList<BrokerageHistory> ImportSITransactionsSheet(string fullfileName)
        {
            var xls = new XLWorkbook(fullfileName);
            var planilha = xls.Worksheets.First(w => w.Name == _transactionSheetTabName);

            var totalLinhas = planilha.Rows().Count();
            IList<BrokerageHistory> brokerageHistories = new List<BrokerageHistory>(totalLinhas);
            IList<BrokerageHistory> brokerageHistoriesErrors = new List<BrokerageHistory>();

            for (int l = 2; l <= totalLinhas; l++)
            {
                if (!string.IsNullOrEmpty(planilha.Cell($"C{l}").CellToString()))
                {
                    try
                    {
                        DateTime transactionDate = DateTime.Parse(planilha.Cell($"A{l}").CellToString());
                        var category = planilha.Cell($"B{l}").CellToString();
                        var ticket = planilha.Cell($"C{l}").CellToString();
                        var transactionType = planilha.Cell($"D{l}").CellToString();
                        int quantity = int.Parse(planilha.Cell($"E{l}").CellToString().Split(',')[0]);
                        decimal price = decimal.Parse(planilha.Cell($"F{l}").CellToString());
                        var stockBroker = planilha.Cell($"G{l}").CellToString();
                        decimal brokerage = decimal.Parse(planilha.Cell($"H{l}").CellToString());
                        decimal fees = decimal.Parse(planilha.Cell($"I{l}").CellToString());
                        decimal taxes = decimal.Parse(planilha.Cell($"J{l}").CellToString());
                        decimal irrf = decimal.Parse(planilha.Cell($"K{l}").CellToString());

                        BrokerageHistory brokerageHistory = new(transactionDate, transactionType, stockBroker, ticket, quantity, price, category, brokerage, fees, taxes, irrf);
                        brokerageHistories.Add(brokerageHistory);

                        Console.WriteLine("###### Price:" + brokerageHistory.Price);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine(planilha.Rows(l.ToString()));
                    }
                }
            }

            return brokerageHistories;
        }

        public IList<Earning> ImportSIEarningsSheet(string fullfileName)
        {
            var xls = new XLWorkbook(fullfileName);
            var sheet = xls.Worksheets.First(w => w.Name == _earningSheetTabName);

            var totalRows = sheet.Rows().Count();
            IList<Earning> earnings = new List<Earning>(totalRows);

            for (int l = 2; l <= totalRows; l++)
            {
                var category = sheet.Cell($"A{l}").CellToString();
                if (!string.IsNullOrEmpty(category))
                {
                    var ticketCode = sheet.Cell($"B{l}").CellToString();
                    var stockBroker = sheet.Cell($"C{l}").CellToString();
                    var eventType = sheet.Cell($"D{l}").CellToString();

                    int quantity = int.Parse(sheet.Cell($"E{l}").CellToString().Split(',')[0]);
                    decimal price = decimal.Parse(sheet.Cell($"F{l}").CellToString().Replace("R$ ", ""));
                    decimal totalPrice = decimal.Parse(sheet.Cell($"G{l}").CellToString().Replace("R$ ", ""));
                    decimal totalNetAmount = decimal.Parse(sheet.Cell($"H{l}").CellToString().Replace("R$ ", ""));
                    DateTime withDate = DateTime.Parse(sheet.Cell($"I{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                    DateTime paymentDate = !sheet.Cell($"J{l}").CellToString().Contains("-") ? DateTime.Parse(sheet.Cell($"J{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"))
                                                                                        : DateTime.MinValue;

                    earnings.Add(new Earning(ticketCode, paymentDate, eventType, stockBroker, quantity, price, totalPrice, totalNetAmount, category, withDate));
                }
            }

            return earnings;
        }

        public IList<Moviment> ImportMovimentsB3Sheet(string fullfileName, IDictionary<string, string> keyTickersCategories)
        {
            var xls = new XLWorkbook(fullfileName);
            var sheetTab = xls.Worksheets.First(w => w.Name == _movimentSheetTabName);

            var totalLinhas = sheetTab.Rows().Count();
            IList<Moviment> moviments = new List<Moviment>(totalLinhas);

            for (int l = 2; l <= totalLinhas; l++)
            {
                var product = sheetTab.Cell($"D{l}").CellToString();
                if (!string.IsNullOrEmpty(product))
                {
                    var ticketCode = product.Split('-')[0].Trim().TrimEnd('F');
                    var inputoutput = sheetTab.Cell($"A{l}").CellToString();
                    DateTime date = DateTime.Parse(sheetTab.Cell($"B{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                    string movementType = sheetTab.Cell($"C{l}").CellToString();
                    string stockBroker = sheetTab.Cell($"E{l}").CellToString();

                    decimal quantity = decimal.Parse(sheetTab.Cell($"F{l}").CellToString());
                    decimal unitPrice = decimal.Parse(sheetTab.Cell($"G{l}").CellToString().Replace('-', '0'));
                    decimal transactionValue = decimal.Parse(sheetTab.Cell($"H{l}").CellToString().Replace('-', '0'));

                    if (_movementsTypesRequired.Contains(movementType))
                    {
                        Moviment moviment = new Moviment(inputoutput, date, movementType, ticketCode, stockBroker, quantity, unitPrice, transactionValue);
                        moviments.Add(moviment);
                    }
                }
            }

            return moviments;
        }


    }

    public static class ConvertCustom
    {
        public static string CellToString(this IXLCell xLCell)
        {
            var cell = xLCell.Value;
            return string.Format("{0}", cell);
        }
    }
}
