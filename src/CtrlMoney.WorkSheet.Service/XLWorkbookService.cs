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

        public IList<BrokerageHistory> ImportSheet()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            //get file extension                       
            string fileNameWithPath = Path.Combine(path, "b3.xlsx");
            var xls = new XLWorkbook(fileNameWithPath);
            var planilha = xls.Worksheets.First(w => w.Name == "Negociação");
            
            var totalLinhas = planilha.Rows().Count();
            IList<BrokerageHistory> brokerageHistories = new List<BrokerageHistory>(totalLinhas);

            // primeira linha é o cabecalho
            for (int l = 2; l <= totalLinhas; l++)
            {
                var dataNegociacao = DateTime.Parse(planilha.Cell($"A{l}").Value.ToString());
                var tipoMovimentacao = planilha.Cell($"B{l}").Value.ToString();
                var mercado = planilha.Cell($"C{l}").Value.ToString();
                var vencimento = planilha.Cell($"D{l}").Value.ToString();
                var instituicao = planilha.Cell($"E{l}").Value.ToString();
                var ticket = planilha.Cell($"F{l}").Value.ToString();
                var quantidade = int.Parse(planilha.Cell($"G{l}").Value.ToString());
                var preco = Decimal.Parse(planilha.Cell($"H{l}").Value.ToString());
                var valor = Decimal.Parse(planilha.Cell($"I{l}").Value.ToString());

                DateTime eExpireDate = vencimento == "-" ? DateTime.MinValue : DateTime.Parse(vencimento);
                BrokerageHistory brokerageHistory = new(valor, preco, quantidade, ticket, instituicao, eExpireDate, dataNegociacao, tipoMovimentacao, mercado);
                brokerageHistories.Add(brokerageHistory);                
            }

            return brokerageHistories;
        }
    }
}
