using ClosedXML.Excel;
using CtrlMoney.Domain.Entities;
using CtrlMoney.Domain.Interfaces.Application;

namespace CtrlMoney.WorkSheet.Service
{
    public class XLWorkbookService: IXLWorkbookService
    {
        public XLWorkbookService()
        {

        }

        public IList<Earning> ImportEarningsSheet(string idName)
        {
            throw new NotImplementedException();
        }

        public IList<Position> ImportPositionsSheet(string fullfileName)
        {
            var xls = new XLWorkbook(fullfileName);
            var sheet = xls.Worksheets.First(w => w.Name == "Acoes");
            var sheet2 = xls.Worksheets.First(w => w.Name == "Renda Fixa");
            var totalRowsCount = sheet.Rows().Count();
            IList<Position> brokerageHistories = new List<Position>(totalRowsCount);

            // primeira linha é o cabecalho
            for (int l = 2; l <= totalRowsCount; l++)
            {
                var produto = sheet.Cell($"A{l}").Value.ToString();
               // DateTime.TryParse(planilha.Cell($"A{l}").Value.ToString(), out DateTime dataNegociacao);
               //var tipoMovimentacao = planilha.Cell($"B{l}").Value.ToString();
               //var mercado = planilha.Cell($"C{l}").Value.ToString();
               //var vencimento = planilha.Cell($"D{l}").Value.ToString();
               //var instituicao = planilha.Cell($"E{l}").Value.ToString();
               //var ticket = planilha.Cell($"F{l}").Value.ToString();
               //_ = int.TryParse(planilha.Cell($"G{l}").Value.ToString(), out int quantidade);
               //_ = decimal.TryParse(planilha.Cell($"H{l}").Value.ToString(), out decimal preco);
               //_ = decimal.TryParse(planilha.Cell($"I{l}").Value.ToString(), out decimal valor);

                // DateTime eExpireDate = string.IsNullOrEmpty(vencimento) || vencimento == "-" ? DateTime.MinValue : DateTime.Parse(vencimento);
                // BrokerageHistory brokerageHistory = new(valor, preco, quantidade, ticket, instituicao, eExpireDate, dataNegociacao, tipoMovimentacao, mercado);
                // brokerageHistories.Add(brokerageHistory);
            }

            return brokerageHistories;
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
