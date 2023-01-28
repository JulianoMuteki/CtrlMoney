using ClosedXML.Excel;
using CtrlMoney.CrossCutting.Enums;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;
using System.Globalization;

namespace CtrlMoney.WorkSheet.Service
{
    public class XLWorkbookService : IXLWorkbookService
    {
        public XLWorkbookService()
        {

        }

        public IList<Earning> ImportEarningsSheet(string fullfileName)
        {
            var xls = new XLWorkbook(fullfileName);
            var sheet = xls.Worksheets.First(w => w.Name == "Proventos Recebidos");

            var totalRows = sheet.Rows().Count();
            IList<Earning> earnings = new List<Earning>(totalRows);

            for (int l = 2; l <= totalRows; l++)
            {
                var product = sheet.Cell($"A{l}").CellToString();
                if (!string.IsNullOrEmpty(product))
                {
                    var ticketCode = product.Split('-')[0].Trim();
                    DateTime paymentDate = DateTime.Parse(sheet.Cell($"B{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                    var eventType = sheet.Cell($"C{l}").CellToString();
                    var stockBroker = sheet.Cell($"D{l}").CellToString();
                    int quantity = int.Parse(sheet.Cell($"E{l}").CellToString().Split(',')[0]);
                    decimal unitPrice = decimal.Parse(sheet.Cell($"F{l}").CellToString().Replace('-', '0'));
                    decimal netValue = decimal.Parse(sheet.Cell($"G{l}").CellToString());

                    earnings.Add(new Earning(ticketCode, paymentDate, eventType, stockBroker, quantity, unitPrice, netValue));
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

        public IList<BrokerageHistory> ImportTransactionsSheet(string fullfileName)
        {
            var xls = new XLWorkbook(fullfileName);
            var planilha = xls.Worksheets.First(w => w.Name == "Negociação");

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
                var ticket = planilha.Cell($"F{l}").CellToString();

                int quantity = int.Parse(planilha.Cell($"G{l}").CellToString().Split(',')[0]);
                decimal price = decimal.Parse(planilha.Cell($"H{l}").CellToString());
                decimal totalPrice = decimal.Parse(planilha.Cell($"I{l}").CellToString());

                DateTime eExpireDate = string.IsNullOrEmpty(expire) || expire == "-" ? DateTime.MinValue : DateTime.Parse(expire, CultureInfo.CreateSpecificCulture("pt-BR"));
                BrokerageHistory brokerageHistory = new(totalPrice, price, quantity, ticket, stockBroker, eExpireDate, transactionDate, movementType, market);
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
            var planilha = xls.Worksheets.First(w => w.Name == "Carteira");

            var totalLinhas = planilha.Rows().Count();
            IList<BrokerageHistory> brokerageHistories = new List<BrokerageHistory>(totalLinhas);
            IList<BrokerageHistory> brokerageHistoriesErrors = new List<BrokerageHistory>();

            for (int l = 2; l <= totalLinhas; l++)
            {
                if (!string.IsNullOrEmpty(planilha.Cell($"C{l}").CellToString()))
                {
                    try
                    {
                        DateTime transactionDate = DateTime.Parse(planilha.Cell($"A{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
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
                    }catch(Exception ex)
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
            var sheet = xls.Worksheets.First(w => w.Name == "Proventos");

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

        public IList<Moviment> ImportMovimentsSheet(string fullfileName)
        {
            var xls = new XLWorkbook(fullfileName);
            var planilha = xls.Worksheets.First(w => w.Name == "Movimentação");

            var totalLinhas = planilha.Rows().Count();
            IList<Moviment> moviments = new List<Moviment>(totalLinhas);

            for (int l = 2; l <= totalLinhas; l++)
            {
                var product = planilha.Cell($"D{l}").CellToString();
                if (!string.IsNullOrEmpty(product))
                {
                    var ticketCode = product.Split('-')[0].Trim();

                    var inputoutput = planilha.Cell($"A{l}").CellToString();
                    DateTime date = DateTime.Parse(planilha.Cell($"B{l}").CellToString(), CultureInfo.CreateSpecificCulture("pt-BR"));
                    var movementType = planilha.Cell($"C{l}").CellToString();
                    var stockBroker = planilha.Cell($"E{l}").CellToString();

                    int quantity = int.Parse(planilha.Cell($"F{l}").CellToString().Split(',')[0]);
                    decimal unitPrice = decimal.Parse(planilha.Cell($"G{l}").CellToString().Replace('-', '0'));
                    decimal transactionValue = decimal.Parse(planilha.Cell($"H{l}").CellToString().Replace('-', '0'));

                    Moviment moviment = new Moviment(inputoutput, date, movementType, ticketCode, stockBroker, quantity, unitPrice, transactionValue);
                    moviments.Add(moviment);
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
