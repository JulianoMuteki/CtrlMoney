using ClosedXML.Excel;
using CtrlMoney.Domain.Interfaces.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlMoney.WorkSheet.Service
{
    public class XLWorkbookService: IXLWorkbookService
    {
        public void ImportSheet()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
            //get file extension                       
            string fileNameWithPath = Path.Combine(path, "b3.xlsx");
            var xls = new XLWorkbook(fileNameWithPath);
            var planilha = xls.Worksheets.First(w => w.Name == "Negociação");
            
            var totalLinhas = planilha.Rows().Count();
            // primeira linha é o cabecalho
            for (int l = 2; l <= totalLinhas; l++)
            {
                //Data do Negócio	Tipo de Movimentação	Mercado	Prazo/Vencimento	Instituição	Código de Negociação	Quantidade	Preço	Valor

                var dataNegociacao = DateTime.Parse(planilha.Cell($"A{l}").Value.ToString());
                var tipoMovimentacao = planilha.Cell($"B{l}").Value.ToString();
                var mercado = planilha.Cell($"C{l}").Value.ToString();
                var vencimento = planilha.Cell($"D{l}").Value.ToString();
                var instituicao = planilha.Cell($"E{l}").Value.ToString();
                var ticket = planilha.Cell($"F{l}").Value.ToString();
                var quantidade = int.Parse(planilha.Cell($"G{l}").Value.ToString());
                var preco = planilha.Cell($"H{l}").Value.ToString();
                var valor = planilha.Cell($"I{l}").Value.ToString();

            }
        }
    }
}
