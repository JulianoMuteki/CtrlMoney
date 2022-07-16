using ClosedXML.Excel;
using CtrlMoney.CrossCutting.Enums;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;

namespace CtrlMoney.WorkSheet.Service
{
    public class XLWorkbookService : IXLWorkbookService
    {
        public XLWorkbookService()
        {

        }

        public IList<Earning> ImportEarningsSheet(string idName)
        {
            throw new NotImplementedException();
        }

        public IList<Position> ImportPositionsSheet(string fullfileName, DateTime positionDate)
        {
            var xls = new XLWorkbook(fullfileName);
            List<Position> positions = new List<Position>();
            positions.AddRange(ImportSheetTab(xls, positionDate, "Acoes", EInvestmentType.STOCK));

            return positions;
        }

        private static IList<Position> ImportSheetTab(XLWorkbook xls, DateTime positionDate, string tabName, EInvestmentType investmentType)
        {
            var sheet = xls.Worksheets.First(w => w.Name == tabName);
            var totalRowsCount = sheet.Rows().Count();
            IList<Position> positions = new List<Position>();

            for (int l = 2; l <= totalRowsCount; l++)
            {
                var isProductValid = sheet.Cell($"A{l}").Value.ToString();
                if (!string.IsNullOrEmpty(isProductValid))
                {

                    var stockBroker = sheet.Cell($"B{l}").Value.ToString();
                    var tickerCode = sheet.Cell($"D{l}").Value.ToString();
                    var isinCode = sheet.Cell($"E{l}").Value.ToString();
                    var type = sheet.Cell($"F{l}").Value.ToString();
                    var bookkeeping = sheet.Cell($"G{l}").Value.ToString();
                    _ = int.TryParse(sheet.Cell($"H{l}").Value.ToString(), out int quantity);
                    _ = int.TryParse(sheet.Cell($"I{l}").Value.ToString(), out int quantityAvailable);
                    var strQuantityUnavailable = sheet.Cell($"J{l}").Value.ToString();
                    var quantityUnavailable = string.IsNullOrEmpty(strQuantityUnavailable) || strQuantityUnavailable == "-" ? 0 : int.Parse(strQuantityUnavailable);
                    var reason = sheet.Cell($"K{l}").Value.ToString();
                    _ = decimal.TryParse(sheet.Cell($"L{l}").Value.ToString(), out decimal closingPrice);
                    _ = decimal.TryParse(sheet.Cell($"L{l}").Value.ToString(), out decimal valueUpdated);

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
                DateTime.TryParse(planilha.Cell($"A{l}").Value.ToString(), out DateTime dataNegociacao);
                var tipoMovimentacao = planilha.Cell($"B{l}").Value.ToString();
                var mercado = planilha.Cell($"C{l}").Value.ToString();
                var vencimento = planilha.Cell($"D{l}").Value.ToString();
                var instituicao = planilha.Cell($"E{l}").Value.ToString();
                var ticket = planilha.Cell($"F{l}").Value.ToString();
                _ = int.TryParse(planilha.Cell($"G{l}").Value.ToString(), out int quantidade);
                _ = decimal.TryParse(planilha.Cell($"H{l}").Value.ToString(), out decimal preco);
                _ = decimal.TryParse(planilha.Cell($"I{l}").Value.ToString(), out decimal valor);

                DateTime eExpireDate = string.IsNullOrEmpty(vencimento) || vencimento == "-" ? DateTime.MinValue : DateTime.Parse(vencimento);
                BrokerageHistory brokerageHistory = new(valor, preco, quantidade, ticket, instituicao, eExpireDate, dataNegociacao, tipoMovimentacao, mercado);
                brokerageHistories.Add(brokerageHistory);
            }

            return brokerageHistories;
        }
    }
}
